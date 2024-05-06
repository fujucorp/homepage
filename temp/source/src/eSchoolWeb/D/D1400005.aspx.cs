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
    public partial class D1400005 : BasePage
    {
        #region Property
        /// <summary>
        /// 儲存業務別碼代碼的查詢條件
        /// </summary>
        private string QueryReceiveType
        {
            get
            {
                #region [MDY:20190906] (2019擴充案) 原掃誤判，所以多做轉換
                string value = ViewState["QueryReceiveType"] as string;
                return value == null ? null : Server.HtmlEncode(value);
                #endregion
            }
            set
            {
                ViewState["QueryReceiveType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存對照表代碼的查詢條件
        /// </summary>
        private string QueryMappingID
        {
            get
            {
                return ViewState["QueryMappingID"] as string;
            }
            set
            {
                ViewState["QueryMappingID"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private bool InitialUI()
        {
            #region ucFilter1 設定
            string receiveType = null;
            string yearID = null;
            string termID = null;
            string depID = null;
            string ReceiveID = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearID, out termID, out depID, out ReceiveID)
                || String.IsNullOrEmpty(receiveType)
                || String.IsNullOrEmpty(yearID))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得業務別碼");
                this.ShowJsAlert(msg);
                return false;
            }

            this.QueryReceiveType = receiveType;
            this.ucFilter1.GetDataAndBind(this.QueryReceiveType, "", "");
            #endregion

            #region 檢查業務別碼授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(receiveType))
            {
                string msg = this.GetLocalized("該業務別碼未授權");
                this.ShowJsAlert(msg);
                return false;
            }
            #endregion

            return this.GetDataAndBind(receiveType);
        }

        private bool GetDataAndBind(string qReceiveType)
        {
            #region 檢查查詢權限
            if (!this.HasMaintainAuth())
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無查詢權限");
                this.ShowJsAlert(msg);
                return false;
            }
            #endregion

            #region 查詢條件
            Expression where = new Expression();
            where.And(MappingrrXlsmdbEntity.Field.ReceiveType, qReceiveType);
            #endregion

            #region 排序條件
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(MappingrrXlsmdbEntity.Field.MappingId, OrderByEnum.Asc);
            #endregion

            MappingrrXlsmdbEntity[] XlsDatas = null;
            XmlResult result = DataProxy.Current.SelectAll<MappingrrXlsmdbEntity>(this, where, orderbys, out XlsDatas);
            if (!result.IsSuccess)
            {
                //[TODO] 固定顯示訊息的收集
                this.ShowSystemMessage(this.GetLocalized("查詢資料失敗") + "，" + result.Message);
                return false;
            }

            MappingrrTxtEntity[] TxtDatas = null;
            result = DataProxy.Current.SelectAll<MappingrrTxtEntity>(this, where, orderbys, out TxtDatas);
            if (!result.IsSuccess)
            {
                //[TODO] 固定顯示訊息的收集
                this.ShowSystemMessage(this.GetLocalized("查詢資料失敗") + "，" + result.Message);
                return false;
            }
            else if ((TxtDatas == null || TxtDatas.Length == 0) && (XlsDatas == null || XlsDatas.Length == 0))
            {
                ////[TODO] 固定顯示訊息的收集
                //string msg = this.GetLocalized("查無資料");
                //this.ShowSystemMessage(msg);
                this.gvResult.DataSource = TxtDatas;
                this.gvResult.DataBind();
                return result.IsSuccess;
            }

            //將 Xls 及 Txt 兩個 Table 的資料放在一起
            int iCount = TxtDatas.Length + XlsDatas.Length;
            MappingrrXlsmdbEntity[] allDatas = new MappingrrXlsmdbEntity[iCount];
            for (int i = 0; i < XlsDatas.Length; i++)
            {
                allDatas[i] = new MappingrrXlsmdbEntity();
                allDatas[i].ReceiveType = XlsDatas[i].ReceiveType;
                allDatas[i].MappingId = XlsDatas[i].MappingId;
                allDatas[i].MappingName = XlsDatas[i].MappingName + ",xls";  //用以區分資料來源(TXT || XLS)
            }

            for (int i = 0; i < TxtDatas.Length; i++)
            {
                allDatas[i + XlsDatas.Length] = new MappingrrXlsmdbEntity();
                allDatas[i + XlsDatas.Length].ReceiveType = TxtDatas[i].ReceiveType;
                allDatas[i + XlsDatas.Length].MappingId = TxtDatas[i].MappingId;
                allDatas[i + XlsDatas.Length].MappingName = TxtDatas[i].MappingName + ",txt";  //用以區分資料來源(TXT || XLS)
            }

            this.gvResult.DataSource = allDatas;
            this.gvResult.DataBind();
            return result.IsSuccess;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }

        protected void ccbtnInsert_Click(object sender, EventArgs e)
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無維護權限");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            KeyValueList<string> QueryString = new KeyValueList<string>();
            QueryString.Add("Action", "A");
            QueryString.Add("ReceiveType", this.QueryReceiveType);
            Session["QueryString"] = QueryString;
            Server.Transfer("D1400005M.aspx");
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無維護權限");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            #region 處理資料參數
            string argument = e.CommandArgument as string;
            if (String.IsNullOrEmpty(argument))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            string[] args = argument.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length != 4)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要處理資料的參數!");
                this.ShowSystemMessage(msg);
                return;
            }
            #endregion

            switch (e.CommandName)
            {
                case ButtonCommandName.Modify:
                    #region 修改資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", "M");
                        QueryString.Add("FileType", args[0]);
                        QueryString.Add("ReceiveType", args[1]);
                        QueryString.Add("MappingId", args[2]);
                        QueryString.Add("MappingName", args[3]);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl("D1400005M.aspx"));
                        #endregion
                    }
                    #endregion
                    break;
                case ButtonCommandName.Delete:
                    #region 刪除資料
                    {
                        XmlResult result = new XmlResult();
                        int count = 0;
                        if (args[0] == "xls")
                        {
                            MappingrrXlsmdbEntity XlsData = new MappingrrXlsmdbEntity();
                            XlsData.ReceiveType = args[1];
                            XlsData.MappingId = args[2];
                            result = DataProxy.Current.Delete<MappingrrXlsmdbEntity>(this, XlsData, out count);
                        }
                        else
                        {
                            MappingrrTxtEntity TxtData = new MappingrrTxtEntity();
                            TxtData.ReceiveType = args[1];
                            TxtData.MappingId = args[2];
                            result = DataProxy.Current.Delete<MappingrrTxtEntity>(this, TxtData, out count);
                        }

                        if (result.IsSuccess)
                        {
                            if (count == 0)
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("刪除資料失敗，無資料被刪除");
                                this.ShowSystemMessage(msg);
                            }
                            else
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("刪除資料成功");
                                this.ShowSystemMessage(msg);
                                this.GetDataAndBind(this.QueryReceiveType);
                            }
                        }
                        else
                        {
                            //[TODO] 變動顯示訊息怎麼多語系
                            this.ShowSystemMessage(this.GetLocalized("刪除資料失敗") + "，" + result.Message);
                        }
                    }
                    #endregion
                    break;

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                case "GenXlsSample":
                case "GenOdsSample":
                    #region 產生試算表範本檔
                    {
                        if (args[0] == "xls")
                        {
                            MappingrrXlsmdbEntity data = null;
                            Expression where = new Expression(MappingrrXlsmdbEntity.Field.ReceiveType, args[1])
                                .And(MappingrrXlsmdbEntity.Field.MappingId, args[2]);
                            XmlResult result = DataProxy.Current.SelectFirst<MappingrrXlsmdbEntity>(this, where, null, out data);
                            if (!result.IsSuccess)
                            {
                                this.ShowSystemMessage(result.Code, result.Message);
                                return;
                            }
                            if (data == null)
                            {
                                this.ShowSystemMessage(ErrorCode.D_DATA_NOT_FOUND, "查無資料");
                                return;
                            }

                            #region [Old] 因為 Web 端參考 NPOI V2.0，所以改用 ExcelHelper.dll 的 ConvertFileHelper
                            //XlsMapField[] mapFields = data.GetMapFields();
                            //ConvertFileHelper helper = new ConvertFileHelper();
                            //byte[] content = helper.XlsMapField2XlsSample(mapFields, "sheet1");
                            #endregion

                            #region 收集欄位名稱
                            XlsMapField[] mapFields = data.GetMapFields();
                            List<string> headTexts = new List<string>(mapFields.Length);
                            foreach (XlsMapField mapField in mapFields)
                            {
                                headTexts.Add(mapField.CellName);
                            }
                            #endregion

                            byte[] content = null;
                            string sheetName = "Sheet1";
                            string extName = null;
                            if (e.CommandName == "GenXlsSample")
                            {
                                extName = "xls";
                                #region 產生 xls 的範本
                                #region [MDY:20220503] 改用 ConvertFileHelper.XlsMapField2XlsSample()
                                #region [OLD]
                                //ExcelHelper.ConvertFileHelper helper = new ExcelHelper.ConvertFileHelper();
                                //content = helper.GenXlsSample(headTexts, "sheet1");
                                #endregion

                                ConvertFileHelper helper = new ConvertFileHelper();
                                content = helper.XlsMapField2XlsSample(mapFields, sheetName);
                                #endregion
                                #endregion
                            }
                            else
                            {
                                extName = "ods";
                                #region 產生 Ods 的範本
                                ODSHelper helper = new ODSHelper();
                                content = helper.GenOdsSample(headTexts, sheetName);
                                #endregion
                            }

                            if (content == null)
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("產生範本失敗");
                                this.ShowSystemMessage(msg);
                                return;
                            }
                            else
                            {
                                string fileName = String.Concat(data.MappingName, "_範本.", extName);
                                this.ResponseFile(fileName, content);
                            }
                        }
                        else
                        {
                            this.ShowJsAlert("非試算表格式，無法產生範本");
                        }
                    }
                    #endregion
                    break;
                #endregion

                default:
                    break;
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            MappingrrXlsmdbEntity[] datas = this.gvResult.DataSource as MappingrrXlsmdbEntity[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式 (Xls 與 Ods 共用設定檔)
            string lbtnSampleXlsText = "Xls " + this.GetLocalized("範本");
            string lbtnSampleOdsText = "Ods " + this.GetLocalized("範本");

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                MappingrrXlsmdbEntity data = datas[row.RowIndex];

                string[] strings = data.MappingName.Split(',');
                string mappingName = strings[0];
                string fileType = strings[1];
                //設定資料參數 0-FileType, 1-ReceiveType, 2-MappingId, 3-MappingName
                string argument = String.Format("{0},{1},{2},{3}", fileType, data.ReceiveType, data.MappingId, mappingName);

                LinkButton lbtnSampleXLS = row.FindControl("lbtnSampleXLS") as LinkButton;
                if (lbtnSampleXLS != null)
                {
                    lbtnSampleXLS.Visible = (fileType == "xls");
                    lbtnSampleXLS.Text = lbtnSampleXlsText;
                    lbtnSampleXLS.CommandArgument = argument;
                    lbtnSampleXLS.CommandName = "GenXlsSample";
                }

                LinkButton lbtnSampleODS = row.FindControl("lbtnSampleODS") as LinkButton;
                if (lbtnSampleODS != null)
                {
                    lbtnSampleODS.Visible = (fileType == "xls");
                    lbtnSampleODS.Text = lbtnSampleOdsText;
                    lbtnSampleODS.CommandArgument = argument;
                    lbtnSampleODS.CommandName = "GenOdsSample";
                }

                MyModifyButton ccbtnModify = row.FindControl("ccbtnModify") as MyModifyButton;
                if (ccbtnModify != null)
                {
                    ccbtnModify.CommandArgument = argument;
                }

                MyDeleteButton ccbtnDelete = row.FindControl("ccbtnDelete") as MyDeleteButton;
                if (ccbtnDelete != null)
                {
                    ccbtnDelete.CommandArgument = argument;
                }

                //設定顯示文字
                row.Cells[1].Text = mappingName;
                if (fileType == "xls")
                {
                    row.Cells[0].Text = "試算表";
                }
                else
                {
                    row.Cells[0].Text = "純文字(txt)";
                }
            }
            #endregion
        }
    }
}