<%@ Page Title="土地銀行 - 代收學雜費服務網 - D38資料查詢" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5400007.aspx.cs" Inherits="eSchoolWeb.S.S5400007" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!--//查詢條件----------------------------------------------------------------->
<table id="condition" class="condition" summary="查詢條件" width="100%">
<tr style="display:none">
    <th width="15%"><%= GetLocalized("工作序號") %>：</th>
    <td colspan="3">
        <asp:TextBox ID="tbxJobNo" runat="server" MaxLength="9"></asp:TextBox>
    </td>
</tr>
<tr>
    <th width="15%"><%= GetLocalized("商家代號") %>：</th>
    <td width="30%">
        <asp:DropDownList ID="ddlReceiveType" runat="server"></asp:DropDownList>
    </td>
    <th width="15%"><%= GetLocalized("學校代碼") %>：</th>
    <td width="40%">
        <asp:TextBox ID="tbxSchIdenty" runat="server" MaxLength="4"></asp:TextBox>
    </td>
</tr>
<tr>
    <th ><%= GetLocalized("虛擬帳號") %>：</th>
    <td >
        <asp:TextBox ID="tbxCancelNo" runat="server" MaxLength="16"></asp:TextBox>
    </td>
    <th ><%= GetLocalized("發動日期") %>：</th>
    <td >
        <asp:TextBox ID="tbxSDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>～
        <asp:TextBox ID="tbxEDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
</div>

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
			<cc:MyBoundField DataField="SchIdenty" LocationHeaderText="學校代碼">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReceiveType" LocationHeaderText="商家代號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="UpdFlagText" LocationHeaderText="異動註記">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReceiveAmount" LocationHeaderText="金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="StuId" LocationHeaderText="學號<br/>姓名" HtmlEncode="false">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="CrtDate" LocationHeaderText="發動時間" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd <br/>HH:mm:ss}">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="status" LocationHeaderText="狀態">
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
