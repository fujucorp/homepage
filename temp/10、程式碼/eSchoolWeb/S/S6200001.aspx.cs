using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.S
{
    /// <summary>
    /// KP3預設值維護
    /// </summary>
    public partial class S6200001 : BasePage
    {
        #region Private Method
        private void Intitial()
        {
            #region Bind 特約機構屬性
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("G", "非個人特約機構"),
                    new CodeText("H", "個人特約機構")
                };
                WebHelper.SetDropDownListItems(this.ddlDataItem05, DefaultItem.Kind.Select, false, items, true, false, 0, "");
            }
            #endregion

            #region Bind 特約機構類型
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("1", "第一類"),
                    new CodeText("2", "第二類")
                };
                WebHelper.SetDropDownListItems(this.ddlDataItem09, DefaultItem.Kind.Select, false, items, true, false, 0, "");
            }
            #endregion

            #region Bind 營業型態
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("1", "實體"),
                    new CodeText("2", "網路"),
                    new CodeText("3", "實體及網路")
                };
                WebHelper.SetDropDownListItems(this.ddlDataItem23, DefaultItem.Kind.Select, false, items, true, false, 0, "");
            }
            #endregion

            #region Bind 業務行為
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("1", "境內"),
                    new CodeText("2", "跨境"),
                    new CodeText("3", "境內及跨境"),
                    new CodeText("4", "境外特約機構")
                };
                WebHelper.SetDropDownListItems(this.ddlDataItem26, DefaultItem.Kind.Select, false, items, true, false, 0, "");
            }
            #endregion

            #region Bind 是否受理電子支付帳戶或儲值卡服務
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("1", "境內電子支付帳戶服務"),
                    new CodeText("2", "境外機構支付帳戶服務"),
                    new CodeText("3", "境內儲值卡服務"),
                    new CodeText("4", "境外機構記名儲值卡服務")
                };
                WebHelper.SetDropDownListItems(this.ddlDataItem27, DefaultItem.Kind.Select, false, items, true, false, 0, "");
            }
            #endregion

            #region Bind 是否受理信用卡服務
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("Y", "是"),
                    new CodeText("N", "否")
                };
                WebHelper.SetDropDownListItems(this.ddlDataItem29, DefaultItem.Kind.Select, false, items, true, false, 0, "");
            }
            #endregion

            #region Bind 是否有銷售遞延性商品或服務
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("Y", "是"),
                    new CodeText("N", "否")
                };
                WebHelper.SetDropDownListItems(this.ddlDataItem33, DefaultItem.Kind.Select, false, items, true, false, 0, "");
            }
            #endregion

            #region Bind 是否安裝端末設備
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("Y", "是"),
                    new CodeText("N", "否")
                };
                WebHelper.SetDropDownListItems(this.ddlDataItem34, DefaultItem.Kind.Omit, false, items, true, false, 0, "");
            }
            #endregion

            #region Bind 是否安裝錄影設備
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("Y", "是"),
                    new CodeText("N", "否")
                };
                WebHelper.SetDropDownListItems(this.ddlDataItem35, DefaultItem.Kind.Omit, false, items, true, false, 0, "");
            }
            #endregion

            #region Bind 連鎖店加盟或直營
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("", "非連鎖店"),
                    new CodeText("1", "加盟"),
                    new CodeText("2", "直營"),
                    new CodeText("3", "無法判別加盟或直營")
                };
                WebHelper.SetDropDownListItems(this.ddlDataItem36, DefaultItem.Kind.None, false, items, true, false, 0, "");
            }
            #endregion
        }

        private void GetAndBindData()
        {
            KP3Config kp3Config = null;

            #region 讀取資料
            {
                ConfigEntity configData = null;
                Expression where = new Expression(ConfigEntity.Field.ConfigKey, KP3Config.RootLocalName);
                KeyValueList<OrderByEnum> orderbys = null;
                XmlResult xmlResult = DataProxy.Current.SelectFirst<ConfigEntity>(this.Page, where, orderbys, out configData);
                if (xmlResult.IsSuccess)
                {
                    if (configData == null)
                    {
                        kp3Config = new KP3Config();
                    }
                    else
                    {
                        kp3Config = KP3Config.Create(configData.ConfigValue);
                        if (kp3Config == null)
                        {
                            this.ShowErrorMessage(xmlResult.Code, "缺少KP3參數設定或資料不正確，請先設定");
                            return;
                        }
                    }
                }
                else
                {
                    this.ShowActionFailureMessage(this.GetLocalized("讀取KP3參數設定"), xmlResult.Code, xmlResult.Message);
                    return;
                }
            }
            #endregion

            #region 結繫資料
            {
                #region 報送單位代號
                this.tbxUnit.Text = kp3Config.Unit;
                #endregion

                #region 管理者清單
                this.tbxManagers.Text = kp3Config.Managers;
                #endregion

                #region FTPUrl
                this.tbxFTPUrl.Text = kp3Config.FTPUrl;
                #endregion

                #region FTPAcct
                this.tbxFTPAcct.Text = kp3Config.FTPAcct;
                #endregion

                #region [MDY:20220530] Checkmarx 調整
                #region FTPPXX
                this.tbxFTPPXX.Text = kp3Config.FTPPXX;
                #endregion
                #endregion

                #region 資訊格式代號
                this.tbxHeadItem01.Text = kp3Config.HeadItem01;
                #endregion

                #region 聯絡電話
                this.tbxHeadItem07.Text = kp3Config.HeadItem07;
                #endregion

                #region 聯絡人資訊或訊息
                this.tbxHeadItem08.Text = kp3Config.HeadItem08;
                #endregion

                #region 特約機構屬性
                WebHelper.SetDropDownListSelectedValue(this.ddlDataItem05, kp3Config.DataItem05);
                #endregion

                #region 特約機構類型
                WebHelper.SetDropDownListSelectedValue(this.ddlDataItem09, kp3Config.DataItem09);
                #endregion

                #region 營業型態
                WebHelper.SetDropDownListSelectedValue(this.ddlDataItem23, kp3Config.DataItem23);
                #endregion

                #region 業務行為
                WebHelper.SetDropDownListSelectedValue(this.ddlDataItem26, kp3Config.DataItem26);
                #endregion

                #region 是否受理電子支付帳戶或儲值卡服務
                WebHelper.SetDropDownListSelectedValue(this.ddlDataItem27, kp3Config.DataItem27);
                #endregion

                #region 是否受理信用卡服務
                this.ddlDataItem29.Text = kp3Config.DataItem29;
                #endregion

                #region 是否有銷售遞延性商品或服務
                this.ddlDataItem33.Text = kp3Config.DataItem33;
                #endregion

                #region 是否安裝端末設備
                this.ddlDataItem34.Text = kp3Config.DataItem34;
                #endregion

                #region 是否安裝錄影設備
                this.ddlDataItem35.Text = kp3Config.DataItem35;
                #endregion

                #region 連鎖店加盟或直營
                WebHelper.SetDropDownListSelectedValue(this.ddlDataItem36, kp3Config.DataItem36);
                #endregion
            }
            #endregion
        }

        private bool CheckFTPUrl(string ftpUrl)
        {
            if (String.IsNullOrWhiteSpace(ftpUrl))
            {
                return false;
            }

            Uri uri = null;
            if (ftpUrl.StartsWith("FTPS://", StringComparison.CurrentCultureIgnoreCase))
            {
                return Uri.TryCreate(ftpUrl.Replace("FTPS://", "ftp://"), UriKind.Absolute, out uri);
            }
            else if (ftpUrl.StartsWith("SFTP://", StringComparison.CurrentCultureIgnoreCase))
            {
                return Uri.TryCreate(ftpUrl.Replace("SFTP://", "ftp://"), UriKind.Absolute, out uri);
            }
            else
            {
                return Uri.TryCreate(ftpUrl, UriKind.Absolute, out uri);
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (!this.HasMaintainAuth())
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                    this.ccbtnOK.Visible = false;
                    return;
                }

                this.Intitial();
                this.GetAndBindData();
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            if (!this.HasMaintainAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                this.ccbtnOK.Visible = false;
                return;
            }

            #region 取得輸入資料
            KP3Config kp3Config = new KP3Config();
            {
                System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("big5");

                #region 報送單位代號
                kp3Config.Unit = this.tbxUnit.Text;
                if (String.IsNullOrEmpty(kp3Config.Unit) || !Common.IsEnglishNumber(kp3Config.Unit, 3))
                {
                    this.ShowSystemMessage("『報送單位代號』必須為3碼的阿拉伯數字或大寫英文字母組成");
                    return;
                }
                #endregion

                #region 管理者清單
                kp3Config.Managers = this.tbxManagers.Text;
                if (String.IsNullOrEmpty(kp3Config.Managers))
                {
                    this.ShowSystemMessage("『管理者清單』至少需要一筆資料");
                    return;
                }
                #endregion

                #region FTPUrl
                kp3Config.FTPUrl = this.tbxFTPUrl.Text;
                if (!CheckFTPUrl(kp3Config.FTPUrl))
                {
                    this.ShowSystemMessage("『FTP 網址』必須為 FTP、FTPS、SFTP 開頭的網址");
                    return;
                }
                #endregion

                #region FTPAcct
                kp3Config.FTPAcct = this.tbxFTPAcct.Text;
                if (String.IsNullOrEmpty(kp3Config.FTPAcct))
                {
                    this.ShowSystemMessage("請輸入『FTP 帳號』");
                    return;
                }
                #endregion

                #region [MDY:20220530] Checkmarx 調整
                #region FTPPXX
                kp3Config.FTPPXX = this.tbxFTPPXX.Text;
                if (String.IsNullOrEmpty(kp3Config.FTPPXX))
                {
                    this.ShowSystemMessage("請輸入『FTP 密碼』");
                    return;
                }
                #endregion
                #endregion

                #region 資訊格式代號
                kp3Config.HeadItem01 = this.tbxHeadItem01.Text;
                if (String.IsNullOrEmpty(kp3Config.HeadItem01) || kp3Config.HeadItem01.Length != 18 || !Common.IsEnglishNumber(kp3Config.HeadItem01.Replace("-", "")))
                {
                    this.ShowSystemMessage("『報送單位代號』必須為18碼的阿拉伯數字、大寫英文字母或 dash 符號組成");
                    return;
                }
                #endregion

                #region 聯絡電話
                kp3Config.HeadItem07 = this.tbxHeadItem07.Text;
                if (String.IsNullOrEmpty(kp3Config.HeadItem07) || kp3Config.HeadItem07.Length > 16 || !Common.IsNumber(kp3Config.HeadItem07.Replace("-", "").Replace("#", "")))
                {
                    this.ShowSystemMessage("『聯絡電話』必須為最多16碼的阿拉伯數字、dash 或 pound 符號組成");
                    return;
                }
                #endregion

                #region 聯絡人資訊或訊息
                kp3Config.HeadItem08 = this.tbxHeadItem08.Text;
                if (String.IsNullOrEmpty(kp3Config.HeadItem08) || encoding.GetByteCount(kp3Config.HeadItem08) > 80)
                {
                    this.ShowSystemMessage("『聯絡人資訊或訊息』最多80個字元（40個中文或全形字）");
                    return;
                }
                #endregion

                #region 特約機構屬性
                kp3Config.DataItem05 = this.ddlDataItem05.SelectedValue;
                if (kp3Config.DataItem05 != "G" && kp3Config.DataItem05 != "H")
                {
                    this.ShowSystemMessage("『特約機構屬性』只能是 G (非個人特約機構) 或 H (個人特約機構)");
                    return;
                }
                #endregion

                #region 特約機構類型
                kp3Config.DataItem09 = this.ddlDataItem09.SelectedValue;
                if (kp3Config.DataItem09 != "1" && kp3Config.DataItem09 != "2")
                {
                    this.ShowSystemMessage("『特約機構類型』只能是 1 (第一類) 或 2 (第二類)");
                    return;
                }
                #endregion

                #region 營業型態
                kp3Config.DataItem23 = this.ddlDataItem23.SelectedValue;
                if (kp3Config.DataItem23 != "1" && kp3Config.DataItem23 != "2" && kp3Config.DataItem23 != "3")
                {
                    this.ShowSystemMessage("『營業型態』只能是 1 (實體) 、 2 (網路) 或 3 (實體及網路)");
                    return;
                }
                #endregion

                #region 業務行為
                kp3Config.DataItem26 = this.ddlDataItem26.SelectedValue;
                if (kp3Config.DataItem26 != "1" && kp3Config.DataItem26 != "2" && kp3Config.DataItem26 != "3" && kp3Config.DataItem26 != "4")
                {
                    this.ShowSystemMessage("『營業型態』只能是 1 (境內) 、 2 (跨境) 、 3 (境內及跨境) 或 4 (境外特約機構)");
                    return;
                }
                #endregion

                #region 是否受理電子支付帳戶或儲值卡服務
                kp3Config.DataItem27 = this.ddlDataItem27.SelectedValue;
                if (kp3Config.DataItem27 != "1" && kp3Config.DataItem27 != "2" && kp3Config.DataItem27 != "3" && kp3Config.DataItem27 != "4")
                {
                    this.ShowSystemMessage("『是否受理電子支付帳戶或儲值卡服務』只能是 1 (境內電子支付帳戶服務) 、 2 (境外機構支付帳戶服務) 、 3 (境內儲值卡服務) 或 4 (境外機構記名儲值卡服務)");
                    return;
                }
                #endregion

                #region 是否受理信用卡服務
                kp3Config.DataItem29 = this.ddlDataItem29.SelectedValue;
                if (kp3Config.DataItem29 != "Y" && kp3Config.DataItem29 != "N")
                {
                    this.ShowSystemMessage("『是否受理信用卡服務』只能是 Y (是) 、 N (否)");
                    return;
                }
                #endregion

                #region 是否有銷售遞延性商品或服務
                kp3Config.DataItem33 = this.ddlDataItem33.SelectedValue;
                if (kp3Config.DataItem33 != "Y" && kp3Config.DataItem33 != "N")
                {
                    this.ShowSystemMessage("『是否有銷售遞延性商品或服務』只能是 Y (是) 、 N (否)");
                    return;
                }
                #endregion

                #region 是否安裝端末設備
                kp3Config.DataItem34 = this.ddlDataItem34.SelectedValue;
                if (kp3Config.DataItem34 != "" && kp3Config.DataItem34 != "Y" && kp3Config.DataItem34 != "N")
                {
                    this.ShowSystemMessage("『是否安裝端末設備』只能是 (忽略) 、Y (是) 、 N (否)");
                    return;
                }
                #endregion

                #region 是否安裝錄影設備
                kp3Config.DataItem35 = this.ddlDataItem35.SelectedValue;
                if (kp3Config.DataItem35 != "" && kp3Config.DataItem35 != "Y" && kp3Config.DataItem35 != "N")
                {
                    this.ShowSystemMessage("『是否安裝錄影設備』只能是 (忽略) 、  Y (是) 、 N (否)");
                    return;
                }
                #endregion

                #region 連鎖店加盟或直營
                kp3Config.DataItem36 = this.ddlDataItem36.SelectedValue;
                if (kp3Config.DataItem36 != "1" && kp3Config.DataItem36 != "2" && kp3Config.DataItem36 != "3" && kp3Config.DataItem36 != "")
                {
                    this.ShowSystemMessage("『營業型態』只能是 1 (加盟) 、 2 (直營) 、 3 (無法判別加盟或直營) 或 (非連鎖店)");
                    return;
                }
                #endregion
            }
            #endregion

            #region 儲存資料
            {
                ConfigEntity configData = null;
                Expression where = new Expression(ConfigEntity.Field.ConfigKey, KP3Config.RootLocalName);
                KeyValueList<OrderByEnum> orderbys = null;
                XmlResult xmlResult = DataProxy.Current.SelectFirst<ConfigEntity>(this.Page, where, orderbys, out configData);
                if (xmlResult.IsSuccess)
                {
                    if (configData == null)
                    {
                        configData = new ConfigEntity();
                        configData.ConfigKey = KP3Config.RootLocalName;
                        configData.ConfigValue = kp3Config.ToXml();
                        if (String.IsNullOrEmpty(configData.ConfigValue))
                        {
                            this.ShowActionFailureMessage(this.GetLocalized("儲存料"), "資料序列化失敗");
                        }
                        else
                        {
                            int count = 0;
                            xmlResult = DataProxy.Current.Insert<ConfigEntity>(this.Page, configData, out count);
                            if (xmlResult.IsSuccess)
                            {
                                this.ShowActionSuccessMessage(this.GetLocalized("儲存資料"));
                            }
                            else
                            {
                                this.ShowActionFailureMessage(this.GetLocalized("儲存資料"), xmlResult.Code, xmlResult.Message);
                            }
                        }
                    }
                    else
                    {
                        configData.ConfigValue = kp3Config.ToXml();
                        if (String.IsNullOrEmpty(configData.ConfigValue))
                        {
                            this.ShowActionFailureMessage(this.GetLocalized("儲存料"), "資料序列化失敗");
                        }
                        else
                        {
                            int count = 0;
                            xmlResult = DataProxy.Current.Update<ConfigEntity>(this.Page, configData, out count);
                            if (xmlResult.IsSuccess)
                            {
                                this.ShowActionSuccessMessage(this.GetLocalized("儲存資料"));
                            }
                            else
                            {
                                this.ShowActionFailureMessage(this.GetLocalized("儲存資料"), xmlResult.Code, xmlResult.Message);
                            }
                        }
                    }
                }
                else
                {
                    this.ShowActionFailureMessage(this.GetLocalized("儲存資料"), xmlResult.Code, xmlResult.Message);
                    return;
                }
            }
            #endregion
        }
    }
}