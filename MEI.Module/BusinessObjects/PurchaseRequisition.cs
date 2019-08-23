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
    [NavigationItem("Purchase")]
    [DefaultProperty("Code")]
    [ListViewFilter("AllDataPurchaseRequisition", "", "All Data", "All Data In Purchase Requisition", 1, true)]
    [ListViewFilter("OpenPurchaseRequisition", "Status = 'Open'", "Open", "Open Data Status In Purchase Requisition", 2, true)]
    [ListViewFilter("ProgressPurchaseRequisition", "Status = 'Progress'", "Progress", "Progress Data Status In Purchase Requisition", 3, true)]
    [ListViewFilter("LockPurchaseRequisition", "Status = 'Close'", "Close", "Close Data Status In Purchase Requisition", 4, true)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PurchaseRequisition : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private bool _activationPosting;
        private string _code;
        private string _description;
        private DirectionType _directionType;
        private Status _status;
        private DateTime _statusDate;
        private PrePurchaseOrder _prePurchaseOrder;
        private ProjectHeader _projectHeader;
        private string _signCode;
        private GlobalFunction _globFunc;

        public PurchaseRequisition(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (!IsLoading)
            {
                _globFunc = new GlobalFunction();
                this.Code = _globFunc.GetNumberingUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.PurchaseRequisition);
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
        [Appearance("ProjectRequisitionCodeClose", Enabled = false)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        [Appearance("PurchaseRequisitionDescriptionClose", Criteria = "ActivationPosting = true", Enabled = false)]
        [Size(512)]
        public string Description
        {
            get { return _description; }
            set { SetPropertyValue("Description", ref _description, value); }
        }

        [Appearance("PurchaseRequisitionDirectionTypeClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public DirectionType DirectionType
        {
            get { return _directionType; }
            set { SetPropertyValue("DirectionType", ref _directionType, value); }
        }

        [Appearance("ProjectRequisitionStatusClose", Enabled = false)]
        public Status Status
        {
            get { return _status; }
            set { SetPropertyValue("Status", ref _status, value); }
        }

        [Appearance("ProjectRequisitionStatusDateClose", Enabled = false)]
        public DateTime StatusDate
        {
            get { return _statusDate; }
            set { SetPropertyValue("StatusDate", ref _statusDate, value); }
        }

        [Appearance("PurchaseRequisitionPrePurchaseOrderClose", Criteria = "ActivationPosting = true", Enabled = false)]
        [Association("PrePurchaseOrder-PurchaseRequisitions")]
        public PrePurchaseOrder PrePurchaseOrder
        {
            get { return _prePurchaseOrder; }
            set { SetPropertyValue("PrePurchaseOrder", ref _prePurchaseOrder, value); }
        }

        [Appearance("PurchaseRequisitionProjectHeaderClose", Criteria = "ActivationPosting = true", Enabled = false)]
        [Association("ProjectHeader-PurchaseRequisitions")]
        public ProjectHeader ProjectHeader
        {
            get { return _projectHeader; }
            set { SetPropertyValue("ProjectHeader", ref _projectHeader, value); }
        }

        [Appearance("PurchaseRequisitionSignCodeClose", Enabled = false)]
        public string SignCode
        {
            get { return _signCode; }
            set { SetPropertyValue("SignCode", ref _signCode, value); }
        }

        [Association("PurchaseRequisition-PurchaseRequisitionLines")]
        public XPCollection<PurchaseRequisitionLine> PurchaseRequisitionLines
        {
            get { return GetCollection<PurchaseRequisitionLine>("PurchaseRequisitionLines"); }
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