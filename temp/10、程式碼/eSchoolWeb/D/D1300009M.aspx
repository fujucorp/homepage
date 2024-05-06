<%@ Page Title="土地銀行 - 代收學雜費服務網 - 減免類別標準" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1300009M.aspx.cs" Inherits="eSchoolWeb.D.D1300009M" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" YearVisible="true" TermVisible="true" AutoGetDataBound="true" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Label" />

<!--//表格_修改----------------------------------------------------------------->
<table id="condition" class="condition" summary="表格_修改" width="100%">
    <tr>
		<th><div align="left"><cc:MyLabel ID="MyLabel3" runat="server" LocationText="減免名稱" />：<asp:DropDownList ID="ddlReduceId" runat="server"></asp:DropDownList></div></th>
        <th><div align="left"><cc:MyLabel ID="MyLabel1" runat="server" LocationText="減免方式" />：<asp:DropDownList ID="ddlReduceWay" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlReduceWay_SelectedIndexChanged">
                <asp:ListItem Text="依百分比計算" Value="1"></asp:ListItem>
                <asp:ListItem Text="依金額計算" Value="2"></asp:ListItem>
                <asp:ListItem Text="金額依次減免" Value="3"></asp:ListItem>
                </asp:DropDownList></div>
        </th>
    </tr>
	<tr>
	<td colspan="2">
		<table width="100%" class="#">
            <asp:Literal ID="litHtml" runat="server"></asp:Literal>
		</table>
	</td>
	</tr>
	<tr>
		    <th colspan="2">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                <asp:DropDownList ID="ddlMemo" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlMemo_SelectedIndexChanged" >
                    <asp:ListItem Value="1" Selected="True">直接從原收入科目扣除金額</asp:ListItem>
                    <asp:ListItem Value="2">將扣除金額放入另一個減項收入科目</asp:ListItem>
                </asp:DropDownList>
                <cc:MyLabel ID="labReceiveItem" runat="server" LocationText="減項所屬的收入科目：" />
                <asp:DropDownList ID="ddlReceiveItem" runat="server"></asp:DropDownList>
        </ContentTemplate>
    </asp:UpdatePanel>
		    </th>
	</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server" ></cc:MyGoBackButton>
</div>
</asp:Content>
