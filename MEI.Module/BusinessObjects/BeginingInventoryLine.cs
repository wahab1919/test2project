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
    public class BeginingInventoryLine : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private string _code;
        private Item _item;
        private Location _location;
        private BinLocation _binLocation;
        private double _qtyBegining;
        private double _qtyMinimal;
        private double _qtyAvailable;
        private double _qtySale;
        private UnitOfMeasure _defaultUom;
        private StockType _stockType;
        private bool _active;
        private ProjectHeader _projectHeader;
        private BeginingInventory _beginingInventory;
        private GlobalFunction _globFunc;

        public BeginingInventoryLine(Session session)
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

        public Item Item
        {
            get { return _item; }
            set { SetPropertyValue("Item", ref _item, value); }
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
            get { return _defaultUom; }
            set { SetPropertyValue("DefaultUOM", ref _defaultUom, value); }
        }

        public StockType StockType
        {
            get { return _stockType; }
            set { SetPropertyValue("StockType", ref _stockType, value); }
        }

        public bool Active
        {
            get { return _active; }
            set { SetPropertyValue("Active", ref _active, value); }
        }

        public ProjectHeader ProjectHeader
        {
            get { return _projectHeader; }
            set { SetPropertyValue("ProjectHeader", ref _projectHeader, value); }
        }

        [ImmediatePostData()]
        [Association("BeginingInventory-BeginingInventoryLines")]
        public BeginingInventory BeginingInventory
        {
            get { return _beginingInventory; }
            set {
                SetPropertyValue("BeginingInventory", ref _beginingInventory, value);
                if(!IsLoading)
                {
                    if(this._beginingInventory != null)
                    {
                        this.DefaultUOM = _beginingInventory.DefaultUOM;
                        this.Item = _beginingInventory.Item;
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
    }
}