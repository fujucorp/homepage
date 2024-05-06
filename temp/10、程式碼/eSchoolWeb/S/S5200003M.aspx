<%@ Page Title="土地銀行 - 代收學雜費服務網 - 群組管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5200003M.aspx.cs" Inherits="eSchoolWeb.S.S5200003M" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
	<th><cc:MyLabel ID="cclabRole" runat="server" LocationText="群組角色"></cc:MyLabel>：</th>
	<td>
		<asp:Label ID="labRole" runat="server"></asp:Label>
		<asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged" >
			<asp:ListItem Text="行員" Value="1"></asp:ListItem>
			<asp:ListItem Text="學校" Value="2"></asp:ListItem>
		</asp:DropDownList>
	</td>
</tr>
<tr id="trBank" runat="server">
	<th><cc:MyLabel ID="cclabBank" runat="server" LocationText="特定分行"></cc:MyLabel>：</th>
	<td>
		<asp:Label ID="labBank" runat="server"></asp:Label>
		<asp:DropDownList ID="ddlBank" runat="server" AutoPostBack="false" >
		</asp:DropDownList>
	</td>
</tr>
<tr id="trSchIdenty" runat="server">
	<th><cc:MyLabel ID="cclabSchIdenty" runat="server" LocationText="學校代碼"></cc:MyLabel>：</th>
	<td class="tdSchIdenty">
		<asp:Label ID="labSchIdenty" runat="server"></asp:Label>
		<asp:DropDownList ID="ddlSchIdenty" runat="server" AutoPostBack="false">
		</asp:DropDownList>
	</td>
</tr>
<tr>
	<th width="30%"><cc:MyLabel ID="cclabGroupId" runat="server" LocationText="群組代碼"></cc:MyLabel>：</th>
	<td>
		<asp:Label ID="labGroupPrefix" runat="server"></asp:Label>
		<asp:TextBox ID="tbxGroupId" runat="server" MaxLength="4" Width="100px"></asp:TextBox>
	</td>
</tr>
<tr>
	<th><cc:MyLabel ID="cclabGroupName" runat="server" LocationText="群組名稱"></cc:MyLabel>：</th>
	<td>
		<asp:TextBox ID="tbxGroupName" runat="server" MaxLength="20" Width="80%"></asp:TextBox>
	</td>
</tr>
<tr>
	<th><cc:MyLabel ID="cclabRoleType" runat="server" LocationText="權限角色"></cc:MyLabel>：</th>
	<td>
		<asp:Label ID="labRoleType" runat="server"></asp:Label>
		<asp:RadioButtonList ID="rblRoleType" runat="server" AutoPostBack="false" RepeatDirection="Horizontal" RepeatLayout="Flow">
			<asp:ListItem Text="主管" Value="3"></asp:ListItem>
			<asp:ListItem Text="經辦" Value="2"></asp:ListItem>
		</asp:RadioButtonList>
	</td>
</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<script type="text/javascript">
    $(function () {
        $('.tdSchIdenty > select').change(function () {
            var val = $(this).val();
            $('#<%= this.labGroupPrefix.ClientID %>').text(val);
        });
    });
</script>
</asp:Content>
