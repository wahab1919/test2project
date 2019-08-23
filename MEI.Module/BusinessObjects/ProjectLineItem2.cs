﻿using System;
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
    [ListViewFilter("AllDataProjectLineItem2", "", "All Data", "All Data In Project Line Item2", 1, true)]
    [ListViewFilter("OpenProjectLineItem2", "Status = 'Open'", "Open", "Open Data Status In Project Line Item2", 2, true)]
    [ListViewFilter("ProgressProjectLineItem2", "Status = 'Progress'", "Progress", "Progress Data Status In Project Line Item2", 3, true)]
    [ListViewFilter("LockProjectLineItem2", "Status = 'Lock'", "Lock", "Lock Data Status In Project Line Item2", 4, true)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ProjectLineItem2 : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private bool _activationPosting;
        private int _no;
        private string _code;
        private string _name;
        private Item _item;
        private double _dQty;
        private UnitOfMeasure _dUom;
        private double _qty;
        private XPCollection<UnitOfMeasure> _availableUnitOfMeasure;
        private UnitOfMeasure _uom;
        private Brand _brand;
        private double _tQty;
        private double _unitPrice;
        private double _totalUnitPrice;
        private string _description;
        private Status _status;
        private DateTime _statusDate;
        private ProjectLineItem _projectLineItem;
        private ProjectLine _projectLine;
        private ProjectHeader _projectHeader;
        private GlobalFunction _globFunc;


        public ProjectLineItem2(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (!IsLoading)
            {

                _globFunc = new GlobalFunction();
                this.Code = _globFunc.GetNumberingUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.ProjectLineItem2);
                DateTime now = DateTime.Now;
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
            RecoveryDeleteNo();
        }

        [Browsable(false)]
        public bool ActivationPosting
        {
            get { return _activationPosting; }
            set { SetPropertyValue("ActivationPosting", ref _activationPosting, value); }
        }

        [Appearance("ProjectLineItem2NoClose", Enabled = false)]
        public int No
        {
            get { return _no; }
            set { SetPropertyValue("No", ref _no, value); }
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Appearance("ProjectLineItem2CodeClose", Enabled = false)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        [Appearance("ProjectLineItem2NameLock", Criteria = "ActivationPosting = true", Enabled = false)]
        public string Name
        {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }

        [DataSourceCriteria("Active = true")]
        [Appearance("ProjectLineItem2ItemLock", Criteria = "ActivationPosting = true", Enabled = false)]
        public Item Item
        {
            get { return _item; }
            set { SetPropertyValue("Item", ref _item, value); }
        }

        #region DefaultQty

        [Appearance("ProjectLineItem2DQtyLock", Criteria = "ActivationPosting = true", Enabled = false)]
        [ImmediatePostData()]
        public double DQty
        {
            get { return _dQty; }
            set {
                SetPropertyValue("DQty", ref _dQty, value);
                if (!IsLoading)
                {
                    SetTotalQty();
                    SetTotalPrice();
                }
            }
        }

        [DataSourceCriteria("Active = true")]
        [Appearance("ProjectLineItem2DUOMLock", Criteria = "ActivationPosting = true", Enabled = false)]
        [ImmediatePostData()]
        public UnitOfMeasure DUOM
        {
            get { return _dUom; }
            set {
                SetPropertyValue("DUOM", ref _dUom, value);
                if (!IsLoading)
                {
                    SetTotalQty();
                    SetTotalPrice();
                }
            }
        }

        [Appearance("ProjectLineItem2QtyLock", Criteria = "ActivationPosting = true", Enabled = false)]
        [ImmediatePostData()]
        public double Qty
        {
            get { return _qty; }
            set {
                SetPropertyValue("Qty", ref _qty, value);
                if (!IsLoading)
                {
                    SetTotalQty();
                    SetTotalPrice();
                }
            }
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

        [Appearance("ProjectLineItem2UOMLock", Criteria = "ActivationPosting = true", Enabled = false)]
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
                    SetTotalPrice();
                }
            }
        }

        [Appearance("ProjectLineItem2TotalQtyClose", Enabled = false)]
        public double TQty
        {
            get { return _tQty; }
            set { SetPropertyValue("TQty", ref _tQty, value); }
        }

        #endregion DefaultQty

        #region VendorPrice

        [DataSourceCriteria("Active = true")]
        [Appearance("ProjectLineItem2BrandLock", Criteria = "ActivationPosting = true", Enabled = false)]
        public Brand Brand
        {
            get { return _brand; }
            set { SetPropertyValue("Brand", ref _brand, value); }
        }

        [Appearance("ProjectLineItem2UnitPriceLock", Criteria = "ActivationPosting = true", Enabled = false)]
        [ImmediatePostData()]
        public double UnitPrice
        {
            get { return _unitPrice; }
            set
            {
                SetPropertyValue("UnitPrice", ref _unitPrice, value);
                if (!IsLoading)
                {
                    SetTotalPrice();
                }
            }
        }

        [Appearance("ProjectLineItem2TotalUnitPriceClose", Enabled = false)]
        public double TotalUnitPrice
        {
            get { return _totalUnitPrice; }
            set { SetPropertyValue("TotalUnitPrice", ref _totalUnitPrice, value); }
        }

        #endregion CustomerPrice

        [Appearance("ProjectLineItem2DescriptionLock", Criteria = "ActivationPosting = true", Enabled = false)]
        [Size(512)]
        public string Description
        {
            get { return _description; }
            set { SetPropertyValue("Description", ref _description, value); }
        }

        [Appearance("ProjectLineItem2StatusClose", Enabled = false)]
        public Status Status
        {
            get { return _status; }
            set { SetPropertyValue("Status", ref _status, value); }
        }

        [Appearance("ProjectLineItem2StatusDateClose", Enabled = false)]
        public DateTime StatusDate
        {
            get { return _statusDate; }
            set { SetPropertyValue("StatusDate", ref _statusDate, value); }
        }

        [Browsable(false)]
        public ProjectLineItem ProjectLineItem
        {
            get { return _projectLineItem; }
            set {
                SetPropertyValue("ProjectLineItem", ref _projectLineItem, value);
                if(!IsLoading)
                {
                    if(this._projectLineItem != null)
                    {
                        if(this._projectLineItem.Item != null)
                        {
                            if(this._projectLineItem.Item.Name != null)
                            {
                                this.Name = (_projectLineItem.Code) +" For " + this._projectLineItem.Item.Name;
                            }
                        }
                    }
                }
            }
        }

        [Appearance("ProjectLineItem2ProjectLineClose", Enabled = false)]
        [RuleRequiredField(DefaultContexts.Save)]
        [ImmediatePostData()]
        [Association("ProjectLine-ProjectLineItem2s")]
        public ProjectLine ProjectLine
        {
            get { return _projectLine; }
            set
            {
                SetPropertyValue("ProjectLine", ref _projectLine, value);
                if (!IsLoading)
                {
                    if (this._projectLine != null)
                    {
                        if (this._projectLine.ProjectHeader != null)
                        {
                            this.ProjectHeader = this._projectLine.ProjectHeader;
                        }
                    }
                }
            }
        }

        [Browsable(false)]
        public ProjectHeader ProjectHeader
        {
            get { return _projectHeader; }
            set { SetPropertyValue("ProjectHeader", ref _projectHeader, value); }
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
                    if (this.ProjectLine != null)
                    {
                        object _makRecord = Session.Evaluate<ProjectLineItem2>(CriteriaOperator.Parse("Max(No)"), CriteriaOperator.Parse("ProjectLine=?", this.ProjectLine));
                        this.No = Convert.ToInt32(_makRecord) + 1;
                        this.Save();
                        //this.Session.CommitTransaction();
                        RecoveryUpdateNo();
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = ProjectLineItem2 " + ex.ToString());
            }
        }

        public void RecoveryUpdateNo()
        {
            try
            {
                if (this.ProjectLine != null)
                {
                    ProjectLine _numHeader = Session.FindObject<ProjectLine>
                                                (new BinaryOperator("Code", this.ProjectLine.Code));

                    XPCollection<ProjectLineItem2> _numLines = new XPCollection<ProjectLineItem2>
                                                (Session, new BinaryOperator("ProjectLine", _numHeader),
                                                new SortProperty("No", DevExpress.Xpo.DB.SortingDirection.Ascending));
                    if (_numLines != null)
                    {
                        int i1 = 0;
                        foreach (ProjectLineItem2 _numLine in _numLines)
                        {
                            i1 += 1;
                            _numLine.No = i1;
                            _numLine.Save();
                            //_numLine.Session.CommitTransaction();
                        }
                        i1 = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = ProjectLineItem2 " + ex.ToString());
            }
        }

        public void RecoveryDeleteNo()
        {
            try
            {
                if (this.ProjectLine != null)
                {
                    ProjectLine _numHeader = Session.FindObject<ProjectLine>
                                                (new BinaryOperator("Code", this.ProjectLine.Code));

                    XPCollection<ProjectLineItem2> _numLines = new XPCollection<ProjectLineItem2>
                                                (Session, new GroupOperator(GroupOperatorType.And,
                                                 new BinaryOperator("This", this, BinaryOperatorType.NotEqual),
                                                 new BinaryOperator("ProjectLine", _numHeader)),
                                                 new SortProperty("No", DevExpress.Xpo.DB.SortingDirection.Ascending));
                    if (_numLines != null)
                    {
                        int i = 0;
                        foreach (ProjectLineItem2 _numLine in _numLines)
                        {
                            i += 1;
                            _numLine.No = i;
                            _numLine.Save();
                            //_numLine.Session.CommitTransaction();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = ProjectLineItem2 " + ex.ToString());
            }
        }

        #endregion No

        #region PriceMethod

        private void SetTotalPrice()
        {
            try
            {
                if (_tQty >= 0 && _unitPrice >= 0)
                {
                    this.TotalUnitPrice = this.TQty * this.UnitPrice;
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = ProjectLineItem2 " + ex.ToString());
            }
        }

        #endregion PriceMethod

        #region QtyMethod

        public void SetTotalQty()
        {
            try
            {
                double _locInvLineTotal = 0;
                DateTime now = DateTime.Now;
                if (this.ProjectLine != null)
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
                Tracing.Tracer.LogError(" BusinessObject = ProjectLineItem2 " + ex.ToString());
            }
        }

        #endregion QtyMethod
    }
}