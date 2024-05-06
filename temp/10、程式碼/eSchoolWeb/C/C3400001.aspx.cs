using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

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

namespace eSchoolWeb.C
{
    /// <summary>
    /// 每日銷帳結果查詢
    /// </summary>
    public partial class C3400001 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack)
            {
                this.litResult.Text = String.Empty;
            }
        }

        #region [MDY:2018xxxx] 提供匯出XLS功能
        /// <summary>
        /// 匯出資料
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="sAccountDate"></param>
        /// <param name="eAccountDate"></param>
        private void ExportData(string receiveType, string yearId, string termId, string depId, string receiveId, DateTime sAccountDate, DateTime eAccountDate)
        {
            string action = ActionMode.GetActionLocalized("匯出");

            CancelResultEntity[] datas = null;
            XmlResult xmlResult = DataProxy.Current.GetC3400001Result(this.Page, receiveType, yearId, termId, depId, receiveId, sAccountDate, eAccountDate, out datas);
            if (!xmlResult.IsSuccess)
            {
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                return;
            }

            if (datas == null || datas.Length == 0)
            {
                this.litResult.Text = "查無資料";
                return;
            }

            List<decimal> totalCounts = new List<decimal>();
            List<decimal> totalAmounts = new List<decimal>();

            CancelResultEntity firstData = datas[0];
            string fileName = String.Format("{0}_{1}{2}{3}_銷帳結果.XLS"
                , receiveType
                , yearId
                , termId
                , (String.IsNullOrEmpty(receiveId) ? String.Empty : firstData.ReceiveName));
            string sheetName = String.Format("{0:yyyyMMdd}~{1:yyyyMMdd}_銷帳結果.XLS", sAccountDate, eAccountDate);

            try
            {
                HSSFWorkbook wb = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)wb.CreateSheet(sheetName);
                HSSFCell cell = null;
                int rowIdx = 0, colIdx = 0;

                #region Decimal 格式化用變數
                Type decimalType = typeof(System.Decimal);
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
                        cell.SetCellValue(accountDate == null ? String.Empty : DataFormat.GetDateText(accountDate.Value));
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

                byte[] fileContent = null;
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(1024 * 1024 * 64))
                {
                    wb.Write(ms);
                    ms.Flush();
                    fileContent = ms.ToArray();
                }
                GC.Collect();

                this.ResponseFile(HttpUtility.HtmlEncode(fileName), fileContent, "XLS");
            }
            catch (Exception ex)
            {
                this.ShowActionFailureMessage(action, ex.Message);
                return;
            }
        }
        #endregion

        private void GetAndBindData(string receiveType, string yearId, string termId, string depId, string receiveId, DateTime sAccountDate, DateTime eAccountDate)
        {
            this.litResult.Text = String.Empty;

            CancelResultEntity[] datas = null;
            XmlResult xmlResult = DataProxy.Current.GetC3400001Result(this.Page, receiveType, yearId, termId, depId, receiveId, sAccountDate, eAccountDate, out datas);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                return;
            }

            if (datas == null || datas.Length == 0)
            {
                this.litResult.Text = "查無資料";
                return;
            }

            StringBuilder html = new StringBuilder();
            decimal count = 0;
            decimal amount = 0M;
			List<decimal> totalCounts = new List<decimal>();
			List<decimal> totalAmounts = new List<decimal>();
            int idx = 0;
            html.AppendLine("<table id=\"result\" class=\"result\" summary=\"查詢結果\" width=\"100%\">");
            //html.AppendLine("<tr><td>");

            #region
            //html.AppendLine("<table width=\"100%\" >");

            #region 學年
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">學年</td>");
            foreach (CancelResultEntity data in datas)
            {
                html.AppendFormat("<td style=\"min-width:100px;\">{0}</td>", data.YearName);

                //初始化 total 
                totalCounts.Add(0);
                totalAmounts.Add(0);
            }
            html.AppendLine("<td style=\"min-width:100px;\">合計</td>");
            html.AppendLine("</tr>");
            #endregion

            #region 學期
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">學期</td>");
            foreach (CancelResultEntity data in datas)
            {
                html.AppendFormat("<td style=\"min-width:100px;\">{0}</td>", data.TermName);
            }
            html.AppendLine("<td style=\"min-width:100px;\">&nbsp;</td>");
            html.AppendLine("</tr>");
            #endregion

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //#region 部別
            //html.AppendLine("<tr>");
            //html.AppendLine("<td style=\"min-width:100px;\">部別</td>");
            //foreach (CancelResultEntity data in datas)
            //{
            //    html.AppendFormat("<td style=\"min-width:100px;\">{0}</td>", data.DepName);
            //}
            //html.AppendLine("<td style=\"min-width:100px;\">&nbsp;</td>");
            //html.AppendLine("</tr>");
            //#endregion
            #endregion

            #region 費用別
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">費用別</td>");
            foreach (CancelResultEntity data in datas)
            {
                html.AppendFormat("<td style=\"min-width:100px;\">{0}</td>", data.ReceiveName);
            }
            html.AppendLine("<td style=\"min-width:100px;\">&nbsp;</td>");
            html.AppendLine("</tr>");
            #endregion

            #region 入帳日
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">入帳日</td>");
            foreach (CancelResultEntity data in datas)
            {
                DateTime? accountDate = DataFormat.ConvertDateText(data.AccountDate);
                html.AppendFormat("<td style=\"min-width:100px;\">{0}</td>", accountDate == null ? String.Empty : DataFormat.GetDateText(accountDate.Value));
            }
            html.AppendLine("<td style=\"min-width:100px;\">&nbsp;</td>");
            html.AppendLine("</tr>");
            #endregion

            #region 學校自收
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">學校自收筆數</td>");
            count = 0;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                count += data.SoCount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", data.SoCount);

                totalCounts[idx] += data.SoCount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            html.AppendLine("</tr>");

            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">學校自收金額</td>");
            amount = 0M;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                amount += data.SoAmount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(data.SoAmount));

                totalAmounts[idx] += data.SoAmount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            html.AppendLine("</tr>");
            #endregion

            //html.AppendLine("</table>");
            #endregion

            //html.AppendLine("</td></tr>");
            //html.AppendLine("<tr><td>");

            #region
            //html.AppendLine("<table width=\"100%\" >");

            #region 統一
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">統一代收筆數</td>");
            count = 0;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                count += data.GCount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", data.GCount);

                totalCounts[idx] += data.GCount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            html.AppendLine("</tr>");

            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">統一代收金額</td>");
            amount = 0M;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                amount += data.GAmount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(data.GAmount));

                totalAmounts[idx] += data.GAmount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            html.AppendLine("</tr>");
            #endregion

            #region 全家
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">全家代收筆數</td>");
            count = 0;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                count += data.DCount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", data.DCount);

                totalCounts[idx] += data.DCount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            html.AppendLine("</tr>");

            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">全家代收金額</td>");
            amount = 0M;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                amount += data.DAmount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(data.DAmount));

                totalAmounts[idx] += data.DAmount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            html.AppendLine("</tr>");
            #endregion

            #region 萊爾富
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">萊爾富代收筆數</td>");
            count = 0;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                count += data.NCount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", data.NCount);

                totalCounts[idx] += data.NCount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            html.AppendLine("</tr>");

            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">萊爾富代收金額</td>");
            amount = 0M;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                amount += data.NAmount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", DataFormat.GetAmountCommaText(data.NAmount));

                totalAmounts[idx] += data.NAmount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            html.AppendLine("</tr>");
            #endregion

            #region ＯＫ
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">ＯＫ代收筆數</td>");
            count = 0;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                count += data.JCount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", data.JCount);

                totalCounts[idx] += data.JCount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            html.AppendLine("</tr>");

            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">ＯＫ代收金額</td>");
            amount = 0M;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                amount += data.JAmount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(data.JAmount));

                totalAmounts[idx] += data.JAmount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            html.AppendLine("</tr>");
            #endregion

            #region 中信平台
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">中信平台代收筆數</td>");
            count = 0;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                count += data.WCount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", data.WCount);

                totalCounts[idx] += data.WCount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            html.AppendLine("</tr>");

            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">中信平台代收金額</td>");
            amount = 0M;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                amount += data.WAmount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(data.WAmount));

                totalAmounts[idx] += data.WAmount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            html.AppendLine("</tr>");
            #endregion

            #region 財金
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">財金代收筆數</td>");
            count = 0;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                count += data.KCount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", data.KCount);

                totalCounts[idx] += data.KCount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            html.AppendLine("</tr>");

            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">財金代收金額</td>");
            amount = 0M;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                amount += data.KAmount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(data.KAmount));

                totalAmounts[idx] += data.KAmount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            html.AppendLine("</tr>");
            #endregion

            #region [Old] 語音銀行
            //html.AppendLine("<tr>");
            //html.AppendLine("<td style=\"min-width:100px;\">語音銀行代收筆數</td>");
            //count = 0;
            //idx = 0;
            //foreach (CancelResultEntity data in datas)
            //{
            //    count += data.BCount;
            //    html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", data.BCount);

            //    totalCounts[idx] += data.BCount;
            //    idx++;
            //}
            //html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            //html.AppendLine("</tr>");

            //html.AppendLine("<tr>");
            //html.AppendLine("<td style=\"min-width:100px;\">語音銀行代收金額</td>");
            //amount = 0M;
            //idx = 0;
            //foreach (CancelResultEntity data in datas)
            //{
            //    amount += data.BAmount;
            //    html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(data.BAmount));

            //    totalAmounts[idx] += data.BAmount;
            //    idx++;
            //}
            //html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            //html.AppendLine("</tr>");
            #endregion

            #region ＡＴＭ
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">ＡＴＭ代收筆數</td>");
            count = 0;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                count += data.ACount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", data.ACount);

                totalCounts[idx] += data.ACount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            html.AppendLine("</tr>");

            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">ＡＴＭ代收金額</td>");
            amount = 0M;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                amount += data.AAmount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(data.AAmount));

                totalAmounts[idx] += data.AAmount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            html.AppendLine("</tr>");
            #endregion

            #region 網路銀行
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">網路銀行代收筆數</td>");
            count = 0;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                count += data.ICount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", data.ICount);

                totalCounts[idx] += data.ICount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            html.AppendLine("</tr>");

            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">網路銀行代收金額</td>");
            amount = 0M;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                amount += data.IAmount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(data.IAmount));

                totalAmounts[idx] += data.IAmount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            html.AppendLine("</tr>");
            #endregion

            #region 臨櫃
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">臨櫃代收筆數</td>");
            count = 0;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                count += data.CmfCount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", data.CmfCount);

                totalCounts[idx] += data.CmfCount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            html.AppendLine("</tr>");

            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">臨櫃代收金額</td>");
            amount = 0M;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                amount += data.CmfAmount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(data.CmfAmount));

                totalAmounts[idx] += data.CmfAmount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            html.AppendLine("</tr>");
            #endregion

            #region 匯款
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">匯款代收筆數</td>");
            count = 0;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                count += data.HCount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", data.HCount);

                totalCounts[idx] += data.HCount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            html.AppendLine("</tr>");

            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">匯款代收金額</td>");
            amount = 0M;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                amount += data.HAmount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(data.HAmount));

                totalAmounts[idx] += data.HAmount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            html.AppendLine("</tr>");
            #endregion

            #region 支付寶 (C09)
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">支付寶代收筆數</td>");
            count = 0;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                count += data.C09Count;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", data.C09Count);

                totalCounts[idx] += data.C09Count;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            html.AppendLine("</tr>");

            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">支付寶代收金額</td>");
            amount = 0M;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                amount += data.C09Amount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(data.C09Amount));

                totalAmounts[idx] += data.C09Amount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            html.AppendLine("</tr>");
            #endregion

            #region [MDY:20171127] 增加全國繳費網 (C08) (20170831_01)
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">全國繳費網代收筆數</td>");
            count = 0;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                count += data.C08Count;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", data.C08Count);

                totalCounts[idx] += data.C08Count;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            html.AppendLine("</tr>");

            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">全國繳費網代收金額</td>");
            amount = 0M;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                amount += data.C08Amount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(data.C08Amount));

                totalAmounts[idx] += data.C08Amount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            html.AppendLine("</tr>");
            #endregion

            #region [MDY:20171127] 增加台灣Pay (C10) (20170831_01)
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">台灣Pay代收筆數</td>");
            count = 0;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                count += data.C10Count;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", data.C10Count);

                totalCounts[idx] += data.C10Count;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            html.AppendLine("</tr>");

            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">台灣Pay代收金額</td>");
            amount = 0M;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                amount += data.C10Amount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(data.C10Amount));

                totalAmounts[idx] += data.C10Amount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            html.AppendLine("</tr>");
            #endregion

            #region [MDY:20191214] (2019擴充案) 國際信用卡
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">國際信用卡代收筆數</td>");
            count = 0;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                count += data.NCCount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", data.NCCount);

                totalCounts[idx] += data.NCCount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            html.AppendLine("</tr>");

            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">國際信用卡代收金額</td>");
            amount = 0M;
            idx = 0;
            foreach (CancelResultEntity data in datas)
            {
                amount += data.NCAmount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(data.NCAmount));

                totalAmounts[idx] += data.NCAmount;
                idx++;
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            html.AppendLine("</tr>");
            #endregion

            //html.AppendLine("</table>");
            #endregion

            //html.AppendLine("</td></tr>");
            //html.AppendLine("<tr><td>");

            #region
            //html.AppendLine("<table width=\"100%\" >");

            #region 合計
            //html.AppendLine("<table width=\"100%\" >");
            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">合計筆數</td>");
            count = 0;
            foreach (decimal totalCount in totalCounts)
            {
                count += totalCount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", totalCount);
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0:0}</td>", count);
            html.AppendLine("</tr>");

            html.AppendLine("<tr>");
            html.AppendLine("<td style=\"min-width:100px;\">合計金額</td>");
            amount = 0M;
            foreach (decimal totalAmount in totalAmounts)
            {
                amount += totalAmount;
                html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(totalAmount));
            }
            html.AppendFormat("<td style=\"min-width:100px; text-align:right;\">{0}</td>", DataFormat.GetAmountCommaText(amount));
            html.AppendLine("</tr>");
            #endregion

            //html.AppendLine("</table>");
            #endregion

            //html.AppendLine("</td></tr>");
            html.AppendLine("</table>");
            this.litResult.Text = html.ToString();
        }

        protected void ccbtnQuery_Click(object sender, EventArgs e)
        {
            #region 檢查查詢權限
            if (!this.HasQueryAuth())
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無查詢權限");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            string receiveType = ucFilter1.SelectedReceiveType;
            if (String.IsNullOrEmpty(receiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return;
            }

            string yearId = ucFilter1.SelectedYearID;
            string termId = ucFilter1.SelectedTermID;

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //string depId = ucFilter1.SelectedDepID;
            #endregion

            string depId = String.Empty;
            string receiveId = ucFilter2.SelectedReceiveID;

            WebHelper.SetFilterArguments(receiveType, yearId, termId, depId, receiveId);

            string sDate = this.tbxSAccountDate.Text.Trim();
            if (String.IsNullOrEmpty(sDate))
            {
                this.ShowMustInputAlert("入帳日期區間的起日");
                return;
            }
            DateTime sAccountDate;
            if (!DateTime.TryParse(sDate, out sAccountDate))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("入帳日期區間的起日不是有效的日期");
                this.ShowJsAlert(msg);
                return;
            }
            string eDate = this.tbxEAccountDate.Text.Trim();
            if (String.IsNullOrEmpty(eDate))
            {
                this.ShowMustInputAlert("入帳日期區間的迄日");
                return;
            }
            DateTime eAccountDate;
            if (!DateTime.TryParse(eDate, out eAccountDate))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("入帳日期區間的迄日不是有效的日期");
                this.ShowJsAlert(msg);
                return;
            }

            this.GetAndBindData(receiveType, yearId, termId, depId, receiveId, sAccountDate, eAccountDate);
        }

        protected void ccbtnExport_Click(object sender, EventArgs e)
        {
            this.litResult.Text = String.Empty;

            #region 檢查查詢權限
            if (!this.HasQueryAuth())
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無查詢權限");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            string receiveType = ucFilter1.SelectedReceiveType;
            if (String.IsNullOrEmpty(receiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return;
            }

            string yearId = ucFilter1.SelectedYearID;
            string termId = ucFilter1.SelectedTermID;
            string depId = String.Empty;
            string receiveId = ucFilter2.SelectedReceiveID;

            //WebHelper.SetFilterArguments(receiveType, yearId, termId, depId, receiveId);

            string sDate = this.tbxSAccountDate.Text.Trim();
            if (String.IsNullOrEmpty(sDate))
            {
                this.ShowMustInputAlert("入帳日期區間的起日");
                return;
            }
            DateTime sAccountDate;
            if (!DateTime.TryParse(sDate, out sAccountDate))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("入帳日期區間的起日不是有效的日期");
                this.ShowJsAlert(msg);
                return;
            }

            string eDate = this.tbxEAccountDate.Text.Trim();
            if (String.IsNullOrEmpty(eDate))
            {
                this.ShowMustInputAlert("入帳日期區間的迄日");
                return;
            }
            DateTime eAccountDate;
            if (!DateTime.TryParse(eDate, out eAccountDate))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("入帳日期區間的迄日不是有效的日期");
                this.ShowJsAlert(msg);
                return;
            }

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
            {
                string fileType = "XLS";
                LinkButton control = sender as LinkButton;
                if (control.CommandArgument == "ODS")
                {
                    fileType = "ODS";
                }

                byte[] fileContent = null;
                XmlResult xmlResult = DataProxy.Current.GetC3400001ResultFile(this.Page, receiveType, yearId, termId, depId, receiveId, sAccountDate, eAccountDate, fileType, out fileContent);
                if (!xmlResult.IsSuccess)
                {
                    string action = ActionMode.GetActionLocalized(ActionMode.Query);
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    return;
                }

                if (fileContent == null || fileContent.Length == 0)
                {
                    this.litResult.Text = "查無資料";
                    return;
                }
                else
                {
                    string fileName = String.Format("{0}_{1}{2}{3}_銷帳結果.{4}"
                        , receiveType
                        , yearId
                        , termId
                        , (String.IsNullOrEmpty(receiveId) ? String.Empty : ucFilter2.SelectedReceiveName)
                        , fileType);
                    this.ResponseFile(fileName, fileContent);
                }
            }
            #endregion
        }
    }
}