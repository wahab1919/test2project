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
using DevExpress.ExpressApp.ConditionalAppearance;

namespace MEI.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NonPersistent()]
    [DeferredDeletion(false)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class MEISysBaseObject : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private string _createBy;
        private DateTime _createDate;
        private string _modifyBy;
        private DateTime _modifyDate;

        public MEISysBaseObject(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreateBy = SecuritySystem.CurrentUserName;
            CreateDate = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            ModifiedBy = SecuritySystem.CurrentUserName;
            ModifiedDate = DateTime.Now;

        }

        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("CCS10SysBaseObjectCreatedBy", Enabled = false)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("User")]
        public string CreateBy
        {
            get { return _createBy; }
            set { SetPropertyValue("CreateBy", ref _createBy, value); }
        }

        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("CCS10SysBaseObjectCreatedDate", Enabled = false)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Date")]
        public DateTime CreateDate
        {
            get { return _createDate; }
            set { SetPropertyValue("CreateDate", ref _createDate, value); }
        }

        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("CCS10SysBaseObjectModifiedBy", Enabled = false)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("User")]
        public string ModifiedBy
        {
            get { return _modifyBy; }
            set { SetPropertyValue("ModifiedBy", ref _modifyBy, value); }
        }

        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("CCS10SysBaseObjectModifiedDate", Enabled = false)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Date")]
        public DateTime ModifiedDate
        {
            get { return _modifyDate; }
            set { SetPropertyValue("ModifiedDate", ref _modifyDate, value); }
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