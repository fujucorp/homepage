<%@ Page Title="土地銀行 - 代收學雜費服務網 - 代收費用檔" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1200001.aspx.cs" Inherits="eSchoolWeb.D.D1200001" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" ReceiveDefaultKind="All" ReceiveDefaultMode="ByKind" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" />

<br/>
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
	OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender">
	<Columns>
		<cc:MyBoundField DataField="ReceiveId" LocationHeaderText="代收費用別代碼">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="ReceiveName" LocationHeaderText="代收費用別名稱">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="CreditItem" LocationHeaderText="課程所屬收入科目">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="BillFormName" LocationHeaderText="繳費單模板">
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
