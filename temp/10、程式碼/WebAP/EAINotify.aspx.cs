using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;
using System.IO;
using System.Configuration;
using System.Xml;
using Entities;
using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
namespace WebAP
{
    public partial class EAINotify : System.Web.UI.Page
    {
        const string HCODE = "1";   //沖帳記號
        public string _log_path = "";

        private void checkLogPath(string log_path)
        {
            DirectoryInfo di = new DirectoryInfo(log_path);
            if (!di.Exists)
            {
                try
                {
                    di.Create();
                }
                catch (Exception)
                {
                    //Response.Write(string.Format("建立資料夾發生錯誤，錯誤訊息={0}", ex.Message));
                    //Response.End();
                    //return;
                }
            }
        }

        private void writeLog(string log_path, string msg)
        {
            string page_id = "EAINotify";
            try
            {
                #region [MDY:20220410] Checkmarx 調整
                using (StreamWriter sw = new StreamWriter(log_path + page_id + "." + DateTime.Now.ToString("yyyyMMdd") + ".log", true, Encoding.UTF8))
                {
                    sw.WriteLine(string.Format("[{0}]{1}{2}{3}", DateTime.Now.ToString("HH:mm:ss"), System.Environment.NewLine, msg, System.Environment.NewLine));
                    sw.Close();
                    //sw.Dispose();
                }
                #endregion
            }
            catch (Exception)
            {
                //Response.Write(string.Format("寫log發生錯誤，錯誤訊息={0}", ex.Message));
                //Response.End();
                //return;
            }
        }

