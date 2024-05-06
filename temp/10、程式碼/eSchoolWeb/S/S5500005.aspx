<%@ Page Title="土地銀行 - 代收學雜費服務網 - 報稅資料產生" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5500005.aspx.cs" Inherits="eSchoolWeb.S.S5500005" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
    <th><%=this.GetLocalized("商家代碼") %>：</th>
    <td>
        <asp:CheckBoxList ID="cblReceiveType" runat="server"></asp:CheckBoxList>
    </td>
</tr>
<tr>
    <th><%=this.GetLocalized("報稅年度") %>：</th>
    <td>
        <asp:TextBox ID="tbxTaxYear" runat="server" Width="50%" MaxLength="3"></asp:TextBox>
        <span style="font-size:11px">(請輸入3碼的民國年)</span>
    </td>
</tr>
<tr>
    <th><%=this.GetLocalized("學校代號") %>：<br />(<%=this.GetLocalized("長度為6碼") %>)</th>
    <td><asp:TextBox ID="tbxSchoolId" runat="server" Width="50%" MaxLength="6"></asp:TextBox></td>
</tr>
<tr>
    <th><%=this.GetLocalized("學校統編") %>：</th>
    <td><asp:TextBox ID="tbxSchoolIdenty" runat="server" Width="50%" MaxLength="8"></asp:TextBox></td>
</tr>
<tr>
    <th><%=this.GetLocalized("學制") %>：</th>
    <td>
        <asp:DropDownList ID="ddlSchLevel" runat="server">
            <asp:ListItem Value=""  Text="--請選擇--"></asp:ListItem>
            <asp:ListItem Value="2"  Text="二專"></asp:ListItem>
            <asp:ListItem Value="5"  Text="五專"></asp:ListItem>
            <asp:ListItem Value="B1"  Text="大學"></asp:ListItem>
            <asp:ListItem Value="B2"  Text="四技"></asp:ListItem>
            <asp:ListItem Value="C"  Text="二技"></asp:ListItem>
            <asp:ListItem Value="D"  Text="博士"></asp:ListItem>
            <asp:ListItem Value="M"  Text="碩士"></asp:ListItem>
        </asp:DropDownList>
	</td>
</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
