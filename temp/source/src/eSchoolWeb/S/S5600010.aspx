<%@ Page Title="土地銀行 - 代收學雜費服務網 - 繳費單模板管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600010.aspx.cs" Inherits="eSchoolWeb.S.S5600010" %>
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

<!--//查詢條件----------------------------------------------------------------->
<table id="condition" class="information" summary="查詢條件" width="100%">
<tr>
	<td width="100"><%= GetLocalized("公版") %></td>
	<td>
		<asp:CheckBox ID="chkPublicOnly" runat="server" />
	</td>
</tr>
<tr>
	<td width="100"><%= GetLocalized("學校代碼") %></td>
	<td>
		<asp:TextBox ID="tbxSchIdenty" runat="server" MaxLength="4" Width="80%"></asp:TextBox>
	</td>
</tr>
<tr>
	<td width="100"><%= GetLocalized("商家代號") %></td>
	<td>
		<asp:TextBox ID="tbxReceiveType" runat="server" MaxLength="4" Width="80%"></asp:TextBox>
	</td>
</tr>
</table>

<div class="button">
	<cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
</div>

<br />

<table class="information">
<tr align="right">
	<td>
		<cc:MyInsertButton ID="ccbtnInsert" runat="server" CssClass="btn" OnClick="ccbtnInsert_Click"></cc:MyInsertButton>
	</td>
</tr>
</table>

<div id="divResult" runat="server" >
	<div class='pageControl'>
		<uc:Paging ID="ucPaging1" runat="server"></uc:Paging>
	</div>
	<asp:GridView ID="gvResult" runat="server" CssClass="modify"
		AutoGenerateColumns="false" AllowPaging="false"
		RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
		OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender"
		EmptyDataText="<%$ Resources:Localized, 查無資料 %>" >
		<Columns>
			<cc:MyBoundField DataField="BillFormId" LocationHeaderText="模板代號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="BillFormEdition" LocationHeaderText="模板類別">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="BillFormName" LocationHeaderText="模板名稱">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReceiveType" LocationHeaderText="商家代碼">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="預覽">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<asp:LinkButton ID="lbtnView" runat="server" CommandName="View" CssClass="btn">預覽</asp:LinkButton>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="修改">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<cc:MyModifyButton ID="ccbtnModify" runat="server" CssClass="btn"></cc:MyModifyButton>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="刪除">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<cc:MyDeleteButton ID="ccbtnDelete" runat="server" CssClass="btn"></cc:MyDeleteButton>
				</ItemTemplate>
			</cc:MyTemplateField>
		</Columns>
	</asp:GridView>
	<div class='pageControl'>
		<uc:Paging ID="ucPaging2" runat="server"></uc:Paging>
	</div>
</div>

<div class="button">
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
