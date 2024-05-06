<%@ Page Title="土地銀行 - 代收學雜費服務網 - 繳費單模板管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5700001M.aspx.cs" Inherits="eSchoolWeb.S.S5700001M" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
    <tr>
        <th width="30%"><%= GetLocalized("模板類別") %>：</th>
        <td>專屬</td>
    </tr>
    <tr>
        <th><%= GetLocalized("商家代碼") %>：</th>
        <td>
            <asp:DropDownList ID="ddlReceiveType" runat="server"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th><%= GetLocalized("模板種類") %>：</th>
        <td>
            <asp:RadioButtonList ID="rdoBillFormType" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server">
                <asp:ListItem Value="0">繳費單&nbsp;</asp:ListItem>
                <asp:ListItem Value="1">收據</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th><%= GetLocalized("模板代號") %>：</th>
        <td>
            <span id="spnPrefix" runat="server" clientidmode="Static"></span>
            <asp:TextBox ID="tbxBillFormId" runat="server" MaxLength="4" Width="100px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th><%= GetLocalized("模板名稱") %>：</th>
        <td><asp:TextBox ID="tbxBillFormName" runat="server" MaxLength="90" Width="80%"></asp:TextBox></td>
    </tr>
    <tr id="trFileUpload" runat="server">
        <th><%= GetLocalized("上傳檔案") %>：</th>
        <td>
            <asp:FileUpload ID="fileUpload" runat="server" Width="80%" />
        </td>
    </tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<script type="text/javascript">
    $('.tdReceiveType > select').change(function () {
        var val = $(this).val();
        $('#spnPrefix').text(val);
    });

    $('.tdReceiveType > select').trigger("change");
</script>
</asp:Content>
