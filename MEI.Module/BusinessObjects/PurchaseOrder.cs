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
    [ListViewFilter("AllDataPurchaseOrder", "", "All Data", "All Data In Purchase Order", 1, true)]
    [ListViewFilter("OpenPurchaseOrder", "Status = 'Open'", "Open", "Open Data Status In Purchase Order", 2, true)]
    [ListViewFilter("ProgressPurchaseOrder", "Status = 'Progress'", "Progress", "Progress Data Status In Purchase Order", 3, true)]
    [ListViewFilter("LockPurchaseOrder", "Status = 'Lock'", "Lock", "Lock Data Status In Purchase Order", 4, true)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PurchaseOrder : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private bool _activationPosting;
        private string _code;
        private BusinessPartner _buyFromVendor;
        private string _buyFromContact;
        private Country _buyFromCountry;
        private City _buyFromCity;
        private DateTime _orderDate;
        private DateTime _documentDate;
        private string _vendorOrderNo;
        private string _vendorShipmentNo;
        private string _vendorInvoiceNo;
        private TermOfPayment _top;
        private DirectionType _directionType;
        private string _remarkTOP;
        private Status _status;
        private DateTime _statusDate;
        private DateTime _estimatedDate;
        private ProjectHeader _projectHeader;
        private PurchaseRequisition _purchaseRequisition;
        private string _signCode;
        private GlobalFunction _globFunc;

        public PurchaseOrder(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (!IsLoading)
            {
                _globFunc = new GlobalFunction();
                this.Code = _globFunc.GetNumberingUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.PurchaseOrder);
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
        [Appearance("PurchaseOrderCodeClose", Enabled = false)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        [DataSourceCriteria("Active = true")]
        [Appearance("PurchaseOrderBuyFromVendorClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public BusinessPartner BuyFromVendor
        {
            get { return _buyFromVendor; }
            set { SetPropertyValue("BuyFromVendor", ref _buyFromVendor, value); }
        }

        [Appearance("PurchaseOrderBuyFromContactClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public string BuyFromContact
        {
            get { return _buyFromContact; }
            set { SetPropertyValue("BuyFromContact", ref _buyFromContact, value); }
        }

        [DataSourceCriteria("Active = true")]
        [Appearance("PurchaseOrderBuyFromCountryClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public Country BuyFromCountry
        {
            get { return _buyFromCountry; }
            set { SetPropertyValue("BuyFromCountry", ref _buyFromCountry, value); }
        }

        [DataSourceCriteria("Active = true")]
        [Appearance("PurchaseOrderBuyFromCityClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public City BuyFromCity
        {
            get { return _buyFromCity; }
            set { SetPropertyValue("BuyFromCity", ref _buyFromCity, value); }
        }

        [Appearance("PurchaseOrderOrderDateClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public DateTime OrderDate
        {
            get { return _orderDate; }
            set { SetPropertyValue("OrderDate", ref _orderDate, value); }
        }

        [Appearance("PurchaseOrderDocumentDateClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public DateTime DocumentDate
        {
            get { return _documentDate; }
            set { SetPropertyValue("DocumentDate", ref _documentDate, value); }
        }

        [Appearance("PurchaseOrderVendorOrderNoClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public string VendorOrderNo
        {
            get { return _vendorOrderNo; }
            set { SetPropertyValue("VendorOrderNo", ref _vendorOrderNo, value); }
        }

        [Appearance("PurchaseOrderVendorShipmentNoClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public string VendorShipmentNo
        {
            get { return _vendorShipmentNo; }
            set { SetPropertyValue("VendorShipmentNo", ref _vendorShipmentNo, value); }
        }

        [Appearance("PurchaseOrderVendorInvoiceNoClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public string VendorInvoiceNo
        {
            get { return _vendorInvoiceNo; }
            set { SetPropertyValue("VendorInvoiceNo", ref _vendorInvoiceNo, value); }
        }

        [DataSourceCriteria("Active = true")]
        [Appearance("PurchaseOrderTOPClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public TermOfPayment TOP
        {
            get { return _top; }
            set { SetPropertyValue("TOP", ref _top, value); }
        }

        [Appearance("PurchaseOrderOrderCategoryClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public DirectionType DirectionType
        {
            get { return _directionType; }
            set { SetPropertyValue("DirectionType", ref _directionType, value); }
        }

        [Appearance("PurchaseOrderRemarkTOPClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public string RemarkTOP
        {
            get { return _remarkTOP; }
            set { SetPropertyValue("RemarkTOP", ref _remarkTOP, value); }
        }

        [Appearance("PurchaseOrderStatusClose", Enabled = false)]
        public Status Status
        {
            get { return _status; }
            set { SetPropertyValue("Status", ref _status, value); }
        }

        [Appearance("PurchaseOrderStatusDateClose", Enabled = false)]
        public DateTime StatusDate
        {
            get { return _statusDate; }
            set { SetPropertyValue("StatusDate", ref _statusDate, value); }
        }

        [Appearance("PurchaseOrderEstimatedDateClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public DateTime EstimatedDate
        {
            get { return _estimatedDate; }
            set { SetPropertyValue("EstimatedDate", ref _estimatedDate, value); }
        }

        [Appearance("PurchaseOrderPurchaseProjectHeaderClose", Enabled = false)]
        [Association("ProjectHeader-PurchaseOrders")]
        public ProjectHeader ProjectHeader
        {
            get { return _projectHeader; }
            set { SetPropertyValue("ProjectHeader", ref _projectHeader, value); }
        }

        [Appearance("PurchaseOrderPurchaseRequisitionClose", Enabled = false)]
        public PurchaseRequisition PurchaseRequisition
        {
            get { return _purchaseRequisition; }
            set { SetPropertyValue("PurchaseRequisition", ref _purchaseRequisition, value); }
        }

        [Appearance("PurchaseOrderSignCodeClose", Enabled = false)]
        public string SignCode
        {
            get { return _signCode; }
            set { SetPropertyValue("SignCode", ref _signCode, value); }
        }

        [Association("PurchaseOrder-PurchaseOrderLines")]
        public XPCollection<PurchaseOrderLine> PurchaseOrderLines
        {
            get { return GetCollection<PurchaseOrderLine>("PurchaseOrderLines"); }
        }

        [Association("PurchaseOrder-InventoryTransfers")]
        public XPCollection<InventoryTransfer> InventoryTransfers
        {
            get { return GetCollection<InventoryTransfer>("InventoryTransfers"); }
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