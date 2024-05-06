using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fuju;

using NPOI;
using NPOI.HPSF;
using NPOI.POIFS;
using NPOI.POIFS.FileSystem;
using NPOI.Util;

using NPOI.XSSF;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;

namespace Entities
{
    #region [Old]
    //public enum XlsCellTypeEnum
    //{
    //    String = 0,
    //    Number = 1
    //}

    //public sealed class XlsCell
    //{
    //    private bool _IsChangeRow = false;
    //    /// <summary>
    //    /// 是否指示換行
    //    /// </summary>
    //    public bool IsChangeRow
    //    {
    //        get
    //        {
    //            return _IsChangeRow;
    //        }
    //        set
    //        {
    //            _IsChangeRow = value;
    //        }
    //    }

    //    /// <summary>
    //    /// 是否忽略此 Cell
    //    /// </summary>
    //    public bool IsSkipCell
    //    {
    //        get
    //        {
    //            return String.IsNullOrEmpty(_DataColumnName) && _FixText == null;
    //        }
    //    }

    //    private string _FixText = null;
    //    /// <summary>
    //    /// 固定文字
    //    /// </summary>
    //    public string FixText
    //    {
    //        get
    //        {
    //            return _FixText;
    //        }
    //        set
    //        {
    //            _FixText = value == null ? null : value;
    //        }
    //    }

    //    private string _DataColumnName = String.Empty;
    //    /// <summary>
    //    /// 資料欄位名稱 (DataTable 的 Column Name)
    //    /// </summary>
    //    public string DataColumnName
    //    {
    //        get
    //        {
    //            return _DataColumnName;
    //        }
    //        set
    //        {
    //            _DataColumnName = value == null ? String.Empty : value.Trim();
    //        }
    //    }

    //    private XlsCellTypeEnum _CellType = XlsCellTypeEnum.String;
    //    /// <summary>
    //    /// Cell 的型別
    //    /// </summary>
    //    public XlsCellTypeEnum CellType
    //    {
    //        get
    //        {
    //            return _CellType;
    //        }
    //        set
    //        {
    //            _CellType = value;
    //        }
    //    }

    //    private int _MergedColumn = 0;
    //    /// <summary>
    //    /// 合併 Column
    //    /// </summary>
    //    public int MergedColumn
    //    {
    //        get
    //        {
    //            return _MergedColumn;
    //        }
    //        set
    //        {
    //            _MergedColumn = value < 0 ? 0 : value;
    //        }
    //    }

    //    private int _MergedRow = 0;
    //    /// <summary>
    //    /// 合併 Row
    //    /// </summary>
    //    public int MergedRow
    //    {
    //        get
    //        {
    //            return _MergedRow;
    //        }
    //        set
    //        {
    //            _MergedRow = value < 0 ? 0 : value;
    //        }
    //    }
    //}
    #endregion

    public class GenReportHelper
    {
        #region
        Type _StringType = typeof(System.String);
        Type _Int32Type = typeof(System.Int32);
        Type _Int64Type = typeof(System.Int64);
        #endregion

        #region 繳費銷帳總表 A2
        /// <summary>
        /// 繳費銷帳總表 A2
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
                #region 使用 HSSFWorkbook 產生 xls
                string sheetName = "sheet1";

                HSSFWorkbook wb = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)wb.CreateSheet(sheetName);

                int columnCount = receiveItems.Count;

                #region 指定紙張大小 A3=8, A4=9, B4=12, Letter=1
                sheet.PrintSetup.PaperSize = 9; //A4
                if (columnCount > 12 && columnCount <= 15)
                {
                    sheet.PrintSetup.PaperSize = 12; //B4
                }
                else if (columnCount > 15)
                {
                    sheet.PrintSetup.PaperSize = 8; //A3
                }
                #endregion

                #region 指定每頁資料筆數
                int pageSize = 16;
                if (sheet.PrintSetup.PaperSize == 12)   //B4
                {
                    pageSize = 21;
                }
                else if (sheet.PrintSetup.PaperSize == 8)   //A3
                {
                    pageSize = 25;
                }
                #endregion

                #region 指定直式或橫式 true=橫式 false=直式
                sheet.PrintSetup.Landscape = true;
                #endregion

                #region 藏隱格線
                sheet.DisplayGridlines = false;
                #endregion

                #region Page Head (粗體、14字、置中)儲存格格式
                HSSFCellStyle pageHeadCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                HSSFFont pageHeadFont = (HSSFFont)wb.CreateFont();
                pageHeadFont.Boldweight = (short)FontBoldWeight.Bold;
                pageHeadFont.FontHeightInPoints = 14;
                pageHeadCellStyle.SetFont(pageHeadFont);
                pageHeadCellStyle.Alignment = HorizontalAlignment.Center;
                #endregion

                #region Column Title (粗體、底線、右靠)儲存格格式
                HSSFCellStyle colTitleCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                HSSFFont colTitleFont = (HSSFFont)wb.CreateFont();
                colTitleFont.Boldweight = (short)FontBoldWeight.Bold;
                colTitleCellStyle.SetFont(colTitleFont);
                colTitleCellStyle.BorderBottom = BorderStyle.Thin;
                colTitleCellStyle.Alignment = HorizontalAlignment.Right;
                #endregion

                #region 金額(千分位逗號、底線、右靠)儲存格格式
                HSSFCellStyle moneyCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                moneyCellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
                moneyCellStyle.BorderBottom = BorderStyle.Thin;
                moneyCellStyle.Alignment = HorizontalAlignment.Right;
                #endregion

                #region 其他 XLS 屬性設定
                //HSSFCellStyle borderStyle = null;
                //HSSFCellStyle colorStyle = null;
                //HSSFCellStyle fontStyle = null;
                //HSSFCellStyle heightStyle = null;
                //HSSFCellStyle spanStyle = null;
                //HSSFCellStyle wrapStyle = null;
                //HSSFFont font = null;

                //borderStyle = workbook.CreateCellStyle();
                //colorStyle = workbook.CreateCellStyle();
                //fontStyle = workbook.CreateCellStyle();
                //heightStyle = workbook.CreateCellStyle();
                //spanStyle = workbook.CreateCellStyle();
                //wrapStyle = workbook.CreateCellStyle();

                ////Style設定    
                //borderStyle.BorderTop = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderLeft = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderBottom = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderRight = HSSFCellStyle.BORDER_THIN;
                //colorStyle.FillForegroundColor = HSSFColor.LIGHT_CORNFLOWER_BLUE.index;
                //colorStyle.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
                //fontStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //fontStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //heightStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //heightStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //spanStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //spanStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //wrapStyle.WrapText = true;

                ////字型大小
                //font = workbook.CreateFont();
                //font.FontHeightInPoints = 14;
                //font.Boldweight = HSSFFont.BOLDWEIGHT_BOLD;
                //fontStyle.SetFont(font);
                //cell = sheet.CreateRow(1).CreateCell(0);
                //cell.CellStyle = fontStyle;
                //cell.SetCellValue("字型大小14粗體");

                ////合併儲存格
                //cell = sheet.CreateRow(2).CreateCell(0);
                //cell.SetCellValue("合併儲存格");
                //cell.CellStyle = spanStyle;
                //sheet.AddMergedRegion(new Region(2, 0, 3, 1));

                ////Wrap
                //cell = sheet.CreateRow(4).CreateCell(0);
                //cell.SetCellValue(string.Format("換行{0}測試", System.Environment.NewLine));
                //cell.CellStyle = wrapStyle;

                ////增加邊框
                //cell = sheet.CreateRow(5).CreateCell(1);
                //cell.SetCellValue("邊框                  ");
                //cell.CellStyle = borderStyle;

                ////背景
                //cell = sheet.CreateRow(6).CreateCell(0);
                //cell.SetCellValue("背景");
                //cell.CellStyle = colorStyle;
                #endregion

