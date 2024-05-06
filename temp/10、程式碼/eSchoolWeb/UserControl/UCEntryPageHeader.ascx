<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCEntryPageHeader.ascx.cs" Inherits="eSchoolWeb.UCEntryPageHeader" %>

<div id="logo"><a href="/index.aspx"><img src="/img/logo.gif" /></a></div>
<div id="webTitle"><a href="#">代收學雜費服務網</a></div>
<div id="language"><span id="spnLanguage" runat="server"><asp:LinkButton ID="lbEN" runat="server" OnClick="lbEN_Click">English</asp:LinkButton><span>∣</span><asp:LinkButton ID="lbTW" runat="server" OnClick="lbTW_Click">繁體中文</asp:LinkButton></span></div>
<div id="channel">
  <ul>
    <li><a href="/index.aspx"><%= GetLocalized("回首頁") %><!--回首頁--></a></li>
    <li><a href="javascript:void(0);" onclick="window.location.replace('../Sitemap.aspx')"><%= GetLocalized("網站導覽") %></a></li>
    <li><a href="javascript:void(0);" onclick="window.location.replace('https://mybank.landbank.com.tw/DesktopDefault.htm')"><%= GetLocalized("網路銀行") %></a></li>
    <li><a href="javascript:void(0);" onclick="window.location.replace('https://eatm.landbank.com.tw/2008index.aspx')"><%= GetLocalized("網路ATM") %></a></li>
  </ul>
</div>