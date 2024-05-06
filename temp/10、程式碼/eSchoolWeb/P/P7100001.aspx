<%@ Page Title="土地銀行 - 代收學雜費服務網 - 個人資料修改" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="P7100001.aspx.cs" Inherits="eSchoolWeb.P.P7100001" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<br/>
<table id="result" class="result" summary="密碼變更" width="100%">
<tr>
	<th style="width:25%"><%= GetLocalized("舊的密碼") %>：</th>
	<td>
		<asp:TextBox ID="tbxOldPXX" runat="server" MaxLength="20" TextMode="Password"></asp:TextBox>
	</td>
</tr>
<tr>
	<th><%= GetLocalized("新的密碼")%>：</th>
	<td>
		<asp:TextBox ID="tbxNewPXX" runat="server" MaxLength="20" TextMode="Password"></asp:TextBox>
		<br /><span style="color: Red; font-size:smaller;"><%= GetLocalized("8 ~ 20 碼英數字混合（同時含英文與數字），且不可含連續3個(或以上)相同或連號的英文或數字，且不可與帳號相同")%></span>
	</td>
</tr>
<tr>
	<th><%= GetLocalized("確認新密碼")%>：</th>
	<td>
		<asp:TextBox ID="tbxChkPXX" runat="server" MaxLength="20" TextMode="Password"></asp:TextBox>
	</td>
</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server" Visible="false"></cc:MyGoBackButton>
</div>
</asp:Content>
