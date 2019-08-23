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
    [ListViewFilter("AllDataProjectLineService", "", "All Data", "All Data In Project Line Service", 1, true)]
    [ListViewFilter("OpenProjectLineService", "Status = 'Open'", "Open", "Open Data Status In Project Line Service", 2, true)]
    [ListViewFilter("ProgressProjectLineService", "Status = 'Progress'", "Progress", "Progress Data Status In Project Line Service", 3, true)]
    [ListViewFilter("LockProjectLineService", "Status = 'Lock'", "Lock", "Lock Data Status In Project Line Service", 4, true)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ProjectLineService : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private bool _activationPosting;
        private int _no;
        private string _code;
        private string _name;
        private double _qty;
        private UnitOfMeasure _uom;
        private double _unitPrice;
        private double _totalUnitPrice;
        private Status _status;
        private DateTime _statusDate;
        private ProjectLine _projectLine;
        private ProjectHeader _projectHeader;
        private ProjectLineItem _projectLineItem;
        private GlobalFunction _globFunc;

        public ProjectLineService(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (!IsLoading)
            {
                _globFunc = new GlobalFunction();
                this.Code = _globFunc.GetNumberingUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.ProjectLineService);
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

        [Appearance("ProjectLineServiceNoClose", Enabled = false)]
        public int No
        {
            get { return _no; }
            set { SetPropertyValue("No", ref _no, value); }
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Appearance("ProjectLineServiceCodeClose", Enabled = false)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        [Appearance("ProjectLineServiceNameLock", Criteria = "ActivationPosting = true", Enabled = false)]
        public string Name
        {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }

        [ImmediatePostData()]
        [Appearance("ProjectLineServiceQtyLock", Criteria = "ActivationPosting = true", Enabled = false)]
        public double Qty
        {
            get { return _qty; }
            set {
                SetPropertyValue("Qty", ref _qty, value);
                if(!IsLoading)
                {
                    SetTotalPrice();
                }
            }
        }

        [DataSourceCriteria("Active = true")]
        [ImmediatePostData()]
        [Appearance("ProjectLineServiceUOMLock", Criteria = "ActivationPosting = true", Enabled = false)]
        public UnitOfMeasure UOM
        {
            get { return _uom; }
            set {
                SetPropertyValue("UOM", ref _uom, value);
                if (!IsLoading)
                {
                    SetTotalPrice();
                }
            }
        }

        [ImmediatePostData()]
        [Appearance("ProjectLineServiceUnitPriceLock", Criteria = "ActivationPosting = true", Enabled = false)]
        public double UnitPrice
        {
            get { return _unitPrice; }
            set {
                SetPropertyValue("Price", ref _unitPrice, value);
                if (!IsLoading)
                {
                    SetTotalPrice();
                }
            }
        }

        [Appearance("ProjectLineServiceTotalUnitPriceClose", Enabled = false)]
        public double TotalUnitPrice
        {
            get { return _totalUnitPrice; }
            set { SetPropertyValue("TotalPrice", ref _totalUnitPrice, value); }
        }

        [Appearance("ProjectLineServiceStatusClose", Enabled = false)]
        public Status Status
        {
            get { return _status; }
            set { SetPropertyValue("Status", ref _status, value); }
        }

        [Appearance("ProjectLineServiceStatusDateClose", Enabled = false)]
        public DateTime StatusDate
        {
            get { return _statusDate; }
            set { SetPropertyValue("StatusDate", ref _statusDate, value); }
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Appearance("ProjectLineServiceProjectLineClose", Enabled = false)]
        [Association("ProjectLine-ProjectLineServices")]
        public ProjectLine ProjectLine
        {
            get { return _projectLine; }
            set {
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

        [Browsable(false)]
        [ImmediatePostData()]
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
                                this.Name = "Services of " + this._projectLineItem.Item.Name;
                            }
                        }
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
                    if (this.ProjectLine != null)
                    {
                        object _makRecord = Session.Evaluate<ProjectLineService>(CriteriaOperator.Parse("Max(No)"), CriteriaOperator.Parse("ProjectLine=?", this.ProjectLine));
                        this.No = Convert.ToInt32(_makRecord) + 1;
                        this.Save();
                        RecoveryUpdateNo();
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = ProjectLineService " + ex.ToString());
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

                    XPCollection<ProjectLineService> _numLines = new XPCollection<ProjectLineService>
                                                (Session, new BinaryOperator("ProjectLine", _numHeader),
                                                new SortProperty("No", DevExpress.Xpo.DB.SortingDirection.Ascending));
                    if (_numLines != null)
                    {
                        int i1 = 0;
                        foreach (ProjectLineService _numLine in _numLines)
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
                Tracing.Tracer.LogError(" BusinessObject = ProjectLineService " + ex.ToString());
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

                    XPCollection<ProjectLineService> _numLines = new XPCollection<ProjectLineService>
                                                (Session, new GroupOperator(GroupOperatorType.And,
                                                 new BinaryOperator("This", this, BinaryOperatorType.NotEqual),
                                                 new BinaryOperator("ProjectLine", _numHeader)),
                                                 new SortProperty("No", DevExpress.Xpo.DB.SortingDirection.Ascending));
                    if (_numLines != null)
                    {
                        int i = 0;
                        foreach (ProjectLineService _numLine in _numLines)
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
                Tracing.Tracer.LogError(" BusinessObject = ProjectLineService " + ex.ToString());
            }
        }

        #endregion No

        #region PriceMethod

        private void SetTotalPrice()
        {
            try
            {
                if (_qty >= 0 && _unitPrice >= 0)
                {
                    this.TotalUnitPrice = this.Qty * this.UnitPrice;
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = ProjectLineItem " + ex.ToString());
            }
        }

        #endregion PriceMethod
    }
}