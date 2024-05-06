<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="eSchoolWeb.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>土地銀行 - 代收學雜費服務網</title>
<link href="/css/whole.css" rel="stylesheet" type="text/css" />
<link href="/css/index.css" rel="stylesheet" type="text/css" />
<link href="/css/flora.datepicker.css" rel="stylesheet" type="text/css"  />

<style type="text/css">
    #webTitle a, .title p, .title span, .masthead p, .masthead_s p, #channel a, #menu a, #logOut a, .button  {
        font-family: 蘋果儷中黑, 微軟正黑體, 新細明體, Arial, Helvetica, sans-serif;
    }

    html {
        display: none;
    }
</style>
<script type="text/javascript" language="javascript" src="/js/jquery-3.5.1.min.js"></script>
<script type="text/javascript" language="javascript" src="/js/ui-datepicker-1.6rc2.packed.js"></script>
<script type="text/javascript" language="javascript" src="/js/ui.datepicker-zh-TW.js"></script>
<script type="text/javascript" language="javascript">
    //if (top != self) {
    //    top.location = self.location
    //}
    if (self === top) {
        document.documentElement.style.display = 'initial';
    }
    else {
        top.location = decodeURIComponent(self.location);
    }
</script>
</head>

<body>

    <form id="form1" runat="server">
<div id="container">

    <!--begin of header--------------------------------------------------------------------------------------------------------------------------------------------->
    <div id="header">
        <uc:UCEntryPageHeader runat="server" ID="UCEntryPageHeader" />
    </div>
    <!--end of header-->

    <!--begin of content-------------------------------------------------------------------------------------------------------------------------------------------->
    <div id="bg">
        <div id="content">

            <!--begin of menu------------------------------------------------------------------>
            <div id="menu"> 
                <uc:UCEntryPageMenu runat="server" ID="UCEntryPageMenu" />
            </div>
            <!--end of menu-->

            <!--begin of main------------------------------------------------------------------>
            <div id="main">

                <!--主要內容左區----------------------------------->
                <uc:UCEntryPageAD id="ucEntryPageAD" runat="server" />
                <!--end of rightArea-->

                <!--最新消息-->
                <div class="masthead_s"><span></span><p><%= GetLocalized("最新消息") %><!--最新消息--></p></div>
                <div id="news">
                    <asp:Literal ID="litNews" runat="server"></asp:Literal>
                </div>
                <!--end of news-->
            </div>
            <!--end of main-->

        </div><!--end of content-->
    </div><!--end of bg-->

    <uc:UCFooter runat="server" ID="UCFooter" />

</div>
    </form>
</body>

<script type="text/javascript" language="javascript">
    $(function () {
        var lastDate = new Date('2018/10/31');
        if (Date.now() < lastDate) {
            var width = 510, height = 300;
            var position = '';
            if ('screenX' in window) {
                var top = window.screenY + ((window.outerHeight - height) / 2);
                var left = window.screenX + ((window.outerWidth - width) / 2);
                position = 'top=' + top + ',left=' + left + ',';
            } else if ('screenLeft' in window) {
                var top = window.screenTop + ((window.outerHeight - height) / 2);
                var left = window.screenLeft + ((window.outerWidth - width) / 2);
                position = 'top=' + top + ',left=' + left + ',';
            }

            window.open('Popup.html', 'popup', 'width=' + width + ', height = ' + height + ',' + position + 'status=no,toolbar==no,location=no')
        }
    })
</script>
</html>