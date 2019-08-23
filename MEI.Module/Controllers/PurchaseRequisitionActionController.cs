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
    public partial class PurchaseRequisitionActionController : ViewController
    {
        public PurchaseRequisitionActionController()
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

        private void PurchaseRequisitionProgressAction_Execute(object sender, SimpleActionExecuteEventArgs e)
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
                        PurchaseRequisition _locPurchaseRequisitionOS = (PurchaseRequisition)_objectSpace.GetObject(obj);

                        if (_locPurchaseRequisitionOS != null)
                        {
                            if (_locPurchaseRequisitionOS.Code != null)
                            {
                                _currObjectId = _locPurchaseRequisitionOS.Code;

                                PurchaseRequisition _locPurchaseRequisitionXPO = _currSession.FindObject<PurchaseRequisition>
                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                     new BinaryOperator("Code", _currObjectId)));

                                if (_locPurchaseRequisitionXPO != null)
                                {
                                    if(_locPurchaseRequisitionXPO.Status == Status.Open || _locPurchaseRequisitionXPO.Status == Status.Progress)
                                    {
                                        if(_locPurchaseRequisitionXPO.Status == Status.Open)
                                        {
                                            _locPurchaseRequisitionXPO.Status = Status.Progress;
                                            _locPurchaseRequisitionXPO.StatusDate = now;
                                            _locPurchaseRequisitionXPO.Save();
                                            _locPurchaseRequisitionXPO.Session.CommitTransaction();
                                        }

                                        XPCollection<PurchaseRequisitionLine> _locPurchaseRequisitionLines = new XPCollection<PurchaseRequisitionLine>
                                                           (_currSession, new GroupOperator(GroupOperatorType.And,
                                                            new BinaryOperator("PurchaseRequisition", _locPurchaseRequisitionXPO),
                                                            new BinaryOperator("Status", Status.Open)));

                                        if (_locPurchaseRequisitionLines != null && _locPurchaseRequisitionLines.Count > 0)
                                        {
                                            foreach (PurchaseRequisitionLine _locPurchaseRequisitionLine in _locPurchaseRequisitionLines)
                                            {
                                                _locPurchaseRequisitionLine.Status = Status.Progress;
                                                _locPurchaseRequisitionLine.StatusDate = now;
                                                _locPurchaseRequisitionLine.Save();
                                                _locPurchaseRequisitionLine.Session.CommitTransaction();
                                            }
                                        }

                                    }  

                                    SuccessMessageShow("PR has successfully updated to progress");
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
                Tracing.Tracer.LogError(" BusinessObject = PurchaseRequisition " + ex.ToString());
            }
        }

        private void PurchaseRequisitionPurchaseOrderAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                GlobalFunction _globFunc = new GlobalFunction();
                IObjectSpace _objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
                ArrayList _objectsToProcess = new ArrayList(e.SelectedObjects);
                DateTime now = DateTime.Now;
                Session _currSession = null;
                string _currObjectId = null;
                string _currSignCode = null;

                if (this.ObjectSpace != null)
                {
                    _currSession = ((XPObjectSpace)this.ObjectSpace).Session;
                }

                if (_objectsToProcess != null)
                {
                    foreach (Object obj in _objectsToProcess)
                    {
                        PurchaseRequisition _locPurchaseRequisitionOS = (PurchaseRequisition)_objectSpace.GetObject(obj);

                        if (_locPurchaseRequisitionOS != null)
                        {
                            if (_locPurchaseRequisitionOS.Code != null)
                            {
                                _currObjectId = _locPurchaseRequisitionOS.Code;
                                PurchaseOrder _locPurchaseOrderByPR = null;
                                PurchaseOrder _locPurchaseOrderByPRandPH = null;

                                PurchaseRequisition _locPurchaseRequisitionXPO = _currSession.FindObject<PurchaseRequisition>
                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                     new BinaryOperator("Code", _currObjectId)));

                                if (_locPurchaseRequisitionXPO != null)
                                {
                                    if(_locPurchaseRequisitionXPO.Status == Status.Progress || _locPurchaseRequisitionXPO.Status == Status.Lock)
                                    {
                                        _locPurchaseOrderByPR = _currSession.FindObject<PurchaseOrder>(new GroupOperator(GroupOperatorType.And,
                                                            new BinaryOperator("PurchaseRequisition", _locPurchaseRequisitionXPO)));

                                        if (_locPurchaseRequisitionXPO.ProjectHeader != null)
                                        {
                                            _locPurchaseOrderByPRandPH = _currSession.FindObject<PurchaseOrder>(new GroupOperator(GroupOperatorType.And,
                                                                       new BinaryOperator("PurchaseRequisition", _locPurchaseRequisitionXPO),
                                                                       new BinaryOperator("ProjectHeader", _locPurchaseRequisitionXPO.ProjectHeader)));
                                        }

                                        if (_locPurchaseOrderByPR == null && _locPurchaseOrderByPRandPH == null)
                                        {
                                            _currSignCode = _globFunc.GetNumberingSignUnlockOptimisticRecord(_currSession.DataLayer, ObjectList.PurchaseOrder);

                                            if (_currSignCode != null)
                                            {
                                                PurchaseOrder _saveDataPO = new PurchaseOrder(_currSession)
                                                {
                                                    DirectionType = _locPurchaseRequisitionXPO.DirectionType,
                                                    SignCode = _currSignCode,
                                                    ProjectHeader = _locPurchaseRequisitionXPO.ProjectHeader,
                                                    PurchaseRequisition = _locPurchaseRequisitionXPO,
                                                };
                                                _saveDataPO.Save();
                                                _saveDataPO.Session.CommitTransaction();

                                                XPCollection<PurchaseRequisitionLine> _numLinePurchaseRequisitionLines = new XPCollection<PurchaseRequisitionLine>(_currSession,
                                                                                                        new GroupOperator(GroupOperatorType.And,
                                                                                                        new BinaryOperator("PurchaseRequisition", _locPurchaseRequisitionXPO)));

                                                PurchaseOrder _locPurchaseOrder2 = _currSession.FindObject<PurchaseOrder>(new GroupOperator(GroupOperatorType.And,
                                                                                    new BinaryOperator("SignCode", _currSignCode)));
                                                if (_locPurchaseOrder2 != null)
                                                {
                                                    if (_numLinePurchaseRequisitionLines != null && _numLinePurchaseRequisitionLines.Count > 0)
                                                    {
                                                        foreach (PurchaseRequisitionLine _numLinePurchaseRequisitionLine in _numLinePurchaseRequisitionLines)
                                                        {
                                                            if(_numLinePurchaseRequisitionLine.Status == Status.Progress || _numLinePurchaseRequisitionLine.Status == Status.Lock)
                                                            {
                                                                PurchaseOrderLine _saveDataPurchaseOrderLine = new PurchaseOrderLine(_currSession)
                                                                {
                                                                    PurchaseType = _numLinePurchaseRequisitionLine.PurchaseType,
                                                                    Item = _numLinePurchaseRequisitionLine.Item,
                                                                    Description = _numLinePurchaseRequisitionLine.Description,
                                                                    MxDQty = _numLinePurchaseRequisitionLine.MxDQty,
                                                                    MxDUOM = _numLinePurchaseRequisitionLine.MxDUOM,
                                                                    MxQty = _numLinePurchaseRequisitionLine.MxQty,
                                                                    MxUOM = _numLinePurchaseRequisitionLine.MxUOM,
                                                                    MxTQty = _numLinePurchaseRequisitionLine.MxTQty,
                                                                    DQty = _numLinePurchaseRequisitionLine.DQty,
                                                                    DUOM = _numLinePurchaseRequisitionLine.DUOM,
                                                                    Qty = _numLinePurchaseRequisitionLine.Qty,
                                                                    UOM = _numLinePurchaseRequisitionLine.UOM,
                                                                    TQty = _numLinePurchaseRequisitionLine.TQty,
                                                                    PurchaseOrder = _locPurchaseOrder2,
                                                                };
                                                                _saveDataPurchaseOrderLine.Save();
                                                                _saveDataPurchaseOrderLine.Session.CommitTransaction();

                                                                _numLinePurchaseRequisitionLine.ActivationPosting = true;
                                                                _numLinePurchaseRequisitionLine.Status = Status.Posted;
                                                                _numLinePurchaseRequisitionLine.StatusDate = now;
                                                                _numLinePurchaseRequisitionLine.Save();
                                                                _numLinePurchaseRequisitionLine.Session.CommitTransaction();
                                                            }
                                                            
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    
                                    _locPurchaseRequisitionXPO.Status = Status.Posted;
                                    _locPurchaseRequisitionXPO.StatusDate = now;
                                    _locPurchaseRequisitionXPO.ActivationPosting = true;
                                    _locPurchaseRequisitionXPO.Save();
                                    _locPurchaseRequisitionXPO.Session.CommitTransaction();
                                    SuccessMessageShow("PO has successfully created");
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
                Tracing.Tracer.LogError(" BusinessObject = PurchaseRequisition " + ex.ToString());
            }
        }

        #region Global Method

        private void SuccessMessageShow(string _locActionName)
        {
            MessageOptions options = new MessageOptions();
            options.Duration = 2000;
            options.Message = string.Format("{0} !", _locActionName);
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
