<%@ Page Title="土地銀行 - 代收學雜費服務網 - 維護上傳對照檔次選單" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1400000.aspx.cs" Inherits="eSchoolWeb.D.D1400000" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" YearVisible="false" TermVisible="false" AutoPostBack="false" />
<uc:SubMenu ID="ucSubMenu" runat="server" RepeatColumns="2" OnMenuClick="ucSubMenu_MenuClick" />
</asp:Content>
