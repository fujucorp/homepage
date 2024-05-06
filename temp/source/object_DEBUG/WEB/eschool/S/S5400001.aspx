<%@ Page Title="土地銀行 - 代收學雜費服務網 - 使用者操作記錄查詢" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5400001.aspx.cs" Inherits="eSchoolWeb.S.S5400001" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
<style type="text/css">
html { display: none; }
</style>
<script type="text/javascript" language="javascript">
    if (self === top) {
        document.documentElement.style.display = 'block';
    }
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<!--//查詢條件----------------------------------------------------------------->
<table id="condition" class="information" summary="查詢條件" width="100%">
<tr id="trUserQual" runat="server" >
    <td>
        <%=this.GetLocalized("單位類別") %>：<asp:DropDownList ID="ddlUserQual" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUserQual_SelectedIndexChanged"></asp:DropDownList>
    </td>
</tr>
<tr id="trBank" runat="server">
    <td>
        <%=this.GetLocalized("分行") %>：<asp:DropDownList ID="ddlBank" runat="server" AutoPostBack="false"></asp:DropDownList>
    </td>
</tr>
<tr id="trSchIdenty" runat="server">
    <td>
        <%=this.GetLocalized("學校") %>：<asp:DropDownList ID="ddlSchIdenty" runat="server" AutoPostBack="false"></asp:DropDownList>
    </td>
</tr>
<tr>
    <td><%=this.GetLocalized("使用者帳號") %>：
        <asp:TextBox ID="tbxUserId" runat="server"></asp:TextBox>
    </td>
</tr>
<tr>
    <td><%=this.GetLocalized("日期區間") %>：
        <asp:TextBox ID="tbxDateS" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox> 至
        <asp:TextBox ID="tbxDateE" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
    </td>
</tr>
<tr >
    <td><%=this.GetLocalized("功能") %>：
        <asp:DropDownList ID="ddlFunction" runat="server">
        </asp:DropDownList>
    </td>
</tr>
<tr >
    <td><%=this.GetLocalized("操作") %>：
        <asp:DropDownList ID="ddlLogType" runat="server">
            <asp:ListItem Value="A" Text="新增"></asp:ListItem>
            <asp:ListItem Value="U" Text="修改"></asp:ListItem>
            <asp:ListItem Value="D" Text="刪除"></asp:ListItem>
            <asp:ListItem Value="S" Text="查詢"></asp:ListItem>
        </asp:DropDownList>
    </td>
</tr>
</table>

<div class="button">
	<cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
</div>

<div id="divResult" runat="server" >
	<div class='pageControl'>
		<uc:Paging ID="ucPaging1" runat="server"></uc:Paging>
	</div>
	<asp:GridView ID="gvResult" runat="server" CssClass="modify"
		AutoGenerateColumns="false" AllowPaging="false"
		RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
		OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender"
		EmptyDataText="<%$ Resources:Localized, 查無資料 %>" >
		<Columns>
			<cc:MyBoundField DataField="Role" LocationHeaderText="單位類別" Visible="false">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="UserId" LocationHeaderText="使用者帳號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="LogDate" LocationHeaderText="操作時間">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="FuncName" LocationHeaderText="功能">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="LogType" LocationHeaderText="操作">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="Notation" LocationHeaderText="說明">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
		</Columns>
	</asp:GridView>
	<div class='pageControl'>
		<uc:Paging ID="ucPaging2" runat="server"></uc:Paging>
	</div>
</div>

<div class="button">
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

</asp:Content>
