<%@ Page Title="土地銀行 - 代收學雜費服務網 - 首頁功能日誌查詢" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5400015.aspx.cs" Inherits="eSchoolWeb.S.S5400015" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
<style type="text/css">
    pre.scroll {
        display: block;
        width:100%;
        max-width: 250px;
        width:250px\9;  /* IE 8 */
        min-height: 100%;
        max-height: 80px;
        overflow: auto;
    }
    td.scroll {
        overflow: auto;
    }
    .padding0 {
        padding : 0px;
    }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="condition" class="condition" summary="查詢條件" width="100%">
<tr>
    <th width="15%" style="font-size:14px;"><%= GetLocalized("日誌日期區間") %>：</th>
    <td colspan="3">
        <asp:TextBox ID="tbxQSDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>～
        <asp:TextBox ID="tbxQEDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
        &nbsp;&nbsp; (最多 30 天)
    </td>
</tr>
<tr>
    <th width="15%"><%= GetLocalized("功能") %>：</th>
    <td width="35%">
        <asp:DropDownList ID="ddlQRequestId" runat="server" OnSelectedIndexChanged="ddlQRequestId_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    </td>
    <th width="15%"><%= GetLocalized("操作") %>：</th>
    <td width="35%">
        <asp:DropDownList ID="ddlQRequestKind" runat="server"></asp:DropDownList>
    </td>
</tr>
<tr>
    <th width="15%"><%= GetLocalized("用戶 IP") %>：</th>
    <td width="35%">
        <asp:TextBox ID="tbxQClientIP" runat="server"></asp:TextBox>
    </td>
    <th width="15%"><%= GetLocalized("網站主機") %>：</th>
    <td width="35%">
        <asp:TextBox ID="tbxQWebMachine" runat="server" MaxLength="32"></asp:TextBox>
    </td>
</tr>
<tr id="trQRequestArgs" runat="server" visible="false">
    <th width="15%"><%= GetLocalized("網頁參數") %>：</th>
    <td colspan="3">
        <div id ="divQReceiveType" runat="server" visible="false" style="margin-bottom:2px;">
            <%= GetLocalized("商家代號") %>：<asp:TextBox ID="tbxQReceiveType" runat="server"></asp:TextBox>
        </div>
        <div id ="divQCancelNo" runat="server" visible="false" style="margin-bottom:2px;">
            <%= GetLocalized("虛擬帳號") %>：<asp:TextBox ID="tbxQConcelNo" runat="server"></asp:TextBox>
        </div>
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
			<cc:MyTemplateField LocationHeaderText="功能說明<br/>網站主機">
				<HeaderStyle Wrap="False"></HeaderStyle>
				<ItemTemplate>
					<pre><asp:Literal ID="litFuncInfo" runat="server" ></asp:Literal></pre>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="日誌時間<br/>狀態代碼">
				<HeaderStyle Wrap="False"></HeaderStyle>
				<ItemTemplate>
					<pre><asp:Literal ID="litTimeInfo" runat="server" ></asp:Literal></pre>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="用戶帳號<br/>用戶 IP">
				<HeaderStyle Wrap="False"></HeaderStyle>
				<ItemTemplate>
					<pre><asp:Literal ID="litUserInfo" runat="server" ></asp:Literal></pre>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="網頁參數">
				<ItemTemplate>
					<pre class="scroll"><asp:Literal ID="litRequestArgs" runat="server"></asp:Literal></pre>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="明細">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<asp:LinkButton ID="lbtnDetail" runat="server" CommandName="Detail" CssClass="btn">明細</asp:LinkButton>
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
