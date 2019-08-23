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
    public class ItemUnitOfMeasure : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private int _no;
        private string _code;
        private Item _item;
        private UnitOfMeasure _uom;
        private Double _conversion;
        private UnitOfMeasure _defaultUOM;
        private Double _defaultConversion;
        private bool _active;
        private GlobalFunction _globFunc;

        public ItemUnitOfMeasure(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (!IsLoading)
            {
                _globFunc = new GlobalFunction();
                this.Code = _globFunc.GetNumberingUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.ItemUnitOfMeasure);
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

        [Appearance("ItemUnitOfMeasureNoEnabled", Enabled = false)]
        public int No
        {
            get { return _no; }
            set { SetPropertyValue("No", ref _no, value); }
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        [Association("Item-ItemUnitOfMeasures")]
        [DataSourceCriteria("Active = true"), ImmediatePostData()]
        [RuleRequiredField(DefaultContexts.Save)]
        public Item Item
        {
            get { return _item; }
            set
            {
                SetPropertyValue("Item", ref _item, value);
                if (!IsLoading)
                {
                    if (_item != null)
                    {
                        if (_item.BasedUOM != null)
                        {
                            this.DefaultUOM = _item.BasedUOM;
                        }
                    }
                }
            }
        }

        [DataSourceCriteria("Active = true")]
        public UnitOfMeasure UOM
        {
            get { return _uom; }
            set { SetPropertyValue("UOM", ref _uom, value); }
        }

        public Double Conversion
        {
            get { return _conversion; }
            set { SetPropertyValue("Conversion", ref _conversion, value); }
        }

        public UnitOfMeasure DefaultUOM
        {
            get { return _defaultUOM; }
            set { SetPropertyValue("DefaultUOM", ref _defaultUOM, value); }
        }

        public Double DefaultConversion
        {
            get { return _defaultConversion; }
            set { SetPropertyValue("DefaultConversion", ref _defaultConversion, value); }
        }

        public bool Active
        {
            get { return _active; }
            set { SetPropertyValue("Active", ref _active, value); }
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
                    if (this.Item != null)
                    {
                        object _makRecord = Session.Evaluate<ItemUnitOfMeasure>(CriteriaOperator.Parse("Max(No)"), CriteriaOperator.Parse("Item=?", this.Item));
                        this.No = Convert.ToInt32(_makRecord) + 1;
                        this.Save();
                        RecoveryUpdateNo();
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = ItemUnitOfMeasure " + ex.ToString());
            }
        }

        public void RecoveryUpdateNo()
        {
            try
            {
                if (this.Item != null)
                {
                    Item _numHeader = Session.FindObject<Item>
                                                (new BinaryOperator("OID", this.Item.Oid));

                    XPCollection<ItemUnitOfMeasure> _numLines = new XPCollection<ItemUnitOfMeasure>
                                                (Session, new BinaryOperator("Item", _numHeader),
                                                new SortProperty("No", DevExpress.Xpo.DB.SortingDirection.Ascending));
                    if (_numLines != null)
                    {
                        int i1 = 0;
                        foreach (ItemUnitOfMeasure _numLine in _numLines)
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
                Tracing.Tracer.LogError(" BusinessObject = ItemUnitOfMeasure " + ex.ToString());
            }
        }

        public void RecoveryDeleteNo()
        {
            try
            {
                if (this.Item != null)
                {
                    Item _numHeader = Session.FindObject<Item>
                                                (new BinaryOperator("OID", this.Item.Oid));

                    XPCollection<ItemUnitOfMeasure> _numLines = new XPCollection<ItemUnitOfMeasure>
                                                (Session, new GroupOperator(GroupOperatorType.And,
                                                 new BinaryOperator("This", this, BinaryOperatorType.NotEqual),
                                                 new BinaryOperator("Item", _numHeader)),
                                                 new SortProperty("No", DevExpress.Xpo.DB.SortingDirection.Ascending));
                    if (_numLines != null)
                    {
                        int i = 0;
                        foreach (ItemUnitOfMeasure _numLine in _numLines)
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
                Tracing.Tracer.LogError(" BusinessObject = ItemUnitOfMeasure " + ex.ToString());
            }
        }

        #endregion No
    }
}