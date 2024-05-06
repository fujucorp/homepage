<%@ Page Title="土地銀行 - 代收學雜費服務網 - 交易紀錄查詢" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5400002.aspx.cs" Inherits="eSchoolWeb.S.S5400002" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<table id="condition" class="information" summary="查詢條件" width="100%">
<tr>
    <td width="100"><%=this.GetLocalized("單位代碼") %></td>
    <td>
        <asp:TextBox ID="tbxUserUnitId" MaxLength="6" runat="server"></asp:TextBox> (請輸入學校代碼 4 碼或分行代碼 6 碼)
    </td>
</tr>
<tr>
    <td><%=this.GetLocalized("使用者帳號") %></td>
    <td>
        <asp:TextBox ID="tbxUserId" MaxLength="10" runat="server"></asp:TextBox>
    </td>
</tr>
<tr>
    <td><%=this.GetLocalized("日期區間") %></td>
    <td>
        <asp:TextBox ID="tbxSDate" CssClass="datepicker" MaxLength="10" runat="server"></asp:TextBox> 至
        <asp:TextBox ID="tbxEDate" CssClass="datepicker" MaxLength="10" runat="server"></asp:TextBox>
    </td>
</tr>
<tr >
    <td><%=this.GetLocalized("功能") %></td>
    <td>
        <asp:DropDownList ID="ddlFunction" runat="server">
        </asp:DropDownList>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
</div>

<div id="divResult" runat="server">
	<div class='pageControl'>
		<uc:Paging ID="ucPaging1" runat="server"></uc:Paging>
	</div>
	<asp:GridView ID="gvResult" runat="server" CssClass="modify"
		AutoGenerateColumns="false" AllowPaging="false"
		RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
		OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender">
		<Columns>
			<cc:MyBoundField DataField="Role" LocationHeaderText="單位類別">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="UserId" LocationHeaderText="使用者帳號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="LogDate" LocationHeaderText="時間">
				<HeaderStyle Wrap="true"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="FuncName" LocationHeaderText="交易">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="Notation" LocationHeaderText="說明">
				<HeaderStyle Wrap="true"></HeaderStyle>
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
