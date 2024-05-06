<%@ Page Title="土地銀行 - 代收學雜費服務網 - 電文日誌查詢" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5400014.aspx.cs" Inherits="eSchoolWeb.S.S5400014" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
<style type="text/css">
    pre.scroll {
        display: block;
        width:100%;
        max-width: 250px;
        width:250px\9;  /* IE 8 */
        height: 60px;
        min-height: 100%;
        max-height: 150px;
        overflow: scroll;
        white-space: nowrap;
    }
    td.scroll {
        overflow: scroll;
    }
    .padding0 {
        padding : 0px;
    }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="condition" class="condition" summary="查詢條件" width="100%">
<tr>
    <th width="15%"><%= GetLocalized("電文類別") %>：</th>
    <td width="35%">
        <asp:DropDownList ID="ddlKind" runat="server"></asp:DropDownList>
    </td>
    <th width="15%"><%= GetLocalized("伺服器名稱") %>：</th>
    <td width="35%">
        <asp:TextBox ID="tbxMachineName" runat="server" MaxLength="32"></asp:TextBox>
    </td>
</tr>
<tr>
    <th ><%= GetLocalized("發動日期") %>：</th>
    <td colspan="3">
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
			<cc:MyBoundField DataField="KindText" LocationHeaderText="電文類別">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="伺服器名稱<br/>電文 RqUID">
				<HeaderStyle Wrap="False"></HeaderStyle>
				<ItemTemplate>
					<asp:Literal ID="labMachineName" runat="server" ></asp:Literal>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="電文內容">
				<ItemTemplate>
					<pre class="scroll"><asp:Literal ID="litXml" runat="server"></asp:Literal></pre>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="回覆狀態">
				<HeaderStyle Wrap="False"></HeaderStyle>
				<ItemTemplate>
					<asp:Literal ID="labRsStatus" runat="server" ></asp:Literal>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="發動時間<br/>發動結果">
				<HeaderStyle Wrap="False"></HeaderStyle>
				<ItemTemplate>
					<asp:Literal ID="labSendResult" runat="server" ></asp:Literal>
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
