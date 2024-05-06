<%@ Page Title="土地銀行 - 代收學雜費服務網 - 中心入帳資料查詢" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3700006.aspx.cs" Inherits="eSchoolWeb.C.C3700006" %>

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
<uc:Filter1 ID="ucFilter1" runat="server" ReceiveKind="" YearVisible="false" TermVisible="false" />

<br/>
<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
    <td>
        <div align="left"><%=this.GetLocalized("繳款方式") %>
            <asp:DropDownList ID="ddlReceiveWay" runat="server">
            </asp:DropDownList>
        </div>
    </td>
</tr>
<tr>
    <td>
        <div align="left"><%=this.GetLocalized("代收日區間") %>
            <asp:TextBox ID="tbxReceiveDateS" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>～
            <asp:TextBox ID="tbxReceiveDateE" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
        </div>
    </td>
</tr>
<tr>
    <td colspan="2">
        <div align="left"><%=this.GetLocalized("虛擬帳號") %>
            <asp:TextBox ID="tbxCancelNo" MaxLength="16" runat="server"></asp:TextBox>
        </div>
    </td>
</tr>
<tr>
    <td colspan="2">
        <div style="text-align:left">
            [匯出功能說明]：<br />
            1.匯出檔案為 EXCEL 格式 (XLS)。<br />
            2.匯出資料限制最多 65535 筆 (XLS 檔限制)，超過限制將匯出失敗。<br />
            3.匯出欄位依序為【商家代號】、【虛擬帳號】、【繳費金額】、【代收日期】、【代收時間】、【繳款方式】、【代收分行】、【更正註記】。<br />
        </div>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
    <cc:MyLinkButton ID="ccbtnExport" runat="server" LocationText="匯出XLS" CommandArgument="XLS" OnClick="ccbtnExport_Click"></cc:MyLinkButton>
    <cc:MyLinkButton ID="ccbtnExportODS" runat="server" LocationText="匯出ODS" CommandArgument="ODS" OnClick="ccbtnExport_Click"></cc:MyLinkButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<div id="divResult" runat="server" >
	<table id="tabSummary" runat="server" class="result" summary="查詢結果" width="100%">
	<tr>
		<td><%=this.GetLocalized("總筆數") %>：<asp:Label ID="labDataCount" runat="server" Text=""></asp:Label></td>
		<td><%=this.GetLocalized("總金額") %>：<asp:Label ID="labSumAmount" runat="server" Text=""></asp:Label></td>
	</tr>
	</table>

	<div class='pageControl'>
		<uc:Paging ID="ucPaging1" runat="server"></uc:Paging>
	</div>
	<asp:GridView ID="gvResult" runat="server" CssClass="modify"
		AutoGenerateColumns="false" AllowPaging="false"
		RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
		OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender"
		EmptyDataText="<%$ Resources:Localized, 查無資料 %>" >
		<Columns>
			<cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReceiveAmount" LocationHeaderText="繳費金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReceiveDateFormat" LocationHeaderText="代收日期">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReceiveTime" LocationHeaderText="代收時間">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReceiveWayName" LocationHeaderText="繳款方式">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReceiveBankName" LocationHeaderText="代收分行">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="Reserve2Text" LocationHeaderText="更正註記">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
		</Columns>
	</asp:GridView>
	<div class='pageControl'>
		<uc:Paging ID="ucPaging2" runat="server"></uc:Paging>
	</div>
</div>
</asp:Content>
