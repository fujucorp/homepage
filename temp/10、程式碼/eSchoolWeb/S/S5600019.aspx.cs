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
    /// 產生測試繳款單
    /// </summary>
    public partial class S5600019 : BasePage
    {
        #region Private Method
        private DateTime GetPayDueDate()
        {
            object value = ViewState["PayDueDate"];
            if (value is DateTime)
            {
                return (DateTime)value;
            }
            else
            {
                DateTime date = DateTime.Today.AddDays(30);
                ViewState["PayDueDate"] = date;
                return date;
            }
        }

        /// <summary>
        /// 取得並結繫超商代收代號選項
        /// </summary>
        private void GetAndBindSMBarcodeIdOptions()
        {
            CodeText[] items = null;

            #region 取資料
            {
                Expression where = new Expression(ChannelWayEntity.Field.ChannelId, new string[] { ChannelHelper.SM_DEFAULT, ChannelHelper.SM_MOBILE });

                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(ChannelWayEntity.Field.BarcodeId, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { ChannelWayEntity.Field.ChannelId, ChannelWayEntity.Field.BarcodeId };
                string codeCombineFormat = "{0}-{1}";
                string[] textFieldNames = new string[] { ChannelWayEntity.Field.BarcodeId };
                string textCombineFormat = null;

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<ChannelWayEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("查詢超商代收代號資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion

            WebHelper.SetDropDownListItems(this.ddlSMBarcodeId, DefaultItem.Kind.Select, false, items, false, false, 0, null);
        }

        /// <summary>
        /// 取得並結繫代收管道資料
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="channelId"></param>
        /// <param name="borcodeId"></param>
        private void GetAndBindChannelInfo(string channelId, string borcodeId)
        {
            ChannelWayEntity data = null;
            if (!String.IsNullOrEmpty(channelId) && !String.IsNullOrEmpty(borcodeId))
            {
                Expression where = new Expression(ChannelWayEntity.Field.ChannelId, channelId)
                    .And(ChannelWayEntity.Field.BarcodeId, borcodeId);
                XmlResult xmlResult = DataProxy.Current.SelectFirst<ChannelWayEntity>(this, where, null, out data);
                string action = this.GetLocalized("讀取超商代收代號資料");
                if (!xmlResult.IsSuccess)
                {
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
                if (data == null)
                {
                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                }
            }
            if (data == null)
            {
                this.labChannelInfo.Text = "";
            }
            else
            {
                this.labChannelInfo.Text = String.Format("繳費下限：{0:#,##0}; 繳費上限：{1:#,##0}; 應付手續費：{2:#,##0}; 繳款人手續：{3:#,##0}; 繳款期限：{4:yyyy/MM/dd}", data.MinMoney, data.MaxMoney, data.ChannelCharge, data.RCSPay, this.GetPayDueDate());
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //#region 檢查查詢權限
                //if (!this.HasQueryAuth())
                //{
                //    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無查詢權限");
                //    return;
                //}
                //#endregion

                #region Initial
                this.GetAndBindSMBarcodeIdOptions();
                #endregion
            }
        }

        protected void ddlSMBarcodeId_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region 超商代收代號
            string channelId = null, barcodeId = null;
            {
                string value = this.ddlSMBarcodeId.SelectedValue;
                if (!String.IsNullOrEmpty(value))
                {
                    string[] args = value.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                    if (args.Length == 2)
                    {
                        channelId = args[0];
                        barcodeId = args[1];
                    }
                    else
                    {
                        this.ShowSystemMessage("無法取得指定『超商代收代號』參數");
                    }
                }
            }
            #endregion

            this.GetAndBindChannelInfo(channelId, barcodeId);
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            #region 商家代號
            string receiveType = this.tbxReceiveType.Text.Trim();
            if (String.IsNullOrEmpty(receiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return;
            }
            #endregion

            #region 超商代收代號
            string channelId = null, barcodeId = null;
            {
                string value = this.ddlSMBarcodeId.SelectedValue;
                if (String.IsNullOrEmpty(value))
                {
                    this.ShowMustInputAlert("超商代收代號");
                    return;
                }
                else
                {
                    string[] args = value.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                    if (args.Length == 2)
                    {
                        channelId = args[0];
                        barcodeId = args[1];
                    }
                    else
                    {
                        this.ShowSystemMessage("無法取得指定『超商代收代號』參數");
                        return;
                    }
                }
            }
            #endregion

            #region 產生張數
            int dataCount = 0;
            {
                string value = this.tbxDataCount.Text.Trim();
                if (String.IsNullOrEmpty(value))
                {
                    this.ShowMustInputAlert("產生張數");
                    return;
                }
                if (!Int32.TryParse(value, out dataCount) || dataCount < 1 || dataCount > 99)
                {
                    this.ShowSystemMessage("『產生張數』必須為 1 ~ 99 的整數");
                    return;
                }
            }
            #endregion

            #region 產生 PDF
            {
                string action = this.GetLocalized("產生繳費單");
                object returnData = null;
                KeyValue<string>[] arguments = new KeyValue<string>[] {
                    new KeyValue<string>("ReceiveType", receiveType),
                    new KeyValue<string>("ChannelId", channelId),
                    new KeyValue<string>("BarcodeId", barcodeId),
                    new KeyValue<string>("DataCount", dataCount.ToString()),
                    new KeyValue<string>("BillFormId", "1"),
                    new KeyValue<string>("PayDueDate", this.GetPayDueDate().ToString("yyyy/MM/dd"))
                };
                XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.GenSMBarcodePDF, arguments, out returnData);
                if (xmlResult.IsSuccess)
                {
                    byte[] fileContent = returnData as byte[];
                    if (fileContent == null || fileContent.Length == 0)
                    {
                        this.ShowActionFailureMessage(action, ErrorCode.S_INVALID_RETURN_VALUE, "不正確的回傳資料");
                    }
                    else
                    {
                        #region [MDY:20210401] 原碼修正
                        this.ResponseFile(String.Format("{0}_{1}_測試繳費單.PDF", HttpUtility.UrlEncode(receiveType), HttpUtility.UrlEncode(barcodeId)), fileContent, "PDF");
                        #endregion
                    }
                }
                else
                {
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion
        }
    }
}