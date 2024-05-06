<%@ Page Title="土地銀行 - 代收學雜費服務網 - 上傳課程收費標準" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1500004.aspx.cs" Inherits="eSchoolWeb.D.D1500004" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Option" Visible="false" />

<!--//表格_修改----------------------------------------------------------------->
<table id="modify" class="modify" summary="表格_修改" width="100%">
<tr>
    <th><%= GetLocalized("選擇上傳檔案類型") %>：</th>
    <td>
        <asp:DropDownList ID="ddlFileType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFileType_SelectedIndexChanged">
            <asp:ListItem Value="xls">試算表(xls/xlsx/ods)</asp:ListItem>
            <asp:ListItem Value="txt">純文字(txt)</asp:ListItem>
        </asp:DropDownList>
    </td>
</tr>
<tr>
    <th><%= GetLocalized("選擇上傳對照檔") %>：</th>
    <td><asp:DropDownList ID="ddlMappingId" runat="server"></asp:DropDownList></td>
</tr>
<tr class="light">
    <th><%= GetLocalized("選擇上傳檔案所在位置") %>：</th>
    <td>
        <asp:FileUpload ID="fileUpload" runat="server" Width="80%" />
        <div style="font-size:10pt; color:red; margin-top:4px;">＊注意：不得含有底線(_)以外的符號(如#,&,(,$,@,),!,*,^,+,~...等)，以免造成上傳匯入失敗</div>
    </td>
</tr>
<tr>
    <th><%= GetLocalized("輸入工作表名稱") %>：</th>
    <td>
        <asp:TextBox ID="tbxSheetName" runat="server"></asp:TextBox>
        <div style="font-size:10pt; color:red; margin-top:4px;">＊注意：不得含有底線(_)以外的符號(如#,&,(,$,@,),!,*,^,+,~...等)，以免造成上傳匯入失敗</div>
    </td>
</tr>
<tr class="dark">
    <th><%= GetLocalized("上傳批號") %>：</th>
    <td><asp:Label ID="labSeriorNo" runat="server" Text=""></asp:Label></td>
</tr>
<tr class="light">
    <td colspan="2">
        [操作說明]<br />
        (1) 上傳檔案必須與上傳檔案類型、上傳對照檔相批配。<br />
        (2) 檔案匯入相當耗時，請耐心等待。上傳檔案後，請至【查閱檔案上傳結果】查詢處理結果。<br/>
        <font color="red">(3) 檔案處理完前，請勿上傳重複資料的檔案，以避免資料互相覆蓋。</font><br />
        <br />
        [試算表檔案類型說明]<br />
        (1) 上傳檔案必須為 Excel 的 xls、xlsx 或 Calc 的 ods 試算表檔案。<br />
        (2) 第一列為表頭列，必須指定欄位名稱，且須與對照檔的設定完全相同（大小寫、空白都相同）。<br />
        (3) 第二列開始為資料列，不可有空白的資料列。<br />
        <br />
        [工作表名稱說明]<br />
        開啟試算表檔案後，左下方的名稱即為工作表名稱 (如圖所示)<br />
        <asp:Image ID="imgSheetName" runat="server" ImageUrl="/img/SheetName.JPG" />
    </td>
</tr>
<tr id="trWaitingMsg" style="display:none">
    <td colspan="2" align="center">
        <%= GetLocalized("檔案上傳相當耗時，請耐心等待") %>
        <%= GetLocalized("請至【查閱檔案上傳結果】查詢處理結果") %>
    </td>
</tr>
</table>

<div class="button">
    <asp:LinkButton ID="lbtnUpload" runat="server" OnClick="lbtnUpload_Click" OnClientClick="showWaitingMsg();"><%= GetLocalized("上傳") %></asp:LinkButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
<script type="text/javascript" language="javascript">
    function showWaitingMsg() {
        $('.trWaitingMsg').show();
        return true;
    }
</script>
</asp:Content>
