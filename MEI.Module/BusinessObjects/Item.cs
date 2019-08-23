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
    [NavigationItem("Inventory")]
    [DefaultProperty("Name")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Item : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private string _code;
        private ObjectList _objectList;
        XPCollection<NumberingLine> _availableNumberingLine;
        private NumberingLine _numbering;
        private string _name;
        private string _shortName;
        private UnitOfMeasure _basedUOM;
        private DateTime _startDate;
        private DateTime _endDate;
        private ItemType _itemType;
        private string _description;
        private bool _active;
        private GlobalFunction _globFunc;

        public Item(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (!IsLoading)
            {
                _globFunc = new GlobalFunction();
                this.Code = _globFunc.GetNumberingUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.Item);
                this.ObjectList = CustomProcess.ObjectList.Item;
            }
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        [Browsable(false)]
        public ObjectList ObjectList
        {
            get { return _objectList; }
            set { SetPropertyValue("ObjectList", ref _objectList, value); }
        }

        [Browsable(false)]
        public XPCollection<NumberingLine> AvailableNumberingLine
        {
            get
            {
                if (ObjectList == ObjectList.None)
                {
                    _availableNumberingLine = new XPCollection<NumberingLine>(Session);
                }
                else
                {
                    _availableNumberingLine = new XPCollection<NumberingLine>(Session,
                                              new GroupOperator(GroupOperatorType.And,
                                              new BinaryOperator("ObjectList", this.ObjectList),
                                              new BinaryOperator("Selection", true),
                                              new BinaryOperator("Active", true)));
                }
                return _availableNumberingLine;

            }
        }

        [NonPersistent()]
        [ImmediatePostData()]
        [DataSourceProperty("AvailableNumberingLine", DataSourcePropertyIsNullMode.SelectAll)]
        public NumberingLine Numbering
        {
            get { return _numbering; }
            set
            {
                NumberingLine _oldNumbering = _numbering;
                SetPropertyValue("Numbering", ref _numbering, value);
                if (!IsLoading && _numbering != null && _numbering != _oldNumbering)
                {
                    this.Code = _globFunc.GetNumberingSelectionUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.Item, _numbering);
                }
            }
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public string Name
        {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }

        public string ShortName
        {
            get { return _shortName; }
            set { SetPropertyValue("ShortName", ref _shortName, value); }
        }

        [DataSourceCriteria("Active = true")]
        public UnitOfMeasure BasedUOM
        {
            get { return _basedUOM; }
            set { SetPropertyValue("BasedUOM", ref _basedUOM, value); }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetPropertyValue("StartDate", ref _startDate, value); }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { SetPropertyValue("EndDate", ref _endDate, value); }
        }

        [DataSourceCriteria("Active = true")]   
        public ItemType ItemType
        {
            get { return _itemType; }
            set { SetPropertyValue("ItemType", ref _itemType, value); }
        }

        [Size(512)]
        public string Description
        {
            get { return _description; }
            set { SetPropertyValue("Description", ref _description, value); }
        }

        public bool Active
        {
            get { return _active; }
            set { SetPropertyValue("Active", ref _active, value); }
        }

        [Association("Item-ItemUnitOfMeasures")]
        public XPCollection<ItemUnitOfMeasure> ItemUnitOfMeasures
        {
            get { return GetCollection<ItemUnitOfMeasure>("ItemUnitOfMeasures"); }
        }

        [Association("Item-ItemComponents")]
        public XPCollection<ItemComponent> ItemComponents
        {
            get { return GetCollection<ItemComponent>("ItemComponents"); }
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