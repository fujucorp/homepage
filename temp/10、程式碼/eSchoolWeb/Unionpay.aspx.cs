using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;
using System.Text;
using Entities;

namespace eSchoolWeb
{
    public partial class Unionpay : LocalizedPage // System.Web.UI.Page, IMenuPage
    {
        #region Override LocalizedPage's IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "Unionpay";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "Unionpay";
            }
        }

        /// <summary>
        /// 取得是否為編輯頁面
        /// </summary>
        public override bool IsEditPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得是否為延伸頁面
        /// </summary>
        public override bool IsSubPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得選單(功能)代碼是否符合命名規則
        /// </summary>
        public override bool IsMatchMenuID
        {
            get
            {
                return false;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            UCPageNews.PageId = BoardTypeCodeTexts.UNITCARD;

        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {

        }
    }
}