<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fisc_inbound_response.aspx.cs" Inherits="eSchoolWeb.fisc_inbound_response" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        html {
            display: none;
        }
    </style>
    <script  type="text/javascript">
        if (self === top) {
            document.documentElement.style.display = 'initial';
        }
        else {
            top.location = '/fisc_inbound_response.aspx';
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>交易回傳資料：</div>
        <table border="1" cellspacing="0" cellpadding="5">
            <tr><td style="text-align:right;">訂單編號</td><td><asp:Label ID="labOrderNumber" runat="server"></asp:Label></td></tr>
            <tr><td style="text-align:right;">交易金額</td><td><asp:Label ID="labPurchAmt" runat="server"></asp:Label></td></tr>
            <%--<tr><td style="text-align:right;">交易處理回應時間</td><td><asp:Label ID="labTransRespTime" runat="server"></asp:Label></td></tr>--%>
            <tr><td style="text-align:right;">回應碼</td><td><asp:Label ID="labResponseCode" runat="server"></asp:Label></td></tr>
            <tr><td style="text-align:right;">交易結果</td><td><asp:Label ID="labResult" runat="server"></asp:Label></td></tr>
            <tr><td style="text-align:right;">錯誤描述</td><td><asp:Label ID="labErrDesc" runat="server"></asp:Label></td></tr>
        </table>
        <br />
        <asp:Label ID="labMessage" runat="server" ViewStateMode="Disabled"></asp:Label>
    </div>
    </form>
</body>
</html>
