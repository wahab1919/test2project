namespace MEI.Module.Controllers
{
    partial class PurchaseOrderActionController
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
            this.PurchaseOrderReceiptAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PurchaseOrderProgressAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // PurchaseOrderReceiptAction
            // 
            this.PurchaseOrderReceiptAction.Caption = "Receipt";
            this.PurchaseOrderReceiptAction.ConfirmationMessage = null;
            this.PurchaseOrderReceiptAction.Id = "PurchaseOrderReceiptActionId";
            this.PurchaseOrderReceiptAction.TargetObjectType = typeof(MEI.Module.BusinessObjects.PurchaseOrder);
            this.PurchaseOrderReceiptAction.ToolTip = null;
            this.PurchaseOrderReceiptAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PurchaseOrderReceiptAction_Execute);
            // 
            // PurchaseOrderProgressAction
            // 
            this.PurchaseOrderProgressAction.Caption = "Progress";
            this.PurchaseOrderProgressAction.ConfirmationMessage = null;
            this.PurchaseOrderProgressAction.Id = "PurchaseOrderProgressActionId";
            this.PurchaseOrderProgressAction.ToolTip = null;
            this.PurchaseOrderProgressAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PurchaseOrderProgressAction_Execute);
            // 
            // PurchaseOrderActionController
            // 
            this.Actions.Add(this.PurchaseOrderReceiptAction);
            this.Actions.Add(this.PurchaseOrderProgressAction);
            this.TargetObjectType = typeof(MEI.Module.BusinessObjects.PurchaseOrder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction PurchaseOrderReceiptAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PurchaseOrderProgressAction;
    }
}
