using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

using Fuju;
using Fuju.Configuration;
using Fuju.DB;
using Fuju.DB.Configuration;
using Fuju.DB.Data;

using Entities;

namespace WebAP
{
    public partial class EAINotifyTest : System.Web.UI.Page
    {
        #region Config 取值相關
        private const string ConfigMachineGroupKey = "MachineGroup";

        private string _MyConfigMachineGroupValue = null;
        private string GetMyConfigMachineGroupValue()
        {
            if (_MyConfigMachineGroupValue == null)
            {
                string value = ConfigManager.Current.GetSystemConfigValue(ConfigMachineGroupKey, Environment.MachineName, StringComparison.CurrentCultureIgnoreCase);
                if (value == null)
                {
                    _MyConfigMachineGroupValue = String.Empty;
                }
                else
                {
                    _MyConfigMachineGroupValue = value.Trim();
                }
            }
            return _MyConfigMachineGroupValue;
        }

        private string GetMyConfigGroup(string group)
        {
            if (String.IsNullOrEmpty(group))
            {
                return null;
            }

            string machineGroupName = this.GetMyConfigMachineGroupValue();
            if (String.IsNullOrEmpty(machineGroupName))
            {
                return group.Trim();
            }
            else
            {
                return String.Concat(group.Trim(), "_", machineGroupName);
            }
        }

        private string GetMyConfigName(string name)
        {
            string machineGroupName = this.GetMyConfigMachineGroupValue();
            if (String.IsNullOrEmpty(machineGroupName))
            {
                return name == null ? String.Empty : name.Trim();
            }
            else if (name == null)
            {
                return machineGroupName;
            }
            else
            {
                return String.Concat(name.Trim(), "_", machineGroupName);
            }
        }

        private string GetProjectConfigValue(string group, string name, StringComparison? comparisonType = null)
        {
            string myGroup = this.GetMyConfigGroup(group);
            if (myGroup == null)
            {
                return null;
            }
            else
            {
                if (comparisonType == null)
                {
                    return ConfigManager.Current.GetProjectConfigValue(myGroup, name);
                }
                else
                {
                    return ConfigManager.Current.GetProjectConfigValue(myGroup, name, comparisonType.Value);
                }
            }
        }

        private ConnectionSetting GetDBConnectionSetting(string dbConfigName, StringComparison? comparisonType = null)
        {
            string myDBConfigName = this.GetMyConfigName(dbConfigName);
            if (comparisonType == null)
            {
                return DBConfigManager.Current.GetConnectionSetting(myDBConfigName);
            }
            else
            {
                return DBConfigManager.Current.GetConnectionSetting(myDBConfigName, comparisonType.Value);
            }
        }
        #endregion

