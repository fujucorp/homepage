<%@ Page Title="土地銀行 - 代收學雜費服務網 - 系統訊息公告(最新消息)" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600007.aspx.cs" Inherits="eSchoolWeb.S.S5600007" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<table id="condition" class="information" summary="查詢條件" width="100%">
<tr id="trAccountDateRange">
    <td><%=this.GetLocalized("公告日期") %>：</td>
    <td>
        <div align="left">
            <asp:TextBox ID="tbxStartDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox> ～
            <asp:TextBox ID="tbxEndDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
        </div>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
</div>

<table class="information">
<tr align="right">
	<td>
		<cc:MyInsertButton ID="ccbtnInsert" runat="server" CssClass="btn" OnClick="ccbtnInsert_Click"></cc:MyInsertButton>
	</td>
</tr>
</table>

<div id="divResult" runat="server">
	<asp:GridView ID="gvResult" runat="server" CssClass="modify"
		AutoGenerateColumns="false" AllowPaging="false"
		RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
		OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender"
		EmptyDataText="<%$ Resources:Localized, 查無資料 %>" >
		<Columns>
			<cc:MyBoundField DataField="BoardSubject" LocationHeaderText="公告主旨">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="StartDate" LocationHeaderText="公告日期" DataFormatString="{0:yyyy/MM/dd}">
				<HeaderStyle Wrap="False"></HeaderStyle>
				<ItemStyle Wrap="False"></ItemStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="EndDate" LocationHeaderText="有效日期" DataFormatString="{0:yyyy/MM/dd}">
				<HeaderStyle Wrap="False"></HeaderStyle>
				<ItemStyle Wrap="False"></ItemStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField ItemStyle-Width="50px" LocationHeaderText="修改">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<cc:MyModifyButton ID="ccbtnModify" runat="server" CssClass="btn"></cc:MyModifyButton>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField ItemStyle-Width="50px" LocationHeaderText="刪除">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<cc:MyDeleteButton ID="ccbtnDelete" runat="server" CssClass="btn"></cc:MyDeleteButton>
				</ItemTemplate>
			</cc:MyTemplateField>
		</Columns>
	</asp:GridView>
</div>

<div class="button">
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
