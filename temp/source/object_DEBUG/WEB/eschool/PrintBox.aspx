<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintBox.aspx.cs" Inherits="eSchoolWeb.PrintBox" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>土地銀行 - 代收學雜費服務網</title>
<link type="text/css" rel="stylesheet" href="/css/whole.css" />
<link type="text/css" rel="stylesheet" href="/css/content.css" />
<script type="text/javascript" language="javascript" src="/js/jquery-3.5.1.min.js"></script>

<style type="text/css">
#webTitle {
	display:block;
	padding: 22px 0px 10px 3px;
	font-size: 17pt;
	color:#000000;
	font-weight: bold;
	text-decoration:none;
	letter-spacing: -1px;
	}
#main {
	padding: 0 0 20px 0;
	margin:0px;
	}
html {
    display: none;
}
</style>
<script  type="text/javascript">
    if (self === top) {
        document.documentElement.style.display = 'initial';
    }
    else {
        top.location = '/PrintBox.aspx';
    }
</script>
</head>

<body>
    <form id="form1" runat="server">

<div id="container">
    <div id="logo"><asp:Image ID="imgLogo" runat="server" ImageUrl="/img/logo.gif" Width="169" Height="34" ToolTip="logo" /></div>
    <div id="webTitle"><cc:MyLiteral ID="litWebTitle" runat="server" ResourceKey="WebTitle" Text="代收學雜費服務網" /></div>

    <!--begin of bg---------------------------------------------------------------------------->
    <div id="bg" >
        <!--begin of content------------------------------------------------------------------->
        <div id="content">

<table style="width:98%; height:100%;" border="0" cellspacing="0" cellpadding="0" summary="這是一個讓畫面伸縮的排版用表格" >
<tr>
	<td>
		<table id="pageLabel" style="width:100%; height:100%;" border="0" cellspacing="0" cellpadding="0" summary="這是一個控制網頁頭、尾及主要內容的排版用表格">
	
		<tr class="container">
			<td >&nbsp;</td>
			<td height="100%">
				<!--//主要內容------------------------------------------------------------------------------------------------------------------>
				<div>
                    <!--
					<h1>系統名稱：代收學雜費服務網</h1>
					<div >
						<table class="resultPrintHead" summary="結果頁的表格頁首">
						<tr><td>列印人員：<%= LogonUserName %></td></tr>
						<tr>
							<td>列印日期：<%= DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") %></td>
						</tr>
						</table>
					</div>
                    -->
					<div class="PrintContent">

					</div>
					<!--
					<table class="resultPrintFoot" summary="結果頁的表格頁尾">
					<tr>
						<td width="200" nowrap>經辦：　　　　　　</td>
					<td class="R" width="200" nowrap>主管：　　　　　　</td>
					</tr>
					</table>
					-->

				</div>
				<!--\\主要內容------------------------------------------------------------------------------------------------------------------>
			</td>
			<td class="rightSideBg">&nbsp;</td>
		</tr>
		</table>
	</td>
</tr>
</table>

            <!--begin of footer---------------------------------------------------------------->
            <uc:UCFooter ID="ucFooter" runat="server" />
            <!--end of footer-->
        </div>
        <!--end of content-->
    </div>
    <!--end of bg-->
</div>

    </form>

<script type="text/javascript" language="javascript">
	$(document).ready(function() {
		<%
		string pSelector = Request.QueryString["P"] == null ? string.Empty : HttpUtility.HtmlEncode(Request.QueryString["P"].Trim());
		if(pSelector.Length == 0)
		{
			pSelector = ".form.print";
		}
		%>
		var selector = '<%=pSelector %>';
		if (selector != '') {
			var jqObj = window.opener.$(selector);
			if(jqObj.length > 0) {
				//$('.PrintContent').html(jqObj.outerHTML());
				$('.PrintContent').html(jqObj[0].outerHTML);
				$('.PrintContent').children().removeClass().addClass('result');
			}
		}
		$('#btnPrint').parent().parent().hide();
		$(':button, :submit, button, .btn').each(function () {
			$(this).remove();
		});
		$('a').each(function() {
			$(this).removeAttr("href").removeAttr("onclick");
		});
		$('*').filter(":input").each(function() {
			$(this).attr('disabled', true);
		});
		window.print();
	});
</script>
</body>
</html>
