namespace MEI.Module.Controllers
{
    partial class PurchaseRequisitionActionController
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
            this.PurchaseRequisitionProgressAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PurchaseRequisitionPurchaseOrderAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // PurchaseRequisitionProgressAction
            // 
            this.PurchaseRequisitionProgressAction.Caption = "Progress";
            this.PurchaseRequisitionProgressAction.ConfirmationMessage = null;
            this.PurchaseRequisitionProgressAction.Id = "PurchaseRequisitionProgressActionId";
            this.PurchaseRequisitionProgressAction.TargetObjectType = typeof(MEI.Module.BusinessObjects.PurchaseRequisition);
            this.PurchaseRequisitionProgressAction.ToolTip = null;
            this.PurchaseRequisitionProgressAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PurchaseRequisitionProgressAction_Execute);
            // 
            // PurchaseRequisitionPurchaseOrderAction
            // 
            this.PurchaseRequisitionPurchaseOrderAction.Caption = "PO";
            this.PurchaseRequisitionPurchaseOrderAction.ConfirmationMessage = null;
            this.PurchaseRequisitionPurchaseOrderAction.Id = "PurchaseRequisitionPurchaseOrderActionId";
            this.PurchaseRequisitionPurchaseOrderAction.TargetObjectType = typeof(MEI.Module.BusinessObjects.PurchaseRequisition);
            this.PurchaseRequisitionPurchaseOrderAction.ToolTip = null;
            this.PurchaseRequisitionPurchaseOrderAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PurchaseRequisitionPurchaseOrderAction_Execute);
            // 
            // PurchaseRequisitionActionController
            // 
            this.Actions.Add(this.PurchaseRequisitionProgressAction);
            this.Actions.Add(this.PurchaseRequisitionPurchaseOrderAction);
            this.TargetObjectType = typeof(MEI.Module.BusinessObjects.PurchaseRequisition);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction PurchaseRequisitionProgressAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PurchaseRequisitionPurchaseOrderAction;
    }
}
