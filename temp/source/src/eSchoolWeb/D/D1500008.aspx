<%@ Page Title="土地銀行 - 代收學雜費服務網 - 上傳學生基本資料" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1500008.aspx.cs" Inherits="eSchoolWeb.D.D1500008" %>

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
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" YearVisible="false" TermVisible="false"  />

<!--//表格_修改----------------------------------------------------------------->
<table id="modify" class="modify" summary="表格_修改" width="100%">
<tr>
    <th><%= GetLocalized("選擇上傳檔案所在位置") %>：</th>
    <td>
        <asp:FileUpload ID="fileUpload" runat="server" Width="80%" />
        <div style="font-size:10pt; color:red; margin-top:4px;">＊注意：不得含有底線(_)以外的符號(如#,&,(,$,@,),!,*,^,+,~...等)，以免造成上傳匯入失敗</div>
    </td>
</tr>
<tr>
    <th><%= GetLocalized("輸入工作表名稱") %>：</th>
    <td>
        <asp:TextBox ID="tbxSheetName" runat="server" MaxLength="20"></asp:TextBox>
        <div style="font-size:10pt; color:red; margin-top:4px;">＊注意：不得含有底線(_)以外的符號(如#,&,(,$,@,),!,*,^,+,~...等)，以免造成上傳匯入失敗</div>
    </td>
</tr>
<tr class="light">
    <td colspan="2">
        [工作表名稱說明]<br />
        開啟試算表檔案後，左下方的名稱即為工作表名稱 (如圖所示)<br />
        <asp:Image ID="imgSheetName" runat="server" ImageUrl="/img/SheetName.JPG" />
    </td>
</tr>
<tr class="dark">
    <th><%= GetLocalized("上傳批號") %>：</th>
    <td><asp:Label ID="labSeriorNo" runat="server" Text=""></asp:Label></td>
</tr>
<tr class="light">
    <td colspan="2" >
        [操作說明]<br />
        (1) 上傳檔案必須為 Excel 的 xls、xlsx 或 Calc 的 ods 試算表檔案。<br />
        (2) 如需範本請點選 <asp:LinkButton ID="lbtnSampleXLS" runat="server" CommandArgument="XLS" CssClass="btn" Text="下載範本XLS" OnClick="lbtnSample_Click" /> 或 <asp:LinkButton ID="lbtnSampleODS" CommandArgument="ODS" runat="server" CssClass="btn" Text="下載範本ODS" OnClick="lbtnSample_Click" /> 按鈕。<br/>
        <font color="red">(3) 第一列為表頭列，其欄位名稱不可異動。未輸入資料的欄位視為要清除該欄位值。</font><br/>
        (4) 檔案匯入相當耗時，請耐心等待。上傳檔案後，請至【查閱檔案上傳結果】查詢處理結果。<br/>
        <font color="red">(5) 檔案處理完前，請勿上傳重複資料的檔案，以避免資料互相覆蓋。</font><br />
    </td>
</tr>
</table>

<div class="button">
    <asp:LinkButton ID="lbtnUpload" runat="server" OnClick="lbtnUpload_Click" ><%= GetLocalized("上傳") %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
