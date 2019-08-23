namespace MEI.Module.Controllers
{
    partial class InventoryTransferActionController
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
            this.InventoryTransferProgressAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.InventoryTransferReceiveAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // InventoryTransferProgressAction
            // 
            this.InventoryTransferProgressAction.Caption = "Progress";
            this.InventoryTransferProgressAction.ConfirmationMessage = null;
            this.InventoryTransferProgressAction.Id = "InventoryTransferProgressActionId";
            this.InventoryTransferProgressAction.ToolTip = null;
            this.InventoryTransferProgressAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.InventoryTransferProgressAction_Execute);
            // 
            // InventoryTransferReceiveAction
            // 
            this.InventoryTransferReceiveAction.Caption = "Receive";
            this.InventoryTransferReceiveAction.ConfirmationMessage = null;
            this.InventoryTransferReceiveAction.Id = "InventoryTransferReceiveActionId";
            this.InventoryTransferReceiveAction.TargetObjectType = typeof(MEI.Module.BusinessObjects.InventoryTransfer);
            this.InventoryTransferReceiveAction.ToolTip = null;
            this.InventoryTransferReceiveAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.InventoryTransferReceiveAction_Execute);
            // 
            // InventoryTransferActionController
            // 
            this.Actions.Add(this.InventoryTransferProgressAction);
            this.Actions.Add(this.InventoryTransferReceiveAction);
            this.TargetObjectType = typeof(MEI.Module.BusinessObjects.InventoryTransfer);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction InventoryTransferProgressAction;
        private DevExpress.ExpressApp.Actions.SimpleAction InventoryTransferReceiveAction;
    }
}
