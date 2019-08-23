namespace MEI.Module.Controllers
{
    partial class ProjectHeaderActionController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ProjectHeaderPrePOAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ProjectHeaderProgressAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ProjectHeaderLockAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ProjectHeaderOrderAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ProjectHeaderPrePOProgressAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ProjectHeaderPrePOAction
            // 
            this.ProjectHeaderPrePOAction.Caption = "Pre PO Posting";
            this.ProjectHeaderPrePOAction.ConfirmationMessage = null;
            this.ProjectHeaderPrePOAction.Id = "ProjectHeaderPrePOActionId";
            this.ProjectHeaderPrePOAction.TargetObjectType = typeof(MEI.Module.BusinessObjects.ProjectHeader);
            this.ProjectHeaderPrePOAction.ToolTip = null;
            this.ProjectHeaderPrePOAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ProjectHeaderPrePOAction_Execute);
            // 
            // ProjectHeaderProgressAction
            // 
            this.ProjectHeaderProgressAction.Caption = "Progress";
            this.ProjectHeaderProgressAction.ConfirmationMessage = null;
            this.ProjectHeaderProgressAction.Id = "ProjectHeaderProgressActionId";
            this.ProjectHeaderProgressAction.TargetObjectType = typeof(MEI.Module.BusinessObjects.ProjectHeader);
            this.ProjectHeaderProgressAction.ToolTip = null;
            this.ProjectHeaderProgressAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ProjectHeaderProgressAction_Execute);
            // 
            // ProjectHeaderLockAction
            // 
            this.ProjectHeaderLockAction.Caption = "Lock";
            this.ProjectHeaderLockAction.ConfirmationMessage = null;
            this.ProjectHeaderLockAction.Id = "ProjectHeaderLockActionId";
            this.ProjectHeaderLockAction.ToolTip = null;
            this.ProjectHeaderLockAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ProjectHeaderLockAction_Execute);
            // 
            // ProjectHeaderOrderAction
            // 
            this.ProjectHeaderOrderAction.Caption = "Order";
            this.ProjectHeaderOrderAction.ConfirmationMessage = null;
            this.ProjectHeaderOrderAction.Id = "ProjectHeaderOrderActionId";
            this.ProjectHeaderOrderAction.TargetObjectType = typeof(MEI.Module.BusinessObjects.ProjectHeader);
            this.ProjectHeaderOrderAction.ToolTip = null;
            this.ProjectHeaderOrderAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ProjectHeaderOrderAction_Execute);
            // 
            // ProjectHeaderPrePOProgressAction
            // 
            this.ProjectHeaderPrePOProgressAction.Caption = "Pre PO Progress";
            this.ProjectHeaderPrePOProgressAction.ConfirmationMessage = null;
            this.ProjectHeaderPrePOProgressAction.Id = "ProjectHeaderPrePOProgressActionId";
            this.ProjectHeaderPrePOProgressAction.TargetObjectType = typeof(MEI.Module.BusinessObjects.ProjectHeader);
            this.ProjectHeaderPrePOProgressAction.ToolTip = null;
            this.ProjectHeaderPrePOProgressAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ProjectHeaderPrePOProgressAction_Execute);
            // 
            // ProjectHeaderActionController
            // 
            this.Actions.Add(this.ProjectHeaderPrePOAction);
            this.Actions.Add(this.ProjectHeaderProgressAction);
            this.Actions.Add(this.ProjectHeaderLockAction);
            this.Actions.Add(this.ProjectHeaderOrderAction);
            this.Actions.Add(this.ProjectHeaderPrePOProgressAction);
            this.TargetObjectType = typeof(MEI.Module.BusinessObjects.ProjectHeader);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ProjectHeaderPrePOAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ProjectHeaderProgressAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ProjectHeaderLockAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ProjectHeaderOrderAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ProjectHeaderPrePOProgressAction;
    }
}
