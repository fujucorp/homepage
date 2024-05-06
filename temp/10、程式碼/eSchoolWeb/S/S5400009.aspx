<%@ Page Title="土地銀行 - 代收學雜費服務網 - 上傳D00I70檔案" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5400009.aspx.cs" Inherits="eSchoolWeb.S.S5400009" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server"  UIMode="Label" AutoPostBack="false" ReceiveKind="2" YearVisible="false" TermVisible="false" Visible="false" />

<table id="modify" class="modify" summary="檔案上傳" width="100%">
<tr class="dark">
	<th><%= GetLocalized("選擇上傳檔案所在位置") %>：</th>
	<td>
		<asp:FileUpload ID="fileUpload" runat="server" Width="90%" />
	</td>
</tr>
<tr class="light" style="display:none">
    <td colspan="2" >
        [ 注意 ]<br />
        1. 僅支援 Excel 檔案，可使用 xls 或 xlsx 格式<br/>
        2. 僅支援一個工作表，名稱需命名為 sheet1。<br/>
        3. 第一行為欄位名稱，依序為學號、姓名、虛擬帳號、金額，名稱與順序不可改變。 <br/>
        4. 資料最多 65535 筆。<br/>
        5. 檔案最下方不得殘存出現空白列。<br/>
        6. 建議將儲存格格式設為 [文字]。<br/>
    </td>
</tr>
<tr>
    <td colspan="2" align="left">
        <pre><asp:Label ID="labResult" runat="server" style="padding:0px"></asp:Label></pre>
    </td>
</tr>
</table>
<div class="button">
    <asp:LinkButton ID="lbtnUpload" runat="server" OnClick="lbtnUpload_Click" OnClientClick="openWaitDialog();"><%= GetLocalized("上傳") %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<div id="divWaitDialog" title="上傳檔案處理中" style="text-align:center;">
    <p>&nbsp;</p>
    <fieldset>
        <%= GetLocalized("上傳檔案匯入相當耗時，請耐心等待") %>
    </fieldset>
</div>

<script type="text/javascript">
    var dialog = $("#divWaitDialog").dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true
    });
    function openWaitDialog() {
        dialog.dialog("open");
    }
</script>
</asp:Content>
