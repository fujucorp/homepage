<%@ Page Title="土地銀行 - 代收學雜費服務網 - 查閱檔案上傳結果" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1500007.aspx.cs" Inherits="eSchoolWeb.D.D1500007" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" />

<asp:GridView ID="gvResult" runat="server" CssClass="modify"
	AutoGenerateColumns="false" AllowPaging="false"
	RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
	OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender">
	<Columns>
		<cc:MyBoundField DataField="Jno" LocationHeaderText="功能代碼名稱">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="Jowner" LocationHeaderText="使用者帳號">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="CDate" LocationHeaderText="上傳日期時間">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="Jrecid" LocationHeaderText="費用別">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="SeriorNo" LocationHeaderText="批號">
			<ItemStyle Wrap ="False"></ItemStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="Jresultid" LocationHeaderText="狀態">
			<HeaderStyle Wrap="False"></HeaderStyle>
			<ItemStyle Wrap ="False"></ItemStyle>
		</cc:MyBoundField>
		<cc:MyTemplateField LocationHeaderText="明細">
			<ItemStyle HorizontalAlign="Center" Wrap ="False" />
			<ItemTemplate>
				<cc:MyLinkButton ID="cclbtnDetail" runat="server" CssClass="btn" CommandName="Detail" LocationText="明細" Visible="false" />
				<asp:LinkButton ID="lbtnDetail" runat="server" CommandName="Detail" CssClass="btn">明細</asp:LinkButton>
			</ItemTemplate>
		</cc:MyTemplateField>
	</Columns>
</asp:GridView>

<div class="button">
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
