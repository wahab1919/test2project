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
    [NavigationItem("Project Transaction")]
    [DefaultProperty("Code")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PrePurchaseOrder : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private bool _activationPosting;
        private string _code;
        private bool _choose; 
        private Item _item;
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
        private double _qty;
        private double _tQty;
        private double _rmDQty;
        private double _rmQty;
        private double _rmTQty;
        private DirectionType _directionType;
        private int _postedCount;
        private Status _status;
        private DateTime _statusDate;
        private ProjectHeader _projectHeader;
        private GlobalFunction _globFunc;

        public PrePurchaseOrder(Session session)
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
                this.Code = _globFunc.GetNumberingUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.PrePurchaseOrder);
                this.DirectionType = DirectionType.External;
                this.Status = Status.Open;
                this.StatusDate = now;
            }
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        
        //[ImmediatePostData()]
        [Browsable(false)]
        public bool ActivationPosting
        {
            get { return _activationPosting; }
            set { SetPropertyValue("ActivationPosting", ref _activationPosting, value); }
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Appearance("PrePurchaseOrderCodeClose", Enabled = false)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        [Appearance("PrePurchaseOrderChooseClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public bool Choose
        {
            get { return _choose; }
            set { SetPropertyValue("Choose", ref _choose, value); }
        }

        [DataSourceCriteria("Active = true")]
        [Appearance("PrePurchaseOrderItemClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public Item Item
        {
            get { return _item; }
            set { SetPropertyValue("Item", ref _item, value); }
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

        [Appearance("PrePurchaseOrderMxDQtyClose", Enabled = false)]
        [ImmediatePostData()]
        public double MxDQty
        {
            get { return _mxDQTY; }
            set {
                SetPropertyValue("MxDQty", ref _mxDQTY, value);
                if(!IsLoading)
                {
                    SetMaxTotalQty();
                    SetTotalUnitPrice();
                }
            }
        }

        [ImmediatePostData()]
        [DataSourceCriteria("Active = true")]
        [Appearance("PrePurchaseOrderMxDUOMClose", Enabled = false)]
        public UnitOfMeasure MxDUOM
        {
            get { return _mxDUom; }
            set {
                SetPropertyValue("MxDUOM", ref _mxDUom, value);
                if (!IsLoading)
                {
                    SetMaxTotalQty();
                    SetTotalUnitPrice();
                }
            }
        }

        [ImmediatePostData()]
        [Appearance("PrePurchaseOrderMxQtyClose", Enabled = false)]
        public double MxQty
        {
            get { return _mxQty; }
            set {
                SetPropertyValue("MxQty", ref _mxQty, value);
                if (!IsLoading)
                {
                    SetMaxTotalQty();
                    SetTotalUnitPrice();
                }
            }
        }

        [DataSourceProperty("AvailableUnitOfMeasure", DataSourcePropertyIsNullMode.SelectAll)]
        [DataSourceCriteria("Active = true")]
        [Appearance("PrePurchaseOrderMxUOMClose", Enabled = false)]
        public UnitOfMeasure MxUOM
        {
            get { return _mxUom; }
            set {
                SetPropertyValue("MxUOM", ref _mxUom, value);
                if (!IsLoading)
                {
                    SetMaxTotalQty();
                    SetTotalUnitPrice();
                }
            }
        }

        [Appearance("PrePurchaseOrderMxTQtyClose", Enabled = false)]
        public double MxTQty
        {
            get { return _mxTQty; }
            set { SetPropertyValue("MxTQty", ref _mxTQty, value); }
        }

        #endregion MaxDefaultQty

        #region VendorPrice

        [ImmediatePostData()]
        [Appearance("PrePurchaseOrderMxUPriceClose", Enabled = false)]
        public double MxUPrice
        {
            get { return _mxUPrice; }
            set
            {
                SetPropertyValue("MxUPrice", ref _mxUPrice, value);
                if (!IsLoading)
                {
                    if (this._mxUPrice >= 0)
                    {
                        SetTotalUnitPrice();
                    }
                }
            }
        }

        [Appearance("PrePurchaseOrderMxTUPriceClose", Enabled = false)]
        public double MxTUPrice
        {
            get { return _mxTUPrice; }
            set { SetPropertyValue("MxTUPrice", ref _mxTUPrice, value); }
        }

        #endregion VendorPrice

        #region DefaultQty

        [Appearance("PrePurchaseOrderDQtyClose", Criteria = "ActivationPosting = true", Enabled = false)]
        [ImmediatePostData()]
        public double DQty
        {
            get { return _dQty; }
            set { SetPropertyValue("DQty", ref _dQty, value);
                if (!IsLoading)
                {
                    SetTotalQty();
                    if (this.PostedCount > 0)
                    {
                        if (this.RmDQty == 0)
                        {
                            if (this._dQty > this.MxDQty || this._dQty <= this.MxDQty)
                            {
                                this._dQty = 0;
                                SetTotalQty();
                            }
                        }

                        if (this.RmDQty > 0)
                        {
                            if (this._dQty > this.RmDQty)
                            {
                                this._dQty = this.RmDQty;
                                SetTotalQty();
                            }
                        }
                    }
                    else
                    {
                        if (this._dQty > this.MxDQty)
                        {
                            this._dQty = this.MxDQty;
                            SetTotalQty();
                        }
                    }
                }
            }
        }

        [Appearance("PrePurchaseOrderQtyClose", Criteria = "ActivationPosting = true", Enabled = false)]
        [ImmediatePostData()]
        public double Qty
        {
            get { return _qty; }
            set { SetPropertyValue("Qty", ref _qty, value);
                if (!IsLoading)
                {
                    SetTotalQty();
                    if (this.PostedCount > 0)
                    {
                        if (this.RmQty == 0)
                        {
                            if (this._qty > this.MxQty || this._qty <= this.MxQty)
                            {
                                this._qty = 0;
                                SetTotalQty();
                            }
                        }

                        if (this.RmDQty > 0)
                        {
                            if (this._qty > this.RmDQty)
                            {
                                this._qty = this.RmDQty;
                                SetTotalQty();
                            }
                        }
                    }
                    else
                    {
                        if (this._qty > this.MxDQty)
                        {
                            this._qty = this.MxDQty;
                            SetTotalQty();
                        }
                    }
                }
            }
        }

        [Appearance("PrePurchaseOrderTotalQtyClose", Enabled = false)]
        public double TotalQty
        {
            get { return _tQty; }
            set { SetPropertyValue("TotalQty", ref _tQty, value); }
        }

        #endregion DefaultQty

        #region RemainQty

        [Appearance("PrePurchaseOrderRmDQtyClose", Enabled = false)]
        public double RmDQty
        {
            get { return _rmDQty; }
            set { SetPropertyValue("RmDQty", ref _rmDQty, value); }
        }

        [Appearance("PrePurchaseOrderRmQtyClose", Enabled = false)]
        public double RmQty
        {
            get { return _rmQty; }
            set { SetPropertyValue("RmQty", ref _rmQty, value); }
        }

        [Appearance("PrePurchaseOrderRmTQtyClose", Enabled = false)]
        public double RmTQty
        {
            get { return _rmTQty; }
            set { SetPropertyValue("RmTQty", ref _rmTQty, value); }
        }

        #endregion RemainQty

        [Appearance("PrePurchaseOrderOrderTypeClose", Criteria = "ActivationPosting = true", Enabled = false)]
        public DirectionType DirectionType
        {
            get { return _directionType; }
            set { SetPropertyValue("DirectionType", ref _directionType, value); }
        }

        [Appearance("PrePurchaseOrderPostedCountClose", Enabled = false)]
        public int PostedCount
        {
            get { return _postedCount; }
            set { SetPropertyValue("PostedCount", ref _postedCount, value); }
        }

        [Appearance("PrePurchaseOrderStatusClose", Enabled = false)]
        public Status Status
        {
            get { return _status; }
            set { SetPropertyValue("Status", ref _status, value); }
        }

        [Appearance("PrePurchaseOrderStatusDateClose", Enabled = false)]
        public DateTime StatusDate
        {
            get { return _statusDate; }
            set { SetPropertyValue("StatusDate", ref _statusDate, value); }
        }

        [Appearance("PrePurchaseOrderProjectHeaderClose", Enabled = false)]
        [Association("ProjectHeader-PrePurchaseOrders")]
        public ProjectHeader ProjectHeader
        {
            get { return _projectHeader; }
            set { SetPropertyValue("ProjectHeader", ref _projectHeader, value); }
        }

        [Association("PrePurchaseOrder-PurchaseRequisitions")]
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

        //================================================== Code Only ==================================================

        #region PriceMethod

        private void SetTotalUnitPrice()
        {
            try
            {
                if (_tQty >= 0 && _mxUPrice >= 0)
                {
                    this.MxTUPrice = this.TotalQty * this.MxUPrice;
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = PrePurchaseOrder " + ex.ToString());
            }
        }

        #endregion PriceMethod

        #region QtyMethod

        public void SetTotalQty()
        {
            try
            {
                double _locInvLineTotal = 0;
                if (this.ProjectHeader != null)
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

                            this.TotalQty = _locInvLineTotal;
                        }
                    }
                    else
                    {
                        _locInvLineTotal = this.Qty + this.DQty;
                        this.TotalQty = _locInvLineTotal;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = PrePurchaseOrder " + ex.ToString());
            }
        }

        public void SetMaxTotalQty()
        {
            try
            {
                double _locInvLineTotal = 0;
                DateTime now = DateTime.Now;
                if (this.ProjectHeader != null)
                {
                    if (this.Item != null && this.MxUOM != null && this.MxDUOM != null)
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
                Tracing.Tracer.LogError(" BusinessObject = PrePurchaseOrder " + ex.ToString());
            }
        }

        #endregion QtyMethod
    }
}