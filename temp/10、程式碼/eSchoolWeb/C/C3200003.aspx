<%@ Page Title="土地銀行 - 代收學雜費服務網 - 學校自收整批上傳結果查詢" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3200003.aspx.cs" Inherits="eSchoolWeb.C.C3200003" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<!--//查詢條件----------------------------------------------------------------->
<table id="condition" class="information" summary="查詢條件" width="100%">
<tr>
	<td>
		<%=this.GetLocalized("商家代號") %>：<asp:Label ID="lbReceiveType" runat="server" Text=""></asp:Label>
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
			<cc:MyBoundField LocationHeaderText="使用者帳號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="LogDate" LocationHeaderText="操作時間">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="Notation" LocationHeaderText="說明">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
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
