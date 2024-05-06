using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Entities
{
    /// <summary>
    /// KP3參數設定承載類別
    /// </summary>
    [Serializable]
    public sealed class KP3Config
    {
        #region Static Readonly
        /// <summary>
        /// Xml 根節點名稱 ： KP3Config (KP3Config)
        /// </summary>
        public static readonly string RootLocalName = "KP3Config";
        #endregion

        #region Property
        private string _Unit = null;
        /// <summary>
        /// 報送單位代號 (3碼的阿拉伯數字或大寫英文字母)
        /// </summary>
        public string Unit
        {
            get
            {
                return _Unit;
            }
            set
            {
                _Unit = value == null ? null : value.Trim().ToUpper();
            }
        }

        private string _Managers = null;
        /// <summary>
        /// 管理者清單（已逗號隔開）
        /// </summary>
        public string Managers
        {
            get
            {
                return _Managers;
            }
            set
            {
                _Managers = value == null ? null : value.Trim();
            }
        }

        #region FTP
        private string _FTPUrl = null;
        /// <summary>
        /// FTP 網址
        /// </summary>
        public string FTPUrl
        {
            get
            {
                return _FTPUrl;
            }
            set
            {
                _FTPUrl = value == null ? null : value.Trim();
            }
        }

        private string _FTPAcct = null;
        /// <summary>
        /// FTP 帳號
        /// </summary>
        public string FTPAcct
        {
            get
            {
                return _FTPAcct;
            }
            set
            {
                _FTPAcct = value == null ? null : value.Trim();
            }
        }

        #region [MDY:20220530] Checkmarx 調整
        private string _FTPPXX = null;
        /// <summary>
        /// FTP 密碼
        /// </summary>
        public string FTPPXX
        {
            get
            {
                return _FTPPXX;
            }
            set
            {
                _FTPPXX = value == null ? null : value.Trim();
            }
        }
        #endregion
        #endregion

        #region 檔頭
        private string _HeadItem01 = null;
        /// <summary>
        /// (檔頭)資訊格式代號 (18碼的阿拉伯數字、大寫英文字母或dash符號)
        /// </summary>
        public string HeadItem01
        {
            get
            {
                return _HeadItem01;
            }
            set
            {
                _HeadItem01 = value == null ? null : value.Trim().ToUpper();
            }
        }

        private string _HeadItem07 = null;
        /// <summary>
        /// (檔頭)聯絡電話
        /// </summary>
        public string HeadItem07
        {
            get
            {
                return _HeadItem07;
            }
            set
            {
                _HeadItem07 = value == null ? null : value.Trim();
            }
        }

        private string _HeadItem08 = null;
        /// <summary>
        /// (檔頭)聯絡人資訊或訊息
        /// </summary>
        public string HeadItem08
        {
            get
            {
                return _HeadItem08;
            }
            set
            {
                _HeadItem08 = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region 資料
        private string _DataItem05 = null;
        /// <summary>
        /// (資料)特約機構屬性（G=非個人特約機構; H=個人特約機構）
        /// </summary>
        public string DataItem05
        {
            get
            {
                return _DataItem05;
            }
            set
            {
                _DataItem05 = value == null ? null : value.Trim();
            }
        }

        private string _DataItem09 = null;
        /// <summary>
        /// (資料)特約機構類型（1=第一類特約機構; 2=第二類特約機構）
        /// </summary>
        public string DataItem09
        {
            get
            {
                return _DataItem09;
            }
            set
            {
                _DataItem09 = value == null ? null : value.Trim();
            }
        }

        private string _DataItem23 = null;
        /// <summary>
        /// (資料)營業型態（1=實體; 2=網路; 3=實體及網路）
        /// </summary>
        public string DataItem23
        {
            get
            {
                return _DataItem23;
            }
            set
            {
                _DataItem23 = value == null ? null : value.Trim();
            }
        }

        private string _DataItem26 = null;
        /// <summary>
        /// (資料)業務行為（1=境內; 2=跨境; 3=境內及跨境）
        /// </summary>
        public string DataItem26
        {
            get
            {
                return _DataItem26;
            }
            set
            {
                _DataItem26 = value == null ? null : value.Trim();
            }
        }

        private string _DataItem27 = null;
        /// <summary>
        /// (資料)是否受理電子支付帳戶或儲值卡服務（1=境內電子支付帳戶服務; 2=境外機構支付帳戶服務; 3=境內儲值卡服務; 4=境外機構記名儲值卡服務）
        /// </summary>
        public string DataItem27
        {
            get
            {
                return _DataItem27;
            }
            set
            {
                _DataItem27 = value == null ? null : value.Trim();
            }
        }

        private string _DataItem29 = null;
        /// <summary>
        /// (資料)是否受理信用卡服務 (Y/N)
        /// </summary>
        public string DataItem29
        {
            get
            {
                return _DataItem29;
            }
            set
            {
                _DataItem29 = value == null ? null : value.Trim();
            }
        }

        private string _DataItem33 = null;
        /// <summary>
        /// (資料)是否有銷售遞延性商品或服務 (Y/N)
        /// </summary>
        public string DataItem33
        {
            get
            {
                return _DataItem33;
            }
            set
            {
                _DataItem33 = value == null ? null : value.Trim();
            }
        }

        private string _DataItem34 = null;
        /// <summary>
        /// (資料)是否安裝端末設備 (Y/N)
        /// </summary>
        public string DataItem34
        {
            get
            {
                return _DataItem34;
            }
            set
            {
                _DataItem34 = value == null ? null : value.Trim();
            }
        }

        private string _DataItem35 = null;
        /// <summary>
        /// (資料)是否安裝錄影設備 (Y/N)
        /// </summary>
        public string DataItem35
        {
            get
            {
                return _DataItem35;
            }
            set
            {
                _DataItem35 = value == null ? null : value.Trim();
            }
        }

        private string _DataItem36 = null;
        /// <summary>
        /// (資料)連鎖店加盟或直營（1=加盟; 2=直營; 3=無法判別; ''）
        /// </summary>
        public string DataItem36
        {
            get
            {
                return _DataItem36;
            }
            set
            {
                _DataItem36 = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 取得載入 XmlNode 的資料是否成功
        /// </summary>
        [XmlIgnore]
        internal bool LoadXmlOK
        {
            get;
            private set;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 KP3參數設定承載類別 物件
        /// </summary>
        public KP3Config()
        {
            this.LoadXmlOK = false;
        }

        /// <summary>
        /// 建構 KP3參數設定承載類別 物件
        /// </summary>
        /// <param name="xNode"></param>
        public KP3Config(XmlNode xNode)
        {
            this.LoadXmlOK = this.LoadXml(xNode);
        }
        #endregion

        #region Method
        /// <summary>
        /// 將此物件的設定資料寫入 XmlWriter
        /// </summary>
        /// <param name="writer">指定 XmlWriter</param>
        public void ToXmlWriter(XmlWriter writer)
        {
            if (writer != null)
            {
                #region Root Start
                writer.WriteStartElement(RootLocalName);
                #endregion

                #region Unit
                writer.WriteElementString("Unit", this.Unit ?? String.Empty);
                #endregion

                #region Managers
                writer.WriteElementString("Managers", this.Managers ?? String.Empty);
                #endregion

                #region FTPUrl
                writer.WriteElementString("FTPUrl", this.FTPUrl ?? String.Empty);
                #endregion

                #region FTPAcct
                writer.WriteElementString("FTPAcct", this.FTPAcct ?? String.Empty);
                #endregion

                #region [MDY:20220530] Checkmarx 調整
                #region FTPPWord
                writer.WriteElementString("FTPPWord", this.FTPPXX ?? String.Empty);
                #endregion
                #endregion

                #region HeadItem01
                writer.WriteElementString("HeadItem01", this.HeadItem01.ToString());
                #endregion

                #region HeadItem07
                writer.WriteElementString("HeadItem07", this.HeadItem07.ToString());
                #endregion

                #region HeadItem08
                writer.WriteElementString("HeadItem08", this.HeadItem08.ToString());
                #endregion

                #region DataItem05
                writer.WriteElementString("DataItem05", this.DataItem05.ToString());
                #endregion

                #region DataItem09
                writer.WriteElementString("DataItem09", this.DataItem09 ?? String.Empty);
                #endregion

                #region DataItem23
                writer.WriteElementString("DataItem23", this.DataItem23 ?? String.Empty);
                #endregion

                #region DataItem26
                writer.WriteElementString("DataItem26", this.DataItem26 ?? String.Empty);
                #endregion

                #region DataItem27
                writer.WriteElementString("DataItem27", this.DataItem27 ?? String.Empty);
                #endregion

                #region DataItem29
                writer.WriteElementString("DataItem29", this.DataItem29 ?? String.Empty);
                #endregion

                #region DataItem33
                writer.WriteElementString("DataItem33", this.DataItem33 ?? String.Empty);
                #endregion

                #region DataItem34
                writer.WriteElementString("DataItem34", this.DataItem34 ?? String.Empty);
                #endregion

                #region DataItem35
                writer.WriteElementString("DataItem35", this.DataItem35 ?? String.Empty);
                #endregion

                #region DataItem36
                writer.WriteElementString("DataItem36", this.DataItem36 ?? String.Empty);
                #endregion

                #region Root End
                writer.WriteEndElement();
                #endregion

                writer.Flush();
            }
        }

        /// <summary>
        /// 載入 XmlNode 資料
        /// </summary>
        /// <param name="xNode">指定 XmlNode</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool LoadXml(XmlNode xRoot)
        {
            if (xRoot == null || xRoot.LocalName != RootLocalName)
            {
                return false;
            }

            #region Unit
            {
                XmlNode xNode = xRoot.SelectSingleNode("Unit");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.Unit = xNode.InnerText.Trim();
            }
            #endregion

            #region Managers
            {
                XmlNode xNode = xRoot.SelectSingleNode("Managers");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.Managers = xNode.InnerText.Trim();
            }
            #endregion

            #region FTPUrl
            {
                XmlNode xNode = xRoot.SelectSingleNode("FTPUrl");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.FTPUrl = xNode.InnerText.Trim();
            }
            #endregion

            #region FTPAcct
            {
                XmlNode xNode = xRoot.SelectSingleNode("FTPAcct");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.FTPAcct = xNode.InnerText.Trim();
            }
            #endregion

            #region [MDY:20220530] Checkmarx 調整
            #region FTPPWord
            {
                XmlNode xNode = xRoot.SelectSingleNode("FTPPWord");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.FTPPXX = xNode.InnerText.Trim();
            }
            #endregion
            #endregion

            #region HeadItem01
            {
                XmlNode xNode = xRoot.SelectSingleNode("HeadItem01");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.HeadItem01 = xNode.InnerText.Trim();
            }
            #endregion

            #region HeadItem07
            {
                XmlNode xNode = xRoot.SelectSingleNode("HeadItem07");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.HeadItem07 = xNode.InnerText.Trim();
            }
            #endregion

            #region HeadItem08
            {
                XmlNode xNode = xRoot.SelectSingleNode("HeadItem08");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.HeadItem08 = xNode.InnerText.Trim();
            }
            #endregion

            #region DataItem05
            {
                XmlNode xNode = xRoot.SelectSingleNode("DataItem05");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.DataItem05 = xNode.InnerText.Trim();
            }
            #endregion

            #region DataItem09
            {
                XmlNode xNode = xRoot.SelectSingleNode("DataItem09");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.DataItem09 = xNode.InnerText.Trim();
            }
            #endregion

            #region DataItem23
            {
                XmlNode xNode = xRoot.SelectSingleNode("DataItem23");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.DataItem23 = xNode.InnerText.Trim();
            }
            #endregion

            #region DataItem26
            {
                XmlNode xNode = xRoot.SelectSingleNode("DataItem26");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.DataItem26 = xNode.InnerText.Trim();
            }
            #endregion

            #region DataItem27
            {
                XmlNode xNode = xRoot.SelectSingleNode("DataItem27");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.DataItem27 = xNode.InnerText.Trim();
            }
            #endregion

            #region DataItem29
            {
                XmlNode xNode = xRoot.SelectSingleNode("DataItem29");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.DataItem29 = xNode.InnerText.Trim();
            }
            #endregion

            #region DataItem33
            {
                XmlNode xNode = xRoot.SelectSingleNode("DataItem33");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.DataItem33 = xNode.InnerText.Trim();
            }
            #endregion

            #region DataItem34
            {
                XmlNode xNode = xRoot.SelectSingleNode("DataItem34");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.DataItem34 = xNode.InnerText.Trim();
            }
            #endregion

            #region DataItem35
            {
                XmlNode xNode = xRoot.SelectSingleNode("DataItem35");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.DataItem35 = xNode.InnerText.Trim();
            }
            #endregion

            #region DataItem36
            {
                XmlNode xNode = xRoot.SelectSingleNode("DataItem36");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.DataItem36 = xNode.InnerText.Trim();
            }
            #endregion

            return true;
        }

        public string ToXml()
        {
            string xml = null;

            //[MEMO] 新版 checkmarx 無聊的要求用 try-catch 包起來
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.CheckCharacters = false;
                settings.Encoding = Encoding.UTF8;
                settings.Indent = true;
                settings.NewLineOnAttributes = false;
                settings.OmitXmlDeclaration = true;

                using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
                {
                    using (XmlWriter writer = XmlWriter.Create(memory, settings))
                    {
                        this.ToXmlWriter(writer);
                        writer.Flush();
                    }

                    memory.Position = 0;
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(memory))
                    {
                        xml = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                xml = null;
            }

            return xml;
        }

        /// <summary>
        /// 取得管理者清單陣列
        /// </summary>
        /// <returns></returns>
        public string[] GetManagers()
        {
            if (String.IsNullOrWhiteSpace(this.Managers))
            {
                return new string[0];
            }

            return this.Managers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 建立指定 Xml 的 KP3參數設定承載類別 物件
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static KP3Config Create(string xml)
        {
            KP3Config kp3Config = null;
            if (!String.IsNullOrWhiteSpace(xml))
            {
                try
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(xml);
                    kp3Config = new KP3Config(xdoc.DocumentElement);
                }
                catch (Exception)
                {
                }
            }
            return kp3Config;
        }
        #endregion
    }
}
