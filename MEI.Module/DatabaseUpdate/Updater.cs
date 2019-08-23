using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using MEI.Module.BusinessObjects;
using MEI.Module.CustomProcess;

namespace MEI.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            //string name = "MyName";
            //DomainObject1 theObject = ObjectSpace.FindObject<DomainObject1>(CriteriaOperator.Parse("Name=?", name));
            //if(theObject == null) {
            //    theObject = ObjectSpace.CreateObject<DomainObject1>();
            //    theObject.Name = name;
            //}
            DefSecurity1();
            CreateDefaultRole();
            DefAppSetup();
        }

        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }

        private void CreateDefaultRole()
        {
            UserAccessRole defaultRole = ObjectSpace.FindObject<UserAccessRole>(new BinaryOperator("Name", "Default"));
            if (defaultRole == null)
            {
                defaultRole = ObjectSpace.CreateObject<UserAccessRole>();
                defaultRole.Name = "Default";

                defaultRole.AddObjectPermission<UserAccessRole>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<UserAccessRole>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<UserAccessRole>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<UserAccessRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
            }
            ObjectSpace.CommitChanges();
        }

        private void DefSecurity1()
        {

            //--- Menjalankan program ini harus terlebih dahulu menjalankan program pada point 1 -----
            UserAccessRole adminEmployeeRole = ObjectSpace.FindObject<UserAccessRole>(new BinaryOperator("Name", SecurityStrategy.AdministratorRoleName));
            if (adminEmployeeRole == null)
            {
                adminEmployeeRole = ObjectSpace.CreateObject<UserAccessRole>();
                adminEmployeeRole.Name = SecurityStrategy.AdministratorRoleName;
                adminEmployeeRole.IsAdministrative = true;
                adminEmployeeRole.Save();
            }

            UserAccess adminEmployee = ObjectSpace.FindObject<UserAccess>(
                new BinaryOperator("UserName", "admin"));
            if (adminEmployee == null)
            {
                adminEmployee = ObjectSpace.CreateObject<UserAccess>();
                adminEmployee.UserName = "admin";
                adminEmployee.SetPassword("skadmin@2019");
                adminEmployee.UserAccessRoles.Add(adminEmployeeRole);
            }
            UserAccess adminEmployee1 = ObjectSpace.FindObject<UserAccess>(
                new BinaryOperator("UserName", "admin1"));
            if (adminEmployee1 == null)
            {
                adminEmployee1 = ObjectSpace.CreateObject<UserAccess>();
                adminEmployee1.UserName = "admin1";
                adminEmployee1.SetPassword("skadmin@2019");
                adminEmployee1.UserAccessRoles.Add(adminEmployeeRole);
            }
            ObjectSpace.CommitChanges();
        }

        private void DefAppSetup()
        {
            try
            {
                ApplicationSetup _appSetup = ObjectSpace.FindObject<ApplicationSetup>
                                         (CriteriaOperator.Parse("Name = 'Setup App 1'"));
                if (_appSetup == null)
                {
                    _appSetup = ObjectSpace.CreateObject<ApplicationSetup>();
                    _appSetup.Name = "Setup App 1";
                    _appSetup.Code = "APS0001";
                    _appSetup.Active = true;
                    _appSetup.DefaultSystem = true;

                    //Numbering Header
                    NumberingHeader _objNumberingHeader = new NumberingHeader(_appSetup.Session)
                    {
                        Code = "GN-0001",
                        Name = "Group Numbering Object",
                        NumberingType = NumberingType.Objects,
                        Active = true,
                        ApplicationSetup = _appSetup
                    };

                    _objNumberingHeader = new NumberingHeader(_appSetup.Session)
                    {
                        Code = "GN-0002",
                        Name = "Group Numbering Document",
                        NumberingType = NumberingType.Documents,
                        Active = true,
                        ApplicationSetup = _appSetup
                    };

                    //List Import
                    ListImport _objListImport = new ListImport(_appSetup.Session)
                    {
                        No = 1,
                        ObjectList = ObjectList.NumberingLine
                    };

                    _objListImport = new ListImport(_appSetup.Session)
                    {
                        No = 2,
                        ObjectList = ObjectList.ListImport
                    };
                    ObjectSpace.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(ex);
            }
        }
    }
}
