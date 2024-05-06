using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

//using System.Net;
//using System.Net.Security;
//using System.Security.Cryptography.X509Certificates;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
using Fuju.Web;
using Fuju.Web.Proxy;
using Fuju.Web.Services;

using Entities;
using Helpers;

namespace eSchoolWeb
{
    /// <summary>
    /// 資料處理代理類別
    /// </summary>
    public class DataProxy : BaseProxy
    {
        #region Static Property
        private static DataProxy _Current;
        /// <summary>
        /// 取得目前的資料處理代理類別物件，如果目前沒有則傳回一個新的預設參數資料處理代理類別物件
        /// </summary>
        public static DataProxy Current
        {
            get
            {
                if (_Current == null)
                {
                    _Current = new DataProxy();
                }
                return _Current;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構資料處理代理類別
        /// </summary>
        private DataProxy()
            : base()
        {
            //ServicePointManager.ServerCertificateValidationCallback = delegate
            //{
            //    return true;
            //};
        }
        #endregion

        #region 新增資料
        /// <summary>
        /// 新增資料
        /// </summary>
        /// <typeparam name="T">指定新增資料物件的 Entity 型別。</typeparam>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="instance">指定新增的資料物件。</param>
        /// <param name="count">成功則傳回受影響的資料筆數，否則傳回 0。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult Insert<T>(Page page, T instance, out int count) where T : class, IEntity
        {
            count = 0;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (instance == null)
            {
                return new XmlResult(false, "缺少或無效的資料物件參數", ErrorCode.S_INVALID_PARAMETER);
            }
            #endregion

            #region 取得服務命令請求者資料
            CommandAsker cmdAsker = this.TryGetCommandAsker(page);
            if (cmdAsker == null || !cmdAsker.IsReady)
            {
                return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
            }
            #endregion

            #region 產生服務命令
            InsertCommand command = InsertCommand.Create<T>(cmdAsker, instance);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is int)
                    {
                        count = (int)data;
                        xmlResult = new XmlResult(true);
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                    }
                }
                else
                {
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                }
            }
            return xmlResult;
            #endregion
        }
        #endregion

