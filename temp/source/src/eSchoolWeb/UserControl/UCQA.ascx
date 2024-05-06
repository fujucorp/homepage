<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCQA.ascx.cs" Inherits="eSchoolWeb.UserControl.UCQA" %>

<style type="text/css">
ul, li {
	margin: 0;
	padding: 0;
	list-style: none;
}
.abgne_tab {
	clear: left;
	width: 100%;/*400px;*/
	margin: 10px 0;
}
ul.tabs {
	width: 100%;
	height: 32px;
	border-bottom: 1px solid #999;
	border-left: 1px solid #999;
}
ul.tabs li {
	float: left;
	height: 31px;
	line-height: 31px;
	overflow: hidden;
	position: relative;
	margin-bottom: -1px;	/* 讓 li 往下移來遮住 ul 的部份 border-bottom */
	border: 1px solid #999;
	border-left: none;
	background: #e1e1e1;
}
ul.tabs li a {
	display: block;
	padding: 0 20px;
	color: #000;
	border: 1px solid #fff;
	text-decoration: none;
}
ul.tabs li a:hover {
	background: #ccc;
}
ul.tabs li.active  {
	background: #fff;
	border-bottom: 1px solid#fff;
}
ul.tabs li.active a:hover {
	background: #fff;
}
div.tab_container {
	clear: left;
	width: 100%;
	border: 1px solid #999;
	border-top: none;
	background: #fff;
}
div.tab_container .tab_content {
	padding: 20px;
}
div.tab_container .tab_content h2 {
	margin: 0 0 20px;
}
</style>
<style type="text/css">
html { display: none; }
</style>
<script type="text/javascript" language="javascript">
    if (self === top) {
        document.documentElement.style.display = 'block';
    }
</script>
<script type="text/javascript" language="javascript" >
    $(function () {
        // 預設顯示第一個 Tab
        var activeIndex = 0;
        $('ul.tabs li').eq(activeIndex).addClass('active');

        //顯示對應的 Content
        $('.tab_container .tab_content').eq(activeIndex).siblings().hide();

        // 當 li 頁籤被點擊時...
        // 若要改成滑鼠移到 li 頁籤就切換時, 把 click 改成 mouseover
        $('ul.tabs li').click(function () {
            // 把目前點擊到的 li 頁籤加上 .active
            // 並把兄弟元素中有 .active 的都移除 class
            $(this).addClass('active').siblings('.active').removeClass('active');

            //找出 tab 有 active 的索引值
            var activeIndex = 0;
            $('ul.tabs li').each(function (index) {
                if ($(this).hasClass('active')) {
                    activeIndex = index;
                    return false;
                }
            })

            // 淡入相對應的內容並隱藏兄弟元素
            $('.tab_container .tab_content').eq(activeIndex).stop(false, true).fadeIn().siblings().hide();

            return false;
        }).find('a').focus(function () {
            this.blur();
        });
    });
</script>

<asp:Literal ID="Literal1" runat="server"></asp:Literal>

