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

namespace eSchoolWeb.S
{
    /// <summary>
    /// 學校資料管理 (維護頁)
    /// </summary>    
    public partial class S5100001M : BasePage
    {
        #region Keep 頁面參數
        /// <summary>
        /// 操作模式參數
        /// </summary>
        private string Action
        {
            get
            {
                return ViewState["ACTION"] as string;
            }
            set
            {
                ViewState["ACTION"] = value == null ? null : value.Trim().ToUpper();
            }
        }

        /// <summary>
        /// 編輯的業務別碼
        /// </summary>
        private string EditReceiveType
        {
            get
            {
                return ViewState["EditReceiveType"] as string;
            }
            set
            {
                ViewState["EditReceiveType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的代收類別資料
        /// </summary>
        private SchoolRTypeEntity EditSchoolRType
        {
            get
            {
                return ViewState["EditSchoolRType"] as SchoolRTypeEntity;
            }
            set
            {
                ViewState["EditSchoolRType"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                #region 檢查維護權限
                if (!this.HasMaintainAuth())
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("無維護權限");
                    this.ShowJsAlert(msg);
                    return;
                }
                #endregion

                #region 處理參數
                bool isOK = false;

                KeyValueList<string> QueryString = Session["QueryString"] as KeyValueList<string>;
                if (QueryString == null || QueryString.Count == 0)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("缺少網頁參數");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }

                SchoolRTypeEntity data = null;
                this.Action = QueryString.TryGetValue("Action", String.Empty);
                this.EditReceiveType = QueryString.TryGetValue("ReceiveType", String.Empty);
                switch (this.Action)
                {
                    case ActionMode.Insert:
                        #region 新增
                        {
                            if(this.EditReceiveType !="")//學校新增商家代號
                            {
                                //新增代收類別，先把學校基本資料帶出來
                                #region
                                Expression where = new Expression(SchoolRTypeEntity.Field.SchIdenty, this.EditReceiveType);//這裡沒錯，帶過來的是學校代號，為了帶出預設值
                                #endregion

                                string receiveType = QueryString.TryGetValue("ReceiveType", null);
                                if (receiveType != null)
                                {
                                    //取得修改 / 刪除資料
                                    XmlResult result = DataProxy.Current.SelectFirst<SchoolRTypeEntity>(this, where, null, out data);
                                    if (result.IsSuccess)
                                    {
                                        this.EditSchoolRType = data;
                                        isOK = true;
                                    }
                                }
                            }
                            else//新增一所學校
                            {
                                data = new SchoolRTypeEntity();
                                initData(ref data);
                            }
                        }
                        #endregion
                        isOK = true;
                        break;
                    case ActionMode.Modify:
                    case ActionMode.Delete:
                        #region 修改 / 刪除
                        {
                            #region 查詢條件
                            Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, this.EditReceiveType);
                            #endregion

                            string receiveType = QueryString.TryGetValue("ReceiveType", null);
                            if (receiveType != null)
                            {
                                //取得修改 / 刪除資料
                                XmlResult result = DataProxy.Current.SelectFirst<SchoolRTypeEntity>(this, where, null, out data);
                                if (result.IsSuccess)
                                {
                                    this.EditSchoolRType = data;
                                    isOK = true;
                                }
                            }
                        }
                        #endregion
                        break;
                }
                #endregion

                #region 網頁參數不正確
                if (!isOK)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                this.BindEditData(this.Action, data);
            }
        }

        private void initData(ref SchoolRTypeEntity data)
        {
            data.ReceiveType = "";
            data.SchName = "";
            data.SchId = "";
            data.SchAddress = "";
            data.SchPrincipal = "";
            data.AccountName = "";
            data.BankId = "";
            data.PayOver = "N";
            data.Url = "";
            data.SchIdenty = "";
            data.SchAccount = "";
            data.FtpLocation = "";
            data.FtpPort = "";
            data.FtpAccount = "";

            #region [MDY:20210401] 原碼修正
            #region [MDY:20220503] Checkmarx 調整
            data.FtpPXX = "";
            #endregion
            #endregion

            data.SchPri = "";
            data.SchHAmount = "";
            data.SchPostal = "";
            data.SchContract = "";
            data.SchConTel = "";
            data.SchContract1 = "";
            data.SchConTel1 = "";
            data.EFlag = "N";
            data.AFlag = "N";
            data.SchMail = "";
            data.BankMail = "";
            data.CFlag = 0;
            data.DiviFlag = "0";
            data.UseEduSubsidy = "Y";

            data.DeductId = "";

            #region [MDY:202203XX] 2022擴充案 英文資料啟用、英文名稱
            data.EngEnabled = "N";
            data.SchEName = "";
            #endregion
        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.GetAndBindCancelNoRuleOptions();
            this.GetAndBindCorpTypeOptions();
            this.GetAndBindBankOptions();

            WebHelper.SetRadioButtonListItems(this.rblReceiveKind, new ReceiveKindCodeTexts(), true, 2, ReceiveKindCodeTexts.SCHOOL);

            #region [MDY:202203XX] 2022擴充案 英文資料啟用 欄位
            {
                CodeText[] items = new CodeText[] { new CodeText("Y", "啟用"), new CodeText("N", "停用") };
                WebHelper.SetRadioButtonListItems(this.rblEngEnabled, items, true, 2, null);
            }
            #endregion

            //this.chkPayOver.Checked = false;
            this.tbxSchName.Text = String.Empty;

            #region [MDY:202203XX] 2022擴充案 英文名稱
            this.tbxSchEName.Text = String.Empty;
            #endregion

            this.chkStatus.Checked = false;
            this.tbxSchPrincipal.Text = String.Empty;
            this.chkBigReceiveId.Checked = false;
            this.tbxSchPostal.Text = String.Empty;
            this.tbxSchAddress.Text = String.Empty;
            this.tbxUrl.Text = String.Empty;
            this.tbxSchMail.Text = String.Empty;

            #region [MDY:20211001] M202110_01 設定 FTP 種類 (2021擴充案先做)
            WebHelper.SetDropDownListSelectedValue(this.ddlFtpKind, String.Empty);
            #endregion

            this.tbxFtpLocation.Text = String.Empty;
            this.tbxFtpPort.Text = String.Empty;
            this.tbxFtpAccount.Text = String.Empty;

            #region [MDY:20220503] Checkmarx 調整
            this.tbxFtpPXX.Text = String.Empty;
            #endregion

            this.tbxSchContract.Text = String.Empty;
            this.tbxSchConTel.Text = String.Empty;
            this.tbxSchContract1.Text = String.Empty;
            this.tbxSchConTel1.Text = String.Empty;
            this.tbxBankMail.Text = String.Empty;
            this.tbxAccountName.Text = String.Empty;
            this.tbxSchAccount.Text = String.Empty;
            this.tbxSchHAmount.Text = String.Empty;
            //this.ddlEFlag
            this.chkDiviFlag.Checked = false;
            tbx.Text = String.Empty;        //項(可輸入1~16)
            this.chkCalcSchoolLoan.Checked = false;
            this.chkPostFeeInclude.Checked = false;
            this.tbxPostFee1.Text = String.Empty;
            this.tbxPostFee2.Text = String.Empty;
            this.chkUseStuId20.Checked = false;
            this.chkUseEduSubsidy.Checked = false;
            this.tbxEduSubsidyLabel.Text = String.Empty;

            #region 土銀特有欄位
            this.txtDeductId.Text = String.Empty;
            this.tbxMerchantId.Text = String.Empty;
            this.tbxTerminalId.Text = String.Empty;
            this.tbxMerId.Text = String.Empty;
            #endregion

            #region [MDY:20191214] 2019擴充案 國際信用卡 - 財金特店參數相關欄位
            this.tbxMerchantId2.Text = String.Empty;
            this.tbxTerminalId2.Text = String.Empty;
            this.tbxMerId2.Text = String.Empty;
            this.tbxHandlingFeeRate.Text = String.Empty;
            #endregion

            this.ccbtnOK.Visible = true;
        }

        private void GetAndBindCancelNoRuleOptions()
        {
            #region [Old]
            //CodeText[] items = new CodeText[5];

            //List<CodeText> datas = new List<CodeText>();
            //CodeText item = new CodeText();
            //item.Code = "";
            //item.Text = "請選擇";
            //datas.Add(item);

            //item = new CodeText();
            //item.Code = "M14A";
            //item.Text = "14碼土銀規則(M14A)";
            //datas.Add(item);

            //item = new CodeText();
            //item.Code = "M14B";
            //item.Text = "14碼廠商規則(M14B)";
            //datas.Add(item);

            //item = new CodeText();
            //item.Code = "M14C";
            //item.Text = "14碼無檢碼(M14C)";
            //datas.Add(item);

            //item = new CodeText();
            //item.Code = "M14G";
            //item.Text = "14碼檢查日期(M14G)";
            //datas.Add(item);

            //item = new CodeText();
            //item.Code = "M16A";
            //item.Text = "16碼土銀規則(M16A)";
            //datas.Add(item);

            //item = new CodeText();
            //item.Code = "M16B";
            //item.Text = "16碼廠商規則(M16B)";
            //datas.Add(item);

            //item = new CodeText();
            //item.Code = "M16C";
            //item.Text = "16碼無檢碼(M16C)";
            //datas.Add(item);

            //item = new CodeText();
            //item.Code = "M16D";
            //item.Text = "16碼檢查身分證字號(M16D)";
            //datas.Add(item);

            //item = new CodeText();
            //item.Code = "M16E";
            //item.Text = "16碼檢查統一編號(M16E)";
            //datas.Add(item);

            //item = new CodeText();
            //item.Code = "M16G";
            //item.Text = "16碼檢查日期(M16G)";
            //datas.Add(item);
            //items = datas.ToArray();
            #endregion

            CodeText[] items = CancelNoHelper.Module.GetIdNames();

            WebHelper.SetDropDownListItems(this.ddlCancelNoRule, DefaultItem.Kind.Select, false, items, false, false, 0, null);
        }

        private void GetAndBindCorpTypeOptions()
        {
            CodeText[] items = new CodeText[5];

            List<CodeText> datas = new List<CodeText>();
            CodeText item = new CodeText();
            item.Code = "";
            item.Text = "all";
            datas.Add(item);

            item = new CodeText();
            item.Code = "1";
            item.Text = "大專院校(1)";
            datas.Add(item);

            item = new CodeText();
            item.Code = "2";
            item.Text = "高中職(2)";
            datas.Add(item);

            item = new CodeText();
            item.Code = "3";
            item.Text = "國中小(3)";
            datas.Add(item);

            item = new CodeText();
            item.Code = "4";
            item.Text = "幼兒園(4)";
            datas.Add(item);

            items = datas.ToArray();
            WebHelper.SetDropDownListItems(this.ddlCorpType, DefaultItem.Kind.Select, false, items, false, false, 0, null);

        }

        /// <summary>
        /// 取得並結繫銀行代碼選項
        /// </summary>
        private void GetAndBindBankOptions()
        {
            CodeText[] items = null;
            Expression where = new Expression();
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(BankEntity.Field.BankNo, OrderByEnum.Asc);

            string[] codeFieldNames = new string[] { BankEntity.Field.BankNo };
            string codeCombineFormat = null;
            string[] textFieldNames = new string[] { BankEntity.Field.BankSName };
            string textCombineFormat = null;

            XmlResult xmlResult = DataProxy.Current.GetEntityOptions<BankEntity>(this, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
            if (!xmlResult.IsSuccess)
            {
                string action = this.GetLocalized("查詢銀行代碼資料");
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            if (xmlResult.IsSuccess)
            {
                if (items != null)
                {
                    for (int idx = 0; idx < items.Length; idx++)
                    {
                        items[idx].Text = string.Format("{0}({1})", items[idx].Text, items[idx].Code);
                    }
                }
            }
            WebHelper.SetDropDownListItems(this.ddlBank, DefaultItem.Kind.Select, false, items, false, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlMainBank, DefaultItem.Kind.Select, false, items, false, false, 0, null);
        }

        private void BindEditData(string action, SchoolRTypeEntity data)
        {
            if (data == null)
            {
                this.InitialUI();
                this.ccbtnOK.Visible = false;
                return;
            }

            this.EditSchoolRType = data;

            bool isPKeyEditable = ActionMode.IsPKeyEditableMode(this.Action);
            bool isDataEditable = ActionMode.IsDataEditableMode(this.Action);

            switch (action)
            {
                case ActionMode.Insert:
                    #region 新增
                    {
                        this.tbxReceiveType.Text = string.Empty;
                        this.txtSchIdenty.Text = data.SchIdenty;
                        this.txtSchIdenty.Enabled = String.IsNullOrWhiteSpace(this.txtSchIdenty.Text);
                    }
                    #endregion
                    break;
                default:
                    #region [MDY:20210401] 原碼修正
                    this.tbxReceiveType.Text = HttpUtility.HtmlEncode(this.EditReceiveType);
                    #endregion
                    this.tbxReceiveType.Enabled = isPKeyEditable;
                    this.txtSchIdenty.Text = data.SchIdenty.Trim();
                    this.txtSchIdenty.Enabled = isPKeyEditable;
                    break;
            }

            //this.chkPayOver.Checked = data.PayOver == "Y" ? true : false;
            //this.chkPayOver.Enabled = isDataEditable;
            //this.txtPayOver.Value = data.PayOver;
            //this.txtPayOver.Enabled = isDataEditable;

            WebHelper.SetDropDownListSelectedValue(this.ddlCancelNoRule, data.CancelNoRule);
            this.ddlCancelNoRule.Enabled = isDataEditable;

            WebHelper.SetRadioButtonListSelectedValue(this.rblReceiveKind, data.ReceiveKind);
            this.rblReceiveKind.Enabled = isDataEditable;

            #region [MDY:202203XX] 2022擴充案 英文資料啟用 欄位
            {
                WebHelper.SetRadioButtonListSelectedValue(this.rblEngEnabled, data.EngEnabled);
                this.rblEngEnabled.Enabled = isDataEditable;
            }
            #endregion

            this.tbxSchName.Text = data.SchName;
            this.tbxSchName.Enabled = isDataEditable;

            #region [MDY:202203XX] 2022擴充案 英文名稱
            this.tbxSchEName.Text = data.SchEName;
            this.tbxSchEName.Enabled = isDataEditable;
            #endregion

            this.chkStatus.Checked = data.Status == DataStatusCodeTexts.DISABLED ? true : false;
            this.chkStatus.Enabled = isDataEditable;
            this.tbxSchPrincipal.Text = data.SchPrincipal;
            this.tbxSchPrincipal.Enabled = isDataEditable;
            this.chkBigReceiveId.Checked = data.IsBigReceiveId();
            if (this.chkBigReceiveId.Checked)
            {
                //啟用後無法取消
                this.chkBigReceiveId.Enabled = false;
            }
            else
            {
                this.chkBigReceiveId.Enabled = isDataEditable;
            }
            this.tbxSchPostal.Text = data.SchPostal;
            this.tbxSchPostal.Enabled = isDataEditable;
            this.tbxSchAddress.Text = data.SchAddress;
            this.tbxSchAddress.Enabled = isDataEditable;
            this.tbxUrl.Text = data.Url;
            this.tbxUrl.Enabled = isDataEditable;
            this.tbxSchMail.Text = data.SchMail;
            this.tbxSchMail.Enabled = isDataEditable;

            #region [MDY:20211001] M202110_01 設定 FTP 種類 (2021擴充案先做)
            WebHelper.SetDropDownListSelectedValue(this.ddlFtpKind, data.FtpKind);
            this.ddlFtpKind.Enabled = isDataEditable;
            #endregion

            this.tbxFtpLocation.Text = data.FtpLocation;
            this.tbxFtpLocation.Enabled = isDataEditable;
            this.tbxFtpPort.Text = data.FtpPort;
            this.tbxFtpPort.Enabled = isDataEditable;
            this.tbxFtpAccount.Text = data.FtpAccount;
            this.tbxFtpAccount.Enabled = isDataEditable;

            #region [MDY:20220503] Checkmarx 調整
            #region [MDY:20210401] 原碼修正
            this.tbxFtpPXX.Text = data.FtpPXX;
            #endregion

            this.tbxFtpPXX.Enabled = isDataEditable;
            #endregion

            this.tbxSchContract.Text = data.SchContract;
            this.tbxSchContract.Enabled = isDataEditable;
            this.tbxSchConTel.Text = data.SchConTel;
            this.tbxSchConTel.Enabled = isDataEditable;
            this.tbxSchContract1.Text = data.SchContract1;
            this.tbxSchContract1.Enabled = isDataEditable;
            this.tbxSchConTel1.Text = data.SchConTel1;
            this.tbxSchConTel1.Enabled = isDataEditable;
            this.tbxBankMail.Text = data.BankMail;
            this.tbxBankMail.Enabled = isDataEditable;
            WebHelper.SetDropDownListSelectedValue(this.ddlMainBank, data.BankId.Trim());
            this.ddlMainBank.Enabled = isDataEditable;
            //this.ddlBank.SelectedValue = data.BankId.Trim();
            //this.ddlBank.Enabled = isDataEditable;

            this.tbxAccountName.Text = data.AccountName;
            this.tbxAccountName.Enabled = isDataEditable;
            this.tbxSchAccount.Text = data.SchAccount;
            this.tbxSchAccount.Enabled = isDataEditable;
            //優先順序 ddlPrioity
            this.tbxSchHAmount.Text = data.SchHAmount;
            this.tbxSchHAmount.Enabled = isDataEditable;
            this.ddlEFlag.SelectedValue = "";   //縣市下拉
            this.ddlEFlag.Enabled = isDataEditable;
            //銷帳完帳和產生銷帳編號後、再上傳學生資料
            this.ddlAFlag.SelectedValue = data.AFlag == null ? "" : data.AFlag.ToString();
            this.ddlAFlag.Enabled = isDataEditable;
            //分行控管才可新增代收費用別
            this.ddlCFlag.SelectedValue = data.CFlag == null ? "" : data.CFlag.ToString();
            this.ddlCFlag.Enabled = isDataEditable;
            this.ddlBillFormType.SelectedValue = data.PayStyle == null ? "" : data.PayStyle.ToString();
            this.ddlBillFormType.Enabled = isDataEditable;
            //超商4~6萬使用的識別碼(不知對哪個欄位)
            this.ddlCorpType.SelectedValue = data.CorpType;
            this.ddlCorpType.Enabled = isDataEditable;
            //是否提供分項繳費單旗標 (1=是; 0=否)DiviFlag
            this.chkDiviFlag.Checked = data.DiviFlag == "1" ? true : false;
            this.chkDiviFlag.Enabled = isDataEditable;
            this.tbx.Text = "";        //項(可輸入1~16)
            this.tbx.Enabled = isDataEditable;
            this.chkCalcSchoolLoan.Checked = data.CalcSchoolLoan == "Y" ? true : false;
            this.chkCalcSchoolLoan.Enabled = isDataEditable;
            //郵局手續費(內含)(Y=是; N=否)
            this.chkPostFeeInclude.Checked = data.PostFeeInclude == "Y" ? true : false;
            this.chkPostFeeInclude.Enabled = isDataEditable;
            this.tbxPostFee1.Text = DataFormat.GetAmountText(data.PostFee1);
            this.tbxPostFee1.Enabled = isDataEditable;
            this.tbxPostFee2.Text = DataFormat.GetAmountText(data.PostFee2);
            this.tbxPostFee2.Enabled = isDataEditable;
            //是否使用20碼的學號旗標UseStuId20 (Y=是; N=否)
            this.chkUseStuId20.Checked = data.UseStuId20 == "Y" ? true : false;
            this.chkUseStuId20.Enabled = isDataEditable;
            //使用「教育部補助」UseEduSubsidy
            this.chkUseEduSubsidy.Checked = data.UseEduSubsidy == "Y" ? true : false;
            this.chkUseEduSubsidy.Enabled = isDataEditable;
            this.tbxEduSubsidyLabel.Text = data.EduSubsidyLabel;
            this.tbxEduSubsidyLabel.Enabled = isDataEditable;

            #region 土銀特有欄位
            this.txtDeductId.Text = data.DeductId;
            this.txtDeductId.Enabled = isDataEditable;
            this.tbxMerchantId.Text = data.MerchantId;
            this.tbxMerchantId.Enabled = isDataEditable;
            this.tbxTerminalId.Text = data.TerminalId;
            this.tbxTerminalId.Enabled = isDataEditable;
            this.tbxMerId.Text = data.MerId;
            this.tbxMerId.Enabled = isDataEditable;
            #endregion

            #region [MDY:20191214] 2019擴充案 國際信用卡 - 財金特店參數相關欄位
            this.tbxMerchantId2.Text = data.MerchantId2;
            this.tbxMerchantId2.Enabled = isDataEditable;
            this.tbxTerminalId2.Text = data.TerminalId2;
            this.tbxTerminalId2.Enabled = isDataEditable;
            this.tbxMerId2.Text = data.MerId2;
            this.tbxMerId2.Enabled = isDataEditable;
            this.tbxHandlingFeeRate.Text = data.HandlingFeeRate.HasValue ? data.HandlingFeeRate.Value.ToString("0.######") : String.Empty;
            this.tbxHandlingFeeRate.Enabled = isDataEditable;
            #endregion
        }

        /// <summary>
        /// 取得輸入的維護資料
        /// </summary>
        /// <returns></returns>
        private SchoolRTypeEntity GetEditData()
        {
            SchoolRTypeEntity data = this.EditSchoolRType;

            switch (this.Action)
            {
                case ActionMode.Insert:
                    data.ReceiveType = this.tbxReceiveType.Text.Trim();
                    data.SchIdenty = this.txtSchIdenty.Text.Trim();
                    break;
                case ActionMode.Modify:
                    break;
                default:
                    return data;
            }

            //data.ReceiveType = this.tbxReceiveType.Text.Trim();
            //data.SchIdenty = this.txtSchIdenty.Text.Trim();

            data.CancelNoRule = this.ddlCancelNoRule.SelectedValue;

            string receiveKind = this.rblReceiveKind.SelectedValue;
            if (receiveKind != data.ReceiveKind)
            {
                data.ReceiveKind = this.rblReceiveKind.SelectedValue;
            }

            #region [MDY:202203XX] 2022擴充案 英文資料啟用 欄位
            data.EngEnabled = this.rblEngEnabled.SelectedValue;
            #endregion

            data.PayOver = "N";
            data.SchName = this.tbxSchName.Text.Trim();

            #region [MDY:202203XX] 2022擴充案 英文名稱
            data.SchEName = this.tbxSchEName.Text.Trim();
            #endregion

            data.Status = this.chkStatus.Checked ? DataStatusCodeTexts.DISABLED : DataStatusCodeTexts.NORMAL;
            data.SchPrincipal = this.tbxSchPrincipal.Text.Trim();
            if (!data.IsBigReceiveId())
            {
                //啟用後無法取消，所以未啟用才能更改
                data.BigReceiveIdFlag = this.chkBigReceiveId.Checked ? "Y" : "N";
            }
            data.SchPostal = this.tbxSchPostal.Text.Trim();
            data.SchAddress = this.tbxSchAddress.Text.Trim();
            data.Url = this.tbxUrl.Text.Trim();
            data.SchMail = this.tbxSchMail.Text.Trim();

            #region [MDY:20211001] M202110_01 設定 FTP 種類 (2021擴充案先做)
            data.FtpKind = this.ddlFtpKind.SelectedValue.Trim();
            #endregion

            data.FtpLocation = this.tbxFtpLocation.Text.Trim();
            data.FtpPort = this.tbxFtpPort.Text.Trim();
            data.FtpAccount = this.tbxFtpAccount.Text.Trim();

            #region [MDY:20210401] 原碼修正
            #region [MDY:20220503] Checkmarx 調整
            data.FtpPXX = this.tbxFtpPXX.Text.Trim();
            #endregion
            #endregion

            data.SchContract = this.tbxSchContract.Text.Trim();
            data.SchConTel = this.tbxSchConTel.Text.Trim();
            data.SchContract1 = this.tbxSchContract1.Text.Trim();
            data.SchConTel1 = this.tbxSchConTel1.Text.Trim();
            data.BankMail = this.tbxBankMail.Text.Trim();
            data.BankId = this.ddlMainBank.SelectedValue;
            data.AccountName = this.tbxAccountName.Text.Trim();
            data.SchAccount = this.tbxSchAccount.Text.Trim();
            //優先順序 ddlPrioity
            data.SchHAmount = this.tbxSchHAmount.Text;
            data.EFlag = this.ddlEFlag.SelectedValue;
            data.AFlag = this.ddlAFlag.SelectedValue;
            data.CFlag = Convert.ToInt16(this.ddlCFlag.SelectedValue);
            data.PayStyle = this.ddlBillFormType.SelectedValue;
            //超商4~6萬使用的識別碼
            data.CorpType = this.ddlCorpType.SelectedValue;
            //提供分項繳費單，分[30]項
            //data. = this.tbx.Text;
            //是否提供分項繳費單旗標 (1=是; 0=否)DiviFlag
            data.DiviFlag = this.chkDiviFlag.Checked ? "1" : "0";
            data.CalcSchoolLoan = this.chkCalcSchoolLoan.Checked ? "Y" : "N";
            //郵局手續費(內含)(Y=是; N=否)
            data.PostFeeInclude = this.chkPostFeeInclude.Checked ? "Y" : "N";
            data.PostFee1 = Convert.ToDecimal(this.tbxPostFee1.Text);
            data.PostFee2 = Convert.ToDecimal(this.tbxPostFee2.Text);
            data.UseStuId20 = this.chkUseStuId20.Checked ? "Y" : "N";
            //使用「教育部補助」UseEduSubsidy
            data.UseEduSubsidy = this.chkUseEduSubsidy.Checked ? "Y" : "N";
            data.EduSubsidyLabel = this.tbxEduSubsidyLabel.Text.Trim();

            #region 土銀特有欄位
            data.DeductId = this.txtDeductId.Text.Trim();
            data.MerchantId = this.tbxMerchantId.Text.Trim();
            data.TerminalId = this.tbxTerminalId.Text.Trim();
            data.MerId = this.tbxMerId.Text.Trim();
            #endregion

            #region [MDY:20191214] 2019擴充案 國際信用卡 - 財金特店參數相關欄位
            data.MerchantId2 = this.tbxMerchantId2.Text.Trim();
            data.TerminalId2 = this.tbxTerminalId2.Text.Trim();
            data.MerId2 = this.tbxMerId2.Text.Trim();
            decimal handlingFeeRate = 0;
            if (Decimal.TryParse(this.tbxHandlingFeeRate.Text.Trim(), out handlingFeeRate))
            {
                data.HandlingFeeRate = handlingFeeRate;
            }
            else
            {
                data.HandlingFeeRate = null;
            }
            #endregion

            data.StampSize = 0;
            data.UseVerify = string.Empty;
            data.UseStuRemark = "N";
            return data;
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            LogonUser logonUser = this.GetLogonUser();
            SchoolRTypeEntity data = this.GetEditData();
            if (!this.CheckEditData(data))
            {
                return;
            }

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "S5100001.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    int count = 0;
                    //郵局手續費金額
                    data.UseNewPostCharge = string.Empty;
                    data.UseStamp = string.Empty;
                    data.UsePostDueDate = string.Empty;
                    data.UseMultiCopy = string.Empty;
                    data.UseUTF8 = string.Empty;
                    data.UseP16PDF = string.Empty;
                    data.UseStuRemark = string.Empty;
                    data.UseVerify = string.Empty;
                    data.StampSize = 0;
                    data.PostFee = 0;
                    data.Status = DataStatusCodeTexts.NORMAL;
                    data.CrtDate = DateTime.Now;
                    data.CrtUser = logonUser.UserId;
                    XmlResult xmlResult = DataProxy.Current.Insert<SchoolRTypeEntity>(this, data, out count);
                    if (xmlResult.IsSuccess)
                    {
                        if (count < 1)
                        {
                            this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
                        }
                        else
                        {
                            //新增此學年下的 所有代收類別的第一學期 第二學期                                
                            object returnData = null;
                            KeyValue<string>[] arguments = new KeyValue<string>[] {
                                        new KeyValue<string>("Type", "2"),
                                        new KeyValue<string>("insertKey", data.ReceiveType)
                                    };
                            xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.InsertTermListDatas, arguments, out returnData);
                            if (xmlResult.IsSuccess)
                            {
                                WebHelper.SetFilterArguments(data.ReceiveType, string.Empty, string.Empty, string.Empty, string.Empty);
                                this.ShowActionSuccessAlert(action, backUrl);
                            }
                            else
                            {
                                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                            }
                        }
                    }
                    else
                    {
                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    }
                    #endregion
                    break;
                case ActionMode.Modify:     //修改
                    #region 修改
                    count = 0;
                    data.MdyDate = DateTime.Now;
                    data.MdyUser = logonUser.UserId;
                    xmlResult = DataProxy.Current.Update<SchoolRTypeEntity>(this, data, out count);
                    if (xmlResult.IsSuccess)
                    {
                        if (count < 1)
                        {
                            this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                        }
                        else
                        {
                            WebHelper.SetFilterArguments(data.ReceiveType, string.Empty, string.Empty, string.Empty, string.Empty);

                            this.ShowActionSuccessAlert(action, backUrl);
                        }
                    }
                    else
                    {
                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    }

                    #endregion
                    break;
                case ActionMode.Delete:     //刪除
                    #region 刪除
                    count = 0;
                    xmlResult = DataProxy.Current.Delete<SchoolRTypeEntity>(this, data, out count);
                    if (xmlResult.IsSuccess)
                    {
                        if (count < 1)
                        {
                            this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                        }
                        else
                        {
                            WebHelper.SetFilterArguments(data.ReceiveType, string.Empty, string.Empty, string.Empty, string.Empty);

                            this.ShowActionSuccessAlert(action, backUrl);
                        }
                    }
                    else
                    {
                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    }
                    #endregion
                    break;
            }
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData(SchoolRTypeEntity data)
        {
            if (this.Action == ActionMode.Delete)
            {
                return true;
            }

            #region 學校中文名稱
            if (String.IsNullOrEmpty(data.SchName))
            {
                this.ShowMustInputAlert("學校中文名稱");
                return false;
            }
            #endregion

            #region 代收種類
            if (String.IsNullOrEmpty(data.ReceiveKind))
            {
                this.ShowMustInputAlert("代收種類");
                return false;
            }
            #endregion

            #region [MDY:202203XX] 2022擴充案 英文資料啟用 欄位
            if (String.IsNullOrEmpty(data.EngEnabled))
            {
                this.ShowMustInputAlert("英文資料啟用");
                return false;
            }
            #endregion

            #region 學校英文名稱
            if (data.EngEnabled == "Y" && String.IsNullOrEmpty(data.SchEName))
            {
                this.ShowMustInputAlert("學校英文名稱");
                return false;
            }
            #endregion

            #region 檢碼規則
            if (String.IsNullOrEmpty(data.CancelNoRule) && data.ReceiveKind != ReceiveKindCodeTexts.UPCTCB)
            {
                this.ShowMustInputAlert("檢碼規則");
                return false;
            }
            #endregion

            #region 學校代碼
            if (String.IsNullOrEmpty(data.SchIdenty))
            {
                this.ShowMustInputAlert("學校代碼");
                return false;
            }
            else if (!Common.IsNumber(data.SchIdenty, 4))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("學校代碼必須為 4 碼的數字");
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion

            #region 商家代號
            if (String.IsNullOrEmpty(data.ReceiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return false;
            }
            else if (!Common.IsNumber(data.ReceiveType, 4))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("商家代號必須為 4 碼的數字");
                this.ShowSystemMessage(msg);
                return false;
            }

            #region [MDY:20161124] 檢查商家代號是否落在需要次應用代碼的區間
            if (DataFormat.CheckHasSubAppNo(data.ReceiveType))
            {
                string msg = String.Format("學雜費不支援 {0} ~ {1} 的商家代號", DataFormat.MinAppNoForHasSub, DataFormat.MaxAppNoForHasSub);
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion
            #endregion

            #region [MDY:20211001] M202110_01 設定 FTP 種類 (2021擴充案先做)
            if (String.IsNullOrEmpty(data.FtpKind))
            {
                #region [MDY:20220503] Checkmarx 調整
                if (!String.IsNullOrEmpty(data.FtpLocation) || !String.IsNullOrEmpty(data.FtpPort)
                    || !String.IsNullOrEmpty(data.FtpAccount) || !String.IsNullOrEmpty(data.FtpPXX))
                {
                    string msg = this.GetLocalized("未指定學校FTP種類時不可指定學校FTP伺服器、學校FTP埠、學校FTP帳號、學校FTP密碼");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                #endregion
            }
            else
            {
                #region [MDY:20220503] Checkmarx 調整
                if (String.IsNullOrEmpty(data.FtpLocation) || String.IsNullOrEmpty(data.FtpPort)
                    || String.IsNullOrEmpty(data.FtpAccount) || String.IsNullOrEmpty(data.FtpPXX))
                {
                    string msg = this.GetLocalized("指定學校FTP種類時必須同時指定學校FTP伺服器、學校FTP埠、學校FTP帳號、學校FTP密碼");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                #endregion

                Int32 value = 0;
                if (!Int32.TryParse(data.FtpPort, out value) || value < 1)
                {
                    string msg = this.GetLocalized("學校FTP埠必須為大於 0 的整數");
                    this.ShowSystemMessage(msg);
                    return false;
                }
            }
            #endregion

            #region 主辦分行
            if (String.IsNullOrEmpty(data.BankId))
            {
                this.ShowMustInputAlert("主辦分行");
                return false;
            }
            #endregion

            #region [Old] 郵局手續費
            //if (String.IsNullOrEmpty(data.PostFee1.ToString()))
            //{
            //    this.ShowMustInputAlert("郵局手續費1");
            //    return false;
            //}

            //if (String.IsNullOrEmpty(data.PostFee2.ToString()))
            //{
            //    this.ShowMustInputAlert("郵局手續費2");
            //    return false;
            //}

            //if (!Common.IsMoney(data.PostFee1.ToString()))
            //{
            //    string msg = this.GetLocalized("「郵局手續費」不是合法的金額");
            //    this.ShowSystemMessage(msg);
            //    return false;
            //}

            //if (!Common.IsMoney(data.PostFee2.ToString()))
            //{
            //    string msg = this.GetLocalized("「郵局手續費」不是合法的金額");
            //    this.ShowSystemMessage(msg);
            //    return false;
            //}
            #endregion

            #region 財金參數
            bool hasMerchantId = !String.IsNullOrEmpty(data.MerchantId);
            bool hasTerminalId = !String.IsNullOrEmpty(data.TerminalId);
            bool hasMerId = !String.IsNullOrEmpty(data.MerId);
            if (hasMerchantId != hasTerminalId || hasMerchantId != hasMerId)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("財金特店代碼、財金端末機代號、財金特店編號參數必須同時都有設定值或都不設定");
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion

            #region [MDY:20191214] 2019擴充案 國際信用卡 - 財金特店參數相關欄位
            bool hasMerchantId2 = !String.IsNullOrEmpty(data.MerchantId2);
            bool hasTerminalId2 = !String.IsNullOrEmpty(data.TerminalId2);
            bool hasMerId2 = !String.IsNullOrEmpty(data.MerId2);
            if (hasMerchantId2 != hasTerminalId2 || hasMerchantId2 != hasMerId2 || hasMerId2 != data.HandlingFeeRate.HasValue)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("國際信用卡 - 財金特店參數的特店代碼、端末機代號、特店編號、手續費率必須同時都有設定值或都不設定");
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion

            return true;
        }
    }
}