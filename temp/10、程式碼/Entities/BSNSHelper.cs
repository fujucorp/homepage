using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Fuju;
using Fuju.Configuration;
using Entities.MsgHandler;

namespace Entities
{
    public class BSNSHelper
    {
        #region Member
        private string _ClientIP = null;

        /// <summary>
        /// 業務通道代碼
        /// </summary>
        private string _Channel = null;
        /// <summary>
        /// 電文來源代碼
        /// </summary>
        private string _TelegramSource = null;
        /// <summary>
        /// 產品代碼
        /// </summary>
        private string _ProductID = null;
        /// <summary>
        /// 分類代碼
        /// </summary>
        private string _NotificationClassID = null;
        #endregion

        public BSNSHelper()
        {
            _ClientIP = this.GetClientIP();

            _Channel = ConfigManager.Current.GetProjectConfigValue("BSNS", "Channel", StringComparison.CurrentCultureIgnoreCase);
            _TelegramSource = ConfigManager.Current.GetProjectConfigValue("BSNS", "TelegramSource", StringComparison.CurrentCultureIgnoreCase);
            _ProductID = ConfigManager.Current.GetProjectConfigValue("BSNS", "ProductID", StringComparison.CurrentCultureIgnoreCase);
            _NotificationClassID = ConfigManager.Current.GetProjectConfigValue("BSNS", "NotificationClassID", StringComparison.CurrentCultureIgnoreCase);
        }

