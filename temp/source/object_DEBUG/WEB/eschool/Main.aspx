<%@ Page Title="土地銀行 - 代收學雜費服務網" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="eSchoolWeb.Main1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
    <link href="/css/login_success.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title">
        <p></p>
        <span><%= GetLocalized("登入成功") %></span>
    </div>
    <div id="bulletin">
        <cc:MyLiteral ID="cclitWelcome" runat="server" ResourceKey="Welcome" Text="<p>歡迎光臨！</p><p>您已經成功登入代收學雜費服務網站。</p>"></cc:MyLiteral>
        <asp:Literal ID="litChangePXX" runat="server" Visible="false"></asp:Literal>
    </div>
    <!--end of bulletin-->
</asp:Content>
