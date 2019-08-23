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
    [DefaultProperty("FullName")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Location : MEISysBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private string _code;
        private string _fullName;
        private string _name;
        private string _address;
        private Country _country;
        private City _city;
        private StockType _stockType;
        private bool _active;
        private GlobalFunction _globFunc;

        public Location(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (!IsLoading)
            {
                _globFunc = new GlobalFunction();
                this.Code = _globFunc.GetNumberingUnlockOptimisticRecord(this.Session.DataLayer, ObjectList.Location);
            }
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Appearance("LocationCodeEnabled", Enabled = false)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        [Appearance("LocationFullNameClose", Enabled = false)]
        public string FullName
        {
            get { return _fullName; }
            set { SetPropertyValue("FullName", ref _fullName, value); }
        }

        [ImmediatePostData()]
        public string Name
        {
            get { return _name; }
            set {
                SetPropertyValue("Name", ref _name, value);
                if (!IsLoading)
                {
                    if(this._name != null)
                    {
                        SetFullName();
                    }
                    
                }
            }
        }

        [Size(512)]
        public string Address
        {
            get { return _address; }
            set { SetPropertyValue("Address", ref _address, value); }
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

        [ImmediatePostData()]
        public StockType StockType
        {
            get { return _stockType; }
            set {
                SetPropertyValue("StockType", ref _stockType, value);
                if (!IsLoading)
                {
                    if(this._stockType != StockType.None)
                    {
                        SetFullName();
                    } 
                }
            }
        }

        public bool Active
        {
            get { return _active; }
            set { SetPropertyValue("Active", ref _active, value); }
        }

        [Association("Location-BinLocations")]
        public XPCollection<BinLocation> BinLocations
        {
            get { return GetCollection<BinLocation>("BinLocations"); }
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

        private void SetFullName()
        {
            try
            {
                if (this._name != null && this.StockType != StockType.None)
                {
                    this.FullName = String.Format("{0} ({1})", this.Name, GetStockType(this.StockType));
                }
                
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError("Business Object = Location ", ex.ToString());
            }
        }

        private string GetStockType(StockType _locStockType)
        {
            string _result = null;
            try
            {
                if(_locStockType == StockType.Good)
                {
                    _result = "Good";
                }else if(_locStockType == StockType.Bad)
                {
                    _result = "Bad";
                }
                else
                {
                    _result = "None";
                }

            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError("Business Object = Location ", ex.ToString());
            }
            return _result;
        }
    }
}