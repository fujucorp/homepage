<%@ Page Title="土地銀行 - 代收學雜費服務網 - 問題檔資料刪除" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3600002.aspx.cs" Inherits="eSchoolWeb.C.C3600002" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Option" YearVisible="false" TermVisible="false"  AutoPostBack="false" />

<br/>

<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
    <tr>
        <th style="width:30%"><%=this.GetLocalized("虛擬帳號") %>：</th>
        <td><asp:TextBox ID="tbxCancelNo" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("繳款金額") %>：</th>
        <td><asp:TextBox ID="tbxPayAmount" runat="server" MaxLength="16"></asp:TextBox></td>
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
    <tr>
        <th>
            <%=this.GetLocalized("日期區間") %>：
        </th>
        <td>
            <div align="left">
                <asp:DropDownList ID="ddlQueryDateType" runat="server"></asp:DropDownList>&nbsp;&nbsp;
                <asp:TextBox ID="tbxQuerySDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>～
                <asp:TextBox ID="tbxQueryEDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
            </div>
        </td>
    </tr>
</table>

<table class="result" width="100%" id="tabResult" runat="server">
<tr>
    <th style="text-align:center;">
        查詢結果最多 <%= MaxRecordCount %> 筆資料。<br /><br />
        <asp:Label ID="labMoreMsg" runat="server"></asp:Label>
    </th>
</tr>
</table>

<div class="button">
    <cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
</div>

<asp:GridView ID="gvResult" runat="server" CssClass="modify"
	AutoGenerateColumns="false" AllowPaging="false"
	RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
	OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender"
	EmptyDataText="<%$ Resources:Localized, 查無資料 %>" >
	<Columns>
		<asp:TemplateField ItemStyle-Width="40px">
			<HeaderTemplate>
				<asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />全選
			</HeaderTemplate>
			<ItemStyle HorizontalAlign="Center" />
			<ItemTemplate>
				<asp:CheckBox ID="chkSelected" runat="server" />
			</ItemTemplate>
		</asp:TemplateField>
		<cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="PayAmount" LocationHeaderText="繳款金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="ReceiveWayName" LocationHeaderText="繳款方式">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="ProblemFlag" LocationHeaderText="問題註記">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField LocationHeaderText="代收日期/時間">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="AccountDate" LocationHeaderText="入帳日期" DataFormatString="{0:yyyy/MM/dd}">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField LocationHeaderText="學號/姓名">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField LocationHeaderText="備註">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
	</Columns>
</asp:GridView>

<div class="button">
    <cc:MyDeleteButton ID="MyDeleteButton" runat="server" OnClick="MyDeleteButton_Click"></cc:MyDeleteButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<div class="modify">
    <asp:Label ID="labLog" runat="server"></asp:Label>
</div>

<script type="text/javascript">
    function checkAll(obj) {
        var checked = obj.checked;
        $('input:checkbox:enabled').each(function (idx) {
            var cbx = $(this)[0];
            if (cbx.id != obj.id) {
                cbx.checked = checked;
            }
        });
    }
</script>
</asp:Content>
