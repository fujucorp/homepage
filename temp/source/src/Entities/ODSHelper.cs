using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fuju;
using Fuju.ODS;

namespace Entities
{
    /// <summary>
    /// ODS 處理類別
    /// </summary>
    public class ODSHelper
    {
        #region Private Member
        #region 型別變數
        private Type _DecimalType = typeof(System.Decimal);
        private Type _DatetimeType = typeof(System.DateTime);

        private Type _StringType = typeof(System.String);
        private Type _Int32Type = typeof(System.Int32);
        private Type _Int64Type = typeof(System.Int64);
        #endregion
        #endregion

        #region Constructor
        public ODSHelper()
        {
        }
        #endregion

        #region Public Method
        #region [OLD]
        ///// <summary>
        ///// 將指定 DataTable 轉成 ODS，並依據 outFilePath 參數產生檔案或 byte 陣列
        ///// </summary>
        ///// <param name="dtData">指定 DataTable。</param>
        ///// <param name="sheetName">指定工作表名稱，不指定則使用預設值 Sheet1。</param>
        ///// <param name="isUseColumnCaption">指定表頭（欄位名稱）是否使用 Column 的 Caption。</param>
        ///// <param name="isUseAmountStyle">指定 Decimal 欄位是否套用金額格式（#,##0 或 #,##0.00）。</param>
        ///// <param name="amountColumns">當 isUseAmountStyle 參數為 true，指定套用金額格式的欄位名稱，不指定表示所有 Decimal 都套用。</param>
        ///// <param name="isDecimalTruncate">指定 Decimal 欄位值是否只取整數（#,##0）。</param>
        ///// <param name="truncateColumns">當 isDecimalTruncate 參數為 true，指定套用整數的欄位名稱，不指定表示所有 Decimal 都套用。</param>
        ///// <param name="isUseDateTimeStyle">指定 DateTime 欄位是否套用日期時間格式（yyyy/MM/dd HH:mm:ss 或 yyyy/MM/dd）。</param>
        ///// <param name="datetimeColumns">當 isUseDateTimeStyle 參數為 true，指定套用日期時間格式的欄位名稱，不指定表示所有 DateTime 都套用。</param>
        ///// <param name="isDateOnly">指定 DateTime 欄位值是否只取日期（yyyy/MM/dd）。</param>
        ///// <param name="dateOnlyColumns">當 isDateOnly 參數為 true，指定套用純日期的欄位名稱，不指定表示所有 DateTime 都套用。</param>
        ///// <param name="outFilePath">指定產生檔案的完整檔名路徑，如果不指定則傳回檔案的 byte 陣列。</param>
        ///// <returns>不指定 outFilePath 參數時傳回檔案的 byte 陣列，否則傳回 null。</returns>
        //public byte[] DataTable2ODS(DataTable dtData
        //    , string sheetName = "Sheet1", bool isUseColumnCaption = false
        //    , bool isUseAmountStyle = false, string[] amountColumns = null
        //    , bool isDecimalTruncate = false, string[] truncateColumns = null
        //    , bool isUseDateTimeStyle = false, string[] datetimeColumns = null
        //    , bool isDateOnly = false, string[] dateOnlyColumns = null
        //    , string outFilePath = null)
        //{
        //    if (dtData != null)
        //    {
        //        if (String.IsNullOrWhiteSpace(sheetName))
        //        {
        //            sheetName = "Sheet1";
        //        }
        //        else
        //        {
        //            sheetName = sheetName.Trim();
        //        }

        //        #region 變數初始化
        //        ODSWorkbook wb = new ODSWorkbook(1);
        //        ODSSheet sheet = wb.CreateSheet(sheetName);
        //        ODSRow odsRow = null;
        //        ODSCell odsCell = null;
        //        int startColumnNo = 1;  //起始的欄編號

        //        #region 型別變數
        //        Type decimalType = typeof(System.Decimal);
        //        Type datetimeType = typeof(System.DateTime);
        //        #endregion

        //        #region 金額格式化用變數
        //        ODSCellStyle floatAmountCellStyle = null;
        //        if (isUseAmountStyle)
        //        {
        //            floatAmountCellStyle = wb.CreateCellStyle();
        //            floatAmountCellStyle.HAlignment = HorizontalAlignment.Right;
        //            floatAmountCellStyle.SetDataFormat(ODSNumberFormat.FloatComma);
        //        }
        //        #endregion

        //        #region 金額且整數格式化用變數
        //        ODSCellStyle integerAmountCellStyle = null;
        //        if (isDecimalTruncate)
        //        {
        //            integerAmountCellStyle = wb.CreateCellStyle();
        //            integerAmountCellStyle.HAlignment = HorizontalAlignment.Right;
        //            integerAmountCellStyle.SetDataFormat(ODSNumberFormat.IntegerComma);
        //        }
        //        #endregion

        //        #region 整數格式化用變數
        //        ODSCellStyle integerCellStyle = null;
        //        if (isDecimalTruncate)
        //        {
        //            integerCellStyle = wb.CreateCellStyle();
        //            integerCellStyle.SetDataFormat(ODSNumberFormat.Integer);
        //        }
        //        #endregion

        //        #region 日期時間格式化用變數
        //        ODSCellStyle datetimeCellStyle = null;
        //        if (isUseDateTimeStyle)
        //        {
        //            datetimeCellStyle = wb.CreateCellStyle();
        //            datetimeCellStyle.SetDataFormat(ODSDateFormat.DateTime);
        //        }
        //        #endregion

        //        #region 純日期格式化用變數
        //        ODSCellStyle dateOnlyCellStyle = null;
        //        if (isUseDateTimeStyle && isDateOnly)
        //        {
        //            dateOnlyCellStyle = wb.CreateCellStyle();
        //            dateOnlyCellStyle.SetDataFormat(ODSDateFormat.Date);
        //        }
        //        #endregion
        //        #endregion

        //        #region Head Row (表頭列)
        //        if (dtData.Columns.Count > 0)
        //        {
        //            int headRowNo = 1;  //第一列為表頭列
        //            odsRow = sheet.CreateRow(headRowNo);
        //            int columnNo = startColumnNo;
        //            foreach (DataColumn dataColumn in dtData.Columns)
        //            {
        //                odsCell = odsRow.CreateCell(columnNo, CellType.String);
        //                if (isUseColumnCaption)
        //                {
        //                    odsCell.SetCellValue(dataColumn.Caption);
        //                }
        //                else
        //                {
        //                    odsCell.SetCellValue(dataColumn.ColumnName);
        //                }
        //                columnNo++;
        //            }
        //        }
        //        #endregion

        //        #region Data Row (資料列)
        //        if (dtData.Rows.Count > 0)
        //        {
        //            bool hasAmountColumns = (amountColumns != null && amountColumns.Length > 0);
        //            bool hasTruncateColumns = (truncateColumns != null && truncateColumns.Length > 0);
        //            bool hasDatetimeColumns = (datetimeColumns != null && datetimeColumns.Length > 0);
        //            bool hasDateOnlyColumns = (dateOnlyColumns != null && dateOnlyColumns.Length > 0);

        //            int dataRowNo = 2;  //第2列開始為資料列
        //            foreach (DataRow dataRow in dtData.Rows)
        //            {
        //                odsRow = sheet.CreateRow(dataRowNo);
        //                int columnNo = 1;
        //                foreach (DataColumn dataColumn in dtData.Columns)
        //                {
        //                    if (dataColumn.DataType.IsValueType)
        //                    {
        //                        if (dataColumn.DataType == datetimeType)
        //                        {
        //                            #region DateTime 型別
        //                            DateTime? cellValue = dataRow.IsNull(dataColumn) ? (DateTime?)null : Convert.ToDateTime(dataRow[dataColumn]);
        //                            ODSCellStyle myCellStyle = null;

        //                            //使用日期時間格式化但沒指定欄位，表示所有日期時間型別的欄位都套用，否則符合指定欄位要套用
        //                            if (isUseDateTimeStyle && (!hasDatetimeColumns || datetimeColumns.Contains(dataColumn.ColumnName)))
        //                            {
        //                                //使用純日期但沒指定欄位，表示所有日期時間型別的欄位都套用，否則符合指定欄位要套用
        //                                if (isDateOnly && (!hasDateOnlyColumns || dateOnlyColumns.Contains(dataColumn.ColumnName)))
        //                                {
        //                                    if (cellValue.HasValue)
        //                                    {
        //                                        cellValue = cellValue.Value.Date;
        //                                    }
        //                                    myCellStyle = dateOnlyCellStyle;
        //                                }
        //                                else
        //                                {
        //                                    myCellStyle = datetimeCellStyle;
        //                                }
        //                            }
        //                            else if (isDateOnly && (!hasDateOnlyColumns || dateOnlyColumns.Contains(dataColumn.ColumnName)))
        //                            {
        //                                if (cellValue.HasValue)
        //                                {
        //                                    cellValue = cellValue.Value.Date;
        //                                }
        //                                myCellStyle = dateOnlyCellStyle;
        //                            }

        //                            odsCell = odsRow.CreateCell(columnNo, CellType.Numeric);
        //                            odsCell.CellStyle = myCellStyle;
        //                            if (cellValue.HasValue)
        //                            {
        //                                odsCell.SetCellValue(cellValue.Value);
        //                            }
        //                            else
        //                            {
        //                                odsCell.SetCellValue(String.Empty);
        //                            }
        //                            #endregion
        //                        }
        //                        else if (dataColumn.DataType == decimalType)
        //                        {
        //                            #region Decimal 型別
        //                            Double? cellValue = dataRow.IsNull(dataColumn) ? (Double?)null : Convert.ToDouble(dataRow[dataColumn]);
        //                            ODSCellStyle myCellStyle = null;

        //                            //使用金額格式化但沒指定欄位，表示所有數值型別的欄位都套用，否則符合指定欄位要套用
        //                            if (isUseAmountStyle && (!hasAmountColumns || amountColumns.Contains(dataColumn.ColumnName)))
        //                            {
        //                                //使用去小數但沒指定欄位，表示所有數值型別的欄位都套用，否則符合指定欄位要套用
        //                                if (isDecimalTruncate && (!hasTruncateColumns || truncateColumns.Contains(dataColumn.ColumnName)))
        //                                {
        //                                    if (cellValue.HasValue)
        //                                    {
        //                                        cellValue = Convert.ToDouble(cellValue.Value.ToString("0"));
        //                                    }
        //                                    myCellStyle = integerAmountCellStyle;
        //                                }
        //                                else
        //                                {
        //                                    myCellStyle = floatAmountCellStyle;
        //                                }
        //                            }
        //                            else if (isDecimalTruncate && (!hasTruncateColumns || truncateColumns.Contains(dataColumn.ColumnName)))
        //                            {
        //                                if (cellValue.HasValue)
        //                                {
        //                                    cellValue = Convert.ToDouble(cellValue.Value.ToString("0"));
        //                                }
        //                                myCellStyle = integerCellStyle;
        //                            }

        //                            odsCell = odsRow.CreateCell(columnNo, CellType.Numeric);
        //                            odsCell.CellStyle = myCellStyle;
        //                            if (cellValue.HasValue)
        //                            {
        //                                odsCell.SetCellValue(cellValue.Value);
        //                            }
        //                            else
        //                            {
        //                                odsCell.SetCellValue(String.Empty);
        //                            }
        //                            #endregion
        //                        }
        //                        else
        //                        {
        //                            #region 其他值型別
        //                            odsCell = odsRow.CreateCell(columnNo, CellType.Numeric);
        //                            if (dataRow.IsNull(dataColumn))
        //                            {
        //                                odsCell.SetCellValue(String.Empty);
        //                            }
        //                            else
        //                            {
        //                                odsCell.SetCellValue(Convert.ToDouble(dataRow[dataColumn]));
        //                            }
        //                            #endregion
        //                        }
        //                    }
        //                    else
        //                    {
        //                        #region 非值型別 (以字串處理)
        //                        odsCell = odsRow.CreateCell(columnNo, CellType.String);
        //                        if (dataRow.IsNull(dataColumn))
        //                        {
        //                            odsCell.SetCellValue(String.Empty);
        //                        }
        //                        else
        //                        {
        //                            odsCell.SetCellValue(dataRow[dataColumn].ToString());
        //                        }
        //                        #endregion
        //                    }
        //                    columnNo++;
        //                }
        //                dataRowNo++;
        //            }
        //        }
        //        #endregion

        //        #region 將內容寫成檔案或轉成 byte 陣列
        //        if (String.IsNullOrEmpty(outFilePath))
        //        {
        //            byte[] fileContent = null;
        //            wb.WriteToBytes(out fileContent);
        //            return fileContent;
        //        }
        //        else
        //        {
        //            wb.WriteToFile(outFilePath);
        //            return null;
        //        }
        //        #endregion
        //    }
        //    return null;
        //}
        #endregion