        #region 資料庫存取相關

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region
                int sqlRows = 0;
                string txt = Request.QueryString["SqlRows"];
                if (!String.IsNullOrWhiteSpace(txt) && int.TryParse(txt, out sqlRows))
                {
                    int minRows = 10;
                    int maxRows = 30;
                    if (sqlRows <= minRows)
                    {
                        this.tbxSql.Rows = minRows;
                    }
                    else if (sqlRows >= maxRows)
                    {
                        this.tbxSql.Rows = maxRows;
                    }
                    else
                    {
                        this.tbxSql.Rows = sqlRows;
                    }
                }
                #endregion
            }
        }

        protected void btnExec_Click(object sender, EventArgs e)
        {
            #region Parse And Check Sql
            string sql = null;
            if (!String.IsNullOrWhiteSpace(this.tbxSql.Text))
            {
                #region [MDY:20220410] Checkmarx 調整
                try
                {
                    List<string> sqlLines = new List<string>();
                    using (StringReader reader = new StringReader(this.tbxSql.Text.Trim()))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            #region 忽略空白行與註解行
                            if (String.IsNullOrWhiteSpace(line))
                            {
                                continue;
                            }
                            line = line.Trim();
                            if (line.StartsWith("--") || (line.StartsWith("/*") && line.StartsWith("*/")))
                            {
                                continue;
                            }
                            #endregion

                            sqlLines.Add(line);
                        }
                    }
                    sql = string.Join(Environment.NewLine, sqlLines);
                }
                catch (Exception)
                {
                    this.divResult.InnerText = "讀取輸入字串處理失敗";
                    return;
                }
                #endregion
            }
            #endregion

            if (String.IsNullOrEmpty(sql))
            {
                this.divResult.InnerText = "請先輸入有效的 Sql 指令";
                return;
            }

            if (sql.IndexOf("CREATE ", StringComparison.CurrentCultureIgnoreCase) > -1
                || sql.IndexOf("DROP ", StringComparison.CurrentCultureIgnoreCase) > -1
                || sql.IndexOf("ALTER ", StringComparison.CurrentCultureIgnoreCase) > -1)
            {
                this.divResult.InnerText = "Sql 指令不僅允許包含 CREATE、DROP 與 ALTER 關鍵字，請使用 [] 包覆";
                return;
            }

            string cmdType = null;
            int startIndex = 0;
            int maxRecords = 0;
            if (this.rbtIsExecSql.Checked)
            {
                #region 執行增修刪指令
                if (sql.IndexOf("INSERT ", StringComparison.CurrentCultureIgnoreCase) > -1
                    || sql.IndexOf("UPDATE ", StringComparison.CurrentCultureIgnoreCase) > -1
                    || sql.IndexOf("DELETE ", StringComparison.CurrentCultureIgnoreCase) > -1
                    || (sql.IndexOf("SELECT ", StringComparison.CurrentCultureIgnoreCase) > -1 && sql.IndexOf("INTO ", StringComparison.CurrentCultureIgnoreCase) > -1)
                    )
                {
                    cmdType = "EXEC";
                }
                else
                {
                    this.divResult.InnerText = "未找到增修刪資料的 Sql 指令";
                    return;
                }
                #endregion
            }
            else
            {
                #region 執行查詢指令
                if (sql.IndexOf("INSERT ", StringComparison.CurrentCultureIgnoreCase) > -1
                    || sql.IndexOf("UPDATE ", StringComparison.CurrentCultureIgnoreCase) > -1
                    || sql.IndexOf("DELETE ", StringComparison.CurrentCultureIgnoreCase) > -1
                    || (sql.IndexOf("SELECT ", StringComparison.CurrentCultureIgnoreCase) > -1 && sql.IndexOf("INTO ", StringComparison.CurrentCultureIgnoreCase) > -1)
                    )
                {
                    this.divResult.InnerText = "查詢資料時，請勿在 Sql 指令中留有 INSERT、UPDATE、DELETE 指令，增修刪資料時請勾選【執行增修刪指令】";
                    return;
                }
                else
                {
                    cmdType = "SELECT";

                    string startIndexText = this.tbxStartIndex.Text.Trim();
                    if (String.IsNullOrEmpty(startIndexText))
                    {
                        this.divResult.InnerText = "請先輸入起始索引";
                        return;
                    }
                    if (!Int32.TryParse(startIndexText, out startIndex) || startIndex < 0)
                    {
                        this.divResult.InnerText = "起始索引必須是正整數";
                        return;
                    }

                    string maxRecordsText = this.tbxMaxRecords.Text.Trim();
                    if (String.IsNullOrEmpty(maxRecordsText))
                    {
                        this.divResult.InnerText = "請先輸入最大筆數";
                        return;
                    }
                    if (!Int32.TryParse(maxRecordsText, out maxRecords) || maxRecords < 0)
                    {
                        this.divResult.InnerText = "最大筆數必須是正整數";
                        return;
                    }
                }
                #endregion
            }

            try
            {
                ConnectionSetting connectionString = this.GetDBConnectionSetting(null, null);
                if (connectionString == null)
                {
                    this.divResult.InnerText = "(dbs) 未設定預設的資料庫連線";
                    return;
                }
                else if (connectionString.DBType != DBTypeEnum.MSSQL)
                {
                    this.divResult.InnerText = "(dbs) 預設資料庫連線的 dbType 不是 MSSQL";
                    return;
                }

                if (cmdType == "SELECT")
                {
                    DataTable dt = null;
                    using (MSSQLFactory factory = new MSSQLFactory(connectionString.ConnectionString))
                    {
                        KeyValue[] parameters = null;
                        dt = factory.GetDataTable(sql, parameters, "QUERY", startIndex, maxRecords, 600);
                    }
                    if (dt.Rows.Count == 0)
                    {
                        this.divResult.InnerText = "無資料";
                        return;
                    }

                    StringBuilder html = new StringBuilder();
                    html.AppendLine("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\">");

                    html.AppendLine("<tr>");
                    foreach (DataColumn dColumn in dt.Columns)
                    {
                        html.AppendFormat("<td>{0}</td>", dColumn.ColumnName);
                    }
                    html.AppendLine("</tr>");

                    Type byteArrayType = typeof(byte[]);
                    Type stringType = typeof(string);
                    foreach (DataRow dRow in dt.Rows)
                    {
                        html.AppendLine("<tr>");
                        foreach (DataColumn dColumn in dt.Columns)
                        {
                            Type dataType = dColumn.DataType;
                            if (dRow.IsNull(dColumn))
                            {
                                html.AppendLine("<td>IS NULL</td>");
                            }
                            else if (dataType == byteArrayType)
                            {
                                html.AppendLine("<td>IS BYTE[]</td>");
                            }
                            else if (dataType == stringType || dataType.IsValueType)
                            {
                                html.AppendFormat("<td>{0}</td>", dRow[dColumn].ToString()).AppendLine();
                            }
                            else
                            {
                                html.AppendFormat("<td>{0}</td>", dRow[dColumn].ToString()).AppendLine();
                            }
                        }
                        html.AppendLine("</tr>");
                    }

                    html.AppendLine("</table>");
                    this.divResult.InnerHtml = html.ToString();
                }
                else if (cmdType == "EXEC")
                {
                    int count = 0;

                    using (MSSQLFactory factory = new MSSQLFactory(connectionString.ConnectionString))
                    {
                        KeyValue[] parameters = null;
                        count = factory.ExecuteNonQuery(sql, parameters, 600);
                    }
                    this.divResult.InnerText = String.Format("受影響筆數 {0}", count);
                }
                else
                {
                    this.divResult.InnerText = "無法分辨的 SQL 指令類型";
                }
            }
            catch (Exception ex)
            {
                this.divResult.InnerText = ex.Message;
                return;
            }
        }

        #region [Old]
