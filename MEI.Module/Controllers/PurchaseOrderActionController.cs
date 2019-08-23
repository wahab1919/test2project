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
    public partial class PurchaseOrderActionController : ViewController
    {
        public PurchaseOrderActionController()
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

        private void PurchaseOrderProgressAction_Execute(object sender, SimpleActionExecuteEventArgs e)
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
                        PurchaseOrder _locPurchaseOrderOS = (PurchaseOrder)_objectSpace.GetObject(obj);

                        if (_locPurchaseOrderOS != null)
                        {
                            if (_locPurchaseOrderOS.Code != null)
                            {
                                _currObjectId = _locPurchaseOrderOS.Code;

                                PurchaseOrder _locPurchaseOrderXPO = _currSession.FindObject<PurchaseOrder>
                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                     new BinaryOperator("Code", _currObjectId)));

                                if (_locPurchaseOrderXPO != null)
                                {
                                    if(_locPurchaseOrderXPO.Status == Status.Open || _locPurchaseOrderXPO.Status == Status.Progress)
                                    {
                                        if(_locPurchaseOrderXPO.Status == Status.Open)
                                        {
                                            _locPurchaseOrderXPO.Status = Status.Progress;
                                            _locPurchaseOrderXPO.StatusDate = now;
                                            _locPurchaseOrderXPO.Save();
                                            _locPurchaseOrderXPO.Session.CommitTransaction();
                                        }
                                        
                                        XPCollection<PurchaseOrderLine> _locPurchaseOrderLines = new XPCollection<PurchaseOrderLine>
                                                               (_currSession, new GroupOperator(GroupOperatorType.And,
                                                                new BinaryOperator("PurchaseOrder", _locPurchaseOrderXPO),
                                                                new BinaryOperator("Status", Status.Open)));

                                        if (_locPurchaseOrderLines != null && _locPurchaseOrderLines.Count > 0)
                                        {
                                            foreach (PurchaseOrderLine _locPurchaseOrderLine in _locPurchaseOrderLines)
                                            {
                                                _locPurchaseOrderLine.Status = Status.Progress;
                                                _locPurchaseOrderLine.StatusDate = now;
                                                _locPurchaseOrderLine.Save();
                                                _locPurchaseOrderLine.Session.CommitTransaction();
                                            }
                                        }
                                    }
                                    SuccessMessageShow3(_locPurchaseOrderXPO);
                                }
                                else
                                {
                                    ErrorMessageShow("Data Purchase Order Not Available");
                                }

                            }
                            else
                            {
                                ErrorMessageShow("Data Purchase Order Not Available");
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
                Tracing.Tracer.LogError(" BusinessObject = PurchaseOrder " + ex.ToString());
            }
        }

        private void PurchaseOrderReceiptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                GlobalFunction _globFunc = new GlobalFunction();
                IObjectSpace _objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
                ArrayList _objectsToProcess = new ArrayList(e.SelectedObjects);
                DateTime now = DateTime.Now;
                Session _currSession = null;
                string _currObjectId = null;
                string _locDocCode = null;
                ProjectHeader _locProjectHeader = null;


                if (this.ObjectSpace != null)
                {
                    _currSession = ((XPObjectSpace)this.ObjectSpace).Session;
                }

                if (_objectsToProcess != null)
                {
                    foreach (Object obj in _objectsToProcess)
                    {
                        PurchaseOrder _locPurchaseOrderOS = (PurchaseOrder)_objectSpace.GetObject(obj);

                        if (_locPurchaseOrderOS != null)
                        {
                            if (_locPurchaseOrderOS.Code != null)
                            {
                                _currObjectId = _locPurchaseOrderOS.Code;

                                PurchaseOrder _locPurchaseOrderXPO = _currSession.FindObject<PurchaseOrder>
                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                     new BinaryOperator("Code", _currObjectId),
                                                                     new BinaryOperator("Status", Status.Progress)));

                                if (_locPurchaseOrderXPO != null)
                                {
                                    if(_locPurchaseOrderXPO.ProjectHeader != null)
                                    {
                                        _locProjectHeader = _locPurchaseOrderXPO.ProjectHeader;
                                    }
                                    

                                    DocumentType _locDocumentTypeXPO = _currSession.FindObject<DocumentType>
                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                     new BinaryOperator("DirectionType", DirectionType.External),
                                                                     new BinaryOperator("InventoryMovingType", InventoryMovingType.Receive),
                                                                     new BinaryOperator("ObjectList", ObjectList.InventoryTransfer),
                                                                     new BinaryOperator("DocumentRule", DocumentRule.Vendor),
                                                                     new BinaryOperator("Active", true),
                                                                     new BinaryOperator("Default", true)));

                                    if(_locDocumentTypeXPO != null)
                                    {
                                        _locDocCode = _globFunc.GetDocumentNumberingUnlockOptimisticRecord(_currSession.DataLayer, _locDocumentTypeXPO);

                                        if (_locDocCode != null)
                                        {
                                           
                                            InventoryTransfer _saveDataIT = new InventoryTransfer(_currSession)
                                            {
                                                DirectionType = _locDocumentTypeXPO.DirectionType,
                                                InventoryMovingType = _locDocumentTypeXPO.InventoryMovingType,
                                                ObjectList = _locDocumentTypeXPO.ObjectList,
                                                DocumentType = _locDocumentTypeXPO,
                                                DocNo = _locDocCode,
                                                EstimatedDate = _locPurchaseOrderXPO.EstimatedDate,
                                                PurchaseOrder = _locPurchaseOrderXPO,
                                                ProjectHeader = _locProjectHeader
                                            };
                                            _saveDataIT.Save();
                                            _saveDataIT.Session.CommitTransaction();

                                            InventoryTransfer _locInventoryTransfer = _currSession.FindObject<InventoryTransfer>
                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                     new BinaryOperator("DocNo", _locDocCode),
                                                                     new BinaryOperator("PurchaseOrder", _locPurchaseOrderXPO),
                                                                     new BinaryOperator("ProjectHeader", _locProjectHeader)));

                                            if(_locInventoryTransfer != null)
                                            {
                                               
                                                XPCollection<PurchaseOrderLine> _locPurchaseOrderLines = new XPCollection<PurchaseOrderLine>
                                                           (_currSession, new GroupOperator(GroupOperatorType.And,
                                                            new BinaryOperator("PurchaseOrder", _locPurchaseOrderXPO),
                                                            new BinaryOperator("Status", Status.Progress)));

                                                if(_locPurchaseOrderLines != null && _locPurchaseOrderLines.Count > 0)
                                                {
                                                    foreach(PurchaseOrderLine _locPurchaseOrderLine in _locPurchaseOrderLines)
                                                    {
                                                        InventoryTransferLine _saveDataInventoryTransferLine = new InventoryTransferLine(_currSession)
                                                        {
                                                            Item = _locPurchaseOrderLine.Item,
                                                            MxDQty = _locPurchaseOrderLine.DQty,
                                                            MxDUOM = _locPurchaseOrderLine.DUOM,
                                                            MxQty = _locPurchaseOrderLine.Qty,
                                                            MxUOM = _locPurchaseOrderLine.UOM,
                                                            MxTQty = _locPurchaseOrderLine.TQty,
                                                            DQty = _locPurchaseOrderLine.DQty,
                                                            DUOM = _locPurchaseOrderLine.DUOM,
                                                            Qty = _locPurchaseOrderLine.Qty,
                                                            UOM = _locPurchaseOrderLine.UOM,
                                                            TQty = _locPurchaseOrderLine.TQty,
                                                            EstimatedDate = _locInventoryTransfer.EstimatedDate,
                                                            InventoryTransfer = _locInventoryTransfer
                                                        };
                                                        _saveDataInventoryTransferLine.Save();
                                                        _saveDataInventoryTransferLine.Session.CommitTransaction();

                                                        //Change Status POL From Progress To Posted
                                                        _locPurchaseOrderLine.Status = Status.Posted;
                                                        _locPurchaseOrderLine.StatusDate = now;
                                                        _locPurchaseOrderLine.Save();
                                                        _locPurchaseOrderLine.Session.CommitTransaction();

                                                    } 
                                                }
                                            }
                                        }
                                    }

                                    //Change Status PO From Progress To Posted
                                    _locPurchaseOrderXPO.Status = Status.Posted;
                                    _locPurchaseOrderXPO.StatusDate = now;
                                    _locPurchaseOrderXPO.Save();
                                    _locPurchaseOrderXPO.Session.CommitTransaction();

                                    SuccessMessageShow(_locPurchaseOrderXPO, "Inventory Transfer");
                                }
                                else
                                {
                                    ErrorMessageShow("Data Purchase Order Not Available");
                                }

                            }
                            else
                            {
                                ErrorMessageShow("Data Purchase Order Not Available");
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
                Tracing.Tracer.LogError(" BusinessObject = PurchaseOrder " + ex.ToString());
            }
        }

        #region Global Method

        private void SuccessMessageShow(PurchaseOrder _locPurchaseOrder, string _locActionName)
        {
            MessageOptions options = new MessageOptions();
            options.Duration = 2000;
            options.Message = string.Format("{0}  have been successfully posted to {1}!", _locPurchaseOrder.Code, _locActionName);
            options.Type = InformationType.Success;
            options.Web.Position = InformationPosition.Right;
            options.Win.Caption = "Success";
            options.Win.Type = WinMessageType.Flyout;
            Application.ShowViewStrategy.ShowMessage(options);
        }

        private void SuccessMessageShow2(PurchaseOrder _locPurchaseOrder, string _locActionName)
        {
            MessageOptions options = new MessageOptions();
            options.Duration = 4000;
            options.Message = string.Format("{0}  have been successfully posted with Sign {1}!", _locPurchaseOrder.Code, _locActionName);
            options.Type = InformationType.Success;
            options.Web.Position = InformationPosition.Right;
            options.Win.Caption = "Success";
            options.Win.Type = WinMessageType.Flyout;
            Application.ShowViewStrategy.ShowMessage(options);
        }

        private void SuccessMessageShow3(PurchaseOrder _locPurchaseOrder)
        {
            MessageOptions options = new MessageOptions();
            options.Duration = 4000;
            options.Message = string.Format("{0}  has been change successfully to Progress ", _locPurchaseOrder.Code);
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

        #endregion Global Method

        
    }
}
