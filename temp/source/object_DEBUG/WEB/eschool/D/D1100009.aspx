<%@ Page Title="土地銀行 - 代收學雜費服務網 - 身分註記代碼" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1100009.aspx.cs" Inherits="eSchoolWeb.D.D1100009" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
<style type="text/css">
html { display: none; }
</style>
<script type="text/javascript" language="javascript">
if (self === top) {
    document.documentElement.style.display = 'block';
}
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" ReceiveVisible="false" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" />
<table class="information">
<tr>
	<td>
		<%= GetLocalized("身分註記") %>：<asp:DropDownList ID="ddlIdentifyType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlIdentifyType_SelectedIndexChanged"></asp:DropDownList>
	</td>
</tr>
</table>

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
		<cc:MyBoundField DataField="IdentifyId" LocationHeaderText="身分註記代碼">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="IdentifyName" LocationHeaderText="身分註記名稱">
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
