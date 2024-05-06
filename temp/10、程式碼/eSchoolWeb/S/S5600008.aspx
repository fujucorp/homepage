<%@ Page Title="土地銀行 - 代收學雜費服務網 - 信用卡服務銀行管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600008.aspx.cs" Inherits="eSchoolWeb.S.S5600008" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table class="information">
<tr align="right">
	<td>
		<cc:MyInsertButton ID="ccbtnInsert" runat="server" CssClass="btn" OnClick="ccbtnInsert_Click"></cc:MyInsertButton>
	</td>
</tr>
</table>

<asp:GridView ID="gvResult" runat="server" CssClass="modify"
	AutoGenerateColumns="false" AllowPaging="false"
	RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
	OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender"
	EmptyDataText="<%$ Resources:Localized, 查無資料 %>" >
	<Columns>
		<cc:MyBoundField DataField="BankName" LocationHeaderText="發卡行">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="ApName" LocationHeaderText="信用卡繳費平台">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyTemplateField LocationHeaderText="修改">
			<ItemStyle HorizontalAlign="Center" Width="80px" />
			<ItemTemplate>
				<cc:MyModifyButton ID="ccbtnModify" runat="server" CssClass="btn"></cc:MyModifyButton>
			</ItemTemplate>
		</cc:MyTemplateField>
		<cc:MyTemplateField LocationHeaderText="刪除">
			<ItemStyle HorizontalAlign="Center" Width="80px" />
			<ItemTemplate>
				<cc:MyDeleteButton ID="ccbtnDelete" runat="server" CssClass="btn" UseDefaultJSConfirm="false"></cc:MyDeleteButton>
			</ItemTemplate>
		</cc:MyTemplateField>
	</Columns>
</asp:GridView>

<div class="button">
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
