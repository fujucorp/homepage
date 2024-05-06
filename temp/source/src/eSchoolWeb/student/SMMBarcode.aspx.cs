using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.Configuration;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.student
{
    public partial class SMMBarcode : LocalizedPage
    {
        #region [MDY:20191014] M201910_01 (2019擴充案+小修正) Override LocalizedPage's IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "SMMBarcode";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "行動版超商條碼";
            }
        }

        /// <summary>
        /// 取得是否為編輯頁面
        /// </summary>
        public override bool IsEditPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得是否為延伸頁面
        /// </summary>
        public override bool IsSubPage
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 取得選單(功能)代碼是否符合命名規則
        /// </summary>
        public override bool IsMatchMenuID
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region [MDY:20191014] M201910_01 (2019擴充案+小修正) 處理 WebLog 相關
        /// <summary>
        /// Request 時間
        /// </summary>
        public WebLogEntity WebLog
        {
            get;
            set;
        }

        #region Override Page Method
        protected override void OnLoad(EventArgs e)
        {
            if (!this.IsPostBack)
            {
                LogonUser logonUser = WebHelper.GetLogonUser();
                this.WebLog = new WebLogEntity()
                {
                    TaskNo = Guid.NewGuid().ToString().ToUpper(),
                    RequestId = this.MenuID,
                    RequestTime = DateTime.Now,
                    WebMachine = Environment.MachineName,
                    SessionId = Session.SessionID,
                    ClientIp = Request.UserHostAddress,
                    UserUnitKind = logonUser.UserQual,
                    UserUnitId = logonUser.UnitId,
                    UserLoginId = logonUser.UserId,
                };
            }

            base.OnLoad(e);
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);

            #region 處理網頁日誌
            if (this.WebLog != null && !String.IsNullOrEmpty(this.WebLog.RequestKind))
            {
                this.WebLog.LogTime = DateTime.Now;
                XmlResult xmlResult = null;
                try
                {
                    if (!this.WebLog.ResponseTime.HasValue)
                    {
                        this.WebLog.ResponseTime = this.WebLog.LogTime;
                    }
                    int count = 0;
                    xmlResult = DataProxy.Current.Insert<WebLogEntity>(this.Page, this.WebLog, out count);
                }
                catch (Exception ex)
                {
                    xmlResult = new XmlResult(false, "新增網站日誌資料發生例外。" + ex.Message, BaseStatusCode.UNKNOWN_EXCEPTION);
                }
                finally
                {
                    #region 失敗寫 Log File
                    if (xmlResult != null && !xmlResult.IsSuccess)
                    {
                        string logPath = System.Configuration.ConfigurationManager.AppSettings.Get("LOG_PATH");
                        if (!String.IsNullOrEmpty(logPath))
                        {
                            try
                            {
                                string xmlWebLog = null;
                                Fuju.Common.TryToXmlExplicitly<WebLogEntity>(this.WebLog, out xmlWebLog);

                                string logFileName = String.Format("WebLogFail_{0:yyyyMMdd}.log", this.WebLog.LogTime);
                                string logFileFullName = System.IO.Path.Combine(logPath, logFileName);
                                StringBuilder sb = new StringBuilder();
                                sb
                                    .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 處理網頁日誌失敗。", this.WebLog.LogTime).AppendLine()
                                    .AppendFormat("  錯誤代碼：{0}", xmlResult.Code).AppendLine()
                                    .AppendFormat("  錯誤訊息：{0}", xmlResult.Message).AppendLine();
                                if (!String.IsNullOrEmpty(xmlWebLog))
                                {
                                    sb.AppendLine("  網頁日誌：").AppendLine(xmlWebLog).AppendLine();
                                }
                                System.IO.File.AppendAllText(logFileFullName, sb.ToString());
                            }
                            catch
                            {
                            }
                        }
                    }
                    #endregion
                }
            }
            #endregion
        }
        #endregion
        #endregion

        #region Private Method
        private void ShowWebArgumentError()
        {
            string msg = this.GetLocalized("網頁參數不正確");
            this.ShowSystemMessage(msg);

            #region [MDY:20191014] M201910_01 (2019擴充案+小修正) 紀錄網頁日誌的 Response & Status 相關資訊
            if (this.WebLog != null)
            {
                this.WebLog.ResponseTime = DateTime.Now;
                this.WebLog.ResponseData = msg;

                this.WebLog.StatusCode = BaseStatusCode.INVALID_PARAMETER;
                this.WebLog.StatusMessage = String.Empty;
            }
            #endregion
        }

        /// <summary>
        /// 產生超商條碼圖
        /// </summary>
        /// <param name="barcode1"></param>
        /// <param name="barcode2"></param>
        /// <param name="barcode3"></param>
        /// <param name="imgBuffer"></param>
        /// <returns></returns>
        private string GenSMBarcodeImage(string barcode1, string barcode2, string barcode3, out byte[] imgBuffer)
        {
            imgBuffer = null;

            #region [MDY:20220410] Checkmarx 調整
            try
            {
                #region Barcode 用變數
                const char code39Symbol = '*';  //起始與結束符號 *
                char[] code39Symbols = { code39Symbol };
                const string code39SymbolMap = "010010100";  //起始與結束符號 * 的 Map

                //Code39 的各字母
                const string code39Text = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*";

                //Code39 的各字母對應碼 (0=白線; 1=黑線)
                string[] code39Maps = {
                "000110100", // 0
                "100100001", // 1
                "001100001", // 2
                "101100000", // 3
                "000110001", // 4
                "100110000", // 5
                "001110000", // 6
                "000100101", // 7
                "100100100", // 8
                "001100100", // 9
                "100001001", // A
                "001001001", // B
                "101001000", // C
                "000011001", // D
                "100011000", // E
                "001011000", // F
                "000001101", // G
                "100001100", // H
                "001001100", // I
                "000011100", // J
                "100000011", // K
                "001000011", // L
                "101000010", // M
                "000010011", // N
                "100010010", // O
                "001010010", // P
                "000000111", // Q
                "100000110", // R
                "001000110", // S
                "000010110", // T
                "110000001", // U
                "011000001", // V
                "111000000", // W
                "010010001", // X
                "110010000", // Y
                "011010000", // Z
                "010000101", // -
                "110000100", // .
                "011000100", // ' '
                "010101000", // $
                "010100010", // /
                "010001010", // +
                "000101010", // %
                "010010100"  // *
            };
                #endregion

                barcode1 = barcode1.ToUpper().Trim(code39Symbols);
                barcode2 = barcode2.ToUpper().Trim(code39Symbols);
                barcode3 = barcode3.ToUpper().Trim(code39Symbols);

                #region 將條碼轉成對應碼並檢查是否有非法字元
                string barcode1Map = null;
                {
                    StringBuilder map = new StringBuilder();
                    map.Append(code39SymbolMap);  //起始符號 *
                    for (int idx = 0; idx < barcode1.Length; idx++)
                    {
                        char chr = barcode1[idx];
                        int code39MapIndex = code39Text.IndexOf(chr);
                        if (code39MapIndex == -1 || chr == code39Symbol)
                        {
                            return "barcode1 含有非法字元";
                        }
                        map.Append("0").Append(code39Maps[code39MapIndex]); //每個字元用 0 白線間隔
                    }
                    map.Append("0").Append(code39SymbolMap);  //結束符號 *
                    barcode1Map = map.ToString();
                }

                string barcode2Map = null;
                {
                    StringBuilder map = new StringBuilder();
                    map.Append(code39SymbolMap);  //起始符號 *
                    for (int idx = 0; idx < barcode2.Length; idx++)
                    {
                        char chr = barcode2[idx];
                        int code39MapIndex = code39Text.IndexOf(chr);
                        if (code39MapIndex == -1 || chr == code39Symbol)
                        {
                            return "barcode2 含有非法字元";
                        }
                        map.Append("0").Append(code39Maps[code39MapIndex]); //每個字元用 0 白線間隔
                    }
                    map.Append("0").Append(code39SymbolMap);  //結束符號 *
                    barcode2Map = map.ToString();
                }

                string barcode3Map = null;
                {
                    StringBuilder map = new StringBuilder();
                    map.Append(code39SymbolMap);  //起始符號 *
                    for (int idx = 0; idx < barcode3.Length; idx++)
                    {
                        char chr = barcode3[idx];
                        int code39MapIndex = code39Text.IndexOf(chr);
                        if (code39MapIndex == -1 || chr == code39Symbol)
                        {
                            return "barcode3 含有非法字元";
                        }
                        map.Append("0").Append(code39Maps[code39MapIndex]); //每個字元用 0 白線間隔
                    }
                    map.Append("0").Append(code39SymbolMap);  //結束符號 *
                    barcode3Map = map.ToString();
                }
                #endregion

                #region 計算圖的大小
                //最大條碼長度
                int maxBarcodeLength = barcode1.Length > barcode2.Length ? barcode1.Length : barcode2.Length;
                if (maxBarcodeLength < barcode3.Length)
                {
                    maxBarcodeLength = barcode3.Length;
                }

                int paddingLeft = 5;  //左邊界
                int paddingTop = 5;   //上邊界
                int blackWidth = 4;   //最小 BarCode 黑線寬度 (黑線與白線的寬度必須為 2:1)
                int whiteWidth = 2;   //最小 BarCode 白線寬度

                int imgWidth = 360, imgHeight = 300;  //圖的寬度與高度
                int minWidth = (blackWidth * 3 + whiteWidth * 7) * (maxBarcodeLength + 2) + (paddingLeft * 2);  //最小寬度 (註：每一碼由 3 個黑線 1 個白線組成)
                if (imgWidth < minWidth)
                {
                    imgWidth = minWidth;
                }

                int barcodeTextHeigth = SystemFonts.DefaultFont.Height;
                int barcodeSplitHeight = 10;  //條碼間隔高度

                int barcodeHeigth = ((imgHeight - (paddingTop * 2) - (barcodeSplitHeight * 2)) / 3) - barcodeTextHeigth;
                if (barcodeHeigth < 25)
                {
                    barcodeHeigth = 25;
                    imgHeight = ((barcodeHeigth + barcodeTextHeigth) * 3) + (barcodeSplitHeight * 2) + (paddingTop * 2);
                }
                #endregion

                #region 繪圖
                using (Bitmap image = new Bitmap(imgWidth, imgHeight))
                using (Graphics g = Graphics.FromImage(image))
                {
                    //填上底色
                    g.FillRectangle(Brushes.White, 0, 0, imgWidth, imgHeight);

                    int x, y, barWidth;
                    Brush[] brushs = { Brushes.Black, Brushes.White };
                    string barcodeText = null;
                    SizeF sizef;

                    #region barcode1Map
                    x = paddingLeft;
                    y = paddingTop;
                    for (int idx = 0; idx < barcode1Map.Length; idx++) //依碼畫出Code39 BarCode
                    {
                        barWidth = barcode1Map[idx] == '1' ? blackWidth : whiteWidth;
                        g.FillRectangle(brushs[idx % 2], x, y, barWidth, barcodeHeigth);
                        x += barWidth;
                    }
                    barcodeText = "*" + barcode1 + "*";
                    sizef = g.MeasureString(barcodeText, SystemFonts.DefaultFont);
                    g.DrawString(barcodeText, SystemFonts.DefaultFont, Brushes.Black, (x - sizef.Width) / 2, y + barcodeHeigth);
                    #endregion

                    #region barcode2Map
                    x = paddingLeft;
                    y += barcodeHeigth + barcodeTextHeigth + barcodeSplitHeight;
                    for (int idx = 0; idx < barcode2Map.Length; idx++) //依碼畫出Code39 BarCode
                    {
                        barWidth = barcode2Map[idx] == '1' ? blackWidth : whiteWidth;
                        g.FillRectangle(brushs[idx % 2], x, y, barWidth, barcodeHeigth);
                        x += barWidth;
                    }
                    barcodeText = "*" + barcode2 + "*";
                    sizef = g.MeasureString(barcodeText, SystemFonts.DefaultFont);
                    g.DrawString(barcodeText, SystemFonts.DefaultFont, Brushes.Black, (x - sizef.Width) / 2, y + barcodeHeigth);
                    #endregion

                    #region barcode3Map
                    x = paddingLeft;
                    y += barcodeHeigth + barcodeTextHeigth + barcodeSplitHeight;
                    for (int idx = 0; idx < barcode3Map.Length; idx++) //依碼畫出Code39 BarCode
                    {
                        barWidth = barcode3Map[idx] == '1' ? blackWidth : whiteWidth;
                        g.FillRectangle(brushs[idx % 2], x, y, barWidth, barcodeHeigth);
                        x += barWidth;
                    }
                    barcodeText = "*" + barcode3 + "*";
                    sizef = g.MeasureString(barcodeText, SystemFonts.DefaultFont);
                    g.DrawString(barcodeText, SystemFonts.DefaultFont, Brushes.Black, (x - sizef.Width) / 2, y + barcodeHeigth);
                    #endregion

                    #region 轉成 byte[]
                    using (MemoryStream ms = new MemoryStream())
                    {
                        //將圖像保存到指定的流
                        image.Save(ms, ImageFormat.Jpeg);
                        imgBuffer = ms.ToArray();
                    }
                    #endregion
                }
                #endregion

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            #endregion
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region 取得登入者資料
                LogonUser user = WebHelper.GetLogonUser();
                if (user == null)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("無法取得登入者資訊");
                    this.ShowSystemMessage(msg);
                    return;
                }
                #endregion

                #region 處理參數
                {
                    KeyValueList<string> QueryString = Session["QueryString"] as KeyValueList<string>;
                    if (QueryString == null || QueryString.Count == 0)
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("缺少網頁參數");
                        this.ShowSystemMessage(msg);
                        return;
                    }

                    #region 學號
                    string stuId = QueryString.TryGetValue("StuId", null);
                    if (String.IsNullOrWhiteSpace(stuId))
                    {
                        this.ShowWebArgumentError();
                        return;
                    }
                    #endregion

                    #region 姓名
                    string stuName = QueryString.TryGetValue("StuName", null);
                    if (String.IsNullOrWhiteSpace(stuName))
                    {
                        this.ShowWebArgumentError();
                        return;
                    }
                    #endregion

                    #region 超商繳費期限 (應繳日期)
                    DateTime? smPayDueDate = DataFormat.ConvertDateText(QueryString.TryGetValue("SMPayDueDate", null));
                    if (!smPayDueDate.HasValue)
                    {
                        this.ShowWebArgumentError();
                        return;
                    }
                    #endregion

                    #region 超商延遲天數
                    Int32 smExtraDays = 0;
                    if (!Int32.TryParse(QueryString.TryGetValue("SMExtraDays", null), out smExtraDays) || smExtraDays < 0)
                    {
                        this.ShowWebArgumentError();
                        return;
                    }
                    #endregion

                    #region 超商最後期限 (繳費期限)
                    DateTime lastSMPayDueDate = smExtraDays > 0 ? smPayDueDate.Value.AddDays(smExtraDays) : smPayDueDate.Value;
                    #endregion

                    #region 超商行動版手序費代碼
                    string smmBarcodeId = QueryString.TryGetValue("SMMBarcodeId", null);
                    if (String.IsNullOrWhiteSpace(smmBarcodeId) || smmBarcodeId.Length != 3)
                    {
                        this.ShowWebArgumentError();
                        return;
                    }
                    #endregion

                    #region 超商行動版手序費金額
                    Int32 smmCharge = 0;
                    if (!Int32.TryParse(QueryString.TryGetValue("SMMCharge", null), out smmCharge) || smmCharge < 0)
                    {
                        this.ShowWebArgumentError();
                        return;
                    }
                    #endregion

                    #region 虛擬帳號
                    string cancelNo = QueryString.TryGetValue("CancelNo", null);
                    if (!Common.IsNumber(cancelNo) || (cancelNo.Length != 14 && cancelNo.Length != 16))
                    {
                        this.ShowWebArgumentError();
                        return;
                    }
                    #endregion

                    #region 繳費金額
                    Decimal receiveAmount = 0M;
                    if (!Decimal.TryParse(QueryString.TryGetValue("ReceiveAmount", null), out receiveAmount) || receiveAmount < 0)
                    {
                        this.ShowWebArgumentError();
                        return;
                    }
                    #endregion

                    #region [MDY:20191014] M201910_01 (2019擴充案+小修正) 紀錄網頁日誌的 Request 相關資訊
                    if (this.WebLog != null)
                    {
                        this.WebLog.RequestKind = WebLogRequestKindCodeTexts.VIEW_CODE;
                        this.WebLog.RequestDesc = this.MenuName;
                        this.WebLog.IndexCancelNo = cancelNo;
                        if ((cancelNo.Length == 14 || cancelNo.Length == 16) && Common.IsNumber(cancelNo))
                        {
                            this.WebLog.IndexReceiveType = cancelNo.Substring(0, 4);
                        }
                        this.WebLog.RequestArgs = String.Format("StuId={0}; StuName={1}; SMPayDueDate={2:yyyy/MM/dd}; SMExtraDays={3}; SMMBarcodeId={4}; SMMCharge={5}; CancelNo={6}; ReceiveAmount={7}", stuId, stuName, smPayDueDate, smExtraDays, smmBarcodeId, smmCharge, cancelNo, receiveAmount);
                    }
                    #endregion

                    this.labStuId.Text = Server.HtmlEncode(stuId);
                    this.labStuName.Text = Server.HtmlEncode(DataFormat.MaskText(stuName, DataFormat.MaskDataType.Name));
                    this.labSMPayDueDate.Text = DataFormat.GetDateText(smPayDueDate.Value);
                    this.labReceiveAmount.Text = DataFormat.GetAmountCommaText(receiveAmount);
                    this.labCancelNo.Text = Server.HtmlEncode(cancelNo);

                    #region 產生圖檔 CODE-39
                    {
                        ChannelHelper helper = new ChannelHelper();
                        string barcode1 = helper.GenSMBarcode1(lastSMPayDueDate, smmBarcodeId);
                        string barcode2 = helper.GenSMBarcode2(cancelNo);
                        string barcode3 = helper.GenSMBarcode3(barcode1, barcode2, smPayDueDate.Value, receiveAmount);

                        byte[] imgBuffer = null;
                        string errmsg = GenSMBarcodeImage(barcode1, barcode2, barcode3, out imgBuffer);
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            this.imgSMMBarcode.ImageUrl = "data:image/.jpeg;base64," + Convert.ToBase64String(imgBuffer);

                            #region [MDY:20191014] M201910_01 (2019擴充案+小修正) 紀錄網頁日誌的 Response & Status 相關資訊
                            if (this.WebLog != null)
                            {
                                this.WebLog.ResponseTime = DateTime.Now;
                                this.WebLog.ResponseData = String.Format("條碼 1={0}; 條碼 2={1}; 條碼 3={2};", barcode1, barcode2, barcode3);

                                this.WebLog.StatusCode = BaseStatusCode.NORMAL_STATUS;
                                this.WebLog.StatusMessage = String.Empty;
                            }
                            #endregion
                        }
                        else
                        {
                            this.ShowJsAlert(errmsg);

                            #region [MDY:20191014] M201910_01 (2019擴充案+小修正) 紀錄網頁日誌的 Response & Status 相關資訊
                            if (this.WebLog != null)
                            {
                                this.WebLog.ResponseTime = DateTime.Now;
                                this.WebLog.ResponseData = errmsg;

                                this.WebLog.StatusCode = BaseStatusCode.UNKNOWN_ERROR;
                                this.WebLog.StatusMessage = errmsg;
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                #endregion
            }
        }
    }
}