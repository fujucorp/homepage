<%@ Page Title="土地銀行 - 代收學雜費服務網 - 檔案上傳匯入次選單" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1500000.aspx.cs" Inherits="eSchoolWeb.D.D1500000" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" AutoPostBack="false" />
<uc:SubMenu ID="ucSubMenu" runat="server" OnMenuClick="ucSubMenu_MenuClick" />
</asp:Content>
