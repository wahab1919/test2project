using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using MEI.Module.BusinessObjects;
using MEI.Module.CustomProcess;
using System.Collections;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;

namespace MEI.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class InventoryTransferActionController : ViewController
    {

        public InventoryTransferActionController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void InventoryTransferProgressAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                GlobalFunction _globFunc = new GlobalFunction();
                IObjectSpace _objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
                ArrayList _objectsToProcess = new ArrayList(e.SelectedObjects);
                DateTime now = DateTime.Now;
                Session _currSession = null;
                string _currObjectId = null;


                if (this.ObjectSpace != null)
                {
                    _currSession = ((XPObjectSpace)this.ObjectSpace).Session;
                }

                if (_objectsToProcess != null)
                {
                    foreach (Object obj in _objectsToProcess)
                    {
                        InventoryTransfer _locInventoryTransferOS = (InventoryTransfer)_objectSpace.GetObject(obj);

                        if (_locInventoryTransferOS != null)
                        {
                            if (_locInventoryTransferOS.Code != null)
                            {
                                _currObjectId = _locInventoryTransferOS.Code;

                                InventoryTransfer _locInventoryTransferXPO = _currSession.FindObject<InventoryTransfer>
                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                     new BinaryOperator("Code", _currObjectId)));

                                if (_locInventoryTransferXPO != null)
                                {
                                    if(_locInventoryTransferXPO.Status == Status.Open)
                                    {
                                        _locInventoryTransferXPO.Status = Status.Progress;
                                        _locInventoryTransferXPO.StatusDate = now;
                                        _locInventoryTransferXPO.Save();
                                        _locInventoryTransferXPO.Session.CommitTransaction();

                                        XPCollection<InventoryTransferLine> _locInventoryTransferLines = new XPCollection<InventoryTransferLine>
                                                               (_currSession, new GroupOperator(GroupOperatorType.And,
                                                                new BinaryOperator("InventoryTransfer", _locInventoryTransferXPO)));

                                        if (_locInventoryTransferLines != null && _locInventoryTransferLines.Count > 0)
                                        {
                                            foreach (InventoryTransferLine _locInventoryTransferLine in _locInventoryTransferLines)
                                            {
                                                if(_locInventoryTransferLine.Status == Status.Open)
                                                {
                                                    _locInventoryTransferLine.Status = Status.Progress;
                                                    _locInventoryTransferLine.StatusDate = now;
                                                    _locInventoryTransferLine.Save();
                                                    _locInventoryTransferLine.Session.CommitTransaction();
                                                }
                                            }
                                        }
                                        SuccessMessageShow(_locInventoryTransferXPO.Code + " has been change successfully to Progress");
                                    }
                                }
                                else
                                {
                                    ErrorMessageShow("Data Inventory Transfer Not Available");
                                }

                            }
                            else
                            {
                                ErrorMessageShow("Data Inventory Transfer Not Available");
                            }
                        }
                    }
                }

                if (View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    _objectSpace.CommitChanges();
                    _objectSpace.Refresh();
                }
                if (View is ListView)
                {
                    _objectSpace.CommitChanges();
                    View.ObjectSpace.Refresh();
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = InventoryTransfer " + ex.ToString());
            }
        }

        private void InventoryTransferReceiveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                GlobalFunction _globFunc = new GlobalFunction();
                IObjectSpace _objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
                ArrayList _objectsToProcess = new ArrayList(e.SelectedObjects);
                DateTime now = DateTime.Now;
                Session _currSession = null;
                string _currObjectId = null;


                if (this.ObjectSpace != null)
                {
                    _currSession = ((XPObjectSpace)this.ObjectSpace).Session;
                }

                if (_objectsToProcess != null)
                {
                    foreach (Object obj in _objectsToProcess)
                    {
                        InventoryTransfer _locInventoryTransferOS = (InventoryTransfer)_objectSpace.GetObject(obj);

                        if (_locInventoryTransferOS != null)
                        {
                            if (_locInventoryTransferOS.Code != null)
                            {
                                _currObjectId = _locInventoryTransferOS.Code;

                                InventoryTransfer _locInventoryTransferXPO = _currSession.FindObject<InventoryTransfer>
                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                     new BinaryOperator("Code", _currObjectId),
                                                                     new BinaryOperator("InventoryMovingType", InventoryMovingType.Receive)));

                                if (_locInventoryTransferXPO != null)
                                {
                                    SetReceiveBeginingInventory(_currSession, _locInventoryTransferOS);

                                    SetReceiveInventoryJournal(_currSession, _locInventoryTransferOS);

                                    SetRemainReceivedQty(_currSession, _locInventoryTransferOS);

                                    SetProcessCount(_currSession, _locInventoryTransferOS);

                                    SetStatus(_currSession, _locInventoryTransferOS);

                                    SetNormalQuantity(_currSession, _locInventoryTransferOS);

                                    SuccessMessageShow(_locInventoryTransferXPO.Code + " has been change successfully to Receive");
                                }
                                else
                                {
                                    ErrorMessageShow("Data Inventory Transfer Not Available");
                                }

                            }
                            else
                            {
                                ErrorMessageShow("Data Inventory Transfer Not Available");
                            }
                        }
                    }
                }

                if (View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    _objectSpace.CommitChanges();
                    _objectSpace.Refresh();
                }
                if (View is ListView)
                {
                    _objectSpace.CommitChanges();
                    View.ObjectSpace.Refresh();
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = InventoryTransfer " + ex.ToString());
            }
        }

        #region InventoryReceive

        //Menambahkan Qty Available ke Begining Inventory
        private void SetReceiveBeginingInventory(Session _currSession, InventoryTransfer _inventoryTransfer)
        {
            try
            {
                XPCollection<InventoryTransferLine> _locInvTransLines = new XPCollection<InventoryTransferLine>(_currSession,
                                                                        new BinaryOperator("InventoryTransfer", _inventoryTransfer));

                if (_locInvTransLines != null && _locInvTransLines.Count > 0)
                {
                    double _locInvLineTotal = 0;
                    XPCollection<BeginingInventoryLine> _locBegInventoryLines = null;
                    string _fullString = null;
                    string _locItemParse = null;
                    string _locLocationParse = null;
                    string _locBinLocationParse = null;
                    string _locDUOMParse = null;
                    string _locStockTypeParse = null;
                    string _locActiveParse = null; 


                    foreach (InventoryTransferLine _locInvTransLine in _locInvTransLines)
                    {
                        if(_locInvTransLine.Status == Status.Progress || _locInvTransLine.Status == Status.Posted)
                        {
                            if(_locInvTransLine.DQty > 0 || _locInvTransLine.Qty > 0)
                            {
                                ItemUnitOfMeasure _locItemUOM = _currSession.FindObject<ItemUnitOfMeasure>
                                                        (new GroupOperator(GroupOperatorType.And,
                                                         new BinaryOperator("Item", _locInvTransLine.Item),
                                                         new BinaryOperator("UOM", _locInvTransLine.UOM),
                                                         new BinaryOperator("DefaultUOM", _locInvTransLine.DUOM),
                                                         new BinaryOperator("Active", true)));

                                BeginingInventory _locBeginingInventory = _currSession.FindObject<BeginingInventory>(new GroupOperator(GroupOperatorType.And,
                                                                    new BinaryOperator("Item", _locInvTransLine.Item)));

                                if (_locBeginingInventory != null)
                                {
                                    if (_locInvTransLine.Item != null) { _locItemParse = "[Item.Code]=='" + _locInvTransLine.Item.Code + "'"; }
                                    else { _locItemParse = ""; }

                                    if (_locInvTransLine.Location != null && (_locInvTransLine.Item != null))
                                    { _locLocationParse = "AND [Location.Code]=='" + _locInvTransLine.Location.Code + "'"; }
                                    else if (_locInvTransLine.Location != null && _locInvTransLine.Item == null)
                                    { _locLocationParse = " [Location.Code]=='" + _locInvTransLine.Location.Code + "'"; }
                                    else { _locLocationParse = ""; }

                                    if (_locInvTransLine.BinLocation != null && (_locInvTransLine.Item != null || _locInvTransLine.Location != null))
                                    { _locBinLocationParse = "AND [BinLocation.Code]=='" + _locInvTransLine.BinLocation.Code + "'"; }
                                    else if (_locInvTransLine.BinLocation != null && _locInvTransLine.Item == null && _locInvTransLine.Location == null)
                                    { _locBinLocationParse = " [BinLocation.Code]=='" + _locInvTransLine.BinLocation.Code + "'"; }
                                    else { _locBinLocationParse = ""; }

                                    if (_locInvTransLine.DUOM != null && (_locInvTransLine.Item != null || _locInvTransLine.Location != null || _locInvTransLine.BinLocation != null))
                                    { _locDUOMParse = "AND [DUOM.Code]=='" + _locInvTransLine.DUOM.Code + "'"; }
                                    else if (_locInvTransLine.DUOM != null && _locInvTransLine.Item == null && _locInvTransLine.Location == null && _locInvTransLine.BinLocation == null)
                                    { _locDUOMParse = " [DUOM.Code]=='" + _locInvTransLine.DUOM.Code + "'"; }
                                    else { _locDUOMParse = ""; }

                                    if (_locInvTransLine.StockType != StockType.None && (_locInvTransLine.Item != null || _locInvTransLine.Location != null || _locInvTransLine.BinLocation != null
                                        || _locInvTransLine.DUOM != null))
                                    { _locStockTypeParse = "AND [StockType]=='" + GetStockType(_locInvTransLine.StockType).ToString() + "'"; }
                                    else if (_locInvTransLine.StockType != StockType.None && _locInvTransLine.Item == null && _locInvTransLine.Location == null && _locInvTransLine.BinLocation == null
                                        && _locInvTransLine.DUOM == null)
                                    { _locStockTypeParse = " [StockType]=='" + GetStockType(_locInvTransLine.StockType).ToString() + "'"; }
                                    else { _locStockTypeParse = ""; }

                                    if (_locInvTransLine.Item == null && _locInvTransLine.Location == null && _locInvTransLine.BinLocation == null
                                        && _locInvTransLine.DUOM == null && _locInvTransLine.StockType != StockType.None)
                                    { _locActiveParse = " [Active]=='" + GetActive(true).ToString() + "'"; }
                                    else { _locActiveParse = "AND [Active]=='" + GetActive(true).ToString() + "'"; }


                                    if (_locItemParse != null || _locLocationParse != null || _locBinLocationParse != null || _locDUOMParse != null || _locStockTypeParse != null)
                                    {
                                        _fullString = _locItemParse + _locLocationParse + _locBinLocationParse + _locDUOMParse + _locStockTypeParse + _locActiveParse;
                                    }
                                    else
                                    {
                                        _fullString = _locActiveParse;
                                    }

                                    _locBegInventoryLines = new XPCollection<BeginingInventoryLine>(_currSession, CriteriaOperator.Parse(_fullString));

                                    if (_locBegInventoryLines != null && _locBegInventoryLines.Count > 0)
                                    {

                                        foreach (BeginingInventoryLine _locBegInventoryLine in _locBegInventoryLines)
                                        {
                                            if (_locItemUOM != null)
                                            {
                                                if (_locItemUOM.Conversion < _locItemUOM.DefaultConversion)
                                                {
                                                    _locInvLineTotal = _locInvTransLine.Qty * _locItemUOM.DefaultConversion + _locInvTransLine.DQty;
                                                }
                                                else if (_locItemUOM.Conversion > _locItemUOM.DefaultConversion)
                                                {
                                                    _locInvLineTotal = _locInvTransLine.Qty / _locItemUOM.Conversion + _locInvTransLine.DQty;
                                                }
                                                else if (_locItemUOM.Conversion == _locItemUOM.DefaultConversion)
                                                {
                                                    _locInvLineTotal = _locInvTransLine.Qty + _locInvTransLine.DQty;
                                                }

                                            }
                                            else
                                            {
                                                _locInvLineTotal = _locInvTransLine.Qty + _locInvTransLine.DQty;
                                            }
                                            _locBegInventoryLine.QtyAvailable = _locBegInventoryLine.QtyAvailable + _locInvLineTotal;
                                            _locBegInventoryLine.Save();
                                            _locBegInventoryLine.Session.CommitTransaction();
                                        }
                                    }
                                    else
                                    {
                                        if (_locItemUOM != null)
                                        {
                                            if (_locItemUOM.Conversion < _locItemUOM.DefaultConversion)
                                            {
                                                _locInvLineTotal = _locInvTransLine.Qty * _locItemUOM.DefaultConversion + _locInvTransLine.DQty;
                                            }
                                            else if (_locItemUOM.Conversion > _locItemUOM.DefaultConversion)
                                            {
                                                _locInvLineTotal = _locInvTransLine.Qty / _locItemUOM.Conversion + _locInvTransLine.DQty;
                                            }
                                            else if (_locItemUOM.Conversion == _locItemUOM.DefaultConversion)
                                            {
                                                _locInvLineTotal = _locInvTransLine.Qty + _locInvTransLine.DQty;
                                            }
                                        }
                                        else
                                        {
                                            _locInvLineTotal = _locInvTransLine.Qty + _locInvTransLine.DQty;
                                        }

                                        BeginingInventoryLine _locSaveDataBeginingInventory = new BeginingInventoryLine(_currSession)
                                        {
                                            Item = _locInvTransLine.Item,
                                            Location = _locInvTransLine.Location,
                                            BinLocation = _locInvTransLine.BinLocation,
                                            QtyAvailable = _locInvLineTotal,
                                            DefaultUOM = _locInvTransLine.DUOM
                                        };
                                        _locSaveDataBeginingInventory.Save();
                                        _locSaveDataBeginingInventory.Session.CommitTransaction();
                                    }
                                }
                            } 
                        } 
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError("Business Object = InventoryTransfer ", ex.ToString());
            }
        }

        //Membuat jurnal positif di inventory journal
        private void SetReceiveInventoryJournal(Session _currSession, InventoryTransfer _inventoryTransfer)
        {
            try
            {
                XPCollection<InventoryTransferLine> _locInvTransLines = new XPCollection<InventoryTransferLine>(_currSession,
                                                                        new BinaryOperator("InventoryTransfer", _inventoryTransfer));

                if (_locInvTransLines != null && _locInvTransLines.Count > 0)
                {
                    double _locInvLineTotal = 0;
                    DateTime now = DateTime.Now;

                    foreach (InventoryTransferLine _locInvTransLine in _locInvTransLines)
                    {
                        if (_locInvTransLine.Status == Status.Progress  || _locInvTransLine.Status == Status.Posted)
                        {
                            if(_locInvTransLine.DQty > 0 || _locInvTransLine.Qty > 0)
                            {
                                ItemUnitOfMeasure _locItemUOM = _currSession.FindObject<ItemUnitOfMeasure>
                                                        (new GroupOperator(GroupOperatorType.And,
                                                         new BinaryOperator("Item", _locInvTransLine.Item),
                                                         new BinaryOperator("UOM", _locInvTransLine.UOM),
                                                         new BinaryOperator("DefaultUOM", _locInvTransLine.DUOM),
                                                         new BinaryOperator("Active", true)));
                                if (_locItemUOM != null)
                                {

                                    if (_locItemUOM.Conversion < _locItemUOM.DefaultConversion)
                                    {
                                        _locInvLineTotal = _locInvTransLine.Qty * _locItemUOM.DefaultConversion + _locInvTransLine.DQty;
                                    }
                                    else if (_locItemUOM.Conversion > _locItemUOM.DefaultConversion)
                                    {
                                        _locInvLineTotal = _locInvTransLine.Qty / _locItemUOM.Conversion + _locInvTransLine.DQty;
                                    }
                                    else if (_locItemUOM.Conversion == _locItemUOM.DefaultConversion)
                                    {
                                        _locInvLineTotal = _locInvTransLine.Qty + _locInvTransLine.DQty;
                                    }

                                    InventoryJournal _locPositifInventoryJournal = new InventoryJournal(_currSession)
                                    {
                                        DocumentType = _inventoryTransfer.DocumentType,
                                        DocNo = _inventoryTransfer.DocNo,
                                        Location = _locInvTransLine.Location,
                                        BinLocation = _locInvTransLine.BinLocation,
                                        Item = _locInvTransLine.Item,
                                        QtyNeg = 0,
                                        QtyPos = _locInvLineTotal,
                                        JournalDate = now,
                                        ProjectHeader = _inventoryTransfer.ProjectHeader
                                    };
                                    _locPositifInventoryJournal.Save();
                                    _locPositifInventoryJournal.Session.CommitTransaction();

                                }
                                else
                                {
                                    _locInvLineTotal = _locInvTransLine.Qty + _locInvTransLine.DQty;

                                    InventoryJournal _locPositifInventoryJournal = new InventoryJournal(_currSession)
                                    {

                                        DocumentType = _inventoryTransfer.DocumentType,
                                        DocNo = _inventoryTransfer.DocNo,
                                        Location = _locInvTransLine.Location,
                                        BinLocation = _locInvTransLine.BinLocation,
                                        Item = _locInvTransLine.Item,
                                        QtyNeg = 0,
                                        QtyPos = _locInvLineTotal,
                                        JournalDate = now,
                                        ProjectHeader = _inventoryTransfer.ProjectHeader
                                    };
                                    _locPositifInventoryJournal.Save();
                                    _locPositifInventoryJournal.Session.CommitTransaction();
                                }
                            }  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError("Business Object = InventoryTransfer ", ex.ToString());
            }
        }

        //Menentukan sisa dari transaksi receive
        private void SetRemainReceivedQty(Session _currSession, InventoryTransfer _inventoryTransfer)
        {
            try
            {
                if(_inventoryTransfer != null)
                {
                    XPCollection<InventoryTransferLine> _locInventoryTransferLines = new XPCollection<InventoryTransferLine>(_currSession,
                                                            new GroupOperator(GroupOperatorType.And,
                                                            new BinaryOperator("InventoryTransfer", _inventoryTransfer)));

                    if (_locInventoryTransferLines != null && _locInventoryTransferLines.Count > 0)
                    {
                        double _locRmDQty = 0;
                        double _locRmQty = 0;
                        double _locInvLineTotal = 0;

                        foreach (InventoryTransferLine _locInventoryTransferLine in _locInventoryTransferLines)
                        {
                            if(_locInventoryTransferLine.ProcessCount == 0)
                            {
                                if (_locInventoryTransferLine.MxDQty > 0)
                                {
                                    if(_locInventoryTransferLine.DQty > 0)
                                    {
                                        _locRmDQty = _locInventoryTransferLine.MxDQty - _locInventoryTransferLine.DQty;
                                    }
                                    
                                    if(_locInventoryTransferLine.Qty > 0)
                                    {
                                        _locRmQty = _locInventoryTransferLine.MxQty - _locInventoryTransferLine.Qty;
                                    }
                                    

                                    ItemUnitOfMeasure _locItemUOM = _currSession.FindObject<ItemUnitOfMeasure>
                                                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                                                     new BinaryOperator("Item", _locInventoryTransferLine.Item),
                                                                                                     new BinaryOperator("UOM", _locInventoryTransferLine.MxUOM),
                                                                                                     new BinaryOperator("DefaultUOM", _locInventoryTransferLine.MxDUOM),
                                                                                                     new BinaryOperator("Active", true)));
                                    if (_locItemUOM != null)
                                    {
                                        if (_locItemUOM.Conversion < _locItemUOM.DefaultConversion)
                                        {
                                            _locInvLineTotal = _locRmQty * _locItemUOM.DefaultConversion + _locRmDQty;
                                        }
                                        else if (_locItemUOM.Conversion > _locItemUOM.DefaultConversion)
                                        {
                                            _locInvLineTotal = _locRmQty / _locItemUOM.Conversion + _locRmDQty;
                                        }
                                        else if (_locItemUOM.Conversion == _locItemUOM.DefaultConversion)
                                        {
                                            _locInvLineTotal = _locRmQty + _locRmDQty;
                                        }
                                    }
                                    else
                                    {
                                        _locInvLineTotal = _locRmQty + _locRmDQty;
                                    }
                                    
                                }
                            }

                            if(_locInventoryTransferLine.ProcessCount > 0)
                            {
                                if(_locInventoryTransferLine.RmDQty > 0)
                                {
                                    _locRmDQty = _locInventoryTransferLine.RmDQty - _locInventoryTransferLine.DQty;
                                }

                                if(_locInventoryTransferLine.RmQty > 0)
                                {
                                    _locRmQty = _locInventoryTransferLine.RmQty - _locInventoryTransferLine.Qty;
                                }

                                ItemUnitOfMeasure _locItemUOM = _currSession.FindObject<ItemUnitOfMeasure>
                                                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                                                     new BinaryOperator("Item", _locInventoryTransferLine.Item),
                                                                                                     new BinaryOperator("UOM", _locInventoryTransferLine.MxUOM),
                                                                                                     new BinaryOperator("DefaultUOM", _locInventoryTransferLine.MxDUOM),
                                                                                                     new BinaryOperator("Active", true)));
                                if (_locItemUOM != null)
                                {
                                    if (_locItemUOM.Conversion < _locItemUOM.DefaultConversion)
                                    {
                                        _locInvLineTotal = _locRmQty * _locItemUOM.DefaultConversion + _locRmDQty;
                                    }
                                    else if (_locItemUOM.Conversion > _locItemUOM.DefaultConversion)
                                    {
                                        _locInvLineTotal = _locRmQty / _locItemUOM.Conversion + _locRmDQty;
                                    }
                                    else if (_locItemUOM.Conversion == _locItemUOM.DefaultConversion)
                                    {
                                        _locInvLineTotal = _locRmQty + _locRmDQty;
                                    }
                                }
                                else
                                {
                                    _locInvLineTotal = _locRmQty + _locRmDQty;
                                }

                            }

                            _locInventoryTransferLine.RmDQty = _locRmDQty;
                            _locInventoryTransferLine.RmQty = _locRmQty;
                            _locInventoryTransferLine.RmTQty = _locInvLineTotal;
                            _locInventoryTransferLine.Save();
                            _locInventoryTransferLine.Session.CommitTransaction();
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = InventoryTransfer " + ex.ToString());
            }
            
        }

        //Menentukan Banyak process receive
        private void SetProcessCount(Session _currSession, InventoryTransfer _inventoryTransfer)
        {
            try
            {
                if (_inventoryTransfer != null)
                {
                    XPCollection<InventoryTransferLine> _locInventoryTransferLines = new XPCollection<InventoryTransferLine>(_currSession,
                                                            new GroupOperator(GroupOperatorType.And,
                                                            new BinaryOperator("InventoryTransfer", _inventoryTransfer)));

                    if (_locInventoryTransferLines != null && _locInventoryTransferLines.Count > 0)
                    {
                        
                        foreach (InventoryTransferLine _locInventoryTransferLine in _locInventoryTransferLines)
                        {
                            if(_locInventoryTransferLine.Status == Status.Progress || _locInventoryTransferLine.Status == Status.Posted)
                            {
                                if (_locInventoryTransferLine.DQty > 0 || _locInventoryTransferLine.Qty > 0)
                                {
                                    _locInventoryTransferLine.ProcessCount = _locInventoryTransferLine.ProcessCount + 1;
                                    _locInventoryTransferLine.Save();
                                    _locInventoryTransferLine.Session.CommitTransaction();
                                }
                            }  
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = InventoryTransfer " + ex.ToString());
            }

        }

        //Menentukan Status Receive
        private void SetStatus(Session _currSession, InventoryTransfer _inventoryTransfer)
        {
            try
            {
                DateTime now = DateTime.Now;
                if (_inventoryTransfer != null)
                {
                    XPCollection<InventoryTransferLine> _locInventoryTransferLines = new XPCollection<InventoryTransferLine>(_currSession,
                                                            new GroupOperator(GroupOperatorType.And,
                                                            new BinaryOperator("InventoryTransfer", _inventoryTransfer)));

                    if (_locInventoryTransferLines != null && _locInventoryTransferLines.Count > 0)
                    {

                        foreach (InventoryTransferLine _locInventoryTransferLine in _locInventoryTransferLines)
                        {
                            if(_locInventoryTransferLine.Status == Status.Progress || _locInventoryTransferLine.Status == Status.Posted)
                            {
                                if (_locInventoryTransferLine.DQty > 0 || _locInventoryTransferLine.Qty > 0)
                                {
                                    if (_locInventoryTransferLine.RmDQty == 0 && _locInventoryTransferLine.RmQty == 0 && _locInventoryTransferLine.RmTQty == 0)
                                    {
                                        _locInventoryTransferLine.Status = Status.Close;
                                        _locInventoryTransferLine.ActivationPosting = true;
                                        _locInventoryTransferLine.StatusDate = now;
                                    }
                                    else
                                    {
                                        _locInventoryTransferLine.Status = Status.Posted;
                                        _locInventoryTransferLine.StatusDate = now;
                                    }
                                    _locInventoryTransferLine.Save();
                                    _locInventoryTransferLine.Session.CommitTransaction();
                                }
                            }
                            

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = InventoryTransfer " + ex.ToString());
            }
        }

        //Menormalkan Quantity
        private void SetNormalQuantity(Session _currSession, InventoryTransfer _inventoryTransfer)
        {
            try
            {
                DateTime now = DateTime.Now;
                if (_inventoryTransfer != null)
                {
                    XPCollection<InventoryTransferLine> _locInventoryTransferLines = new XPCollection<InventoryTransferLine>(_currSession,
                                                            new GroupOperator(GroupOperatorType.And,
                                                            new BinaryOperator("InventoryTransfer", _inventoryTransfer)));

                    if (_locInventoryTransferLines != null && _locInventoryTransferLines.Count > 0)
                    {

                        foreach (InventoryTransferLine _locInventoryTransferLine in _locInventoryTransferLines)
                        {
                            if (_locInventoryTransferLine.Status == Status.Progress || _locInventoryTransferLine.Status == Status.Posted)
                            {
                                if (_locInventoryTransferLine.DQty > 0 || _locInventoryTransferLine.Qty > 0)
                                {
                                    _locInventoryTransferLine.DQty = 0;
                                    _locInventoryTransferLine.Qty = 0;
                                    _locInventoryTransferLine.Save();
                                    _locInventoryTransferLine.Session.CommitTransaction();
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = InventoryTransfer " + ex.ToString());
            }
        }

        #endregion InventoryReceive

        #region Global Method

        private void SuccessMessageShow(string _locMessage)
        {
            MessageOptions options = new MessageOptions();
            options.Duration = 4000;
            options.Message = string.Format("{0}", _locMessage);
            options.Type = InformationType.Success;
            options.Web.Position = InformationPosition.Right;
            options.Win.Caption = "Success";
            options.Win.Type = WinMessageType.Flyout;
            Application.ShowViewStrategy.ShowMessage(options);
        }

        private void ErrorMessageShow(string _locMessage)
        {
            MessageOptions options = new MessageOptions();
            options.Duration = 2000;
            options.Message = string.Format(_locMessage);
            options.Type = InformationType.Warning;
            options.Web.Position = InformationPosition.Right;
            options.Win.Caption = "Error";
            options.Win.Type = WinMessageType.Alert;
            Application.ShowViewStrategy.ShowMessage(options);
        }

        private int GetStockType(StockType objectName)
        {
            int _result = 0;
            try
            {
                if (objectName == StockType.Good)
                {
                    _result = 1;
                }
                else if (objectName == StockType.Bad)
                {
                    _result = 2;
                }
                else
                {
                    _result = 0;
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" Module = InventoryTransferAction " + ex.ToString());
            }
            return _result;
        }

        private int GetActive(bool objectName)
        {
            int _result = 0;
            try
            {
                if (objectName == true)
                {
                    _result = 1;
                }
                else
                {
                    _result = 0;
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" Module = InventoryTransferAction " + ex.ToString());
            }
            return _result;
        }

        #endregion Global Method


    }
}
