using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.D
{
    /// <summary>
    /// 代收費用檔
    /// </summary>
    public partial class D1200001M : BasePage
    {
        //private const int MaxReceiveItemCount = 40;

        #region Keep 頁面參數
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
        /// 編輯的業務別碼參數
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
        /// 編輯的學年參數
        /// </summary>
        private string EditYearId
        {
            get
            {
                return ViewState["EditYearId"] as string;
            }
            set
            {
                ViewState["EditYearId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的學期參數
        /// </summary>
        private string EditTermId
        {
            get
            {
                return ViewState["EditTermId"] as string;
            }
            set
            {
                ViewState["EditTermId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的部別參數
        /// </summary>
        private string EditDepId
        {
            get
            {
                return ViewState["EditDepId"] as string;
            }
            set
            {
                ViewState["EditDepId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的代收費用別參數
        /// </summary>
        private string EditReceiveId
        {
            get
            {
                return ViewState["EditReceiveId"] as string;
            }
            set
            {
                ViewState["EditReceiveId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的代收費用型態 (固定為 1)
        /// </summary>
        private string EditReceiveStatus
        {
            get
            {
                return "1"; //固定為 1 (已有繳費者資料之收費)
            }
        }

        /// <summary>
        /// 編輯的代收費用檔
        /// </summary>
        private SchoolRidEntity EditSchoolRid
        {
            get
            {
                return ViewState["EditSchoolRid"] as SchoolRidEntity;
            }
            set
            {
                ViewState["EditSchoolRid"] = value;
            }
        }

        /// <summary>
        /// 原始的信用卡繳款期限
        /// </summary>
        private string OrginalPayDueDate2
        {
            get
            {
                return ViewState["OrginalPayDueDate2"] as string;
            }
            set
            {
                ViewState["OrginalPayDueDate2"] = value == null ? String.Empty : value.Trim();
            }
        }
        #endregion

        #region [OLD:202202XX] 2022擴充案 改寫所以 MARK
        //#region 收入科目、就貸、教育部補助、申報學雜費、申報住宿費 控制相關方法
        ////private const int ReceiveItemCount = 40;
        ////private const int OtherItemCount = ReceiveItemCount - 16;
        ////private TextBox[] _ReceiveItemTextBoxs = null;
        ////private TextBox GetReceiveItemTextBox(int idx)
        ////{
        ////    if (_ReceiveItemTextBoxs == null)
        ////    {
        ////        _ReceiveItemTextBoxs = new TextBox[] {
        ////            this.tbxReceiveItem01, this.tbxReceiveItem02, this.tbxReceiveItem03, this.tbxReceiveItem04, this.tbxReceiveItem05,
        ////            this.tbxReceiveItem06, this.tbxReceiveItem07, this.tbxReceiveItem08, this.tbxReceiveItem09, this.tbxReceiveItem10,
        ////            this.tbxReceiveItem11, this.tbxReceiveItem12, this.tbxReceiveItem13, this.tbxReceiveItem14, this.tbxReceiveItem15,
        ////            this.tbxReceiveItem16, this.tbxReceiveItem17, this.tbxReceiveItem18, this.tbxReceiveItem19, this.tbxReceiveItem20,
        ////            this.tbxReceiveItem21, this.tbxReceiveItem22, this.tbxReceiveItem23, this.tbxReceiveItem24, this.tbxReceiveItem25,
        ////            this.tbxReceiveItem26, this.tbxReceiveItem27, this.tbxReceiveItem28, this.tbxReceiveItem29, this.tbxReceiveItem30,
        ////            this.tbxReceiveItem31, this.tbxReceiveItem32, this.tbxReceiveItem33, this.tbxReceiveItem34, this.tbxReceiveItem35,
        ////            this.tbxReceiveItem36, this.tbxReceiveItem37, this.tbxReceiveItem38, this.tbxReceiveItem39, this.tbxReceiveItem40
        ////        };
        ////    }
        ////    if (idx < _ReceiveItemTextBoxs.Length)
        ////    {
        ////        return _ReceiveItemTextBoxs[idx];
        ////    }
        ////    else
        ////    {
        ////        return null;
        ////    }
        ////}
        ////private void SetReceiveItemTextBoxValue(int idx, string text)
        ////{
        ////    TextBox tbx = this.GetReceiveItemTextBox(idx);
        ////    if (tbx != null)
        ////    {
        ////        tbx.Text = text;
        ////    }
        ////}
        ////private string GetReceiveItemTextBoxValue(int idx)
        ////{
        ////    TextBox tbx = this.GetReceiveItemTextBox(idx);
        ////    if (tbx != null)
        ////    {
        ////        return tbx.Text.Trim();
        ////    }
        ////    else
        ////    {
        ////        return String.Empty;
        ////    }
        ////}

        //private CheckBox[] _LoanItemCheckBoxs = null;
        //private CheckBox GetLoanItemCheckBox(int idx)
        //{
        //    if (_LoanItemCheckBoxs == null)
        //    {
        //        _LoanItemCheckBoxs = new CheckBox[] {
        //            this.cbxLoanItem01, this.cbxLoanItem02, this.cbxLoanItem03, this.cbxLoanItem04, this.cbxLoanItem05,
        //            this.cbxLoanItem06, this.cbxLoanItem07, this.cbxLoanItem08, this.cbxLoanItem09, this.cbxLoanItem10,
        //            this.cbxLoanItem11, this.cbxLoanItem12, this.cbxLoanItem13, this.cbxLoanItem14, this.cbxLoanItem15,
        //            this.cbxLoanItem16, this.cbxLoanItem17, this.cbxLoanItem18, this.cbxLoanItem19, this.cbxLoanItem20,
        //            this.cbxLoanItem21, this.cbxLoanItem22, this.cbxLoanItem23, this.cbxLoanItem24, this.cbxLoanItem25,
        //            this.cbxLoanItem26, this.cbxLoanItem27, this.cbxLoanItem28, this.cbxLoanItem29, this.cbxLoanItem30,
        //            this.cbxLoanItem31, this.cbxLoanItem32, this.cbxLoanItem33, this.cbxLoanItem34, this.cbxLoanItem35,
        //            this.cbxLoanItem36, this.cbxLoanItem37, this.cbxLoanItem38, this.cbxLoanItem39, this.cbxLoanItem40
        //        };
        //    }
        //    if (idx < _LoanItemCheckBoxs.Length)
        //    {
        //        return _LoanItemCheckBoxs[idx];
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //private void SetLoanItemCheckBoxChecked(int idx, bool isChecked)
        //{
        //    CheckBox cbx = this.GetLoanItemCheckBox(idx);
        //    if (cbx != null)
        //    {
        //        cbx.Checked = isChecked;
        //    }
        //}
        //private bool GetLoanItemCheckBoxChecked(int idx)
        //{
        //    CheckBox cbx = this.GetLoanItemCheckBox(idx);
        //    if (cbx != null)
        //    {
        //        return cbx.Checked;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //private CheckBox[] _IsSubsidyCheckBoxs = null;
        //private CheckBox GetIsSubsidyCheckBox(int idx)
        //{
        //    if (_IsSubsidyCheckBoxs == null)
        //    {
        //        _IsSubsidyCheckBoxs = new CheckBox[] {
        //            this.cbxIsSubsidy01, this.cbxIsSubsidy02, this.cbxIsSubsidy03, this.cbxIsSubsidy04, this.cbxIsSubsidy05,
        //            this.cbxIsSubsidy06, this.cbxIsSubsidy07, this.cbxIsSubsidy08, this.cbxIsSubsidy09, this.cbxIsSubsidy10,
        //            this.cbxIsSubsidy11, this.cbxIsSubsidy12, this.cbxIsSubsidy13, this.cbxIsSubsidy14, this.cbxIsSubsidy15,
        //            this.cbxIsSubsidy16, this.cbxIsSubsidy17, this.cbxIsSubsidy18, this.cbxIsSubsidy19, this.cbxIsSubsidy20,
        //            this.cbxIsSubsidy21, this.cbxIsSubsidy22, this.cbxIsSubsidy23, this.cbxIsSubsidy24, this.cbxIsSubsidy25,
        //            this.cbxIsSubsidy26, this.cbxIsSubsidy27, this.cbxIsSubsidy28, this.cbxIsSubsidy29, this.cbxIsSubsidy30,
        //            this.cbxIsSubsidy31, this.cbxIsSubsidy32, this.cbxIsSubsidy33, this.cbxIsSubsidy34, this.cbxIsSubsidy35,
        //            this.cbxIsSubsidy36, this.cbxIsSubsidy37, this.cbxIsSubsidy38, this.cbxIsSubsidy39, this.cbxIsSubsidy40
        //        };
        //    }
        //    if (idx < _IsSubsidyCheckBoxs.Length)
        //    {
        //        return _IsSubsidyCheckBoxs[idx];
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //private void SetIsSubsidyCheckBoxChecked(int idx, bool isChecked)
        //{
        //    CheckBox cbx = this.GetIsSubsidyCheckBox(idx);
        //    if (cbx != null)
        //    {
        //        cbx.Checked = isChecked;
        //    }
        //}
        //private bool GetIsSubsidyCheckBoxChecked(int idx)
        //{
        //    CheckBox cbx = this.GetIsSubsidyCheckBox(idx);
        //    if (cbx != null)
        //    {
        //        return cbx.Checked;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //private CheckBox[] _EduTaxCheckBoxs = null;
        //private CheckBox GetEduTaxCheckBox(int idx)
        //{
        //    if (_EduTaxCheckBoxs == null)
        //    {
        //        _EduTaxCheckBoxs = new CheckBox[] {
        //            this.cbxEduTax01, this.cbxEduTax02, this.cbxEduTax03, this.cbxEduTax04, this.cbxEduTax05,
        //            this.cbxEduTax06, this.cbxEduTax07, this.cbxEduTax08, this.cbxEduTax09, this.cbxEduTax10,
        //            this.cbxEduTax11, this.cbxEduTax12, this.cbxEduTax13, this.cbxEduTax14, this.cbxEduTax15,
        //            this.cbxEduTax16, this.cbxEduTax17, this.cbxEduTax18, this.cbxEduTax19, this.cbxEduTax20,
        //            this.cbxEduTax21, this.cbxEduTax22, this.cbxEduTax23, this.cbxEduTax24, this.cbxEduTax25,
        //            this.cbxEduTax26, this.cbxEduTax27, this.cbxEduTax28, this.cbxEduTax29, this.cbxEduTax30,
        //            this.cbxEduTax31, this.cbxEduTax32, this.cbxEduTax33, this.cbxEduTax34, this.cbxEduTax35,
        //            this.cbxEduTax36, this.cbxEduTax37, this.cbxEduTax38, this.cbxEduTax39, this.cbxEduTax40
        //        };
        //    }
        //    if (idx < _EduTaxCheckBoxs.Length)
        //    {
        //        return _EduTaxCheckBoxs[idx];
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //private void SetEduTaxCheckBoxChecked(int idx, bool isChecked)
        //{
        //    CheckBox cbx = this.GetEduTaxCheckBox(idx);
        //    if (cbx != null)
        //    {
        //        cbx.Checked = isChecked;
        //    }
        //}
        //private bool GetEduTaxCheckBoxChecked(int idx)
        //{
        //    CheckBox cbx = this.GetEduTaxCheckBox(idx);
        //    if (cbx != null)
        //    {
        //        return cbx.Checked;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //private CheckBox[] _StayTaxCheckBoxs = null;
        //private CheckBox GetStayTaxCheckBox(int idx)
        //{
        //    if (_StayTaxCheckBoxs == null)
        //    {
        //        _StayTaxCheckBoxs = new CheckBox[] {
        //            this.cbxStayTax01, this.cbxStayTax02, this.cbxStayTax03, this.cbxStayTax04, this.cbxStayTax05,
        //            this.cbxStayTax06, this.cbxStayTax07, this.cbxStayTax08, this.cbxStayTax09, this.cbxStayTax10,
        //            this.cbxStayTax11, this.cbxStayTax12, this.cbxStayTax13, this.cbxStayTax14, this.cbxStayTax15,
        //            this.cbxStayTax16, this.cbxStayTax17, this.cbxStayTax18, this.cbxStayTax19, this.cbxStayTax20,
        //            this.cbxStayTax21, this.cbxStayTax22, this.cbxStayTax23, this.cbxStayTax24, this.cbxStayTax25,
        //            this.cbxStayTax26, this.cbxStayTax27, this.cbxStayTax28, this.cbxStayTax29, this.cbxStayTax30,
        //            this.cbxStayTax31, this.cbxStayTax32, this.cbxStayTax33, this.cbxStayTax34, this.cbxStayTax35,
        //            this.cbxStayTax36, this.cbxStayTax37, this.cbxStayTax38, this.cbxStayTax39, this.cbxStayTax40
        //        };
        //    }
        //    if (idx < _StayTaxCheckBoxs.Length)
        //    {
        //        return _StayTaxCheckBoxs[idx];
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //private void SetStayTaxCheckBoxChecked(int idx, bool isChecked)
        //{
        //    CheckBox cbx = this.GetStayTaxCheckBox(idx);
        //    if (cbx != null)
        //    {
        //        cbx.Checked = isChecked;
        //    }
        //}
        //private bool GetStayTaxCheckBoxChecked(int idx)
        //{
        //    CheckBox cbx = this.GetStayTaxCheckBox(idx);
        //    if (cbx != null)
        //    {
        //        return cbx.Checked;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //#endregion
        #endregion

        #region [OLD:202202XX] 2022擴充案 備註項目 改寫所以 MARK
        //#region 備註標題
        ///// <summary>
        ///// 備註標題數量
        ///// </summary>
        //private const int MemoTitleCount = SchoolRidEntity.MemoTitleCount;
        //private TextBox[] _MemoTitleTextBoxs = null;
        //private TextBox GetMemoTitleTextBox(int idx)
        //{
        //    if (_MemoTitleTextBoxs == null)
        //    {
        //        _MemoTitleTextBoxs = new TextBox[MemoTitleCount] {
        //            this.tbxMemoTitle01, this.tbxMemoTitle02, this.tbxMemoTitle03, this.tbxMemoTitle04, this.tbxMemoTitle05,
        //            this.tbxMemoTitle06, this.tbxMemoTitle07, this.tbxMemoTitle08, this.tbxMemoTitle09, this.tbxMemoTitle10,
        //            this.tbxMemoTitle11, this.tbxMemoTitle12, this.tbxMemoTitle13, this.tbxMemoTitle14, this.tbxMemoTitle15,
        //            this.tbxMemoTitle16, this.tbxMemoTitle17, this.tbxMemoTitle18, this.tbxMemoTitle19, this.tbxMemoTitle20,
        //            this.tbxMemoTitle21
        //        };
        //    }
        //    if (idx < _MemoTitleTextBoxs.Length)
        //    {
        //        return _MemoTitleTextBoxs[idx];
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //private void SetMemoTitleTextBoxValue(int idx, string text)
        //{
        //    TextBox tbx = this.GetMemoTitleTextBox(idx);
        //    if (tbx != null)
        //    {
        //        tbx.Text = text;
        //    }
        //}
        //private string GetMemoTitleTextBoxValue(int idx)
        //{
        //    TextBox tbx = this.GetMemoTitleTextBox(idx);
        //    if (tbx != null)
        //    {
        //        return tbx.Text.Trim();
        //    }
        //    else
        //    {
        //        return String.Empty;
        //    }
        //}
        //#endregion
        #endregion

        #region [MDY:202203XX] 2022擴充案 收入科目是否就貸旗標 相關
        private CheckBox[] GetLoanItemCheckBoxs()
        {
            return new CheckBox[ReceiveItemCount] {
                this.cbxLoanItem01, this.cbxLoanItem02, this.cbxLoanItem03, this.cbxLoanItem04, this.cbxLoanItem05,
                this.cbxLoanItem06, this.cbxLoanItem07, this.cbxLoanItem08, this.cbxLoanItem09, this.cbxLoanItem10,
                this.cbxLoanItem11, this.cbxLoanItem12, this.cbxLoanItem13, this.cbxLoanItem14, this.cbxLoanItem15,
                this.cbxLoanItem16, this.cbxLoanItem17, this.cbxLoanItem18, this.cbxLoanItem19, this.cbxLoanItem20,
                this.cbxLoanItem21, this.cbxLoanItem22, this.cbxLoanItem23, this.cbxLoanItem24, this.cbxLoanItem25,
                this.cbxLoanItem26, this.cbxLoanItem27, this.cbxLoanItem28, this.cbxLoanItem29, this.cbxLoanItem30,
                this.cbxLoanItem31, this.cbxLoanItem32, this.cbxLoanItem33, this.cbxLoanItem34, this.cbxLoanItem35,
                this.cbxLoanItem36, this.cbxLoanItem37, this.cbxLoanItem38, this.cbxLoanItem39, this.cbxLoanItem40
            };
        }

        private void BindLoanItemEditData(SchoolRidEntity data, bool enabled)
        {
            CheckBox[] checkBoxs = this.GetLoanItemCheckBoxs();

            string[] values = data?.GetAllLoanItems();
            if (values == null)
            {
                foreach (CheckBox cbx in checkBoxs)
                {
                    cbx.Checked = false;
                    cbx.Enabled = enabled;
                }
            }
            else
            {
                int idx = 0;
                foreach (CheckBox cbx in checkBoxs)
                {
                    cbx.Checked = "Y".Equals(values[idx]);
                    cbx.Enabled = enabled;
                    idx++;
                }
            }
        }

        private void GetLoanItemEditData(SchoolRidEntity data)
        {
            CheckBox[] checkBoxs = this.GetLoanItemCheckBoxs();

            int no = 0;
            foreach (CheckBox cbx in checkBoxs)
            {
                no++;
                data.SetLoanItemByNo(no, (cbx.Checked ? "Y" : "N"));
            }
        }
        #endregion

        #region [MDY:202203XX] 2022擴充案 收入科目是否教育部補助旗標 相關
        private CheckBox[] GetIsSubsidyCheckBoxs()
        {
            return new CheckBox[ReceiveItemCount] {
                this.cbxIsSubsidy01, this.cbxIsSubsidy02, this.cbxIsSubsidy03, this.cbxIsSubsidy04, this.cbxIsSubsidy05,
                this.cbxIsSubsidy06, this.cbxIsSubsidy07, this.cbxIsSubsidy08, this.cbxIsSubsidy09, this.cbxIsSubsidy10,
                this.cbxIsSubsidy11, this.cbxIsSubsidy12, this.cbxIsSubsidy13, this.cbxIsSubsidy14, this.cbxIsSubsidy15,
                this.cbxIsSubsidy16, this.cbxIsSubsidy17, this.cbxIsSubsidy18, this.cbxIsSubsidy19, this.cbxIsSubsidy20,
                this.cbxIsSubsidy21, this.cbxIsSubsidy22, this.cbxIsSubsidy23, this.cbxIsSubsidy24, this.cbxIsSubsidy25,
                this.cbxIsSubsidy26, this.cbxIsSubsidy27, this.cbxIsSubsidy28, this.cbxIsSubsidy29, this.cbxIsSubsidy30,
                this.cbxIsSubsidy31, this.cbxIsSubsidy32, this.cbxIsSubsidy33, this.cbxIsSubsidy34, this.cbxIsSubsidy35,
                this.cbxIsSubsidy36, this.cbxIsSubsidy37, this.cbxIsSubsidy38, this.cbxIsSubsidy39, this.cbxIsSubsidy40
            };
        }

        private void BindIsSubsidyEditData(SchoolRidEntity data, bool enabled)
        {
            CheckBox[] checkBoxs = this.GetIsSubsidyCheckBoxs();

            string[] values = data?.GetAllIsSubsidys();
            if (values == null)
            {
                foreach (CheckBox cbx in checkBoxs)
                {
                    cbx.Checked = false;
                    cbx.Enabled = enabled;
                }
            }
            else
            {
                int idx = 0;
                foreach (CheckBox cbx in checkBoxs)
                {
                    cbx.Checked = "Y".Equals(values[idx]);
                    cbx.Enabled = enabled;
                    idx++;
                }
            }
        }

        private void GetIsSubsidyEditData(SchoolRidEntity data)
        {
            CheckBox[] checkBoxs = this.GetIsSubsidyCheckBoxs();

            int no = 0;
            foreach (CheckBox cbx in checkBoxs)
            {
                no++;
                data.SetIsSubsidyByNo(no, (cbx.Checked ? "Y" : "N"));
            }
        }
        #endregion

        #region [MDY:202203XX] 2022擴充案 收入科目是否申報學雜費旗標 相關
        private CheckBox[] GetEduTaxFlagCheckBoxs()
        {
            return new CheckBox[ReceiveItemCount] {
                this.cbxEduTax01, this.cbxEduTax02, this.cbxEduTax03, this.cbxEduTax04, this.cbxEduTax05,
                this.cbxEduTax06, this.cbxEduTax07, this.cbxEduTax08, this.cbxEduTax09, this.cbxEduTax10,
                this.cbxEduTax11, this.cbxEduTax12, this.cbxEduTax13, this.cbxEduTax14, this.cbxEduTax15,
                this.cbxEduTax16, this.cbxEduTax17, this.cbxEduTax18, this.cbxEduTax19, this.cbxEduTax20,
                this.cbxEduTax21, this.cbxEduTax22, this.cbxEduTax23, this.cbxEduTax24, this.cbxEduTax25,
                this.cbxEduTax26, this.cbxEduTax27, this.cbxEduTax28, this.cbxEduTax29, this.cbxEduTax30,
                this.cbxEduTax31, this.cbxEduTax32, this.cbxEduTax33, this.cbxEduTax34, this.cbxEduTax35,
                this.cbxEduTax36, this.cbxEduTax37, this.cbxEduTax38, this.cbxEduTax39, this.cbxEduTax40
            };
        }

        private void BindEduTaxFlagEditData(SchoolRidEntity data, bool enabled)
        {
            CheckBox[] checkBoxs = this.GetEduTaxFlagCheckBoxs();

            string[] values = data?.GetAllEduTaxFlags();
            if (values == null)
            {
                foreach (CheckBox cbx in checkBoxs)
                {
                    cbx.Checked = false;
                    cbx.Enabled = enabled;
                }
            }
            else
            {
                int idx = 0;
                foreach (CheckBox cbx in checkBoxs)
                {
                    cbx.Checked = "Y".Equals(values[idx]);
                    cbx.Enabled = enabled;
                    idx++;
                }
            }
        }

        private void GetEduTaxFlagEditData(SchoolRidEntity data)
        {
            CheckBox[] checkBoxs = this.GetEduTaxFlagCheckBoxs();

            int no = 0;
            foreach (CheckBox cbx in checkBoxs)
            {
                no++;
                data.SetEduTaxFlagByNo(no, (cbx.Checked ? "Y" : "N"));
            }
        }
        #endregion

        #region [MDY:202203XX] 2022擴充案 收入科目是否申報住宿費旗標 相關
        private CheckBox[] GetStayTaxFlagCheckBoxs()
        {
            return new CheckBox[ReceiveItemCount] {
                this.cbxStayTax01, this.cbxStayTax02, this.cbxStayTax03, this.cbxStayTax04, this.cbxStayTax05,
                this.cbxStayTax06, this.cbxStayTax07, this.cbxStayTax08, this.cbxStayTax09, this.cbxStayTax10,
                this.cbxStayTax11, this.cbxStayTax12, this.cbxStayTax13, this.cbxStayTax14, this.cbxStayTax15,
                this.cbxStayTax16, this.cbxStayTax17, this.cbxStayTax18, this.cbxStayTax19, this.cbxStayTax20,
                this.cbxStayTax21, this.cbxStayTax22, this.cbxStayTax23, this.cbxStayTax24, this.cbxStayTax25,
                this.cbxStayTax26, this.cbxStayTax27, this.cbxStayTax28, this.cbxStayTax29, this.cbxStayTax30,
                this.cbxStayTax31, this.cbxStayTax32, this.cbxStayTax33, this.cbxStayTax34, this.cbxStayTax35,
                this.cbxStayTax36, this.cbxStayTax37, this.cbxStayTax38, this.cbxStayTax39, this.cbxStayTax40
            };
        }

        private void BindStayTaxFlagEditData(SchoolRidEntity data, bool enabled)
        {
            CheckBox[] checkBoxs = this.GetStayTaxFlagCheckBoxs();

            string[] values = data?.GetAllStayTaxFlags();
            if (values == null)
            {
                foreach (CheckBox cbx in checkBoxs)
                {
                    cbx.Checked = false;
                    cbx.Enabled = enabled;
                }
            }
            else
            {
                int idx = 0;
                foreach (CheckBox cbx in checkBoxs)
                {
                    cbx.Checked = "Y".Equals(values[idx]);
                    cbx.Enabled = enabled;
                    idx++;
                }
            }
        }

        private void GetStayTaxFlagEditData(SchoolRidEntity data)
        {
            CheckBox[] checkBoxs = this.GetStayTaxFlagCheckBoxs();

            int no = 0;
            foreach (CheckBox cbx in checkBoxs)
            {
                no++;
                data.SetStayTaxFlagByNo(no, (cbx.Checked ? "Y" : "N"));
            }
        }
        #endregion

        #region [MDY:202203XX] 2022擴充案 英文欄位相關
        #region 英文欄位 PlaceHolder
        private void ChangeEngPlaceHolders(bool visible)
        {
            PlaceHolder[] engPlaceHolders = new PlaceHolder[]
            {
                phdStyle,
                phdBillFormEId, phdInvoiceFormEId,
                phdReceiveItemEng01, phdReceiveItemEng02, phdReceiveItemEng03, phdReceiveItemEng04, phdReceiveItemEng05,
                phdReceiveItemEng06, phdReceiveItemEng07, phdReceiveItemEng08, phdReceiveItemEng09, phdReceiveItemEng10,
                phdReceiveItemEng11, phdReceiveItemEng12, phdReceiveItemEng13, phdReceiveItemEng14, phdReceiveItemEng15,
                phdReceiveItemEng16, phdReceiveItemEng17, phdReceiveItemEng18, phdReceiveItemEng19, phdReceiveItemEng20,
                phdReceiveItemEng21, phdReceiveItemEng22, phdReceiveItemEng23, phdReceiveItemEng24, phdReceiveItemEng25,
                phdReceiveItemEng26, phdReceiveItemEng27, phdReceiveItemEng28, phdReceiveItemEng29, phdReceiveItemEng30,
                phdReceiveItemEng31, phdReceiveItemEng32, phdReceiveItemEng33, phdReceiveItemEng34, phdReceiveItemEng35,
                phdReceiveItemEng36, phdReceiveItemEng37, phdReceiveItemEng38, phdReceiveItemEng39, phdReceiveItemEng40,
                phdMemoTitleEng01, phdMemoTitleEng02, phdMemoTitleEng03, phdMemoTitleEng04, phdMemoTitleEng05,
                phdMemoTitleEng06, phdMemoTitleEng07, phdMemoTitleEng08, phdMemoTitleEng09, phdMemoTitleEng10,
                phdMemoTitleEng11, phdMemoTitleEng12, phdMemoTitleEng13, phdMemoTitleEng14, phdMemoTitleEng15,
                phdMemoTitleEng16, phdMemoTitleEng17, phdMemoTitleEng18, phdMemoTitleEng19, phdMemoTitleEng20,
                phdMemoTitleEng21,
                phdBriefEng1, phdBriefEng2, phdBriefEng3, phdBriefEng4, phdBriefEng5,
                phdBriefEng6
            };
            foreach (PlaceHolder phdControl in engPlaceHolders)
            {
                phdControl.Visible = visible;
            }
        }
        #endregion

        #region 收入科目 (中英文名稱) 相關
        private const int ReceiveItemCount = 40;

        private const int OtherItemCount = ReceiveItemCount - 16;

        private void GetReceiveItemTextboxs(out TextBox[] chtTextBoxs, out TextBox[] engTextBoxs)
        {
            chtTextBoxs = new TextBox[ReceiveItemCount] {
                this.tbxReceiveItemCht01, this.tbxReceiveItemCht02, this.tbxReceiveItemCht03, this.tbxReceiveItemCht04, this.tbxReceiveItemCht05,
                this.tbxReceiveItemCht06, this.tbxReceiveItemCht07, this.tbxReceiveItemCht08, this.tbxReceiveItemCht09, this.tbxReceiveItemCht10,
                this.tbxReceiveItemCht11, this.tbxReceiveItemCht12, this.tbxReceiveItemCht13, this.tbxReceiveItemCht14, this.tbxReceiveItemCht15,
                this.tbxReceiveItemCht16, this.tbxReceiveItemCht17, this.tbxReceiveItemCht18, this.tbxReceiveItemCht19, this.tbxReceiveItemCht20,
                this.tbxReceiveItemCht21, this.tbxReceiveItemCht22, this.tbxReceiveItemCht23, this.tbxReceiveItemCht24, this.tbxReceiveItemCht25,
                this.tbxReceiveItemCht26, this.tbxReceiveItemCht27, this.tbxReceiveItemCht28, this.tbxReceiveItemCht29, this.tbxReceiveItemCht30,
                this.tbxReceiveItemCht31, this.tbxReceiveItemCht32, this.tbxReceiveItemCht33, this.tbxReceiveItemCht34, this.tbxReceiveItemCht35,
                this.tbxReceiveItemCht36, this.tbxReceiveItemCht37, this.tbxReceiveItemCht38, this.tbxReceiveItemCht39, this.tbxReceiveItemCht40
            };

            engTextBoxs = new TextBox[ReceiveItemCount] {
                this.tbxReceiveItemEng01, this.tbxReceiveItemEng02, this.tbxReceiveItemEng03, this.tbxReceiveItemEng04, this.tbxReceiveItemEng05,
                this.tbxReceiveItemEng06, this.tbxReceiveItemEng07, this.tbxReceiveItemEng08, this.tbxReceiveItemEng09, this.tbxReceiveItemEng10,
                this.tbxReceiveItemEng11, this.tbxReceiveItemEng12, this.tbxReceiveItemEng13, this.tbxReceiveItemEng14, this.tbxReceiveItemEng15,
                this.tbxReceiveItemEng16, this.tbxReceiveItemEng17, this.tbxReceiveItemEng18, this.tbxReceiveItemEng19, this.tbxReceiveItemEng20,
                this.tbxReceiveItemEng21, this.tbxReceiveItemEng22, this.tbxReceiveItemEng23, this.tbxReceiveItemEng24, this.tbxReceiveItemEng25,
                this.tbxReceiveItemEng26, this.tbxReceiveItemEng27, this.tbxReceiveItemEng28, this.tbxReceiveItemEng29, this.tbxReceiveItemEng30,
                this.tbxReceiveItemEng31, this.tbxReceiveItemEng32, this.tbxReceiveItemEng33, this.tbxReceiveItemEng34, this.tbxReceiveItemEng35,
                this.tbxReceiveItemEng36, this.tbxReceiveItemEng37, this.tbxReceiveItemEng38, this.tbxReceiveItemEng39, this.tbxReceiveItemEng40
            };
        }

        private void BindReceiveItemEditData(SchoolRidEntity data, bool enabled, bool isEngEabled)
        {
            TextBox[] chtTextBoxs = null, engTextBoxs = null;
            this.GetReceiveItemTextboxs(out chtTextBoxs, out engTextBoxs);

            if (data == null)
            {
                #region 中文
                foreach (TextBox tbx in chtTextBoxs)
                {
                    tbx.Text = String.Empty;
                    tbx.Enabled = enabled;
                }
                #endregion

                #region 英文
                foreach (TextBox tbx in engTextBoxs)
                {
                    tbx.Text = String.Empty;
                    tbx.Enabled = enabled;
                }
                #endregion
            }
            else
            {
                #region 中文
                {
                    int no = 0;
                    foreach (TextBox tbx in chtTextBoxs)
                    {
                        no++;
                        tbx.Text = data.GetReceiveItemChtByNo(no);
                        tbx.Enabled = enabled;
                    }
                }
                #endregion

                #region 英文
                if (isEngEabled)
                {
                    int no = 0;
                    foreach (TextBox tbx in engTextBoxs)
                    {
                        no++;
                        tbx.Text = data.GetReceiveItemEngByNo(no);
                        tbx.Enabled = enabled;
                    }
                }
                else
                {
                    foreach (TextBox tbx in engTextBoxs)
                    {
                        tbx.Text = String.Empty;
                        tbx.Enabled = enabled;
                    }
                }
                #endregion
            }
        }

        private void GetReceiveItemEditData(SchoolRidEntity data, bool isEngEabled)
        {
            TextBox[] chtTextBoxs = null, engTextBoxs = null;
            this.GetReceiveItemTextboxs(out chtTextBoxs, out engTextBoxs);

            #region 中文
            {
                int no = 0;
                foreach (TextBox tbx in chtTextBoxs)
                {
                    no++;
                    data.SetReceiveItemChtByNo(no, tbx.Text.Trim());
                }
            }
            #endregion

            #region 英文
            if (isEngEabled)
            {
                int no = 0;
                foreach (TextBox tbx in engTextBoxs)
                {
                    no++;
                    data.SetReceiveItemEngByNo(no, tbx.Text.Trim());
                }
            }
            else
            {
                for (int no = 1; no <= SchoolRidEntity.ReceiveItemMaxCount; no++)
                {
                    data.SetReceiveItemEngByNo(no, null);
                }
            }
            #endregion
        }
        #endregion

        #region 備註項目 (中英文標題) 相關
        private const int MemoTitleCount = 21;

        private void GetMemoTitleTextboxs(out TextBox[] chtTextBoxs, out TextBox[] engTextBoxs)
        {
            chtTextBoxs = new TextBox[MemoTitleCount] {
                this.tbxMemoTitleCht01, this.tbxMemoTitleCht02, this.tbxMemoTitleCht03, this.tbxMemoTitleCht04, this.tbxMemoTitleCht05,
                this.tbxMemoTitleCht06, this.tbxMemoTitleCht07, this.tbxMemoTitleCht08, this.tbxMemoTitleCht09, this.tbxMemoTitleCht10,
                this.tbxMemoTitleCht11, this.tbxMemoTitleCht12, this.tbxMemoTitleCht13, this.tbxMemoTitleCht14, this.tbxMemoTitleCht15,
                this.tbxMemoTitleCht16, this.tbxMemoTitleCht17, this.tbxMemoTitleCht18, this.tbxMemoTitleCht19, this.tbxMemoTitleCht20,
                this.tbxMemoTitleCht21
            };

            engTextBoxs = new TextBox[MemoTitleCount] {
                this.tbxMemoTitleEng01, this.tbxMemoTitleEng02, this.tbxMemoTitleEng03, this.tbxMemoTitleEng04, this.tbxMemoTitleEng05,
                this.tbxMemoTitleEng06, this.tbxMemoTitleEng07, this.tbxMemoTitleEng08, this.tbxMemoTitleEng09, this.tbxMemoTitleEng10,
                this.tbxMemoTitleEng11, this.tbxMemoTitleEng12, this.tbxMemoTitleEng13, this.tbxMemoTitleEng14, this.tbxMemoTitleEng15,
                this.tbxMemoTitleEng16, this.tbxMemoTitleEng17, this.tbxMemoTitleEng18, this.tbxMemoTitleEng19, this.tbxMemoTitleEng20,
                this.tbxMemoTitleEng21
            };
        }

        private void BindMemoTitleEditData(SchoolRidEntity data, bool enabled, bool isEngEabled)
        {
            TextBox[] chtTextBoxs = null, engTextBoxs = null;
            this.GetMemoTitleTextboxs(out chtTextBoxs, out engTextBoxs);

            if (data == null)
            {
                #region 中文
                foreach (TextBox tbx in chtTextBoxs)
                {
                    tbx.Text = String.Empty;
                    tbx.Enabled = enabled;
                }
                #endregion

                #region 英文
                foreach (TextBox tbx in engTextBoxs)
                {
                    tbx.Text = String.Empty;
                    tbx.Enabled = enabled;
                }
                #endregion
            }
            else
            {
                #region 中文
                {
                    int no = 0;
                    foreach (TextBox tbx in chtTextBoxs)
                    {
                        no++;
                        tbx.Text = data.GetMemoTitleChtByNo(no);
                        tbx.Enabled = enabled;
                    }
                }
                #endregion

                #region 英文
                if (isEngEabled)
                {
                    int no = 0;
                    foreach (TextBox tbx in engTextBoxs)
                    {
                        no++;
                        tbx.Text = data.GetMemoTitleEngByNo(no);
                        tbx.Enabled = enabled;
                    }
                }
                else
                {
                    foreach (TextBox tbx in engTextBoxs)
                    {
                        tbx.Text = String.Empty;
                        tbx.Enabled = enabled;
                    }
                }
                #endregion
            }
        }

        private void GetMemoTitleEditData(SchoolRidEntity data, bool isEngEabled)
        {
            TextBox[] chtTextBoxs = null, engTextBoxs = null;
            this.GetMemoTitleTextboxs(out chtTextBoxs, out engTextBoxs);

            #region 中文
            {
                int no = 0;
                foreach (TextBox tbx in chtTextBoxs)
                {
                    no++;
                    data.SetMemoTitleChtByNo(no, tbx.Text.Trim());
                }
            }
            #endregion

            #region 英文
            if (isEngEabled)
            {
                int no = 0;
                foreach (TextBox tbx in engTextBoxs)
                {
                    no++;
                    data.SetMemoTitleEngByNo(no, tbx.Text.Trim());
                }
            }
            else
            {
                for (int no = 1; no <= SchoolRidEntity.MemoTitleMaxCount; no++)
                {
                    data.SetMemoTitleEngByNo(no, null);
                }
            }
            #endregion
        }
        #endregion

        #region 注意事項 (中英文內容) 相關
        private const int BriefCount = 6;

        private void GetBriefTextboxs(out TextBox[] chtTextBoxs, out TextBox[] engTextBoxs)
        {
            chtTextBoxs = new TextBox[BriefCount] {
                this.tbxBriefCht1, this.tbxBriefCht2, this.tbxBriefCht3, this.tbxBriefCht4, this.tbxBriefCht5,
                this.tbxBriefCht6
            };

            engTextBoxs = new TextBox[BriefCount] {
                this.tbxBriefEng1, this.tbxBriefEng2, this.tbxBriefEng3, this.tbxBriefEng4, this.tbxBriefEng5,
                this.tbxBriefEng6
            };
        }

        private void BindBriefEditData(SchoolRidEntity data, bool enabled, bool isEngEabled)
        {
            TextBox[] chtTextBoxs = null, engTextBoxs = null;
            this.GetBriefTextboxs(out chtTextBoxs, out engTextBoxs);

            if (data == null)
            {
                #region 中文
                foreach (TextBox tbx in chtTextBoxs)
                {
                    tbx.Text = String.Empty;
                    tbx.Enabled = enabled;
                }
                #endregion

                #region 英文
                foreach (TextBox tbx in engTextBoxs)
                {
                    tbx.Text = String.Empty;
                    tbx.Enabled = enabled;
                }
                #endregion
            }
            else
            {
                #region 中文
                {
                    int no = 0;
                    foreach (TextBox tbx in chtTextBoxs)
                    {
                        no++;
                        tbx.Text = data.GetBriefChtByNo(no);
                        tbx.Enabled = enabled;
                    }
                }
                #endregion

                #region 英文
                if (isEngEabled)
                {
                    int no = 0;
                    foreach (TextBox tbx in engTextBoxs)
                    {
                        no++;
                        tbx.Text = data.GetBriefEngByNo(no);
                        tbx.Enabled = enabled;
                    }
                }
                else
                {
                    foreach (TextBox tbx in engTextBoxs)
                    {
                        tbx.Text = String.Empty;
                        tbx.Enabled = enabled;
                    }
                }
                #endregion
            }
        }

        private void GetBriefEditData(SchoolRidEntity data, bool isEngEabled)
        {
            TextBox[] chtTextBoxs = null, engTextBoxs = null;
            this.GetBriefTextboxs(out chtTextBoxs, out engTextBoxs);

            #region 中文
            {
                int no = 0;
                foreach (TextBox tbx in chtTextBoxs)
                {
                    no++;
                    data.SetBriefChtByNo(no, tbx.Text.Trim());
                }
            }
            #endregion

            #region 英文
            if (isEngEabled)
            {
                int no = 0;
                foreach (TextBox tbx in engTextBoxs)
                {
                    no++;
                    data.SetBriefEngByNo(no, tbx.Text.Trim());
                }
            }
            else
            {
                for (int no = 1; no <= SchoolRidEntity.BriefMaxCount; no++)
                {
                    data.SetBriefEngByNo(no, null);
                }
            }
            #endregion
        }
        #endregion
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            #region 繳費單模板選項
            {
                Expression whereOr = new Expression(BillFormEntity.Field.BillFormEdition, BillFormEditionCodeTexts.PUBLIC)
                    .Or(BillFormEntity.Field.ReceiveType, this.EditReceiveType);
                Expression where = new Expression(BillFormEntity.Field.BillFormType, BillFormTypeCodeTexts.BILLING)
                    .And(whereOr);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(BillFormEntity.Field.BillFormId, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { BillFormEntity.Field.BillFormId };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { BillFormEntity.Field.BillFormName };
                string textCombineFormat = null;

                CodeText[] items = null;
                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<BillFormEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                }

                #region [MDY:202203XX] 2022擴充案 繳費單中、英文模板
                WebHelper.SetDropDownListItems(this.ddlBillFormChtId, DefaultItem.Kind.Select, false, items, false, true, 0, null);

                WebHelper.SetDropDownListItems(this.ddlBillFormEngId, DefaultItem.Kind.Select, false, items, false, true, 0, null);
                #endregion
            }
            #endregion

            #region 收據模板選項
            {
                Expression whereOr = new Expression(BillFormEntity.Field.BillFormEdition, BillFormEditionCodeTexts.PUBLIC)
                    .Or(BillFormEntity.Field.ReceiveType, this.EditReceiveType);
                Expression where = new Expression(BillFormEntity.Field.BillFormType, BillFormTypeCodeTexts.RECEIPT)
                    .And(whereOr);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(BillFormEntity.Field.BillFormId, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { BillFormEntity.Field.BillFormId };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { BillFormEntity.Field.BillFormName };
                string textCombineFormat = null;

                CodeText[] items = null;
                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<BillFormEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                }

                #region [MDY:202203XX] 2022擴充案 收據中、英文模板
                WebHelper.SetDropDownListItems(this.ddlInvoiceFormChtId, DefaultItem.Kind.Select, false, items, false, true, 0, null);

                WebHelper.SetDropDownListItems(this.ddlInvoiceFormEngId, DefaultItem.Kind.Select, false, items, false, true, 0, null);
                #endregion
            }
            #endregion

            this.tbxSchWord.Text = String.Empty;
            this.tbxPayDate.Text = String.Empty;

            #region 延遲日選項
            this.ddlExtraDays.Items.Clear();
            for (int idx = 0; idx <= 99; idx++)
            {
                string value = idx.ToString();
                this.ddlExtraDays.Items.Add(new ListItem(value, value));
            }
            #endregion

            this.tbxPayDueDate2.Text = String.Empty;
            this.tbxPayDueDate3.Text = String.Empty;

            this.tbxBillOpenDate.Text = String.Empty;
            this.tbxBillCloseDate.Text = String.Empty;
            this.cbxBillCloseDate.Checked = false;

            #region [MDY:2018xxxx] 新增 列印收據關閉日
            this.tbxInvoiceCloseDate.Text = String.Empty;
            #endregion

            #region 機關長官、主辦會計、主辦出納
            this.tbxATitle1.Text = String.Empty;
            this.tbxAName1.Text = String.Empty;
            this.tbxATitle2.Text = String.Empty;
            this.tbxAName2.Text = String.Empty;
            this.tbxATitle3.Text = String.Empty;
            this.tbxAName3.Text = String.Empty;
            #endregion

            #region 學生學分費計算方式選項
            {
                CodeText[] items = new CodeText[] {
                    new CodeText("1", "此業務別碼無學分費之收入科目"),
                    new CodeText("2", "以學分數計算"),
                    new CodeText("3", "以上課時數計算"),
                    new CodeText("4", "以小於某學分數才收學分費"),
                    new CodeText("5", "以小於某上課時數才以上課時數收學分費"),
                    new CodeText("6", "以小於某學分數才以上課時數收學分費")
                };
                WebHelper.SetDropDownListItems(this.ddlStudentType, DefaultItem.Kind.None, false, items, false, true, 0, null);
            }
            #endregion

            this.tbxCreditBasic.Text = String.Empty;
            this.cbxEnabledTax.Checked = false;
            //this.tbxBillOpenDate.Text = String.Empty;
            //this.tbxBillOpenDate.Text = String.Empty;

            #region [OLD:202202XX] 2022擴充案 改寫所以 MARK
            //#region 收入科目、就貸、教育部補助、申報學雜費、申報住宿費
            //{
            //    for (int idx = 0; idx < ReceiveItemCount; idx++)
            //    {
            //        this.SetReceiveItemTextBoxValue(idx, String.Empty);
            //        this.SetLoanItemCheckBoxChecked(idx, false);
            //        this.SetIsSubsidyCheckBoxChecked(idx, false);
            //        this.SetEduTaxCheckBoxChecked(idx, false);
            //        this.SetStayTaxCheckBoxChecked(idx, false);
            //    }
            //}
            //#endregion
            #endregion

            #region [OLD:202202XX] 2022擴充案 備註項目 改寫所以 MARK
            //#region 備註標題
            //{
            //    for (int idx = 0; idx < MemoTitleCount; idx++)
            //    {
            //        this.SetMemoTitleTextBoxValue(idx, String.Empty);
            //    }
            //}
            //#endregion
            #endregion

            #region [MDY:202203XX] 2022擴充案 收入科目的 就貸、教育部補助、申報學雜費、申報住宿費 旗標
            this.BindLoanItemEditData(null, false);
            this.BindIsSubsidyEditData(null, false);
            this.BindEduTaxFlagEditData(null, false);
            this.BindStayTaxFlagEditData(null, false);
            #endregion

            this.tbxCreditItem.Text = String.Empty;
            this.tbxReturnItem.Text = String.Empty;
            this.tbxLoanFee.Text = String.Empty;

            this.rbtnFlagRL0.Checked = false;
            this.rbtnFlagRL1.Checked = true;
            this.rbtnLoanQual1.Checked = true;
            this.rbtnLoanQual2.Checked = false;

            this.tbxReceiveMemo.Text = String.Empty;

            #region [MDY:2018xxxx] 新增 備註不自動換行
            this.cbxReceiveMemoNoWrap.Checked = false;
            #endregion

            #region [OLD:202202XX] 2022擴充案 注意事項 改寫所以 MARK
            //this.tbxBrief1.Text = String.Empty;
            //this.tbxBrief2.Text = String.Empty;
            //this.tbxBrief3.Text = String.Empty;
            //this.tbxBrief4.Text = String.Empty;
            //this.tbxBrief5.Text = String.Empty;
            //this.tbxBrief6.Text = String.Empty;
            #endregion

            this.ccbtnOK.Visible = true;

            #region [MDY:202203XX] 2022擴充案 含英文相關欄位
            bool isEngEnabled = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);

            this.ChangeEngPlaceHolders(isEngEnabled);

            #region 收入科目 (中英文名稱)
            this.BindReceiveItemEditData(null, false, isEngEnabled);
            #endregion

            #region 備註項目 (中英文標題)
            this.BindMemoTitleEditData(null, false, isEngEnabled);
            #endregion

            #region 注意事項 (中英文內容)
            this.BindBriefEditData(null, false, isEngEnabled);
            #endregion
            #endregion
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(SchoolRidEntity data)
        {
            this.InitialUI();

            if (data == null)
            {
                this.ccbtnOK.Visible = false;
                return;
            }

            this.EditSchoolRid = data;

            bool enabled = (this.Action == ActionMode.Insert || this.Action == ActionMode.Modify);

            #region [MDY:202203XX] 2022擴充案 繳費單中、英文模板
            WebHelper.SetDropDownListSelectedValue(this.ddlBillFormChtId, data.BillformId);
            WebHelper.SetDropDownListSelectedValue(this.ddlBillFormEngId, data.BillFormEId);
            this.ddlBillFormChtId.Enabled = enabled;
            this.ddlBillFormEngId.Enabled = enabled;
            #endregion

            #region [MDY:202203XX] 2022擴充案 收據中、英文模板
            WebHelper.SetDropDownListSelectedValue(this.ddlInvoiceFormChtId, data.InvoiceformId);
            WebHelper.SetDropDownListSelectedValue(this.ddlInvoiceFormEngId, data.InvoiceFormEId);
            this.ddlInvoiceFormChtId.Enabled = enabled;
            this.ddlInvoiceFormEngId.Enabled = enabled;
            #endregion

            this.tbxSchWord.Text = data.SchWord;
            this.tbxSchWord.Enabled = enabled;

            this.tbxPayDate.Text = DataFormat.ConvertTWDate7ToDate(data.PayDate).Trim();
            this.tbxPayDate.Enabled = enabled;

            WebHelper.SetDropDownListSelectedValue(this.ddlExtraDays, data.ExtraDays.ToString());
            this.ddlExtraDays.Enabled = enabled;

            this.tbxPayDueDate2.Text = DataFormat.ConvertTWDate7ToDate(data.PayDueDate2).Trim();
            this.tbxPayDueDate2.Enabled = enabled;
            this.tbxPayDueDate3.Text = DataFormat.ConvertTWDate7ToDate(data.PayDueDate3).Trim();
            this.tbxPayDueDate3.Enabled = enabled;

            this.tbxBillOpenDate.Text = DataFormat.GetDate8Text(data.BillValidDate);
            this.tbxBillCloseDate.Text = DataFormat.GetDate8Text(data.BillCloseDate);
            this.cbxBillCloseDate.Checked = (!String.IsNullOrWhiteSpace(data.BillCloseDate));
            this.tbxBillOpenDate.Enabled = enabled;
            this.tbxBillCloseDate.Enabled = enabled;
            this.cbxBillCloseDate.Enabled = enabled;

            #region [MDY:2018xxxx] 新增 列印收據關閉日
            this.tbxInvoiceCloseDate.Text = DataFormat.GetDate8Text(data.InvoiceCloseDate);
            this.tbxInvoiceCloseDate.Enabled = enabled;
            #endregion

            #region 機關長官、主辦會計、主辦出納
            this.tbxATitle1.Text = data.ATitle1;
            this.tbxAName1.Text = data.AName1;
            this.tbxATitle2.Text = data.ATitle2;
            this.tbxAName2.Text = data.AName2;
            this.tbxATitle3.Text = data.ATitle3;
            this.tbxAName3.Text = data.AName3;
            this.tbxATitle1.Enabled = enabled;
            this.tbxAName1.Enabled = enabled;
            this.tbxATitle2.Enabled = enabled;
            this.tbxAName2.Enabled = enabled;
            this.tbxATitle3.Enabled = enabled;
            this.tbxAName3.Enabled = enabled;
            #endregion

            WebHelper.SetDropDownListSelectedValue(this.ddlStudentType, data.StudentType);
            this.ddlStudentType.Enabled = enabled;

            this.tbxCreditBasic.Text = data.CreditBasic == null ? string.Empty : data.CreditBasic.Value.ToString();
            this.cbxEnabledTax.Checked = data.EnabledTax == "Y";
            this.tbxCreditBasic.Enabled = enabled;
            this.cbxEnabledTax.Enabled = enabled;

            #region [OLD:202202XX] 2022擴充案 改寫所以 MARK
            //#region 收入科目
            //{
            //    string[] receiveItems = data.GetAllReceiveItems();
            //    for (int idx = 0; idx < receiveItems.Length; idx++)
            //    {
            //        TextBox tbx = this.GetReceiveItemTextBox(idx);
            //        if (tbx != null)
            //        {
            //            tbx.Text = receiveItems[idx];
            //            tbx.Enabled = enabled;
            //        }
            //    }
            //}
            //#endregion

            //#region 就貸
            //{
            //    string[] loanItemValues = data.GetAllLoanItems();
            //    for (int idx = 0; idx < loanItemValues.Length; idx++)
            //    {
            //        CheckBox cbx = this.GetLoanItemCheckBox(idx);
            //        if (cbx != null)
            //        {
            //            cbx.Checked = (loanItemValues[idx] == "Y");
            //            cbx.Enabled = enabled;
            //        }
            //    }
            //}
            //#endregion

            //#region 教育部補助
            //{
            //    string[] isSubsidyValues = data.GetAllIsSubsidys ();
            //    for (int idx = 0; idx < isSubsidyValues.Length; idx++)
            //    {
            //        CheckBox cbx = this.GetIsSubsidyCheckBox(idx);
            //        if (cbx != null)
            //        {
            //            cbx.Checked = (isSubsidyValues[idx] == "Y");
            //            cbx.Enabled = enabled;
            //        }
            //    }
            //}
            //#endregion

            //#region 申報學雜費
            //{
            //    string[] eduTaxFlags = data.GetAllEduTaxFlags();
            //    for (int idx = 0; idx < eduTaxFlags.Length; idx++)
            //    {
            //        CheckBox cbx = this.GetEduTaxCheckBox(idx);
            //        if (cbx != null)
            //        {
            //            cbx.Checked = (eduTaxFlags[idx] == "Y");
            //            cbx.Enabled = enabled;
            //        }
            //    }
            //}
            //#endregion

            //#region 申報住宿費
            //{
            //    string[] stayTaxFlags = data.GetAllStayTaxFlags();
            //    for (int idx = 0; idx < stayTaxFlags.Length; idx++)
            //    {
            //        CheckBox cbx = this.GetStayTaxCheckBox(idx);
            //        if (cbx != null)
            //        {
            //            cbx.Checked = (stayTaxFlags[idx] == "Y");
            //            cbx.Enabled = enabled;
            //        }
            //    }
            //}
            //#endregion
            #endregion

            #region [OLD:202202XX] 2022擴充案 備註項目 改寫所以 MARK
            //#region 備註標題
            //{
            //    string[] memoTitles = data.GetAllMemoTitles();
            //    for (int idx = 0; idx < memoTitles.Length; idx++)
            //    {
            //        TextBox tbx = this.GetMemoTitleTextBox(idx);
            //        if (tbx != null)
            //        {
            //            tbx.Text = memoTitles[idx];
            //            tbx.Enabled = enabled;
            //        }
            //    }
            //}
            //#endregion
            #endregion

            #region [MDY:202203XX] 2022擴充案 收入科目的 就貸、教育部補助、申報學雜費、申報住宿費 旗標
            this.BindLoanItemEditData(data, enabled);
            this.BindIsSubsidyEditData(data, enabled);
            this.BindEduTaxFlagEditData(data, enabled);
            this.BindStayTaxFlagEditData(data, enabled);
            #endregion

            this.tbxCreditItem.Text = data.CreditItem;
            this.tbxReturnItem.Text = data.ReturnItem;
            this.tbxLoanFee.Text = data.LoanFee == null ? string.Empty : data.LoanFee.Value.ToString("0");
            this.rbtnFlagRL0.Checked = data.FlagRl == "0";
            this.rbtnFlagRL1.Checked = data.FlagRl == "1";
            this.rbtnLoanQual1.Checked = data.LoanQual == "1";
            this.rbtnLoanQual2.Checked = data.LoanQual == "2";
            this.tbxCreditItem.Enabled = enabled;
            this.tbxReturnItem.Enabled = enabled;
            this.tbxLoanFee.Enabled = enabled;
            this.rbtnFlagRL0.Enabled = enabled;
            this.rbtnFlagRL1.Enabled = enabled;
            this.rbtnLoanQual1.Enabled = enabled;
            this.rbtnLoanQual2.Enabled = enabled;

            this.tbxReceiveMemo.Text = data.ReceiveMemo;
            this.tbxReceiveMemo.Enabled = enabled;

            #region [MDY:2018xxxx] 新增 備註不自動換行
            this.cbxReceiveMemoNoWrap.Checked = ("Y".Equals(data.ReceiveMemoNoWrap, StringComparison.CurrentCultureIgnoreCase));
            this.cbxReceiveMemoNoWrap.Enabled = enabled;
            #endregion

            #region [OLD:202202XX] 2022擴充案 注意事項 改寫所以 MARK
            //this.tbxBrief1.Text = data.Brief1;
            //this.tbxBrief2.Text = data.Brief2;
            //this.tbxBrief3.Text = data.Brief3;
            //this.tbxBrief4.Text = data.Brief4;
            //this.tbxBrief5.Text = data.Brief5;
            //this.tbxBrief6.Text = data.Brief6;
            //this.tbxBrief1.Enabled = enabled;
            //this.tbxBrief2.Enabled = enabled;
            //this.tbxBrief3.Enabled = enabled;
            //this.tbxBrief4.Enabled = enabled;
            //this.tbxBrief5.Enabled = enabled;
            //this.tbxBrief6.Enabled = enabled;
            #endregion

            this.ccbtnOK.Visible = true;

            #region [MDY:202203XX] 2022擴充案 含英文相關欄位
            bool isEngEnabled = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);

            this.ChangeEngPlaceHolders(isEngEnabled);

            #region 收入科目 (中英文名稱)
            this.BindReceiveItemEditData(data, enabled, isEngEnabled);
            #endregion

            #region 備註項目 (中英文標題)
            this.BindMemoTitleEditData(data, enabled, isEngEnabled);
            #endregion

            #region 注意事項 (中英文內容)
            this.BindBriefEditData(data, enabled, isEngEnabled);
            #endregion
            #endregion
        }

        /// <summary>
        /// 取得輸入的維護資料
        /// </summary>
        /// <returns>傳回輸入的維護資料</returns>
        private SchoolRidEntity GetEditData()
        {
            SchoolRidEntity data = this.EditSchoolRid;

            if (this.Action != ActionMode.Insert && this.Action != ActionMode.Modify)
            {
                return data;
            }

            #region [MDY:202203XX] 2022擴充案 改寫所以 MARK
            //data.BillformId = this.ddlBillFormChtId.SelectedValue;
            //data.InvoiceformId = this.ddlInvoiceFormChtId.SelectedValue;
            #endregion

            data.SchWord = this.tbxSchWord.Text.Trim();

            data.PayDate = DataFormat.ConvertDateToTWDate7(this.tbxPayDate.Text.Trim(), true);

            data.ExtraDays = int.Parse(this.ddlExtraDays.SelectedValue);

            data.PayDueDate2 = DataFormat.ConvertDateToTWDate7(this.tbxPayDueDate2.Text.Trim(), true);
            data.PayDueDate3 = DataFormat.ConvertDateToTWDate7(this.tbxPayDueDate3.Text.Trim(), true);

            data.BillValidDate = DataFormat.ConvertDateToDate8(this.tbxBillOpenDate.Text.Trim(), true);
            if (this.cbxBillCloseDate.Checked)
            {
                data.BillCloseDate = DataFormat.ConvertDateToDate8(this.tbxBillCloseDate.Text.Trim(), true);
            }
            else
            {
                data.BillCloseDate = String.Empty;
            }

            #region [MDY:2018xxxx] 新增 列印收據關閉日
            data.InvoiceCloseDate = DataFormat.ConvertDateToDate8(this.tbxInvoiceCloseDate.Text.Trim(), true);
            #endregion

            #region 機關長官、主辦會計、主辦出納
            data.ATitle1 = this.tbxATitle1.Text.Trim();
            data.AName1 = this.tbxAName1.Text.Trim();
            data.ATitle2 = this.tbxATitle2.Text.Trim();
            data.AName2 = this.tbxAName2.Text.Trim();
            data.ATitle3 = this.tbxATitle3.Text.Trim();
            data.AName3 = this.tbxAName3.Text.Trim();
            #endregion

            data.StudentType = this.ddlStudentType.SelectedValue;

            int creditBasic = 0;
            if (int.TryParse(this.tbxCreditBasic.Text.Trim(), out creditBasic))
            {
                data.CreditBasic = creditBasic;
            }
            else
            {
                data.CreditBasic = null;
            }

            data.EnabledTax = this.cbxEnabledTax.Checked ? "Y" : "N";

            #region [OLD:202202XX] 2022擴充案 改寫所以 MARK
            //#region 收入科目
            //{
            //    data.ReceiveItem01 = this.GetReceiveItemTextBoxValue(0);
            //    data.ReceiveItem02 = this.GetReceiveItemTextBoxValue(1);
            //    data.ReceiveItem03 = this.GetReceiveItemTextBoxValue(2);
            //    data.ReceiveItem04 = this.GetReceiveItemTextBoxValue(3);
            //    data.ReceiveItem05 = this.GetReceiveItemTextBoxValue(4);
            //    data.ReceiveItem06 = this.GetReceiveItemTextBoxValue(5);
            //    data.ReceiveItem07 = this.GetReceiveItemTextBoxValue(6);
            //    data.ReceiveItem08 = this.GetReceiveItemTextBoxValue(7);
            //    data.ReceiveItem09 = this.GetReceiveItemTextBoxValue(8);
            //    data.ReceiveItem10 = this.GetReceiveItemTextBoxValue(9);
            //    data.ReceiveItem11 = this.GetReceiveItemTextBoxValue(10);
            //    data.ReceiveItem12 = this.GetReceiveItemTextBoxValue(11);
            //    data.ReceiveItem13 = this.GetReceiveItemTextBoxValue(12);
            //    data.ReceiveItem14 = this.GetReceiveItemTextBoxValue(13);
            //    data.ReceiveItem15 = this.GetReceiveItemTextBoxValue(14);
            //    data.ReceiveItem16 = this.GetReceiveItemTextBoxValue(15);

            //    data.ReceiveItem17 = this.GetReceiveItemTextBoxValue(16);
            //    data.ReceiveItem18 = this.GetReceiveItemTextBoxValue(17);
            //    data.ReceiveItem19 = this.GetReceiveItemTextBoxValue(18);
            //    data.ReceiveItem20 = this.GetReceiveItemTextBoxValue(19);
            //    data.ReceiveItem21 = this.GetReceiveItemTextBoxValue(20);
            //    data.ReceiveItem22 = this.GetReceiveItemTextBoxValue(21);
            //    data.ReceiveItem23 = this.GetReceiveItemTextBoxValue(22);
            //    data.ReceiveItem24 = this.GetReceiveItemTextBoxValue(23);
            //    data.ReceiveItem25 = this.GetReceiveItemTextBoxValue(24);
            //    data.ReceiveItem26 = this.GetReceiveItemTextBoxValue(25);
            //    data.ReceiveItem27 = this.GetReceiveItemTextBoxValue(26);
            //    data.ReceiveItem28 = this.GetReceiveItemTextBoxValue(27);
            //    data.ReceiveItem29 = this.GetReceiveItemTextBoxValue(28);
            //    data.ReceiveItem30 = this.GetReceiveItemTextBoxValue(29);

            //    data.ReceiveItem31 = this.GetReceiveItemTextBoxValue(30);
            //    data.ReceiveItem32 = this.GetReceiveItemTextBoxValue(31);
            //    data.ReceiveItem33 = this.GetReceiveItemTextBoxValue(32);
            //    data.ReceiveItem34 = this.GetReceiveItemTextBoxValue(33);
            //    data.ReceiveItem35 = this.GetReceiveItemTextBoxValue(34);
            //    data.ReceiveItem36 = this.GetReceiveItemTextBoxValue(35);
            //    data.ReceiveItem37 = this.GetReceiveItemTextBoxValue(36);
            //    data.ReceiveItem38 = this.GetReceiveItemTextBoxValue(37);
            //    data.ReceiveItem39 = this.GetReceiveItemTextBoxValue(38);
            //    data.ReceiveItem40 = this.GetReceiveItemTextBoxValue(39);
            //}
            //#endregion

            //#region 就貸
            //{
            //    data.LoanItem01 = this.GetLoanItemCheckBoxChecked(0) ? "Y" : "N";
            //    data.LoanItem02 = this.GetLoanItemCheckBoxChecked(1) ? "Y" : "N";
            //    data.LoanItem03 = this.GetLoanItemCheckBoxChecked(2) ? "Y" : "N";
            //    data.LoanItem04 = this.GetLoanItemCheckBoxChecked(3) ? "Y" : "N";
            //    data.LoanItem05 = this.GetLoanItemCheckBoxChecked(4) ? "Y" : "N";
            //    data.LoanItem06 = this.GetLoanItemCheckBoxChecked(5) ? "Y" : "N";
            //    data.LoanItem07 = this.GetLoanItemCheckBoxChecked(6) ? "Y" : "N";
            //    data.LoanItem08 = this.GetLoanItemCheckBoxChecked(7) ? "Y" : "N";
            //    data.LoanItem09 = this.GetLoanItemCheckBoxChecked(8) ? "Y" : "N";
            //    data.LoanItem10 = this.GetLoanItemCheckBoxChecked(9) ? "Y" : "N";
            //    data.LoanItem11 = this.GetLoanItemCheckBoxChecked(10) ? "Y" : "N";
            //    data.LoanItem12 = this.GetLoanItemCheckBoxChecked(11) ? "Y" : "N";
            //    data.LoanItem13 = this.GetLoanItemCheckBoxChecked(12) ? "Y" : "N";
            //    data.LoanItem14 = this.GetLoanItemCheckBoxChecked(13) ? "Y" : "N";
            //    data.LoanItem15 = this.GetLoanItemCheckBoxChecked(14) ? "Y" : "N";
            //    data.LoanItem16 = this.GetLoanItemCheckBoxChecked(15) ? "Y" : "N";

            //    data.LoanItem17 = this.GetLoanItemCheckBoxChecked(16) ? "Y" : "N";
            //    data.LoanItem18 = this.GetLoanItemCheckBoxChecked(17) ? "Y" : "N";
            //    data.LoanItem19 = this.GetLoanItemCheckBoxChecked(18) ? "Y" : "N";
            //    data.LoanItem20 = this.GetLoanItemCheckBoxChecked(19) ? "Y" : "N";
            //    data.LoanItem21 = this.GetLoanItemCheckBoxChecked(20) ? "Y" : "N";
            //    data.LoanItem22 = this.GetLoanItemCheckBoxChecked(21) ? "Y" : "N";
            //    data.LoanItem23 = this.GetLoanItemCheckBoxChecked(22) ? "Y" : "N";
            //    data.LoanItem24 = this.GetLoanItemCheckBoxChecked(23) ? "Y" : "N";
            //    data.LoanItem25 = this.GetLoanItemCheckBoxChecked(24) ? "Y" : "N";
            //    data.LoanItem26 = this.GetLoanItemCheckBoxChecked(25) ? "Y" : "N";
            //    data.LoanItem27 = this.GetLoanItemCheckBoxChecked(26) ? "Y" : "N";
            //    data.LoanItem28 = this.GetLoanItemCheckBoxChecked(27) ? "Y" : "N";
            //    data.LoanItem29 = this.GetLoanItemCheckBoxChecked(28) ? "Y" : "N";
            //    data.LoanItem30 = this.GetLoanItemCheckBoxChecked(29) ? "Y" : "N";
            //    data.LoanItem31 = this.GetLoanItemCheckBoxChecked(30) ? "Y" : "N";
            //    data.LoanItem32 = this.GetLoanItemCheckBoxChecked(31) ? "Y" : "N";
            //    data.LoanItem33 = this.GetLoanItemCheckBoxChecked(32) ? "Y" : "N";
            //    data.LoanItem34 = this.GetLoanItemCheckBoxChecked(33) ? "Y" : "N";
            //    data.LoanItem35 = this.GetLoanItemCheckBoxChecked(34) ? "Y" : "N";
            //    data.LoanItem36 = this.GetLoanItemCheckBoxChecked(35) ? "Y" : "N";
            //    data.LoanItem37 = this.GetLoanItemCheckBoxChecked(36) ? "Y" : "N";
            //    data.LoanItem38 = this.GetLoanItemCheckBoxChecked(37) ? "Y" : "N";
            //    data.LoanItem39 = this.GetLoanItemCheckBoxChecked(38) ? "Y" : "N";
            //    data.LoanItem40 = this.GetLoanItemCheckBoxChecked(39) ? "Y" : "N";

            //    string[] others = new string[OtherItemCount];
            //    for (int idx = 0; idx < others.Length; idx++)
            //    {
            //        others[idx] = this.GetLoanItemCheckBoxChecked(16 + idx) ? "Y" : "N";
            //    }
            //    data.LoanItemOthers = String.Join("", others);
            //}
            //#endregion

            //#region 教育部補助
            //{
            //    data.Issubsidy01 = this.GetIsSubsidyCheckBoxChecked(00) ? "Y" : "N";
            //    data.Issubsidy02 = this.GetIsSubsidyCheckBoxChecked(01) ? "Y" : "N";
            //    data.Issubsidy03 = this.GetIsSubsidyCheckBoxChecked(02) ? "Y" : "N";
            //    data.Issubsidy04 = this.GetIsSubsidyCheckBoxChecked(03) ? "Y" : "N";
            //    data.Issubsidy05 = this.GetIsSubsidyCheckBoxChecked(04) ? "Y" : "N";
            //    data.Issubsidy06 = this.GetIsSubsidyCheckBoxChecked(05) ? "Y" : "N";
            //    data.Issubsidy07 = this.GetIsSubsidyCheckBoxChecked(06) ? "Y" : "N";
            //    data.Issubsidy08 = this.GetIsSubsidyCheckBoxChecked(07) ? "Y" : "N";
            //    data.Issubsidy09 = this.GetIsSubsidyCheckBoxChecked(08) ? "Y" : "N";
            //    data.Issubsidy10 = this.GetIsSubsidyCheckBoxChecked(09) ? "Y" : "N";
            //    data.Issubsidy11 = this.GetIsSubsidyCheckBoxChecked(10) ? "Y" : "N";
            //    data.Issubsidy12 = this.GetIsSubsidyCheckBoxChecked(11) ? "Y" : "N";
            //    data.Issubsidy13 = this.GetIsSubsidyCheckBoxChecked(12) ? "Y" : "N";
            //    data.Issubsidy14 = this.GetIsSubsidyCheckBoxChecked(13) ? "Y" : "N";
            //    data.Issubsidy15 = this.GetIsSubsidyCheckBoxChecked(14) ? "Y" : "N";
            //    data.Issubsidy16 = this.GetIsSubsidyCheckBoxChecked(15) ? "Y" : "N";

            //    data.Issubsidy17 = this.GetIsSubsidyCheckBoxChecked(16) ? "Y" : "N";
            //    data.Issubsidy18 = this.GetIsSubsidyCheckBoxChecked(17) ? "Y" : "N";
            //    data.Issubsidy19 = this.GetIsSubsidyCheckBoxChecked(18) ? "Y" : "N";
            //    data.Issubsidy20 = this.GetIsSubsidyCheckBoxChecked(19) ? "Y" : "N";
            //    data.Issubsidy21 = this.GetIsSubsidyCheckBoxChecked(20) ? "Y" : "N";
            //    data.Issubsidy22 = this.GetIsSubsidyCheckBoxChecked(21) ? "Y" : "N";
            //    data.Issubsidy23 = this.GetIsSubsidyCheckBoxChecked(22) ? "Y" : "N";
            //    data.Issubsidy24 = this.GetIsSubsidyCheckBoxChecked(23) ? "Y" : "N";
            //    data.Issubsidy25 = this.GetIsSubsidyCheckBoxChecked(24) ? "Y" : "N";
            //    data.Issubsidy26 = this.GetIsSubsidyCheckBoxChecked(25) ? "Y" : "N";
            //    data.Issubsidy27 = this.GetIsSubsidyCheckBoxChecked(26) ? "Y" : "N";
            //    data.Issubsidy28 = this.GetIsSubsidyCheckBoxChecked(27) ? "Y" : "N";
            //    data.Issubsidy29 = this.GetIsSubsidyCheckBoxChecked(28) ? "Y" : "N";
            //    data.Issubsidy30 = this.GetIsSubsidyCheckBoxChecked(29) ? "Y" : "N";
            //    data.Issubsidy31 = this.GetIsSubsidyCheckBoxChecked(30) ? "Y" : "N";
            //    data.Issubsidy32 = this.GetIsSubsidyCheckBoxChecked(31) ? "Y" : "N";
            //    data.Issubsidy33 = this.GetIsSubsidyCheckBoxChecked(32) ? "Y" : "N";
            //    data.Issubsidy34 = this.GetIsSubsidyCheckBoxChecked(33) ? "Y" : "N";
            //    data.Issubsidy35 = this.GetIsSubsidyCheckBoxChecked(34) ? "Y" : "N";
            //    data.Issubsidy36 = this.GetIsSubsidyCheckBoxChecked(35) ? "Y" : "N";
            //    data.Issubsidy37 = this.GetIsSubsidyCheckBoxChecked(36) ? "Y" : "N";
            //    data.Issubsidy38 = this.GetIsSubsidyCheckBoxChecked(37) ? "Y" : "N";
            //    data.Issubsidy39 = this.GetIsSubsidyCheckBoxChecked(38) ? "Y" : "N";
            //    data.Issubsidy40 = this.GetIsSubsidyCheckBoxChecked(39) ? "Y" : "N";

            //    string[] others = new string[OtherItemCount];
            //    for (int idx = 0; idx < others.Length; idx++)
            //    {
            //        others[idx] = this.GetIsSubsidyCheckBoxChecked(16 + idx) ? "Y" : "N";
            //    }
            //    data.IssubsidyOthers = String.Join("", others);
            //}
            //#endregion

            //#region 申報學雜費
            //{
            //    string[] eduTaxFlags = new string[ReceiveItemCount];
            //    for (int idx = 0; idx < eduTaxFlags.Length; idx++)
            //    {
            //        eduTaxFlags[idx] = this.GetEduTaxCheckBoxChecked(idx) ? "Y" : "N";
            //    }
            //    data.EduTax = String.Join("", eduTaxFlags);
            //}
            //#endregion

            //#region 申報住宿費
            //{
            //    string[] stayTaxFlags = new string[ReceiveItemCount];
            //    for (int idx = 0; idx < stayTaxFlags.Length; idx++)
            //    {
            //        stayTaxFlags[idx] = this.GetStayTaxCheckBoxChecked(idx) ? "Y" : "N";
            //    }
            //    data.StayTax = String.Join("", stayTaxFlags);
            //}
            //#endregion
            #endregion

            #region [OLD:202202XX] 2022擴充案 備註項目 改寫所以 MARK
            //#region 備註標題
            //{
            //    for (int no = 1; no <= MemoTitleCount; no++)
            //    {
            //        data.SetMemoTitle(no, this.GetMemoTitleTextBoxValue(no - 1));
            //    }
            //}
            //#endregion
            #endregion

            #region [MDY:202203XX] 2022擴充案 收入科目的 就貸、教育部補助、申報學雜費、申報住宿費 旗標
            this.GetLoanItemEditData(data);
            this.GetIsSubsidyEditData(data);
            this.GetEduTaxFlagEditData(data);
            this.GetStayTaxFlagEditData(data);
            #endregion

            data.CreditItem = this.tbxCreditItem.Text.Trim();
            data.ReturnItem = this.tbxReturnItem.Text.Trim();

            decimal loanFee = 0;
            if (Decimal.TryParse(this.tbxLoanFee.Text.Trim(), out loanFee))
            {
                data.LoanFee = loanFee;
            }
            else
            {
                data.LoanFee = null;
            }

            if (this.rbtnFlagRL0.Checked)
            {
                data.FlagRl = "0";
            }
            else if (this.rbtnFlagRL1.Checked)
            {
                data.FlagRl = "1";
            }
            else
            {
                data.FlagRl = String.Empty;
            }

            if (this.rbtnLoanQual1.Checked)
            {
                data.LoanQual = "1";
            }
            else if (this.rbtnLoanQual2.Checked)
            {
                data.LoanQual = "2";
            }
            else
            {
                data.LoanQual = String.Empty;
            }

            #region 備註
            {
                //string memo = this.tbxReceiveMemo.Text.Trim();
                //if (!String.IsNullOrEmpty(memo))
                //{
                //    System.Text.StringBuilder text = new System.Text.StringBuilder();
                //    string[] lines = memo.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                //    for (int idx = 0; idx < lines.Length; idx++)
                //    {
                //        string line = lines[idx].Trim();
                //        if (line.Length > 24)
                //        {
                //            int startIndex = 0;
                //            while (line.Length > startIndex + 24)
                //            {
                //                int length = line.Length - startIndex;
                //                if (length > 24)
                //                {
                //                    text.AppendLine(line.Substring(startIndex, 24));
                //                }
                //                else
                //                {
                //                    text.AppendLine(line.Substring(startIndex, length));
                //                }
                //                startIndex += 24;
                //            }
                //            text.AppendLine(line.Substring(startIndex));
                //        }
                //        else
                //        {
                //            text.AppendLine(line);
                //        }
                //    }
                //    data.ReceiveMemo = text.ToString(0, text.Length - 2);   //去掉最後的換行符號 0x0D, 0x0A
                //}
                //else
                //{
                //    data.ReceiveMemo = String.Empty;
                //}
                data.ReceiveMemo = this.tbxReceiveMemo.Text.Trim();
            }
            #endregion

            #region [MDY:2018xxxx] 新增 備註不自動換行
            data.ReceiveMemoNoWrap = this.cbxReceiveMemoNoWrap.Checked ? "Y" : "N";
            #endregion

            #region [OLD:202202XX] 2022擴充案 注意事項 改寫所以 MARK
            //data.Brief1 = this.tbxBrief1.Text.Trim();
            //data.Brief2 = this.tbxBrief2.Text.Trim();
            //data.Brief3 = this.tbxBrief3.Text.Trim();
            //data.Brief4 = this.tbxBrief4.Text.Trim();
            //data.Brief5 = this.tbxBrief5.Text.Trim();
            //data.Brief6 = this.tbxBrief6.Text.Trim();
            #endregion

            #region [MDY:202203XX] 2022擴充案 含英文相關欄位
            bool isEngEabled = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);

            #region 繳費單中、英文模板
            data.BillformId = this.ddlBillFormChtId.SelectedValue;
            if (isEngEabled)
            {
                data.BillFormEId = this.ddlBillFormEngId.SelectedValue;
            }
            else
            {
                data.BillFormEId = String.Empty;
            }
            #endregion

            #region [MDY:202203XX] 收據中、英文模板
            data.InvoiceformId = this.ddlInvoiceFormChtId.SelectedValue;
            if (isEngEabled)
            {
                data.InvoiceFormEId = this.ddlInvoiceFormEngId.SelectedValue;
            }
            else
            {
                data.InvoiceFormEId = String.Empty;
            }
            #endregion

            #region 收入科目 (中英文名稱)
            this.GetReceiveItemEditData(data, isEngEabled);
            #endregion

            #region 備註項目 (中英文標題)
            this.GetMemoTitleEditData(data, isEngEabled);
            #endregion

            #region 注意事項 (中英文內容)
            this.GetBriefEditData(data, isEngEabled);
            #endregion
            #endregion

            return data;
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData(SchoolRidEntity data)
        {
            #region 繳費單模板
            if (String.IsNullOrEmpty(data.BillformId))
            {
                this.ShowMustInputAlert("繳費單模板");
                return false;
            }
            #endregion

            #region 收據模板
            //if (String.IsNullOrEmpty(data.BillFormId))
            //{
            //    this.ShowMustInputAlert("收據模板");
            //    return false;
            //}
            #endregion

            #region 繳款期限
            if (String.IsNullOrEmpty(data.PayDate))
            {
                if (!String.IsNullOrWhiteSpace(this.tbxPayDate.Text))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「繳款期限」不是合法的日期");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                // [TODO] 如果為必要欄位，這裡的 Mark 移除
                //else
                //{
                //    this.ShowMustInputAlert("繳款期限");
                //    return false;
                //}
            }
            #endregion

            #region 信用卡繳款期限
            if (String.IsNullOrEmpty(data.PayDueDate2))
            {
                if (!String.IsNullOrWhiteSpace(this.tbxPayDueDate2.Text))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「信用卡繳款期限」不是合法的日期");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                // [TODO] 如果為必要欄位，這裡的 Mark 移除
                //else
                //{
                //    this.ShowMustInputAlert("繳款期限");
                //    return false;
                //}
            }
            #endregion

            #region 財金繳款期限
            if (String.IsNullOrEmpty(data.PayDueDate3))
            {
                if (!String.IsNullOrWhiteSpace(this.tbxPayDueDate3.Text))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「財金繳款期限」不是合法的日期");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                // [TODO] 如果為必要欄位，這裡的 Mark 移除
                //else
                //{
                //    this.ShowMustInputAlert("繳款期限");
                //    return false;
                //}
            }
            #endregion

            #region 開放列印日期
            DateTime? openDate = null;
            if (String.IsNullOrEmpty(data.BillValidDate))
            {
                if (!String.IsNullOrWhiteSpace(this.tbxBillOpenDate.Text))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「開放列印日期」不是合法的日期");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                // [TODO] 如果為必要欄位，這裡的 Mark 移除
                //else
                //{
                //    this.ShowMustInputAlert("開放列印日期");
                //    return false;
                //}
            }
            else
            {
                openDate = DataFormat.ConvertDateText(data.BillValidDate);
            }
            #endregion

            #region 關閉列印日期
            DateTime? closeDate = null;
            if (String.IsNullOrEmpty(data.BillCloseDate) && this.cbxBillCloseDate.Checked)
            {
                if (!String.IsNullOrWhiteSpace(this.tbxBillCloseDate.Text))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「關閉列印日期」不是合法的日期");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                else
                {
                    this.ShowMustInputAlert("關閉列印日期");
                    return false;
                }
            }
            else
            {
                closeDate = DataFormat.ConvertDateText(data.BillValidDate);
            }
            #endregion

            #region 關閉列印日期 不可 小於 開放列印日期
            #region [Old]
            //if (!String.IsNullOrEmpty(data.BillValidDate) && !String.IsNullOrEmpty(data.BillCloseDate))
            //{
            //    DateTime today = DateTime.Now;
            //    DateTime? opendate = DataFormat.ConvertDateText(data.BillValidDate);
            //    DateTime? closedate = DataFormat.ConvertDateText(data.BillCloseDate);
            //    if (opendate != null && closedate != null)
            //    {
            //        if (new TimeSpan(today.Ticks - ((DateTime)opendate).Ticks).Days > 0)
            //        {
            //            //[TODO] 固定顯示訊息的收集
            //            string msg = this.GetLocalized("「開放列印日期」不可小於今日");
            //            this.ShowSystemMessage(msg);
            //            return false;
            //        }
            //        if (new TimeSpan(today.Ticks - ((DateTime)closedate).Ticks).Days > 0)
            //        {
            //            //[TODO] 固定顯示訊息的收集
            //            string msg = this.GetLocalized("「關閉列印日期」不可小於今日");
            //            this.ShowSystemMessage(msg);
            //            return false;
            //        }
            //        if (new TimeSpan(((DateTime)opendate).Ticks - ((DateTime)closedate).Ticks).Days > 0)
            //        {
            //            //[TODO] 固定顯示訊息的收集
            //            string msg = this.GetLocalized("「關閉列印日期」不可小於「開放列印日期」");
            //            this.ShowSystemMessage(msg);
            //            return false;
            //        }
            //    }
            //}
            #endregion
            if (openDate != null && closeDate != null && openDate.Value > closeDate.Value)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「關閉列印日期」不可小於「開放列印日期」");
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion

            #region [MDY:2018xxxx] 新增 列印收據關閉日
            if (String.IsNullOrEmpty(data.InvoiceCloseDate))
            {
                if (!String.IsNullOrWhiteSpace(this.tbxInvoiceCloseDate.Text))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「列印收據關閉日」不是合法的日期");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                // [TODO] 如果為必要欄位，這裡的 Mark 移除
                //else
                //{
                //    this.ShowMustInputAlert("列印收據關閉日");
                //    return false;
                //}
            }
            #endregion

            #region 學生學分費計算方式
            if (String.IsNullOrEmpty(data.StudentType))
            {
                this.ShowMustInputAlert("學生學分費計算方式");
                return false;
            }
            #endregion

            #region 學分費比較基準
            if (data.CreditBasic == null)
            {
                if (!String.IsNullOrWhiteSpace(this.tbxCreditBasic.Text))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「學分費比較基準」不是大於0的整數數值");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                // [TODO] 如果為必要欄位，這裡的 Mark 移除
                //else
                //{
                //    this.ShowMustInputAlert("學分費比較基準");
                //    return false;
                //}
            }
            else
            {
                // [TODO] 如果有其他條件檢查寫在這裡
                int creditBasic = data.CreditBasic.Value;
                if (creditBasic < 0)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「學分費比較基準」不是大於0的整數數值");
                    this.ShowSystemMessage(msg);
                    return false;
                }
            }
            #endregion

            #region 收入科目
            {
                //bool hasReceiveItem = false;
                //string[] receiveItems = data.GetAllReceiveItems();
                //for (int idx = 0; idx < receiveItems.Length; idx++)
                //{
                //    if (!String.IsNullOrEmpty(receiveItems[idx]))
                //    {
                //        hasReceiveItem = true;
                //        break;
                //    }
                //}
                //if (!hasReceiveItem)
                //{
                //    //[TODO] 固定顯示訊息的收集
                //    string msg = this.GetLocalized("「收入科目」至少要設定一項");
                //    this.ShowSystemMessage(msg);
                //    return false;
                //}
            }
            #endregion

            #region 除收入科目外就學貸款可貸之額外固定金額
            if (data.LoanFee == null)
            {
                if (!String.IsNullOrWhiteSpace(this.tbxLoanFee.Text))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「除收入科目外就學貸款可貸之額外固定金額」不是大於0的金額");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                // [TODO] 如果為必要欄位，這裡的 Mark 移除
                //else
                //{
                //    this.ShowMustInputAlert("學分費比較基準");
                //    return false;
                //}
            }
            else
            {
                // [TODO] 如果有其他條件檢查寫在這裡
                decimal loanFee = data.LoanFee.Value;
                if (loanFee < 0)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「除收入科目外就學貸款可貸之額外固定金額」不是大於0的金額");
                    this.ShowSystemMessage(msg);
                    return false;
                }
            }
            #endregion

            #region 依收費標準計算時，就貸可貸金額計算依據
            if (String.IsNullOrEmpty(data.FlagRl))
            {
                this.ShowMustInputAlert("依收費標準計算時，就貸可貸金額計算依據");
                return false;
            }
            #endregion

            #region 繳費單上就學貸款可貸金額列印欄位
            if (String.IsNullOrEmpty(data.LoanQual))
            {
                this.ShowMustInputAlert("繳費單上就學貸款可貸金額列印欄位");
                return false;
            }
            #endregion

            #region 備註
            if (!String.IsNullOrEmpty(data.ReceiveMemo))
            {
                string[] lines = data.ReceiveMemo.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                if (lines.Length > 9)
                {
                    //[TODO] 固定顯示訊息的收集
                    //string msg = this.GetLocalized("「備註」超過 9 行");
                    //this.ShowSystemMessage(msg);
                    //return false;
                }
                else
                {
                    for (int idx = 0; idx < lines.Length; idx++)
                    {
                        if (lines[idx].Length > 24)
                        {
                            //[TODO] 固定顯示訊息的收集
                            //string msg = this.GetLocalized(String.Format("備註的第{0}行超過 24 個字", idx + 1));
                            //this.ShowSystemMessage(msg);
                            //return false;
                        }
                    }
                }
            }
            #endregion

            #region [MDY:202203XX] 2022擴充案 英文相關欄位
            bool isEngEnabled = this.IsEngEabled(this.EditReceiveType, !this.IsPostBack);

            if (isEngEnabled)
            {
                #region 繳費單英文模板
                if (String.IsNullOrEmpty(data.BillFormEId))
                {
                    this.ShowMustInputAlert("繳費單英文模板");
                    return false;
                }
                #endregion
            }
            #endregion

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region 檢查維護權限
                if (!this.HasMaintainAuth())
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
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
                    this.ccbtnOK.Visible = false;
                    return;
                }

                this.Action = QueryString.TryGetValue("Action", String.Empty);
                this.EditReceiveType = QueryString.TryGetValue("ReceiveType", String.Empty);
                this.EditYearId = QueryString.TryGetValue("YearId", String.Empty);
                this.EditTermId = QueryString.TryGetValue("TermId", String.Empty);
                this.EditDepId = QueryString.TryGetValue("DepId", String.Empty);
                this.EditReceiveId = QueryString.TryGetValue("ReceiveId", String.Empty);

                if (String.IsNullOrEmpty(this.EditReceiveType)
                    || String.IsNullOrEmpty(this.EditYearId)
                    || String.IsNullOrEmpty(this.EditTermId)
                    //|| String.IsNullOrEmpty(this.EditDepId)
                    || String.IsNullOrEmpty(this.EditReceiveId)
                    || !ActionMode.IsMaintinaMode(this.Action)
                    )
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }

                WebHelper.SetFilterArguments(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);
                #endregion

                #region [MDY:202203XX]
                //this.InitialUI();
                #endregion

                #region 檢查業務別碼授權
                if (!this.GetLogonUser().IsAuthReceiveTypes(this.EditReceiveType))
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得維護資料
                SchoolRidEntity data = null;
                {
                    string action = this.GetLocalized("查詢要維護的資料");

                    #region 查詢條件
                    Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, this.EditReceiveType)
                        .And(SchoolRidEntity.Field.YearId, this.EditYearId)
                        .And(SchoolRidEntity.Field.TermId, this.EditTermId)
                        .And(SchoolRidEntity.Field.DepId, this.EditDepId)
                        .And(SchoolRidEntity.Field.ReceiveId, this.EditReceiveId);
                    #endregion

                    #region 查詢資料
                    XmlResult xmlResult = DataProxy.Current.SelectFirst<SchoolRidEntity>(this, where, null, out data);
                    if (!xmlResult.IsSuccess)
                    {
                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        this.ccbtnOK.Visible = false;
                        return;
                    }
                    #endregion

                    switch (this.Action)
                    {
                        case ActionMode.Insert:   //新增
                            #region 新增
                            if (data == null)
                            {
                                //空的資料
                                data = new SchoolRidEntity();
                                data.ReceiveType = this.EditReceiveType;
                                data.YearId = this.EditYearId;
                                data.TermId = this.EditTermId;
                                data.DepId = this.EditDepId;
                                data.ReceiveId = this.EditReceiveId;
                                data.ReceiveStatus = this.EditReceiveStatus;

                                data.FlagRl = "1";
                                data.LoanQual = "1";
                            }
                            else
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
                                this.ccbtnOK.Visible = false;
                                return;
                            }
                            #endregion
                            break;
                        case ActionMode.Modify:   //修改
                        case ActionMode.Delete:   //刪除
                            #region 修改 | 刪除
                            if (data == null)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                                this.ccbtnOK.Visible = false;
                                return;
                            }
                            else
                            {
                                this.OrginalPayDueDate2 = data.PayDueDate2;
                            }
                            #endregion
                            break;
                    }
                }
                #endregion

                this.ucFilter1.GetDataAndBind(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);

                this.BindEditData(data);
            }
        }

        protected void ccbtnPreview_Click(object sender, EventArgs e)
        {
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            SchoolRidEntity data = this.GetEditData();
            if (this.Action != ActionMode.Delete && !this.CheckEditData(data))
            {
                return;
            }

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "D1200001.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    {
                        #region 補齊資料
                        if (data.LoanQual == null)
                        {
                            data.LoanQual = String.Empty;
                        }
                        if (data.Hide == null)
                        {
                            data.Hide = String.Empty;
                        }
                        if (data.SchLevel == null)
                        {
                            data.SchLevel = String.Empty;
                        }

                        if (data.SchLevel == null)
                        {
                            data.SchLevel = String.Empty;
                        }
                        if (data.SchLevel == null)
                        {
                            data.SchLevel = String.Empty;
                        }
                        if (data.SchLevel == null)
                        {
                            data.SchLevel = String.Empty;
                        }

                        data.Usestamp = "N";
                        data.Usewatermark = "N";
                        if (data.Usepostdueday == null)
                        {
                            data.Usepostdueday = String.Empty;
                        }
                        //data.Status = DataStatusCodeTexts.NORMAL;
                        //data.CrtUser = this.GetLogonUser().UserId;
                        //data.CrtDate = DateTime.Now;

                        #region [MDY:202203XX] 2022擴充案 調整
                        data.UpdateWho = this.GetLogonUser().UserId;
                        data.UpdateTime = DateTime.Now;
                        #endregion

                        data.PostExtraDays = 0;
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Insert<SchoolRidEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
                            }
                            else
                            {
                               // WebHelper.SetFilterArguments(data.ReceiveType, data.YearId, data.TermId, data.DepId, string.Empty);

                                this.ShowActionSuccessAlert(action, backUrl);
                            }
                        }
                        else
                        {
                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        }
                    }
                    #endregion
                    break;
                case ActionMode.Modify:     //修改
                    #region 修改
                    {
                        #region [MDY:202203XX] 2022擴充案 調整
                        data.UpdateWho = this.GetLogonUser().UserId;
                        data.UpdateTime = DateTime.Now;
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Update<SchoolRidEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                DateTime? payDueDate2 = DataFormat.ConvertDateText(data.PayDueDate2);
                                if (payDueDate2 != null && payDueDate2.Value >= DateTime.Today && this.OrginalPayDueDate2 != data.PayDueDate2)
                                {
                                    Expression w1 = new Expression(StudentReceiveEntity.Field.CancelNo, RelationEnum.NotEqual, null)
                                        .And(StudentReceiveEntity.Field.CancelNo, RelationEnum.NotEqual, String.Empty);
                                    Expression w2 = new Expression(StudentReceiveEntity.Field.ReceiveWay, null)
                                        .Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty);
                                    Expression w3 = new Expression(StudentReceiveEntity.Field.ReceiveDate, null)
                                        .Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty);
                                    Expression w4 = new Expression(StudentReceiveEntity.Field.AccountDate, null)
                                        .Or(StudentReceiveEntity.Field.AccountDate, String.Empty);
                                    Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, data.ReceiveType)
                                        .And(StudentReceiveEntity.Field.YearId, data.YearId)
                                        .And(StudentReceiveEntity.Field.TermId, data.TermId)
                                        .And(StudentReceiveEntity.Field.DepId, data.DepId)
                                        .And(StudentReceiveEntity.Field.ReceiveId, data.ReceiveId)
                                        .And(StudentReceiveEntity.Field.CFlag, "1") //已送過
                                        .And(w1)
                                        .And(w2)
                                        .And(w3)
                                        .And(w4);
                                    KeyValue[] fieldValues = new KeyValue[] {
                                        new KeyValue(StudentReceiveEntity.Field.CFlag, "0")
                                    };
                                    int count2 = 0;
                                    XmlResult xmlResult2 = DataProxy.Current.UpdateFields<StudentReceiveEntity>(this, where, fieldValues, out count2);
                                    if (!xmlResult2.IsSuccess && xmlResult2.Code != "FD002")
                                    {
                                        this.ShowJsAlertAndGoUrl("更新代收費用檔資料成功，但相關學生繳費資料的中國信託上傳註記清除失敗", backUrl);
                                        return;
                                    }
                                }

                                //WebHelper.SetFilterArguments(data.ReceiveType, data.YearId, data.TermId, data.DepId, string.Empty);

                                this.ShowActionSuccessAlert(action, backUrl);
                            }
                        }
                        else
                        {
                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        }
                    }
                    #endregion
                    break;
                case ActionMode.Delete:     //刪除
                    #region 刪除
                    {
                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Delete<SchoolRidEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                //WebHelper.SetFilterArguments(data.ReceiveType, data.YearId, data.TermId, data.DepId, string.Empty);

                                this.ShowActionSuccessAlert(action, backUrl);
                            }
                        }
                        else
                        {
                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        }
                    }
                    #endregion
                    break;
            }
        }
    }
}