//        protected void Button1_Click(object sender, EventArgs e)
//        {
//            //this.TextBox3.Text = "";

//            //string content = this.TextBox1.Text.Trim();
//            //string url = this.TextBox2.Text.Trim();
//            //byte[] buff = Encoding.UTF8.GetBytes(content);
//            //HttpWebRequest r = HttpWebRequest.Create(url) as HttpWebRequest;
//            //r.Method = "POST";
//            //r.ContentType = "application/x-www-form-urlencoded";
//            //r.Timeout = 30000;
//            //r.ContentLength = buff.Length;
//            //using (Stream st = r.GetRequestStream())
//            //{
//            //    st.Write(buff, 0, buff.Length);
//            //}

//            //string result = "";
//            //using (HttpWebResponse rsp = r.GetResponse() as HttpWebResponse)
//            //{
//            //    using (StreamReader sr = new StreamReader(rsp.GetResponseStream()))
//            //    {
//            //        result = sr.ReadToEnd();
//            //    }
//            //}

//            //this.TextBox3.Text = result;
//        }

//        protected void btnEncodeInitKey_Click(object sender, EventArgs e)
//        {
//            EAIHelper helper = new EAIHelper();
//            string errmsg = helper.EncodEAIInitKey();
//            this.labEncodeResult.Text = errmsg;
//        }

