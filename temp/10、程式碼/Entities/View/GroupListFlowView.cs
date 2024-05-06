using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 群組 + Flow Status 的 View
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public class GroupListFlowView : GroupListEntity
    {
        #region SAMPLE
//SELECT *
//     , ISNULL((SELECT TOP 1 [Status] FROM [FlowData] AS FD
//                WHERE FD.[Form_Id] = 'S5300001'
//                  AND FD.[Status] IN ('0', '2')
//                  AND FD.[Apply_Kind] IN ('U', 'D')
//                  AND FD.[Data_Key] = G.[Group_Id]), '1') AS [Flow_Status]
//  FROM [Group_List] G
//UNION
//SELECT [Data_Key] AS [Group_Id]
//     , SUBSTRING([Form_Desc], CHARINDEX('群組名稱：', [Form_Desc], 0) + 5, CHARINDEX(';', [Form_Desc], CHARINDEX('群組名稱：', [Form_Desc], 0) + 5) - (CHARINDEX('群組名稱：', [Form_Desc], 0) + 5)) AS [Group_Name]
//     , [Data_Role] AS [Role], [Data_Role_Type] AS [Role_Type], [Data_Unit_Id] AS [Branchs]
//     , [Apply_Date] AS [Create_Date], Apply_User_Id AS [Create_User], NULL AS [Modify_Date], NULL AS [Modify_User]
//     , 'I' [Status], [Status] AS [Flow_Status]
//  FROM [FlowData]
// WHERE [Form_Id] = 'S5200003' AND [Status] IN ('0', '2') AND [Apply_Kind] = 'I'
        #endregion

        protected const string VIEWSQL = @"
SELECT *
     , ISNULL((SELECT TOP 1 [" + FlowDataEntity.Field.Status + @"] FROM [" + FlowDataEntity.TABLE_NAME + @"] AS FD 
                WHERE FD.[" + FlowDataEntity.Field.FormId + @"] = '" + FormCodeTexts.S5200003 + @"'
                  AND FD.[" + FlowDataEntity.Field.Status + @"] IN ('" + FlowStatusCodeTexts.FLOWING + @"', '" + FlowStatusCodeTexts.PROCESSING + @"')
                  AND FD.[" + FlowDataEntity.Field.ApplyKind + @"] IN ('" + ApplyKindCodeTexts.UPDATE + @"', '" + ApplyKindCodeTexts.DELETE + @"')
                  AND FD.[" + FlowDataEntity.Field.DataKey + @"] = G.[" + GroupListEntity.Field.GroupId + @"]), '" + FlowStatusCodeTexts.ENDING + @"') AS [Flow_Status]
  FROM [" + GroupListEntity.TABLE_NAME + @"] AS G
UNION
SELECT [" + FlowDataEntity.Field.DataKey + @"] AS [Group_Id]
     , SUBSTRING([" + FlowDataEntity.Field.FormDesc + @"], CHARINDEX('群組名稱：', [" + FlowDataEntity.Field.FormDesc + @"], 0) + 5, CHARINDEX(';', [" + FlowDataEntity.Field.FormDesc + @"], CHARINDEX('群組名稱：', [" + FlowDataEntity.Field.FormDesc + @"], 0) + 5) - (CHARINDEX('群組名稱：', [" + FlowDataEntity.Field.FormDesc + @"], 0) + 5)) AS [Group_Name]
     , [" + FlowDataEntity.Field.DataRole + @"] AS [Role]
     , [" + FlowDataEntity.Field.DataRoleType + @"] AS [Role_Type]
     , [" + FlowDataEntity.Field.DataUnitId + @"] AS [Branchs]
     , [" + FlowDataEntity.Field.ApplyDate + @"] AS [Create_Date]
     , [" + FlowDataEntity.Field.ApplyUserId + @"] AS [Create_User]
     , NULL AS [Modify_Date], NULL AS [Modify_User], '" + ApplyKindCodeTexts.INSERT + @"' [Status]
     , [" + FlowDataEntity.Field.Status + @"] AS [Flow_Status]
  FROM [" + FlowDataEntity.TABLE_NAME + @"]
 WHERE [" + FlowDataEntity.Field.FormId + @"] = '" + FormCodeTexts.S5200003 + @"'
   AND [" + FlowDataEntity.Field.Status + @"] IN ('" + FlowStatusCodeTexts.FLOWING + @"', '" + FlowStatusCodeTexts.PROCESSING + @"')
   AND [" + FlowDataEntity.Field.ApplyKind + @"] = '" + ApplyKindCodeTexts.INSERT + @"' ";

        #region Field Name Const Class
        /// <summary>
        /// GroupListFlowView 欄位名稱定義抽象類別
        /// </summary>
        public abstract new class Field : GroupListEntity.Field
        {
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
        /// GroupListFlowView 類別建構式
        /// </summary>
        public GroupListFlowView()
            : base()
        {
        }
        #endregion

        #region Property
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
                return RoleCodeTexts.GetText(this.Role);
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
                return RoleTypeCodeTexts.GetText(this.RoleType);
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
