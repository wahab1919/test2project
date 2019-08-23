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
    [ListViewFilter("AllDataInventoryTransferLine", "", "All Data", "All Data In Inventory Transfer Line", 1, true)]
    [ListViewFilter("OpenInventoryTransferLine", "Status = 'Open'", "Open", "Open Data Status In Inventory Transfer Line", 2, true)]
    [ListViewFilter("ProgressInventoryTransferLine", "Status = 'Progress'", "Progress", "Progress Data Status In Inventory Transfer Line", 3, true)]
    [ListViewFilter("LockInventoryTransferLine", "Status = 'Close'", "Close", "Close Data Status In Inventory Transfer Line", 4, true)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class InventoryTransferLine : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private bool _activationPosting;
        private int _no;
        private string _code;
        private string _name;
        private Item _item;
        private string _description;
        private XPCollection<UnitOfMeasure> _availableUnitOfMeasure;
        #region InisialisasiMaxQty
        private double _mxDQTY;
        private UnitOfMeasure _mxDUom;
        private double _mxQty;
        private UnitOfMeasure _mxUom;
        private double _mxTQty;
        #endregion InisialisasiMaxQty
        private double _dQty;
        private UnitOfMeasure _dUom;
        private double _qty;
        private UnitOfMeasure _uom;
        private double _tQty;
        private double _rmDQty;
        private double _rmQty;
        private double _rmTQty;
        private Location _location;
        private BinLocation _binLocation;
        private DateTime _estimatedDate;
        private DateTime _actualDate;
        private Status _status;
        private DateTime _statusDate;
        private StockType _stockType;
        private int _processCount;
        private InventoryTransfer _inventoryTransfer;
        private GlobalFunction _globFunc;

        public InventoryTransferLine(Session session)
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
                this.Code = _globFunc.GetNumberingUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.InventoryTransferLine);
                this.Status = Status.Open;
                this.StatusDate = now;
            }
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (!IsLoading && IsSaving)
            {
                UpdateNo();
            }
        }

        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (!IsLoading)
            {
                RecoveryDeleteNo();
            }
        }

        [Browsable(false)]
        public bool ActivationPosting
        {
            get { return _activationPosting; }
            set { SetPropertyValue("ActivationPosting", ref _activationPosting, value); }
        }

        [Appearance("InventoryTransferLineNoEnabled", Enabled = false)]
        public int No
        {
            get { return _no; }
            set { SetPropertyValue("No", ref _no, value); }
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Appearance("InventoryTransferLineCodeEnabled", Enabled = false)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        [Appearance("InventoryTransferLineNameClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public string Name
        {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }

        [Appearance("InventoryTransferLineItemClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public Item Item
        {
            get { return _item; }
            set { SetPropertyValue("Item", ref _item, value); }
        }

        [Appearance("InventoryTransferLineDescriptionClose", Criteria = "ActivationPosting = true", Enabled = false)]
        [Size(512)]
        public string Description
        {
            get { return _description; }
            set { SetPropertyValue("Description", ref _description, value); }
        }

        [Browsable(false)]
        public XPCollection<UnitOfMeasure> AvailableUnitOfMeasure
        {
            get
            {
                if (Item == null)
                {
                    _availableUnitOfMeasure = new XPCollection<UnitOfMeasure>(Session);
                }
                else
                {
                    XPQuery<UnitOfMeasure> _locQueryUnitOfMeasures = new XPQuery<UnitOfMeasure>(Session);
                    XPQuery<ItemUnitOfMeasure> _locQueryItemUnitOfMeasures = new XPQuery<ItemUnitOfMeasure>(Session);
                    var _results = from _locUOM in _locQueryUnitOfMeasures
                                   join _locIUOM in _locQueryItemUnitOfMeasures on _locUOM.Oid equals _locIUOM.UOM.Oid
                                   where (_locIUOM.Item.Oid == this.Item.Oid)
                                   select _locUOM;

                    _availableUnitOfMeasure = new XPCollection<UnitOfMeasure>(Session, _results.ToList());

                }
                return _availableUnitOfMeasure;

            }
        }

        #region MaxDefaultQty

        [Appearance("InventoryTransferLineMxDQtyClose", Enabled = false)]
        [ImmediatePostData()]
        public double MxDQty
        {
            get { return _mxDQTY; }
            set
            {
                SetPropertyValue("MxDQty", ref _mxDQTY, value);
                if (!IsLoading)
                {
                    SetMaxTotalQty();
                }
            }
        }

        [ImmediatePostData()]
        [DataSourceCriteria("Active = true")]
        [Appearance("InventoryTransferLineMxDUOMClose", Enabled = false)]
        public UnitOfMeasure MxDUOM
        {
            get { return _mxDUom; }
            set
            {
                SetPropertyValue("MxDUOM", ref _mxDUom, value);
                if (!IsLoading)
                {
                    SetMaxTotalQty();
                }
            }
        }

        [ImmediatePostData()]
        [Appearance("InventoryTransferLineMxQtyClose", Enabled = false)]
        public double MxQty
        {
            get { return _mxQty; }
            set
            {
                SetPropertyValue("MxQty", ref _mxQty, value);
                if (!IsLoading)
                {
                    SetMaxTotalQty();
                }
            }
        }

        [DataSourceProperty("AvailableUnitOfMeasure", DataSourcePropertyIsNullMode.SelectAll)]
        [DataSourceCriteria("Active = true")]
        [Appearance("InventoryTransferLineMxUOMClose", Enabled = false)]
        public UnitOfMeasure MxUOM
        {
            get { return _mxUom; }
            set
            {
                SetPropertyValue("MxUOM", ref _mxUom, value);
                if (!IsLoading)
                {
                    SetMaxTotalQty();
                }
            }
        }

        [Appearance("InventoryTransferLineMxTQtyClose", Enabled = false)]
        public double MxTQty
        {
            get { return _mxTQty; }
            set { SetPropertyValue("MxTQty", ref _mxTQty, value); }
        }

        #endregion MaxDefaultQty

        #region DefaultQty

        [Appearance("InventoryTransferLineDQtyClose", Criteria = "ActivationPosting = true", Enabled = false)]
        [ImmediatePostData()]
        public double DQty
        {
            get { return _dQty; }
            set
            {
                SetPropertyValue("DQty", ref _dQty, value);
                if (!IsLoading)
                {
                    SetTotalQty();
                    if (this.ProcessCount > 0)
                    {
                        if (this.RmDQty == 0)
                        {
                            if (this._dQty > this.MxDQty || this._dQty <= this.MxDQty)
                            {
                                this._dQty = 0;
                            }
                        }

                        if (this.RmDQty > 0)
                        {
                            if (this._dQty > this.RmDQty)
                            {
                                this._dQty = this.RmDQty;
                            }
                        }
                    }

                    if (this.ProcessCount == 0)
                    {
                        if (this.DQty > this.MxDQty)
                        {
                            this.DQty = this.MxDQty;
                        }
                    }
                }
            }
        }

        [Appearance("InventoryTransferLineDUOMClose", Criteria = "ActivationPosting = true", Enabled = false)]
        [ImmediatePostData()]
        [DataSourceCriteria("Active = true")]
        public UnitOfMeasure DUOM
        {
            get { return _dUom; }
            set
            {
                SetPropertyValue("DUOM", ref _dUom, value);
                if (!IsLoading)
                {
                    SetTotalQty();
                }
            }
        }

        [Appearance("InventoryTransferLineQtyClose", Criteria = "ActivationPosting = true", Enabled = false)]
        [ImmediatePostData()]
        public double Qty
        {
            get { return _qty; }
            set
            {
                SetPropertyValue("Qty", ref _qty, value);
                if (!IsLoading)
                {
                    SetTotalQty();
                    if (this.ProcessCount > 0)
                    {
                        if (this.RmQty == 0)
                        {
                            if (this._qty > this.MxQty || this._qty <= this.MxQty)
                            {
                                this._qty = 0;
                            }
                        }

                        if (this.RmDQty > 0)
                        {
                            if (this._qty > this.RmDQty)
                            {
                                this._qty = this.RmDQty;
                            }
                        }
                    }

                    if (this.ProcessCount == 0)
                    {
                        if (this.DQty > this.MxDQty)
                        {
                            this.DQty = this.MxDQty;
                        }
                    }
                }
            }
        }

        [Appearance("InventoryTransferLineUOMClose", Criteria = "ActivationPosting = true", Enabled = false)]
        [ImmediatePostData()]
        [DataSourceProperty("AvailableUnitOfMeasure", DataSourcePropertyIsNullMode.SelectAll)]
        [DataSourceCriteria("Active = true")]
        public UnitOfMeasure UOM
        {
            get { return _uom; }
            set
            {
                SetPropertyValue("UOM", ref _uom, value);
                if (!IsLoading)
                {
                    SetTotalQty();
                }
            }
        }

        [Appearance("InventoryTransferLineTotalQtyEnabled", Enabled = false)]
        public double TQty
        {
            get { return _tQty; }
            set { SetPropertyValue("TQty", ref _tQty, value); }
        }

        #endregion DefaultQty

        #region RemainQty

        [ImmediatePostData()]
        [Appearance("InventoryTransferLineRmDQtyClose", Enabled = false)]
        public double RmDQty
        {
            get { return _rmDQty; }
            set {
                SetPropertyValue("RmDQty", ref _rmDQty, value);
                if(!IsLoading)
                {
                    SetRemainTotalQty();
                }
            }
        }

        [ImmediatePostData()]
        [Appearance("InventoryTransferLineRmQtyClose", Enabled = false)]
        public double RmQty
        {
            get { return _rmQty; }
            set {
                SetPropertyValue("RmQty", ref _rmQty, value);
                if (!IsLoading)
                {
                    SetRemainTotalQty();
                }
            }
        }

        [Appearance("InventoryTransferLineTQtyClose", Enabled = false)]
        public double RmTQty
        {
            get { return _rmTQty; }
            set { SetPropertyValue("RmTQty", ref _rmTQty, value); }
        }

        #endregion RemainQty

        public Location Location
        {
            get { return _location; }
            set { SetPropertyValue("Location", ref _location, value); }
        }

        public BinLocation BinLocation
        {
            get { return _binLocation; }
            set { SetPropertyValue("BinLocation", ref _binLocation, value); }
        }

        [Appearance("InventoryTransferLineEstimatedDateEnabled", Enabled = false)]
        public DateTime EstimatedDate
        {
            get { return _estimatedDate; }
            set { SetPropertyValue("EstimatedDate", ref _estimatedDate, value); }
        }

        [Appearance("InventoryTransferLineActualDateEnabled", Enabled = false)]
        public DateTime ActualDate
        {
            get { return _actualDate; }
            set { SetPropertyValue("ActualDate", ref _actualDate, value); }
        }

        [Appearance("InventoryTransferLineStatusEnabled", Enabled = false)]
        public Status Status
        {
            get { return _status; }
            set { SetPropertyValue("Status", ref _status, value); }
        }

        [Appearance("InventoryTransferLineStatusDateEnabled", Enabled = false)]
        public DateTime StatusDate
        {
            get { return _statusDate; }
            set { SetPropertyValue("StatusDate", ref _statusDate, value); }
        }

        [Appearance("InventoryTransferLineStockTypeClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public StockType StockType
        {
            get { return _stockType; }
            set { SetPropertyValue("StockType", ref _stockType, value); }
        }

        [Appearance("InventoryTransferLineProcessCountEnabled", Enabled = false)]
        public int ProcessCount
        {
            get { return _processCount; }
            set { SetPropertyValue("ProcessCount", ref _processCount, value); }
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [ImmediatePostData()]
        [Association("InventoryTransfer-InventoryTransferLines")]
        public InventoryTransfer InventoryTransfer
        {
            get { return _inventoryTransfer; }
            set {
                SetPropertyValue("InventoryTransfer", ref _inventoryTransfer, value);
                if(!IsLoading)
                {
                    if(_inventoryTransfer != null)
                    {
                        this.EstimatedDate = _inventoryTransfer.EstimatedDate;
                        this.StockType = _inventoryTransfer.StockType;
                    }
                }
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

        //================================================== Code Only ==================================================

        #region No

        public void UpdateNo()
        {
            try
            {
                if (!IsLoading && Session.IsNewObject(this))
                {
                    if (this.InventoryTransfer != null)
                    {
                        object _makRecord = Session.Evaluate<InventoryTransferLine>(CriteriaOperator.Parse("Max(No)"), CriteriaOperator.Parse("InventoryTransfer=?", this.InventoryTransfer));
                        this.No = Convert.ToInt32(_makRecord) + 1;
                        this.Save();
                        RecoveryUpdateNo();
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = InventoryTransferLine " + ex.ToString());
            }
        }

        public void RecoveryUpdateNo()
        {
            try
            {
                if (this.InventoryTransfer != null)
                {
                    InventoryTransfer _numHeader = Session.FindObject<InventoryTransfer>
                                                (new BinaryOperator("Code", this.InventoryTransfer.Code));

                    XPCollection<InventoryTransferLine> _numLines = new XPCollection<InventoryTransferLine>
                                                (Session, new BinaryOperator("InventoryTransfer", _numHeader),
                                                new SortProperty("No", DevExpress.Xpo.DB.SortingDirection.Ascending));
                    if (_numLines != null)
                    {
                        int i1 = 0;
                        foreach (InventoryTransferLine _numLine in _numLines)
                        {
                            i1 += 1;
                            _numLine.No = i1;
                            _numLine.Save();
                        }
                        i1 = 1;
                        Session.CommitTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = InventoryTransferLine " + ex.ToString());
            }
        }

        public void RecoveryDeleteNo()
        {
            try
            {
                if (this.InventoryTransfer != null)
                {
                    InventoryTransfer _numHeader = Session.FindObject<InventoryTransfer>
                                                (new BinaryOperator("Code", this.InventoryTransfer.Code));

                    XPCollection<InventoryTransferLine> _numLines = new XPCollection<InventoryTransferLine>
                                                (Session, new GroupOperator(GroupOperatorType.And,
                                                 new BinaryOperator("This", this, BinaryOperatorType.NotEqual),
                                                 new BinaryOperator("InventoryTransfer", _numHeader)),
                                                 new SortProperty("No", DevExpress.Xpo.DB.SortingDirection.Ascending));
                    if (_numLines != null)
                    {
                        int i = 0;
                        foreach (InventoryTransferLine _numLine in _numLines)
                        {
                            i += 1;
                            _numLine.No = i;
                            _numLine.Save();
                        }
                        Session.CommitTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = InventoryTransferLine " + ex.ToString());
            }
        }

        #endregion No

        private void SetTotalQty()
        {
            try
            {
                double _locInvLineTotal = 0;
                DateTime now = DateTime.Now;
                if (this.InventoryTransfer != null)
                {
                    ItemUnitOfMeasure _locItemUOM = Session.FindObject<ItemUnitOfMeasure>
                                                        (new GroupOperator(GroupOperatorType.And,
                                                         new BinaryOperator("Item", this.Item),
                                                         new BinaryOperator("UOM", this.UOM),
                                                         new BinaryOperator("DefaultUOM", this.DUOM),
                                                         new BinaryOperator("Active", true)));
                    if (_locItemUOM != null)
                    {
                        if (_locItemUOM.Conversion < _locItemUOM.DefaultConversion)
                        {
                            _locInvLineTotal = this.Qty * _locItemUOM.DefaultConversion + this.DQty;
                        }
                        else if (_locItemUOM.Conversion > _locItemUOM.DefaultConversion)
                        {
                            _locInvLineTotal = this.Qty / _locItemUOM.Conversion + this.DQty;
                        }
                        else if (_locItemUOM.Conversion == _locItemUOM.DefaultConversion)
                        {
                            _locInvLineTotal = this.Qty + this.DQty;
                        }

                        this.TQty = _locInvLineTotal;
                    }
                    else
                    {
                        _locInvLineTotal = this.Qty + this.DQty;
                        this.TQty = _locInvLineTotal;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = InventoryTransferLine " + ex.ToString());
            }
        }

        private void SetMaxTotalQty()
        {
            try
            {
                double _locInvLineTotal = 0;
                DateTime now = DateTime.Now;
                if (this.InventoryTransfer != null)
                {
                    ItemUnitOfMeasure _locItemUOM = Session.FindObject<ItemUnitOfMeasure>
                                                        (new GroupOperator(GroupOperatorType.And,
                                                         new BinaryOperator("Item", this.Item),
                                                         new BinaryOperator("UOM", this.MxUOM),
                                                         new BinaryOperator("DefaultUOM", this.MxDUOM),
                                                         new BinaryOperator("Active", true)));
                    if (_locItemUOM != null)
                    {
                        if (_locItemUOM.Conversion < _locItemUOM.DefaultConversion)
                        {
                            _locInvLineTotal = this.MxQty * _locItemUOM.DefaultConversion + this.MxDQty;
                        }
                        else if (_locItemUOM.Conversion > _locItemUOM.DefaultConversion)
                        {
                            _locInvLineTotal = this.MxQty / _locItemUOM.Conversion + this.MxDQty;
                        }
                        else if (_locItemUOM.Conversion == _locItemUOM.DefaultConversion)
                        {
                            _locInvLineTotal = this.MxQty + this.MxDQty;
                        }

                        this.MxTQty = _locInvLineTotal;
                    }
                    else
                    {
                        _locInvLineTotal = this.MxQty + this.MxDQty;
                        this.MxTQty = _locInvLineTotal;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = InventoryTransferLine " + ex.ToString());
            }
        }

        private void SetRemainTotalQty()
        {
            try
            {
                double _locInvLineTotal = 0;
                DateTime now = DateTime.Now;
                if (this.InventoryTransfer != null)
                {
                    ItemUnitOfMeasure _locItemUOM = Session.FindObject<ItemUnitOfMeasure>
                                                        (new GroupOperator(GroupOperatorType.And,
                                                         new BinaryOperator("Item", this.Item),
                                                         new BinaryOperator("UOM", this.UOM),
                                                         new BinaryOperator("DefaultUOM", this.DUOM),
                                                         new BinaryOperator("Active", true)));
                    if (_locItemUOM != null)
                    {
                        if (_locItemUOM.Conversion < _locItemUOM.DefaultConversion)
                        {
                            _locInvLineTotal = this.RmQty * _locItemUOM.DefaultConversion + this.RmDQty;
                        }
                        else if (_locItemUOM.Conversion > _locItemUOM.DefaultConversion)
                        {
                            _locInvLineTotal = this.RmQty / _locItemUOM.Conversion + this.RmDQty;
                        }
                        else if (_locItemUOM.Conversion == _locItemUOM.DefaultConversion)
                        {
                            _locInvLineTotal = this.RmQty + this.RmDQty;
                        }

                        this.RmTQty = _locInvLineTotal;
                    }
                    else
                    {
                        _locInvLineTotal = this.RmQty + this.RmDQty;
                        this.RmTQty = _locInvLineTotal;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = InventoryTransferLine " + ex.ToString());
            }
        }
    }
}
