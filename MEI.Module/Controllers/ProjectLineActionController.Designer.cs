namespace MEI.Module.Controllers
{
    partial class ProjectLineActionController
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
            this.ProjectLineGetPreviousAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ProjectLineMirrorAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ProjectLineGetPreviousAction
            // 
            this.ProjectLineGetPreviousAction.Caption = "Get Previous Data";
            this.ProjectLineGetPreviousAction.ConfirmationMessage = null;
            this.ProjectLineGetPreviousAction.Id = "ProjectLineGetPreviousActionId";
            this.ProjectLineGetPreviousAction.TargetObjectType = typeof(MEI.Module.BusinessObjects.ProjectLine);
            this.ProjectLineGetPreviousAction.ToolTip = null;
            this.ProjectLineGetPreviousAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ProjectLineGetPreviousAction_Execute);
            // 
            // ProjectLineMirrorAction
            // 
            this.ProjectLineMirrorAction.Caption = "Mirror";
            this.ProjectLineMirrorAction.ConfirmationMessage = null;
            this.ProjectLineMirrorAction.Id = "ProjectLineMirrorActionId";
            this.ProjectLineMirrorAction.ToolTip = null;
            this.ProjectLineMirrorAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ProjectLineMirrorAction_Execute);
            // 
            // ProjectLineActionController
            // 
            this.Actions.Add(this.ProjectLineGetPreviousAction);
            this.Actions.Add(this.ProjectLineMirrorAction);
            this.TargetObjectType = typeof(MEI.Module.BusinessObjects.ProjectLine);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ProjectLineGetPreviousAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ProjectLineMirrorAction;
    }
}
