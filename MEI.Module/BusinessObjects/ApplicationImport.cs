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
using MEI.Module.CustomProcess;
using System.Web;
using System.IO;

namespace MEI.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Setup")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ApplicationImport : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private ListImport _no;
        private ObjectList _objectList;
        private FileData _dataImport;
        private string _message;
        private GlobalFunction _globFunc;

        public ApplicationImport(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        [ImmediatePostData()]
        public ListImport No
        {
            get { return _no; }
            set {
                SetPropertyValue("No", ref _no, value);
                if(!IsLoading)
                {
                    if(this._no != null)
                    {
                        this.ObjectList = this._no.ObjectList;
                    }
                }
            }
        }

        public ObjectList ObjectList
        {
            get { return _objectList; }
            set { SetPropertyValue("ObjectList", ref _objectList, value); }
        }

        public FileData DataImport
        {
            get { return _dataImport; }
            set { SetPropertyValue("DataImport", ref _dataImport, value); }
        }

        public string Message
        {
            get
            {
                if (DataImport != null)
                {
                    string fileExist = null;
                    fileExist = HttpContext.Current.Server.MapPath("~/UploadFile/" + DataImport.FileName);
                    if (File.Exists(fileExist))
                    {
                        this._message = "File is already available in server";
                    }
                    else
                    {
                        this._message = null;
                    }
                }
                return _message;
            }
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

        //=============================================== Code In Here ===============================================

        [Action(Caption = "Import", ConfirmationMessage = "Are you sure?", AutoCommit = false)]
        public void Import()
        {
            try
            {
                if (this.DataImport != null && this.No.ObjectList != CustomProcess.ObjectList.None)
                {
                    string targetpath = null;
                    if (Message != "File is already available in server")
                    {
                        targetpath = HttpContext.Current.Server.MapPath("~/UploadFile/" + this.DataImport.FileName);
                        string ext = System.IO.Path.GetExtension(this.DataImport.FileName);
                        FileStream fileStream = new FileStream(targetpath, FileMode.OpenOrCreate);
                        this.DataImport.SaveToStream(fileStream);
                        fileStream.Close();
                        if (File.Exists(targetpath))
                        {
                            _globFunc = new GlobalFunction();
                            _globFunc.ImportDataExcel(Session, targetpath, ext, this.No.ObjectList);
                        }
                        this.DataImport.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" Business Object = ApplicationImport", ex.ToString());
            }
        }
    }
}