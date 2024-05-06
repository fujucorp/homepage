<%@ Page Title="土地銀行 - 代收學雜費服務網 - 上傳中國信託次選單" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1600000.aspx.cs" Inherits="eSchoolWeb.D.D1600000" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" AutoPostBack="false" ReceiveKind="2" YearVisible="false" TermVisible="false" />
<uc:SubMenu ID="ucSubMenu" runat="server" OnMenuClick="ucSubMenu_MenuClick" />
</asp:Content>
