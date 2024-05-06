<%@ Page Title="土地銀行 - 代收學雜費服務網 - 代收費用別代碼" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1100003M.aspx.cs" Inherits="eSchoolWeb.D.D1100003M" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" AutoGetDataBound="false" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Label" ReceiveVisible="false" AutoGetDataBound="false" />
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
	<th style="width:30%"><cc:MyLabel ID="cclabReceiveIdd" runat="server" LocationText="代收費用別代碼"></cc:MyLabel>：</th>
	<td>
		<asp:TextBox ID="tbxReceiveId" runat="server" MaxLength="1" Width="100px"></asp:TextBox>
		<asp:Label ID="labBigReceiveIdMemo" runat="server" Text="<br/>(如果輸入兩碼的第一碼為0，則視同只有一碼。例如:01與1視為相同)" Visible="false"></asp:Label>
	</td>
</tr>
<tr>
	<th><cc:MyLabel ID="cclabReceiveName" runat="server" LocationText="代收費用別名稱"></cc:MyLabel>：</th>
	<td>
		<asp:TextBox ID="tbxReceiveName" runat="server" MaxLength="40" Width="80%"></asp:TextBox>
	</td>
</tr>
<asp:PlaceHolder ID="phdReceiveEName" runat="server" Visible="false">
<tr>
	<th><cc:MyLabel ID="cclabReceiveEName" runat="server" LocationText="代收費用別英文名稱"></cc:MyLabel>：</th>
	<td>
		<asp:TextBox ID="tbxReceiveEName" runat="server" MaxLength="40" Width="80%"></asp:TextBox>
	</td>
</tr>
</asp:PlaceHolder>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
