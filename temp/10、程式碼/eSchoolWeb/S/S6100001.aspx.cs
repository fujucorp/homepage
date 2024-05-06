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
    /// KP3資料登錄
    /// </summary>
    public partial class S6100001 : BasePage
    {
        #region Keep 頁面參數
        private string EditItem07
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["EditItem07"] as string);
            }
            set
            {
                ViewState["EditItem07"] = String.IsNullOrWhiteSpace(value) ? String.Empty : value;
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 依據指定終止契約種類取得終止契約原因選項資料集合
        /// </summary>
        /// <param name="item12">指定終止契約種類</param>
        /// <returns>傳回終止契約原因選項資料集合</returns>
        private List<CodeText> Get13Items(string item12)
        {
            switch (item12)
            {
                case "1":
                    #region 一般終止契約
                    {
                        List<CodeText> items = new List<CodeText>()
                        {
                            new CodeText("A", "停業、歇業、註銷"),
                            new CodeText("B", "倒閉"),
                            new CodeText("C", "天災（水災、火災、地震）"),
                            new CodeText("L", "不符經濟效益（無實際消費）"),
                            new CodeText("N", "特約機構申請終止契約"),
                            new CodeText("O", "遷移"),
                            new CodeText("V", "電子支付機構終止、移轉業務或被併購"),
                            new CodeText("7", "除戶  個人特約機構專用代號"),
                            new CodeText("8", "受監護或輔助宣告"),
                            new CodeText("9", "疑似無營業事實"),
                            new CodeText("D", "提交虛偽身分資料之虞（被害）")
                        };
                        return items;
                    }
                    #endregion

                case "2":
                    #region 變更組織
                    {
                        List<CodeText> items = new List<CodeText>()
                        {
                            new CodeText("D", "變更登記"),
                            new CodeText("E", "轉讓、變更負責人"),
                            new CodeText("M", "調整行業")
                        };
                        return items;
                    }
                    #endregion

                case "3":
                    #region 不滿簽約條件
                    {
                        List<CodeText> items = new List<CodeText>()
                        {
                            new CodeText("F", "手續費等成本高"),
                            new CodeText("G", "付款時效、條件差")
                        };
                        return items;
                    }
                    #endregion

                case "4":
                    #region 特約機構違約
                    {
                        List<CodeText> items = new List<CodeText>()
                        {
                            new CodeText("H", "拒絕簽帳"),
                            new CodeText("I", "加收手續費")
                        };
                        return items;
                    }
                    #endregion

                case "5":
                    #region 電子支付機構終止契約
                    {
                        List<CodeText> items = new List<CodeText>()
                        {
                            new CodeText("J", "疑似偽卡交易"),
                            new CodeText("K", "疑似地下錢莊"),
                            new CodeText("R", "疑似與詐欺集團勾結"),
                            new CodeText("P", "疑似不實簽單交易"),
                            new CodeText("Q", "疑似無效卡交易"),
                            new CodeText("S", "疑似自編授權碼"),
                            new CodeText("T", "疑似磁條盜錄"),
                            new CodeText("U", "法院判決異常"),
                            new CodeText("W", "銷退貨異常"),
                            new CodeText("Y", "違約欠款"),
                            new CodeText("Z", "涉嫌虛設行號"),
                            new CodeText("1", "疑似自店消費"), 
                            new CodeText("2", "疑似調現交易"),
                            new CodeText("3", "疑似吸金交易"), 
                            new CodeText("4", "疑似非法禮券交易"), 
                            new CodeText("5", "疑似違約銷售遞延性商品或服務"),
                            new CodeText("6", "欠稅終止契約"), 
                            new CodeText("7", "提交虛偽身分資料之虞（加害）"),
                            new CodeText("8", "疑似從事詐欺、洗錢等不法行為"),
                            new CodeText("9", "疑似非法特約機構"),
                            new CodeText("A", "有財務或營運等狀況惡化，足以影響營運之情事"),
                            new CodeText("X", "其他")
                        };
                        return items;
                    }
                    #endregion

                case "6":
                    #region 其他
                    {
                        List<CodeText> items = new List<CodeText>()
                        {
                            new CodeText("X", "其他")
                        };
                        return items;
                    }
                    #endregion

                default:
                    return null;
            }
        }

        private void Intitial()
        {
            #region Bind 報送代碼
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("C", "異動"),
                    new CodeText("D", "刪除")
                };
                WebHelper.SetDropDownListItems(this.ddlItem02, DefaultItem.Kind.None, false, items, true, false, 0, "");
            }
            #endregion

            #region Bind 特約機構屬性
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("G", "非個人特約機構"),
                    new CodeText("H", "個人特約機構")
                };
                WebHelper.SetDropDownListItems(this.ddlItem05, DefaultItem.Kind.Select, false, items, true, false, 0, "");
            }
            #endregion

            #region Bind 特約機構類型
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("1", "第一類"),
                    new CodeText("2", "第二類")
                };
                WebHelper.SetDropDownListItems(this.ddlItem09, DefaultItem.Kind.Select, false, items, true, false, 0, "");
            }
            #endregion

            #region [OLD]
            //#region Bind 終止契約種類代號
            //{
            //    List<CodeText> items = new List<CodeText>
            //    {
            //        new CodeText("", "無終止契約"),
            //        new CodeText("1", "一般終止契約"),
            //        new CodeText("2", "變更組織"),
            //        new CodeText("3", "不滿簽約條件"),
            //        new CodeText("4", "特約機構違約"),
            //        new CodeText("5", "電子支付機構終止契約"),
            //        new CodeText("6", "其他")
            //    };
            //    WebHelper.SetDropDownListItems(this.ddlItem12, DefaultItem.Kind.None, false, items, true, false, 0, "");
            //}
            //#endregion
            #endregion

            #region Bind 終止契約原因代號
            //{
            //    string item12 = this.ddlItem12.SelectedValue;
            //    List<CodeText> items = this.Get13Items(item12);
            //    WebHelper.SetDropDownListItems(this.ddlItem13, DefaultItem.Kind.Omit, false, items, true, false, 0, "");
            //}
            #endregion

            #region Bind 終止契約後有應收未收取款項
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("Y", "是"),
                    new CodeText("N", "否")
                };
                WebHelper.SetDropDownListItems(this.ddlItem15, DefaultItem.Kind.Omit, false, items, true, false, 0, "");
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
                WebHelper.SetDropDownListItems(this.ddlItem23, DefaultItem.Kind.Select, false, items, true, false, 0, "");
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
                WebHelper.SetDropDownListItems(this.ddlItem26, DefaultItem.Kind.Select, false, items, true, false, 0, "");
            }
            #endregion

            #region Bind 是否受理電子支付帳戶或儲值卡服務
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("1", "1.境內電子支付帳戶服務"),
                    new CodeText("2", "2.境外機構支付帳戶服務"),
                    new CodeText("3", "3.境內儲值卡服務"),
                    new CodeText("4", "4.境外機構記名儲值卡服務")
                };
                WebHelper.SetCheckBoxListItems(this.cblItem27, items, false, 2, null);
            }
            #endregion

            #region Bind 是否受理信用卡服務
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("Y", "是"),
                    new CodeText("N", "否")
                };
                WebHelper.SetDropDownListItems(this.ddlItem29, DefaultItem.Kind.Select, false, items, true, false, 0, "");
            }
            #endregion

            #region Bind 受理信用卡別名稱
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("A", "美國運通信用卡 AE（CREDIT）"),
                    new CodeText("C", "中國銀聯卡 CUP"),
                    new CodeText("D", "大來卡 DINERS"),
                    new CodeText("E", "美國運通簽帳卡 AE（CHARGE）"),
                    new CodeText("J", "日本吉世美卡 JCB"),
                    new CodeText("M", "萬事達卡 MASTER"),
                    new CodeText("N", "聯合信用卡 NCCC"),
                    new CodeText("V", "威士卡 VISA"),
                    new CodeText("O", "其他 OTHER")
                };
                WebHelper.SetCheckBoxListItems(this.cblItem31, items, false, 2, null);
            }
            #endregion

            #region Bind 是否有銷售遞延性商品或服務
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("Y", "是"),
                    new CodeText("N", "否")
                };
                WebHelper.SetDropDownListItems(this.ddlItem33, DefaultItem.Kind.Select, false, items, true, false, 0, "");
            }
            #endregion

            #region Bind 是否安裝端末設備
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("Y", "是"),
                    new CodeText("N", "否")
                };
                WebHelper.SetDropDownListItems(this.ddlItem34, DefaultItem.Kind.Omit, false, items, true, false, 0, "");
            }
            #endregion

            #region Bind 是否安裝錄影設備
            {
                List<CodeText> items = new List<CodeText>()
                {
                    new CodeText("Y", "是"),
                    new CodeText("N", "否")
                };
                WebHelper.SetDropDownListItems(this.ddlItem35, DefaultItem.Kind.Omit, false, items, true, false, 0, "");
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
                WebHelper.SetDropDownListItems(this.ddlItem36, DefaultItem.Kind.None, false, items, true, false, 0, "");
            }
            #endregion
        }

        private KP3Config _KP3Config = null;
        /// <summary>
        /// 取得 KP3Config
        /// </summary>
        /// <returns></returns>
        private KP3Config GetKP3Config()
        {
            if (_KP3Config == null)
            {
                ConfigEntity configData = null;
                Expression where = new Expression(ConfigEntity.Field.ConfigKey, KP3Config.RootLocalName);
                KeyValueList<OrderByEnum> orderbys = null;
                XmlResult xmlResult = DataProxy.Current.SelectFirst<ConfigEntity>(this.Page, where, orderbys, out configData);
                if (xmlResult.IsSuccess)
                {
                    if (configData == null)
                    {
                        this.ShowErrorMessage(xmlResult.Code, "缺少KP3參數設定資料，請先設定");
                        return null;
                    }
                    else
                    {
                        KP3Config kp3Config = KP3Config.Create(configData.ConfigValue);
                        if (kp3Config == null)
                        {
                            this.ShowErrorMessage(xmlResult.Code, "缺少KP3參數設定或資料不正確，請先設定");
                        }
                        _KP3Config = kp3Config;
                        return _KP3Config;
                    }
                }
                else
                {
                    this.ShowActionFailureMessage(this.GetLocalized("讀取KP3參數設定"), xmlResult.Code, xmlResult.Message);
                    return null;
                }
            }
            else
            {
                return _KP3Config;
            }
        }

        private KP3Entity GetKP3Data(string item07)
        {
            KP3Config kp3Config = this.GetKP3Config();
            if (kp3Config == null)
            {
                return null;
            }

            // [MEMO] 這個功能處理的永遠是同一個 Item07 的最後一筆資料
            KP3Entity kp3Data = null;
            Expression where = new Expression(KP3Entity.Field.Item07, item07);
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(KP3Entity.Field.CreateDate, OrderByEnum.Desc);
            XmlResult xmlResult = DataProxy.Current.SelectFirst<KP3Entity>(this.Page, where, orderbys, out kp3Data);
            if (xmlResult.IsSuccess)
            {
                LogonUser logonUser = this.GetLogonUser();
                if (kp3Data == null)
                {
                    //[MEMO] 沒有資料一定是新增登錄，套用預設值。Item02 一定是 A
                    kp3Data = new KP3Entity(kp3Config);
                    kp3Data.SN = null;
                    kp3Data.Item02 = "A";
                    kp3Data.Item07 = item07;
                    kp3Data.Item40 = String.Empty;
                    kp3Data.Status = KP3StatusCodeTexts.STATUS10_CODE;

                    kp3Data.BranchCode = logonUser.BankId;
                    kp3Data.CreateDate = DateTime.Now;
                    kp3Data.CreateUnit = logonUser.BankId;
                    kp3Data.CreateUser = logonUser.UserId;
                }
                else
                {
                    //[MEMO] 因為 Item02 可能會被更動，所以先將 Item02 串入 FeedbackResult
                    if (!String.IsNullOrEmpty(kp3Data.RenderSN) && !String.IsNullOrEmpty(kp3Data.FeedbackStatus))
                    {
                        kp3Data.FeedbackResult = String.Format("報送{0}{1}。{2}", kp3Data.GetItem02Text(), (kp3Data.FeedbackStatus == "Y" ? "成功" : "失敗"), kp3Data.FeedbackResult);
                    }

                    if (kp3Data.Status == KP3StatusCodeTexts.STATUS10_CODE)
                    {
                        //[MEMO] 最後一筆資料已登錄表示修改登錄，不套預設值。Item02 保持原值
                        //kp3Data.Item40 = String.Empty;

                        kp3Data.BranchCode = logonUser.BankId;
                        kp3Data.CreateDate = DateTime.Now;
                        kp3Data.CreateUnit = logonUser.BankId;
                        kp3Data.CreateUser = logonUser.UserId;
                    }
                    else if (kp3Data.Status == KP3StatusCodeTexts.STATUS40_CODE)
                    {
                        //[MEMO] 最後一筆資料已處理表示重新登錄，套用預設值。Item02 預設 C
                        kp3Data.SN = null;
                        //如果原報送代碼為 D (刪除) 且回饋狀態為 Y (成功)，則重新登錄時報送代碼要用 A (新增) 否則用 C (異動)
                        switch (kp3Data.Item02)
                        {
                            case "A":
                                //原報送新增成功，新的報送預設為修改，否則還是原來的新增
                                kp3Data.Item02 = kp3Data.FeedbackStatus == "Y" ? "C" : "A";
                                break;
                            case "D":
                                //原報送刪除成功，新的報送只能是新增，否則還是原來的刪除
                                kp3Data.Item02 = kp3Data.FeedbackStatus == "Y" ? "A" : "D";
                                break;
                            default:
                                //原報送異動，不管是否成功，還是原來的異動
                                kp3Data.Item02 = "C";
                                break;
                        }
                        kp3Data.Item40 = String.Empty;

                        kp3Data.Item03 = kp3Config.Unit;
                        kp3Data.Item05 = kp3Config.DataItem05;
                        kp3Data.Item09 = kp3Config.DataItem09;
                        kp3Data.Item23 = kp3Config.DataItem23;
                        kp3Data.Item26 = kp3Config.DataItem26;
                        kp3Data.Item27 = kp3Config.DataItem27;
                        kp3Data.Item29 = kp3Config.DataItem29;
                        kp3Data.Item33 = kp3Config.DataItem33;
                        kp3Data.Item34 = kp3Config.DataItem34;
                        kp3Data.Item35 = kp3Config.DataItem35;
                        kp3Data.Item36 = kp3Config.DataItem36;

                        kp3Data.BranchCode = logonUser.BankId;
                        kp3Data.CreateDate = DateTime.Now;
                        kp3Data.CreateUnit = logonUser.BankId;
                        kp3Data.CreateUser = logonUser.UserId;

                        kp3Data.Status = KP3StatusCodeTexts.STATUS10_CODE;

                        //[MEMO] 不清空回饋相關欄位，因為編輯介面要顯示
                    }
                    else
                    {
                        //[MEMO] 最後一筆資料處理中表示僅提供檢視，只能顯示所以不做調整
                    }
                }
            }
            else
            {
                this.ShowActionFailureMessage(this.GetLocalized("讀取已登錄資料"), xmlResult.Code, xmlResult.Message);
            }
            return kp3Data;
        }

        private void BindData(KP3Entity data)
        {
            this.EditItem07 = data.Item07;

            KP3Config kp3Config = this.GetKP3Config();

            bool isEditable = (data.Status == KP3StatusCodeTexts.STATUS10_CODE);
            WebControl[] controls = new WebControl[] {
                this.tbxItem06, this.tbxItem10, this.tbxItem11, this.tbxItem14, /* this.tbxItem16, */
                this.tbxItem17, this.tbxItem18, this.tbxItem19, this.tbxItem20, this.tbxItem21,
                this.tbxItem24, this.tbxItem25, /* this.tbxItem30, */ this.tbxItem37, this.tbxItem38,

                this.ddlItem02, this.ddlItem12, this.ddlItem13, /* this.ddlItem15, */

                /* this.cblItem31 */
            };
            foreach (WebControl control in controls)
            {
                control.Enabled = isEditable;
            }
            //Item34 允許不設定預設值，沒有預設值就依據狀態決定是否可修改
            if (String.IsNullOrEmpty(kp3Config.DataItem34))
            {
                this.ddlItem34.Enabled = isEditable;
            }
            //Item34 允許不設定預設值，沒有預設值就依據狀態決定是否可修改
            if (String.IsNullOrEmpty(kp3Config.DataItem35))
            {
                this.ddlItem35.Enabled = isEditable;
            }

            this.lblItem01.Text = HttpUtility.HtmlEncode(data.Item01);
            this.lblItem03.Text = HttpUtility.HtmlEncode(data.Item03);
            this.lblItem40.Text = HttpUtility.HtmlEncode(data.Item40);

            this.tbxItem06.Text = HttpUtility.HtmlEncode(data.Item06);
            this.tbxItem07.Text = HttpUtility.HtmlEncode(data.Item07);
            this.tbxItem10.Text = HttpUtility.HtmlEncode(data.Item10);
            this.tbxItem11.Text = HttpUtility.HtmlEncode(data.Item11);
            this.tbxItem14.Text = HttpUtility.HtmlEncode(data.Item14);
            this.tbxItem16.Text = HttpUtility.HtmlEncode(data.Item16);
            this.tbxItem17.Text = HttpUtility.HtmlEncode(data.Item17);
            this.tbxItem18.Text = HttpUtility.HtmlEncode(data.Item18);
            this.tbxItem19.Text = HttpUtility.HtmlEncode(data.Item19);
            this.tbxItem20.Text = HttpUtility.HtmlEncode(data.Item20);
            this.tbxItem21.Text = HttpUtility.HtmlEncode(data.Item21);
            this.tbxItem24.Text = HttpUtility.HtmlEncode(data.Item24);
            this.tbxItem25.Text = HttpUtility.HtmlEncode(data.Item25);
            this.tbxItem30.Text = HttpUtility.HtmlEncode(data.Item30);
            this.tbxItem37.Text = HttpUtility.HtmlEncode(data.Item37);
            this.tbxItem38.Text = HttpUtility.HtmlEncode(data.Item38);

            switch (data.Item02)
            {
                case "A":
                    this.lblItem02.Visible =  true;
                    this.ddlItem02.Visible = false;
                    break;
                case "C":
                case "D":
                    this.lblItem02.Visible =  false;
                    this.ddlItem02.Visible = true;
                     WebHelper.SetDropDownListSelectedValue(this.ddlItem02, data.Item02);
                    break;
                default:
                    this.lblItem02.Visible = false;
                    this.ddlItem02.Visible = false;
                    break;
            }

            WebHelper.SetDropDownListSelectedValue(this.ddlItem05, data.Item05);
            WebHelper.SetDropDownListSelectedValue(this.ddlItem09, data.Item09);

            //WebHelper.SetDropDownListSelectedValue(this.ddlItem12, data.Item12);
            this.BindItem12(!String.IsNullOrEmpty(data.Item14), data.Item12);

            List<CodeText> items = this.Get13Items(data.Item12);
            WebHelper.SetDropDownListItems(this.ddlItem13, DefaultItem.Kind.Select, false, items, true, false, 0, data.Item13);

            WebHelper.SetDropDownListSelectedValue(this.ddlItem15, data.Item15);
            WebHelper.SetDropDownListSelectedValue(this.ddlItem23, data.Item23);
            WebHelper.SetDropDownListSelectedValue(this.ddlItem26, data.Item26);
            WebHelper.SetDropDownListSelectedValue(this.ddlItem29, data.Item29);
            WebHelper.SetDropDownListSelectedValue(this.ddlItem33, data.Item33);
            WebHelper.SetDropDownListSelectedValue(this.ddlItem34, data.Item34);
            WebHelper.SetDropDownListSelectedValue(this.ddlItem35, data.Item35);
            WebHelper.SetDropDownListSelectedValue(this.ddlItem36, data.Item36);

            WebHelper.SetCheckBoxListSelectedValues(this.cblItem27, data.GetItem27());
            WebHelper.SetCheckBoxListSelectedValues(this.cblItem31, data.GetItem31());

            this.lblStatus.Text = (String.IsNullOrEmpty(data.SN) ? "登錄資料中" : KP3StatusCodeTexts.GetText(data.Status));
            if (data.Status != KP3StatusCodeTexts.STATUS10_CODE)
            {
                this.ccbtnOK.Visible = false;
                this.ShowSystemMessage(this.GetLocalized("該特約機構資料處理中，僅提供檢視"));
            }
            else
            {
                this.ccbtnOK.Visible = true;
            }

            if (String.IsNullOrEmpty(data.RenderSN) || !String.IsNullOrEmpty(data.SN))
            {
                this.trFeedback.Visible = false;
                this.lblFeedback.Text = String.Empty;
            }
            else
            {
                this.trFeedback.Visible = true;
                this.lblFeedback.Text = HttpUtility.HtmlEncode(data.FeedbackResult);
            }

            this.divQuery.Visible = false;
            this.divEdit.Visible = true;
        }

        private void GetAndBindData(string item07)
        {
            KP3Entity kp3Data = this.GetKP3Data(item07);
            if (kp3Data != null)
            {
                this.BindData(kp3Data);
            }
        }

        private void BindItem12(bool item14HasValue, string item12SelectedValue)
        {
            if (item14HasValue)
            {
                #region 有終止契約日期
                if (this.ddlItem12.Items.Count != 7 || this.ddlItem12.Items[0].Text != "請選擇")
                {
                    List<CodeText> items = new List<CodeText>
                    {
                        new CodeText(String.Empty, "請選擇"),
                        new CodeText("1", "一般終止契約"),
                        new CodeText("2", "變更組織"),
                        new CodeText("3", "不滿簽約條件"),
                        new CodeText("4", "特約機構違約"),
                        new CodeText("5", "電子支付機構終止契約"),
                        new CodeText("6", "其他")
                    };
                    WebHelper.SetDropDownListItems(this.ddlItem12, DefaultItem.Kind.None, false, items, true, false, 0, item12SelectedValue);
                }
                else
                {
                    WebHelper.SetDropDownListSelectedValue(this.ddlItem12, item12SelectedValue);
                }
                #endregion
            }
            else
            {
                #region 無終止契約日期
                if (this.ddlItem12.Items.Count != 1 || this.ddlItem12.Items[0].Text != "無終止契約")
                {
                    List<CodeText> items = new List<CodeText>
                    {
                        new CodeText(String.Empty, "無終止契約")
                    };
                    WebHelper.SetDropDownListItems(this.ddlItem12, DefaultItem.Kind.None, false, items, true, false, 0, String.Empty);
                }
                else
                {
                    WebHelper.SetDropDownListSelectedValue(this.ddlItem12, String.Empty);  //只有空白這項，所以只能是空白
                }
                #endregion
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (!this.HasMaintainAuth())
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無登錄權限");
                    this.ccbtnEdit.Visible = false;
                    return;
                }

                this.Intitial();
            }
        }

        protected void ccbtnEdit_Click(object sender, EventArgs e)
        {
            if (!this.HasMaintainAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無登錄權限");
                this.ccbtnEdit.Visible = false;
                return;
            }

            string item07 = this.tbxQItem07.Text.Trim();
            if (String.IsNullOrEmpty(item07))
            {
                this.ShowMustInputAlert("特約機構代號");
            }
            else
            {
                this.GetAndBindData(item07);
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            if (!this.HasMaintainAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無登錄權限");
                this.ccbtnEdit.Visible = false;
                return;
            }

            KP3Entity data = this.GetKP3Data(this.EditItem07);
            if (data == null || data.Status != KP3StatusCodeTexts.STATUS10_CODE)
            {
                this.ShowSystemMessage(this.GetLocalized("該特約機構資料處理中，僅提供檢視"));
                this.divQuery.Visible = true;
                this.divEdit.Visible = false;
                return;
            }

            #region 資料別
            #endregion

            #region 報送代碼 (Item02)
            if (data.Item02 != "A")
            {
                if (this.ddlItem02.Enabled && this.ddlItem02.Visible)
                {
                    data.Item02 = this.ddlItem02.SelectedValue;
                }
            }
            #endregion

            #region 電子支付機構代號 (Item03)
            #endregion

            #region 特約機構屬性
            if (this.ddlItem05.Enabled)
            {
                data.Item05 = this.ddlItem05.SelectedValue;
            }
            #endregion

            #region 特約機構 BAN/IDN
            if (this.tbxItem06.Enabled)
            {
                data.Item06 = this.tbxItem06.Text;
            }
            #endregion

            #region 特約機構代號
            if (this.tbxItem07.Enabled)
            {
                data.Item07 = this.tbxItem07.Text;
            }
            #endregion

            #region 特約機構類型
            if (this.ddlItem09.Enabled)
            {
                data.Item09 = this.ddlItem09.SelectedValue;
            }
            #endregion

            #region 負責人/代表人 IDN
            if (this.tbxItem10.Enabled)
            {
                data.Item10 = this.tbxItem10.Text;
            }
            #endregion

            #region 簽約日期
            if (this.tbxItem11.Enabled)
            {
                data.Item11 = this.tbxItem11.Text;
            }
            #endregion

            #region 終止契約種類代號
            if (this.ddlItem12.Enabled)
            {
                data.Item12 = this.ddlItem12.SelectedValue;
            }
            #endregion

            #region 終止契約原因代號
            if (this.ddlItem13.Enabled)
            {
                data.Item13 = this.ddlItem13.SelectedValue;
            }
            #endregion

            #region 終止契約日期
            if (this.tbxItem14.Enabled)
            {
                data.Item14 = this.tbxItem14.Text;
            }
            #endregion

            #region 終止契約後有應收未收取款項
            if (this.ddlItem15.Enabled)
            {
                data.Item15 = this.ddlItem15.SelectedValue;
            }
            else
            {
                if (String.IsNullOrEmpty(data.Item14))
                {
                    //無終止契約日期：終止契約後有應收未收取款項 固定為 忽略
                    data.Item15 = String.Empty;
                }
                else
                {
                    //有終止契約日期：終止契約後有應收未收取款項 固定為 N-否
                    data.Item15 = "N";
                }
            }
            #endregion

            #region 終止契約後應收未收取款項金額
            if (this.tbxItem16.Enabled)
            {
                data.Item16 = this.tbxItem16.Text;
            }
            else
            {
                if (String.IsNullOrEmpty(data.Item14))
                {
                    //無終止契約日期：終止契約後應收未收取款項金額 固定為 空白
                    data.Item16 = String.Empty;
                }
                else
                {
                    //有終止契約日期：終止契約後應收未收取款項金額 固定為 0000000000
                    data.Item16 = "0000000000";
                }
            }
            #endregion

            #region 登記名稱
            if (this.tbxItem17.Enabled)
            {
                data.Item17 = this.tbxItem17.Text;
            }
            #endregion

            #region 登記地址
            if (this.tbxItem18.Enabled)
            {
                data.Item18 = this.tbxItem18.Text;
            }
            #endregion

            #region 招牌名稱
            if (this.tbxItem19.Enabled)
            {
                data.Item19 = this.tbxItem19.Text;
            }
            #endregion

            #region 營業地址
            if (this.tbxItem20.Enabled)
            {
                data.Item20 = this.tbxItem20.Text;
            }
            #endregion

            #region 英文名稱
            if (this.tbxItem21.Enabled)
            {
                data.Item21 = this.tbxItem21.Text;
            }
            #endregion

            #region 營業型態
            if (this.ddlItem23.Enabled)
            {
                data.Item23 = this.ddlItem23.SelectedValue;
            }
            #endregion

            #region 資本額
            if (this.tbxItem24.Enabled)
            {
                data.Item24 = this.tbxItem24.Text;
            }
            #endregion

            #region 設立日期
            if (this.tbxItem25.Enabled)
            {
                data.Item25 = this.tbxItem25.Text;
            }
            #endregion

            #region 業務行為
            if (this.ddlItem26.Enabled)
            {
                data.Item26 = this.ddlItem26.SelectedValue;
            }
            #endregion

            #region 是否受理電子支付帳戶或儲值卡服務
            if (this.cblItem27.Enabled)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (ListItem item in this.cblItem27.Items)
                {
                    if (item.Selected)
                    {
                        sb.Append(item.Value);
                    }
                }
                data.Item27 = sb.ToString();
            }
            #endregion

            #region 是否受理信用卡服務
            if (this.ddlItem29.Enabled)
            {
                data.Item29 = this.ddlItem29.SelectedValue;
            }
            #endregion

            #region 營業性質
            if (this.tbxItem30.Enabled)
            {
                data.Item30 = this.tbxItem30.Text;
            }
            else
            {
                data.Item30 = String.Empty;
            }
            #endregion

            #region 受理信用卡別名稱
            if (this.cblItem31.Enabled)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (ListItem item in this.cblItem31.Items)
                {
                    if (item.Selected)
                    {
                        sb.Append(item.Value);
                    }
                }
                data.Item31 = sb.ToString();
            }
            else
            {
                data.Item31 = String.Empty;
            }
            #endregion

            #region 是否有銷售遞延性商品或服務
            if (this.ddlItem33.Enabled)
            {
                data.Item33 = this.ddlItem33.SelectedValue;
            }
            #endregion

            #region 是否安裝端末設備
            if (this.ddlItem34.Enabled)
            {
                data.Item34 = this.ddlItem34.SelectedValue;
            }
            #endregion

            #region 是否安裝錄影設備
            if (this.ddlItem35.Enabled)
            {
                data.Item35 = this.ddlItem35.SelectedValue;
            }
            #endregion

            #region 連鎖店加盟或直營
            if (this.ddlItem36.Enabled)
            {
                data.Item36 = this.ddlItem36.SelectedValue;
            }
            #endregion

            #region 保證人1 IDN/BAN
            if (this.tbxItem37.Enabled)
            {
                data.Item37 = this.tbxItem37.Text;
            }
            #endregion

            #region 保證人2 IDN/BAN
            if (this.tbxItem38.Enabled)
            {
                data.Item38 = this.tbxItem38.Text;
            }
            #endregion

            #region 資料更新日期
            data.Item40 = Common.GetTWDate7();
            #endregion

            #region 回饋相關資料清空
            {
                data.RenderSN = null;
                data.FeedbackStatus = null;
                data.FeedbackResult = null;
            }
            #endregion

            string errmsg = data.CheckItemValue();
            if (!String.IsNullOrEmpty(errmsg))
            {
                this.ShowSystemMessage(errmsg);
                return;
            }

            bool hasSN = !String.IsNullOrEmpty(data.SN);

            int count = 0;
            XmlResult xmlResult = null;
            if (hasSN)
            {
                xmlResult = DataProxy.Current.Update(this.Page, data, out count);
            }
            else
            {
                data.SN = System.Guid.NewGuid().ToString("N").ToUpper();
                xmlResult = DataProxy.Current.Insert(this.Page, data, out count);
            }
            if (xmlResult.IsSuccess)
            {
                this.ShowActionSuccessMessage(this.GetLocalized("登錄資料"));
                this.divQuery.Visible = true;
                this.divEdit.Visible = false;
            }
            else
            {
                this.ShowActionFailureMessage(this.GetLocalized("登錄資料"), xmlResult.Code, xmlResult.Message);
            }
        }

        protected void ccbtnGoBack_Click(object sender, EventArgs e)
        {
            this.tbxQItem07.Text = String.Empty;
            this.divQuery.Visible = true;
            this.divEdit.Visible = false;
        }

        protected void ddlItem12_SelectedIndexChanged(object sender, EventArgs e)
        {
            string item12 = this.ddlItem12.SelectedValue;
            List<CodeText> items = this.Get13Items(item12);
            WebHelper.SetDropDownListItems(this.ddlItem13, DefaultItem.Kind.Select, false, items, true, false, 0, String.Empty);
        }

        protected void tbxItem14_TextChanged(object sender, EventArgs e)
        {
            string item14 = this.tbxItem14.Text.Trim();
            DateTime date;
            if (!String.IsNullOrEmpty(item14) && !Common.TryConvertTWDate7(item14, out date))
            {
                this.ShowSystemMessage("『終止契約日期』必須為7碼民國年YYYMMDD的日期");
            }

            if (String.IsNullOrEmpty(item14))
            {
                #region 無終止契約日期
                #region 終止契約種類代號 固定為 無終止契約
                {
                    this.BindItem12(false, this.ddlItem12.SelectedValue);
                }
                #endregion

                #region 終止契約後有應收未收取款項 固定為 忽略
                {
                    WebHelper.SetDropDownListSelectedValue(this.ddlItem15, String.Empty);
                }
                #endregion

                #region 終止契約後應收未收取款項金額 固定為 空白
                {
                    this.tbxItem16.Text = String.Empty;
                }
                #endregion
                #endregion
            }
            else
            {
                #region 有終止契約日期
                #region 終止契約種類代號 無終止契約 改為請選擇
                {
                    this.BindItem12(true, this.ddlItem12.SelectedValue);
                }
                #endregion

                #region 終止契約後有應收未收取款項 固定為 N-否
                {
                    WebHelper.SetDropDownListSelectedValue(this.ddlItem15, "N");
                }
                #endregion

                #region 終止契約後應收未收取款項金額 固定為 0000000000
                {
                    this.tbxItem16.Text = "0000000000";
                }
                #endregion
                #endregion
            }
        }
    }
}