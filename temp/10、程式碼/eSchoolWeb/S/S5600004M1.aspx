<%@ Page Title="土地銀行 - 代收學雜費服務網 - 維護代收管道管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600004M1.aspx.cs" Inherits="eSchoolWeb.S.S5600004M1" %>
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
			<cc:MyBoundField DataField="BarcodeId" LocationHeaderText="管道名稱">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="BarcodeId" LocationHeaderText="繳款方式">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="IncludePay" LocationHeaderText="繳費單<br />是否包含手續費">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="RCFlagName" LocationHeaderText="手續費負擔別">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ChannelCharge" DataFormatString="{0:N0}" LocationHeaderText="手續費">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="修改">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<cc:MyModifyButton ID="ccbtnModify" runat="server" CssClass="btn"></cc:MyModifyButton>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="刪除">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<cc:MyDeleteButton ID="ccbtnDelete" UseDefaultJSConfirm="false" runat="server" CssClass="btn"></cc:MyDeleteButton>
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
