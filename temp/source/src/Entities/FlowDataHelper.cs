using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 流程資料處理工具類別
    /// </summary>
    public class FlowDataHelper
    {
        /// <summary>
        /// 取得新的流程 Guid
        /// </summary>
        /// <returns></returns>
        public string GetNewFlowGuid()
        {
            return Common.GetGUID();
        }

        /// <summary>
        /// 取得權限管理 (S5200002) 的表單資料序列化字串
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public string GetS5200002FormData(KeyValueList<string> datas)
        {
            string xml = null;
            if (datas != null && Common.TryToXmlExplicitly2(datas, typeof(KeyValueList<string>), out xml))
            {
                return xml;
            }
            return null;
        }

        /// <summary>
        /// 取得群組管理 (S5200003) 的表單資料序列化字串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetS5200003FormData(GroupListEntity data)
        {
            string xml = null;
            if  (data != null && Common.TryToXmlExplicitly2(data, typeof(GroupListEntity), out xml))
            {
                return xml;
            }
            return null;
        }

        /// <summary>
        /// 取得使用者管理 (S5300001) 的表單資料序列化字串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetS5300001FormData(KeyValueList<string> data)
        {
            string xml = null;
            if (data != null && Common.TryToXmlExplicitly2(data, typeof(KeyValueList<string>), out xml))
            {
                return xml;
            }
            return null;
        }

        /// <summary>
        /// 取得使用者管理 (S5300001) 的表單資料索引鍵值
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public string GetS5300001DataKey(string userId, string groupId, string bankId)
        {
            return String.Format("{0},{1},{2}", userId, groupId, bankId);
        }



        /// <summary>
        /// 處理流程資料
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="processKind"></param>
        /// <param name="processMemo"></param>
        /// <param name="processUserQual"></param>
        /// <param name="processUnitId"></param>
        /// <param name="processUserId"></param>
        /// <param name="processUserName"></param>
        /// <returns></returns>
        public Result ProcessFlowData(DBLogger dbLogger, string guid, string processKind, string processMemo, string processUserQual, string processUnitId, string processUserId, string processUserName)
        {
            #region 取得 FlowData
            FlowDataEntity flowData = null;
            {
                Result result = null;
                using (EntityFactory factory = new EntityFactory())
                {
                    Expression where = new Expression(FlowDataEntity.Field.Guid, guid)
                        .And(FlowDataEntity.Field.Status, FlowStatusCodeTexts.FLOWING);
                    result = factory.SelectFirst<FlowDataEntity>(where, null, out flowData);
                    if (result.IsSuccess)
                    {
                        if (flowData == null)
                        {
                            result = new Result(false, "該待辦事項不存在或已處理", ErrorCode.D_DATA_NOT_FOUND, null);
                        }
                        else
                        {
                            KeyValue[] fieldValues = new KeyValue[] { new KeyValue(FlowDataEntity.Field.Status, FlowStatusCodeTexts.PROCESSING) };
                            int count = 0;
                            result = factory.UpdateFields<FlowDataEntity>(fieldValues, where, out count);
                            if (result.IsSuccess && count == 0)
                            {
                                result = new Result(false, "該待辦事項不存在或已處理", ErrorCode.D_DATA_NOT_FOUND, null);
                            }
                        }
                    }
                }
                if (result.IsSuccess)
                {
                    flowData.Status = FlowStatusCodeTexts.PROCESSING;
                }
                else
                {
                    return result;
                }
            }
            #endregion

            #region 處理流程資料
            {
                Result result = null;
                Expression where = new Expression(FlowDataEntity.Field.Guid, guid)
                    .And(FlowDataEntity.Field.Status, FlowStatusCodeTexts.PROCESSING);
                KeyValueList fieldValues = new KeyValueList();
                fieldValues.Add(FlowDataEntity.Field.Status, FlowStatusCodeTexts.ENDING);

                Result processResult = null;

                if (processKind == ProcessKindCodeTexts.APPROVE || processKind == ProcessKindCodeTexts.REJECT)
                {
                    #region 處理資料的放行或駁回
                    try
                    {
                        switch (flowData.FormId)
                        {
                            case FormCodeTexts.S5200002:
                                processResult = this.ProcessS5200002(dbLogger, flowData, processKind, processMemo, processUserQual, processUnitId, processUserId, processUserName);
                                break;
                            case FormCodeTexts.S5200003:
                                processResult = this.ProcessS5200003(dbLogger, flowData, processKind, processMemo, processUserQual, processUnitId, processUserId, processUserName);
                                break;
                            case FormCodeTexts.S5300001:
                                processResult = this.ProcessS5300001(dbLogger, flowData, processKind, processMemo, processUserQual, processUnitId, processUserId, processUserName);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        processResult = new Result(false, ex.Message, CoreStatusCode.UNKNOWN_EXCEPTION, null);
                    }
                    #endregion

                    #region 更新 FlowData 的處理結果
                    if (processResult.IsSuccess)
                    {
                        fieldValues.Add(FlowDataEntity.Field.ProcessKind, processKind);
                        fieldValues.Add(FlowDataEntity.Field.ProcessUserQual, processUserQual);
                        fieldValues.Add(FlowDataEntity.Field.ProcessUnitId, processUnitId);
                        fieldValues.Add(FlowDataEntity.Field.ProcessUserId, processUserId);
                        fieldValues.Add(FlowDataEntity.Field.ProcessUserName, processUserName);
                    }
                    else
                    {
                        processMemo = String.Concat("資料處理失敗，", processResult.Message);
                        fieldValues.Add(FlowDataEntity.Field.ProcessKind, ProcessKindCodeTexts.REJECT);
                        fieldValues.Add(FlowDataEntity.Field.ProcessUserQual, "");
                        fieldValues.Add(FlowDataEntity.Field.ProcessUnitId, "SYSTEM");
                        fieldValues.Add(FlowDataEntity.Field.ProcessUserId, "SYSTEM");
                        fieldValues.Add(FlowDataEntity.Field.ProcessUserName, "SYSTEM");
                    }

                    #region [MDY:20160705]
                    fieldValues.Add(FlowDataEntity.Field.ProcessMemo, processMemo.Length > 200 ? processMemo.Substring(0, 200) : processMemo);
                    fieldValues.Add(FlowDataEntity.Field.ProcessDate, DateTime.Now);
                    #endregion

                    int count = 0;
                    using (EntityFactory factory = new EntityFactory())
                    {
                        result = factory.UpdateFields<FlowDataEntity>(fieldValues, where, out count);
                        if (result.IsSuccess && count == 0)
                        {
                            result = new Result(false, "該待辦事項已處理", ErrorCode.D_DATA_NOT_FOUND, null);
                        }
                        else
                        {
                            result = processResult;
                        }
                    }
                    #endregion
                }
                else
                {
                    result = new Result(false, "處理參數不正確", ErrorCode.S_INVALID_PARAMETER, null);
                }
                return result;
            }
            #endregion
        }

        /// <summary>
        /// 處理 S5200002 (權限管理)
        /// </summary>
        /// <param name="dbLogger"></param>
        /// <param name="flowData"></param>
        /// <param name="processKind"></param>
        /// <param name="processMemo"></param>
        /// <param name="processUserQual"></param>
        /// <param name="processUnitId"></param>
        /// <param name="processUserId"></param>
        /// <param name="processUserName"></param>
        /// <returns></returns>
        private Result ProcessS5200002(DBLogger dbLogger, FlowDataEntity flowData, string processKind, string processMemo, string processUserQual, string processUnitId, string processUserId, string processUserName)
        {
            if (processKind == ProcessKindCodeTexts.REJECT)
            {
                //此待辦事項 Rject 不用處理任何資料，直接回傳 true
                return new Result(true);
            }

            #region 處理資料參數
            string groupId = flowData.DataKey;
            if (String.IsNullOrEmpty(groupId))
            {
                return new Result(false, "待辦事項資料中缺少資料索引參數或資料索引不正確", CoreStatusCode.UNKNOWN_ERROR, null);
            }

            KeyValueList<string> datas = null;
            if (flowData.ApplyKind == ApplyKindCodeTexts.UPDATE)
            {
                object formData = null;
                if (!String.IsNullOrEmpty(flowData.FormData) && Common.TryDeXmlExplicitly2(flowData.FormData, typeof(KeyValueList<string>), out formData))
                {
                    datas = formData as KeyValueList<string>;
                }
                if (datas == null || datas.Count == 0)
                {
                    return new Result(false, "待辦事項資料中資料參數不正確", CoreStatusCode.UNKNOWN_ERROR, null);
                }
                KeyValue<string> data = datas.Find(x => x.Key.Equals("groupID", StringComparison.CurrentCultureIgnoreCase));
                if (data == null || data.Value != groupId)
                {
                    return new Result(false, "待辦事項資料中資料索引值不一致", CoreStatusCode.UNKNOWN_ERROR, null);
                }
            }
            else
            {
                return new Result(false, "待辦事項資料中申請種類參數不正確", ErrorCode.D_DATA_NOT_FOUND, null);
            }
            #endregion

            Result result = null;
            string funcId = flowData.FormId;
            string funcName = FormCodeTexts.GetText(flowData.FormId);

            #region 取得功能選單資料
            FuncMenuEntity[] funcMenus = null;
            {
                using (EntityFactory factory = new EntityFactory())
                {
                    Expression where = new Expression(FuncMenuEntity.Field.Status, DataStatusCodeTexts.NORMAL);
                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                    orderbys.Add(FuncMenuEntity.Field.FuncId, OrderByEnum.Asc);
                    result = factory.SelectAll<FuncMenuEntity>(where, orderbys, out funcMenus);

                    //這裡取的資料不會回傳，不重要所以不記 DBLogger 了，以免記太多
                }
                if (funcMenus == null)
                {
                    funcMenus = new FuncMenuEntity[0];
                }
            }
            #endregion

            #region 檢查參數中的功能代碼，並處理父層
            KeyValueList<string> funcRights = new KeyValueList<string>();
            if (result.IsSuccess && datas != null)
            {
                foreach (KeyValue<string> data in datas)
                {
                    string key = data.Key;
                    if (key.Equals("GroupId", StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }
                    else if (key.Equals("FormInfos", StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }
                    else if (key.Length > 3 && funcMenus != null && funcMenus.Length > 0)    //funcId.Length < 3 視為 Menu，不處理
                    {
                        string rightCode = GroupRightEntity.FormatRightCode(data.Value == null ? null : data.Value.Trim());
                        if (rightCode != GroupRightEntity.None_RightCode)
                        {
                            FuncMenuEntity myFuncMenu = null;
                            myFuncMenu = Array.Find<FuncMenuEntity>(funcMenus, x => x.FuncId == key);
                            if (myFuncMenu != null)
                            {
                                funcRights.Add(key, rightCode);

                                #region 有父層且為 Menu (parnetId.Length <= 3) 且未加入
                                string parnetId = myFuncMenu.ParentId;
                                if (!String.IsNullOrEmpty(parnetId) && parnetId.Length <= 3 && funcRights.GetKeyFirstIndex(parnetId) < 0)
                                {
                                    FuncMenuEntity myParentMenu = Array.Find<FuncMenuEntity>(funcMenus, x => x.FuncId == parnetId);
                                    if (myParentMenu != null)
                                    {
                                        funcRights.Add(parnetId, GroupRightEntity.All_RightCode);

                                        #region 最多有三層，要處理父層的父層
                                        string grandParnetId = myParentMenu.ParentId;
                                        if (!String.IsNullOrEmpty(grandParnetId) && grandParnetId.Length <= 3 && funcRights.GetKeyFirstIndex(grandParnetId) < 0)
                                        {
                                            FuncMenuEntity myGrandParentMenu = Array.Find<FuncMenuEntity>(funcMenus, x => x.FuncId == grandParnetId);
                                            if (myGrandParentMenu != null)
                                            {
                                                funcRights.Add(grandParnetId, GroupRightEntity.All_RightCode);
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            #endregion

            #region 更新群組權限資料 (使用交易，因為整個申請的資料要全部處理成功才算成功)
            if (result.IsSuccess)
            {
                #region [MDY:20220910] Checkmarx - Improper Transaction Handling 誤判調整
                #region [OLD]
                //using (EntityFactory factory = new EntityFactory(true))
                //{
                //    try
                //    {
                //        #region 刪除群組權限資料資料
                //        {
                //            string sql = String.Format(@"DELETE {0} WHERE Group_Id=@GROUP_ID", GroupRightEntity.TABLE_NAME);
                //            KeyValue[] parameters = new KeyValue[1] { new KeyValue("@GROUP_ID", groupId) };
                //            int count = 0;
                //            result = factory.ExecuteNonQuery(sql, parameters, out count);

                //            #region 新增日誌資料
                //            if (dbLogger != null)
                //            {
                //                string notation = null;
                //                if (result.IsSuccess)
                //                {
                //                    notation = String.Format("[{0}] {1}資料成功 (共{2}筆)", funcName, LogTypeCodeTexts.DELETE_TEXT, count);
                //                }
                //                else
                //                {
                //                    notation = String.Format("[{0}] {1}資料失敗 (錯誤訊息：{2})", funcName, LogTypeCodeTexts.DELETE_TEXT, result.Message);
                //                }
                //                dbLogger.AppendLog(flowData.ApplyUserQual, flowData.ApplyUnitId, funcId, LogTypeCodeTexts.DELETE, flowData.ApplyUserId, notation);
                //            }
                //            #endregion
                //        }
                //        #endregion

                //        #region 新增群組權限資料
                //        if (result.IsSuccess)
                //        {
                //            #region 逐筆新增
                //            int totoalCount = 0;
                //            string sql = String.Format(@"INSERT INTO {0}(Group_Id, Func_Id, Right_Code, status, crt_date, crt_user) Values(@GroupId, @FuncId, @RightCode, @Status, GETDATE(), @CrtUser) ", GroupRightEntity.TABLE_NAME);
                //            foreach (KeyValue<string> funcRightCode in funcRights)
                //            {
                //                KeyValue[] parameters = new KeyValue[5] {
                //                    new KeyValue("@GroupId", groupId),
                //                    new KeyValue("@FuncId", funcRightCode.Key),
                //                    new KeyValue("@RightCode", funcRightCode.Value),
                //                    new KeyValue("@Status", DataStatusCodeTexts.NORMAL),
                //                    new KeyValue("@CrtUser", flowData.ApplyUserId)
                //                };

                //                int count = 0;
                //                result = factory.ExecuteNonQuery(sql, parameters, out count);

                //                if (result.IsSuccess)
                //                {
                //                    totoalCount += count;
                //                }
                //                else
                //                {
                //                    break;
                //                }
                //            }
                //            #endregion

                //            #region 新增日誌資料
                //            if (dbLogger != null)
                //            {
                //                string notation = null;
                //                if (result.IsSuccess)
                //                {
                //                    notation = String.Format("[{0}] {1}資料成功 (共{2}筆)", funcName, LogTypeCodeTexts.INSERT_TEXT, totoalCount);
                //                }
                //                else
                //                {
                //                    notation = String.Format("[{0}] {1}資料失敗 (錯誤訊息：{2})", funcName, LogTypeCodeTexts.INSERT_TEXT, result.Message);
                //                }
                //                dbLogger.AppendLog(flowData.ApplyUserQual, flowData.ApplyUnitId, funcId, LogTypeCodeTexts.INSERT, flowData.ApplyUserId, notation);
                //            }
                //            #endregion
                //        }
                //        #endregion
                //    }
                //    catch (Exception ex)
                //    {
                //        string logType = LogTypeCodeTexts.EXECUTE;
                //        string notation = String.Format("[{0}] {1}資料發生例外 (錯誤訊息：{2})", funcName, logType, ex.Message);
                //        result = new Result(false, notation, CoreStatusCode.UNKNOWN_EXCEPTION, ex);

                //        #region 新增日誌資料
                //        if (dbLogger != null)
                //        {
                //            dbLogger.AppendLog(flowData.ApplyUserQual, flowData.ApplyUnitId, funcId, logType, flowData.ApplyUserId, notation);
                //        }
                //        #endregion
                //    }
                //    finally
                //    {
                //        if (result.IsSuccess)
                //        {
                //            factory.Commit();
                //        }
                //        else
                //        {
                //            factory.Rollback();
                //        }
                //    }
                //}
                #endregion

                using (EntityFactory factory = new EntityFactory(true))
                {
                    #region 刪除群組權限資料資料
                    if (result.IsSuccess)
                    {
                        string sql = String.Format(@"DELETE {0} WHERE Group_Id=@GROUP_ID", GroupRightEntity.TABLE_NAME);
                        KeyValue[] parameters = new KeyValue[1] { new KeyValue("@GROUP_ID", groupId) };
                        int count = 0;
                        result = factory.ExecuteNonQuery(sql, parameters, out count);

                        #region 新增日誌資料
                        if (dbLogger != null)
                        {
                            string notation = null;
                            if (result.IsSuccess)
                            {
                                notation = String.Format("[{0}] {1}資料成功 (共{2}筆)", funcName, LogTypeCodeTexts.DELETE_TEXT, count);
                            }
                            else
                            {
                                notation = String.Format("[{0}] {1}資料失敗 (錯誤訊息：{2})", funcName, LogTypeCodeTexts.DELETE_TEXT, result.Message);
                            }
                            dbLogger.AppendLog(flowData.ApplyUserQual, flowData.ApplyUnitId, funcId, LogTypeCodeTexts.DELETE, flowData.ApplyUserId, notation);
                        }
                        #endregion
                    }
                    #endregion

                    #region 新增群組權限資料
                    if (result.IsSuccess)
                    {
                        #region 逐筆新增
                        int totoalCount = 0;
                        string sql = String.Format(@"INSERT INTO {0}(Group_Id, Func_Id, Right_Code, status, crt_date, crt_user) Values(@GroupId, @FuncId, @RightCode, @Status, GETDATE(), @CrtUser) ", GroupRightEntity.TABLE_NAME);
                        foreach (KeyValue<string> funcRightCode in funcRights)
                        {
                            KeyValue[] parameters = new KeyValue[5] {
                                new KeyValue("@GroupId", groupId),
                                new KeyValue("@FuncId", funcRightCode.Key),
                                new KeyValue("@RightCode", funcRightCode.Value),
                                new KeyValue("@Status", DataStatusCodeTexts.NORMAL),
                                new KeyValue("@CrtUser", flowData.ApplyUserId)
                            };

                            int count = 0;
                            result = factory.ExecuteNonQuery(sql, parameters, out count);

                            if (result.IsSuccess)
                            {
                                totoalCount += count;
                            }
                            else
                            {
                                break;
                            }
                        }
                        #endregion

                        #region 新增日誌資料
                        if (dbLogger != null)
                        {
                            string notation = null;
                            if (result.IsSuccess)
                            {
                                notation = String.Format("[{0}] {1}資料成功 (共{2}筆)", funcName, LogTypeCodeTexts.INSERT_TEXT, totoalCount);
                            }
                            else
                            {
                                notation = String.Format("[{0}] {1}資料失敗 (錯誤訊息：{2})", funcName, LogTypeCodeTexts.INSERT_TEXT, result.Message);
                            }
                            dbLogger.AppendLog(flowData.ApplyUserQual, flowData.ApplyUnitId, funcId, LogTypeCodeTexts.INSERT, flowData.ApplyUserId, notation);
                        }
                        #endregion
                    }
                    #endregion

                    if (result.IsSuccess)
                    {
                        factory.Commit();
                    }
                    else
                    {
                        factory.Rollback();
                    }
                }
                #endregion
            }
            #endregion

            return result;
        }

        /// <summary>
        /// 處理 S5200003 (群組管理)
        /// </summary>
        /// <param name="dbLogger"></param>
        /// <param name="flowData"></param>
        /// <param name="processKind"></param>
        /// <param name="processMemo"></param>
        /// <param name="processUserQual"></param>
        /// <param name="processUnitId"></param>
        /// <param name="processUserId"></param>
        /// <param name="processUserName"></param>
        /// <returns></returns>
        private Result ProcessS5200003(DBLogger dbLogger, FlowDataEntity flowData, string processKind, string processMemo, string processUserQual, string processUnitId, string processUserId, string processUserName)
        {
            if (processKind == ProcessKindCodeTexts.REJECT)
            {
                //此待辦事項 Rject 不用處理任何資料，直接回傳 true
                return new Result(true);
            }

            #region 處理資料參數
            string groupId = flowData.DataKey;
            if (String.IsNullOrEmpty(groupId))
            {
                return new Result(false, "待辦事項資料中缺少資料索引參數或資料索引不正確", CoreStatusCode.UNKNOWN_ERROR, null);
            }

            GroupListEntity data = null;
            if (flowData.ApplyKind == ApplyKindCodeTexts.INSERT || flowData.ApplyKind == ApplyKindCodeTexts.UPDATE)
            {
                object formData = null;
                if (!String.IsNullOrEmpty(flowData.FormData) && Common.TryDeXmlExplicitly2(flowData.FormData, typeof(GroupListEntity), out formData))
                {
                    data = formData as GroupListEntity;
                }
                if (data == null)
                {
                    return new Result(false, "待辦事項資料中資料參數不正確", CoreStatusCode.UNKNOWN_ERROR, null);
                }
                if (data.GroupId != groupId)
                {
                    return new Result(false, "待辦事項資料中資料索引值不一致", CoreStatusCode.UNKNOWN_ERROR, null);
                }
            }
            #endregion

            #region 檢查 data 欄位值
            if (data != null)
            {
                if (String.IsNullOrEmpty(data.GroupName) || !RoleCodeTexts.IsDefine(data.Role) || RoleTypeCodeTexts.GetCodeText(data.RoleType) == null
                    || DataStatusCodeTexts.GetCodeText(data.Status) == null)
                {
                    return new Result(false, "待辦事項資料中資料參數不正確", CoreStatusCode.UNKNOWN_ERROR, null);
                }
                if (flowData.ApplyKind == ApplyKindCodeTexts.INSERT)
                {
                    if (String.IsNullOrEmpty(data.CrtUser))
                    {
                        return new Result(false, "待辦事項資料中資料參數不正確", CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    if (data.CrtDate < DateTime.Today)
                    {
                        data.CrtDate = DateTime.Now;
                    }
                    if (data.Role == RoleCodeTexts.SCHOOL && String.IsNullOrEmpty(data.Branchs))
                    {
                        return new Result(false, "待辦事項資料中資料參數不正確", CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                }
                else if (flowData.ApplyKind == ApplyKindCodeTexts.UPDATE)
                {
                    if (String.IsNullOrEmpty(data.MdyUser))
                    {
                        return new Result(false, "待辦事項資料中資料參數不正確", CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    if (data.MdyDate < DateTime.Today)
                    {
                        data.MdyDate = DateTime.Now;
                    }
                }
            }
            #endregion

            Result result = null;
            string funcId = flowData.FormId;
            string funcName = FormCodeTexts.GetText(flowData.FormId);
            string logType = null;

            using (EntityFactory factory = new EntityFactory())
            {
                int count = 0;
                switch (flowData.ApplyKind)
                {
                    case ApplyKindCodeTexts.INSERT:
                        #region 新增
                        {
                            logType = LogTypeCodeTexts.INSERT;

                            //先檢查 Key 再新增
                            Expression where = new Expression(GroupListEntity.Field.GroupId, groupId);
                            result = factory.SelectCount<GroupListEntity>(where, out count);
                            if (result.IsSuccess && count > 0)
                            {
                                result = new Result(false, "該群組代碼已存在", ErrorCode.D_DATA_EXISTS, null);
                            }
                            if (result.IsSuccess)
                            {
                                result = factory.Insert(data, out count);
                                if (result.IsSuccess && count == 0)
                                {
                                    result = new Result(false, "該群組代碼已存在", ErrorCode.D_DATA_EXISTS, null);
                                }
                            }
                        }
                        #endregion
                        break;
                    case ApplyKindCodeTexts.UPDATE:
                        #region 修改
                        {
                            logType = LogTypeCodeTexts.UPDATE;

                            #region 更新條件
                            Expression where = new Expression(GroupListEntity.Field.GroupId, data.GroupId)
                                .And(GroupListEntity.Field.Role, data.Role);
                                //.And(GroupListEntity.Field.Branchs, data.Branchs);
                            #endregion

                            #region 更新欄位
                            KeyValueList fieldValues = new KeyValueList();
                            fieldValues.Add(GroupListEntity.Field.GroupName, data.GroupName);
                            fieldValues.Add(GroupListEntity.Field.RoleType, data.RoleType);
                            fieldValues.Add(GroupListEntity.Field.MdyUser, data.MdyUser);
                            fieldValues.Add(GroupListEntity.Field.MdyDate, data.MdyDate);
                            //fieldValues.Add(GroupListEntity.Field.Status, data.Status);
                            #endregion

                            result = factory.UpdateFields<GroupListEntity>(fieldValues, where, out count);
                            if (result.IsSuccess && count == 0)
                            {
                                result = new Result(false, "資料不存在或已處理", ErrorCode.D_DATA_NOT_FOUND, null);
                            }
                        }
                        #endregion
                        break;
                    case ApplyKindCodeTexts.DELETE:
                        #region 刪除
                        {
                            logType = LogTypeCodeTexts.DELETE;

                            data = new GroupListEntity();
                            data.GroupId = groupId;
                            result = factory.Delete(data, out count);
                            if (result.IsSuccess && count == 0)
                            {
                                result = new Result(false, "資料不存在或已處理", ErrorCode.D_DATA_NOT_FOUND, null);
                            }
                        }
                        #endregion
                        break;
                    default:
                        result = new Result(false, "待辦事項資料中申請種類參數不正確", ErrorCode.D_DATA_NOT_FOUND, null);
                        break;
                }
            }

            #region 新增日誌資料
            if (dbLogger != null)
            {
                string notation = null;
                if (result.IsSuccess)
                {
                    notation = String.Format("[{0}] {1}資料成功 (GUID={2}; GroupId={3})", funcName, LogTypeCodeTexts.GetText(logType), flowData.Guid, groupId);
                }
                else
                {
                    notation = String.Format("[{0}] {1}資料失敗 (GUID={2}; GroupId={3})，錯誤訊息：{4})", funcName, LogTypeCodeTexts.GetText(logType), flowData.Guid, groupId, result.Message);
                }
                dbLogger.AppendLog(flowData.ApplyUserQual, flowData.ApplyUnitId, funcId, logType, flowData.ApplyUserId, notation);
            }
            #endregion

            return result;
        }

        /// <summary>
        /// 處理 S5300001 (使用者管理)
        /// </summary>
        /// <param name="dbLogger"></param>
        /// <param name="flowData"></param>
        /// <param name="processKind"></param>
        /// <param name="processMemo"></param>
        /// <param name="processUserQual"></param>
        /// <param name="processUnitId"></param>
        /// <param name="processUserId"></param>
        /// <param name="processUserName"></param>
        /// <returns></returns>
        private Result ProcessS5300001(DBLogger dbLogger, FlowDataEntity flowData, string processKind, string processMemo, string processUserQual, string processUnitId, string processUserId, string processUserName)
        {
            if (processKind == ProcessKindCodeTexts.REJECT)
            {
                //此待辦事項 Rject 不用處理任何資料，直接回傳 true
                return new Result(true);
            }

            #region [MDY:20220530] Checkmarx 調整
            #region 處理資料參數
            string dataKey = flowData.DataKey;
            if (String.IsNullOrEmpty(dataKey))
            {
                return new Result(false, "待辦事項資料中缺少資料索引參數或資料索引不正確", CoreStatusCode.UNKNOWN_ERROR, null);
            }

            KeyValueList<string> args = null;
            string userId = null;
            string bankId = null;
            string groupId = null;
            string receiveType = null;
            string userName = null;
            string newPXX = null;
            string tel = null;
            string email = null;
            string newReceiveType = null;
            string isLocked = null;
            string title = String.Empty;
            {
                object formData = null;
                if (!String.IsNullOrEmpty(flowData.FormData) && Common.TryDeXmlExplicitly2(flowData.FormData, typeof(KeyValueList<string>), out formData))
                {
                    args = formData as KeyValueList<string>;
                }
                if (args == null || args.Count == 0)
                {
                    return new Result(false, "待辦事項資料中資料參數不正確", CoreStatusCode.UNKNOWN_ERROR, null);
                }

                userId = args.TryGetValue("UserId", String.Empty);
                bankId = args.TryGetValue("BankId", String.Empty);
                groupId = args.TryGetValue("GroupId", String.Empty);
                receiveType = args.TryGetValue("ReceiveType", String.Empty);
                string argKey = this.GetS5300001DataKey(userId, groupId, bankId);
                if (dataKey != argKey)
                {
                    return new Result(false, "待辦事項資料中資料索引值不一致", CoreStatusCode.UNKNOWN_ERROR, null);
                }
                if (String.IsNullOrEmpty(userId) || String.IsNullOrEmpty(bankId) || String.IsNullOrEmpty(groupId))
                {
                    return new Result(false, "待辦事項資料中索引值參數不正確", CoreStatusCode.UNKNOWN_ERROR, null);
                }

                switch (flowData.ApplyKind)
                {
                    case ApplyKindCodeTexts.INSERT:
                        userName = args.TryGetValue("UserName", String.Empty);
                        newPXX = args.TryGetValue("NewPXX", String.Empty);
                        tel = args.TryGetValue("Tel", String.Empty);
                        email = args.TryGetValue("Email", String.Empty);
                        if (String.IsNullOrEmpty(userName))
                        {
                            return new Result(false, "待辦事項資料中資料參數不正確", CoreStatusCode.UNKNOWN_ERROR, null);
                        }
                        break;
                    case ApplyKindCodeTexts.UPDATE:
                        userName = args.TryGetValue("UserName", String.Empty);
                        newPXX = args.TryGetValue("NewPXX", String.Empty);
                        tel = args.TryGetValue("Tel", String.Empty);
                        email = args.TryGetValue("Email", String.Empty);
                        newReceiveType = args.TryGetValue("NewReceiveType", String.Empty);
                        isLocked = args.TryGetValue("IsLocked", String.Empty);
                        if (String.IsNullOrEmpty(userName) || (isLocked != "Y" && isLocked != "N"))
                        {
                            return new Result(false, "待辦事項資料中資料參數不正確", CoreStatusCode.UNKNOWN_ERROR, null);
                        }
                        break;
                }

                if (!String.IsNullOrEmpty(receiveType) && receiveType != "*")
                {
                    string[] receiveTypes = receiveType.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int idx = 0; idx < receiveTypes.Length; idx++)
                    {
                        receiveTypes[idx] = receiveTypes[idx].Trim();
                    }
                    receiveType = "," + String.Join(",", receiveTypes) + ",";
                }
                if (!String.IsNullOrEmpty(newReceiveType) && newReceiveType != "*")
                {
                    string[] newReceiveTypes = receiveType.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int idx = 0; idx < newReceiveTypes.Length; idx++)
                    {
                        newReceiveTypes[idx] = newReceiveTypes[idx].Trim();
                    }
                    newReceiveType = "," + String.Join(",", newReceiveTypes) + ",";
                }
            }
            #endregion

            #region 檢查參數值
            GroupListEntity group = null;
            using (EntityFactory factory = new EntityFactory())
            {
                Expression where = new Expression(GroupListEntity.Field.GroupId, groupId);
                Result result2 = factory.SelectFirst<GroupListEntity>(where, null, out group);
                if (result2.IsSuccess)
                {
                    if (group == null)
                    {
                        return new Result(false, "待辦事項資料中群組資料不正確", CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    if (group.Role == RoleCodeTexts.STAFF)
                    {
                        if (receiveType != String.Empty)
                        {
                            return new Result(false, "待辦事項資料中授權商家代號與群組資料不合", CoreStatusCode.UNKNOWN_ERROR, null);
                        }

                        #region [MDY:20160814] 修正分行代碼判斷 (因為總行類的分行第6碼為英文)
                        #region [Old]
                        //if (!bankId.StartsWith(DataFormat.MyBankID) || !Common.IsNumber(bankId, 6))
                        //{
                        //    return new Result(false, "待辦事項資料中分行代碼與群組資料不合", CoreStatusCode.UNKNOWN_ERROR, null);
                        //}
                        #endregion

                        if (!DataFormat.IsMyBankCode(bankId))
                        {
                            return new Result(false, "待辦事項資料中分行代碼與群組資料不合", CoreStatusCode.UNKNOWN_ERROR, null);
                        }
                        #endregion
                    }
                    else
                    {
                        if (group.RoleType == RoleTypeCodeTexts.MANAGER)
                        {
                            if (receiveType != "*")
                            {
                                return new Result(false, "待辦事項資料中授權商家代號與群組資料不合", CoreStatusCode.UNKNOWN_ERROR, null);
                            }
                        }
                        else if (String.IsNullOrEmpty(receiveType) || receiveType == "*")
                        {
                            return new Result(false, "待辦事項資料中授權商家代號與群組資料不合", CoreStatusCode.UNKNOWN_ERROR, null);
                        }
                        if (!Common.IsNumber(bankId, 4) || !group.GroupId.StartsWith(bankId))
                        {
                            return new Result(false, "待辦事項資料中學校代碼與群組資料不合", CoreStatusCode.UNKNOWN_ERROR, null);
                        }
                        if (flowData.ApplyKind == ApplyKindCodeTexts.INSERT && String.IsNullOrEmpty(newPXX))
                        {
                            return new Result(false, "待辦事項資料中密碼資料不正確", CoreStatusCode.UNKNOWN_ERROR, null);
                        }
                    }
                }
                else
                {
                    return new Result(false, "查詢群組資料失敗，" + result2.Message, result2.Code, result2.Exception);
                }
            }
            #endregion

            Result result = null;
            string funcId = flowData.FormId;
            string funcName = FormCodeTexts.GetText(flowData.FormId);
            string logType = null;
            DateTime now = DateTime.Now;

            using (EntityFactory factory = new EntityFactory())
            {
                int count = 0;
                switch (flowData.ApplyKind)
                {
                    case ApplyKindCodeTexts.INSERT:
                        #region 新增
                        {
                            logType = LogTypeCodeTexts.INSERT;

                            UsersEntity user = new UsersEntity();

                            user.UId = userId;
                            user.UGrp = groupId;
                            user.URt = receiveType;
                            user.UBank = bankId;
                            user.UName = userName;
                            user.Tel = tel ?? String.Empty;
                            user.Title = title ?? String.Empty;
                            user.Email = email ?? String.Empty;

                            #region [MDY:20220530] Checkmarx 調整
                            user.UPXX = newPXX;
                            #endregion

                            user.ULock = "N";

                            user.ResetUid = null;   // flowData.ApplyUserId;
                            user.Resetdate = null;  // Common.GetTWDate7(now);

                            #region [MDY:20220530] Checkmarx 調整
                            user.ResetPXX = null; // user.UPXX;

                            //如果要讓首次登入強迫變更密碼，user.InitPXX 要設定為 user.UPXX
                            user.InitPXX = user.UPXX;    //String.Empty;
                            #endregion

                            user.UNum = null;

                            #region [MDY:20220530] Checkmarx 調整
                            user.UPXX1 = null;
                            #endregion

                            user.Sessionid = null;
                            user.Remark = String.Empty;

                            user.Creator = flowData.ApplyUserId;
                            user.Createdate = Common.GetTWDate7(now);   // Common.GetTWDate7(flowData.ApplyDate);
                            user.Approver = processUserId;
                            user.Approvedate = Common.GetTWDate7(now);
                            user.LastApproveUser = null;
                            user.LastApproveTime = null;
                            user.Flag = "0";

                            user.DataStatus = "0";
                            user.ProcessStatus = "0";
                            user.LastModifyUser = null;
                            user.LastModifyTime = null;
                            user.PXXChangeDate = String.Empty;


                            //先檢查 Key 再新增
                            #region [MDY:20161013] 行員的帳號為所有銀行唯一，學校的帳號為該校唯一
                            if (group.Role == RoleCodeTexts.STAFF)
                            {
                                Expression where = new Expression(UsersEntity.Field.UBank, RelationEnum.Like, DataFormat.MyBankID + "___")
                                    .And(UsersEntity.Field.UId, userId);
                                result = factory.SelectCount<UsersEntity>(where, out count);
                                if (result.IsSuccess && count > 0)
                                {
                                    result = new Result(false, "此行員帳號已存在", ErrorCode.D_DATA_EXISTS, null);
                                }
                            }
                            else
                            {
                                Expression where = new Expression(UsersEntity.Field.UBank, bankId)
                                    .And(UsersEntity.Field.UId, userId);
                                result = factory.SelectCount<UsersEntity>(where, out count);
                                if (result.IsSuccess && count > 0)
                                {
                                    result = new Result(false, "該單位已有此使用者帳號", ErrorCode.D_DATA_EXISTS, null);
                                }
                            }
                            #endregion

                            if (result.IsSuccess)
                            {
                                result = factory.Insert(user, out count);

                                if (result.IsSuccess && count == 0)
                                {
                                    result = new Result(false, "該使用者帳號已存在", CoreStatusCode.D_NOT_DATA_INSERT, null);
                                }
                            }
                        }
                        #endregion
                        break;
                    case ApplyKindCodeTexts.UPDATE:
                        #region 修改
                        {
                            logType = LogTypeCodeTexts.UPDATE;

                            KeyValueList parameters = new KeyValueList();

                            #region 密碼欄位
                            #region [MDY:20220530] Checkmarx 調整
                            #region [MDY:20210401] 原碼修正
                            string sqlSetPXX = null;
                            if (String.IsNullOrEmpty(newPXX)) //不更新密碼欄位
                            {
                                sqlSetPXX = String.Empty;
                            }
                            else
                            {
                                sqlSetPXX = String.Format(", [{0}] = [{1}], [{1}] = @NewPXX, [{2}] = @PXXChangeDate", UsersEntity.Field.UPXX1, UsersEntity.Field.UPXX, UsersEntity.Field.PXXChangeDate);
                                parameters.Add(new KeyValue("@NewPXX", newPXX));
                                parameters.Add(new KeyValue("@PXXChangeDate", Common.GetTWDate7(now)));
                            }
                            #endregion
                            #endregion
                            #endregion

                            string sql = String.Format(@"UPDATE [{0}] 
SET [{1}] = @ReceiveType, [{2}] = @UserName, [{3}] = @Tel, [{4}] = @Title, [{5}] = @Email, [{6}] = @LockFlag 
{7} 
, [{12}] = '0', [{13}] = '0', [{14}] = 0 
, [{15}] = @ModifyUser, [{16}] = @ModifyTime, [{17}] = @ApproveUser, [{18}] = @ApproveTime
WHERE [{8}] = @OrgUserId AND [{9}] = @OrgGroupId AND [{10}] = @OrgReceiveType AND [{11}] = @OrgBankId"
                                            , UsersEntity.TABLE_NAME            //0 (TABLE NAME)
                                            , UsersEntity.Field.URt     //1 (SET)
                                            , UsersEntity.Field.UName
                                            , UsersEntity.Field.Tel
                                            , UsersEntity.Field.Title
                                            , UsersEntity.Field.Email
                                            , UsersEntity.Field.ULock        //6
                                            , sqlSetPXX      //7
                                            , UsersEntity.Field.UId      //8 (WHERE)
                                            , UsersEntity.Field.UGrp
                                            , UsersEntity.Field.URt
                                            , UsersEntity.Field.UBank      //11
                                            , UsersEntity.Field.ProcessStatus   //12    舊學雜費寫法，不知道為什麽
                                            , UsersEntity.Field.DataStatus      //13    舊學雜費寫法，不知道為什麽
                                            , UsersEntity.Field.UNum            //14    舊學雜費寫法，不知道為什麽
                                            , UsersEntity.Field.LastModifyUser      //15
                                            , UsersEntity.Field.LastModifyTime      //16
                                            , UsersEntity.Field.LastApproveUser     //17
                                            , UsersEntity.Field.LastApproveTime);   //18
                            parameters.Add("@ReceiveType", newReceiveType);
                            parameters.Add("@UserName", userName);
                            parameters.Add("@Tel", tel);
                            parameters.Add("@Title", title);
                            parameters.Add("@Email", email);
                            parameters.Add("@LockFlag", isLocked);
                            parameters.Add("@OrgUserId", userId);
                            parameters.Add("@OrgGroupId", groupId);
                            parameters.Add("@OrgReceiveType", receiveType);
                            parameters.Add("@OrgBankId", bankId);
                            parameters.Add("@ModifyUser", flowData.ApplyUserId);
                            parameters.Add("@ModifyTime", now);
                            parameters.Add("@ApproveUser", processUserId);
                            parameters.Add("@ApproveTime", now);

                            result = factory.ExecuteNonQuery(sql, parameters, out count);

                            if (result.IsSuccess && count == 0)
                            {
                                result = new Result(false, "無使用者資料被更新", CoreStatusCode.D_NOT_DATA_UPDATE, null);
                            }
                        }
                        #endregion
                        break;
                    case ApplyKindCodeTexts.DELETE:
                        #region 刪除
                        {
                            logType = LogTypeCodeTexts.DELETE;

                            KeyValueList parameters = new KeyValueList();

                            string sql = String.Format(@"DELETE [{0}] WHERE [{1}] = @UserId AND [{2}] = @GroupId AND [{3}] = @ReceiveType AND [{4}] = @BankId"
                                            , UsersEntity.TABLE_NAME        //0 (TABLE NAME)
                                            , UsersEntity.Field.UId      //1 (WHERE)
                                            , UsersEntity.Field.UGrp
                                            , UsersEntity.Field.URt
                                            , UsersEntity.Field.UBank);
                            parameters.Add("@UserId", userId);
                            parameters.Add("@GroupId", groupId);
                            parameters.Add("@ReceiveType", receiveType);
                            parameters.Add("@BankId", bankId);

                            result = factory.ExecuteNonQuery(sql, parameters, out count);

                            if (result.IsSuccess && count == 0)
                            {
                                result = new Result(false, "無使用者資料被刪除", CoreStatusCode.D_NOT_DATA_UPDATE, null);
                            }
                        }
                        #endregion
                        break;
                    default:
                        result = new Result(false, "待辦事項資料中申請種類參數不正確", ErrorCode.D_DATA_NOT_FOUND, null);
                        break;
                }
            }
            #endregion

            #region 新增日誌資料
            if (dbLogger != null)
            {
                string notation = null;
                if (result.IsSuccess)
                {
                    notation = String.Format("[{0}] {1}資料成功 (GUID={2}; GroupId={3})", funcName, LogTypeCodeTexts.GetText(logType), flowData.Guid, groupId);
                }
                else
                {
                    notation = String.Format("[{0}] {1}資料失敗 (GUID={2}; GroupId={3})，錯誤訊息：{4})", funcName, LogTypeCodeTexts.GetText(logType), flowData.Guid, groupId, result.Message);
                }
                dbLogger.AppendLog(flowData.ApplyUserQual, flowData.ApplyUnitId, funcId, logType, flowData.ApplyUserId, notation);
            }
            #endregion

            return result;
        }
    }
}
