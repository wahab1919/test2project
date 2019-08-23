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
    [NavigationItem("Setup")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class NumberingLine : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private int _no;
        private string _name;
        private ObjectList _objectList;
        private string _prefix;
        private string _formatNumber;
        private string _suffix;
        private int _lastValue;
        private int _incrementValue;
        private int _minValue;
        private string _formatedValue;
        private DocumentType _documentType;
        private bool _active;
        private bool _default;
        private bool _selection;
        private bool _sign;
        private NumberingHeader _numberingHeader;

        public NumberingLine(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if(!IsLoading)
            {
                if(this.Default == true)
                {
                    CheckDefaults();
                }
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

        [Appearance("NumberingLineNoEnabled", Enabled = false)]
        public int No
        {
            get { return _no; }
            set { SetPropertyValue("No", ref _no, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }

        public ObjectList ObjectList
        {
            get { return _objectList; }
            set { SetPropertyValue("ObjectList", ref _objectList, value); }
        }

        public string Prefix
        {
            get { return _prefix; }
            set { SetPropertyValue("Prefix", ref _prefix, value); }
        }

        public string FormatNumber
        {
            get { return _formatNumber; }
            set { SetPropertyValue("FormatNumber", ref _formatNumber, value); }
        }

        public string Suffix
        {
            get { return _suffix; }
            set { SetPropertyValue("Suffix", ref _suffix, value); }
        }

        public int LastValue
        {
            get { return _lastValue; }
            set { SetPropertyValue("LastValue", ref _lastValue, value); }
        }

        public int IncrementValue
        {
            get { return _incrementValue; }
            set { SetPropertyValue("IncrementValue", ref _incrementValue, value); }
        }

        public int MinValue
        {
            get { return _minValue; }
            set { SetPropertyValue("MinValue", ref _minValue, value); }
        }

        public string FormatedValue
        {
            get
            {
                string strFormat;
                if (this.FormatNumber == null)
                {
                    strFormat = "{0:0}";
                }
                else
                {
                    strFormat = String.Format("{{0:{0}}}", this.FormatNumber);
                }
                _formatedValue = this.Prefix + string.Format(strFormat, this.LastValue) + this.Suffix;
                return _formatedValue;
            }
        }

        public DocumentType DocumentType
        {
            get { return _documentType; }
            set { SetPropertyValue("DocumentType", ref _documentType, value); }
        }

        public bool Active
        {
            get { return _active; }
            set { SetPropertyValue("Active", ref _active, value); }
        }

        public bool Default
        {
            get { return _default; }
            set { SetPropertyValue("Default", ref _default, value); }
        }

        public bool Selection
        {
            get { return _selection; }
            set { SetPropertyValue("Selection", ref _selection, value); }
        }

        public bool Sign
        {
            get { return _sign; }
            set { SetPropertyValue("Sign", ref _sign, value); }
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Association("NumberingHeader-NumberingLines")]
        public NumberingHeader NumberingHeader
        {
            get { return _numberingHeader; }
            set
            {
                NumberingHeader oldNumberingHeader = _numberingHeader;
                SetPropertyValue("NumberingHeader", ref _numberingHeader, value);
                if (!IsLoading && !IsSaving && !object.ReferenceEquals(oldNumberingHeader, _numberingHeader))
                {
                    oldNumberingHeader = oldNumberingHeader ?? _numberingHeader;
                    oldNumberingHeader.UpdateTotalLine(true);
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

        //=============================================== Code In Here ===============================================

        #region No

        public void UpdateNo()
        {
            try
            {
                if (!IsLoading && Session.IsNewObject(this))
                {
                    if (this.NumberingHeader != null)
                    {
                        object _makRecord = Session.Evaluate<NumberingLine>(CriteriaOperator.Parse("Max(No)"), CriteriaOperator.Parse("NumberingHeader=?", this.NumberingHeader));
                        this.No = Convert.ToInt32(_makRecord) + 1;
                        this.Save();
                        RecoveryUpdateNo();
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = NumberingLine " + ex.ToString());
            }
        }

        public void RecoveryUpdateNo()
        {
            try
            {
                if (this.NumberingHeader != null)
                {
                    NumberingHeader _numHeader = Session.FindObject<NumberingHeader>
                                                (new BinaryOperator("Code", this.NumberingHeader.Code));

                    XPCollection<NumberingLine> _numLines = new XPCollection<NumberingLine>
                                                (Session, new BinaryOperator("NumberingHeader", _numHeader),
                                                new SortProperty("No", DevExpress.Xpo.DB.SortingDirection.Ascending));
                    if (_numLines != null)
                    {
                        int i1 = 0;
                        foreach (NumberingLine _numLine in _numLines)
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
                if (this.NumberingHeader != null)
                {
                    NumberingHeader _numHeader = Session.FindObject<NumberingHeader>
                                                (new BinaryOperator("Code", this.NumberingHeader.Code));

                    XPCollection<NumberingLine> _numLines = new XPCollection<NumberingLine>
                                                (Session, new GroupOperator(GroupOperatorType.And,
                                                 new BinaryOperator("This", this, BinaryOperatorType.NotEqual),
                                                 new BinaryOperator("NumberingHeader", _numHeader)),
                                                 new SortProperty("No", DevExpress.Xpo.DB.SortingDirection.Ascending));
                    if (_numLines != null)
                    {
                        int i = 0;
                        foreach (NumberingLine _numLine in _numLines)
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

        private void CheckDefaults()
        {
            try
            {
                XPCollection<NumberingLine> _numSetups = new XPCollection<NumberingLine>(Session,
                                                            new BinaryOperator("This", this, BinaryOperatorType.NotEqual));
                if (_numSetups == null)
                {
                    return;
                }
                else
                {
                    foreach (NumberingLine _numSetup in _numSetups)
                    {
                        if (_numSetup.ObjectList == this.ObjectList)
                        {
                            _numSetup.Default = false;
                            _numSetup.Save();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError("Business Object = NumberingLine ", ex.ToString());
            }
        }
    }
}