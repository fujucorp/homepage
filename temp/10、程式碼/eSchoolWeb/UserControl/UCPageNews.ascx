<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCPageNews.ascx.cs" Inherits="eSchoolWeb.UserControl.UCPageNews" %>

<!--公告事項-->
<div class="masthead_s"><span></span>
    <p><%= GetLocalized("公告事項") %><!--公告事項--></p>
</div>
<div id="bulletin">
    <ul>
        <asp:Literal ID="litNews" runat="server"></asp:Literal>
    </ul>
</div>
<!--end of bulletin-->