        private void processEAI(string data)
        {
            #region 電文sample
//<?xml version="1.0" encoding="utf-8"?>
//<LandBankML xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" version="1.0" xmlns="urn:schema-bluestar-com:multichannel">
//    <SignonRq>
//        <SignonPswd>
//            <CustId>
//                <SPName>ST</SPName>
//                <CustLoginId>SST00ETA11</CustLoginId>
//            </CustId>
//            <CustPswd>
//                <CryptType>DES1</CryptType>
//                <Pswd>CFE0D49312A6E311</Pswd>
//            </CustPswd>
//        </SignonPswd>
//        <ClientDt>2015/05/19 11:35:29</ClientDt>
//    </SignonRq>
//    <BankSvcRq>
//        <SPName>ST</SPName>
//        <TellerInfo>
//            <KinBr>908</KinBr>
//            <TrmSeq>4101</TrmSeq>
//            <TxtNo>00000000</TxtNo>
//            <TTskId>01</TTskId>
//            <TrmType>1</TrmType>
//        </TellerInfo>
//        <D338Rq>
//            <MsgId>D338</MsgId>
//            <RqUID />
//            <AppNo>7217</AppNo>
//            <TrnSeq>000001</TrnSeq>
//            <CustNo>0000000005</CustNo>
//            <TrnDt>01040519</TrnDt>
//            <TrnTime>113528</TrnTime>
//            <TrnAmt>100000.00</TrnAmt>
//            <HCode>0</HCode>
//            <Source>03</Source>
//            <MainDt>01040519</MainDt>
//            <BrNo>900</BrNo>
//            <Memo>INBKT -02-K74-CDIF  </Memo>
//            <Telegram>908410100000000011K740001D 00338019100000001040519010405190630000000%07217000001000000000501040519113528000001000000000301040519900INBKT -02-K74-CDIF  INBKT -900-0002-K7400-CDIF              </Telegram>
//        </D338Rq>
//    </BankSvcRq>
//</LandBankML>
            #endregion

            string xml = data.Replace("xmlns=\"urn:schema-bluestar-com:multichannel\"", ""); //為了避免namespace處理的困擾
            xml = xml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            xml = xml.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
            if (xml[0] == 65279)
            {
                xml = xml.Substring(1);
            }

            XmlDocument doc = new XmlDocument();
            XmlNode node = null;

            try
            {
                #region [MDY:20160329] 修正 FORTIFY 弱點
                #region [Old]
                //doc.LoadXml(xml);
                #endregion

                using (MemoryStream stream = new MemoryStream(Encoding.Default.GetBytes(xml)))
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.DtdProcessing = DtdProcessing.Prohibit;
                    settings.XmlResolver = null;
                    using (XmlReader reader = XmlReader.Create(stream, settings))
                    {
                        doc.Load(reader);
                    }
                }
                #endregion

                #region 欄位說明
                //MsgId   [0] : 電文編號
                //RqUID   [1] : ??
                //AppNo   [2] : 商家代號
                //TrnSeq  [3] : 交易序號
                //CustNo  [4] : 用戶編號 (銷編裡的客戶編號)
                //TrnDt   [5] : 代收日期
                //TrnTime [6] : 代收時間
                //TrnAmt  [7] : 代收金額
                //HCode   [8] : 沖帳註記
                //Source  [9] : 代收管道
                //MainDt  [10] : 入帳日期
                //BrNo    [11] : 銀行代碼
                //Memo    [12] : 備註
                //Telegram[13] : 
                #endregion

                //雖然只會有單筆，不過還是以多筆來處理
                List<CancelDebtsEntity> list = null;

                string[] dataTags = { "MsgId", "RqUID", "AppNo", "TrnSeq", "CustNo", "TrnDt", "TrnTime", "TrnAmt", "HCode", "Source", "MainDt", "BrNo", "Memo", "Telegram" };
                string[] dataVals = new string[dataTags.Length];
                List<string> receiveTypes = new List<string>();
                List<string> parameterNames = new List<string>();
                KeyValueList parameters = new KeyValueList();

                #region 取得 D338Rq node
                XmlNodeList D338Rq = null;
                try
                {
                    D338Rq = doc.SelectNodes("/LandBankML/BankSvcRq/D338Rq");
                }
                catch(Exception ex1)
                {
                    //路徑錯誤
                    writeLog(_log_path, string.Format("拆解xml發生錯誤，找不到路徑={0}，錯誤訊息={1}", "/LandBankML/BankSvcRq/D338Rq", ex1.Message));
                    return;
                }
                #endregion

                if (D338Rq != null && D338Rq.Count > 0)
                {
                    #region 拆解繳款訊息
                    list = new List<CancelDebtsEntity>(D338Rq.Count);
                    foreach (XmlNode rec in D338Rq)
                    {
                        #region 
                        #region
                        for (int idx = 0; idx < dataTags.Length; idx++)
                        {
                            node = rec.SelectSingleNode(dataTags[idx]);
                            if (node != null)
                            {
                                dataVals[idx] = node.InnerText.Trim();
                            }
                            else
                            {
                                dataVals[idx] = String.Empty;
                            }
                        }
                        #endregion

                        DateTime date;

                        #region 電文編號
                        string eai_code = dataVals[0].Trim();
                        #endregion

                        #region 商家代號
                        string receive_type = dataVals[2].Trim();
                        #endregion

                        #region 交易序號
                        int txSeq = 0;
                        int.TryParse(dataVals[3], out txSeq);
                        #endregion

                        #region 銷編裡的客戶編號
                        string customNo = dataVals[4].Trim();
                        #endregion

                        #region 代收日期
                        string receiveDate = null;
                        if (Fuju.Common.TryConvertTWDate8(dataVals[5], out date))
                        {
                            receiveDate = Fuju.Common.GetTWDate7(date); //資料庫存的是民國年7碼
                        }
                        else
                        {
                            receiveDate = dataVals[5];
                        }
                        #endregion

                        #region 交易時間
                        string receiveTime = dataVals[6];
                        #endregion

                        #region 交易金額
                        decimal receiveAmount = 0;
                        decimal.TryParse(dataVals[7], out receiveAmount);
                        #endregion

                        #region 沖銷註記
                        //及時下來的應該不會有沖銷吧???
                        #region 沖帳處理
                        //1. 如果遇到沖帳資料，則由該資料往前找到相同的資料，取消該資料的處理
                        //2. 將取消處理的資料與該沖帳資料記錄到 Log
                        bool rollbackFlag = (dataVals[8].Trim() == HCODE);	//沖帳記號
                        if (rollbackFlag)
                        {
                            //_Log.AppendFormat("發現沖帳資料 {0} : {1} ", dataVals[0], rec.OuterXml).AppendLine();
                            //for (int idx = list.Count - 1; idx >= 0; idx--)
                            //{
                            //    CancelDebtsEntity one = list[idx];
                            //    if (one.ReceiveType == appno && one.FileName == dataVals[1] && one.AccountDate == accountDate
                            //        && one.ReceiveWay == dataVals[6] && one.ReceiveBank == dataVals[8]
                            //        && one.ReceiveDate == receiveDate && one.ReceiveAmount == receiveAmount)
                            //    {
                            //        list.RemoveAt(idx);
                            //        _Log.AppendFormat("找到被沖資料 {0} : {1} ", one.FileName, one.Remark).AppendLine();
                            //        break;
                            //    }
                            //}
                            //continue;
                        }
                        #endregion
                        #endregion

                        #region 代收管道
                        string receiveWay = dataVals[9];
                        #endregion

                        #region 入帳日期
                        string accountDate = null;
                        if (Fuju.Common.TryConvertTWDate8(dataVals[10], out date))
                        {
                            accountDate = Fuju.Common.GetTWDate7(date); //資料庫存的是民國年7碼
                        }
                        else
                        {
                            accountDate = dataVals[10];
                        }
                        #endregion

                        if (receiveWay == ChannelHelper.PO)
                        {
                            //郵局的 PTXDAY (行員確認日) 才是實際入帳日，所以 accountDate 與 receiveDate 互換
                            string tmp = accountDate;
                            accountDate = receiveDate;
                            receiveDate = tmp;
                        }

                        #region 代收分行
                        string receiveBank = "005" + dataVals[11];   //EAI只給分行3碼，所以補上 005
                        #endregion

                        CancelDebtsEntity cancel_debts = new CancelDebtsEntity();

                        #region 繳款資料
                        cancel_debts.ReceiveType = receive_type;
                        cancel_debts.CancelNo = receive_type + customNo;
                        cancel_debts.ReceiveDate = receiveDate;
                        cancel_debts.ReceiveTime = receiveTime;
                        cancel_debts.AccountDate = accountDate;
                        cancel_debts.ReceiveBank = receiveBank;
                        cancel_debts.ReceiveWay = receiveWay;
                        cancel_debts.ReceiveAmount = receiveAmount;
                        #endregion

                        #region 資料來源
                        cancel_debts.FileName = String.Format("{0}_{1}_{2}", "D38", receive_type, DateTime.Now.ToString("yyyyMMddHHmm"));
                        cancel_debts.SourceData = rec.OuterXml;
                        cancel_debts.SourceSeq = txSeq;
                        #endregion

                        cancel_debts.PayDueDate = String.Empty;
                        cancel_debts.Reserve1 = String.Empty;
                        cancel_debts.Reserve2 = String.Empty;
                        cancel_debts.Remark = String.Empty;

                        cancel_debts.ModifyDate = DateTime.Now;
                        cancel_debts.RollbackDate = null;
                        cancel_debts.CancelDate = null;
                        cancel_debts.Status = "1";  //未處理

                        list.Add(cancel_debts);

                        #region 收集商家代號
                        if (!receiveTypes.Contains(receive_type))
                        {
                            receiveTypes.Add(receive_type);
                            string paramaterName = String.Format("@ReceiveType{0}", receiveTypes.Count);
                            parameterNames.Add(paramaterName);
                            parameters.Add(paramaterName, receive_type);
                        }
                        #endregion

                        #endregion
                    }

                    //把list寫到cancel_debts
                    if(list!=null && list.Count>0)
                    {
                        EntityFactory factory = new EntityFactory();
                        Result result = null;

                        #region 檢查收集的商家代號是否存在，不存在則移除
                        {
                            System.Data.DataTable dt = null;
                            string sql = String.Format("SELECT Receive_Type FROM School_Rtype WHERE Receive_Type IN ({0})", String.Join(",", parameterNames.ToArray()));
                            result = factory.GetDataTable(sql, parameters, 0, 0, out dt);
                            if (result.IsSuccess)
                            {
                                receiveTypes.Clear();
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    foreach (System.Data.DataRow drow in dt.Rows)
                                    {
                                        receiveTypes.Add(drow["Receive_Type"].ToString().Trim());
                                    }
                                }
                            }
                        }
                        #endregion

                        int count = 0;
                        for (int i = 0; i < list.Count; i++)
                        {
                            CancelDebtsEntity cancel_debts = list[i];
                            if (!receiveTypes.Contains(cancel_debts.ReceiveType))
                            {
                                continue;
                            }
                            result = factory.Insert(cancel_debts, out count);
                            if (!result.IsSuccess)
                            {
                                writeLog(_log_path, string.Format("寫入cancel_debts失敗，錯誤訊息={0}", result.Exception.Message));
                            }
                            else
                            {
                                writeLog(_log_path, string.Format("寫入cancel_debts成功"));
                            }
                        }
                    }
                    #endregion

                    #region 處理銷帳
                    //if (list != null && list.Count > 0)
                    //{
                    //    for(int i=0;i<list.Count;i++)
                    //    {
                    //        //找student_receive


                    //    }
                    //}
                    #endregion
                }
                else
                {
                    #region 沒有值
                    writeLog(_log_path, string.Format("拆解xml發生錯誤，錯誤訊息={0}", "xml沒有D338Rq路徑"));
                    #endregion
                }
            }
            catch(Exception ex)
            {
                //xml錯誤
                writeLog(_log_path, string.Format("拆解xml發生錯誤，錯誤訊息={0}", ex.Message));
            }
        }

        private void CancelProcess(CancelDebtsEntity cancel_debts)
        {
            //找學生繳費單資料
            
            
        }

        private void processD00338(string data)
        {

        }

        private void processD00339(string data)
        {

        }
        private void sendMail()
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                string log_path = ConfigurationManager.AppSettings.Get("LOG_PATH");
                if (!log_path.EndsWith(@"\")) log_path += @"\";
                _log_path = log_path;
                checkLogPath(log_path);

                Int32 content_size = Request.TotalBytes;
                byte[] buff = new byte[content_size];
                buff = this.Request.BinaryRead(content_size);
                string content = Encoding.UTF8.GetString(buff);
                writeLog(log_path, content);

                #region for debug
//                string content = @"<?xml version=""1.0"" encoding=""utf-8""?>
//<LandBankML xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" version=""1.0"" xmlns=""urn:schema-bluestar-com:multichannel"">
//    <SignonRq>
//        <SignonPswd>
//            <CustId>
//                <SPName>ST</SPName>
//                <CustLoginId>SST00ETA11</CustLoginId>
//            </CustId>
//            <CustPswd>
//                <CryptType>DES1</CryptType>
//                <Pswd>398730F542FE630F</Pswd>
//            </CustPswd>
//        </SignonPswd>
//        <ClientDt>2015/06/11 18:41:35</ClientDt>
//    </SignonRq>
//    <BankSvcRq>
//        <SPName>ST</SPName>
//        <TellerInfo>
//            <KinBr>908</KinBr>
//            <TrmSeq>4101</TrmSeq>
//            <TxtNo>00000000</TxtNo>
//            <TTskId>01</TTskId>
//            <TrmType>1</TrmType>
//        </TellerInfo>
//        <D338Rq>
//            <MsgId>D338</MsgId>
//            <RqUID />
//            <AppNo>5026</AppNo>
//            <TrnSeq>000001</TrnSeq>
//            <CustNo>1202020497</CustNo>
//            <TrnDt>01040611</TrnDt>
//            <TrnTime>180628</TrnTime>
//            <TrnAmt>65610.00</TrnAmt>
//            <HCode>0</HCode>
//            <Source>01</Source>
//            <MainDt>01040611</MainDt>
//            <BrNo>041</BrNo>
//            <Memo>930023-32-D70-C     </Memo>
//            <Telegram>908410100000000011D700001D 0033801910000000104061101040611000       ~ 5026000001120202049701040611180628000000656100000101040611041930023-32-D70-C     930023-041-1032-D7000-C                 </Telegram>
//        </D338Rq>
//    </BankSvcRq>
//</LandBankML>
//";
                #endregion

                processEAI(content);

                Response.Write("ok");
                Response.End();
                return;
            }
        }
    }
}