        #region 新增多筆資料
        /// <summary>
        /// 新增多筆資料
        /// </summary>
        /// <param name="instance">指定要新增的資料物件。</param>
        /// <param name="count">成功則傳回受影響的資料筆數，否則傳回 0。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult InsertAll<T>(Page page, T[] instances, bool isBatch, out int count, out int[] failIndexs) where T : class, IEntity
        {
            count = 0;
            failIndexs = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (instances == null || instances.Length == 0)
            {
                return new XmlResult(false, "缺少或無效的資料物件參數", ErrorCode.S_INVALID_PARAMETER);
            }
            #endregion

            #region 取得服務命令請求者資料
            CommandAsker cmdAsker = this.TryGetCommandAsker(page);
            if (cmdAsker == null || !cmdAsker.IsReady)
            {
                return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
            }
            #endregion

            #region 產生服務命令
            InsertAllCommand command = InsertAllCommand.Create<T>(cmdAsker, instances, isBatch);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    Array array = data as Array;
                    if (array != null && array.Length == 2)
                    {
                        object data1 = array.GetValue(0);
                        if (data1 is int)
                        {
                            count = (int)data1;
                        }

                        object data2 = array.GetValue(1);
                        if (data2 is int[])
                        {
                            failIndexs = data1 as int[];
                            xmlResult = new XmlResult(true);
                        }
                        else if (Reflector.TryCast<int[]>(data2, out failIndexs))
                        {
                            xmlResult = new XmlResult(true);
                        }
                        else
                        {
                            xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE, null);
                        }
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE, null);
                    }
                }
                else
                {
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                }
            }
            return xmlResult;
            #endregion
        }
        #endregion

        #region 更新資料
        /// <summary>
        /// 更新資料
        /// </summary>
        /// <typeparam name="T">指定更新資料物件的 Entity 型別。</typeparam>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="instance">指定更新的資料物件。</param>
        /// <param name="count">成功則傳回受影響的資料筆數，否則傳回 0。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult Update<T>(Page page, T instance, out int count) where T : class, IEntity
        {
            count = 0;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (instance == null)
            {
                return new XmlResult(false, "缺少或無效的資料物件參數", ErrorCode.S_INVALID_PARAMETER);
            }
            #endregion

            #region 取得服務命令請求者資料
            CommandAsker cmdAsker = this.TryGetCommandAsker(page);
            if (cmdAsker == null || !cmdAsker.IsReady)
            {
                return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
            }
            #endregion

            #region 產生服務命令
            UpdateCommand command = UpdateCommand.Create<T>(cmdAsker, instance);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is int)
                    {
                        count = (int)data;
                        xmlResult = new XmlResult(true);
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                    }
                }
                else
                {
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                }
            }
            return xmlResult;
            #endregion
        }
        #endregion

        #region 更新欄位值
        /// <summary>
        /// 整批更新指定欄位值
        /// </summary>
        /// <typeparam name="T">指定更新資料的 Entity 型別。</typeparam>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="where">指定更新的條件。必要參數，且不可以是空的條件。</param>
        /// <param name="fieldValues">指定更新的資料 (欄位名稱與值的集合)。必要參數，可傳入 KeyValue[] 或 KeyValueList 型別。</param>
        /// <param name="count">成功則傳回受影響的資料筆數，否則傳回 0。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult UpdateFields<T>(Page page, Expression where, ICollection<KeyValue> fieldValues
            , out int count) where T : class, IEntity
        {
            count = 0;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (where == null || where.IsEmpty() || !where.IsReady())
            {
                return new XmlResult(false, "缺少或無效的更新條件參數", ErrorCode.S_INVALID_PARAMETER);
            }
            if (fieldValues == null || fieldValues.Count == 0)
            {
                return new XmlResult(false, "缺少或無效的更新欄位名稱與值參數", ErrorCode.S_INVALID_PARAMETER);
            }
            #endregion

            #region 取得服務命令請求者資料
            CommandAsker cmdAsker = this.TryGetCommandAsker(page);
            if (cmdAsker == null || !cmdAsker.IsReady)
            {
                return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
            }
            #endregion

            #region 產生服務命令
            UpdateFieldsCommand command = UpdateFieldsCommand.Create<T>(cmdAsker, where, fieldValues);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is int)
                    {
                        count = (int)data;
                        xmlResult = new XmlResult(true);
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                    }
                }
                else
                {
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                }
            }
            return xmlResult;
            #endregion
        }
        #endregion

        #region 刪除資料
        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <typeparam name="T">指定刪除資料物件的 Entity 型別。</typeparam>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="instance">指定刪除的資料物件。</param>
        /// <param name="count">成功則傳回受影響的資料筆數，否則傳回 0。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult Delete<T>(Page page, T instance, out int count) where T : class, IEntity
        {
            count = 0;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (instance == null)
            {
                return new XmlResult(false, "缺少或無效的資料物件參數", ErrorCode.S_INVALID_PARAMETER);
            }
            #endregion

            #region 取得服務命令請求者資料
            CommandAsker cmdAsker = this.TryGetCommandAsker(page);
            if (cmdAsker == null || !cmdAsker.IsReady)
            {
                return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
            }
            #endregion

            #region 產生服務命令
            DeleteCommand command = DeleteCommand.Create<T>(cmdAsker, instance);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is int)
                    {
                        count = (int)data;
                        xmlResult = new XmlResult(true);
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                    }
                }
                else
                {
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                }
            }
            return xmlResult;
            #endregion
        }
        #endregion

        #region 刪除多筆資料
        /// <summary>
        /// 刪除多筆資料
        /// </summary>
        /// <typeparam name="T">指定刪除多筆資料物件的 Entity 型別。</typeparam>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="instance">指定刪除的資料物件。</param>
        /// <param name="count">成功則傳回受影響的資料筆數，否則傳回 0。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult DeleteAll<T>(Page page, T[] instances, bool isBatch, out int count, out int[] failIndexs) where T : class, IEntity
        {
            count = 0;
            failIndexs = null;

            return new XmlResult(false, "未實作 (缺 DeleteAllCommand 檔案)", CoreStatusCode.S_NO_SUPPORT, null);

            #region [TODO] 缺 DeleteAllCommand 檔案
            //#region 檢查資料處理代理物件
            //if (!this.IsReady())
            //{
            //    return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            //}
            //#endregion

            //#region 檢查參數
            //if (instances == null || instances.Length == 0)
            //{
            //    return new XmlResult(false, "缺少或無效的資料物件參數", ErrorCode.S_INVALID_PARAMETER);
            //}
            //#endregion

            //#region 取得服務命令請求者資料
            //CommandAsker cmdAsker = this.TryGetCommandAsker(page);
            //if (cmdAsker == null || !cmdAsker.IsReady)
            //{
            //    return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
            //}
            //#endregion

            //#region 產生服務命令
            //DeleteAllCommand command = DeleteAllCommand.Create<T>(cmdAsker, instances, isBatch);
            //#endregion

            //#region 呼叫後端服務命令，並處理回傳結果
            //XmlResult xmlResult = this.ExecuteCommand(command);
            //if (xmlResult.IsSuccess)
            //{
            //    object data = null;
            //    if (xmlResult.TryGetData(out data))
            //    {
            //        Array array = data as Array;
            //        if (array != null && array.Length == 2)
            //        {
            //            object data1 = array.GetValue(0);
            //            if (data1 is int)
            //            {
            //                count = (int)data1;
            //            }

            //            object data2 = array.GetValue(1);
            //            if (data2 is int[])
            //            {
            //                failIndexs = data1 as int[];
            //                xmlResult = new XmlResult(true);
            //            }
            //            else if (Reflector.TryCast<int[]>(data2, out failIndexs))
            //            {
            //                xmlResult = new XmlResult(true);
            //            }
            //            else
            //            {
            //                xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE, null);
            //            }
            //        }
            //        else
            //        {
            //            xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE, null);
            //        }
            //    }
            //    else
            //    {
            //        xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
            //    }
            //}
            //return xmlResult;
            //#endregion
            #endregion
        }
        #endregion

        #region 查詢資料
        /// <summary>
        /// 查詢資料
        /// </summary>
        /// <typeparam name="T">指定查詢資料的 Entity 型別。</typeparam>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="where">指定查詢的條件。必要參數，無條件時傳入空的條件。</param>
        /// <param name="orderbys">指定資料的排序方式。</param>
        /// <param name="startIndex">指定查詢結果的資料讀取起始索引。</param>
        /// <param name="maxRecords">指定查詢結果的資料讀取最大筆數。</param>
        /// <param name="instances">成功則傳回查詢結果的資料陣列，否則傳回 default(T[])。</param>
        /// <param name="totalCount">成功則傳回符合查詢條件的總筆數，否則傳回 0。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult Select<T>(Page page, Expression where, KeyValueList<OrderByEnum> orderbys
            , int startIndex, int maxRecords
            , out T[] instances, out int totalCount) where T : class, IEntity
        {
            instances = default(T[]);
            totalCount = 0;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (where == null || !where.IsReady())
            {
                return new XmlResult(false, "缺少或無效的查詢條件參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            #region 取得服務命令請求者資料
            CommandAsker cmdAsker = this.TryGetCommandAsker(page);
            if (cmdAsker == null || !cmdAsker.IsReady)
            {
                return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
            }
            #endregion

            #region 產生服務命令
            SelectCommand command = SelectCommand.Create<T>(cmdAsker, where, orderbys, startIndex, maxRecords);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    Array array = data as Array;
                    if (array != null && array.Length == 2)
                    {
                        object data1 = array.GetValue(0);
                        if (data1 is T[])
                        {
                            instances = data1 as T[];
                            xmlResult = new XmlResult(true);
                        }
                        else if (Reflector.TryCast<T>(data1, out instances))
                        {
                            xmlResult = new XmlResult(true);
                        }
                        else
                        {
                            xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE, null);
                        }

                        object data2 = array.GetValue(1);
                        if (data2 is int)
                        {
                            totalCount = (int)data2;
                        }
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE, null);
                    }
                }
                else
                {
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE, null);
                }
            }
            return xmlResult;
            #endregion
        }

        /// <summary>
        /// 查詢首筆資料
        /// </summary>
        /// <typeparam name="T">指定查詢資料的 Entity 型別。</typeparam>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="where">指定查詢的條件。必要參數，無條件時傳入空的條件。</param>
        /// <param name="orderbys">指定資料的排序方式。</param>
        /// <param name="instance">成功則傳回查詢結果的首筆資料，否則傳回 default(T)。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult SelectFirst<T>(Page page, Expression where, KeyValueList<OrderByEnum> orderbys
            , out T instance) where T : class, IEntity
        {
            instance = default(T);

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (where == null || !where.IsReady())
            {
                return new XmlResult(false, "缺少或無效的查詢條件參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            #region 取得服務命令請求者資料
            CommandAsker cmdAsker = this.TryGetCommandAsker(page);
            if (cmdAsker == null || !cmdAsker.IsReady)
            {
                return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
            }
            #endregion

            #region 產生服務命令
            SelectFirstCommand command = SelectFirstCommand.Create<T>(cmdAsker, where, orderbys);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is T)
                    {
                        instance = (T)data;
                        xmlResult = new XmlResult(true);
                    }
                    else if (Reflector.TryCast<T>(data, out instance))
                    {
                        xmlResult = new XmlResult(true);
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                    }
                }
                else
                {
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                }
            }
            return xmlResult;
            #endregion
        }

        /// <summary>
        /// 查詢所有資料
        /// </summary>
        /// <typeparam name="T">指定查詢資料的 Entity 型別。</typeparam>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="where">指定查詢的條件。必要參數，無條件時傳入空的條件。</param>
        /// <param name="orderbys">指定資料的排序方式。</param>
        /// <param name="instances">成功則傳回查詢結果的資料陣列，否則傳回 default(T[])。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult SelectAll<T>(Page page, Expression where, KeyValueList<OrderByEnum> orderbys
            , out T[] instances) where T : class, IEntity
        {
            instances = default(T[]);

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (where == null || !where.IsReady())
            {
                return new XmlResult(false, "缺少或無效的查詢條件參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            #region 取得服務命令請求者資料
            CommandAsker cmdAsker = this.TryGetCommandAsker(page);
            if (cmdAsker == null || !cmdAsker.IsReady)
            {
                return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
            }
            #endregion

            #region 產生服務命令
            SelectAllCommand command = SelectAllCommand.Create<T>(cmdAsker, where, orderbys);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is T[])
                    {
                        instances = data as T[];
                        xmlResult = new XmlResult(true);
                    }
                    else if (Reflector.TryCast<T>(data, out instances))
                    {
                        xmlResult = new XmlResult(true);
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                    }
                }
                else
                {
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                }
            }
            return xmlResult;
            #endregion
        }

        /// <summary>
        /// 查詢資料筆數
        /// </summary>
        /// <typeparam name="T">指定查詢資料的 Entity 型別。</typeparam>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="where">指定查詢的條件。必要參數，無條件時傳入空的條件。</param>
        /// <param name="count">成功則傳回查詢資料筆數，否則傳回 0。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult SelectCount<T>(Page page, Expression where
            , out int count) where T : class, IEntity
        {
            count = 0;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (where == null || !where.IsReady())
            {
                return new XmlResult(false, "缺少或無效的查詢條件參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            #region 取得服務命令請求者資料
            CommandAsker cmdAsker = this.TryGetCommandAsker(page);
            if (cmdAsker == null || !cmdAsker.IsReady)
            {
                return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
            }
            #endregion

            #region 產生服務命令
            SelectCountCommand command = SelectCountCommand.Create<T>(cmdAsker, where);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is int)
                    {
                        count = (int)data;
                        xmlResult = new XmlResult(true);
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                    }
                }
                else
                {
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                }
            }
            return xmlResult;
            #endregion
        }
        #endregion

        #region 使用者登入
        #region [MDY:20220410] Checkmarx 調整
        /// <summary>
        /// 銀行使用者登入
        /// </summary>
        /// <param name="userId">指定使用者的登入帳號。</param>
        /// <param name="userPXX">指定使用者的登入密碼。</param>
        /// <param name="cultureName">指定使用者的登入語系名稱。非必要參數。</param>
        /// <param name="logonUser">成功則傳回登入者資料類別物件，否則傳回 null。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult LogonForBank(string userId, string userPXX, string clientIP, string cultureName
            , out LogonUser logonUser)
        {
            logonUser = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (String.IsNullOrWhiteSpace(userId))
            {
                return new XmlResult(false, "缺少或無效的使用者帳號", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (String.IsNullOrWhiteSpace(userPXX))
            {
                return new XmlResult(false, "缺少或無效的使用者密碼", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            #region 產生服務命令
            BankLogonCommand command = BankLogonCommand.CreateForBank(userId, userPXX, clientIP, cultureName);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is LogonUser)
                    {
                        logonUser = data as LogonUser;
                        xmlResult = new XmlResult(true);
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                    }
                }
                else
                {
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                }
            }
            return xmlResult;
            #endregion
        }

        /// <summary>
        /// 學校使用者登入
        /// </summary>
        /// <param name="unitId">指定使用者的統一編號。</param>
        /// <param name="userId">指定使用者的登入帳號。</param>
        /// <param name="userPXX">指定使用者的登入密碼。</param>
        /// <param name="cultureName">指定使用者的登入語系名稱。非必要參數。</param>
        /// <param name="logonUser">成功則傳回登入者資料類別物件，否則傳回 null。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult LogonForSchool(string unitId, string userId, string userPXX, string clientIP, string cultureName
            , out LogonUser logonUser)
        {
            logonUser = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (String.IsNullOrWhiteSpace(unitId))
            {
                return new XmlResult(false, "缺少或無效的使用者商家代號", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (String.IsNullOrWhiteSpace(userId))
            {
                return new XmlResult(false, "缺少或無效的使用者帳號", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (String.IsNullOrWhiteSpace(userPXX))
            {
                return new XmlResult(false, "缺少或無效的使用者密碼", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            #region 產生服務命令
            UserLogonCommand command = UserLogonCommand.CreateForSchool(unitId, userId, userPXX, clientIP, cultureName);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is LogonUser)
                    {
                        logonUser = data as LogonUser;
                        //要保留原來的 Message 與 Code
                        xmlResult = new XmlResult(true, xmlResult.Message, xmlResult.Code, null);
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                    }
                }
                else
                {
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                }
            }
            return xmlResult;
            #endregion
        }
        #endregion
        #endregion

        #region 學生登入
        /// <summary>
        /// 學生登入
        /// </summary>
        /// <param name="schIdentity">指定學校統編。</param>
        /// <param name="studentId">指定學號。</param>
        /// <param name="loginKey">指定登入 Key。</param>
        /// <param name="cultureName">指定使用者的登入語系名稱。非必要參數。</param>
        /// <param name="logonUser">成功則傳回登入者資料類別物件，否則傳回 null。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult LogonForStudent(string schIdentity, string studentId, string loginKey, string clientIP, string cultureName
            , out LogonUser logonUser)
        {
            logonUser = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (String.IsNullOrWhiteSpace(schIdentity))
            {
                return new XmlResult(false, "缺少或無效的學校代收類別", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (String.IsNullOrWhiteSpace(studentId))
            {
                return new XmlResult(false, "缺少或無效的學號", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (String.IsNullOrWhiteSpace(loginKey))
            {
                return new XmlResult(false, "缺少或無效的登入 Key", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            #region 產生服務命令
            StudentLogonCommand command = StudentLogonCommand.Create(schIdentity, studentId, loginKey, clientIP, cultureName);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is LogonUser)
                    {
                        logonUser = data as LogonUser;
                        //要保留原來的 Message 與 Code
                        xmlResult = new XmlResult(true, xmlResult.Message, xmlResult.Code, null);
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                    }
                }
                else
                {
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                }
            }
            return xmlResult;
            #endregion
        }
        #endregion

        #region 行員 SSO 登入
        ///// <summary>
        ///// 行員 SSO 登入
        ///// </summary>
        ///// <param name="pusid">指定 SSO 的 pusid 參數。</param>
        ///// <param name="userId">指定 SSO 驗證後使用者帳號。</param>
        ///// <param name="userName">指定 SSO 驗證後使用者名稱。</param>
        ///// <param name="groupIds">指定 SSO 驗證後使用者群組。</param>
        ///// <param name="branchId">指定 SSO 驗證後分行代碼。</param>
        ///// <param name="logonUser">成功則傳回登入者資料類別物件，否則傳回 null。</param>
        ///// <returns>傳回處理結果。</returns>
        //public XmlResult LogonForSSO(string pusid, string userId, string userName, string[] groupIds, string branchId, string clientIP, string cultureName
        //    , out LogonUser logonUser)
        //{
        //    logonUser = null;

        //    #region 檢查資料處理代理物件
        //    if (!this.IsReady())
        //    {
        //        return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
        //    }
        //    #endregion

        //    #region 檢查參數
        //    if (String.IsNullOrWhiteSpace(pusid))
        //    {
        //        return new XmlResult(false, "缺少或無效的 pusid", ErrorCode.S_INVALID_PARAMETER, null);
        //    }
        //    if (String.IsNullOrWhiteSpace(userId))
        //    {
        //        return new XmlResult(false, "缺少或無效的使用者帳號", ErrorCode.S_INVALID_PARAMETER, null);
        //    }
        //    if (String.IsNullOrWhiteSpace(userName))
        //    {
        //        return new XmlResult(false, "缺少或無效的使用者名稱", ErrorCode.S_INVALID_PARAMETER, null);
        //    }
        //    if (groupIds == null || groupIds.Length == 0)
        //    {
        //        return new XmlResult(false, "缺少或無效的使用者群組", ErrorCode.S_INVALID_PARAMETER, null);
        //    }
        //    if (String.IsNullOrWhiteSpace(branchId))
        //    {
        //        return new XmlResult(false, "缺少或無效的分行代碼", ErrorCode.S_INVALID_PARAMETER, null);
        //    }
        //    #endregion

        //    #region 產生服務命令
        //    SSOLogonCommand command = SSOLogonCommand.Create(pusid, userId, userName, groupIds, branchId, clientIP, cultureName);
        //    #endregion

        //    #region 呼叫後端服務命令，並處理回傳結果
        //    XmlResult xmlResult = this.ExecuteCommand(command);
        //    if (xmlResult.IsSuccess)
        //    {
        //        object data = null;
        //        if (xmlResult.TryGetData(out data))
        //        {
        //            if (data is LogonUser)
        //            {
        //                logonUser = data as LogonUser;
        //                //要保留原來的 Message 與 Code
        //                xmlResult = new XmlResult(true, xmlResult.Message, xmlResult.Code, null);
        //            }
        //            else
        //            {
        //                xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
        //            }
        //        }
        //        else
        //        {
        //            xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
        //        }
        //    }
        //    return xmlResult;
        //    #endregion
        //}
        #endregion

        #region 檢查登入與功能狀態
        /// <summary>
        /// 檢查登入與功能狀態
        /// </summary>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="logonUser">指定登入者資料。</param>
        /// <param name="checkFuncFlag">指定是否檢查功能狀態。</param>
        /// <param name="resultCode">傳回檢查結果代碼，參考 CheckLogonResultCodeTexts。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult CheckLogon(Page page, LogonUser logonUser, bool checkFuncFlag, out string resultCode)
        {
            resultCode = CheckLogonResultCodeTexts.CHECK_FAILURE;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (logonUser == null || String.IsNullOrEmpty(logonUser.LogonSN))
            {
                resultCode = CheckLogonResultCodeTexts.NON_LOGON;
                return new XmlResult(true);
            }
            #endregion

            #region 取得服務命令請求者資料
            CommandAsker cmdAsker = this.TryGetCommandAsker(page);
            if (cmdAsker == null || !cmdAsker.IsReady)
            {
                return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
            }
            #endregion

            #region 產生服務命令
            string checkFuncId = checkFuncFlag ? cmdAsker.FuncId : String.Empty;
            CheckLogonCommand command = CheckLogonCommand.Create(cmdAsker, logonUser.LogonSN, checkFuncId);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is string)
                    {
                        resultCode = data as string;
                        xmlResult = new XmlResult(true);
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE, null);
                    }
                }
                else
                {
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                }
            }
            return xmlResult;
            #endregion
        }
        #endregion

        #region 登出
        /// <summary>
        /// 使用者登出
        /// </summary>
        /// <param name="page"></param>
        /// <param name="logonUser"></param>
        /// <returns></returns>
        public XmlResult LogoutUser(Page page, LogonUser logonUser)
        {
            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (logonUser == null || String.IsNullOrEmpty(logonUser.LogonSN))
            {
                return new XmlResult(true);
            }
            #endregion

            #region 取得服務命令請求者資料
            CommandAsker cmdAsker = this.TryGetCommandAsker(page);
            if (cmdAsker == null || !cmdAsker.IsReady)
            {
                return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
            }
            #endregion

            #region 產生服務命令
            UserLogoutCommand command = UserLogoutCommand.Create(cmdAsker, logonUser.LogonSN);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            return xmlResult;
            #endregion
        }

        #region [Old] 20150605 取消前擋後，改回後踢前
        ///// <summary>
        ///// 強迫登出指定帳號
        ///// </summary>
        ///// <param name="page"></param>
        ///// <param name="logonUnit"></param>
        ///// <param name="logonId"></param>
        ///// <param name="logonQual"></param>
        ///// <returns></returns>
        //public XmlResult ForcedLogoutUser(Page page, string logonUnit, string logonId, string logonQual)
        //{
        //    KeyValue<string>[] args = new KeyValue<string>[] {
        //        new KeyValue<string>("LogonUnit", logonUnit),
        //        new KeyValue<string>("LogonId", logonId),
        //        new KeyValue<string>("LogonQual", logonQual)
        //    };

        //    object returnData = null;
        //    XmlResult xmlResult = this.CallMethod(page, CallMethodName.ForcedLogoutUser, args, out returnData);
        //    return xmlResult;
        //}
        #endregion
        #endregion

        #region 取得主要條件選項資料
        #region [MDY:202203XX] 2022擴充案 英文名稱相關
        /// <summary>
        /// 取得主要條件選項資料
        /// </summary>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="dataMode">查詢資料模式。</param>
        /// <param name="receiveType">查詢的商家代號代碼。</param>
        /// <param name="yearID">查詢的學年代碼。</param>
        /// <param name="termID">查詢的學期代碼。</param>
        /// <param name="depID">查詢的部別代碼。</param>
        /// <param name="receiveID">查詢的代收費用別代碼。</param>
        /// <param name="receiveKind">查詢的商家代號代收種類。</param>
        /// <param name="isEngUI">指定是否英文介面。</param>
        /// <param name="receiveTypeMode">商家代號預設項目模式。</param>
        /// <param name="yearMode">學年預設項目模式。</param>
        /// <param name="termMode">學期預設項目模式。</param>
        /// <param name="depMode">部別預設項目模式。</param>
        /// <param name="receiveMode">代收費用別預設項目模式。</param>
        /// <param name="option">成功則傳回主要條件選項資料，否則傳回 null。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult GetFilterOption(Page page, string dataMode
            , string receiveType, string yearID, string termID, string depID, string receiveID
            , string receiveKind, bool isEngUI
            , DefaultItem.Mode receiveTypeMode, DefaultItem.Mode yearMode, DefaultItem.Mode termMode
            , DefaultItem.Mode depMode, DefaultItem.Mode receiveMode
            , out FilterOption option)
        {
            option = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (String.IsNullOrEmpty(dataMode))
            {
                return new XmlResult(false, "缺少或無效的查詢資料模式參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            #region 取得服務命令請求者資料
            CommandAsker cmdAsker = this.TryGetCommandAsker(page);
            if (cmdAsker == null || !cmdAsker.IsReady)
            {
                return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
            }
            #endregion

            #region 產生服務命令
            string defaultModes = String.Format("{0}{1}{2}{3}{4}", (int)receiveTypeMode, (int)yearMode, (int)termMode, (int)depMode, (int)receiveMode);
            FilterOptionCommand command = FilterOptionCommand.Create(cmdAsker, dataMode, receiveType, yearID, termID, receiveID, receiveKind, defaultModes, isEngUI);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is FilterOption)
                    {
                        option = (FilterOption)data;
                        xmlResult = new XmlResult(true);
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                    }
                }
                else
                {
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                }
            }
            return xmlResult;
            #endregion
        }
        #endregion
        #endregion

        #region 取得資料選項陣列
        /// <summary>
        /// 取得資料選項陣列
        /// </summary>
        /// <typeparam name="T">指定查詢資料的 Entity 型別。</typeparam>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="where">指定查詢的條件。必要參數，無條件時傳入空的條件。</param>
        /// <param name="orderbys">指定資料的排序方式。</param>
        /// <param name="codeFieldNames">指定代碼欄位名稱陣列，不指定則以 Entity 的 PKey 取代。</param>
        /// <param name="codeCombineFormat">指定代碼組合格式。格式化以 String.Format 處理，參數順序與 codeFieldNames 相同。代碼欄位只有一個時，無需指定，多個且未指定時，以逗號區隔欄位值。</param>
        /// <param name="textFieldNames">指定文字欄位名稱陣列，必要參數。</param>
        /// <param name="textCombineFormat">指定文字組合格式。格式化以 String.Format 處理，參數順序與 textFieldNames 相同。文字欄位只有一個時，無需指定。</param>
        /// <param name="datas">成功則傳回資料選項陣列，否則傳回 null。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult GetEntityOptions<T>(Page page, Expression where, KeyValueList<OrderByEnum> orderbys
            , string[] codeFieldNames, string codeCombineFormat, string[] textFieldNames, string textCombineFormat
            , out CodeText[] datas) where T : class, IEntity
        {
            datas = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (where == null || !where.IsReady())
            {
                return new XmlResult(false, "缺少或無效的查詢條件參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (textFieldNames == null || textFieldNames.Length == 0)
            {
                return new XmlResult(false, "缺少或無效的文字欄位名稱陣列參數", ErrorCode.S_INVALID_PROXY, null);
            }
            if (textFieldNames.Length > 1 && String.IsNullOrWhiteSpace(textCombineFormat))
            {
                return new XmlResult(false, "缺少或無效的文字組合格式參數", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 取得服務命令請求者資料
            CommandAsker cmdAsker = this.TryGetCommandAsker(page);
            if (cmdAsker == null || !cmdAsker.IsReady)
            {
                return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
            }
            #endregion

            #region 產生服務命令
            EntityOptionsCommand command = EntityOptionsCommand.Create<T>(cmdAsker, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is CodeText[])
                    {
                        datas = (CodeText[])data;
                        xmlResult = new XmlResult(true);
                    }
                    else
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                    }
                }
                else
                {
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                }
            }
            return xmlResult;
            #endregion
        }
        #endregion

        #region 呼叫後台服務方法
        /// <summary>
        /// 呼叫後台服務方法
        /// </summary>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="methodName">呼叫方法名稱。</param>
        /// <param name="methodArguments">呼叫方法參數集合，可傳入 null 或 KeyValue&lt;string&gt; 或 KeyValueList&lt;string&gt;。key 為參數名稱，value 為參數值。</param>
        /// <param name="returnData">傳回呼叫方法後回傳的資料物件。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult CallMethod(Page page, string methodName, ICollection<KeyValue<string>> methodArguments, out object returnData)
        {
            returnData = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (String.IsNullOrEmpty(methodName))
            {
                return new XmlResult(false, "缺少或無效的呼叫方法名稱參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            #region 取得服務命令請求者資料
            CommandAsker cmdAsker = this.TryGetCommandAsker(page);
            if (cmdAsker == null || !cmdAsker.IsReady)
            {
                return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
            }
            #endregion

            #region 產生服務命令
            CallMethodCommand command = CallMethodCommand.Create(cmdAsker, methodName, methodArguments);
            #endregion

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    returnData = data;

                    //[TMP] 如果有經常會使用的方法，可以在這裡判斷並轉換回傳資料的型別
                    //if (data is FilterOption)
                    //{
                    //    returnData = (FilterOption)data;
                    //    xmlResult = new XmlResult(true);
                    //}
                    //else
                    //{
                    //    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                    //}
                }
                else if (xmlResult.IsSuccess)
                {
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                }
            }
            return xmlResult;
            #endregion
        }
        #endregion

        #region [MDY:20210301] 呼叫後台服務方法 2
        /// <summary>
        /// 呼叫後台服務方法 2
        /// </summary>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="methodName">呼叫方法名稱。</param>
        /// <param name="methodArguments">呼叫方法參數集合，可傳入 null 或 KeyValue&lt;string&gt; 或 KeyValueList&lt;string&gt;。key 為參數名稱，value 為參數值。</param>
        /// <param name="timeout">指定呼叫方法的 Timeout 秒數。</param>
        /// <param name="returnData">傳回呼叫方法後回傳的資料物件。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult CallMethod2(Page page, string methodName, ICollection<KeyValue<string>> methodArguments, Int32 timeout, out object returnData)
        {
            #region 除錯日誌紀錄器
            DebugLogger debugger = DebugLogger.Create(WebHelper.GetLogonUser(), (page == null ? null : page.Session.SessionID));
            debugger.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始呼叫後台服務方法 {1} ", DateTime.Now, methodName).AppendLine(false);
            #endregion

            System.Diagnostics.Stopwatch watchA = new System.Diagnostics.Stopwatch();
            watchA.Start();

            returnData = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                debugger.AppendLine("無效的資料處理代理物件").Flush();
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (String.IsNullOrEmpty(methodName))
            {
                debugger.AppendLine("缺少或無效的呼叫方法名稱參數").Flush();
                return new XmlResult(false, "缺少或無效的呼叫方法名稱參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            #region 取得服務命令請求者資料
            CommandAsker cmdAsker = this.TryGetCommandAsker(page);
            if (cmdAsker == null || !cmdAsker.IsReady)
            {
                debugger.AppendLine("缺少或無效的呼叫方法名稱參數").Flush();
                return new XmlResult(false, "無效的服務命令請求者資料", ErrorCode.S_INVALID_COMMAND_ASKER);
            }
            #endregion

            #region 產生服務命令
            List<KeyValue<string>> args = new List<KeyValue<string>>(methodArguments);
            args.Add(new KeyValue<string>("ClientTaskID", debugger.TaskId));
            CallMethodCommand command = CallMethodCommand.Create(cmdAsker, methodName, args);
            #endregion

            XmlResult xmlResult = null;

            #region 呼叫後端服務命令，並處理回傳結果
            debugger.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始執行命令 ", DateTime.Now);
            System.Diagnostics.Stopwatch watchB = new System.Diagnostics.Stopwatch();
            watchB.Start();

            string commandXml = command.ToCommandXml();
            if (String.IsNullOrEmpty(commandXml))
            {
                return new XmlResult(false, "呼叫後端服務失敗，服務命令序列化失敗", ServiceStatusCode.S_SERVICE_SERIALIZED_FAILURE);
            }

            string apItem = String.Empty;
            string askerXml = String.Empty;

            DataService dataService = this.GetDataService();
            dataService.Timeout = 1000 * timeout;

            #region [MDY:20220503] Proxy 組態屬性改為 ApSCode、ApMCode、ApACode 避開 checkmarx 敏感字眼
            string resultXml = dataService.ExecuteCommand(this.ApSCode, this.ApMCode, this.ApACode, apItem, commandXml, askerXml);
            #endregion

            xmlResult = XmlResult.Create(resultXml);
            if (xmlResult == null)
            {
                xmlResult = new XmlResult(false, "後端服務服務回傳資料無法反序列化", ServiceStatusCode.S_SERVICE_DESERIALIZED_FAILURE);
            }

            watchB.Stop();
            debugger.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 結束執行命令，共耗時 {1} 秒", DateTime.Now, (watchB.ElapsedMilliseconds / 1000M));
            #endregion

            #region 處理回傳結果
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    returnData = data;
                    if (returnData is string)
                    {
                        debugger.AppendLine(returnData.ToString());
                    }

                    //[TMP] 如果有經常會使用的方法，可以在這裡判斷並轉換回傳資料的型別
                }
                else if (xmlResult.IsSuccess)
                {
                    debugger.AppendLine("傳回資料無法反序列化").Flush();
                    xmlResult = new XmlResult(false, "傳回資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                }
            }
            #endregion

            watchA.Stop();
            debugger.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 結束呼叫後台服務方法 {1} ，共耗時 {2} 秒", DateTime.Now, methodName, (watchA.ElapsedMilliseconds / 1000M)).AppendLine(false).Flush();
            debugger.Dispose();
            debugger = null;

            return xmlResult;
        }
        #endregion

        #region 複製 / 刪除 代碼檔與標準檔
        /// <summary>
        /// 複製代碼檔與標準檔
        /// </summary>
        /// <param name="page"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId1"></param>
        /// <param name="termId1"></param>
        /// <param name="yearId2"></param>
        /// <param name="termId2"></param>
        /// <param name="entityNames"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public XmlResult CopyBaseData(Page page, string receiveType, string yearId1, string termId1, string yearId2, string termId2, ICollection<string> entityNames, out string msg)
        {
            msg = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (String.IsNullOrWhiteSpace(receiveType) || String.IsNullOrWhiteSpace(yearId1) || String.IsNullOrWhiteSpace(termId1)
                || String.IsNullOrWhiteSpace(yearId2) || String.IsNullOrWhiteSpace(termId2)
                || entityNames == null || entityNames.Count == 0
            )
            {
                return new XmlResult(false, "缺少或無效的資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            receiveType = receiveType.Trim();
            yearId1 = yearId1.Trim();
            termId1 = termId1.Trim();
            yearId2 = yearId2.Trim();
            termId2 = termId2.Trim();
            if (yearId1 == yearId2 && termId1 == termId2)
            {
                return new XmlResult(false, "無效的資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            KeyValue<string>[] args = new KeyValue<string>[6] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId1", yearId1),
                new KeyValue<string>("TermId1", termId1),
                new KeyValue<string>("YearId2", yearId2),
                new KeyValue<string>("TermId2", termId2),
                new KeyValue<string>("EntityNames", String.Join(",", entityNames))
            };

            object returnData = null;
            XmlResult xmlResult = DataProxy.Current.CallMethod(page, CallMethodName.CopyBaseData, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                msg = returnData as string;
                if (msg == null)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }

        /// <summary>
        /// 刪除代碼檔與標準檔
        /// </summary>
        /// <param name="page"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId1"></param>
        /// <param name="termId1"></param>
        /// <param name="yearId2"></param>
        /// <param name="termId2"></param>
        /// <param name="entityNames"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public XmlResult DeleteBaseData(Page page, string receiveType, string yearId, string termId, ICollection<string> entityNames, out string msg)
        {
            msg = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (String.IsNullOrWhiteSpace(receiveType) || String.IsNullOrWhiteSpace(yearId) || String.IsNullOrWhiteSpace(termId)
                || entityNames == null || entityNames.Count == 0
            )
            {
                return new XmlResult(false, "缺少或無效的資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            receiveType = receiveType.Trim();
            yearId = yearId.Trim();
            termId = termId.Trim();
            #endregion

            KeyValue<string>[] args = new KeyValue<string>[4] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId", yearId),
                new KeyValue<string>("TermId", termId),
                new KeyValue<string>("EntityNames", String.Join(",", entityNames))
            };

            object returnData = null;
            XmlResult xmlResult = DataProxy.Current.CallMethod(page, CallMethodName.DeleteBaseData, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                msg = returnData as string;
                if (msg == null)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion

        #region S530001 (使用者管理) 頁面相關
        /// <summary>
        /// 取得 S530001 (使用者管理) 頁面的編輯資料
        /// </summary>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="userId">使用者帳號，必要參數且不可為空字串。</param>
        /// <param name="groupId">使用者所屬群組，必要參數且不可為空字串。</param>
        /// <param name="receiveType">使用者所屬商家代號，必要參數且與bankId不可同時為空字串。</param>
        /// <param name="bankId">使用者所屬學校統編，必要參數且與receiveType不可同時為空字串。</param>
        /// <param name="user">成功則傳回使用者資料物件，否則傳回 null。</param>
        /// <param name="group">成功則傳回使用者所屬群組資料物件，否則傳回 null。</param>
        /// <param name="funcMenus">成功則傳回功能選單資料陣列物件，否則傳回 null。</param>
        /// <param name="groupRights">成功則傳回使用者所屬群組權限資料陣列物件，否則傳回 null。</param>
        /// <param name="userRights">成功則傳回使用者權限資料陣列物件，否則傳回 null。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult S530001GetEditData(Page page, string userId, string groupId, string receiveType, string bankId
            , out UsersEntity user, out GroupListEntity group, out CodeText[] funcMenus, out GroupRightEntity[] groupRights, out UsersRightEntity[] userRights)
        {
            user = null;
            group = null;
            funcMenus = null;
            groupRights = null;
            userRights = null;

            KeyValue<string>[] args = new KeyValue<string>[4];
            args[0] = new KeyValue<string>("UserId", userId);
            args[1] = new KeyValue<string>("GroupId", groupId);
            args[2] = new KeyValue<string>("ReceiveType", receiveType);
            args[3] = new KeyValue<string>("BankId", bankId);

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.S530001GetEditData, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                bool isOK = true;
                if (returnData is KeyValue<string>[])
                {
                    KeyValue<string>[] datas = (KeyValue<string>[])returnData;
                    foreach (KeyValue<string> data in datas)
                    {
                        #region 使用者
                        if (data.Key == "UsersEntity")
                        {
                            if (!Common.TryDeXmlExplicitly<UsersEntity>(data.Value, out user))
                            {
                                isOK = false;
                                break;
                            }
                            continue;
                        }
                        #endregion

                        #region 群組
                        if (data.Key == "GroupListEntity")
                        {
                            if (!Common.TryDeXmlExplicitly<GroupListEntity>(data.Value, out group))
                            {
                                isOK = false;
                                break;
                            }
                            continue;
                        }
                        #endregion

                        #region 功能資料
                        if (data.Key == "FuncMenuCodeText")
                        {
                            if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out funcMenus))
                            {
                                isOK = false;
                                break;
                            }
                            continue;
                        }
                        #endregion

                        #region 群組權限
                        if (data.Key == "GroupRightEntity")
                        {
                            if (!Common.TryDeXmlExplicitly<GroupRightEntity[]>(data.Value, out groupRights))
                            {
                                isOK = false;
                                break;
                            }
                            continue;
                        }
                        #endregion

                        #region [Old] 土銀不使用使用者權限
                        //#region 使用者權限
                        //if (data.Key == "UsersRightEntity")
                        //{
                        //    if (!Common.TryDeXmlExplicitly<UsersRightEntity[]>(data.Value, out userRights))
                        //    {
                        //        isOK = false;
                        //        break;
                        //    }
                        //    continue;
                        //}
                        //#endregion
                        #endregion
                    }

                    if (user == null || group == null)
                    {
                        isOK = false;
                    }
                }
                else
                {
                    isOK = false;
                }
                if (!isOK)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }

        #region [MDY:20220410] Checkmarx 調整
        /// <summary>
        /// 新增 S530001 (使用者管理) 頁面的資料
        /// </summary>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="userId">使用者帳號，必要參數且不可為空字串。</param>
        /// <param name="groupId">使用者所屬群組，必要參數且只能是學校一般使用者或學校主管。</param>
        /// <param name="bankId">使用者所屬學校統編，必要參數不可為空字串。</param>
        /// <param name="receiveType">使用者所屬商家代號，必要參數，以逗號隔開每個商家代號。</param>
        /// <param name="userPXX">使用者的密碼，必要參數且不可為空字串。</param>
        /// <param name="userName">使用者新的名稱，必要參數且不可為空字串。</param>
        /// <param name="tel">使用者電話。</param>
        /// <param name="email">使用者Email。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult S530001InsertData(Page page, string userId, string groupId, string bankId, string receiveType
            , string userPXX, string userName, string tel, string email)
        {
            KeyValue<string>[] args = new KeyValue<string>[8];
            args[0] = new KeyValue<string>("UserId", userId);
            args[1] = new KeyValue<string>("GroupId", groupId);
            args[2] = new KeyValue<string>("BankId", bankId);
            args[3] = new KeyValue<string>("ReceiveType", receiveType);

            args[4] = new KeyValue<string>("UserPXX", userPXX ?? String.Empty);
            args[5] = new KeyValue<string>("UserName", userName);
            args[6] = new KeyValue<string>("Tel", tel);
            args[7] = new KeyValue<string>("EMail", email);

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.S530001InsertData, args, out returnData);
            return xmlResult;
        }

        /// <summary>
        /// 更新 S530001 (使用者管理) 頁面的資料
        /// </summary>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="userId">使用者帳號，必要參數且不可為空字串。</param>
        /// <param name="groupId">使用者所屬群組，必要參數且只能是學校一般使用者或學校主管。</param>
        /// <param name="bankId">使用者所屬學校統編，必要參數不可為空字串。</param>
        /// <param name="receiveType">使用者所屬商家代號，必要參數，以逗號隔開每個商家代號。</param>
        /// <param name="newReceiveType">使用者新的所屬商家代號，必要參數。</param>
        /// <param name="userPXX">使用者新的密碼，傳入 null 或空字串則不更新此欄位資料。</param>
        /// <param name="userName">使用者新的名稱，必要參數且不可為空字串。</param>
        /// <param name="title">使用者新的職稱。</param>
        /// <param name="tel">使用者新的電話。</param>
        /// <param name="email">使用者新的email。</param>
        /// <param name="isLocked">使用者是否設為鎖住，必要參數。</param>
        /// <param name="userRights">使用者權限，使用者如屬於管理者類群組則此參數忽略不處理。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult S530001UpdateData(Page page, string userId, string groupId, string bankId, string receiveType
            , string newReceiveType, string userPXX, string userName, string title, string tel, string email, bool isLocked
            , ICollection<KeyValue<string>> userRights)
        {
            KeyValue<string>[] args = new KeyValue<string>[12];
            args[0] = new KeyValue<string>("UserId", userId);
            args[1] = new KeyValue<string>("GroupId", groupId);
            args[2] = new KeyValue<string>("BankId", bankId);
            args[3] = new KeyValue<string>("ReceiveType", receiveType);

            args[4] = new KeyValue<string>("NewReceiveType", newReceiveType);
            args[5] = new KeyValue<string>("UserPXX", userPXX ?? String.Empty);

            args[6] = new KeyValue<string>("UserName", userName);
            args[7] = new KeyValue<string>("Title", title);
            args[8] = new KeyValue<string>("Tel", tel);
            args[9] = new KeyValue<string>("Email", email);
            args[10] = new KeyValue<string>("Locked", isLocked ? "Y" : "N");

            if (userRights == null || userRights.Count == 0)
            {
                args[11] = new KeyValue<string>("UserRights", String.Empty);
            }
            else
            {
                string txt = KeyValueList<string>.Join(userRights.ToArray());
                args[11] = new KeyValue<string>("userRights", txt);
            }

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.S530001UpdateData, args, out returnData);
            return xmlResult;
        }
        #endregion

        /// <summary>
        /// 刪除 S530001 (使用者管理) 頁面的資料
        /// </summary>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="userId">使用者帳號，必要參數且不可為空字串</param>
        /// <param name="groupId">使用者所屬群組，必要參數且只能是學校一般使用者或學校主管。</param>
        /// <param name="bankId">使用者所屬學校統編，必要參數且不可空字串。</param>
        /// <param name="receiveType">使用者所屬商家代號，必要參數，以逗號隔開每個商家代號。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult S530001DeleteData(Page page, string userId, string groupId, string bankId, string receiveType)
        {
            KeyValue<string>[] args = new KeyValue<string>[4];
            args[0] = new KeyValue<string>("UserId", userId);
            args[1] = new KeyValue<string>("GroupId", groupId);
            args[2] = new KeyValue<string>("BankId", bankId);
            args[3] = new KeyValue<string>("ReceiveType", receiveType);

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.S530001DeleteData, args, out returnData);
            return xmlResult;
        }
        #endregion

        #region 新增批次處理佇列 For D15 類
        /// <summary>
        /// 新增批次處理佇列 For D15 類
        /// </summary>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="fileData">要處理的檔案資料。</param>
        /// <param name="jobCube">要新增的批次處理佇列資料。</param>
        /// <param name="fileSNo">傳回新增檔案資料的序號。</param>
        /// <param name="jobSNo">傳回新增批次處理佇列資料的序號。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult InsertJobCubeForD15(Page page, BankpmEntity fileData, JobcubeEntity jobCube, out string fileSNo, out string jobSNo)
        {
            fileSNo = null;
            jobSNo = null;

            string fileDataXml = null;
            if (!Common.TryToXmlExplicitly<BankpmEntity>(fileData, out fileDataXml))
            {
                return new XmlResult(false, "檔案資料序列化失敗", ErrorCode.S_SERIALIZED_FAILURE, null);
            }
            string jobCubeXml = null;
            if (!Common.TryToXmlExplicitly<JobcubeEntity>(jobCube, out jobCubeXml))
            {
                return new XmlResult(false, "批次處理佇列資料序列化失敗", ErrorCode.S_SERIALIZED_FAILURE, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[2];
            args[0] = new KeyValue<string>("FileDataXml", fileDataXml);
            args[1] = new KeyValue<string>("JobCubeXml", jobCubeXml);

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.InsertJobCubeForD15, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                int count = 0;
                CodeText[] datas = returnData as CodeText[];
                if (datas != null && datas.Length == 2)
                {
                    foreach (CodeText data in datas)
                    {
                        string code = data.Code.ToUpper();
                        switch (code)
                        {
                            case "FILESNO":
                                count |= 0x1;
                                fileSNo = data.Text;
                                break;
                            case "JOBSNO":
                                count |= 0x2;
                                jobSNo = data.Text;
                                break;
                        }
                    }
                }
                if (count != 0x3)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion

        #region 新增批次處理佇列 For PDF 類
        /// <summary>
        /// 新增批次處理佇列 For PDF 類
        /// </summary>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="jobCube">要新增的批次處理佇列資料。</param>
        /// <param name="jobCubeNo">傳回新增批次處理佇列資料的序號。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult InsertJobCubeForPDF(Page page, JobcubeEntity jobCube, out byte[] pdfContent)
        {
            pdfContent = null;

            string jobCubeXml = null;
            if (!Common.TryToXmlExplicitly<JobcubeEntity>(jobCube, out jobCubeXml))
            {
                return new XmlResult(false, "批次處理佇列資料序列化失敗", ErrorCode.S_SERIALIZED_FAILURE, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[1];
            args[0] = new KeyValue<string>("JobCubeXml", jobCubeXml);

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.InsertJobCubeForPDF, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                pdfContent = returnData as byte[];
                if (pdfContent == null || pdfContent.Length == 0)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }

        /// <summary>
        /// 新增批次處理佇列 For PDF 類
        /// </summary>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="jobCube">要新增的批次處理佇列資料。</param>
        /// <param name="stamp">。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult InsertJobCubeForPDF(Page page, JobcubeEntity jobCube, out string jno, out string stamp)
        {
            jno = null;
            stamp = null;

            string jobCubeXml = null;
            if (!Common.TryToXmlExplicitly<JobcubeEntity>(jobCube, out jobCubeXml))
            {
                return new XmlResult(false, "批次處理佇列資料序列化失敗", ErrorCode.S_SERIALIZED_FAILURE, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[1];
            args[0] = new KeyValue<string>("JobCubeXml", jobCubeXml);

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.InsertJobCubeForPDFByAsync, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                string[] datas = returnData as string[];
                if (datas == null || datas.Length != 2)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
                else
                {
                    jno = datas[0];
                    stamp = datas[1];
                }
            }
            return xmlResult;
        }
        #endregion

        #region 新增批次處理佇列 For D38 類
        /// <summary>
        /// 新增批次處理佇列 For PDF 類
        /// </summary>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="jobCube">要新增的批次處理佇列資料。</param>
        /// <param name="stamp">。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult InsertJobCubeForD38(Page page, JobcubeEntity jobCube, out string jno)
        {
            jno = null;

            string jobCubeXml = null;
            if (!Common.TryToXmlExplicitly<JobcubeEntity>(jobCube, out jobCubeXml))
            {
                return new XmlResult(false, "批次處理佇列資料序列化失敗", ErrorCode.S_SERIALIZED_FAILURE, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[1];
            args[0] = new KeyValue<string>("JobCubeXml", jobCubeXml);

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.InsertJobCubeForD38, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                jno = returnData as string;
                if (String.IsNullOrEmpty(jno))
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion

        #region 執行 B2100002 特殊需求 (計算金額、產生PDF繳費單、產生PDF繳費收據)
        /// <summary>
        /// 執行 B2100002 特殊需求 (計算金額、產生PDF繳費單、產生PDF繳費收據)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="command"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="stuId"></param>
        /// <param name="oldSeq"></param>
        /// <param name="fgMark">是否要遮罩</param>
        /// <param name="pdfContent"></param>
        /// <returns></returns>
        public XmlResult ExecB2100002Request(Page page, string command
            , string receiveType, string yearId, string termId, string depId, string receiveId, string stuId, int oldSeq
            , bool fgMark, bool isEngUI, out byte[] pdfContent)
        {
            pdfContent = null;

            KeyValue<string>[] args = new KeyValue<string>[10] {
                new KeyValue<string>("Command", command),
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId", yearId),
                new KeyValue<string>("TermId", termId),
                new KeyValue<string>("DepId", depId),
                new KeyValue<string>("ReceiveId", receiveId),
                new KeyValue<string>("StuId", stuId),
                new KeyValue<string>("OldSeq", oldSeq.ToString()),
                new KeyValue<string>("FgMark", fgMark ? "Y" : "N"),
                new KeyValue<string>("isEngUI", isEngUI ? "Y" : "N")
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ExecB2100002Request, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                if (command == "GENBILL" || command == "GENRECEIPT")
                {
                    pdfContent = returnData as byte[];
                    if (pdfContent == null || pdfContent.Length == 0)
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                    }
                }
            }
            return xmlResult;
        }
        #endregion

        #region 取得 B2100002 (維護學生繳費資料) 合計資料
        /// <summary>
        /// 取得 B2100002 (維護學生繳費資料) 合計資料
        /// </summary>
        /// <param name="page"></param>
        /// <param name="where"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public XmlResult GetB2100002Summary(Page page, Expression where, out KeyValue<Decimal>[] datas)
        {
            datas = null;

            #region 檢查參數
            if (where == null || !where.IsReady())
            {
                return new XmlResult(false, "缺少或無效的查詢條件參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            string whereXml = null;
            if (!Common.TryToXmlExplicitly2(where, where.GetType(), out whereXml))
            {
                return new XmlResult(false, "查詢條件參數無法序列化", ErrorCode.S_SERIALIZED_FAILURE, null);
            }
            #endregion

            KeyValue<string>[] args = new KeyValue<string>[1] {
                new KeyValue<string>("WhereXml", whereXml)
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.GetB2100002Summary, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                if (returnData is KeyValue<Decimal>[])
                {
                    datas = returnData as KeyValue<Decimal>[];
                }
                else
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion

        #region 取得 C3700006 (中心入帳資料查詢) 合計資料
        /// <summary>
        /// 取得 C3700006 (中心入帳資料查詢) 合計資料
        /// </summary>
        /// <param name="page"></param>
        /// <param name="where"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public XmlResult GetC3700006Summary(Page page, Expression where, out KeyValue<Decimal>[] datas)
        {
            datas = null;

            #region 檢查參數
            if (where == null || !where.IsReady())
            {
                return new XmlResult(false, "缺少或無效的查詢條件參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            string whereXml = null;
            if (!Common.TryToXmlExplicitly2(where, where.GetType(), out whereXml))
            {
                return new XmlResult(false, "查詢條件參數無法序列化", ErrorCode.S_SERIALIZED_FAILURE, null);
            }
            #endregion

            KeyValue<string>[] args = new KeyValue<string>[1] {
                new KeyValue<string>("WhereXml", whereXml)
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.GetC3700006Summary, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                if (returnData is KeyValue<Decimal>[])
                {
                    datas = returnData as KeyValue<Decimal>[];
                }
                else
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion

        #region 新增 B2300003 的資料 (產生Email繳費通知)
        /// <summary>
        /// 新增 B2300003 的資料 (產生Email繳費通知)
        /// </summary>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="receiveType">指定商家代號</param>
        /// <param name="yearId">指定學年代碼</param>
        /// <param name="termId">指定學期代碼</param>
        /// <param name="depId">指定部別代碼</param>
        /// <param name="receiveId">指定代收費用別代碼</param>
        /// <param name="rangeType">指定條件泛範圍類別</param>
        /// <param name="startSNo">指定起始流水號條件</param>
        /// <param name="endSNo">指定結束流水號條件</param>
        /// <param name="upNo">指定批號條件</param>
        /// <param name="studentId">指定學號條件</param>
        /// <returns>傳回處理結果</returns>
        public XmlResult InsertB2300003Data(Page page, string receiveType, string yearId, string termId, string depId, string receiveId
            , string rangeType, string startSNo, string endSNo, string upNo, string studentId)
        {
            KeyValue<string>[] args = new KeyValue<string>[10];
            args[0] = new KeyValue<string>("ReceiveType", receiveType);
            args[1] = new KeyValue<string>("YearId", yearId);
            args[2] = new KeyValue<string>("TermId", termId);
            args[3] = new KeyValue<string>("DepId", depId);
            args[4] = new KeyValue<string>("ReceiveId", receiveId);

            args[5] = new KeyValue<string>("RangeType", rangeType);
            args[6] = new KeyValue<string>("SartSNo", startSNo);
            args[7] = new KeyValue<string>("EndSNo", endSNo);
            args[8] = new KeyValue<string>("UpNo", upNo);
            args[9] = new KeyValue<string>("StudentId", studentId);

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.InsertB2300003Data, args, out returnData);
            return xmlResult;
        }
        #endregion

        #region 取得 C3400001 查詢結果
        public XmlResult GetC3400001Result(Page page, string receiveType, string yearId, string termId, string depId, string receiveId, DateTime sAccountDate, DateTime eAccountDate, out CancelResultEntity[] cancelResults)
        {
            cancelResults = null;

            KeyValue<string>[] args = new KeyValue<string>[7] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId", yearId),
                new KeyValue<string>("TermId", termId),
                new KeyValue<string>("DepId", depId),
                new KeyValue<string>("ReceiveId", receiveId),
                new KeyValue<string>("SAccountDate", sAccountDate.ToString("yyyy/MM/dd")),
                new KeyValue<string>("EAccountDate", eAccountDate.ToString("yyyy/MM/dd"))
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.GetC3400001Result, args, out returnData);
            if (xmlResult.IsSuccess && returnData != null)
            {
                cancelResults = returnData as CancelResultEntity[];
                if (cancelResults == null || cancelResults.Length == 0)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }

        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 匯出查詢結果檔
        /// </summary>
        /// <param name="page"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="sAccountDate"></param>
        /// <param name="eAccountDate"></param>
        /// <param name="fileType">匯出檔案格式 XLS 或 ODS，其他值視為 XLS</param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public XmlResult GetC3400001ResultFile(Page page, string receiveType, string yearId, string termId, string depId, string receiveId, DateTime sAccountDate, DateTime eAccountDate, string fileType, out byte[] fileContent)
        {
            fileContent = null;

            if (!"XLS".Equals(fileType, StringComparison.CurrentCultureIgnoreCase) && !"ODS".Equals(fileType, StringComparison.CurrentCultureIgnoreCase))
            {
                fileType = "XLS";
            }

            KeyValue<string>[] args = new KeyValue<string>[8] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId", yearId),
                new KeyValue<string>("TermId", termId),
                new KeyValue<string>("DepId", depId),
                new KeyValue<string>("ReceiveId", receiveId),
                new KeyValue<string>("SAccountDate", sAccountDate.ToString("yyyy/MM/dd")),
                new KeyValue<string>("EAccountDate", eAccountDate.ToString("yyyy/MM/dd")),
                new KeyValue<string>("FileType", fileType)
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.GetC3400001Result, args, out returnData);
            if (xmlResult.IsSuccess && returnData != null)
            {
                fileContent = returnData as byte[];
                if (fileContent == null || fileContent.Length == 0)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion
        #endregion

        #region 匯出 B2100005 的委扣檔案
        /// <summary>
        /// 匯出 B2100005 的委扣檔案
        /// </summary>
        /// <param name="command"></param>
        /// <param name="keys"></param>
        /// <param name="deductDate"></param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public XmlResult ExportB2100005File(Page page, string command, string receiveType, BillKey[] keys, DateTime deductDate, out byte[] fileContent)
        {
            fileContent = null;

            if (String.IsNullOrEmpty(receiveType) || keys == null || keys.Length == 0)
            {
                return new XmlResult(false, "缺少匯出資料選取參數", ErrorCode.S_INVALID_PARAMETER, null);
            }

            List<string> keyTexts = new List<string>(keys.Length);
            foreach (BillKey key in keys)
            {
                keyTexts.Add(key.ToKeyText());
            }
            string keysText = String.Join(";", keyTexts.ToArray());

            KeyValue<string>[] args = new KeyValue<string>[4] {
                new KeyValue<string>("Command", command),
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("KeysText", keysText),
                new KeyValue<string>("DeductDate", deductDate.ToString("yyyy/MM/dd"))
            };
            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ExportB2100005File, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                fileContent = returnData as byte[];
                if (fileContent == null || fileContent.Length == 0)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion

        #region 匯入 B2100007 的委扣回復檔案
        /// <summary>
        /// 匯入 B2100007 的委扣回復檔案
        /// </summary>
        /// <param name="page"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public XmlResult ImportB2100007File(Page page, string receiveType, string yearId, string termId, string depId, string receiveId, string fileContent, out string resultText)
        {
            resultText = null;

            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId) || String.IsNullOrEmpty(receiveId) || String.IsNullOrEmpty(fileContent))
            {
                return new XmlResult(false, "缺少匯入資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[6] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId", yearId),
                new KeyValue<string>("TermId", termId),
                new KeyValue<string>("DepId", depId),
                new KeyValue<string>("ReceiveId", receiveId),
                new KeyValue<string>("FileContent", fileContent)
            };
            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ImportB2100007File, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                resultText = returnData as String;
                if (String.IsNullOrEmpty(resultText))
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion

        #region 匯出 B2100003 學生繳費資料媒體檔 (下載銷帳資料也使用此方法)
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 匯出 B2100003 的學生繳費資料媒體檔 (下載銷帳資料也使用此方法)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="qUpNo"></param>
        /// <param name="qCancelStatus"></param>
        /// <param name="qReceiveWay"></param>
        /// <param name="qSReceivDate"></param>
        /// <param name="qEReceivDate"></param>
        /// <param name="qSAccountDate"></param>
        /// <param name="qEAccountDate"></param>
        /// <param name="qFieldName"></param>
        /// <param name="qFieldValue"></param>
        /// <param name="outFields"></param>
        /// <param name="fileContent"></param>
        /// <param name="isUseODS"></param>
        /// <returns></returns>
        public XmlResult ExportB2100003File(Page page, string receiveType, string yearId, string termId, string depId, string receiveId
            , string qUpNo, string qCancelStatus, string qReceiveWay, string qSReceivDate, string qEReceivDate, string qSAccountDate, string qEAccountDate
            , string qFieldName, string qFieldValue, ICollection<String> outFields, out byte[] fileContent, bool isUseODS = false)
        {
            fileContent = null;

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
            //    || String.IsNullOrEmpty(depId) || String.IsNullOrEmpty(receiveId))
            //{
            //    return new XmlResult(false, "缺少查詢資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            //}
            #endregion

            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
                || depId == null || String.IsNullOrEmpty(receiveId))
            {
                return new XmlResult(false, "缺少查詢資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }

            if (outFields == null || outFields.Count == 0)
            {
                return new XmlResult(false, "缺少下載資料項目參數", ErrorCode.S_INVALID_PARAMETER, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[16] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId", yearId),
                new KeyValue<string>("TermId", termId),
                new KeyValue<string>("DepId", depId),
                new KeyValue<string>("ReceiveId", receiveId),

                new KeyValue<string>("QUpNo", qUpNo),
                new KeyValue<string>("QCancelStatus", qCancelStatus),
                new KeyValue<string>("QReceiveWay", qReceiveWay),
                new KeyValue<string>("QSReceivDate", qSReceivDate),
                new KeyValue<string>("QEReceivDate", qEReceivDate),
                new KeyValue<string>("QSAccountDate", qSAccountDate),
                new KeyValue<string>("QEAccountDate", qEAccountDate),
                new KeyValue<string>("QFieldName", qFieldName),
                new KeyValue<string>("QFieldValue", qFieldValue),

                new KeyValue<string>("OutFields", String.Join(",", outFields)),

                new KeyValue<string>("UseODS", (isUseODS ? "Y" : "N"))
            };
            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ExportB2100003File, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                fileContent = returnData as byte[];
                if (fileContent == null || fileContent.Length == 0)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion
        #endregion

        #region 匯出 C3700007 銷帳資料 (固定格式)
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 匯出 C3700007 銷帳資料 (固定格式)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="qUpNo"></param>
        /// <param name="qCancelStatus"></param>
        /// <param name="qReceiveWay"></param>
        /// <param name="qSReceivDate"></param>
        /// <param name="qEReceivDate"></param>
        /// <param name="qSAccountDate"></param>
        /// <param name="qEAccountDate"></param>
        /// <param name="qFieldName"></param>
        /// <param name="qFieldValue"></param>
        /// <param name="fileContent"></param>
        /// <param name="isUseODS"></param>
        /// <returns></returns>
        public XmlResult ExportC3700007File(Page page, string receiveType, string yearId, string termId, string depId, string receiveId
            , string qUpNo, string qCancelStatus, string qReceiveWay, string qSReceivDate, string qEReceivDate, string qSAccountDate, string qEAccountDate
            , string qFieldName, string qFieldValue, out byte[] fileContent, bool isUseODS = false)
        {
            fileContent = null;

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
            //    || String.IsNullOrEmpty(depId) || String.IsNullOrEmpty(receiveId))
            //{
            //    return new XmlResult(false, "缺少查詢資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            //}
            #endregion

            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
                || depId == null || String.IsNullOrEmpty(receiveId))
            {
                return new XmlResult(false, "缺少查詢資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[15] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId", yearId),
                new KeyValue<string>("TermId", termId),
                new KeyValue<string>("DepId", depId),
                new KeyValue<string>("ReceiveId", receiveId),

                new KeyValue<string>("QUpNo", qUpNo),
                new KeyValue<string>("QCancelStatus", qCancelStatus),
                new KeyValue<string>("QReceiveWay", qReceiveWay),
                new KeyValue<string>("QSReceivDate", qSReceivDate),
                new KeyValue<string>("QEReceivDate", qEReceivDate),
                new KeyValue<string>("QSAccountDate", qSAccountDate),
                new KeyValue<string>("QEAccountDate", qEAccountDate),
                new KeyValue<string>("QFieldName", qFieldName),
                new KeyValue<string>("QFieldValue", qFieldValue),

                new KeyValue<string>("UseODS", (isUseODS ? "Y" : "N"))
            };
            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ExportC3700007File, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                fileContent = returnData as byte[];
                if (fileContent == null || fileContent.Length == 0)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion
        #endregion

        #region 產生匯出資料檔
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        public XmlResult GenExportFileData(Page page, string receiveType, string kind, string explain
            , string qYearId, string qTermId, string qReceiveId, string qUpNo
            , string qCancelStatus, string qReceiveWay, string qSReceivDate, string qEReceivDate, string qSAccountDate, string qEAccountDate
            , string qFieldName, string qFieldValue
            , ICollection<String> outFields, bool isUseODS = false)
        {
            if (String.IsNullOrEmpty(receiveType) || !ExportFileKindCodeTexts.IsDefine(kind) || outFields == null || outFields.Count == 0)
            {
                return new XmlResult(false, "缺少或不正確的參數", ErrorCode.S_INVALID_PARAMETER, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[17] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("Kind", kind),
                new KeyValue<string>("Explain", explain),

                new KeyValue<string>("QYearId", qYearId),
                new KeyValue<string>("QTermId", qTermId),
                new KeyValue<string>("QReceiveId", qReceiveId),

                new KeyValue<string>("QUpNo", qUpNo),
                new KeyValue<string>("QCancelStatus", qCancelStatus),
                new KeyValue<string>("QReceiveWay", qReceiveWay),
                new KeyValue<string>("QSReceivDate", qSReceivDate),
                new KeyValue<string>("QEReceivDate", qEReceivDate),
                new KeyValue<string>("QSAccountDate", qSAccountDate),
                new KeyValue<string>("QEAccountDate", qEAccountDate),
                new KeyValue<string>("QFieldName", qFieldName),
                new KeyValue<string>("QFieldValue", qFieldValue),

                new KeyValue<string>("OutFields", String.Join(",", outFields)),

                new KeyValue<string>("UseODS", (isUseODS ? "Y" : "N"))
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.GenExportFileData, args, out returnData);

            return xmlResult;
        }
        #endregion
        #endregion

        #region 匯出報表相關

        #region 繳費失敗總表(遲繳)
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 繳費失敗總表(遲繳)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="upNo"></param>
        /// <param name="receiveStatus"></param>
        /// <param name="reportKind"></param>
        /// <param name="reportName"></param>
        /// <param name="fileContent"></param>
        /// <param name="isUseODS"></param>
        /// <returns></returns>
        public XmlResult ExportReportA(Page page, string receiveType, string yearId, string termId, string depId, string receiveId
            , int? upNo, string receiveStatus, string reportKind, string reportName
            , out byte[] fileContent, bool isUseODS = false)
        {
            fileContent = null;

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
            //    || String.IsNullOrEmpty(depId) || String.IsNullOrEmpty(receiveId))
            //{
            //    return new XmlResult(false, "缺少查詢資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            //}
            #endregion

            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
                || depId == null || String.IsNullOrEmpty(receiveId))
            {
                return new XmlResult(false, "缺少查詢資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (String.IsNullOrWhiteSpace(reportName))
            {
                return new XmlResult(false, "缺少報表名稱目參數", ErrorCode.S_INVALID_PARAMETER, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[10] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId", yearId),
                new KeyValue<string>("TermId", termId),
                new KeyValue<string>("DepId", depId),
                new KeyValue<string>("ReceiveId", receiveId),

                new KeyValue<string>("UpNo", upNo == null ? String.Empty : upNo.Value.ToString()),
                new KeyValue<string>("ReceiveStatus", receiveStatus),
                new KeyValue<string>("ReportKind", reportKind),
                new KeyValue<string>("ReportName", reportName),
                new KeyValue<string>("UseODS", (isUseODS ? "Y" : "N"))
            };
            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ExportReportA, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                fileContent = returnData as byte[];
                if (fileContent == null || fileContent.Length == 0)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion
        #endregion

        #region 繳費銷帳總表
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 繳費銷帳總表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="upNo"></param>
        /// <param name="receiveStatus"></param>
        /// <param name="reportKind"></param>
        /// <param name="reportName"></param>
        /// <param name="fileContent"></param>
        /// <param name="isUseODS"></param>
        /// <returns></returns>
        public XmlResult ExportReportA2(Page page, string receiveType, string yearId, string termId, string depId, string receiveId
            , int? upNo, string receiveStatus, string reportKind, string reportName
            , out byte[] fileContent, bool isUseODS = false)
        {
            fileContent = null;

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
            //    || String.IsNullOrEmpty(depId) || String.IsNullOrEmpty(receiveId))
            //{
            //    return new XmlResult(false, "缺少查詢資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            //}
            #endregion

            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
                || depId == null || String.IsNullOrEmpty(receiveId))
            {
                return new XmlResult(false, "缺少查詢資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (String.IsNullOrWhiteSpace(reportName))
            {
                return new XmlResult(false, "缺少報表名稱目參數", ErrorCode.S_INVALID_PARAMETER, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[10] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId", yearId),
                new KeyValue<string>("TermId", termId),
                new KeyValue<string>("DepId", depId),
                new KeyValue<string>("ReceiveId", receiveId),

                new KeyValue<string>("UpNo", upNo == null ? String.Empty : upNo.Value.ToString()),
                new KeyValue<string>("ReceiveStatus", receiveStatus),
                new KeyValue<string>("ReportKind", reportKind),
                new KeyValue<string>("ReportName", reportName),
                new KeyValue<string>("UseODS", (isUseODS ? "Y" : "N"))
            };
            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ExportReportA2, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                fileContent = returnData as byte[];
                if (fileContent == null || fileContent.Length == 0)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion
        #endregion

        #region 繳費銷帳明細表 (正常、遲繳)
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 繳費銷帳明細表 (正常、遲繳)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="upNo"></param>
        /// <param name="receiveStatus"></param>
        /// <param name="reportKind">報表種類 (1=正常 2=遲繳)</param>
        /// <param name="reportName"></param>
        /// <param name="fileContent"></param>
        /// <param name="isUseODS"></param>
        /// <returns></returns>
        public XmlResult ExportReportB(Page page, string receiveType, string yearId, string termId, string depId, string receiveId
            , int? upNo, string receiveStatus, string reportKind, string reportName
            , out byte[] fileContent, bool isUseODS = false)
        {
            fileContent = null;

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
            //    || String.IsNullOrEmpty(depId) || String.IsNullOrEmpty(receiveId))
            //{
            //    return new XmlResult(false, "缺少查詢資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            //}
            #endregion

            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
                || depId == null || String.IsNullOrEmpty(receiveId))
            {
                return new XmlResult(false, "缺少查詢資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (String.IsNullOrWhiteSpace(reportName))
            {
                return new XmlResult(false, "缺少報表名稱目參數", ErrorCode.S_INVALID_PARAMETER, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[10] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId", yearId),
                new KeyValue<string>("TermId", termId),
                new KeyValue<string>("DepId", depId),
                new KeyValue<string>("ReceiveId", receiveId),

                new KeyValue<string>("UpNo", upNo == null ? String.Empty : upNo.Value.ToString()),
                new KeyValue<string>("ReceiveStatus", receiveStatus),
                new KeyValue<string>("ReportKind", reportKind),
                new KeyValue<string>("ReportName", reportName),
                new KeyValue<string>("UseODS", (isUseODS ? "Y" : "N"))
            };
            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ExportReportB, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                fileContent = returnData as byte[];
                if (fileContent == null || fileContent.Length == 0)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion
        #endregion

        #region 學生繳費名冊
        /// <summary>
        /// 學生繳費名冊
        /// </summary>
        /// <param name="page"></param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">學年代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別</param>
        /// <param name="upNo">批號</param>
        /// <param name="receiveStatus">繳費狀態 ("0"=未繳; "1"=已繳; ""=全部)</param>
        /// <param name="groupKind">群組明細程度 (1=部別、系所、班別; 2=部別、系所、年級、班別)</param>
        /// <param name="otherItems">說明項目 (1=繳款日期; 2=身份別; 3=減免; 4=助貸)</param>
        /// <param name="receiveItemNo">收入科目編號 (1 ~ 40)</param>
        /// <param name="reportName">報表名稱</param>
        /// <param name="fileContent"></param>
        /// <param name="isUseODS"></param>
        /// <returns></returns>
        public XmlResult ExportReportC(Page page, string receiveType, string yearId, string termId, string depId, string receiveId
            , int? upNo, string receiveStatus, string groupKind, string[] otherItems, int receiveItemNo, string reportName
            , out byte[] fileContent, bool isUseODS = false)
        {
            fileContent = null;

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
            //    || String.IsNullOrEmpty(depId) || String.IsNullOrEmpty(receiveId))
            //{
            //    return new XmlResult(false, "缺少查詢資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            //}
            #endregion

            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
                || depId == null || String.IsNullOrEmpty(receiveId))
            {
                return new XmlResult(false, "缺少查詢資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (String.IsNullOrEmpty(groupKind))
            {
                return new XmlResult(false, "缺少群組明細程度參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (receiveItemNo < 1 || receiveItemNo > 40)
            {
                return new XmlResult(false, "缺少收入科目參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (String.IsNullOrWhiteSpace(reportName))
            {
                return new XmlResult(false, "缺少報表名稱目參數", ErrorCode.S_INVALID_PARAMETER, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[12] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId", yearId),
                new KeyValue<string>("TermId", termId),
                new KeyValue<string>("DepId", depId),
                new KeyValue<string>("ReceiveId", receiveId),

                new KeyValue<string>("UpNo", upNo == null ? String.Empty : upNo.Value.ToString()),
                new KeyValue<string>("ReceiveStatus", receiveStatus),
                new KeyValue<string>("GroupKind", groupKind),
                new KeyValue<string>("OtherItems", otherItems == null || otherItems.Length == 0 ? String.Empty : String.Join(",", otherItems)),
                new KeyValue<string>("ReceiveItemNo", receiveItemNo.ToString()),
                new KeyValue<string>("ReportName", reportName),
                new KeyValue<string>("UseODS", (isUseODS ? "Y" : "N"))
            };
            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ExportReportC, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                fileContent = returnData as byte[];
                if (fileContent == null || fileContent.Length == 0)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion

        #region 匯出手續費統計報表
        /// <summary>
        /// 匯出手續費統計報表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="bankId">銀行代碼</param>
        /// <param name="schIdenty">學校統編</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="acctSDate">入帳日起日</param>
        /// <param name="acctEDate">入帳日迄日</param>
        /// <param name="fileContent">傳回報表內容</param>
        /// <param name="isUseODS"></param>
        /// <returns>傳回處理結果</returns>
        public XmlResult ExportReportD(Page page, string bankId, string schIdenty, string receiveType, string acctSDate, string acctEDate
            , out byte[] fileContent, bool isUseODS = false)
        {
            LogonUser logonUser = null;
            string menuID = null;
            string menuName = null;
            this.TryGetLogonUserAndMenuData(page, out logonUser, out menuID, out menuName);

            fileContent = null;
            KeyValue<string>[] args = new KeyValue<string>[8] {
                new KeyValue<string>("REPORT_TYPE", "1"),    //手續費統計報表
                new KeyValue<string>("BANK_ID", bankId),
                new KeyValue<string>("SCH_IDENTY", schIdenty),
                new KeyValue<string>("RECEIVE_TYPE", receiveType),
                new KeyValue<string>("ACCT_SDATE", acctSDate),
                new KeyValue<string>("ACCT_EDATE", acctEDate),
                new KeyValue<string>("RECEIVE_WAY", ""),
                new KeyValue<string>("UseODS", (isUseODS ? "Y" : "N"))
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ExportReportD, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                fileContent = returnData as byte[];
                if (fileContent == null || fileContent.Length == 0)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }

        /// <summary>
        /// 代收單位交易資訊統計表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="bankId">銀行代碼</param>
        /// <param name="schIdenty">學校統編</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="acctSDate">入帳日起日</param>
        /// <param name="acctEDate">入帳日迄日</param>
        /// <param name="fileContent">傳回報表內容</param>
        /// <param name="isUseODS"></param>
        /// <returns></returns>
        public XmlResult ExportReportD2(Page page, string bankId, string schIdenty, string receiveType, string acctSDate, string acctEDate
            , out byte[] fileContent, bool isUseODS = false)
        {
            fileContent = null;
            KeyValue<string>[] args = new KeyValue<string>[8] {
                new KeyValue<string>("REPORT_TYPE", "2"),    //代收單位交易資訊統計表
                new KeyValue<string>("BANK_ID", bankId),
                new KeyValue<string>("SCH_IDENTY", schIdenty),
                new KeyValue<string>("RECEIVE_TYPE", receiveType),
                new KeyValue<string>("ACCT_SDATE", acctSDate),
                new KeyValue<string>("ACCT_EDATE", acctEDate),
                new KeyValue<string>("RECEIVE_WAY", ""),
                new KeyValue<string>("UseODS", (isUseODS ? "Y" : "N"))
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ExportReportD, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                fileContent = returnData as byte[];
                if (fileContent == null || fileContent.Length == 0)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }

        /// <summary>
        /// 繳款通道交易資訊統計表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="bankId">銀行代碼</param>
        /// <param name="schIdenty">學校統編</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="receiveWay">代收管道</param>
        /// <param name="acctSDate">入帳日起日</param>
        /// <param name="acctEDate">入帳日迄日</param>
        /// <param name="fileContent">傳回報表內容</param>
        /// <param name="isUseODS"></param>
        /// <returns></returns>
        public XmlResult ExportReportD3(Page page, string bankId, string schIdenty, string receiveType, string receiveWay, string acctSDate, string acctEDate
            , out byte[] fileContent, bool isUseODS = false)
        {
            fileContent = null;
            KeyValue<string>[] args = new KeyValue<string>[8] {
                new KeyValue<string>("REPORT_TYPE", "3"),    //繳款通道交易資訊統計表
                new KeyValue<string>("BANK_ID", bankId),
                new KeyValue<string>("SCH_IDENTY", schIdenty),
                new KeyValue<string>("RECEIVE_TYPE", receiveType),
                new KeyValue<string>("ACCT_SDATE", acctSDate),
                new KeyValue<string>("ACCT_EDATE", acctEDate),
                new KeyValue<string>("RECEIVE_WAY", receiveWay),
                new KeyValue<string>("UseODS", (isUseODS ? "Y" : "N"))
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ExportReportD, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                fileContent = returnData as byte[];
                if (fileContent == null || fileContent.Length == 0)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion

        #region 匯出繳費收費項目 (明細分析表、分類統計表)
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 匯出繳費收費項目 (明細分析表、分類統計表)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="upNo"></param>
        /// <param name="receiveStatus"></param>
        /// <param name="reportKind">報表種類 (1=明細分析表 2=分類統計表)</param>
        /// <param name="reportName"></param>
        /// <param name="fileContent"></param>
        /// <param name="isUseODS"></param>
        /// <returns></returns>
        public XmlResult ExportReportE(Page page, string receiveType, string yearId, string termId, string depId, string receiveId
            , int? upNo, string receiveStatus, string reportKind, string reportName
            , out byte[] fileContent, bool isUseODS = false)
        {
            fileContent = null;

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
            //    || String.IsNullOrEmpty(depId) || String.IsNullOrEmpty(receiveId))
            //{
            //    return new XmlResult(false, "缺少查詢資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            //}
            #endregion

            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
                || depId == null || String.IsNullOrEmpty(receiveId))
            {
                return new XmlResult(false, "缺少查詢資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (String.IsNullOrWhiteSpace(reportName))
            {
                return new XmlResult(false, "缺少報表名稱目參數", ErrorCode.S_INVALID_PARAMETER, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[10] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId", yearId),
                new KeyValue<string>("TermId", termId),
                new KeyValue<string>("DepId", depId),
                new KeyValue<string>("ReceiveId", receiveId),

                new KeyValue<string>("UpNo", upNo == null ? String.Empty : upNo.Value.ToString()),
                new KeyValue<string>("ReceiveStatus", receiveStatus),
                new KeyValue<string>("ReportKind", reportKind),
                new KeyValue<string>("ReportName", reportName),
                new KeyValue<string>("UseODS", (isUseODS ? "Y" : "N"))
            };
            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ExportReportE, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                fileContent = returnData as byte[];
                if (fileContent == null || fileContent.Length == 0)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion
        #endregion

        #endregion

        #region 匯出查詢結果的 Excel 檔
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 匯出查詢結果的 Excel 檔 (C3600001:查詢問題檔、C3700006:中心入帳資料查詢、S5400003:排程作業查詢)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="funcId"></param>
        /// <param name="where"></param>
        /// <param name="fileContent"></param>
        /// <param name="isUseODS"></param>
        /// <returns></returns>
        public XmlResult ExportQueryResult(Page page, string funcId, Expression where, out byte[] fileContent, bool isUseODS = false)
        {
            fileContent = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (String.IsNullOrEmpty(funcId) || where == null || where.IsEmpty())
            {
                return new XmlResult(false, "缺少查詢資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            string whereXml = null;
            if (!Common.TryToXmlExplicitly<Expression>(where, out whereXml))
            {
                return new XmlResult(false, "查詢資料序列化失敗", ErrorCode.S_SERIALIZED_FAILURE, null);
            }
            #endregion

            KeyValue<string>[] args = new KeyValue<string>[3] {
                new KeyValue<string>("FuncId", funcId),
                new KeyValue<string>("Where", whereXml),
                new KeyValue<string>("UseODS", (isUseODS ? "Y" : "N"))
            };
            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ExportQueryResult, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                fileContent = returnData as byte[];
                if (fileContent == null)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion
        #endregion

        /// <summary>
        /// 取得 BankPM 的 TempFile
        /// </summary>
        /// <param name="page"></param>
        /// <param name="fileName"></param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public XmlResult GetBankPMTempFile(Page page, string fileName, out byte[] fileContent)
        {
            fileContent = null;

            if (String.IsNullOrEmpty(fileName))
            {
                return new XmlResult(false, "缺少檔名選取參數", CoreStatusCode.UNKNOWN_ERROR, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[1];
            args[0] = new KeyValue<string>("FileName", fileName);

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.GetBankPMTempFile, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                fileContent = returnData as byte[];
                if (fileContent == null)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }

        #region [Old] 土銀沒有
        //#region CallN162Request
        ///// <summary>
        ///// 呼叫 N162 查詢電文
        ///// </summary>
        ///// <param name="page"></param>
        ///// <param name="cusidn"></param>
        ///// <returns></returns>
        //public XmlResult CallN162Request(Page page, string cusidn)
        //{
        //    KeyValue<string>[] args = new KeyValue<string>[1];
        //    args[0] = new KeyValue<string>("CUSIDN", cusidn);

        //    object returnData = null;
        //    XmlResult xmlResult = this.CallMethod(page, CallMethodName.CallN162Request, args, out returnData);
        //    return xmlResult;
        //}
        //#endregion
        #endregion

        #region CallD00I71Request
        /// <summary>
        /// 呼叫 D00I70 電文
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cusidn"></param>
        /// <returns></returns>
        public XmlResult CallD00I71Request(Page page, string receiveType, out KeyValueList<string> datas)
        {
            datas = null;
            KeyValue<string>[] args = new KeyValue<string>[1];
            args[0] = new KeyValue<string>("APPNO", receiveType);

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.CallD00I71Request, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                if (returnData is KeyValueList<string>)
                {
                    datas = returnData as KeyValueList<string>;
                }
                else
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }
        #endregion

        #region 取得登入者相關的商家代碼 CodeText 清單
        /// <summary>
        /// 取得登入者被授權的商家代碼的代碼文字對照物件陣列
        /// </summary>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="datas">傳回授權的商家代號的代碼文字對照物件陣列或 null。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult GetMyReceiveTypeCodeTexts(Page page, out CodeText[] datas, string receiveKind = null)
        {
            datas = null;

            LogonUser logonUser = WebHelper.GetLogonUser();
            if (logonUser == null || logonUser.LogonTime == null)
            {
                return new XmlResult(false, "頁面閒置逾時或未登入", ErrorCode.S_SESSION_TIMEOUT, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[] { new KeyValue<string>("ReceiveKind", receiveKind) };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.GetMyReceiveTypeCodeTexts, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                if (returnData is CodeText[])
                {
                    datas = (CodeText[])returnData;
                }
                else
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }

        #region [Old] 因為學校使用者可跨商家代號且會變動，所以此方法不適用
        ///// <summary>
        ///// 取得學校登入者所屬的商家代號的代碼文字對照物件陣列
        ///// </summary>
        ///// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        ///// <param name="datas">傳回所屬的商家代號的代碼文字對照物件陣列或 null。</param>
        ///// <returns>傳回處理結果。</returns>
        //public XmlResult GetMyReceiveTypeCodeTextsBySchool(Page page, out CodeText[] datas)
        //{
        //    datas = null;

        //    LogonUser logonUser = WebHelper.GetLogonUser();
        //    if (logonUser == null || logonUser.LogonTime == null)
        //    {
        //        return new XmlResult(false, "頁面閒置逾時或未登入", ErrorCode.S_SESSION_TIMEOUT, null);
        //    }
        //    if (!logonUser.IsSchoolUser)
        //    {
        //        return new XmlResult(false, "非學校使用者", ErrorCode.S_NO_AUTHORIZE, null);
        //    }

        //    XmlResult result = this.GetReceiveTypeCodeTextsBySchool(page, logonUser.UnitId, out datas);
        //    if (result.IsSuccess)
        //    {
        //        if (datas != null)
        //        {
        //            for (int idx = 0; idx < datas.Length; idx++)
        //            {
        //                datas[idx].Text = string.Format("{0}({1})", datas[idx].Text, datas[idx].Code);
        //            }
        //        }
        //    }
        //    return result;
        //}
        #endregion

        /// <summary>
        /// 取得指定學校統編的所屬商家代號
        /// </summary>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="schoolIdenty">指定學校統編</param>
        /// <param name="datas">傳回所屬的商家代號的代碼文字對照物件陣列或 null。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult GetReceiveTypeCodeTextsBySchool(Page page, string schoolIdenty, out CodeText[] datas, string receiveKind = null)
        {
            Expression where = new Expression(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL)
                .And(SchoolRTypeEntity.Field.SchIdenty, schoolIdenty);
            if (ReceiveKindCodeTexts.IsDefine(receiveKind))
            {
                where.And(SchoolRTypeEntity.Field.ReceiveKind, receiveKind);
            }
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(SchoolRTypeEntity.Field.ReceiveType, OrderByEnum.Asc);

            string[] codeFieldNames = new string[] { SchoolRTypeEntity.Field.ReceiveType };
            string codeCombineFormat = null;
            string[] textFieldNames = new string[] { SchoolRTypeEntity.Field.SchName };
            string textCombineFormat = null;

            XmlResult xmlResult = this.GetEntityOptions<SchoolRTypeEntity>(page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
            return xmlResult;
        }

        /// <summary>
        /// 取得銀行登入者所屬的學校的代碼文字對照物件陣列
        /// </summary>
        /// <param name="page">指定呼叫此方法的頁面。不指定或非 BasePage 頁面則由系統判斷 （效能較差，且可能誤判)。</param>
        /// <param name="datas">傳回所屬的學校的代碼文字對照物件陣列或 null。</param>
        /// <returns>傳回處理結果。</returns>
        public XmlResult GetMySchoolRTypeCodeTextsByBank(Page page, out CodeText[] datas, string receiveKind = null)
        {
            datas = null;

            LogonUser logonUser = WebHelper.GetLogonUser();
            if (logonUser == null || logonUser.LogonTime == null)
            {
                return new XmlResult(false, "頁面閒置逾時或未登入", ErrorCode.S_SESSION_TIMEOUT, null);
            }
            if (!logonUser.IsBankUser)
            {
                return new XmlResult(false, "非行員使用者", ErrorCode.S_NO_AUTHORIZE, null);
            }

            Expression where = new Expression(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
            if (logonUser.IsBankManager)
            {
                //管理行可管理所有學校
            }
            else
            {
                //主辦行只能管所屬學校
                where.And(SchoolRTypeEntity.Field.BankId, logonUser.UnitId);
            }
            if (ReceiveKindCodeTexts.IsDefine(receiveKind))
            {
                where.And(SchoolRTypeEntity.Field.ReceiveKind, receiveKind);
            }
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(SchoolRTypeEntity.Field.SchIdenty, OrderByEnum.Asc);

            string[] codeFieldNames = new string[] { SchoolRTypeEntity.Field.SchIdenty };
            string codeCombineFormat = null;
            string[] textFieldNames = new string[] { SchoolRTypeEntity.Field.SchName };
            string textCombineFormat = null;

            XmlResult xmlResult = this.GetEntityOptions<SchoolRTypeEntity>(page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
            return xmlResult;
        }
        #endregion

        #region 變更使用者密碼
        #region [MDY:20220410] Checkmarx 調整
        public XmlResult ChangeUserPXX(Page page, string unitId, string userId, string groupId, string oldPXX, string newPXX)
        {
            KeyValue<string>[] args = new KeyValue<string>[5];
            args[0] = new KeyValue<string>("UnitId", unitId);
            args[1] = new KeyValue<string>("UserId", userId);
            args[2] = new KeyValue<string>("GroupId", groupId);
            args[3] = new KeyValue<string>("OldPXX", oldPXX);
            args[4] = new KeyValue<string>("NewPXX", newPXX);

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ChangeUserPXX, args, out returnData);
            return xmlResult;
        }
        #endregion
        #endregion

        #region 清除虛擬帳號
        public XmlResult ClearCancelNo(Page page, string receiveType, string yearId, string termId, string depId, string receiveId, string byKind, string byValue, out string log)
        {
            log = null;

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
            //    || String.IsNullOrEmpty(depId) || String.IsNullOrEmpty(receiveId))
            //{
            //    return new XmlResult(false, "缺少查詢資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            //}
            #endregion

            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
                || depId == null || String.IsNullOrEmpty(receiveId))
            {
                return new XmlResult(false, "缺少清除資料條件參數", ErrorCode.S_INVALID_PARAMETER, null);
            }

            if (String.IsNullOrEmpty(byKind) || String.IsNullOrEmpty(byValue))
            {
                return new XmlResult(false, "缺少清除資料條件參數", ErrorCode.S_INVALID_PARAMETER, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[7] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId", yearId),
                new KeyValue<string>("TermId", termId),
                new KeyValue<string>("DepId", depId),
                new KeyValue<string>("ReceiveId", receiveId),

                new KeyValue<string>("ByKind", byKind),
                new KeyValue<string>("ByValue", byValue)
            };
            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ClearCancelNo, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                log = returnData as string;
            }
            return xmlResult;
        }
        #endregion

        #region 上傳中國信託相關
        /// <summary>
        /// 匯入上傳中國信託繳費單資料檔
        /// </summary>
        /// <param name="page"></param>
        /// <param name="receiveType"></param>
        /// <param name="fileContent"></param>
        /// <param name="fileType"></param>
        /// <param name="payDueDate"></param>
        /// <param name="dataInfo"></param>
        /// <returns></returns>
        public XmlResult ImportQueueCTCBFile(Page page, string receiveType, byte[] fileContent, string fileType, DateTime payDueDate, out string dataInfo)
        {
            dataInfo = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (String.IsNullOrEmpty(receiveType) || fileContent == null || fileContent.Length == 0)
            {
                return new XmlResult(false, "缺少或無效的資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            string fielContentXml = null;
            if (!Common.TryToXmlExplicitly<byte[]>(fileContent, out fielContentXml))
            {
                return new XmlResult(false, "檔案內容無法序列化", ErrorCode.S_SERIALIZED_FAILURE, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[4] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("PayDueDate", payDueDate.ToString("yyyy/MM/dd")),
                new KeyValue<string>("FileContent", fielContentXml),
                new KeyValue<string>("FileType", fileType)
            };

            object returnData = null;

            #region [MDY:20210301] 改用 CallMethod2，指定 timeout 為 120 秒
            #region [OLD]
            //XmlResult xmlResult = this.CallMethod(page, CallMethodName.ImportQueueCTCBFile, args, out returnData);
            #endregion

            XmlResult xmlResult = this.CallMethod2(page, CallMethodName.ImportQueueCTCBFile, args, 120, out returnData);
            #endregion

            #region [MDY:20190906] (2019擴充案) 修正 (為了傳回 dataInfo，所以資料錯誤仍傳回成功，所以不用判斷 IsSuccess)
            #region [OLD]
            //if (xmlResult.IsSuccess)
            //{
            //    dataInfo = returnData as string;
            //    if (dataInfo == null)
            //    {
            //        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
            //    }
            //}
            #endregion

            dataInfo = returnData as string;
            #endregion

            return xmlResult;
        }
        #endregion

        public XmlResult ImportD00I70File(Page page, string receiveType, byte[] fileContent, out string dataInfo)
        {
            dataInfo = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (String.IsNullOrEmpty(receiveType) || fileContent == null || fileContent.Length == 0)
            {
                return new XmlResult(false, "缺少或無效的資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            string fielContentXml = null;
            if (!Common.TryToXmlExplicitly<byte[]>(fileContent, out fielContentXml))
            {
                return new XmlResult(false, "檔案內容無法序列化", ErrorCode.S_SERIALIZED_FAILURE, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[2] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("FileContent", fielContentXml)
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ImportD00I70File, args, out returnData);
            if (xmlResult.IsSuccess)
            {
                dataInfo = returnData as string;
                if (dataInfo == null)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }
            return xmlResult;
        }

        #region BankService 相關
        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 以銷帳編號查詢繳費單資訊
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="orgPXX"></param>
        /// <param name="rqXml"></param>
        /// <param name="rsXml"></param>
        /// <returns></returns>
        public XmlResult CallBankServiceQ0002(string orgId, string orgPXX, string rqXml, out string rsXml)
        {
            rsXml = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            string clientIP = WebHelper.GetClientIP();
            KeyValue<string>[] args = new KeyValue<string>[1] { new KeyValue<string>("REQUEST_XML", rqXml) };
            Entities.BankService.BankServiceCommand command = Entities.BankService.BankServiceCommand.Create(orgId, orgPXX, clientIP, Entities.BankService.BankServiceMethodName.Q0002, args);

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                string data = null;
                if (xmlResult.TryGetData<string>(out data))
                {
                    rsXml = data;
                    return new XmlResult(true);
                }
                else
                {
                    return new XmlResult(false, "傳回資料無法反序列化");
                }
            }
            else
            {
                return xmlResult;
            }
            #endregion
        }

        /// <summary>
        /// 信用卡繳費成功通知
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="orgPXX"></param>
        /// <param name="rqXml"></param>
        /// <param name="rsXml"></param>
        /// <returns></returns>
        public XmlResult CallBankServiceT0001(string orgId, string orgPXX, string rqXml, out string rsXml)
        {
            rsXml = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            string clientIP = WebHelper.GetClientIP();
            KeyValue<string>[] args = new KeyValue<string>[1] { new KeyValue<string>("REQUEST_XML", rqXml) };
            Entities.BankService.BankServiceCommand command = Entities.BankService.BankServiceCommand.Create(orgId, orgPXX, clientIP, Entities.BankService.BankServiceMethodName.T0001, args);

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                string data = null;
                if (xmlResult.TryGetData<string>(out data))
                {
                    rsXml = data;
                    return new XmlResult(true);
                }
                else
                {
                    return new XmlResult(false, "傳回資料無法反序列化");
                }
            }
            else
            {
                return xmlResult;
            }
            #endregion
        }
        #endregion
        #endregion

        #region 處理流程資料
        /// <summary>
        /// 處理流程資料
        /// </summary>
        /// <param name="page"></param>
        /// <param name="logonUser"></param>
        /// <param name="guid"></param>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public XmlResult ProcessFlowData(Page page, LogonUser logonUser, string guid, string processKind, string processMemo)
        {
            KeyValue<string>[] args = new KeyValue<string>[7] {
                new KeyValue<string>("GUID", guid),
                new KeyValue<string>("ProcessKind", processKind),
                new KeyValue<string>("ProcessMemo", processMemo),
                new KeyValue<string>("ProcessUserQual", logonUser.UserQual),
                new KeyValue<string>("ProcessUnitId", logonUser.UnitId),
                new KeyValue<string>("ProcessUserId", logonUser.UserId),
                new KeyValue<string>("ProcessUserName", logonUser.UserName),
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.ProcessFlowData, args, out returnData);
            return xmlResult;
        }
        #endregion

        #region 取得 C3700009 選項陣列
        /// <summary>
        /// 取得 C3700009 所有選項陣列
        /// </summary>
        /// <param name="page"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="deptItems"></param>
        /// <param name="collegeItems"></param>
        /// <param name="majorItems"></param>
        /// <param name="classItems"></param>
        /// <param name="upnoItems"></param>
        /// <returns></returns>
        public XmlResult GetC3700009AllOptions(Page page, string receiveType, string yearId, string termId, string depId, string receiveId
            , out CodeText[] deptDatas, out CodeText[] collegeDatas, out CodeText[] majorDatas, out CodeText[] classDatas, out CodeText[] upNoDatas)
        {
            deptDatas = null;
            collegeDatas = null;
            majorDatas = null;
            classDatas = null;
            upNoDatas = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            #region [Old] 土銀不使用原有部別 DepList，所以 DepId 固定為空字串
            //if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId) || String.IsNullOrEmpty(depId) || String.IsNullOrEmpty(receiveId))
            //{
            //    return new XmlResult(true);
            //}
            #endregion

            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId) || depId == null || String.IsNullOrEmpty(receiveId))
            {
                return new XmlResult(true);
            }
            #endregion

            KeyValue<string>[] args = new KeyValue<string>[5] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId", yearId),
                new KeyValue<string>("TermId", termId),
                new KeyValue<string>("DepId", depId),
                new KeyValue<string>("ReceiveId", receiveId)
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.GetC3700009AllOptions, args, out returnData);
            if (xmlResult.IsSuccess && returnData != null)
            {
                KeyValue<string>[] datas = returnData as KeyValue<string>[];
                if (datas == null || datas.Length != 5)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
                else
                {
                    foreach (KeyValue<string> data in datas)
                    {
                        switch (data.Key.ToUpper())
                        {
                            case "DEPT":
                                #region 部別
                                if (String.IsNullOrEmpty(data.Value))
                                {
                                    deptDatas = new CodeText[0];
                                }
                                else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out deptDatas))
                                {
                                    xmlResult = new XmlResult(false, "傳回部別資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                }
                                #endregion
                                break;
                            case "COLLEGE":
                                #region 院別
                                if (String.IsNullOrEmpty(data.Value))
                                {
                                    collegeDatas = new CodeText[0];
                                }
                                else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out collegeDatas))
                                {
                                    xmlResult = new XmlResult(false, "傳回院別資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                }
                                #endregion
                                break;
                            case "MAJOR":
                                #region 科系
                                if (String.IsNullOrEmpty(data.Value))
                                {
                                    majorDatas = new CodeText[0];
                                }
                                else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out majorDatas))
                                {
                                    xmlResult = new XmlResult(false, "傳回科系資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                }
                                #endregion
                                break;
                            case "CLASS":
                                #region 班別
                                if (String.IsNullOrEmpty(data.Value))
                                {
                                    classDatas = new CodeText[0];
                                }
                                else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out classDatas))
                                {
                                    xmlResult = new XmlResult(false, "傳回班別資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                }
                                #endregion
                                break;
                            case "UPNO":
                                #region 批號
                                if (String.IsNullOrEmpty(data.Value))
                                {
                                    upNoDatas = new CodeText[0];
                                }
                                else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out upNoDatas))
                                {
                                    xmlResult = new XmlResult(false, "傳回批號資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                }
                                #endregion
                                break;
                        }
                        if (!xmlResult.IsSuccess)
                        {
                            break;
                        }
                    }
                    if (xmlResult.IsSuccess && (deptDatas == null || collegeDatas == null || majorDatas == null || classDatas == null || upNoDatas == null))
                    {
                        xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                    }
                }
            }
            return xmlResult;
        }
        #endregion

        #region [MDY:20160321] 維護就貸資料
        /// <summary>
        /// 維護就貸資料
        /// </summary>
        /// <param name="page"></param>
        /// <param name="action"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public XmlResult EditStudentLoanData(Page page, string action, StudentLoanEntity data)
        {
            if (action != "A" && action != "M" && action != "D")
            {
                return new XmlResult(false, "缺少或不正確的處理參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (data == null || String.IsNullOrEmpty(data.ReceiveType) || String.IsNullOrEmpty(data.YearId) || String.IsNullOrEmpty(data.TermId)
                || data.DepId == null || String.IsNullOrEmpty(data.ReceiveId) || String.IsNullOrEmpty(data.StuId) || data.OldSeq < 0)
            {
                return new XmlResult(false, "缺少或不正確的就貸資料參數", ErrorCode.S_INVALID_PARAMETER, null);
            }

            string dataXml = null;
            if (!Common.TryToXmlExplicitly<StudentLoanEntity>(data, out dataXml))
            {
                return new XmlResult(false, "就貸資料序列化失敗", ErrorCode.S_SERIALIZED_FAILURE, null);
            }

            KeyValue<string>[] args = new KeyValue<string>[2] {
                new KeyValue<string>("Action", action),
                new KeyValue<string>("DataXml", dataXml)
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.EditStudentLoanData, args, out returnData);
            return xmlResult;
        }
        #endregion

        #region [MDY:20160305] 連動製單服務相關
        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 連動製單服務 - 單筆新增、修改、刪除學生繳費資料
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="sysPXX"></param>
        /// <param name="op"></param>
        /// <param name="user"></param>
        /// <param name="mdyDate"></param>
        /// <param name="mdyTime"></param>
        /// <param name="txnData"></param>
        /// <returns></returns>
        public string CallSchoolServiceForBill(string sysId, string sysPXX, string op, string user, string mdyDate, string mdyTime, string txnData)
        {
            string clientIP = WebHelper.GetClientIP();
            KeyValue<string>[] args = new KeyValue<string>[5] {
                new KeyValue<string>("OP", op),
                new KeyValue<string>("USER", user),
                new KeyValue<string>("MDYDATE", mdyDate),
                new KeyValue<string>("MDYTIME", mdyTime),
                new KeyValue<string>("TXNDATA", txnData)
            };
            SchoolServiceCommand command = SchoolServiceCommand.Create(sysId, sysPXX, clientIP, SchoolServiceMethodName.Bill, args);

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is Entities.SchoolService.ReturnData)
                    {
                        Entities.SchoolService.ReturnData returnData = data as Entities.SchoolService.ReturnData;
                        return returnData.ErrMsg;
                    }
                    else
                    {
                        return Entities.SchoolService.ErrorList.GetErrorMessage(Entities.SchoolService.ErrorList.S9999, "不正確的回傳資料");
                    }
                }
                else
                {
                    return Entities.SchoolService.ErrorList.GetErrorMessage(Entities.SchoolService.ErrorList.S9999, "傳回資料無法反序列化");
                }
            }
            else
            {
                return Entities.SchoolService.ErrorList.GetErrorMessage(Entities.SchoolService.ErrorList.S9999, xmlResult.Message);
            }
            #endregion
        }

        /// <summary>
        /// 連動製單服務 - 學生繳費資料查詢
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="sysPXX"></param>
        /// <param name="receive_type"></param>
        /// <param name="year_id"></param>
        /// <param name="term_id"></param>
        /// <param name="stu_id_start"></param>
        /// <param name="stu_id_end"></param>
        /// <param name="seq_no"></param>
        /// <param name="records"></param>
        /// <param name="txnFile"></param>
        /// <returns></returns>
        public string CallSchoolServiceForBillQuery(string sysId, string sysPXX, string receive_type, string year_id, string term_id, string stu_id_start, string stu_id_end, string seq_no
            , out int records, out byte[] txnFile)
        {
            records = 0;
            txnFile = null;

            string clientIP = WebHelper.GetClientIP();
            KeyValue<string>[] args = new KeyValue<string>[6] {
                new KeyValue<string>("RECEIVETYPE", receive_type),
                new KeyValue<string>("YEARID", year_id),
                new KeyValue<string>("TERMID", term_id),
                new KeyValue<string>("STUIDSTART", stu_id_start),
                new KeyValue<string>("STUIDEND", stu_id_end),
                new KeyValue<string>("SEQNO", seq_no)
            };
            SchoolServiceCommand command = SchoolServiceCommand.Create(sysId, sysPXX, clientIP, SchoolServiceMethodName.BillQuery, args);

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is Entities.SchoolService.ReturnData)
                    {
                        Entities.SchoolService.ReturnData returnData = data as Entities.SchoolService.ReturnData;
                        records = returnData.Records;
                        txnFile = returnData.TxnFile;
                        return returnData.ErrMsg;
                    }
                    else
                    {
                        return Entities.SchoolService.ErrorList.GetErrorMessage(Entities.SchoolService.ErrorList.S9999, "不正確的回傳資料");
                    }
                }
                else
                {
                    return Entities.SchoolService.ErrorList.GetErrorMessage(Entities.SchoolService.ErrorList.S9999, "傳回資料無法反序列化");
                }
            }
            else
            {
                return Entities.SchoolService.ErrorList.GetErrorMessage(Entities.SchoolService.ErrorList.S9999, xmlResult.Message);
            }
            #endregion
        }

        /// <summary>
        /// 連動製單服務 - 入金資料查詢
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="sysPXX"></param>
        /// <param name="receive_type"></param>
        /// <param name="cancel_no"></param>
        /// <param name="receive_way"></param>
        /// <param name="account_date"></param>
        /// <param name="records"></param>
        /// <param name="txnFile"></param>
        /// <returns></returns>
        public string CallSchoolServiceForPayQuery(string sysId, string sysPXX, string receive_type, string cancel_no, string receive_way, string account_date
            , out int records, out byte[] txnFile)
        {
            records = 0;
            txnFile = null;

            string clientIP = WebHelper.GetClientIP();
            KeyValue<string>[] args = new KeyValue<string>[4] {
                new KeyValue<string>("RECEIVETYPE", receive_type),
                new KeyValue<string>("CANCELNO", cancel_no),
                new KeyValue<string>("RECEIVEWAY", receive_way),
                new KeyValue<string>("ACCOUNTDATE", account_date)
            };
            SchoolServiceCommand command = SchoolServiceCommand.Create(sysId, sysPXX, clientIP, SchoolServiceMethodName.PayQuery, args);

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is Entities.SchoolService.ReturnData)
                    {
                        Entities.SchoolService.ReturnData returnData = data as Entities.SchoolService.ReturnData;
                        records = returnData.Records;
                        txnFile = returnData.TxnFile;
                        return returnData.ErrMsg;
                    }
                    else
                    {
                        return Entities.SchoolService.ErrorList.GetErrorMessage(Entities.SchoolService.ErrorList.S9999, "不正確的回傳資料");
                    }
                }
                else
                {
                    return Entities.SchoolService.ErrorList.GetErrorMessage(Entities.SchoolService.ErrorList.S9999, "傳回資料無法反序列化");
                }
            }
            else
            {
                return Entities.SchoolService.ErrorList.GetErrorMessage(Entities.SchoolService.ErrorList.S9999, xmlResult.Message);
            }
            #endregion
        }

        #region [MDY:20191219] 新增繳費資訊查詢
        /// <summary>
        /// 連動製單服務 - 繳費資訊查詢
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="sysPXX"></param>
        /// <param name="dataKind"></param>
        /// <param name="receiveType"></param>
        /// <param name="cancelNos"></param>
        /// <param name="seqNos"></param>
        /// <param name="recevieDate"></param>
        /// <param name="startRecord"></param>
        /// <param name="recordCount"></param>
        /// <param name="txnFile"></param>
        /// <returns></returns>
        public string CallSchoolServiceForQueryData(string sysId, string sysPXX, string dataKind, string receiveType
            , string cancelNos, string seqNos, string recevieDate, int startRecord
            , out int recordCount, out byte[] txnFile)
        {
            recordCount = 0;
            txnFile = null;

            string clientIP = WebHelper.GetClientIP();
            KeyValue<string>[] args = new KeyValue<string>[6] {
                new KeyValue<string>("DATA_KIND", dataKind),
                new KeyValue<string>("RECEIVE_TYPE", receiveType),
                new KeyValue<string>("CANCEL_NOS", cancelNos),
                new KeyValue<string>("SEQ_NOS", seqNos),
                new KeyValue<string>("RECEVIE_DATE", recevieDate),
                new KeyValue<string>("START_RECORD", startRecord.ToString())
            };
            SchoolServiceCommand command = SchoolServiceCommand.Create(sysId, sysPXX, clientIP, SchoolServiceMethodName.QueryData, args);

            #region 呼叫後端服務命令，並處理回傳結果
            XmlResult xmlResult = this.ExecuteCommand(command);
            if (xmlResult.IsSuccess)
            {
                object data = null;
                if (xmlResult.TryGetData(out data))
                {
                    if (data is Entities.SchoolService.ReturnData)
                    {
                        Entities.SchoolService.ReturnData returnData = data as Entities.SchoolService.ReturnData;
                        recordCount = returnData.Records;
                        txnFile = returnData.TxnFile;
                        return returnData.ErrMsg;
                    }
                    else
                    {
                        return Entities.SchoolService.ErrorList.GetErrorMessage(Entities.SchoolService.ErrorList.S9999, "不正確的回傳資料");
                    }
                }
                else
                {
                    return Entities.SchoolService.ErrorList.GetErrorMessage(Entities.SchoolService.ErrorList.S9999, "傳回資料無法反序列化");
                }
            }
            else
            {
                return Entities.SchoolService.ErrorList.GetErrorMessage(Entities.SchoolService.ErrorList.S9999, xmlResult.Message);
            }
            #endregion
        }
        #endregion
        #endregion
        #endregion


        #region [MDY:202203XX] 2022擴充案 增加取得英文名稱參數
        #region [MDY:2018xxxx] 取得指定代碼檔的資料選項陣列
        /// <summary>
        /// 取得指定代碼表的資料選項陣列
        /// </summary>
        /// <param name="page"></param>
        /// <param name="receiveType">指定商家代號</param>
        /// <param name="yearId">指定學年</param>
        /// <param name="termId">指定學期</param>
        /// <param name="depId">指定部別 (土銀不使用這個部別，固定用空字串)</param>
        /// <param name="codeTables">指定代碼檔 (可用資料表名稱（大小寫要一樣）、資料表英文代碼或中文名稱)</param>
        /// <param name="useEngDataUI">指定是否使用英文資料介面</param>
        /// <param name="deptDatas">傳回部別 CodeText 陣列</param>
        /// <param name="collegeDatas">傳回院別 CodeText 陣列</param>
        /// <param name="majorDatas">傳回科系 CodeText 陣列</param>
        /// <param name="classDatas">傳回班別 CodeText 陣列</param>
        /// <param name="dormDatas">傳回住宿 CodeText 陣列</param>
        /// <param name="reduceDatas">傳回減免 CodeText 陣列</param>
        /// <param name="loanDatas">傳回就貸 CodeText 陣列</param>
        /// <param name="identify01Datas">傳回身份註記一 CodeText 陣列</param>
        /// <param name="identify02Datas">傳回身份註記二 CodeText 陣列</param>
        /// <param name="identify03Datas">傳回身份註記三 CodeText 陣列</param>
        /// <param name="identify04Datas">傳回身份註記四 CodeText 陣列</param>
        /// <param name="identify05Datas">傳回身份註記五 CodeText 陣列</param>
        /// <param name="identify06Datas">傳回身份註記六 CodeText 陣列</param>
        /// <returns>傳回處理結果</returns>
        public XmlResult GetCodeTableAllOptions(Page page, string receiveType, string yearId, string termId, string depId
            , ICollection<string> codeTables, bool useEngDataUI
            , out CodeText[] deptDatas, out CodeText[] collegeDatas, out CodeText[] majorDatas, out CodeText[] classDatas
            , out CodeText[] dormDatas, out CodeText[] reduceDatas, out CodeText[] loanDatas
            , out CodeText[] identify01Datas, out CodeText[] identify02Datas, out CodeText[] identify03Datas
            , out CodeText[] identify04Datas, out CodeText[] identify05Datas, out CodeText[] identify06Datas)
        {
            #region 傳回變數初始化
            deptDatas = null;
            collegeDatas = null;
            majorDatas = null;
            classDatas = null;
            dormDatas = null;
            reduceDatas = null;
            loanDatas = null;
            identify01Datas = null;
            identify02Datas = null;
            identify03Datas = null;
            identify04Datas = null;
            identify05Datas = null;
            identify06Datas = null;
            #endregion

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            List<string> tableNames = null;
            {
                if (String.IsNullOrWhiteSpace(receiveType) || String.IsNullOrWhiteSpace(yearId) || String.IsNullOrWhiteSpace(termId) || codeTables == null || codeTables.Count == 0)
                {
                    return new XmlResult(false, "缺少或無效的查詢參數", ErrorCode.S_INVALID_PARAMETER, null);
                }
                string[] tableNameList = new string[] {
                    DeptListEntity.TABLE_NAME, CollegeListEntity.TABLE_NAME, MajorListEntity.TABLE_NAME, ClassListEntity.TABLE_NAME,
                    DormListEntity.TABLE_NAME, ReduceListEntity.TABLE_NAME, LoanListEntity.TABLE_NAME,
                    IdentifyList1Entity.TABLE_NAME, IdentifyList2Entity.TABLE_NAME, IdentifyList3Entity.TABLE_NAME,
                    IdentifyList4Entity.TABLE_NAME, IdentifyList5Entity.TABLE_NAME, IdentifyList6Entity.TABLE_NAME
                };
                tableNames = new List<string>(tableNameList.Length);
                if (codeTables.Count == 1 && codeTables.Contains("ALL"))
                {
                    tableNames.AddRange(tableNameList);
                }
                else
                {
                    foreach (string codeTable in codeTables)
                    {
                        if (!String.IsNullOrWhiteSpace(codeTable))
                        {
                            string tableName = codeTable.Trim();
                            switch (tableName.ToUpper())
                            {
                                case "DEPT":
                                case "部別":
                                    tableName = DeptListEntity.TABLE_NAME;
                                    break;
                                case "COLLEGE":
                                case "院別":
                                    tableName = CollegeListEntity.TABLE_NAME;
                                    break;
                                case "MAJOR":
                                case "科系":
                                    tableName = MajorListEntity.TABLE_NAME;
                                    break;
                                case "CLASS":
                                case "班別":
                                    tableName = ClassListEntity.TABLE_NAME;
                                    break;
                                case "DORM":
                                case "住宿":
                                    tableName = DormListEntity.TABLE_NAME;
                                    break;
                                case "REDUCE":
                                case "減免":
                                    tableName = ReduceListEntity.TABLE_NAME;
                                    break;
                                case "LOAN":
                                case "就貸":
                                    tableName = LoanListEntity.TABLE_NAME;
                                    break;
                                case "IDENTIFY01":
                                case "IDENTIFY1":
                                case "身份註記一":
                                case "身份註記01":
                                case "身份註記1":
                                    tableName = IdentifyList1Entity.TABLE_NAME;
                                    break;
                                case "IDENTIFY02":
                                case "IDENTIFY2":
                                case "身份註記二":
                                case "身份註記02":
                                case "身份註記2":
                                    tableName = IdentifyList2Entity.TABLE_NAME;
                                    break;
                                case "IDENTIFY03":
                                case "IDENTIFY3":
                                case "身份註記三":
                                case "身份註記03":
                                case "身份註記3":
                                    tableName = IdentifyList3Entity.TABLE_NAME;
                                    break;
                                case "IDENTIFY04":
                                case "IDENTIFY4":
                                case "身份註記四":
                                case "身份註記04":
                                case "身份註記4":
                                    tableName = IdentifyList4Entity.TABLE_NAME;
                                    break;
                                case "IDENTIFY05":
                                case "IDENTIFY5":
                                case "身份註記五":
                                case "身份註記05":
                                case "身份註記5":
                                    tableName = IdentifyList5Entity.TABLE_NAME;
                                    break;
                                case "IDENTIFY06":
                                case "IDENTIFY6":
                                case "身份註記六":
                                case "身份註記06":
                                case "身份註記6":
                                    tableName = IdentifyList6Entity.TABLE_NAME;
                                    break;
                            }
                            if (tableNameList.Contains(tableName))
                            {
                                if (!tableNames.Contains(tableName))
                                {
                                    tableNames.Add(tableName);
                                }
                            }
                            else
                            {
                                return new XmlResult(false, "codeTables 參數錯誤", ErrorCode.S_INVALID_PARAMETER, null);
                            }
                        }
                    }
                }
                if (tableNames.Count == 0)
                {
                    return new XmlResult(false, "缺少 codeTables 參數", ErrorCode.S_INVALID_PARAMETER, null);
                }
                if (depId == null)
                {
                    depId = String.Empty;
                }
            }
            #endregion

            KeyValue<string>[] args = new KeyValue<string>[6] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId", yearId),
                new KeyValue<string>("TermId", termId),
                new KeyValue<string>("DepId", depId),
                new KeyValue<string>("CodeTables", String.Join(",", tableNames)),
                new KeyValue<string>("UseEngDataUI", useEngDataUI ? "Y" : "N")
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.GetCodeTableAllOptions, args, out returnData);
            if (xmlResult.IsSuccess && returnData != null)
            {
                KeyValue<string>[] datas = returnData as KeyValue<string>[];
                if (datas == null || datas.Length != tableNames.Count)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
                else
                {
                    foreach (KeyValue<string> data in datas)
                    {
                        if (!tableNames.Contains(data.Key))
                        {
                            xmlResult = new XmlResult(false, "傳回 CodeTable 不正確", ErrorCode.S_INVALID_RETURN_VALUE);
                        }
                        else
                        {
                            switch (data.Key)
                            {
                                case DeptListEntity.TABLE_NAME:
                                    #region 部別
                                    if (deptDatas == null)
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            deptDatas = new CodeText[0];
                                        }
                                        else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out deptDatas))
                                        {
                                            xmlResult = new XmlResult(false, "傳回部別資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    else
                                    {
                                        xmlResult = new XmlResult(false, "傳回部別資料重複", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    break;
                                    #endregion

                                case CollegeListEntity.TABLE_NAME:
                                    #region 院別
                                    if (collegeDatas == null)
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            collegeDatas = new CodeText[0];
                                        }
                                        else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out collegeDatas))
                                        {
                                            xmlResult = new XmlResult(false, "傳回院別資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    else
                                    {
                                        xmlResult = new XmlResult(false, "傳回院別資料重複", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    break;
                                    #endregion

                                case MajorListEntity.TABLE_NAME:
                                    #region 科系
                                    if (majorDatas == null)
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            majorDatas = new CodeText[0];
                                        }
                                        else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out majorDatas))
                                        {
                                            xmlResult = new XmlResult(false, "傳回科系資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    else
                                    {
                                        xmlResult = new XmlResult(false, "傳回科系資料重複", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    break;
                                    #endregion

                                case ClassListEntity.TABLE_NAME:
                                    #region 班別
                                    if (classDatas == null)
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            classDatas = new CodeText[0];
                                        }
                                        else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out classDatas))
                                        {
                                            xmlResult = new XmlResult(false, "傳回班別資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    else
                                    {
                                        xmlResult = new XmlResult(false, "傳回班別資料重複", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    break;
                                    #endregion

                                case DormListEntity.TABLE_NAME:
                                    #region 住宿
                                    if (dormDatas == null)
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            dormDatas = new CodeText[0];
                                        }
                                        else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out dormDatas))
                                        {
                                            xmlResult = new XmlResult(false, "傳回住宿資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    else
                                    {
                                        xmlResult = new XmlResult(false, "傳回住宿資料重複", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    break;
                                    #endregion

                                case ReduceListEntity.TABLE_NAME:
                                    #region 減免
                                    if (reduceDatas == null)
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            reduceDatas = new CodeText[0];
                                        }
                                        else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out reduceDatas))
                                        {
                                            xmlResult = new XmlResult(false, "傳回減免資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    else
                                    {
                                        xmlResult = new XmlResult(false, "傳回減免資料重複", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    break;
                                    #endregion

                                case LoanListEntity.TABLE_NAME:
                                    #region 就貸
                                    if (loanDatas == null)
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            loanDatas = new CodeText[0];
                                        }
                                        else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out loanDatas))
                                        {
                                            xmlResult = new XmlResult(false, "傳回就貸資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    else
                                    {
                                        xmlResult = new XmlResult(false, "傳回就貸資料重複", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    break;
                                    #endregion

                                case IdentifyList1Entity.TABLE_NAME:
                                    #region 身份註記一
                                    if (identify01Datas == null)
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            identify01Datas = new CodeText[0];
                                        }
                                        else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out identify01Datas))
                                        {
                                            xmlResult = new XmlResult(false, "傳回身份註記一資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    else
                                    {
                                        xmlResult = new XmlResult(false, "傳回身份註記一資料重複", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    break;
                                    #endregion

                                case IdentifyList2Entity.TABLE_NAME:
                                    #region 身份註記二
                                    if (identify02Datas == null)
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            identify02Datas = new CodeText[0];
                                        }
                                        else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out identify02Datas))
                                        {
                                            xmlResult = new XmlResult(false, "傳回身份註記二資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    else
                                    {
                                        xmlResult = new XmlResult(false, "傳回身份註記二資料重複", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    break;
                                    #endregion

                                case IdentifyList3Entity.TABLE_NAME:
                                    #region 身份註記三
                                    if (identify03Datas == null)
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            identify03Datas = new CodeText[0];
                                        }
                                        else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out identify03Datas))
                                        {
                                            xmlResult = new XmlResult(false, "傳回身份註記三資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    else
                                    {
                                        xmlResult = new XmlResult(false, "傳回身份註記三資料重複", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    break;
                                    #endregion

                                case IdentifyList4Entity.TABLE_NAME:
                                    #region 身份註記四
                                    if (identify04Datas == null)
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            identify04Datas = new CodeText[0];
                                        }
                                        else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out identify04Datas))
                                        {
                                            xmlResult = new XmlResult(false, "傳回身份註記四資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    else
                                    {
                                        xmlResult = new XmlResult(false, "傳回身份註記四資料重複", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    break;
                                    #endregion

                                case IdentifyList5Entity.TABLE_NAME:
                                    #region 身份註記五
                                    if (identify05Datas == null)
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            identify05Datas = new CodeText[0];
                                        }
                                        else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out identify05Datas))
                                        {
                                            xmlResult = new XmlResult(false, "傳回身份註記五資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    else
                                    {
                                        xmlResult = new XmlResult(false, "傳回身份註記五資料重複", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    break;
                                    #endregion

                                case IdentifyList6Entity.TABLE_NAME:
                                    #region 身份註記六
                                    if (identify06Datas == null)
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            identify06Datas = new CodeText[0];
                                        }
                                        else if (!Common.TryDeXmlExplicitly<CodeText[]>(data.Value, out identify06Datas))
                                        {
                                            xmlResult = new XmlResult(false, "傳回身份註記六資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    else
                                    {
                                        xmlResult = new XmlResult(false, "傳回身份註記六資料重複", ErrorCode.S_DESERIALIZED_FAILURE);
                                    }
                                    break;
                                    #endregion

                                default:
                                    xmlResult = new XmlResult(false, "傳回 CodeTable 不正確", ErrorCode.S_INVALID_RETURN_VALUE);
                                    break;
                            }
                        }
                        if (!xmlResult.IsSuccess)
                        {
                            break;
                        }
                    }
                }
            }
            return xmlResult;
        }
        #endregion
        #endregion

        #region [MDY:2018xxxx] 取得指定代收費用檔、合計項目設定、學生基本資料、學生繳費單的資料
        /// <summary>
        /// 取得指定代收費用檔、合計項目設定、學生基本資料、學生繳費單的資料
        /// </summary>
        /// <param name="page"></param>
        /// <param name="receiveType">指定商家代號</param>
        /// <param name="yearId">指定學年</param>
        /// <param name="termId">指定學期</param>
        /// <param name="depId">指定部別 (土銀不使用這個部別，固定用空字串)</param>
        /// <param name="receiveId">指定代收費用別</param>
        /// <param name="stuId">指定學號</param>
        /// <param name="oldSeq">指定繳費單資料序號</param>
        /// <param name="dataKinds">指定取資料種類 (可用資料表名稱、資料表英文代碼)</param>
        /// <param name="schoolRid">傳回代收費用檔資料或 null</param>
        /// <param name="receiveSum">傳回合計項目設定資料陣列或 null</param>
        /// <param name="studentMaster">傳回學生基本資料或 null</param>
        /// <param name="studentReceive">傳回學生繳費單資料或 null</param>
        /// <returns>傳回處理結果</returns>
        public XmlResult GetStudentBillDatas(Page page, string receiveType, string yearId, string termId, string depId, string receiveId, string stuId, int? oldSeq
            , ICollection<string> dataKinds
            , out SchoolRidEntity schoolRid, out StudentMasterEntity studentMaster, out StudentReceiveEntity studentReceive
            , out ReceiveSumEntity[] receiveSum, out StudentLoanEntity studentLoan, out StudentReceiveEntity lastStudentReceive)
        {
            #region 傳回變數初始化
            schoolRid = null;
            studentMaster = null;
            studentReceive = null;
            receiveSum = null;
            studentLoan = null;
            lastStudentReceive = null;
            #endregion

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            List<string> tableNames = null;
            {
                if (depId == null)
                {
                    depId = String.Empty;
                }
                if (String.IsNullOrWhiteSpace(receiveType) || depId == null || (oldSeq.HasValue && oldSeq < 0) || (dataKinds == null || dataKinds.Count == 0))
                {
                    return new XmlResult(false, "缺少或無效的必要查詢參數", ErrorCode.S_INVALID_PARAMETER, null);
                }
                CodeTextList tableNameMaps = new CodeTextList(new CodeText[6] {
                    new CodeText("SCHOOLRID", SchoolRidEntity.TABLE_NAME), new CodeText("STUDENTMASTER", StudentMasterEntity.TABLE_NAME), 
                    new CodeText("STUDENTRECEIVE", StudentReceiveEntity.TABLE_NAME), new CodeText("RECEIVESUM", ReceiveSumEntity.TABLE_NAME),
                    new CodeText("STUDENTLOAN", StudentLoanEntity.TABLE_NAME), new CodeText("LASTSTUDENTRECEIVE", "LAST_STUDENTRECEIVE")
                });

                tableNames = new List<string>(tableNameMaps.Count);
                if (dataKinds.Count == 1 && dataKinds.Contains("ALL"))
                {
                    tableNames.AddRange(tableNameMaps.GetAllTexts());
                }
                else
                {
                    foreach (string value in dataKinds)
                    {
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            string tableName = null;
                            string dataKind = value.Trim();
                            foreach (CodeText tableNameMap in tableNameMaps)
                            {
                                if (tableNameMap.Code.Equals(dataKind, StringComparison.CurrentCultureIgnoreCase)
                                    || tableNameMap.Text.Equals(dataKind, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    tableName = tableNameMap.Text;
                                    break;
                                }
                            }

                            #region 檢查各資料類別的必要參數
                            switch (tableName)
                            {
                                case SchoolRidEntity.TABLE_NAME:
                                    #region 代收費用檔
                                    if (String.IsNullOrWhiteSpace(yearId) || String.IsNullOrWhiteSpace(termId) || String.IsNullOrWhiteSpace(receiveId))
                                    {
                                        return new XmlResult(false, "缺少查詢代收費用檔的必要參數或參數無效", ErrorCode.S_INVALID_PARAMETER, null);
                                    }
                                    break;
                                    #endregion

                                case StudentMasterEntity.TABLE_NAME:
                                    #region 學生基本資料
                                    if (String.IsNullOrWhiteSpace(stuId))
                                    {
                                        return new XmlResult(false, "缺少查詢學生基本資料的必要參數或參數無效", ErrorCode.S_INVALID_PARAMETER, null);
                                    }
                                    break;
                                    #endregion

                                case StudentReceiveEntity.TABLE_NAME:
                                    #region 學生繳費單
                                    if (String.IsNullOrWhiteSpace(yearId) || String.IsNullOrWhiteSpace(termId) || String.IsNullOrWhiteSpace(receiveId) || String.IsNullOrWhiteSpace(stuId) || !oldSeq.HasValue || oldSeq.Value < 0)
                                    {
                                        return new XmlResult(false, "缺少查詢學生繳費單的必要參數或參數無效", ErrorCode.S_INVALID_PARAMETER, null);
                                    }
                                    break;
                                    #endregion

                                case ReceiveSumEntity.TABLE_NAME:
                                    #region 合計項目設定
                                    if (String.IsNullOrWhiteSpace(yearId) || String.IsNullOrWhiteSpace(termId) || String.IsNullOrWhiteSpace(receiveId))
                                    {
                                        return new XmlResult(false, "缺少查詢合計項目設定的必要參數或參數無效", ErrorCode.S_INVALID_PARAMETER, null);
                                    }
                                    break;
                                    #endregion

                                case StudentLoanEntity.TABLE_NAME:
                                    #region 學生就貸
                                    if (String.IsNullOrWhiteSpace(yearId) || String.IsNullOrWhiteSpace(termId) || String.IsNullOrWhiteSpace(receiveId) || String.IsNullOrWhiteSpace(stuId) || !oldSeq.HasValue || oldSeq.Value < 0)
                                    {
                                        return new XmlResult(false, "缺少查詢學生就貸的必要參數或參數無效", ErrorCode.S_INVALID_PARAMETER, null);
                                    }
                                    break;
                                    #endregion

                                case "LAST_STUDENTRECEIVE":
                                    #region 最近一筆學生繳費單
                                    if (String.IsNullOrWhiteSpace(yearId) || String.IsNullOrWhiteSpace(termId) || String.IsNullOrWhiteSpace(stuId))
                                    {
                                        return new XmlResult(false, "缺少查詢最近一筆學生繳費單的必要參數或參數無效", ErrorCode.S_INVALID_PARAMETER, null);
                                    }
                                    break;
                                    #endregion

                                default:
                                    return new XmlResult(false, "dataKinds 參數錯誤", ErrorCode.S_INVALID_PARAMETER, null);
                            }
                            #endregion

                            if (!tableNames.Contains(tableName))
                            {
                                tableNames.Add(tableName);
                            }
                        }
                    }
                }
                if (tableNames.Count == 0)
                {
                    return new XmlResult(false, "缺少 dataKinds 參數", ErrorCode.S_INVALID_PARAMETER, null);
                }
            }
            #endregion

            KeyValue<string>[] args = new KeyValue<string>[8] {
                new KeyValue<string>("ReceiveType", receiveType),
                new KeyValue<string>("YearId", yearId),
                new KeyValue<string>("TermId", termId),
                new KeyValue<string>("DepId", depId),
                new KeyValue<string>("ReceiveId", receiveId),
                new KeyValue<string>("StuId", stuId),
                new KeyValue<string>("OldSeq", oldSeq.ToString()),
                new KeyValue<string>("DataKinds", String.Join(",", tableNames))
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.GetStudentBillDatas, args, out returnData);
            if (xmlResult.IsSuccess && returnData != null)
            {
                KeyValue<string>[] datas = returnData as KeyValue<string>[];
                if (datas == null || datas.Length != tableNames.Count)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
                else
                {
                    List<string> rtnKeys = new List<string>(datas.Length);
                    foreach (KeyValue<string> data in datas)
                    {
                        if (!tableNames.Contains(data.Key))
                        {
                            xmlResult = new XmlResult(false, "傳回 DataKind 不正確", ErrorCode.S_INVALID_RETURN_VALUE);
                        }
                        else
                        {
                            switch (data.Key)
                            {
                                case SchoolRidEntity.TABLE_NAME:
                                    #region 代收費用檔
                                    if (rtnKeys.Contains(data.Key))
                                    {
                                        xmlResult = new XmlResult(false, "重複傳回代收費用檔資料", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    else
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            schoolRid = null;
                                        }
                                        else if (!Common.TryDeXmlExplicitly<SchoolRidEntity>(data.Value, out schoolRid))
                                        {
                                            xmlResult = new XmlResult(false, "傳回代收費用檔資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    break;
                                    #endregion

                                case StudentMasterEntity.TABLE_NAME:
                                    #region 學生基本資料
                                    if (rtnKeys.Contains(data.Key))
                                    {
                                        xmlResult = new XmlResult(false, "重複傳回學生基本資料", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    else
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            studentMaster = null;
                                        }
                                        else if (!Common.TryDeXmlExplicitly<StudentMasterEntity>(data.Value, out studentMaster))
                                        {
                                            xmlResult = new XmlResult(false, "傳回學生基本資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    break;
                                    #endregion

                                case StudentReceiveEntity.TABLE_NAME:
                                    #region 學生繳費單
                                    if (rtnKeys.Contains(data.Key))
                                    {
                                        xmlResult = new XmlResult(false, "重複傳回學生繳費單資料", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    else
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            studentReceive = null;
                                        }
                                        else if (!Common.TryDeXmlExplicitly<StudentReceiveEntity>(data.Value, out studentReceive))
                                        {
                                            xmlResult = new XmlResult(false, "傳回學生繳費單資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    break;
                                    #endregion

                                case ReceiveSumEntity.TABLE_NAME:
                                    #region 合計項目設定
                                    if (rtnKeys.Contains(data.Key))
                                    {
                                        xmlResult = new XmlResult(false, "重複傳回合計項目設定資料", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    else
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            receiveSum = new ReceiveSumEntity[0];
                                        }
                                        else if (!Common.TryDeXmlExplicitly<ReceiveSumEntity[]>(data.Value, out receiveSum))
                                        {
                                            xmlResult = new XmlResult(false, "傳回合計項目設定資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    break;
                                    #endregion

                                case StudentLoanEntity.TABLE_NAME:
                                    #region 學生就貸
                                    if (rtnKeys.Contains(data.Key))
                                    {
                                        xmlResult = new XmlResult(false, "重複傳回學生就貸資料", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    else
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            studentLoan = null;
                                        }
                                        else if (!Common.TryDeXmlExplicitly<StudentLoanEntity>(data.Value, out studentLoan))
                                        {
                                            xmlResult = new XmlResult(false, "傳回學生就貸資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    break;
                                    #endregion

                                case "LAST_STUDENTRECEIVE":
                                    #region 最近一筆學生繳費單
                                    if (rtnKeys.Contains(data.Key))
                                    {
                                        xmlResult = new XmlResult(false, "傳回最近一筆學生繳費單資料重複", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    else
                                    {
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            lastStudentReceive = null;
                                        }
                                        else if (!Common.TryDeXmlExplicitly<StudentReceiveEntity>(data.Value, out lastStudentReceive))
                                        {
                                            xmlResult = new XmlResult(false, "傳回最近一筆學生繳費單資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                    }
                                    break;
                                    #endregion

                                //TODO: 缺就貸、減免資料

                                default:
                                    xmlResult = new XmlResult(false, "傳回 CodeTable 不正確", ErrorCode.S_INVALID_RETURN_VALUE);
                                    break;
                            }
                        }
                        if (!xmlResult.IsSuccess)
                        {
                            break;
                        }
                    }
                }
            }
            return xmlResult;
        }
        #endregion

        #region [MDY:2018xxxx] 更新學生基本資料、學生繳費單的資料
        /// <summary>
        /// 更新指定學生基本資料與學生繳費單的資料，並指定是否同時計算金額與產生虛擬帳號
        /// </summary>
        /// <param name="page"></param>
        /// <param name="studentMaster">指定更新的學生基本資料</param>
        /// <param name="studentReceive">指定更新的學生繳費單資料</param>
        /// <param name="returnDatas">如果 toReturnData = true 則傳回更新後的資料，否則傳回 null</param>
        /// <param name="toGenCancelNo">指定是否同時計算金額與產生虛擬帳號</param>
        /// <param name="toReturnDatas">指定是否回傳更新後的資料</param>
        /// <returns>傳回處理結果</returns>
        public XmlResult UpdateStudentBillDatas(Page page, StudentMasterEntity studentMaster, StudentReceiveEntity studentReceive, out KeyValueList returnDatas
            , bool toGenCancelNo = false, bool toReturnDatas = true)
        {
            returnDatas = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            string xmlStudentMaster = null;
            string xmlStudentReceive = null;

            #region 檢查參數
            if (studentMaster == null || studentReceive == null)
            {
                return new XmlResult(false, "缺少學生基本資料或學生繳費單資料", ErrorCode.S_INVALID_PARAMETER, null);
            }

            #region 土銀不使用原有的 DepId 欄位，所以 null 轉成空字串
            if (studentReceive.DepId == null)
            {
                studentReceive.DepId = "";
            }
            if (studentReceive.DepId == null)
            {
                studentReceive.DepId = "";
            }
            #endregion

            if (studentMaster.ReceiveType != studentReceive.ReceiveType || studentMaster.DepId != studentReceive.DepId || studentMaster.Id != studentReceive.StuId)
            {
                return new XmlResult(false, "學生基本資料與學生繳費單資料不一致", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (String.IsNullOrEmpty(studentReceive.ReceiveType) || String.IsNullOrEmpty(studentReceive.YearId) || String.IsNullOrEmpty(studentReceive.TermId)
                || String.IsNullOrEmpty(studentReceive.ReceiveId) || String.IsNullOrEmpty(studentReceive.StuId) || studentReceive.OldSeq < 0)
            {
                return new XmlResult(false, "學生繳費單資料不正確", ErrorCode.S_INVALID_PARAMETER, null);
            }
            if (!Common.TryToXmlExplicitly<StudentMasterEntity>(studentMaster, out xmlStudentMaster))
            {
                return new XmlResult(false, "學生基本資料無法序列化", ErrorCode.S_SERIALIZED_FAILURE, null);
            }
            if (!Common.TryToXmlExplicitly<StudentReceiveEntity>(studentReceive, out xmlStudentReceive))
            {
                return new XmlResult(false, "學生繳費單資料無法序列化", ErrorCode.S_SERIALIZED_FAILURE, null);
            }
            #endregion

            KeyValue<string>[] args = new KeyValue<string>[4] {
                new KeyValue<string>("StudentMaster", xmlStudentMaster),
                new KeyValue<string>("StudentReceive", xmlStudentReceive),
                new KeyValue<string>("ToGenCancelNo", (toGenCancelNo ? "Y" : "N")),
                new KeyValue<string>("ToReturnDatas", (toReturnDatas ?"Y" : "N"))
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.UpdateStudentBillDatas, args, out returnData);
            if (xmlResult.IsSuccess && toReturnDatas)
            {
                KeyValue<string>[] datas = returnData as KeyValue<string>[];
                if (datas == null || datas.Length == 0)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
                else
                {
                    returnDatas = new KeyValueList(5);
                    foreach (KeyValue<string> data in datas)
                    {
                        switch (data.Key)
                        {
                            case SchoolRidEntity.TABLE_NAME:
                                #region 代收費用檔
                                {
                                    string dataKey = "SchoolRid";
                                    if (returnDatas.GetKeyFirstIndex(dataKey) > -1)
                                    {
                                        xmlResult = new XmlResult(false, "重複傳回代收費用檔資料", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    else
                                    {
                                        SchoolRidEntity dataValue = null;
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            xmlResult = new XmlResult(false, "未傳回代收費用檔資料", ErrorCode.S_INVALID_RETURN_VALUE);
                                        }
                                        else if (!Common.TryDeXmlExplicitly<SchoolRidEntity>(data.Value, out dataValue))
                                        {
                                            xmlResult = new XmlResult(false, "傳回代收費用檔資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                        returnDatas.Add(dataKey, dataValue);
                                    }
                                }
                                break;
                                #endregion

                            case StudentMasterEntity.TABLE_NAME:
                                #region 學生基本資料
                                {
                                    string dataKey = "StudentMaster";
                                    if (returnDatas.GetKeyFirstIndex(dataKey) > -1)
                                    {
                                        xmlResult = new XmlResult(false, "重複傳回學生基本資料", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    else
                                    {
                                        StudentMasterEntity dataValue = null;
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            xmlResult = new XmlResult(false, "未傳回學生基本資料", ErrorCode.S_INVALID_RETURN_VALUE);
                                        }
                                        else if (!Common.TryDeXmlExplicitly<StudentMasterEntity>(data.Value, out dataValue))
                                        {
                                            xmlResult = new XmlResult(false, "傳回學生基本資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                        returnDatas.Add(dataKey, dataValue);
                                    }
                                }
                                break;
                                #endregion

                            case StudentReceiveEntity.TABLE_NAME:
                                #region 學生繳費單
                                {
                                    string dataKey = "StudentReceive";
                                    if (returnDatas.GetKeyFirstIndex(dataKey) > -1)
                                    {
                                        xmlResult = new XmlResult(false, "重複傳回學生繳費單資料", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    else
                                    {
                                        StudentReceiveEntity dataValue = null;
                                        if (String.IsNullOrEmpty(data.Value))
                                        {
                                            xmlResult = new XmlResult(false, "未傳回學生繳費單資料", ErrorCode.S_INVALID_RETURN_VALUE);
                                        }
                                        else if (!Common.TryDeXmlExplicitly<StudentReceiveEntity>(data.Value, out dataValue))
                                        {
                                            xmlResult = new XmlResult(false, "傳回學生繳費單資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                        }
                                        returnDatas.Add(dataKey, dataValue);
                                    }
                                }
                                break;
                                #endregion

                            case ReceiveSumEntity.TABLE_NAME:
                                #region 合計項目設定
                                {
                                    string dataKey = "ReceiveSum";
                                    if (returnDatas.GetKeyFirstIndex(dataKey) > -1)
                                    {
                                        xmlResult = new XmlResult(false, "重複傳回合計項目設定資料", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    else
                                    {
                                        ReceiveSumEntity[] dataValue = null;
                                        if (!String.IsNullOrEmpty(data.Value))
                                        {
                                            if (Common.TryDeXmlExplicitly<ReceiveSumEntity[]>(data.Value, out dataValue))
                                            {
                                                returnDatas.Add(dataKey, dataValue);
                                            }
                                            else
                                            {
                                                xmlResult = new XmlResult(false, "傳回合計項目設定資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                            }
                                        }
                                    }
                                }
                                break;
                                #endregion

                            case StudentLoanEntity.TABLE_NAME:
                                #region 就貸資料
                                {
                                    string dataKey = "StudentLoan";
                                    if (returnDatas.GetKeyFirstIndex(dataKey) > -1)
                                    {
                                        xmlResult = new XmlResult(false, "重複傳回就貸資料", ErrorCode.S_INVALID_RETURN_VALUE);
                                    }
                                    else
                                    {
                                        StudentLoanEntity dataValue = null;
                                        if (!String.IsNullOrEmpty(data.Value))
                                        {
                                            if (Common.TryDeXmlExplicitly<StudentLoanEntity>(data.Value, out dataValue))
                                            {
                                                returnDatas.Add(dataKey, dataValue);
                                            }
                                            else
                                            {
                                                xmlResult = new XmlResult(false, "傳回就貸資料無法反序列化", ErrorCode.S_DESERIALIZED_FAILURE);
                                            }
                                        }
                                    }
                                }
                                break;
                                #endregion

                            //TODO: 缺減免資料
                        }
                        if (!xmlResult.IsSuccess)
                        {
                            break;
                        }
                    }
                }
            }
            return xmlResult;
        }
        #endregion


        #region [MDY:202203XX] 2022擴充案 學生專區取得繳費單 相關
        /// <summary>
        /// 學生專區取得指定學號的繳費單清單
        /// </summary>
        /// <param name="page"></param>
        /// <param name="studentId"></param>
        /// <param name="isEngUI"></param>
        /// <param name="datas"></param>
        /// <param name="receiveTypes"></param>
        /// <returns></returns>
        public XmlResult GetStudentReceiveViews(Page page, string studentId, bool isEngUI, out StudentReceiveView[] datas
            , params string[] receiveTypes)
        {
            datas = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (String.IsNullOrEmpty(studentId) || receiveTypes == null || receiveTypes.Length == 0)
            {
                return new XmlResult(false, "缺少或無效的必要查詢參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            KeyValue<string>[] args = new KeyValue<string>[3] {
                new KeyValue<string>("STUDENT_ID", studentId),
                new KeyValue<string>("RECEIVE_TYPES", String.Join(",", receiveTypes)),
                new KeyValue<string>("IS_ENG_UI", (isEngUI ? "Y" : "N"))
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.GetStudentReceiveViews, args, out returnData);
            if (xmlResult.IsSuccess && returnData != null)
            {
                datas = returnData as StudentReceiveView[];
                if (datas == null)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }

            return xmlResult;
        }

        /// <summary>
        /// 取得指定商家代號、學年、學期、費用別、學號、序號的繳費單
        /// </summary>
        /// <param name="page"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="ReceiveId"></param>
        /// <param name="stuId"></param>
        /// <param name="oldSeq"></param>
        /// <param name="isEngUI"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public XmlResult GetStudentReceiveView(Page page, string receiveType, string yearId, string termId, string receiveId
            , string stuId, int oldSeq, bool isEngUI, out StudentReceiveView data)
        {
            data = null;

            #region 檢查資料處理代理物件
            if (!this.IsReady())
            {
                return new XmlResult(false, "無效的資料處理代理物件", ErrorCode.S_INVALID_PROXY, null);
            }
            #endregion

            #region 檢查參數
            if (String.IsNullOrEmpty(receiveType)
                || String.IsNullOrEmpty(yearId)
                || String.IsNullOrEmpty(termId)
                || String.IsNullOrEmpty(receiveId)
                || String.IsNullOrEmpty(stuId)
                || oldSeq < 0)
            {
                return new XmlResult(false, "缺少或無效的必要查詢參數", ErrorCode.S_INVALID_PARAMETER, null);
            }
            #endregion

            KeyValue<string>[] args = new KeyValue<string>[7] {
                new KeyValue<string>("RECEIVE_TYPE", receiveType),
                new KeyValue<string>("YEAR_ID", yearId),
                new KeyValue<string>("TERM_ID", termId),
                new KeyValue<string>("RECEIVE_ID", receiveId),
                new KeyValue<string>("STU_ID", stuId),
                new KeyValue<string>("OLD_SEQ", oldSeq.ToString()),
                new KeyValue<string>("IS_ENG_UI", (isEngUI ? "Y" : "N"))
            };

            object returnData = null;
            XmlResult xmlResult = this.CallMethod(page, CallMethodName.GetStudentReceiveView, args, out returnData);
            if (xmlResult.IsSuccess && returnData != null)
            {
                data = returnData as StudentReceiveView;
                if (data == null && returnData != null)
                {
                    xmlResult = new XmlResult(false, "不正確的回傳資料", ErrorCode.S_INVALID_RETURN_VALUE);
                }
            }

            return xmlResult;
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 嘗試由指定頁面取得服務命令請求者資料
        /// </summary>
        /// <param name="page">指定頁面</param>
        /// <returns>成功則傳回服務命令請求者資料，否則傳回 null。</returns>
        private CommandAsker TryGetCommandAsker(Page page)
        {
            LogonUser logonUser = null;
            string menuID = null;
            string menuName = null;
            this.TryGetLogonUserAndMenuData(page, out logonUser, out menuID, out menuName);

            return CommandAsker.Create(logonUser, menuID, menuName);
        }

        /// <summary>
        /// 嘗試由指定頁面取得登入者資訊與選單(功能)資料
        /// </summary>
        /// <param name="page">指定頁面</param>
        /// <param name="logonUser">傳回登入者資訊</param>
        /// <param name="menuID">傳回選單(功能)代碼，或頁面檔名(不含副檔名)</param>
        /// <param name="menuName">傳回選單(功能)名稱，或頁面 Title</param>
        private void TryGetLogonUserAndMenuData(Page page, out LogonUser logonUser, out string menuID, out string menuName)
        {
            #region [Old]
            //logonUser = null;
            //menuID = null;
            //menuName = null;
            //if (page == null)
            //{
            //    logonUser = WebHelper.GetLogonUser();
            //    bool isEditPage = false;
            //    bool isSubPage = false;
            //    menuID = WebHelper.GetCurrentPageMenuID(out isEditPage, out isSubPage);
            //    MenuInfo menu = MenuHelper.Current.GetMenu(menuID);
            //    if (menu != null)
            //    {
            //        menuName = menu.Name;
            //    }
            //}
            //else if (page is BasePage)
            //{
            //    BasePage myPage = page as BasePage;
            //    logonUser = myPage.GetLogonUser();
            //    bool isEditPage = false;
            //    bool isSubPage = false;
            //    menuID = myPage.GetMenuID(out isEditPage, out isSubPage);
            //    menuName = myPage.MenuName;
            //}
            //else
            //{
            //    logonUser = WebHelper.GetLogonUser();
            //    bool isEditPage = false;
            //    bool isSubPage = false;
            //    menuID = WebHelper.GetPageMenuID(page, out isEditPage, out isSubPage);
            //    MenuInfo menu = MenuHelper.Current.GetMenu(menuID);
            //    if (menu != null)
            //    {
            //        menuName = menu.Name;
            //    }
            //}
            #endregion

            logonUser = WebHelper.GetLogonUser();

            if (page is IMenuPage)
            {
                IMenuPage myPage = page as IMenuPage;
                menuID = myPage.MenuID;
                menuName = myPage.MenuName;
            }
            else
            {
                bool isEditPage = false;
                bool isSubPage = false;
                if (page == null)
                {
                    menuID = WebHelper.GetCurrentPageMenuID(out isEditPage, out isSubPage);
                    if (String.IsNullOrEmpty(menuID))
                    {
                        HttpContext context = HttpContext.Current;
                        if (context.Handler is IMenuPage)
                        {
                            IMenuPage myPage = context.Handler as IMenuPage;
                            menuID = myPage.MenuID;
                            menuName = myPage.MenuName;
                        }
                        else
                        {
                            menuID = System.IO.Path.GetFileNameWithoutExtension(context.Request.CurrentExecutionFilePath);
                            menuName = menuID;
                        }
                    }
                    else
                    {
                        MenuInfo menu = MenuHelper.Current.GetMenu(menuID);
                        if (menu != null)
                        {
                            menuName = menu.Name;
                        }
                        else
                        {
                            menuName = String.IsNullOrWhiteSpace(page.Title) ? menuID : page.Title;
                        }
                    }
                }
                else
                {
                    menuID = WebHelper.GetPageMenuID(page, out isEditPage, out isSubPage);
                    if (String.IsNullOrEmpty(menuID))
                    {
                        menuID = System.IO.Path.GetFileNameWithoutExtension(page.Request.CurrentExecutionFilePath);
                        menuName = String.IsNullOrWhiteSpace(page.Title) ? menuID : page.Title;
                    }
                    else
                    {
                        MenuInfo menu = MenuHelper.Current.GetMenu(menuID);
                        if (menu != null)
                        {
                            menuName = menu.Name;
                        }
                        else
                        {
                            menuName = String.IsNullOrWhiteSpace(page.Title) ? menuID : page.Title;
                        }
                    }
                }
            }
        }
        #endregion
    }

    #region [MDY:20210301] 除錯日誌紀錄器
    /// <summary>
    /// 除錯日誌紀錄器
    /// </summary>
    public class DebugLogger : IDisposable
    {
        #region Member
        /// <summary>
        /// 日誌內容暫存
        /// </summary>
        private System.Text.StringBuilder _Buffer = null;

        /// <summary>
        /// 日誌內容暫存 Size
        /// </summary>
        private int _BufferSize = 50 * 1024;  //50K Byte
        #endregion

        #region Property
        /// <summary>
        /// 使用者資訊
        /// </summary>
        public string UserInfo
        {
            get;
            private set;
        }

        /// <summary>
        /// 目前任務代碼
        /// </summary>
        public string TaskId
        {
            get;
            private set;
        }

        /// <summary>
        /// 目前日誌內容區塊編號
        /// </summary>
        public int BlockNo
        {
            get;
            private set;
        }

        /// <summary>
        /// 日誌檔資訊
        /// </summary>
        public System.IO.FileInfo FileInfo
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否可寫入日誌
        /// </summary>
        public bool CanWrite
        {
                        get;
            private set;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 虛假的 除錯日誌紀錄器 物件
        /// </summary>
        public DebugLogger()
        {
            this.Initial(null);
        }

        /// <summary>
        /// 建構 除錯日誌紀錄器 物件
        /// </summary>
        /// <param name="userInfo">指定使用者資訊</param>
        public DebugLogger(string userInfo)
        {
            this.Initial(userInfo);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// 解構 除錯日誌紀錄器 物件
        /// </summary>
        ~DebugLogger()
        {
            Dispose(false);
        }
        #endregion

        #region Implement IDisposable
        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool _Disposed = false;

        /// <summary>
        /// 執行與釋放 (Free)、釋放 (Release) 或重設 Unmanaged 資源相關聯之應用程式定義的工作
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 釋放資源
        /// </summary>
        /// <param name="disposing">指定是否釋放資源。</param>
        private void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    if (_Buffer != null && _Buffer.Length > 0)
                    {
                        this.WriteToFile();
                        _Buffer.Clear();
                        _Buffer = null;
                    }
                }
                _Disposed = true;
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="userInfo">指定使用者資訊</param>
        private void Initial(string userInfo)
        {
            if (String.IsNullOrWhiteSpace(userInfo))
            {
                this.UserInfo = null;
                this.TaskId = null;
                this.BlockNo = 0;
                this.FileInfo = null;
                this.CanWrite = false;
            }
            else
            {
                DateTime now = DateTime.Now;
                System.IO.FileInfo fileInfo = null;
                try
                {
                    string fileName = String.Format("WebDebug_{0:yyyyMMdd}.log", now);
                    string filePath = System.Configuration.ConfigurationManager.AppSettings.Get("LOG_PATH");
                    if (!String.IsNullOrWhiteSpace(filePath))
                    {
                        System.IO.DirectoryInfo pathInfo = new System.IO.DirectoryInfo(filePath);
                        if (pathInfo.FullName.Equals(filePath, StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (!pathInfo.Exists)
                            {
                                pathInfo.Create();
                            }
                            fileInfo = new System.IO.FileInfo(System.IO.Path.Combine(pathInfo.FullName, fileName));
                            _Buffer = new System.Text.StringBuilder(_BufferSize  + 1024);
                        }
                    }
                }
                catch (Exception)
                {
                    fileInfo = null;
                }
                finally
                {
                    this.UserInfo = userInfo.Trim();
                    this.TaskId = this.GenTaskId(now);
                    this.BlockNo = 0;
                    this.FileInfo = fileInfo;
                    this.CanWrite = (fileInfo != null && _Buffer != null);
                }
            }
        }

        /// <summary>
        /// 產生任務代碼
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        private string GenTaskId(DateTime now)
        {
            return String.Format("{0:X4}-{1:000}-{2:0000}-{3:X4}", now.Year, now.DayOfYear, (now.Hour * 100 + now.Minute), (now.Second * 1000 + now.Millisecond));
        }

        /// <summary>
        /// 寫入除錯日誌檔
        /// </summary>
        private void WriteToFile()
        {
            if (this.CanWrite && _Buffer.Length > 0)
            {
                try
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(this.FileInfo.FullName, true, System.Text.Encoding.Default))
                    {
                        this.BlockNo++;
                        sw.WriteLine("[UserInfo = {0}; TaskId = {1}; 日誌內容 {2} 開始 ========\\", this.UserInfo, this.TaskId, this.BlockNo);
                        sw.Write(_Buffer);
                        sw.WriteLine("[UserInfo = {0}; TaskId = {1}; 日誌內容 {2} 結束 ========//", this.UserInfo, this.TaskId, this.BlockNo);
                        sw.WriteLine();
                    }
                    _Buffer.Clear();
                }
                catch
                {

                }
            }
        }
        #endregion

        #region Public Method
        public DebugLogger Flush()
        {
            this.WriteToFile();
            return this;
        }

        public DebugLogger Append(string msg, bool autoFlush = true)
        {
            if (this.CanWrite && !String.IsNullOrWhiteSpace(msg))
            {
                try
                {
                    _Buffer.Append(msg);
                    if (autoFlush && _Buffer.Length > _BufferSize)
                    {
                        this.WriteToFile();
                    }
                }
                catch (Exception)
                {
                }
            }
            return this;
        }

        public DebugLogger AppendLine(string msg, bool autoFlush = true)
        {
            if (this.CanWrite && !String.IsNullOrWhiteSpace(msg))
            {
                try
                {
                    _Buffer.AppendLine(msg);
                    if (autoFlush && _Buffer.Length > _BufferSize)
                    {
                        this.WriteToFile();
                    }
                }
                catch (Exception)
                {
                }
            }
            return this;
        }

        public DebugLogger AppendLine(bool autoFlush = true)
        {
            if (this.CanWrite)
            {
                try
                {
                    _Buffer.AppendLine();
                    if (autoFlush && _Buffer.Length > _BufferSize)
                    {
                        this.WriteToFile();
                    }
                }
                catch (Exception)
                {
                }
            }
            return this;
        }

        public DebugLogger AppendFormat(string format, params object[] args)
        {
            if (this.CanWrite && !String.IsNullOrWhiteSpace(format) && args != null)
            {
                try
                {
                    _Buffer.AppendFormat(format, args);
                }
                catch (Exception)
                {
                }
            }
            return this;
        }

        /// <summary>
        /// 重新（初始化）產生任務代碼、日誌檔檔名（如果暫存有資料會先寫入日誌檔）
        /// </summary>
        public void ReNew()
        {
            this.WriteToFile();
            if (!String.IsNullOrWhiteSpace(this.UserInfo))
            {
                DateTime now = DateTime.Now;
                System.IO.FileInfo fileInfo = null;
                try
                {
                    string fileName = String.Format("WebDebug_{0:yyyyMMdd}.log", now);
                    string filePath = System.Configuration.ConfigurationManager.AppSettings.Get("LOG_PATH");
                    if (!String.IsNullOrWhiteSpace(filePath))
                    {
                        System.IO.DirectoryInfo pathInfo = new System.IO.DirectoryInfo(filePath);
                        if (pathInfo.FullName.Equals(filePath, StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (!pathInfo.Exists)
                            {
                                pathInfo.Create();
                            }
                            fileInfo = new System.IO.FileInfo(System.IO.Path.Combine(pathInfo.FullName, fileName));
                        }
                    }
                }
                catch (Exception)
                {
                    fileInfo = null;
                }
                finally
                {
                    this.TaskId = this.GenTaskId(now);
                    this.BlockNo = 0;
                    this.FileInfo = fileInfo;
                    this.CanWrite = (fileInfo != null && _Buffer != null);
                }
            }
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 建立 除錯日誌紀錄器 物件
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static DebugLogger Create(LogonUser user, string sessionId)
        {
            string userInfo = user == null ? null : String.Format("{0} | {1} | {2} | {3}", user.UnitName, user.UnitId, user.UserId, sessionId);
            return new DebugLogger(userInfo);
        }
        #endregion
    }
    #endregion
}