        /// <summary>
        /// 將指定 DataTable 轉成 ODS，並依據 outFilePath 參數產生檔案或 byte 陣列
        /// </summary>
        /// <param name="dtData">指定 DataTable。</param>
        /// <param name="sheetName">指定工作表名稱，不指定則使用預設值 Sheet1。</param>
        /// <param name="isUseColumnCaption">指定表頭（欄位名稱）是否使用 Column 的 Caption。</param>
        /// <param name="isUseAmountStyle">指定 Decimal 欄位是否套用金額格式（#,##0 或 #,##0.00）。</param>
        /// <param name="amountColumns">當 isUseAmountStyle 參數為 true，指定套用金額格式的欄位名稱，不指定表示所有 Decimal 都套用。</param>
        /// <param name="isDecimalTruncate">指定 Decimal 欄位值是否只取整數（#,##0）。</param>
        /// <param name="truncateColumns">當 isDecimalTruncate 參數為 true，指定套用整數的欄位名稱，不指定表示所有 Decimal 都套用。</param>
        /// <param name="isUseDateTimeStyle">指定 DateTime 欄位是否套用日期時間格式（yyyy/MM/dd HH:mm:ss 或 yyyy/MM/dd）。</param>
        /// <param name="datetimeColumns">當 isUseDateTimeStyle 參數為 true，指定套用日期時間格式的欄位名稱，不指定表示所有 DateTime 都套用。</param>
        /// <param name="isDateOnly">指定 DateTime 欄位值是否只取日期（yyyy/MM/dd）。</param>
        /// <param name="dateOnlyColumns">當 isDateOnly 參數為 true，指定套用純日期的欄位名稱，不指定表示所有 DateTime 都套用。</param>
        /// <param name="isDefaultTextStyle">指定是否所有欄位預設使用文字格式樣式，預設 fasle，表示不指定樣式。</param>
        /// <param name="decimalTextFormat">指定使用文字格式樣式時，Decimal 的文字格式化字串，預設 #,##0。</param>
        /// <param name="datetimeTextFormat">指定使用文字格式樣式時，DateTime 的文字格式化字串，預設 yyyy/MM/dd。</param>
        /// <param name="outFilePath">指定產生檔案的完整檔名路徑，如果不指定則傳回檔案的 byte 陣列。</param>
        /// <returns>不指定 outFilePath 參數時傳回檔案的 byte 陣列，否則傳回 null。</returns>
        /// <remarks>
        /// <para>1. truncateColumns 與 amountColumns 指定的欄位如果重疊，以 truncateColumns 為準。</para>
        /// <para>2. dateOnlyColumns 與 datetimeColumns 指定的欄位如果重疊，以 dateOnlyColumns 為準。</para>
        /// </remarks>
        public byte[] DataTable2ODS(DataTable dtData
            , string sheetName = "Sheet1", bool isUseColumnCaption = false
            , bool isUseAmountStyle = false, string[] amountColumns = null
            , bool isDecimalTruncate = false, string[] truncateColumns = null
            , bool isUseDateTimeStyle = false, string[] datetimeColumns = null
            , bool isDateOnly = false, string[] dateOnlyColumns = null
            , bool isDefaultTextStyle = false, string decimalTextFormat = "#,##0", string datetimeTextFormat = "yyyy/MM/dd"
            , string outFilePath = null)
        {
            if (dtData != null)
            {
                if (String.IsNullOrWhiteSpace(sheetName))
                {
                    sheetName = "Sheet1";
                }
                else
                {
                    sheetName = sheetName.Trim();
                }

                #region 變數初始化
                ODSWorkbook wb = new ODSWorkbook(1);
                ODSSheet sheet = wb.CreateSheet(sheetName);
                ODSRow odsRow = null;
                ODSCell odsCell = null;
                int startColumnNo = 1;  //起始的欄編號

                #region 金額格式化用變數
                ODSCellStyle floatAmountCellStyle = null;
                if (isUseAmountStyle)
                {
                    floatAmountCellStyle = wb.CreateCellStyle();
                    floatAmountCellStyle.HAlignment = HorizontalAlignment.Right;
                    floatAmountCellStyle.SetDataFormat(ODSNumberFormat.FloatComma);
                }
                #endregion

                #region 金額且整數格式化用變數
                ODSCellStyle integerAmountCellStyle = null;
                if (isDecimalTruncate)
                {
                    integerAmountCellStyle = wb.CreateCellStyle();
                    integerAmountCellStyle.HAlignment = HorizontalAlignment.Right;
                    integerAmountCellStyle.SetDataFormat(ODSNumberFormat.IntegerComma);
                }
                #endregion

                #region 整數格式化用變數
                ODSCellStyle integerCellStyle = null;
                if (isDecimalTruncate)
                {
                    integerCellStyle = wb.CreateCellStyle();
                    integerCellStyle.SetDataFormat(ODSNumberFormat.Integer);
                }
                #endregion

                #region 日期時間格式化用變數
                ODSCellStyle datetimeCellStyle = null;
                if (isUseDateTimeStyle)
                {
                    datetimeCellStyle = wb.CreateCellStyle();
                    datetimeCellStyle.SetDataFormat(ODSDateFormat.DateTime);
                }
                #endregion

                #region 純日期格式化用變數
                ODSCellStyle dateOnlyCellStyle = null;
                if (isUseDateTimeStyle && isDateOnly)
                {
                    dateOnlyCellStyle = wb.CreateCellStyle();
                    dateOnlyCellStyle.SetDataFormat(ODSDateFormat.Date);
                }
                #endregion

                #region 文字格式
                ODSCellStyle textCellStyle = null;
                if (isDefaultTextStyle)
                {
                    textCellStyle = wb.CreateCellStyle();
                    textCellStyle.SetDataFormat(ODSTextFormat.Regular);
                }
                #endregion
                #endregion

                bool hasAmountColumns = (amountColumns != null && amountColumns.Length > 0);
                bool hasTruncateColumns = (truncateColumns != null && truncateColumns.Length > 0);
                bool hasDatetimeColumns = (datetimeColumns != null && datetimeColumns.Length > 0);
                bool hasDateOnlyColumns = (dateOnlyColumns != null && dateOnlyColumns.Length > 0);

                #region Head Row (表頭列)
                if (dtData.Columns.Count > 0)
                {
                    int headRowNo = 1;  //第一列為表頭列
                    odsRow = sheet.CreateRow(headRowNo);
                    int columnNo = startColumnNo;
                    foreach (DataColumn dataColumn in dtData.Columns)
                    {
                        #region Column Style
                        {
                            ODSColumn odsColumn = null;
                            if (dataColumn.DataType.IsValueType)
                            {
                                if (dataColumn.DataType == _DatetimeType)
                                {
                                    #region DateTime 型別
                                    //使用純日期但沒指定欄位，表示所有日期時間型別的欄位都套用，否則符合指定欄位要套用
                                    if (isDateOnly && (!hasDateOnlyColumns || dateOnlyColumns.Contains(dataColumn.ColumnName)))
                                    {
                                        odsColumn = sheet.GetOrCreateColumn(columnNo);
                                        odsColumn.CellStyle = dateOnlyCellStyle;
                                    }
                                    //使用日期時間格式化但沒指定欄位，表示所有日期時間型別的欄位都套用，否則符合指定欄位要套用
                                    else if (isUseDateTimeStyle && (!hasDatetimeColumns || datetimeColumns.Contains(dataColumn.ColumnName)))
                                    {
                                        odsColumn = sheet.GetOrCreateColumn(columnNo);
                                        odsColumn.CellStyle = datetimeCellStyle;
                                    }
                                    #endregion
                                }
                                else if (dataColumn.DataType == _DecimalType)
                                {
                                    #region Decimal 型別
                                    //使用去小數但沒指定欄位，表示所有數值型別的欄位都套用，否則符合指定欄位要套用
                                    if (isDecimalTruncate && (!hasTruncateColumns || truncateColumns.Contains(dataColumn.ColumnName)))
                                    {
                                        odsColumn = sheet.GetOrCreateColumn(columnNo);
                                        odsColumn.CellStyle = integerAmountCellStyle;
                                    }
                                    //使用金額格式化但沒指定欄位，表示所有數值型別的欄位都套用，否則符合指定欄位要套用
                                    else if (isUseAmountStyle && (!hasAmountColumns || amountColumns.Contains(dataColumn.ColumnName)))
                                    {
                                        odsColumn = sheet.GetOrCreateColumn(columnNo);
                                        odsColumn.CellStyle = floatAmountCellStyle;
                                    }
                                    #endregion
                                }
                            }
                            if (odsColumn == null && isDefaultTextStyle)
                            {
                                #region 預設使用文字格式
                                odsColumn = sheet.GetOrCreateColumn(columnNo);
                                odsColumn.CellStyle = textCellStyle;
                                #endregion
                            }
                        }
                        #endregion

                        odsCell = odsRow.CreateCell(columnNo, CellType.String);
                        if (isUseColumnCaption)
                        {
                            odsCell.SetCellValue(dataColumn.Caption);
                        }
                        else
                        {
                            odsCell.SetCellValue(dataColumn.ColumnName);
                        }
                        columnNo++;
                    }
                }
                #endregion

                #region Data Row (資料列)
                if (dtData.Rows.Count > 0)
                {
                    int dataRowNo = 2;  //第2列開始為資料列
                    foreach (DataRow dataRow in dtData.Rows)
                    {
                        odsRow = sheet.CreateRow(dataRowNo);
                        int columnNo = 1;
                        foreach (DataColumn dataColumn in dtData.Columns)
                        {
                            if (dataColumn.DataType.IsValueType)
                            {
                                if (dataColumn.DataType == _DatetimeType)
                                {
                                    #region DateTime 型別
                                    DateTime? cellValue = dataRow.IsNull(dataColumn) ? (DateTime?)null : Convert.ToDateTime(dataRow[dataColumn]);

                                    odsCell = odsRow.CreateCell(columnNo, CellType.Numeric);
                                    if (cellValue.HasValue)
                                    {
                                        //使用純日期但沒指定欄位，表示所有日期時間型別的欄位都套用，否則符合指定欄位要套用
                                        if (isDateOnly && (!hasDateOnlyColumns || dateOnlyColumns.Contains(dataColumn.ColumnName)))
                                        {
                                            odsCell.SetCellValue(cellValue.Value.Date);
                                        }
                                        //使用日期時間格式化但沒指定欄位，表示所有日期時間型別的欄位都套用，否則符合指定欄位要套用
                                        else if (isUseDateTimeStyle && (!hasDatetimeColumns || datetimeColumns.Contains(dataColumn.ColumnName)))
                                        {
                                            odsCell.SetCellValue(cellValue.Value);
                                        }
                                        //預設使用文字格式
                                        else if (isDefaultTextStyle && !String.IsNullOrWhiteSpace(datetimeTextFormat))
                                        {
                                            odsCell.SetCellValue(cellValue.Value.ToString(datetimeTextFormat));
                                        }
                                        else
                                        {
                                            odsCell.SetCellValue(cellValue.Value);
                                        }
                                    }
                                    else
                                    {
                                        odsCell.SetCellValue(String.Empty);
                                    }
                                    #endregion
                                }
                                else if (dataColumn.DataType == _DecimalType)
                                {
                                    #region Decimal 型別
                                    Double? cellValue = dataRow.IsNull(dataColumn) ? (Double?)null : Convert.ToDouble(dataRow[dataColumn]);

                                    odsCell = odsRow.CreateCell(columnNo, CellType.Numeric);
                                    if (cellValue.HasValue)
                                    {
                                        //使用去小數但沒指定欄位，表示所有數值型別的欄位都套用，否則符合指定欄位要套用
                                        if (isDecimalTruncate && (!hasTruncateColumns || truncateColumns.Contains(dataColumn.ColumnName)))
                                        {
                                            odsCell.SetCellValue(Math.Truncate(cellValue.Value));
                                        }
                                        //使用金額格式化但沒指定欄位，表示所有數值型別的欄位都套用，否則符合指定欄位要套用
                                        else if (isUseAmountStyle && (!hasAmountColumns || amountColumns.Contains(dataColumn.ColumnName)))
                                        {
                                            odsCell.SetCellValue(cellValue.Value);
                                        }
                                        //預設使用文字格式
                                        else if (isDefaultTextStyle && !String.IsNullOrWhiteSpace(decimalTextFormat))
                                        {
                                            odsCell.SetCellValue(cellValue.Value.ToString(decimalTextFormat));
                                        }
                                        else
                                        {
                                            odsCell.SetCellValue(cellValue.Value);
                                        }
                                    }
                                    else
                                    {
                                        odsCell.SetCellValue(String.Empty);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 其他值型別
                                    odsCell = odsRow.CreateCell(columnNo, CellType.Numeric);
                                    if (dataRow.IsNull(dataColumn))
                                    {
                                        odsCell.SetCellValue(String.Empty);
                                    }
                                    else
                                    {
                                        //預設使用文字格式
                                        if (isDefaultTextStyle)
                                        {
                                            odsCell.SetCellValue(dataRow[dataColumn].ToString());
                                        }
                                        else
                                        {
                                            odsCell.SetCellValue(Convert.ToDouble(dataRow[dataColumn]));
                                        }
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                #region 非值型別 (以字串處理)
                                odsCell = odsRow.CreateCell(columnNo, CellType.String);
                                if (dataRow.IsNull(dataColumn))
                                {
                                    odsCell.SetCellValue(String.Empty);
                                }
                                else
                                {
                                    odsCell.SetCellValue(dataRow[dataColumn].ToString());
                                }
                                #endregion
                            }
                            columnNo++;
                        }
                        dataRowNo++;
                    }
                }
                #endregion

                #region 將內容寫成檔案或轉成 byte 陣列
                if (String.IsNullOrEmpty(outFilePath))
                {
                    byte[] fileContent = null;
                    wb.WriteToBytes(out fileContent);
                    return fileContent;
                }
                else
                {
                    wb.WriteToFile(outFilePath);
                    return null;
                }
                #endregion
            }
            return null;
        }

        /// <summary>
        /// 產生 Ods 範本
        /// </summary>
        /// <param name="headTexts">指定 Ods 範本的表頭欄位名稱集合。</param>
        /// <param name="sheetName">指定工作表名稱。</param>
        /// <returns>傳回 ODS 檔的 byte 陣列。</returns>
        public byte[] GenOdsSample(ICollection<string> headTexts, string sheetName)
        {
            if (String.IsNullOrWhiteSpace(sheetName))
            {
                sheetName = "Sheet1";
            }
            else
            {
                sheetName = sheetName.Trim();
            }

            #region 變數初始化
            ODSWorkbook wb = new ODSWorkbook(1);
            ODSSheet sheet = wb.CreateSheet(sheetName);
            ODSRow odsRow = null;
            ODSCell odsCell = null;
            int startColumnNo = 1;  //起始的欄編號

            #region 文字格式
            ODSCellStyle textCellStyle = wb.CreateCellStyle();
            textCellStyle.SetDataFormat(ODSTextFormat.Regular);
            #endregion
            #endregion

            #region Head Row (表頭列)
            {
                int headRowNo = 1;  //第一列為表頭列
                odsRow = sheet.CreateRow(headRowNo);
                int columnNo = startColumnNo;
                foreach (string headText in headTexts)
                {
                    #region Column Style
                    {
                        ODSColumn odsColumn = sheet.GetOrCreateColumn(columnNo);
                        odsColumn.CellStyle = textCellStyle;
                    }
                    #endregion

                    #region Cell Value
                    odsCell = odsRow.CreateCell(columnNo, CellType.String);
                    odsCell.SetCellValue(headText);
                    #endregion

                    columnNo++;
                }
            }
            #endregion

            #region 將內容寫轉成 byte 陣列
            byte[] fileContent = null;
            wb.WriteToBytes(out fileContent);
            return fileContent;
            #endregion
        }
        #endregion

        #region 報表相關方法
        #region 繳費銷帳總表
        /// <summary>
        /// 繳費銷帳總表
        /// </summary>
        /// <param name="dtHead">表頭資料</param>
        /// <param name="dtDeptSum">部別小計資料</param>
        /// <param name="dtMajorSum">系所小計資料</param>
        /// <param name="dtClassSum">班別小計資料</param>
        /// <param name="receiveItems">收入科目資料</param>
        /// <param name="dtData">表身資料</param>
        /// <returns>成功則傳回 null，否怎傳回錯誤訊息</returns>
        public string GenReportA2(DataTable dtHead, DataTable dtDeptSum, DataTable dtMajorSum, DataTable dtClassSum, KeyValueList<string> receiveItems, out byte[] content)
        {
            content = null;
            string errmsg = null;
            try
            {
                #region 使用 ODSWorkbook 產生 ODS
                string sheetName = "Sheet1";

                ODSWorkbook wb = new ODSWorkbook();
                ODSSheet sheet = wb.CreateSheet(sheetName);

                int columnCount = receiveItems.Count;

                #region 指定紙張大小
                wb.PageStyle.PaperSize = PapeSizeKind.A4;
                if (columnCount > 12 && columnCount <= 15)
                {
                    wb.PageStyle.PaperSize = PapeSizeKind.B4_JIS;
                }
                else if (columnCount > 15)
                {
                    wb.PageStyle.PaperSize = PapeSizeKind.A3;
                }
                #endregion

                #region 指定每頁資料筆數
                int pageSize = 16;
                if (wb.PageStyle.PaperSize == PapeSizeKind.B4_JIS)   //B4
                {
                    pageSize = 21;
                }
                else if (wb.PageStyle.PaperSize == PapeSizeKind.A3)   //A3
                {
                    pageSize = 25;
                }
                #endregion

                #region 指定直式或橫式 Lendscape=橫式 Portrait=直式
                wb.PageStyle.PaperOrientation = PageOrientationKind.Lendscape;
                #endregion

                #region 藏隱格線
                wb.IsShowGrid = false;
                #endregion

                #region Page Head (粗體、14字、置中)儲存格格式
                ODSCellStyle pageHeadCellStyle = wb.CreateCellStyle();
                {
                    ODSFont pageHeadFont = wb.CreateFont();
                    pageHeadFont.IsBold = true;
                    pageHeadFont.FontSize = 14D;
                    pageHeadCellStyle.SetFont(pageHeadFont);
                    pageHeadCellStyle.HAlignment = HorizontalAlignment.Center;
                }
                #endregion

                #region Column Title (粗體、底線、右靠)儲存格格式
                ODSCellStyle colTitleCellStyle = wb.CreateCellStyle();
                {
                    ODSFont colTitleFont = wb.CreateFont();
                    colTitleFont.IsBold = true;
                    colTitleCellStyle.SetFont(colTitleFont);
                    colTitleCellStyle.HAlignment = HorizontalAlignment.Right;
                    colTitleCellStyle.BottomBorderStyle = BorderStyle.Thin;
                }
                #endregion

                #region 金額(千分位逗號、底線、右靠)儲存格格式
                ODSCellStyle moneyCellStyle = wb.CreateCellStyle();
                {
                    moneyCellStyle.SetDataFormat(ODSNumberFormat.IntegerComma);
                    moneyCellStyle.HAlignment = HorizontalAlignment.Right;
                    moneyCellStyle.BottomBorderStyle = BorderStyle.Thin;
                }
                #endregion

                int rowNo = 0;
                int pageNo = 0;
                int totalRecord = dtClassSum.Rows.Count + dtMajorSum.Rows.Count + dtDeptSum.Rows.Count;
                int totalPage = dtClassSum == null ? 0 : (int)(totalRecord / pageSize);
                if (totalRecord > 0 && totalRecord % pageSize != 0)
                {
                    totalPage++;
                }

                if (totalPage == 0)
                {
                    #region Gen 表頭
                    {
                        rowNo = this.GenReportA2Head(sheet, rowNo, dtHead, pageNo, totalPage, receiveItems, pageHeadCellStyle, colTitleCellStyle);
                    }
                    #endregion

                    #region 查無資料
                    rowNo++;
                    ODSRow row = sheet.CreateRow(rowNo);
                    int colNo = 1;
                    ODSCell cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("查無資料");
                    #endregion
                }
                else
                {
                    pageNo = 1;

                    #region Gen 表頭
                    {
                        rowNo = this.GenReportA2Head(sheet, rowNo, dtHead, pageNo, totalPage, receiveItems, pageHeadCellStyle, colTitleCellStyle);
                    }
                    #endregion

                    int pageRowCount = 0;  //分頁 Row 計數
                    string receiveType = null;
                    string yearId = null;
                    string termId = null;
                    string depId = null;
                    string receiveId = null;
                    string oldMajorId = null;
                    string oldMajorName = null;
                    string oldDeptId = null;
                    string oldDeptName = null;
                    foreach (DataRow classRow in dtClassSum.Rows)
                    {
                        receiveType = classRow["Receive_Type"].ToString().Replace("'", "");
                        yearId = classRow["Year_Id"].ToString().Replace("'", "");
                        termId = classRow["Term_Id"].ToString().Replace("'", "");
                        depId = classRow["Dep_Id"].ToString().Replace("'", "");
                        receiveId = classRow["Receive_Id"].ToString().Replace("'", "");

                        string classId = classRow["Class_Id"].ToString();
                        string className = classRow["Class_Name"].ToString();
                        string majorId = classRow["Major_Id"].ToString();
                        string majorName = classRow["Major_Name"].ToString();
                        string deptId = classRow["Dept_Id"].ToString();
                        string deptName = classRow["Dept_Name"].ToString();

                        if (oldMajorId == null)
                        {
                            oldMajorId = majorId;
                            oldMajorName = majorName;
                        }
                        if (oldDeptId == null)
                        {
                            oldDeptId = deptId;
                            oldDeptName = deptName;
                        }

                        if (pageRowCount >= pageSize)
                        {
                            pageNo++;
                            pageRowCount = 0;
                            rowNo = this.GenNextPageAndHead(sheet, rowNo, dtHead, pageNo, totalPage, receiveItems, pageHeadCellStyle, colTitleCellStyle);
                        }

                        #region 系所合計
                        {
                            if (oldMajorId != majorId || oldDeptId != deptId)
                            {
                                pageRowCount++;

                                #region [MDY:20220530] Checkmarx 調整
                                #region [OLD]
                                //DataRow[] majorSumRows = dtMajorSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType, yearId, termId, depId, receiveId, oldDeptId, oldMajorId));
                                #endregion

                                DataRow[] majorSumRows = dtMajorSum.Rows.Cast<DataRow>().Where(
                                    row => row["Receive_Type"].Equals(receiveType)
                                        && row["Year_Id"].Equals(yearId)
                                        && row["Term_Id"].Equals(termId)
                                        && row["Dep_Id"].Equals(depId)
                                        && row["Receive_Id"].Equals(receiveId)
                                        && row["Dept_Id"].Equals(oldDeptId)
                                        && row["Major_Id"].Equals(oldMajorId)).ToArray();
                                #endregion

                                rowNo = this.GenReportA2MajorSumRow(sheet, rowNo, majorSumRows, oldDeptName, oldMajorName, receiveItems, moneyCellStyle);

                                oldMajorId = majorId;
                                oldMajorName = majorName;

                                if (pageRowCount >= pageSize)
                                {
                                    pageNo++;
                                    pageRowCount = 0;
                                    rowNo = this.GenNextPageAndHead(sheet, rowNo, dtHead, pageNo, totalPage, receiveItems, pageHeadCellStyle, colTitleCellStyle);
                                }
                            }
                        }
                        #endregion

                        #region 部別合計
                        {
                            if (oldDeptId != deptId)
                            {
                                pageRowCount++;

                                #region [MDY:20220530] Checkmarx 調整
                                #region [OLD]
                                //DataRow[] deptSumRows = dtDeptSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType, yearId, termId, depId, receiveId, oldDeptId));
                                #endregion

                                DataRow[] deptSumRows = dtDeptSum.Rows.Cast<DataRow>().Where(
                                    row => row["Receive_Type"].Equals(receiveType)
                                        && row["Year_Id"].Equals(yearId)
                                        && row["Term_Id"].Equals(termId)
                                        && row["Dep_Id"].Equals(depId)
                                        && row["Receive_Id"].Equals(receiveId)
                                        && row["Dept_Id"].Equals(oldDeptId)).ToArray();
                                #endregion

                                rowNo = this.GenReportA2DeptSumRow(sheet, rowNo, deptSumRows, oldDeptName, receiveItems, moneyCellStyle);

                                oldDeptId = deptId;
                                oldDeptName = deptName;

                                if (pageRowCount >= pageSize)
                                {
                                    pageNo++;
                                    pageRowCount = 0;
                                    rowNo = this.GenNextPageAndHead(sheet, rowNo, dtHead, pageNo, totalPage, receiveItems, pageHeadCellStyle, colTitleCellStyle);
                                }
                            }
                        }
                        #endregion

                        #region 班別合計
                        {
                            pageRowCount++;
                            ODSRow row = null;
                            ODSCell cell = null;

                            #region 班別合計 Head Row (部別名稱 + 系所名稱 + 班別名稱)
                            {
                                rowNo++;
                                row = sheet.CreateRow(rowNo);

                                int colNo = 1;
                                cell = row.CreateCell(colNo, CellType.String);
                                cell.SetCellValue(deptName);
                                cell.SetMergedCount(0, 2);

                                colNo += 3;
                                cell = row.CreateCell(colNo, CellType.String);
                                cell.SetCellValue(majorName);
                                cell.SetMergedCount(0, 2);

                                colNo += 3;
                                cell = row.CreateCell(colNo, CellType.String);
                                cell.SetCellValue(className);
                                cell.SetMergedCount(0, 2);
                            }
                            #endregion

                            #region 班別合計 Data Row
                            {
                                rowNo++;
                                row = sheet.CreateRow(rowNo);

                                int colNo = 1;
                                foreach (KeyValue<string> receiveItem in receiveItems)
                                {
                                    string columnName = receiveItem.Value;
                                    cell = row.CreateCell(colNo, CellType.Numeric);
                                    if (!String.IsNullOrEmpty(columnName) && !classRow.IsNull(columnName))
                                    {
                                        double value = Double.Parse(classRow[columnName].ToString());
                                        cell.CellStyle = moneyCellStyle;
                                        cell.SetCellValue(value);
                                    }
                                    else
                                    {
                                        cell.CellStyle = moneyCellStyle;
                                        cell.SetCellValue("");
                                    }
                                    colNo++;
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }

                    if (pageRowCount >= pageSize)
                    {
                        pageNo++;
                        pageRowCount = 0;
                        rowNo = this.GenNextPageAndHead(sheet, rowNo, dtHead, pageNo, totalPage, receiveItems, pageHeadCellStyle, colTitleCellStyle);
                    }

                    #region 補最後的系所合計
                    {
                        pageRowCount++;

                        #region [MDY:20220530] Checkmarx 調整
                        #region [OLD]
                        //DataRow[] majorSumRows = dtMajorSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType, yearId, termId, depId, receiveId, oldDeptId, oldMajorId));
                        #endregion

                        DataRow[] majorSumRows = dtMajorSum.Rows.Cast<DataRow>().Where(
                            row => row["Receive_Type"].Equals(receiveType)
                                && row["Year_Id"].Equals(yearId)
                                && row["Term_Id"].Equals(termId)
                                && row["Dep_Id"].Equals(depId)
                                && row["Receive_Id"].Equals(receiveId)
                                && row["Dept_Id"].Equals(oldDeptId)
                                && row["Major_Id"].Equals(oldMajorId)).ToArray();
                        #endregion

                        rowNo = this.GenReportA2MajorSumRow(sheet, rowNo, majorSumRows, oldDeptName, oldMajorName, receiveItems, moneyCellStyle);

                        if (pageRowCount >= pageSize)
                        {
                            pageNo++;
                            pageRowCount = 0;
                            rowNo = this.GenNextPageAndHead(sheet, rowNo, dtHead, pageNo, totalPage, receiveItems, pageHeadCellStyle, colTitleCellStyle);
                        }
                    }
                    #endregion

                    #region 補最後的部別合計
                    {
                        pageRowCount++;

                        #region [MDY:20220530] Checkmarx 調整
                        #region [OLD]
                        //DataRow[] deptSumRows = dtDeptSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType, yearId, termId, depId, receiveId, oldDeptId));
                        #endregion

                        DataRow[] deptSumRows = dtDeptSum.Rows.Cast<DataRow>().Where(
                            row => row["Receive_Type"].Equals(receiveType)
                                && row["Year_Id"].Equals(yearId)
                                && row["Term_Id"].Equals(termId)
                                && row["Dep_Id"].Equals(depId)
                                && row["Receive_Id"].Equals(receiveId)
                                && row["Dept_Id"].Equals(oldDeptId)).ToArray();
                        #endregion

                        rowNo = this.GenReportA2DeptSumRow(sheet, rowNo, deptSumRows, oldDeptName, receiveItems, moneyCellStyle);
                    }
                    #endregion
                }
                #endregion

                #region 自動調整欄寬
                // [TODO] 目前不支援
                //for (int colNo = 1; colNo <= receiveItems.Count; colNo++)
                //{
                //    string txt = receiveItems[colNo].Key;
                //    if (txt.Length > 4)
                //    {
                //        //sheet.SetColumnWidth(colNo, txt.Length * 20 + 10);
                //        sheet.AutoSizeColumn(colNo);
                //    }
                //}
                #endregion

                #region 將 ODSWorkbook 轉成 byte[]
                wb.WriteToBytes(out content);
                #endregion

                sheet = null;
                wb = null;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg;
        }

        /// <summary>
        /// 產生繳費銷帳總表 A2 的分頁與表頭
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo">目前的 Row No</param>
        /// <param name="dtHead"></param>
        /// <param name="pageNo"></param>
        /// <param name="totalPage"></param>
        /// <param name="receiveItems"></param>
        /// <param name="pageHeadCellStyle"></param>
        /// <param name="colTitleCellStyle"></param>
        /// <returns>傳回最後產生的 Row No</returns>
        private int GenNextPageAndHead(ODSSheet sheet, int rowNo, DataTable dtHead, int pageNo, int totalPage, KeyValueList<string> receiveItems, ODSCellStyle pageHeadCellStyle, ODSCellStyle colTitleCellStyle)
        {
            int genRowNo = rowNo;

            #region 空白行+插入分頁
            {
                genRowNo++;
                sheet.CreateRow(genRowNo);
                sheet.SetRowBreak(genRowNo);
            }
            #endregion

            #region Gen 表頭
            {
                genRowNo = this.GenReportA2Head(sheet, genRowNo, dtHead, pageNo, totalPage, receiveItems, pageHeadCellStyle, colTitleCellStyle);
            }
            #endregion

            return genRowNo;
        }

        /// <summary>
        /// 產生繳費銷帳總表 A2 的表頭
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo">目前的 Row No</param>
        /// <param name="dtHead"></param>
        /// <param name="pageNo"></param>
        /// <param name="totalPage"></param>
        /// <param name="receiveItems"></param>
        /// <param name="pageHeadCellStyle"></param>
        /// <param name="colTitleCellStyle"></param>
        /// <returns>傳回最後產生的 Row No</returns>
        private int GenReportA2Head(ODSSheet sheet, int rowNo, DataTable dtHead, int pageNo, int totalPage, KeyValueList<string> receiveItems, ODSCellStyle pageHeadCellStyle, ODSCellStyle colTitleCellStyle)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;
            DataRow dRow = dtHead.Rows[0];

            int columnCount = receiveItems.Count;

            #region Head Row 0
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);

                int colNo = 1;
                string schName = dRow["Sch_Name"].ToString();
                string reportName = dRow["ReportName"].ToString();
                string value = String.Format("{0}          {1}", schName, reportName);
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
                cell.CellStyle = pageHeadCellStyle;
                cell.SetMergedCount(0, columnCount - 1);
            }
            #endregion

            #region Head Row 1
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);

                #region 學年
                {
                    int colNo = 1;
                    string value = String.Format("學年：{0}", dRow["Year_Name"].ToString());
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);
                    cell.SetMergedCount(0, 2);
                }
                #endregion

                #region 學期
                {
                    int colNo = 5;
                    string value = String.Format("學期：{0}", dRow["Term_Name"].ToString());
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);
                    cell.SetMergedCount(0, 2);
                }
                #endregion

                #region 繳費狀態
                {
                    int colNo = 9;
                    string value = String.Format("繳費狀態：{0}", dRow["ReceiveStatusName"].ToString());
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);
                    cell.SetMergedCount(0, 2);
                }
                #endregion
            }
            #endregion

            #region Head Row 2
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);

