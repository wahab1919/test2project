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
using DevExpress.ExpressApp.ConditionalAppearance;

namespace MEI.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Inventory")]
    [DefaultProperty("FullName")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class UnitOfMeasure : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private string _code;
        private string _name;
        private string _fullName;
        private string _description;
        private UnitOfMeasureType _type;
        private bool _active;
        private bool _default;
        private GlobalFunction _globFunc;

        public UnitOfMeasure(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (!IsLoading)
            {
                _globFunc = new GlobalFunction();
                this.Code = _globFunc.GetNumberingUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.UnitOfMeasure);
            }
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        [ImmediatePostData()]
        [VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public string Code
        {
            get { return _code; }
            set {
                SetPropertyValue("Code", ref _code, value);
                if (!IsLoading)
                {
                    if (this._name != null)
                    {
                        SetFullName();
                    }
                }
            }
        }

        [ImmediatePostData()]
        [VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public string Name
        {
            get { return _name; }
            set {
                SetPropertyValue("Name", ref _name, value);
                if(!IsLoading)
                {
                    if(this._name != null)
                    {
                        SetFullName();
                    }
                }
            }
        }

        [Appearance("UnitOfMeasureFullNameEnabled", Enabled = false)]
        public string FullName
        {
            get { return _fullName; }
            set { SetPropertyValue("FullName", ref _fullName, value); }
        }

        [Size(512)]
        public string Description
        {
            get { return _description; }
            set { SetPropertyValue("Description", ref _description, value); }
        }

        [DataSourceCriteria("Active = true")]
        public UnitOfMeasureType Type
        {
            get { return _type; }
            set { SetPropertyValue("Type", ref _type, value); }
        }

        public bool Active
        {
            get { return _active; }
            set { SetPropertyValue("Active", ref _active, value); }
        }

        public bool Default
        {
            get { return _default; }
            set { SetPropertyValue("Default", ref _default, value); }
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

        private void SetFullName()
        {
            try
            {
                if (this._code != null && this._name != null)
                {
                    this.FullName = String.Format("{0} ({1})", this.Code, this.Name);
                }

            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError("Business Object = Location ", ex.ToString());
            }
        }
    }
}