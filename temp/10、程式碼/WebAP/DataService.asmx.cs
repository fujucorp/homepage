using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using Fuju;
using Fuju.DB;
using Fuju.Configuration;
using Fuju.Web;
using Fuju.Web.Services;

using Entities;
using Helpers;

namespace WebAP
{
    /// <summary>
    /// 學雜費後台資料處理服務
    /// </summary>
    [WebService(Namespace = "http://fuju.com.tw/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class DataService : BaseService
    {
        #region Constructor
        /// <summary>
        /// 建構後台資料存取服務物件，並使用預組態設定
        /// </summary>
        public DataService()
            : base()
        {
            //如果使用設計的元件，請取消註解下行程式碼 
            //InitializeComponent(); 
        }
        #endregion

        #region Implement BaseService's Method
        /// <summary>
        /// 取得服務執行器
        /// </summary>
        /// <returns></returns>
        protected override IServiceExecutor GetServiceExecutor()
        {
            DataServiceExecutor executor =  new DataServiceExecutor();
            executor.TempPath = ConfigManager.Current.GetSystemConfigValue("DATA_SERVICE", "TEMP_PATH");
            return executor;
        }

        #region 友善訊息範例
        ///// <summary>
        ///// 轉換處理結果類別成為要回傳的序列化 Xml (要做多語系、友善訊息時 override 此方法)
        ///// </summary>
        ///// <param name="xmlResult"></param>
        ///// <returns></returns>
        //protected override string GetReturnXml(XmlResult xmlResult)
        //{
        //    #region 友善訊息處理
        //    //對 xmlResult.Code 取得友善訊息後，指定給 xmlResult.Message
        //    #endregion

        //    return base.GetReturnXml(xmlResult);
        //}
        #endregion
        #endregion
    }
}
