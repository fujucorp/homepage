<%@ Page Title="土地銀行 - 代收學雜費服務網 - 產生虛擬帳號" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="B2200002D.aspx.cs" Inherits="eSchoolWeb.B.B2200002D" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" AutoGetDataBound="false" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Label" AutoGetDataBound="false" />

<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
    <th>
        <div style="text-align:center">
        <%= GetLocalized("批號") %>：<asp:Label ID="labUpNo" runat="server" ></asp:Label>
        </div>
    </th>
</tr>
<tr>
    <td>
        <table class="#" width="100%">
        <tr><td><div align="center">此作業較耗時，建議您稍後（5-10分鐘）再查詢產生繳費金額結果。</div></td></tr>
        <tr style="display:none"><td>&nbsp;</td></tr>
        <tr style="display:none"><td>&nbsp;</td></tr>
        <tr style="display:none"><td><div align="center">系統正在處理學生繳費資料的計算金額，請稍後再查詢計算金額的結果</div></td></tr>
        </table>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