                #region 商家代號
                {
                    int colNo = 1;
                    string value = String.Format("商家代號：{0}", dRow["Receive_Type"].ToString());
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);
                    cell.SetMergedCount(0, 2);
                }
                #endregion

                #region 代收費用
                {
                    int colNo = 5;
                    string value = String.Format("代收費用：{0}", dRow["Receive_Name"].ToString());
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);
                    cell.SetMergedCount(0, 2);
                }
                #endregion

                #region 批號
                {
                    int colNo = 9;
                    string value = String.Format("批號：{0}", dRow["UpNo"].ToString());
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);
                    cell.SetMergedCount(0, 1);
                }
                #endregion

                #region 日期
                {
                    int colNo = columnCount - 1;
                    string value = dRow["ReportDate"].ToString();
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);
                }
                #endregion

                #region 頁數
                {
                    int colNo = columnCount;
                    string value = String.Format("第{0}頁/共{1}頁", pageNo, totalPage);
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);
                }
                #endregion
            }
            #endregion

            #region 收入科目名稱 Row
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);
                int colNo = 1;
                foreach (KeyValue<string> receiveItem in receiveItems)
                {
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(receiveItem.Key);
                    cell.CellStyle = colTitleCellStyle;
                    colNo++;
                }
            }
            #endregion

            return genRowNo;
        }

        /// <summary>
        /// 產生繳費銷帳總表 A2 的系所合計
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo">目前的 Row No</param>
        /// <param name="majorSumRows"></param>
        /// <param name="deptName"></param>
        /// <param name="majorName"></param>
        /// <param name="receiveItems"></param>
        /// <param name="moneyCellStyle"></param>
        /// <returns>傳回最後產生的 Row No</returns>
        private int GenReportA2MajorSumRow(ODSSheet sheet, int rowNo, DataRow[] majorSumRows, string deptName, string majorName, KeyValueList<string> receiveItems, ODSCellStyle moneyCellStyle)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            if (majorSumRows != null && majorSumRows.Length > 0)
            {
                //應該只有一筆，多筆表示 dtMajorSum 資料可能有錯，這裡用 foreach 是為了容易發現錯誤
                foreach (DataRow majorSumRow in majorSumRows)
                {
                    #region 系所合計 Head Row (部別名稱 + 系所名稱)
                    {
                        genRowNo++;
                        row = sheet.CreateRow(genRowNo);

                        int colNo = 1;
                        cell = row.CreateCell(colNo, CellType.String);
                        cell.SetCellValue(deptName);
                        cell.SetMergedCount(0, 2);

                        colNo += 3;
                        cell = row.CreateCell(colNo, CellType.String);
                        cell.SetCellValue(majorName);
                        cell.SetMergedCount(0, 2);

                        colNo += 3;
                        cell = row.CreateCell(colNo, CellType.String);
                        cell.SetCellValue("(系所合計)");
                        cell.SetMergedCount(0, 2);
                    }
                    #endregion

                    #region 系所合計 Data Row
                    {
                        genRowNo++;
                        row = sheet.CreateRow(genRowNo);

                        int colNo = 1;
                        foreach (KeyValue<string> receiveItem in receiveItems)
                        {
                            string columnName = receiveItem.Value;
                            cell = row.CreateCell(colNo, CellType.Numeric);
                            if (!String.IsNullOrEmpty(columnName) && !majorSumRow.IsNull(columnName))
                            {
                                double value = Double.Parse(majorSumRow[columnName].ToString());
                                cell.CellStyle = moneyCellStyle;
                                cell.SetCellValue(value);
                            }
                            else
                            {
                                cell.CellStyle = moneyCellStyle;
                                cell.SetCellValue("");
                            }
                            colNo++;
                        }
                    }
                    #endregion
                }
            }
            else
            {
                //應該會有資料，這裡只是為了容易發現錯誤
                #region 系所合計 Head Row (部別名稱 + 系所名稱)
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);

                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(deptName);
                    cell.SetMergedCount(0, 2);

                    colNo += 3;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(majorName);
                    cell.SetMergedCount(0, 2);

                    colNo += 3;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("(系所合計)");
                    cell.SetMergedCount(0, 2);
                }
                #endregion

                #region 系所合計 Data Row
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);

                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("查無資料");
                }
                #endregion
            }
            return genRowNo;
        }

        /// <summary>
        /// 產生繳費銷帳總表 A2 的部別合計
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo">目前的 Row No</param>
        /// <param name="deptSumRows"></param>
        /// <param name="deptName"></param>
        /// <param name="receiveItems"></param>
        /// <param name="moneyCellStyle"></param>
        /// <returns>傳回最後產生的 Row No</returns>
        private int GenReportA2DeptSumRow(ODSSheet sheet, int rowNo, DataRow[] deptSumRows, string deptName, KeyValueList<string> receiveItems, ODSCellStyle moneyCellStyle)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            if (deptSumRows != null && deptSumRows.Length > 0)
            {
                //應該只有一筆，多筆表示 dtMajorSum 資料可能有錯，這裡用 foreach 是為了容易發現錯誤
                foreach (DataRow deptSumRow in deptSumRows)
                {
                    #region 部別合計 Head Row (部別名稱)
                    {
                        genRowNo++;
                        row = sheet.CreateRow(genRowNo);

                        int colNo = 1;
                        cell = row.CreateCell(colNo, CellType.String);
                        cell.SetCellValue(deptName);
                        cell.SetMergedCount(0, 2);

                        colNo += 3;
                        cell = row.CreateCell(colNo, CellType.String);
                        cell.SetCellValue("(部別合計)");
                        cell.SetMergedCount(0, 2);
                    }
                    #endregion

                    #region 部別合計 Data Row
                    {
                        genRowNo++;
                        row = sheet.CreateRow(genRowNo);

                        int colNo = 1;
                        foreach (KeyValue<string> receiveItem in receiveItems)
                        {
                            string columnName = receiveItem.Value;
                            cell = row.CreateCell(colNo, CellType.Numeric);
                            if (!String.IsNullOrEmpty(columnName) && !deptSumRow.IsNull(columnName))
                            {
                                double value = Double.Parse(deptSumRow[columnName].ToString());
                                cell.CellStyle = moneyCellStyle;
                                cell.SetCellValue(value);
                            }
                            else
                            {
                                cell.CellStyle = moneyCellStyle;
                                cell.SetCellValue("");
                            }
                            colNo++;
                        }
                    }
                    #endregion
                }
            }
            else
            {
                //應該會有資料，這裡只是為了容易發現錯誤
                #region 部別合計 Head Row (部別名稱)
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);

                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(deptName);
                    cell.SetMergedCount(0, 2);

                    colNo += 3;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("(部別合計)");
                    cell.SetMergedCount(0, 2);
                }
                #endregion

                #region 部別合計 Data Row
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);

                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("查無資料");
                }
                #endregion
            }
            return genRowNo;
        }
        #endregion

        #region 繳費失敗總表(遲繳)
        /// <summary>
        /// 繳費失敗總表(遲繳)
        /// </summary>
        /// <param name="dtHead">表頭資料</param>
        /// <param name="dtPage">分頁資料</param>
        /// <param name="dtDeptSum">部別小計資料</param>
        /// <param name="dtMajorSum">系所小計資料</param>
        /// <param name="dtClassSum">班別小計資料</param>
        /// <param name="receiveItems">收入科目資料</param>
        /// <param name="dtData">表身資料</param>
        /// <returns>成功則傳回 null，否怎傳回錯誤訊息</returns>
        public string GenReportA(DataTable dtHead, DataTable dtPage, DataTable dtDeptSum, DataTable dtMajorSum, DataTable dtClassSum, KeyValueList<string> receiveItems, DataTable dtData, out byte[] content)
        {
            content = null;
            string errmsg = null;
            try
            {
                #region 使用 ODSWorkbook 產生 ODS
                string sheetName = "Sheet1";

                ODSWorkbook wb = new ODSWorkbook();
                ODSSheet sheet = wb.CreateSheet(sheetName);

                #region 指定紙張大小
                wb.PageStyle.PaperSize = PapeSizeKind.A4;
                #endregion

                #region 指定直式或橫式 Lendscape=橫式 Portrait=直式
                wb.PageStyle.PaperOrientation = PageOrientationKind.Lendscape;
                #endregion

                #region 藏隱格線
                wb.IsShowGrid = false;
                #endregion

                int rowNo = 0;
                int pageNo = 0;
                int totalPage = dtPage.Rows.Count;

                if (totalPage == 0)
                {
                    #region Gen 表頭
                    {
                        rowNo = this.GenReportAHead(sheet, rowNo, dtHead, pageNo, totalPage);
                    }
                    #endregion

                    #region 查無資料
                    rowNo++;
                    ODSRow row = sheet.CreateRow(rowNo);
                    int colNo = 1;
                    ODSCell cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("查無資料");
                    #endregion
                }
                else
                {
                    foreach (DataRow pageRow in dtPage.Rows)
                    {
                        pageNo++;

                        string receiveType = pageRow["Receive_Type"].ToString().Replace("'", "");
                        string yearId = pageRow["Year_Id"].ToString().Replace("'", "");
                        string termId = pageRow["Term_Id"].ToString().Replace("'", "");
                        string depId = pageRow["Dep_Id"].ToString().Replace("'", "");
                        string receiveId = pageRow["Receive_Id"].ToString().Replace("'", "");
                        string deptId = pageRow["Dept_Id"].ToString().Replace("'", "");

                        #region Gen 表頭
                        {
                            rowNo = this.GenReportAHead(sheet, rowNo, dtHead, pageNo, totalPage);
                        }
                        #endregion

                        #region Gen 表身 + 小計
                        {
                            rowNo = this.GenReportAData(sheet, rowNo, dtDeptSum, dtMajorSum, dtClassSum, receiveItems, dtData, receiveType, yearId, termId, depId, receiveId, deptId);
                        }
                        #endregion

                        #region 空白行+插入分頁
                        rowNo++;
                        sheet.CreateRow(rowNo);
                        sheet.SetRowBreak(rowNo);
                        #endregion
                    }
                }
                #endregion

                #region 將 ODSWorkbook 轉成 byte[]
                wb.WriteToBytes(out content);
                #endregion

                sheet = null;
                wb = null;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg;
        }

        /// <summary>
        /// 產生表頭
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo"></param>
        /// <param name="dtHead"></param>
        /// <param name="pageNo"></param>
        /// <param name="totalPage"></param>
        /// <returns>傳回最後的 Row No</returns>
        private int GenReportAHead(ODSSheet sheet, int rowNo, DataTable dtHead, int pageNo, int totalPage)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            #region Head Row 0
            genRowNo++;
            row = sheet.CreateRow(genRowNo);
            DataRow dRow = dtHead.Rows[0];

            #region 學校名稱
            {
                int colNo = 1;
                string value = dRow["Sch_Name"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 報表名稱
            {
                int colNo = 4;
                string value = dRow["ReportName"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion
            #endregion

            #region Head Row 1
            genRowNo++;
            row = sheet.CreateRow(genRowNo);

            #region 學年
            {
                int colNo = 1;
                string value = "學年：";
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);

                colNo = 2;
                value = dRow["Year_Name"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 學期
            {
                int colNo = 5;
                string value = "學期：";
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);

                colNo = 6;
                value = dRow["Term_Name"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 繳費狀態
            {
                string receiveStatusName = dRow["ReceiveStatusName"].ToString();
                if (!String.IsNullOrEmpty(receiveStatusName))
                {
                    int colNo = 9;
                    string value = "繳費狀態：";
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);

                    colNo = 10;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(receiveStatusName);
                }
            }
            #endregion
            #endregion

            #region Head Row 2
            genRowNo++;
            row = sheet.CreateRow(genRowNo);

            #region 商家代號
            {
                int colNo = 1;
                string value = "商家代號：";
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);

                colNo = 2;
                value = dRow["Receive_Type"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 代收費用
            {
                int colNo = 5;
                string value = "代收費用：";
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);

                colNo = 6;
                value = dRow["Receive_Name"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 批號
            {
                string upNo = dRow["UpNo"].ToString();
                if (!String.IsNullOrEmpty(upNo))
                {
                    int colNo = 9;
                    string value = "批號：";
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);

                    colNo = 10;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(upNo);
                }
            }
            #endregion

            #region 日期
            {
                int colNo = 12;
                string value = dRow["ReportDate"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 頁數
            {
                int colNo = 13;
                string value = String.Format("第{0}頁/共{1}頁", pageNo, totalPage);
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion
            #endregion

            return genRowNo;
        }

        /// <summary>
        /// 產生表身 + 小計
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo"></param>
        /// <param name="dtDeptSum"></param>
        /// <param name="dtMajorSum"></param>
        /// <param name="dtClassSum"></param>
        /// <param name="receiveItems"></param>
        /// <param name="dtData"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        private int GenReportAData(ODSSheet sheet, int rowNo
            , DataTable dtDeptSum, DataTable dtMajorSum, DataTable dtClassSum, KeyValueList<string> receiveItems, DataTable dtData
            , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            #region [MDY:20220530] Checkmarx 調整
            #region [OLD]
            //DataRow[] deptRows = dtDeptSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType, yearId, termId, depId, receiveId, deptId));
            #endregion

            DataRow[] deptRows = dtDeptSum.Rows.Cast<DataRow>().Where(
                    drow => drow["Receive_Type"].Equals(receiveType)
                        && drow["Year_Id"].Equals(yearId)
                        && drow["Term_Id"].Equals(termId)
                        && drow["Dep_Id"].Equals(depId)
                        && drow["Receive_Id"].Equals(receiveId)
                        && drow["Dept_Id"].Equals(deptId)).ToArray();
            #endregion

            if (deptRows != null && deptRows.Length > 0)
            {
                DataRow deptRow = deptRows[0];  //只會有一筆
                string deptName = deptRow["Dept_Name"].ToString();

                #region [MDY:20220530] Checkmarx 調整
                #region [OLD]
                //DataRow[] majorRows = dtMajorSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType, yearId, termId, depId, receiveId, deptId), "Major_Id");
                #endregion

                DataRow[] majorRows = dtMajorSum.Rows.Cast<DataRow>().Where(
                        drow => drow["Receive_Type"].Equals(receiveType)
                            && drow["Year_Id"].Equals(yearId)
                            && drow["Term_Id"].Equals(termId)
                            && drow["Dep_Id"].Equals(depId)
                            && drow["Receive_Id"].Equals(receiveId)
                            && drow["Dept_Id"].Equals(deptId)
                        ).OrderBy(drow => drow.Field<string>("Major_Id")).ToArray();
                #endregion

                if (majorRows != null && majorRows.Length > 0)
                {
                    foreach (DataRow majorRow in majorRows)
                    {
                        string majorId = majorRow["Major_Id"].ToString();
                        string majorName = majorRow["Major_Name"].ToString();

                        #region 班別
                        #region [MDY:20220530] Checkmarx 調整
                        #region [OLD]
                        //DataRow[] classRows = dtClassSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType, yearId, termId, depId, receiveId, deptId, majorId), "Class_Id");
                        #endregion

                        DataRow[] classRows = dtClassSum.Rows.Cast<DataRow>().Where(
                                drow => drow["Receive_Type"].Equals(receiveType)
                                    && drow["Year_Id"].Equals(yearId)
                                    && drow["Term_Id"].Equals(termId)
                                    && drow["Dep_Id"].Equals(depId)
                                    && drow["Receive_Id"].Equals(receiveId)
                                    && drow["Dept_Id"].Equals(deptId)
                                    && drow["Major_Id"].Equals(majorId)
                                ).OrderBy(drow => drow.Field<string>("Class_Id")).ToArray();
                        #endregion

                        if (classRows != null && classRows.Length > 0)
                        {
                            foreach (DataRow classRow in classRows)
                            {
                                string classId = classRow["Class_Id"].ToString();
                                string className = classRow["Class_Name"].ToString();

                                genRowNo++;
                                genRowNo = this.GenReportAForClassData(sheet, genRowNo, deptName, majorName, className, receiveItems, dtData, receiveType, yearId, termId, depId, receiveId, deptId, majorId, classId);

                                #region 班別合計
                                {
                                    genRowNo++;
                                    row = sheet.CreateRow(genRowNo);

                                    #region 班別人數
                                    int colNo = 1;
                                    cell = row.CreateCell(colNo, CellType.String);
                                    cell.SetCellValue("班別人數：");

                                    int sumCount = Int32.Parse(classRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                                    colNo++;
                                    cell = row.CreateCell(colNo, CellType.Numeric);
                                    cell.SetCellValue(sumCount);
                                    #endregion

                                    #region 班別金額
                                    colNo = 4;
                                    cell = row.CreateCell(colNo, CellType.String);
                                    cell.SetCellValue("班別金額：");

                                    double sumAmount = Double.Parse(classRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                                    colNo++;
                                    cell = row.CreateCell(colNo, CellType.Numeric);
                                    cell.SetCellValue(sumAmount);
                                    #endregion
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            genRowNo = this.GenReportAForMajorData(sheet, genRowNo, deptName, majorName, receiveItems, dtData, receiveType, yearId, termId, depId, receiveId, deptId, majorId);
                        }
                        #endregion

                        #region 系所合計
                        {
                            genRowNo++;
                            row = sheet.CreateRow(genRowNo);

                            #region 系所人數
                            int colNo = 1;
                            cell = row.CreateCell(colNo, CellType.String);
                            cell.SetCellValue("系所人數：");

                            int sumCount = Int32.Parse(majorRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                            colNo++;
                            cell = row.CreateCell(colNo, CellType.Numeric);
                            cell.SetCellValue(sumCount);
                            #endregion

                            #region 系所金額
                            colNo = 4;
                            cell = row.CreateCell(colNo, CellType.String);
                            cell.SetCellValue("系所金額：");

                            double sumAmount = Double.Parse(majorRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                            colNo++;
                            cell = row.CreateCell(colNo, CellType.Numeric);
                            cell.SetCellValue(sumAmount);
                            #endregion
                        }
                        #endregion
                    }
                }
                else
                {
                    genRowNo = this.GenReportAForDeptData(sheet, genRowNo, deptName, receiveItems, dtData, receiveType, yearId, termId, depId, receiveId, deptId);
                }

                #region 部別合計
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);

                    #region 部別人數
                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("部別人數：");

                    int sumCount = Int32.Parse(deptRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                    colNo++;
                    cell = row.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(sumCount);
                    #endregion

                    #region 部別金額
                    colNo = 4;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("部別金額：");

                    double sumAmount = Double.Parse(deptRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                    colNo++;
                    cell = row.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(sumAmount);
                    #endregion
                }
                #endregion
            }
            else
            {
                #region 部別合計
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);

                    #region 部別人數
                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("部別人數：");

                    int sumCount = 0;
                    colNo++;
                    cell = row.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(sumCount);
                    #endregion

                    #region 部別金額
                    colNo = 4;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("部別金額：");

                    double sumAmount = 0;
                    colNo++;
                    cell = row.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(sumAmount);
                    #endregion
                }
                #endregion
            }
            return genRowNo;
        }

        private int GenReportAForClassData(ODSSheet sheet, int rowNo
            , string deptName, string majorName, string className
            , KeyValueList<string> receiveItems, DataTable dtData
            , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId, string majorId, string classId)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            #region Data Row 0
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);

                #region 部別名稱
                {
                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(deptName);
                }
                #endregion

                #region 系所名稱
                {
                    int colNo = 3;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(majorName);
                }
                #endregion

                #region 班別名稱
                {
                    int colNo = 5;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(className);
                }
                #endregion
            }
            #endregion

            #region Data Row 1
            genRowNo++;
            row = sheet.CreateRow(genRowNo);
            #region 收入科目名稱
            {
                int colNo = 1;
                foreach (KeyValue<string> receiveItem in receiveItems)
                {
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(receiveItem.Key);
                    colNo++;
                }
            }
            #endregion
            #endregion

            #region Data Row 2 ~ dRows.Count + 1
            #region [MDY:20220530] Checkmarx 調整
            #region [OLD]
            //DataRow[] dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Class_Id='{7}'", receiveType, yearId, termId, depId, receiveId, deptId, majorId, classId), "Stu_Id");
            #endregion

            DataRow[] dRows = dtData.Rows.Cast<DataRow>().Where(
                    drow => drow["Receive_Type"].Equals(receiveType)
                        && drow["Year_Id"].Equals(yearId)
                        && drow["Term_Id"].Equals(termId)
                        && drow["Dep_Id"].Equals(depId)
                        && drow["Receive_Id"].Equals(receiveId)
                        && drow["Dept_Id"].Equals(deptId)
                        && drow["Major_Id"].Equals(majorId)
                        && drow["Class_Id"].Equals(classId)
                    ).OrderBy(drow => drow.Field<string>("Stu_Id")).ToArray();
            #endregion

            if (dRows != null && dRows.Length > 0)
            {
                foreach (DataRow dRow in dRows)
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);
                    int colNo = 1;
                    foreach (KeyValue<string> receiveItem in receiveItems)
                    {
                        string columnName = receiveItem.Value;
                        cell = row.CreateCell(colNo, CellType.Numeric);
                        if (!dRow.IsNull(columnName))
                        {
                            double value = Double.Parse(dRow[columnName].ToString());
                            cell.SetCellValue(value);
                        }
                        colNo++;
                    }
                }
            }
            #endregion

            return genRowNo;
        }

        private int GenReportAForMajorData(ODSSheet sheet, int rowNo
            , string deptName, string majorName
            , KeyValueList<string> receiveItems, DataTable dtData
            , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId, string majorId)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            #region Data Row 0
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);

                #region 部別名稱
                {
                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(deptName);
                }
                #endregion

                #region 系所名稱
                {
                    int colNo = 3;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(majorName);
                }
                #endregion
            }
            #endregion

            #region Data Row 1
            genRowNo++;
            row = sheet.CreateRow(genRowNo);
            #region 收入科目名稱
            {
                int colNo = 1;
                foreach (KeyValue<string> receiveItem in receiveItems)
                {
                    cell = row.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(receiveItem.Key);
                    colNo++;
                }
            }
            #endregion
            #endregion

            #region Data Row 2 ~ dRows.Count + 1
            #region [MDY:20220530] Checkmarx 調整
            #region [OLD]
            //DataRow[] dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType, yearId, termId, depId, receiveId, deptId, majorId), "Stu_Id");
            #endregion

            DataRow[] dRows = dtData.Rows.Cast<DataRow>().Where(
                    drow => drow["Receive_Type"].Equals(receiveType)
                        && drow["Year_Id"].Equals(yearId)
                        && drow["Term_Id"].Equals(termId)
                        && drow["Dep_Id"].Equals(depId)
                        && drow["Receive_Id"].Equals(receiveId)
                        && drow["Dept_Id"].Equals(deptId)
                        && drow["Major_Id"].Equals(majorId)
                    ).OrderBy(drow => drow.Field<string>("Stu_Id")).ToArray();
            #endregion

            if (dRows != null && dRows.Length > 0)
            {
                foreach (DataRow dRow in dRows)
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);
                    int colNo = 1;
                    foreach (KeyValue<string> receiveItem in receiveItems)
                    {
                        string columnName = receiveItem.Value;
                        cell = row.CreateCell(colNo, CellType.Numeric);
                        if (!dRow.IsNull(columnName))
                        {
                            double value = Double.Parse(dRow[columnName].ToString());
                            cell.SetCellValue(value);
                        }
                        colNo++;
                    }
                }
            }
            #endregion

            return genRowNo;
        }

        private int GenReportAForDeptData(ODSSheet sheet, int rowNo
            , string deptName
            , KeyValueList<string> receiveItems, DataTable dtData
            , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            #region Data Row 0
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);

                #region 部別名稱
                {
                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(deptName);
                }
                #endregion
            }
            #endregion

            #region Data Row 1
            genRowNo++;
            row = sheet.CreateRow(genRowNo);
            #region 收入科目名稱
            {
                int colNo = 1;
                foreach (KeyValue<string> receiveItem in receiveItems)
                {
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(receiveItem.Key);
                    colNo++;
                }
            }
            #endregion
            #endregion

            #region Data Row 2 ~ dRows.Count + 1
            #region [MDY:20220530] Checkmarx 調整
            #region [OLD]
            //DataRow[] dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType, yearId, termId, depId, receiveId, deptId), "Stu_Id");
            #endregion

            DataRow[] dRows = dtData.Rows.Cast<DataRow>().Where(
                    drow => drow["Receive_Type"].Equals(receiveType)
                        && drow["Year_Id"].Equals(yearId)
                        && drow["Term_Id"].Equals(termId)
                        && drow["Dep_Id"].Equals(depId)
                        && drow["Receive_Id"].Equals(receiveId)
                        && drow["Dept_Id"].Equals(deptId)
                    ).OrderBy(drow => drow.Field<string>("Stu_Id")).ToArray();
            #endregion

            if (dRows != null && dRows.Length > 0)
            {
                foreach (DataRow dRow in dRows)
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);
                    int colNo = 1;
                    foreach (KeyValue<string> receiveItem in receiveItems)
                    {
                        string columnName = receiveItem.Value;
                        cell = row.CreateCell(colNo, CellType.Numeric);
                        if (!dRow.IsNull(columnName))
                        {
                            double value = Double.Parse(dRow[columnName].ToString());
                            cell.SetCellValue(value);
                        }
                        colNo++;
                    }
                }
            }
            #endregion

            return genRowNo;
        }
        #endregion

        #region 繳費銷帳明細表 (正常、遲繳)
        /// <summary>
        /// 繳費銷帳明細表 (正常、遲繳)
        /// </summary>
        /// <param name="dtHead">表頭資料</param>
        /// <param name="dtPage">分頁資料</param>
        /// <param name="dtDeptSum">部別小計資料</param>
        /// <param name="dtMajorSum">系所小計資料</param>
        /// <param name="dtClassSum">班別小計資料</param>
        /// <param name="receiveItems">收入科目資料</param>
        /// <param name="dtData">表身資料</param>
        /// <param name="content">成功則傳回產生檔案的 byte 陣列，否則傳回 null</param>
        /// <returns>成功則傳回 null，否怎傳回錯誤訊息</returns>
        public string GenReportB(DataTable dtHead, DataTable dtPage, DataTable dtDeptSum, DataTable dtMajorSum, DataTable dtClassSum, KeyValueList<string> receiveItems, DataTable dtData, out byte[] content)
        {
            content = null;
            string errmsg = null;
            try
            {
                #region 使用 ODSWorkbook 產生 ODS
                string sheetName = "Sheet1";

                ODSWorkbook wb = new ODSWorkbook();
                ODSSheet sheet = wb.CreateSheet(sheetName);
                ODSRow row = null;
                ODSCell cell = null;

                #region 指定紙張大小
                wb.PageStyle.PaperSize = PapeSizeKind.A4;
                #endregion

                #region 指定直式或橫式 Lendscape=橫式 Portrait=直式
                wb.PageStyle.PaperOrientation = PageOrientationKind.Lendscape;
                #endregion

                #region 藏隱格線
                wb.IsShowGrid = false;
                #endregion

                int rowNo = 0;
                int pageNo = 0;
                int totalPage = dtPage.Rows.Count;

                if (totalPage == 0)
                {
                    #region Gen 表頭
                    {
                        rowNo = this.GenReportBHead(sheet, rowNo, dtHead, pageNo, totalPage);
                    }
                    #endregion

                    #region 查無資料
                    rowNo++;
                    row = sheet.CreateRow(rowNo);
                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("查無資料");
                    #endregion
                }
                else
                {
                    int idx = 0;
                    DataRow pageRow = dtPage.Rows[0];
                    while (pageRow != null && idx < totalPage)
                    {
                        pageNo++;

                        string receiveType = pageRow["Receive_Type"].ToString().Replace("'", "");
                        string yearId = pageRow["Year_Id"].ToString().Replace("'", "");
                        string termId = pageRow["Term_Id"].ToString().Replace("'", "");
                        string depId = pageRow["Dep_Id"].ToString().Replace("'", "");
                        string receiveId = pageRow["Receive_Id"].ToString().Replace("'", "");
                        string deptId = pageRow["Dept_Id"].ToString().Replace("'", "");
                        string majorId = pageRow["Major_Id"].ToString().Replace("'", "");
                        string classId = pageRow["Class_Id"].ToString().Replace("'", "");
                        string deptName = pageRow["Dept_Name"].ToString();
                        string majorName = pageRow["Major_Name"].ToString();
                        string className = pageRow["Class_Name"].ToString();
                        string deptKey = deptId.Trim();
                        string majorKey = String.Concat(deptId.Trim(), ",", majorId.Trim());

                        #region Gen 表頭
                        {
                            rowNo = this.GenReportBHead(sheet, rowNo, dtHead, pageNo, totalPage);
                        }
                        #endregion

                        #region Gen 表身
                        {
                            rowNo++;
                            rowNo = this.GenReportBForClassData(sheet, rowNo, deptName, majorName, className, receiveItems, dtData, receiveType, yearId, termId, depId, receiveId, deptId, majorId, classId);
                        }
                        #endregion

                        #region 班級合計
                        {
                            int sumCount = 0;
                            double sumAmount = 0;

                            #region [MDY:20220530] Checkmarx 調整
                            #region [OLD]
                            //DataRow[] classRows = dtClassSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Class_Id='{7}'", receiveType, yearId, termId, depId, receiveId, deptId, majorId, classId));
                            #endregion

                            DataRow[] classRows = dtClassSum.Rows.Cast<DataRow>().Where(
                                drow => drow["Receive_Type"].Equals(receiveType)
                                    && drow["Year_Id"].Equals(yearId)
                                    && drow["Term_Id"].Equals(termId)
                                    && drow["Dep_Id"].Equals(depId)
                                    && drow["Receive_Id"].Equals(receiveId)
                                    && drow["Dept_Id"].Equals(deptId)
                                    && drow["Major_Id"].Equals(majorId)
                                    && drow["Class_Id"].Equals(classId)).ToArray();
                            #endregion

                            if (classRows != null && classRows.Length > 0)
                            {
                                DataRow classRow = classRows[0];
                                sumCount = Int32.Parse(classRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                                sumAmount = Double.Parse(classRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                            }

                            rowNo++;
                            row = sheet.CreateRow(rowNo);

                            #region 班別人數
                            int colNo = 1;
                            cell = row.CreateCell(colNo, CellType.String);
                            cell.SetCellValue("班別人數：");

                            colNo++;
                            cell = row.CreateCell(colNo, CellType.Numeric);
                            cell.SetCellValue(sumCount);
                            #endregion

                            #region 班別金額
                            colNo = 4;
                            cell = row.CreateCell(colNo, CellType.String);
                            cell.SetCellValue("班別金額：");

                            colNo++;
                            cell = row.CreateCell(colNo, CellType.Numeric);
                            cell.SetCellValue(sumAmount);
                            #endregion
                        }
                        #endregion

                        idx++;
                        string nextDeptKey = null;
                        string nextMajorKey = null;
                        if (idx < totalPage)
                        {
                            pageRow = dtPage.Rows[idx];
                            string nextDeptId = pageRow["Dept_Id"].ToString();
                            string nextMajorId = pageRow["Major_Id"].ToString();
                            nextDeptKey = nextDeptId.Trim();
                            nextMajorKey = String.Concat(nextDeptId.Trim(), ",", nextMajorId.Trim());
                        }
                        else
                        {
                            pageRow = null;
                        }

                        #region 系所合計
                        if (majorKey != nextMajorKey)
                        {
                            int sumCount = 0;
                            double sumAmount = 0;

                            #region [MDY:20220530] Checkmarx 調整
                            #region [OLD]
                            //DataRow[] majorRows = dtMajorSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType, yearId, termId, depId, receiveId, deptId, majorId));
                            #endregion

                            DataRow[] majorRows = dtMajorSum.Rows.Cast<DataRow>().Where(
                                drow => drow["Receive_Type"].Equals(receiveType)
                                    && drow["Year_Id"].Equals(yearId)
                                    && drow["Term_Id"].Equals(termId)
                                    && drow["Dep_Id"].Equals(depId)
                                    && drow["Receive_Id"].Equals(receiveId)
                                    && drow["Dept_Id"].Equals(deptId)
                                    && drow["Major_Id"].Equals(majorId)).ToArray();
                            #endregion

                            if (majorRows != null && majorRows.Length > 0)
                            {
                                DataRow majorRow = majorRows[0];
                                sumCount = Int32.Parse(majorRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                                sumAmount = Double.Parse(majorRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                            }

                            rowNo++;
                            row = sheet.CreateRow(rowNo);

                            #region 系所人數
                            int colNo = 1;
                            cell = row.CreateCell(colNo, CellType.String);
                            cell.SetCellValue("系所人數：");

                            colNo++;
                            cell = row.CreateCell(colNo, CellType.Numeric);
                            cell.SetCellValue(sumCount);
                            #endregion

                            #region 系所金額
                            colNo = 4;
                            cell = row.CreateCell(colNo, CellType.String);
                            cell.SetCellValue("系所金額：");

                            colNo++;
                            cell = row.CreateCell(colNo, CellType.Numeric);
                            cell.SetCellValue(sumAmount);
                            #endregion
                        }
                        #endregion

                        #region 部別合計
                        if (deptKey != nextDeptKey)
                        {
                            int sumCount = 0;
                            double sumAmount = 0;

                            #region [MDY:20220530] Checkmarx 調整
                            #region [OLD]
                            //DataRow[] deptRows = dtDeptSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType, yearId, termId, depId, receiveId, deptId));
                            #endregion

                            DataRow[] deptRows = dtDeptSum.Rows.Cast<DataRow>().Where(
                                drow => drow["Receive_Type"].Equals(receiveType)
                                    && drow["Year_Id"].Equals(yearId)
                                    && drow["Term_Id"].Equals(termId)
                                    && drow["Dep_Id"].Equals(depId)
                                    && drow["Receive_Id"].Equals(receiveId)
                                    && drow["Dept_Id"].Equals(deptId)).ToArray();
                            #endregion

                            if (deptRows != null && deptRows.Length > 0)
                            {
                                DataRow deptRow = deptRows[0];
                                sumCount = Int32.Parse(deptRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                                sumAmount = Double.Parse(deptRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                            }
                            rowNo++;
                            row = sheet.CreateRow(rowNo);

                            #region 部別人數
                            int colNo = 1;
                            cell = row.CreateCell(colNo, CellType.String);
                            cell.SetCellValue("部別人數：");

                            colNo++;
                            cell = row.CreateCell(colNo, CellType.Numeric);
                            cell.SetCellValue(sumCount);
                            #endregion

                            #region 部別金額
                            colNo = 4;
                            cell = row.CreateCell(colNo, CellType.String);
                            cell.SetCellValue("部別金額：");

                            colNo++;
                            cell = row.CreateCell(colNo, CellType.Numeric);
                            cell.SetCellValue(sumAmount);
                            #endregion
                        }
                        #endregion

                        #region 空白行+插入分頁
                        rowNo++;
                        sheet.CreateRow(rowNo);
                        sheet.SetRowBreak(rowNo);
                        #endregion
                    }
                }
                #endregion

                #region 將 ODSWorkbook 轉成 byte[]
                wb.WriteToBytes(out content);
                #endregion

                sheet = null;
                wb = null;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg;
        }

        /// <summary>
        /// 產生表頭
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo"></param>
        /// <param name="dtHead"></param>
        /// <param name="pageNo"></param>
        /// <param name="totalPage"></param>
        /// <returns>傳回最後的 Row No</returns>
        private int GenReportBHead(ODSSheet sheet, int rowNo, DataTable dtHead, int pageNo, int totalPage)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            #region Head Row 0
            genRowNo++;
            row = sheet.CreateRow(genRowNo);
            DataRow dRow = dtHead.Rows[0];

            #region 學校名稱
            {
                int colNo = 1;
                string value = dRow["Sch_Name"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 報表名稱
            {
                int colNo = 4;
                string value = dRow["ReportName"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion
            #endregion

            #region Head Row 1
            genRowNo++;
            row = sheet.CreateRow(genRowNo);

            #region 學年
            {
                int colNo = 1;
                string value = "學年：";
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);

                colNo = 2;
                value = dRow["Year_Name"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 學期
            {
                int colNo = 5;
                string value = "學期：";
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);

                colNo = 6;
                value = dRow["Term_Name"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 繳費狀態
            {
                string receiveStatusName = dRow["ReceiveStatusName"].ToString();
                if (!String.IsNullOrEmpty(receiveStatusName))
                {
                    int colNo = 9;
                    string value = "繳費狀態：";
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);

                    colNo = 10;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(receiveStatusName);
                }
            }
            #endregion
            #endregion

            #region Head Row 2
            genRowNo++;
            row = sheet.CreateRow(genRowNo);

            #region 商家代號
            {
                int colNo = 1;
                string value = "商家代號：";
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);

                colNo = 2;
                value = dRow["Receive_Type"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 代收費用
            {
                int colNo = 5;
                string value = "代收費用：";
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);

                colNo = 6;
                value = dRow["Receive_Name"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 批號
            {
                string upNo = dRow["UpNo"].ToString();
                if (!String.IsNullOrEmpty(upNo))
                {
                    int colNo = 9;
                    string value = "批號：";
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);

                    colNo = 10;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(upNo);
                }
            }
            #endregion

            #region 日期
            {
                int colNo = 12;
                string value = dRow["ReportDate"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 頁數
            {
                int colNo = 13;
                string value = String.Format("第{0}頁/共{1}頁", pageNo, totalPage);
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion
            #endregion

            return genRowNo;
        }

        /// <summary>
        /// 產生表身
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo"></param>
        /// <param name="deptName"></param>
        /// <param name="majorName"></param>
        /// <param name="className"></param>
        /// <param name="receiveItems"></param>
        /// <param name="dtData"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="deptId"></param>
        /// <param name="majorId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        private int GenReportBForClassData(ODSSheet sheet, int rowNo
            , string deptName, string majorName, string className
            , KeyValueList<string> receiveItems, DataTable dtData
            , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId, string majorId, string classId)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            #region Data Row 0
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);

                #region 部別名稱
                {
                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(deptName);
                }
                #endregion

                #region 系所名稱
                {
                    int colNo = 3;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(majorName);
                }
                #endregion

                #region 班別名稱
                {
                    int colNo = 5;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(className);
                }
                #endregion
            }
            #endregion

            #region Data Row 1
            genRowNo++;
            row = sheet.CreateRow(genRowNo);
            #region 收入科目名稱
            {
                int colNo = 1;
                foreach (KeyValue<string> receiveItem in receiveItems)
                {
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(receiveItem.Key);
                    colNo++;
                }
            }
            #endregion
            #endregion

            #region Data Row 2 ~ dRows.Count + 1
            #region [MDY:20220530] Checkmarx 調整
            #region [OLD]
            //DataRow[] dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Class_Id='{7}'", receiveType, yearId, termId, depId, receiveId, deptId, majorId, classId), "Stu_Id");
            #endregion

            DataRow[] dRows = dtData.Rows.Cast<DataRow>().Where(
                    drow => drow["Receive_Type"].Equals(receiveType)
                        && drow["Year_Id"].Equals(yearId)
                        && drow["Term_Id"].Equals(termId)
                        && drow["Dep_Id"].Equals(depId)
                        && drow["Receive_Id"].Equals(receiveId)
                        && drow["Dept_Id"].Equals(deptId)
                        && drow["Major_Id"].Equals(majorId)
                        && drow["Class_Id"].Equals(classId)
                    ).OrderBy(drow => drow.Field<string>("Stu_Id")).ToArray();
            #endregion

            if (dRows != null && dRows.Length > 0)
            {
                foreach (DataRow dRow in dRows)
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);
                    int colNo = 1;
                    foreach (KeyValue<string> receiveItem in receiveItems)
                    {
                        string columnName = receiveItem.Value;
                        if (!dRow.IsNull(columnName))
                        {
                            //[MDY:20170702] 排除以 Receive_ 開頭但非數值的欄位
                            if (columnName.StartsWith("Receive_") && dtData.Columns[columnName].DataType.FullName == "System.Decimal")
                            {
                                cell = row.CreateCell(colNo, CellType.Numeric);
                                double value = Double.Parse(dRow[columnName].ToString());
                                cell.SetCellValue(value);
                            }
                            else
                            {
                                cell = row.CreateCell(colNo, CellType.String);
                                string value = dRow[columnName].ToString();
                                cell.SetCellValue(value);
                            }
                        }
                        colNo++;
                    }
                }
            }
            #endregion

            return genRowNo;
        }
        #endregion

        #region 學生繳費名冊
        /// <summary>
        /// 學生繳費名冊
        /// </summary>
        /// <param name="dtHead">表頭資料</param>
        /// <param name="dtPage">分頁資料</param>
        /// <param name="dtDeptSum">部別小計資料</param>
        /// <param name="dtMajorSum">系所小計資料</param>
        /// <param name="dtGradeSum">年級小計資料</param>
        /// <param name="dtClassSum">班別小計資料</param>
        /// <param name="dtData">表身資料</param>
        /// <param name="otherFields">說明項目欄位集合</param>
        /// <param name="content">成功則傳回產生檔案的 byte 陣列，否則傳回 null</param>
        /// <returns>成功則傳回 null，否怎傳回錯誤訊息</returns>
        public string GenReportC(DataTable dtHead, DataTable dtPage, DataTable dtDeptSum, DataTable dtMajorSum, DataTable dtGradeSum, DataTable dtClassSum, DataTable dtData, KeyValueList<string> otherFields, out byte[] content)
        {
            content = null;
            string errmsg = null;
            try
            {
                #region 使用 ODSWorkbook 產生 ODS
                string sheetName = "Sheet1";

                ODSWorkbook wb = new ODSWorkbook();
                ODSSheet sheet = wb.CreateSheet(sheetName);
                ODSRow row = null;
                ODSCell cell = null;

                #region 指定紙張大小 A3=8, A4=9, Letter=1
                wb.PageStyle.PaperSize = PapeSizeKind.A4;
                #endregion

                #region 指定直式或橫式 Lendscape=橫式 Portrait=直式
                wb.PageStyle.PaperOrientation = PageOrientationKind.Lendscape;
                #endregion

                #region 藏隱格線
                wb.IsShowGrid = false;
                #endregion

                #region Page Head (粗體、14字、置中)儲存格格式
                ODSCellStyle pageHeadCellStyle = wb.CreateCellStyle();
                {
                    ODSFont pageHeadFont = wb.CreateFont();
                    pageHeadFont.IsBold = true;
                    pageHeadFont.FontSize = 14D;
                    pageHeadCellStyle.SetFont(pageHeadFont);
                    pageHeadCellStyle.HAlignment = HorizontalAlignment.Center;
                }
                #endregion

                #region Column Title (粗體、底線)儲存格格式
                ODSCellStyle colTitleCellStyle = wb.CreateCellStyle();
                {
                    ODSFont colTitleFont = wb.CreateFont();
                    colTitleFont.IsBold = true;
                    colTitleCellStyle.SetFont(colTitleFont);
                    colTitleCellStyle.BottomBorderStyle = BorderStyle.Thin;
                }
                #endregion

                #region SumColumn Title (右靠)儲存格格式
                ODSCellStyle sumColTitleCellStyle = wb.CreateCellStyle();
                {
                    sumColTitleCellStyle.HAlignment = HorizontalAlignment.Right;
                }
                #endregion

                #region 金額(千分位逗號、右靠)儲存格格式
                ODSCellStyle moneyCellStyle = wb.CreateCellStyle();
                {
                    moneyCellStyle.SetDataFormat(ODSNumberFormat.IntegerComma);
                    moneyCellStyle.HAlignment = HorizontalAlignment.Right;
                }
                #endregion

                int rowNo = 0;
                int pageNo = 0;
                int totalPage = dtPage.Rows.Count;

                if (totalPage == 0)
                {
                    #region Gen 表頭
                    {
                        rowNo = this.GenReportCHead(sheet, rowNo, dtHead, pageNo, totalPage, pageHeadCellStyle);
                    }
                    #endregion

                    #region 查無資料
                    rowNo++;
                    row = sheet.CreateRow(rowNo);
                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("查無資料");
                    #endregion
                }
                else
                {
                    int totalCount = 0;
                    double totalAmount = 0D;

                    foreach (DataRow pageRow in dtPage.Rows)
                    {
                        pageNo++;

                        string receiveType = pageRow["Receive_Type"].ToString().Replace("'", "");
                        string yearId = pageRow["Year_Id"].ToString().Replace("'", "");
                        string termId = pageRow["Term_Id"].ToString().Replace("'", "");
                        string depId = pageRow["Dep_Id"].ToString().Replace("'", "");
                        string receiveId = pageRow["Receive_Id"].ToString().Replace("'", "");
                        string deptId = pageRow["Dept_Id"].ToString().Replace("'", "");

                        #region Gen 表頭
                        {
                            rowNo = this.GenReportCHead(sheet, rowNo, dtHead, pageNo, totalPage, pageHeadCellStyle);
                        }
                        #endregion

                        #region Gen 表身 (含小計)
                        {
                            int deptCount = 0;
                            double deptAmount = 0D;
                            rowNo = this.GenReportCData(sheet, rowNo, dtDeptSum, dtMajorSum, dtGradeSum, dtClassSum, dtData, otherFields, receiveType, yearId, termId, depId, receiveId, deptId, colTitleCellStyle, moneyCellStyle, sumColTitleCellStyle, out deptCount, out deptAmount);
                            totalCount += deptCount;
                            totalAmount += deptAmount;
                        }
                        #endregion

                        #region 空白行+插入分頁
                        if (pageNo < totalPage)
                        {
                            rowNo++;
                            sheet.CreateRow(rowNo);
                            sheet.SetRowBreak(rowNo);
                        }
                        #endregion
                    }

                    #region 總合計
                    {
                        rowNo++;
                        row = sheet.CreateRow(rowNo);

                        #region 總合計人數
                        int colNo = 3;
                        cell = row.CreateCell(colNo, CellType.String);
                        cell.SetCellValue("總合計人數：");
                        cell.CellStyle = sumColTitleCellStyle;
                        cell.SetMergedCount(0, 1);

                        colNo += 2;
                        cell = row.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(totalCount);
                        #endregion

                        #region 總合計金額
                        colNo = 6;
                        cell = row.CreateCell(colNo, CellType.String);
                        cell.SetCellValue("總合計金額：");
                        cell.CellStyle = sumColTitleCellStyle;
                        cell.SetMergedCount(0, 1);

                        colNo += 2;
                        cell = row.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(totalAmount);
                        cell.CellStyle = moneyCellStyle;
                        cell.SetMergedCount(0, 1);
                        #endregion
                    }
                    #endregion
                }
                #endregion

                #region 將 ODSWorkbook 轉成 byte[]
                wb.WriteToBytes(out content);
                #endregion

                sheet = null;
                wb = null;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg;
        }

        /// <summary>
        /// 產生 學生繳費名冊 表頭
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
        /// <param name="dtHead"></param>
        /// <param name="pageNo"></param>
        /// <param name="totalPage"></param>
        /// <returns>傳回最後的 Row Index</returns>
        private int GenReportCHead(ODSSheet sheet, int rowIndex, DataTable dtHead, int pageNo, int totalPage, ODSCellStyle pageHeadCellStyle)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int rowIdx = rowIndex;

            DataRow dRow = dtHead.Rows[0];
            int columnCount = 15;

            #region Head Row 0
            {
                rowIdx++;
                row = sheet.CreateRow(rowIdx);

                int colNo = 1;
                string schName = dRow["Sch_Name"].ToString();
                string reportName = dRow["ReportName"].ToString();
                string value = String.Format("{0}          {1}", schName, reportName);
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
                cell.CellStyle = pageHeadCellStyle;
                cell.SetMergedCount(0, columnCount - 1);
            }
            #endregion

            #region Head Row 1
            {
                rowIdx++;
                row = sheet.CreateRow(rowIdx);

                #region 學年
                {
                    int colNo = 1;
                    string value = String.Format("學年：{0}", dRow["Year_Name"].ToString());
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);
                    cell.SetMergedCount(0, 2);
                }
                #endregion

                #region 學期
                {
                    int colNo = 5;
                    string value = String.Format("學期：{0}", dRow["Term_Name"].ToString());
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);
                    cell.SetMergedCount(0, 2);
                }
                #endregion

                #region 繳費狀態
                {
                    int colNo = 9;
                    string value = String.Format("繳費狀態：{0}", dRow["ReceiveStatusName"].ToString());
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);
                    cell.SetMergedCount(0, 2);
                }
                #endregion
            }
            #endregion

            #region Head Row 2
            {
                rowIdx++;
                row = sheet.CreateRow(rowIdx);

                #region 商家代號
                {
                    int colNo = 1;
                    string value = String.Format("商家代號：{0}", dRow["Receive_Type"].ToString());
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);
                    cell.SetMergedCount(0, 2);
                }
                #endregion

                #region 代收費用
                {
                    int colNo = 5;
                    string value = String.Format("代收費用：{0}", dRow["Receive_Name"].ToString());
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);
                    cell.SetMergedCount(0, 2);
                }
                #endregion

                #region 批號
                {
                    int colNo = 9;
                    string value = String.Format("批號：{0}", dRow["UpNo"].ToString());
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);
                    cell.SetMergedCount(0, 1);
                }
                #endregion

                #region 日期
                {
                    int colNo = columnCount - 2;
                    string value = dRow["ReportDate"].ToString();
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);
                }
                #endregion

                #region 頁數
                {
                    int colNo = columnCount - 1;
                    string value = String.Format("第{0}頁/共{1}頁", pageNo, totalPage);
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);
                }
                #endregion
            }
            #endregion

            return rowIdx;
        }

        /// <summary>
        /// 產生 學生繳費名冊 表身 (含小計)
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo"></param>
        /// <param name="dtDeptSum"></param>
        /// <param name="dtMajorSum"></param>
        /// <param name="dtGradeSum"></param>
        /// <param name="dtClassSum"></param>
        /// <param name="dtData"></param>
        /// <param name="otherFields"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        private int GenReportCData(ODSSheet sheet, int rowNo
            , DataTable dtDeptSum, DataTable dtMajorSum, DataTable dtGradeSum, DataTable dtClassSum, DataTable dtData, KeyValueList<string> otherFields
            , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId
            , ODSCellStyle colTitleCellStyle, ODSCellStyle moneyCellStyle, ODSCellStyle sumColTitleCellStyle
            , out int deptCount, out double deptAmount)
        {
            deptCount = 0;
            deptAmount = 0D;

            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;
            bool isHasGrade = (dtGradeSum != null && dtGradeSum.Rows.Count > 0);

            #region [MDY:20220530] Checkmarx 調整
            #region [OLD]
            //DataRow[] deptRows = dtDeptSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType, yearId, termId, depId, receiveId, deptId));
            #endregion

            DataRow[] deptRows = dtDeptSum.Rows.Cast<DataRow>().Where(
                    drow => drow["Receive_Type"].Equals(receiveType)
                        && drow["Year_Id"].Equals(yearId)
                        && drow["Term_Id"].Equals(termId)
                        && drow["Dep_Id"].Equals(depId)
                        && drow["Receive_Id"].Equals(receiveId)
                        && drow["Dept_Id"].Equals(deptId)).ToArray();
            #endregion

            if (deptRows != null && deptRows.Length > 0)
            {
                DataRow deptRow = deptRows[0];  //只會有一筆
                string deptName = deptRow["Dept_Name"].ToString();

                #region [MDY:20220530] Checkmarx 調整
                #region [OLD]
                //DataRow[] majorRows = dtMajorSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType, yearId, termId, depId, receiveId, deptId), "Major_Id");
                #endregion

                DataRow[] majorRows = dtMajorSum.Rows.Cast<DataRow>().Where(
                        drow => drow["Receive_Type"].Equals(receiveType)
                            && drow["Year_Id"].Equals(yearId)
                            && drow["Term_Id"].Equals(termId)
                            && drow["Dep_Id"].Equals(depId)
                            && drow["Receive_Id"].Equals(receiveId)
                            && drow["Dept_Id"].Equals(deptId)
                        ).OrderBy(drow => drow.Field<string>("Major_Id")).ToArray();
                #endregion

                if (majorRows != null && majorRows.Length > 0)
                {
                    foreach (DataRow majorRow in majorRows)
                    {
                        string majorId = majorRow["Major_Id"].ToString();
                        string majorName = majorRow["Major_Name"].ToString();

                        if (isHasGrade)
                        {
                            #region 群組明細程度：部別、系所、年級、班別
                            #region [MDY:20220530] Checkmarx 調整
                            #region [OLD]
                            //DataRow[] gradeRows = dtGradeSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' ", receiveType, yearId, termId, depId, receiveId, deptId, majorId), "Stu_Grade");
                            #endregion

                            DataRow[] gradeRows = dtGradeSum.Rows.Cast<DataRow>().Where(
                                    drow => drow["Receive_Type"].Equals(receiveType)
                                        && drow["Year_Id"].Equals(yearId)
                                        && drow["Term_Id"].Equals(termId)
                                        && drow["Dep_Id"].Equals(depId)
                                        && drow["Receive_Id"].Equals(receiveId)
                                        && drow["Dept_Id"].Equals(deptId)
                                        && drow["Major_Id"].Equals(majorId)
                                    ).OrderBy(drow => drow.Field<string>("Stu_Grade")).ToArray();
                            #endregion

                            if (gradeRows != null && gradeRows.Length > 0)
                            {
                                foreach (DataRow gradeRow in gradeRows)
                                {
                                    string stuGrade = gradeRow["Stu_Grade"].ToString();
                                    string gradeName = GradeCodeTexts.GetText(stuGrade);
                                    if (String.IsNullOrEmpty(gradeName))
                                    {
                                        gradeName = stuGrade;
                                    }

                                    #region 班別資料 + 班別合計
                                    {
                                        #region [MDY:20220530] Checkmarx 調整
                                        #region [OLD]
                                        //DataRow[] classRows = dtClassSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Stu_Grade='{7}'", receiveType, yearId, termId, depId, receiveId, deptId, majorId, stuGrade), "Class_Id");
                                        #endregion

                                        DataRow[] classRows = dtClassSum.Rows.Cast<DataRow>().Where(
                                                drow => drow["Receive_Type"].Equals(receiveType)
                                                    && drow["Year_Id"].Equals(yearId)
                                                    && drow["Term_Id"].Equals(termId)
                                                    && drow["Dep_Id"].Equals(depId)
                                                    && drow["Receive_Id"].Equals(receiveId)
                                                    && drow["Dept_Id"].Equals(deptId)
                                                    && drow["Major_Id"].Equals(majorId)
                                                    && drow["Stu_Grade"].Equals(stuGrade)
                                                ).OrderBy(drow => drow.Field<string>("Class_Id")).ToArray();
                                        #endregion

                                        if (classRows != null && classRows.Length > 0)
                                        {
                                            foreach (DataRow classRow in classRows)
                                            {
                                                string classId = classRow["Class_Id"].ToString();
                                                string className = classRow["Class_Name"].ToString();

                                                genRowNo++;
                                                genRowNo = this.GenReportCForClassData(sheet, genRowNo, deptName, majorName, gradeName, className, dtData, otherFields, receiveType, yearId, termId, depId, receiveId, deptId, majorId, stuGrade, classId, colTitleCellStyle, moneyCellStyle);

                                                #region 班別合計
                                                {
                                                    genRowNo++;
                                                    row = sheet.CreateRow(genRowNo);

                                                    #region 班級人數
                                                    int colNo = 3;
                                                    cell = row.CreateCell(colNo, CellType.String);
                                                    cell.SetCellValue("班級人數：");
                                                    cell.CellStyle = sumColTitleCellStyle;
                                                    cell.SetMergedCount(0, 1);

                                                    int sumCount = Int32.Parse(classRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                                                    colNo += 2;
                                                    cell = row.CreateCell(colNo, CellType.Numeric);
                                                    cell.SetCellValue(sumCount);
                                                    #endregion

                                                    #region 班級金額
                                                    colNo = 6;
                                                    cell = row.CreateCell(colNo, CellType.String);
                                                    cell.SetCellValue("班級金額：");
                                                    cell.CellStyle = sumColTitleCellStyle;
                                                    cell.SetMergedCount(0, 1);

                                                    double sumAmount = Double.Parse(classRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                                                    colNo += 2;
                                                    cell = row.CreateCell(colNo, CellType.Numeric);
                                                    cell.SetCellValue(sumAmount);
                                                    cell.CellStyle = moneyCellStyle;
                                                    cell.SetMergedCount(0, 1);
                                                    #endregion
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                    #endregion

                                    #region 年級合計
                                    {
                                        genRowNo++;
                                        row = sheet.CreateRow(genRowNo);

                                        #region 年級人數
                                        int colNo = 3;
                                        cell = row.CreateCell(colNo, CellType.String);
                                        cell.SetCellValue("年級人數：");
                                        cell.CellStyle = sumColTitleCellStyle;
                                        cell.SetMergedCount(0, 1);

                                        int sumCount = Int32.Parse(gradeRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                                        colNo += 2;
                                        cell = row.CreateCell(colNo, CellType.Numeric);
                                        cell.SetCellValue(sumCount);
                                        #endregion

                                        #region 年級金額
                                        colNo = 6;
                                        cell = row.CreateCell(colNo, CellType.String);
                                        cell.SetCellValue("年級金額：");
                                        cell.CellStyle = sumColTitleCellStyle;
                                        cell.SetMergedCount(0, 1);

                                        double sumAmount = Double.Parse(gradeRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                                        colNo += 2;
                                        cell = row.CreateCell(colNo, CellType.Numeric);
                                        cell.SetCellValue(sumAmount);
                                        cell.CellStyle = moneyCellStyle;
                                        cell.SetMergedCount(0, 1);
                                        #endregion
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region 群組明細程度：部別、系所、班別
                            string stuGrade = null;
                            string gradeName = null;

                            #region 班別資料 + 班別合計
                            {
                                #region [MDY:20220530] Checkmarx 調整
                                #region [OLD]
                                //DataRow[] classRows = dtClassSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType, yearId, termId, depId, receiveId, deptId, majorId), "Class_Id");
                                #endregion

                                DataRow[] classRows = dtClassSum.Rows.Cast<DataRow>().Where(
                                        drow => drow["Receive_Type"].Equals(receiveType)
                                            && drow["Year_Id"].Equals(yearId)
                                            && drow["Term_Id"].Equals(termId)
                                            && drow["Dep_Id"].Equals(depId)
                                            && drow["Receive_Id"].Equals(receiveId)
                                            && drow["Dept_Id"].Equals(deptId)
                                            && drow["Major_Id"].Equals(majorId)
                                        ).OrderBy(drow => drow.Field<string>("Class_Id")).ToArray();
                                #endregion

                                if (classRows != null && classRows.Length > 0)
                                {
                                    foreach (DataRow classRow in classRows)
                                    {
                                        string classId = classRow["Class_Id"].ToString();
                                        string className = classRow["Class_Name"].ToString();

                                        genRowNo++;
                                        genRowNo = this.GenReportCForClassData(sheet, genRowNo, deptName, majorName, gradeName, className, dtData, otherFields, receiveType, yearId, termId, depId, receiveId, deptId, majorId, stuGrade, classId, colTitleCellStyle, moneyCellStyle);

                                        #region 班別合計
                                        {
                                            genRowNo++;
                                            row = sheet.CreateRow(genRowNo);

                                            #region 班級人數
                                            int colNo = 3;
                                            cell = row.CreateCell(colNo, CellType.String);
                                            cell.SetCellValue("班級人數：");
                                            cell.CellStyle = sumColTitleCellStyle;
                                            cell.SetMergedCount(0, 1);

                                            int sumCount = Int32.Parse(classRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                                            colNo += 2;
                                            cell = row.CreateCell(colNo, CellType.Numeric);
                                            cell.SetCellValue(sumCount);
                                            #endregion

                                            #region 班級金額
                                            colNo = 6;
                                            cell = row.CreateCell(colNo, CellType.String);
                                            cell.SetCellValue("班級金額：");
                                            cell.CellStyle = sumColTitleCellStyle;
                                            cell.SetMergedCount(0, 1);

                                            double sumAmount = Double.Parse(classRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                                            colNo += 2;
                                            cell = row.CreateCell(colNo, CellType.Numeric);
                                            cell.SetCellValue(sumAmount);
                                            cell.CellStyle = moneyCellStyle;
                                            cell.SetMergedCount(0, 1);
                                            #endregion
                                        }
                                        #endregion
                                    }
                                }
                            }
                            #endregion
                            #endregion
                        }

                        #region 系所合計
                        {
                            genRowNo++;
                            row = sheet.CreateRow(genRowNo);

                            #region 系所人數
                            int colNo = 3;
                            cell = row.CreateCell(colNo, CellType.String);
                            cell.SetCellValue("系所人數：");
                            cell.CellStyle = sumColTitleCellStyle;
                            cell.SetMergedCount(0, 1);

                            int sumCount = Int32.Parse(majorRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                            colNo += 2;
                            cell = row.CreateCell(colNo, CellType.Numeric);
                            cell.SetCellValue(sumCount);
                            #endregion

                            #region 系所金額
                            colNo = 6;
                            cell = row.CreateCell(colNo, CellType.String);
                            cell.SetCellValue("系所金額：");
                            cell.CellStyle = sumColTitleCellStyle;
                            cell.SetMergedCount(0, 1);

                            double sumAmount = Double.Parse(majorRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                            colNo += 2;
                            cell = row.CreateCell(colNo, CellType.Numeric);
                            cell.SetCellValue(sumAmount);
                            cell.CellStyle = moneyCellStyle;
                            cell.SetMergedCount(0, 1);
                            #endregion
                        }
                        #endregion
                    }
                }

                #region 部別合計
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);

                    #region 部別人數
                    int colNo = 3;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("部別人數：");
                    cell.CellStyle = sumColTitleCellStyle;
                    cell.SetMergedCount(0, 1);

                    int sumCount = Int32.Parse(deptRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                    colNo += 2;
                    cell = row.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(sumCount);

                    deptCount = sumCount;
                    #endregion

                    #region 部別金額
                    colNo = 6;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("部別金額：");
                    cell.CellStyle = sumColTitleCellStyle;
                    cell.SetMergedCount(0, 1);

                    double sumAmount = Double.Parse(deptRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                    colNo += 2;
                    cell = row.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(sumAmount);
                    cell.CellStyle = moneyCellStyle;
                    cell.SetMergedCount(0, 1);

                    deptAmount = sumAmount;
                    #endregion
                }
                #endregion
            }
            else
            {
                #region 部別合計
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);

                    #region 部別人數
                    int colNo = 3;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("部別人數：");
                    cell.CellStyle = sumColTitleCellStyle;
                    cell.SetMergedCount(0, 1);

                    int sumCount = 0;
                    colNo += 2;
                    cell = row.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(sumCount);
                    #endregion

                    #region 部別金額
                    colNo = 6;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("部別金額：");
                    cell.CellStyle = sumColTitleCellStyle;
                    cell.SetMergedCount(0, 1);

                    double sumAmount = 0;
                    colNo += 2;
                    cell = row.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(sumAmount);
                    cell.CellStyle = moneyCellStyle;
                    cell.SetMergedCount(0, 1);
                    #endregion
                }
                #endregion
            }

            return genRowNo;
        }

        /// <summary>
        /// 產生 學生繳費名冊 表身 的班別資料
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo"></param>
        /// <param name="deptName"></param>
        /// <param name="majorName"></param>
        /// <param name="gradeName"></param>
        /// <param name="className"></param>
        /// <param name="dtData"></param>
        /// <param name="otherFields"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="deptId"></param>
        /// <param name="majorId"></param>
        /// <param name="stuGrade"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        private int GenReportCForClassData(ODSSheet sheet, int rowNo
            , string deptName, string majorName, string gradeName, string className
            , DataTable dtData, KeyValueList<string> otherFields
            , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId, string majorId, string stuGrade, string classId
            , ODSCellStyle colTitleCellStyle, ODSCellStyle moneyCellStyle)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            #region Data Row 部別、系所、年級、班別 名稱 (0 ~ 2/3)
            {
                #region 部別名稱
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);

                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(deptName);
                    cell.CellStyle = colTitleCellStyle;
                    cell.SetMergedCount(0, 3);
                }
                #endregion

                #region 系所名稱
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);

                    int colNo = 2;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(majorName);
                    cell.CellStyle = colTitleCellStyle;
                    cell.SetMergedCount(0, 3);
                }
                #endregion

                #region 年級名稱
                //有年級參數表示群組明細程度為部別、系所、年級、班別，才要顯示年級名稱
                if (stuGrade != null)
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);

                    int colNo = 3;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("年級：" + gradeName);
                    cell.CellStyle = colTitleCellStyle;
                    cell.SetMergedCount(0, 3);
                }
                #endregion

                #region 班別名稱
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);

                    int colNo = 4;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("班級：" + className);
                    cell.CellStyle = colTitleCellStyle;
                    cell.SetMergedCount(0, 11);
                }
                #endregion
            }
            #endregion

            #region Data Row 欄位名稱 (3/4)
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);

                #region 學號
                {
                    int colNo = 4;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("學號");
                    cell.CellStyle = colTitleCellStyle;
                    cell.SetMergedCount(0, 1);
                }
                #endregion

                #region 姓名
                {
                    int colNo = 6;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("姓名");
                    cell.CellStyle = colTitleCellStyle;
                    cell.SetMergedCount(0, 1);
                }
                #endregion

                #region 金額
                {
                    int colNo = 8;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("金額");
                    cell.CellStyle = colTitleCellStyle;
                    cell.SetMergedCount(0, 1);
                }
                #endregion

                #region 說明
                {
                    int colNo = 10;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("說明");
                    cell.CellStyle = colTitleCellStyle;
                    cell.SetMergedCount(0, 5);
                }
                #endregion
            }
            #endregion

            #region Data Row 資料 (4/5 ~ 4/5 + data Count)
            {
                DataRow[] dRows = null;
                if (stuGrade != null)
                {
                    //有年級參數表示群組明細程度為部別、系所、年級、班別
                    #region [MDY:20220530] Checkmarx 調整
                    #region [OLD]
                    //dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Stu_Grade='{7}' AND Class_Id='{8}'", receiveType, yearId, termId, depId, receiveId, deptId, majorId, stuGrade, classId), "Stu_Id");
                    #endregion

                    dRows = dtData.Rows.Cast<DataRow>().Where(
                            drow => drow["Receive_Type"].Equals(receiveType)
                                && drow["Year_Id"].Equals(yearId)
                                && drow["Term_Id"].Equals(termId)
                                && drow["Dep_Id"].Equals(depId)
                                && drow["Receive_Id"].Equals(receiveId)
                                && drow["Dept_Id"].Equals(deptId)
                                && drow["Major_Id"].Equals(majorId)
                                && drow["Stu_Grade"].Equals(stuGrade)
                                && drow["Class_Id"].Equals(classId)
                            ).OrderBy(drow => drow.Field<string>("Stu_Id")).ToArray();
                    #endregion
                }
                else
                {
                    #region [MDY:20220530] Checkmarx 調整
                    #region [OLD]
                    //dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Class_Id='{7}'", receiveType, yearId, termId, depId, receiveId, deptId, majorId, classId), "Stu_Id");
                    #endregion

                    dRows = dtData.Rows.Cast<DataRow>().Where(
                            drow => drow["Receive_Type"].Equals(receiveType)
                                && drow["Year_Id"].Equals(yearId)
                                && drow["Term_Id"].Equals(termId)
                                && drow["Dep_Id"].Equals(depId)
                                && drow["Receive_Id"].Equals(receiveId)
                                && drow["Dept_Id"].Equals(deptId)
                                && drow["Major_Id"].Equals(majorId)
                                && drow["Class_Id"].Equals(classId)
                            ).OrderBy(drow => drow.Field<string>("Stu_Id")).ToArray();
                    #endregion
                }
                if (dRows != null && dRows.Length > 0)
                {
                    foreach (DataRow dRow in dRows)
                    {
                        genRowNo++;
                        row = sheet.CreateRow(genRowNo);

                        #region 學號
                        {
                            int colNo = 4;
                            cell = row.CreateCell(colNo, CellType.String);
                            string stuId = dRow["Stu_Id"].ToString();
                            cell.SetCellValue(stuId);
                            cell.SetMergedCount(0, 1);
                        }
                        #endregion

                        #region 姓名
                        {
                            int colNo = 6;
                            cell = row.CreateCell(colNo, CellType.String);
                            string stuName = dRow["Stu_Name"].ToString();
                            cell.SetCellValue(stuName);
                            cell.SetMergedCount(0, 1);
                        }
                        #endregion

                        #region 金額
                        {
                            int colNo = 8;
                            cell = row.CreateCell(colNo, CellType.Numeric);
                            if (dRow.IsNull("Receive_Item_Amount"))
                            {
                                cell.SetCellValue("");
                            }
                            else
                            {
                                double receiveItemAmount = Convert.ToDouble(dRow["Receive_Item_Amount"]);
                                cell.SetCellValue(receiveItemAmount);
                            }
                            cell.CellStyle = moneyCellStyle;
                            cell.SetMergedCount(0, 1);
                        }
                        #endregion

                        #region 說明
                        {
                            int colNo = 10;
                            cell = row.CreateCell(colNo, CellType.String);
                            if (otherFields != null && otherFields.Count > 0)
                            {
                                StringBuilder sb = new StringBuilder();
                                foreach (KeyValue<string> otherField in otherFields)
                                {
                                    sb.AppendFormat("{0}={1}；", otherField.Key, dRow[otherField.Value]);
                                }
                                cell.SetCellValue(sb.ToString());
                            }
                            else
                            {
                                cell.SetCellValue("");
                            }
                            cell.SetMergedCount(0, 5);
                        }
                        #endregion
                    }
                }
            }
            #endregion

            return genRowNo;
        }

        /// <summary>
        /// 在指定的 Cell 後面，建立指定數量的空白格 並 Merge 到指定的 Cell
        /// </summary>
        /// <param name="cell">指定的 Cell</param>
        /// <param name="columnCount">指定空白格數量</param>
        //private void CreatEmptyColumnCellAndMerge(ODSCell cell, int columnCount)
        //{
        //    if (cell != null && columnCount > 0)
        //    {
        //        IRow row = cell.Row;
        //        for (int no = 1; no <= columnCount; no++)
        //        {
        //            int colIndex = cell.ColumnIndex + no;
        //            ODSCell cell2 = row.CreateCell(colIndex, cell.CellType);
        //            cell2.SetCellValue("");
        //            cell2.CellStyle = cell.CellStyle;
        //        }
        //        cell.Sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + columnCount));
        //    }
        //}
        #endregion

        #region 手續費統計報表
        /// <summary>
        /// 手續費統計報表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string GenReportD(DataTable data, out byte[] content)
        {
            content = null;
            string errmsg = null;

            try
            {
                #region 使用 ODSWorkbook 產生 ODS
                string sheetName = "Sheet1";

                ODSWorkbook wb = new ODSWorkbook();
                ODSSheet sheet = wb.CreateSheet(sheetName);
                ODSRow row = null;
                ODSCell cell = null;

                #region 指定紙張大小
                wb.PageStyle.PaperSize = PapeSizeKind.A4;
                #endregion

                #region 指定直式或橫式 Lendscape=橫式 Portrait=直式
                wb.PageStyle.PaperOrientation = PageOrientationKind.Lendscape;
                #endregion

                #region 顯示格線
                wb.IsShowGrid = true;
                #endregion

                #region Page Head (粗體、14字、置中)儲存格格式
                ODSCellStyle pageHeadCellStyle = wb.CreateCellStyle();
                {
                    ODSFont pageHeadFont = wb.CreateFont();
                    pageHeadFont.IsBold = true;
                    pageHeadFont.FontSize = 14D;
                    pageHeadCellStyle.SetFont(pageHeadFont);
                    pageHeadCellStyle.HAlignment = HorizontalAlignment.Center;
                }
                #endregion

                #region Column Title (粗體、底線、置中)儲存格格式
                ODSCellStyle colTitleCellStyle = wb.CreateCellStyle();
                {
                    ODSFont colTitleFont = wb.CreateFont();
                    colTitleFont.IsBold = true;
                    colTitleCellStyle.SetFont(colTitleFont);
                    colTitleCellStyle.HAlignment = HorizontalAlignment.Center;
                    colTitleCellStyle.BottomBorderStyle = BorderStyle.Thin;
                }
                #endregion

                #region 金額(千分位逗號、底線、右靠)儲存格格式
                ODSCellStyle moneyCellStyle = wb.CreateCellStyle();
                {
                    moneyCellStyle.SetDataFormat(ODSNumberFormat.IntegerComma);
                    moneyCellStyle.HAlignment = HorizontalAlignment.Right;
                    moneyCellStyle.BottomBorderStyle = BorderStyle.Thin;
                }
                #endregion

                int rowNo = 0;

                rowNo = GenReportDHeader(sheet, rowNo, data.Columns, pageHeadCellStyle, colTitleCellStyle);
                if (data == null || data.Rows.Count <= 0)
                {
                    #region 查無資料
                    rowNo++;
                    row = sheet.CreateRow(rowNo);
                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("查無資料");
                    #endregion
                }
                else
                {
                    rowNo = GenReportDData(sheet, rowNo, data, moneyCellStyle);
                }
                #endregion

                #region 將 ODSWorkbook 轉成 byte[]
                wb.WriteToBytes(out content);
                #endregion

                sheet = null;
                wb = null;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg;
        }

        /// <summary>
        /// 產生手續費統計報表表頭
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo"></param>
        /// <param name="columns"></param>
        /// <param name="pageHeadCellStyle"></param>
        /// <param name="colTitleCellStyle"></param>
        /// <returns></returns>
        private int GenReportDHeader(ODSSheet sheet, int rowNo, DataColumnCollection columns, ODSCellStyle pageHeadCellStyle, ODSCellStyle colTitleCellStyle)
        {
            if (columns == null)
            {
                return rowNo;
            }

            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            #region Head Row 0
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);
                int colNo = 0;

                colNo++;
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue("手續費統計報表");
                cell.CellStyle = pageHeadCellStyle;

                cell.SetMergedCount(0, columns.Count - 1);
            }
            #endregion

            #region Head Row 1
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);
                int colNo = 0;

                foreach (DataColumn column in columns)
                {
                    colNo++;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(column.Caption);
                    cell.CellStyle = colTitleCellStyle;
                }
            }
            #endregion

            return genRowNo;
        }

        /// <summary>
        /// 產生手續費統計報表表身
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo"></param>
        /// <param name="data"></param>
        /// <param name="moneyCellStyle"></param>
        /// <returns></returns>
        private int GenReportDData(ODSSheet sheet, int rowNo, DataTable data, ODSCellStyle moneyCellStyle)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            foreach (DataRow dataRow in data.Rows)
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);
                int colNo = 0;

                foreach (DataColumn column in data.Columns)
                {
                    colNo++;

                    if (column.DataType == _Int32Type || column.DataType == _Int64Type)
                    {
                        #region 數值
                        Int64 value = dataRow.IsNull(column) ? 0 : Convert.ToInt64(dataRow[column]);
                        cell = row.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(value);
                        cell.CellStyle = moneyCellStyle;
                        #endregion
                    }
                    else
                    {
                        #region 字串
                        string value = dataRow.IsNull(column) ? String.Empty : Convert.ToString(dataRow[column]);
                        cell = row.CreateCell(colNo, CellType.String);
                        cell.SetCellValue(value);
                        #endregion
                    }
                }
            }

            return genRowNo;
        }
        #endregion

        #region 代收單位交易資訊統計表
        /// <summary>
        /// 代收單位交易資訊統計表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string GenReportD2(DataTable data, out byte[] content)
        {
            content = null;
            string errmsg = null;

            try
            {
                #region 使用 ODSWorkbook 產生 ODS
                string sheetName = "Sheet1";

                ODSWorkbook wb = new ODSWorkbook();
                ODSSheet sheet = wb.CreateSheet(sheetName);
                ODSRow row = null;
                ODSCell cell = null;

                #region 指定紙張大小
                wb.PageStyle.PaperSize = PapeSizeKind.A4;
                #endregion

                #region 指定直式或橫式 Lendscape=橫式 Portrait=直式
                wb.PageStyle.PaperOrientation = PageOrientationKind.Lendscape;
                #endregion

                #region 顯示格線
                wb.IsShowGrid = true;
                #endregion

                #region Page Head (粗體、14字、置中)儲存格格式
                ODSCellStyle pageHeadCellStyle = wb.CreateCellStyle();
                {
                    ODSFont pageHeadFont = wb.CreateFont();
                    pageHeadFont.IsBold = true;
                    pageHeadFont.FontSize = 14D;
                    pageHeadCellStyle.SetFont(pageHeadFont);
                    pageHeadCellStyle.HAlignment = HorizontalAlignment.Center;
                }
                #endregion

                #region Column Title (粗體、底線、置中)儲存格格式
                ODSCellStyle colTitleCellStyle = wb.CreateCellStyle();
                {
                    ODSFont colTitleFont = wb.CreateFont();
                    colTitleFont.IsBold = true;
                    colTitleCellStyle.SetFont(colTitleFont);
                    colTitleCellStyle.HAlignment = HorizontalAlignment.Center;
                    colTitleCellStyle.BottomBorderStyle = BorderStyle.Thin;
                }
                #endregion

                #region 金額(千分位逗號、底線、右靠)儲存格格式
                ODSCellStyle moneyCellStyle = wb.CreateCellStyle();
                {
                    moneyCellStyle.SetDataFormat(ODSNumberFormat.IntegerComma);
                    moneyCellStyle.HAlignment = HorizontalAlignment.Right;
                    moneyCellStyle.BottomBorderStyle = BorderStyle.Thin;
                }
                #endregion

                int rowNo = 0;

                rowNo = GenReportD2Header(sheet, rowNo, data.Columns, pageHeadCellStyle, colTitleCellStyle);
                if (data == null || data.Rows.Count <= 0)
                {
                    #region 查無資料
                    rowNo++;
                    row = sheet.CreateRow(rowNo);
                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("查無資料");
                    #endregion
                }
                else
                {
                    rowNo = GenReportD2Data(sheet, rowNo, data, moneyCellStyle);
                }
                #endregion

                #region 將 ODSWorkbook 轉成 byte[]
                wb.WriteToBytes(out content);
                #endregion

                sheet = null;
                wb = null;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg;
        }

        /// <summary>
        /// 產生代收單位交易資訊統計表表頭
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo"></param>
        /// <param name="columns"></param>
        /// <param name="pageHeadCellStyle"></param>
        /// <param name="colTitleCellStyle"></param>
        /// <returns></returns>
        private int GenReportD2Header(ODSSheet sheet, int rowNo, DataColumnCollection columns, ODSCellStyle pageHeadCellStyle, ODSCellStyle colTitleCellStyle)
        {
            if (columns == null)
            {
                return rowNo;
            }

            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            #region Head Row 0
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);
                int colNo = 0;

                colNo++;
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue("代收單位交易資訊統計表");
                cell.CellStyle = pageHeadCellStyle;

                cell.SetMergedCount(0, columns.Count - 1);
            }
            #endregion

            #region Head Row 1
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);
                int colNo = 0;

                foreach (DataColumn column in columns)
                {
                    colNo++;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(column.Caption);
                    cell.CellStyle = colTitleCellStyle;
                }
            }
            #endregion

            return genRowNo;
        }

        /// <summary>
        /// 產生代收單位交易資訊統計表表身
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo"></param>
        /// <param name="data"></param>
        /// <param name="moneyCellStyle"></param>
        /// <returns></returns>
        private int GenReportD2Data(ODSSheet sheet, int rowNo, DataTable data, ODSCellStyle moneyCellStyle)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            foreach (DataRow dataRow in data.Rows)
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);
                int colNo = 0;

                foreach (DataColumn column in data.Columns)
                {
                    colNo++;

                    if (column.DataType == _Int32Type || column.DataType == _Int64Type)
                    {
                        #region 數值
                        Int64 value = dataRow.IsNull(column) ? 0 : Convert.ToInt64(dataRow[column]);
                        cell = row.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(value);
                        cell.CellStyle = moneyCellStyle;
                        #endregion
                    }
                    else
                    {
                        #region 字串
                        string value = dataRow.IsNull(column) ? String.Empty : Convert.ToString(dataRow[column]);
                        cell = row.CreateCell(colNo, CellType.String);
                        cell.SetCellValue(value);
                        #endregion
                    }
                }
            }

            return genRowNo;
        }
        #endregion

        #region 繳款通道交易資訊統計表
        /// <summary>
        /// 繳款通道交易資訊統計表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string GenReportD3(DataTable data, out byte[] content)
        {
            content = null;
            string errmsg = null;

            try
            {
                #region 使用 ODSWorkbook 產生 ODS
                string sheetName = "Sheet1";

                ODSWorkbook wb = new ODSWorkbook();
                ODSSheet sheet = (ODSSheet)wb.CreateSheet(sheetName);
                ODSRow row = null;
                ODSCell cell = null;

                #region 指定紙張大小
                wb.PageStyle.PaperSize = PapeSizeKind.A4;
                #endregion

                #region 指定直式或橫式 Lendscape=橫式 Portrait=直式
                wb.PageStyle.PaperOrientation = PageOrientationKind.Lendscape;
                #endregion

                #region 顯示格線
                wb.IsShowGrid = true;
                #endregion

                #region Page Head (粗體、14字、置中)儲存格格式
                ODSCellStyle pageHeadCellStyle = wb.CreateCellStyle();
                {
                    ODSFont pageHeadFont = wb.CreateFont();
                    pageHeadFont.IsBold = true;
                    pageHeadFont.FontSize = 14D;
                    pageHeadCellStyle.SetFont(pageHeadFont);
                    pageHeadCellStyle.HAlignment = HorizontalAlignment.Center;
                }
                #endregion

                #region Column Title (粗體、底線、置中)儲存格格式
                ODSCellStyle colTitleCellStyle = wb.CreateCellStyle();
                {
                    ODSFont colTitleFont = wb.CreateFont();
                    colTitleFont.IsBold = true;
                    colTitleCellStyle.SetFont(colTitleFont);
                    colTitleCellStyle.HAlignment = HorizontalAlignment.Center;
                    colTitleCellStyle.BottomBorderStyle = BorderStyle.Thin;
                }
                #endregion

                #region 金額(千分位逗號、底線、右靠)儲存格格式
                ODSCellStyle moneyCellStyle = wb.CreateCellStyle();
                {
                    moneyCellStyle.SetDataFormat(ODSNumberFormat.IntegerComma);
                    moneyCellStyle.HAlignment = HorizontalAlignment.Right;
                    moneyCellStyle.BottomBorderStyle = BorderStyle.Thin;
                }
                #endregion

                int rowNo = 0;

                rowNo = GenReportD3Header(sheet, rowNo, data.Columns, pageHeadCellStyle, colTitleCellStyle);
                if (data == null || data.Rows.Count <= 0)
                {
                    #region 查無資料
                    rowNo++;
                    row = sheet.CreateRow(rowNo);
                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("查無資料");
                    #endregion
                }
                else
                {
                    rowNo = GenReportD3Data(sheet, rowNo, data, moneyCellStyle);
                }
                #endregion

                #region 將 ODSWorkbook 轉成 byte[]
                wb.WriteToBytes(out content);
                #endregion

                sheet = null;
                wb = null;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg;
        }

        /// <summary>
        /// 產生繳款通道交易資訊統計表表頭
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo"></param>
        /// <param name="columns"></param>
        /// <param name="pageHeadCellStyle"></param>
        /// <param name="colTitleCellStyle"></param>
        /// <returns></returns>
        private int GenReportD3Header(ODSSheet sheet, int rowNo, DataColumnCollection columns, ODSCellStyle pageHeadCellStyle, ODSCellStyle colTitleCellStyle)
        {
            if (columns == null)
            {
                return rowNo;
            }

            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            #region Head Row 0
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);
                int colNo = 0;

                colNo++;
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue("繳款通道交易資訊統計表");
                cell.CellStyle = pageHeadCellStyle;

                cell.SetMergedCount(0, columns.Count - 1);
            }
            #endregion

            #region Head Row 1
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);
                int colNo = 0;

                foreach (DataColumn column in columns)
                {
                    colNo++;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(column.Caption);
                    cell.CellStyle = colTitleCellStyle;
                }
            }
            #endregion

            return genRowNo;
        }

        /// <summary>
        /// 產生繳款通道交易資訊統計表表身
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo"></param>
        /// <param name="data"></param>
        /// <param name="moneyCellStyle"></param>
        /// <returns></returns>
        private int GenReportD3Data(ODSSheet sheet, int rowNo, DataTable data, ODSCellStyle moneyCellStyle)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            foreach (DataRow dataRow in data.Rows)
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);
                int colNo = 0;

                foreach (DataColumn column in data.Columns)
                {
                    colNo++;

                    if (column.DataType == _Int32Type || column.DataType == _Int64Type)
                    {
                        #region 數值
                        Int64 value = dataRow.IsNull(column) ? 0 : Convert.ToInt64(dataRow[column]);
                        cell = row.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(value);
                        cell.CellStyle = moneyCellStyle;
                        #endregion
                    }
                    else
                    {
                        #region 字串
                        string value = dataRow.IsNull(column) ? String.Empty : Convert.ToString(dataRow[column]);
                        cell = row.CreateCell(colNo, CellType.String);
                        cell.SetCellValue(value);
                        #endregion
                    }
                }
            }

            return genRowNo;
        }
        #endregion

        #region 繳費收費項目 (明細分析表、分類統計表)
        /// <summary>
        /// 繳費收費項目明細分析表
        /// </summary>
        /// <param name="dtHead">表頭資料</param>
        /// <param name="dtPage">部別(分頁)資料</param>
        /// <param name="receiveItems">收入科目資料</param>
        /// <param name="dtDatas">表身資料集合</param>
        /// <param name="content">成功則傳回產生檔案的 byte 陣列，否則傳回 null</param>
        /// <returns>成功則傳回 null，否怎傳回錯誤訊息</returns>
        public string GenReportE1(DataTable dtHead, DataTable dtPage, KeyValueList<string> receiveItems, List<DataTable> dtDatas, out byte[] content)
        {
            content = null;
            string errmsg = null;
            try
            {
                #region 使用 ODSWorkbook 產生 ODS
                string sheetName = "Sheet1";

                ODSWorkbook wb = new ODSWorkbook();
                ODSSheet sheet = wb.CreateSheet(sheetName);
                ODSRow row = null;
                ODSCell cell = null;

                #region 指定紙張大小
                wb.PageStyle.PaperSize = PapeSizeKind.A4;
                #endregion

                #region 指定直式或橫式 Lendscape=橫式 Portrait=直式
                wb.PageStyle.PaperOrientation = PageOrientationKind.Lendscape;
                #endregion

                #region 指定縮放 (0 ~ 100)
                //不提供
                //sheet.PrintSetup.Scale = 100;
                //sheet.PrintSetup.FitWidth = 0;
                //sheet.PrintSetup.FitHeight = 0;
                #endregion

                #region 藏隱格線
                wb.IsShowGrid = false;
                #endregion

                #region 日期 (左靠、yyyy/mm/dd)儲存格格式
                ODSCellStyle dateCellStyle = wb.CreateCellStyle();
                dateCellStyle.SetDataFormat(ODSDateFormat.Date);
                dateCellStyle.HAlignment = HorizontalAlignment.Left;
                #endregion

                #region left (左靠、底線)儲存格格式
                ODSCellStyle leftCellStyle = wb.CreateCellStyle();
                leftCellStyle.HAlignment = HorizontalAlignment.Left;
                leftCellStyle.BottomBorderStyle = BorderStyle.Thin;
                #endregion

                #region rigth (右靠、底線)儲存格格式
                ODSCellStyle rightCellStyle = wb.CreateCellStyle();
                rightCellStyle.HAlignment = HorizontalAlignment.Right;
                rightCellStyle.BottomBorderStyle = BorderStyle.Thin;
                #endregion

                #region center (置中、底線)儲存格格式
                ODSCellStyle centerCellStyle = wb.CreateCellStyle();
                centerCellStyle.HAlignment = HorizontalAlignment.Center;
                centerCellStyle.BottomBorderStyle = BorderStyle.Thin;
                #endregion

                #region 金額(千分位逗號、右靠、底線)儲存格格式
                ODSCellStyle moneyCellStyle = wb.CreateCellStyle();
                moneyCellStyle.SetDataFormat(ODSNumberFormat.IntegerComma);
                moneyCellStyle.HAlignment = HorizontalAlignment.Right;
                moneyCellStyle.BottomBorderStyle = BorderStyle.Thin;
                #endregion

                int rowNo = 0;
                int pageNo = 0;
                int totalPage = dtPage.Rows.Count * receiveItems.Count;

                if (totalPage == 0)
                {
                    #region Gen 表頭
                    {
                        rowNo = this.GenReportE1Head(sheet, dateCellStyle, rowNo, dtHead, pageNo, totalPage);
                    }
                    #endregion

                    #region 查無資料
                    rowNo++;
                    row = sheet.CreateRow(rowNo);
                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("查無資料");
                    #endregion
                }
                else
                {
                    foreach (KeyValue<string> receiveItem in receiveItems)
                    {
                        DataTable dtData = dtDatas.Find(x => x.TableName == receiveItem.Key);

                        foreach (DataRow pageRow in dtPage.Rows)
                        {
                            pageNo++;

                            #region Gen 表頭
                            {
                                rowNo = this.GenReportE1Head(sheet, dateCellStyle, rowNo, dtHead, pageNo, totalPage);
                            }
                            #endregion

                            string receiveType = pageRow["Receive_Type"].ToString().Replace("'", "");
                            string yearId = pageRow["Year_Id"].ToString().Replace("'", "");
                            string termId = pageRow["Term_Id"].ToString().Replace("'", "");
                            string depId = pageRow["Dep_Id"].ToString().Replace("'", "");
                            string receiveId = pageRow["Receive_Id"].ToString().Replace("'", "");
                            string deptId = pageRow["Dept_Id"].ToString().Replace("'", "");
                            string deptName = pageRow["Dept_Name"].ToString();

                            #region Gen 表身
                            {
                                rowNo++;
                                rowNo = this.GenReportE1Body(sheet, leftCellStyle, rightCellStyle, centerCellStyle, moneyCellStyle
                                    , rowNo, dtData, receiveType, yearId, termId, depId, receiveId, receiveItem.Key, receiveItem.Value, deptId, deptName);
                            }
                            #endregion

                            #region 空白行+插入分頁
                            rowNo++;
                            sheet.CreateRow(rowNo);
                            sheet.SetRowBreak(rowNo);
                            #endregion
                        }

                    }
                }
                #endregion

                #region 將 ODSWorkbook 轉成 byte[]
                wb.WriteToBytes(out content);
                #endregion

                sheet = null;
                wb = null;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg;
        }

        /// <summary>
        /// 產生 繳費收費項目明細分析表 表頭
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo"></param>
        /// <param name="dtHead"></param>
        /// <param name="pageNo"></param>
        /// <param name="totalPage"></param>
        /// <returns>傳回最後的 Row No</returns>
        private int GenReportE1Head(ODSSheet sheet, ODSCellStyle dateCellStyle, int rowNo, DataTable dtHead, int pageNo, int totalPage)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            #region Head Row 0
            genRowNo++;
            row = sheet.CreateRow(genRowNo);
            DataRow dRow = dtHead.Rows[0];

            #region 學校名稱
            {
                int colNo = 1;
                string value = dRow["Sch_Name"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 報表名稱
            {
                int colNo = 4;
                string value = dRow["ReportName"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion
            #endregion

            #region Head Row 1
            genRowNo++;
            row = sheet.CreateRow(genRowNo);

            #region 學年
            {
                int colNo = 1;
                string value = "學年：";
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);

                colNo = 2;
                value = dRow["Year_Name"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 學期
            {
                int colNo = 5;
                string value = "學期：";
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);

                colNo = 6;
                value = dRow["Term_Name"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 繳費狀態
            {
                string receiveStatusName = dRow["ReceiveStatusName"].ToString();
                if (!String.IsNullOrEmpty(receiveStatusName))
                {
                    int colNo = 9;
                    string value = "繳費狀態：";
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);

                    colNo = 10;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(receiveStatusName);
                }
            }
            #endregion
            #endregion

            #region Head Row 2
            genRowNo++;
            row = sheet.CreateRow(genRowNo);

            #region 商家代號
            {
                int colNo = 1;
                string value = "商家代號：";
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);

                colNo = 2;
                value = dRow["Receive_Type"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 代收費用
            {
                int colNo = 5;
                string value = "代收費用：";
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);

                colNo = 6;
                value = dRow["Receive_Name"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 批號
            {
                string upNo = dRow["UpNo"].ToString();
                if (!String.IsNullOrEmpty(upNo))
                {
                    int colNo = 9;
                    string value = "批號：";
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);

                    colNo = 10;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(upNo);
                }
            }
            #endregion

            #region 日期
            {
                int colNo = 11;
                string value = dRow["ReportDate"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
                cell.CellStyle = dateCellStyle;
            }
            #endregion

            #region 頁數
            {
                int colNo = 13;
                string value = String.Format("第{0}頁/共{1}頁", pageNo, totalPage);
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion
            #endregion

            return genRowNo;
        }

        /// <summary>
        /// 產生 繳費收費項目明細分析表 表身
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="leftCellStyle"></param>
        /// <param name="rightCellStyle"></param>
        /// <param name="centerCellStyle"></param>
        /// <param name="moneyCellStyle"></param>
        /// <param name="rowNo"></param>
        /// <param name="dtData"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="receiveItemKey"></param>
        /// <param name="receiveItemName"></param>
        /// <param name="deptId"></param>
        /// <param name="deptName"></param>
        /// <returns></returns>
        private int GenReportE1Body(ODSSheet sheet, ODSCellStyle leftCellStyle, ODSCellStyle rightCellStyle, ODSCellStyle centerCellStyle, ODSCellStyle moneyCellStyle
            , int rowNo, DataTable dtData, string receiveType, string yearId, string termId, string depId, string receiveId, string receiveItemKey, string receiveItemName, string deptId, string deptName)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            #region Data Row 0, 1, 2
            {
                #region 欄位名稱
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);

                    #region 識別名稱
                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("識別名稱");
                    cell.CellStyle = centerCellStyle;
                    cell.SetMergedCount(0, 5);
                    #endregion

                    #region 收費金額
                    colNo = 7;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("收費金額");
                    cell.CellStyle = rightCellStyle;
                    cell.SetMergedCount(0, 1);
                    #endregion

                    #region 人數
                    colNo = 9;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("人數");
                    cell.CellStyle = rightCellStyle;
                    cell.SetMergedCount(0, 1);
                    #endregion

                    #region 合計
                    colNo = 11;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("合計");
                    cell.CellStyle = rightCellStyle;
                    cell.SetMergedCount(0, 2);
                    #endregion

                    #region 空白 Cell
                    colNo = 14;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("");
                    cell.CellStyle = leftCellStyle;
                    #endregion
                }
                #endregion

                #region 收入科目名稱
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);

                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(receiveItemName);
                    cell.CellStyle = leftCellStyle;
                    cell.SetMergedCount(0, 2);
                }
                #endregion

                #region 部別名稱
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);

                    int colNo = 2;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(deptName);
                    cell.CellStyle = leftCellStyle;
                    cell.SetMergedCount(0, 2);
                }
                #endregion
            }
            #endregion

            #region 系所名稱, 合計資料
            {
                #region [MDY:20220530] Checkmarx 調整
                #region [OLD]
                //DataRow[] dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Receive_Item_Key='{5}' AND Dept_Id='{6}'", receiveType, yearId, termId, depId, receiveId, receiveItemKey, deptId), "MAJOR_FLAG DESC, Major_Id, CLASS_FLAG DESC, Class_Id, Receive_Item_Amount");
                #endregion

                DataRow[] dRows = dtData.Rows.Cast<DataRow>().Where(
                        drow => drow["Receive_Type"].Equals(receiveType)
                            && drow["Year_Id"].Equals(yearId)
                            && drow["Term_Id"].Equals(termId)
                            && drow["Dep_Id"].Equals(depId)
                            && drow["Receive_Id"].Equals(receiveId)
                            && drow["Receive_Item_Key"].Equals(receiveItemKey)
                            && drow["Dept_Id"].Equals(deptId))
                        .OrderByDescending(drow => drow.Field<string>("MAJOR_FLAG"))
                        .ThenBy(drow => drow.Field<string>("Major_Id"))
                        .ThenByDescending(drow => drow.Field<string>("CLASS_FLAG"))
                        .ThenBy(drow => drow.Field<string>("Class_Id"))
                        .ThenBy(drow => drow.Field<string>("Receive_Item_Amount")).ToArray();
                #endregion

                if (dRows != null && dRows.Length > 0)
                {
                    string currentMajorId = null;
                    foreach (DataRow dRow in dRows)
                    {
                        string majorFlag = dRow["MAJOR_FLAG"].ToString().Trim();
                        string classFlag = dRow["CLASS_FLAG"].ToString().Trim();
                        string majorId = dRow.IsNull("Major_Id") ? String.Empty : dRow["Major_Id"].ToString().Trim();
                        //string classId = dRow.IsNull("Class_Id") ? String.Empty : dRow["Class_Id"].ToString().Trim();
                        string className = dRow["Class_Name"].ToString().Trim();
                        double? receiveItemAmount = null;
                        if (!dRow.IsNull("Receive_Item_Amount"))
                        {
                            receiveItemAmount = Convert.ToDouble(dRow["Receive_Item_Amount"].ToString());
                        }
                        int dataCount = Convert.ToInt32(dRow["Data_Count"].ToString());
                        double sumAmount = Convert.ToDouble(dRow["Sum_Amount"].ToString());

                        if (currentMajorId != majorId && majorFlag == "Y")
                        {
                            #region 系所名稱
                            {
                                currentMajorId = majorId;
                                string majorName = dRow["Major_Name"].ToString().Trim();

                                genRowNo++;
                                row = sheet.CreateRow(genRowNo);

                                int colNo = 3;
                                cell = row.CreateCell(colNo, CellType.String);
                                cell.SetCellValue(majorName);
                                cell.CellStyle = leftCellStyle;
                                cell.SetMergedCount(0, 2);
                            }
                            #endregion
                        }

                        #region 合計資料 (班別合計 Or 系所合計 Or 部別合計)
                        {
                            genRowNo++;
                            row = sheet.CreateRow(genRowNo);

                            #region 合計名稱
                            {
                                string dataKind = null;
                                int colNo = 1;
                                int mergeCount = 0;
                                if (majorFlag == "N")
                                {
                                    colNo = 5;
                                    dataKind = "部別合計：";
                                    mergeCount = 1;
                                }
                                else if (classFlag == "N")
                                {
                                    colNo = 5;
                                    dataKind = "系所合計：";
                                    mergeCount = 1;
                                }
                                else
                                {
                                    colNo = 4;
                                    dataKind = className; //班級名稱
                                    mergeCount = 2;
                                }

                                cell = row.CreateCell(colNo, CellType.String);
                                cell.SetCellValue(dataKind);
                                cell.CellStyle = leftCellStyle;
                                cell.SetMergedCount(0, mergeCount);
                            }
                            #endregion

                            #region 收費金額
                            {
                                int colNo = 7;
                                cell = row.CreateCell(colNo, CellType.Numeric);
                                if (receiveItemAmount != null)
                                {
                                    cell.SetCellValue(receiveItemAmount.Value);
                                }
                                else
                                {
                                    cell.SetCellValue("");
                                }
                                cell.CellStyle = moneyCellStyle;
                                cell.SetMergedCount(0, 1);
                            }
                            #endregion

                            #region 人數
                            {
                                int colNo = 9;
                                cell = row.CreateCell(colNo, CellType.Numeric);
                                cell.SetCellValue(dataCount);
                                cell.CellStyle = rightCellStyle;
                                cell.SetMergedCount(0, 1);
                            }
                            #endregion

                            #region 合計
                            {
                                int colNo = 11;
                                cell = row.CreateCell(colNo, CellType.Numeric);
                                cell.SetCellValue(sumAmount);
                                cell.CellStyle = moneyCellStyle;
                                cell.SetMergedCount(0, 2);
                            }
                            #endregion

                            #region 空白 Cell
                            {
                                int colNo = 14;
                                cell = row.CreateCell(colNo, CellType.String);
                                cell.SetCellValue("");
                                cell.CellStyle = leftCellStyle;
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
                else
                {
                    //無資料
                }
            }
            #endregion

            return genRowNo;
        }

        /// <summary>
        /// 繳費收費項目分類統計表
        /// </summary>
        /// <param name="dtHead">表頭資料</param>
        /// <param name="dtPage">部別(分頁)資料</param>
        /// <param name="dtMajor">系所資料</param>
        /// <param name="dtClass">班級資料</param>
        /// <param name="receiveItems">收入科目資料</param>
        /// <param name="receiveItemAmounts">收入科目金額資料</param>
        /// <param name="dtDatas">表身資料集合</param>
        /// <param name="content">成功則傳回產生檔案的 byte 陣列，否則傳回 null</param>
        /// <returns>成功則傳回 null，否怎傳回錯誤訊息</returns>
        public string GenReportE2(DataTable dtHead, DataTable dtPage, DataTable dtMajor, DataTable dtClass, KeyValueList<string> receiveItems, KeyValueList<double> receiveItemAmounts, List<DataTable> dtDatas, out byte[] content)
        {
            content = null;
            string errmsg = null;
            try
            {
                #region 使用 ODSWorkbook 產生 ODS
                string sheetName = "Sheet1";

                ODSWorkbook wb = new ODSWorkbook();
                ODSSheet sheet = wb.CreateSheet(sheetName);
                ODSRow row = null;
                ODSCell cell = null;

                #region 指定紙張大小
                wb.PageStyle.PaperSize = PapeSizeKind.A4;
                #endregion

                #region 指定直式或橫式 Lendscape=橫式 Portrait=直式
                wb.PageStyle.PaperOrientation = PageOrientationKind.Lendscape;
                #endregion

                #region 指定縮放 (0 ~ 100)
                //不提供
                //sheet.PrintSetup.Scale = 100;
                //sheet.PrintSetup.FitWidth = 0;
                //sheet.PrintSetup.FitHeight = 0;
                #endregion

                #region 藏隱格線
                wb.IsShowGrid = false;
                #endregion

                #region 日期 (左靠、yyyy/mm/dd)儲存格格式
                ODSCellStyle dateCellStyle = wb.CreateCellStyle();
                dateCellStyle.SetDataFormat(ODSDateFormat.Date);
                dateCellStyle.HAlignment = HorizontalAlignment.Left;
                #endregion

                #region left (左靠、底線)儲存格格式
                ODSCellStyle leftCellStyle = wb.CreateCellStyle();
                leftCellStyle.HAlignment = HorizontalAlignment.Left;
                leftCellStyle.BottomBorderStyle = BorderStyle.Thin;
                #endregion

                #region rigth (右靠、底線)儲存格格式
                ODSCellStyle rightCellStyle = wb.CreateCellStyle();
                rightCellStyle.HAlignment = HorizontalAlignment.Right;
                rightCellStyle.BottomBorderStyle = BorderStyle.Thin;
                #endregion

                #region rigth2 (右靠、無底線)儲存格格式
                ODSCellStyle right2CellStyle = wb.CreateCellStyle();
                right2CellStyle.HAlignment = HorizontalAlignment.Right;
                #endregion

                #region center (置中、底線)儲存格格式
                ODSCellStyle centerCellStyle = wb.CreateCellStyle();
                centerCellStyle.HAlignment = HorizontalAlignment.Center;
                centerCellStyle.BottomBorderStyle = BorderStyle.Thin;
                #endregion

                #region 金額(千分位逗號、右靠、底線)儲存格格式
                ODSCellStyle moneyCellStyle = wb.CreateCellStyle();
                moneyCellStyle.SetDataFormat(ODSNumberFormat.IntegerComma);
                moneyCellStyle.HAlignment = HorizontalAlignment.Right;
                moneyCellStyle.BottomBorderStyle = BorderStyle.Thin;
                #endregion

                #region 金額(千分位逗號、右靠、無底線)儲存格格式
                ODSCellStyle money2CellStyle = wb.CreateCellStyle();
                money2CellStyle.SetDataFormat(ODSNumberFormat.IntegerComma);
                money2CellStyle.HAlignment = HorizontalAlignment.Right;
                #endregion

                int rowNo = 0;
                int pageNo = 0;
                int totalPage = dtPage.Rows.Count;

                if (totalPage == 0)
                {
                    #region Gen 表頭
                    {
                        rowNo = this.GenReportE1Head(sheet, dateCellStyle, rowNo, dtHead, pageNo, totalPage);
                    }
                    #endregion

                    #region 查無資料
                    rowNo++;
                    row = sheet.CreateRow(rowNo);
                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("查無資料");
                    #endregion
                }
                else
                {
                    foreach (DataRow pageRow in dtPage.Rows)
                    {
                        pageNo++;

                        #region Gen 表頭
                        {
                            rowNo = this.GenReportE2Head(sheet, dateCellStyle, rowNo, dtHead, pageNo, totalPage);
                        }
                        #endregion

                        string receiveType = pageRow["Receive_Type"].ToString().Replace("'", "");
                        string yearId = pageRow["Year_Id"].ToString().Replace("'", "");
                        string termId = pageRow["Term_Id"].ToString().Replace("'", "");
                        string depId = pageRow["Dep_Id"].ToString().Replace("'", "");
                        string receiveId = pageRow["Receive_Id"].ToString().Replace("'", "");
                        string deptId = pageRow["Dept_Id"].ToString().Replace("'", "");
                        string deptName = pageRow["Dept_Name"].ToString();

                        #region Gen 表身
                        {
                            rowNo++;
                            rowNo = this.GenReportE2Body(sheet, leftCellStyle, rightCellStyle, right2CellStyle, centerCellStyle, moneyCellStyle, money2CellStyle
                                , rowNo, dtMajor, dtClass, dtDatas, receiveItems, receiveItemAmounts, receiveType, yearId, termId, depId, receiveId, deptId, deptName, (pageNo == totalPage));
                        }
                        #endregion

                        #region 空白行+插入分頁
                        rowNo++;
                        sheet.CreateRow(rowNo);
                        sheet.SetRowBreak(rowNo);
                        #endregion
                    }
                }
                #endregion

                #region 將 ODSWorkbook 轉成 byte[]
                wb.WriteToBytes(out content);
                #endregion

                sheet = null;
                wb = null;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg;
        }

        /// <summary>
        /// 產生 繳費收費項目分類統計表 表頭
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNo"></param>
        /// <param name="dtHead"></param>
        /// <param name="pageNo"></param>
        /// <param name="totalPage"></param>
        /// <returns>傳回最後的 Row No</returns>
        private int GenReportE2Head(ODSSheet sheet, ODSCellStyle dateCellStyle, int rowNo, DataTable dtHead, int pageNo, int totalPage)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            #region Head Row 0
            genRowNo++;
            row = sheet.CreateRow(genRowNo);
            DataRow dRow = dtHead.Rows[0];

            #region 學校名稱
            {
                int colNo = 1;
                string value = dRow["Sch_Name"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 報表名稱
            {
                int colNo = 4;
                string value = dRow["ReportName"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion
            #endregion

            #region Head Row 1
            genRowNo++;
            row = sheet.CreateRow(genRowNo);

            #region 學年
            {
                int colNo = 1;
                string value = "學年：";
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);

                colNo = 2;
                value = dRow["Year_Name"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 學期
            {
                int colNo = 5;
                string value = "學期：";
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);

                colNo = 6;
                value = dRow["Term_Name"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 繳費狀態
            {
                string receiveStatusName = dRow["ReceiveStatusName"].ToString();
                if (!String.IsNullOrEmpty(receiveStatusName))
                {
                    int colNo = 9;
                    string value = "繳費狀態：";
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);

                    colNo = 10;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(receiveStatusName);
                }
            }
            #endregion
            #endregion

            #region Head Row 2
            genRowNo++;
            row = sheet.CreateRow(genRowNo);

            #region 商家代號
            {
                int colNo = 1;
                string value = "商家代號：";
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);

                colNo = 2;
                value = dRow["Receive_Type"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 代收費用
            {
                int colNo = 5;
                string value = "代收費用：";
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);

                colNo = 6;
                value = dRow["Receive_Name"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 批號
            {
                string upNo = dRow["UpNo"].ToString();
                if (!String.IsNullOrEmpty(upNo))
                {
                    int colNo = 9;
                    string value = "批號：";
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(value);

                    colNo = 10;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue(upNo);
                }
            }
            #endregion

            #region 日期
            {
                int colNo = 11;
                string value = dRow["ReportDate"].ToString();
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
                cell.CellStyle = dateCellStyle;
            }
            #endregion

            #region 頁數
            {
                int colNo = 13;
                string value = String.Format("第{0}頁/共{1}頁", pageNo, totalPage);
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion
            #endregion

            return genRowNo;
        }

        /// <summary>
        /// 產生 繳費收費項目分類統計表 表身
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="leftCellStyle"></param>
        /// <param name="rightCellStyle">右靠、有底線 儲存格樣式</param>
        /// <param name="right2CellStyle">右靠、無底線 儲存格樣式</param>
        /// <param name="centerCellStyle"></param>
        /// <param name="moneyCellStyle">金額(千分位逗號、右靠、有底線)儲存格格式</param>
        /// <param name="money2CellStyle">金額(千分位逗號、右靠、無底線)儲存格格式</param>
        /// <param name="rowNo"></param>
        /// <param name="dtMajor">系所資料</param>
        /// <param name="dtClass">班級資料</param>
        /// <param name="dtDatas"></param>
        /// <param name="receiveItems"></param>
        /// <param name="receiveItemAmounts"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="deptId"></param>
        /// <param name="deptName"></param>
        /// <param name="isLastPage">是否為最後一頁</param>
        /// <returns></returns>
        private int GenReportE2Body(ODSSheet sheet, ODSCellStyle leftCellStyle, ODSCellStyle rightCellStyle, ODSCellStyle right2CellStyle, ODSCellStyle centerCellStyle, ODSCellStyle moneyCellStyle, ODSCellStyle money2CellStyle
            , int rowNo, DataTable dtMajor, DataTable dtClass, List<DataTable> dtDatas, KeyValueList<string> receiveItems, KeyValueList<double> receiveItemAmounts
            , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId, string deptName, bool isLastPage)
        {
            ODSRow row = null;
            ODSCell cell = null;
            int genRowNo = rowNo;

            //總計的 Column No
            int rowSumColNo = 2 + receiveItemAmounts.Count * 2;

            #region Data Row 0 (部別名稱)
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);

                int colNo = 1;
                cell = row.CreateCell(colNo, CellType.String);
                cell.SetCellValue(deptName);
                cell.CellStyle = leftCellStyle;
                cell.SetMergedCount(0, 2);
            }
            #endregion

            #region 逐系所產生表身
            #region [MDY:20220530] Checkmarx 調整
            #region [OLD]
            //DataRow[] drMajors = dtMajor.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType, yearId, termId, depId, receiveId, deptId), "Major_Id");
            #endregion

            DataRow[] drMajors = dtMajor.Rows.Cast<DataRow>().Where(
                    drow => drow["Receive_Type"].Equals(receiveType)
                        && drow["Year_Id"].Equals(yearId)
                        && drow["Term_Id"].Equals(termId)
                        && drow["Dep_Id"].Equals(depId)
                        && drow["Receive_Id"].Equals(receiveId)
                        && drow["Dept_Id"].Equals(deptId)
                    ).OrderBy(drow => drow.Field<string>("Major_Id")).ToArray();
            #endregion

            if (drMajors != null && drMajors.Length > 0)
            {
                foreach (DataRow drMajor in drMajors)
                {
                    string majorId = drMajor["Major_Id"].ToString();
                    string majorName = drMajor["Major_Name"].ToString();

                    #region Data Row (系所名稱)
                    {
                        genRowNo++;
                        row = sheet.CreateRow(genRowNo);

                        int colNo = 2;
                        cell = row.CreateCell(colNo, CellType.String);
                        cell.SetCellValue(majorName);
                        cell.CellStyle = leftCellStyle;
                        cell.SetMergedCount(0, 2);
                    }
                    #endregion

                    #region 逐班級產生表身
                    #region [MDY:20220530] Checkmarx 調整
                    #region [OLD]
                    //DataRow[] drClasses = dtClass.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType, yearId, termId, depId, receiveId, deptId, majorId), "Class_Id");
                    #endregion

                    DataRow[] drClasses = dtClass.Rows.Cast<DataRow>().Where(
                            drow => drow["Receive_Type"].Equals(receiveType)
                                && drow["Year_Id"].Equals(yearId)
                                && drow["Term_Id"].Equals(termId)
                                && drow["Dep_Id"].Equals(depId)
                                && drow["Receive_Id"].Equals(receiveId)
                                && drow["Dept_Id"].Equals(deptId)
                                && drow["Major_Id"].Equals(majorId)
                            ).OrderBy(drow => drow.Field<string>("Class_Id")).ToArray();
                    #endregion

                    if (drClasses != null && drClasses.Length > 0)
                    {
                        foreach (DataRow drClass in drClasses)
                        {
                            string classId = drClass["Class_Id"].ToString();
                            string className = drClass["Class_Name"].ToString();

                            #region Data Row (班級名稱)
                            {
                                genRowNo++;
                                row = sheet.CreateRow(genRowNo);

                                int colNo = 1;
                                cell = row.CreateCell(colNo, CellType.String);
                                cell.SetCellValue(className);
                                cell.CellStyle = leftCellStyle;
                                cell.SetMergedCount(0, (receiveItemAmounts.Count + 1) * 2); //每個收入科目金額 2 格 + 總計 2 格
                            }
                            #endregion

                            #region Data Row (收入科目名稱 + 總計)
                            {
                                genRowNo++;
                                row = sheet.CreateRow(genRowNo);

                                #region 收入科目名稱
                                {
                                    int colNo = 2;
                                    foreach (KeyValue<double> receiveItemAmount in receiveItemAmounts)
                                    {
                                        KeyValue<string> receiveItem = receiveItems.Find(x => x.Key == receiveItemAmount.Key);

                                        cell = row.CreateCell(colNo, CellType.String);
                                        cell.SetCellValue(receiveItem.Value);
                                        cell.CellStyle = right2CellStyle;
                                        cell.SetMergedCount(0, 1); //每個收入科目金額 2 格
                                        colNo += 2;
                                    }
                                }
                                #endregion

                                #region 總計
                                {
                                    cell = row.CreateCell(rowSumColNo, CellType.String);
                                    cell.SetCellValue("總計");
                                    cell.CellStyle = right2CellStyle;
                                    cell.SetMergedCount(0, 1); //總計 2 格
                                }
                                #endregion
                            }
                            #endregion

                            #region Data Row (單價)
                            {
                                genRowNo++;
                                row = sheet.CreateRow(genRowNo);

                                #region 單價
                                {
                                    int colNo = 1;
                                    cell = row.CreateCell(colNo, CellType.String);
                                    cell.SetCellValue("單　價：");
                                    cell.CellStyle = right2CellStyle;
                                }
                                #endregion

                                #region 收入科目金額資料
                                {
                                    int colNo = 2;
                                    foreach (KeyValue<double> receiveItemAmount in receiveItemAmounts)
                                    {
                                        cell = row.CreateCell(colNo, CellType.Numeric);
                                        cell.SetCellValue(receiveItemAmount.Value);
                                        cell.CellStyle = money2CellStyle;
                                        cell.SetMergedCount(0, 1); //每個收入科目金額 2 格
                                        colNo += 2;
                                    }
                                }
                                #endregion
                            }
                            #endregion

                            #region Data Row (人數)
                            {
                                genRowNo++;
                                row = sheet.CreateRow(genRowNo);

                                #region 人數
                                {
                                    int colNo = 1;
                                    cell = row.CreateCell(colNo, CellType.String);
                                    cell.SetCellValue("人　數：");
                                    cell.CellStyle = right2CellStyle;
                                }
                                #endregion

                                #region 收入科目人數資料
                                {
                                    int colNo = 2;
                                    DataTable dtData = null;
                                    foreach (KeyValue<double> receiveItemAmount in receiveItemAmounts)
                                    {
                                        string receiveItemKey = receiveItemAmount.Key;
                                        double receiveItemValue = receiveItemAmount.Value;
                                        if (dtData == null || dtData.TableName != receiveItemKey)
                                        {
                                            dtData = dtDatas.Find(x => x.TableName == receiveItemKey);
                                        }

                                        if (dtData != null)
                                        {
                                            #region [MDY:20220530] Checkmarx 調整
                                            #region [OLD]
                                            //DataRow[] drDatas = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Class_Id = '{7}' AND Receive_Item_Key = '{8}' AND Receive_Item_Amount = {9}", receiveType, yearId, termId, depId, receiveId, deptId, majorId, classId, receiveItemKey, receiveItemValue));
                                            #endregion

                                            DataRow[] drDatas = dtData.Rows.Cast<DataRow>().Where(
                                                    drow => drow["Receive_Type"].Equals(receiveType)
                                                        && drow["Year_Id"].Equals(yearId)
                                                        && drow["Term_Id"].Equals(termId)
                                                        && drow["Dep_Id"].Equals(depId)
                                                        && drow["Receive_Id"].Equals(receiveId)
                                                        && drow["Dept_Id"].Equals(deptId)
                                                        && drow["Major_Id"].Equals(majorId)
                                                        && drow["Class_Id"].Equals(classId)
                                                        && drow["Receive_Item_Key"].Equals(receiveItemKey)
                                                        && drow["Receive_Item_Amount"].Equals(receiveItemValue)).ToArray();
                                            #endregion

                                            if (drDatas != null && drDatas.Length > 0)
                                            {
                                                double dataCount = Convert.ToDouble(drDatas[0]["Data_Count"]);
                                                cell = row.CreateCell(colNo, CellType.Numeric);
                                                cell.SetCellValue(dataCount);
                                                cell.CellStyle = money2CellStyle;
                                                cell.SetMergedCount(0, 1); //每個收入科目金額 2 格
                                            }
                                        }
                                        colNo += 2;
                                    }
                                }
                                #endregion
                            }
                            #endregion

                            #region Data Row (總金額)
                            {
                                genRowNo++;
                                row = sheet.CreateRow(genRowNo);

                                #region 總金額
                                {
                                    int colNo = 1;
                                    cell = row.CreateCell(colNo, CellType.String);
                                    cell.SetCellValue("總金額：");
                                    cell.CellStyle = rightCellStyle;
                                }
                                #endregion

                                #region 收入科目總金額資料
                                double rowSumAmount = 0;    //各收入科目總金額的總計
                                {
                                    int colNo = 2;
                                    DataTable dtData = null;
                                    foreach (KeyValue<double> receiveItemAmount in receiveItemAmounts)
                                    {
                                        string receiveItemKey = receiveItemAmount.Key;
                                        double receiveItemValue = receiveItemAmount.Value;
                                        if (dtData == null || dtData.TableName != receiveItemKey)
                                        {
                                            dtData = dtDatas.Find(x => x.TableName == receiveItemKey);
                                        }

                                        DataRow drData = null;
                                        if (dtData != null)
                                        {
                                            #region [MDY:20220530] Checkmarx 調整
                                            #region [OLD]
                                            //DataRow[] drDatas = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Class_Id = '{7}' AND Receive_Item_Key = '{8}' AND Receive_Item_Amount = {9}", receiveType, yearId, termId, depId, receiveId, deptId, majorId, classId, receiveItemKey, receiveItemValue));
                                            #endregion

                                            DataRow[] drDatas = dtData.Rows.Cast<DataRow>().Where(
                                                    drow => drow["Receive_Type"].Equals(receiveType)
                                                        && drow["Year_Id"].Equals(yearId)
                                                        && drow["Term_Id"].Equals(termId)
                                                        && drow["Dep_Id"].Equals(depId)
                                                        && drow["Receive_Id"].Equals(receiveId)
                                                        && drow["Dept_Id"].Equals(deptId)
                                                        && drow["Major_Id"].Equals(majorId)
                                                        && drow["Class_Id"].Equals(classId)
                                                        && drow["Receive_Item_Key"].Equals(receiveItemKey)
                                                        && drow["Receive_Item_Amount"].Equals(receiveItemValue)).ToArray();
                                            #endregion

                                            if (drDatas != null && drDatas.Length > 0)
                                            {
                                                drData = drDatas[0];
                                                //double sumAmount = Convert.ToDouble(drDatas[0]["Sum_Amount"]);
                                                //cell = row.CreateCell(colNo, CellType.Numeric);
                                                //cell.SetCellValue(sumAmount);
                                                //cell.CellStyle = moneyCellStyle;
                                                //cell.SetMergedCount(0, 1); //每個收入科目金額 2 格
                                                //rowSumAmount += sumAmount;
                                            }
                                        }
                                        if (drData != null)
                                        {
                                            double sumAmount = Convert.ToDouble(drData["Sum_Amount"]);
                                            cell = row.CreateCell(colNo, CellType.Numeric);
                                            cell.SetCellValue(sumAmount);
                                            cell.CellStyle = moneyCellStyle;
                                            cell.SetMergedCount(0, 1); //每個收入科目金額 2 格
                                            rowSumAmount += sumAmount;
                                        }
                                        else
                                        {
                                            //總金額有畫底線，所以無資料要補空白的儲存格
                                            cell = row.CreateCell(colNo, CellType.Numeric);
                                            cell.SetCellValue(String.Empty);
                                            cell.CellStyle = moneyCellStyle;
                                            cell.SetMergedCount(0, 1); //每個收入科目金額 2 格
                                        }
                                        colNo += 2;
                                    }
                                }
                                #endregion

                                #region 總計
                                {
                                    cell = row.CreateCell(rowSumColNo, CellType.Numeric);
                                    cell.SetCellValue(rowSumAmount);
                                    cell.CellStyle = moneyCellStyle;
                                    cell.SetMergedCount(0, 1); //總計 2 格
                                }
                                #endregion
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region Data Row (系所合計)
                    {
                        genRowNo++;
                        row = sheet.CreateRow(genRowNo);

                        #region 系所合計
                        {
                            int colNo = 1;
                            cell = row.CreateCell(colNo, CellType.String);
                            cell.SetCellValue("系所合計：");
                            cell.CellStyle = right2CellStyle;
                        }
                        #endregion

                        #region 收入科目金額 系所合計 資料
                        double rowSumAmount = 0;    //各收入科目系所合計的總計
                        {
                            int colNo = 2;
                            DataTable dtData = null;
                            foreach (KeyValue<double> receiveItemAmount in receiveItemAmounts)
                            {
                                string receiveItemKey = receiveItemAmount.Key;
                                double receiveItemValue = receiveItemAmount.Value;
                                if (dtData == null || dtData.TableName != receiveItemKey)
                                {
                                    dtData = dtDatas.Find(x => x.TableName == receiveItemKey);
                                }

                                if (dtData != null)
                                {
                                    #region [MDY:20220530] Checkmarx 調整
                                    #region [OLD]
                                    //DataRow[] drDatas = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Receive_Item_Key = '{7}' AND Receive_Item_Amount = {8} AND CLASS_FLAG = 'N'", receiveType, yearId, termId, depId, receiveId, deptId, majorId, receiveItemKey, receiveItemValue));
                                    #endregion

                                    DataRow[] drDatas = dtData.Rows.Cast<DataRow>().Where(
                                            drow => drow["Receive_Type"].Equals(receiveType)
                                                && drow["Year_Id"].Equals(yearId)
                                                && drow["Term_Id"].Equals(termId)
                                                && drow["Dep_Id"].Equals(depId)
                                                && drow["Receive_Id"].Equals(receiveId)
                                                && drow["Dept_Id"].Equals(deptId)
                                                && drow["Major_Id"].Equals(majorId)
                                                && drow["Receive_Item_Key"].Equals(receiveItemKey)
                                                && drow["Receive_Item_Amount"].Equals(receiveItemValue)
                                                && drow["CLASS_FLAG"].Equals("N")).ToArray();
                                    #endregion

                                    if (drDatas != null && drDatas.Length > 0)
                                    {
                                        double sumAmount = Convert.ToDouble(drDatas[0]["Sum_Amount"]);
                                        cell = row.CreateCell(colNo, CellType.Numeric);
                                        cell.SetCellValue(sumAmount);
                                        cell.CellStyle = money2CellStyle;
                                        cell.SetMergedCount(0, 1); //每個收入科目金額 2 格
                                        rowSumAmount += sumAmount;
                                    }
                                }
                                colNo += 2;
                            }
                        }
                        #endregion

                        #region 總計
                        {
                            cell = row.CreateCell(rowSumColNo, CellType.Numeric);
                            cell.SetCellValue(rowSumAmount);
                            cell.CellStyle = moneyCellStyle;
                            cell.SetMergedCount(0, 1); //總計 2 格
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            #endregion

            #region Data Row (部別合計)
            {
                genRowNo++;
                row = sheet.CreateRow(genRowNo);

                #region 部別合計
                {
                    int colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("部別合計：");
                    cell.CellStyle = right2CellStyle;
                }
                #endregion

                #region 收入科目金額 部別合計 資料
                double rowSumAmount = 0;    //各收入科目部別合計的總計
                {
                    int colNo = 2;
                    DataTable dtData = null;
                    foreach (KeyValue<double> receiveItemAmount in receiveItemAmounts)
                    {
                        string receiveItemKey = receiveItemAmount.Key;
                        double receiveItemValue = receiveItemAmount.Value;
                        if (dtData == null || dtData.TableName != receiveItemKey)
                        {
                            dtData = dtDatas.Find(x => x.TableName == receiveItemKey);
                        }

                        if (dtData != null)
                        {
                            #region [MDY:20220530] Checkmarx 調整
                            #region [OLD]
                            //DataRow[] drDatas = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Receive_Item_Key = '{6}' AND Receive_Item_Amount = {7} AND MAJOR_FLAG = 'N' AND CLASS_FLAG = 'N'", receiveType, yearId, termId, depId, receiveId, deptId, receiveItemKey, receiveItemValue));
                            #endregion

                            DataRow[] drDatas = dtData.Rows.Cast<DataRow>().Where(
                                    drow => drow["Receive_Type"].Equals(receiveType)
                                        && drow["Year_Id"].Equals(yearId)
                                        && drow["Term_Id"].Equals(termId)
                                        && drow["Dep_Id"].Equals(depId)
                                        && drow["Receive_Id"].Equals(receiveId)
                                        && drow["Dept_Id"].Equals(deptId)
                                        && drow["Receive_Item_Key"].Equals(receiveItemKey)
                                        && drow["Receive_Item_Amount"].Equals(receiveItemValue)
                                        && drow["MAJOR_FLAG"].Equals("N")
                                        && drow["CLASS_FLAG"].Equals("N")).ToArray();
                            #endregion

                            if (drDatas != null && drDatas.Length > 0)
                            {
                                double sumAmount = Convert.ToDouble(drDatas[0]["Sum_Amount"]);
                                cell = row.CreateCell(colNo, CellType.Numeric);
                                cell.SetCellValue(sumAmount);
                                cell.CellStyle = money2CellStyle;
                                cell.SetMergedCount(0, 1); //每個收入科目金額 2 格
                                rowSumAmount += sumAmount;
                            }
                        }
                        colNo += 2;
                    }
                }
                #endregion

                #region 總計
                {
                    cell = row.CreateCell(rowSumColNo, CellType.Numeric);
                    cell.SetCellValue(rowSumAmount);
                    cell.CellStyle = moneyCellStyle;
                    cell.SetMergedCount(0, 1); //總計 2 格
                }
                #endregion
            }
            #endregion

            if (isLastPage)
            {
                #region Data Row (總合計)
                {
                    genRowNo++;
                    row = sheet.CreateRow(genRowNo);

                    #region 總合計
                    {
                        int colNo = 1;
                        cell = row.CreateCell(colNo, CellType.String);
                        cell.SetCellValue("總 合 計：");
                        cell.CellStyle = right2CellStyle;
                    }
                    #endregion

                    #region 收入科目金額 部別合計 資料
                    double rowSumAmount = 0;    //各收入科目總合計的總計
                    {
                        int colNo = 2;
                        DataTable dtData = null;
                        foreach (KeyValue<double> receiveItemAmount in receiveItemAmounts)
                        {
                            string receiveItemKey = receiveItemAmount.Key;
                            double receiveItemValue = receiveItemAmount.Value;
                            if (dtData == null || dtData.TableName != receiveItemKey)
                            {
                                dtData = dtDatas.Find(x => x.TableName == receiveItemKey);
                            }

                            if (dtData != null)
                            {
                                #region [MDY:20220530] Checkmarx 調整
                                #region [OLD]
                                //DataRow[] drDatas = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Receive_Item_Key = '{5}' AND Receive_Item_Amount = {6} AND DEPT_FLAG = 'N' AND MAJOR_FLAG = 'N' AND CLASS_FLAG = 'N'", receiveType, yearId, termId, depId, receiveId, receiveItemKey, receiveItemValue));
                                #endregion

                                DataRow[] drDatas = dtData.Rows.Cast<DataRow>().Where(
                                        drow => drow["Receive_Type"].Equals(receiveType)
                                            && drow["Year_Id"].Equals(yearId)
                                            && drow["Term_Id"].Equals(termId)
                                            && drow["Dep_Id"].Equals(depId)
                                            && drow["Receive_Id"].Equals(receiveId)
                                            && drow["Receive_Item_Key"].Equals(receiveItemKey)
                                            && drow["Receive_Item_Amount"].Equals(receiveItemValue)
                                            && drow["DEPT_FLAG"].Equals("N")
                                            && drow["MAJOR_FLAG"].Equals("N")
                                            && drow["CLASS_FLAG"].Equals("N")).ToArray();
                                #endregion

                                if (drDatas != null && drDatas.Length > 0)
                                {
                                    double sumAmount = Convert.ToDouble(drDatas[0]["Sum_Amount"]);
                                    cell = row.CreateCell(colNo, CellType.Numeric);
                                    cell.SetCellValue(sumAmount);
                                    cell.CellStyle = money2CellStyle;
                                    cell.SetMergedCount(0, 1); //每個收入科目金額 2 格
                                    rowSumAmount += sumAmount;
                                }
                            }
                            colNo += 2;
                        }
                    }
                    #endregion

                    #region 總計
                    {
                        cell = row.CreateCell(rowSumColNo, CellType.Numeric);
                        cell.SetCellValue(rowSumAmount);
                        cell.CellStyle = moneyCellStyle;
                        cell.SetMergedCount(0, 1); //總計 2 格
                    }
                    #endregion
                }
                #endregion
            }

            return genRowNo;
        }

        #endregion

        #region 匯出每日銷帳結果查詢結果檔 (C3400001)
        /// <summary>
        /// 匯出每日銷帳結果查詢結果檔 (C3400001)
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="sheetName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string ExportC3400001ResutFile(CancelResultEntity[] datas, string sheetName, out byte[] content)
        {
            content = null;
            string errmsg = null;

            try
            {
                #region 使用 ODSWorkbook 產生 ODS
                List<decimal> totalCounts = new List<decimal>();
                List<decimal> totalAmounts = new List<decimal>();

                ODSWorkbook wb = new ODSWorkbook();
                ODSSheet sheet = wb.CreateSheet(sheetName);
                ODSCell cell = null;
                int rowNo = 0, colNo = 0;

                #region 金額(千分位逗號、右靠)儲存格格式
                ODSCellStyle amountCellStyle = wb.CreateCellStyle();
                {
                    amountCellStyle.SetDataFormat(ODSNumberFormat.IntegerComma);
                    amountCellStyle.HAlignment = HorizontalAlignment.Right;
                }
                #endregion

                #region 學年 Row
                {
                    rowNo = 1;
                    ODSRow row = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("學年");
                    #endregion

                    #region 各資料(學年)欄位
                    foreach (CancelResultEntity data in datas)
                    {
                        colNo++;
                        cell = row.CreateCell(colNo, CellType.String);
                        cell.SetCellValue(data.YearName);

                        //初始化 total
                        totalCounts.Add(0);
                        totalAmounts.Add(0);
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("合計");
                    #endregion
                }
                #endregion

                #region 學期 Row
                {
                    rowNo++;
                    ODSRow row = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("學期");
                    #endregion

                    #region 各資料(學期)欄位
                    foreach (CancelResultEntity data in datas)
                    {
                        colNo++;
                        cell = row.CreateCell(colNo, CellType.String);
                        cell.SetCellValue(data.TermName);
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("");
                    #endregion
                }
                #endregion

                #region 費用別
                {
                    rowNo++;
                    ODSRow row = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("費用別");
                    #endregion

                    #region 各資料(費用別)欄位
                    foreach (CancelResultEntity data in datas)
                    {
                        colNo++;
                        cell = row.CreateCell(colNo, CellType.String);
                        cell.SetCellValue(data.ReceiveName);
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("");
                    #endregion
                }
                #endregion

                #region 入帳日
                {
                    rowNo++;
                    ODSRow row = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("入帳日");
                    #endregion

                    #region 各資料(入帳日)欄位
                    foreach (CancelResultEntity data in datas)
                    {
                        DateTime? accountDate = DataFormat.ConvertDateText(data.AccountDate);

                        colNo++;
                        cell = row.CreateCell(colNo, CellType.String);
                        cell.SetCellValue(accountDate == null ? String.Empty : accountDate.Value.ToString("yyyy/MM/dd"));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("");
                    #endregion
                }
                #endregion

                #region 學校自收
                {
                    rowNo++;
                    ODSRow row1 = sheet.CreateRow(rowNo);
                    rowNo++;
                    ODSRow row2 = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row1.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("學校自收筆數");
                    cell = row2.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("學校自收金額");
                    #endregion

                    #region 各資料(學校自收)欄位
                    decimal rowCount = 0M, rowAmount = 0M;
                    int idx = 0;
                    foreach (CancelResultEntity data in datas)
                    {
                        decimal myCount = data.SoCount;
                        decimal myAmount = data.SoAmount;
                        rowCount += myCount;
                        rowAmount += myAmount;
                        totalCounts[idx] += myCount;
                        totalAmounts[idx] += myAmount;
                        idx++;

                        colNo++;
                        cell = row1.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = row2.CreateCell(colNo, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row1.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = row2.CreateCell(colNo, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 統一代收
                {
                    rowNo++;
                    ODSRow row1 = sheet.CreateRow(rowNo);
                    rowNo++;
                    ODSRow row2 = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row1.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("統一代收筆數");
                    cell = row2.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("統一代收金額");
                    #endregion

                    #region 各資料(統一代收)欄位
                    decimal rowCount = 0M, rowAmount = 0M;
                    int idx = 0;
                    foreach (CancelResultEntity data in datas)
                    {
                        decimal myCount = data.GCount;
                        decimal myAmount = data.GAmount;
                        rowCount += myCount;
                        rowAmount += myAmount;
                        totalCounts[idx] += myCount;
                        totalAmounts[idx] += myAmount;
                        idx++;

                        colNo++;
                        cell = row1.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = row2.CreateCell(colNo, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row1.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = row2.CreateCell(colNo, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 全家代收
                {
                    rowNo++;
                    ODSRow row1 = sheet.CreateRow(rowNo);
                    rowNo++;
                    ODSRow row2 = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row1.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("全家代收筆數");
                    cell = row2.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("全家代收金額");
                    #endregion

                    #region 各資料(全家代收)欄位
                    decimal rowCount = 0M, rowAmount = 0M;
                    int idx = 0;
                    foreach (CancelResultEntity data in datas)
                    {
                        decimal myCount = data.DCount;
                        decimal myAmount = data.DAmount;
                        rowCount += myCount;
                        rowAmount += myAmount;
                        totalCounts[idx] += myCount;
                        totalAmounts[idx] += myAmount;
                        idx++;

                        colNo++;
                        cell = row1.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = row2.CreateCell(colNo, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row1.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = row2.CreateCell(colNo, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 萊爾富代收
                {
                    rowNo++;
                    ODSRow row1 = sheet.CreateRow(rowNo);
                    rowNo++;
                    ODSRow row2 = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row1.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("萊爾富代收筆數");
                    cell = row2.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("萊爾富代收金額");
                    #endregion

                    #region 各資料(萊爾富代收)欄位
                    decimal rowCount = 0M, rowAmount = 0M;
                    int idx = 0;
                    foreach (CancelResultEntity data in datas)
                    {
                        decimal myCount = data.NCount;
                        decimal myAmount = data.NAmount;
                        rowCount += myCount;
                        rowAmount += myAmount;
                        totalCounts[idx] += myCount;
                        totalAmounts[idx] += myAmount;
                        idx++;

                        colNo++;
                        cell = row1.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = row2.CreateCell(colNo, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row1.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = row2.CreateCell(colNo, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region ＯＫ代收
                {
                    rowNo++;
                    ODSRow row1 = sheet.CreateRow(rowNo);
                    rowNo++;
                    ODSRow row2 = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row1.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("ＯＫ代收筆數");
                    cell = row2.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("ＯＫ代收金額");
                    #endregion

                    #region 各資料(ＯＫ代收)欄位
                    decimal rowCount = 0M, rowAmount = 0M;
                    int idx = 0;
                    foreach (CancelResultEntity data in datas)
                    {
                        decimal myCount = data.JCount;
                        decimal myAmount = data.JAmount;
                        rowCount += myCount;
                        rowAmount += myAmount;
                        totalCounts[idx] += myCount;
                        totalAmounts[idx] += myAmount;
                        idx++;

                        colNo++;
                        cell = row1.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = row2.CreateCell(colNo, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row1.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = row2.CreateCell(colNo, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 中信平台代收
                {
                    rowNo++;
                    ODSRow row1 = sheet.CreateRow(rowNo);
                    rowNo++;
                    ODSRow row2 = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row1.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("中信平台代收筆數");
                    cell = row2.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("中信平台代收金額");
                    #endregion

                    #region 各資料(中信平台代收)欄位
                    decimal rowCount = 0M, rowAmount = 0M;
                    int idx = 0;
                    foreach (CancelResultEntity data in datas)
                    {
                        decimal myCount = data.WCount;
                        decimal myAmount = data.WAmount;
                        rowCount += myCount;
                        rowAmount += myAmount;
                        totalCounts[idx] += myCount;
                        totalAmounts[idx] += myAmount;
                        idx++;

                        colNo++;
                        cell = row1.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = row2.CreateCell(colNo, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row1.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = row2.CreateCell(colNo, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 財金代收
                {
                    rowNo++;
                    ODSRow row1 = sheet.CreateRow(rowNo);
                    rowNo++;
                    ODSRow row2 = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row1.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("財金代收筆數");
                    cell = row2.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("財金代收金額");
                    #endregion

                    #region 各資料(財金代收)欄位
                    decimal rowCount = 0M, rowAmount = 0M;
                    int idx = 0;
                    foreach (CancelResultEntity data in datas)
                    {
                        decimal myCount = data.KCount;
                        decimal myAmount = data.KAmount;
                        rowCount += myCount;
                        rowAmount += myAmount;
                        totalCounts[idx] += myCount;
                        totalAmounts[idx] += myAmount;
                        idx++;

                        colNo++;
                        cell = row1.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = row2.CreateCell(colNo, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row1.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = row2.CreateCell(colNo, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region ＡＴＭ代收
                {
                    rowNo++;
                    ODSRow row1 = sheet.CreateRow(rowNo);
                    rowNo++;
                    ODSRow row2 = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row1.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("ＡＴＭ代收筆數");
                    cell = row2.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("ＡＴＭ代收金額");
                    #endregion

                    #region 各資料(ＡＴＭ代收)欄位
                    decimal rowCount = 0M, rowAmount = 0M;
                    int idx = 0;
                    foreach (CancelResultEntity data in datas)
                    {
                        decimal myCount = data.ACount;
                        decimal myAmount = data.AAmount;
                        rowCount += myCount;
                        rowAmount += myAmount;
                        totalCounts[idx] += myCount;
                        totalAmounts[idx] += myAmount;
                        idx++;

                        colNo++;
                        cell = row1.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = row2.CreateCell(colNo, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row1.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = row2.CreateCell(colNo, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 網路銀行代收
                {
                    rowNo++;
                    ODSRow row1 = sheet.CreateRow(rowNo);
                    rowNo++;
                    ODSRow row2 = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row1.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("網路銀行代收筆數");
                    cell = row2.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("網路銀行代收金額");
                    #endregion

                    #region 各資料(網路銀行代收)欄位
                    decimal rowCount = 0M, rowAmount = 0M;
                    int idx = 0;
                    foreach (CancelResultEntity data in datas)
                    {
                        decimal myCount = data.ICount;
                        decimal myAmount = data.IAmount;
                        rowCount += myCount;
                        rowAmount += myAmount;
                        totalCounts[idx] += myCount;
                        totalAmounts[idx] += myAmount;
                        idx++;

                        colNo++;
                        cell = row1.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = row2.CreateCell(colNo, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row1.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = row2.CreateCell(colNo, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 臨櫃代收
                {
                    rowNo++;
                    ODSRow row1 = sheet.CreateRow(rowNo);
                    rowNo++;
                    ODSRow row2 = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row1.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("臨櫃代收筆數");
                    cell = row2.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("臨櫃代收金額");
                    #endregion

                    #region 各資料(臨櫃代收)欄位
                    decimal rowCount = 0M, rowAmount = 0M;
                    int idx = 0;
                    foreach (CancelResultEntity data in datas)
                    {
                        decimal myCount = data.CmfCount;
                        decimal myAmount = data.CmfAmount;
                        rowCount += myCount;
                        rowAmount += myAmount;
                        totalCounts[idx] += myCount;
                        totalAmounts[idx] += myAmount;
                        idx++;

                        colNo++;
                        cell = row1.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = row2.CreateCell(colNo, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row1.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = row2.CreateCell(colNo, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 匯款代收
                {
                    rowNo++;
                    ODSRow row1 = sheet.CreateRow(rowNo);
                    rowNo++;
                    ODSRow row2 = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row1.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("匯款代收筆數");
                    cell = row2.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("匯款代收金額");
                    #endregion

                    #region 各資料(匯款代收)欄位
                    decimal rowCount = 0M, rowAmount = 0M;
                    int idx = 0;
                    foreach (CancelResultEntity data in datas)
                    {
                        decimal myCount = data.HCount;
                        decimal myAmount = data.HAmount;
                        rowCount += myCount;
                        rowAmount += myAmount;
                        totalCounts[idx] += myCount;
                        totalAmounts[idx] += myAmount;
                        idx++;

                        colNo++;
                        cell = row1.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = row2.CreateCell(colNo, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row1.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = row2.CreateCell(colNo, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 支付寶代收 (C09)
                {
                    rowNo++;
                    ODSRow row1 = sheet.CreateRow(rowNo);
                    rowNo++;
                    ODSRow row2 = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row1.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("支付寶代收筆數");
                    cell = row2.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("支付寶代收金額");
                    #endregion

                    #region 各資料(支付寶代收)欄位
                    decimal rowCount = 0M, rowAmount = 0M;
                    int idx = 0;
                    foreach (CancelResultEntity data in datas)
                    {
                        decimal myCount = data.C09Count;
                        decimal myAmount = data.C09Amount;
                        rowCount += myCount;
                        rowAmount += myAmount;
                        totalCounts[idx] += myCount;
                        totalAmounts[idx] += myAmount;
                        idx++;

                        colNo++;
                        cell = row1.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = row2.CreateCell(colNo, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row1.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = row2.CreateCell(colNo, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 全國繳費網代收 (C08)
                {
                    rowNo++;
                    ODSRow row1 = sheet.CreateRow(rowNo);
                    rowNo++;
                    ODSRow row2 = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row1.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("全國繳費網代收筆數");
                    cell = row2.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("全國繳費網代收金額");
                    #endregion

                    #region 各資料(全國繳費網代收)欄位
                    decimal rowCount = 0M, rowAmount = 0M;
                    int idx = 0;
                    foreach (CancelResultEntity data in datas)
                    {
                        decimal myCount = data.C08Count;
                        decimal myAmount = data.C08Amount;
                        rowCount += myCount;
                        rowAmount += myAmount;
                        totalCounts[idx] += myCount;
                        totalAmounts[idx] += myAmount;
                        idx++;

                        colNo++;
                        cell = row1.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = row2.CreateCell(colNo, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row1.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = row2.CreateCell(colNo, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 台灣Pay代收 (C10)
                {
                    rowNo++;
                    ODSRow row1 = sheet.CreateRow(rowNo);
                    rowNo++;
                    ODSRow row2 = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row1.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("台灣Pay代收筆數");
                    cell = row2.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("台灣Pay代收金額");
                    #endregion

                    #region 各資料(台灣Pay代收 )欄位
                    decimal rowCount = 0M, rowAmount = 0M;
                    int idx = 0;
                    foreach (CancelResultEntity data in datas)
                    {
                        decimal myCount = data.C10Count;
                        decimal myAmount = data.C10Amount;
                        rowCount += myCount;
                        rowAmount += myAmount;
                        totalCounts[idx] += myCount;
                        totalAmounts[idx] += myAmount;
                        idx++;

                        colNo++;
                        cell = row1.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = row2.CreateCell(colNo, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row1.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = row2.CreateCell(colNo, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region [MDY:20191214] (2019擴充案) 國際信用卡
                {
                    rowNo++;
                    ODSRow row1 = sheet.CreateRow(rowNo);
                    rowNo++;
                    ODSRow row2 = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row1.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("國際信用卡代收筆數");
                    cell = row2.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("國際信用卡代收金額");
                    #endregion

                    #region 各資料(國際信用卡代收)欄位
                    decimal rowCount = 0M, rowAmount = 0M;
                    int idx = 0;
                    foreach (CancelResultEntity data in datas)
                    {
                        decimal myCount = data.NCCount;
                        decimal myAmount = data.NCAmount;
                        rowCount += myCount;
                        rowAmount += myAmount;
                        totalCounts[idx] += myCount;
                        totalAmounts[idx] += myAmount;
                        idx++;

                        colNo++;
                        cell = row1.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = row2.CreateCell(colNo, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row1.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = row2.CreateCell(colNo, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 合計
                {
                    rowNo++;
                    ODSRow row1 = sheet.CreateRow(rowNo);
                    rowNo++;
                    ODSRow row2 = sheet.CreateRow(rowNo);

                    #region 橫軸欄位名稱
                    colNo = 1;
                    cell = row1.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("合計筆數");
                    cell = row2.CreateCell(colNo, CellType.String);
                    cell.SetCellValue("合計金額");
                    #endregion

                    #region 各資料(合計)欄位
                    decimal rowCount = 0M, rowAmount = 0M;
                    for (int idx = 0; idx < totalCounts.Count; idx++)
                    {
                        decimal myCount = totalCounts[idx];
                        decimal myAmount = totalAmounts[idx];
                        rowCount += myCount;
                        rowAmount += myAmount;
                        idx++;

                        colNo++;
                        cell = row1.CreateCell(colNo, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = row2.CreateCell(colNo, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colNo++;
                    cell = row1.CreateCell(colNo, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = row2.CreateCell(colNo, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion
                #endregion

                #region 將 ODSWorkbook 轉成 byte[]
                wb.WriteToBytes(out content);
                #endregion

                sheet = null;
                wb = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg;
        }
        #endregion
        #endregion
    }
}
