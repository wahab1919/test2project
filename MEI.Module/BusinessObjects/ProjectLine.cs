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
    [DefaultProperty("Title")]
    [ListViewFilter("AllDataProjectLine", "", "All Data", "All Data In Project Line", 1, true)]
    [ListViewFilter("OpenProjectLine", "Status = 'Open'", "Open", "Open Data Status In Project Line", 2, true)]
    [ListViewFilter("ProgressProjectLine", "Status = 'Progress'", "Progress", "Progress Data Status In Project Line", 3, true)]
    [ListViewFilter("LockProjectLine", "Status = 'Lock'", "Lock", "Lock Data Status In Project Line", 4, true)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ProjectLine : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private bool _activationPosting;
        private int _no;
        private string _code;
        private string _title;
        private string _title2;
        private string _title3;
        private Status _status;
        private DateTime _statusDate;
        private string _description;
        private ProjectHeader _projectHeader;
        private GlobalFunction _globFunc;

        public ProjectLine(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (!IsLoading)
            {
                _globFunc = new GlobalFunction();
                this.Code = _globFunc.GetNumberingUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.ProjectLine);
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
            RecoveryDeleteNo();
        }

        [Browsable(false)]
        public bool ActivationPosting
        {
            get { return _activationPosting; }
            set { SetPropertyValue("ActivationPosting", ref _activationPosting, value); }
        }

        [Appearance("ProjectLineNoClose", Enabled = false)]
        public int No
        {
            get { return _no; }
            set { SetPropertyValue("No", ref _no, value); }
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Appearance("ProjectLineCodeClose", Enabled = false)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        [Appearance("ProjectLineTitleLock", Criteria = "ActivationPosting = true", Enabled = false)]
        public string Title
        {
            get { return _title; }
            set { SetPropertyValue("Title", ref _title, value); }
        }

        [Appearance("ProjectLineTitle2Lock", Criteria = "ActivationPosting = true", Enabled = false)]
        public string Title2
        {
            get { return _title2; }
            set { SetPropertyValue("Title2", ref _title2, value); }
        }

        [Appearance("ProjectLineTitle3Lock", Criteria = "ActivationPosting = true", Enabled = false)]
        public string Title3
        {
            get { return _title3; }
            set { SetPropertyValue("Title3", ref _title3, value); }
        }

        [Appearance("ProjectLineStatusClose", Enabled = false)]
        public Status Status
        {
            get { return _status; }
            set { SetPropertyValue("Status", ref _status, value); }
        }

        [Appearance("ProjectLineStatusDateClose", Enabled = false)]
        public DateTime StatusDate
        {
            get { return _statusDate; }
            set { SetPropertyValue("StatusDate", ref _statusDate, value); }
        }

        [Appearance("ProjectLineDescriptionLock", Criteria = "ActivationPosting = true", Enabled = false)]
        [Size(512)]
        public string Description
        {
            get { return _description; }
            set { SetPropertyValue("Description", ref _description, value); }
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Appearance("ProjectLineProjectHeaderClose", Enabled = false)]
        [Association("ProjectHeader-ProjectLines")]
        public ProjectHeader ProjectHeader
        {
            get { return _projectHeader; }
            set { SetPropertyValue("ProjectHeader", ref _projectHeader, value); }
        }


        [Association("ProjectLine-ProjectLineItems")]
        public XPCollection<ProjectLineItem> ProjectLineItems
        {
            get { return GetCollection<ProjectLineItem>("ProjectLineItems"); }
        }

        [Association("ProjectLine-ProjectLineItem2s")]
        public XPCollection<ProjectLineItem2> ProjectLineItem2s
        {
            get { return GetCollection<ProjectLineItem2>("ProjectLineItem2s"); }
        }

        [Association("ProjectLine-ProjectLineServices")]
        public XPCollection<ProjectLineService> ProjectLineServices
        {
            get { return GetCollection<ProjectLineService>("ProjectLineServices"); }
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

        #region No

        public void UpdateNo()
        {
            try
            {
                if (!IsLoading && Session.IsNewObject(this))
                {
                    if (this.ProjectHeader != null)
                    {
                        object _makRecord = Session.Evaluate<ProjectLine>(CriteriaOperator.Parse("Max(No)"), CriteriaOperator.Parse("ProjectHeader=?", this.ProjectHeader));
                        this.No = Convert.ToInt32(_makRecord) + 1;
                        this.Save();
                        RecoveryUpdateNo();
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = ProjectHeader " + ex.ToString());
            }
        }

        public void RecoveryUpdateNo()
        {
            try
            {
                if (this.ProjectHeader != null)
                {
                    ProjectHeader _numHeader = Session.FindObject<ProjectHeader>
                                                (new BinaryOperator("Code", this.ProjectHeader.Code));

                    XPCollection<ProjectLine> _numLines = new XPCollection<ProjectLine>
                                                (Session, new BinaryOperator("ProjectHeader", _numHeader),
                                                new SortProperty("No", DevExpress.Xpo.DB.SortingDirection.Ascending));
                    if (_numLines != null)
                    {
                        int i1 = 0;
                        foreach (ProjectLine _numLine in _numLines)
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
                Tracing.Tracer.LogError(" BusinessObject = ProjectLine " + ex.ToString());
            }
        }

        public void RecoveryDeleteNo()
        {
            try
            {
                if (this.ProjectHeader != null)
                {
                    ProjectHeader _numHeader = Session.FindObject<ProjectHeader>
                                                (new BinaryOperator("Code", this.ProjectHeader.Code));

                    XPCollection<ProjectLine> _numLines = new XPCollection<ProjectLine>
                                                (Session, new GroupOperator(GroupOperatorType.And,
                                                 new BinaryOperator("This", this, BinaryOperatorType.NotEqual),
                                                 new BinaryOperator("ProjectHeader", _numHeader)),
                                                 new SortProperty("No", DevExpress.Xpo.DB.SortingDirection.Ascending));
                    if (_numLines != null)
                    {
                        int i = 0;
                        foreach (ProjectLine _numLine in _numLines)
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
                Tracing.Tracer.LogError(" BusinessObject = ProjectLine " + ex.ToString());
            }
        }

        #endregion No
    }
}