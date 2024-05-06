<%@ Page CodePage="950" ContentType="application/x-www-form-urlencoded;charset=BIG5"  Language="C#" AutoEventWireup="true" CodeBehind="EZPosResponse.aspx.cs" Inherits="eSchoolWeb.EZPosResponse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=big5" />
<title>土地銀行_信用卡繳費</title>
<link href="/css/whole.css" rel="stylesheet" type="text/css" />
<style type="text/css">
#webTitle a, .title p, .masthead p, .button {
	font-family: 蘋果儷中黑, 微軟正黑體, 新細明體, Arial, Helvetica, sans-serif;
}
#page {
	width: 500px;
	height:300px;
	position: relative;
	margin: 0 auto;
	padding:0;
	border: 0px solid #000000;
	background: transparent url(../images/bg/page.jpg) no-repeat top left;
	overflow: auto;
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
        top.location = decodeURIComponent(self.location);
    }
</script>
</head>

<body>
    <form id="form1" runat="server">
<div id="container">
    <!--begin of header-------------------------------------------------------------------->
    <div id="header">
        <uc:UCEntryPageHeader ID="ucEntryPageHeader" runat="server" />
    </div><!--end of header-->

    <!--begin of content------------------------------------------------------------------->
    <div id="content">
        <!--begin of main------------------------------------------------------------------>
        <div  style="height:500px; width:100%; margin-top:50px;">
            <!--主要內容左區----------------------------------->

<div>
    <asp:Label ID="labResult" runat="server" ></asp:Label>
    <br />
    <input type="button" name="button" value="<%= GetLocalized("關閉視窗") %>" onclick="window.close();" />
</div>

        </div><!--end of main-->

        <!--begin of footer---------------------------------------------------------------->
        <div id="footer">
            <uc:UCFooter ID="ucFooter" runat="server" />
        </div><!--end of footer-->
    </div><!--end of content-->
</div>
    </form>
</body>
</html>