//        protected void btnUpdateSeriorNoAndCancelNo_Click(object sender, EventArgs e)
//        {
//            #region 輸入參數
//            string receiveType = this.tbxReceiveType.Text.Trim();
//            string yearId = this.tbxYearId.Text.Trim();
//            string termId = this.tbxTermId.Text.Trim();
//            string depId = String.Empty;    //土銀的部別不使用此欄位，所以固定為空字串
//            string receiveId = this.tbxReceiveId.Text.Trim();
//            string upNo = this.tbxUpNo.Text.Trim();
//            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId) || String.IsNullOrEmpty(receiveId) || String.IsNullOrEmpty(receiveId))
//            {
//                this.labUpdateSeriorNoAndCancelNoResult.Text = "缺少商家代號、學年、學期、費用別或資料批號";
//                return;
//            }

//            int sUpOrder = 0;
//            string txtSUpOrder = this.tbxSUpOrder.Text.Trim();
//            if (String.IsNullOrEmpty(txtSUpOrder) || !Int32.TryParse(txtSUpOrder, out sUpOrder) || sUpOrder < 1)
//            {
//                this.labUpdateSeriorNoAndCancelNoResult.Text = "缺少或不正確的起始資料序號";
//                return;
//            }
//            txtSUpOrder = sUpOrder.ToString().PadLeft(6, '0');
//            int sSeriorNo = 0;
//            string txtSSeriorNo = this.tbxSSeriroNo.Text.Trim();
//            if (String.IsNullOrEmpty(txtSSeriorNo) || !Int32.TryParse(txtSSeriorNo, out sSeriorNo) || sSeriorNo < 1)
//            {
//                this.labUpdateSeriorNoAndCancelNoResult.Text = "缺少或不正確的起始虛擬帳號流水號";
//                return;
//            }
//            int eUpOrder = 0;
//            string txtEUpOrder = this.tbxEUpOrder.Text.Trim();
//            if (String.IsNullOrEmpty(txtEUpOrder) || !Int32.TryParse(txtEUpOrder, out eUpOrder) || eUpOrder < 1)
//            {
//                this.labUpdateSeriorNoAndCancelNoResult.Text = "缺少或不正確的結束資料序號";
//                return;
//            }
//            txtEUpOrder = eUpOrder.ToString().PadLeft(6, '0');
//            #endregion

//            StringBuilder updateSqls = new StringBuilder();
//            try
//            {
//                string prefix = String.Format("{0}{1}{2}{3}", receiveType, yearId.Substring(yearId.Length - 1, 1), termId, receiveId);

//                CancelNoHelper helper = new CancelNoHelper();
//                CancelNoHelper.Module module = helper.GetModuleByReceiveType(receiveType);
//                if (module == null)
//                {
//                    this.labUpdateSeriorNoAndCancelNoResult.Text = "無法取得檢碼模組資料";
//                    return;
//                }

//                ConnectionSetting connectionString = DBConfigManager.Current.GetConnectionSetting(null);
//                using (MSSQLFactory factory = new MSSQLFactory(connectionString.ConnectionString))
//                {
//                    string sql = @"
//SELECT Stu_Id, Old_Seq, Up_Order, ISNULL(Receive_Amount, -1) AS Receive_Amount, ISNULL(Serior_No, '') AS Serior_No, ISNULL(Cancel_No, '') AS Cancel_No FROM Student_Receive 
// WHERE Receive_Type = @Receive_Type AND Year_Id = @Year_Id AND Term_Id = @Term_Id AND Dep_Id = @Dep_Id AND Receive_Id = @Receive_Id 
//   AND Up_No = @Up_No AND Up_Order >= @S_Up_Order AND Up_Order <= @E_Up_Order 
// ORDER BY Up_Order";
//                    KeyValue[] parameters = new KeyValue[] {
//                        new KeyValue("@Receive_Type", receiveType),
//                        new KeyValue("@Year_Id", yearId),
//                        new KeyValue("@Term_Id", termId),
//                        new KeyValue("@Dep_Id", depId),
//                        new KeyValue("@Receive_Id", receiveId),
//                        new KeyValue("@Up_No", upNo),
//                        new KeyValue("@S_Up_Order", txtSUpOrder),
//                        new KeyValue("@E_Up_Order", txtEUpOrder)
//                    };

