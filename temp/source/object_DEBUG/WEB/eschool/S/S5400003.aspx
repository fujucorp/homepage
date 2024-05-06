<%@ Page Title="土地銀行 - 代收學雜費服務網 - 排程作業查詢" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5400003.aspx.cs" Inherits="eSchoolWeb.S.S5400003" %>
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
<!--//查詢條件----------------------------------------------------------------->
<table id="condition" class="information" summary="查詢條件" width="100%">
<tr>
	<td><%=this.GetLocalized("執行日期區間") %>：	
		<asp:TextBox ID="tbxDateS" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox> 至
		<asp:TextBox ID="tbxDateE" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
	</td>
</tr>
<tr>
	<td><%= GetLocalized("服務名稱") %>：
		<asp:DropDownList ID="ddlJobCubeType" runat="server"></asp:DropDownList>
	</td>
</tr>
</table>

<div class="button">
	<cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
	<cc:MyLinkButton ID="ccbtnExport" runat="server" LocationText="匯出XLS" CommandArgument="XLS" OnClick="ccbtnExport_Click"></cc:MyLinkButton>
	<cc:MyLinkButton ID="ccbtnExportODS" runat="server" LocationText="匯出ODS" CommandArgument="ODS" OnClick="ccbtnExport_Click"></cc:MyLinkButton>
</div>

<div id="divResult" runat="server">
	<div class='pageControl'>
		<uc:Paging ID="ucPaging1" runat="server"></uc:Paging>
	</div>
	<asp:GridView ID="gvResult" runat="server" CssClass="modify"
		AutoGenerateColumns="false" AllowPaging="false"
		RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
		OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender"
		EmptyDataText="<%$ Resources:Localized, 查無資料 %>" >
		<Columns>
			<cc:MyBoundField DataField="Jtypeid" LocationHeaderText="服務名稱">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="Jstd" LocationHeaderText="開始時間" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="Memo" LocationHeaderText="處理說明">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="JLog" LocationHeaderText="處理日誌">
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
