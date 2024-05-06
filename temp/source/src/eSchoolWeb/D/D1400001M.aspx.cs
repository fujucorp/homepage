using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;
using Fuju.DB.Data;

using Entities;
using Helpers;

namespace eSchoolWeb.D
{
    /// <summary>
    /// 維護上傳繳費資料對照表
    /// </summary>
    public partial class D1400001M : BasePage
    {
        /// <summary>
        /// 備註數量
        /// </summary>
        private const int MemoCount = StudentReceiveEntity.MemoCount;

        #region Property
        /// <summary>
        /// 編輯模式參數
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
        /// 檔案類型
        /// </summary>
        private string EditFileType
        {
            get
            {
                return ViewState["EditFileType"] as string;
            }
            set
            {
                ViewState["EditFileType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存商家代號代碼的查詢條件
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
        /// 儲存對照表代碼的查詢條件
        /// </summary>
        private string EditMappingId
        {
            get
            {
                return ViewState["EditMappingId"] as string;
            }
            set
            {
                ViewState["EditMappingId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存對照表名稱
        /// </summary>
        private string EditMappingName
        {
            get
            {
                return ViewState["EditMappingName"] as string;
            }
            set
            {
                ViewState["EditMappingName"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存編輯的 MappingItem 資料
        /// </summary>
        private List<MappingItem> EditMappingItems
        {
            get
            {
                return ViewState["EditMappingItems"] as List<MappingItem>;
            }
            set
            {
                ViewState["EditMappingItems"] = value;
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            #region Step1 介面
            #region 清除所有 CheckBox 的 Checked
            List<CheckBox> checkboxs = this.GetStep1AllCheckBoxs();
            foreach (CheckBox checkbox in checkboxs)
            {
                checkbox.Checked = false;
            }
            #endregion

            #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標
            {
                #region 檢查是否有設定國際信用卡財金參數
                bool hasNCCardSetting = false;
                {
                    //因為 InitialUI 在 PageLoad 執行時 ucFilter1 還沒初始化，所以要用 WebHelper.GetFilterArguments 取得 receiveType
                    string receiveType = null;
                    string yearId = null;
                    string termId = null;
                    string depId = null;
                    string receiveId = null;
                    if (WebHelper.GetFilterArguments(out receiveType, out yearId, out termId, out depId, out receiveId) && !String.IsNullOrEmpty(receiveType))
                    {
                        Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType)
                            .And(SchoolRTypeEntity.Field.MerchantId2, RelationEnum.NotEqual, String.Empty)
                            .And(SchoolRTypeEntity.Field.MerchantId2, RelationEnum.NotEqual, null)
                            .And(SchoolRTypeEntity.Field.TerminalId2, RelationEnum.NotEqual, String.Empty)
                            .And(SchoolRTypeEntity.Field.TerminalId2, RelationEnum.NotEqual, null)
                            .And(SchoolRTypeEntity.Field.MerId2, RelationEnum.NotEqual, String.Empty)
                            .And(SchoolRTypeEntity.Field.MerId2, RelationEnum.NotEqual, null);

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.SelectCount<SchoolRTypeEntity>(this.Page, where, out count);
                        if (xmlResult.IsSuccess)
                        {
                            hasNCCardSetting = (count > 0);
                        }
                        else
                        {
                            this.ShowJsAlert("查詢學校的國際信用卡參數資料失敗");
                        }
                    }
                }
                #endregion

                this.chkNCCardFlag.Visible = hasNCCardSetting;
            }
            #endregion

            #region 學號固定為勾選且不可改
            this.chkStu_Id.Enabled = false;
            this.chkStu_Id.Checked = true;
            #endregion

            this.tbxMappingName.Text = "";
            this.tbxCreditIdCount.Text = "";
            this.tbxReceiveCount.Text = "";
            this.tbxCourseIdCount.Text = "";
            #endregion

            #region Step2 介面
            this.litFieldsHtml.Text = String.Empty;
            #endregion

            this.divStep1.Visible = true;
            this.divStep2.Visible = false;
        }

        private bool CheckXlsMappingItems(List<MappingItem> mappingItems)
        {
            if (mappingItems == null || mappingItems.Count == 0)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無任何對照欄位資料");
                this.ShowSystemMessage(msg);
                return false;
            }

            #region [MDY:2018xxxx] 收入科目金額的試算表欄位名稱允許空值，表示略過該項目
            //收入科目金額的欄位名稱
            List<string> receiveFieldNames = new List<string>(new string[] {
                MappingreXlsmdbEntity.Field.Receive1, MappingreXlsmdbEntity.Field.Receive2, MappingreXlsmdbEntity.Field.Receive3,
                MappingreXlsmdbEntity.Field.Receive4, MappingreXlsmdbEntity.Field.Receive5, MappingreXlsmdbEntity.Field.Receive6,
                MappingreXlsmdbEntity.Field.Receive7, MappingreXlsmdbEntity.Field.Receive8, MappingreXlsmdbEntity.Field.Receive9,
                MappingreXlsmdbEntity.Field.Receive10, MappingreXlsmdbEntity.Field.Receive11, MappingreXlsmdbEntity.Field.Receive12,
                MappingreXlsmdbEntity.Field.Receive13, MappingreXlsmdbEntity.Field.Receive14, MappingreXlsmdbEntity.Field.Receive15,
                MappingreXlsmdbEntity.Field.Receive16, MappingreXlsmdbEntity.Field.Receive17, MappingreXlsmdbEntity.Field.Receive18,
                MappingreXlsmdbEntity.Field.Receive19, MappingreXlsmdbEntity.Field.Receive20, MappingreXlsmdbEntity.Field.Receive21,
                MappingreXlsmdbEntity.Field.Receive22, MappingreXlsmdbEntity.Field.Receive23, MappingreXlsmdbEntity.Field.Receive24,
                MappingreXlsmdbEntity.Field.Receive25, MappingreXlsmdbEntity.Field.Receive26, MappingreXlsmdbEntity.Field.Receive27,
                MappingreXlsmdbEntity.Field.Receive28, MappingreXlsmdbEntity.Field.Receive29, MappingreXlsmdbEntity.Field.Receive30,
                MappingreXlsmdbEntity.Field.Receive31, MappingreXlsmdbEntity.Field.Receive32, MappingreXlsmdbEntity.Field.Receive33,
                MappingreXlsmdbEntity.Field.Receive34, MappingreXlsmdbEntity.Field.Receive35, MappingreXlsmdbEntity.Field.Receive36,
                MappingreXlsmdbEntity.Field.Receive37, MappingreXlsmdbEntity.Field.Receive38, MappingreXlsmdbEntity.Field.Receive39,
                MappingreXlsmdbEntity.Field.Receive40
            });

            //收入科目金額是否有勾選
            bool isReceiveChecked = false;
            //收入科目金額是否未指定試算表欄位名稱
            bool notReceiveXlsName = true;
            #endregion

            #region [MDY:20220808] 2022擴充案 備註項目對照欄位 & 英文標題欄位
            List<string> memoFieldNames = new  List<string>(new string[]
            {
                MappingreXlsmdbEntity.Field.Memo01, MappingreXlsmdbEntity.Field.Memo02,
                MappingreXlsmdbEntity.Field.Memo03, MappingreXlsmdbEntity.Field.Memo04,
                MappingreXlsmdbEntity.Field.Memo05, MappingreXlsmdbEntity.Field.Memo06,
                MappingreXlsmdbEntity.Field.Memo07, MappingreXlsmdbEntity.Field.Memo08,
                MappingreXlsmdbEntity.Field.Memo09, MappingreXlsmdbEntity.Field.Memo10,
                MappingreXlsmdbEntity.Field.Memo11, MappingreXlsmdbEntity.Field.Memo12,
                MappingreXlsmdbEntity.Field.Memo13, MappingreXlsmdbEntity.Field.Memo14,
                MappingreXlsmdbEntity.Field.Memo15, MappingreXlsmdbEntity.Field.Memo16,
                MappingreXlsmdbEntity.Field.Memo17, MappingreXlsmdbEntity.Field.Memo18,
                MappingreXlsmdbEntity.Field.Memo19, MappingreXlsmdbEntity.Field.Memo20,
                MappingreXlsmdbEntity.Field.Memo21
            });
            #endregion

            bool isEngEnabled = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);

            int checkCount = 0;
            List<string> xlsNames = new List<string>(mappingItems.Count);
            foreach (MappingItem item in mappingItems)
            {
                if (item.IsChecked && item.MaxItemCount == 0)
                {
                    checkCount++;
                    if (item.FileType != "xls")
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized(item.Name + "檔案型別不正確");
                        this.ShowSystemMessage(msg);
                        return false;
                    }

                    #region [MDY:20220808] 2022擴充案 增加收入科目英文名稱、備註項目英文標題
                    #region [MDY:2018xxxx] 收入科目金額的試算表欄位名稱允許空值，表示略過該項目 (null 才算未設定)
                    #region [OLD]
                    //if (String.IsNullOrEmpty(item.XlsName))
                    //{
                    //    this.ShowMustInputAlert(item.Name);
                    //    return false;
                    //}

                    //if (xlsNames.Contains(item.XlsName))
                    //{
                    //    this.ShowSystemMessage(String.Format("Excel欄位名稱 {0} 重複出現", item.XlsName));
                    //    return false;
                    //}
                    #endregion

                    if (receiveFieldNames.Contains(item.FieldName))
                    {
                        isReceiveChecked = true;
                        if (notReceiveXlsName && !String.IsNullOrEmpty(item.XlsName))
                        {
                            notReceiveXlsName = false;
                        }
                        #region 檢查收入科目的英文名稱必須指定
                        if (isEngEnabled && !String.IsNullOrEmpty(item.XlsName) && String.IsNullOrEmpty(item.EngFieldValue))
                        {
                            this.ShowMustInputAlert($"{item.Name}英文名稱");
                            return false;
                        }
                        #endregion
                    }
                    else if (memoFieldNames.Contains(item.FieldName))
                    {
                        if (String.IsNullOrEmpty(item.XlsName))
                        {
                            this.ShowMustInputAlert(item.Name);
                            return false;
                        }
                        #region 檢查備註項目的英文標題必須指定
                        if (isEngEnabled && String.IsNullOrEmpty(item.EngFieldValue))
                        {
                            this.ShowMustInputAlert($"{item.Name}英文標題");
                            return false;
                        }
                        #endregion
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(item.XlsName))
                        {
                            if (!isEngEnabled || (isEngEnabled && this.IsEngXlsMaapingItem(item.Key)))
                            {
                                this.ShowMustInputAlert(item.Name);
                                return false;
                            }
                        }
                    }

                    if (xlsNames.Contains(item.XlsName))
                    {
                        this.ShowSystemMessage(String.Format("試算表欄位名稱 {0} 重複出現", item.XlsName));
                        return false;
                    }
                    #endregion
                    #endregion

                    xlsNames.Add(item.XlsName);
                }
            }
            if (checkCount == 0)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("未選任何項目");
                this.ShowSystemMessage(msg);
                return false;
            }

            #region [MDY:2018xxxx] 如果有勾選收入科目金額，至少要有一筆收入科目金額的試算表欄位名稱有值
            if (isReceiveChecked && notReceiveXlsName)
            {
                string msg = this.GetLocalized("勾選收入科目金額，但未指定任何一個收入科目金額的試算表欄位名稱");
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 取得指定 XLS 檔案格式設定資料的 MappingItem 資料集合
        /// </summary>
        /// <param name="data">指定 XLS 檔案格式設定資料</param>
        /// <returns>傳回 MappingItem 集合</returns>
        private List<MappingItem> GetMappingItems(MappingreXlsmdbEntity data)
        {
            if (data == null)
            {
                return new List<MappingItem>(0);
            }

            List<MappingItem> items = new List<MappingItem>();

            #region 單一項目的欄位
            {
                string[] fields = new string[] {
                    #region 學生基本資料對照欄位
                    MappingreXlsmdbEntity.Field.StuId,

                    MappingreXlsmdbEntity.Field.StuName, MappingreXlsmdbEntity.Field.IdNumber,
                    MappingreXlsmdbEntity.Field.StuBirthday, MappingreXlsmdbEntity.Field.StuTel,
                    MappingreXlsmdbEntity.Field.StuAddcode, MappingreXlsmdbEntity.Field.StuAdd,
                    MappingreXlsmdbEntity.Field.Email, MappingreXlsmdbEntity.Field.StuParent,
                    #endregion

                    #region [MDY:20160331] 增加資料序號與繳款期限對照欄位
                    MappingreXlsmdbEntity.Field.OldSeq, MappingreXlsmdbEntity.Field.PayDueDate,
                    #endregion

                    #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標對照欄位
                    MappingreXlsmdbEntity.Field.NCCardFlag,
                    #endregion

                    MappingreXlsmdbEntity.Field.StuGrade, MappingreXlsmdbEntity.Field.StuHid,

                    #region [MDY:20220808] 2022擴充案 增加 學籍資料英文名稱對照欄位
                    MappingreXlsmdbEntity.Field.ClassId, MappingreXlsmdbEntity.Field.ClassName,
                    MappingreXlsmdbEntity.Field.ClassEName,
                    MappingreXlsmdbEntity.Field.DeptId, MappingreXlsmdbEntity.Field.DeptName, 
                    MappingreXlsmdbEntity.Field.DeptEName,
                    MappingreXlsmdbEntity.Field.CollegeId, MappingreXlsmdbEntity.Field.CollegeName,
                    MappingreXlsmdbEntity.Field.CollegeEName,
                    MappingreXlsmdbEntity.Field.MajorId, MappingreXlsmdbEntity.Field.MajorName,
                    MappingreXlsmdbEntity.Field.MajorEName, 
                    #endregion

                    #region [MDY:20220808] 2022擴充案 增加 減免、就貸、住宿英文名稱對照欄位
                    MappingreXlsmdbEntity.Field.ReduceId, MappingreXlsmdbEntity.Field.ReduceName,
                    MappingreXlsmdbEntity.Field.ReduceEName,
                    MappingreXlsmdbEntity.Field.LoanId, MappingreXlsmdbEntity.Field.LoanName,
                    MappingreXlsmdbEntity.Field.LoanEName,
                    MappingreXlsmdbEntity.Field.DormId, MappingreXlsmdbEntity.Field.DormName,
                    MappingreXlsmdbEntity.Field.DormEName,
                    #endregion

                    #region [MDY:20220808] 2022擴充案 增加 身分註記英文名稱對照欄位
                    MappingreXlsmdbEntity.Field.IdentifyId1, MappingreXlsmdbEntity.Field.IdentifyName1,
                    MappingreXlsmdbEntity.Field.IdentifyEName1,
                    MappingreXlsmdbEntity.Field.IdentifyId2, MappingreXlsmdbEntity.Field.IdentifyName2,
                    MappingreXlsmdbEntity.Field.IdentifyEName2,
                    MappingreXlsmdbEntity.Field.IdentifyId3, MappingreXlsmdbEntity.Field.IdentifyName3,
                    MappingreXlsmdbEntity.Field.IdentifyEName3,
                    MappingreXlsmdbEntity.Field.IdentifyId4, MappingreXlsmdbEntity.Field.IdentifyName4,
                    MappingreXlsmdbEntity.Field.IdentifyEName4,
                    MappingreXlsmdbEntity.Field.IdentifyId5, MappingreXlsmdbEntity.Field.IdentifyName5,
                    MappingreXlsmdbEntity.Field.IdentifyEName5,
                    MappingreXlsmdbEntity.Field.IdentifyId6, MappingreXlsmdbEntity.Field.IdentifyName6,
                    MappingreXlsmdbEntity.Field.IdentifyEName6,
                    #endregion

                    MappingreXlsmdbEntity.Field.StuHour, MappingreXlsmdbEntity.Field.StuCredit,
                    MappingreXlsmdbEntity.Field.LoanAmount,

                    MappingreXlsmdbEntity.Field.ReceiveAmount,
                    MappingreXlsmdbEntity.Field.SeriorNo, MappingreXlsmdbEntity.Field.CancelNo,

                    #region 轉帳資料對照欄位
                    MappingreXlsmdbEntity.Field.DeductBankid, MappingreXlsmdbEntity.Field.DeductAccountno,
                    MappingreXlsmdbEntity.Field.DeductAccountname, MappingreXlsmdbEntity.Field.DeductAccountid,
                    #endregion

                    #region [MDY:20220808] 2022擴充案 改寫所以 MARK
                    #region [OLD] 備註對照欄位
                    //MappingreXlsmdbEntity.Field.Memo01, MappingreXlsmdbEntity.Field.Memo02,
                    //MappingreXlsmdbEntity.Field.Memo03, MappingreXlsmdbEntity.Field.Memo04,
                    //MappingreXlsmdbEntity.Field.Memo05, MappingreXlsmdbEntity.Field.Memo06,
                    //MappingreXlsmdbEntity.Field.Memo07, MappingreXlsmdbEntity.Field.Memo08,
                    //MappingreXlsmdbEntity.Field.Memo09, MappingreXlsmdbEntity.Field.Memo10,
                    //MappingreXlsmdbEntity.Field.Memo11, MappingreXlsmdbEntity.Field.Memo12,
                    //MappingreXlsmdbEntity.Field.Memo13, MappingreXlsmdbEntity.Field.Memo14,
                    //MappingreXlsmdbEntity.Field.Memo15, MappingreXlsmdbEntity.Field.Memo16,
                    //MappingreXlsmdbEntity.Field.Memo17, MappingreXlsmdbEntity.Field.Memo18,
                    //MappingreXlsmdbEntity.Field.Memo19, MappingreXlsmdbEntity.Field.Memo20,
                    //MappingreXlsmdbEntity.Field.Memo21,
                    #endregion
                    #endregion
                };

                string[] values = new string[] {
                    #region 學生基本資料對照欄位
                    data.StuId,

                    data.StuName, data.IdNumber,
                    data.StuBirthday, data.StuTel,
                    data.StuAddcode, data.StuAdd,
                    data.Email, data.StuParent,
                    #endregion

                    #region [MDY:20160131] 增加資料序號與繳款期限對照欄位
                    data.OldSeq, data.PayDueDate,
                    #endregion

                    #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標對照欄位
                    data.NCCardFlag,
                    #endregion

                    data.StuGrade, data.StuHid,

                    #region [MDY:20220808] 2022擴充案 增加 學籍資料英文名稱對照欄位
                    data.ClassId, data.ClassName,
                    data.ClassEName,
                    data.DeptId, data.DeptName,
                    data.DeptEName,
                    data.CollegeId, data.CollegeName,
                    data.CollegeEName,
                    data.MajorId, data.MajorName,
                    data.MajorEName,
                    #endregion

                    #region [MDY:20220808] 2022擴充案 增加 減免、就貸、住宿英文名稱對照欄位
                    data.ReduceId, data.ReduceName,
                    data.ReduceEName,
                    data.LoanId, data.LoanName,
                    data.LoanEName,
                    data.DormId, data.DormName,
                    data.DormEName,
                    #endregion

                    #region [MDY:20220808] 2022擴充案 增加 身分註記英文名稱對照欄位
                    data.IdentifyId1, data.IdentifyName1,
                    data.IdentifyEName1,
                    data.IdentifyId2, data.IdentifyName2,
                    data.IdentifyEName2,
                    data.IdentifyId3, data.IdentifyName3,
                    data.IdentifyEName3,
                    data.IdentifyId4, data.IdentifyName4,
                    data.IdentifyEName4,
                    data.IdentifyId5, data.IdentifyName5,
                    data.IdentifyEName5,
                    data.IdentifyId6, data.IdentifyName6,
                    data.IdentifyEName6,
                    #endregion

                    data.StuHour, data.StuCredit,
                    data.LoanAmount,

                    data.ReceiveAmount,
                    data.SeriorNo, data.CancelNo,

                    #region 轉帳資料對照欄位
                    data.DeductBankid, data.DeductAccountno,
                    data.DeductAccountname, data.DeductAccountid,
                    #endregion

                    #region [MDY:20220808] 2022擴充案 改寫所以 MARK
                    #region [OLD] 備註對照欄位
                    //data.Memo01, data.Memo02,
                    //data.Memo03, data.Memo04,
                    //data.Memo05, data.Memo06,
                    //data.Memo07, data.Memo08,
                    //data.Memo09, data.Memo10,
                    //data.Memo11, data.Memo12,
                    //data.Memo13, data.Memo14,
                    //data.Memo15, data.Memo16,
                    //data.Memo17, data.Memo18,
                    //data.Memo19, data.Memo20,
                    //data.Memo21,
                    #endregion
                    #endregion

                };

                for (int idx = 0; idx < fields.Length; idx++)
                {
                    MappingItem item = MappingItem.CreateByXls(fields[idx], null, fields[idx], values[idx]);
                    items.Add(item);
                }
            }
            #endregion

            #region 學分基準欄位
            {
                string[] fields = new string[] {
                    MappingreXlsmdbEntity.Field.CreditId1, MappingreXlsmdbEntity.Field.CreditId2, MappingreXlsmdbEntity.Field.CreditId3,
                    MappingreXlsmdbEntity.Field.CreditId4, MappingreXlsmdbEntity.Field.CreditId5, MappingreXlsmdbEntity.Field.CreditId6,
                    MappingreXlsmdbEntity.Field.CreditId7, MappingreXlsmdbEntity.Field.CreditId8, MappingreXlsmdbEntity.Field.CreditId9,
                    MappingreXlsmdbEntity.Field.CreditId10, MappingreXlsmdbEntity.Field.CreditId11, MappingreXlsmdbEntity.Field.CreditId12,
                    MappingreXlsmdbEntity.Field.CreditId13, MappingreXlsmdbEntity.Field.CreditId14, MappingreXlsmdbEntity.Field.CreditId15,
                    MappingreXlsmdbEntity.Field.CreditId16, MappingreXlsmdbEntity.Field.CreditId17, MappingreXlsmdbEntity.Field.CreditId18,
                    MappingreXlsmdbEntity.Field.CreditId19, MappingreXlsmdbEntity.Field.CreditId20, MappingreXlsmdbEntity.Field.CreditId21,
                    MappingreXlsmdbEntity.Field.CreditId22, MappingreXlsmdbEntity.Field.CreditId23, MappingreXlsmdbEntity.Field.CreditId24,
                    MappingreXlsmdbEntity.Field.CreditId25, MappingreXlsmdbEntity.Field.CreditId26, MappingreXlsmdbEntity.Field.CreditId27,
                    MappingreXlsmdbEntity.Field.CreditId28, MappingreXlsmdbEntity.Field.CreditId29, MappingreXlsmdbEntity.Field.CreditId30,
                    MappingreXlsmdbEntity.Field.CreditId31, MappingreXlsmdbEntity.Field.CreditId32, MappingreXlsmdbEntity.Field.CreditId33,
                    MappingreXlsmdbEntity.Field.CreditId34, MappingreXlsmdbEntity.Field.CreditId35, MappingreXlsmdbEntity.Field.CreditId36,
                    MappingreXlsmdbEntity.Field.CreditId37, MappingreXlsmdbEntity.Field.CreditId38, MappingreXlsmdbEntity.Field.CreditId39,
                    MappingreXlsmdbEntity.Field.CreditId40
                };
                string[] values = new string[] {
                    data.CreditId1, data.CreditId2, data.CreditId3,
                    data.CreditId4, data.CreditId5, data.CreditId6,
                    data.CreditId7, data.CreditId8, data.CreditId9,
                    data.CreditId10, data.CreditId11, data.CreditId12,
                    data.CreditId13, data.CreditId14, data.CreditId15,
                    data.CreditId16, data.CreditId17, data.CreditId18,
                    data.CreditId19, data.CreditId20, data.CreditId21,
                    data.CreditId22, data.CreditId23, data.CreditId24,
                    data.CreditId25, data.CreditId26, data.CreditId27,
                    data.CreditId28, data.CreditId29, data.CreditId30,
                    data.CreditId31, data.CreditId32, data.CreditId33,
                    data.CreditId34, data.CreditId35, data.CreditId36,
                    data.CreditId37, data.CreditId38, data.CreditId39,
                    data.CreditId40
                };
                int count = 0;
                for (int idx = 0; idx < fields.Length; idx++)
                {
                    MappingItem item = MappingItem.CreateByXls(fields[idx], null, fields[idx], values[idx]);
                    items.Add(item);
                    if (item.IsChecked)
                    {
                        count++;
                    }
                }
                items.Add(MappingItem.CreateByItemCount("Credit_Id", "學分基準", 40, (count > 0 ? count.ToString() : String.Empty)));
            }
            #endregion

            #region 課程代碼欄位
            {
                string[] fields = new string[] {
                    MappingreXlsmdbEntity.Field.CourseId1, MappingreXlsmdbEntity.Field.CourseId2, MappingreXlsmdbEntity.Field.CourseId3,
                    MappingreXlsmdbEntity.Field.CourseId4, MappingreXlsmdbEntity.Field.CourseId5, MappingreXlsmdbEntity.Field.CourseId6,
                    MappingreXlsmdbEntity.Field.CourseId7, MappingreXlsmdbEntity.Field.CourseId8, MappingreXlsmdbEntity.Field.CourseId9,
                    MappingreXlsmdbEntity.Field.CourseId10, MappingreXlsmdbEntity.Field.CourseId11, MappingreXlsmdbEntity.Field.CourseId12,
                    MappingreXlsmdbEntity.Field.CourseId13, MappingreXlsmdbEntity.Field.CourseId14, MappingreXlsmdbEntity.Field.CourseId15,
                    MappingreXlsmdbEntity.Field.CourseId16, MappingreXlsmdbEntity.Field.CourseId17, MappingreXlsmdbEntity.Field.CourseId18,
                    MappingreXlsmdbEntity.Field.CourseId19, MappingreXlsmdbEntity.Field.CourseId20, MappingreXlsmdbEntity.Field.CourseId21,
                    MappingreXlsmdbEntity.Field.CourseId22, MappingreXlsmdbEntity.Field.CourseId23, MappingreXlsmdbEntity.Field.CourseId24,
                    MappingreXlsmdbEntity.Field.CourseId25, MappingreXlsmdbEntity.Field.CourseId26, MappingreXlsmdbEntity.Field.CourseId27,
                    MappingreXlsmdbEntity.Field.CourseId28, MappingreXlsmdbEntity.Field.CourseId29, MappingreXlsmdbEntity.Field.CourseId30,
                    MappingreXlsmdbEntity.Field.CourseId31, MappingreXlsmdbEntity.Field.CourseId32, MappingreXlsmdbEntity.Field.CourseId33,
                    MappingreXlsmdbEntity.Field.CourseId34, MappingreXlsmdbEntity.Field.CourseId35, MappingreXlsmdbEntity.Field.CourseId36,
                    MappingreXlsmdbEntity.Field.CourseId37, MappingreXlsmdbEntity.Field.CourseId38, MappingreXlsmdbEntity.Field.CourseId39,
                    MappingreXlsmdbEntity.Field.CourseId40
                };
                string[] values = new string[] {
                    data.CourseId1, data.CourseId2, data.CourseId3,
                    data.CourseId4, data.CourseId5, data.CourseId6,
                    data.CourseId7, data.CourseId8, data.CourseId9,
                    data.CourseId10, data.CourseId11, data.CourseId12,
                    data.CourseId13, data.CourseId14, data.CourseId15,
                    data.CourseId16, data.CourseId17, data.CourseId18,
                    data.CourseId19, data.CourseId20, data.CourseId21,
                    data.CourseId22, data.CourseId23, data.CourseId24,
                    data.CourseId25, data.CourseId26, data.CourseId27,
                    data.CourseId28, data.CourseId29, data.CourseId30,
                    data.CourseId31, data.CourseId32, data.CourseId33,
                    data.CourseId34, data.CourseId35, data.CourseId36,
                    data.CourseId37, data.CourseId38, data.CourseId39,
                    data.CourseId40
                };
                int count = 0;
                for (int idx = 0; idx < fields.Length; idx++)
                {
                    MappingItem item = MappingItem.CreateByXls(fields[idx], null, fields[idx], values[idx]);
                    items.Add(item);
                    if (item.IsChecked)
                    {
                        count++;
                    }
                }
                items.Add(MappingItem.CreateByItemCount("Course_Id", "課程代碼", 40, (count > 0 ? count.ToString() : String.Empty)));
            }
            #endregion

            #region 學分基準或課程學分數欄位
            {
                string[] fields = new string[] {
                    MappingreXlsmdbEntity.Field.Credit1, MappingreXlsmdbEntity.Field.Credit2, MappingreXlsmdbEntity.Field.Credit3,
                    MappingreXlsmdbEntity.Field.Credit4, MappingreXlsmdbEntity.Field.Credit5, MappingreXlsmdbEntity.Field.Credit6,
                    MappingreXlsmdbEntity.Field.Credit7, MappingreXlsmdbEntity.Field.Credit8, MappingreXlsmdbEntity.Field.Credit9,
                    MappingreXlsmdbEntity.Field.Credit10, MappingreXlsmdbEntity.Field.Credit11, MappingreXlsmdbEntity.Field.Credit12,
                    MappingreXlsmdbEntity.Field.Credit13, MappingreXlsmdbEntity.Field.Credit14, MappingreXlsmdbEntity.Field.Credit15,
                    MappingreXlsmdbEntity.Field.Credit16, MappingreXlsmdbEntity.Field.Credit17, MappingreXlsmdbEntity.Field.Credit18,
                    MappingreXlsmdbEntity.Field.Credit19, MappingreXlsmdbEntity.Field.Credit20, MappingreXlsmdbEntity.Field.Credit21,
                    MappingreXlsmdbEntity.Field.Credit22, MappingreXlsmdbEntity.Field.Credit23, MappingreXlsmdbEntity.Field.Credit24,
                    MappingreXlsmdbEntity.Field.Credit25, MappingreXlsmdbEntity.Field.Credit26, MappingreXlsmdbEntity.Field.Credit27,
                    MappingreXlsmdbEntity.Field.Credit28, MappingreXlsmdbEntity.Field.Credit29, MappingreXlsmdbEntity.Field.Credit30,
                    MappingreXlsmdbEntity.Field.Credit31, MappingreXlsmdbEntity.Field.Credit32, MappingreXlsmdbEntity.Field.Credit33,
                    MappingreXlsmdbEntity.Field.Credit34, MappingreXlsmdbEntity.Field.Credit35, MappingreXlsmdbEntity.Field.Credit36,
                    MappingreXlsmdbEntity.Field.Credit37, MappingreXlsmdbEntity.Field.Credit38, MappingreXlsmdbEntity.Field.Credit39,
                    MappingreXlsmdbEntity.Field.Credit40
                };
                string[] values = new string[] {
                    data.Credit1, data.Credit2, data.Credit3,
                    data.Credit4, data.Credit5, data.Credit6,
                    data.Credit7, data.Credit8, data.Credit9,
                    data.Credit10, data.Credit11, data.Credit12,
                    data.Credit13, data.Credit14, data.Credit15,
                    data.Credit16, data.Credit17, data.Credit18,
                    data.Credit19, data.Credit20, data.Credit21,
                    data.Credit22, data.Credit23, data.Credit24,
                    data.Credit25, data.Credit26, data.Credit27,
                    data.Credit28, data.Credit29, data.Credit30,
                    data.Credit31, data.Credit32, data.Credit33,
                    data.Credit34, data.Credit35, data.Credit36,
                    data.Credit37, data.Credit38, data.Credit39,
                    data.Credit40
                };
                int count = 0;
                for (int idx = 0; idx < fields.Length; idx++)
                {
                    MappingItem item = MappingItem.CreateByXls(fields[idx], null, fields[idx], values[idx]);
                    items.Add(item);
                    if (item.IsChecked)
                    {
                        count++;
                    }
                }
                items.Add(MappingItem.CreateByItemCount("Credit", "學分數", 40, (count > 0 ? count.ToString() : String.Empty)));
            }
            #endregion

            #region 收入科目金額欄位
            #region [MDY:20220808] 2022擴充案 增加收入科目英文名稱欄位
            {
                string[] fields = new string[] {
                    MappingreXlsmdbEntity.Field.Receive1, MappingreXlsmdbEntity.Field.Receive2, MappingreXlsmdbEntity.Field.Receive3,
                    MappingreXlsmdbEntity.Field.Receive4, MappingreXlsmdbEntity.Field.Receive5, MappingreXlsmdbEntity.Field.Receive6,
                    MappingreXlsmdbEntity.Field.Receive7, MappingreXlsmdbEntity.Field.Receive8, MappingreXlsmdbEntity.Field.Receive9,
                    MappingreXlsmdbEntity.Field.Receive10, MappingreXlsmdbEntity.Field.Receive11, MappingreXlsmdbEntity.Field.Receive12,
                    MappingreXlsmdbEntity.Field.Receive13, MappingreXlsmdbEntity.Field.Receive14, MappingreXlsmdbEntity.Field.Receive15,
                    MappingreXlsmdbEntity.Field.Receive16, MappingreXlsmdbEntity.Field.Receive17, MappingreXlsmdbEntity.Field.Receive18,
                    MappingreXlsmdbEntity.Field.Receive19, MappingreXlsmdbEntity.Field.Receive20, MappingreXlsmdbEntity.Field.Receive21,
                    MappingreXlsmdbEntity.Field.Receive22, MappingreXlsmdbEntity.Field.Receive23, MappingreXlsmdbEntity.Field.Receive24,
                    MappingreXlsmdbEntity.Field.Receive25, MappingreXlsmdbEntity.Field.Receive26, MappingreXlsmdbEntity.Field.Receive27,
                    MappingreXlsmdbEntity.Field.Receive28, MappingreXlsmdbEntity.Field.Receive29, MappingreXlsmdbEntity.Field.Receive30,
                    MappingreXlsmdbEntity.Field.Receive31, MappingreXlsmdbEntity.Field.Receive32, MappingreXlsmdbEntity.Field.Receive33,
                    MappingreXlsmdbEntity.Field.Receive34, MappingreXlsmdbEntity.Field.Receive35, MappingreXlsmdbEntity.Field.Receive36,
                    MappingreXlsmdbEntity.Field.Receive37, MappingreXlsmdbEntity.Field.Receive38, MappingreXlsmdbEntity.Field.Receive39,
                    MappingreXlsmdbEntity.Field.Receive40
                };
                string[] values = new string[] {
                    data.Receive1, data.Receive2, data.Receive3,
                    data.Receive4, data.Receive5, data.Receive6,
                    data.Receive7, data.Receive8, data.Receive9,
                    data.Receive10, data.Receive11, data.Receive12,
                    data.Receive13, data.Receive14, data.Receive15,
                    data.Receive16, data.Receive17, data.Receive18,
                    data.Receive19, data.Receive20, data.Receive21,
                    data.Receive22, data.Receive23, data.Receive24,
                    data.Receive25, data.Receive26, data.Receive27,
                    data.Receive28, data.Receive29, data.Receive30,
                    data.Receive31, data.Receive32, data.Receive33,
                    data.Receive34, data.Receive35, data.Receive36,
                    data.Receive37, data.Receive38, data.Receive39,
                    data.Receive40
                };

                string[] engFields = new string[] {
                    MappingreXlsmdbEntity.Field.ReceiveItemE01, MappingreXlsmdbEntity.Field.ReceiveItemE02, MappingreXlsmdbEntity.Field.ReceiveItemE03,
                    MappingreXlsmdbEntity.Field.ReceiveItemE04, MappingreXlsmdbEntity.Field.ReceiveItemE05, MappingreXlsmdbEntity.Field.ReceiveItemE06,
                    MappingreXlsmdbEntity.Field.ReceiveItemE07, MappingreXlsmdbEntity.Field.ReceiveItemE08, MappingreXlsmdbEntity.Field.ReceiveItemE09,
                    MappingreXlsmdbEntity.Field.ReceiveItemE10, MappingreXlsmdbEntity.Field.ReceiveItemE11, MappingreXlsmdbEntity.Field.ReceiveItemE12,
                    MappingreXlsmdbEntity.Field.ReceiveItemE13, MappingreXlsmdbEntity.Field.ReceiveItemE14, MappingreXlsmdbEntity.Field.ReceiveItemE15,
                    MappingreXlsmdbEntity.Field.ReceiveItemE16, MappingreXlsmdbEntity.Field.ReceiveItemE17, MappingreXlsmdbEntity.Field.ReceiveItemE18,
                    MappingreXlsmdbEntity.Field.ReceiveItemE19, MappingreXlsmdbEntity.Field.ReceiveItemE20, MappingreXlsmdbEntity.Field.ReceiveItemE21,
                    MappingreXlsmdbEntity.Field.ReceiveItemE22, MappingreXlsmdbEntity.Field.ReceiveItemE23, MappingreXlsmdbEntity.Field.ReceiveItemE24,
                    MappingreXlsmdbEntity.Field.ReceiveItemE25, MappingreXlsmdbEntity.Field.ReceiveItemE26, MappingreXlsmdbEntity.Field.ReceiveItemE27,
                    MappingreXlsmdbEntity.Field.ReceiveItemE28, MappingreXlsmdbEntity.Field.ReceiveItemE29, MappingreXlsmdbEntity.Field.ReceiveItemE30,
                    MappingreXlsmdbEntity.Field.ReceiveItemE31, MappingreXlsmdbEntity.Field.ReceiveItemE32, MappingreXlsmdbEntity.Field.ReceiveItemE33,
                    MappingreXlsmdbEntity.Field.ReceiveItemE34, MappingreXlsmdbEntity.Field.ReceiveItemE35, MappingreXlsmdbEntity.Field.ReceiveItemE36,
                    MappingreXlsmdbEntity.Field.ReceiveItemE37, MappingreXlsmdbEntity.Field.ReceiveItemE38, MappingreXlsmdbEntity.Field.ReceiveItemE39,
                    MappingreXlsmdbEntity.Field.ReceiveItemE40
                };

                string[] engValues = new string[] {
                    data.ReceiveItemE01, data.ReceiveItemE02, data.ReceiveItemE03,
                    data.ReceiveItemE04, data.ReceiveItemE05, data.ReceiveItemE06,
                    data.ReceiveItemE07, data.ReceiveItemE08, data.ReceiveItemE09,
                    data.ReceiveItemE10, data.ReceiveItemE11, data.ReceiveItemE12,
                    data.ReceiveItemE13, data.ReceiveItemE14, data.ReceiveItemE15,
                    data.ReceiveItemE16, data.ReceiveItemE17, data.ReceiveItemE18,
                    data.ReceiveItemE19, data.ReceiveItemE20, data.ReceiveItemE21,
                    data.ReceiveItemE22, data.ReceiveItemE23, data.ReceiveItemE24,
                    data.ReceiveItemE25, data.ReceiveItemE26, data.ReceiveItemE27,
                    data.ReceiveItemE28, data.ReceiveItemE29, data.ReceiveItemE30,
                    data.ReceiveItemE31, data.ReceiveItemE32, data.ReceiveItemE33,
                    data.ReceiveItemE34, data.ReceiveItemE35, data.ReceiveItemE36,
                    data.ReceiveItemE37, data.ReceiveItemE38, data.ReceiveItemE39,
                    data.ReceiveItemE40
                };

                int count = 0;
                for (int idx = 0; idx < fields.Length; idx++)
                {
                    MappingItem item = MappingItem.CreateByXls(fields[idx], null, fields[idx], values[idx], engFields[idx], engValues[idx]);
                    item.XlsNameSize = 40;    //中文名稱最多 40字 （把XLS表頭對照欄位做為中文名稱）
                    item.EngFieldSize = 140;  //英文名稱最多140字
                    items.Add(item);
                    if (item.IsChecked)
                    {
                        count++;
                    }
                }
                items.Add(MappingItem.CreateByItemCount("Receive", "收入科目金額", 40, (count > 0 ? count.ToString() : String.Empty)));
            }
            #endregion
            #endregion

            #region [MDY:20220808] 2022擴充案 備註項目對照欄位 & 英文標題欄位
            {
                string[] fields = new string[]
                {
                    MappingreXlsmdbEntity.Field.Memo01, MappingreXlsmdbEntity.Field.Memo02,
                    MappingreXlsmdbEntity.Field.Memo03, MappingreXlsmdbEntity.Field.Memo04,
                    MappingreXlsmdbEntity.Field.Memo05, MappingreXlsmdbEntity.Field.Memo06,
                    MappingreXlsmdbEntity.Field.Memo07, MappingreXlsmdbEntity.Field.Memo08,
                    MappingreXlsmdbEntity.Field.Memo09, MappingreXlsmdbEntity.Field.Memo10,
                    MappingreXlsmdbEntity.Field.Memo11, MappingreXlsmdbEntity.Field.Memo12,
                    MappingreXlsmdbEntity.Field.Memo13, MappingreXlsmdbEntity.Field.Memo14,
                    MappingreXlsmdbEntity.Field.Memo15, MappingreXlsmdbEntity.Field.Memo16,
                    MappingreXlsmdbEntity.Field.Memo17, MappingreXlsmdbEntity.Field.Memo18,
                    MappingreXlsmdbEntity.Field.Memo19, MappingreXlsmdbEntity.Field.Memo20,
                    MappingreXlsmdbEntity.Field.Memo21
                };
                string[] values = new string[]
                {
                    data.Memo01, data.Memo02,
                    data.Memo03, data.Memo04,
                    data.Memo05, data.Memo06,
                    data.Memo07, data.Memo08,
                    data.Memo09, data.Memo10,
                    data.Memo11, data.Memo12,
                    data.Memo13, data.Memo14,
                    data.Memo15, data.Memo16,
                    data.Memo17, data.Memo18,
                    data.Memo19, data.Memo20,
                    data.Memo21
                };

                string[] engFields = new string[]
                {
                    MappingreXlsmdbEntity.Field.MemoTitleE01, MappingreXlsmdbEntity.Field.MemoTitleE02, MappingreXlsmdbEntity.Field.MemoTitleE03,
                    MappingreXlsmdbEntity.Field.MemoTitleE04, MappingreXlsmdbEntity.Field.MemoTitleE05, MappingreXlsmdbEntity.Field.MemoTitleE06,
                    MappingreXlsmdbEntity.Field.MemoTitleE07, MappingreXlsmdbEntity.Field.MemoTitleE08, MappingreXlsmdbEntity.Field.MemoTitleE09,
                    MappingreXlsmdbEntity.Field.MemoTitleE10, MappingreXlsmdbEntity.Field.MemoTitleE11, MappingreXlsmdbEntity.Field.MemoTitleE12,
                    MappingreXlsmdbEntity.Field.MemoTitleE13, MappingreXlsmdbEntity.Field.MemoTitleE14, MappingreXlsmdbEntity.Field.MemoTitleE15,
                    MappingreXlsmdbEntity.Field.MemoTitleE16, MappingreXlsmdbEntity.Field.MemoTitleE17, MappingreXlsmdbEntity.Field.MemoTitleE18,
                    MappingreXlsmdbEntity.Field.MemoTitleE19, MappingreXlsmdbEntity.Field.MemoTitleE20, MappingreXlsmdbEntity.Field.MemoTitleE21
                };
                string[] engValues = new string[] {
                    data.MemoTitleE01, data.MemoTitleE02, data.MemoTitleE03,
                    data.MemoTitleE04, data.MemoTitleE05, data.MemoTitleE06,
                    data.MemoTitleE07, data.MemoTitleE08, data.MemoTitleE09,
                    data.MemoTitleE10, data.MemoTitleE11, data.MemoTitleE12,
                    data.MemoTitleE13, data.MemoTitleE14, data.MemoTitleE15,
                    data.MemoTitleE16, data.MemoTitleE17, data.MemoTitleE18,
                    data.MemoTitleE19, data.MemoTitleE20, data.MemoTitleE21
                };

                for (int idx = 0; idx < fields.Length; idx++)
                {
                    MappingItem item = MappingItem.CreateByXls(fields[idx], null, fields[idx], values[idx], engFields[idx], engValues[idx]);
                    item.XlsNameSize = 40;    //中文名稱最多 40字 （把XLS表頭對照欄位做為中文名稱）
                    item.EngFieldSize = 140;  //英文名稱最多140字
                    items.Add(item);
                }
            }
            #endregion

            return items;
        }

        private MappingreXlsmdbEntity GetMappingreXlsmdbEntity(List<MappingItem> mappingItems)
        {
            MappingreXlsmdbEntity data = new MappingreXlsmdbEntity();
            if (mappingItems != null && mappingItems.Count > 0)
            {
                #region 單一項目的欄位
                {
                    string[] mapFields = new string[] {
                        #region 學生基本資料對照欄位
                        MappingreXlsmdbEntity.Field.StuId,

                        MappingreXlsmdbEntity.Field.StuName, MappingreXlsmdbEntity.Field.IdNumber,
                        MappingreXlsmdbEntity.Field.StuBirthday, MappingreXlsmdbEntity.Field.StuTel,
                        MappingreXlsmdbEntity.Field.StuAddcode, MappingreXlsmdbEntity.Field.StuAdd,
                        MappingreXlsmdbEntity.Field.Email, MappingreXlsmdbEntity.Field.StuParent,
                        #endregion

                        #region [MDY:20160131] 增加資料序號與繳款期限對照欄位
                        MappingreXlsmdbEntity.Field.OldSeq, MappingreXlsmdbEntity.Field.PayDueDate,
                        #endregion

                        #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標對照欄位
                        MappingreXlsmdbEntity.Field.NCCardFlag,
                        #endregion

                        MappingreXlsmdbEntity.Field.StuGrade, MappingreXlsmdbEntity.Field.StuHid,

                        #region [MDY:20220808] 2022擴充案 增加 學籍資料英文名稱對照欄位
                        MappingreXlsmdbEntity.Field.ClassId, MappingreXlsmdbEntity.Field.ClassName,
                        MappingreXlsmdbEntity.Field.ClassEName,
                        MappingreXlsmdbEntity.Field.DeptId, MappingreXlsmdbEntity.Field.DeptName,
                        MappingreXlsmdbEntity.Field.DeptEName,
                        MappingreXlsmdbEntity.Field.CollegeId, MappingreXlsmdbEntity.Field.CollegeName,
                        MappingreXlsmdbEntity.Field.CollegeEName,
                        MappingreXlsmdbEntity.Field.MajorId, MappingreXlsmdbEntity.Field.MajorName,
                        MappingreXlsmdbEntity.Field.MajorEName,
                        #endregion

                        #region [MDY:20220808] 2022擴充案 增加 減免、就貸、住宿英文名稱對照欄位
                        MappingreXlsmdbEntity.Field.ReduceId, MappingreXlsmdbEntity.Field.ReduceName,
                        MappingreXlsmdbEntity.Field.ReduceEName,
                        MappingreXlsmdbEntity.Field.LoanId, MappingreXlsmdbEntity.Field.LoanName,
                        MappingreXlsmdbEntity.Field.LoanEName,
                        MappingreXlsmdbEntity.Field.DormId, MappingreXlsmdbEntity.Field.DormName,
                        MappingreXlsmdbEntity.Field.DormEName,
                        #endregion

                        #region [MDY:20220808] 2022擴充案 增加 身分註記英文名稱對照欄位
                        MappingreXlsmdbEntity.Field.IdentifyId1, MappingreXlsmdbEntity.Field.IdentifyName1,
                        MappingreXlsmdbEntity.Field.IdentifyEName1,
                        MappingreXlsmdbEntity.Field.IdentifyId2, MappingreXlsmdbEntity.Field.IdentifyName2,
                        MappingreXlsmdbEntity.Field.IdentifyEName2,
                        MappingreXlsmdbEntity.Field.IdentifyId3, MappingreXlsmdbEntity.Field.IdentifyName3,
                        MappingreXlsmdbEntity.Field.IdentifyEName3,
                        MappingreXlsmdbEntity.Field.IdentifyId4, MappingreXlsmdbEntity.Field.IdentifyName4,
                        MappingreXlsmdbEntity.Field.IdentifyEName4,
                        MappingreXlsmdbEntity.Field.IdentifyId5, MappingreXlsmdbEntity.Field.IdentifyName5,
                        MappingreXlsmdbEntity.Field.IdentifyEName5,
                        MappingreXlsmdbEntity.Field.IdentifyId6, MappingreXlsmdbEntity.Field.IdentifyName6,
                        MappingreXlsmdbEntity.Field.IdentifyEName6,
                        #endregion

                        MappingreXlsmdbEntity.Field.StuHour, MappingreXlsmdbEntity.Field.StuCredit,
                        MappingreXlsmdbEntity.Field.LoanAmount,

                        MappingreXlsmdbEntity.Field.ReceiveAmount,
                        MappingreXlsmdbEntity.Field.SeriorNo, MappingreXlsmdbEntity.Field.CancelNo,

                        #region 轉帳資料對照欄位
                        MappingreXlsmdbEntity.Field.DeductBankid, MappingreXlsmdbEntity.Field.DeductAccountno,
                        MappingreXlsmdbEntity.Field.DeductAccountname, MappingreXlsmdbEntity.Field.DeductAccountid,
                        #endregion

                        #region [MDY:20220808] 2022擴充案 改寫所以 MARK
                        #region [OLD] 備註對照欄位
                        //MappingreXlsmdbEntity.Field.Memo01, MappingreXlsmdbEntity.Field.Memo02,
                        //MappingreXlsmdbEntity.Field.Memo03, MappingreXlsmdbEntity.Field.Memo04,
                        //MappingreXlsmdbEntity.Field.Memo05, MappingreXlsmdbEntity.Field.Memo06,
                        //MappingreXlsmdbEntity.Field.Memo07, MappingreXlsmdbEntity.Field.Memo08,
                        //MappingreXlsmdbEntity.Field.Memo09, MappingreXlsmdbEntity.Field.Memo10,
                        //MappingreXlsmdbEntity.Field.Memo11, MappingreXlsmdbEntity.Field.Memo12,
                        //MappingreXlsmdbEntity.Field.Memo13, MappingreXlsmdbEntity.Field.Memo14,
                        //MappingreXlsmdbEntity.Field.Memo15, MappingreXlsmdbEntity.Field.Memo16,
                        //MappingreXlsmdbEntity.Field.Memo17, MappingreXlsmdbEntity.Field.Memo18,
                        //MappingreXlsmdbEntity.Field.Memo19, MappingreXlsmdbEntity.Field.Memo20,
                        //MappingreXlsmdbEntity.Field.Memo21,
                        #endregion
                        #endregion
                    };

                    string[] fieldNames = new string[] {
                        #region 學生基本資料對照欄位
                        "StuId",

                        "StuName", "IdNumber",
                        "StuBirthday", "StuTel",
                        "StuAddcode", "StuAdd",
                        "Email", "StuParent",
                        #endregion

                        #region [MDY:20160131] 增加資料序號與繳款期限對照欄位
                        "OldSeq", "PayDueDate",
                        #endregion

                        #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標對照欄位
                        "NCCardFlag",
                        #endregion

                        "StuGrade", "StuHid",

                        #region [MDY:20220808] 2022擴充案 增加 學籍資料英文名稱對照欄位
                        "ClassId", "ClassName",
                        "ClassEName",
                        "DeptId", "DeptName",
                        "DeptEName",
                        "CollegeId", "CollegeName",
                        "CollegeEName",
                        "MajorId", "MajorName",
                        "MajorEName",
                        #endregion

                        #region [MDY:20220808] 2022擴充案 增加 減免、就貸、住宿英文名稱對照欄位
                        "ReduceId", "ReduceName",
                        "ReduceEName",
                        "LoanId", "LoanName",
                        "LoanEName",
                        "DormId", "DormName",
                        "DormEName",
                        #endregion

                        #region [MDY:20220808] 2022擴充案 增加 身分註記英文名稱對照欄位
                        "IdentifyId1", "IdentifyName1",
                        "IdentifyEName1",
                        "IdentifyId2", "IdentifyName2",
                        "IdentifyEName2",
                        "IdentifyId3", "IdentifyName3",
                        "IdentifyEName3",
                        "IdentifyId4", "IdentifyName4",
                        "IdentifyEName4",
                        "IdentifyId5", "IdentifyName5",
                        "IdentifyEName5",
                        "IdentifyId6", "IdentifyName6",
                        "IdentifyEName6",
                        #endregion

                        "StuHour", "StuCredit",
                        "LoanAmount",

                        "ReceiveAmount",
                        "SeriorNo", "CancelNo",

                        #region 轉帳資料對照欄位
                        "DeductBankid", "DeductAccountno",
                        "DeductAccountname", "DeductAccountid",
                        #endregion

                        #region [MDY:20220808] 2022擴充案 改寫所以 MARK
                        #region [OLD] 備註對照欄位
                        //"Memo01", "Memo02",
                        //"Memo03", "Memo04",
                        //"Memo05", "Memo06",
                        //"Memo07", "Memo08",
                        //"Memo09", "Memo10",
                        //"Memo11", "Memo12",
                        //"Memo13", "Memo14",
                        //"Memo15", "Memo16",
                        //"Memo17", "Memo18",
                        //"Memo19", "Memo20",
                        //"Memo21",
                        #endregion
                        #endregion
                    };

                    for (int idx = 0; idx < mapFields.Length; idx++)
                    {
                        string mapField = mapFields[idx];
                        string fieldName = fieldNames[idx];
                        MappingItem mapItem = mappingItems.Find(x => x.Key == mapField);
                        string fieldValue = null;
                        if (mapItem != null && mapItem.IsChecked)
                        {
                            if (mapItem.FileType != "xls")
                            {
                                return null;
                            }
                            fieldValue = mapItem.XlsName;
                        }
                        if (!data.SetValue(fieldName, fieldValue).IsSuccess)
                        {
                            return null;
                        }
                    }
                }
                #endregion

                #region 學分基準欄位
                {
                    string[] mapFields = new string[] {
                        MappingreXlsmdbEntity.Field.CreditId1, MappingreXlsmdbEntity.Field.CreditId2, MappingreXlsmdbEntity.Field.CreditId3,
                        MappingreXlsmdbEntity.Field.CreditId4, MappingreXlsmdbEntity.Field.CreditId5, MappingreXlsmdbEntity.Field.CreditId6,
                        MappingreXlsmdbEntity.Field.CreditId7, MappingreXlsmdbEntity.Field.CreditId8, MappingreXlsmdbEntity.Field.CreditId9,
                        MappingreXlsmdbEntity.Field.CreditId10, MappingreXlsmdbEntity.Field.CreditId11, MappingreXlsmdbEntity.Field.CreditId12,
                        MappingreXlsmdbEntity.Field.CreditId13, MappingreXlsmdbEntity.Field.CreditId14, MappingreXlsmdbEntity.Field.CreditId15,
                        MappingreXlsmdbEntity.Field.CreditId16, MappingreXlsmdbEntity.Field.CreditId17, MappingreXlsmdbEntity.Field.CreditId18,
                        MappingreXlsmdbEntity.Field.CreditId19, MappingreXlsmdbEntity.Field.CreditId20, MappingreXlsmdbEntity.Field.CreditId21,
                        MappingreXlsmdbEntity.Field.CreditId22, MappingreXlsmdbEntity.Field.CreditId23, MappingreXlsmdbEntity.Field.CreditId24,
                        MappingreXlsmdbEntity.Field.CreditId25, MappingreXlsmdbEntity.Field.CreditId26, MappingreXlsmdbEntity.Field.CreditId27,
                        MappingreXlsmdbEntity.Field.CreditId28, MappingreXlsmdbEntity.Field.CreditId29, MappingreXlsmdbEntity.Field.CreditId30,
                        MappingreXlsmdbEntity.Field.CreditId31, MappingreXlsmdbEntity.Field.CreditId32, MappingreXlsmdbEntity.Field.CreditId33,
                        MappingreXlsmdbEntity.Field.CreditId34, MappingreXlsmdbEntity.Field.CreditId35, MappingreXlsmdbEntity.Field.CreditId36,
                        MappingreXlsmdbEntity.Field.CreditId37, MappingreXlsmdbEntity.Field.CreditId38, MappingreXlsmdbEntity.Field.CreditId39,
                        MappingreXlsmdbEntity.Field.CreditId40
                    };

                    for (int idx = 0; idx < mapFields.Length; idx++)
                    {
                        int no = idx + 1;
                        string mapField = mapFields[idx];
                        string fieldName = String.Format("CreditId{0}", no);
                        MappingItem mapItem = mappingItems.Find(x => x.Key == mapField);
                        string fieldValue = null;
                        if (mapItem != null && mapItem.IsChecked)
                        {
                            if (mapItem.FileType != "xls")
                            {
                                return null;
                            }
                            fieldValue = mapItem.XlsName;
                        }
                        if (!data.SetValue(fieldName, fieldValue).IsSuccess)
                        {
                            return null;
                        }
                    }
                }
                #endregion

                #region 課程代碼欄位
                {
                    string[] mapFields = new string[] {
                        MappingreXlsmdbEntity.Field.CourseId1, MappingreXlsmdbEntity.Field.CourseId2, MappingreXlsmdbEntity.Field.CourseId3,
                        MappingreXlsmdbEntity.Field.CourseId4, MappingreXlsmdbEntity.Field.CourseId5, MappingreXlsmdbEntity.Field.CourseId6,
                        MappingreXlsmdbEntity.Field.CourseId7, MappingreXlsmdbEntity.Field.CourseId8, MappingreXlsmdbEntity.Field.CourseId9,
                        MappingreXlsmdbEntity.Field.CourseId10, MappingreXlsmdbEntity.Field.CourseId11, MappingreXlsmdbEntity.Field.CourseId12,
                        MappingreXlsmdbEntity.Field.CourseId13, MappingreXlsmdbEntity.Field.CourseId14, MappingreXlsmdbEntity.Field.CourseId15,
                        MappingreXlsmdbEntity.Field.CourseId16, MappingreXlsmdbEntity.Field.CourseId17, MappingreXlsmdbEntity.Field.CourseId18,
                        MappingreXlsmdbEntity.Field.CourseId19, MappingreXlsmdbEntity.Field.CourseId20, MappingreXlsmdbEntity.Field.CourseId21,
                        MappingreXlsmdbEntity.Field.CourseId22, MappingreXlsmdbEntity.Field.CourseId23, MappingreXlsmdbEntity.Field.CourseId24,
                        MappingreXlsmdbEntity.Field.CourseId25, MappingreXlsmdbEntity.Field.CourseId26, MappingreXlsmdbEntity.Field.CourseId27,
                        MappingreXlsmdbEntity.Field.CourseId28, MappingreXlsmdbEntity.Field.CourseId29, MappingreXlsmdbEntity.Field.CourseId30,
                        MappingreXlsmdbEntity.Field.CourseId31, MappingreXlsmdbEntity.Field.CourseId32, MappingreXlsmdbEntity.Field.CourseId33,
                        MappingreXlsmdbEntity.Field.CourseId34, MappingreXlsmdbEntity.Field.CourseId35, MappingreXlsmdbEntity.Field.CourseId36,
                        MappingreXlsmdbEntity.Field.CourseId37, MappingreXlsmdbEntity.Field.CourseId38, MappingreXlsmdbEntity.Field.CourseId39,
                        MappingreXlsmdbEntity.Field.CourseId40
                    };

                    for (int idx = 0; idx < mapFields.Length; idx++)
                    {
                        int no = idx + 1;
                        string mapField = mapFields[idx];
                        string fieldName = String.Format("CourseId{0}", no);
                        MappingItem mapItem = mappingItems.Find(x => x.Key == mapField);
                        string fieldValue = null;
                        if (mapItem != null && mapItem.IsChecked)
                        {
                            if (mapItem.FileType != "xls")
                            {
                                return null;
                            }
                            fieldValue = mapItem.XlsName;
                        }
                        if (!data.SetValue(fieldName, fieldValue).IsSuccess)
                        {
                            return null;
                        }
                    }
                }
                #endregion

                #region 學分基準或課程學分數欄位
                {
                    string[] mapFields = new string[] {
                        MappingreXlsmdbEntity.Field.Credit1, MappingreXlsmdbEntity.Field.Credit2, MappingreXlsmdbEntity.Field.Credit3,
                        MappingreXlsmdbEntity.Field.Credit4, MappingreXlsmdbEntity.Field.Credit5, MappingreXlsmdbEntity.Field.Credit6,
                        MappingreXlsmdbEntity.Field.Credit7, MappingreXlsmdbEntity.Field.Credit8, MappingreXlsmdbEntity.Field.Credit9,
                        MappingreXlsmdbEntity.Field.Credit10, MappingreXlsmdbEntity.Field.Credit11, MappingreXlsmdbEntity.Field.Credit12,
                        MappingreXlsmdbEntity.Field.Credit13, MappingreXlsmdbEntity.Field.Credit14, MappingreXlsmdbEntity.Field.Credit15,
                        MappingreXlsmdbEntity.Field.Credit16, MappingreXlsmdbEntity.Field.Credit17, MappingreXlsmdbEntity.Field.Credit18,
                        MappingreXlsmdbEntity.Field.Credit19, MappingreXlsmdbEntity.Field.Credit20, MappingreXlsmdbEntity.Field.Credit21,
                        MappingreXlsmdbEntity.Field.Credit22, MappingreXlsmdbEntity.Field.Credit23, MappingreXlsmdbEntity.Field.Credit24,
                        MappingreXlsmdbEntity.Field.Credit25, MappingreXlsmdbEntity.Field.Credit26, MappingreXlsmdbEntity.Field.Credit27,
                        MappingreXlsmdbEntity.Field.Credit28, MappingreXlsmdbEntity.Field.Credit29, MappingreXlsmdbEntity.Field.Credit30,
                        MappingreXlsmdbEntity.Field.Credit31, MappingreXlsmdbEntity.Field.Credit32, MappingreXlsmdbEntity.Field.Credit33,
                        MappingreXlsmdbEntity.Field.Credit34, MappingreXlsmdbEntity.Field.Credit35, MappingreXlsmdbEntity.Field.Credit36,
                        MappingreXlsmdbEntity.Field.Credit37, MappingreXlsmdbEntity.Field.Credit38, MappingreXlsmdbEntity.Field.Credit39,
                        MappingreXlsmdbEntity.Field.Credit40
                    };

                    for (int idx = 0; idx < mapFields.Length; idx++)
                    {
                        int no = idx + 1;
                        string mapField = mapFields[idx];
                        string fieldName = String.Format("Credit{0}", no);
                        MappingItem mapItem = mappingItems.Find(x => x.Key == mapField);
                        string fieldValue = null;
                        if (mapItem != null && mapItem.IsChecked)
                        {
                            if (mapItem.FileType != "xls")
                            {
                                return null;
                            }
                            fieldValue = mapItem.XlsName;
                        }
                        if (!data.SetValue(fieldName, fieldValue).IsSuccess)
                        {
                            return null;
                        }
                    }
                }
                #endregion

                #region 收入科目金額欄位
                #region [MDY:20220808] 2022擴充案 增加收入科目英文名稱欄位
                {
                    string[] mapFields = new string[] {
                        MappingreXlsmdbEntity.Field.Receive1, MappingreXlsmdbEntity.Field.Receive2, MappingreXlsmdbEntity.Field.Receive3,
                        MappingreXlsmdbEntity.Field.Receive4, MappingreXlsmdbEntity.Field.Receive5, MappingreXlsmdbEntity.Field.Receive6,
                        MappingreXlsmdbEntity.Field.Receive7, MappingreXlsmdbEntity.Field.Receive8, MappingreXlsmdbEntity.Field.Receive9,
                        MappingreXlsmdbEntity.Field.Receive10, MappingreXlsmdbEntity.Field.Receive11, MappingreXlsmdbEntity.Field.Receive12,
                        MappingreXlsmdbEntity.Field.Receive13, MappingreXlsmdbEntity.Field.Receive14, MappingreXlsmdbEntity.Field.Receive15,
                        MappingreXlsmdbEntity.Field.Receive16, MappingreXlsmdbEntity.Field.Receive17, MappingreXlsmdbEntity.Field.Receive18,
                        MappingreXlsmdbEntity.Field.Receive19, MappingreXlsmdbEntity.Field.Receive20, MappingreXlsmdbEntity.Field.Receive21,
                        MappingreXlsmdbEntity.Field.Receive22, MappingreXlsmdbEntity.Field.Receive23, MappingreXlsmdbEntity.Field.Receive24,
                        MappingreXlsmdbEntity.Field.Receive25, MappingreXlsmdbEntity.Field.Receive26, MappingreXlsmdbEntity.Field.Receive27,
                        MappingreXlsmdbEntity.Field.Receive28, MappingreXlsmdbEntity.Field.Receive29, MappingreXlsmdbEntity.Field.Receive30,
                        MappingreXlsmdbEntity.Field.Receive31, MappingreXlsmdbEntity.Field.Receive32, MappingreXlsmdbEntity.Field.Receive33,
                        MappingreXlsmdbEntity.Field.Receive34, MappingreXlsmdbEntity.Field.Receive35, MappingreXlsmdbEntity.Field.Receive36,
                        MappingreXlsmdbEntity.Field.Receive37, MappingreXlsmdbEntity.Field.Receive38, MappingreXlsmdbEntity.Field.Receive39,
                        MappingreXlsmdbEntity.Field.Receive40
                    };

                    for (int idx = 0; idx < mapFields.Length; idx++)
                    {
                        int no = idx + 1;
                        string mapField = mapFields[idx];
                        string fieldName = String.Format("Receive{0}", no);
                        MappingItem mapItem = mappingItems.Find(x => x.Key == mapField);
                        string fieldValue = null;
                        string engFieldName = null, engFieldValue = null;
                        if (mapItem != null && mapItem.IsChecked)
                        {
                            if (mapItem.FileType != "xls")
                            {
                                return null;
                            }
                            fieldValue = mapItem.XlsName;
                            engFieldName = mapItem.EngFieldName;
                            engFieldValue = mapItem.EngFieldValue;
                        }
                        if (!data.SetValue(fieldName, fieldValue).IsSuccess)
                        {
                            return null;
                        }
                        if (!String.IsNullOrEmpty(engFieldName))
                        {
                            if (!data.SetValue(engFieldName, engFieldValue).IsSuccess)
                            {
                                return null;
                            }
                        }
                    }
                }
                #endregion
                #endregion

                #region [MDY:20220808] 2022擴充案 備註項目對照欄位 & 英文標題欄位
                {
                    string[] mapFields = new string[]
                    {
                        MappingreXlsmdbEntity.Field.Memo01, MappingreXlsmdbEntity.Field.Memo02,
                        MappingreXlsmdbEntity.Field.Memo03, MappingreXlsmdbEntity.Field.Memo04,
                        MappingreXlsmdbEntity.Field.Memo05, MappingreXlsmdbEntity.Field.Memo06,
                        MappingreXlsmdbEntity.Field.Memo07, MappingreXlsmdbEntity.Field.Memo08,
                        MappingreXlsmdbEntity.Field.Memo09, MappingreXlsmdbEntity.Field.Memo10,
                        MappingreXlsmdbEntity.Field.Memo11, MappingreXlsmdbEntity.Field.Memo12,
                        MappingreXlsmdbEntity.Field.Memo13, MappingreXlsmdbEntity.Field.Memo14,
                        MappingreXlsmdbEntity.Field.Memo15, MappingreXlsmdbEntity.Field.Memo16,
                        MappingreXlsmdbEntity.Field.Memo17, MappingreXlsmdbEntity.Field.Memo18,
                        MappingreXlsmdbEntity.Field.Memo19, MappingreXlsmdbEntity.Field.Memo20,
                        MappingreXlsmdbEntity.Field.Memo21,
                    };
                    string[] fieldNames = new string[]
                    {
                        "Memo01", "Memo02",
                        "Memo03", "Memo04",
                        "Memo05", "Memo06",
                        "Memo07", "Memo08",
                        "Memo09", "Memo10",
                        "Memo11", "Memo12",
                        "Memo13", "Memo14",
                        "Memo15", "Memo16",
                        "Memo17", "Memo18",
                        "Memo19", "Memo20",
                        "Memo21"
                    };

                    for (int idx = 0; idx < mapFields.Length; idx++)
                    {
                        string mapField = mapFields[idx];
                        string fieldName = fieldNames[idx];
                        MappingItem mapItem = mappingItems.Find(x => x.Key == mapField);
                        string fieldValue = null;
                        string engFieldName = null, engFieldValue = null;
                        if (mapItem != null && mapItem.IsChecked)
                        {
                            if (mapItem.FileType != "xls")
                            {
                                return null;
                            }
                            fieldValue = mapItem.XlsName;
                            engFieldName = mapItem.EngFieldName;
                            engFieldValue = mapItem.EngFieldValue;
                        }
                        if (!data.SetValue(fieldName, fieldValue).IsSuccess)
                        {
                            return null;
                        }
                        if (!String.IsNullOrEmpty(engFieldName))
                        {
                            if (!data.SetValue(engFieldName, engFieldValue).IsSuccess)
                            {
                                return null;
                            }
                        }
                    }
                }
                #endregion
            }
            return data;
        }

        private bool CheckTxtMappingItems(List<MappingItem> mappingItems)
        {
            if (mappingItems == null || mappingItems.Count == 0)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無任何對照欄位資料");
                this.ShowSystemMessage(msg);
                return false;
            }

            #region [MDY:2018xxxx] 收入科目金額的起始位置允許 0，表示略過該項目
            //收入科目金額的欄位名稱
            List<string> receiveFieldNames = new List<string>(new string[] {
                MappingreXlsmdbEntity.Field.Receive1, MappingreXlsmdbEntity.Field.Receive2, MappingreXlsmdbEntity.Field.Receive3,
                MappingreXlsmdbEntity.Field.Receive4, MappingreXlsmdbEntity.Field.Receive5, MappingreXlsmdbEntity.Field.Receive6,
                MappingreXlsmdbEntity.Field.Receive7, MappingreXlsmdbEntity.Field.Receive8, MappingreXlsmdbEntity.Field.Receive9,
                MappingreXlsmdbEntity.Field.Receive10, MappingreXlsmdbEntity.Field.Receive11, MappingreXlsmdbEntity.Field.Receive12,
                MappingreXlsmdbEntity.Field.Receive13, MappingreXlsmdbEntity.Field.Receive14, MappingreXlsmdbEntity.Field.Receive15,
                MappingreXlsmdbEntity.Field.Receive16, MappingreXlsmdbEntity.Field.Receive17, MappingreXlsmdbEntity.Field.Receive18,
                MappingreXlsmdbEntity.Field.Receive19, MappingreXlsmdbEntity.Field.Receive20, MappingreXlsmdbEntity.Field.Receive21,
                MappingreXlsmdbEntity.Field.Receive22, MappingreXlsmdbEntity.Field.Receive23, MappingreXlsmdbEntity.Field.Receive24,
                MappingreXlsmdbEntity.Field.Receive25, MappingreXlsmdbEntity.Field.Receive26, MappingreXlsmdbEntity.Field.Receive27,
                MappingreXlsmdbEntity.Field.Receive28, MappingreXlsmdbEntity.Field.Receive29, MappingreXlsmdbEntity.Field.Receive30,
                MappingreXlsmdbEntity.Field.Receive31, MappingreXlsmdbEntity.Field.Receive32, MappingreXlsmdbEntity.Field.Receive33,
                MappingreXlsmdbEntity.Field.Receive34, MappingreXlsmdbEntity.Field.Receive35, MappingreXlsmdbEntity.Field.Receive36,
                MappingreXlsmdbEntity.Field.Receive37, MappingreXlsmdbEntity.Field.Receive38, MappingreXlsmdbEntity.Field.Receive39,
                MappingreXlsmdbEntity.Field.Receive40
            });

            //收入科目金額是否有勾選
            bool isReceiveChecked = false;
            //收入科目金額是否未指定試算表欄位名稱
            bool notReceiveXlsName = true;
            #endregion

            int checkCount = 0;
            foreach (MappingItem item in mappingItems)
            {
                if (item.IsChecked && item.MaxItemCount == 0)
                {
                    checkCount++;
                    if (item.FileType != "txt")
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized(item.Name + "檔案型別不正確");
                        this.ShowSystemMessage(msg);
                        return false;
                    }

                    #region [MDY:2018xxxx] 收入科目金額的起始位置允許 0，表示略過該項目 (null 才算未設定)
                    #region [OLD]
                    //if (String.IsNullOrEmpty(item.TxtStart))
                    //{
                    //    //[TODO] 固定顯示訊息的收集
                    //    string msg = this.GetLocalized(item.Name + "起始位置");
                    //    this.ShowMustInputAlert(msg);
                    //    return false;
                    //}
                    //if (String.IsNullOrEmpty(item.TxtLength))
                    //{
                    //    //[TODO] 固定顯示訊息的收集
                    //    string msg = this.GetLocalized(item.Name + "長度");
                    //    this.ShowMustInputAlert(msg);
                    //    return false;
                    //}
                    //int? start = item.GetTxtStart();
                    //if (start == null || start.Value < 1)
                    //{
                    //    //[TODO] 固定顯示訊息的收集
                    //    string msg = this.GetLocalized(item.Name + "起始位置限輸入大於0的整數");
                    //    this.ShowSystemMessage(msg);
                    //    return false;
                    //}
                    //int? length = item.GetTxtLength();
                    //if (length == null || length.Value < 1)
                    //{
                    //    //[TODO] 固定顯示訊息的收集
                    //    string msg = this.GetLocalized(item.Name + "長度限輸入大於0的整數");
                    //    this.ShowSystemMessage(msg);
                    //    return false;
                    //}
                    #endregion

                    if (String.IsNullOrEmpty(item.TxtStart))
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized(item.Name + "起始位置");
                        this.ShowMustInputAlert(msg);
                        return false;
                    }
                    if (String.IsNullOrEmpty(item.TxtLength))
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized(item.Name + "長度");
                        this.ShowMustInputAlert(msg);
                        return false;
                    }

                    int? start = item.GetTxtStart();
                    if (!start.HasValue)
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized(item.Name + "起始位置限輸入整數值");
                        this.ShowSystemMessage(msg);
                        return false;
                    }
                    int? length = item.GetTxtLength();
                    if (!length.HasValue)
                    {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized(item.Name + "長度限輸入整數值");
                            this.ShowSystemMessage(msg);
                            return false;
                    }

                    if (receiveFieldNames.Contains(item.FieldName))
                    {
                        isReceiveChecked = true;
                        if (start.Value < 0)
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized(item.Name + "起始位置限輸入大於等於0的整數");
                            this.ShowSystemMessage(msg);
                            return false;
                        }
                        if (start.Value == 0 && length.Value != 0)
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized(item.Name + "起始位置等於 0 時，長度必須為 0");
                            this.ShowSystemMessage(msg);
                            return false;
                        }
                        if (start.Value > 0 && length.Value < 1)
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized(item.Name + "長度限輸入大於0的整數");
                            this.ShowSystemMessage(msg);
                            return false;
                        }
                        if (notReceiveXlsName && start.Value > 0)
                        {
                            notReceiveXlsName = false;
                        }
                    }
                    else
                    {
                        if (start.Value < 1)
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized(item.Name + "起始位置限輸入大於0的整數");
                            this.ShowSystemMessage(msg);
                            return false;
                        }
                        if (length.Value < 1)
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized(item.Name + "長度限輸入大於0的整數");
                            this.ShowSystemMessage(msg);
                            return false;
                        }
                    }
                    #endregion
                }
            }
            if (checkCount == 0)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("未選任何項目");
                this.ShowSystemMessage(msg);
                return false;
            }

            #region [MDY:2018xxxx] 如果有勾選收入科目金額，至少要有一筆收入科目金額試算表欄位名稱有值
            if (isReceiveChecked && notReceiveXlsName)
            {
                string msg = this.GetLocalized("勾選收入科目金額，但未指定任何一個收入科目金額的位置設定");
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 取得指定 TXT 檔案格式設定資料的 MappingItem 資料集合
        /// </summary>
        /// <param name="data">指定 TXT 檔案格式設定資料</param>
        /// <returns>傳回 MappingItem 集合</returns>
        private List<MappingItem> GetMappingItems(MappingreTxtEntity data)
        {
            if (data == null)
            {
                return new List<MappingItem>(0);
            }

            List<MappingItem> items = new List<MappingItem>();

            #region 單一項目的欄位
            {
                string[] fields = new string[] {
                    #region 學生基本資料對照欄位
                    MappingreTxtEntity.Field.StuIdS,

                    MappingreTxtEntity.Field.StuNameS, MappingreTxtEntity.Field.IdNumberS,
                    MappingreTxtEntity.Field.StuBirthdayS, MappingreTxtEntity.Field.StuTelS,
                    MappingreTxtEntity.Field.StuAddcodeS, MappingreTxtEntity.Field.StuAddS,
                    MappingreTxtEntity.Field.EmailS, MappingreTxtEntity.Field.StuParentS,
                    #endregion

                    #region [MDY:20160131] 增加資料序號與繳款期限對照欄位
                    MappingreTxtEntity.Field.OldSeqS, MappingreTxtEntity.Field.PayDueDateS,
                    #endregion

                    #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標對照欄位
                    MappingreTxtEntity.Field.NCCardFlagS,
                    #endregion

                    MappingreTxtEntity.Field.StuGradeS, MappingreTxtEntity.Field.StuHidS,

                    #region [MDY:20220808] 2022擴充案 增加 學籍資料英文名稱對照欄位
                    MappingreTxtEntity.Field.ClassIdS, MappingreTxtEntity.Field.ClassNameS,
                    MappingreTxtEntity.Field.ClassENameS,
                    MappingreTxtEntity.Field.DeptIdS, MappingreTxtEntity.Field.DeptNameS,
                    MappingreTxtEntity.Field.DeptENameS,
                    MappingreTxtEntity.Field.CollegeIdS, MappingreTxtEntity.Field.CollegeNameS,
                    MappingreTxtEntity.Field.CollegeENameS,
                    MappingreTxtEntity.Field.MajorIdS, MappingreTxtEntity.Field.MajorNameS,
                    MappingreTxtEntity.Field.MajorENameS,
                    #endregion

                    #region [MDY:20220808] 2022擴充案 增加 減免、就貸、住宿英文名稱對照欄位
                    MappingreTxtEntity.Field.ReduceIdS, MappingreTxtEntity.Field.ReduceNameS,
                    MappingreTxtEntity.Field.ReduceENameS,
                    MappingreTxtEntity.Field.LoanIdS, MappingreTxtEntity.Field.LoanNameS,
                    MappingreTxtEntity.Field.LoanENameS,
                    MappingreTxtEntity.Field.DormIdS, MappingreTxtEntity.Field.DormNameS,
                    MappingreTxtEntity.Field.DormENameS,
                    #endregion

                    #region [MDY:20220808] 2022擴充案 增加 身分註記英文名稱對照欄位
                    MappingreTxtEntity.Field.IdentifyId1S, MappingreTxtEntity.Field.IdentifyName1S,
                    MappingreTxtEntity.Field.IdentifyEName1S,
                    MappingreTxtEntity.Field.IdentifyId2S, MappingreTxtEntity.Field.IdentifyName2S,
                    MappingreTxtEntity.Field.IdentifyEName2S,
                    MappingreTxtEntity.Field.IdentifyId3S, MappingreTxtEntity.Field.IdentifyName3S,
                    MappingreTxtEntity.Field.IdentifyEName3S,
                    MappingreTxtEntity.Field.IdentifyId4S, MappingreTxtEntity.Field.IdentifyName4S,
                    MappingreTxtEntity.Field.IdentifyEName4S,
                    MappingreTxtEntity.Field.IdentifyId5S, MappingreTxtEntity.Field.IdentifyName5S,
                    MappingreTxtEntity.Field.IdentifyEName5S,
                    MappingreTxtEntity.Field.IdentifyId6S, MappingreTxtEntity.Field.IdentifyName6S,
                    MappingreTxtEntity.Field.IdentifyEName6S,
                    #endregion

                    MappingreTxtEntity.Field.StuHourS, MappingreTxtEntity.Field.StuCreditS,
                    MappingreTxtEntity.Field.LoanAmountS,

                    MappingreTxtEntity.Field.ReceiveAmountS,
                    MappingreTxtEntity.Field.SeriorNoS, MappingreTxtEntity.Field.CancelNoS,

                    #region 轉帳資料對照欄位
                    MappingreTxtEntity.Field.DeductBankidS, MappingreTxtEntity.Field.DeductAccountnoS,
                    MappingreTxtEntity.Field.DeductAccountnameS, MappingreTxtEntity.Field.DeductAccountidS,
                    #endregion

                    MappingreTxtEntity.Field.Memo01S, MappingreTxtEntity.Field.Memo02S,
                    MappingreTxtEntity.Field.Memo03S, MappingreTxtEntity.Field.Memo04S,
                    MappingreTxtEntity.Field.Memo05S, MappingreTxtEntity.Field.Memo06S,
                    MappingreTxtEntity.Field.Memo07S, MappingreTxtEntity.Field.Memo08S,
                    MappingreTxtEntity.Field.Memo09S, MappingreTxtEntity.Field.Memo10S,
                    MappingreTxtEntity.Field.Memo11S, MappingreTxtEntity.Field.Memo12S,
                    MappingreTxtEntity.Field.Memo13S, MappingreTxtEntity.Field.Memo14S,
                    MappingreTxtEntity.Field.Memo15S, MappingreTxtEntity.Field.Memo16S,
                    MappingreTxtEntity.Field.Memo17S, MappingreTxtEntity.Field.Memo18S,
                    MappingreTxtEntity.Field.Memo19S, MappingreTxtEntity.Field.Memo20S,
                    MappingreTxtEntity.Field.Memo21S
                };

                int?[] startValues = new int?[] {
                    #region 學生基本資料對照欄位
                    data.StuIdS,

                    data.StuNameS, data.IdNumberS,
                    data.StuBirthdayS, data.StuTelS,
                    data.StuAddcodeS, data.StuAddS,
                    data.EmailS, data.StuParentS,
                    #endregion

                    #region [MDY:20160131] 增加資料序號與繳款期限對照欄位
                    data.OldSeqS, data.PayDueDateS,
                    #endregion

                    #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標對照欄位
                    data.NCCardFlagS,
                    #endregion

                    data.StuGradeS, data.StuHidS,

                    #region [MDY:20220808] 2022擴充案 增加 學籍資料英文名稱對照欄位
                    data.ClassIdS, data.ClassNameS,
                    data.ClassENameS,
                    data.DeptIdS, data.DeptNameS,
                    data.DeptENameS,
                    data.CollegeIdS, data.CollegeNameS,
                    data.CollegeENameS,
                    data.MajorIdS, data.MajorNameS,
                    data.MajorENameS,
                    #endregion

                    #region [MDY:20220808] 2022擴充案 增加 減免、就貸、住宿英文名稱對照欄位
                    data.ReduceIdS, data.ReduceNameS,
                    data.ReduceENameS,
                    data.LoanIdS, data.LoanNameS,
                    data.LoanENameS,
                    data.DormIdS, data.DormNameS,
                    data.DormENameS,
                    #endregion

                    #region [MDY:20220808] 2022擴充案 增加 身分註記英文名稱對照欄位
                    data.IdentifyId1S, data.IdentifyName1S,
                    data.IdentifyEName1S,
                    data.IdentifyId2S, data.IdentifyName2S,
                    data.IdentifyEName2S,
                    data.IdentifyId3S, data.IdentifyName3S,
                    data.IdentifyEName3S,
                    data.IdentifyId4S, data.IdentifyName4S,
                    data.IdentifyEName4S,
                    data.IdentifyId5S, data.IdentifyName5S,
                    data.IdentifyEName5S,
                    data.IdentifyId6S, data.IdentifyName6S,
                    data.IdentifyEName6S,
                    #endregion

                    data.StuHourS, data.StuCreditS,
                    data.LoanAmountS,

                    data.ReceiveAmountS,
                    data.SeriorNoS, data.CancelNoS,

                    #region 轉帳資料對照欄位
                    data.DeductBankidS, data.DeductAccountnoS,
                    data.DeductAccountnameS, data.DeductAccountidS,
                    #endregion

                    data.Memo01S, data.Memo02S,
                    data.Memo03S, data.Memo04S,
                    data.Memo05S, data.Memo06S,
                    data.Memo07S, data.Memo08S,
                    data.Memo09S, data.Memo10S,
                    data.Memo11S, data.Memo12S,
                    data.Memo13S, data.Memo14S,
                    data.Memo15S, data.Memo16S,
                    data.Memo17S, data.Memo18S,
                    data.Memo19S, data.Memo20S,
                    data.Memo21S
                };

                int?[] lengthValues = new int?[] {
                    #region 學生基本資料對照欄位
                    data.StuIdL,

                    data.StuNameL, data.IdNumberL,
                    data.StuBirthdayL, data.StuTelL,
                    data.StuAddcodeL, data.StuAddL,
                    data.EmailL, data.StuParentL,
                    #endregion

                    #region [MDY:20160131] 增加資料序號與繳款期限對照欄位
                    data.OldSeqL, data.PayDueDateL,
                    #endregion

                    #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標對照欄位
                    data.NCCardFlagL,
                    #endregion

                    data.StuGradeL, data.StuHidL,

                    #region [MDY:20220808] 2022擴充案 增加 學籍資料英文名稱對照欄位
                    data.ClassIdL, data.ClassNameL,
                    data.ClassENameL,
                    data.DeptIdL, data.DeptNameL,
                    data.DeptENameL,
                    data.CollegeIdL, data.CollegeNameL,
                    data.CollegeENameL,
                    data.MajorIdL, data.MajorNameL,
                    data.MajorENameL,
                    #endregion

                    #region [MDY:20220808] 2022擴充案 增加 減免、就貸、住宿英文名稱對照欄位
                    data.ReduceIdL, data.ReduceNameL,
                    data.ReduceENameL,
                    data.LoanIdL, data.LoanNameL,
                    data.LoanENameL,
                    data.DormIdL, data.DormNameL,
                    data.DormENameL,
                    #endregion

                    #region [MDY:20220808] 2022擴充案 增加 身分註記英文名稱對照欄位
                    data.IdentifyId1L, data.IdentifyName1L,
                    data.IdentifyEName1L,
                    data.IdentifyId2L, data.IdentifyName2L,
                    data.IdentifyEName2L,
                    data.IdentifyId3L, data.IdentifyName3L,
                    data.IdentifyEName3L,
                    data.IdentifyId4L, data.IdentifyName4L,
                    data.IdentifyEName4L,
                    data.IdentifyId5L, data.IdentifyName5L,
                    data.IdentifyEName5L,
                    data.IdentifyId6L, data.IdentifyName6L,
                    data.IdentifyEName6L,
                    #endregion

                    data.StuHourL, data.StuCreditL,
                    data.LoanAmountL,

                    data.ReceiveAmountL,
                    data.SeriorNoL, data.CancelNoL,

                    #region 轉帳資料對照欄位
                    data.DeductBankidL, data.DeductAccountnoL,
                    data.DeductAccountnameL, data.DeductAccountidL,
                    #endregion

                    data.Memo01L, data.Memo02L,
                    data.Memo03L, data.Memo04L,
                    data.Memo05L, data.Memo06L,
                    data.Memo07L, data.Memo08L,
                    data.Memo09L, data.Memo10L,
                    data.Memo11L, data.Memo12L,
                    data.Memo13L, data.Memo14L,
                    data.Memo15L, data.Memo16L,
                    data.Memo17L, data.Memo18L,
                    data.Memo19L, data.Memo20L,
                    data.Memo21L
                };

                for (int idx = 0; idx < fields.Length; idx++)
                {
                    string key = fields[idx].Substring(0, fields[idx].Length - 2);
                    MappingItem item = MappingItem.CreateByTxt(key, null, key, startValues[idx], lengthValues[idx]);
                    items.Add(item);
                }
            }
            #endregion

            #region 學分基準欄位
            {
                string[] fields = new string[] {
                    MappingreTxtEntity.Field.CreditId1S, MappingreTxtEntity.Field.CreditId2S, MappingreTxtEntity.Field.CreditId3S,
                    MappingreTxtEntity.Field.CreditId4S, MappingreTxtEntity.Field.CreditId5S, MappingreTxtEntity.Field.CreditId6S,
                    MappingreTxtEntity.Field.CreditId7S, MappingreTxtEntity.Field.CreditId8S, MappingreTxtEntity.Field.CreditId9S,
                    MappingreTxtEntity.Field.CreditId10S, MappingreTxtEntity.Field.CreditId11S, MappingreTxtEntity.Field.CreditId12S,
                    MappingreTxtEntity.Field.CreditId13S, MappingreTxtEntity.Field.CreditId14S, MappingreTxtEntity.Field.CreditId15S,
                    MappingreTxtEntity.Field.CreditId16S, MappingreTxtEntity.Field.CreditId17S, MappingreTxtEntity.Field.CreditId18S,
                    MappingreTxtEntity.Field.CreditId19S, MappingreTxtEntity.Field.CreditId20S, MappingreTxtEntity.Field.CreditId21S,
                    MappingreTxtEntity.Field.CreditId22S, MappingreTxtEntity.Field.CreditId23S, MappingreTxtEntity.Field.CreditId24S,
                    MappingreTxtEntity.Field.CreditId25S, MappingreTxtEntity.Field.CreditId26S, MappingreTxtEntity.Field.CreditId27S,
                    MappingreTxtEntity.Field.CreditId28S, MappingreTxtEntity.Field.CreditId29S, MappingreTxtEntity.Field.CreditId30S,
                    MappingreTxtEntity.Field.CreditId31S, MappingreTxtEntity.Field.CreditId32S, MappingreTxtEntity.Field.CreditId33S,
                    MappingreTxtEntity.Field.CreditId34S, MappingreTxtEntity.Field.CreditId35S, MappingreTxtEntity.Field.CreditId36S,
                    MappingreTxtEntity.Field.CreditId37S, MappingreTxtEntity.Field.CreditId38S, MappingreTxtEntity.Field.CreditId39S,
                    MappingreTxtEntity.Field.CreditId40S
                };
                int?[] startValues = new int?[] {
                    data.CreditId1S, data.CreditId2S, data.CreditId3S,
                    data.CreditId4S, data.CreditId5S, data.CreditId6S,
                    data.CreditId7S, data.CreditId8S, data.CreditId9S,
                    data.CreditId10S, data.CreditId11S, data.CreditId12S,
                    data.CreditId13S, data.CreditId14S, data.CreditId15S,
                    data.CreditId16S, data.CreditId17S, data.CreditId18S,
                    data.CreditId19S, data.CreditId20S, data.CreditId21S,
                    data.CreditId22S, data.CreditId23S, data.CreditId24S,
                    data.CreditId25S, data.CreditId26S, data.CreditId27S,
                    data.CreditId28S, data.CreditId29S, data.CreditId30S,
                    data.CreditId31S, data.CreditId32S, data.CreditId33S,
                    data.CreditId34S, data.CreditId35S, data.CreditId36S,
                    data.CreditId37S, data.CreditId38S, data.CreditId39S,
                    data.CreditId40S
                };
                int?[] lengthValues = new int?[] {
                    data.CreditId1L, data.CreditId2L, data.CreditId3L,
                    data.CreditId4L, data.CreditId5L, data.CreditId6L,
                    data.CreditId7L, data.CreditId8L, data.CreditId9L,
                    data.CreditId10L, data.CreditId11L, data.CreditId12L,
                    data.CreditId13L, data.CreditId14L, data.CreditId15L,
                    data.CreditId16L, data.CreditId17L, data.CreditId18L,
                    data.CreditId19L, data.CreditId20L, data.CreditId21L,
                    data.CreditId22L, data.CreditId23L, data.CreditId24L,
                    data.CreditId25L, data.CreditId26L, data.CreditId27L,
                    data.CreditId28L, data.CreditId29L, data.CreditId30L,
                    data.CreditId31L, data.CreditId32L, data.CreditId33L,
                    data.CreditId34L, data.CreditId35L, data.CreditId36L,
                    data.CreditId37L, data.CreditId38L, data.CreditId39L,
                    data.CreditId40L
                };
                int count = 0;
                for (int idx = 0; idx < fields.Length; idx++)
                {
                    string key = fields[idx].Substring(0, fields[idx].Length - 2);
                    MappingItem item = MappingItem.CreateByTxt(key, null, key, startValues[idx], lengthValues[idx]);
                    items.Add(item);
                    if (item.IsChecked)
                    {
                        count++;
                    }
                }
                items.Add(MappingItem.CreateByItemCount("Credit_Id", "學分基準", 40, (count > 0 ? count.ToString() : String.Empty)));
            }
            #endregion

            #region 課程代碼欄位
            {
                string[] fields = new string[] {
                    MappingreTxtEntity.Field.CourseId1S, MappingreTxtEntity.Field.CourseId2S, MappingreTxtEntity.Field.CourseId3S,
                    MappingreTxtEntity.Field.CourseId4S, MappingreTxtEntity.Field.CourseId5S, MappingreTxtEntity.Field.CourseId6S,
                    MappingreTxtEntity.Field.CourseId7S, MappingreTxtEntity.Field.CourseId8S, MappingreTxtEntity.Field.CourseId9S,
                    MappingreTxtEntity.Field.CourseId10S, MappingreTxtEntity.Field.CourseId11S, MappingreTxtEntity.Field.CourseId12S,
                    MappingreTxtEntity.Field.CourseId13S, MappingreTxtEntity.Field.CourseId14S, MappingreTxtEntity.Field.CourseId15S,
                    MappingreTxtEntity.Field.CourseId16S, MappingreTxtEntity.Field.CourseId17S, MappingreTxtEntity.Field.CourseId18S,
                    MappingreTxtEntity.Field.CourseId19S, MappingreTxtEntity.Field.CourseId20S, MappingreTxtEntity.Field.CourseId21S,
                    MappingreTxtEntity.Field.CourseId22S, MappingreTxtEntity.Field.CourseId23S, MappingreTxtEntity.Field.CourseId24S,
                    MappingreTxtEntity.Field.CourseId25S, MappingreTxtEntity.Field.CourseId26S, MappingreTxtEntity.Field.CourseId27S,
                    MappingreTxtEntity.Field.CourseId28S, MappingreTxtEntity.Field.CourseId29S, MappingreTxtEntity.Field.CourseId30S,
                    MappingreTxtEntity.Field.CourseId31S, MappingreTxtEntity.Field.CourseId32S, MappingreTxtEntity.Field.CourseId33S,
                    MappingreTxtEntity.Field.CourseId34S, MappingreTxtEntity.Field.CourseId35S, MappingreTxtEntity.Field.CourseId36S,
                    MappingreTxtEntity.Field.CourseId37S, MappingreTxtEntity.Field.CourseId38S, MappingreTxtEntity.Field.CourseId39S,
                    MappingreTxtEntity.Field.CourseId40S
                };
                int?[] startValues = new int?[] {
                    data.CourseId1S, data.CourseId2S, data.CourseId3S,
                    data.CourseId4S, data.CourseId5S, data.CourseId6S,
                    data.CourseId7S, data.CourseId8S, data.CourseId9S,
                    data.CourseId10S, data.CourseId11S, data.CourseId12S,
                    data.CourseId13S, data.CourseId14S, data.CourseId15S,
                    data.CourseId16S, data.CourseId17S, data.CourseId18S,
                    data.CourseId19S, data.CourseId20S, data.CourseId21S,
                    data.CourseId22S, data.CourseId23S, data.CourseId24S,
                    data.CourseId25S, data.CourseId26S, data.CourseId27S,
                    data.CourseId28S, data.CourseId29S, data.CourseId30S,
                    data.CourseId31S, data.CourseId32S, data.CourseId33S,
                    data.CourseId34S, data.CourseId35S, data.CourseId36S,
                    data.CourseId37S, data.CourseId38S, data.CourseId39S,
                    data.CourseId40S
                };
                int?[] lengthValues = new int?[] {
                    data.CourseId1L, data.CourseId2L, data.CourseId3L,
                    data.CourseId4L, data.CourseId5L, data.CourseId6L,
                    data.CourseId7L, data.CourseId8L, data.CourseId9L,
                    data.CourseId10L, data.CourseId11L, data.CourseId12L,
                    data.CourseId13L, data.CourseId14L, data.CourseId15L,
                    data.CourseId16L, data.CourseId17L, data.CourseId18L,
                    data.CourseId19L, data.CourseId20L, data.CourseId21L,
                    data.CourseId22L, data.CourseId23L, data.CourseId24L,
                    data.CourseId25L, data.CourseId26L, data.CourseId27L,
                    data.CourseId28L, data.CourseId29L, data.CourseId30L,
                    data.CourseId31L, data.CourseId32L, data.CourseId33L,
                    data.CourseId34L, data.CourseId35L, data.CourseId36L,
                    data.CourseId37L, data.CourseId38L, data.CourseId39L,
                    data.CourseId40L
                };
                int count = 0;
                for (int idx = 0; idx < fields.Length; idx++)
                {
                    string key = fields[idx].Substring(0, fields[idx].Length - 2);
                    MappingItem item = MappingItem.CreateByTxt(key, null, key, startValues[idx], lengthValues[idx]);
                    items.Add(item);
                    if (item.IsChecked)
                    {
                        count++;
                    }
                }
                items.Add(MappingItem.CreateByItemCount("Course_Id", "課程代碼", 40, (count > 0 ? count.ToString() : String.Empty)));
            }
            #endregion

            #region 學分基準或課程學分數欄位
            {
                string[] fields = new string[] {
                    MappingreTxtEntity.Field.Credit1S, MappingreTxtEntity.Field.Credit2S, MappingreTxtEntity.Field.Credit3S,
                    MappingreTxtEntity.Field.Credit4S, MappingreTxtEntity.Field.Credit5S, MappingreTxtEntity.Field.Credit6S,
                    MappingreTxtEntity.Field.Credit7S, MappingreTxtEntity.Field.Credit8S, MappingreTxtEntity.Field.Credit9S,
                    MappingreTxtEntity.Field.Credit10S, MappingreTxtEntity.Field.Credit11S, MappingreTxtEntity.Field.Credit12S,
                    MappingreTxtEntity.Field.Credit13S, MappingreTxtEntity.Field.Credit14S, MappingreTxtEntity.Field.Credit15S,
                    MappingreTxtEntity.Field.Credit16S, MappingreTxtEntity.Field.Credit17S, MappingreTxtEntity.Field.Credit18S,
                    MappingreTxtEntity.Field.Credit19S, MappingreTxtEntity.Field.Credit20S, MappingreTxtEntity.Field.Credit21S,
                    MappingreTxtEntity.Field.Credit22S, MappingreTxtEntity.Field.Credit23S, MappingreTxtEntity.Field.Credit24S,
                    MappingreTxtEntity.Field.Credit25S, MappingreTxtEntity.Field.Credit26S, MappingreTxtEntity.Field.Credit27S,
                    MappingreTxtEntity.Field.Credit28S, MappingreTxtEntity.Field.Credit29S, MappingreTxtEntity.Field.Credit30S,
                    MappingreTxtEntity.Field.Credit31S, MappingreTxtEntity.Field.Credit32S, MappingreTxtEntity.Field.Credit33S,
                    MappingreTxtEntity.Field.Credit34S, MappingreTxtEntity.Field.Credit35S, MappingreTxtEntity.Field.Credit36S,
                    MappingreTxtEntity.Field.Credit37S, MappingreTxtEntity.Field.Credit38S, MappingreTxtEntity.Field.Credit39S,
                    MappingreTxtEntity.Field.Credit40S
                };
                int?[] startValues = new int?[] {
                    data.Credit1S, data.Credit2S, data.Credit3S,
                    data.Credit4S, data.Credit5S, data.Credit6S,
                    data.Credit7S, data.Credit8S, data.Credit9S,
                    data.Credit10S, data.Credit11S, data.Credit12S,
                    data.Credit13S, data.Credit14S, data.Credit15S,
                    data.Credit16S, data.Credit17S, data.Credit18S,
                    data.Credit19S, data.Credit20S, data.Credit21S,
                    data.Credit22S, data.Credit23S, data.Credit24S,
                    data.Credit25S, data.Credit26S, data.Credit27S,
                    data.Credit28S, data.Credit29S, data.Credit30S,
                    data.Credit31S, data.Credit32S, data.Credit33S,
                    data.Credit34S, data.Credit35S, data.Credit36S,
                    data.Credit37S, data.Credit38S, data.Credit39S,
                    data.Credit40S
                };
                int?[] lengthValues = new int?[] {
                    data.Credit1L, data.Credit2L, data.Credit3L,
                    data.Credit4L, data.Credit5L, data.Credit6L,
                    data.Credit7L, data.Credit8L, data.Credit9L,
                    data.Credit10L, data.Credit11L, data.Credit12L,
                    data.Credit13L, data.Credit14L, data.Credit15L,
                    data.Credit16L, data.Credit17L, data.Credit18L,
                    data.Credit19L, data.Credit20L, data.Credit21L,
                    data.Credit22L, data.Credit23L, data.Credit24L,
                    data.Credit25L, data.Credit26L, data.Credit27L,
                    data.Credit28L, data.Credit29L, data.Credit30L,
                    data.Credit31L, data.Credit32L, data.Credit33L,
                    data.Credit34L, data.Credit35L, data.Credit36L,
                    data.Credit37L, data.Credit38L, data.Credit39L,
                    data.Credit40L
                };
                int count = 0;
                for (int idx = 0; idx < fields.Length; idx++)
                {
                    string key = fields[idx].Substring(0, fields[idx].Length - 2);
                    MappingItem item = MappingItem.CreateByTxt(key, null, key, startValues[idx], lengthValues[idx]);
                    items.Add(item);
                    if (item.IsChecked)
                    {
                        count++;
                    }
                }
                items.Add(MappingItem.CreateByItemCount("Credit", "學分數", 40, (count > 0 ? count.ToString() : String.Empty)));
            }
            #endregion

            #region 收入科目金額欄位
            {
                string[] fields = new string[] {
                    MappingreTxtEntity.Field.Receive1S, MappingreTxtEntity.Field.Receive2S, MappingreTxtEntity.Field.Receive3S,
                    MappingreTxtEntity.Field.Receive4S, MappingreTxtEntity.Field.Receive5S, MappingreTxtEntity.Field.Receive6S,
                    MappingreTxtEntity.Field.Receive7S, MappingreTxtEntity.Field.Receive8S, MappingreTxtEntity.Field.Receive9S,
                    MappingreTxtEntity.Field.Receive10S, MappingreTxtEntity.Field.Receive11S, MappingreTxtEntity.Field.Receive12S,
                    MappingreTxtEntity.Field.Receive13S, MappingreTxtEntity.Field.Receive14S, MappingreTxtEntity.Field.Receive15S,
                    MappingreTxtEntity.Field.Receive16S, MappingreTxtEntity.Field.Receive17S, MappingreTxtEntity.Field.Receive18S,
                    MappingreTxtEntity.Field.Receive19S, MappingreTxtEntity.Field.Receive20S, MappingreTxtEntity.Field.Receive21S,
                    MappingreTxtEntity.Field.Receive22S, MappingreTxtEntity.Field.Receive23S, MappingreTxtEntity.Field.Receive24S,
                    MappingreTxtEntity.Field.Receive25S, MappingreTxtEntity.Field.Receive26S, MappingreTxtEntity.Field.Receive27S,
                    MappingreTxtEntity.Field.Receive28S, MappingreTxtEntity.Field.Receive29S, MappingreTxtEntity.Field.Receive30S,
                    MappingreTxtEntity.Field.Receive31S, MappingreTxtEntity.Field.Receive32S, MappingreTxtEntity.Field.Receive33S,
                    MappingreTxtEntity.Field.Receive34S, MappingreTxtEntity.Field.Receive35S, MappingreTxtEntity.Field.Receive36S,
                    MappingreTxtEntity.Field.Receive37S, MappingreTxtEntity.Field.Receive38S, MappingreTxtEntity.Field.Receive39S,
                    MappingreTxtEntity.Field.Receive40S
                };
                int?[] startValues = new int?[] {
                    data.Receive1S, data.Receive2S, data.Receive3S,
                    data.Receive4S, data.Receive5S, data.Receive6S,
                    data.Receive7S, data.Receive8S, data.Receive9S,
                    data.Receive10S, data.Receive11S, data.Receive12S,
                    data.Receive13S, data.Receive14S, data.Receive15S,
                    data.Receive16S, data.Receive17S, data.Receive18S,
                    data.Receive19S, data.Receive20S, data.Receive21S,
                    data.Receive22S, data.Receive23S, data.Receive24S,
                    data.Receive25S, data.Receive26S, data.Receive27S,
                    data.Receive28S, data.Receive29S, data.Receive30S,
                    data.Receive31S, data.Receive32S, data.Receive33S,
                    data.Receive34S, data.Receive35S, data.Receive36S,
                    data.Receive37S, data.Receive38S, data.Receive39S,
                    data.Receive40S
                };
                int?[] lengthValues = new int?[] {
                    data.Receive1L, data.Receive2L, data.Receive3L,
                    data.Receive4L, data.Receive5L, data.Receive6L,
                    data.Receive7L, data.Receive8L, data.Receive9L,
                    data.Receive10L, data.Receive11L, data.Receive12L,
                    data.Receive13L, data.Receive14L, data.Receive15L,
                    data.Receive16L, data.Receive17L, data.Receive18L,
                    data.Receive19L, data.Receive20L, data.Receive21L,
                    data.Receive22L, data.Receive23L, data.Receive24L,
                    data.Receive25L, data.Receive26L, data.Receive27L,
                    data.Receive28L, data.Receive29L, data.Receive30L,
                    data.Receive31L, data.Receive32L, data.Receive33L,
                    data.Receive34L, data.Receive35L, data.Receive36L,
                    data.Receive37L, data.Receive38L, data.Receive39L,
                    data.Receive40L
                };
                int count = 0;
                for (int idx = 0; idx < fields.Length; idx++)
                {
                    string key = fields[idx].Substring(0, fields[idx].Length - 2);
                    MappingItem item = MappingItem.CreateByTxt(key, null, key, startValues[idx], lengthValues[idx]);
                    items.Add(item);
                    if (item.IsChecked)
                    {
                        count++;
                    }
                }
                items.Add(MappingItem.CreateByItemCount("Receive", "收入科目金額", 40, (count > 0 ? count.ToString() : String.Empty)));
            }
            #endregion

            return items;
        }

        private MappingreTxtEntity GetMappingreTxtEntity(List<MappingItem> mappingItems)
        {
            MappingreTxtEntity data = new MappingreTxtEntity();
            if (mappingItems != null && mappingItems.Count > 0)
            {
                #region 單一項目的欄位
                {
                    string[] mapFields = new string[] {
                        #region 學生基本資料對照欄位
                        MappingreXlsmdbEntity.Field.StuId,

                        MappingreXlsmdbEntity.Field.StuName, MappingreXlsmdbEntity.Field.IdNumber,
                        MappingreXlsmdbEntity.Field.StuBirthday, MappingreXlsmdbEntity.Field.StuTel,
                        MappingreXlsmdbEntity.Field.StuAddcode, MappingreXlsmdbEntity.Field.StuAdd,
                        MappingreXlsmdbEntity.Field.Email, MappingreXlsmdbEntity.Field.StuParent,
                        #endregion

                        #region [MDY:20160131] 增加資料序號與繳款期限對照欄位
                        MappingreXlsmdbEntity.Field.OldSeq, MappingreXlsmdbEntity.Field.PayDueDate,
                        #endregion

                        #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標對照欄位
                        MappingreXlsmdbEntity.Field.NCCardFlag,
                        #endregion

                        MappingreXlsmdbEntity.Field.StuGrade, MappingreXlsmdbEntity.Field.StuHid,

                        #region [MDY:20220808] 2022擴充案 增加 學籍資料英文名稱對照欄位
                        MappingreXlsmdbEntity.Field.ClassId, MappingreXlsmdbEntity.Field.ClassName,
                        MappingreXlsmdbEntity.Field.ClassEName,
                        MappingreXlsmdbEntity.Field.DeptId, MappingreXlsmdbEntity.Field.DeptName,
                        MappingreXlsmdbEntity.Field.DeptEName,
                        MappingreXlsmdbEntity.Field.CollegeId, MappingreXlsmdbEntity.Field.CollegeName,
                        MappingreXlsmdbEntity.Field.CollegeEName,
                        MappingreXlsmdbEntity.Field.MajorId, MappingreXlsmdbEntity.Field.MajorName,
                        MappingreXlsmdbEntity.Field.MajorEName,
                        #endregion

                        #region [MDY:20220808] 2022擴充案 增加 減免、就貸、住宿英文名稱對照欄位
                        MappingreXlsmdbEntity.Field.ReduceId, MappingreXlsmdbEntity.Field.ReduceName,
                        MappingreXlsmdbEntity.Field.ReduceEName,
                        MappingreXlsmdbEntity.Field.LoanId, MappingreXlsmdbEntity.Field.LoanName,
                        MappingreXlsmdbEntity.Field.LoanEName,
                        MappingreXlsmdbEntity.Field.DormId, MappingreXlsmdbEntity.Field.DormName,
                        MappingreXlsmdbEntity.Field.DormEName,
                        #endregion

                        #region [MDY:20220808] 2022擴充案 增加 身分註記英文名稱對照欄位
                        MappingreXlsmdbEntity.Field.IdentifyId1, MappingreXlsmdbEntity.Field.IdentifyName1,
                        MappingreXlsmdbEntity.Field.IdentifyEName1,
                        MappingreXlsmdbEntity.Field.IdentifyId2, MappingreXlsmdbEntity.Field.IdentifyName2,
                        MappingreXlsmdbEntity.Field.IdentifyEName2,
                        MappingreXlsmdbEntity.Field.IdentifyId3, MappingreXlsmdbEntity.Field.IdentifyName3,
                        MappingreXlsmdbEntity.Field.IdentifyEName3,
                        MappingreXlsmdbEntity.Field.IdentifyId4, MappingreXlsmdbEntity.Field.IdentifyName4,
                        MappingreXlsmdbEntity.Field.IdentifyEName4,
                        MappingreXlsmdbEntity.Field.IdentifyId5, MappingreXlsmdbEntity.Field.IdentifyName5,
                        MappingreXlsmdbEntity.Field.IdentifyEName5,
                        MappingreXlsmdbEntity.Field.IdentifyId6, MappingreXlsmdbEntity.Field.IdentifyName6,
                        MappingreXlsmdbEntity.Field.IdentifyEName6,
                        #endregion

                        MappingreXlsmdbEntity.Field.StuHour, MappingreXlsmdbEntity.Field.StuCredit,
                        MappingreXlsmdbEntity.Field.LoanAmount,

                        MappingreXlsmdbEntity.Field.ReceiveAmount,
                        MappingreXlsmdbEntity.Field.SeriorNo, MappingreXlsmdbEntity.Field.CancelNo,

                        #region 轉帳資料對照欄位
                        MappingreXlsmdbEntity.Field.DeductBankid, MappingreXlsmdbEntity.Field.DeductAccountno,
                        MappingreXlsmdbEntity.Field.DeductAccountname, MappingreXlsmdbEntity.Field.DeductAccountid,
                        #endregion

                        MappingreXlsmdbEntity.Field.Memo01, MappingreXlsmdbEntity.Field.Memo02,
                        MappingreXlsmdbEntity.Field.Memo03, MappingreXlsmdbEntity.Field.Memo04,
                        MappingreXlsmdbEntity.Field.Memo05, MappingreXlsmdbEntity.Field.Memo06,
                        MappingreXlsmdbEntity.Field.Memo07, MappingreXlsmdbEntity.Field.Memo08,
                        MappingreXlsmdbEntity.Field.Memo09, MappingreXlsmdbEntity.Field.Memo10,
                        MappingreXlsmdbEntity.Field.Memo11, MappingreXlsmdbEntity.Field.Memo12,
                        MappingreXlsmdbEntity.Field.Memo13, MappingreXlsmdbEntity.Field.Memo14,
                        MappingreXlsmdbEntity.Field.Memo15, MappingreXlsmdbEntity.Field.Memo16,
                        MappingreXlsmdbEntity.Field.Memo17, MappingreXlsmdbEntity.Field.Memo18,
                        MappingreXlsmdbEntity.Field.Memo19, MappingreXlsmdbEntity.Field.Memo20,
                        MappingreXlsmdbEntity.Field.Memo21
                    };
                    string[] fieldNames = new string[] {
                        #region 學生基本資料對照欄位
                        "StuId",

                        "StuName", "IdNumber",
                        "StuBirthday", "StuTel",
                        "StuAddcode", "StuAdd",
                        "Email", "StuParent",
                        #endregion

                        #region [MDY:20160131] 增加資料序號與繳款期限對照欄位
                        "OldSeq", "PayDueDate",
                        #endregion

                        #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標對照欄位
                        "NCCardFlag",
                        #endregion

                        "StuGrade", "StuHid",

                        #region [MDY:20220808] 2022擴充案 增加 學籍資料英文名稱對照欄位
                        "ClassId", "ClassName",
                        "ClassEName",
                        "DeptId", "DeptName",
                        "DeptEName",
                        "CollegeId", "CollegeName",
                        "CollegeEName",
                        "MajorId", "MajorName",
                        "MajorEName",
                        #endregion

                        #region [MDY:20220808] 2022擴充案 增加 減免、就貸、住宿英文名稱對照欄位
                        "ReduceId", "ReduceName",
                        "ReduceEName",
                        "LoanId", "LoanName",
                        "LoanEName",
                        "DormId", "DormName",
                        "DormEName",
                        #endregion

                        #region [MDY:20220808] 2022擴充案 增加 身分註記英文名稱對照欄位
                        "IdentifyId1", "IdentifyName1",
                        "IdentifyEName1",
                        "IdentifyId2", "IdentifyName2",
                        "IdentifyEName2",
                        "IdentifyId3", "IdentifyName3",
                        "IdentifyEName3",
                        "IdentifyId4", "IdentifyName4",
                        "IdentifyEName4",
                        "IdentifyId5", "IdentifyName5",
                        "IdentifyEName5",
                        "IdentifyId6", "IdentifyName6",
                        "IdentifyEName6",
                        #endregion

                        "StuHour", "StuCredit",
                        "LoanAmount",

                        "ReceiveAmount",
                        "SeriorNo", "CancelNo",

                        #region 轉帳資料對照欄位
                        "DeductBankid", "DeductAccountno",
                        "DeductAccountname", "DeductAccountid",
                        #endregion

                        "Memo01", "Memo02",
                        "Memo03", "Memo04",
                        "Memo05", "Memo06",
                        "Memo07", "Memo08",
                        "Memo09", "Memo10",
                        "Memo11", "Memo12",
                        "Memo13", "Memo14",
                        "Memo15", "Memo16",
                        "Memo17", "Memo18",
                        "Memo19", "Memo20",
                        "Memo21"
                    };

                    for (int idx = 0; idx < mapFields.Length; idx++)
                    {
                        string mapField = mapFields[idx];
                        string fieldName = fieldNames[idx];
                        string startFieldName = String.Concat(fieldName, "S");
                        string lengthFieldName = String.Concat(fieldName, "L");
                        MappingItem mapItem = mappingItems.Find(x => x.Key == mapField);
                        int? startValue = null;
                        int? lengthValue = null;
                        if (mapItem != null && mapItem.IsChecked)
                        {
                            if (mapItem.FileType != "txt")
                            {
                                return null;
                            }
                            startValue = mapItem.GetTxtStart();
                            lengthValue = mapItem.GetTxtLength();
                        }
                        if (!data.SetValue(startFieldName, startValue).IsSuccess)
                        {
                            return null;
                        }
                        if (!data.SetValue(lengthFieldName, lengthValue).IsSuccess)
                        {
                            return null;
                        }
                    }
                }
                #endregion

                #region 學分基準欄位
                {
                    string[] mapFields = new string[] {
                        MappingreXlsmdbEntity.Field.CreditId1, MappingreXlsmdbEntity.Field.CreditId2, MappingreXlsmdbEntity.Field.CreditId3,
                        MappingreXlsmdbEntity.Field.CreditId4, MappingreXlsmdbEntity.Field.CreditId5, MappingreXlsmdbEntity.Field.CreditId6,
                        MappingreXlsmdbEntity.Field.CreditId7, MappingreXlsmdbEntity.Field.CreditId8, MappingreXlsmdbEntity.Field.CreditId9,
                        MappingreXlsmdbEntity.Field.CreditId10, MappingreXlsmdbEntity.Field.CreditId11, MappingreXlsmdbEntity.Field.CreditId12,
                        MappingreXlsmdbEntity.Field.CreditId13, MappingreXlsmdbEntity.Field.CreditId14, MappingreXlsmdbEntity.Field.CreditId15,
                        MappingreXlsmdbEntity.Field.CreditId16, MappingreXlsmdbEntity.Field.CreditId17, MappingreXlsmdbEntity.Field.CreditId18,
                        MappingreXlsmdbEntity.Field.CreditId19, MappingreXlsmdbEntity.Field.CreditId20, MappingreXlsmdbEntity.Field.CreditId21,
                        MappingreXlsmdbEntity.Field.CreditId22, MappingreXlsmdbEntity.Field.CreditId23, MappingreXlsmdbEntity.Field.CreditId24,
                        MappingreXlsmdbEntity.Field.CreditId25, MappingreXlsmdbEntity.Field.CreditId26, MappingreXlsmdbEntity.Field.CreditId27,
                        MappingreXlsmdbEntity.Field.CreditId28, MappingreXlsmdbEntity.Field.CreditId29, MappingreXlsmdbEntity.Field.CreditId30,
                        MappingreXlsmdbEntity.Field.CreditId31, MappingreXlsmdbEntity.Field.CreditId32, MappingreXlsmdbEntity.Field.CreditId33,
                        MappingreXlsmdbEntity.Field.CreditId34, MappingreXlsmdbEntity.Field.CreditId35, MappingreXlsmdbEntity.Field.CreditId36,
                        MappingreXlsmdbEntity.Field.CreditId37, MappingreXlsmdbEntity.Field.CreditId38, MappingreXlsmdbEntity.Field.CreditId39,
                        MappingreXlsmdbEntity.Field.CreditId40
                    };
                    for (int idx = 0; idx < mapFields.Length; idx++)
                    {
                        int no = idx + 1;
                        string mapField = mapFields[idx];
                        string startFieldName = String.Format("CreditId{0}S", no);
                        string lengthFieldName = String.Format("CreditId{0}L", no);
                        MappingItem mapItem = mappingItems.Find(x => x.Key == mapField);
                        int? startValue = null;
                        int? lengthValue = null;
                        if (mapItem != null && mapItem.IsChecked)
                        {
                            if (mapItem.FileType != "txt")
                            {
                                return null;
                            }
                            startValue = mapItem.GetTxtStart();
                            lengthValue = mapItem.GetTxtLength();
                        }
                        if (!data.SetValue(startFieldName, startValue).IsSuccess)
                        {
                            return null;
                        }
                        if (!data.SetValue(lengthFieldName, lengthValue).IsSuccess)
                        {
                            return null;
                        }
                    }
                }
                #endregion

                #region 課程代碼欄位
                {
                    string[] mapFields = new string[] {
                        MappingreXlsmdbEntity.Field.CourseId1, MappingreXlsmdbEntity.Field.CourseId2, MappingreXlsmdbEntity.Field.CourseId3,
                        MappingreXlsmdbEntity.Field.CourseId4, MappingreXlsmdbEntity.Field.CourseId5, MappingreXlsmdbEntity.Field.CourseId6,
                        MappingreXlsmdbEntity.Field.CourseId7, MappingreXlsmdbEntity.Field.CourseId8, MappingreXlsmdbEntity.Field.CourseId9,
                        MappingreXlsmdbEntity.Field.CourseId10, MappingreXlsmdbEntity.Field.CourseId11, MappingreXlsmdbEntity.Field.CourseId12,
                        MappingreXlsmdbEntity.Field.CourseId13, MappingreXlsmdbEntity.Field.CourseId14, MappingreXlsmdbEntity.Field.CourseId15,
                        MappingreXlsmdbEntity.Field.CourseId16, MappingreXlsmdbEntity.Field.CourseId17, MappingreXlsmdbEntity.Field.CourseId18,
                        MappingreXlsmdbEntity.Field.CourseId19, MappingreXlsmdbEntity.Field.CourseId20, MappingreXlsmdbEntity.Field.CourseId21,
                        MappingreXlsmdbEntity.Field.CourseId22, MappingreXlsmdbEntity.Field.CourseId23, MappingreXlsmdbEntity.Field.CourseId24,
                        MappingreXlsmdbEntity.Field.CourseId25, MappingreXlsmdbEntity.Field.CourseId26, MappingreXlsmdbEntity.Field.CourseId27,
                        MappingreXlsmdbEntity.Field.CourseId28, MappingreXlsmdbEntity.Field.CourseId29, MappingreXlsmdbEntity.Field.CourseId30,
                        MappingreXlsmdbEntity.Field.CourseId31, MappingreXlsmdbEntity.Field.CourseId32, MappingreXlsmdbEntity.Field.CourseId33,
                        MappingreXlsmdbEntity.Field.CourseId34, MappingreXlsmdbEntity.Field.CourseId35, MappingreXlsmdbEntity.Field.CourseId36,
                        MappingreXlsmdbEntity.Field.CourseId37, MappingreXlsmdbEntity.Field.CourseId38, MappingreXlsmdbEntity.Field.CourseId39,
                        MappingreXlsmdbEntity.Field.CourseId40
                    };

                    for (int idx = 0; idx < mapFields.Length; idx++)
                    {
                        int no = idx + 1;
                        string mapField = mapFields[idx];
                        string startFieldName = String.Format("CourseId{0}S", no);
                        string lengthFieldName = String.Format("CourseId{0}L", no);
                        MappingItem mapItem = mappingItems.Find(x => x.Key == mapField);
                        int? startValue = null;
                        int? lengthValue = null;
                        if (mapItem != null && mapItem.IsChecked)
                        {
                            if (mapItem.FileType != "txt")
                            {
                                return null;
                            }
                            startValue = mapItem.GetTxtStart();
                            lengthValue = mapItem.GetTxtLength();
                        }
                        if (!data.SetValue(startFieldName, startValue).IsSuccess)
                        {
                            return null;
                        }
                        if (!data.SetValue(lengthFieldName, lengthValue).IsSuccess)
                        {
                            return null;
                        }
                    }
                }
                #endregion

                #region 學分基準或課程學分數欄位
                {
                    string[] mapFields = new string[] {
                        MappingreXlsmdbEntity.Field.Credit1, MappingreXlsmdbEntity.Field.Credit2, MappingreXlsmdbEntity.Field.Credit3,
                        MappingreXlsmdbEntity.Field.Credit4, MappingreXlsmdbEntity.Field.Credit5, MappingreXlsmdbEntity.Field.Credit6,
                        MappingreXlsmdbEntity.Field.Credit7, MappingreXlsmdbEntity.Field.Credit8, MappingreXlsmdbEntity.Field.Credit9,
                        MappingreXlsmdbEntity.Field.Credit10, MappingreXlsmdbEntity.Field.Credit11, MappingreXlsmdbEntity.Field.Credit12,
                        MappingreXlsmdbEntity.Field.Credit13, MappingreXlsmdbEntity.Field.Credit14, MappingreXlsmdbEntity.Field.Credit15,
                        MappingreXlsmdbEntity.Field.Credit16, MappingreXlsmdbEntity.Field.Credit17, MappingreXlsmdbEntity.Field.Credit18,
                        MappingreXlsmdbEntity.Field.Credit19, MappingreXlsmdbEntity.Field.Credit20, MappingreXlsmdbEntity.Field.Credit21,
                        MappingreXlsmdbEntity.Field.Credit22, MappingreXlsmdbEntity.Field.Credit23, MappingreXlsmdbEntity.Field.Credit24,
                        MappingreXlsmdbEntity.Field.Credit25, MappingreXlsmdbEntity.Field.Credit26, MappingreXlsmdbEntity.Field.Credit27,
                        MappingreXlsmdbEntity.Field.Credit28, MappingreXlsmdbEntity.Field.Credit29, MappingreXlsmdbEntity.Field.Credit30,
                        MappingreXlsmdbEntity.Field.Credit31, MappingreXlsmdbEntity.Field.Credit32, MappingreXlsmdbEntity.Field.Credit33,
                        MappingreXlsmdbEntity.Field.Credit34, MappingreXlsmdbEntity.Field.Credit35, MappingreXlsmdbEntity.Field.Credit36,
                        MappingreXlsmdbEntity.Field.Credit37, MappingreXlsmdbEntity.Field.Credit38, MappingreXlsmdbEntity.Field.Credit39,
                        MappingreXlsmdbEntity.Field.Credit40
                    };

                    for (int idx = 0; idx < mapFields.Length; idx++)
                    {
                        int no = idx + 1;
                        string mapField = mapFields[idx];
                        string startFieldName = String.Format("Credit{0}S", no);
                        string lengthFieldName = String.Format("Credit{0}L", no);
                        MappingItem mapItem = mappingItems.Find(x => x.Key == mapField);
                        int? startValue = null;
                        int? lengthValue = null;
                        if (mapItem != null && mapItem.IsChecked)
                        {
                            if (mapItem.FileType != "txt")
                            {
                                return null;
                            }
                            startValue = mapItem.GetTxtStart();
                            lengthValue = mapItem.GetTxtLength();
                        }
                        if (!data.SetValue(startFieldName, startValue).IsSuccess)
                        {
                            return null;
                        }
                        if (!data.SetValue(lengthFieldName, lengthValue).IsSuccess)
                        {
                            return null;
                        }
                    }
                }
                #endregion

                #region 收入科目金額欄位
                {
                    string[] mapFields = new string[] {
                        MappingreXlsmdbEntity.Field.Receive1, MappingreXlsmdbEntity.Field.Receive2, MappingreXlsmdbEntity.Field.Receive3,
                        MappingreXlsmdbEntity.Field.Receive4, MappingreXlsmdbEntity.Field.Receive5, MappingreXlsmdbEntity.Field.Receive6,
                        MappingreXlsmdbEntity.Field.Receive7, MappingreXlsmdbEntity.Field.Receive8, MappingreXlsmdbEntity.Field.Receive9,
                        MappingreXlsmdbEntity.Field.Receive10, MappingreXlsmdbEntity.Field.Receive11, MappingreXlsmdbEntity.Field.Receive12,
                        MappingreXlsmdbEntity.Field.Receive13, MappingreXlsmdbEntity.Field.Receive14, MappingreXlsmdbEntity.Field.Receive15,
                        MappingreXlsmdbEntity.Field.Receive16, MappingreXlsmdbEntity.Field.Receive17, MappingreXlsmdbEntity.Field.Receive18,
                        MappingreXlsmdbEntity.Field.Receive19, MappingreXlsmdbEntity.Field.Receive20, MappingreXlsmdbEntity.Field.Receive21,
                        MappingreXlsmdbEntity.Field.Receive22, MappingreXlsmdbEntity.Field.Receive23, MappingreXlsmdbEntity.Field.Receive24,
                        MappingreXlsmdbEntity.Field.Receive25, MappingreXlsmdbEntity.Field.Receive26, MappingreXlsmdbEntity.Field.Receive27,
                        MappingreXlsmdbEntity.Field.Receive28, MappingreXlsmdbEntity.Field.Receive29, MappingreXlsmdbEntity.Field.Receive30,
                        MappingreXlsmdbEntity.Field.Receive31, MappingreXlsmdbEntity.Field.Receive32, MappingreXlsmdbEntity.Field.Receive33,
                        MappingreXlsmdbEntity.Field.Receive34, MappingreXlsmdbEntity.Field.Receive35, MappingreXlsmdbEntity.Field.Receive36,
                        MappingreXlsmdbEntity.Field.Receive37, MappingreXlsmdbEntity.Field.Receive38, MappingreXlsmdbEntity.Field.Receive39,
                        MappingreXlsmdbEntity.Field.Receive40
                    };

                    for (int idx = 0; idx < mapFields.Length; idx++)
                    {
                        int no = idx + 1;
                        string mapField = mapFields[idx];
                        string startFieldName = String.Format("Receive{0}S", no);
                        string lengthFieldName = String.Format("Receive{0}L", no);
                        MappingItem mapItem = mappingItems.Find(x => x.Key == mapField);
                        int? startValue = null;
                        int? lengthValue = null;
                        if (mapItem != null && mapItem.IsChecked)
                        {
                            if (mapItem.FileType != "txt")
                            {
                                return null;
                            }
                            startValue = mapItem.GetTxtStart();
                            lengthValue = mapItem.GetTxtLength();
                        }
                        if (!data.SetValue(startFieldName, startValue).IsSuccess)
                        {
                            return null;
                        }
                        if (!data.SetValue(lengthFieldName, lengthValue).IsSuccess)
                        {
                            return null;
                        }
                    }
                }
                #endregion
            }
            return data;
        }

        /// <summary>
        /// Step1 的 CheckBox 控制項集合快取
        /// </summary>
        private List<CheckBox> _CacheStep1CheckBoxs = null;
        /// <summary>
        /// 取得 Step1 的所有 CheckBox 控制項集合
        /// </summary>
        /// <returns>傳回 Step1 的所有 CheckBox 控制項集合</returns>
        private List<CheckBox> GetStep1AllCheckBoxs()
        {
            if (_CacheStep1CheckBoxs == null)
            {
                _CacheStep1CheckBoxs = new List<CheckBox>();
                foreach (Control control in this.divStep1.Controls)
                {
                    if (control is CheckBox)
                    {
                        _CacheStep1CheckBoxs.Add((CheckBox)control);
                    }
                }
            }
            return _CacheStep1CheckBoxs;
        }

        /// <summary>
        /// 結繫 Step1 資料
        /// </summary>
        /// <param name="fileType">指定上傳檔案格式</param>
        /// <param name="mappingName">指定對照表名稱</param>
        /// <param name="items">指定 MappingItem 資料集合</param>
        private void BindStep1Data(string fileType, string mappingName, List<MappingItem> items)
        {
            this.EditMappingItems = items;

            WebHelper.SetDropDownListSelectedValue(this.ddlFileType, fileType);

            #region [MDY:20210401] 原碼修正
            this.tbxMappingName.Text = mappingName == null ? String.Empty : HttpUtility.HtmlEncode(mappingName.Trim());
            #endregion

            this.tbxCreditIdCount.Text = String.Empty;
            this.tbxCourseIdCount.Text = String.Empty;
            this.tbxReceiveCount.Text = String.Empty;

            if (items == null || items.Count == 0)
            {
                #region 除了學號，其他 CheckBox 清除 Checked
                List<CheckBox> checkboxs = this.GetStep1AllCheckBoxs();
                foreach (CheckBox checkbox in checkboxs)
                {
                    if (checkbox != chkStu_Id)
                    {
                        checkbox.Checked = false;
                    }
                }
                #endregion
            }
            else
            {
                bool isDataEditable = ActionMode.IsDataEditableMode(this.Action);
                bool isPKeyEditable = ActionMode.IsPKeyEditableMode(this.Action);

                #region PKey 欄位
                this.ddlFileType.Enabled = isPKeyEditable;
                #endregion

                #region 勾選項目
                List<CheckBox> checkboxs = this.GetStep1AllCheckBoxs();
                foreach (CheckBox checkbox in checkboxs)
                {
                    string key = checkbox.ID.Substring(3);
                    MappingItem item = items.Find(x => key.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                    if (checkbox != chkStu_Id)
                    {
                        checkbox.Checked = (item != null && item.IsChecked);
                        checkbox.Enabled = isDataEditable;
                    }
                }
                #endregion

                #region 學分基準項數
                {
                    MappingItem item = items.Find(x => "Credit_Id".Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                    if (item != null && item.IsChecked && item.MaxItemCount > 0)
                    {
                        this.tbxCreditIdCount.Text = item.ItemCount;
                    }
                    this.tbxCreditIdCount.Enabled = isDataEditable;
                }
                #endregion

                #region 課程代碼項數
                {
                    MappingItem item = items.Find(x => "Course_Id".Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                    if (item != null && item.IsChecked && item.MaxItemCount > 0)
                    {
                        this.tbxCourseIdCount.Text = item.ItemCount;
                    }
                    this.tbxCourseIdCount.Enabled = isDataEditable;
                }
                #endregion

                #region 收入科目金額項數
                {
                    MappingItem item = items.Find(x => "Receive".Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                    if (item != null && item.IsChecked && item.MaxItemCount > 0)
                    {
                        #region [MDY:20210401] 原碼修正
                        this.tbxReceiveCount.Text = HttpUtility.HtmlEncode(item.ItemCount);
                        #endregion
                    }
                    this.tbxReceiveCount.Enabled = isDataEditable;
                }
                #endregion
            }
        }

        /// <summary>
        /// 取得指定項目的項數輸入格的資料
        /// </summary>
        /// <param name="itemName">指定項目的名稱</param>
        /// <param name="tbx">指定項目的項數輸入格控制項</param>
        /// <param name="maxCount">指定項目的最大項數</param>
        /// <param name="count">傳回項目的輸入項數</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool GetStep1ItemCount(string itemName, TextBox tbx, int maxCount, out int count)
        {
            count = 0;
            if (tbx == null)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized(String.Concat("無法取得「", itemName, "」項數資料"));
                this.ShowSystemMessage(msg);
                return false;
            }
            string txt = tbx.Text.Trim();
            if (String.IsNullOrEmpty(txt))
            {
                this.ShowMustInputAlert(itemName + "的項數");
                return false;
            }
            if (!Int32.TryParse(txt, out count) || count < 1 || count > maxCount)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized(String.Format("「{0}」的項數限輸入 1 ~ {1} 的整數", itemName, maxCount));
                this.ShowSystemMessage(msg);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 檢查 Step1 輸入資料
        /// </summary>
        /// <returns>正確傳回 true，否則傳回 false</returns>
        public bool CheckStep1InputData()
        {
            if (this.Action == ActionMode.Insert)
            {
                if (String.IsNullOrEmpty(this.ddlFileType.SelectedValue))
                {
                    this.ShowMustInputAlert("上傳檔案格式");
                    return false;
                }
                if (String.IsNullOrWhiteSpace(this.tbxMappingName.Text))
                {
                    this.ShowMustInputAlert("對照表名稱");
                    return false;
                }
            }

            #region 學分基準項數
            if (this.chkCredit_Id.Checked)
            {
                string itemName = "學分基準";
                int maxCount = 40;
                int count = 0;
                if (!this.GetStep1ItemCount(itemName, this.tbxCreditIdCount, maxCount, out count))
                {
                    return false;
                }
            }
            #endregion

            #region 課程代碼項數
            if (this.chkCourse_Id.Checked)
            {
                string itemName = "課程代碼";
                int maxCount = 40;
                int count = 0;
                if (!this.GetStep1ItemCount(itemName, this.tbxCourseIdCount, maxCount, out count))
                {
                    return false;
                }
            }
            #endregion

            #region 學分基準或課程學分數
            if (this.chkCredit.Checked && !this.chkCredit_Id.Checked && !this.chkCourse_Id.Checked)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("當勾選「學分基準或課程學分數」時，「學分基準」或「課程代碼」至少要勾選一項");
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion

            #region 收入科目金額項數
            if (this.chkReceive.Checked)
            {
                string itemName = "收入科目金額";
                int maxCount = 40;
                int count = 0;
                if (!this.GetStep1ItemCount(itemName, this.tbxReceiveCount, maxCount, out count))
                {
                    return false;
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 結繫 Step2 資料
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="mappingName"></param>
        /// <param name="items"></param>
        private void BindStep2Data(string fileType, string mappingName, List<MappingItem> items)
        {
            this.EditMappingItems = items;

            this.labMappingName.Text = HttpUtility.HtmlEncode(mappingName);
            switch (fileType)
            {
                case "xls":
                    this.labFileType.Text = "試算表(xls/xlsx/ods)";
                    this.BindXlsFieldsHtml(items);
                    break;
                case "txt":
                    this.labFileType.Text = "純文字(txt)";
                    this.BindTxtFieldsHtml(items);
                    break;
            }
        }

        private string GenTxtItemHtml(MappingItem item)
        {
            return String.Format(@"<tr>
<td>{0}</td>
<td><input type=""text"" name=""tbx{1}_S"" id=""tbx{1}_S"" value=""{2}"" maxlength=""4"" /></td>
<td><input type=""text"" name=""tbx{1}_L"" id=""tbx{1}_L"" value=""{3}"" maxlength=""2"" /></td>
</tr>", HttpUtility.HtmlEncode(item.Name), HttpUtility.HtmlEncode(item.Key), HttpUtility.HtmlEncode(item.TxtStart), HttpUtility.HtmlEncode(item.TxtLength));
        }

        #region [MDY:20220808] 2022擴充案
        private string GenTxtItemHtml(MappingItem chtItem, MappingItem engItem)
        {
            string chtKey = HttpUtility.HtmlEncode(chtItem.Key);
            string engKey = HttpUtility.HtmlEncode(engItem.Key);

            return $@"<tr>
<td>{HttpUtility.HtmlEncode(chtItem.Name)} 中文</td>
<td><input type=""text"" name=""tbx{chtKey}_S"" id=""tbx{chtKey}_S"" value=""{HttpUtility.HtmlEncode(chtItem.TxtStart)}"" maxlength=""4"" /></td>
<td><input type=""text"" name=""tbx{chtKey}_L"" id=""tbx{chtKey}_L"" value=""{HttpUtility.HtmlEncode(chtItem.TxtLength)}"" maxlength=""2"" /></td>
</tr>
<tr>
<td>{HttpUtility.HtmlEncode(chtItem.Name)} 英文</td>
<td><input type=""text"" name=""tbx{engKey}_S"" id=""tbx{engKey}_S"" value=""{HttpUtility.HtmlEncode(engItem.TxtStart)}"" maxlength=""4"" /></td>
<td><input type=""text"" name=""tbx{engKey}_L"" id=""tbx{engKey}_L"" value=""{HttpUtility.HtmlEncode(engItem.TxtLength)}"" maxlength=""2"" /></td>
</tr>";
        }

        /// <summary>
        /// 檢查對照檔項目設定
        /// </summary>
        /// <param name="items">所有對照檔項目設定集合</param>
        /// <param name="itemKey">要檢查的對照檔項目 key</param>
        /// <param name="engItem">傳回對應的英文名稱對照項目設定</param>
        /// <returns>指定項目為主要設定項目則傳回 true，否則傳回 false</returns>
        private bool CheckTxtMappingItem(List<MappingItem> items, string itemKey, out MappingItem engItem)
        {
            engItem = null;
            switch (itemKey)
            {
                #region 有英文名稱對照項目的項目
                case "Dept_Name":
                case "College_Name":
                case "Major_Name":
                case "Class_Name":
                case "Reduce_Name":
                case "Dorm_Name":
                case "Loan_Name":
                case "Identify_Name1":
                case "Identify_Name2":
                case "Identify_Name3":
                case "Identify_Name4":
                case "Identify_Name5":
                case "Identify_Name6":
                    {
                        string engItemKey = itemKey.Replace("_Name", "_EName");
                        if (!String.IsNullOrEmpty(engItemKey))
                        {
                            engItem = items.Find(x => x.Key == engItemKey);
                        }
                        return true;
                    }
                #endregion

                #region 英文名稱對照項目
                case "Dept_EName":
                case "College_EName":
                case "Major_EName":
                case "Class_EName":
                case "Reduce_EName":
                case "Dorm_EName":
                case "Loan_EName":
                case "Identify_EName1":
                case "Identify_EName2":
                case "Identify_EName3":
                case "Identify_EName4":
                case "Identify_EName5":
                case "Identify_EName6":
                    {
                        return false;
                    }
                #endregion

                default:
                    return true;
            }
        }

        private void BindTxtFieldsHtml(List<MappingItem> items)
        {
            bool isEngEnabled = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);

            StringBuilder sb = new StringBuilder();
            sb
                .AppendLine("<table width=\"100%\" class=\"#\">")
                .AppendLine("<tr>")
                .AppendLine("<th><div style=\"text-align:center\">繳費資料欄位</div></th>")
                .AppendLine("<th><div style=\"text-align:center\">資料起始位置</div></th>")
                .AppendLine("<th><div style=\"text-align:center\">資料長度</div></th>")
                .AppendLine("</tr>");

            List<CheckBox> checkboxs = this.GetStep1AllCheckBoxs();
            foreach (CheckBox checkbox in checkboxs)
            {
                string key = checkbox.ID.Substring(3);
                MappingItem item = items.Find(x => key.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                if (item != null && item.IsChecked)
                {
                    switch (item.Key)
                    {
                        case "Credit_Id":
                        case "Course_Id":
                        case "Credit":
                            #region 學分基準 | 課程代碼 | 學分基準或課程學分數
                            {
                                for (int no = 1; no <= item.MaxItemCount; no++)
                                {
                                    string subKey = String.Format("{0}{1}", item.Key, no);
                                    MappingItem subItem = items.Find(x => subKey.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                                    if (subItem != null && subItem.IsChecked)
                                    {
                                        sb.AppendLine(this.GenTxtItemHtml(subItem));
                                    }
                                }
                            }
                            #endregion
                            break;
                        case "Receive":
                            #region 收入科目金額
                            {
                                for (int no = 1; no <= item.MaxItemCount; no++)
                                {
                                    string subKey = String.Format("{0}_{1}", item.Key, no);
                                    MappingItem subItem = items.Find(x => subKey.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                                    if (subItem != null && subItem.IsChecked)
                                    {
                                        sb.AppendLine(this.GenTxtItemHtml(subItem));
                                    }
                                }
                            }
                            #endregion
                            break;
                        default:
                            #region
                            {
                                MappingItem engItem = null;
                                if (this.CheckTxtMappingItem(items, item.Key, out engItem))
                                {
                                    if (isEngEnabled && engItem != null)
                                    {
                                        engItem.Name = $"{item.Name}英文";
                                        engItem.IsChecked = item.IsChecked;
                                        sb.AppendLine(this.GenTxtItemHtml(item, engItem));
                                    }
                                    else
                                    {
                                        sb.AppendLine(this.GenTxtItemHtml(item));
                                    }
                                }
                            }
                            #endregion
                            break;
                    }
                }
            }

            sb.AppendLine("</table>");
            this.litFieldsHtml.Text = sb.ToString();
        }
        #endregion

        #region [MDY:20220808] 2022擴充案
        private string GenXlsItemHtml(MappingItem item, bool isEngEnabled)
        {
            string name = HttpUtility.HtmlEncode(item.Name);
            string key = HttpUtility.HtmlEncode(item.Key);
            string value = HttpUtility.HtmlEncode(item.XlsName);
            string size = HttpUtility.HtmlEncode(item.XlsNameSize);
            if (isEngEnabled && !String.IsNullOrEmpty(item.EngFieldName))
            {
                string engKey = HttpUtility.HtmlEncode(item.EngFieldName);
                string engValue = HttpUtility.HtmlEncode(item.EngFieldValue);
                string engSize = HttpUtility.HtmlEncode(item.EngFieldSize);
                return $@"<tr>
<td>{name}</td>
<td>
    <input type=""text"" name=""tbx{key}"" id=""tbx{key}"" value=""{value}"" maxlength=""{size}"" />
    <span style=""padding: 0 0 0 5px;"">英文名稱：<input type=""text"" name=""tbx{engKey}"" id=""tbx{engKey}"" value=""{engValue}"" maxlength=""{engSize}"" /></span>
</td>
</tr>";
            }
            else
            {
                return $@"<tr>
<td>{name}</td>
<td><input type=""text"" name=""tbx{key}"" id=""tbx{key}"" value=""{value}"" maxlength=""{size}"" /></td>
</tr>";
            }
        }

        private string GenXlsItemHtml(MappingItem chtItem, MappingItem engItem)
        {
            string chtKey = HttpUtility.HtmlEncode(chtItem.Key);
            string engKey = HttpUtility.HtmlEncode(engItem.Key);
            string chtValue = HttpUtility.HtmlEncode(chtItem.XlsName);
            string engValue = HttpUtility.HtmlEncode(engItem.XlsName);
            string chtSize = HttpUtility.HtmlEncode(chtItem.XlsNameSize);
            string engSize = HttpUtility.HtmlEncode(engItem.XlsNameSize);
            return $@"<tr>
<td>{HttpUtility.HtmlEncode(chtItem.Name)}</td>
<td>
    <input type=""text"" name=""tbx{chtKey}"" id=""tbx{chtKey}"" value=""{chtValue}"" maxlength=""{chtSize}"" />
    <span style=""padding: 0 0 0 5px;"">英文欄位：<input type=""text"" name=""tbx{engKey}"" id=""tbx{engKey}"" value=""{engValue}"" maxlength=""{engSize}"" /></span>
</td>
</tr>";
        }

        /// <summary>
        /// 檢查對照檔項目設定
        /// </summary>
        /// <param name="items">所有對照檔項目設定集合</param>
        /// <param name="itemKey">要檢查的對照檔項目 key</param>
        /// <param name="engItem">傳回對應的英文名稱對照項目設定</param>
        /// <returns>指定項目為主要設定項目則傳回 true，否則傳回 false</returns>
        private bool CheckXlsMappingItem(List<MappingItem> items, string itemKey, out MappingItem engItem)
        {
            engItem = null;
            switch (itemKey)
            {
                #region 有英文名稱對照項目的項目
                case "Dept_Name":
                case "College_Name":
                case "Major_Name":
                case "Class_Name":
                case "Reduce_Name":
                case "Dorm_Name":
                case "Loan_Name":
                case "Identify_Name1":
                case "Identify_Name2":
                case "Identify_Name3":
                case "Identify_Name4":
                case "Identify_Name5":
                case "Identify_Name6":
                    {
                        string engItemKey = itemKey.Replace("_", "_E");
                        if (!String.IsNullOrEmpty(engItemKey))
                        {
                            engItem = items.Find(x => x.Key == engItemKey);
                        }
                        return true;
                    }
                #endregion

                #region 英文名稱對照項目
                case "Dept_EName":
                case "College_EName":
                case "Major_EName":
                case "Class_EName":
                case "Reduce_EName":
                case "Dorm_EName":
                case "Loan_EName":
                case "Identify_EName1":
                case "Identify_EName2":
                case "Identify_EName3":
                case "Identify_EName4":
                case "Identify_EName5":
                case "Identify_EName6":
                    {
                        return false;
                    }
                #endregion

                default:
                    return true;
            }
        }

        private bool IsEngXlsMaapingItem(string itemKey)
        {
            switch (itemKey)
            {
                case "Dept_EName":
                case "College_EName":
                case "Major_EName":
                case "Class_EName":
                case "Reduce_EName":
                case "Dorm_EName":
                case "Loan_EName":
                case "Identify_EName1":
                case "Identify_EName2":
                case "Identify_EName3":
                case "Identify_EName4":
                case "Identify_EName5":
                case "Identify_EName6":
                        return true;

                default:
                    return false;
            }
        }

        private void BindXlsFieldsHtml(List<MappingItem> items)
        {
            bool isEngEnabled = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);

            StringBuilder sb = new StringBuilder();
            sb
                .AppendLine("<table width=\"100%\" class=\"#\">")
                .AppendLine("<tr>")
                .AppendLine($"<th style=\"width:{(isEngEnabled ? 25 : 50)}%\"><div style=\"text-align:center\">繳費資料欄位</div></th>")
                .AppendLine($"<th style=\"width:{(isEngEnabled ? 75 : 50)}%\"><div style=\"text-align:center\">試算表欄位名稱</div></th>")
                .AppendLine("</tr>");

            List<CheckBox> checkboxs = this.GetStep1AllCheckBoxs();
            foreach (CheckBox checkbox in checkboxs)
            {
                string key = checkbox.ID.Substring(3);
                MappingItem item = items.Find(x => key.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                if (item != null && item.IsChecked)
                {
                    switch (item.Key)
                    {
                        case "Credit_Id":
                        case "Course_Id":
                        case "Credit":
                            #region 學分基準 | 課程代碼 | 學分基準或課程學分數
                            {
                                for (int no = 1; no <= item.MaxItemCount; no++)
                                {
                                    string subKey = String.Format("{0}{1}", item.Key, no);
                                    MappingItem subItem = items.Find(x => subKey.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                                    if (subItem != null && subItem.IsChecked)
                                    {
                                        sb.AppendLine(this.GenXlsItemHtml(subItem, isEngEnabled));
                                    }
                                }
                            }
                            #endregion
                            break;
                        case "Receive":
                            #region 收入科目金額
                            {
                                for (int no = 1; no <= item.MaxItemCount; no++)
                                {
                                    string subKey = String.Format("{0}_{1}", item.Key, no);
                                    MappingItem subItem = items.Find(x => subKey.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                                    if (subItem != null && subItem.IsChecked)
                                    {
                                        sb.AppendLine(this.GenXlsItemHtml(subItem, isEngEnabled));
                                    }
                                }
                            }
                            #endregion
                            break;
                        default:
                            #region
                            {
                                MappingItem engItem = null;
                                if (this.CheckXlsMappingItem(items, item.Key, out engItem))
                                {
                                    if (isEngEnabled && engItem != null)
                                    {
                                        engItem.Name = $"{item.Name}英文";
                                        engItem.IsChecked = item.IsChecked;
                                        sb.AppendLine(this.GenXlsItemHtml(item, engItem));
                                    }
                                    else
                                    {
                                        sb.AppendLine(this.GenXlsItemHtml(item, isEngEnabled));
                                    }
                                }
                            }
                            #endregion
                            break;
                    }
                }
            }

            sb.AppendLine("</table>");
            this.litFieldsHtml.Text = sb.ToString();
        }
        #endregion

        /// <summary>
        /// 取得 Step2 的輸入資料
        /// </summary>
        /// <returns></returns>
        private List<MappingItem> GetStep2Data()
        {
            bool isEngEnabled = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);

            List<MappingItem> items = this.EditMappingItems;
            switch (this.EditFileType)
            {
                case "txt":
                    foreach (MappingItem item in items)
                    {
                        item.FileType = "txt";
                        if (item.IsChecked)
                        {
                            #region [MDY:20210325] 原碼修正
                            item.TxtStart = this.Request.Form[String.Format("tbx{0}_S", item.Key)];
                            item.TxtLength = this.Request.Form[String.Format("tbx{0}_L", item.Key)];
                            #endregion
                        }
                        else
                        {
                            item.TxtStart = String.Empty;
                            item.TxtLength = String.Empty;
                        }
                    }
                    break;
                case "xls":
                    foreach (MappingItem item in items)
                    {
                        item.FileType = "xls";
                        if (item.IsChecked)
                        {
                            #region [MDY:20210325] 原碼修正
                            if (this.IsEngXlsMaapingItem(item.Key))
                            {
                                if (isEngEnabled)
                                {
                                    item.XlsName = this.Request.Form[String.Format("tbx{0}", item.Key)];
                                }
                                else
                                {
                                    item.XlsName = null;
                                }
                            }
                            else
                            {
                                item.XlsName = this.Request.Form[String.Format("tbx{0}", item.Key)];
                            }

                            if (!String.IsNullOrEmpty(item.EngFieldName))
                            {
                                if (isEngEnabled)
                                {
                                    item.EngFieldValue = this.Request.Form[String.Format("tbx{0}", item.EngFieldName)];
                                }
                                else
                                {
                                    item.EngFieldValue = null;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            item.XlsName = String.Empty;
                        }
                    }
                    break;
            }
            return items;
        }

        protected string GetNewMappingID(string tableName)
        {
            if (tableName == MappingreTxtEntity.TABLE_NAME)
            {
                MappingreTxtEntity data = null;
                Expression where = new Expression(MappingreTxtEntity.Field.ReceiveType, this.EditReceiveType);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(MappingreTxtEntity.Field.MappingId, OrderByEnum.Desc);

                XmlResult result = DataProxy.Current.SelectFirst<MappingreTxtEntity>(this, where, orderbys, out data);
                if (result.IsSuccess)
                {
                    if (data == null)
                    {
                        return "01";
                    }
                    else
                    {
                        int iCount = Convert.ToInt32(data.MappingId);
                        return (iCount + 1).ToString("00");
                    }
                }

                return "01";
            }
            else
            {
                MappingreXlsmdbEntity XlsData = null;
                Expression where = new Expression(MappingreXlsmdbEntity.Field.ReceiveType, this.EditReceiveType);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(MappingreXlsmdbEntity.Field.MappingId, OrderByEnum.Desc);

                XmlResult result = DataProxy.Current.SelectFirst<MappingreXlsmdbEntity>(this, where, orderbys, out XlsData);
                if (result.IsSuccess)
                {
                    if (XlsData == null)
                    {
                        return "01";
                    }
                    else
                    {
                        int iCount = Convert.ToInt32(XlsData.MappingId);
                        return (iCount + 1).ToString("00");
                    }
                }

                return "01";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                #region 檢查維護權限
                if (!this.HasMaintainAuth())
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                    this.lbtnGoStep2.Visible = false;
                    return;
                }
                #endregion

                #region 處理參數
                KeyValueList<string> QueryString = Session["QueryString"] as KeyValueList<string>;
                if (QueryString == null || QueryString.Count == 0)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("缺少網頁參數");
                    this.ShowSystemMessage(msg);
                    this.lbtnGoStep2.Visible = false;
                    return;
                }

                this.Action = QueryString.TryGetValue("Action", String.Empty);
                this.EditMappingId = QueryString.TryGetValue("MappingId", String.Empty);
                this.EditReceiveType = QueryString.TryGetValue("ReceiveType", String.Empty);
                this.EditFileType = QueryString.TryGetValue("FileType", String.Empty);
                this.EditMappingName = QueryString.TryGetValue("MappingName", String.Empty);
                //this.EditMode = QueryString.TryGetValue("Mode", String.Empty);

                if (String.IsNullOrEmpty(this.EditReceiveType)
                    || !ActionMode.IsMaintinaMode(this.Action)
                    || (!ActionMode.IsPKeyEditableMode(this.Action) && (String.IsNullOrEmpty(this.EditMappingId) || String.IsNullOrEmpty(this.EditFileType))))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.lbtnGoStep2.Visible = false;
                    return;
                }
                #endregion

                #region 檢查商家代號授權
                if (!this.GetLogonUser().IsAuthReceiveTypes(this.EditReceiveType))
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該商家代號");
                    this.lbtnGoStep2.Visible = false;
                    return;
                }
                #endregion

                #region 結繫商家代號資料
                {
                    XmlResult xmlResult = this.ucFilter1.GetDataAndBind(this.EditReceiveType, null, null);
                    if (!xmlResult.IsSuccess)
                    {
                        string action = "查詢商家代號資料";
                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        this.lbtnGoStep2.Visible = false;
                        return;
                    }
                }
                #endregion

                #region 取得並結繫維護資料
                List<MappingItem> items = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            switch (this.EditFileType)
                            {
                                case "xls":
                                    #region xls
                                    {
                                        MappingreXlsmdbEntity xlsData = new MappingreXlsmdbEntity();
                                        items = GetMappingItems(xlsData);
                                    }
                                    #endregion
                                    break;
                                case "txt":
                                    #region txt
                                    {
                                        MappingreTxtEntity txtData = new MappingreTxtEntity();
                                        items = GetMappingItems(txtData);
                                    }
                                    #endregion
                                    break;
                                default:
                                    #region xls
                                    {
                                        MappingreXlsmdbEntity xlsData = new MappingreXlsmdbEntity();
                                        items = GetMappingItems(xlsData);
                                    }
                                    #endregion
                                    break;
                            }
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");
                            switch (this.EditFileType)
                            {
                                case "xls":
                                    #region xls
                                    {
                                        Expression where = new Expression(MappingreXlsmdbEntity.Field.ReceiveType, this.EditReceiveType)
                                            .And(MappingreXlsmdbEntity.Field.MappingId, this.EditMappingId);
                                        MappingreXlsmdbEntity xlsData = null;
                                        XmlResult xmlResult = DataProxy.Current.SelectFirst<MappingreXlsmdbEntity>(this, where, null, out xlsData);
                                        if (!xmlResult.IsSuccess)
                                        {
                                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                                            this.lbtnGoStep2.Visible = false;
                                            return;
                                        }
                                        if (xlsData == null)
                                        {
                                            this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                                            this.lbtnGoStep2.Visible = false;
                                            return;
                                        }
                                        items = GetMappingItems(xlsData);
                                    }
                                    #endregion
                                    break;
                                case "txt":
                                    #region txt
                                    {
                                        Expression where = new Expression(MappingreTxtEntity.Field.ReceiveType, this.EditReceiveType)
                                            .And(MappingreTxtEntity.Field.MappingId, this.EditMappingId);
                                        MappingreTxtEntity txtData = null;
                                        XmlResult xmlResult = DataProxy.Current.SelectFirst<MappingreTxtEntity>(this, where, null, out txtData);
                                        if (!xmlResult.IsSuccess)
                                        {
                                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                                            this.lbtnGoStep2.Visible = false;
                                            return;
                                        }
                                        if (txtData == null)
                                        {
                                            this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                                            this.lbtnGoStep2.Visible = false;
                                            return;
                                        }
                                        items = GetMappingItems(txtData);
                                    }
                                    #endregion
                                    break;
                                default:
                                    //[TODO] 固定顯示訊息的收集
                                    string msg = this.GetLocalized("網頁參數不正確");
                                    this.ShowSystemMessage(msg);
                                    this.lbtnGoStep2.Visible = false;
                                    return;
                            }
                        }
                        #endregion
                        break;
                    default:
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized("網頁參數不正確");
                            this.ShowSystemMessage(msg);
                            this.lbtnGoStep2.Visible = false;
                        }
                        return;
                }
                #endregion

                this.BindStep1Data(this.EditFileType, this.EditMappingName, items);
                this.divStep1.Visible = true;
                this.divStep2.Visible = false;
            }
        }

        protected void lbtnGoStep2_Click(object sender, EventArgs e)
        {
            if (!this.CheckStep1InputData())
            {
                return;
            }

            string fileType = null;
            string mappingName = null;
            switch (this.Action)
            {
                case ActionMode.Insert:
                    fileType = this.ddlFileType.SelectedValue;
                    this.EditFileType = fileType;
                    break;
                default:
                    fileType = this.EditFileType;
                    break;
            }

            mappingName = this.tbxMappingName.Text.Trim();
            this.EditMappingName = mappingName;

            #region 取得勾選項目與項數
            bool hasSeriorNo = false;
            bool hasCancelNo = false;
            bool hasReceiveAmount = false;
            bool hasReceive = false;
            List<MappingItem> items = this.EditMappingItems;
            {
                #region 勾選項目
                {
                    List<CheckBox> checkboxs = this.GetStep1AllCheckBoxs();
                    foreach (MappingItem item in items)
                    {
                        CheckBox cbx = checkboxs.Find(x => item.Key.Equals(x.ID.Substring(3), StringComparison.CurrentCultureIgnoreCase));
                        if (cbx != null)
                        {
                            item.IsChecked = cbx.Checked;
                            item.Name = cbx.Text;

                            if (item.IsChecked)
                            {
                                switch (item.Key.ToUpper())
                                {
                                    case "SERIOR_NO":
                                        hasSeriorNo = true;
                                        break;
                                    case "CANCEL_NO":
                                        hasCancelNo = true;
                                        break;
                                    case "RECEIVE_AMOUNT":
                                        hasReceiveAmount = true;
                                        break;
                                    case "RECEIVE":
                                        hasReceive = true;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            item.IsChecked = false;
                        }
                    }
                }
                #endregion

                #region 學分基準
                int creditIdCount = 0;
                {
                    string key = "Credit_Id";
                    string name = this.chkCredit_Id.Text;
                    MappingItem item = items.Find(x => key.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                    if (item != null && item.IsChecked)
                    //if (item != null)
                    {
                        item.ItemCount = item.IsChecked ? this.tbxCreditIdCount.Text : String.Empty;
                        int? count = item.GetItemCount();
                        if (count != null && count.Value > 0)
                        {
                            creditIdCount = count.Value;
                        }
                        for (int no = 1; no <= item.MaxItemCount; no++)
                        {
                            string subKey = String.Format("{0}{1}", key, no);
                            MappingItem subItem = items.Find(x => subKey.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                            if (subItem != null)
                            {
                                subItem.IsChecked = (no <= creditIdCount);
                                subItem.Name = String.Format("{0}{1}", name, no);
                            }
                        }
                    }
                }
                #endregion

                #region 課程代碼
                int courseIdCount = 0;
                {
                    string key = "Course_Id";
                    string name = this.chkCourse_Id.Text;
                    MappingItem item = items.Find(x => key.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                    if (item != null && item.IsChecked)
                    //if (item != null)
                    {
                        item.ItemCount = item.IsChecked ? this.tbxCourseIdCount.Text : String.Empty;
                        int? count = item.GetItemCount();
                        if (count != null && count.Value > 0)
                        {
                            courseIdCount = count.Value;
                        }
                        for (int no = 1; no <= item.MaxItemCount; no++)
                        {
                            string subKey = String.Format("{0}{1}", key, no);
                            MappingItem subItem = items.Find(x => subKey.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                            if (subItem != null)
                            {
                                subItem.IsChecked = (no <= courseIdCount);
                                subItem.Name = String.Format("{0}{1}", name, no);
                            }
                        }
                    }
                }
                #endregion

                #region 收入科目金額
                int receiveCount = 0;
                {
                    string key = "Receive";
                    string name = this.chkReceive.Text;
                    MappingItem item = items.Find(x => key.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                    if (item != null && item.IsChecked)
                    //if (item != null)
                    {
                        item.ItemCount = item.IsChecked ? this.tbxReceiveCount.Text : String.Empty;
                        int? count = item.GetItemCount();
                        if (count != null && count.Value > 0)
                        {
                            receiveCount = count.Value;
                        }
                        for (int no = 1; no <= item.MaxItemCount; no++)
                        {
                            string subKey = String.Format("{0}_{1}", key, no);
                            MappingItem subItem = items.Find(x => subKey.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                            if (subItem != null)
                            {
                                subItem.IsChecked = (no <= receiveCount);
                                subItem.Name = String.Format("{0}{1}", name, no);
                            }
                        }
                    }
                }
                #endregion

                #region 學分基準或課程學分數
                {
                    int creditCount = 0;
                    string name = null;

                    #region [Old]
                    //if (creditIdCount > 0)
                    //{
                    //    creditCount = creditIdCount;
                    //    name = "學分基準學分數";
                    //}
                    //else if (courseIdCount > 0)
                    //{
                    //    creditCount = courseIdCount;
                    //    name = "課程學分數";
                    //}
                    //string key = "Credit";
                    //MappingItem item = items.Find(x => key.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                    //if (item != null && item.IsChecked)
                    //{
                    //    item.ItemCount = item.IsChecked ? this.tbxCreditIdCount.Text : String.Empty;
                    //}
                    #endregion

                    #region [New]
                    string key = "Credit";
                    MappingItem item = items.Find(x => key.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                    if (item != null && item.IsChecked)
                    {
                        if (creditIdCount > 0)
                        {
                            creditCount = creditIdCount;
                            name = "學分基準學分數";
                        }
                        else if (courseIdCount > 0)
                        {
                            creditCount = courseIdCount;
                            name = "課程學分數";
                        }
                        item.ItemCount = creditCount.ToString();
                    }
                    #endregion

                    for (int no = 1; no <= item.MaxItemCount; no++)
                    {
                        string subKey = String.Format("{0}{1}", key, no);
                        MappingItem subItem = items.Find(x => subKey.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase));
                        if (subItem != null)
                        {
                            subItem.IsChecked = (no <= creditCount);
                            subItem.Name = String.Format("{0}{1}", name, no);
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 檢查勾選項目
            {
                if (hasSeriorNo && hasCancelNo)
                {
                    string mag = this.GetLocalized("「流水號」與「虛擬帳號」不可同時上傳");
                    this.ShowSystemMessage(mag);
                    return;
                }
                if (hasCancelNo && !hasReceiveAmount)
                {
                    string mag = this.GetLocalized("上傳「虛擬帳號」必須同時上傳「繳費金額」");
                    this.ShowSystemMessage(mag);
                    return;
                }
                if (hasReceiveAmount && !hasReceive)
                {
                    string mag = this.GetLocalized("上傳「繳費金額」必須同時上傳「收入科目金額」");
                    this.ShowSystemMessage(mag);
                    return;
                }
            }
            #endregion

            this.BindStep2Data(fileType, mappingName, items);
            this.divStep1.Visible = false;
            this.divStep2.Visible = true;
        }

        protected void lbtnGoStep1_Click(object sender, EventArgs e)
        {
            this.EditMappingItems = this.GetStep2Data();
            this.BindStep1Data(this.EditFileType, this.EditMappingName, this.EditMappingItems);
            this.divStep1.Visible = true;
            this.divStep2.Visible = false;
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "D1400001.aspx";

            switch (this.Action)
            {
                case ActionMode.Insert:
                case ActionMode.Modify:
                    #region Insert | Modify
                    {
                        List<MappingItem> items = this.GetStep2Data();
                        this.EditMappingItems = items;

                        switch (this.EditFileType)
                        {
                            case "txt":
                                #region Txt 格式
                                if (this.CheckTxtMappingItems(items))
                                {
                                    MappingreTxtEntity mapping = this.GetMappingreTxtEntity(items);
                                    if (mapping == null)
                                    {
                                        this.ShowSystemMessage("無法將對照欄位設定轉成 Entity 型別");
                                        this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                                        return;
                                    }

                                    mapping.ReceiveType = this.EditReceiveType;
                                    mapping.MappingName = this.EditMappingName;
                                    mapping.Status = DataStatusCodeTexts.NORMAL;
                                    mapping.CrtDate = DateTime.Now;
                                    mapping.CrtUser = this.GetLogonUser().UserId;

                                    int count = 0;
                                    string noDataMsg = null;
                                    XmlResult xmlResult = null;
                                    if (this.Action == ActionMode.Insert)
                                    {
                                        noDataMsg = "無資料被新增";
                                        mapping.MappingId = this.GetNewMappingID(MappingreTxtEntity.TABLE_NAME);
                                        xmlResult = DataProxy.Current.Insert<MappingreTxtEntity>(this.Page, mapping, out count);
                                    }
                                    else
                                    {
                                        noDataMsg = "無資料被更新";
                                        mapping.MappingId = this.EditMappingId;
                                        xmlResult = DataProxy.Current.Update<MappingreTxtEntity>(this.Page, mapping, out count);
                                    }
                                    if (xmlResult.IsSuccess)
                                    {
                                        if (count < 1)
                                        {
                                            this.ShowActionFailureMessage(action, noDataMsg);
                                            this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                                        }
                                        else
                                        {
                                            this.ShowActionSuccessAlert(action, backUrl);
                                        }
                                    }
                                    else
                                    {
                                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                                        this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                                    }
                                }
                                else
                                {
                                    this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                                }
                                #endregion
                                break;
                            case "xls":
                                #region Xls 格式
                                if (this.CheckXlsMappingItems(items))
                                {
                                    MappingreXlsmdbEntity mapping = this.GetMappingreXlsmdbEntity(items);
                                    if (mapping == null)
                                    {
                                        this.ShowSystemMessage("無法將對照欄位設定轉成 Entity 型別");
                                        this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                                        return;
                                    }

                                    mapping.ReceiveType = this.EditReceiveType;
                                    mapping.MappingName = this.EditMappingName;
                                    mapping.Status = DataStatusCodeTexts.NORMAL;
                                    mapping.CrtDate = DateTime.Now;
                                    mapping.CrtUser = this.GetLogonUser().UserId;

                                    int count = 0;
                                    string noDataMsg = null;
                                    XmlResult xmlResult = null;
                                    if (this.Action == ActionMode.Insert)
                                    {
                                        noDataMsg = "無資料被新增";
                                        mapping.MappingId = this.GetNewMappingID(MappingreXlsmdbEntity.TABLE_NAME);
                                        xmlResult = DataProxy.Current.Insert<MappingreXlsmdbEntity>(this.Page, mapping, out count);
                                    }
                                    else
                                    {
                                        noDataMsg = "無資料被更新";
                                        mapping.MappingId = this.EditMappingId;
                                        xmlResult = DataProxy.Current.Update<MappingreXlsmdbEntity>(this.Page, mapping, out count);
                                    }
                                    if (xmlResult.IsSuccess)
                                    {
                                        if (count < 1)
                                        {
                                            this.ShowActionFailureMessage(action, noDataMsg);
                                            this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                                        }
                                        else
                                        {
                                            this.ShowActionSuccessAlert(action, backUrl);
                                        }
                                    }
                                    else
                                    {
                                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                                        this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                                    }
                                }
                                else
                                {
                                    this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                                }
                                #endregion
                                break;
                        }


                        #region [Old]
                        //KeyValueList<string> args = new KeyValueList<string>(items.Count + 4);
                        //#region [Old]
                        ////foreach (MappingItem item in items)
                        ////{
                        ////    if (item.IsChecked && item.MaxItemCount == 0)
                        ////    {
                        ////        switch (this.EditFileType)
                        ////        {
                        ////            case "txt":
                        ////                #region Txt 格式
                        ////                {
                        ////                    if (String.IsNullOrEmpty(item.TxtStart))
                        ////                    {
                        ////                        this.ShowMustInputAlert(item.Name + "起始位置");
                        ////                        this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                        ////                        return;
                        ////                    }
                        ////                    if (String.IsNullOrEmpty(item.TxtLength))
                        ////                    {
                        ////                        this.ShowMustInputAlert(item.Name + "長度");
                        ////                        this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                        ////                        return;
                        ////                    }
                        ////                    int? start = item.GetTxtStart();
                        ////                    if (start == null || start.Value < 1)
                        ////                    {
                        ////                        //[TODO] 固定顯示訊息的收集
                        ////                        string msg = this.GetLocalized(item.Name + "起始位置限輸入大於0的整數");
                        ////                        this.ShowJsAlert(msg);
                        ////                        this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                        ////                        return;
                        ////                    }
                        ////                    int? length = item.GetTxtLength();
                        ////                    if (length == null || length.Value < 1)
                        ////                    {
                        ////                        //[TODO] 固定顯示訊息的收集
                        ////                        string msg = this.GetLocalized(item.Name + "長度限輸入大於0的整數");
                        ////                        this.ShowJsAlert(msg);
                        ////                        this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                        ////                        return;
                        ////                    }
                        ////                    //args.Add(String.Format("{0}_S, {0}_L", item.FieldName), String.Format("{0}, {1}", start.Value, length.Value));
                        ////                    args.Add(String.Format("{0}_S", item.FieldName), start.Value.ToString());
                        ////                    args.Add(String.Format("{0}_L", item.FieldName), length.Value.ToString());
                        ////                }
                        ////                #endregion
                        ////                break;
                        ////            case "xls":
                        ////                #region xls 格式
                        ////                {
                        ////                    if (String.IsNullOrEmpty(item.XlsName))
                        ////                    {
                        ////                        this.ShowMustInputAlert(item.Name);
                        ////                        this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                        ////                        return;
                        ////                    }
                        ////                    else if (xlsNames.Contains(item.XlsName))
                        ////                    {
                        ////                        this.ShowSystemMessage(String.Format("Excel欄位名稱 {0} 重複出現", item.XlsName));
                        ////                        this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                        ////                        return;
                        ////                    }
                        ////                    else
                        ////                    {
                        ////                        xlsNames.Add(item.XlsName);
                        ////                        args.Add(item.FieldName, item.XlsName);
                        ////                    }
                        ////                }
                        ////                #endregion
                        ////                break;
                        ////        }
                        ////    }
                        ////}
                        //#endregion

                        //switch (this.EditFileType)
                        //{
                        //    case "txt":
                        //        #region Txt 格式
                        //        {
                        //            foreach (MappingItem item in items)
                        //            {
                        //                if (item.IsChecked && item.MaxItemCount == 0)
                        //                {
                        //                    if (String.IsNullOrEmpty(item.TxtStart))
                        //                    {
                        //                        this.ShowMustInputAlert(item.Name + "起始位置");
                        //                        this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                        //                        return;
                        //                    }
                        //                    if (String.IsNullOrEmpty(item.TxtLength))
                        //                    {
                        //                        this.ShowMustInputAlert(item.Name + "長度");
                        //                        this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                        //                        return;
                        //                    }
                        //                    int? start = item.GetTxtStart();
                        //                    if (start == null || start.Value < 1)
                        //                    {
                        //                        //[TODO] 固定顯示訊息的收集
                        //                        string msg = this.GetLocalized(item.Name + "起始位置限輸入大於0的整數");
                        //                        this.ShowJsAlert(msg);
                        //                        this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                        //                        return;
                        //                    }
                        //                    int? length = item.GetTxtLength();
                        //                    if (length == null || length.Value < 1)
                        //                    {
                        //                        //[TODO] 固定顯示訊息的收集
                        //                        string msg = this.GetLocalized(item.Name + "長度限輸入大於0的整數");
                        //                        this.ShowJsAlert(msg);
                        //                        this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                        //                        return;
                        //                    }
                        //                    //args.Add(String.Format("{0}_S, {0}_L", item.FieldName), String.Format("{0}, {1}", start.Value, length.Value));
                        //                    args.Add(String.Format("{0}_S", item.FieldName), start.Value.ToString());
                        //                    args.Add(String.Format("{0}_L", item.FieldName), length.Value.ToString());
                        //                }
                        //            }
                        //        }
                        //        #endregion
                        //        break;
                        //    case "xls":
                        //        #region Xls 格式
                        //        {
                        //            List<string> xlsNames = new List<string>(items.Count);
                        //            foreach (MappingItem item in items)
                        //            {
                        //                if (item.IsChecked && item.MaxItemCount == 0)
                        //                {
                        //                    if (String.IsNullOrEmpty(item.XlsName))
                        //                    {
                        //                        this.ShowMustInputAlert(item.Name);
                        //                        this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                        //                        return;
                        //                    }
                        //                    else if (xlsNames.Contains(item.XlsName))
                        //                    {
                        //                        this.ShowSystemMessage(String.Format("Excel欄位名稱 {0} 重複出現", item.XlsName));
                        //                        this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                        //                        return;
                        //                    }
                        //                    else
                        //                    {
                        //                        xlsNames.Add(item.XlsName);
                        //                        args.Add(item.FieldName, item.XlsName);
                        //                    }
                        //                }
                        //            }
                        //        }
                        //        #endregion
                        //        break;
                        //}

                        //string mappingId = null;
                        //if (this.Action == ActionMode.Insert)
                        //{
                        //    //如果是新增要取一個新的 mappingID
                        //    mappingId = this.GetNewMappingID();
                        //}
                        //else
                        //{
                        //    mappingId = this.EditMappingId;
                        //}

                        //args.Add("TableName", (this.EditFileType == "txt" ? MappingreTxtEntity.TABLE_NAME : MappingreXlsmdbEntity.TABLE_NAME));
                        //args.Add("ReceiveType", this.EditReceiveType);
                        //args.Add("MappingId", mappingId);
                        //args.Add("MappingName", this.EditMappingName);

                        //object returnData = null;
                        //XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyMappingXlsData, args, out returnData);
                        //if (xmlResult.IsSuccess)
                        //{
                        //    this.ShowActionSuccessAlert(action, backUrl);
                        //}
                        //else
                        //{
                        //    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        //    this.BindStep2Data(this.EditFileType, this.EditMappingName, items);
                        //}
                        #endregion
                    }
                    #endregion
                    break;
                case ActionMode.Delete:
                    #region Delete
                    {
                        XmlResult xmlResult = null;
                        int count = 0;
                        if (this.EditFileType == "xls")
                        {
                            MappingreXlsmdbEntity xlsData = new MappingreXlsmdbEntity();
                            xlsData.ReceiveType = this.EditReceiveType;
                            xlsData.MappingId = this.EditMappingId;
                            xmlResult = DataProxy.Current.Delete<MappingreXlsmdbEntity>(this, xlsData, out count);
                        }
                        else
                        {
                            MappingreTxtEntity txtData = new MappingreTxtEntity();
                            txtData.ReceiveType = this.EditReceiveType;
                            txtData.MappingId = this.EditMappingId;
                            xmlResult = DataProxy.Current.Delete<MappingreTxtEntity>(this, txtData, out count);
                        }
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                this.ShowActionSuccessAlert(action, backUrl);
                            }
                        }
                        else
                        {
                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                            //this.BindStep2Data(this.EditFileType, this.EditMappingName, this.EditMappingItems);
                        }
                    }
                    #endregion
                    return;
            }
        }
    }
}