<%@ Page Title="土地銀行 - 代收學雜費服務網 - 維護上傳已產生虛擬帳號就貸資料對照表" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1400004M.aspx.cs" Inherits="eSchoolWeb.D.D1400004M1" %>

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
<div id="divStep1" runat="server">
    <uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" YearVisible="false" TermVisible="false" AutoGetDataBound="false" />
    <table id="condition" class="condition" summary="表格_修改" width="100%">
    <tr>
        <th colspan="4"><div align="left"><%= GetLocalized("勾選上傳資料對照表項目") %></div></th>
    </tr>
    <tr>
        <th><%= GetLocalized("上傳檔案格式") %>：</th>
        <td>
            <asp:DropDownList ID="ddlFileType" runat="server">
                <asp:ListItem Selected="True" Value="xls">試算表(xls/xlsx/ods)</asp:ListItem>
                <asp:ListItem Value="txt">純文字(txt)</asp:ListItem>
            </asp:DropDownList>
        </td>
        <th><%= GetLocalized("對照表名稱") %>：</th>
        <td><asp:TextBox ID="tbxMappingName" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td colspan="4">
            <table width="100%" class="#">
            <tr>
                <td >
                    <asp:CheckBox ID="chkCancel_No" Enabled="false" Checked="true" Text="<%$ Resources:Localized, 虛擬帳號%>" runat="server" />
                </td>
                <td >
                    <asp:CheckBox ID="chkS_Id" Enabled="false" Checked="true" Text="<%$ Resources:Localized, 學號%>" runat="server" />
                </td>
            </tr>
            <tr>
                <td><asp:CheckBox ID="chkLo_Name" runat="server" Text="<%$ Resources:Localized, 就貸代碼%>" /></td>
                <td><asp:CheckBox ID="chkLo_Count" runat="server" Text="<%$ Resources:Localized, 就貸名稱%>" /></td>
            </tr>
            <tr>
                <td id="tdLoAmount"><asp:CheckBox ID="chkLo_amount" runat="server" Text="<%$ Resources:Localized, 就貸金額%>" Enabled="false" Checked="true" /></td>
                <td id="tdLoItem">
                    <asp:CheckBox ID="chkLo" runat="server" Text="<%$ Resources:Localized, 就貸金額項目%>" />&nbsp;
                    <asp:TextBox ID="tbxLoCount" Width="30px" runat="server"></asp:TextBox>項
                </td>
            </tr>
            </table>
        </td>
    </tr>
    </table>
    <div class="button">
        <asp:LinkButton ID="lbtnNext" runat="server" OnClick="lbtnNext_Click"><%= GetLocalized("下一步") %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
        <cc:MyGoBackButton ID="MyGoBackButton1" runat="server"></cc:MyGoBackButton>
    </div>
<%--    <script type="text/javascript">
        $(function () {
            $('#tdLoAmount > input:checkbox').click(function () {
                if ($(this)[0].checked) {
                    $('#tdLoItem > input:checkbox')[0].checked = false;
                }
            });
            $('#tdLoItem > input:checkbox').click(function () {
                if ($(this)[0].checked) {
                    $('#tdLoAmount > input:checkbox')[0].checked = false;
                }
            });
        });
    </script>--%>
</div>

<div id="divStep2A" runat="server">
    <table id="table2A" class="condition" summary="表格_修改" width="100%">
    <tr>
        <th ><div align="left"><%= GetLocalized("對照表名稱") %>：<asp:Label ID="labMappingNameA" runat="server" Text=""></asp:Label></div></th>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" class="#">
            <tr>
                <th><div align="center"><%= GetLocalized("欄位") %></div></th>
                <th><div align="center"><%= GetLocalized("起始位置") %></div></th>
                <th><div align="center"><%= GetLocalized("長度") %></div></th>
            </tr>
            <asp:Literal ID="litHtmlA" runat="server"></asp:Literal>
            </table>
        </td>
    </tr>
    </table>
    <div class="button">
        <asp:LinkButton ID="lbtnGoStep1A" runat="server" OnClick="lbtnBack_Click"><%= GetLocalized("上一步") %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="lbtnOK2A" runat="server" OnClick="lbtnOK2A_Click"><%= GetLocalized("確定") %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
        <cc:MyGoBackButton ID="ccbtnGoBack2A" runat="server"></cc:MyGoBackButton>
    </div>
</div>

<div id="divStep2B" runat="server">
    <table id="table2B" class="condition" summary="表格_修改" width="100%">
    <tr>
        <th ><div align="left"><%= GetLocalized("對照表名稱") %>：<asp:Label ID="labMappingNameB" runat="server" Text=""></asp:Label></div></th>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" class="#">
            <tr><th><div align="center"><%= GetLocalized("欄位") %></div></th>
            <th><div align="center"><%= GetLocalized("試算表欄位名稱") %></div></th></tr>
            <asp:Literal ID="litHtmlB" runat="server"></asp:Literal>
            </table>
        </td>
    </tr>
    </table>
    <div class="button">
        <asp:LinkButton ID="lbtnGoStep1B" runat="server" OnClick="lbtnBack_Click"><%= GetLocalized("上一步") %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="lbtnOK2B" runat="server" OnClick="lbtnOK2B_Click"><%= GetLocalized("確定") %></asp:LinkButton>
        <cc:MyGoBackButton ID="ccbtnGoBack2B" runat="server"></cc:MyGoBackButton>
    </div>
</div>
</asp:Content>
