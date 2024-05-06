<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Filter1.ascx.cs" Inherits="eSchoolWeb.Filter1" %>
<table class="information">
<tr>
	<td>
		<%= GetControlLocalized("Filter1", "ReceiveType", "商家代號")%>：
		<asp:DropDownList ID="ddlReceiveType" runat="server" AutoPostBack="True" CssClass="ddlFilter"
			OnSelectedIndexChanged="ddlReceiveType_SelectedIndexChanged">
		</asp:DropDownList>
		<asp:Label ID="labReceiveType" runat="server" Visible="false"></asp:Label>
	</td>
	<td id="tdYear" runat="server">
		<%= GetControlLocalized("Filter1", "Year", "學年")%>：
		<asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="True" CssClass="ddlFilter"
			OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
		</asp:DropDownList>
		<asp:Label ID="labYear" runat="server" Visible="false"></asp:Label>
	</td>
	<td id="tdTerm" runat="server">
		<%= GetControlLocalized("Filter1", "Term", "學期")%>：
		<asp:DropDownList ID="ddlTerm" runat="server" AutoPostBack="True" CssClass="ddlFilter"
			OnSelectedIndexChanged="ddlTerm_SelectedIndexChanged">
		</asp:DropDownList>
		<asp:Label ID="labTerm" runat="server" Visible="false"></asp:Label>
	</td>
</tr>
</table>
