<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Paging.ascx.cs" Inherits="eSchoolWeb.Paging" %>
<div class="pageControl">
	<div class="pageControl_Left">
		<asp:Label ID="labPageNo" runat="server" Text="第0頁"></asp:Label>
		（<span><asp:Label ID="labMaxPageNo" runat="server" Text="共0頁"></asp:Label></span>） 
		"到第"
		<asp:DropDownList ID="ddlGoPageNo" runat="server" AutoPostBack="true" 
			OnSelectedIndexChanged="ddlGoPageNo_SelectedIndexChanged">
		</asp:DropDownList>
		"頁"
	</div>
	<div class="pageControl_right">
		"每頁"
		<asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" 
			OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
			<asp:ListItem Value="10" Selected="True">10</asp:ListItem>
			<asp:ListItem Value="20">20</asp:ListItem>
			<asp:ListItem Value="50">50</asp:ListItem>
			<asp:ListItem Value="100">100</asp:ListItem>
			<asp:ListItem Value="0">不分頁</asp:ListItem>
		</asp:DropDownList>
		"筆"
		<asp:LinkButton ID="lbtnGoFirstPage" runat="server" CssClass="firstPage" Text="最前頁" OnClick="lbtnGoFirstPage_Click"></asp:LinkButton>
		<asp:LinkButton ID="lbtnGoPreviousPage" runat="server" CssClass="forwardPage" Text="上一頁" OnClick="lbtnGoPreviousPage_Click"></asp:LinkButton>
		<asp:LinkButton ID="lbtnGoNextPage" runat="server" CssClass="nextPage" Text="下一頁" OnClick="lbtnGoNextPage_Click"></asp:LinkButton>
		<asp:LinkButton ID="lbtnGoLastPage" runat="server" CssClass="lasttPage" Text="最後頁" OnClick="lbtnGoLastPage_Click"></asp:LinkButton>
	</div>
</div>
