<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePWD.aspx.cs" Inherits="eSchoolWeb.ChangePWD" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>土地銀行_代收學雜費服務網</title>
<link href="/css/whole.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    #webTitle a, .title p, .masthead p, .button  {
        font-family: 蘋果儷中黑, 微軟正黑體, 新細明體, Arial, Helvetica, sans-serif;
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
        top.location = '/ChangePWD.aspx';
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
            <div style="clear:initial; margin-left:30%">
                <table style="width:500px;">
                <tr style="height:30px;">
                    <th style="font-size:20px; text-align:center;" colspan="2"><%= GetLocalized("變更密碼") %></th>
                </tr>
                <tr style="height:30px;">
                    <th style="width:30%; text-align:right;"><%= GetLocalized("統一編號") %>：</th>
                    <td style="text-align:left;"><asp:Label ID="labUnitID" runat="server"></asp:Label></td>
                </tr>
                <tr style="height:30px;">
                    <th style="text-align:right;"><%= GetLocalized("使用者帳號") %>：</th>
                    <td style="text-align:left;"><asp:Label ID="labUserID" runat="server"></asp:Label></td>
                </tr>
                <tr style="height:30px;">
                    <th style="text-align:right;"><%= GetLocalized("舊的密碼") %>：</th>
                    <td style="text-align:left;"><asp:TextBox ID="tbxOldPXX" runat="server" MaxLength="14" TextMode="Password"></asp:TextBox></td>
                </tr>
                <tr style="height:30px;">
                    <th style="text-align:right;"><%= GetLocalized("新的密碼") %>：</th>
                    <td style="text-align:left;">
                        <asp:TextBox ID="tbxNewPXX" runat="server" MaxLength="14" TextMode="Password"></asp:TextBox>
                        &nbsp;<span style="color: Red">(<%= GetLocalized("6 ~ 8 碼英數字")%>)</span>
                    </td>
                </tr>
                <tr style="height:30px;">
                    <th style="text-align:right;"><%= GetLocalized("確認新密碼") %>：</th>
                    <td style="text-align:left;"><asp:TextBox ID="tbxChkPXX" runat="server" MaxLength="14" TextMode="Password"></asp:TextBox></td>
                </tr>
                <tr style="height:30px;">
                    <td class="button" colspan="2">
                        <asp:LinkButton ID="lbtnOK" runat="server" CssClass="btn" OnClick="lbtnOK_Click" >確定</asp:LinkButton>
                    </td>
                </tr>
                </table>
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
