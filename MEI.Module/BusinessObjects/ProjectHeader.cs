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
using DevExpress.ExpressApp.SystemModule;

namespace MEI.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Project Transaction")]
    [DefaultProperty("Code")]
    [ListViewFilter("AllDataProjectHeader", "", "All Data", "All Data In Project Header", 1, true)]
    [ListViewFilter("OpenProjectHeader", "Status = 'Open'", "Open", "Open Data Status In Project Header", 2, true)]
    [ListViewFilter("ProgressProjectHeader", "Status = 'Progress'", "Progress", "Progress Data Status In Project Header", 3, true)]
    [ListViewFilter("LockProjectHeader", "Status = 'Lock'", "Lock", "Lock Data Status In Project Header", 4, true)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ProjectHeader : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private bool _activationPosting;
        private string _code;
        private string _name;
        private BusinessPartner _customer;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _description;
        private FileData _attachment;
        private Status _status;
        private DateTime _statusDate;
        private GlobalFunction _globFunc;

        public ProjectHeader(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (!IsLoading)
            {
                _globFunc = new GlobalFunction();
                this.Code = _globFunc.GetNumberingUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.ProjectHeader);
                DateTime now = DateTime.Now;
                this.Status = CustomProcess.Status.Open;
                this.StatusDate = now;
            }
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        [Browsable(false)]
        public bool ActivationPosting
        {
            get { return _activationPosting; }
            set { SetPropertyValue("ActivationPosting", ref _activationPosting, value); }
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Appearance("ProjectHeaderCodeClose", Enabled = false)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        [Appearance("ProjectHeaderNameLock", Criteria = "ActivationPosting = true", Enabled = false)]
        public string Name
        {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }

        [DataSourceCriteria("Active = true")]
        [RuleRequiredField(DefaultContexts.Save)]
        [Appearance("ProjectHeaderCustomerLock", Criteria = "ActivationPosting = true", Enabled = false)]
        public BusinessPartner Customer
        {
            get { return _customer; }
            set { SetPropertyValue("Customer", ref _customer, value); }
        }

        [Appearance("ProjectHeaderStartDateLock", Criteria = "ActivationPosting = true", Enabled = false)]
        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetPropertyValue("StartDate", ref _startDate, value); }
        }

        [Appearance("ProjectHeaderEndDateLock", Criteria = "ActivationPosting = true", Enabled = false)]
        public DateTime EndDate
        {
            get { return _endDate; }
            set { SetPropertyValue("EndDate", ref _endDate, value); }
        }

        [Appearance("ProjectHeaderDescriptionLock", Criteria = "ActivationPosting = true", Enabled = false)]
        [Size(512)]
        public string Description
        {
            get { return _description; }
            set { SetPropertyValue("Description", ref _description, value); }
        }

        [Appearance("ProjectHeaderAttachmentLock", Criteria = "ActivationPosting = true", Enabled = false)]
        public FileData Attachment
        {
            get { return _attachment; }
            set { SetPropertyValue("Attachment", ref _attachment, value); }
        }

        [Appearance("ProjectHeaderStatusClose", Enabled = false)]
        public Status Status
        {
            get { return _status; }
            set { SetPropertyValue("Status", ref _status, value); }
        }

        [Appearance("ProjectHeaderStatusDateClose", Enabled = false)]
        public DateTime StatusDate
        {
            get { return _statusDate; }
            set { SetPropertyValue("StatusDate", ref _statusDate, value); }
        }

        [Association("ProjectHeader-ProjectLines")]
        public XPCollection<ProjectLine> ProjectLines
        {
            get { return GetCollection<ProjectLine>("ProjectLines"); }
        }

        [Association("ProjectHeader-PrePurchaseOrders")]
        public XPCollection<PrePurchaseOrder> PrePurchaseOrders
        {
            get { return GetCollection<PrePurchaseOrder>("PrePurchaseOrders"); }
        }

        [Association("ProjectHeader-PurchaseOrders")]
        public XPCollection<PurchaseOrder> PurchaseOrders
        {
            get { return GetCollection<PurchaseOrder>("PurchaseOrders"); }
        }

        [Association("ProjectHeader-InventoryTransfers")]
        public XPCollection<InventoryTransfer> InventoryTransfers
        {
            get { return GetCollection<InventoryTransfer>("InventoryTransfers"); }
        }

        [Association("ProjectHeader-PurchaseRequisitions")]
        public XPCollection<PurchaseRequisition> PurchaseRequisitions
        {
            get { return GetCollection<PurchaseRequisition>("PurchaseRequisitions"); }
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