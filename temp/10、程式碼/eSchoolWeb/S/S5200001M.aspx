<%@ Page Title="土地銀行 - 代收學雜費服務網 - 功能管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5200001M.aspx.cs" Inherits="eSchoolWeb.S.S5200001M" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
	<th width="30%"><cc:MyLabel ID="cclabFuncId" runat="server" LocationText="功能代碼"></cc:MyLabel>：</th>
	<td>
		<asp:TextBox ID="tbxFuncId" runat="server" MaxLength="8" Width="100px" OnTextChanged="tbxFuncId_TextChanged" AutoPostBack="true"></asp:TextBox>
	</td>
</tr>
<tr>
	<th><cc:MyLabel ID="cclabFuncName" runat="server" LocationText="功能名稱"></cc:MyLabel>：</th>
	<td>
		<asp:TextBox ID="tbxFuncName" runat="server" MaxLength="50" Width="80%"></asp:TextBox>
	</td>
</tr>
<tr>
	<th><cc:MyLabel ID="cclabParentId" runat="server" LocationText="父層功能"></cc:MyLabel>：</th>
	<td>
		<asp:DropDownList ID="ddlParentId" runat="server" AutoPostBack="false"></asp:DropDownList>
	</td>
</tr>
<tr>
	<th><cc:MyLabel ID="cclabFuncUrl" runat="server" LocationText="功能網頁"></cc:MyLabel>：</th>
	<td>
		<asp:TextBox ID="tbxFuncUrl" runat="server" MaxLength="100" Width="80%"></asp:TextBox>
	</td>
</tr>
<tr>
	<th><cc:MyLabel ID="cclabSortNo" runat="server" LocationText="顯示排序編號"></cc:MyLabel>：</th>
	<td>
		<asp:TextBox ID="tbxSortNo" runat="server" MaxLength="5" Width="100px"></asp:TextBox>
	</td>
</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
