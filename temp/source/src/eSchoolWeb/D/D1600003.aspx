<%@ Page Title="土地銀行 - 代收學雜費服務網 - 查詢中國信託繳費資料" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1600003.aspx.cs" Inherits="eSchoolWeb.D.D1600003" %>
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
<uc:Filter1 ID="ucFilter1" runat="server" ReceiveKind="" AutoPostBack="false" YearVisible="false" TermVisible="false" />

<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
    <td>
        <div align="left">中信帳務處理日區間
            <asp:TextBox ID="tbxQSDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox> ~
            <asp:TextBox ID="tbxQEDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;(<%=this.GetLocalized("最多 30 天") %>)
        </div>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
    <cc:MyLinkButton ID="ccbtnExportXls" runat="server" LocationText="匯出 Excel" OnClick="ccbtnExportXls_Click"></cc:MyLinkButton>
    <cc:MyLinkButton ID="ccbtnExportOds" runat="server" LocationText="匯出 ODF" OnClick="ccbtnExportOds_Click"></cc:MyLinkButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<div id="divResult" runat="server" >
	<div class='pageControl'>
		<uc:Paging ID="ucPaging1" runat="server"></uc:Paging>
	</div>
	<asp:GridView ID="gvResult" runat="server" CssClass="modify"
		AutoGenerateColumns="false" AllowPaging="false"
		RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
		EmptyDataText="<%$ Resources:Localized, 查無資料 %>" >
		<Columns>
			<cc:MyBoundField DataField="CancelNo" LocationHeaderText="繳款帳號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="TranferDate" LocationHeaderText="中信帳務處理日" DataFormatString="{0:yyyy/MM/dd}">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReceiveDate" LocationHeaderText="中信交易繳款日期" DataFormatString="{0:yyyy/MM/dd}">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="StuName" LocationHeaderText="學生姓名">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReceiveAmount" LocationHeaderText="學費總額" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
		</Columns>
	</asp:GridView>
	<div class='pageControl'>
		<uc:Paging ID="ucPaging2" runat="server"></uc:Paging>
	</div>
</div>
</asp:Content>
