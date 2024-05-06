<%@ Page Title="土地銀行 - 代收學雜費服務網 - 學校自收整批上傳" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3200002.aspx.cs" Inherits="eSchoolWeb.C.C3200002" %>

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
<!--//表格_修改----------------------------------------------------------------->
<table id="modify" class="modify" summary="表格_修改" width="100%">
<tr class="dark">
    <th><%= GetLocalized("上傳銷帳媒體檔案") %>：</th>
    <td>
        <asp:FileUpload ID="fileUpload" runat="server" Width="90%" />
    </td>
</tr>
<tr class="dark">
    <th><%= GetLocalized("代收日") %>：</th>
    <td>
        <asp:TextBox ID="tbxReceiveDate" runat="server" MaxLength="7" Width="100px"></asp:TextBox>&nbsp;
        <span style="font-size:11px">(請輸入3碼的民國年+2碼月+2碼日。例如 2015/01/02 請輸入 1040102)</span>
    </td>
</tr>
<tr class="dark">
    <th><%= GetLocalized("入帳日") %>：</th>
    <td>
        <asp:TextBox ID="tbxAccountDate" runat="server" MaxLength="7" Width="100px"></asp:TextBox>&nbsp;
        <span style="font-size:11px">(請輸入3碼的民國年+2碼月+2碼日。例如 2015/01/02 請輸入 1040102)</span>
    </td>
</tr>
<tr class="light">
    <td colspan="2">
        <div style="text-align:left">
            [上傳檔案說明]：<br />
            1. 僅支援 Excel 的 xls、xlsx 或 Calc 的 ods 試算表檔案。<br />
            2. 工作表名稱必須為 Sheet1。<br />
            3. 第一列為表頭列，須指定欄位名稱，依序為虛擬帳號、繳費金額。<br />
            4. 上傳資料最多 1000 筆，以避免處理時間太長造成網頁逾時。<br />
        </div>
    </td>
</tr>
</table>

<div class="modify">
    <asp:Label ID="labLog" runat="server"></asp:Label>
</div>

<div class="button">
    <asp:LinkButton ID="lbtnUpload" runat="server" OnClick="lbtnUpload_Click"><%= GetLocalized("上傳") %></asp:LinkButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

</asp:Content>
