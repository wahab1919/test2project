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
    public partial class ProjectHeaderActionController : ViewController
    {
        public ProjectHeaderActionController()
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

        private void ProjectHeaderProgressAction_Execute(object sender, SimpleActionExecuteEventArgs e)
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
                        ProjectHeader _locProjectHeaderOS = (ProjectHeader)_objectSpace.GetObject(obj);

                        if (_locProjectHeaderOS != null)
                        {
                            if (_locProjectHeaderOS.Code != null)
                            {
                                _currObjectId = _locProjectHeaderOS.Code;

                                ProjectHeader _locProjectHeaderXPO = _currSession.FindObject<ProjectHeader>
                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                     new BinaryOperator("Code", _currObjectId)));

                                if (_locProjectHeaderXPO != null)
                                {
                                    if(_locProjectHeaderXPO.Status == Status.Open || _locProjectHeaderXPO.Status == Status.Progress || _locProjectHeaderXPO.Status == Status.Lock)
                                    {
                                        if(_locProjectHeaderXPO.Status == Status.Open)
                                        {
                                            _locProjectHeaderXPO.Status = Status.Progress;
                                            _locProjectHeaderXPO.StatusDate = now;
                                            _locProjectHeaderXPO.Save();
                                            _locProjectHeaderXPO.Session.CommitTransaction();
                                        }
                                        

                                        XPCollection<ProjectLine> _locProjectLines = new XPCollection<ProjectLine>(_currSession,
                                                                                     new GroupOperator(GroupOperatorType.And,
                                                                                     new BinaryOperator("ProjectHeader", _locProjectHeaderXPO)));

                                        if (_locProjectLines != null && _locProjectLines.Count > 0)
                                        {
                                            foreach (ProjectLine _locProjectLine in _locProjectLines)
                                            {
                                                if(_locProjectLine.Status == Status.Open || _locProjectLine.Status == Status.Progress || _locProjectLine.Status == Status.Lock)
                                                {
                                                    if(_locProjectLine.Status == Status.Open)
                                                    {
                                                        _locProjectLine.Status = Status.Progress;
                                                        _locProjectLine.StatusDate = now;
                                                        _locProjectLine.Save();
                                                        _locProjectLine.Session.CommitTransaction();
                                                    }
                                                    

                                                    XPCollection<ProjectLineItem> _locProjectLineItems = new XPCollection<ProjectLineItem>(_currSession,
                                                                                                     new GroupOperator(GroupOperatorType.And,
                                                                                                     new BinaryOperator("ProjectLine", _locProjectLine),
                                                                                                     new BinaryOperator("ProjectHeader", _locProjectHeaderXPO)));

                                                    XPCollection<ProjectLineItem2> _locProjectLineItem2s = new XPCollection<ProjectLineItem2>(_currSession,
                                                                                                         new GroupOperator(GroupOperatorType.And,
                                                                                                         new BinaryOperator("ProjectLine", _locProjectLine),
                                                                                                         new BinaryOperator("ProjectHeader", _locProjectHeaderXPO)));

                                                    XPCollection<ProjectLineService> _locProjectLineServices = new XPCollection<ProjectLineService>(_currSession,
                                                                                                         new GroupOperator(GroupOperatorType.And,
                                                                                                         new BinaryOperator("ProjectLine", _locProjectLine),
                                                                                                         new BinaryOperator("ProjectHeader", _locProjectHeaderXPO)));

                                                    if (_locProjectLineItems != null && _locProjectLineItems.Count > 0)
                                                    {
                                                        foreach (ProjectLineItem _locProjectLineItem in _locProjectLineItems)
                                                        {
                                                            if(_locProjectLineItem.Status == Status.Open)
                                                            {
                                                                _locProjectLineItem.Status = Status.Progress;
                                                                _locProjectLineItem.StatusDate = now;
                                                                _locProjectLineItem.Save();
                                                                _locProjectLineItem.Session.CommitTransaction();
                                                            } 
                                                        }
                                                    }

                                                    if (_locProjectLineItem2s != null && _locProjectLineItem2s.Count > 0)
                                                    {
                                                        foreach(ProjectLineItem2 _locProjectLineItem2 in _locProjectLineItem2s)
                                                        {
                                                            if(_locProjectLineItem2.Status == Status.Open)
                                                            {
                                                                _locProjectLineItem2.Status = Status.Progress;
                                                                _locProjectLineItem2.StatusDate = now;
                                                                _locProjectLineItem2.Save();
                                                                _locProjectLineItem2.Session.CommitTransaction();
                                                            }
                                                        }
                                                    }

                                                    if (_locProjectLineServices != null && _locProjectLineServices.Count > 0)
                                                    {
                                                        foreach (ProjectLineService _locProjectLineService in _locProjectLineServices)
                                                        {
                                                            if (_locProjectLineService.Status == Status.Open)
                                                            {
                                                                _locProjectLineService.Status = Status.Progress;
                                                                _locProjectLineService.StatusDate = now;
                                                                _locProjectLineService.Save();
                                                                _locProjectLineService.Session.CommitTransaction();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    
                                    SuccessMessageShow("Project Header has been successfully updated to progress");
                                }
                                else
                                {
                                    ErrorMessageShow("Project Header Data Not Available");
                                }
                            }
                            else
                            {
                                ErrorMessageShow("Project Header Data Not Available");
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
                Tracing.Tracer.LogError(" BusinessObject = ProjectHeader " + ex.ToString());
            }
        }

        private void ProjectHeaderLockAction_Execute(object sender, SimpleActionExecuteEventArgs e)
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
                        ProjectHeader _locProjectHeaderOS = (ProjectHeader)_objectSpace.GetObject(obj);

                        if (_locProjectHeaderOS != null)
                        {
                            if (_locProjectHeaderOS.Code != null)
                            {
                                _currObjectId = _locProjectHeaderOS.Code;

                                ProjectHeader _locProjectHeaderXPO = _currSession.FindObject<ProjectHeader>
                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                     new BinaryOperator("Code", _currObjectId)));

                                if (_locProjectHeaderXPO != null)
                                {
                                    if (_locProjectHeaderXPO.Status == Status.Progress || _locProjectHeaderXPO.Status == Status.Lock)
                                    {
                                        if(_locProjectHeaderXPO.Status == Status.Progress)
                                        {
                                            _locProjectHeaderXPO.Status = Status.Lock;
                                            _locProjectHeaderXPO.StatusDate = now;
                                            _locProjectHeaderXPO.ActivationPosting = true;
                                            _locProjectHeaderXPO.Save();
                                            _locProjectHeaderXPO.Session.CommitTransaction();
                                        }
                                        

                                        XPCollection<ProjectLine> _locProjectLines = new XPCollection<ProjectLine>(_currSession,
                                                                                     new GroupOperator(GroupOperatorType.And,
                                                                                     new BinaryOperator("ProjectHeader", _locProjectHeaderXPO)));

                                        if (_locProjectLines != null && _locProjectLines.Count > 0)
                                        {
                                            foreach (ProjectLine _locProjectLine in _locProjectLines)
                                            {
                                                if (_locProjectLine.Status == Status.Progress || _locProjectLine.Status == Status.Lock)
                                                {
                                                    if(_locProjectLine.Status == Status.Progress)
                                                    {
                                                        _locProjectLine.Status = Status.Lock;
                                                        _locProjectLine.StatusDate = now;
                                                        _locProjectLine.ActivationPosting = true;
                                                        _locProjectLine.Save();
                                                        _locProjectLine.Session.CommitTransaction();
                                                    }
                                                    

                                                    XPCollection<ProjectLineItem> _locProjectLineItems = new XPCollection<ProjectLineItem>(_currSession,
                                                                                                     new GroupOperator(GroupOperatorType.And,
                                                                                                     new BinaryOperator("ProjectLine", _locProjectLine),
                                                                                                     new BinaryOperator("ProjectHeader", _locProjectHeaderXPO)));

                                                    XPCollection<ProjectLineItem2> _locProjectLineItem2s = new XPCollection<ProjectLineItem2>(_currSession,
                                                                                                         new GroupOperator(GroupOperatorType.And,
                                                                                                         new BinaryOperator("ProjectLine", _locProjectLine),
                                                                                                         new BinaryOperator("ProjectHeader", _locProjectHeaderXPO)));

                                                    XPCollection<ProjectLineService> _locProjectLineServices = new XPCollection<ProjectLineService>(_currSession,
                                                                                                         new GroupOperator(GroupOperatorType.And,
                                                                                                         new BinaryOperator("ProjectLine", _locProjectLine),
                                                                                                         new BinaryOperator("ProjectHeader", _locProjectHeaderXPO)));

                                                    if (_locProjectLineItems != null && _locProjectLineItems.Count > 0)
                                                    {
                                                        foreach (ProjectLineItem _locProjectLineItem in _locProjectLineItems)
                                                        {
                                                            if (_locProjectLineItem.Status == Status.Progress)
                                                            {
                                                                _locProjectLineItem.Status = Status.Lock;
                                                                _locProjectLineItem.StatusDate = now;
                                                                _locProjectLineItem.ActivationPosting = true;
                                                                _locProjectLineItem.Save();
                                                                _locProjectLineItem.Session.CommitTransaction();
                                                            }
                                                        }
                                                    }

                                                    if (_locProjectLineItem2s != null && _locProjectLineItem2s.Count > 0)
                                                    {
                                                        foreach (ProjectLineItem2 _locProjectLineItem2 in _locProjectLineItem2s)
                                                        {
                                                            if (_locProjectLineItem2.Status == Status.Progress)
                                                            {
                                                                _locProjectLineItem2.Status = Status.Lock;
                                                                _locProjectLineItem2.StatusDate = now;
                                                                _locProjectLineItem2.ActivationPosting = true;
                                                                _locProjectLineItem2.Save();
                                                                _locProjectLineItem2.Session.CommitTransaction();
                                                            }
                                                        }
                                                    }

                                                    if (_locProjectLineServices != null && _locProjectLineServices.Count > 0)
                                                    {
                                                        foreach (ProjectLineService _locProjectLineService in _locProjectLineServices)
                                                        {
                                                            if (_locProjectLineService.Status == Status.Progress)
                                                            {
                                                                _locProjectLineService.Status = Status.Lock;
                                                                _locProjectLineService.StatusDate = now;
                                                                _locProjectLineService.ActivationPosting = true;
                                                                _locProjectLineService.Save();
                                                                _locProjectLineService.Session.CommitTransaction();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    SuccessMessageShow("Project Header has been successfully updated to Lock");
                                }
                                else
                                {
                                    ErrorMessageShow("Project Header Data Not Available");
                                }
                            }
                            else
                            {
                                ErrorMessageShow("Project Header Data Not Available");
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
                Tracing.Tracer.LogError(" BusinessObject = ProjectHeader " + ex.ToString());
            }
        }

        private void ProjectHeaderPrePOAction_Execute(object sender, SimpleActionExecuteEventArgs e)
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
                        ProjectHeader _locProjectHeaderOS = (ProjectHeader)_objectSpace.GetObject(obj);

                        if(_locProjectHeaderOS != null)
                        {
                            if(_locProjectHeaderOS.Code != null)
                            {
                                _currObjectId = _locProjectHeaderOS.Code;

                                XPQuery<ProjectLineItem2> _projectLineItem2sQuery = new XPQuery<ProjectLineItem2>(_currSession);

                                ProjectHeader _locProjectHeaderXPO = _currSession.FindObject<ProjectHeader>
                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                     new BinaryOperator("Code", _currObjectId)));

                                #region ProjectHeaderXPO
                                if(_locProjectHeaderXPO != null)
                                {
                                    if(_locProjectHeaderXPO.Status == Status.Lock)
                                    {
                                        var _projectLineItem2s = from pli2 in _projectLineItem2sQuery
                                                                 where (pli2.ProjectHeader == _locProjectHeaderXPO)
                                                                 where (pli2.Status == Status.Lock)
                                                                 group pli2 by pli2.Item into g
                                                                 select new { Item = g.Key };

                                        if (_projectLineItem2s != null && _projectLineItem2s.Count() > 0)
                                        {
                                            foreach (var _projectLineItem2 in _projectLineItem2s)
                                            {
                                                XPCollection<ProjectLineItem2> _locProjectLineItem2s = new XPCollection<ProjectLineItem2>(_currSession,
                                                                        new GroupOperator(GroupOperatorType.And,
                                                                        new BinaryOperator("ProjectHeader", _locProjectHeaderXPO),
                                                                        new BinaryOperator("Item", _projectLineItem2.Item),
                                                                        new BinaryOperator("Status", Status.Lock)));

                                                double _locDefaultQty = 0;
                                                UnitOfMeasure _locDefaultUOM = null;
                                                double _locQty = 0;
                                                UnitOfMeasure _locUOM = null;
                                                double _locTotalQty = 0;
                                                double _locUnitPrice = 0;
                                                double _locTotalUnitPrice = 0;

                                                if (_locProjectLineItem2s != null && _locProjectLineItem2s.Count > 0)
                                                {
                                                    foreach (ProjectLineItem2 _locProjectLineItem2 in _locProjectLineItem2s)
                                                    {
                                                        _locDefaultQty = _locDefaultQty + _locProjectLineItem2.DQty;
                                                        _locDefaultUOM = _locProjectLineItem2.DUOM;
                                                        _locQty = _locQty + _locProjectLineItem2.Qty;
                                                        _locUOM = _locProjectLineItem2.UOM;
                                                        _locTotalQty = _locTotalQty + _locProjectLineItem2.TQty;
                                                        _locUnitPrice = _locProjectLineItem2.UnitPrice;
                                                        _locTotalUnitPrice = _locTotalUnitPrice + _locProjectLineItem2.TotalUnitPrice;
                                                    }

                                                    PrePurchaseOrder _locPrePurchaseOrder = _currSession.FindObject<PrePurchaseOrder>
                                                                                        (new GroupOperator(GroupOperatorType.And,
                                                                                         new BinaryOperator("ProjectHeader", _locProjectHeaderXPO),
                                                                                         new BinaryOperator("Item", _projectLineItem2.Item)));

                                                    if (_locPrePurchaseOrder != null)
                                                    {
                                                        _locPrePurchaseOrder.Item = _projectLineItem2.Item;
                                                        _locPrePurchaseOrder.MxDQty = _locDefaultQty;
                                                        _locPrePurchaseOrder.MxDUOM = _locDefaultUOM;
                                                        _locPrePurchaseOrder.MxQty = _locQty;
                                                        _locPrePurchaseOrder.MxTQty = _locTotalQty;
                                                        _locPrePurchaseOrder.MxUPrice = _locUnitPrice;
                                                        _locPrePurchaseOrder.MxTUPrice = _locTotalUnitPrice;
                                                        _locPrePurchaseOrder.DQty = _locDefaultQty;
                                                        _locPrePurchaseOrder.Qty = _locQty;
                                                        _locPrePurchaseOrder.TotalQty = _locTotalQty;
                                                        _locPrePurchaseOrder.Save();
                                                        _locPrePurchaseOrder.Session.CommitTransaction();
                                                    }
                                                    else
                                                    {
                                                        PrePurchaseOrder _saveData = new PrePurchaseOrder(_currSession)
                                                        {
                                                            Item = _projectLineItem2.Item,
                                                            MxDQty = _locDefaultQty,
                                                            MxDUOM = _locDefaultUOM,
                                                            MxQty = _locQty,
                                                            MxTQty = _locTotalQty,
                                                            MxUPrice = _locUnitPrice,
                                                            MxTUPrice = _locTotalUnitPrice,
                                                            DQty = _locDefaultQty,
                                                            Qty = _locQty,
                                                            TotalQty = _locTotalQty,
                                                            ProjectHeader = _locProjectHeaderXPO,
                                                        };
                                                        _saveData.Save();
                                                        _saveData.Session.CommitTransaction();
                                                    }
                                                }

                                                 
                                            }
                                        }
                                        SuccessMessageShow("Pre Purchase Order was succesfully created");
                                    }
                                }
                                #endregion ProjectHeaderXPO
                                ErrorMessageShow("Project Header Not Available");
                            }
                        }
                        ErrorMessageShow("Project Header Not Available");
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
                Tracing.Tracer.LogError(" Controller = ProjectHeaderAction " + ex.ToString());
            }
        }

        private void ProjectHeaderOrderAction_Execute(object sender, SimpleActionExecuteEventArgs e)
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
                        ProjectHeader _locProjectHeaderOS = (ProjectHeader)_objectSpace.GetObject(obj);

                        if (_locProjectHeaderOS != null)
                        {
                            if (_locProjectHeaderOS.Code != null)
                            {
                                _currObjectId = _locProjectHeaderOS.Code;

                                ProjectHeader _locProjectHeaderXPO = _currSession.FindObject<ProjectHeader>
                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                     new BinaryOperator("Code", _currObjectId),
                                                                     new BinaryOperator("Status", Status.Lock)));

                                #region ProjectHeaderXPO
                                if (_locProjectHeaderXPO != null)
                                {
                                    XPQuery<PrePurchaseOrder> _prePurchaseOrdersQuery = new XPQuery<PrePurchaseOrder>(_currSession);

                                    var _prePurchaseOrders = from ppo in _prePurchaseOrdersQuery
                                                             where (ppo.ProjectHeader == _locProjectHeaderXPO && ppo.Choose == true && ppo.Status == Status.Progress)
                                                             group ppo by ppo.DirectionType into g
                                                             select new { DirectionType = g.Key };

                                    if (_prePurchaseOrders != null && _prePurchaseOrders.Count() > 0)
                                    {
                                        foreach (var _prePurchaseOrder in _prePurchaseOrders)
                                        {
                                            #region DirectionTypeExternal
                                            if (_prePurchaseOrder.DirectionType == DirectionType.External)
                                            {
                                                _currSignCode = _globFunc.GetNumberingSignUnlockOptimisticRecord(_currSession.DataLayer, ObjectList.PurchaseOrder);
                                                if (_currSignCode != null)
                                                {
                                                    PurchaseOrder _saveDataPo = new PurchaseOrder(_currSession)
                                                    {
                                                        DirectionType = DirectionType.External,
                                                        SignCode = _currSignCode,
                                                        ProjectHeader = _locProjectHeaderXPO,
                                                    };
                                                    _saveDataPo.Save();
                                                    _saveDataPo.Session.CommitTransaction();

                                                    PurchaseOrder _locPurchaseOrder = _currSession.FindObject<PurchaseOrder>
                                                                                        (new GroupOperator(GroupOperatorType.And,
                                                                                         new BinaryOperator("SignCode", _currSignCode)));

                                                    XPCollection<PrePurchaseOrder> _locPrePurchaseOrders = new XPCollection<PrePurchaseOrder>(_currSession,
                                                                                                            new GroupOperator(GroupOperatorType.And,
                                                                                                            new BinaryOperator("ProjectHeader", _locProjectHeaderXPO),
                                                                                                            new BinaryOperator("DirectionType", DirectionType.External),
                                                                                                            new BinaryOperator("Choose", true)));
                                                    if (_locPurchaseOrder != null)
                                                    {
                                                        if (_locPrePurchaseOrders != null && _locPrePurchaseOrders.Count > 0)
                                                        {
                                                            
                                                            foreach (PrePurchaseOrder _locPrePurchaseOrder in _locPrePurchaseOrders)
                                                            {
                                                                if(_locPrePurchaseOrder.Status == Status.Progress || _locPrePurchaseOrder.Status == Status.Lock)
                                                                {

                                                                    PurchaseOrderLine _saveDataPol = new PurchaseOrderLine(_currSession)
                                                                    {
                                                                        
                                                                        PurchaseType = OrderType.Item,
                                                                        Item = _locPrePurchaseOrder.Item,
                                                                        MxDQty = _locPrePurchaseOrder.DQty,
                                                                        MxDUOM = _locPrePurchaseOrder.MxDUOM,
                                                                        MxQty = _locPrePurchaseOrder.Qty,
                                                                        MxUOM = _locPrePurchaseOrder.MxUOM,
                                                                        MxTQty = _locPrePurchaseOrder.TotalQty,
                                                                        DQty = _locPrePurchaseOrder.DQty,
                                                                        DUOM = _locPrePurchaseOrder.MxDUOM,
                                                                        Qty = _locPrePurchaseOrder.Qty,
                                                                        UOM = _locPrePurchaseOrder.MxUOM,
                                                                        TQty = _locPrePurchaseOrder.TotalQty,
                                                                        PurchaseOrder = _locPurchaseOrder,
                                                                    };
                                                                    _saveDataPol.Save();
                                                                    _saveDataPol.Session.CommitTransaction();

                                                                    SetRemainQuantity(_currSession, _locPrePurchaseOrder);

                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion DirectionRuleDirect

                                            #region DirectionRuleIndirect
                                            if (_prePurchaseOrder.DirectionType == DirectionType.Internal)
                                            {
                                                _currSignCode = _globFunc.GetNumberingSignUnlockOptimisticRecord(_currSession.DataLayer, ObjectList.PurchaseRequisition);
                                                if (_currSignCode != null)
                                                {
                                                    PurchaseRequisition _saveDataPo = new PurchaseRequisition(_currSession)
                                                    {
                                                        DirectionType = DirectionType.Internal,
                                                        SignCode = _currSignCode,
                                                        ProjectHeader = _locProjectHeaderXPO,
                                                    };
                                                    _saveDataPo.Save();
                                                    _saveDataPo.Session.CommitTransaction();

                                                    PurchaseRequisition _locPurchaseRequisition = _currSession.FindObject<PurchaseRequisition>
                                                                                        (new GroupOperator(GroupOperatorType.And,
                                                                                         new BinaryOperator("SignCode", _currSignCode)));

                                                    XPCollection<PrePurchaseOrder> _locPrePurchaseOrders = new XPCollection<PrePurchaseOrder>(_currSession,
                                                                                                            new GroupOperator(GroupOperatorType.And,
                                                                                                            new BinaryOperator("ProjectHeader", _locProjectHeaderXPO),
                                                                                                            new BinaryOperator("DirectionType", DirectionType.Internal),
                                                                                                            new BinaryOperator("Choose", true)));
                                                    if (_locPurchaseRequisition != null)
                                                    {
                                                        if (_locPrePurchaseOrders != null && _locPrePurchaseOrders.Count > 0)
                                                        {
                                                            foreach (PrePurchaseOrder _locPrePurchaseOrder in _locPrePurchaseOrders)
                                                            {
                                                                if(_locPrePurchaseOrder.Status == Status.Progress || _locPrePurchaseOrder.Status == Status.Lock )
                                                                {
                                                                    PurchaseRequisitionLine _saveDataPol = new PurchaseRequisitionLine(_currSession)
                                                                    {
                                                                        PurchaseType = OrderType.Item,
                                                                        Item = _locPrePurchaseOrder.Item,
                                                                        MxDQty = _locPrePurchaseOrder.DQty,
                                                                        MxDUOM = _locPrePurchaseOrder.MxDUOM,
                                                                        MxQty = _locPrePurchaseOrder.Qty,
                                                                        MxUOM = _locPrePurchaseOrder.MxUOM,
                                                                        MxTQty = _locPrePurchaseOrder.TotalQty,
                                                                        DQty = _locPrePurchaseOrder.DQty,
                                                                        DUOM = _locPrePurchaseOrder.MxDUOM,
                                                                        Qty = _locPrePurchaseOrder.Qty,
                                                                        UOM = _locPrePurchaseOrder.MxUOM,
                                                                        TQty = _locPrePurchaseOrder.TotalQty,
                                                                        PurchaseRequisition = _locPurchaseRequisition,
                                                                    };
                                                                    _saveDataPol.Save();
                                                                    _saveDataPol.Session.CommitTransaction();

                                                                    SetRemainQuantity(_currSession, _locPrePurchaseOrder);

                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion DirectionRuleIndirect
                                        }
                                        SuccessMessageShow("PrePurchase Order Successfully Posted");
                                    }

                                }
                                #endregion ProjectHeaderXPO

                            }
                            ErrorMessageShow("Project Header Not Available");
                        }
                        ErrorMessageShow("Project Header Not Available");
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
                Tracing.Tracer.LogError(" Controller = ProjectHeaderAction" + ex.ToString());
            }
        }

        private void ProjectHeaderPrePOProgressAction_Execute(object sender, SimpleActionExecuteEventArgs e)
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
                        ProjectHeader _locProjectHeaderOS = (ProjectHeader)_objectSpace.GetObject(obj);

                        if (_locProjectHeaderOS != null)
                        {
                            if (_locProjectHeaderOS.Code != null)
                            {
                                _currObjectId = _locProjectHeaderOS.Code;

                                ProjectHeader _locProjectHeaderXPO = _currSession.FindObject<ProjectHeader>
                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                     new BinaryOperator("Code", _currObjectId)));

                                if (_locProjectHeaderXPO != null)
                                {
                                    if (_locProjectHeaderXPO.Status == Status.Lock )
                                    {

                                        XPCollection<PrePurchaseOrder> _locPrePurchaseOrders = new XPCollection<PrePurchaseOrder>(_currSession,
                                                                                     new GroupOperator(GroupOperatorType.And,
                                                                                     new BinaryOperator("ProjectHeader", _locProjectHeaderXPO),
                                                                                     new BinaryOperator("Status", Status.Open)));

                                        if(_locPrePurchaseOrders != null && _locPrePurchaseOrders.Count > 0)
                                        {
                                            foreach(PrePurchaseOrder _locPrePurchaseOrder in _locPrePurchaseOrders)
                                            {
                                                _locPrePurchaseOrder.Status = Status.Progress;
                                                _locPrePurchaseOrder.StatusDate = now;
                                                _locPrePurchaseOrder.Save();
                                                _locPrePurchaseOrder.Session.CommitTransaction();
                                            }
                                        }
                                        SuccessMessageShow("Pre Purchase Order has been successfully updated to progress");
                                    }
                                }    
                                else
                                {
                                    ErrorMessageShow("Project Header Data Not Available");
                                }
                            }
                            else
                            {
                                ErrorMessageShow("Project Header Data Not Available");
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
                Tracing.Tracer.LogError(" BusinessObject = ProjectHeader " + ex.ToString());
            }
        }

        #region Global Method

        private void SetRemainQuantity(Session _currSession, PrePurchaseOrder _locPrePurchaseOrder)
        {
            try
            {

                #region Remain Qty  with PostedCount != 0
                if (_locPrePurchaseOrder.PostedCount > 0)
                {
                    double _locRmDqty = 0;
                    double _locRmQty = 0;
                    double _locInvLineTotal = 0;
                    double _locRmTQty = 0;
                    bool _locActivationPosting = false;

                    if (_locPrePurchaseOrder.RmDQty > 0 || _locPrePurchaseOrder.RmQty > 0 && _locPrePurchaseOrder.MxTQty != _locPrePurchaseOrder.RmTQty)
                    {
                        _locRmDqty = _locPrePurchaseOrder.RmDQty - _locPrePurchaseOrder.DQty;
                        _locRmQty = _locPrePurchaseOrder.RmQty - _locPrePurchaseOrder.Qty;

                        if (_locPrePurchaseOrder.Item != null && _locPrePurchaseOrder.MxUOM != null && _locPrePurchaseOrder.MxDUOM != null)
                        {
                            ItemUnitOfMeasure _locItemUOM = _currSession.FindObject<ItemUnitOfMeasure>
                                                    (new GroupOperator(GroupOperatorType.And,
                                                     new BinaryOperator("Item", _locPrePurchaseOrder.Item),
                                                     new BinaryOperator("UOM", _locPrePurchaseOrder.MxUOM),
                                                     new BinaryOperator("DefaultUOM", _locPrePurchaseOrder.MxDUOM),
                                                     new BinaryOperator("Active", true)));
                            if (_locItemUOM != null)
                            {
                                if (_locItemUOM.Conversion < _locItemUOM.DefaultConversion)
                                {
                                    _locInvLineTotal = _locRmQty * _locItemUOM.DefaultConversion + _locRmDqty;
                                }
                                else if (_locItemUOM.Conversion > _locItemUOM.DefaultConversion)
                                {
                                    _locInvLineTotal = _locRmQty / _locItemUOM.Conversion + _locRmDqty;
                                }
                                else if (_locItemUOM.Conversion == _locItemUOM.DefaultConversion)
                                {
                                    _locInvLineTotal = _locRmQty + _locRmDqty;
                                }
                            }
                        }
                        else
                        {
                            _locInvLineTotal = _locRmQty + _locRmDqty;
                        }

                        if (_locRmDqty > 0 || _locRmQty > 0)
                        {
                            _locRmTQty = _locPrePurchaseOrder.RmTQty - _locPrePurchaseOrder.TotalQty;
                        }
                        else
                        {
                            _locRmTQty = _locPrePurchaseOrder.RmTQty - _locPrePurchaseOrder.TotalQty;
                            _locActivationPosting = true;
                        }
                        

                        _locPrePurchaseOrder.ActivationPosting = _locActivationPosting;
                        _locPrePurchaseOrder.Choose = false;
                        _locPrePurchaseOrder.RmDQty = _locRmDqty;
                        _locPrePurchaseOrder.RmQty = _locRmQty;
                        _locPrePurchaseOrder.RmTQty = _locRmTQty;
                        _locPrePurchaseOrder.PostedCount = _locPrePurchaseOrder.PostedCount + 1;
                        _locPrePurchaseOrder.DQty = 0;
                        _locPrePurchaseOrder.Qty = 0;
                        _locPrePurchaseOrder.TotalQty = 0;
                        _locPrePurchaseOrder.Save();
                        _locPrePurchaseOrder.Session.CommitTransaction();
                    }
                }
                #endregion Remain Qty  with PostedCount != 0

                #region Remain Qty with PostedCount == 0
                else
                {
                    double _locRmDqty = 0;
                    double _locRmQty = 0;
                    double _locInvLineTotal = 0;
                    double _locRmTQty = 0;
                    _locRmDqty = _locPrePurchaseOrder.MxDQty - _locPrePurchaseOrder.DQty;
                    _locRmQty = _locPrePurchaseOrder.MxQty - _locPrePurchaseOrder.Qty;

                    #region Remain > 0
                    if (_locRmDqty > 0 || _locRmQty > 0)
                    {
                        if (_locPrePurchaseOrder.Item != null && _locPrePurchaseOrder.MxUOM != null && _locPrePurchaseOrder.MxDUOM != null)
                        {
                            ItemUnitOfMeasure _locItemUOM = _currSession.FindObject<ItemUnitOfMeasure>
                                                    (new GroupOperator(GroupOperatorType.And,
                                                     new BinaryOperator("Item", _locPrePurchaseOrder.Item),
                                                     new BinaryOperator("UOM", _locPrePurchaseOrder.MxUOM),
                                                     new BinaryOperator("DefaultUOM", _locPrePurchaseOrder.MxDUOM),
                                                     new BinaryOperator("Active", true)));
                            if (_locItemUOM != null)
                            {
                                if (_locItemUOM.Conversion < _locItemUOM.DefaultConversion)
                                {
                                    _locInvLineTotal = _locRmQty * _locItemUOM.DefaultConversion + _locRmDqty;
                                }
                                else if (_locItemUOM.Conversion > _locItemUOM.DefaultConversion)
                                {
                                    _locInvLineTotal = _locRmQty / _locItemUOM.Conversion + _locRmDqty;
                                }
                                else if (_locItemUOM.Conversion == _locItemUOM.DefaultConversion)
                                {
                                    _locInvLineTotal = _locRmQty + _locRmDqty;
                                }
                            }
                        }
                        else
                        {
                            _locInvLineTotal = _locRmQty + _locRmDqty;
                        }

                        if (_locInvLineTotal > 0)
                        {
                            if (_locPrePurchaseOrder.RmTQty > 0)
                            {
                                _locRmTQty = _locPrePurchaseOrder.RmTQty - _locInvLineTotal;
                            }
                            else
                            {
                                _locRmTQty = _locInvLineTotal;
                            }

                        }

                        _locPrePurchaseOrder.RmDQty = _locRmDqty;
                        _locPrePurchaseOrder.RmQty = _locRmQty;
                        _locPrePurchaseOrder.RmTQty = _locRmTQty;
                        _locPrePurchaseOrder.PostedCount = _locPrePurchaseOrder.PostedCount + 1;
                        _locPrePurchaseOrder.Choose = false;
                        _locPrePurchaseOrder.DQty = 0;
                        _locPrePurchaseOrder.Qty = 0;
                        _locPrePurchaseOrder.TotalQty = 0;
                        _locPrePurchaseOrder.Save();
                        _locPrePurchaseOrder.Session.CommitTransaction();
                    }
                    #endregion Remain > 0

                    #region Remain <= 0
                    if (_locRmDqty <= 0 && _locRmQty <= 0)
                    {
                        _locPrePurchaseOrder.ActivationPosting = true;
                        _locPrePurchaseOrder.RmDQty = 0;
                        _locPrePurchaseOrder.RmQty = 0;
                        _locPrePurchaseOrder.RmTQty = 0;
                        _locPrePurchaseOrder.PostedCount = _locPrePurchaseOrder.PostedCount + 1;
                        _locPrePurchaseOrder.Choose = false;
                        _locPrePurchaseOrder.DQty = 0;
                        _locPrePurchaseOrder.Qty = 0;
                        _locPrePurchaseOrder.TotalQty = 0;
                        _locPrePurchaseOrder.Save();
                        _locPrePurchaseOrder.Session.CommitTransaction();
                    }
                    #endregion Remain < 0
                }
                #endregion Remain Qty with PostedCount == 0

            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = PrePurchaseOrder " + ex.ToString());
            }
        }

        private double GetTotalQuantity(Session _currSession, PrePurchaseOrder _locPrePurchaseOrder)
        {
            double _result = 0;
            try
            {
         
                if (_locPrePurchaseOrder.Item != null && _locPrePurchaseOrder.MxUOM != null && _locPrePurchaseOrder.MxDUOM != null)
                {
                    ItemUnitOfMeasure _locItemUOM = _currSession.FindObject<ItemUnitOfMeasure>
                                            (new GroupOperator(GroupOperatorType.And,
                                             new BinaryOperator("Item", _locPrePurchaseOrder.Item),
                                             new BinaryOperator("UOM", _locPrePurchaseOrder.MxUOM),
                                             new BinaryOperator("DefaultUOM", _locPrePurchaseOrder.MxDUOM),
                                             new BinaryOperator("Active", true)));
                    if (_locItemUOM != null)
                    {
                        if (_locItemUOM.Conversion < _locItemUOM.DefaultConversion)
                        {
                            _result = _locPrePurchaseOrder.Qty * _locItemUOM.DefaultConversion + _locPrePurchaseOrder.DQty;
                        }
                        else if (_locItemUOM.Conversion > _locItemUOM.DefaultConversion)
                        {
                            _result = _locPrePurchaseOrder.Qty / _locItemUOM.Conversion + _locPrePurchaseOrder.DQty;
                        }
                        else if (_locItemUOM.Conversion == _locItemUOM.DefaultConversion)
                        {
                            _result = _locPrePurchaseOrder.Qty + _locPrePurchaseOrder.DQty;
                        }
                    }
                }
                else
                {
                    _result = _locPrePurchaseOrder.Qty + _locPrePurchaseOrder.DQty;
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" BusinessObject = PrePurchaseOrder " + ex.ToString());
            }
            return _result;
        }

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
