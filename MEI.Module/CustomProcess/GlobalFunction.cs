using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using MEI.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using System.Reflection;
using Excel;
using System.Data;
using System.IO;

namespace MEI.Module.CustomProcess
{
    class GlobalFunction
    {
        #region Numbering

        public string GetNumberingUnlockOptimisticRecord(IDataLayer _currIDataLayer, ObjectList _currObject)
        {
            string _result = null;
            try
            {
                Session _generatorSession = new Session(_currIDataLayer);

                ApplicationSetup _appSetup = _generatorSession.FindObject<ApplicationSetup>(new GroupOperator(GroupOperatorType.And,
                                            new BinaryOperator("Active", true), new BinaryOperator("DefaultSystem", true)));
                if (_appSetup != null)
                {
                    NumberingHeader _numberingHeader = _generatorSession.FindObject<NumberingHeader>
                                                        (new GroupOperator(GroupOperatorType.And,
                                                         new BinaryOperator("NumberingType", NumberingType.Objects),
                                                         new BinaryOperator("ApplicationSetup", _appSetup, BinaryOperatorType.Equal),
                                                         new BinaryOperator("Active", true)));
                    if (_numberingHeader != null)
                    {
                        NumberingLine _numberingLine = _generatorSession.FindObject<NumberingLine>
                                                    (new GroupOperator(GroupOperatorType.And,
                                                     new BinaryOperator("ObjectList", _currObject),
                                                     new BinaryOperator("Default", true),
                                                     new BinaryOperator("Active", true),
                                                     new BinaryOperator("NumberingHeader", _numberingHeader)));
                        if (_numberingLine != null)
                        {
                            _numberingLine.LastValue = _numberingLine.LastValue + _numberingLine.IncrementValue;
                            _numberingLine.Save();
                            _result = _numberingLine.FormatedValue;
                            _numberingLine.Session.CommitTransaction();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" Module = GlobalFunction " + ex.ToString());
            }
            return _result;
        }

        public string GetDocumentNumberingUnlockOptimisticRecord(IDataLayer _currIDataLayer, DocumentType _currDocType)
        {
            string _result = null;
            try
            {
                Session _generatorSession = new Session(_currIDataLayer);

                ApplicationSetup _appSetup = _generatorSession.FindObject<ApplicationSetup>(new GroupOperator(GroupOperatorType.And,
                                            new BinaryOperator("Active", true), new BinaryOperator("DefaultSystem", true)));
                if (_appSetup != null)
                {
                    NumberingHeader _numberingHeader = _generatorSession.FindObject<NumberingHeader>
                                                        (new GroupOperator(GroupOperatorType.And,
                                                         new BinaryOperator("NumberingType", NumberingType.Documents),
                                                         new BinaryOperator("ApplicationSetup", _appSetup, BinaryOperatorType.Equal),
                                                         new BinaryOperator("Active", true)));

                    if (_numberingHeader != null)
                    {
                        NumberingLine _numberingLine = _generatorSession.FindObject<NumberingLine>
                                                    (new GroupOperator(GroupOperatorType.And,
                                                     new BinaryOperator("DocumentType.Oid", _currDocType.Oid),
                                                     new BinaryOperator("Active", true),
                                                     new BinaryOperator("NumberingHeader", _numberingHeader)));
                        if (_numberingLine != null)
                        {
                            _numberingLine.LastValue = _numberingLine.LastValue + _numberingLine.IncrementValue;
                            _numberingLine.Save();
                            _result = _numberingLine.FormatedValue;
                            _numberingLine.Session.CommitTransaction();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" Module = GlobalFunction " + ex.ToString());
            }
            return _result;
        }

        public string GetNumberingSelectionUnlockOptimisticRecord(IDataLayer _currIDataLayer, ObjectList _currObject, NumberingLine _currNumberingLine)
        {
            string _result = null;
            try
            {
                Session _generatorSession = new Session(_currIDataLayer);

                ApplicationSetup _appSetup = _generatorSession.FindObject<ApplicationSetup>(new GroupOperator(GroupOperatorType.And,
                                            new BinaryOperator("Active", true), new BinaryOperator("DefaultSystem", true)));
                if (_appSetup != null)
                {
                    NumberingHeader _numberingHeader = _generatorSession.FindObject<NumberingHeader>
                                                        (new GroupOperator(GroupOperatorType.And,
                                                         new BinaryOperator("NumberingType", NumberingType.Objects),
                                                         new BinaryOperator("ApplicationSetup", _appSetup, BinaryOperatorType.Equal),
                                                         new BinaryOperator("Active", true)));
                    if (_numberingHeader != null)
                    {
                        NumberingLine _numberingLine = _generatorSession.FindObject<NumberingLine>
                                                    (new GroupOperator(GroupOperatorType.And,
                                                     new BinaryOperator("Oid", _currNumberingLine.Oid),
                                                     new BinaryOperator("ObjectList", _currObject),
                                                     new BinaryOperator("Selection", true),
                                                     new BinaryOperator("Active", true),
                                                     new BinaryOperator("NumberingHeader", _numberingHeader)));
                        if (_numberingLine != null)
                        {
                            _numberingLine.LastValue = _numberingLine.LastValue + _numberingLine.IncrementValue;
                            _numberingLine.Save();
                            _result = _numberingLine.FormatedValue;
                            _numberingLine.Session.CommitTransaction();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" Module = GlobalFunction " + ex.ToString());
            }
            return _result;
        }

        public string GetNumberingSignUnlockOptimisticRecord(IDataLayer _currIDataLayer, ObjectList _currObject)
        {
            string _result = null;
            try
            {
                Session _generatorSession = new Session(_currIDataLayer);

                ApplicationSetup _appSetup = _generatorSession.FindObject<ApplicationSetup>(new GroupOperator(GroupOperatorType.And,
                                            new BinaryOperator("Active", true), new BinaryOperator("DefaultSystem", true)));
                if (_appSetup != null)
                {
                    NumberingHeader _numberingHeader = _generatorSession.FindObject<NumberingHeader>
                                                        (new GroupOperator(GroupOperatorType.And,
                                                         new BinaryOperator("NumberingType", NumberingType.Objects),
                                                         new BinaryOperator("ApplicationSetup", _appSetup, BinaryOperatorType.Equal),
                                                         new BinaryOperator("Active", true)));
                    if (_numberingHeader != null)
                    {
                        NumberingLine _numberingLine = _generatorSession.FindObject<NumberingLine>
                                                    (new GroupOperator(GroupOperatorType.And,
                                                     new BinaryOperator("ObjectList", _currObject),
                                                     new BinaryOperator("Sign", true),
                                                     new BinaryOperator("Active", true),
                                                     new BinaryOperator("NumberingHeader", _numberingHeader)));
                        if (_numberingLine != null)
                        {
                            _numberingLine.LastValue = _numberingLine.LastValue + _numberingLine.IncrementValue;
                            _numberingLine.Save();
                            _result = _numberingLine.FormatedValue;
                            _numberingLine.Session.CommitTransaction();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" Module = GlobalFunction " + ex.ToString());
            }
            return _result;
        }

        #endregion Numbering

        #region ImportData

        public void ImportDataExcel(Session _currSession, string _fullPath, string _ext, ObjectList _objList)
        {
            PropertyInfo[] _propInfo = null;
            object _newObject = null;
            IExcelDataReader excelReader;
            DataSet ds = new DataSet();
            Boolean _dataBool;
            try
            {
                FileStream stream = File.Open(_fullPath, FileMode.Open, FileAccess.Read);
                if (_ext.ToLower() == ".xlsx")
                {
                    //1. Reading from a binary Excel file ('97-2003 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    //2. Reading from a OpenXml Excel file (2007 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }

                excelReader.IsFirstRowAsColumnNames = true;
                ds = excelReader.AsDataSet();
                DataTable dt = ds.Tables[0];
                _propInfo = GetPropInfo(_objList);
                excelReader.Close();

                if (_propInfo != null)
                {
                    foreach (DataRow dRow in dt.Rows)
                    {
                        _newObject = GetBusinessObject(_currSession, _objList);
                        for (int i = 0; i <= dRow.ItemArray.Length - 1; i++)
                        {
                            for (int j = 0; j <= _propInfo.Length - 1; j++)
                            {
                                if (_propInfo[j].Name.ToLower().Replace(" ", "") == dt.Columns[i].ToString().Trim().ToLower().Replace(" ", ""))
                                {
                                    #region DefaultProperty

                                    if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(string)))
                                    {
                                        _propInfo[j].SetValue(_newObject, dRow[i].ToString(), null);
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(bool)))
                                    {
                                        if (dRow[i].ToString().Trim().ToLower() == "Checked".ToLower())
                                        {
                                            _dataBool = true;
                                        }
                                        else if (dRow[i].ToString().Trim().ToLower() == "TRUE".ToLower())
                                        {
                                            _dataBool = true;
                                        }
                                        else
                                        {
                                            _dataBool = false;
                                        }
                                        _propInfo[j].SetValue(_newObject, _dataBool, null);
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(decimal)))
                                    {
                                        _propInfo[j].SetValue(_newObject, Convert.ToDecimal(dRow[i].ToString()), null);
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(int)))
                                    {
                                        _propInfo[j].SetValue(_newObject, Convert.ToInt32(dRow[i].ToString()), null);
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(double)))
                                    {
                                        _propInfo[j].SetValue(_newObject, Convert.ToDouble(dRow[i].ToString()), null);
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(System.DateTime)))
                                    {
                                        if (CheckDate(dRow[i].ToString()))
                                        {
                                            _propInfo[j].SetValue(_newObject, Convert.ToDateTime(dRow[i].ToString()), null);
                                        }
                                    }

                                    #endregion DefaultProperty

                                    #region For Enum

                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(ObjectList)))
                                    {
                                        _propInfo[j].SetValue(_newObject, GetObjectList(dRow[i].ToString()), null);
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(DirectionType)))
                                    {
                                        _propInfo[j].SetValue(_newObject, GetDirectionType(dRow[i].ToString()), null);
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(InventoryMovingType)))
                                    {
                                        _propInfo[j].SetValue(_newObject, GetInventoryMovingType(dRow[i].ToString()), null);
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(DocumentRule)))
                                    {
                                        _propInfo[j].SetValue(_newObject, GetDocumentRule(dRow[i].ToString()), null);
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(StockType)))
                                    {
                                        _propInfo[j].SetValue(_newObject, GetStockType(dRow[i].ToString()), null);
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(BusinessPartnerType)))
                                    {
                                        _propInfo[j].SetValue(_newObject, GetBusinessPartnerType(dRow[i].ToString()), null);
                                    }

                                    #endregion For Enum

                                    #region For Object
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(NumberingHeader)))
                                    {
                                        if (dRow[i].ToString() != null)
                                        {
                                            NumberingHeader _lineData = _currSession.FindObject<NumberingHeader>
                                                (new BinaryOperator("Name", dRow[i].ToString()));
                                            if (_lineData != null)
                                            {
                                                _propInfo[j].SetValue(_newObject, _lineData, null);
                                            }
                                        }
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(DocumentType)))
                                    {
                                        if (dRow[i].ToString() != null)
                                        {
                                            DocumentType _lineData = _currSession.FindObject<DocumentType>
                                                (new BinaryOperator("Code", dRow[i].ToString()));
                                            if (_lineData != null)
                                            {
                                                _propInfo[j].SetValue(_newObject, _lineData, null);
                                            }
                                        }
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(Country)))
                                    {
                                        if (dRow[i].ToString() != null)
                                        {
                                            Country _lineData = _currSession.FindObject<Country>
                                                (new BinaryOperator("Name", dRow[i].ToString()));
                                            if (_lineData != null)
                                            {
                                                _propInfo[j].SetValue(_newObject, _lineData, null);
                                            }
                                        }
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(City)))
                                    {
                                        if (dRow[i].ToString() != null)
                                        {
                                            City _lineData = _currSession.FindObject<City>
                                                (new BinaryOperator("Name", dRow[i].ToString()));
                                            if (_lineData != null)
                                            {
                                                _propInfo[j].SetValue(_newObject, _lineData, null);
                                            }
                                        }
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(Location)))
                                    {
                                        if (dRow[i].ToString() != null)
                                        {
                                            Location _lineData = _currSession.FindObject<Location>
                                                (new BinaryOperator("FullName", dRow[i].ToString()));
                                            if (_lineData != null)
                                            {
                                                _propInfo[j].SetValue(_newObject, _lineData, null);
                                            }
                                        }
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(UnitOfMeasureType)))
                                    {
                                        if (dRow[i].ToString() != null)
                                        {
                                            UnitOfMeasureType _lineData = _currSession.FindObject<UnitOfMeasureType>
                                                (new BinaryOperator("Name", dRow[i].ToString()));
                                            if (_lineData != null)
                                            {
                                                _propInfo[j].SetValue(_newObject, _lineData, null);
                                            }
                                        }
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(UnitOfMeasure)))
                                    {
                                        if (dRow[i].ToString() != null)
                                        {
                                            UnitOfMeasure _lineData = _currSession.FindObject<UnitOfMeasure>
                                                (new BinaryOperator("FullName", dRow[i].ToString()));
                                            if (_lineData != null)
                                            {
                                                _propInfo[j].SetValue(_newObject, _lineData, null);
                                            }
                                        }
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(ItemType)))
                                    {
                                        if (dRow[i].ToString() != null)
                                        {
                                            ItemType _lineData = _currSession.FindObject<ItemType>
                                                (new BinaryOperator("Name", dRow[i].ToString()));
                                            if (_lineData != null)
                                            {
                                                _propInfo[j].SetValue(_newObject, _lineData, null);
                                            }
                                        }
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(Item)))
                                    {
                                        if (dRow[i].ToString() != null)
                                        {
                                            Item _lineData = _currSession.FindObject<Item>
                                                (new BinaryOperator("Name", dRow[i].ToString()));
                                            if (_lineData != null)
                                            {
                                                _propInfo[j].SetValue(_newObject, _lineData, null);
                                            }
                                        }
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(TermOfPayment)))
                                    {
                                        if (dRow[i].ToString() != null)
                                        {
                                            TermOfPayment _lineData = _currSession.FindObject<TermOfPayment>
                                                (new BinaryOperator("Name", dRow[i].ToString()));
                                            if (_lineData != null)
                                            {
                                                _propInfo[j].SetValue(_newObject, _lineData, null);
                                            }
                                        }
                                    }
                                    else if (object.ReferenceEquals(_propInfo[j].PropertyType, typeof(BusinessPartner)))
                                    {
                                        if (dRow[i].ToString() != null)
                                        {
                                            BusinessPartner _lineData = _currSession.FindObject<BusinessPartner>
                                                (new BinaryOperator("Name", dRow[i].ToString()));
                                            if (_lineData != null)
                                            {
                                                _propInfo[j].SetValue(_newObject, _lineData, null);
                                            }
                                        }
                                    }

                                    #endregion For Object
                                }
                            }
                        }
                        _currSession.Save(_newObject);
                        _currSession.CommitTransaction();
                    }
                    //_currSession.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" Module = GlobalFunction " + ex.ToString());
            }
        }

        private PropertyInfo[] GetPropInfo(ObjectList _objList)
        {
            PropertyInfo[] _result = null;
            try
            {
                if (_objList == ObjectList.Address)
                {
                    _result = typeof(Address).GetProperties();
                }
                else if (_objList == ObjectList.ApplicationImport)
                {
                    _result = typeof(ApplicationImport).GetProperties();
                }
                else if (_objList == ObjectList.ApplicationSetup)
                {
                    _result = typeof(ApplicationSetup).GetProperties();
                }
                else if (_objList == ObjectList.ApplicationSetupDetail)
                {
                    _result = typeof(ApplicationSetupDetail).GetProperties();
                }
                else if (_objList == ObjectList.BeginingInventory)
                {
                    _result = typeof(BeginingInventory).GetProperties();
                }
                else if (_objList == ObjectList.BinLocation)
                {
                    _result = typeof(BinLocation).GetProperties();
                }
                else if (_objList == ObjectList.Brand)
                {
                    _result = typeof(Brand).GetProperties();
                }
                else if (_objList == ObjectList.BusinessPartner)
                {
                    _result = typeof(BusinessPartner).GetProperties();
                }
                else if (_objList == ObjectList.City)
                {
                    _result = typeof(City).GetProperties();
                }
                else if (_objList == ObjectList.Company)
                {
                    _result = typeof(Company).GetProperties();
                }
                else if (_objList == ObjectList.Country)
                {
                    _result = typeof(Country).GetProperties();
                }
                else if (_objList == ObjectList.Department)
                {
                    _result = typeof(Department).GetProperties();
                }
                else if (_objList == ObjectList.Division)
                {
                    _result = typeof(Division).GetProperties();
                }
                else if (_objList == ObjectList.DocumentType)
                {
                    _result = typeof(DocumentType).GetProperties();
                }
                else if (_objList == ObjectList.Employee)
                {
                    _result = typeof(Employee).GetProperties();
                }
                else if (_objList == ObjectList.InventoryJournal)
                {
                    _result = typeof(InventoryJournal).GetProperties();
                }
                else if (_objList == ObjectList.InventoryTransfer)
                {
                    _result = typeof(InventoryTransfer).GetProperties();
                }
                else if (_objList == ObjectList.InventoryTransferLine)
                {
                    _result = typeof(InventoryTransferLine).GetProperties();
                }
                else if (_objList == ObjectList.Item)
                {
                    _result = typeof(Item).GetProperties();
                }
                else if (_objList == ObjectList.ItemComponent)
                {
                    _result = typeof(ItemComponent).GetProperties();
                }
                else if (_objList == ObjectList.ItemType)
                {
                    _result = typeof(ItemType).GetProperties();
                }
                else if (_objList == ObjectList.ItemUnitOfMeasure)
                {
                    _result = typeof(ItemUnitOfMeasure).GetProperties();
                }
                else if (_objList == ObjectList.ListImport)
                {
                    _result = typeof(ListImport).GetProperties();
                }
                else if (_objList == ObjectList.Location)
                {
                    _result = typeof(Location).GetProperties();
                }
                else if (_objList == ObjectList.NumberingHeader)
                {
                    _result = typeof(NumberingHeader).GetProperties();
                }
                else if (_objList == ObjectList.NumberingLine)
                {
                    _result = typeof(NumberingLine).GetProperties();
                }
                else if (_objList == ObjectList.Position)
                {
                    _result = typeof(Position).GetProperties();
                }
                else if (_objList == ObjectList.PrePurchaseOrder)
                {
                    _result = typeof(PrePurchaseOrder).GetProperties();
                }
                else if (_objList == ObjectList.ProjectHeader)
                {
                    _result = typeof(ProjectHeader).GetProperties();
                }
                else if (_objList == ObjectList.ProjectLine)
                {
                    _result = typeof(ProjectLine).GetProperties();
                }
                else if (_objList == ObjectList.ProjectLineItem)
                {
                    _result = typeof(ProjectLineItem).GetProperties();
                }
                else if (_objList == ObjectList.ProjectLineItem2)
                {
                    _result = typeof(ProjectLineItem2).GetProperties();
                }
                else if (_objList == ObjectList.ProjectLineService)
                {
                    _result = typeof(ProjectLineService).GetProperties();
                }
                else if (_objList == ObjectList.PurchaseOrder)
                {
                    _result = typeof(PurchaseOrder).GetProperties();
                }
                else if (_objList == ObjectList.PurchaseOrderLine)
                {
                    _result = typeof(PurchaseOrderLine).GetProperties();
                }
                else if (_objList == ObjectList.PurchaseRequisition)
                {
                    _result = typeof(PurchaseRequisition).GetProperties();
                }
                else if (_objList == ObjectList.PurchaseRequisitionLine)
                {
                    _result = typeof(PurchaseRequisitionLine).GetProperties();
                }
                else if (_objList == ObjectList.SalesQuotation)
                {
                    _result = typeof(SalesQuotation).GetProperties();
                }
                else if (_objList == ObjectList.SalesQuotationLine)
                {
                    _result = typeof(SalesQuotationLine).GetProperties();
                }
                else if (_objList == ObjectList.Section)
                {
                    _result = typeof(Section).GetProperties();
                }
                else if (_objList == ObjectList.StockTransfer)
                {
                    _result = typeof(StockTransfer).GetProperties();
                }
                else if (_objList == ObjectList.StockTransferLine)
                {
                    _result = typeof(StockTransferLine).GetProperties();
                }
                else if (_objList == ObjectList.Tax)
                {
                    _result = typeof(Tax).GetProperties();
                }
                else if (_objList == ObjectList.TaxType)
                {
                    _result = typeof(TaxType).GetProperties();
                }
                else if (_objList == ObjectList.TermOfPayment)
                {
                    _result = typeof(TermOfPayment).GetProperties();
                }
                else if (_objList == ObjectList.UnitOfMeasure)
                {
                    _result = typeof(UnitOfMeasure).GetProperties();
                }
                else if (_objList == ObjectList.UnitOfMeasureType)
                {
                    _result = typeof(UnitOfMeasureType).GetProperties();
                }
                else if (_objList == ObjectList.UserAccess)
                {
                    _result = typeof(UserAccess).GetProperties();
                }
                else if (_objList == ObjectList.UserAccessRole)
                {
                    _result = typeof(UserAccessRole).GetProperties();
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" Module = GlobalFunction " + ex.ToString());
            }
            return _result;
        }

        private object GetBusinessObject(Session _currSession, ObjectList _objList)
        {
            object _result = null;
            try
            {
                
                if (_objList == ObjectList.Address)
                {
                    _result = (Address)Activator.CreateInstance(typeof(Address), _currSession);
                }
                else if (_objList == ObjectList.ApplicationImport)
                {
                    _result = (ApplicationImport)Activator.CreateInstance(typeof(ApplicationImport), _currSession);
                }
                else if (_objList == ObjectList.ApplicationSetup)
                {
                    _result = (ApplicationSetup)Activator.CreateInstance(typeof(ApplicationSetup), _currSession);
                }
                else if (_objList == ObjectList.ApplicationSetupDetail)
                {
                    _result = (ApplicationSetupDetail)Activator.CreateInstance(typeof(ApplicationSetupDetail), _currSession);
                }
                else if (_objList == ObjectList.BusinessPartner)
                {
                    _result = (BusinessPartner)Activator.CreateInstance(typeof(BusinessPartner), _currSession);
                }
                else if (_objList == ObjectList.City)
                {
                    _result = (City)Activator.CreateInstance(typeof(City), _currSession);
                }
                else if (_objList == ObjectList.Company)
                {
                    _result = (Company)Activator.CreateInstance(typeof(Company), _currSession);
                }
                else if (_objList == ObjectList.Country)
                {
                    _result = (Country)Activator.CreateInstance(typeof(Country), _currSession);
                }
                else if (_objList == ObjectList.Department)
                {
                    _result = (Department)Activator.CreateInstance(typeof(Department), _currSession);
                }
                else if (_objList == ObjectList.Division)
                {
                    _result = (Division)Activator.CreateInstance(typeof(Division), _currSession);
                }
                else if (_objList == ObjectList.DocumentType)
                {
                    _result = (DocumentType)Activator.CreateInstance(typeof(DocumentType), _currSession);
                }
                else if (_objList == ObjectList.Employee)
                {
                    _result = (Employee)Activator.CreateInstance(typeof(Employee), _currSession);
                }
                else if (_objList == ObjectList.Item)
                {
                    _result = (Item)Activator.CreateInstance(typeof(Item), _currSession);
                }
                else if (_objList == ObjectList.ItemComponent)
                {
                    _result = (ItemComponent)Activator.CreateInstance(typeof(ItemComponent), _currSession);
                }
                else if (_objList == ObjectList.ItemUnitOfMeasure)
                {
                    _result = (ItemUnitOfMeasure)Activator.CreateInstance(typeof(ItemUnitOfMeasure), _currSession);
                }
                else if (_objList == ObjectList.ListImport)
                {
                    _result = (ListImport)Activator.CreateInstance(typeof(ListImport), _currSession);
                }
                else if (_objList == ObjectList.NumberingHeader)
                {
                    _result = (NumberingHeader)Activator.CreateInstance(typeof(NumberingHeader), _currSession);
                }
                else if (_objList == ObjectList.NumberingLine)
                {
                    _result = (NumberingLine)Activator.CreateInstance(typeof(NumberingLine), _currSession);
                }
                else if (_objList == ObjectList.Position)
                {
                    _result = (Position)Activator.CreateInstance(typeof(Position), _currSession);
                }
                else if (_objList == ObjectList.ProjectHeader)
                {
                    _result = (ProjectHeader)Activator.CreateInstance(typeof(ProjectHeader), _currSession);
                }
                else if (_objList == ObjectList.ProjectLine)
                {
                    _result = (ProjectLine)Activator.CreateInstance(typeof(ProjectLine), _currSession);
                }
                else if (_objList == ObjectList.ProjectLineItem)
                {
                    _result = (ProjectLineItem)Activator.CreateInstance(typeof(ProjectLineItem), _currSession);
                }
                else if (_objList == ObjectList.PurchaseOrder)
                {
                    _result = (PurchaseOrder)Activator.CreateInstance(typeof(PurchaseOrder), _currSession);
                }
                else if (_objList == ObjectList.PurchaseOrderLine)
                {
                    _result = (PurchaseOrderLine)Activator.CreateInstance(typeof(PurchaseOrderLine), _currSession);
                }
                else if (_objList == ObjectList.Section)
                {
                    _result = (Section)Activator.CreateInstance(typeof(Section), _currSession);
                }
                else if (_objList == ObjectList.TermOfPayment)
                {
                    _result = (TermOfPayment)Activator.CreateInstance(typeof(TermOfPayment), _currSession);
                }
                else if (_objList == ObjectList.UnitOfMeasure)
                {
                    _result = (UnitOfMeasure)Activator.CreateInstance(typeof(UnitOfMeasure), _currSession);
                }
                else if (_objList == ObjectList.UnitOfMeasureType)
                {
                    _result = (UnitOfMeasureType)Activator.CreateInstance(typeof(UnitOfMeasureType), _currSession);
                }
                else if (_objList == ObjectList.UserAccess)
                {
                    _result = (UserAccess)Activator.CreateInstance(typeof(UserAccess), _currSession);
                }
                else if (_objList == ObjectList.UserAccessRole)
                {
                    _result = (UserAccessRole)Activator.CreateInstance(typeof(UserAccessRole), _currSession);
                }

            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" Module = GlobalFunction " + ex.ToString());
            }
            return _result;
        }

        public int GetObjectList(string _objectName)
        {
            int _result = 0;
            string _locObjectName = null;
            try
            {
                if (_objectName != null)
                {
                    _locObjectName = _objectName.Trim().Replace(" ", "").ToLower();
                    if (_locObjectName == "Address".ToLower())
                    {
                        _result = 1;
                    }
                    else if (_locObjectName == "ApplicationImport".ToLower())
                    {
                        _result = 2;
                    }
                    else if (_locObjectName == "ApplicationSetup".ToLower())
                    {
                        _result = 3;
                    }
                    else if (_locObjectName == "ApplicationSetupDetail".ToLower())
                    {
                        _result = 4;
                    }
                    else if (_locObjectName == "BeginingInventory".ToLower())
                    {
                        _result = 5;
                    }
                    else if (_locObjectName == "BinLocation".ToLower())
                    {
                        _result = 6;
                    }
                    else if (_locObjectName == "Brand".ToLower())
                    {
                        _result = 7;
                    }
                    else if (_locObjectName == "BusinessPartner".ToLower())
                    {
                        _result = 8;
                    }
                    else if (_locObjectName == "City".ToLower())
                    {
                        _result = 9;
                    }
                    else if (_locObjectName == "Company".ToLower())
                    {
                        _result = 10;
                    }
                    else if (_locObjectName == "Country".ToLower())
                    {
                        _result = 11;
                    }
                    else if (_locObjectName == "Department".ToLower())
                    {
                        _result = 12;
                    }
                    else if (_locObjectName == "Division".ToLower())
                    {
                        _result = 13;
                    }
                    else if (_locObjectName == "DocumentType".ToLower())
                    {
                        _result = 14;
                    }
                    else if (_locObjectName == "Employee".ToLower())
                    {
                        _result = 15;
                    }
                    else if (_locObjectName == "InventoryJournal".ToLower())
                    {
                        _result = 16;
                    }
                    else if (_locObjectName == "InventoryTransfer".ToLower())
                    {
                        _result = 17;
                    }
                    else if (_locObjectName == "InventoryTransferLine".ToLower())
                    {
                        _result = 18;
                    }
                    else if (_locObjectName == "Item".ToLower())
                    {
                        _result = 19;
                    }
                    else if (_locObjectName == "ItemComponent".ToLower())
                    {
                        _result = 20;
                    }
                    else if (_locObjectName == "ItemType".ToLower())
                    {
                        _result = 21;
                    }
                    else if (_locObjectName == "ItemUnitOfMeasure".ToLower())
                    {
                        _result = 22;
                    }
                    else if (_locObjectName == "ListImport".ToLower())
                    {
                        _result = 23;
                    }
                    else if (_locObjectName == "Location".ToLower())
                    {
                        _result = 24;
                    }
                    else if (_locObjectName == "NumberingHeader".ToLower())
                    {
                        _result = 25;
                    }
                    else if (_locObjectName == "NumberingLine".ToLower())
                    {
                        _result = 26;
                    }
                    else if (_locObjectName == "Position".ToLower())
                    {
                        _result = 27;
                    }
                    else if (_locObjectName == "PrePurchaseOrder".ToLower())
                    {
                        _result = 28;
                    }
                    else if (_locObjectName == "ProjectHeader".ToLower())
                    {
                        _result = 29;
                    }
                    else if (_locObjectName == "ProjectLine".ToLower())
                    {
                        _result = 30;
                    }
                    else if (_locObjectName == "ProjectLineItem".ToLower())
                    {
                        _result = 31;
                    }
                    else if (_locObjectName == "ProjectLineItem2".ToLower())
                    {
                        _result = 32;
                    }
                    else if (_locObjectName == "ProjectLineService".ToLower())
                    {
                        _result = 33;
                    }
                    else if (_locObjectName == "PurchaseOrder".ToLower())
                    {
                        _result = 34;
                    }
                    else if (_locObjectName == "PurchaseOrderLine".ToLower())
                    {
                        _result = 35;
                    }
                    else if (_locObjectName == "PurchaseRequisition".ToLower())
                    {
                        _result = 36;
                    }
                    else if (_locObjectName == "PurchaseRequisitionLine".ToLower())
                    {
                        _result = 37;
                    }
                    else if (_locObjectName == "SalesQuotation".ToLower())
                    {
                        _result = 38;
                    }
                    else if (_locObjectName == "SalesQuotationLine".ToLower())
                    {
                        _result = 39;
                    }
                    else if (_locObjectName == "Section".ToLower())
                    {
                        _result = 40;
                    }
                    else if (_locObjectName == "StockTransfer".ToLower())
                    {
                        _result = 41;
                    }
                    else if (_locObjectName == "StockTransferLine".ToLower())
                    {
                        _result = 42;
                    }
                    else if (_locObjectName == "Tax".ToLower())
                    {
                        _result = 43;
                    }
                    else if (_locObjectName == "TaxType".ToLower())
                    {
                        _result = 44;
                    }
                    else if (_locObjectName == "TermOfPayment".ToLower())
                    {
                        _result = 45;
                    }
                    else if (_locObjectName == "UnitOfMeasure".ToLower())
                    {
                        _result = 46;
                    }
                    else if (_locObjectName == "UnitOfMeasureType".ToLower())
                    {
                        _result = 47;
                    }
                    else if (_locObjectName == "UserAccess".ToLower())
                    {
                        _result = 48;
                    }
                    else if (_locObjectName == "UserAccessRole".ToLower())
                    {
                        _result = 49;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" Module = GlobalFunction " + ex.ToString());
            }
            return _result;
        }

        public int GetDirectionType(string _objectName)
        {
            int _result = 0;
            string _locObjectName = null;
            try
            {
                if (_objectName != null)
                {
                    _locObjectName = _objectName.Trim().Replace(" ", "").ToLower();
                    if (_locObjectName == "Internal".ToLower())
                    {
                        _result = 1;
                    }
                    else if (_locObjectName == "External".ToLower())
                    {
                        _result = 2;
                    }else
                    {
                        _result = 0;
                    } 
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" Module = GlobalFunction " + ex.ToString());
            }
            return _result;
        }

        public int GetInventoryMovingType(string _objectName)
        {
            int _result = 0;
            string _locObjectName = null;
            try
            {
                if (_objectName != null)
                {
                    _locObjectName = _objectName.Trim().Replace(" ", "").ToLower();
                    if (_locObjectName == "Receive".ToLower())
                    {
                        _result = 1;
                    }
                    else if (_locObjectName == "Issue".ToLower())
                    {
                        _result = 2;
                    }
                    else if (_locObjectName == "Transfer".ToLower())
                    {
                        _result = 3;
                    }
                    else if (_locObjectName == "Adjust".ToLower())
                    {
                        _result = 4;
                    }
                    else if (_locObjectName == "Return".ToLower())
                    {
                        _result = 5;
                    }
                    else
                    {
                        _result = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" Module = GlobalFunction " + ex.ToString());
            }
            return _result;
        }

        public int GetDocumentRule(string _objectName)
        {
            int _result = 0;
            string _locObjectName = null;
            try
            {
                if (_objectName != null)
                {
                    _locObjectName = _objectName.Trim().Replace(" ", "").ToLower();
                    if (_locObjectName == "Customer".ToLower())
                    {
                        _result = 1;
                    }
                    else if (_locObjectName == "Vendor".ToLower())
                    {
                        _result = 2;
                    }
                    else
                    {
                        _result = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" Module = GlobalFunction " + ex.ToString());
            }
            return _result;
        }

        public int GetStockType(string _objectName)
        {
            int _result = 0;
            string _locObjectName = null;
            try
            {
                if (_objectName != null)
                {
                    _locObjectName = _objectName.Trim().Replace(" ", "").ToLower();
                    if (_locObjectName == "Good".ToLower())
                    {
                        _result = 1;
                    }
                    else if (_locObjectName == "Bad".ToLower())
                    {
                        _result = 2;
                    }
                    else
                    {
                        _result = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" Module = GlobalFunction " + ex.ToString());
            }
            return _result;
        }

        public int GetBusinessPartnerType(string _objectName)
        {
            int _result = 0;
            string _locObjectName = null;
            try
            {
                if (_objectName != null)
                {
                    _locObjectName = _objectName.Trim().Replace(" ", "").ToLower();
                    if (_locObjectName == "Customer".ToLower())
                    {
                        _result = 1;
                    }
                    else if (_locObjectName == "Vendor".ToLower())
                    {
                        _result = 2;
                    }
                    else if(_locObjectName == "All".ToLower())
                    {
                        _result = 3;
                    }
                    else
                    {
                        _result = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(" Module = GlobalFunction " + ex.ToString());
            }
            return _result;
        }

        #endregion ImportData

        #region NormalFunction

        public bool CheckDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion NormalFunction
    }
}
