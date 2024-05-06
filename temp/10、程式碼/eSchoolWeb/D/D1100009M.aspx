<%@ Page Title="土地銀行 - 代收學雜費服務網 - 身分註記代碼" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1100009M.aspx.cs" Inherits="eSchoolWeb.D.D1100009M" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" AutoGetDataBound="false" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Label" ReceiveVisible="false" AutoGetDataBound="false" />
<table class="information">
<tr>
	<td>
		<cc:MyLabel ID="labIdentifyType" runat="server" LocationText="身分註記"></cc:MyLabel>：<asp:Label ID="labIdentifyTypeText" runat="server" ></asp:Label>
	</td>
</tr>
</table>

<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
	<th style="width:30%"><cc:MyLabel ID="labIdentifyId" runat="server" LocationText="身分註記代碼"></cc:MyLabel>：</th>
	<td>
		<asp:TextBox ID="tbxIdentifyId" runat="server" MaxLength="20" Width="80%"></asp:TextBox>
	</td>
</tr>
<tr>
	<th><cc:MyLabel ID="labIdentifyName" runat="server" LocationText="身分註記名稱"></cc:MyLabel>：</th>
	<td>
		<asp:TextBox ID="tbxIdentifyName" runat="server" MaxLength="40" Width="80%"></asp:TextBox>
	</td>
</tr>
<asp:PlaceHolder ID="phdIdentifyEName" runat="server" Visible="false">
<tr>
	<th><cc:MyLabel ID="cclabIdentifyEName" runat="server" LocationText="身分註記英文名稱"></cc:MyLabel>：</th>
	<td>
		<asp:TextBox ID="tbxIdentifyEName" runat="server" MaxLength="40" Width="80%"></asp:TextBox>
	</td>
</tr>
</asp:PlaceHolder>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyLinkButton ID="lbtnBack" runat="server" Text="離開" OnClick="lbtnBack_Click"></cc:MyLinkButton>
</div>
</asp:Content>
