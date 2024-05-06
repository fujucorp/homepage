<%@ Page Title="土地銀行 - 代收學雜費服務網 - 住宿收費標準" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1300007M.aspx.cs" Inherits="eSchoolWeb.D.D1300007M" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" YearVisible="true" TermVisible="true" AutoGetDataBound="true" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Label" />

<!--//表格_修改----------------------------------------------------------------->
<table id="condition" class="condition" summary="表格_修改" width="100%">
    <tr>
		<th><cc:MyLabel ID="MyLabel1" runat="server" LocationText="住宿代碼" />：</th>
        <td><asp:DropDownList ID="ddlDormId" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
		<th><cc:MyLabel ID="MyLabel2" runat="server" LocationText="住宿金額" />：</th>
        <td><asp:TextBox ID="tbxDormAmount" Text="0" runat="server"></asp:TextBox></td>
    </tr>
	<tr>
		<th><cc:MyLabel ID="MyLabel3" runat="server" LocationText="住宿所屬收入科目" />：</th>
        <td><asp:DropDownList ID="ddlReceiveItem" runat="server"></asp:DropDownList></td>
    </tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