        private string GetClientIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress address in host.AddressList)
            {
                // 只取得IP V4的Address
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return address.ToString();
                }
            }
            return String.Empty;
        }

        public bool IsReady()
        {
            return !String.IsNullOrEmpty(_Channel) && !String.IsNullOrEmpty(_TelegramSource) 
                && !String.IsNullOrEmpty(_ProductID) && !String.IsNullOrEmpty(_NotificationClassID);
        }

        /// <summary>
        /// 產生發送 Mail 的 Request Xml
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="toMail"></param>
        /// <param name="toName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string GenMailRqXml(string subject, string toMail, string toName, string content, CodeText[] contentPARAMs)
        {
            string xml = null;

            using (MemoryStream ms = new MemoryStream())
            {
                #region Sample
                //<BlueStar MsgName="BSNS:EventFactory" App="XML" xmlns="" ClientIP="10.101.11.62" RqUid="b7c92e5a-a693-4045-8e78-9026401f959a">
                //    <BSNS_Channel>MF</BSNS_Channel>
                //    <BSNS_TelegramSource>MF</BSNS_TelegramSource>
                //    <BSNS_ProductID>DD</BSNS_ProductID>
                //    <BSNS_NotificationClassID>01</BSNS_NotificationClassID>
                //    <BSNS_CustomerID>09112345</BSNS_CustomerID>
                //    <BSNS_CustomerName>Customer Name</BSNS_CustomerName>
                //    <BSNS_AccountNumber />
                //    <BSNS_AccountCurrency />
                //    <BSNS_TFACode />
                //    <BSNS_TxnDate />
                //    <BSNS_SentDate />
                //    <BSNS_SentTime />
                //    <BSNS_JournalSeqNO />
                //    <BSNS_IsBypassBSNS>Y</BSNS_IsBypassBSNS>
                //    <BSNS_ContactWindowSet>
                //        <BSNS_ContactWindow>
                //            <BSNS_DeliveryChannel>E</BSNS_DeliveryChannel>
                //            <BSNS_ContactName>081396</BSNS_ContactName>
                //            <BSNS_ContactAddress>081396@yahoo.com.tw</BSNS_ContactAddress>
                //            <BSNS_Language>zh-tw</BSNS_Language>
                //        </BSNS_ContactWindow>
                //    </BSNS_ContactWindowSet>
                //    <NotificationTitle />
                //    <ContentText>Hello World</ContentText>
                //    <ContentXML>
                //        <Head>主管您好~</Head>
                //        <Body>信用卡系統有資料需要放行!!</Body>
                //    </ContentXML>
                //</BlueStar>
                #endregion

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.CheckCharacters = false;
                settings.Encoding = Encoding.UTF8;
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                settings.OmitXmlDeclaration = true;

                using (XmlWriter xw = XmlWriter.Create(ms, settings))
                {
                    #region 根節點開始
                    xw.WriteStartElement("BlueStar", "");
                    xw.WriteAttributeString("MsgName", "BSNS:EventFactory");
                    xw.WriteAttributeString("App", "XML");
                    if (!String.IsNullOrEmpty(_ClientIP))
                    {
                        xw.WriteAttributeString("ClientIP", _ClientIP);
                    }
                    xw.WriteAttributeString("RqUid", System.Guid.NewGuid().ToString("D"));
                    #endregion

                    #region 業務通道代碼 BSNS_Channel
                    xw.WriteElementString("BSNS_Channel", _Channel ?? String.Empty);
                    #endregion

                    #region 電文來源代碼 BSNS_TelegramSource
                    xw.WriteElementString("BSNS_TelegramSource", _TelegramSource ?? String.Empty);
                    #endregion

                    #region 產品代碼 BSNS_ProductID
                    xw.WriteElementString("BSNS_ProductID", _ProductID ?? String.Empty);
                    #endregion

                    #region 分類代碼 BSNS_NotificationClassID
                    xw.WriteElementString("BSNS_NotificationClassID", _NotificationClassID ?? String.Empty);
                    #endregion

                    #region 客戶代碼 BSNS_CustomerID
                    xw.WriteElementString("BSNS_CustomerID", String.Empty);
                    #endregion

                    #region 客戶名稱 BSNS_CustomerName
                    xw.WriteElementString("BSNS_CustomerName", String.Empty);
                    #endregion

                    #region [保留，傳不提供]
                    //#region 帳戶代碼 BSNS_AccountNumber
                    //xw.WriteElementString("BSNS_AccountNumber", String.Empty);
                    //#endregion

                    //#region 帳戶幣別 BSNS_AccountCurrency
                    //xw.WriteElementString("BSNS_AccountCurrency", String.Empty);
                    //#endregion

                    //#region 企業識別碼 BSNS_TFACode
                    //xw.WriteElementString("BSNS_TFACode", String.Empty);
                    //#endregion

                    //#region 交易日期 BSNS_TxnDate (yyyyMMdd)
                    //xw.WriteElementString("BSNS_TxnDate", String.Empty);
                    //#endregion

                    //#region 發送日期 BSNS_SentDate (yyyyMMdd)
                    //xw.WriteElementString("BSNS_SentDate", String.Empty);
                    //#endregion

                    //#region 發送時間 BSNS_SentTime (HHmmss)
                    //xw.WriteElementString("BSNS_SentTime", String.Empty);
                    //#endregion

                    //#region 交易序號 BSNS_JournalSeqNO
                    //xw.WriteElementString("BSNS_SentTime", String.Empty);
                    //#endregion
                    #endregion

                    #region 非訂閱訊息(Y/N) 固定填 Y BSNS_IsBypassBSNS (yyyy/mm/dd hh:mm)
                    xw.WriteElementString("BSNS_IsBypassBSNS", "Y");
                    #endregion

                    #region [保留，傳不提供]
                    //#region 排程發送時間 ScheduledSendTime
                    //xw.WriteElementString("ScheduledSendTime", "");
                    //#endregion

                    //#region 批次序號 BatchID
                    //xw.WriteElementString("BatchID", "Y");
                    //#endregion
                    #endregion

                    #region 連絡區塊 BSNS_ContactWindowSet
                    xw.WriteStartElement("BSNS_ContactWindowSet");

                    #region 連絡資訊 BSNS_ContactWindow
                    xw.WriteStartElement("BSNS_ContactWindow");

                    #region 發送管道 BSNS_DeliveryChannel (E：email, S：簡訊, F：傳真)
                    xw.WriteElementString("BSNS_DeliveryChannel", "E");
                    #endregion

                    #region 連絡人 BSNS_ContactName
                    xw.WriteElementString("BSNS_ContactName", toName);
                    #endregion

                    #region 發送位址 BSNS_ContactAddress
                    xw.WriteElementString("BSNS_ContactAddress", toMail);
                    #endregion

                    #region 語系 BSNS_Language
                    xw.WriteElementString("BSNS_Language", "zh-tw");
                    #endregion

                    xw.WriteEndElement();
                    #endregion

                    xw.WriteEndElement();
                    #endregion

                    #region 訊息標題 NotificationTitle
                    xw.WriteElementString("NotificationTitle", subject);
                    #endregion

                    #region 內容 ContentXML
                    xw.WriteStartElement("ContentXML");

                    #region 郵件本文 Body
                    if (!String.IsNullOrWhiteSpace(content))
                    {
                        xw.WriteElementString("Body", content);
                    }
                    else
                    {
                        xw.WriteStartElement("Body", "");
                        if (contentPARAMs != null && contentPARAMs.Length > 0)
                        {
                            foreach (CodeText contentPARAM in contentPARAMs)
                            {
                                xw.WriteElementString(contentPARAM.Code, contentPARAM.Text);
                            }
                        }
                        xw.WriteEndElement();
                    }
                    #endregion

                    xw.WriteEndElement();
                    #endregion

                    #region [保留，傳不提供]
                    //#region 附檔區塊 Attachments
                    //xw.WriteStartElement("Attachments");

                    //#region 附檔資訊 Attachment
                    //xw.WriteElementString("Attachment", "");
                    //#endregion

                    //xw.WriteEndElement();
                    //#endregion
                    #endregion

                    #region 根節點結束
                    xw.WriteEndElement();
                    #endregion

                    xw.Flush();
                }

                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                {
                    xml = sr.ReadToEnd();
                }
            }

            return xml;
        }

        /// <summary>
        /// 解析發送 Mail 的 Response Xml
        /// </summary>
        /// <param name="rsXml"></param>
        /// <param name="statusCode"></param>
        /// <param name="statusDesc"></param>
        /// <returns></returns>
        public bool ParseMailRsXml(string rsXml, out string statusCode, out string statusDesc)
        {
            #region Sample
            //<BlueStar RqUid="b7c92e5a-a693-4045-8e78-9026401f959a" xmlns="http://www.cedar.com.tw/bluestar/" Status="0">
            //    <StatusCode>0</StatusCode>
            //    <StatusDesc>Success</StatusDesc>
            //    <BSNS_EventID>21</BSNS_EventID>
            //</BlueStar>
            #endregion

            bool isOK = false;
            statusCode = null;
            statusDesc = null;
            if (!String.IsNullOrEmpty(rsXml))
            {
                rsXml =rsXml.Trim().Replace(@"MsgName=""BSNS:EventFactory""", "").Replace(@"App=""XML""", "");
                try
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(rsXml);
                    
                    XmlNode xStatusCodeNode = xDoc.DocumentElement.SelectSingleNode("StatusCode");
                    if (xStatusCodeNode != null)
                    {
                        statusCode = xStatusCodeNode.InnerText.Trim();
                    }

                    XmlNode xStatusDescNode = xDoc.DocumentElement.SelectSingleNode("StatusDesc");
                    if (xStatusDescNode != null)
                    {
                        statusDesc = xStatusDescNode.InnerText.Trim();
                    }
                    isOK = true;
                }
                catch (Exception ex)
                {
                    isOK = false;
                    statusDesc = ex.Message;
                }
            }
            return isOK;
        }

        /// <summary>
        /// 發送 mail
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="toMail"></param>
        /// <param name="toName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string SendMail(string subject, string toMail, string toName, string content, CodeText[] contentPARAMs)
        {
            string rqXml = this.GenMailRqXml(subject, toMail, toName, content, contentPARAMs);
            string rsXml = null;

            string endpointConfigurationName = ConfigHelper.Current.GetMsgHandlerSoapEndpointConfigurationName();
            MsgHandlerSoapClient msg = new MsgHandlerSoapClient(endpointConfigurationName);
            int code = msg.SubmitXmlString(rqXml, out rsXml);

            string statusCode = null;
            string statucDesc = null;
            bool isOK = this.ParseMailRsXml(rsXml, out statusCode, out statucDesc);
            if (isOK)
            {
                if (statusCode == "0")
                {
                    return null;
                }
                else
                {
                    return statucDesc;
                }
            }
            else
            {
                return "回覆電文解析失敗，" + statucDesc;
            }
        }
    }
}
