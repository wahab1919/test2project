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

namespace MEI.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Setup")]
    [DefaultProperty("Name")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class NumberingHeader : MEISysBaseObject 
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private string _code;
        private string _name;
        private NumberingType _numberingType;
        private Nullable<int> _totalLine = null;
        private ApplicationSetup _applicationSetup;
        private bool _active;

        public NumberingHeader(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }

        public NumberingType NumberingType
        {
            get { return _numberingType; }
            set { SetPropertyValue("NumberingType", ref _numberingType, value); }
        }

        [Persistent("TotalLine")]
        public Nullable<int> TotalLine
        {
            get
            {
                if (!IsLoading && !IsSaving && !_totalLine.HasValue)
                {
                    UpdateTotalLine(false);
                }
                return _totalLine;
            }
        }

        public ApplicationSetup ApplicationSetup
        {
            get { return _applicationSetup; }
            set { SetPropertyValue("ApplicationSetup", ref _applicationSetup, value);}
        }

        public bool Active
        {
            get { return _active; }
            set { SetPropertyValue("Active", ref _active, value); }
        }

        [Association("NumberingHeader-NumberingLines")]
        public XPCollection<NumberingLine> NumberingLines
        {
            get { return GetCollection<NumberingLine>("NumberingLines"); }
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

        public void UpdateTotalLine(bool forceChangeEvents)
        {
            try
            {
                Nullable<int> oldTotalLine = _totalLine;
                _totalLine = Convert.ToInt32(Session.Evaluate<NumberingHeader>(CriteriaOperator.Parse("NumberingLines.Count"), CriteriaOperator.Parse("Oid=?", Oid)));
                if (forceChangeEvents)
                {
                    OnChanged("TotalLine", oldTotalLine, _totalLine);
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError("Business Object = NumberingHeader ", ex.ToString());
            }
        }
    }
}