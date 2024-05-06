<%@ Page Title="土地銀行 - 代收學雜費服務網 - 身分註記收費標準" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1300008M.aspx.cs" Inherits="eSchoolWeb.D.D1300008M" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" YearVisible="true" TermVisible="true" AutoGetDataBound="true" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Label" />
    
<!--//表格_修改----------------------------------------------------------------->
<table id="condition" class="condition" summary="表格_修改" width="100%">
    <tr>
		<th><cc:MyLabel ID="MyLabel1" runat="server" LocationText="身分註記別" />：</th>
        <td width="70%"><asp:DropDownList ID="ddlIdentifyId" runat="server" AutoPostBack="true" ></asp:DropDownList></td>
    </tr>
	<tr>
		    <th><cc:MyLabel ID="MyLabel2" runat="server" LocationText="註記方式" />：</th>
            <td><asp:DropDownList ID="ddlIdWay" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlIdWay_SelectedIndexChanged">
                    <asp:ListItem Value="1">依百分比計算</asp:ListItem>
                    <asp:ListItem Value="2">依金額計算</asp:ListItem>
                </asp:DropDownList>
            </td>
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
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyLinkButton ID="lbtnBack" runat="server" Text="離開" OnClick="lbtnBack_Click"></cc:MyLinkButton>
</div>
</asp:Content>
