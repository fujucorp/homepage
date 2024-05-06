using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.B
{
    /// <summary>
    /// 下載學生繳費資料媒體檔
    /// </summary>
    public partial class B2100003 : BasePage
    {
        private const string MyExportConfigKind = ExportConfigKindCodeTexts.StudentBill;

        /// <summary>
        /// 備註數量
        /// </summary>
        private const int MemoCount = StudentReceiveEntity.MemoCount;

        #region Property
        /// <summary>
        /// 儲存查詢的商家代號代碼
        /// </summary>
        private string QueryReceiveType
        {
            get
            {
                return ViewState["QueryReceiveType"] as string;
            }
            set
            {
                ViewState["QueryReceiveType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的學年參數
        /// </summary>
        private string QueryYearId
        {
            get
            {
                return ViewState["QueryYearId"] as string;
            }
            set
            {
                ViewState["QueryYearId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的學期參數
        /// </summary>
        private string QueryTermId
        {
            get
            {
                return ViewState["QueryTermId"] as string;
            }
            set
            {
                ViewState["QueryTermId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的部別參數
        /// </summary>
        private string QueryDepId
        {
            get
            {
                return ViewState["QueryDepId"] as string;
            }
            set
            {
                ViewState["QueryDepId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的代收費用別參數
        /// </summary>
        private string QueryReceiveId
        {
            get
            {
                return ViewState["QueryReceiveId"] as string;
            }
            set
            {
                ViewState["QueryReceiveId"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private bool InitialUI()
        {
            #region 處理五個下拉選項
            string receiveType = null;
            string yearId = null;
            string termId = null;
            string depId = null;
            string receiveId = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearId, out termId, out depId, out receiveId)
                || String.IsNullOrEmpty(receiveType)
                || (this.ucFilter1.YearVisible && String.IsNullOrEmpty(yearId))
                || (this.ucFilter1.TermVisible && String.IsNullOrEmpty(termId)))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得商家代號、學年或學期參數");
                this.ShowSystemMessage(msg);
                return false;
            }

            //因為控制項的 Page_Load 比頁面的 Page_Load 晚觸發，所以可以先執行 GetDataAndBind 方法，強迫 ucFilter1 結繫資料
            //因為 ucFilter1 有指定 Filter2ControlID 為 ucFilter2，所以 ucFilter2 頁會被自動結繫資料
            XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearId, termId, depId, receiveId);
            if (xmlResult.IsSuccess)
            {
                receiveId = this.ucFilter2.SelectedReceiveID;
            }
            this.QueryReceiveType = receiveType;
            this.QueryYearId = yearId;
            this.QueryTermId = termId;
            this.QueryDepId = "";
            this.QueryReceiveId = this.ucFilter2.SelectedReceiveID;
            #endregion

            this.ucFilter2_ItemSelectedIndexChanged(this.ucFilter2, null);

            #region 銷帳狀態選項
            WebHelper.SetDropDownListItems(this.ddlCancelStatus, DefaultItem.Kind.All, false, new CancelStatusCodeTexts(), false, true, 0, null);
            #endregion

            #region 繳款方式選項
            {
                Expression where = new Expression();
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(ChannelSetEntity.Field.ChannelId, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { ChannelSetEntity.Field.ChannelId };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { ChannelSetEntity.Field.ChannelName };
                string textCombineFormat = null;

                CodeText[] datas = null;
                XmlResult xmlResult2 = DataProxy.Current.GetEntityOptions<ChannelSetEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);

                //CodeText[] items = new CodeText[] { new CodeText("1", "超商"), new CodeText("2", "ATM"), new CodeText("3", "臨櫃") };
                WebHelper.SetDropDownListItems(this.ddlReceiveWay, DefaultItem.Kind.All, false, datas, true, false, 0, null);
            }
            #endregion

            #region 查詢特定欄位值
            {
                CodeText[] items = new CodeText[] { new CodeText("StuId", "學號"), new CodeText("CancelNo", "虛擬帳號"), new CodeText("IdNumber", "身分證字號") };
                WebHelper.SetRadioButtonListItems(this.rdoSearchField, items, true, 2, items[0].Code);
            }
            #endregion

            #region 資料項目設定
            this.GetAndBindExportConfigOption(this.QueryReceiveType);

            this.GetAndBindExportConfigData(null);
            #endregion

            return true;
        }

        /// <summary>
        /// 取得欄位 與 CheckBox 對照清單
        /// </summary>
        /// <returns></returns>
        private KeyValueList<CheckBox> GetFieldCheckBoxs()
        {
            KeyValueList<CheckBox> fieldCbxs = new KeyValueList<CheckBox>();

            #region 學年學期
            fieldCbxs.Add(StudentReceiveView4.Field.YearId, this.chkYearId);
            fieldCbxs.Add(StudentReceiveView4.Field.YearName, this.chkYearName);
            fieldCbxs.Add(StudentReceiveView4.Field.TermId, this.chkTermId);
            fieldCbxs.Add(StudentReceiveView4.Field.TermName, this.chkTermName);
            #endregion

            #region 學生基本資料
            fieldCbxs.Add(StudentReceiveView4.Field.StuId, this.chkStu_Id);
            fieldCbxs.Add(StudentReceiveView4.Field.StuName, this.chkStu_Name);
            fieldCbxs.Add(StudentReceiveView4.Field.StuIdNumber, this.chkId_Number);
            fieldCbxs.Add(StudentReceiveView4.Field.StuBirthday, this.chkStu_Birthday);
            fieldCbxs.Add(StudentReceiveView4.Field.StuTel, this.chkStu_Tel);
            fieldCbxs.Add(StudentReceiveView4.Field.StuZipCode, this.chkStu_Addcode);
            fieldCbxs.Add(StudentReceiveView4.Field.StuAddress, this.chkStu_Add);
            fieldCbxs.Add(StudentReceiveView4.Field.StuEmail, this.chkEmail);
            fieldCbxs.Add(StudentReceiveView4.Field.StuParent, this.chkStuParent);
            #endregion

            #region [MDY:20160213] 序號、繳款期限
            fieldCbxs.Add(StudentReceiveView4.Field.OldSeq, this.chkOld_Seq);
            fieldCbxs.Add(StudentReceiveView4.Field.PayDueDate, this.chkPay_Due_Date);
            #endregion

            #region 學籍資料
            fieldCbxs.Add(StudentReceiveView4.Field.StuGrade, this.chkStu_Grade);
            fieldCbxs.Add(StudentReceiveView4.Field.StuHid, this.chkStu_Hid);

            fieldCbxs.Add(StudentReceiveView4.Field.ClassId, this.chkClass_Id);
            fieldCbxs.Add(StudentReceiveView4.Field.ClassName, this.chkClass_Name);
            fieldCbxs.Add(StudentReceiveView4.Field.DeptId, this.chkDept_Id);
            fieldCbxs.Add(StudentReceiveView4.Field.DeptName, this.chkDept_Name);
            fieldCbxs.Add(StudentReceiveView4.Field.CollegeId, this.chkCollege_Id);
            fieldCbxs.Add(StudentReceiveView4.Field.CollegeName, this.chkCollege_Name);
            fieldCbxs.Add(StudentReceiveView4.Field.MajorId, this.chkMajor_Id);
            fieldCbxs.Add(StudentReceiveView4.Field.MajorName, this.chkMajor_Name);
            fieldCbxs.Add(StudentReceiveView4.Field.ReduceId, this.chkReduce_Id);
            fieldCbxs.Add(StudentReceiveView4.Field.ReduceName, this.chkReduce_Name);
            fieldCbxs.Add(StudentReceiveView4.Field.LoanId, this.chkLoan_Id);
            fieldCbxs.Add(StudentReceiveView4.Field.LoanName, this.chkLoan_Name);
            fieldCbxs.Add(StudentReceiveView4.Field.DormId, this.chkDorm_Id);
            fieldCbxs.Add(StudentReceiveView4.Field.DormName, this.chkDorm_Name);

            fieldCbxs.Add(StudentReceiveView4.Field.IdentifyId01, this.chkIdentify_Id1);
            fieldCbxs.Add(StudentReceiveView4.Field.IdentifyName01, this.chkIdentify_Name1);
            fieldCbxs.Add(StudentReceiveView4.Field.IdentifyId02, this.chkIdentify_Id2);
            fieldCbxs.Add(StudentReceiveView4.Field.IdentifyName02, this.chkIdentify_Name2);
            fieldCbxs.Add(StudentReceiveView4.Field.IdentifyId03, this.chkIdentify_Id3);
            fieldCbxs.Add(StudentReceiveView4.Field.IdentifyName03, this.chkIdentify_Name3);
            fieldCbxs.Add(StudentReceiveView4.Field.IdentifyId04, this.chkIdentify_Id4);
            fieldCbxs.Add(StudentReceiveView4.Field.IdentifyName04, this.chkIdentify_Name4);
            fieldCbxs.Add(StudentReceiveView4.Field.IdentifyId05, this.chkIdentify_Id5);
            fieldCbxs.Add(StudentReceiveView4.Field.IdentifyName05, this.chkIdentify_Name5);
            fieldCbxs.Add(StudentReceiveView4.Field.IdentifyId06, this.chkIdentify_Id6);
            fieldCbxs.Add(StudentReceiveView4.Field.IdentifyName06, this.chkIdentify_Name6);
            #endregion

            #region 繳費資料
            fieldCbxs.Add(StudentReceiveView4.Field.StuHour, this.chkStu_Hour);
            fieldCbxs.Add(StudentReceiveView4.Field.StuCredit, this.chkStu_Credit);
            fieldCbxs.Add(StudentReceiveView4.Field.LoanAmount, this.chkLoan_Amount);

            fieldCbxs.Add("Receive", this.chkReceive);          //收入科目金額
            fieldCbxs.Add("ReceiveSum", this.chkReceiveSum);    //小計金額
            fieldCbxs.Add("ZBarcode", this.chkZBarcode);        //超商條碼

            fieldCbxs.Add(StudentReceiveView4.Field.SeriorNo, this.chkSerior_No);

            fieldCbxs.Add(StudentReceiveView4.Field.ReceiveAmount, this.chkReceive_Amount);
            fieldCbxs.Add(StudentReceiveView4.Field.CancelNo, this.chkCancel_No);
            fieldCbxs.Add(StudentReceiveView4.Field.ReceiveSmamount, this.chkReceive_SMAmount);
            fieldCbxs.Add(StudentReceiveView4.Field.CancelSmno, this.chkCancel_SMNo);
            #endregion

            #region 轉帳資料
            fieldCbxs.Add(StudentReceiveView4.Field.DeductBankid, this.chkDeduct_BankID);
            fieldCbxs.Add(StudentReceiveView4.Field.DeductAccountno, this.chkDeduct_AccountNo);
            fieldCbxs.Add(StudentReceiveView4.Field.DeductAccountname, this.chkDeduct_AccountName);
            fieldCbxs.Add(StudentReceiveView4.Field.DeductAccountid, this.chkDeduct_AccountId);
            #endregion

            #region 備註
            fieldCbxs.Add(StudentReceiveView4.Field.Memo01, this.chkMemo01);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo02, this.chkMemo02);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo03, this.chkMemo03);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo04, this.chkMemo04);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo05, this.chkMemo05);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo06, this.chkMemo06);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo07, this.chkMemo07);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo08, this.chkMemo08);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo09, this.chkMemo09);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo10, this.chkMemo10);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo11, this.chkMemo11);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo12, this.chkMemo12);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo13, this.chkMemo13);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo14, this.chkMemo14);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo15, this.chkMemo15);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo16, this.chkMemo16);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo17, this.chkMemo17);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo18, this.chkMemo18);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo19, this.chkMemo19);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo20, this.chkMemo20);
            fieldCbxs.Add(StudentReceiveView4.Field.Memo21, this.chkMemo21);
            #endregion

            #region 繳款狀態
            fieldCbxs.Add(StudentReceiveView4.Field.ReceiveDate, this.chkReceive_Date);
            fieldCbxs.Add(StudentReceiveView4.Field.AccountDate, this.chkAccount_Date);
            fieldCbxs.Add(StudentReceiveView4.Field.ReceiveWay, this.chkReceive_Way);
            fieldCbxs.Add(StudentReceiveView4.Field.ReceiveWayName, this.chkReceive_Way_Name);
            #endregion

            return fieldCbxs;
        }

        /// <summary>
        /// 讀取並結繫匯出資料項目設定選項
        /// </summary>
        /// <param name="receiveType"></param>
        private void GetAndBindExportConfigOption(string receiveType)
        {
            CodeText[] datas = null;
            CodeTextList saveDatas = new CodeTextList(99);
            if (!String.IsNullOrWhiteSpace(receiveType))
            {
                string kind = MyExportConfigKind;
                Expression where = new Expression(ExportConfigEntity.Field.ReceiveType, receiveType)
                    .And(ExportConfigEntity.Field.Kind, kind);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(ExportConfigEntity.Field.Id, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { ExportConfigEntity.Field.Id };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { ExportConfigEntity.Field.Name };
                string textCombineFormat = null;

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<ExportConfigEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowActionFailureMessage("讀取匯出資料項目設定檔", xmlResult.Message);
                }

                if (datas != null && datas.Length > 0)
                {
                    for (int no = 1; no <= 99; no++)
                    {
                        string code = ExportConfigEntity.GenID(receiveType, kind, no);
                        CodeText data = Array.Find<CodeText>(datas, x => x.Code == code);
                        if (data == null)
                        {
                            string id = ExportConfigEntity.GenID(receiveType, kind, no);
                            string name = String.Format("{0:00} - 未設定", no);
                            data = new CodeText(id, name);
                        }
                        else
                        {
                            data.Text = String.Format("{0:00} - {1}", no, data.Text);
                        }
                        //string text = String.Format("{0:00} - 未設定", no);
                        saveDatas.Add(data);
                    }
                }
                else
                {
                    for (int no = 1; no <= 99; no++)
                    {
                        string id = ExportConfigEntity.GenID(receiveType, kind, no);
                        string name = String.Format("{0:00} - 未設定", no);
                        saveDatas.Add(id, name);
                    }
                }
            }

            WebHelper.SetDropDownListItems(this.ddlLoadExportConfig, DefaultItem.Kind.Select, false, datas, false, false, 0, null);
            WebHelper.SetDropDownListItems(this.ddlSaveExportConfig, DefaultItem.Kind.None, false, saveDatas, false, false, 0, null);
        }

        /// <summary>
        /// 讀取並結繫匯出資料項目設定資料
        /// </summary>
        /// <param name="id"></param>
        private void GetAndBindExportConfigData(string id)
        {
            string[] outFields = null;
            if (!String.IsNullOrEmpty(id))
            {
                ExportConfigEntity data = null;
                string receiveType = this.QueryReceiveType;
                string kind = MyExportConfigKind;
                Expression where = new Expression(ExportConfigEntity.Field.Id, id)
                    .And(ExportConfigEntity.Field.ReceiveType, receiveType)
                    .And(ExportConfigEntity.Field.Kind, kind);
                XmlResult xmlResult = DataProxy.Current.SelectFirst<ExportConfigEntity>(this.Page, where, null, out data);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowActionFailureMessage("讀取匯出料項目設定", xmlResult.Message);
                }
                else if (data != null)
                {
                    outFields = data.FieldData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
            }

            this.BindExportConfigData(outFields);
        }

        ///// <summary>
        ///// 讀取並結繫匯出資料項目設定資料
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="kind"></param>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //private bool GetExportConfigData(string id, string kind, out ExportConfigEntity data)
        //{
        //    data = null;
        //    Expression where = new Expression(ExportConfigEntity.Field.Id, id)
        //        .And(ExportConfigEntity.Field.Kind, kind);
        //    XmlResult xmlResult = DataProxy.Current.SelectFirst<ExportConfigEntity>(this.Page, where, null, out data);
        //    if (xmlResult.IsSuccess)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        this.ShowActionFailureMessage("讀取匯出料項目設定", xmlResult.Message);
        //        return false;
        //    }
        //}

        /// <summary>
        /// 結繫匯出資料項目資料
        /// </summary>
        /// <param name="outFields"></param>
        private void BindExportConfigData(ICollection<string> outFields)
        {
            //欄位 與 CheckBox 對照清單
            KeyValueList<CheckBox> fieldCbxs = this.GetFieldCheckBoxs();

            #region 勾選項目
            if (outFields == null || outFields.Count == 0)
            {
                foreach (KeyValue<CheckBox> fieldCbx in fieldCbxs)
                {
                    fieldCbx.Value.Checked = false;
                }
            }
            else
            {
                foreach (KeyValue<CheckBox> fieldCbx in fieldCbxs)
                {
                    fieldCbx.Value.Checked = outFields.Contains(fieldCbx.Key);
                }
            }
            #endregion
        }

        /// <summary>
        /// 儲存匯出資料設定
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        private void SaveExportConfigData(string id, string name)
        {
            if (String.IsNullOrEmpty(id) || id.Length < 8)
            {
                this.ShowMustInputAlert("存入資料項目設定");
                return;
            }

            if (String.IsNullOrEmpty(name))
            {
                this.ShowMustInputAlert("存入資料項目設定的名稱");
                return;
            }

            #region 匯出資料項目
            List<string> outFields = new List<string>();
            {
                //欄位 與 CheckBox 對照清單
                KeyValueList<CheckBox> fieldCbxs = this.GetFieldCheckBoxs();

                #region 勾選項目
                foreach (KeyValue<CheckBox> fieldCbx in fieldCbxs)
                {
                    if (fieldCbx.Value.Checked)
                    {
                        outFields.Add(fieldCbx.Key);
                    }
                }
                #endregion

                if (outFields.Count == 0)
                {
                    this.ShowMustInputAlert("匯出資料項目");
                    return;
                }
            }
            #endregion

            string receiveType = this.QueryReceiveType;
            string kind = MyExportConfigKind;
            string no = id.Substring(id.Length - 2);

            ExportConfigEntity data = null;
            Expression where = new Expression(ExportConfigEntity.Field.Id, id);
            XmlResult xmlResult = DataProxy.Current.SelectFirst<ExportConfigEntity>(this.Page, where, null, out data);
            if (!xmlResult.IsSuccess)
            {
                this.ShowActionFailureMessage("讀取匯出料項目設定", xmlResult.Message);
                return;
            }

            string action = this.GetLocalized("儲存匯出料項目設定");
            if (data == null)
            {
                #region 新增
                data = new ExportConfigEntity();
                data.Id = id;
                data.ReceiveType = receiveType;
                data.Kind = kind;
                data.No = no;
                data.Name = name;
                data.FieldData = String.Join(",", outFields);
                data.Status = DataStatusCodeTexts.NORMAL;
                data.CrtDate = DateTime.Now;
                data.CrtUser = this.GetLogonUser().UserId;
                int count = 0;
                xmlResult = DataProxy.Current.Insert<ExportConfigEntity>(this.Page, data, out count);
                if (xmlResult.IsSuccess)
                {
                    if (count < 1)
                    {
                        this.ShowActionFailureMessage(action, "無資料被新增");
                    }
                    else
                    {
                        this.ShowActionSuccessMessage(action);
                        this.GetAndBindExportConfigOption(receiveType);
                        this.tbxSaveExportConfigName.Text = String.Empty;
                    }
                }
                else
                {
                    this.ShowActionFailureMessage(action, xmlResult.Message);
                }
                #endregion
            }
            else
            {
                if (data.ReceiveType != receiveType || data.Kind != kind || data.No != no)
                {
                    this.ShowErrorMessage(CoreStatusCode.UNKNOWN_ERROR, "設定代碼不正確");
                    return;
                }

                #region 修改
                where = new Expression(ExportConfigEntity.Field.Id, id);
                KeyValue[] fieldValues = new KeyValue[] {
                    new KeyValue(ExportConfigEntity.Field.Name, name),
                    new KeyValue(ExportConfigEntity.Field.FieldData, String.Join(",", outFields)),
                    new KeyValue(ExportConfigEntity.Field.MdyDate, DateTime.Now),
                    new KeyValue(ExportConfigEntity.Field.MdyUser, this.GetLogonUser().UserId),
                };
                int count = 0;
                xmlResult = DataProxy.Current.UpdateFields<ExportConfigEntity>(this.Page, where, fieldValues, out count);
                if (xmlResult.IsSuccess)
                {
                    if (count < 1)
                    {
                        this.ShowActionFailureMessage(action, "無資料被更新");
                    }
                    else
                    {
                        this.ShowActionSuccessMessage(action);
                        this.GetAndBindExportConfigOption(receiveType);
                        this.tbxSaveExportConfigName.Text = String.Empty;
                    }
                }
                else
                {
                    this.ShowActionFailureMessage(action, xmlResult.Message);
                }
                #endregion
            }

        }

        /// <summary>
        /// 取得並結繫上傳批號選項
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        private void GetAndBindUpNoOptions(string receiveType, string yearId, string termId, string depId, string receiveId)
        {
            #region 取資料
            CodeText[] items = null;

            #region [Old] 土銀不使用原有部別 DepList，所以 DepId 固定為空字串
            //if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearId) && !String.IsNullOrEmpty(termId)
            //    && !String.IsNullOrEmpty(depId) && !String.IsNullOrEmpty(receiveId))
            #endregion

            if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearId)
                && !String.IsNullOrEmpty(termId) && !String.IsNullOrEmpty(receiveId))
            {
                Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
                    .And(StudentReceiveEntity.Field.YearId, yearId)
                    .And(StudentReceiveEntity.Field.TermId, termId)
                    .And(StudentReceiveEntity.Field.DepId, depId)
                    .And(StudentReceiveEntity.Field.ReceiveId, receiveId)
                    .And(StudentReceiveEntity.Field.UpNo, RelationEnum.NotEqual, String.Empty);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(StudentReceiveEntity.Field.UpNo, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { StudentReceiveEntity.Field.UpNo };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { StudentReceiveEntity.Field.UpNo };
                string textCombineFormat = null;

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<StudentReceiveEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("查詢上傳批號資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion

            #region [MDY:2018xxxx] 批號改用數值遞減排序
            if (items != null)
            {
                WebHelper.SortItemsByValueDesc(ref items);
            }
            #endregion
            WebHelper.SetDropDownListItems(this.ddlUpNo, DefaultItem.Kind.All, false, items, false, false, 0, null);
        }

        /// <summary>
        /// 取得並結繫備註的標題
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        private void GetAndBindMemoTitles(string receiveType, string yearId, string termId, string depId, string receiveId)
        {
            #region 取資料
            SchoolRidEntity schoolRid = null;
            Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, receiveType)
                .And(SchoolRidEntity.Field.YearId, yearId)
                .And(SchoolRidEntity.Field.TermId, termId)
                .And(SchoolRidEntity.Field.DepId, depId)
                .And(SchoolRidEntity.Field.ReceiveId, receiveId);
            XmlResult result = DataProxy.Current.SelectFirst<SchoolRidEntity>(this, where, null, out schoolRid);
            if (!result.IsSuccess)
            {
                this.ShowActionFailureMessage("查詢代收費用設定資料", result.Code, result.Message);
            }
            else if (schoolRid == null)
            {
                this.ShowErrorMessage(ErrorCode.D_DATA_NOT_FOUND, "無該代收費用設定資料");
            }
            #endregion

            HtmlTableRow[] trMemoRows = new System.Web.UI.HtmlControls.HtmlTableRow[] {
                this.trMemoRow1, this.trMemoRow2, this.trMemoRow3, this.trMemoRow4, this.trMemoRow5, this.trMemoRow6
            };
            CheckBox[] cbxMemos = new CheckBox[MemoCount] {
                this.chkMemo01, this.chkMemo02, this.chkMemo03, this.chkMemo04, this.chkMemo05,
                this.chkMemo06, this.chkMemo07, this.chkMemo08, this.chkMemo09, this.chkMemo10,
                this.chkMemo11, this.chkMemo12, this.chkMemo13, this.chkMemo14, this.chkMemo15,
                this.chkMemo16, this.chkMemo17, this.chkMemo18, this.chkMemo19, this.chkMemo20,
                this.chkMemo21
            };

            #region [MDY:202203XX] 2022擴充案 備註項目 改寫，改用 GetAllMemoTitleChts()
            #region [OLD]
            //string[] memoTitles = schoolRid == null ? null : schoolRid.GetAllMemoTitles();
            #endregion

            string[] memoTitles = schoolRid?.GetAllMemoTitleChts();
            #endregion

            if (memoTitles == null || memoTitles.Length == 0)
            {
                foreach (HtmlTableRow trMemoRow in trMemoRows)
                {
                    trMemoRow.Visible = false;
                }
                foreach (CheckBox cbxMemo in cbxMemos)
                {
                    cbxMemo.Text = String.Empty;
                    cbxMemo.Visible = false;
                }
            }
            else
            {
                //要先將上層物件 Visible 設為 true，否則這一層的 Visible 無法設為 true
                foreach (HtmlTableRow trMemoRow in trMemoRows)
                {
                    trMemoRow.Visible = true;
                }

                for (int idx = 0; idx < cbxMemos.Length; idx++)
                {
                    CheckBox cbxMemo = cbxMemos[idx];
                    string memoTitle = null;
                    if (idx < memoTitles.Length)
                    {
                        memoTitle = memoTitles[idx];
                    }
                    if (String.IsNullOrWhiteSpace(memoTitle))
                    {
                        cbxMemo.Text = String.Empty;
                        cbxMemo.Visible = false;
                    }
                    else
                    {
                        cbxMemo.Text = memoTitle.Trim();
                        cbxMemo.Visible = true;
                    }
                }

                int no = 0;
                foreach (HtmlTableRow trMemoRow in trMemoRows)
                {
                    no++;
                    bool isVisible = false;
                    for (int idx = (no - 1) * 4; idx < cbxMemos.Length; idx++)
                    {
                        if (cbxMemos[idx].Visible)
                        {
                            isVisible = true;
                            break;
                        }
                    }
                    trMemoRow.Visible = isVisible;
                }

                #region [Old]
                //for (int idx = 0; idx < memoTitles.Length; idx++)
                //{
                //    CheckBox cbxMemo = cbxMemos[idx];
                //    string memoTitle = (idx < memoTitles.Length) ? memoTitles[idx] : null;
                //    if (String.IsNullOrWhiteSpace(memoTitle))
                //    {
                //        cbxMemo.Text = String.Empty;
                //        cbxMemo.Visible = false;
                //    }
                //    else
                //    {
                //        cbxMemo.Text = memoTitle.Trim();
                //        cbxMemo.Visible = true;
                //    }
                //}
                //this.trMemoRow1.Visible = this.chkMemo01.Visible || this.chkMemo02.Visible || this.chkMemo03.Visible || this.chkMemo04.Visible;
                //this.trMemoRow2.Visible = this.chkMemo05.Visible || this.chkMemo06.Visible || this.chkMemo07.Visible || this.chkMemo08.Visible;
                //this.trMemoRow3.Visible = this.chkMemo09.Visible || this.chkMemo10.Visible;
                #endregion
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                bool isOK = this.InitialUI();
                this.ccbtnDownload.Visible = isOK;
                this.lbtnLoadExportConfig.Visible = isOK;
                this.lbtnSaveExportConfig.Visible = isOK;
            }
        }

        protected void ucFilter2_ItemSelectedIndexChanged(object sender, FilterEventArgs e)
        {
            this.QueryDepId = "";
            this.QueryReceiveId = this.ucFilter2.SelectedReceiveID;
            this.GetAndBindUpNoOptions(this.QueryReceiveType, this.QueryYearId, this.QueryTermId, this.QueryDepId, this.QueryReceiveId);
            this.GetAndBindMemoTitles(this.QueryReceiveType, this.QueryYearId, this.QueryTermId, this.QueryDepId, this.QueryReceiveId);
        }

        protected void lbtnLoadExportConfig_Click(object sender, EventArgs e)
        {
            string id = this.ddlLoadExportConfig.SelectedValue;
            if (String.IsNullOrEmpty(id))
            {
                this.ShowMustInputAlert("載入資料項目設定");
            }
            else
            {
                this.GetAndBindExportConfigData(id);
            }
        }

        protected void lbtnSaveExportConfig_Click(object sender, EventArgs e)
        {
            string id = this.ddlSaveExportConfig.SelectedValue;
            string name = this.tbxSaveExportConfigName.Text.Trim();
            this.SaveExportConfigData(id, name);
        }

        protected void ccbtnDownload_Click(object sender, EventArgs e)
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

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
            string extName = "XLS";
            {
                LinkButton control = sender as LinkButton;
                if (control.CommandArgument == "ODS")
                {
                    extName = "ODS";
                }
            }
            #endregion

            #region 查詢條件
            #region 批號
            string qUpNo = this.ddlUpNo.SelectedValue;
            #endregion

            #region 銷帳狀態
            string qCancelStatus = this.ddlCancelStatus.SelectedValue;
            #endregion

            #region 繳款方式 + 代收日區間 + 入帳日區間
            string qReceiveWay = null;
            string qSReceivDate = null;
            string qEReceivDate = null;
            string qSAccountDate = null;
            string qEAccountDate = null;
            if (qCancelStatus != CancelStatusCodeTexts.NONPAY)
            {
                #region 繳款方式
                qReceiveWay = this.ddlReceiveWay.SelectedValue;
                #endregion

                #region 日期區間
                if (this.rbtReceiveDate.Checked || (this.rbtAccountDate.Checked && (qCancelStatus == CancelStatusCodeTexts.CANCELED || String.IsNullOrEmpty(qCancelStatus))))
                {
                    string dateFieldName = this.rbtReceiveDate.Checked ? "代收日區間" : "入帳日區間";

                    #region 起日
                    DateTime qSDate;
                    string sDate = this.tbxQuerySDate.Text.Trim();
                    if (String.IsNullOrEmpty(sDate))
                    {
                        //[TODO] 固定顯示訊息的收集
                        this.ShowMustInputAlert(String.Format("{0}的起日", dateFieldName));
                        return;
                    }
                    else
                    {
                        if (DateTime.TryParse(sDate, out qSDate) && qSDate.Year >= 1911)
                        {
                            sDate = qSDate.ToString("yyyy/MM/dd");
                        }
                        else
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized(String.Format("「{0}的起日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)", dateFieldName));
                            this.ShowJsAlert(msg);
                            return;
                        }
                    }
                    #endregion

                    #region 迄日
                    DateTime qEDate;
                    string eDate = this.tbxQueryEDate.Text.Trim();
                    if (String.IsNullOrEmpty(eDate))
                    {
                        //[TODO] 固定顯示訊息的收集
                        this.ShowMustInputAlert(String.Format("{0}的迄日", dateFieldName));
                        return;
                    }
                    else
                    {
                        if (DateTime.TryParse(eDate, out qEDate) && qEDate.Year >= 1911)
                        {
                            eDate = qEDate.ToString("yyyy/MM/dd");
                        }
                        else
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized(String.Format("「{0}的迄日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)", dateFieldName));
                            this.ShowJsAlert(msg);
                            return;
                        }
                    }
                    #endregion

                    if (qEDate < qSDate)
                    {
                        //[TODO] 固定顯示訊息的收集
                        this.ShowMustInputAlert(String.Format("{0}的迄日不可小於起日", dateFieldName));
                        return;
                    }

                    if (this.rbtReceiveDate.Checked)
                    {
                        qSReceivDate = sDate;
                        qEReceivDate = eDate;
                    }
                    else
                    {
                        qSAccountDate = sDate;
                        qEAccountDate = eDate;
                    }
                }
                #endregion
            }
            #endregion

            #region 查詢欄位與值
            string qFieldName = this.rdoSearchField.SelectedValue;
            string qFieldValue = this.tbxSearchValue.Text.Trim();
            #endregion
            #endregion

            #region 下載資料項目
            List<string> outFields = new List<string>();
            {
                //欄位 與 CheckBox 對照清單
                KeyValueList<CheckBox> fieldCbxs = this.GetFieldCheckBoxs();

                #region 勾選項目
                foreach (KeyValue<CheckBox> fieldCbx in fieldCbxs)
                {
                    if (fieldCbx.Value.Checked && fieldCbx.Value.Visible)
                    {
                        outFields.Add(fieldCbx.Key);
                    }
                }
                #endregion
            }
            #endregion

            string action = this.GetLocalized("下載學生繳費資料媒體檔");
            Byte[] fileContent = null;

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
            XmlResult xmlResult = DataProxy.Current.ExportB2100003File(this.Page, this.QueryReceiveType, this.QueryYearId, this.QueryTermId, this.QueryDepId, this.QueryReceiveId
                , qUpNo, qCancelStatus, qReceiveWay, qSReceivDate, qEReceivDate, qSAccountDate, qEAccountDate, qFieldName, qFieldValue, outFields, out fileContent, (extName == "ODS"));
            #endregion

            if (xmlResult.IsSuccess)
            {
                if (fileContent == null || fileContent.Length == 0)
                {
                    this.ShowActionFailureMessage(action, "無資料被匯出");
                }
                else
                {
                    #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
                    #region [MDY:20210401] 原碼修正
                    string fileName = String.Format("{0}_{1}_{2}_{3}_{4:yyyyMMdd}.{5}", HttpUtility.UrlEncode(this.QueryReceiveType), HttpUtility.UrlEncode(this.QueryYearId), HttpUtility.UrlEncode(this.QueryTermId), HttpUtility.UrlEncode(this.QueryReceiveId), DateTime.Now, extName);
                    #endregion
                    this.ResponseFile(fileName, fileContent, extName);
                    #endregion
                }
            }
            else
            {
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
        }
    }
}