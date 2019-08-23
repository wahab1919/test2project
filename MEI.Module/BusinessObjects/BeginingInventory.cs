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
    public class BeginingInventory : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private string _code;
        private Item _item;
        private Country _country;
        private City _city;
        private double _qtyBegining;
        private double _qtyMinimal;
        private double _qtyAvailable;
        private double _qtySale;
        private UnitOfMeasure _defaultUOM;
        private bool _active;
        private GlobalFunction _globFunc;

        public BeginingInventory(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (!IsLoading)
            {
                _globFunc = new GlobalFunction();
                this.Code = _globFunc.GetNumberingUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.BeginingInventory);
            }
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Appearance("BeginingInventoryCodeEnabled", Enabled = false)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        public Item Item
        {
            get { return _item; }
            set { SetPropertyValue("Item", ref _item, value); }
        }

        public Country Country
        {
            get { return _country; }
            set { SetPropertyValue("Country", ref _country, value); }
        }

        public City City
        {
            get { return _city; }
            set { SetPropertyValue("City", ref _city, value); }
        }

        public double QtyBegining
        {
            get { return _qtyBegining; }
            set { SetPropertyValue("QtyBegining", ref _qtyBegining, value); }
        }

        public double QtyMinimal
        {
            get { return _qtyMinimal; }
            set { SetPropertyValue("QtyMinimal", ref _qtyMinimal, value); }
        }

        public double QtyAvailable
        {
            get { return _qtyAvailable; }
            set { SetPropertyValue("QtyAvailable", ref _qtyAvailable, value); }
        }

        public double QtySale
        {
            get { return _qtySale; }
            set { SetPropertyValue("QtySale", ref _qtySale, value); }
        }

        public UnitOfMeasure DefaultUOM
        {
            get { return _defaultUOM; }
            set { SetPropertyValue("DefaultUOM", ref _defaultUOM, value); }
        }

        public bool Active
        {
            get { return _active; }
            set { SetPropertyValue("Active", ref _active, value); }
        }

        [Association("BeginingInventory-BeginingInventoryLines")]
        public XPCollection<BeginingInventoryLine> BeginingInventoryLines
        {
            get { return GetCollection<BeginingInventoryLine>("BeginingInventoryLines"); }
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