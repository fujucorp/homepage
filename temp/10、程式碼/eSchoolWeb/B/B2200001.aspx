<%@ Page Title="土地銀行 - 代收學雜費服務網 - 產生繳費金額" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="B2200001.aspx.cs" Inherits="eSchoolWeb.B.B2200001" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" />

<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
	<th>
		<div style="text-align:center">
			<%= GetLocalized("請選擇您上傳的批號") %>：
			<asp:DropDownList ID="ddlUpNo" runat="server"></asp:DropDownList>
		</div>
	</th>
</tr>
<tr style="display:none;">
	<th>
		<div style="text-align:center">
			<%= GetLocalized("代收管道") %>：
			<asp:CheckBox ID="cbxChannel0" runat="server" Enabled="false" Checked="true" /><%= GetLocalized("臨櫃") %>&nbsp;
			<asp:CheckBox ID="cbxChannel2" runat="server" Enabled="false" /><%= GetLocalized("超商") %>&nbsp;
		</div>
	</th>
</tr>
<tr>
	<th>
		<div style="text-align:center">
			<%= GetLocalized("計算方式") %>：
			<asp:RadioButton ID="rbnWay1" runat="server" GroupName="Way" /><%= GetLocalized("依收費標準") %>&nbsp;
			<asp:RadioButton ID="rbnWay2" runat="server" GroupName="Way" Checked="true" /><%= GetLocalized("依輸入金額") %>&nbsp;
		</div>
	</th>
</tr>
<tr id="trSubsidyMemo" runat="server" visible="false">
	<th>
		<div style="text-align:center; color:red">注意：此代收費有勾選【教育部補助】的收入科目，該科目的金額會清為 0</div>
	</th>
</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
