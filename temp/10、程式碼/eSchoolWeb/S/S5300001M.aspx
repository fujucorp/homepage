<%@ Page Title="土地銀行 - 代收學雜費服務網 - 使用者管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5300001M.aspx.cs" Inherits="eSchoolWeb.S.S5300001M" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
<style type="text/css">
table.result td span.nopadding {
	padding: 0px;
}
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="result" class="result" summary="查詢結果" width="100%">
<tr id="trRole" runat="server">
    <th style="width: 30%">
        <%= GetLocalized("群組角色")%>：
    </th>
    <td>
        <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged" >
            <asp:ListItem Text="行員" Value="1"></asp:ListItem>
            <asp:ListItem Text="學校" Value="2"></asp:ListItem>
        </asp:DropDownList>
    </td>
</tr>
<tr id="trSchIdenty" runat="server">
    <th style="width: 30%">
        <%= GetLocalized("學校代碼")%>：
    </th>
    <td>
        <asp:DropDownList ID="ddlSchIdenty" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSchIdenty_SelectedIndexChanged" >
        </asp:DropDownList>
    </td>
</tr>
<tr id="trBankId" runat="server">
    <th style="width: 30%">
        <%= GetLocalized("分行代號")%>：
    </th>
    <td>
        <asp:Label ID="labBank" runat="server" Text=""></asp:Label>
        <asp:DropDownList ID="ddlBank" runat="server" Visible="false"></asp:DropDownList>
    </td>
</tr>
<tr>
    <th style="width: 30%">
        <%= GetLocalized("群組")%>：
    </th>
    <td>
        <asp:DropDownList ID="ddlGroup" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged"></asp:DropDownList>
    </td>
</tr>
<tr id="trReceiveType" runat="server">
    <th style="width: 30%">
        <%= GetLocalized("商家代號")%>：
    </th>
    <td>
        <asp:CheckBoxList ID="cblReceiveType" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="nopadding"></asp:CheckBoxList>
    </td>
</tr>
<tr>
    <th style="width: 30%">
        <%= GetLocalized("使用者帳號")%>：
    </th>
    <td>
        <asp:TextBox ID="tbxUserId" runat="server" MaxLength="16" Columns="20"></asp:TextBox>
        <br /><asp:Label ID="labUserIdComment" runat="server" ForeColor="Red" Font-Size="Small"></asp:Label>
    </td>
</tr>
<tr>
    <th>
        <%= GetLocalized("使用者名稱")%>：
    </th>
    <td>
        <asp:TextBox ID="tbxUserName" runat="server" MaxLength="20" Columns="20" Width="80%"></asp:TextBox>
    </td>
</tr>
<tr id="trNewPXX" runat="server" >
    <th>
        <%= GetLocalized("使用者密碼")%>：
    </th>
    <td>
        <asp:TextBox ID="tbxNewPXX" runat="server" MaxLength="20" Columns="24" TextMode="Password"></asp:TextBox>
        <br /><span style="color: Red; font-size:smaller;"><%= GetLocalized("8 ~ 20 碼英數字混合（同時含英文與數字），且不可含連續3個(或以上)相同或連號的英文或數字，且不可與帳號相同")%></span>
    </td>
</tr>
<tr id="trConfirmPXX" runat="server" >
    <th>
        <%= GetLocalized("確認密碼")%>：
    </th>
    <td>
        <asp:TextBox ID="tbxConfirmPXX" runat="server" MaxLength="14" Columns="24" TextMode="Password"></asp:TextBox>
    </td>
</tr>
<tr>
    <th>
        <%= GetLocalized("電 話")%>：
    </th>
    <td>
        <asp:TextBox ID="tbxTel" runat="server" MaxLength="20" Columns="20"></asp:TextBox>
    </td>
</tr>
<tr >
    <th>
        <%= GetLocalized("EMail")%>：
    </th>
    <td>
        <asp:TextBox ID="tbxEmail" runat="server" MaxLength="64" Columns="64"></asp:TextBox>
    </td>
</tr>
<tr id="trLock" runat="server" >
    <th>
        <%= GetLocalized("帳號狀態")%>：
    </th>
    <td>
        <asp:CheckBox ID="chkLock" runat="server"></asp:CheckBox>&nbsp;<%= GetLocalized("鎖定")%>&nbsp;&nbsp;
        <cc:MyLinkButton ID="cclbtnLogout" runat="server" LocationText="強迫登出" CssClass="btn" Visible="false" OnClick="cclbtnLogout_Click"></cc:MyLinkButton>
    </td>
</tr>
</table>

<asp:GridView ID="gvUserRight" runat="server" CssClass="modify"
    AutoGenerateColumns="false" AllowPaging="false" Visible="false"
    RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
    OnPreRender="gvUserRight_PreRender">
    <Columns>
        <cc:MyBoundField DataField="FuncId" LocationHeaderText="功能代碼">
            <HeaderStyle Wrap="False"></HeaderStyle>
        </cc:MyBoundField>
        <cc:MyBoundField DataField="FuncName" LocationHeaderText="功能名稱">
            <HeaderStyle Wrap="False"></HeaderStyle>
        </cc:MyBoundField>
        <asp:TemplateField ItemStyle-Width="40px">
            <HeaderTemplate>
                <asp:CheckBox ID="chkAllEdit" Width="100px" runat="server" Text="<%$ Resources:Localized, 編緝(全選) %>" CssClass="cbxAllEdit" />
            </HeaderTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:CheckBox ID="chkEdit" runat="server" Width="80px" Text="<%$ Resources:Localized, 編緝 %>"></asp:CheckBox>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-Width="40px">
            <HeaderTemplate>
                <asp:CheckBox ID="chkAllQuery" Width="100px" runat="server" Text="<%$ Resources:Localized, 查詢(全選) %>" CssClass="cbxAllQuery" />
            </HeaderTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:CheckBox ID="chkQuery" runat="server" Width="80px" Text="<%$ Resources:Localized, 查詢 %>"></asp:CheckBox>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<div class="button">
    <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<script type="text/javascript" language="javascript">
    function checkAllEdit(cbx) {
        var grid = document.getElementById("<%= gvUserRight.ClientID %>");
        var cell;
        if (grid.rows.length > 0) {
            for (i = 1; i < grid.rows.length; i++) {
                cell = grid.rows[i].cells[2];
                var chk = cell.getElementsByTagName("input");
                if (!chk[0].disabled) {
                    chk[0].checked = cbx.checked;
                }
            }
        }
    }
    function checkAllQuery(cbx) {
        var grid = document.getElementById("<%= gvUserRight.ClientID %>");
        var cell;
        if (grid.rows.length > 0) {
            for (i = 1; i < grid.rows.length; i++) {
                cell = grid.rows[i].cells[3];
                var chk = cell.getElementsByTagName("input");
                if (!chk[0].disabled) {
                    chk[0].checked = cbx.checked;
                }
            }
        }
    }

    $(function () {
        $('.cbxAllEdit > input:checkbox').click(function () {
            checkAllEdit($(this)[0]);
        });

        $('.cbxAllQuery > input:checkbox').click(function () {
            checkAllQuery($(this)[0]);
        });
    });
</script>
</asp:Content>
