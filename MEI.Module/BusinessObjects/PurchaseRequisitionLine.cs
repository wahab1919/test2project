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
    [ListViewFilter("AllDataPurchaseRequisitionLine", "", "All Data", "All Data In Purchase Requisition Line", 1, true)]
    [ListViewFilter("OpenPurchaseRequisitionLine", "Status = 'Open'", "Open", "Open Data Status In Purchase Requisition Line", 2, true)]
    [ListViewFilter("ProgressPurchaseRequisitionLine", "Status = 'Progress'", "Progress", "Progress Data Status In Purchase Requisition Line", 3, true)]
    [ListViewFilter("LockPurchaseRequisitionLine", "Status = 'Close'", "Close", "Close Data Status In Purchase Requisition Line", 4, true)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PurchaseRequisitionLine : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private bool _activationPosting;
        private int _no;
        private string _code;
        private OrderType _purchaseType;
        private Item _item;
        private string _description;
        private XPCollection<UnitOfMeasure> _availableUnitOfMeasure;
        #region InisialisasiMaxQty
        private double _mxDQTY;
        private UnitOfMeasure _mxDUom;
        private double _mxQty;
        private UnitOfMeasure _mxUom;
        private double _mxTQty;
        private double _mxUPrice;
        private double _mxTUPrice;
        #endregion InisialisasiMaxQty
        private double _dQty;
        private UnitOfMeasure _dUom;
        private double _qty;
        private UnitOfMeasure _uom;
        private double _tQty;
        private Status _status;
        private DateTime _statusDate;
        private PurchaseRequisition _purchaseRequisition;
        private GlobalFunction _globFunc;

        public PurchaseRequisitionLine(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (!IsLoading)
            {
                _globFunc = new GlobalFunction();
                this.Code = _globFunc.GetNumberingUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.PurchaseRequisitionLine);
                DateTime now = DateTime.Now;
                this.Status = CustomProcess.Status.Open;
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

        [Appearance("PurchaseRequisitionLineNoEnabled", Enabled = false)]
        public int No
        {
            get { return _no; }
            set { SetPropertyValue("No", ref _no, value); }
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Appearance("PurchaseRequisitionLineCodeEnabled", Enabled = false)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        [Appearance("PurchaseRequisitionLinePurchaseTypeClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public OrderType PurchaseType
        {
            get { return _purchaseType; }
            set { SetPropertyValue("PurchaseType", ref _purchaseType, value); }
        }

        [DataSourceCriteria("Active = true")]
        [Appearance("PurchaseRequisitionLineItemClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public Item Item
        {
            get { return _item; }
            set { SetPropertyValue("Item", ref _item, value); }
        }

        [Appearance("PurchaseRequisitionLineDescriptionClose", Criteria = "ActivationPosting = true", Enabled = false)]
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

        [Appearance("PurchaseRequisitionLineMxDQtyClose", Enabled = false)]
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
        [Appearance("PurchaseRequisitionLineMxDUOMClose", Enabled = false)]
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
        [Appearance("PurchaseRequisitionLineMxQtyClose", Enabled = false)]
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
        [Appearance("PurchaseRequisitionLineMxUOMClose", Enabled = false)]
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

        [Appearance("PrePurchaseRequisitionMxTQtyClose", Enabled = false)]
        public double MxTQty
        {
            get { return _mxTQty; }
            set { SetPropertyValue("MxTQty", ref _mxTQty, value); }
        }

        #endregion MaxDefaultQty

        #region DefaultQty

        [Appearance("PurchaseRequisitionLineDQtyClose", Criteria = "ActivationPosting = true", Enabled = false)]
        [ImmediatePostData()]
        public double DQty
        {
            get { return _dQty; }
            set {
                SetPropertyValue("DQty", ref _dQty, value);
                if (!IsLoading)
                {
                    
                    if(this.MxDQty > 0)
                    {
                        if(this._dQty > this.MxDQty)
                        {
                            this._dQty = this.MxDQty;
                        }
                    }
                    SetTotalQty();
                }
            }
        }

        [DataSourceCriteria("Active = true")]
        [Appearance("PurchaseRequisitionLineDUOMClose", Criteria = "ActivationPosting = true", Enabled = false)]
        [ImmediatePostData()]
        public UnitOfMeasure DUOM
        {
            get { return _dUom; }
            set {
                SetPropertyValue("DUOM", ref _dUom, value);
                if (!IsLoading)
                {
                    SetTotalQty();
                }
            }
        }

        [Appearance("PurchaseRequisitionLineQtyClose", Criteria = "ActivationPosting = true", Enabled = false)]
        [ImmediatePostData()]
        public double Qty
        {
            get { return _qty; }
            set {
                SetPropertyValue("Qty", ref _qty, value);
                if (!IsLoading)
                {
                    if(this.MxQty > 0)
                    {
                        if(this._qty > this.MxQty)
                        {
                            this._qty = this.MxQty;
                        }
                    }
                    SetTotalQty();
                }
            }
        }

        [Appearance("PurchaseRequisitionLineUOMClose", Criteria = "ActivationPosting = true", Enabled = false)]
        [ImmediatePostData()]
        [DataSourceProperty("AvailableUnitOfMeasure", DataSourcePropertyIsNullMode.SelectAll)]
        [DataSourceCriteria("Active = true")]
        public UnitOfMeasure UOM
        {
            get { return _uom; }
            set {
                SetPropertyValue("UOM", ref _uom, value);
                if (!IsLoading)
                {
                    SetTotalQty();
                }
            }
        }

        [Appearance("PurchaseRequisitionLineTotalQtyEnabled", Enabled = false)]
        public double TQty
        {
            get { return _tQty; }
            set { SetPropertyValue("TQty", ref _tQty, value); }
        }

        #endregion DefaultQty

        [Appearance("PurchaseRequisitionLineStatusEnabled", Enabled = false)]
        public Status Status
        {
            get { return _status; }
            set { SetPropertyValue("Status", ref _status, value); }
        }

        [Appearance("PurchaseRequisitionLineStatusDateEnabled", Enabled = false)]
        public DateTime StatusDate
        {
            get { return _statusDate; }
            set { SetPropertyValue("StatusDate", ref _statusDate, value); }
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Appearance("PurchaseRequisitionLinePurchaseRequisitionEnabled", Enabled = false)]
        [Association("PurchaseRequisition-PurchaseRequisitionLines")]
        public PurchaseRequisition PurchaseRequisition
        {
            get { return _purchaseRequisition; }
            set { SetPropertyValue("PurchaseRequisition", ref _purchaseRequisition, value); }
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
                    if (this.PurchaseRequisition != null)
                    {
                        object _makRecord = Session.Evaluate<PurchaseOrderLine>(CriteriaOperator.Parse("Max(No)"), CriteriaOperator.Parse("PurchaseRequisition=?", this.PurchaseRequisition));
                        this.No = Convert.ToInt32(_makRecord) + 1;
                        this.Save();
                        RecoveryUpdateNo();
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = PurchaseRequisitionLine " + ex.ToString());
            }
        }

        public void RecoveryUpdateNo()
        {
            try
            {
                if (this.PurchaseRequisition != null)
                {
                    PurchaseRequisition _numHeader = Session.FindObject<PurchaseRequisition>
                                                (new BinaryOperator("Code", this.PurchaseRequisition.Code));

                    XPCollection<PurchaseRequisitionLine> _numLines = new XPCollection<PurchaseRequisitionLine>
                                                (Session, new BinaryOperator("PurchaseRequisition", _numHeader),
                                                new SortProperty("No", DevExpress.Xpo.DB.SortingDirection.Ascending));
                    if (_numLines != null)
                    {
                        int i1 = 0;
                        foreach (PurchaseRequisitionLine _numLine in _numLines)
                        {
                            i1 += 1;
                            _numLine.No = i1;
                            _numLine.Save();
                        }
                        i1 = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = PurchaseRequisitionLine " + ex.ToString());
            }
        }

        public void RecoveryDeleteNo()
        {
            try
            {
                if (this.PurchaseRequisition != null)
                {
                    PurchaseRequisition _numHeader = Session.FindObject<PurchaseRequisition>
                                                (new BinaryOperator("Code", this.PurchaseRequisition.Code));

                    XPCollection<PurchaseRequisitionLine> _numLines = new XPCollection<PurchaseRequisitionLine>
                                                (Session, new GroupOperator(GroupOperatorType.And,
                                                 new BinaryOperator("This", this, BinaryOperatorType.NotEqual),
                                                 new BinaryOperator("PurchaseRequisition", _numHeader)),
                                                 new SortProperty("No", DevExpress.Xpo.DB.SortingDirection.Ascending));
                    if (_numLines != null)
                    {
                        int i = 0;
                        foreach (PurchaseRequisitionLine _numLine in _numLines)
                        {
                            i += 1;
                            _numLine.No = i;
                            _numLine.Save();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = PurchaseRequisitionLine " + ex.ToString());
            }
        }

        #endregion No

        private void SetTotalQty()
        {
            try
            {
                double _locInvLineTotal = 0;
                DateTime now = DateTime.Now;
                if (this.PurchaseRequisition != null)
                {
                    if(this.Item != null && this.UOM != null && this.DUOM != null)
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
                Tracing.Tracer.LogError(" BusinessObject = PurchaseOrderLine " + ex.ToString());
            }
        }

        public void SetMaxTotalQty()
        {
            try
            {
                double _locInvLineTotal = 0;
                DateTime now = DateTime.Now;
                if (this.PurchaseRequisition != null)
                {
                    if(this.Item != null && this.MxUOM != null && this.MxDUOM != null)
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
                Tracing.Tracer.LogError(" BusinessObject = PurchaseOrderLine " + ex.ToString());
            }
        }
    }
}