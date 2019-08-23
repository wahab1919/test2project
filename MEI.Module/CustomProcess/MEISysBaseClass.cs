using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEI.Module.CustomProcess
{
    class MEISysBaseClass
    {
    }

    public enum NumberingType
    {
        Documents = 0,
        Objects = 1,
        Other = 2
    }

    public enum ObjectList
    {
        None = 0,
        Address = 1,
        ApplicationImport = 2,
        ApplicationSetup = 3,
        ApplicationSetupDetail = 4,
        BeginingInventory = 5,
        BinLocation = 6,
        Brand = 7,
        BusinessPartner = 8,
        City = 9,
        Company = 10,
        Country = 11,
        Department = 12,
        Division = 13,
        DocumentType = 14,
        Employee = 15,
        InventoryJournal = 16,
        InventoryTransfer = 17,
        InventoryTransferLine = 18,
        Item = 19,
        ItemComponent = 20,
        ItemType = 21,
        ItemUnitOfMeasure = 22,
        ListImport = 23,
        Location = 24,
        NumberingHeader = 25,
        NumberingLine = 26,
        Position = 27,
        PrePurchaseOrder = 28,
        ProjectHeader = 29,
        ProjectLine = 30,
        ProjectLineItem = 31,
        ProjectLineItem2 = 32,
        ProjectLineService = 33,
        PurchaseOrder = 34,
        PurchaseOrderLine = 35,
        PurchaseRequisition = 36,
        PurchaseRequisitionLine = 37,
        SalesQuotation = 38,
        SalesQuotationLine = 39,
        Section = 40,
        StockTransfer = 41,
        StockTransferLine = 42,
        Tax = 43,
        TaxType = 44,
        TermOfPayment = 45,
        UnitOfMeasure = 46,
        UnitOfMeasureType = 47,
        UserAccess = 48,
        UserAccessRole = 49
    }

    public enum BusinessPartnerType
    {
        None = 0,
        Customer = 1,
        Vendor = 2,
        All = 3
    }

    public enum Status
    {
        None = 0,
        Open = 1,
        Progress = 2,
        Posted = 3,
        Close = 4,
        Lock = 5
    }

    public enum OrderType
    {
        None = 0,
        Item = 1,
        FixedAsset = 2,
        Account = 3,
    }

    public enum DirectionType
    {
        None = 0,
        Internal = 1,
        External = 2,
    }

    public enum TaxNature
    {
        None = 0,
        Increase = 1,
        Decrease = 2,
    }

    public enum InventoryMovingType
    {
        None = 0,
        Receive = 1,
        Issue = 2,
        Transfer = 3,
        Adjust = 4,
        Return = 5,
    }

    public enum DocumentRule
    {
        None = 0,
        Customer = 1,
        Vendor = 2
    }

    public enum StockType
    {
        None = 0,
        Good = 1,
        Bad = 2
    }

    public enum DirectionRule
    {
        None = 0,
        Direct = 1,
        Indirect = 2
    }
}
