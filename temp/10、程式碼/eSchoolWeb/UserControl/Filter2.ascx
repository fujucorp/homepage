<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Filter2.ascx.cs" Inherits="eSchoolWeb.Filter2" %>
<table class="information">
<tr>
	<td id="tdReceive" runat="server">
		<%= GetControlLocalized("Filter2", "Receive", "代收費用別")%>：
		<asp:DropDownList ID="ddlReceive" runat="server" AutoPostBack="True" 
			OnSelectedIndexChanged="ddlReceive_SelectedIndexChanged">
		</asp:DropDownList>
		<asp:Label ID="labReceive" runat="server" Visible="false"></asp:Label>
	</td>
</tr>
</table>