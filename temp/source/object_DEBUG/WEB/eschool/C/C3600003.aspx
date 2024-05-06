<%@ Page Title="土地銀行 - 代收學雜費服務網 - 產生及下載媒體" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3600003.aspx.cs" Inherits="eSchoolWeb.C.C3600003" %>
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

<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Option" YearVisible="false" TermVisible="false" />

<br/>

<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
    <tr>
        <th><%=this.GetLocalized("虛擬帳號") %>：</th>
        <td><asp:TextBox ID="tbxCancelNo" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("繳款金額") %>：</th>
        <td><asp:TextBox ID="tbxPayAmount" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("管道") %>：</th>
        <td>
            <asp:DropDownList ID="ddlReceiveWay" runat="server">
                <asp:ListItem Value="1">超商</asp:ListItem>
                <asp:ListItem Value="2">ATM</asp:ListItem>
                <asp:ListItem Value="3">臨櫃</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="trReceiveDateRange">
        <th><%=this.GetLocalized("代收日區間") %>：</th>
        <td>
            <div align="left">
                <asp:TextBox ID="tbxReceiveDateS" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>～
                <asp:TextBox ID="tbxReceiveDateE" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
            </div>
        </td>
    </tr>
    <tr id="trAccountDateRange">
        <th><%=this.GetLocalized("入帳日區間") %>：</th>
        <td>
            <div align="left">
                <asp:TextBox ID="tbxAccountDateS" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>～
                <asp:TextBox ID="tbxAccountDateE" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
            </div>
        </td>
    </tr>
</table>

<div class="button">
    <cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
</div>

<asp:GridView ID="gvResult" runat="server" CssClass="modify"
	AutoGenerateColumns="false" AllowPaging="false"
	RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
	OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender">
	<Columns>
		<cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="PayAmount" LocationHeaderText="繳款金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="ReceiveWay" LocationHeaderText="繳款方式">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="ProblemFlag" LocationHeaderText="問題註記">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="ReceiveDate" LocationHeaderText="代收日">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="AccountDate" LocationHeaderText="入帳日">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="StuId" LocationHeaderText="學號">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="StuName" LocationHeaderText="姓名">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
	</Columns>
</asp:GridView>

<div class="button">
    <cc:MyLinkButton ID="ccbtnGen" runat="server" LocationText="產生媒體檔XLS" CommandArgument="XLS" OnClick="ccbtnGen_Click"></cc:MyLinkButton>
    <cc:MyLinkButton ID="ccbtnGenODS" runat="server" LocationText="產生媒體檔ODS" CommandArgument="ODS" OnClick="ccbtnGen_Click"></cc:MyLinkButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<asp:GridView ID="gvResult2" runat="server" 
	AutoGenerateColumns="False" PageSize="1"
	HeaderStyle-Font-Size="12pt" RowStyle-Font-Size="11pt" 
	BorderStyle="none" BorderWidth="0" Visible="false" >
	<Columns>
		<cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="PayAmount" LocationHeaderText="繳款金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="ReceiveWay" LocationHeaderText="繳款方式">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="ProblemFlag" LocationHeaderText="問題註記">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="ReceiveDate" LocationHeaderText="代收日">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="AccountDate" LocationHeaderText="入帳日">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="StuId" LocationHeaderText="學號">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="StuName" LocationHeaderText="姓名">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
	</Columns>
</asp:GridView>
</asp:Content>
