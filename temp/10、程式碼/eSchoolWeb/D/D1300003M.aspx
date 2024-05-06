<%@ Page Title="土地銀行 - 代收學雜費服務網 - 學分基準收費標準" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1300003M.aspx.cs" Inherits="eSchoolWeb.D.D1300003M" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Label" />

<!--//表格_修改----------------------------------------------------------------->
<table id="condition" class="condition" summary="表格_修改" width="100%">
    <tr>
		<th><cc:MyLabel ID="MyLabel1" runat="server" LocationText="學分基準代碼" />：</th>
        <td><asp:TextBox ID="tbxCreditbId" Text="0" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
		<th><cc:MyLabel ID="MyLabel2" runat="server" LocationText="學分基準名稱" />：</th>
        <td><asp:TextBox ID="tbxCreditbName" Text="0" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
		<th><cc:MyLabel ID="MyLabel4" runat="server" LocationText="每學分基準單價" />：</th>
        <td><asp:TextBox ID="tbxCreditbCprice" Text="0" runat="server"></asp:TextBox></td>
    </tr>
	<tr>
		<th><cc:MyLabel ID="MyLabel3" runat="server" LocationText="所屬收入科目" />：</th>
        <td><asp:DropDownList ID="ddlReceiveItem" runat="server"></asp:DropDownList></td>
    </tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
