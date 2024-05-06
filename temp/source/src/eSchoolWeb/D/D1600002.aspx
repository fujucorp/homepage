<%@ Page Title="土地銀行 - 代收學雜費服務網 - 查詢上傳中國信託繳費單資料" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1600002.aspx.cs" Inherits="eSchoolWeb.D.D1600002" %>
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
<uc:Filter1 ID="ucFilter1" runat="server"  UIMode="Label" AutoPostBack="false" ReceiveKind="2" YearVisible="false" TermVisible="false" />

<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
    <td>
        <div align="left">　　<%=this.GetLocalized("學號") %>
            <asp:TextBox ID="tbxStuId" MaxLength="16" runat="server"></asp:TextBox>
        </div>
    </td>
</tr>
<tr>
    <td>
        <div align="left"><%=this.GetLocalized("虛擬帳號") %>
            <asp:TextBox ID="tbxCancelNo" MaxLength="16" runat="server"></asp:TextBox>
        </div>
    </td>
</tr>
<tr id="trReceiveWay">
    <td>
        <div align="left"><%=this.GetLocalized("繳款期限") %>
            <asp:TextBox ID="tbxPayDueDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
        </div>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
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
			<cc:MyBoundField DataField="StuId" LocationHeaderText="學號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="StuName" LocationHeaderText="姓名">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReceiveAmount" LocationHeaderText="金額" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="PayDueDate" LocationHeaderText="繳費期限">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="SendDate" LocationHeaderText="中信日期" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd <br/>HH:mm:ss}">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="CrtDate" LocationHeaderText="檔案日期" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd <br/>HH:mm:ss}">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
		</Columns>
	</asp:GridView>
	<div class='pageControl'>
		<uc:Paging ID="ucPaging2" runat="server"></uc:Paging>
	</div>
</div>
</asp:Content>
