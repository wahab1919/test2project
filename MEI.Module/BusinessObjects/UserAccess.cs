using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.Base.Security;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;

namespace MEI.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Setup")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class UserAccess : MEISysBaseObject, ISecurityUser, IAuthenticationStandardUser, IAuthenticationActiveDirectoryUser,
    ISecurityUserWithRoles, IPermissionPolicyUser
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private string _code;
        private Employee _employee;

        public UserAccess(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        [Association("Employee-UserAccesses")]
        [DataSourceCriteria("Active = true"), ImmediatePostData()]
        public Employee Employee
        {
            get { return _employee; }
            set
            {
                Employee oldEmployee = _employee;
                SetPropertyValue("Employee", ref _employee, value);
            }
        }

        #region ISecurityUser Members
        private bool isActive = true;
        public bool IsActive
        {
            get { return isActive; }
            set { SetPropertyValue("IsActive", ref isActive, value); }
        }
        private string userName = String.Empty;
        [RuleRequiredField("EmployeeUserNameRequired", DefaultContexts.Save)]
        [RuleUniqueValue("EmployeeUserNameIsUnique", DefaultContexts.Save,
            "The login with the entered user name was already registered within the system.")]
        public string UserName
        {
            get { return userName; }
            set { SetPropertyValue("UserName", ref userName, value); }
        }
        #endregion

        #region IAuthenticationStandardUser Members
        private bool changePasswordOnFirstLogon;
        public bool ChangePasswordOnFirstLogon
        {
            get { return changePasswordOnFirstLogon; }
            set
            {
                SetPropertyValue("ChangePasswordOnFirstLogon", ref changePasswordOnFirstLogon, value);
            }
        }

        private string storedPassword;
        [Browsable(false), Size(SizeAttribute.Unlimited), Persistent, SecurityBrowsable]
        protected string StoredPassword
        {
            get { return storedPassword; }
            set { storedPassword = value; }
        }

        public bool ComparePassword(string password)
        {
            //return SecurityUserBase.ComparePassword(this.storedPassword, password);
            return PasswordCryptographer.VerifyHashedPasswordDelegate(this.storedPassword, password);
        }

        public void SetPassword(string password)
        {
            //this.storedPassword = new PasswordCryptographer().GenerateSaltedPassword(password);
            this.storedPassword = PasswordCryptographer.HashPasswordDelegate(password);
            OnChanged("StoredPassword");
        }
        #endregion

        #region ISecurityUserWithRoles Members
        IList<ISecurityRole> ISecurityUserWithRoles.Roles
        {
            get
            {
                IList<ISecurityRole> result = new List<ISecurityRole>();
                foreach (UserAccessRole role in UserAccessRoles)
                {
                    result.Add(role);
                }
                return result;
            }
        }
        #endregion

        [Association("UserAccess-UserAccessRoles")]
        [RuleRequiredField("UserAccessRoleIsRequired", DefaultContexts.Save,
            TargetCriteria = "IsActive",
            CustomMessageTemplate = "An active employee must have at least one role assigned")]
        public XPCollection<UserAccessRole> UserAccessRoles
        {
            get
            {
                return GetCollection<UserAccessRole>("UserAccessRoles");
            }

        }

        #region IPermissionPolicyUser Members
        IEnumerable<IPermissionPolicyRole> IPermissionPolicyUser.Roles
        {
            get { return UserAccessRoles.OfType<IPermissionPolicyRole>(); }
        }
        #endregion

        [Association("UserAccess-ApplicationSetupDetails")]
        public XPCollection<ApplicationSetupDetail> ApplicationSetupDetails
        {
            get
            { return GetCollection<ApplicationSetupDetail>("ApplicationSetupDetails"); }
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}