                int rowIdx = -1;
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
                        rowIdx = this.GenReportA2Head(sheet, rowIdx, dtHead, pageNo, totalPage, receiveItems, pageHeadCellStyle, colTitleCellStyle);
                    }
                    #endregion

                    #region 查無資料
                    rowIdx++;
                    HSSFRow row = (HSSFRow)sheet.CreateRow(rowIdx);
                    int colIndex = 0;
                    HSSFCell cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("查無資料");
                    #endregion
                }
                else
                {
                    pageNo = 1;

                    #region Gen 表頭
                    {
                        rowIdx = this.GenReportA2Head(sheet, rowIdx, dtHead, pageNo, totalPage, receiveItems, pageHeadCellStyle, colTitleCellStyle);
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
                            rowIdx = this.GenNextPageAndHead(sheet, rowIdx, dtHead, pageNo, totalPage, receiveItems, pageHeadCellStyle, colTitleCellStyle);
                        }

                        #region 系所合計
                        {
                            if (oldMajorId != majorId || oldDeptId != deptId)
                            {
                                pageRowCount++;

                                #region [MDY:20210401] 原碼修正
                                DataRow[] majorSumRows = dtMajorSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), oldDeptId.Replace("'", ""), oldMajorId.Replace("'", "")));
                                #endregion

                                rowIdx = this.GenReportA2MajorSumRow(sheet, rowIdx, majorSumRows, oldDeptName, oldMajorName, receiveItems, moneyCellStyle);

                                oldMajorId = majorId;
                                oldMajorName = majorName;

                                if (pageRowCount >= pageSize)
                                {
                                    pageNo++;
                                    pageRowCount = 0;
                                    rowIdx = this.GenNextPageAndHead(sheet, rowIdx, dtHead, pageNo, totalPage, receiveItems, pageHeadCellStyle, colTitleCellStyle);
                                }
                            }
                        }
                        #endregion

                        #region 部別合計
                        {
                            if (oldDeptId != deptId)
                            {
                                pageRowCount++;

                                #region [MDY:20210401] 原碼修正
                                DataRow[] deptSumRows = dtDeptSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), oldDeptId.Replace("'", "")));
                                #endregion

                                rowIdx = this.GenReportA2DeptSumRow(sheet, rowIdx, deptSumRows, oldDeptName, receiveItems, moneyCellStyle);

                                oldDeptId = deptId;
                                oldDeptName = deptName;

                                if (pageRowCount >= pageSize)
                                {
                                    pageNo++;
                                    pageRowCount = 0;
                                    rowIdx = this.GenNextPageAndHead(sheet, rowIdx, dtHead, pageNo, totalPage, receiveItems, pageHeadCellStyle, colTitleCellStyle);
                                }
                            }
                        }
                        #endregion

                        #region 班別合計
                        {
                            pageRowCount++;
                            HSSFRow row = null;
                            HSSFCell cell = null;

                            #region 班別合計 Head Row (部別名稱 + 系所名稱 + 班別名稱)
                            {
                                rowIdx++;
                                row = (HSSFRow)sheet.CreateRow(rowIdx);

                                int colIndex = 0;
                                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                cell.SetCellValue(deptName);
                                sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));

                                colIndex += 3;
                                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                cell.SetCellValue(majorName);
                                sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));

                                colIndex += 3;
                                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                cell.SetCellValue(className);
                                sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));
                            }
                            #endregion

                            #region 班別合計 Data Row
                            {
                                rowIdx++;
                                row = (HSSFRow)sheet.CreateRow(rowIdx);

                                int colIndex = 0;
                                foreach (KeyValue<string> receiveItem in receiveItems)
                                {
                                    string columnName = receiveItem.Value;
                                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
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
                                    colIndex++;
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
                        rowIdx = this.GenNextPageAndHead(sheet, rowIdx, dtHead, pageNo, totalPage, receiveItems, pageHeadCellStyle, colTitleCellStyle);
                    }

                    #region 補最後的系所合計
                    {
                        pageRowCount++;

                        #region [MDY:20210401] 原碼修正
                        DataRow[] majorSumRows = dtMajorSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), oldDeptId.Replace("'", ""), oldMajorId.Replace("'", "")));
                        #endregion

                        rowIdx = this.GenReportA2MajorSumRow(sheet, rowIdx, majorSumRows, oldDeptName, oldMajorName, receiveItems, moneyCellStyle);

                        if (pageRowCount >= pageSize)
                        {
                            pageNo++;
                            pageRowCount = 0;
                            rowIdx = this.GenNextPageAndHead(sheet, rowIdx, dtHead, pageNo, totalPage, receiveItems, pageHeadCellStyle, colTitleCellStyle);
                        }
                    }
                    #endregion

                    #region 補最後的部別合計
                    {
                        pageRowCount++;

                        #region [MDY:20210401] 原碼修正
                        DataRow[] deptSumRows = dtDeptSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), oldDeptId.Replace("'", "")));
                        #endregion

                        rowIdx = this.GenReportA2DeptSumRow(sheet, rowIdx, deptSumRows, oldDeptName, receiveItems, moneyCellStyle);
                    }
                    #endregion
                }
                #endregion

                #region 自動調整欄寬
                for (int colIndex = 0; colIndex < receiveItems.Count; colIndex++)
                {
                    string txt = receiveItems[colIndex].Key;
                    if (txt.Length > 4)
                    {
                        //sheet.SetColumnWidth(colIndex, txt.Length * 20 + 10);
                        sheet.AutoSizeColumn(colIndex);
                    }
                }
                #endregion

                #region 將 HSSFWorkbook 轉成 byte[]
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.Write(ms);
                    ms.Flush();
                    content = ms.ToArray();
                }
                sheet = null;
                wb = null;
                #endregion
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
        /// <param name="rowIndex"></param>
        /// <param name="dtHead"></param>
        /// <param name="pageNo"></param>
        /// <param name="totalPage"></param>
        /// <param name="receiveItems"></param>
        /// <param name="pageHeadCellStyle"></param>
        /// <param name="colTitleCellStyle"></param>
        /// <returns></returns>
        private int GenNextPageAndHead(HSSFSheet sheet, int rowIndex, DataTable dtHead, int pageNo, int totalPage, KeyValueList<string> receiveItems, HSSFCellStyle pageHeadCellStyle, HSSFCellStyle colTitleCellStyle)
        {
            int rowIdx = rowIndex;

            #region 空白行+插入分頁
            {
                rowIdx++;
                sheet.CreateRow(rowIdx);
                sheet.SetRowBreak(rowIdx);
            }
            #endregion

            #region Gen 表頭
            {
                rowIdx = this.GenReportA2Head(sheet, rowIdx, dtHead, pageNo, totalPage, receiveItems, pageHeadCellStyle, colTitleCellStyle);
            }
            #endregion

            return rowIdx;
        }

        /// <summary>
        /// 產生繳費銷帳總表 A2 的表頭
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
        /// <param name="dtHead"></param>
        /// <param name="pageNo"></param>
        /// <param name="totalPage"></param>
        /// <param name="receiveItems"></param>
        /// <param name="pageHeadCellStyle"></param>
        /// <param name="colTitleCellStyle"></param>
        /// <returns>傳回最後的 Row Index</returns>
        private int GenReportA2Head(HSSFSheet sheet, int rowIndex, DataTable dtHead, int pageNo, int totalPage, KeyValueList<string> receiveItems, HSSFCellStyle pageHeadCellStyle, HSSFCellStyle colTitleCellStyle)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;
            DataRow dRow = dtHead.Rows[0];

            int columnCount = receiveItems.Count;

            #region Head Row 0
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);

                int colIndex = 0;
                string schName = dRow["Sch_Name"].ToString();
                string reportName = dRow["ReportName"].ToString();
                string value = String.Format("{0}          {1}", schName, reportName);
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
                cell.CellStyle = pageHeadCellStyle;
                sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + columnCount - 1));
            }
            #endregion

            #region Head Row 1
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);

                #region 學年
                {
                    int colIndex = 0;
                    string value = String.Format("學年：{0}", dRow["Year_Name"].ToString());
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));
                }
                #endregion

                #region 學期
                {
                    int colIndex = 4;
                    string value = String.Format("學期：{0}", dRow["Term_Name"].ToString());
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));
                }
                #endregion

                #region 繳費狀態
                {
                    int colIndex = 8;
                    string value = String.Format("繳費狀態：{0}", dRow["ReceiveStatusName"].ToString());
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));
                }
                #endregion
            }
            #endregion

            #region Head Row 2
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);

                #region 商家代號
                {
                    int colIndex = 0;
                    string value = String.Format("商家代號：{0}", dRow["Receive_Type"].ToString());
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));
                }
                #endregion

                #region 代收費用
                {
                    int colIndex = 4;
                    string value = String.Format("代收費用：{0}", dRow["Receive_Name"].ToString());
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));
                }
                #endregion

                #region 批號
                {
                    int colIndex = 8;
                    string value = String.Format("批號：{0}", dRow["UpNo"].ToString());
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));
                }
                #endregion

                #region 日期
                {
                    int colIndex = columnCount - 2;
                    string value = dRow["ReportDate"].ToString();
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);
                }
                #endregion

                #region 頁數
                {
                    int colIndex = columnCount - 1;
                    string value = String.Format("第{0}頁/共{1}頁", pageNo, totalPage);
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);
                }
                #endregion
            }
            #endregion

            #region 收入科目名稱 Row
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);
                int colIndex = 0;
                foreach (KeyValue<string> receiveItem in receiveItems)
                {
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(receiveItem.Key);
                    cell.CellStyle = colTitleCellStyle;
                    colIndex++;
                }
            }
            #endregion

            return rowIdx;
        }

        /// <summary>
        /// 產生繳費銷帳總表 A2 的系所合計
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
        /// <param name="majorSumRows"></param>
        /// <param name="deptName"></param>
        /// <param name="majorName"></param>
        /// <param name="receiveItems"></param>
        /// <param name="moneyCellStyle"></param>
        /// <returns>傳回最後的 Row Index</returns>
        private int GenReportA2MajorSumRow(HSSFSheet sheet, int rowIndex, DataRow[] majorSumRows, string deptName, string majorName, KeyValueList<string> receiveItems, HSSFCellStyle moneyCellStyle)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            if (majorSumRows != null && majorSumRows.Length > 0)
            {
                //應該只有一筆，多筆表示 dtMajorSum 資料可能有錯，這裡用 foreach 是為了容易發現錯誤
                foreach (DataRow majorSumRow in majorSumRows)
                {
                    #region 系所合計 Head Row (部別名稱 + 系所名稱)
                    {
                        rowIdx++;
                        row = (HSSFRow)sheet.CreateRow(rowIdx);

                        int colIndex = 0;
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                        cell.SetCellValue(deptName);
                        sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));

                        colIndex += 3;
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                        cell.SetCellValue(majorName);
                        sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));

                        colIndex += 3;
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                        cell.SetCellValue("(系所合計)");
                        sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));
                    }
                    #endregion

                    #region 系所合計 Data Row
                    {
                        rowIdx++;
                        row = (HSSFRow)sheet.CreateRow(rowIdx);

                        int colIndex = 0;
                        foreach (KeyValue<string> receiveItem in receiveItems)
                        {
                            string columnName = receiveItem.Value;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
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
                            colIndex++;
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
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(deptName);
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));

                    colIndex += 3;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(majorName);
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));

                    colIndex += 3;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("(系所合計)");
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));
                }
                #endregion

                #region 系所合計 Data Row
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("查無資料");
                }
                #endregion
            }
            return rowIdx;
        }

        /// <summary>
        /// 產生繳費銷帳總表 A2 的部別合計
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
        /// <param name="deptSumRows"></param>
        /// <param name="deptName"></param>
        /// <param name="receiveItems"></param>
        /// <param name="moneyCellStyle"></param>
        /// <returns>傳回最後的 Row Index</returns>
        private int GenReportA2DeptSumRow(HSSFSheet sheet, int rowIndex, DataRow[] deptSumRows, string deptName, KeyValueList<string> receiveItems, HSSFCellStyle moneyCellStyle)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            if (deptSumRows != null && deptSumRows.Length > 0)
            {
                //應該只有一筆，多筆表示 dtMajorSum 資料可能有錯，這裡用 foreach 是為了容易發現錯誤
                foreach (DataRow deptSumRow in deptSumRows)
                {
                    #region 部別合計 Head Row (部別名稱)
                    {
                        rowIdx++;
                        row = (HSSFRow)sheet.CreateRow(rowIdx);

                        int colIndex = 0;
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                        cell.SetCellValue(deptName);
                        sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));

                        colIndex += 3;
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                        cell.SetCellValue("(部別合計)");
                        sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));
                    }
                    #endregion

                    #region 部別合計 Data Row
                    {
                        rowIdx++;
                        row = (HSSFRow)sheet.CreateRow(rowIdx);

                        int colIndex = 0;
                        foreach (KeyValue<string> receiveItem in receiveItems)
                        {
                            string columnName = receiveItem.Value;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
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
                            colIndex++;
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
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(deptName);
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));

                    colIndex += 3;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("(部別合計)");
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));
                }
                #endregion

                #region 部別合計 Data Row
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("查無資料");
                }
                #endregion
            }
            return rowIdx;
        }
        #endregion

        #region 繳費銷帳總表
        /// <summary>
        /// 繳費銷帳總表
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
                #region 使用 HSSFWorkbook 產生 xls
                string sheetName = "sheet1";

                HSSFWorkbook wb = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)wb.CreateSheet(sheetName);

                #region 指定紙張大小 A3=8, A4=9, Letter=1
                sheet.PrintSetup.PaperSize = 9;
                #endregion

                #region 指定直式或橫式 true=橫式 false=直式
                sheet.PrintSetup.Landscape = true;
                #endregion

                #region 藏隱格線
                sheet.DisplayGridlines = false;
                #endregion

                #region 其他 XLS 屬性設定
                //HSSFCellStyle borderStyle = null;
                //HSSFCellStyle colorStyle = null;
                //HSSFCellStyle fontStyle = null;
                //HSSFCellStyle heightStyle = null;
                //HSSFCellStyle spanStyle = null;
                //HSSFCellStyle wrapStyle = null;
                //HSSFFont font = null;

                //borderStyle = workbook.CreateCellStyle();
                //colorStyle = workbook.CreateCellStyle();
                //fontStyle = workbook.CreateCellStyle();
                //heightStyle = workbook.CreateCellStyle();
                //spanStyle = workbook.CreateCellStyle();
                //wrapStyle = workbook.CreateCellStyle();

                ////Style設定    
                //borderStyle.BorderTop = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderLeft = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderBottom = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderRight = HSSFCellStyle.BORDER_THIN;
                //colorStyle.FillForegroundColor = HSSFColor.LIGHT_CORNFLOWER_BLUE.index;
                //colorStyle.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
                //fontStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //fontStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //heightStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //heightStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //spanStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //spanStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //wrapStyle.WrapText = true;

                ////字型大小
                //font = workbook.CreateFont();
                //font.FontHeightInPoints = 14;
                //font.Boldweight = HSSFFont.BOLDWEIGHT_BOLD;
                //fontStyle.SetFont(font);
                //cell = sheet.CreateRow(1).CreateCell(0);
                //cell.CellStyle = fontStyle;
                //cell.SetCellValue("字型大小14粗體");

                ////合併儲存格
                //cell = sheet.CreateRow(2).CreateCell(0);
                //cell.SetCellValue("合併儲存格");
                //cell.CellStyle = spanStyle;
                //sheet.AddMergedRegion(new Region(2, 0, 3, 1));

                ////Wrap
                //cell = sheet.CreateRow(4).CreateCell(0);
                //cell.SetCellValue(string.Format("換行{0}測試", System.Environment.NewLine));
                //cell.CellStyle = wrapStyle;

                ////增加邊框
                //cell = sheet.CreateRow(5).CreateCell(1);
                //cell.SetCellValue("邊框                  ");
                //cell.CellStyle = borderStyle;

                ////背景
                //cell = sheet.CreateRow(6).CreateCell(0);
                //cell.SetCellValue("背景");
                //cell.CellStyle = colorStyle;
                #endregion

                int rowIdx = -1;
                int pageNo = 0;
                int totalPage = dtPage.Rows.Count;

                if (totalPage == 0)
                {
                    #region Gen 表頭
                    {
                        rowIdx = this.GenReportAHead(sheet, rowIdx, dtHead, pageNo, totalPage);
                    }
                    #endregion

                    #region 查無資料
                    rowIdx++;
                    HSSFRow row = (HSSFRow)sheet.CreateRow(rowIdx);
                    int colIndex = 0;
                    HSSFCell cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
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
                            rowIdx = this.GenReportAHead(sheet, rowIdx, dtHead, pageNo, totalPage);
                        }
                        #endregion

                        #region Gen 表身 + 小計
                        {
                            rowIdx = this.GenReportAData(sheet, rowIdx, dtDeptSum, dtMajorSum, dtClassSum, receiveItems, dtData, receiveType, yearId, termId, depId, receiveId, deptId);
                        }
                        #endregion

                        #region 空白行+插入分頁
                        rowIdx++;
                        sheet.CreateRow(rowIdx);
                        sheet.SetRowBreak(rowIdx);
                        #endregion
                    }
                }
                #endregion

                #region 將 HSSFWorkbook 轉成 byte[]
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.Write(ms);
                    ms.Flush();
                    content = ms.ToArray();
                }
                sheet = null;
                wb = null;
                #endregion
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
        /// <param name="rowIndex"></param>
        /// <param name="dtHead"></param>
        /// <param name="pageNo"></param>
        /// <param name="totalPage"></param>
        /// <returns>傳回最後的 Row Index</returns>
        private int GenReportAHead(HSSFSheet sheet, int rowIndex, DataTable dtHead, int pageNo, int totalPage)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            #region Head Row 0
            rowIdx++;
            row = (HSSFRow)sheet.CreateRow(rowIdx);
            DataRow dRow = dtHead.Rows[0];

            #region 學校名稱
            {
                int colIndex = 0;
                string value = dRow["Sch_Name"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 報表名稱
            {
                int colIndex = 3;
                string value = dRow["ReportName"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion
            #endregion

            #region Head Row 1
            rowIdx++;
            row = (HSSFRow)sheet.CreateRow(rowIdx);

            #region 學年
            {
                int colIndex = 0;
                string value = "學年：";
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);

                colIndex = 1;
                value = dRow["Year_Name"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 學期
            {
                int colIndex = 4;
                string value = "學期：";
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);

                colIndex = 5;
                value = dRow["Term_Name"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 繳費狀態
            {
                string receiveStatusName = dRow["ReceiveStatusName"].ToString();
                if (!String.IsNullOrEmpty(receiveStatusName))
                {
                    int colIndex = 8;
                    string value = "繳費狀態：";
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);

                    colIndex = 9;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(receiveStatusName);
                }
            }
            #endregion
            #endregion

            #region Head Row 2
            rowIdx++;
            row = (HSSFRow)sheet.CreateRow(rowIdx);

            #region 商家代號
            {
                int colIndex = 0;
                string value = "商家代號：";
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);

                colIndex = 1;
                value = dRow["Receive_Type"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 代收費用
            {
                int colIndex = 4;
                string value = "代收費用：";
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);

                colIndex = 5;
                value = dRow["Receive_Name"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 批號
            {
                string upNo = dRow["UpNo"].ToString();
                if (!String.IsNullOrEmpty(upNo))
                {
                    int colIndex = 8;
                    string value = "批號：";
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);

                    colIndex = 9;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(upNo);
                }
            }
            #endregion

            #region 日期
            {
                int colIndex = 11;
                string value = dRow["ReportDate"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 頁數
            {
                int colIndex = 12;
                string value = String.Format("第{0}頁/共{1}頁", pageNo, totalPage);
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion
            #endregion

            return rowIdx;
        }

        /// <summary>
        /// 產生表身 + 小計
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
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
        private int GenReportAData(HSSFSheet sheet, int rowIndex
            , DataTable dtDeptSum, DataTable dtMajorSum, DataTable dtClassSum, KeyValueList<string> receiveItems, DataTable dtData
            , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            #region [MDY:20210401] 原碼修正
            DataRow[] deptRows = dtDeptSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", "")));
            #endregion

            if (deptRows != null && deptRows.Length > 0)
            {
                DataRow deptRow = deptRows[0];  //只會有一筆
                string deptName = deptRow["Dept_Name"].ToString();

                #region [MDY:20210401] 原碼修正
                DataRow[] majorRows = dtMajorSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", "")), "Major_Id");
                #endregion

                if (majorRows != null && majorRows.Length > 0)
                {
                    foreach (DataRow majorRow in majorRows)
                    {
                        string majorId = majorRow["Major_Id"].ToString();
                        string majorName = majorRow["Major_Name"].ToString();

                        #region 班別
                        #region [MDY:20210401] 原碼修正
                        DataRow[] classRows = dtClassSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", ""), majorId.Replace("'", "")), "Class_Id");
                        #endregion

                        if (classRows != null && classRows.Length > 0)
                        {
                            foreach (DataRow classRow in classRows)
                            {
                                string classId = classRow["Class_Id"].ToString();
                                string className = classRow["Class_Name"].ToString();

                                rowIdx++;
                                rowIdx = this.GenReportAForClassData(sheet, rowIdx, deptName, majorName, className, receiveItems, dtData, receiveType, yearId, termId, depId, receiveId, deptId, majorId, classId);

                                #region 班別合計
                                {
                                    rowIdx++;
                                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                                    #region 班別人數
                                    int colIndex = 0;
                                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                    cell.SetCellValue("班別人數：");

                                    int sumCount = Int32.Parse(classRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                                    colIndex++;
                                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                    cell.SetCellValue(sumCount);
                                    #endregion

                                    #region 班別金額
                                    colIndex = 3;
                                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                    cell.SetCellValue("班別金額：");

                                    double sumAmount = Double.Parse(classRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                                    colIndex++;
                                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                    cell.SetCellValue(sumAmount);
                                    #endregion
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            rowIdx = this.GenReportAForMajorData(sheet, rowIdx, deptName, majorName, receiveItems, dtData, receiveType, yearId, termId, depId, receiveId, deptId, majorId);
                        }
                        #endregion

                        #region 系所合計
                        {
                            rowIdx++;
                            row = (HSSFRow)sheet.CreateRow(rowIdx);

                            #region 系所人數
                            int colIndex = 0;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                            cell.SetCellValue("系所人數：");

                            int sumCount = Int32.Parse(majorRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                            colIndex++;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                            cell.SetCellValue(sumCount);
                            #endregion

                            #region 系所金額
                            colIndex = 3;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                            cell.SetCellValue("系所金額：");

                            double sumAmount = Double.Parse(majorRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                            colIndex++;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                            cell.SetCellValue(sumAmount);
                            #endregion
                        }
                        #endregion
                    }
                }
                else
                {
                    rowIdx = this.GenReportAForDeptData(sheet, rowIdx, deptName, receiveItems, dtData, receiveType, yearId, termId, depId, receiveId, deptId);
                }

                #region 部別合計
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 部別人數
                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("部別人數：");

                    int sumCount = Int32.Parse(deptRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                    colIndex++;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                    cell.SetCellValue(sumCount);
                    #endregion

                    #region 部別金額
                    colIndex = 3;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("部別金額：");

                    double sumAmount = Double.Parse(deptRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                    colIndex++;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                    cell.SetCellValue(sumAmount);
                    #endregion
                }
                #endregion
            }
            else
            {
                #region 部別合計
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 部別人數
                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("部別人數：");

                    int sumCount = 0;
                    colIndex++;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                    cell.SetCellValue(sumCount);
                    #endregion

                    #region 部別金額
                    colIndex = 3;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("部別金額：");

                    double sumAmount = 0;
                    colIndex++;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                    cell.SetCellValue(sumAmount);
                    #endregion
                }
                #endregion
            }
            return rowIdx;
        }

        private int GenReportAForClassData(HSSFSheet sheet, int rowIndex
            , string deptName, string majorName, string className
            , KeyValueList<string> receiveItems, DataTable dtData
            , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId, string majorId, string classId)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            #region Data Row 0
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);

                #region 部別名稱
                {
                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(deptName);
                }
                #endregion

                #region 系所名稱
                {
                    int colIndex = 2;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(majorName);
                }
                #endregion

                #region 班別名稱
                {
                    int colIndex = 4;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(className);
                }
                #endregion
            }
            #endregion

            #region Data Row 1
            rowIdx++;
            row = (HSSFRow)sheet.CreateRow(rowIdx);
            #region 收入科目名稱
            {
                int colIndex = 0;
                foreach (KeyValue<string> receiveItem in receiveItems)
                {
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(receiveItem.Key);
                    colIndex++;
                }
            }
            #endregion
            #endregion

            #region Data Row 2 ~ dRows.Count + 1

            #region [MDY:20210401] 原碼修正
            DataRow[] dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Class_Id='{7}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", ""), majorId.Replace("'", ""), classId.Replace("'", "")), "Stu_Id");
            #endregion

            if (dRows != null && dRows.Length > 0)
            {
                foreach (DataRow dRow in dRows)
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);
                    int colIndex = 0;
                    foreach (KeyValue<string> receiveItem in receiveItems)
                    {
                        string columnName = receiveItem.Value;
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                        if (!dRow.IsNull(columnName))
                        {
                            double value = Double.Parse(dRow[columnName].ToString());
                            cell.SetCellValue(value);
                        }
                        colIndex++;
                    }
                }
            }
            #endregion

            return rowIdx;
        }

        private int GenReportAForMajorData(HSSFSheet sheet, int rowIndex
            , string deptName, string majorName
            , KeyValueList<string> receiveItems, DataTable dtData
            , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId, string majorId)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            #region Data Row 0
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);

                #region 部別名稱
                {
                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(deptName);
                }
                #endregion

                #region 系所名稱
                {
                    int colIndex = 2;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(majorName);
                }
                #endregion
             }
            #endregion

            #region Data Row 1
            rowIdx++;
            row = (HSSFRow)sheet.CreateRow(rowIdx);
            #region 收入科目名稱
            {
                int colIndex = 0;
                foreach (KeyValue<string> receiveItem in receiveItems)
                {
                    //colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                    cell.SetCellValue(receiveItem.Key);
                    colIndex++;
                }
            }
            #endregion
            #endregion

            #region Data Row 2 ~ dRows.Count + 1

            #region [MDY:20210401] 原碼修正
            DataRow[] dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", ""), majorId.Replace("'", "")), "Stu_Id");
            #endregion

            if (dRows != null && dRows.Length > 0)
            {
                foreach (DataRow dRow in dRows)
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);
                    int colIndex = 0;
                    foreach (KeyValue<string> receiveItem in receiveItems)
                    {
                        string columnName = receiveItem.Value;
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                        if (!dRow.IsNull(columnName))
                        {
                            double value = Double.Parse(dRow[columnName].ToString());
                            cell.SetCellValue(value);
                        }
                        colIndex++;
                    }
                }
            }
            #endregion

            return rowIdx;
        }

        private int GenReportAForDeptData(HSSFSheet sheet, int rowIndex
            , string deptName
            , KeyValueList<string> receiveItems, DataTable dtData
            , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            #region Data Row 0
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);

                #region 部別名稱
                {
                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(deptName);
                }
                #endregion
            }
            #endregion

            #region Data Row 1
            rowIdx++;
            row = (HSSFRow)sheet.CreateRow(rowIdx);
            #region 收入科目名稱
            {
                int colIndex = 0;
                foreach (KeyValue<string> receiveItem in receiveItems)
                {
                    //colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(receiveItem.Key);
                    colIndex++;
                }
            }
            #endregion
            #endregion

            #region Data Row 2 ~ dRows.Count + 1

            #region [MDY:20210401] 原碼修正
            DataRow[] dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", "")), "Stu_Id");
            #endregion

            if (dRows != null && dRows.Length > 0)
            {
                foreach (DataRow dRow in dRows)
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);
                    int colIndex = 0;
                    foreach (KeyValue<string> receiveItem in receiveItems)
                    {
                        string columnName = receiveItem.Value;
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                        if (!dRow.IsNull(columnName))
                        {
                            double value = Double.Parse(dRow[columnName].ToString());
                            cell.SetCellValue(value);
                        }
                        colIndex++;
                    }
                }
            }
            #endregion

            return rowIdx;
        }
        #endregion

        #region 繳費銷帳明細表
        /// <summary>
        /// 繳費銷帳明細表
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
                #region 使用 HSSFWorkbook 產生 xls
                string sheetName = "sheet1";

                HSSFWorkbook wb = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)wb.CreateSheet(sheetName);
                HSSFRow row = null;
                HSSFCell cell = null;

                #region 指定紙張大小 A3=8, A4=9, Letter=1
                sheet.PrintSetup.PaperSize = 9;
                #endregion

                #region 指定直式或橫式 true=橫式 false=直式
                sheet.PrintSetup.Landscape = true;
                #endregion

                #region 藏隱格線
                sheet.DisplayGridlines = false;
                #endregion

                #region 其他 XLS 屬性設定
                //HSSFCellStyle borderStyle = null;
                //HSSFCellStyle colorStyle = null;
                //HSSFCellStyle fontStyle = null;
                //HSSFCellStyle heightStyle = null;
                //HSSFCellStyle spanStyle = null;
                //HSSFCellStyle wrapStyle = null;
                //HSSFFont font = null;

                //borderStyle = workbook.CreateCellStyle();
                //colorStyle = workbook.CreateCellStyle();
                //fontStyle = workbook.CreateCellStyle();
                //heightStyle = workbook.CreateCellStyle();
                //spanStyle = workbook.CreateCellStyle();
                //wrapStyle = workbook.CreateCellStyle();

                ////Style設定    
                //borderStyle.BorderTop = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderLeft = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderBottom = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderRight = HSSFCellStyle.BORDER_THIN;
                //colorStyle.FillForegroundColor = HSSFColor.LIGHT_CORNFLOWER_BLUE.index;
                //colorStyle.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
                //fontStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //fontStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //heightStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //heightStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //spanStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //spanStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //wrapStyle.WrapText = true;

                ////字型大小
                //font = workbook.CreateFont();
                //font.FontHeightInPoints = 14;
                //font.Boldweight = HSSFFont.BOLDWEIGHT_BOLD;
                //fontStyle.SetFont(font);
                //cell = sheet.CreateRow(1).CreateCell(0);
                //cell.CellStyle = fontStyle;
                //cell.SetCellValue("字型大小14粗體");

                ////合併儲存格
                //cell = sheet.CreateRow(2).CreateCell(0);
                //cell.SetCellValue("合併儲存格");
                //cell.CellStyle = spanStyle;
                //sheet.AddMergedRegion(new Region(2, 0, 3, 1));

                ////Wrap
                //cell = sheet.CreateRow(4).CreateCell(0);
                //cell.SetCellValue(string.Format("換行{0}測試", System.Environment.NewLine));
                //cell.CellStyle = wrapStyle;

                ////增加邊框
                //cell = sheet.CreateRow(5).CreateCell(1);
                //cell.SetCellValue("邊框                  ");
                //cell.CellStyle = borderStyle;

                ////背景
                //cell = sheet.CreateRow(6).CreateCell(0);
                //cell.SetCellValue("背景");
                //cell.CellStyle = colorStyle;
                #endregion

                int rowIdx = -1;
                int pageNo = 0;
                int totalPage = dtPage.Rows.Count;

                if (totalPage == 0)
                {
                    #region Gen 表頭
                    {
                        rowIdx = this.GenReportBHead(sheet, rowIdx, dtHead, pageNo, totalPage);
                    }
                    #endregion

                    #region 查無資料
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);
                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
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
                            rowIdx = this.GenReportBHead(sheet, rowIdx, dtHead, pageNo, totalPage);
                        }
                        #endregion

                        #region Gen 表身
                        {
                            rowIdx++;
                            rowIdx = this.GenReportBForClassData(sheet, rowIdx, deptName, majorName, className, receiveItems, dtData, receiveType, yearId, termId, depId, receiveId, deptId, majorId, classId);
                        }
                        #endregion

                        #region 班級合計
                        {
                            int sumCount = 0;
                            double sumAmount = 0;

                            #region [MDY:20210401] 原碼修正
                            DataRow[] classRows = dtClassSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Class_Id='{7}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", ""), majorId.Replace("'", ""), classId.Replace("'", "")));
                            #endregion

                            if (classRows != null && classRows.Length > 0)
                            {
                                DataRow classRow = classRows[0];
                                sumCount = Int32.Parse(classRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                                sumAmount = Double.Parse(classRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                            }

                            rowIdx++;
                            row = (HSSFRow)sheet.CreateRow(rowIdx);

                            #region 班別人數
                            int colIndex = 0;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                            cell.SetCellValue("班別人數：");

                            colIndex++;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                            cell.SetCellValue(sumCount);
                            #endregion

                            #region 班別金額
                            colIndex = 3;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                            cell.SetCellValue("班別金額：");

                            colIndex++;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
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

                            #region [MDY:20210401] 原碼修正
                            DataRow[] majorRows = dtMajorSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", ""), majorId.Replace("'", "")));
                            #endregion

                            if (majorRows != null && majorRows.Length > 0)
                            {
                                DataRow majorRow = majorRows[0];
                                sumCount = Int32.Parse(majorRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                                sumAmount = Double.Parse(majorRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                            }

                            rowIdx++;
                            row = (HSSFRow)sheet.CreateRow(rowIdx);

                            #region 系所人數
                            int colIndex = 0;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                            cell.SetCellValue("系所人數：");

                            colIndex++;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                            cell.SetCellValue(sumCount);
                            #endregion

                            #region 系所金額
                            colIndex = 3;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                            cell.SetCellValue("系所金額：");

                            colIndex++;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                            cell.SetCellValue(sumAmount);
                            #endregion
                        }
                        #endregion

                        #region 部別合計
                        if (deptKey != nextDeptKey)
                        {
                            int sumCount = 0;
                            double sumAmount = 0;

                            #region [MDY:20210401] 原碼修正
                            DataRow[] deptRows = dtDeptSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", "")));
                            #endregion

                            if (deptRows != null && deptRows.Length > 0)
                            {
                                DataRow deptRow = deptRows[0];
                                sumCount = Int32.Parse(deptRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                                sumAmount = Double.Parse(deptRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                            }
                            rowIdx++;
                            row = (HSSFRow)sheet.CreateRow(rowIdx);

                            #region 部別人數
                            int colIndex = 0;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                            cell.SetCellValue("部別人數：");

                            colIndex++;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                            cell.SetCellValue(sumCount);
                            #endregion

                            #region 部別金額
                            colIndex = 3;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                            cell.SetCellValue("部別金額：");

                            colIndex++;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                            cell.SetCellValue(sumAmount);
                            #endregion
                        }
                        #endregion

                        #region 空白行+插入分頁
                        rowIdx++;
                        sheet.CreateRow(rowIdx);
                        sheet.SetRowBreak(rowIdx);
                        #endregion
                    }
                }
                #endregion

                #region 將 HSSFWorkbook 轉成 byte[]
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.Write(ms);
                    ms.Flush();
                    content = ms.ToArray();
                }
                sheet = null;
                wb = null;
                #endregion
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
        /// <param name="rowIndex"></param>
        /// <param name="dtHead"></param>
        /// <param name="pageNo"></param>
        /// <param name="totalPage"></param>
        /// <returns>傳回最後的 Row Index</returns>
        private int GenReportBHead(HSSFSheet sheet, int rowIndex, DataTable dtHead, int pageNo, int totalPage)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            #region Head Row 0
            rowIdx++;
            row = (HSSFRow)sheet.CreateRow(rowIdx);
            DataRow dRow = dtHead.Rows[0];

            #region 學校名稱
            {
                int colIndex = 0;
                string value = dRow["Sch_Name"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 報表名稱
            {
                int colIndex = 3;
                string value = dRow["ReportName"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion
            #endregion

            #region Head Row 1
            rowIdx++;
            row = (HSSFRow)sheet.CreateRow(rowIdx);

            #region 學年
            {
                int colIndex = 0;
                string value = "學年：";
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);

                colIndex = 1;
                value = dRow["Year_Name"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 學期
            {
                int colIndex = 4;
                string value = "學期：";
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);

                colIndex = 5;
                value = dRow["Term_Name"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 繳費狀態
            {
                string receiveStatusName = dRow["ReceiveStatusName"].ToString();
                if (!String.IsNullOrEmpty(receiveStatusName))
                {
                    int colIndex = 8;
                    string value = "繳費狀態：";
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);

                    colIndex = 9;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(receiveStatusName);
                }
            }
            #endregion
            #endregion

            #region Head Row 2
            rowIdx++;
            row = (HSSFRow)sheet.CreateRow(rowIdx);

            #region 商家代號
            {
                int colIndex = 0;
                string value = "商家代號：";
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);

                colIndex = 1;
                value = dRow["Receive_Type"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 代收費用
            {
                int colIndex = 4;
                string value = "代收費用：";
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);

                colIndex = 5;
                value = dRow["Receive_Name"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 批號
            {
                string upNo = dRow["UpNo"].ToString();
                if (!String.IsNullOrEmpty(upNo))
                {
                    int colIndex = 8;
                    string value = "批號：";
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);

                    colIndex = 9;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(upNo);
                }
            }
            #endregion

            #region 日期
            {
                int colIndex = 11;
                string value = dRow["ReportDate"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 頁數
            {
                int colIndex = 12;
                string value = String.Format("第{0}頁/共{1}頁", pageNo, totalPage);
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion
            #endregion

            return rowIdx;
        }

        /// <summary>
        /// 產生表身
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
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
        private int GenReportBForClassData(HSSFSheet sheet, int rowIndex
            , string deptName, string majorName, string className
            , KeyValueList<string> receiveItems, DataTable dtData
            , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId, string majorId, string classId)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            #region Data Row 0
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);

                #region 部別名稱
                {
                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(deptName);
                }
                #endregion

                #region 系所名稱
                {
                    int colIndex = 2;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(majorName);
                }
                #endregion

                #region 班別名稱
                {
                    int colIndex = 4;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(className);
                }
                #endregion
            }
            #endregion

            #region Data Row 1
            rowIdx++;
            row = (HSSFRow)sheet.CreateRow(rowIdx);
            #region 收入科目名稱
            {
                int colIndex = 0;
                foreach (KeyValue<string> receiveItem in receiveItems)
                {
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(receiveItem.Key);
                    colIndex++;
                }
            }
            #endregion
            #endregion

            #region Data Row 2 ~ dRows.Count + 1

            #region [MDY:20210401] 原碼修正
            DataRow[] dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Class_Id='{7}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", ""), majorId.Replace("'", ""), classId.Replace("'", "")), "Stu_Id");
            #endregion

            if (dRows != null && dRows.Length > 0)
            {
                foreach (DataRow dRow in dRows)
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);
                    int colIndex = 0;
                    foreach (KeyValue<string> receiveItem in receiveItems)
                    {
                        string columnName = receiveItem.Value;
                        if (!dRow.IsNull(columnName))
                        {
                            //[MDY:20170702] 排除以 Receive_ 開頭但非數值的欄位
                            if (columnName.StartsWith("Receive_") && dtData.Columns[columnName].DataType.FullName == "System.Decimal")
                            {
                                cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                double value = Double.Parse(dRow[columnName].ToString());
                                cell.SetCellValue(value);
                            }
                            else
                            {
                                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                string value = dRow[columnName].ToString();
                                cell.SetCellValue(value);
                            }
                        }
                        colIndex++;
                    }
                }
            }
            #endregion

            return rowIdx;
        }

        #region [Old]
        //private int GenReportBData(HSSFSheet sheet, int rowIndex
        //    , DataTable dtDeptSum, DataTable dtMajorSum, DataTable dtClassSum, KeyValueList<string> receiveItems, DataTable dtData
        //    , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId, string majorId, string classId)
        //{
        //    HSSFRow row = null;
        //    HSSFCell cell = null;
        //    int rowIdx = rowIndex;

        //    DataRow[] deptRows = dtDeptSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType, yearId, termId, depId, receiveId, deptId));
        //    if (deptRows != null && deptRows.Length > 0)
        //    {
        //        DataRow deptRow = deptRows[0];  //只會有一筆
        //        string deptName = deptRow["Dept_Name"].ToString();

        //        DataRow[] majorRows = dtMajorSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType, yearId, termId, depId, receiveId, deptId), "Major_Id");
        //        if (majorRows != null && majorRows.Length > 0)
        //        {
        //            foreach (DataRow majorRow in majorRows)
        //            {
        //                string majorId = majorRow["Major_Id"].ToString();
        //                string majorName = majorRow["Major_Name"].ToString();

        //                #region 班別
        //                DataRow[] classRows = dtClassSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType, yearId, termId, depId, receiveId, deptId, majorId), "Class_Id");
        //                if (classRows != null && classRows.Length > 0)
        //                {
        //                    foreach (DataRow classRow in classRows)
        //                    {
        //                        string classId = classRow["Class_Id"].ToString();
        //                        string className = classRow["Class_Name"].ToString();

        //                        rowIdx++;
        //                        rowIdx = this.GenReportBForClassData(sheet, rowIdx, deptName, majorName, className, receiveItems, dtData, receiveType, yearId, termId, depId, receiveId, deptId, majorId, classId);

        //                        #region 班別合計
        //                        {
        //                            rowIdx++;
        //                            row = sheet.CreateRow(rowIdx);

        //                            #region 班別人數
        //                            int colIndex = 0;
        //                            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_STRING);
        //                            cell.SetCellValue("班別人數：");

        //                            int sumCount = Int32.Parse(classRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
        //                            colIndex++;
        //                            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_NUMERIC);
        //                            cell.SetCellValue(sumCount);
        //                            #endregion

        //                            #region 班別金額
        //                            colIndex = 3;
        //                            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_STRING);
        //                            cell.SetCellValue("班別金額：");

        //                            double sumAmount = Double.Parse(classRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
        //                            colIndex++;
        //                            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_NUMERIC);
        //                            cell.SetCellValue(sumAmount);
        //                            #endregion
        //                        }
        //                        #endregion
        //                    }
        //                }
        //                else
        //                {
        //                    rowIdx = this.GenReportBForMajorData(sheet, rowIdx, deptName, majorName, receiveItems, dtData, receiveType, yearId, termId, depId, receiveId, deptId, majorId);
        //                }
        //                #endregion

        //                #region 系所合計
        //                {
        //                    rowIdx++;
        //                    row = sheet.CreateRow(rowIdx);

        //                    #region 系所人數
        //                    int colIndex = 0;
        //                    cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_STRING);
        //                    cell.SetCellValue("系所人數：");

        //                    int sumCount = Int32.Parse(majorRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
        //                    colIndex++;
        //                    cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_NUMERIC);
        //                    cell.SetCellValue(sumCount);
        //                    #endregion

        //                    #region 系所金額
        //                    colIndex = 3;
        //                    cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_STRING);
        //                    cell.SetCellValue("系所金額：");

        //                    double sumAmount = Double.Parse(majorRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
        //                    colIndex++;
        //                    cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_NUMERIC);
        //                    cell.SetCellValue(sumAmount);
        //                    #endregion
        //                }
        //                #endregion
        //            }
        //        }
        //        else
        //        {
        //            rowIdx = this.GenReportBForDeptData(sheet, rowIdx, deptName, receiveItems, dtData, receiveType, yearId, termId, depId, receiveId, deptId);
        //        }

        //        #region 部別合計
        //        {
        //            rowIdx++;
        //            row = sheet.CreateRow(rowIdx);

        //            #region 部別人數
        //            int colIndex = 0;
        //            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_STRING);
        //            cell.SetCellValue("部別人數：");

        //            int sumCount = Int32.Parse(deptRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
        //            colIndex++;
        //            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_NUMERIC);
        //            cell.SetCellValue(sumCount);
        //            #endregion

        //            #region 部別金額
        //            colIndex = 3;
        //            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_STRING);
        //            cell.SetCellValue("部別金額：");

        //            double sumAmount = Double.Parse(deptRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
        //            colIndex++;
        //            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_NUMERIC);
        //            cell.SetCellValue(sumAmount);
        //            #endregion
        //        }
        //        #endregion
        //    }
        //    else
        //    {
        //        #region 部別合計
        //        {
        //            rowIdx++;
        //            row = sheet.CreateRow(rowIdx);

        //            #region 部別人數
        //            int colIndex = 0;
        //            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_STRING);
        //            cell.SetCellValue("部別人數：");

        //            int sumCount = 0;
        //            colIndex++;
        //            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_NUMERIC);
        //            cell.SetCellValue(sumCount);
        //            #endregion

        //            #region 部別金額
        //            colIndex = 3;
        //            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_STRING);
        //            cell.SetCellValue("部別金額：");

        //            double sumAmount = 0;
        //            colIndex++;
        //            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_NUMERIC);
        //            cell.SetCellValue(sumAmount);
        //            #endregion
        //        }
        //        #endregion
        //    }
        //    return rowIdx;
        //}

        //private int GenReportBForMajorData(HSSFSheet sheet, int rowIndex
        //    , string deptName, string majorName
        //    , KeyValueList<string> receiveItems, DataTable dtData
        //    , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId, string majorId)
        //{
        //    HSSFRow row = null;
        //    HSSFCell cell = null;
        //    int rowIdx = rowIndex;

        //    #region Data Row 0
        //    {
        //        rowIdx++;
        //        row = sheet.CreateRow(rowIdx);

        //        #region 部別名稱
        //        {
        //            int colIndex = 0;
        //            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_STRING);
        //            cell.SetCellValue(deptName);
        //        }
        //        #endregion

        //        #region 系所名稱
        //        {
        //            int colIndex = 2;
        //            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_STRING);
        //            cell.SetCellValue(majorName);
        //        }
        //        #endregion
        //    }
        //    #endregion

        //    #region Data Row 1
        //    rowIdx++;
        //    row = sheet.CreateRow(rowIdx);
        //    #region 收入科目名稱
        //    {
        //        int colIndex = 0;
        //        foreach (KeyValue<string> receiveItem in receiveItems)
        //        {
        //            colIndex = 0;
        //            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_STRING);
        //            cell.SetCellValue(receiveItem.Key);
        //            colIndex++;
        //        }
        //    }
        //    #endregion
        //    #endregion

        //    #region Data Row 2 ~ dRows.Count + 1
        //    DataRow[] dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType, yearId, termId, depId, receiveId, deptId, majorId), "Stu_Id");
        //    if (dRows != null && dRows.Length > 0)
        //    {
        //        foreach (DataRow dRow in dRows)
        //        {
        //            rowIdx++;
        //            row = sheet.CreateRow(rowIdx);
        //            int colIndex = 0;
        //            foreach (KeyValue<string> receiveItem in receiveItems)
        //            {
        //                string columnName = receiveItem.Value;
        //                cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_NUMERIC);
        //                if (!dRow.IsNull(columnName))
        //                {
        //                    double value = Double.Parse(dRow[columnName].ToString());
        //                    cell.SetCellValue(value);
        //                }
        //                colIndex++;
        //            }
        //        }
        //    }
        //    #endregion

        //    return rowIdx;
        //}

        //private int GenReportBForDeptData(HSSFSheet sheet, int rowIndex
        //    , string deptName
        //    , KeyValueList<string> receiveItems, DataTable dtData
        //    , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId)
        //{
        //    HSSFRow row = null;
        //    HSSFCell cell = null;
        //    int rowIdx = rowIndex;

        //    #region Data Row 0
        //    {
        //        rowIdx++;
        //        row = sheet.CreateRow(rowIdx);

        //        #region 部別名稱
        //        {
        //            int colIndex = 0;
        //            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_STRING);
        //            cell.SetCellValue(deptName);
        //        }
        //        #endregion
        //    }
        //    #endregion

        //    #region Data Row 1
        //    rowIdx++;
        //    row = sheet.CreateRow(rowIdx);
        //    #region 收入科目名稱
        //    {
        //        int colIndex = 0;
        //        foreach (KeyValue<string> receiveItem in receiveItems)
        //        {
        //            colIndex = 0;
        //            cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_STRING);
        //            cell.SetCellValue(receiveItem.Key);
        //            colIndex++;
        //        }
        //    }
        //    #endregion
        //    #endregion

        //    #region Data Row 2 ~ dRows.Count + 1
        //    DataRow[] dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType, yearId, termId, depId, receiveId, deptId), "Stu_Id");
        //    if (dRows != null && dRows.Length > 0)
        //    {
        //        foreach (DataRow dRow in dRows)
        //        {
        //            rowIdx++;
        //            row = sheet.CreateRow(rowIdx);
        //            int colIndex = 0;
        //            foreach (KeyValue<string> receiveItem in receiveItems)
        //            {
        //                string columnName = receiveItem.Value;
        //                cell = row.CreateCell(colIndex, HSSFCell.CELL_TYPE_NUMERIC);
        //                if (!dRow.IsNull(columnName))
        //                {
        //                    double value = Double.Parse(dRow[columnName].ToString());
        //                    cell.SetCellValue(value);
        //                }
        //                colIndex++;
        //            }
        //        }
        //    }
        //    #endregion

        //    return rowIdx;
        //}
        #endregion
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
                #region 使用 HSSFWorkbook 產生 xls
                string sheetName = "sheet1";

                HSSFWorkbook wb = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)wb.CreateSheet(sheetName);
                HSSFRow row = null;
                HSSFCell cell = null;

                #region 指定紙張大小 A3=8, A4=9, Letter=1
                sheet.PrintSetup.PaperSize = 9;
                #endregion

                #region 指定直式或橫式 true=橫式 false=直式
                sheet.PrintSetup.Landscape = true;
                #endregion

                #region 藏隱格線
                sheet.DisplayGridlines = false;
                #endregion

                #region Page Head (粗體、14字、置中)儲存格格式
                HSSFCellStyle pageHeadCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                HSSFFont pageHeadFont = (HSSFFont)wb.CreateFont();
                pageHeadFont.Boldweight = (short)FontBoldWeight.Bold;
                pageHeadFont.FontHeightInPoints = 14;
                pageHeadCellStyle.SetFont(pageHeadFont);
                pageHeadCellStyle.Alignment = HorizontalAlignment.Center;
                #endregion

                #region Column Title (粗體、底線)儲存格格式
                HSSFCellStyle colTitleCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                HSSFFont colTitleFont = (HSSFFont)wb.CreateFont();
                colTitleFont.Boldweight = (short)FontBoldWeight.Bold;
                colTitleCellStyle.SetFont(colTitleFont);
                colTitleCellStyle.BorderBottom = BorderStyle.Thin;
                #endregion

                #region SumColumn Title (右靠)儲存格格式
                HSSFCellStyle sumColTitleCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                sumColTitleCellStyle.Alignment = HorizontalAlignment.Right;
                #endregion

                #region 金額(千分位逗號、右靠)儲存格格式
                HSSFCellStyle moneyCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                moneyCellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
                moneyCellStyle.Alignment = HorizontalAlignment.Right;
                #endregion

                #region 其他 XLS 屬性設定
                //HSSFCellStyle borderStyle = null;
                //HSSFCellStyle colorStyle = null;
                //HSSFCellStyle fontStyle = null;
                //HSSFCellStyle heightStyle = null;
                //HSSFCellStyle spanStyle = null;
                //HSSFCellStyle wrapStyle = null;
                //HSSFFont font = null;

                //borderStyle = workbook.CreateCellStyle();
                //colorStyle = workbook.CreateCellStyle();
                //fontStyle = workbook.CreateCellStyle();
                //heightStyle = workbook.CreateCellStyle();
                //spanStyle = workbook.CreateCellStyle();
                //wrapStyle = workbook.CreateCellStyle();

                ////Style設定    
                //borderStyle.BorderTop = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderLeft = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderBottom = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderRight = HSSFCellStyle.BORDER_THIN;
                //colorStyle.FillForegroundColor = HSSFColor.LIGHT_CORNFLOWER_BLUE.index;
                //colorStyle.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
                //fontStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //fontStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //heightStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //heightStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //spanStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //spanStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //wrapStyle.WrapText = true;

                ////字型大小
                //font = workbook.CreateFont();
                //font.FontHeightInPoints = 14;
                //font.Boldweight = HSSFFont.BOLDWEIGHT_BOLD;
                //fontStyle.SetFont(font);
                //cell = sheet.CreateRow(1).CreateCell(0);
                //cell.CellStyle = fontStyle;
                //cell.SetCellValue("字型大小14粗體");

                ////合併儲存格
                //cell = sheet.CreateRow(2).CreateCell(0);
                //cell.SetCellValue("合併儲存格");
                //cell.CellStyle = spanStyle;
                //sheet.AddMergedRegion(new Region(2, 0, 3, 1));

                ////Wrap
                //cell = sheet.CreateRow(4).CreateCell(0);
                //cell.SetCellValue(string.Format("換行{0}測試", System.Environment.NewLine));
                //cell.CellStyle = wrapStyle;

                ////增加邊框
                //cell = sheet.CreateRow(5).CreateCell(1);
                //cell.SetCellValue("邊框                  ");
                //cell.CellStyle = borderStyle;

                ////背景
                //cell = sheet.CreateRow(6).CreateCell(0);
                //cell.SetCellValue("背景");
                //cell.CellStyle = colorStyle;
                #endregion

                int rowIdx = -1;
                int pageNo = 0;
                int totalPage = dtPage.Rows.Count;

                if (totalPage == 0)
                {
                    #region Gen 表頭
                    {
                        rowIdx = this.GenReportCHead(sheet, rowIdx, dtHead, pageNo, totalPage, pageHeadCellStyle);
                    }
                    #endregion

                    #region 查無資料
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);
                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
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
                            rowIdx = this.GenReportCHead(sheet, rowIdx, dtHead, pageNo, totalPage, pageHeadCellStyle);
                        }
                        #endregion

                        #region Gen 表身 (含小計)
                        {
                            int deptCount = 0;
                            double deptAmount = 0D;
                            rowIdx = this.GenReportCData(sheet, rowIdx, dtDeptSum, dtMajorSum, dtGradeSum, dtClassSum, dtData, otherFields, receiveType, yearId, termId, depId, receiveId, deptId, colTitleCellStyle, moneyCellStyle, sumColTitleCellStyle, out deptCount, out deptAmount);
                            totalCount += deptCount;
                            totalAmount += deptAmount;
                        }
                        #endregion

                        #region 空白行+插入分頁
                        if (pageNo < totalPage)
                        {
                            rowIdx++;
                            sheet.CreateRow(rowIdx);
                            sheet.SetRowBreak(rowIdx);
                        }
                        #endregion
                    }

                    #region 總合計
                    {
                        rowIdx++;
                        row = (HSSFRow)sheet.CreateRow(rowIdx);

                        #region 總合計人數
                        int colIndex = 2;
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                        cell.SetCellValue("總合計人數：");
                        cell.CellStyle = sumColTitleCellStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));

                        colIndex += 2;
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                        cell.SetCellValue(totalCount);
                        #endregion

                        #region 總合計金額
                        colIndex = 5;
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                        cell.SetCellValue("總合計金額：");
                        cell.CellStyle = sumColTitleCellStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));

                        colIndex += 2;
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                        cell.SetCellValue(totalAmount);
                        cell.CellStyle = moneyCellStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));
                        #endregion
                    }
                    #endregion
                }
                #endregion

                #region 將 HSSFWorkbook 轉成 byte[]
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.Write(ms);
                    ms.Flush();
                    content = ms.ToArray();
                }
                sheet = null;
                wb = null;
                #endregion
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
        private int GenReportCHead(HSSFSheet sheet, int rowIndex, DataTable dtHead, int pageNo, int totalPage, HSSFCellStyle pageHeadCellStyle)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            DataRow dRow = dtHead.Rows[0];
            int columnCount = 15;

            #region Head Row 0
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);

                int colIndex = 0;
                string schName = dRow["Sch_Name"].ToString();
                string reportName = dRow["ReportName"].ToString();
                string value = String.Format("{0}          {1}", schName, reportName);
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
                cell.CellStyle = pageHeadCellStyle;
                sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + columnCount - 1));
            }
            #endregion

            #region Head Row 1
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);

                #region 學年
                {
                    int colIndex = 0;
                    string value = String.Format("學年：{0}", dRow["Year_Name"].ToString());
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));
                }
                #endregion

                #region 學期
                {
                    int colIndex = 4;
                    string value = String.Format("學期：{0}", dRow["Term_Name"].ToString());
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));
                }
                #endregion

                #region 繳費狀態
                {
                    int colIndex = 8;
                    string value = String.Format("繳費狀態：{0}", dRow["ReceiveStatusName"].ToString());
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));
                }
                #endregion
            }
            #endregion

            #region Head Row 2
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);

                #region 商家代號
                {
                    int colIndex = 0;
                    string value = String.Format("商家代號：{0}", dRow["Receive_Type"].ToString());
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));
                }
                #endregion

                #region 代收費用
                {
                    int colIndex = 4;
                    string value = String.Format("代收費用：{0}", dRow["Receive_Name"].ToString());
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 2));
                }
                #endregion

                #region 批號
                {
                    int colIndex = 8;
                    string value = String.Format("批號：{0}", dRow["UpNo"].ToString());
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));
                }
                #endregion

                #region 日期
                {
                    int colIndex = columnCount - 2;
                    string value = dRow["ReportDate"].ToString();
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);
                }
                #endregion

                #region 頁數
                {
                    int colIndex = columnCount - 1;
                    string value = String.Format("第{0}頁/共{1}頁", pageNo, totalPage);
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
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
        /// <param name="rowIndex"></param>
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
        private int GenReportCData(HSSFSheet sheet, int rowIndex
            , DataTable dtDeptSum, DataTable dtMajorSum, DataTable dtGradeSum, DataTable dtClassSum, DataTable dtData, KeyValueList<string> otherFields
            , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId
            , HSSFCellStyle colTitleCellStyle, HSSFCellStyle moneyCellStyle, HSSFCellStyle sumColTitleCellStyle
            , out int deptCount, out double deptAmount)
        {
            deptCount = 0;
            deptAmount = 0D;

            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;
            bool isHasGrade = (dtGradeSum != null && dtGradeSum.Rows.Count > 0);

            #region [MDY:20210401] 原碼修正
            DataRow[] deptRows = dtDeptSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", "")));
            #endregion

            if (deptRows != null && deptRows.Length > 0)
            {
                DataRow deptRow = deptRows[0];  //只會有一筆
                string deptName = deptRow["Dept_Name"].ToString();

                #region [MDY:20210401] 原碼修正
                DataRow[] majorRows = dtMajorSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", "")), "Major_Id");
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

                            #region [MDY:20210401] 原碼修正
                            DataRow[] gradeRows = dtGradeSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' ", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", ""), majorId.Replace("'", "")), "Stu_Grade");
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
                                        #region [MDY:20210401] 原碼修正
                                        DataRow[] classRows = dtClassSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Stu_Grade='{7}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", ""), majorId.Replace("'", ""), stuGrade.Replace("'", "")), "Class_Id");
                                        #endregion

                                        if (classRows != null && classRows.Length > 0)
                                        {
                                            foreach (DataRow classRow in classRows)
                                            {
                                                string classId = classRow["Class_Id"].ToString();
                                                string className = classRow["Class_Name"].ToString();

                                                rowIdx++;
                                                rowIdx = this.GenReportCForClassData(sheet, rowIdx, deptName, majorName, gradeName, className, dtData, otherFields, receiveType, yearId, termId, depId, receiveId, deptId, majorId, stuGrade, classId, colTitleCellStyle, moneyCellStyle);

                                                #region 班別合計
                                                {
                                                    rowIdx++;
                                                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                                                    #region 班級人數
                                                    int colIndex = 2;
                                                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                                    cell.SetCellValue("班級人數：");
                                                    cell.CellStyle = sumColTitleCellStyle;
                                                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));

                                                    int sumCount = Int32.Parse(classRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                                                    colIndex += 2;
                                                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                                    cell.SetCellValue(sumCount);
                                                    #endregion

                                                    #region 班級金額
                                                    colIndex = 5;
                                                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                                    cell.SetCellValue("班級金額：");
                                                    cell.CellStyle = sumColTitleCellStyle;
                                                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));

                                                    double sumAmount = Double.Parse(classRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                                                    colIndex += 2;
                                                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                                    cell.SetCellValue(sumAmount);
                                                    cell.CellStyle = moneyCellStyle;
                                                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));
                                                    #endregion
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                    #endregion

                                    #region 年級合計
                                    {
                                        rowIdx++;
                                        row = (HSSFRow)sheet.CreateRow(rowIdx);

                                        #region 年級人數
                                        int colIndex = 2;
                                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                        cell.SetCellValue("年級人數：");
                                        cell.CellStyle = sumColTitleCellStyle;
                                        sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));

                                        int sumCount = Int32.Parse(gradeRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                                        colIndex += 2;
                                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                        cell.SetCellValue(sumCount);
                                        #endregion

                                        #region 年級金額
                                        colIndex = 5;
                                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                        cell.SetCellValue("年級金額：");
                                        cell.CellStyle = sumColTitleCellStyle;
                                        sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));

                                        double sumAmount = Double.Parse(gradeRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                                        colIndex += 2;
                                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                        cell.SetCellValue(sumAmount);
                                        cell.CellStyle = moneyCellStyle;
                                        sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));
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
                                #region [MDY:20210401] 原碼修正
                                DataRow[] classRows = dtClassSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", ""), majorId.Replace("'", "")), "Class_Id");
                                #endregion

                                if (classRows != null && classRows.Length > 0)
                                {
                                    foreach (DataRow classRow in classRows)
                                    {
                                        string classId = classRow["Class_Id"].ToString();
                                        string className = classRow["Class_Name"].ToString();

                                        rowIdx++;
                                        rowIdx = this.GenReportCForClassData(sheet, rowIdx, deptName, majorName, gradeName, className, dtData, otherFields, receiveType, yearId, termId, depId, receiveId, deptId, majorId, stuGrade, classId, colTitleCellStyle, moneyCellStyle);

                                        #region 班別合計
                                        {
                                            rowIdx++;
                                            row = (HSSFRow)sheet.CreateRow(rowIdx);

                                            #region 班級人數
                                            int colIndex = 2;
                                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                            cell.SetCellValue("班級人數：");
                                            cell.CellStyle = sumColTitleCellStyle;
                                            sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));

                                            int sumCount = Int32.Parse(classRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                                            colIndex += 2;
                                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                            cell.SetCellValue(sumCount);
                                            #endregion

                                            #region 班級金額
                                            colIndex = 5;
                                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                            cell.SetCellValue("班級金額：");
                                            cell.CellStyle = sumColTitleCellStyle;
                                            sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));

                                            double sumAmount = Double.Parse(classRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                                            colIndex += 2;
                                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                            cell.SetCellValue(sumAmount);
                                            cell.CellStyle = moneyCellStyle;
                                            sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));
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
                            rowIdx++;
                            row = (HSSFRow)sheet.CreateRow(rowIdx);

                            #region 系所人數
                            int colIndex = 2;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                            cell.SetCellValue("系所人數：");
                            cell.CellStyle = sumColTitleCellStyle;
                            sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));

                            int sumCount = Int32.Parse(majorRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                            colIndex += 2;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                            cell.SetCellValue(sumCount);
                            #endregion

                            #region 系所金額
                            colIndex = 5;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                            cell.SetCellValue("系所金額：");
                            cell.CellStyle = sumColTitleCellStyle;
                            sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));

                            double sumAmount = Double.Parse(majorRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                            colIndex += 2;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                            cell.SetCellValue(sumAmount);
                            cell.CellStyle = moneyCellStyle;
                            sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));
                            #endregion
                        }
                        #endregion
                    }
                }

                #region 部別合計
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 部別人數
                    int colIndex = 2;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("部別人數：");
                    cell.CellStyle = sumColTitleCellStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));

                    int sumCount = Int32.Parse(deptRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
                    colIndex += 2;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                    cell.SetCellValue(sumCount);

                    deptCount = sumCount;
                    #endregion

                    #region 部別金額
                    colIndex = 5;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("部別金額：");
                    cell.CellStyle = sumColTitleCellStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));

                    double sumAmount = Double.Parse(deptRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
                    colIndex += 2;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                    cell.SetCellValue(sumAmount);
                    cell.CellStyle = moneyCellStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));

                    deptAmount = sumAmount;
                    #endregion
                }
                #endregion
            }
            else
            {
                #region 部別合計
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 部別人數
                    int colIndex = 2;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("部別人數：");
                    cell.CellStyle = sumColTitleCellStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));

                    int sumCount = 0;
                    colIndex += 2;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                    cell.SetCellValue(sumCount);
                    #endregion

                    #region 部別金額
                    colIndex = 5;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("部別金額：");
                    cell.CellStyle = sumColTitleCellStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));

                    double sumAmount = 0;
                    colIndex += 2;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                    cell.SetCellValue(sumAmount);
                    cell.CellStyle = moneyCellStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));
                    #endregion
                }
                #endregion
            }

            return rowIdx;
        }

        /// <summary>
        /// 產生 學生繳費名冊 表身 的班別資料
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
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
        private int GenReportCForClassData(HSSFSheet sheet, int rowIndex
            , string deptName, string majorName, string gradeName, string className
            , DataTable dtData, KeyValueList<string> otherFields
            , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId, string majorId, string stuGrade, string classId
            , HSSFCellStyle colTitleCellStyle, HSSFCellStyle moneyCellStyle)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            #region Data Row 部別、系所、年級、班別 名稱 (0 ~ 2/3)
            {
                #region 部別名稱
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(deptName);
                    cell.CellStyle = colTitleCellStyle;
                    //sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 3));
                    this.CreatEmptyColumnCellAndMerge(cell, 3);
                }
                #endregion

                #region 系所名稱
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                    int colIndex = 1;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(majorName);
                    cell.CellStyle = colTitleCellStyle;
                    //sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 3));
                    this.CreatEmptyColumnCellAndMerge(cell, 3);
                }
                #endregion

                #region 年級名稱
                //有年級參數表示群組明細程度為部別、系所、年級、班別，才要顯示年級名稱
                if (stuGrade != null)
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                    int colIndex = 2;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("年級：" + gradeName);
                    cell.CellStyle = colTitleCellStyle;
                    //sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 3));
                    this.CreatEmptyColumnCellAndMerge(cell, 3);
                }
                #endregion

                #region 班別名稱
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                    int colIndex = 3;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("班級：" + className);
                    cell.CellStyle = colTitleCellStyle;
                    //sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 11));
                    this.CreatEmptyColumnCellAndMerge(cell, 11);
                }
                #endregion
            }
            #endregion

            #region Data Row 欄位名稱 (3/4)
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);

                #region 學號
                {
                    int colIndex = 3;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("學號");
                    cell.CellStyle = colTitleCellStyle;
                    //sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));
                    this.CreatEmptyColumnCellAndMerge(cell, 1);
                }
                #endregion

                #region 姓名
                {
                    int colIndex = 5;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("姓名");
                    cell.CellStyle = colTitleCellStyle;
                    //sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));
                    this.CreatEmptyColumnCellAndMerge(cell, 1);
                }
                #endregion

                #region 金額
                {
                    int colIndex = 7;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("金額");
                    cell.CellStyle = colTitleCellStyle;
                    //sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));
                    this.CreatEmptyColumnCellAndMerge(cell, 1);
                }
                #endregion

                #region 說明
                {
                    int colIndex = 9;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("說明");
                    cell.CellStyle = colTitleCellStyle;
                    //sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 5));
                    this.CreatEmptyColumnCellAndMerge(cell, 5);
                }
                #endregion
            }
            #endregion

            #region Data Row 資料 (4/5 ~ 4/5 + data Count)
            {
                DataRow[] dRows = null;

                #region [MDY:20210401] 原碼修正
                if (stuGrade != null)
                {
                    //有年級參數表示群組明細程度為部別、系所、年級、班別
                    dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Stu_Grade='{7}' AND Class_Id='{8}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", ""), majorId.Replace("'", ""), stuGrade.Replace("'", ""), classId.Replace("'", "")), "Stu_Id");
                }
                else
                {
                    dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Class_Id='{7}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", ""), majorId.Replace("'", ""), classId.Replace("'", "")), "Stu_Id");
                }
                #endregion

                if (dRows != null && dRows.Length > 0)
                {
                    foreach (DataRow dRow in dRows)
                    {
                        rowIdx++;
                        row = (HSSFRow)sheet.CreateRow(rowIdx);

                        #region 學號
                        {
                            int colIndex = 3;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                            string stuId = dRow["Stu_Id"].ToString();
                            cell.SetCellValue(stuId);
                            sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));
                        }
                        #endregion

                        #region 姓名
                        {
                            int colIndex = 5;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                            string stuName = dRow["Stu_Name"].ToString();
                            cell.SetCellValue(stuName);
                            sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));
                        }
                        #endregion

                        #region 金額
                        {
                            int colIndex = 7;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
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
                            sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 1));
                        }
                        #endregion

                        #region 說明
                        {
                            int colIndex = 9;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
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
                            sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + 5));
                        }
                        #endregion
                    }
                }
            }
            #endregion

            return rowIdx;
        }

        /// <summary>
        /// 在指定的 Cell 後面，建立指定數量的空白格 並 Merge 到指定的 Cell
        /// </summary>
        /// <param name="cell">指定的 Cell</param>
        /// <param name="columnCount">指定空白格數量</param>
        private void CreatEmptyColumnCellAndMerge(HSSFCell cell, int columnCount)
        {
            if (cell != null && columnCount > 0)
            {
                IRow row = cell.Row;
                for (int no = 1; no <= columnCount; no++)
                {
                    int colIndex = cell.ColumnIndex + no;
                    HSSFCell cell2 = (HSSFCell)row.CreateCell(colIndex, cell.CellType);
                    cell2.SetCellValue("");
                    cell2.CellStyle = cell.CellStyle;
                }
                cell.Sheet.AddMergedRegion(new CellRangeAddress(cell.RowIndex, cell.RowIndex, cell.ColumnIndex, cell.ColumnIndex + columnCount));
            }
        }
        #endregion

        #region 匯出手續費統計報表
        /// <summary>
        /// 取得空的匯出手續費統計報表 DataTable
        /// </summary>
        /// <param name="channelIds"></param>
        /// <returns></returns>
        public DataTable GetEmptyReportDTable(out string[] channelIds)
        {
            #region [MDY:20191214] (2019擴充案) 國際信用卡 - 增加 NC-國際信用卡
            #region [OLD]
            //#region [MDY:20171127] 增加全國繳費網 (C08)、台灣Pay (C10) (20170831_01)
            //#region [Old]
            ////#region [MDY:20170506] 增加支付寶 & 修正中信管道常數為 CTCB
            ////#region [Old]
            //////channelIds = new string[] {
            //////    ChannelHelper.TABS, ChannelHelper.RM, ChannelHelper.ATM, ChannelHelper.EDI, ChannelHelper.NB
            //////    , ChannelHelper.VO, ChannelHelper.FISC, ChannelHelper.CTCD, ChannelHelper.SM_DEFAULT
            //////};
            ////#endregion

            ////channelIds = new string[] {
            ////    ChannelHelper.TABS, ChannelHelper.RM, ChannelHelper.ATM, ChannelHelper.EDI, ChannelHelper.NB,
            ////    ChannelHelper.VO, ChannelHelper.FISC, ChannelHelper.CTCB, ChannelHelper.SM_DEFAULT, ChannelHelper.ALIPAY
            ////};
            ////#endregion
            //#endregion

            //channelIds = new string[] {
            //    ChannelHelper.TABS, ChannelHelper.RM, ChannelHelper.ATM, ChannelHelper.EDI, ChannelHelper.NB,
            //    ChannelHelper.VO, ChannelHelper.FISC, ChannelHelper.CTCB, ChannelHelper.SM_DEFAULT, ChannelHelper.ALIPAY,
            //    ChannelHelper.EBILL, ChannelHelper.TWPAY
            //};
            //#endregion
            #endregion

            channelIds = new string[] {
                ChannelHelper.TABS, ChannelHelper.RM, ChannelHelper.ATM, ChannelHelper.EDI, ChannelHelper.NB,
                ChannelHelper.VO, ChannelHelper.FISC, ChannelHelper.CTCB, ChannelHelper.SM_DEFAULT, ChannelHelper.ALIPAY,
                ChannelHelper.EBILL, ChannelHelper.TWPAY, ChannelHelper.FISC_NC
            };
            #endregion

            DataTable dt = new DataTable();
            DataColumn col = null;

            col = new DataColumn("Bank_Id", _StringType);
            col.Caption = "分行代碼";
            dt.Columns.Add(col);

            col = new DataColumn("Bank_Name", _StringType);
            col.Caption = "分行名稱";
            dt.Columns.Add(col);

            col = new DataColumn("Sch_Identy", _StringType);
            col.Caption = "學校代碼";
            dt.Columns.Add(col);

            col = new DataColumn("Sch_Name", _StringType);
            col.Caption = "學校名稱";
            dt.Columns.Add(col);

            col = new DataColumn("Total_Records", _Int32Type);
            col.Caption = "代收總筆數";
            dt.Columns.Add(col);

            col = new DataColumn("Total_Amount", typeof(System.Int64));
            col.Caption = "代收總金額";
            dt.Columns.Add(col);

            col = new DataColumn("Total_Fee", _Int32Type);
            col.Caption = "總手續費";
            dt.Columns.Add(col);

            foreach (string channelId in channelIds)
            {
                string channelName = ChannelHelper.GetChannelName(channelId);
                col = new DataColumn(String.Format("{0}_Fee", channelId), _Int32Type);
                col.Caption = String.Format("{0}手續費", channelName);
                dt.Columns.Add(col);
            }

            return dt;
        }

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
                #region 使用 HSSFWorkbook 產生 xls
                string sheetName = "sheet1";

                HSSFWorkbook wb = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)wb.CreateSheet(sheetName);
                HSSFRow row = null;
                HSSFCell cell = null;

                #region 指定紙張大小 A3=8, A4=9, Letter=1
                sheet.PrintSetup.PaperSize = 9;
                #endregion

                #region 指定直式或橫式 true=橫式 false=直式
                sheet.PrintSetup.Landscape = true;
                #endregion

                #region 藏隱格線
                sheet.DisplayGridlines = true;
                #endregion

                #region Page Head (粗體、14字、置中)儲存格格式
                HSSFCellStyle pageHeadCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                HSSFFont pageHeadFont = (HSSFFont)wb.CreateFont();
                pageHeadFont.Boldweight = (short)FontBoldWeight.Bold;
                pageHeadFont.FontHeightInPoints = 14;
                pageHeadCellStyle.SetFont(pageHeadFont);
                pageHeadCellStyle.Alignment = HorizontalAlignment.Center;
                #endregion

                #region Column Title (粗體、底線、置中)儲存格格式
                HSSFCellStyle colTitleCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                HSSFFont colTitleFont = (HSSFFont)wb.CreateFont();
                colTitleFont.Boldweight = (short)FontBoldWeight.Bold;
                colTitleCellStyle.SetFont(colTitleFont);
                colTitleCellStyle.BorderBottom = BorderStyle.Thin;
                colTitleCellStyle.Alignment = HorizontalAlignment.Center;
                #endregion

                #region 金額(千分位逗號、底線、右靠)儲存格格式
                HSSFCellStyle moneyCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                moneyCellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
                moneyCellStyle.BorderBottom = BorderStyle.Thin;
                moneyCellStyle.Alignment = HorizontalAlignment.Right;
                #endregion

                int rowIdx = -1;

                rowIdx = GenReportDHeader(sheet, rowIdx, data.Columns, pageHeadCellStyle, colTitleCellStyle);
                if (data == null || data.Rows.Count <= 0)
                {
                    #region 查無資料
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);
                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("查無資料");
                    #endregion
                }
                else
                {
                    rowIdx = GenReportDData(sheet, rowIdx, data, moneyCellStyle);
                }
                #endregion

                using (MemoryStream ms = new MemoryStream())
                {
                    wb.Write(ms);
                    ms.Flush();
                    content = ms.ToArray();
                }

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
        /// <param name="rowIndex"></param>
        /// <param name="columns"></param>
        /// <param name="pageHeadCellStyle"></param>
        /// <param name="colTitleCellStyle"></param>
        /// <returns></returns>
        private int GenReportDHeader(HSSFSheet sheet, int rowIndex, DataColumnCollection columns, HSSFCellStyle pageHeadCellStyle, HSSFCellStyle colTitleCellStyle)
        {
            if (columns == null)
            {
                return rowIndex;
            }

            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            #region Head Row 0
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);
                int colIndex = -1;

                colIndex++;
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue("手續費統計報表");
                cell.CellStyle = pageHeadCellStyle;

                sheet.AddMergedRegion(new CellRangeAddress(rowIdx, rowIdx, 0, columns.Count - 1));
            }
            #endregion

            #region Head Row 1
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);
                int colIndex = -1;

                foreach (DataColumn column in columns)
                {
                    colIndex++;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(column.Caption);
                    cell.CellStyle = colTitleCellStyle;
                }
            }
            #endregion

            return rowIdx;
        }

        /// <summary>
        /// 產生手續費統計報表表身
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
        /// <param name="data"></param>
        /// <param name="moneyCellStyle"></param>
        /// <returns></returns>
        private int GenReportDData(HSSFSheet sheet, int rowIndex, DataTable data, HSSFCellStyle moneyCellStyle)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            foreach (DataRow dataRow in data.Rows)
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);
                int colIndex = -1;

                foreach (DataColumn column in data.Columns)
                {
                    colIndex++;

                    if (column.DataType == _Int32Type || column.DataType == _Int64Type)
                    {
                        #region 數值
                        Int64 value = dataRow.IsNull(column) ? 0 : Convert.ToInt64(dataRow[column]);
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                        cell.SetCellValue(value);
                        cell.CellStyle = moneyCellStyle;
                        #endregion
                    }
                    else
                    {
                        #region 字串
                        string value = dataRow.IsNull(column) ? String.Empty : Convert.ToString(dataRow[column]);
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                        cell.SetCellValue(value);
                        #endregion
                    }
                }
            }

            return rowIdx;
        }
        #endregion

        #region 匯出代收單位交易資訊統計表
        /// <summary>
        /// 取得空的代收單位交易資訊統計表 DataTable
        /// </summary>
        /// <param name="channelIds"></param>
        /// <returns></returns>
        public DataTable GetEmptyReportD2Table(out string[] channelIds)
        {
            #region [MDY:20191214] (2019擴充案) 國際信用卡 - 增加 NC-國際信用卡
            #region [OLD]
            //#region [MDY:20171127] 增加全國繳費網 (C08)、台灣Pay (C10) (20170831_01)
            //#region [Old]
            ////#region [MDY:20170506] 增加支付寶 & 修正中信管道常數為 CTCB
            ////#region [Old]
            //////channelIds = new string[] {
            //////    ChannelHelper.TABS, ChannelHelper.RM, ChannelHelper.ATM, ChannelHelper.EDI, ChannelHelper.NB, ChannelHelper.VO,
            //////    ChannelHelper.FISC, ChannelHelper.CTCD, ChannelHelper.SM_711, ChannelHelper.SM_TFM, ChannelHelper.SM_OKM, ChannelHelper.SM_HILI,
            //////    ChannelHelper.SELF, ChannelHelper.LB
            //////};
            ////#endregion

            ////channelIds = new string[] {
            ////    ChannelHelper.TABS, ChannelHelper.RM, ChannelHelper.ATM, ChannelHelper.EDI, ChannelHelper.NB, ChannelHelper.VO,
            ////    ChannelHelper.FISC, ChannelHelper.CTCB, ChannelHelper.SM_711, ChannelHelper.SM_TFM, ChannelHelper.SM_OKM, ChannelHelper.SM_HILI,
            ////    ChannelHelper.SELF, ChannelHelper.LB, ChannelHelper.ALIPAY
            ////};
            ////#endregion
            //#endregion

            //channelIds = new string[] {
            //    ChannelHelper.TABS, ChannelHelper.RM, ChannelHelper.ATM, ChannelHelper.EDI, ChannelHelper.NB, ChannelHelper.VO,
            //    ChannelHelper.FISC, ChannelHelper.CTCB, ChannelHelper.SM_711, ChannelHelper.SM_TFM, ChannelHelper.SM_OKM, ChannelHelper.SM_HILI,
            //    ChannelHelper.SELF, ChannelHelper.LB, ChannelHelper.ALIPAY, ChannelHelper.EBILL, ChannelHelper.TWPAY
            //};
            //#endregion
            #endregion

            channelIds = new string[] {
                ChannelHelper.TABS, ChannelHelper.RM, ChannelHelper.ATM, ChannelHelper.EDI, ChannelHelper.NB, ChannelHelper.VO,
                ChannelHelper.FISC, ChannelHelper.CTCB, ChannelHelper.SM_711, ChannelHelper.SM_TFM, ChannelHelper.SM_OKM, ChannelHelper.SM_HILI,
                ChannelHelper.SELF, ChannelHelper.LB, ChannelHelper.ALIPAY, ChannelHelper.EBILL, ChannelHelper.TWPAY, ChannelHelper.FISC_NC
            };
            #endregion

            DataTable dt = new DataTable();
            DataColumn col = null;

            col = new DataColumn("Bank_Id", _StringType);
            col.Caption = "分行代碼";
            dt.Columns.Add(col);

            col = new DataColumn("Bank_Name", _StringType);
            col.Caption = "分行名稱";
            dt.Columns.Add(col);

            col = new DataColumn("Sch_Identy", _StringType);
            col.Caption = "學校代碼";
            dt.Columns.Add(col);

            col = new DataColumn("Sch_Name", _StringType);
            col.Caption = "學校名稱";
            dt.Columns.Add(col);

            col = new DataColumn("Total_Records", _Int32Type);
            col.Caption = "代收總筆數";
            dt.Columns.Add(col);

            col = new DataColumn("Total_Amount", typeof(System.Int64));
            col.Caption = "代收總金額";
            dt.Columns.Add(col);

            foreach (string channelId in channelIds)
            {
                string channelName = ChannelHelper.GetChannelName(channelId);
                col = new DataColumn(String.Format("{0}_DataCount", channelId), _Int32Type);
                col.Caption = String.Format("{0}總筆數", channelName);
                dt.Columns.Add(col);

                col = new DataColumn(String.Format("{0}_SumAmount", channelId), _Int32Type);
                col.Caption = String.Format("{0}總金額", channelName);
                dt.Columns.Add(col);
            }

            return dt;
        }

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
                #region 使用 HSSFWorkbook 產生 xls
                string sheetName = "sheet1";

                HSSFWorkbook wb = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)wb.CreateSheet(sheetName);
                HSSFRow row = null;
                HSSFCell cell = null;

                #region 指定紙張大小 A3=8, A4=9, Letter=1
                sheet.PrintSetup.PaperSize = 9;
                #endregion

                #region 指定直式或橫式 true=橫式 false=直式
                sheet.PrintSetup.Landscape = true;
                #endregion

                #region 藏隱格線
                sheet.DisplayGridlines = true;
                #endregion

                #region Page Head (粗體、14字、置中)儲存格格式
                HSSFCellStyle pageHeadCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                HSSFFont pageHeadFont = (HSSFFont)wb.CreateFont();
                pageHeadFont.Boldweight = (short)FontBoldWeight.Bold;
                pageHeadFont.FontHeightInPoints = 14;
                pageHeadCellStyle.SetFont(pageHeadFont);
                pageHeadCellStyle.Alignment = HorizontalAlignment.Center;
                #endregion

                #region Column Title (粗體、底線、置中)儲存格格式
                HSSFCellStyle colTitleCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                HSSFFont colTitleFont = (HSSFFont)wb.CreateFont();
                colTitleFont.Boldweight = (short)FontBoldWeight.Bold;
                colTitleCellStyle.SetFont(colTitleFont);
                colTitleCellStyle.BorderBottom = BorderStyle.Thin;
                colTitleCellStyle.Alignment = HorizontalAlignment.Center;
                #endregion

                #region 金額(千分位逗號、底線、右靠)儲存格格式
                HSSFCellStyle moneyCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                moneyCellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
                moneyCellStyle.BorderBottom = BorderStyle.Thin;
                moneyCellStyle.Alignment = HorizontalAlignment.Right;
                #endregion

                int rowIdx = -1;

                rowIdx = GenReportD2Header(sheet, rowIdx, data.Columns, pageHeadCellStyle, colTitleCellStyle);
                if (data == null || data.Rows.Count <= 0)
                {
                    #region 查無資料
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);
                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("查無資料");
                    #endregion
                }
                else
                {
                    rowIdx = GenReportD2Data(sheet, rowIdx, data, moneyCellStyle);
                }
                #endregion

                using (MemoryStream ms = new MemoryStream())
                {
                    wb.Write(ms);
                    ms.Flush();
                    content = ms.ToArray();
                }

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
        /// <param name="rowIndex"></param>
        /// <param name="columns"></param>
        /// <param name="pageHeadCellStyle"></param>
        /// <param name="colTitleCellStyle"></param>
        /// <returns></returns>
        private int GenReportD2Header(HSSFSheet sheet, int rowIndex, DataColumnCollection columns, HSSFCellStyle pageHeadCellStyle, HSSFCellStyle colTitleCellStyle)
        {
            if (columns == null)
            {
                return rowIndex;
            }

            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            #region Head Row 0
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);
                int colIndex = -1;

                colIndex++;
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue("代收單位交易資訊統計表");
                cell.CellStyle = pageHeadCellStyle;

                sheet.AddMergedRegion(new CellRangeAddress(rowIdx, rowIdx, 0, columns.Count - 1));
            }
            #endregion

            #region Head Row 1
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);
                int colIndex = -1;

                foreach (DataColumn column in columns)
                {
                    colIndex++;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(column.Caption);
                    cell.CellStyle = colTitleCellStyle;
                }
            }
            #endregion

            return rowIdx;
        }

        /// <summary>
        /// 產生代收單位交易資訊統計表表身
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
        /// <param name="data"></param>
        /// <param name="moneyCellStyle"></param>
        /// <returns></returns>
        private int GenReportD2Data(HSSFSheet sheet, int rowIndex, DataTable data, HSSFCellStyle moneyCellStyle)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            foreach (DataRow dataRow in data.Rows)
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);
                int colIndex = -1;

                foreach (DataColumn column in data.Columns)
                {
                    colIndex++;

                    if (column.DataType == _Int32Type || column.DataType == _Int64Type)
                    {
                        #region 數值
                        Int64 value = dataRow.IsNull(column) ? 0 : Convert.ToInt64(dataRow[column]);
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                        cell.SetCellValue(value);
                        cell.CellStyle = moneyCellStyle;
                        #endregion
                    }
                    else
                    {
                        #region 字串
                        string value = dataRow.IsNull(column) ? String.Empty : Convert.ToString(dataRow[column]);
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                        cell.SetCellValue(value);
                        #endregion
                    }
                }
            }

            return rowIdx;
        }
        #endregion

        #region 匯出繳款通道交易資訊統計表
        /// <summary>
        /// 取得空的繳款通道交易資訊統計表 DataTable
        /// </summary>
        /// <param name="channels">指定繳款通道</param>
        /// <returns></returns>
        public DataTable GetEmptyReportD3Table(ICollection<CodeText> channels)
        {
            DataTable dt = new DataTable();
            DataColumn col = null;

            col = new DataColumn("Bank_Id", _StringType);
            col.Caption = "分行代碼";
            dt.Columns.Add(col);

            col = new DataColumn("Bank_Name", _StringType);
            col.Caption = "分行名稱";
            dt.Columns.Add(col);

            col = new DataColumn("Sch_Identy", _StringType);
            col.Caption = "學校代碼";
            dt.Columns.Add(col);

            col = new DataColumn("Sch_Name", _StringType);
            col.Caption = "學校名稱";
            dt.Columns.Add(col);

            col = new DataColumn("Total_Records", _Int32Type);
            col.Caption = "代收總筆數";
            dt.Columns.Add(col);

            col = new DataColumn("Total_Amount", typeof(System.Int64));
            col.Caption = "代收總金額";
            dt.Columns.Add(col);

            col = new DataColumn("Total_Fee", _Int32Type);
            col.Caption = "總手續費";
            dt.Columns.Add(col);

            foreach (CodeText channel in channels)
            {
                col = new DataColumn(String.Format("{0}_DataCount", channel.Code), _Int32Type);
                col.Caption = String.Format("{0}總筆數", channel.Text);
                dt.Columns.Add(col);

                col = new DataColumn(String.Format("{0}_SumAmount", channel.Code), _Int32Type);
                col.Caption = String.Format("{0}總金額", channel.Text);
                dt.Columns.Add(col);

                col = new DataColumn(String.Format("{0}_Fee", channel.Code), _Int32Type);
                col.Caption = String.Format("{0}手續費", channel.Text);
                dt.Columns.Add(col);
            }

            return dt;
        }

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
                #region 使用 HSSFWorkbook 產生 xls
                string sheetName = "sheet1";

                HSSFWorkbook wb = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)wb.CreateSheet(sheetName);
                HSSFRow row = null;
                HSSFCell cell = null;

                #region 指定紙張大小 A3=8, A4=9, Letter=1
                sheet.PrintSetup.PaperSize = 9;
                #endregion

                #region 指定直式或橫式 true=橫式 false=直式
                sheet.PrintSetup.Landscape = true;
                #endregion

                #region 藏隱格線
                sheet.DisplayGridlines = true;
                #endregion

                #region Page Head (粗體、14字、置中)儲存格格式
                HSSFCellStyle pageHeadCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                HSSFFont pageHeadFont = (HSSFFont)wb.CreateFont();
                pageHeadFont.Boldweight = (short)FontBoldWeight.Bold;
                pageHeadFont.FontHeightInPoints = 14;
                pageHeadCellStyle.SetFont(pageHeadFont);
                pageHeadCellStyle.Alignment = HorizontalAlignment.Center;
                #endregion

                #region Column Title (粗體、底線、置中)儲存格格式
                HSSFCellStyle colTitleCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                HSSFFont colTitleFont = (HSSFFont)wb.CreateFont();
                colTitleFont.Boldweight = (short)FontBoldWeight.Bold;
                colTitleCellStyle.SetFont(colTitleFont);
                colTitleCellStyle.BorderBottom = BorderStyle.Thin;
                colTitleCellStyle.Alignment = HorizontalAlignment.Center;
                #endregion

                #region 金額(千分位逗號、底線、右靠)儲存格格式
                HSSFCellStyle moneyCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                moneyCellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
                moneyCellStyle.BorderBottom = BorderStyle.Thin;
                moneyCellStyle.Alignment = HorizontalAlignment.Right;
                #endregion

                int rowIdx = -1;

                rowIdx = GenReportD3Header(sheet, rowIdx, data.Columns, pageHeadCellStyle, colTitleCellStyle);
                if (data == null || data.Rows.Count <= 0)
                {
                    #region 查無資料
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);
                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("查無資料");
                    #endregion
                }
                else
                {
                    rowIdx = GenReportD3Data(sheet, rowIdx, data, moneyCellStyle);
                }
                #endregion

                using (MemoryStream ms = new MemoryStream())
                {
                    wb.Write(ms);
                    ms.Flush();
                    content = ms.ToArray();
                }

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
        /// <param name="rowIndex"></param>
        /// <param name="columns"></param>
        /// <param name="pageHeadCellStyle"></param>
        /// <param name="colTitleCellStyle"></param>
        /// <returns></returns>
        private int GenReportD3Header(HSSFSheet sheet, int rowIndex, DataColumnCollection columns, HSSFCellStyle pageHeadCellStyle, HSSFCellStyle colTitleCellStyle)
        {
            if (columns == null)
            {
                return rowIndex;
            }

            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            #region Head Row 0
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);
                int colIndex = -1;

                colIndex++;
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue("繳款通道交易資訊統計表");
                cell.CellStyle = pageHeadCellStyle;

                sheet.AddMergedRegion(new CellRangeAddress(rowIdx, rowIdx, 0, columns.Count - 1));
            }
            #endregion

            #region Head Row 1
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);
                int colIndex = -1;

                foreach (DataColumn column in columns)
                {
                    colIndex++;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(column.Caption);
                    cell.CellStyle = colTitleCellStyle;
                }
            }
            #endregion

            return rowIdx;
        }

        /// <summary>
        /// 產生繳款通道交易資訊統計表表身
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
        /// <param name="data"></param>
        /// <param name="moneyCellStyle"></param>
        /// <returns></returns>
        private int GenReportD3Data(HSSFSheet sheet, int rowIndex, DataTable data, HSSFCellStyle moneyCellStyle)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            foreach (DataRow dataRow in data.Rows)
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);
                int colIndex = -1;

                foreach (DataColumn column in data.Columns)
                {
                    colIndex++;

                    if (column.DataType == _Int32Type || column.DataType == _Int64Type)
                    {
                        #region 數值
                        Int64 value = dataRow.IsNull(column) ? 0 : Convert.ToInt64(dataRow[column]);
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                        cell.SetCellValue(value);
                        cell.CellStyle = moneyCellStyle;
                        #endregion
                    }
                    else
                    {
                        #region 字串
                        string value = dataRow.IsNull(column) ? String.Empty : Convert.ToString(dataRow[column]);
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                        cell.SetCellValue(value);
                        #endregion
                    }
                }
            }

            return rowIdx;
        }
        #endregion

        #region [MDY:20190906] (2019擴充案) 匯出每日銷帳結果查詢結果檔 (C3400001)
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
                #region 使用 HSSFWorkbook 產生 xls
                List<decimal> totalCounts = new List<decimal>();
                List<decimal> totalAmounts = new List<decimal>();

                HSSFWorkbook wb = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)wb.CreateSheet(sheetName);
                HSSFCell cell = null;
                int rowIdx = 0, colIdx = 0;

                #region Decimal 格式化用變數
                HSSFCellStyle amountCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                amountCellStyle.Alignment = HorizontalAlignment.Right;
                amountCellStyle.DataFormat = 3;
                #endregion

                #region 學年 Row
                {
                    rowIdx = 0;
                    HSSFRow row = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("學年");
                    #endregion

                    #region 各資料(學年)欄位
                    foreach (CancelResultEntity data in datas)
                    {
                        colIdx++;
                        cell = (HSSFCell)row.CreateCell(colIdx, CellType.String);
                        cell.SetCellValue(data.YearName);

                        //初始化 total
                        totalCounts.Add(0);
                        totalAmounts.Add(0);
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("合計");
                    #endregion
                }
                #endregion

                #region 學期 Row
                {
                    rowIdx++;
                    HSSFRow row = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("學期");
                    #endregion

                    #region 各資料(學期)欄位
                    foreach (CancelResultEntity data in datas)
                    {
                        colIdx++;
                        cell = (HSSFCell)row.CreateCell(colIdx, CellType.String);
                        cell.SetCellValue(data.TermName);
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("");
                    #endregion
                }
                #endregion

                #region 費用別
                {
                    rowIdx++;
                    HSSFRow row = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("費用別");
                    #endregion

                    #region 各資料(費用別)欄位
                    foreach (CancelResultEntity data in datas)
                    {
                        colIdx++;
                        cell = (HSSFCell)row.CreateCell(colIdx, CellType.String);
                        cell.SetCellValue(data.ReceiveName);
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("");
                    #endregion
                }
                #endregion

                #region 入帳日
                {
                    rowIdx++;
                    HSSFRow row = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("入帳日");
                    #endregion

                    #region 各資料(入帳日)欄位
                    foreach (CancelResultEntity data in datas)
                    {
                        DateTime? accountDate = DataFormat.ConvertDateText(data.AccountDate);

                        colIdx++;
                        cell = (HSSFCell)row.CreateCell(colIdx, CellType.String);
                        cell.SetCellValue(accountDate == null ? String.Empty : accountDate.Value.ToString("yyyy/MM/dd"));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("");
                    #endregion
                }
                #endregion

                #region 學校自收
                {
                    rowIdx++;
                    HSSFRow row1 = (HSSFRow)sheet.CreateRow(rowIdx);
                    rowIdx++;
                    HSSFRow row2 = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("學校自收筆數");
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.String);
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

                        colIdx++;
                        cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 統一代收
                {
                    rowIdx++;
                    HSSFRow row1 = (HSSFRow)sheet.CreateRow(rowIdx);
                    rowIdx++;
                    HSSFRow row2 = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("統一代收筆數");
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.String);
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

                        colIdx++;
                        cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 全家代收
                {
                    rowIdx++;
                    HSSFRow row1 = (HSSFRow)sheet.CreateRow(rowIdx);
                    rowIdx++;
                    HSSFRow row2 = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("全家代收筆數");
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.String);
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

                        colIdx++;
                        cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 萊爾富代收
                {
                    rowIdx++;
                    HSSFRow row1 = (HSSFRow)sheet.CreateRow(rowIdx);
                    rowIdx++;
                    HSSFRow row2 = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("萊爾富代收筆數");
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.String);
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

                        colIdx++;
                        cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region ＯＫ代收
                {
                    rowIdx++;
                    HSSFRow row1 = (HSSFRow)sheet.CreateRow(rowIdx);
                    rowIdx++;
                    HSSFRow row2 = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("ＯＫ代收筆數");
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.String);
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

                        colIdx++;
                        cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 中信平台代收
                {
                    rowIdx++;
                    HSSFRow row1 = (HSSFRow)sheet.CreateRow(rowIdx);
                    rowIdx++;
                    HSSFRow row2 = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("中信平台代收筆數");
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.String);
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

                        colIdx++;
                        cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 財金代收
                {
                    rowIdx++;
                    HSSFRow row1 = (HSSFRow)sheet.CreateRow(rowIdx);
                    rowIdx++;
                    HSSFRow row2 = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("財金代收筆數");
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.String);
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

                        colIdx++;
                        cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region ＡＴＭ代收
                {
                    rowIdx++;
                    HSSFRow row1 = (HSSFRow)sheet.CreateRow(rowIdx);
                    rowIdx++;
                    HSSFRow row2 = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("ＡＴＭ代收筆數");
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.String);
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

                        colIdx++;
                        cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 網路銀行代收
                {
                    rowIdx++;
                    HSSFRow row1 = (HSSFRow)sheet.CreateRow(rowIdx);
                    rowIdx++;
                    HSSFRow row2 = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("網路銀行代收筆數");
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.String);
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

                        colIdx++;
                        cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 臨櫃代收
                {
                    rowIdx++;
                    HSSFRow row1 = (HSSFRow)sheet.CreateRow(rowIdx);
                    rowIdx++;
                    HSSFRow row2 = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("臨櫃代收筆數");
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.String);
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

                        colIdx++;
                        cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 匯款代收
                {
                    rowIdx++;
                    HSSFRow row1 = (HSSFRow)sheet.CreateRow(rowIdx);
                    rowIdx++;
                    HSSFRow row2 = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("匯款代收筆數");
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.String);
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

                        colIdx++;
                        cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 支付寶代收 (C09)
                {
                    rowIdx++;
                    HSSFRow row1 = (HSSFRow)sheet.CreateRow(rowIdx);
                    rowIdx++;
                    HSSFRow row2 = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("支付寶代收筆數");
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.String);
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

                        colIdx++;
                        cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 全國繳費網代收 (C08)
                {
                    rowIdx++;
                    HSSFRow row1 = (HSSFRow)sheet.CreateRow(rowIdx);
                    rowIdx++;
                    HSSFRow row2 = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("全國繳費網代收筆數");
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.String);
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

                        colIdx++;
                        cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 台灣Pay代收 (C10)
                {
                    rowIdx++;
                    HSSFRow row1 = (HSSFRow)sheet.CreateRow(rowIdx);
                    rowIdx++;
                    HSSFRow row2 = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("台灣Pay代收筆數");
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.String);
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

                        colIdx++;
                        cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region [MDY:20191214] (2019擴充案) 國際信用卡
                {
                    rowIdx++;
                    HSSFRow row1 = (HSSFRow)sheet.CreateRow(rowIdx);
                    rowIdx++;
                    HSSFRow row2 = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("國際信用卡代收筆數");
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.String);
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

                        colIdx++;
                        cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion

                #region 合計
                {
                    rowIdx++;
                    HSSFRow row1 = (HSSFRow)sheet.CreateRow(rowIdx);
                    rowIdx++;
                    HSSFRow row2 = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 橫軸欄位名稱
                    colIdx = 0;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.String);
                    cell.SetCellValue("合計筆數");
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.String);
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

                        colIdx++;
                        cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                        cell.SetCellValue(Convert.ToDouble(myCount));
                        cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                        cell.CellStyle = amountCellStyle;
                        cell.SetCellValue(Convert.ToDouble(myAmount));
                    }
                    #endregion

                    #region 合計欄位
                    colIdx++;
                    cell = (HSSFCell)row1.CreateCell(colIdx, CellType.Numeric);
                    cell.SetCellValue(Convert.ToDouble(rowCount));
                    cell = (HSSFCell)row2.CreateCell(colIdx, CellType.Numeric);
                    cell.CellStyle = amountCellStyle;
                    cell.SetCellValue(Convert.ToDouble(rowAmount));
                    #endregion
                }
                #endregion
                #endregion

                using (MemoryStream ms = new MemoryStream(1024 * 1024 * 64))
                {
                    wb.Write(ms);
                    ms.Flush();
                    content = ms.ToArray();
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            return errmsg;
        }
        #endregion

        #region 繳費收費項目
        #region [MDY:20170704] 修正報表樣式
        #region [Old]
        ///// <summary>
        ///// 繳費收費項目
        ///// </summary>
        ///// <param name="dtHead">表頭資料</param>
        ///// <param name="dtPage">分頁資料</param>
        ///// <param name="dtDeptSum">部別小計資料</param>
        ///// <param name="dtMajorSum">系所小計資料</param>
        ///// <param name="dtClassSum">班別小計資料</param>
        ///// <param name="receiveItems">收入科目資料</param>
        ///// <param name="dtData">表身資料</param>
        ///// <param name="content">成功則傳回產生檔案的 byte 陣列，否則傳回 null</param>
        ///// <returns>成功則傳回 null，否怎傳回錯誤訊息</returns>
        //public string GenReportE(DataTable dtHead, DataTable dtPage, DataTable dtDeptSum, DataTable dtMajorSum, DataTable dtClassSum, KeyValueList<string> receiveItems, DataTable dtData, out byte[] content)
        //{
        //    content = null;
        //    string errmsg = null;
        //    try
        //    {
        //        #region 使用 HSSFWorkbook 產生 xls
        //        string sheetName = "sheet1";

        //        HSSFWorkbook wb = new HSSFWorkbook();
        //        HSSFSheet sheet = (HSSFSheet)wb.CreateSheet(sheetName);
        //        HSSFRow row = null;
        //        HSSFCell cell = null;

        //        #region 指定紙張大小 A3=8, A4=9, Letter=1
        //        sheet.PrintSetup.PaperSize = 9;
        //        #endregion

        //        #region 指定直式或橫式 true=橫式 false=直式
        //        sheet.PrintSetup.Landscape = true;
        //        #endregion

        //        #region 藏隱格線
        //        sheet.DisplayGridlines = false;
        //        #endregion

        //        #region 其他 XLS 屬性設定
        //        //HSSFCellStyle borderStyle = null;
        //        //HSSFCellStyle colorStyle = null;
        //        //HSSFCellStyle fontStyle = null;
        //        //HSSFCellStyle heightStyle = null;
        //        //HSSFCellStyle spanStyle = null;
        //        //HSSFCellStyle wrapStyle = null;
        //        //HSSFFont font = null;

        //        //borderStyle = workbook.CreateCellStyle();
        //        //colorStyle = workbook.CreateCellStyle();
        //        //fontStyle = workbook.CreateCellStyle();
        //        //heightStyle = workbook.CreateCellStyle();
        //        //spanStyle = workbook.CreateCellStyle();
        //        //wrapStyle = workbook.CreateCellStyle();

        //        ////Style設定    
        //        //borderStyle.BorderTop = HSSFCellStyle.BORDER_THIN;
        //        //borderStyle.BorderLeft = HSSFCellStyle.BORDER_THIN;
        //        //borderStyle.BorderBottom = HSSFCellStyle.BORDER_THIN;
        //        //borderStyle.BorderRight = HSSFCellStyle.BORDER_THIN;
        //        //colorStyle.FillForegroundColor = HSSFColor.LIGHT_CORNFLOWER_BLUE.index;
        //        //colorStyle.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
        //        //fontStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
        //        //fontStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
        //        //heightStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
        //        //heightStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
        //        //spanStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
        //        //spanStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
        //        //wrapStyle.WrapText = true;

        //        ////字型大小
        //        //font = workbook.CreateFont();
        //        //font.FontHeightInPoints = 14;
        //        //font.Boldweight = HSSFFont.BOLDWEIGHT_BOLD;
        //        //fontStyle.SetFont(font);
        //        //cell = sheet.CreateRow(1).CreateCell(0);
        //        //cell.CellStyle = fontStyle;
        //        //cell.SetCellValue("字型大小14粗體");

        //        ////合併儲存格
        //        //cell = sheet.CreateRow(2).CreateCell(0);
        //        //cell.SetCellValue("合併儲存格");
        //        //cell.CellStyle = spanStyle;
        //        //sheet.AddMergedRegion(new Region(2, 0, 3, 1));

        //        ////Wrap
        //        //cell = sheet.CreateRow(4).CreateCell(0);
        //        //cell.SetCellValue(string.Format("換行{0}測試", System.Environment.NewLine));
        //        //cell.CellStyle = wrapStyle;

        //        ////增加邊框
        //        //cell = sheet.CreateRow(5).CreateCell(1);
        //        //cell.SetCellValue("邊框                  ");
        //        //cell.CellStyle = borderStyle;

        //        ////背景
        //        //cell = sheet.CreateRow(6).CreateCell(0);
        //        //cell.SetCellValue("背景");
        //        //cell.CellStyle = colorStyle;
        //        #endregion

        //        int rowIdx = -1;
        //        int pageNo = 0;
        //        int totalPage = dtPage.Rows.Count;

        //        if (totalPage == 0)
        //        {
        //            #region Gen 表頭
        //            {
        //                rowIdx = this.GenReportEHead(sheet, rowIdx, dtHead, pageNo, totalPage);
        //            }
        //            #endregion

        //            #region 查無資料
        //            rowIdx++;
        //            row = (HSSFRow)sheet.CreateRow(rowIdx);
        //            int colIndex = 0;
        //            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //            cell.SetCellValue("查無資料");
        //            #endregion
        //        }
        //        else
        //        {
        //            int idx = 0;
        //            DataRow pageRow = dtPage.Rows[0];
        //            while (pageRow != null && idx < totalPage)
        //            {
        //                pageNo++;

        //                string receiveType = pageRow["Receive_Type"].ToString().Replace("'", "");
        //                string yearId = pageRow["Year_Id"].ToString().Replace("'", "");
        //                string termId = pageRow["Term_Id"].ToString().Replace("'", "");
        //                string depId = pageRow["Dep_Id"].ToString().Replace("'", "");
        //                string receiveId = pageRow["Receive_Id"].ToString().Replace("'", "");
        //                string deptId = pageRow["Dept_Id"].ToString().Replace("'", "");
        //                string majorId = pageRow["Major_Id"].ToString().Replace("'", "");
        //                string classId = pageRow["Class_Id"].ToString().Replace("'", "");
        //                string deptName = pageRow["Dept_Name"].ToString();
        //                string majorName = pageRow["Major_Name"].ToString();
        //                string className = pageRow["Class_Name"].ToString();
        //                string deptKey = deptId.Trim();
        //                string majorKey = String.Concat(deptId.Trim(), ",", majorId.Trim());

        //                #region Gen 表頭
        //                {
        //                    rowIdx = this.GenReportEHead(sheet, rowIdx, dtHead, pageNo, totalPage);
        //                }
        //                #endregion

        //                #region Gen 表身
        //                {
        //                    rowIdx++;
        //                    rowIdx = this.GenReportEForClassData(sheet, rowIdx, deptName, majorName, className, receiveItems, dtData, receiveType, yearId, termId, depId, receiveId, deptId, majorId, classId);
        //                }
        //                #endregion

        //                #region 班級合計
        //                {
        //                    int sumCount = 0;
        //                    double sumAmount = 0;

        //                    DataRow[] classRows = dtClassSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Class_Id='{7}'", receiveType, yearId, termId, depId, receiveId, deptId, majorId, classId));
        //                    if (classRows != null && classRows.Length > 0)
        //                    {
        //                        DataRow classRow = classRows[0];
        //                        sumCount = Int32.Parse(classRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
        //                        sumAmount = Double.Parse(classRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
        //                    }

        //                    rowIdx++;
        //                    row = (HSSFRow)sheet.CreateRow(rowIdx);

        //                    #region 班別人數
        //                    int colIndex = 0;
        //                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //                    cell.SetCellValue("班別人數：");

        //                    colIndex++;
        //                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
        //                    cell.SetCellValue(sumCount);
        //                    #endregion

        //                    #region 班別金額
        //                    colIndex = 3;
        //                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //                    cell.SetCellValue("班別金額：");

        //                    colIndex++;
        //                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
        //                    cell.SetCellValue(sumAmount);
        //                    #endregion
        //                }
        //                #endregion

        //                idx++;
        //                string nextDeptKey = null;
        //                string nextMajorKey = null;
        //                if (idx < totalPage)
        //                {
        //                    pageRow = dtPage.Rows[idx];
        //                    string nextDeptId = pageRow["Dept_Id"].ToString();
        //                    string nextMajorId = pageRow["Major_Id"].ToString();
        //                    nextDeptKey = nextDeptId.Trim();
        //                    nextMajorKey = String.Concat(nextDeptId.Trim(), ",", nextMajorId.Trim());
        //                }
        //                else
        //                {
        //                    pageRow = null;
        //                }

        //                #region 系所合計
        //                if (majorKey != nextMajorKey)
        //                {
        //                    int sumCount = 0;
        //                    double sumAmount = 0;
        //                    DataRow[] majorRows = dtMajorSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType, yearId, termId, depId, receiveId, deptId, majorId));
        //                    if (majorRows != null && majorRows.Length > 0)
        //                    {
        //                        DataRow majorRow = majorRows[0];
        //                        sumCount = Int32.Parse(majorRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
        //                        sumAmount = Double.Parse(majorRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
        //                    }

        //                    rowIdx++;
        //                    row = (HSSFRow)sheet.CreateRow(rowIdx);

        //                    #region 系所人數
        //                    int colIndex = 0;
        //                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //                    cell.SetCellValue("系所人數：");

        //                    colIndex++;
        //                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
        //                    cell.SetCellValue(sumCount);
        //                    #endregion

        //                    #region 系所金額
        //                    colIndex = 3;
        //                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //                    cell.SetCellValue("系所金額：");

        //                    colIndex++;
        //                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
        //                    cell.SetCellValue(sumAmount);
        //                    #endregion
        //                }
        //                #endregion

        //                #region 部別合計
        //                if (deptKey != nextDeptKey)
        //                {
        //                    int sumCount = 0;
        //                    double sumAmount = 0;
        //                    DataRow[] deptRows = dtDeptSum.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType, yearId, termId, depId, receiveId, deptId));
        //                    if (deptRows != null && deptRows.Length > 0)
        //                    {
        //                        DataRow deptRow = deptRows[0];
        //                        sumCount = Int32.Parse(deptRow["SUM_COUNT"].ToString()); //已做 ISNULL 處理
        //                        sumAmount = Double.Parse(deptRow["SUM_AMOUNT"].ToString()); //已做 ISNULL 處理
        //                    }
        //                    rowIdx++;
        //                    row = (HSSFRow)sheet.CreateRow(rowIdx);

        //                    #region 部別人數
        //                    int colIndex = 0;
        //                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //                    cell.SetCellValue("部別人數：");

        //                    colIndex++;
        //                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
        //                    cell.SetCellValue(sumCount);
        //                    #endregion

        //                    #region 部別金額
        //                    colIndex = 3;
        //                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //                    cell.SetCellValue("部別金額：");

        //                    colIndex++;
        //                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
        //                    cell.SetCellValue(sumAmount);
        //                    #endregion
        //                }
        //                #endregion

        //                #region 空白行+插入分頁
        //                rowIdx++;
        //                sheet.CreateRow(rowIdx);
        //                sheet.SetRowBreak(rowIdx);
        //                #endregion
        //            }
        //        }
        //        #endregion

        //        #region 將 HSSFWorkbook 轉成 byte[]
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            wb.Write(ms);
        //            ms.Flush();
        //            content = ms.ToArray();
        //        }
        //        sheet = null;
        //        wb = null;
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        errmsg = ex.Message;
        //    }
        //    return errmsg;
        //}

        ///// <summary>
        ///// 產生表頭
        ///// </summary>
        ///// <param name="sheet"></param>
        ///// <param name="rowIndex"></param>
        ///// <param name="dtHead"></param>
        ///// <param name="pageNo"></param>
        ///// <param name="totalPage"></param>
        ///// <returns>傳回最後的 Row Index</returns>
        //private int GenReportEHead(HSSFSheet sheet, int rowIndex, DataTable dtHead, int pageNo, int totalPage)
        //{
        //    HSSFRow row = null;
        //    HSSFCell cell = null;
        //    int rowIdx = rowIndex;

        //    #region Head Row 0
        //    rowIdx++;
        //    row = (HSSFRow)sheet.CreateRow(rowIdx);
        //    DataRow dRow = dtHead.Rows[0];

        //    #region 學校名稱
        //    {
        //        int colIndex = 0;
        //        string value = dRow["Sch_Name"].ToString();
        //        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //        cell.SetCellValue(value);
        //    }
        //    #endregion

        //    #region 報表名稱
        //    {
        //        int colIndex = 3;
        //        string value = dRow["ReportName"].ToString();
        //        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //        cell.SetCellValue(value);
        //    }
        //    #endregion
        //    #endregion

        //    #region Head Row 1
        //    rowIdx++;
        //    row = (HSSFRow)sheet.CreateRow(rowIdx);

        //    #region 學年
        //    {
        //        int colIndex = 0;
        //        string value = "學年：";
        //        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //        cell.SetCellValue(value);

        //        colIndex = 1;
        //        value = dRow["Year_Name"].ToString();
        //        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //        cell.SetCellValue(value);
        //    }
        //    #endregion

        //    #region 學期
        //    {
        //        int colIndex = 4;
        //        string value = "學期：";
        //        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //        cell.SetCellValue(value);

        //        colIndex = 5;
        //        value = dRow["Term_Name"].ToString();
        //        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //        cell.SetCellValue(value);
        //    }
        //    #endregion

        //    #region 繳費狀態
        //    {
        //        string receiveStatusName = dRow["ReceiveStatusName"].ToString();
        //        if (!String.IsNullOrEmpty(receiveStatusName))
        //        {
        //            int colIndex = 8;
        //            string value = "繳費狀態：";
        //            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //            cell.SetCellValue(value);

        //            colIndex = 9;
        //            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //            cell.SetCellValue(receiveStatusName);
        //        }
        //    }
        //    #endregion
        //    #endregion

        //    #region Head Row 2
        //    rowIdx++;
        //    row = (HSSFRow)sheet.CreateRow(rowIdx);

        //    #region 商家代號
        //    {
        //        int colIndex = 0;
        //        string value = "商家代號：";
        //        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //        cell.SetCellValue(value);

        //        colIndex = 1;
        //        value = dRow["Receive_Type"].ToString();
        //        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //        cell.SetCellValue(value);
        //    }
        //    #endregion

        //    #region 代收費用
        //    {
        //        int colIndex = 4;
        //        string value = "代收費用：";
        //        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //        cell.SetCellValue(value);

        //        colIndex = 5;
        //        value = dRow["Receive_Name"].ToString();
        //        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //        cell.SetCellValue(value);
        //    }
        //    #endregion

        //    #region 批號
        //    {
        //        string upNo = dRow["UpNo"].ToString();
        //        if (!String.IsNullOrEmpty(upNo))
        //        {
        //            int colIndex = 8;
        //            string value = "批號：";
        //            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //            cell.SetCellValue(value);

        //            colIndex = 9;
        //            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //            cell.SetCellValue(upNo);
        //        }
        //    }
        //    #endregion

        //    #region 日期
        //    {
        //        int colIndex = 11;
        //        string value = dRow["ReportDate"].ToString();
        //        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //        cell.SetCellValue(value);
        //    }
        //    #endregion

        //    #region 頁數
        //    {
        //        int colIndex = 12;
        //        string value = String.Format("第{0}頁/共{1}頁", pageNo, totalPage);
        //        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //        cell.SetCellValue(value);
        //    }
        //    #endregion
        //    #endregion

        //    return rowIdx;
        //}

        //private int GenReportEForClassData(HSSFSheet sheet, int rowIndex
        //    , string deptName, string majorName, string className
        //    , KeyValueList<string> receiveItems, DataTable dtData
        //    , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId, string majorId, string classId)
        //{
        //    HSSFRow row = null;
        //    HSSFCell cell = null;
        //    int rowIdx = rowIndex;

        //    #region Data Row 0
        //    {
        //        rowIdx++;
        //        row = (HSSFRow)sheet.CreateRow(rowIdx);

        //        #region 部別名稱
        //        {
        //            int colIndex = 0;
        //            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //            cell.SetCellValue(deptName);
        //        }
        //        #endregion

        //        #region 系所名稱
        //        {
        //            int colIndex = 2;
        //            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //            cell.SetCellValue(majorName);
        //        }
        //        #endregion

        //        #region 班別名稱
        //        {
        //            int colIndex = 4;
        //            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //            cell.SetCellValue(className);
        //        }
        //        #endregion
        //    }
        //    #endregion

        //    #region Data Row 1
        //    rowIdx++;
        //    row = (HSSFRow)sheet.CreateRow(rowIdx);
        //    #region 收入科目名稱
        //    {
        //        int colIndex = 0;
        //        foreach (KeyValue<string> receiveItem in receiveItems)
        //        {
        //            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //            cell.SetCellValue(receiveItem.Key);
        //            colIndex++;
        //        }
        //    }
        //    #endregion
        //    #endregion

        //    #region Data Row 2 ~ dRows.Count + 1
        //    DataRow[] dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Class_Id='{7}'", receiveType, yearId, termId, depId, receiveId, deptId, majorId, classId), "Stu_Id");
        //    if (dRows != null && dRows.Length > 0)
        //    {
        //        foreach (DataRow dRow in dRows)
        //        {
        //            rowIdx++;
        //            row = (HSSFRow)sheet.CreateRow(rowIdx);
        //            int colIndex = 0;
        //            foreach (KeyValue<string> receiveItem in receiveItems)
        //            {
        //                string columnName = receiveItem.Value;
        //                if (!dRow.IsNull(columnName))
        //                {
        //                    //[MDY:20170702] 排除以 Receive_ 開頭但非數值的欄位
        //                    if (columnName.StartsWith("Receive_") && dtData.Columns[columnName].DataType.FullName == "System.Decimal")
        //                    {
        //                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
        //                        double value = Double.Parse(dRow[columnName].ToString());
        //                        cell.SetCellValue(value);
        //                    }
        //                    else
        //                    {
        //                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
        //                        string value = dRow[columnName].ToString();
        //                        cell.SetCellValue(value);
        //                    }
        //                }
        //                colIndex++;
        //            }
        //        }
        //    }
        //    #endregion

        //    return rowIdx;
        //}
        #endregion

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
                #region 使用 HSSFWorkbook 產生 xls
                string sheetName = "sheet1";

                HSSFWorkbook wb = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)wb.CreateSheet(sheetName);
                HSSFRow row = null;
                HSSFCell cell = null;

                #region 指定紙張大小 A3=8, A4=9, Letter=1
                sheet.PrintSetup.PaperSize = 9;
                #endregion

                #region 指定直式或橫式 true=橫式 false=直式
                sheet.PrintSetup.Landscape = true;
                #endregion

                #region 指定縮放 (0 ~ 100)
                sheet.PrintSetup.Scale = 100;
                sheet.PrintSetup.FitWidth = 0;
                sheet.PrintSetup.FitHeight = 0;
                #endregion

                #region 藏隱格線
                sheet.DisplayGridlines = false;
                #endregion

                #region 日期 (左靠、yyyy/mm/dd)儲存格格式
                HSSFCellStyle dateCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                dateCellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("yyyy/mm/dd");
                dateCellStyle.Alignment = HorizontalAlignment.Left;
                #endregion

                #region left (左靠、底線)儲存格格式
                HSSFCellStyle leftCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                leftCellStyle.Alignment = HorizontalAlignment.Left;
                leftCellStyle.BorderBottom = BorderStyle.Thin;
                #endregion

                #region rigth (右靠、底線)儲存格格式
                HSSFCellStyle rightCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                rightCellStyle.Alignment = HorizontalAlignment.Right;
                rightCellStyle.BorderBottom = BorderStyle.Thin;
                #endregion

                #region center (置中、底線)儲存格格式
                HSSFCellStyle centerCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                centerCellStyle.Alignment = HorizontalAlignment.Center;
                centerCellStyle.BorderBottom = BorderStyle.Thin;
                #endregion

                #region 金額(千分位逗號、右靠、底線)儲存格格式
                HSSFCellStyle moneyCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                moneyCellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
                moneyCellStyle.Alignment = HorizontalAlignment.Right;
                moneyCellStyle.BorderBottom = BorderStyle.Thin;
                #endregion

                #region 其他 XLS 屬性設定
                //HSSFCellStyle borderStyle = null;
                //HSSFCellStyle colorStyle = null;
                //HSSFCellStyle fontStyle = null;
                //HSSFCellStyle heightStyle = null;
                //HSSFCellStyle spanStyle = null;
                //HSSFCellStyle wrapStyle = null;
                //HSSFFont font = null;

                //borderStyle = workbook.CreateCellStyle();
                //colorStyle = workbook.CreateCellStyle();
                //fontStyle = workbook.CreateCellStyle();
                //heightStyle = workbook.CreateCellStyle();
                //spanStyle = workbook.CreateCellStyle();
                //wrapStyle = workbook.CreateCellStyle();

                ////Style設定    
                //borderStyle.BorderTop = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderLeft = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderBottom = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderRight = HSSFCellStyle.BORDER_THIN;
                //colorStyle.FillForegroundColor = HSSFColor.LIGHT_CORNFLOWER_BLUE.index;
                //colorStyle.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
                //fontStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //fontStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //heightStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //heightStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //spanStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //spanStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //wrapStyle.WrapText = true;

                ////字型大小
                //font = workbook.CreateFont();
                //font.FontHeightInPoints = 14;
                //font.Boldweight = HSSFFont.BOLDWEIGHT_BOLD;
                //fontStyle.SetFont(font);
                //cell = sheet.CreateRow(1).CreateCell(0);
                //cell.CellStyle = fontStyle;
                //cell.SetCellValue("字型大小14粗體");

                ////合併儲存格
                //cell = sheet.CreateRow(2).CreateCell(0);
                //cell.SetCellValue("合併儲存格");
                //cell.CellStyle = spanStyle;
                //sheet.AddMergedRegion(new Region(2, 0, 3, 1));

                ////Wrap
                //cell = sheet.CreateRow(4).CreateCell(0);
                //cell.SetCellValue(string.Format("換行{0}測試", System.Environment.NewLine));
                //cell.CellStyle = wrapStyle;

                ////增加邊框
                //cell = sheet.CreateRow(5).CreateCell(1);
                //cell.SetCellValue("邊框                  ");
                //cell.CellStyle = borderStyle;

                ////背景
                //cell = sheet.CreateRow(6).CreateCell(0);
                //cell.SetCellValue("背景");
                //cell.CellStyle = colorStyle;
                #endregion

                int rowIdx = -1;
                int pageNo = 0;
                int totalPage = dtPage.Rows.Count * receiveItems.Count;

                if (totalPage == 0)
                {
                    #region Gen 表頭
                    {
                        rowIdx = this.GenReportE1Head(sheet, dateCellStyle, rowIdx, dtHead, pageNo, totalPage);
                    }
                    #endregion

                    #region 查無資料
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);
                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
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
                                rowIdx = this.GenReportE1Head(sheet, dateCellStyle, rowIdx, dtHead, pageNo, totalPage);
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
                                rowIdx++;
                                rowIdx = this.GenReportE1Body(sheet, leftCellStyle, rightCellStyle, centerCellStyle, moneyCellStyle
                                    , rowIdx, dtData, receiveType, yearId, termId, depId, receiveId, receiveItem.Key, receiveItem.Value, deptId, deptName);
                            }
                            #endregion

                            #region 空白行+插入分頁
                            rowIdx++;
                            sheet.CreateRow(rowIdx);
                            sheet.SetRowBreak(rowIdx);
                            #endregion
                        }

                    }
                }
                #endregion

                #region 將 HSSFWorkbook 轉成 byte[]
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.Write(ms);
                    ms.Flush();
                    content = ms.ToArray();
                }
                sheet = null;
                wb = null;
                #endregion
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
        /// <param name="rowIndex"></param>
        /// <param name="dtHead"></param>
        /// <param name="pageNo"></param>
        /// <param name="totalPage"></param>
        /// <returns>傳回最後的 Row Index</returns>
        private int GenReportE1Head(HSSFSheet sheet, HSSFCellStyle dateCellStyle, int rowIndex, DataTable dtHead, int pageNo, int totalPage)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            #region Head Row 0
            rowIdx++;
            row = (HSSFRow)sheet.CreateRow(rowIdx);
            DataRow dRow = dtHead.Rows[0];

            #region 學校名稱
            {
                int colIndex = 0;
                string value = dRow["Sch_Name"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 報表名稱
            {
                int colIndex = 3;
                string value = dRow["ReportName"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion
            #endregion

            #region Head Row 1
            rowIdx++;
            row = (HSSFRow)sheet.CreateRow(rowIdx);

            #region 學年
            {
                int colIndex = 0;
                string value = "學年：";
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);

                colIndex = 1;
                value = dRow["Year_Name"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 學期
            {
                int colIndex = 4;
                string value = "學期：";
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);

                colIndex = 5;
                value = dRow["Term_Name"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 繳費狀態
            {
                string receiveStatusName = dRow["ReceiveStatusName"].ToString();
                if (!String.IsNullOrEmpty(receiveStatusName))
                {
                    int colIndex = 8;
                    string value = "繳費狀態：";
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);

                    colIndex = 9;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(receiveStatusName);
                }
            }
            #endregion
            #endregion

            #region Head Row 2
            rowIdx++;
            row = (HSSFRow)sheet.CreateRow(rowIdx);

            #region 商家代號
            {
                int colIndex = 0;
                string value = "商家代號：";
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);

                colIndex = 1;
                value = dRow["Receive_Type"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 代收費用
            {
                int colIndex = 4;
                string value = "代收費用：";
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);

                colIndex = 5;
                value = dRow["Receive_Name"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 批號
            {
                string upNo = dRow["UpNo"].ToString();
                if (!String.IsNullOrEmpty(upNo))
                {
                    int colIndex = 8;
                    string value = "批號：";
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);

                    colIndex = 9;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(upNo);
                }
            }
            #endregion

            #region 日期
            {
                int colIndex = 10;
                string value = dRow["ReportDate"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
                cell.CellStyle = dateCellStyle;
            }
            #endregion

            #region 頁數
            {
                int colIndex = 12;
                string value = String.Format("第{0}頁/共{1}頁", pageNo, totalPage);
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion
            #endregion

            return rowIdx;
        }

        /// <summary>
        /// 產生 繳費收費項目明細分析表 表身
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="leftCellStyle"></param>
        /// <param name="rightCellStyle"></param>
        /// <param name="centerCellStyle"></param>
        /// <param name="moneyCellStyle"></param>
        /// <param name="rowIndex"></param>
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
        private int GenReportE1Body(HSSFSheet sheet, HSSFCellStyle leftCellStyle, HSSFCellStyle rightCellStyle, HSSFCellStyle centerCellStyle, HSSFCellStyle moneyCellStyle
            , int rowIndex, DataTable dtData, string receiveType, string yearId, string termId, string depId, string receiveId, string receiveItemKey, string receiveItemName, string deptId, string deptName)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            #region Data Row 0, 1, 2
            {
                #region 欄位名稱
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 識別名稱
                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("識別名稱");
                    cell.CellStyle = centerCellStyle;
                    this.CreatEmptyColumnCellAndMerge(cell, 5);
                    #endregion

                    #region 收費金額
                    colIndex = 6;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("收費金額");
                    cell.CellStyle = rightCellStyle;
                    this.CreatEmptyColumnCellAndMerge(cell, 1);
                    #endregion

                    #region 人數
                    colIndex = 8;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("人數");
                    cell.CellStyle = rightCellStyle;
                    this.CreatEmptyColumnCellAndMerge(cell, 1);
                    #endregion

                    #region 合計
                    colIndex = 10;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("合計");
                    cell.CellStyle = rightCellStyle;
                    this.CreatEmptyColumnCellAndMerge(cell, 2);
                    #endregion

                    #region 空白 Cell
                    colIndex = 13;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("");
                    cell.CellStyle = leftCellStyle;
                    #endregion
                }
                #endregion

                #region 收入科目名稱
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(receiveItemName);
                    cell.CellStyle = leftCellStyle;
                    this.CreatEmptyColumnCellAndMerge(cell, 2);
                }
                #endregion

                #region 部別名稱
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                    int colIndex = 1;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(deptName);
                    cell.CellStyle = leftCellStyle;
                    this.CreatEmptyColumnCellAndMerge(cell, 2);
                }
                #endregion
            }
            #endregion

            #region 系所名稱, 合計資料
            {
                #region [MDY:20210401] 原碼修正
                DataRow[] dRows = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Receive_Item_Key='{5}' AND Dept_Id='{6}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), receiveItemKey.Replace("'", ""), deptId.Replace("'", "")), "MAJOR_FLAG DESC, Major_Id, CLASS_FLAG DESC, Class_Id, Receive_Item_Amount");
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

                                rowIdx++;
                                row = (HSSFRow)sheet.CreateRow(rowIdx);

                                int colIndex = 2;
                                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                cell.SetCellValue(majorName);
                                cell.CellStyle = leftCellStyle;
                                this.CreatEmptyColumnCellAndMerge(cell, 2);
                            }
                            #endregion
                        }

                        #region 合計資料 (班別合計 Or 系所合計 Or 部別合計)
                        {
                            rowIdx++;
                            row = (HSSFRow)sheet.CreateRow(rowIdx);

                            #region 合計名稱
                            {
                                string dataKind = null;
                                int colIndex = 0;
                                int mergeCount = 0;
                                if (majorFlag == "N")
                                {
                                    colIndex = 4;
                                    dataKind = "部別合計：";
                                    mergeCount = 1;
                                }
                                else if (classFlag == "N")
                                {
                                    colIndex = 4;
                                    dataKind = "系所合計：";
                                    mergeCount = 1;
                                }
                                else
                                {
                                    colIndex = 3;
                                    dataKind = className; //班級名稱
                                    mergeCount = 2;
                                }

                                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                cell.SetCellValue(dataKind);
                                cell.CellStyle = leftCellStyle;
                                this.CreatEmptyColumnCellAndMerge(cell, mergeCount);
                            }
                            #endregion

                            #region 收費金額
                            {
                                int colIndex = 6;
                                cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                if (receiveItemAmount != null)
                                {
                                    cell.SetCellValue(receiveItemAmount.Value);
                                }
                                else
                                {
                                    cell.SetCellValue("");
                                }
                                cell.CellStyle = moneyCellStyle;
                                this.CreatEmptyColumnCellAndMerge(cell, 1);
                            }
                            #endregion

                            #region 人數
                            {
                                int colIndex = 8;
                                cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                cell.SetCellValue(dataCount);
                                cell.CellStyle = rightCellStyle;
                                this.CreatEmptyColumnCellAndMerge(cell, 1);
                            }
                            #endregion

                            #region 合計
                            {
                                int colIndex = 10;
                                cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                cell.SetCellValue(sumAmount);
                                cell.CellStyle = moneyCellStyle;
                                this.CreatEmptyColumnCellAndMerge(cell, 2);
                            }
                            #endregion

                            #region 空白 Cell
                            {
                                int colIndex = 13;
                                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
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

            return rowIdx;
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
                #region 使用 HSSFWorkbook 產生 xls
                string sheetName = "sheet1";

                HSSFWorkbook wb = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)wb.CreateSheet(sheetName);
                HSSFRow row = null;
                HSSFCell cell = null;

                #region 指定紙張大小 A3=8, A4=9, Letter=1
                sheet.PrintSetup.PaperSize = 9;
                #endregion

                #region 指定直式或橫式 true=橫式 false=直式
                sheet.PrintSetup.Landscape = true;
                #endregion

                #region 指定縮放 (0 ~ 100)
                sheet.PrintSetup.Scale = 100;
                sheet.PrintSetup.FitWidth = 0;
                sheet.PrintSetup.FitHeight = 0;
                #endregion

                #region 藏隱格線
                sheet.DisplayGridlines = false;
                #endregion

                #region 日期 (左靠、yyyy/mm/dd)儲存格格式
                HSSFCellStyle dateCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                dateCellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("yyyy/mm/dd");
                dateCellStyle.Alignment = HorizontalAlignment.Left;
                #endregion

                #region left (左靠、底線)儲存格格式
                HSSFCellStyle leftCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                leftCellStyle.Alignment = HorizontalAlignment.Left;
                leftCellStyle.BorderBottom = BorderStyle.Thin;
                #endregion

                #region rigth (右靠、底線)儲存格格式
                HSSFCellStyle rightCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                rightCellStyle.Alignment = HorizontalAlignment.Right;
                rightCellStyle.BorderBottom = BorderStyle.Thin;
                #endregion

                #region rigth2 (右靠、無底線)儲存格格式
                HSSFCellStyle right2CellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                right2CellStyle.Alignment = HorizontalAlignment.Right;
                #endregion

                #region center (置中、底線)儲存格格式
                HSSFCellStyle centerCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                centerCellStyle.Alignment = HorizontalAlignment.Center;
                centerCellStyle.BorderBottom = BorderStyle.Thin;
                #endregion

                #region 金額(千分位逗號、右靠、底線)儲存格格式
                HSSFCellStyle moneyCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                moneyCellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
                moneyCellStyle.Alignment = HorizontalAlignment.Right;
                moneyCellStyle.BorderBottom = BorderStyle.Thin;
                #endregion

                #region 金額(千分位逗號、右靠、無底線)儲存格格式
                HSSFCellStyle money2CellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                money2CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
                money2CellStyle.Alignment = HorizontalAlignment.Right;
                #endregion

                #region 其他 XLS 屬性設定
                //HSSFCellStyle borderStyle = null;
                //HSSFCellStyle colorStyle = null;
                //HSSFCellStyle fontStyle = null;
                //HSSFCellStyle heightStyle = null;
                //HSSFCellStyle spanStyle = null;
                //HSSFCellStyle wrapStyle = null;
                //HSSFFont font = null;

                //borderStyle = workbook.CreateCellStyle();
                //colorStyle = workbook.CreateCellStyle();
                //fontStyle = workbook.CreateCellStyle();
                //heightStyle = workbook.CreateCellStyle();
                //spanStyle = workbook.CreateCellStyle();
                //wrapStyle = workbook.CreateCellStyle();

                ////Style設定    
                //borderStyle.BorderTop = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderLeft = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderBottom = HSSFCellStyle.BORDER_THIN;
                //borderStyle.BorderRight = HSSFCellStyle.BORDER_THIN;
                //colorStyle.FillForegroundColor = HSSFColor.LIGHT_CORNFLOWER_BLUE.index;
                //colorStyle.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
                //fontStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //fontStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //heightStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //heightStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //spanStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
                //spanStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
                //wrapStyle.WrapText = true;

                ////字型大小
                //font = workbook.CreateFont();
                //font.FontHeightInPoints = 14;
                //font.Boldweight = HSSFFont.BOLDWEIGHT_BOLD;
                //fontStyle.SetFont(font);
                //cell = sheet.CreateRow(1).CreateCell(0);
                //cell.CellStyle = fontStyle;
                //cell.SetCellValue("字型大小14粗體");

                ////合併儲存格
                //cell = sheet.CreateRow(2).CreateCell(0);
                //cell.SetCellValue("合併儲存格");
                //cell.CellStyle = spanStyle;
                //sheet.AddMergedRegion(new Region(2, 0, 3, 1));

                ////Wrap
                //cell = sheet.CreateRow(4).CreateCell(0);
                //cell.SetCellValue(string.Format("換行{0}測試", System.Environment.NewLine));
                //cell.CellStyle = wrapStyle;

                ////增加邊框
                //cell = sheet.CreateRow(5).CreateCell(1);
                //cell.SetCellValue("邊框                  ");
                //cell.CellStyle = borderStyle;

                ////背景
                //cell = sheet.CreateRow(6).CreateCell(0);
                //cell.SetCellValue("背景");
                //cell.CellStyle = colorStyle;
                #endregion

                int rowIdx = -1;
                int pageNo = 0;
                int totalPage = dtPage.Rows.Count;

                if (totalPage == 0)
                {
                    #region Gen 表頭
                    {
                        rowIdx = this.GenReportE1Head(sheet, dateCellStyle, rowIdx, dtHead, pageNo, totalPage);
                    }
                    #endregion

                    #region 查無資料
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);
                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
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
                            rowIdx = this.GenReportE2Head(sheet, dateCellStyle, rowIdx, dtHead, pageNo, totalPage);
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
                            rowIdx++;
                            rowIdx = this.GenReportE2Body(sheet, leftCellStyle, rightCellStyle, right2CellStyle, centerCellStyle, moneyCellStyle, money2CellStyle
                                , rowIdx, dtMajor, dtClass, dtDatas, receiveItems, receiveItemAmounts, receiveType, yearId, termId, depId, receiveId, deptId, deptName, (pageNo == totalPage));
                        }
                        #endregion

                        #region 空白行+插入分頁
                        rowIdx++;
                        sheet.CreateRow(rowIdx);
                        sheet.SetRowBreak(rowIdx);
                        #endregion
                    }
                }
                #endregion

                #region 將 HSSFWorkbook 轉成 byte[]
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.Write(ms);
                    ms.Flush();
                    content = ms.ToArray();
                }
                sheet = null;
                wb = null;
                #endregion
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
        /// <param name="rowIndex"></param>
        /// <param name="dtHead"></param>
        /// <param name="pageNo"></param>
        /// <param name="totalPage"></param>
        /// <returns>傳回最後的 Row Index</returns>
        private int GenReportE2Head(HSSFSheet sheet, HSSFCellStyle dateCellStyle, int rowIndex, DataTable dtHead, int pageNo, int totalPage)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            #region Head Row 0
            rowIdx++;
            row = (HSSFRow)sheet.CreateRow(rowIdx);
            DataRow dRow = dtHead.Rows[0];

            #region 學校名稱
            {
                int colIndex = 0;
                string value = dRow["Sch_Name"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 報表名稱
            {
                int colIndex = 3;
                string value = dRow["ReportName"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion
            #endregion

            #region Head Row 1
            rowIdx++;
            row = (HSSFRow)sheet.CreateRow(rowIdx);

            #region 學年
            {
                int colIndex = 0;
                string value = "學年：";
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);

                colIndex = 1;
                value = dRow["Year_Name"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 學期
            {
                int colIndex = 4;
                string value = "學期：";
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);

                colIndex = 5;
                value = dRow["Term_Name"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 繳費狀態
            {
                string receiveStatusName = dRow["ReceiveStatusName"].ToString();
                if (!String.IsNullOrEmpty(receiveStatusName))
                {
                    int colIndex = 8;
                    string value = "繳費狀態：";
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);

                    colIndex = 9;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(receiveStatusName);
                }
            }
            #endregion
            #endregion

            #region Head Row 2
            rowIdx++;
            row = (HSSFRow)sheet.CreateRow(rowIdx);

            #region 商家代號
            {
                int colIndex = 0;
                string value = "商家代號：";
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);

                colIndex = 1;
                value = dRow["Receive_Type"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 代收費用
            {
                int colIndex = 4;
                string value = "代收費用：";
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);

                colIndex = 5;
                value = dRow["Receive_Name"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion

            #region 批號
            {
                string upNo = dRow["UpNo"].ToString();
                if (!String.IsNullOrEmpty(upNo))
                {
                    int colIndex = 8;
                    string value = "批號：";
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(value);

                    colIndex = 9;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue(upNo);
                }
            }
            #endregion

            #region 日期
            {
                int colIndex = 10;
                string value = dRow["ReportDate"].ToString();
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
                cell.CellStyle = dateCellStyle;
            }
            #endregion

            #region 頁數
            {
                int colIndex = 12;
                string value = String.Format("第{0}頁/共{1}頁", pageNo, totalPage);
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(value);
            }
            #endregion
            #endregion

            return rowIdx;
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
        /// <param name="rowIndex"></param>
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
        private int GenReportE2Body(HSSFSheet sheet, HSSFCellStyle leftCellStyle, HSSFCellStyle rightCellStyle, HSSFCellStyle right2CellStyle, HSSFCellStyle centerCellStyle, HSSFCellStyle moneyCellStyle, HSSFCellStyle money2CellStyle
            , int rowIndex, DataTable dtMajor, DataTable dtClass, List<DataTable> dtDatas, KeyValueList<string> receiveItems, KeyValueList<double> receiveItemAmounts
            , string receiveType, string yearId, string termId, string depId, string receiveId, string deptId, string deptName, bool isLastPage)
        {
            HSSFRow row = null;
            HSSFCell cell = null;
            int rowIdx = rowIndex;

            //總計的 Column Index
            int rowSumColIndex = 1 + receiveItemAmounts.Count * 2;

            #region Data Row 0 (部別名稱)
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);

                int colIndex = 0;
                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                cell.SetCellValue(deptName);
                cell.CellStyle = leftCellStyle;
                this.CreatEmptyColumnCellAndMerge(cell, 2);
            }
            #endregion

            #region 逐系所產生表身

            #region [MDY:20210401] 原碼修正
            DataRow[] drMajors = dtMajor.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", "")), "Major_Id");
            #endregion

            if (drMajors != null && drMajors.Length > 0)
            {
                foreach (DataRow drMajor in drMajors)
                {
                    string majorId = drMajor["Major_Id"].ToString();
                    string majorName = drMajor["Major_Name"].ToString();

                    #region Data Row (系所名稱)
                    {
                        rowIdx++;
                        row = (HSSFRow)sheet.CreateRow(rowIdx);

                        int colIndex = 1;
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                        cell.SetCellValue(majorName);
                        cell.CellStyle = leftCellStyle;
                        this.CreatEmptyColumnCellAndMerge(cell, 2);
                    }
                    #endregion

                    #region 逐班級產生表身

                    #region [MDY:20210401] 原碼修正
                    DataRow[] drClasses = dtClass.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", ""), majorId.Replace("'", "")), "Class_Id");
                    #endregion

                    if (drClasses != null && drClasses.Length > 0)
                    {
                        foreach (DataRow drClass in drClasses)
                        {
                            string classId = drClass["Class_Id"].ToString();
                            string className = drClass["Class_Name"].ToString();

                            #region Data Row (班級名稱)
                            {
                                rowIdx++;
                                row = (HSSFRow)sheet.CreateRow(rowIdx);

                                int colIndex = 0;
                                cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                cell.SetCellValue(className);
                                cell.CellStyle = leftCellStyle;
                                this.CreatEmptyColumnCellAndMerge(cell, (receiveItemAmounts.Count + 1) * 2); //每個收入科目金額 2 格 + 總計 2 格
                            }
                            #endregion

                            #region Data Row (收入科目名稱 + 總計)
                            {
                                rowIdx++;
                                row = (HSSFRow)sheet.CreateRow(rowIdx);

                                #region 收入科目名稱
                                {
                                    int colIndex = 1;
                                    foreach (KeyValue<double> receiveItemAmount in receiveItemAmounts)
                                    {
                                        KeyValue<string> receiveItem = receiveItems.Find(x => x.Key == receiveItemAmount.Key);

                                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                        cell.SetCellValue(receiveItem.Value);
                                        cell.CellStyle = right2CellStyle;
                                        this.CreatEmptyColumnCellAndMerge(cell, 1); //每個收入科目金額 2 格
                                        colIndex += 2;
                                    }
                                }
                                #endregion

                                #region 總計
                                {
                                    cell = (HSSFCell)row.CreateCell(rowSumColIndex, CellType.String);
                                    cell.SetCellValue("總計");
                                    cell.CellStyle = right2CellStyle;
                                    this.CreatEmptyColumnCellAndMerge(cell, 1); //總計 2 格
                                }
                                #endregion
                            }
                            #endregion

                            #region Data Row (單價)
                            {
                                rowIdx++;
                                row = (HSSFRow)sheet.CreateRow(rowIdx);

                                #region 單價
                                {
                                    int colIndex = 0;
                                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                    cell.SetCellValue("單　價：");
                                    cell.CellStyle = right2CellStyle;
                                }
                                #endregion

                                #region 收入科目金額資料
                                {
                                    int colIndex = 1;
                                    foreach (KeyValue<double> receiveItemAmount in receiveItemAmounts)
                                    {
                                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                        cell.SetCellValue(receiveItemAmount.Value);
                                        cell.CellStyle = money2CellStyle;
                                        this.CreatEmptyColumnCellAndMerge(cell, 1); //每個收入科目金額 2 格
                                        colIndex += 2;
                                    }
                                }
                                #endregion
                            }
                            #endregion

                            #region Data Row (人數)
                            {
                                rowIdx++;
                                row = (HSSFRow)sheet.CreateRow(rowIdx);

                                #region 人數
                                {
                                    int colIndex = 0;
                                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                    cell.SetCellValue("人　數：");
                                    cell.CellStyle = right2CellStyle;
                                }
                                #endregion

                                #region 收入科目人數資料
                                {
                                    int colIndex = 1;
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
                                            #region [MDY:20210401] 原碼修正
                                            DataRow[] drDatas = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Class_Id = '{7}' AND Receive_Item_Key = '{8}' AND Receive_Item_Amount = {9}", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", ""), majorId.Replace("'", ""), classId.Replace("'", ""), receiveItemKey.Replace("'", ""), receiveItemValue));
                                            #endregion

                                            if (drDatas != null && drDatas.Length > 0)
                                            {
                                                double dataCount = Convert.ToDouble(drDatas[0]["Data_Count"]);
                                                cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                                cell.SetCellValue(dataCount);
                                                cell.CellStyle = money2CellStyle;
                                                this.CreatEmptyColumnCellAndMerge(cell, 1); //每個收入科目金額 2 格
                                            }
                                        }
                                        colIndex += 2;
                                    }
                                }
                                #endregion
                            }
                            #endregion

                            #region Data Row (總金額)
                            {
                                rowIdx++;
                                row = (HSSFRow)sheet.CreateRow(rowIdx);

                                #region 總金額
                                {
                                    int colIndex = 0;
                                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                                    cell.SetCellValue("總金額：");
                                    cell.CellStyle = rightCellStyle;
                                }
                                #endregion

                                #region 收入科目總金額資料
                                double rowSumAmount = 0;    //各收入科目總金額的總計
                                {
                                    int colIndex = 1;
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
                                            #region [MDY:20210401] 原碼修正
                                            DataRow[] drDatas = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Class_Id = '{7}' AND Receive_Item_Key = '{8}' AND Receive_Item_Amount = {9}", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", ""), majorId.Replace("'", ""), classId.Replace("'", ""), receiveItemKey.Replace("'", ""), receiveItemValue));
                                            #endregion

                                            if (drDatas != null && drDatas.Length > 0)
                                            {
                                                drData = drDatas[0];
                                                //double sumAmount = Convert.ToDouble(drDatas[0]["Sum_Amount"]);
                                                //cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                                //cell.SetCellValue(sumAmount);
                                                //cell.CellStyle = moneyCellStyle;
                                                //this.CreatEmptyColumnCellAndMerge(cell, 1); //每個收入科目金額 2 格
                                                //rowSumAmount += sumAmount;
                                            }
                                        }
                                        if (drData != null)
                                        {
                                            double sumAmount = Convert.ToDouble(drData["Sum_Amount"]);
                                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                            cell.SetCellValue(sumAmount);
                                            cell.CellStyle = moneyCellStyle;
                                            this.CreatEmptyColumnCellAndMerge(cell, 1); //每個收入科目金額 2 格
                                            rowSumAmount += sumAmount;
                                        }
                                        else
                                        {
                                            //總金額有畫底線，所以無資料要補空白的儲存格
                                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                            cell.SetCellValue(String.Empty);
                                            cell.CellStyle = moneyCellStyle;
                                            this.CreatEmptyColumnCellAndMerge(cell, 1); //每個收入科目金額 2 格
                                        }
                                        colIndex += 2;
                                    }
                                }
                                #endregion

                                #region 總計
                                {
                                    cell = (HSSFCell)row.CreateCell(rowSumColIndex, CellType.Numeric);
                                    cell.SetCellValue(rowSumAmount);
                                    cell.CellStyle = moneyCellStyle;
                                    this.CreatEmptyColumnCellAndMerge(cell, 1); //總計 2 格
                                }
                                #endregion
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region Data Row (系所合計)
                    {
                        rowIdx++;
                        row = (HSSFRow)sheet.CreateRow(rowIdx);

                        #region 系所合計
                        {
                            int colIndex = 0;
                            cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                            cell.SetCellValue("系所合計：");
                            cell.CellStyle = right2CellStyle;
                        }
                        #endregion

                        #region 收入科目金額 系所合計 資料
                        double rowSumAmount = 0;    //各收入科目系所合計的總計
                        {
                            int colIndex = 1;
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
                                    #region [MDY:20210401] 原碼修正
                                    DataRow[] drDatas = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Major_Id='{6}' AND Receive_Item_Key = '{7}' AND Receive_Item_Amount = {8} AND CLASS_FLAG = 'N'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", ""), majorId.Replace("'", ""), receiveItemKey.Replace("'", ""), receiveItemValue));
                                    #endregion

                                    if (drDatas != null && drDatas.Length > 0)
                                    {
                                        double sumAmount = Convert.ToDouble(drDatas[0]["Sum_Amount"]);
                                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                        cell.SetCellValue(sumAmount);
                                        cell.CellStyle = money2CellStyle;
                                        this.CreatEmptyColumnCellAndMerge(cell, 1); //每個收入科目金額 2 格
                                        rowSumAmount += sumAmount;
                                    }
                                }
                                colIndex += 2;
                            }
                        }
                        #endregion

                        #region 總計
                        {
                            cell = (HSSFCell)row.CreateCell(rowSumColIndex, CellType.Numeric);
                            cell.SetCellValue(rowSumAmount);
                            cell.CellStyle = moneyCellStyle;
                            this.CreatEmptyColumnCellAndMerge(cell, 1); //總計 2 格
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            #endregion

            #region Data Row (部別合計)
            {
                rowIdx++;
                row = (HSSFRow)sheet.CreateRow(rowIdx);

                #region 部別合計
                {
                    int colIndex = 0;
                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                    cell.SetCellValue("部別合計：");
                    cell.CellStyle = right2CellStyle;
                }
                #endregion

                #region 收入科目金額 部別合計 資料
                double rowSumAmount = 0;    //各收入科目部別合計的總計
                {
                    int colIndex = 1;
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
                            #region [MDY:20210401] 原碼修正
                            DataRow[] drDatas = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Dept_Id='{5}' AND Receive_Item_Key = '{6}' AND Receive_Item_Amount = {7} AND MAJOR_FLAG = 'N' AND CLASS_FLAG = 'N'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), deptId.Replace("'", ""), receiveItemKey.Replace("'", ""), receiveItemValue));
                            #endregion

                            if (drDatas != null && drDatas.Length > 0)
                            {
                                double sumAmount = Convert.ToDouble(drDatas[0]["Sum_Amount"]);
                                cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                cell.SetCellValue(sumAmount);
                                cell.CellStyle = money2CellStyle;
                                this.CreatEmptyColumnCellAndMerge(cell, 1); //每個收入科目金額 2 格
                                rowSumAmount += sumAmount;
                            }
                        }
                        colIndex += 2;
                    }
                }
                #endregion

                #region 總計
                {
                    cell = (HSSFCell)row.CreateCell(rowSumColIndex, CellType.Numeric);
                    cell.SetCellValue(rowSumAmount);
                    cell.CellStyle = moneyCellStyle;
                    this.CreatEmptyColumnCellAndMerge(cell, 1); //總計 2 格
                }
                #endregion
            }
            #endregion

            if (isLastPage)
            {
                #region Data Row (總合計)
                {
                    rowIdx++;
                    row = (HSSFRow)sheet.CreateRow(rowIdx);

                    #region 總合計
                    {
                        int colIndex = 0;
                        cell = (HSSFCell)row.CreateCell(colIndex, CellType.String);
                        cell.SetCellValue("總 合 計：");
                        cell.CellStyle = right2CellStyle;
                    }
                    #endregion

                    #region 收入科目金額 部別合計 資料
                    double rowSumAmount = 0;    //各收入科目總合計的總計
                    {
                        int colIndex = 1;
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
                                #region [MDY:20210401] 原碼修正
                                DataRow[] drDatas = dtData.Select(String.Format("Receive_Type='{0}' AND Year_Id='{1}' AND Term_Id='{2}' AND Dep_Id='{3}' AND Receive_Id='{4}' AND Receive_Item_Key = '{5}' AND Receive_Item_Amount = {6} AND DEPT_FLAG = 'N' AND MAJOR_FLAG = 'N' AND CLASS_FLAG = 'N'", receiveType.Replace("'", ""), yearId.Replace("'", ""), termId.Replace("'", ""), depId.Replace("'", ""), receiveId.Replace("'", ""), receiveItemKey.Replace("'", ""), receiveItemValue));
                                #endregion

                                if (drDatas != null && drDatas.Length > 0)
                                {
                                    double sumAmount = Convert.ToDouble(drDatas[0]["Sum_Amount"]);
                                    cell = (HSSFCell)row.CreateCell(colIndex, CellType.Numeric);
                                    cell.SetCellValue(sumAmount);
                                    cell.CellStyle = money2CellStyle;
                                    this.CreatEmptyColumnCellAndMerge(cell, 1); //每個收入科目金額 2 格
                                    rowSumAmount += sumAmount;
                                }
                            }
                            colIndex += 2;
                        }
                    }
                    #endregion

                    #region 總計
                    {
                        cell = (HSSFCell)row.CreateCell(rowSumColIndex, CellType.Numeric);
                        cell.SetCellValue(rowSumAmount);
                        cell.CellStyle = moneyCellStyle;
                        this.CreatEmptyColumnCellAndMerge(cell, 1); //總計 2 格
                    }
                    #endregion
                }
                #endregion
            }

            return rowIdx;
        }

        #endregion
        #endregion
    }
}
