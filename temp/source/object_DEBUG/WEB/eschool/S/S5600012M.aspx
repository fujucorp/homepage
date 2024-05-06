<%@ Page Title="土地銀行 - 代收學雜費服務網 - 維護檔案下載" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600012M.aspx.cs" Inherits="eSchoolWeb.S.S5600012M" %>
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
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
    <th width="30%"><%= GetLocalized("說明") %>：</th>
    <td>
        <asp:TextBox ID="tbxExplain" runat="server" MaxLength="1000" Width="80%"></asp:TextBox>
    </td>
</tr>
<tr>
    <th><%= GetLocalized("型態") %>：</th>
    <td id="tdFileQual">
        <asp:DropDownList ID="ddlFileQual" runat="server">
            <asp:ListItem Text="連結" Value="1"></asp:ListItem>
            <asp:ListItem Text="檔案" Value="2"></asp:ListItem>
        </asp:DropDownList>
    </td>
</tr>
<tr id="trFile">
    <th><%= GetLocalized("上傳檔案") %>：</th>
    <td>
        <asp:Label ID="labOldFileName" runat="server" Visible="false"></asp:Label>
        <asp:FileUpload ID="FileUpload1" runat="server" Width="90%" />
    </td>
</tr>
<tr id="trUrl">
    <th><%= GetLocalized("連結網址") %>：</th>
    <td>
        <asp:TextBox ID="tbxUrl" runat="server" MaxLength="256" Width="80%"></asp:TextBox>
    </td>
</tr>
<tr>
    <th colspan="2">
        <div style="text-align:left">
            [上傳檔案說明]：<br />
            1.上傳檔案最大10M。<br />
            2.上傳檔案不限制檔案類型，但檔名限制最多100個中文字。<br />
            3.上傳檔案的檔名會作為下載檔案的檔名。<br />
            4.僅修改【說明】欄位時，無須重新上傳檔案。<br />
        </div>
    </th>
</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<script type="text/javascript">
    $(function () {
        $('#tdFileQual > select').change(function () {
            var val = $(this).val();
            if (val == "1") {   //連結
                $('#trFile').hide();
                $('#trUrl').show();
            } else {
                $('#trFile').show();
                $('#trUrl').hide();
            }
        });

        $('#tdFileQual > select').trigger("change");
    });
</script>
</asp:Content>
