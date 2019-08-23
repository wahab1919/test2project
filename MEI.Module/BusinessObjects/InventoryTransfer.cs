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
    [NavigationItem("Inventory")]
    [DefaultProperty("Code")]
    [ListViewFilter("AllDataInventoryTransfer", "", "All Data", "All Data In Inventory Transfer", 1, true)]
    [ListViewFilter("OpenInventoryTransfer", "Status = 'Open'", "Open", "Open Data Status In Inventory Transfer", 2, true)]
    [ListViewFilter("ProgressInventoryTransfer", "Status = 'Progress'", "Progress", "Progress Data Status In Inventory Transfer", 3, true)]
    [ListViewFilter("LockInventoryTransfer", "Status = 'Close'", "Close", "Close Data Status In Inventory Transfer", 4, true)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class InventoryTransfer : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private bool _activationPosting;
        private string _code;
        private string _name;
        private DirectionType _directionType;
        private InventoryMovingType _inventoryMovingType;
        private ObjectList _objectList;
        XPCollection<DocumentType> _availableDocumentType;
        private DocumentType _documentType;
        private string _docNo;
        private Status _status;
        private DateTime _statusDate;
        private DateTime _estimatedDate;
        private Location _location;
        private StockType _stockType;
        private ProjectHeader _projectHeader;
        private PurchaseOrder _purchaseOrder;
        private GlobalFunction _globFunc;

        public InventoryTransfer(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (!IsLoading)
            {
                _globFunc = new GlobalFunction();
                DateTime now = DateTime.Now;
                this.Code = _globFunc.GetNumberingUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.InventoryTransfer);
                this.ObjectList = ObjectList.InventoryTransfer;
                this.Status = Status.Open;
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
        [Appearance("InventoryTransferCodeEnabled", Enabled = false)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        [Appearance("InventoryTransferNameClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public string Name
        {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }

        [Appearance("InventoryTransferDirectionTypeClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public DirectionType DirectionType
        {
            get { return _directionType; }
            set { SetPropertyValue("DirectionType", ref _directionType, value); }
        }

        [Appearance("InventoryTransferInventoryMovingTypeClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public InventoryMovingType InventoryMovingType
        {
            get { return _inventoryMovingType; }
            set { SetPropertyValue("InventoryMovingType", ref _inventoryMovingType, value); }
        }

        [Browsable(false)]
        public ObjectList ObjectList
        {
            get { return _objectList; }
            set { SetPropertyValue("ObjectList", ref _objectList, value); }
        }

        [Browsable(false)]
        public XPCollection<DocumentType> AvailableDocumentType
        {
            get
            {
                if (ObjectList == ObjectList.None)
                {
                    _availableDocumentType = new XPCollection<DocumentType>(Session);
                }
                else
                {
                    _availableDocumentType = new XPCollection<DocumentType>(Session,
                                              new GroupOperator(GroupOperatorType.And,
                                              new BinaryOperator("ObjectList", this.ObjectList),
                                              new BinaryOperator("InventoryMovingType", this.InventoryMovingType),
                                              new BinaryOperator("DirectionType", this.DirectionType),
                                              new BinaryOperator("Active", true)));
                }
                return _availableDocumentType;

            }
        }

        [Appearance("InventoryTransferDocumentTypeClose", Criteria = "ActivationPosting = true", Enabled = false)]
        [ImmediatePostData()]
        [DataSourceProperty("AvailableDocumentType", DataSourcePropertyIsNullMode.SelectAll)]
        public DocumentType DocumentType
        {
            get { return _documentType; }
            set {
                SetPropertyValue("DocumentType", ref _documentType, value);
                if(!IsLoading)
                {
                    if(_documentType != null)
                    {
                        if (_documentType.StockType != StockType.None)
                        {
                            this.StockType = this._documentType.StockType;
                        }
                    }
                }
            }
        }

        [Appearance("InventoryTransferDocNoClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public string DocNo
        {
            get { return _docNo; }
            set { SetPropertyValue("DocNo", ref _docNo, value); }
        }

        [Appearance("InventoryTransferStatusEnabled", Enabled = false)]
        public Status Status
        {
            get { return _status; }
            set { SetPropertyValue("Status", ref _status, value); }
        }

        [Appearance("InventoryTransferStatusDateEnabled", Enabled = false)]
        public DateTime StatusDate
        {
            get { return _statusDate; }
            set { SetPropertyValue("StatusDate", ref _statusDate, value); }
        }

        [Appearance("InventoryTransferEstimatedEnabled", Enabled = false)]
        public DateTime EstimatedDate
        {
            get { return _estimatedDate; }
            set { SetPropertyValue("EstimatedDate", ref _estimatedDate, value); }
        }

        [Appearance("InventoryTransferLocationClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public Location Location
        {
            get { return _location; }
            set { SetPropertyValue("Location", ref _location, value); }
        }

        [Appearance("InventoryTransferStockTypeClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public StockType StockType
        {
            get { return _stockType; }
            set { SetPropertyValue("StockType", ref _stockType, value); }
        }

        [Association("ProjectHeader-InventoryTransfers")]
        public ProjectHeader ProjectHeader
        {
            get { return _projectHeader; }
            set { SetPropertyValue("ProjectHeader", ref _projectHeader, value); }
        }

        [Association("PurchaseOrder-InventoryTransfers")]
        public PurchaseOrder PurchaseOrder
        {
            get { return _purchaseOrder; }
            set { SetPropertyValue("PurchaseOrder", ref _purchaseOrder, value); }
        }

        [Association("InventoryTransfer-InventoryTransferLines")]
        public XPCollection<InventoryTransferLine> InventoryTransferLines
        {
            get { return GetCollection<InventoryTransferLine>("InventoryTransferLines"); }
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