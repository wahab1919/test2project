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
    [NavigationItem("Inventory")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class InventoryJournal : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private string _code;
        private DocumentType _documentType;
        private string _docNo;
        private Location _location;
        private BinLocation _binLocation;
        private Item _item;
        private double _qtyPos;
        private double _qtyNeg;
        private DateTime _journalDate;
        private ProjectHeader _projectHeader;
        private GlobalFunction _globFunc;

        public InventoryJournal(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (!IsLoading)
            {
                _globFunc = new GlobalFunction();
                this.Code = _globFunc.GetNumberingUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.InventoryJournal);
            }
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Appearance("InventoryJournalCodeEnabled", Enabled = false)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        public DocumentType DocumentType
        {
            get { return _documentType; }
            set { SetPropertyValue("DocumentType", ref _documentType, value); }
        }

        public string DocNo
        {
            get { return _docNo; }
            set { SetPropertyValue("DocNo", ref _docNo, value); }
        }

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

        public Item Item
        {
            get { return _item; }
            set { SetPropertyValue("Item", ref _item, value); }
        }

        public double QtyPos
        {
            get { return _qtyPos; }
            set { SetPropertyValue("QtyPos", ref _qtyPos, value); }
        }

        public double QtyNeg
        {
            get { return _qtyNeg; }
            set { SetPropertyValue("QtyNeg", ref _qtyNeg, value); }
        }
        
        public DateTime JournalDate
        {
            get { return _journalDate; }
            set { SetPropertyValue("JournalDate", ref _journalDate, value); }
        }

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
    }
}