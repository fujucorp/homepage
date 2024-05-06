<%@ Page Title="土地銀行 - 代收學雜費服務網 - 權限管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5200002.aspx.cs" Inherits="eSchoolWeb.S.S5200002" %>

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
<script type="text/javascript" language="javascript">
    function CheckAllInsert(Checkbox) {
        var grid = document.getElementById("<%= gvResult.ClientID %>");
        var cell;
        if (grid.rows.length > 0) {
            for (i = 1; i < grid.rows.length; i++) {
                cell = grid.rows[i].cells[2];
                var chk = cell.getElementsByTagName("input");
                if (chk[0].disabled) {
                    chk[0].checked = false;
                }
                else {
                    chk[0].checked = Checkbox.checked;
                }
            }
        }
    }
    function CheckAllUpdate(Checkbox) {
        var grid = document.getElementById("<%= gvResult.ClientID %>");
        var cell;
        if (grid.rows.length > 0) {
            for (i = 1; i < grid.rows.length; i++) {
                cell = grid.rows[i].cells[3];
                var chk = cell.getElementsByTagName("input");
                if (chk[0].disabled) {
                    chk[0].checked = false;
                }
                else {
                    chk[0].checked = Checkbox.checked;
                }
            }
        }
    }
    function CheckAllDelete(Checkbox) {
        var grid = document.getElementById("<%= gvResult.ClientID %>");
        var cell;
        if (grid.rows.length > 0) {
            for (i = 1; i < grid.rows.length; i++) {
                cell = grid.rows[i].cells[4];
                var chk = cell.getElementsByTagName("input");
                if (chk[0].disabled) {
                    chk[0].checked = false;
                }
                else {
                    chk[0].checked = Checkbox.checked;
                }
            }
        }
    }
    function CheckAllSelect(Checkbox) {
        var grid = document.getElementById("<%= gvResult.ClientID %>");
        var cell;
        if (grid.rows.length > 0) {
            for (i = 1; i < grid.rows.length; i++) {
                cell = grid.rows[i].cells[5];
                var chk = cell.getElementsByTagName("input");
                if (chk[0].disabled) {
                    chk[0].checked = false;
                }
                else {
                    chk[0].checked = Checkbox.checked;
                }
            }
        }
    }
    function CheckAllPrint(Checkbox) {
        var grid = document.getElementById("<%= gvResult.ClientID %>");
        var cell;
        if (grid.rows.length > 0) {
            for (i = 1; i < grid.rows.length; i++) {
                cell = grid.rows[i].cells[6];
                var chk = cell.getElementsByTagName("input");
                if (chk[0].disabled) {
                    chk[0].checked = false;
                }
                else {
                    chk[0].checked = Checkbox.checked;
                }
            }
        }
    }
</script>

<table class="information">
<tr>
	<td>
		<cc:MyLabel ID="cclabGroup" runat="server" LocationText="群組"></cc:MyLabel>：
		<asp:DropDownList ID="ddlGroup" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged"></asp:DropDownList>
	</td>
</tr>
</table>

<div id="divResult" runat="server">
	<asp:GridView ID="gvResult" runat="server" CssClass="modify"
		AutoGenerateColumns="false" AllowPaging="false"
		RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
		OnPreRender="gvResult_PreRender">
		<Columns>
			<cc:MyBoundField DataField="FuncId" LocationHeaderText="功能代碼">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="FuncName" LocationHeaderText="功能名稱">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<asp:TemplateField ItemStyle-Width="40px">
				<HeaderTemplate>
					<asp:CheckBox ID="chkInsertAll" Width="40px" runat="server" Text="新增<br/>全選" onclick="CheckAllInsert(this);" />
				</HeaderTemplate>
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<asp:CheckBox ID="chkInsert" runat="server" Text="新增"></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ItemStyle-Width="40px">
				<HeaderTemplate>
					<asp:CheckBox ID="chkUpdateAll" Width="40px" runat="server" Text="修改<br/>全選" onclick="CheckAllUpdate(this);" />
				</HeaderTemplate>
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<asp:CheckBox ID="chkUpdate" runat="server" Text="修改"></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ItemStyle-Width="40px">
				<HeaderTemplate>
					<asp:CheckBox ID="chkDeleteAll" Width="40px" runat="server" Text="刪除<br/>全選" onclick="CheckAllDelete(this);" />
				</HeaderTemplate>
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<asp:CheckBox ID="chkDelete" runat="server" Text="刪除"></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ItemStyle-Width="40px">
				<HeaderTemplate>
					<asp:CheckBox ID="chkSelectAll" Width="40px" runat="server" Text="查詢<br/>全選" onclick="CheckAllSelect(this);" />
				</HeaderTemplate>
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<asp:CheckBox ID="chkSelect" runat="server" Text="查詢"></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ItemStyle-Width="40px">
				<HeaderTemplate>
					<asp:CheckBox ID="chkPrintAll" Width="40px" runat="server" Text="列印<br/>全選" onclick="CheckAllPrint(this);" />
				</HeaderTemplate>
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<asp:CheckBox ID="chkPrint" runat="server" Text="列印"></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
</div>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>
	<cc:MyLinkButton ID="ccbtnExport" runat="server" LocationText="匯出XLS" CommandArgument="XLS" OnClick="ccbtnExport_Click"></cc:MyLinkButton>
	<cc:MyLinkButton ID="ccbtnExportODS" runat="server" LocationText="匯出ODS" CommandArgument="ODS" OnClick="ccbtnExport_Click"></cc:MyLinkButton>
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
