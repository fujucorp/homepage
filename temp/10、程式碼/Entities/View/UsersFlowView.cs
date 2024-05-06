using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 使用者 + Flow Status 的 View
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class UsersFlowView : UsersEntity
    {
        #region 不含審核中的新增使用者
//        protected const string VIEWSQL = @"
//SELECT U.*
//     , ISNULL(G.[" + GroupListEntity.Field.Role + @"], '') AS [Group_Role], ISNULL(G.[" + GroupListEntity.Field.RoleType + @"], '') AS [Group_RoleType]
//     , ISNULL((SELECT TOP 1 [" + FlowDataEntity.Field.Status + @"] FROM [" + FlowDataEntity.TABLE_NAME + @"] AS FD
//                WHERE FD.[" + FlowDataEntity.Field.FormId + @"] = 'S5300001'
//                  AND FD.[" + FlowDataEntity.Field.Status + @"] IN ('" + FlowStatusCodeTexts.FLOWING + @"', '" + FlowStatusCodeTexts.PROCESSING + @"')
//                  AND FD.[" + FlowDataEntity.Field.ApplyKind + @"] IN ('" + ApplyKindCodeTexts.UPDATE + @"', '" + ApplyKindCodeTexts.DELETE + @"')
//                  AND FD.[" + FlowDataEntity.Field.DataKey + @"] = (U.[" + UsersEntity.Field.UId + @"] + ',' + U.[" + UsersEntity.Field.UGrp + @"] + ',' + U.[" + UsersEntity.Field.UBank + @"])), '" + FlowStatusCodeTexts.ENDING + @"') AS [Flow_Status]
//  FROM [" + UsersEntity.TABLE_NAME + @"] AS U 
//  LEFT JOIN [" + GroupListEntity.TABLE_NAME + @"] AS G ON G.[" + GroupListEntity.Field.GroupId + @"] = U.[" + UsersEntity.Field.UGrp + @"] ";
        #endregion

        #region 包含審核中的新增使用者
        #region [Old]
        #region 審核中的新增使用者
        //UNION
//SELECT F.* 
//     , ISNULL(G.[Role], '') AS [Group_Role], ISNULL(G.[Role_type], '') AS [Group_RoleType]
//  FROM (
//SELECT SubString(Data_Key, 0, CHARINDEX(',', Data_Key, 0)) U_ID
//     , CASE WHEN CHARINDEX('商家代號：', Data_Key, 0) = 0 THEN '' ELSE SubString(Data_Key, CHARINDEX('商家代號：', Data_Key, 0) + 5, CHARINDEX(';', Data_Key, CHARINDEX('商家代號：', Data_Key, 0)) - CHARINDEX('商家代號：', Data_Key, 0) - 5) END U_RT
//     , SubString(Data_Key, CHARINDEX(',', Data_Key, 0) + 1, CHARINDEX(',', Data_Key, CHARINDEX(',', Data_Key, 0) + 1) - CHARINDEX(',', Data_Key, 0) - 1) U_Grp
//     , Right(Data_Key, Len(Data_Key) - CHARINDEX(',', Data_Key, CHARINDEX(',', Data_Key, 0) + 1)) U_Bank
//     , SubString(Form_Desc, CHARINDEX('名稱：', Form_Desc, 0) + 3, CHARINDEX(';', Form_Desc, CHARINDEX('名稱：', Form_Desc, 0)) - CHARINDEX('名稱：', Form_Desc, 0) - 3) U_Name
//     , '' U_Pwd, '' Creator, '' Approver, '' Pwd_ChangeDate
//     , '' U_Lock, '' U_num, '' Title, '' Tel, '' EMail, '' U_Pwd1, '' sessionid, '' CreateDate, '' ApproveDate
//     , '' Reset_UID, '' ResetDate, '' ResetPWD, '' data_status, '' process_status, Form_Data remark
//     , '' last_modify_user, null last_modify_time, '' last_approve_user, null last_approve_time
//     , '' flag, '' init_pwd
//     , [Status] [Flow_Status]
//  FROM [FlowData] WHERE [Form_Id] = 'S5300001' AND [Status] IN ('0', '2') AND [Apply_Kind] = 'I' AND [Data_Key] LIKE '%,%,%' 
//) AS F
//  LEFT JOIN [Group_List] AS G ON G.[Group_Id] = F.[U_Grp]
#endregion

//        protected const string VIEWSQL = @"
//SELECT U.*
//     , ISNULL((SELECT TOP 1 [" + FlowDataEntity.Field.Status + @"] FROM [" + FlowDataEntity.TABLE_NAME + @"] AS FD
//                WHERE FD.[" + FlowDataEntity.Field.FormId + @"] = 'S5300001'
//                  AND FD.[" + FlowDataEntity.Field.Status + @"] IN ('" + FlowStatusCodeTexts.FLOWING + @"', '" + FlowStatusCodeTexts.PROCESSING + @"')
//                  AND FD.[" + FlowDataEntity.Field.ApplyKind + @"] IN ('" + ApplyKindCodeTexts.UPDATE + @"', '" + ApplyKindCodeTexts.DELETE + @"')
//                  AND FD.[" + FlowDataEntity.Field.DataKey + @"] = (U.[" + UsersEntity.Field.UId + @"] + ',' + U.[" + UsersEntity.Field.UGrp + @"] + ',' + U.[" + UsersEntity.Field.UBank + @"])), '" + FlowStatusCodeTexts.ENDING + @"') AS [Flow_Status]
//     , ISNULL(G.[" + GroupListEntity.Field.Role + @"], '') AS [Group_Role], ISNULL(G.[" + GroupListEntity.Field.RoleType + @"], '') AS [Group_RoleType]
//  FROM [" + UsersEntity.TABLE_NAME + @"] AS U 
//  LEFT JOIN [" + GroupListEntity.TABLE_NAME + @"] AS G ON G.[" + GroupListEntity.Field.GroupId + @"] = U.[" + UsersEntity.Field.UGrp + @"]
//UNION
//SELECT F.* 
//	 , ISNULL(G.[Role], '') AS [Group_Role], ISNULL(G.[Role_type], '') AS [Group_RoleType]
//  FROM (
//SELECT SubString(" + FlowDataEntity.Field.DataKey + @", 0, CHARINDEX(',', " + FlowDataEntity.Field.DataKey + @", 0)) U_ID
//     , CASE WHEN CHARINDEX('商家代號：', " + FlowDataEntity.Field.DataKey + @", 0) = 0 THEN '' ELSE SubString(" + FlowDataEntity.Field.DataKey + @", CHARINDEX('商家代號：', " + FlowDataEntity.Field.DataKey + @", 0) + 5, CHARINDEX(';', " + FlowDataEntity.Field.DataKey + @", CHARINDEX('商家代號：', " + FlowDataEntity.Field.DataKey + @", 0)) - CHARINDEX('商家代號：', " + FlowDataEntity.Field.DataKey + @", 0) - 5) END U_RT
//     , SubString(" + FlowDataEntity.Field.DataKey + @", CHARINDEX(',', " + FlowDataEntity.Field.DataKey + @", 0) + 1, CHARINDEX(',', " + FlowDataEntity.Field.DataKey + @", CHARINDEX(',', " + FlowDataEntity.Field.DataKey + @", 0) + 1) - CHARINDEX(',', " + FlowDataEntity.Field.DataKey + @", 0) - 1) U_Grp
//     , Right(" + FlowDataEntity.Field.DataKey + @", Len(" + FlowDataEntity.Field.DataKey + @") - CHARINDEX(',', " + FlowDataEntity.Field.DataKey + @", CHARINDEX(',', " + FlowDataEntity.Field.DataKey + @", 0) + 1)) U_Bank
//     , SubString(" + FlowDataEntity.Field.FormDesc + @", CHARINDEX('名稱：', " + FlowDataEntity.Field.FormDesc + @", 0) + 3, CHARINDEX(';', " + FlowDataEntity.Field.FormDesc + @", CHARINDEX('名稱：', " + FlowDataEntity.Field.FormDesc + @", 0)) - CHARINDEX('名稱：', " + FlowDataEntity.Field.FormDesc + @", 0) - 3) U_Name
//     , '' U_Pwd, '' Creator, '' Approver, '' Pwd_ChangeDate
//     , '' U_Lock, '' U_num, '' Title, '' Tel, '' EMail, '' U_Pwd1, '' sessionid, '' CreateDate, '' ApproveDate
//     , '' Reset_UID, '' ResetDate, '' ResetPWD, '' data_status, '' process_status, Form_Data remark
//     , '' last_modify_user, null last_modify_time, '' last_approve_user, null last_approve_time
//     , '' flag, '' init_pwd
//     , [" + FlowDataEntity.Field.Status + @"] [Flow_Status]
//  FROM [" + FlowDataEntity.TABLE_NAME + @"] WHERE [" + FlowDataEntity.Field.FormId + @"] = 'S5300001' AND [" + FlowDataEntity.Field.Status + @"] IN ('0', '2') AND [" + FlowDataEntity.Field.ApplyKind + @"] = 'I' AND [" + FlowDataEntity.Field.DataKey + @"] LIKE '%,%,%'
//) AS F
        //  LEFT JOIN [Group_List] AS G ON G.[Group_Id] = F.[U_Grp] ";
        #endregion

        #region 審核中的新增使用者 SAMPLE
//UNION
//SELECT SubString(Data_Key, 0, CHARINDEX(',', Data_Key, 0)) U_ID
        //     , CASE WHEN CHARINDEX('商家代號：', Form_Desc, 0) = 0 THEN '' ELSE SubString(Data_Key, CHARINDEX('商家代號：', Form_Desc, 0) + 5, CHARINDEX(';', Form_Desc, CHARINDEX('商家代號：', Form_Desc, 0)) - CHARINDEX('商家代號：', Form_Desc, 0) - 5) END U_RT
//     , SubString(Data_Key, CHARINDEX(',', Data_Key, 0) + 1, CHARINDEX(',', Data_Key, CHARINDEX(',', Data_Key, 0) + 1) - CHARINDEX(',', Data_Key, 0) - 1) U_Grp
//     , Right(Data_Key, Len(Data_Key) - CHARINDEX(',', Data_Key, CHARINDEX(',', Data_Key, 0) + 1)) U_Bank
//     , SubString(Form_Desc, CHARINDEX('名稱：', Form_Desc, 0) + 3, CHARINDEX(';', Form_Desc, CHARINDEX('名稱：', Form_Desc, 0)) - CHARINDEX('名稱：', Form_Desc, 0) - 3) U_Name
//     , '' U_Pwd, '' Creator, '' Approver, '' Pwd_ChangeDate
//     , '' U_Lock, '' U_num, '' Title, '' Tel, '' EMail, '' U_Pwd1, '' sessionid, '' CreateDate, '' ApproveDate
//     , '' Reset_UID, '' ResetDate, '' ResetPWD, '' data_status, '' process_status, Form_Data remark
//     , '' last_modify_user, null last_modify_time, '' last_approve_user, null last_approve_time
//     , '' flag, '' init_pwd
//     , Data_Role AS [Group_Role], Data_Role_Type AS [Group_RoleType]
//     , [Status] [Flow_Status]
//  FROM [FlowData] WHERE [Form_Id] = 'S5300001' AND [Status] IN ('0', '2') AND [Apply_Kind] = 'I' AND [Data_Key] LIKE '%,%,%' 
        #endregion

        protected const string VIEWSQL = @"
SELECT U.*
     , ISNULL(G.[" + GroupListEntity.Field.Role + @"], '') AS [Group_Role], ISNULL(G.[" + GroupListEntity.Field.RoleType + @"], '') AS [Group_RoleType]
     , ISNULL((SELECT TOP 1 [" + FlowDataEntity.Field.Status + @"] FROM [" + FlowDataEntity.TABLE_NAME + @"] AS FD
                WHERE FD.[" + FlowDataEntity.Field.FormId + @"] = '" + FormCodeTexts.S5300001 + @"'
                  AND FD.[" + FlowDataEntity.Field.Status + @"] IN ('" + FlowStatusCodeTexts.FLOWING + @"', '" + FlowStatusCodeTexts.PROCESSING + @"')
                  AND FD.[" + FlowDataEntity.Field.ApplyKind + @"] IN ('" + ApplyKindCodeTexts.UPDATE + @"', '" + ApplyKindCodeTexts.DELETE + @"')
                  AND FD.[" + FlowDataEntity.Field.DataKey + @"] = (U.[" + UsersEntity.Field.UId + @"] + ',' + U.[" + UsersEntity.Field.UGrp + @"] + ',' + U.[" + UsersEntity.Field.UBank + @"])), '" + FlowStatusCodeTexts.ENDING + @"') AS [Flow_Status]
  FROM [" + UsersEntity.TABLE_NAME + @"] AS U 
  LEFT JOIN [" + GroupListEntity.TABLE_NAME + @"] AS G ON G.[" + GroupListEntity.Field.GroupId + @"] = U.[" + UsersEntity.Field.UGrp + @"]
UNION
SELECT SubString([" + FlowDataEntity.Field.DataKey + @"], 0, CHARINDEX(',', [" + FlowDataEntity.Field.DataKey + @"], 0)) U_ID
     , CASE WHEN CHARINDEX('商家代號：', [" + FlowDataEntity.Field.FormDesc + @"], 0) = 0 THEN '' ELSE SubString([" + FlowDataEntity.Field.FormDesc + @"], CHARINDEX('商家代號：', [" + FlowDataEntity.Field.FormDesc + @"], 0) + 5, CHARINDEX(';', [" + FlowDataEntity.Field.FormDesc + @"], CHARINDEX('商家代號：', [" + FlowDataEntity.Field.FormDesc + @"], 0)) - CHARINDEX('商家代號：', [" + FlowDataEntity.Field.FormDesc + @"], 0) - 5) END U_RT
     , SubString(" + FlowDataEntity.Field.DataKey + @", CHARINDEX(',', " + FlowDataEntity.Field.DataKey + @", 0) + 1, CHARINDEX(',', " + FlowDataEntity.Field.DataKey + @", CHARINDEX(',', " + FlowDataEntity.Field.DataKey + @", 0) + 1) - CHARINDEX(',', " + FlowDataEntity.Field.DataKey + @", 0) - 1) U_Grp
     , Right([" + FlowDataEntity.Field.DataKey + @"], Len([" + FlowDataEntity.Field.DataKey + @"]) - CHARINDEX(',', [" + FlowDataEntity.Field.DataKey + @"], CHARINDEX(',', [" + FlowDataEntity.Field.DataKey + @"], 0) + 1)) U_Bank
     , SubString([" + FlowDataEntity.Field.FormDesc + @"], CHARINDEX('名稱：', [" + FlowDataEntity.Field.FormDesc + @"], 0) + 3, CHARINDEX(';', [" + FlowDataEntity.Field.FormDesc + @"], CHARINDEX('名稱：', [" + FlowDataEntity.Field.FormDesc + @"], 0)) - CHARINDEX('名稱：', [" + FlowDataEntity.Field.FormDesc + @"], 0) - 3) U_Name
     , '' U_Pwd, '' Creator, '' Approver, '' Pwd_ChangeDate
     , '' U_Lock, '' U_num, '' Title, '' Tel, '' EMail, '' U_Pwd1, '' sessionid, '' CreateDate, '' ApproveDate
     , '' Reset_UID, '' ResetDate, '' ResetPWD, '' data_status, '' process_status, Form_Data remark
     , '' last_modify_user, null last_modify_time, '' last_approve_user, null last_approve_time
     , '' flag, '' init_pwd
     , [" + FlowDataEntity.Field.DataRole + @"] AS [Group_Role], [" + FlowDataEntity.Field.DataRoleType + @"] AS [Group_RoleType]
     , [" + FlowDataEntity.Field.Status + @"] [Flow_Status]
  FROM [" + FlowDataEntity.TABLE_NAME + @"]
 WHERE [" + FlowDataEntity.Field.FormId + @"] = '" + FormCodeTexts.S5300001 + @"'
   AND [" + FlowDataEntity.Field.Status + @"] IN ('" + FlowStatusCodeTexts.FLOWING + @"', '" + FlowStatusCodeTexts.PROCESSING + @"')
   AND [" + FlowDataEntity.Field.ApplyKind + @"] = '" + ApplyKindCodeTexts.INSERT + @"'
   AND [" + FlowDataEntity.Field.DataKey + @"] LIKE '%,%,%' ";
        #endregion

        #region Field Name Const Class
        /// <summary>
        /// UsersView 欄位名稱定義抽象類別
        /// </summary>
        public abstract new class Field : UsersEntity.Field
        {
            #region Group Data
            /// <summary>
            /// 群組角色 (1=行員 / 2=學校，請參考 RoleCodeTexts)
            /// </summary>
            public const string GroupRole = "Group_Role";

            /// <summary>
            /// 群組權限角色。(3=主管 / 2=經辦，請參考 RoleTypeCodeTexts)
            /// </summary>
            public const string GroupRoleType = "Group_RoleType";
            #endregion

            #region Flow Data
            /// <summary>
            /// 流程狀態 (0=待覆核 / 1=已覆核 / 2=處理中) (請參考 FlowStatusCodeTexts)
            /// </summary>
            public const string FlowStatus = "Flow_Status";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// UsersFlowView 類別建構式
        /// </summary>
        public UsersFlowView()
            : base()
        {
        }
        #endregion

        #region Property
        #region Group Data
        /// <summary>
        /// 群組角色 (1=行員 / 2=學校，請參考 RoleCodeTexts)
        /// </summary>
        [FieldSpec(Field.GroupRole, false, FieldTypeEnum.Char, 1, false)]
        public string GroupRole
        {
            get;
            set;
        }

        /// <summary>
        /// 群組權限角色。(3=主管 / 2=經辦，請參考 RoleTypeCodeTexts)
        /// </summary>
        [FieldSpec(Field.GroupRoleType, false, FieldTypeEnum.Char, 1, false)]
        public string GroupRoleType
        {
            get;
            set;
        }
        #endregion

        #region Flow Data
        /// <summary>
        /// 流程狀態 (0=待覆核 / 1=已覆核 / 2=處理中) (請參考 FlowStatusCodeTexts)
        /// </summary>
        [FieldSpec(Field.FlowStatus, false, FieldTypeEnum.VarChar, 5, false)]
        public string FlowStatus
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region Readonly Property
        #region Group Data
        /// <summary>
        /// 群組角色文字
        /// </summary>
        [XmlIgnore]
        public string GroupRoleText
        {
            get
            {
                return RoleCodeTexts.GetText(this.GroupRole);
            }
        }

        /// <summary>
        /// 群組權限角色文字
        /// </summary>
        [XmlIgnore]
        public string GroupRoleTypeText
        {
            get
            {
                return RoleTypeCodeTexts.GetText(this.GroupRoleType);
            }
        }
        #endregion

        #region Flow Data
        /// <summary>
        /// 流程狀態文字
        /// </summary>
        [XmlIgnore]
        public string FlowStatusText
        {
            get
            {
                return FlowStatusCodeTexts.GetText(this.FlowStatus);
            }
        }
        #endregion
        #endregion
    }
}
