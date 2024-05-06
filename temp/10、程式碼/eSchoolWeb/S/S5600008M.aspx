<%@ Page Title="土地銀行 - 代收學雜費服務網 - 信用卡服務銀行管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600008M.aspx.cs" Inherits="eSchoolWeb.S.S5600008M" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
	<th style="width:30%"><%= GetLocalized("發卡銀行代碼") %>：</th>
	<td>
		<asp:TextBox ID="tbxBankId" runat="server" MaxLength="3" Width="100px"></asp:TextBox>
	</td>
</tr>
<tr>
	<th><%= GetLocalized("發卡銀行名稱") %>：</th>
	<td>
		<asp:TextBox ID="tbxBankName" runat="server" MaxLength="50"></asp:TextBox>
	</td>
</tr>
<tr>
	<th><%= GetLocalized("信用卡繳費平台") %>：</th>
	<td>
		<asp:DropDownList ID="ddlAp" runat="server"></asp:DropDownList>
	</td>
</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
