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
    public partial class ProjectLineActionController : ViewController
    {
        public ProjectLineActionController()
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

        private void ProjectLineGetPreviousAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                GlobalFunction _globFunc = new GlobalFunction();
                IObjectSpace _objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
                ArrayList _objectsToProcess = new ArrayList(e.SelectedObjects);
                DateTime now = DateTime.Now;
                Session _currSession = null;
                string _currObjectId = null;
                ProjectHeader _locProjectHeaderXPO = null;
                string _locProjectHeaderCode = null;

                if (this.ObjectSpace != null)
                {
                    _currSession = ((XPObjectSpace)this.ObjectSpace).Session;
                }

                if (_objectsToProcess != null)
                {
                    foreach (Object obj in _objectsToProcess)
                    {
                        ProjectLine _locProjectLineOS = (ProjectLine)_objectSpace.GetObject(obj);


                        if (_locProjectLineOS != null)
                        {
                            if (_locProjectLineOS.ProjectHeader != null)
                            {
                                if (_locProjectLineOS.ProjectHeader.Code != null)
                                {
                                    _locProjectHeaderCode = _locProjectLineOS.ProjectHeader.Code;
                                }
                            }

                            if (_locProjectLineOS.Code != null)
                            {
                                _currObjectId = _locProjectLineOS.Code;

                                ProjectLine _locProjectLineXPO = _currSession.FindObject<ProjectLine>
                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                     new BinaryOperator("Code", _currObjectId)));

                                if (_locProjectLineXPO == null)
                                {
                                    if (_locProjectHeaderCode != null)
                                    {
                                        _locProjectHeaderXPO = _currSession.FindObject<ProjectHeader>
                                                                    (new GroupOperator(GroupOperatorType.And,
                                                                     new BinaryOperator("Code", _locProjectHeaderCode)));
                                    }

                                    if (_locProjectHeaderXPO != null)
                                    {
                                        XPCollection<ProjectLine> _numLines = new XPCollection<ProjectLine>
                                                                            (_currSession, new GroupOperator(GroupOperatorType.And,
                                                                            new BinaryOperator("ProjectHeader", _locProjectHeaderXPO)),
                                                                            new SortProperty("No", DevExpress.Xpo.DB.SortingDirection.Ascending));

                                        if (_numLines != null && _numLines.Count > 0)
                                        {
                                            foreach (ProjectLine _numLine in _numLines)
                                            {
                                                if (_numLine.No == _numLines.Count())
                                                {
                                                    _locProjectLineOS.Title = _numLine.Title;
                                                    _locProjectLineOS.Title2 = _numLine.Title2;
                                                    _locProjectLineOS.Title3 = _numLine.Title3;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ErrorMessageShow("Data Project Line Not Available");
                                }
                            }
                            else
                            {
                                ErrorMessageShow("Data Project Line Not Available");
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
                Tracing.Tracer.LogError(" BusinessObject = ProjectLine " + ex.ToString());
            }

        }

        private void ProjectLineMirrorAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                GlobalFunction _globFunc = new GlobalFunction();
                IObjectSpace _objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
                ArrayList _objectsToProcess = new ArrayList(e.SelectedObjects);
                DateTime now = DateTime.Now;
                Session _currSession = null;
                string _currObjectId = null;
                ProjectHeader _locProjectHeaderXPO = null;
                string _locProjectHeaderCode = null;

                if (this.ObjectSpace != null)
                {
                    _currSession = ((XPObjectSpace)this.ObjectSpace).Session;
                }

                if (_objectsToProcess != null)
                {
                    foreach (Object obj in _objectsToProcess)
                    {
                        ProjectLine _locProjectLineOS = (ProjectLine)_objectSpace.GetObject(obj);


                        if (_locProjectLineOS != null)
                        {
                            if (_locProjectLineOS.ProjectHeader != null)
                            {
                                if (_locProjectLineOS.ProjectHeader.Code != null)
                                {
                                    _locProjectHeaderCode = _locProjectLineOS.ProjectHeader.Code;
                                }
                            }

                            if (_locProjectLineOS.Code != null)
                            {

                                if (_locProjectHeaderCode != null)
                                {
                                    _locProjectHeaderXPO = _currSession.FindObject<ProjectHeader>
                                                                (new GroupOperator(GroupOperatorType.And,
                                                                 new BinaryOperator("Code", _locProjectHeaderCode)));

                                    if (_locProjectHeaderXPO != null)
                                    {
                                        _currObjectId = _locProjectLineOS.Code;

                                        ProjectLine _locProjectLineXPO = _currSession.FindObject<ProjectLine>
                                                                            (new GroupOperator(GroupOperatorType.And,
                                                                             new BinaryOperator("Code", _currObjectId),
                                                                             new BinaryOperator("ProjectHeader", _locProjectHeaderXPO)));


                                        if (_locProjectLineXPO != null)
                                        {
                                            XPCollection<ProjectLineItem> _numProjectLineItems = new XPCollection<ProjectLineItem>
                                                                                (_currSession, new GroupOperator(GroupOperatorType.And,
                                                                                new BinaryOperator("ProjectHeader", _locProjectHeaderXPO),
                                                                                new BinaryOperator("ProjectLine", _locProjectLineXPO)),
                                                                                new SortProperty("No", DevExpress.Xpo.DB.SortingDirection.Ascending));

                                            if (_numProjectLineItems != null && _numProjectLineItems.Count > 0)
                                            {
                                                foreach (ProjectLineItem _numProjectLineItem in _numProjectLineItems)
                                                {

                                                    if (_numProjectLineItem.Item != null && (_numProjectLineItem.Status == Status.Open || _numProjectLineItem.Status == Status.Progress || _numProjectLineItem.Status == Status.Lock))
                                                    {
                                                        #region Mirror ProjectLineItem2
                                                        XPCollection<ItemComponent> _locItemComponents = new XPCollection<ItemComponent>(_currSession, new GroupOperator(GroupOperatorType.And,
                                                                            new BinaryOperator("Item", _numProjectLineItem.Item),
                                                                            new BinaryOperator("Active", true)));

                                                        if (_locItemComponents != null && _locItemComponents.Count > 0)
                                                        {
                                                            foreach (ItemComponent _locItemComponent in _locItemComponents)
                                                            {
                                                                ProjectLineItem2 _locProjectLineItem2 = _currSession.FindObject<ProjectLineItem2>(
                                                                                new GroupOperator(GroupOperatorType.And,
                                                                                new BinaryOperator("ProjectHeader", _locProjectHeaderXPO),
                                                                                new BinaryOperator("ProjectLine", _locProjectLineXPO),
                                                                                new BinaryOperator("ProjectLineItem", _numProjectLineItem),
                                                                                new BinaryOperator("Item", _locItemComponent.ItemComp)
                                                                                ));

                                                                if (_locProjectLineItem2 == null)
                                                                {
                                                                    if(_numProjectLineItem.Item == _locItemComponent.ItemComp)
                                                                    {
                                                                        ProjectLineItem2 _saveDataPLI2 = new ProjectLineItem2(_currSession)
                                                                        {
                                                                            Item = _numProjectLineItem.Item,
                                                                            DUOM = _numProjectLineItem.DUOM,
                                                                            DQty = _numProjectLineItem.DQty,
                                                                            TQty = _numProjectLineItem.TQty,
                                                                            ProjectHeader = _locProjectHeaderXPO,
                                                                            ProjectLine = _locProjectLineXPO,
                                                                            ProjectLineItem = _numProjectLineItem
                                                                        };
                                                                        _saveDataPLI2.Save();
                                                                        _saveDataPLI2.Session.CommitTransaction();
                                                                    }else
                                                                    {
                                                                        ProjectLineItem2 _saveDataPLI2 = new ProjectLineItem2(_currSession)
                                                                        {
                                                                            Item = _locItemComponent.ItemComp,
                                                                            DUOM = _locItemComponent.UOM,
                                                                            DQty = _locItemComponent.Qty,
                                                                            ProjectHeader = _locProjectHeaderXPO,
                                                                            ProjectLine = _locProjectLineXPO,
                                                                            ProjectLineItem = _numProjectLineItem
                                                                        };
                                                                        _saveDataPLI2.Save();
                                                                        _saveDataPLI2.Session.CommitTransaction();
                                                                    }
                                                                    
                                                                }
                                                            }

                                                        }
                                                        else
                                                        {
                                                            ProjectLineItem2 _locProjectLineItem2 = _currSession.FindObject<ProjectLineItem2>(
                                                                                new GroupOperator(GroupOperatorType.And,
                                                                                new BinaryOperator("ProjectHeader", _locProjectHeaderXPO),
                                                                                new BinaryOperator("ProjectLine", _locProjectLineXPO),
                                                                                new BinaryOperator("ProjectLineItem", _numProjectLineItem),
                                                                                new BinaryOperator("Item", _numProjectLineItem.Item)
                                                                                ));

                                                            if (_locProjectLineItem2 != null)
                                                            {
                                                                _locProjectLineItem2.Name = _numProjectLineItem.Name;
                                                                _locProjectLineItem2.Item = _numProjectLineItem.Item;
                                                                _locProjectLineItem2.DQty = _numProjectLineItem.DQty;
                                                                _locProjectLineItem2.DUOM = _numProjectLineItem.DUOM;
                                                                _locProjectLineItem2.Qty = _numProjectLineItem.Qty;
                                                                _locProjectLineItem2.UOM = _numProjectLineItem.UOM;
                                                                _locProjectLineItem2.TQty = _numProjectLineItem.TQty;
                                                                _locProjectLineItem2.Description = _numProjectLineItem.Description;
                                                                _locProjectLineItem2.ProjectHeader = _locProjectHeaderXPO;
                                                                _locProjectLineItem2.ProjectLine = _locProjectLineXPO;
                                                                _locProjectLineItem2.Save();
                                                                _locProjectLineItem2.Session.CommitTransaction();
                                                            }
                                                            else
                                                            {
                                                                ProjectLineItem2 _saveDataPLI2 = new ProjectLineItem2(_currSession)
                                                                {
                                                                    Item = _numProjectLineItem.Item,
                                                                    DUOM = _numProjectLineItem.DUOM,
                                                                    DQty = _numProjectLineItem.DQty,
                                                                    Qty = _numProjectLineItem.Qty,
                                                                    UOM = _numProjectLineItem.UOM,
                                                                    TQty = _numProjectLineItem.TQty,
                                                                    ProjectHeader = _locProjectHeaderXPO,
                                                                    ProjectLine = _locProjectLineXPO,
                                                                    ProjectLineItem = _numProjectLineItem
                                                                };
                                                                _saveDataPLI2.Save();
                                                                _saveDataPLI2.Session.CommitTransaction();
                                                            }
                                                        }
                                                        #endregion Mirror ProjectLineItem2

                                                        #region Mirror ProjectLineServices
                                                        ProjectLineService _locProjectLineService = _currSession.FindObject<ProjectLineService>
                                                                                                   (new GroupOperator(GroupOperatorType.And,
                                                                                                   new BinaryOperator("ProjectHeader", _locProjectHeaderXPO),
                                                                                                   new BinaryOperator("ProjectLine", _locProjectLineXPO),
                                                                                                   new BinaryOperator("ProjectLineItem", _numProjectLineItem)));
                                                        if(_locProjectLineService != null)
                                                        {
                                                            _locProjectLineService.Qty = _numProjectLineItem.TQty;
                                                            _locProjectLineService.UOM = _numProjectLineItem.DUOM;
                                                            _locProjectLineService.ProjectHeader = _locProjectHeaderXPO;
                                                            _locProjectLineService.ProjectLine = _locProjectLineXPO;
                                                            _locProjectLineService.ProjectLineItem = _numProjectLineItem;
                                                        }
                                                        if(_locProjectLineService == null)
                                                        {
                                                            ProjectLineService _saveDataPLS = new ProjectLineService(_currSession)
                                                            {
                                                                Qty = _numProjectLineItem.TQty,
                                                                UOM = _numProjectLineItem.DUOM,
                                                                ProjectHeader = _locProjectHeaderXPO,
                                                                ProjectLine = _locProjectLineXPO,
                                                                ProjectLineItem = _numProjectLineItem
                                                            };
                                                            _saveDataPLS.Save();
                                                            _saveDataPLS.Session.CommitTransaction();
                                                        }
                                                        #endregion Mirror ProjectLineServices
                                                    }
                                                }
                                            }
                                        }
                                        SuccessMessageShow("Project Line Item Has Successfully Mirror to Project Line Item 2");
                                    }
                                }
                                else
                                {
                                    ErrorMessageShow("Data Project Line Not Available");
                                }
                            }
                            else
                            {
                                ErrorMessageShow("Data Project Line Not Available");
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
                Tracing.Tracer.LogError(" BusinessObject = ProjectLine " + ex.ToString());
            }
        }

        #region Global Method

        private void SuccessMessageShow(string _locActionName)
        {
            MessageOptions options = new MessageOptions();
            options.Duration = 2000;
            options.Message = string.Format("{0}", _locActionName);
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
