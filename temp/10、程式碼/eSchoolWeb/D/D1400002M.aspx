<%@ Page Title="土地銀行 - 代收學雜費服務網 - 維護上傳學分費退費資料對照表" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1400002M.aspx.cs" Inherits="eSchoolWeb.D.D1400002M1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
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
                <td colspan="4">
                    <asp:CheckBox ID="chkStu_Id" Enabled="false" Checked="true" Text="<%$ Resources:Localized, 學號 %>" runat="server" />
                </td>
            </tr>
            <tr>
                <td><asp:CheckBox ID="chkRt_Credit" runat="server" Text="<%$ Resources:Localized, 要退的學分數 %>" /></td>
                <td><asp:CheckBox ID="chkRe_Credit" runat="server" Text="<%$ Resources:Localized, 實際所修的學分%>" /></td>
                <td colspan="2"><asp:CheckBox ID="chkRT_Name" runat="server" Text="<%$ Resources:Localized, 退費方式標準代碼%>" /></td>
            </tr>
            <tr>
                <td><asp:CheckBox ID="chkRt_Amount" runat="server" Text="<%$ Resources:Localized, 要退金額%>" /></td>
                <td colspan="3">
                    <asp:CheckBox ID="chkRt" runat="server" Text="<%$ Resources:Localized, 退費金額%>" />&nbsp;
                    <asp:TextBox ID="tbxRtCount" Width="30px" runat="server"></asp:TextBox>項
                </td>
            </tr>
            <tr>
                <td><asp:CheckBox ID="chkRt_Bank_Id" runat="server" Text="<%$ Resources:Localized, 匯款銀行代碼%>" /></td>
                <td colspan="3"><asp:CheckBox ID="chkRt_Account" runat="server" Text="<%$ Resources:Localized, 匯款帳號%>" /></td>
            </tr>
            </table>
        </td>
    </tr>
    </table>
    <div class="button">
        <asp:LinkButton ID="lbtnNext" runat="server" OnClick="lbtnNext_Click"><%= GetLocalized("下一步") %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
        <cc:MyGoBackButton ID="MyGoBackButton1" runat="server"></cc:MyGoBackButton>
    </div>
</div>

<div id="divStep2A" runat="server">
    <table id="table2A" class="condition" summary="表格_修改" width="100%">
    <tr>
        <th ><div align="left"><%= GetLocalized("對照表名稱") %>：<asp:Label ID="labMappingNameA" runat="server" Text=""></asp:Label></div></th>
    </tr>
    <tr>
        <td >
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
        <td >
            <table width="100%" class="#">
            <tr>
                <th><div align="center"><%= GetLocalized("欄位") %></div></th>
                <th><div align="center"><%= GetLocalized("試算表欄位名稱") %></div></th>
            </tr>
            <asp:Literal ID="litHtmlB" runat="server"></asp:Literal>
            </table>
        </td>
    </tr>
    </table>
    <div class="button">
        <asp:LinkButton ID="lbtnGoStep1B" runat="server" OnClick="lbtnBack_Click"><%= GetLocalized("上一步") %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="lbtnOK2B" runat="server" OnClick="lbtnOK2B_Click"><%= GetLocalized("確定") %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
        <cc:MyGoBackButton ID="ccbtnGoBack2B" runat="server"></cc:MyGoBackButton>
    </div>
</div>
</asp:Content>