//                    DataTable dt = factory.GetDataTable(sql, parameters, 0, 0);
//                    if (dt.Rows.Count == 0)
//                    {
//                        this.labUpdateSeriorNoAndCancelNoResult.Text = "查無資料";
//                        return;
//                    }

//                    int updateCount = 0;
//                    foreach(DataRow drow in dt.Rows)
//                    {
//                        string chkUpOrder = sUpOrder.ToString().PadLeft(6, '0');
//                        string upOrder = drow["Up_Order"].ToString();
//                        if (chkUpOrder != upOrder)
//                        {
//                            this.labUpdateSeriorNoAndCancelNoResult.Text = String.Format("不是預期的 {0} 資料序號 ({1})", chkUpOrder, upOrder);
//                            return;
//                        }
//                        string oldCancelNo = drow["Cancel_No"].ToString().Trim();
//                        if (!oldCancelNo.StartsWith(prefix))
//                        {
//                            string stuId = drow["Stu_Id"].ToString().Trim();
//                            string oldSeq = Convert.ToString(drow["Old_Seq"]);
//                            decimal receiveAmount = Convert.ToDecimal(drow["Receive_Amount"]);
//                            string seriorNo = sSeriorNo.ToString().PadLeft(6, '0');
//                            string newCancelNo = null;
//                            string newCustomNo = null;
//                            string newCheckSum = null;
//                            string errmsg = helper.TryGenCancelNo(module, receiveType, yearId, termId, receiveId, seriorNo, false, receiveAmount, out newCancelNo, out newCustomNo, out newCheckSum);
//                            if (!String.IsNullOrEmpty(errmsg))
//                            {
//                                this.labUpdateSeriorNoAndCancelNoResult.Text = String.Format("{0} 產生虛擬帳號失敗，錯誤訊息：{1}", upOrder, errmsg);
//                                return;
//                            }

//                            string updateSql = String.Format("UPDATE Student_Receive SET Serior_No = '{0}', Cancel_No = '{1}', Cancel_ATMNo = '{1}', Cancel_SMNo = '{1}', update_date = GETDATE() WHERE Receive_Type = '{2}' AND Year_Id = '{3}' AND Term_Id = '{4}' AND Dep_Id = '{5}' AND Receive_Id = '{6}' AND Stu_Id = '{7}' AND Old_Seq = {8} AND Up_No = '{9}' AND Up_Order = '{10}' AND Cancel_No = '{11}'"
//                                , seriorNo, newCancelNo, receiveType, yearId, termId, depId, receiveId, stuId, oldSeq, upNo, upOrder, oldCancelNo);
//                            updateSqls.AppendLine(updateSql);

//                            int count = factory.ExecuteNonQuery(updateSql);
//                            if (count == 0)
//                            {
//                                this.labUpdateSeriorNoAndCancelNoResult.Text = "查無資料被更新";
//                                return;
//                            }
//                            else
//                            {
//                                updateCount++;
//                            }
//                        }

//                        sUpOrder++;
//                        sSeriorNo++;
//                    }

//                    this.labUpdateSeriorNoAndCancelNoResult.Text = String.Format("更新 {0} 筆", updateCount);
//                }
//            }
//            catch (Exception ex)
//            {
//                this.labUpdateSeriorNoAndCancelNoResult.Text = "執行處理發生例外，錯誤訊息：" + ex.Message;
//                return;
//            }
//            finally
//            {
//                try
//                {
//                    string logPath = System.Configuration.ConfigurationManager.AppSettings.Get("LOG_PATH");
//                    if (!String.IsNullOrEmpty(logPath))
//                    {
//                        string logFile = Path.Combine(logPath, String.Format("UpdateSeriorNo_{0:yyyyMMddHHss}.log", DateTime.Now));
//                        File.WriteAllText(logFile, updateSqls.ToString());
//                    }
//                }
//                catch (Exception)
//                {
//                    this.labUpdateSeriorNoAndCancelNoResult.Text += updateSqls.ToString();
//                }
//            }
//        }
        #endregion
    }
}