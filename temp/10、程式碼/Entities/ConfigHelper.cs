using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fuju;
using Fuju.Configuration;
using Fuju.DB;
using Fuju.DB.Configuration;

namespace Entities
{
    #region [Old]
    //public sealed class ConfigHelper
    //{
    //    public class pair
    //    {
    //        public string key = "";
    //        public string value = "";
    //    }

    //    public ConfigHelper()
    //    {

    //    }


    //    public bool GetQNACategory(out pair[] items,out string msg)
    //    {
    //        bool rc = false;
    //        items = null;
    //        msg = "";

    //        List<pair> datas = new List<pair>();

    //        string config_key = "QNACategory";
    //        ConfigEntity[] configs = null;
    //        Expression where = new Expression(ConfigEntity.Field.ConfigKey, RelationEnum.Equal, config_key);
    //        KeyValueList<OrderByEnum> orderbys = null;

    //        EntityFactory factory = new EntityFactory();
    //        Result result = factory.SelectAll<ConfigEntity>(where, orderbys, out configs);
    //        if (!result.IsSuccess)
    //        {
    //            msg = string.Format("查詢設定檔發生錯誤，key={0}，錯誤訊息={1}", config_key, result.Message);
    //            return rc;
    //        }

    //        if (configs == null || configs.Length <= 0)
    //        {
    //            msg = string.Format("查詢設定檔發生錯誤，key={0}，錯誤訊息={1}", config_key, "查無資料");
    //            return rc;
    //        }

    //        //只會有一筆
    //        //key_value的組成為  key1=values;key2=value2;...
    //        string tmp = configs[0].ConfigValue.Trim();
    //        if (tmp == "")
    //        {
    //            msg = string.Format("查詢設定檔發生錯誤，key={0}，錯誤訊息={1}", config_key, "設定為空白");
    //            return rc;
    //        }
    //        string[] buff = tmp.Split(';');
    //        foreach (string t in buff)
    //        {
    //            pair item = new pair();
    //            try
    //            {
    //                item.key = t.Split('=')[0].Trim(); ;
    //                item.value = t.Split('=')[1].Trim();
    //                datas.Add(item);
    //            }
    //            catch (Exception)
    //            {
    //                //基本上是自己設定，所以先不防呆
    //            }
    //        }
    //        if (datas != null && datas.Count > 0)
    //        {
    //            items = datas.ToArray<pair>();
    //        }
    //        return rc;
    //    }
    //}
    #endregion

    public class ConfigHelper
    {
        #region Static Property
        private static ConfigHelper _Current;
        /// <summary>
        /// 取得目前的資料處理代理類別物件，如果目前沒有則傳回一個新的預設參數資料處理代理類別物件
        /// </summary>
        public static ConfigHelper Current
        {
            get
            {
                if (_Current == null)
                {
                    _Current = new ConfigHelper();
                }
                return _Current;
            }
        }
        #endregion

        #region Constructor
        private ConfigHelper()
        {

        }
        #endregion

        #region Method
        /// <summary>
        /// 系統組態的電腦群組清單群組常數定義
        /// </summary>
        public const string ConfigMachineGroupKey = "MachineGroup";

        private string _MyConfigMachineGroupValue = null;
        /// <summary>
        /// 取得本機的所屬電腦群組
        /// </summary>
        /// <returns>傳回本機的所屬電腦群組，或空字串  </returns>
        public string GetMyConfigMachineGroupValue()
        {
            if (_MyConfigMachineGroupValue == null)
            {
                string value = ConfigManager.Current.GetSystemConfigValue(ConfigMachineGroupKey, Environment.MachineName, StringComparison.CurrentCultureIgnoreCase);
                if (String.IsNullOrWhiteSpace(value))
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

        /// <summary>
        /// 取得本機所屬的指定組態群組
        /// </summary>
        /// <param name="group">指定組態群組</param>
        /// <returns>傳回本機所屬的指定組態群組，或 null</returns>
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

        /// <summary>
        /// 取得本機所屬的指定組態名稱
        /// </summary>
        /// <param name="name">指定組態名稱</param>
        /// <returns>傳回本機所屬的指定組態名稱</returns>
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

        /// <summary>
        /// 取得指定專案組態群組、專案組態名稱的組態值
        /// </summary>
        /// <param name="group">指定組態群組</param>
        /// <param name="name">指定組態名稱</param>
        /// <param name="comparisonType">指定比較方法</param>
        /// <returns>傳回組態值，或 null</returns>
        public string GetMyProjectConfigValue(string group, string name, StringComparison? comparisonType = null)
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

        /// <summary>
        /// 取得指定資料庫組態名稱的組態值
        /// </summary>
        /// <param name="dbConfigName">指定資料庫組態名稱</param>
        /// <param name="comparisonType">指定比較方法</param>
        /// <returns>傳回組態值，或 null</returns>
        public ConnectionSetting GetMyDBConnectionSetting(string dbConfigName, StringComparison? comparisonType = null)
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

        /// <summary>
        /// 取得行員 AD 驗證服務 (UserServiceExSoap) 的 EndpointConfigurationName
        /// </summary>
        /// <returns></returns>
        public string GetUserServiceExSoapEndpointConfigurationName()
        {
            string myMachineConfigName = ConfigManager.Current.GetMyMachineConfigName("UserServiceExSoap");
            return myMachineConfigName;
        }

        /// <summary>
        /// 取得 Web 端檔案服務 (FileServiceSoap) 的 EndpointConfigurationName
        /// </summary>
        /// <returns></returns>
        public string GetFileServiceSoapEndpointConfigurationName()
        {
            string myMachineConfigName = ConfigManager.Current.GetMyMachineConfigName("FileServiceSoap");
            return myMachineConfigName;
        }

        /// <summary>
        /// 取得 BSNS 寄信服務 (MsgHandlerSoap) 的 EndpointConfigurationName
        /// </summary>
        /// <returns></returns>
        public string GetMsgHandlerSoapEndpointConfigurationName()
        {
            string myMachineConfigName = ConfigManager.Current.GetMyMachineConfigName("MsgHandlerSoap");
            return myMachineConfigName;
        }
        #endregion
    }
}
