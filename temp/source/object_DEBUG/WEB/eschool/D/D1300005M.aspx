<%@ Page Title="土地銀行 - 代收學雜費服務網 - 就貸收費標準" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1300005M.aspx.cs" Inherits="eSchoolWeb.D.D1300005M" %>

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
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" YearVisible="true" TermVisible="true" AutoGetDataBound="true" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Label" />
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<!--//表格_修改----------------------------------------------------------------->
<table id="condition" class="condition" summary="表格_修改" width="100%">
    <tr>
		<th width="100"><div align="left"><cc:MyLabel ID="MyLabel1" runat="server" LocationText="就貸名稱" />：</div></th>
        <td><asp:DropDownList ID="ddlLoanId" runat="server"></asp:DropDownList></td>
    </tr>
	<tr>
		<td colspan="2">
			<table width="100%" class="#">
			    <tr><th><div align="center"><div align="left"><cc:MyLabel ID="MyLabel2" runat="server" LocationText="就貸項目" /></div></div></th><th><div align="center">加入就貸標準</div></th></tr>

                <asp:Literal ID="litHtml" runat="server"></asp:Literal>
			</table>
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<table width="100%" class="#">
            <div id="divLoanAmount" runat="server">
			    <tr>
                    <td><cc:MyLabel ID="MyLabel7" runat="server" LocationText="除可就貸收入科目外" />，
                        <cc:MyLabel ID="MyLabel8" runat="server" LocationText="額外之允許扣抵金額" />：
                        <asp:TextBox ID="tbxLoanAmount" Text="0" runat="server"></asp:TextBox>
                    </td>
			    </tr>
            </div>
			<tr>
                <td>
                    <cc:MyLabel ID="MyLabel6" runat="server" LocationText="是否先行繳費" />：
                    <asp:RadioButtonList ID="rdoPayFlag" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdoPayFlag_SelectedIndexChanged">
                        <asp:ListItem Value="Y">是</asp:ListItem>
                        <asp:ListItem Selected="True" Value="N">否</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
			</tr>
			<tr>
                <td><cc:MyLabel ID="MyLabel9" runat="server" LocationText="就貸可貸金額依" />：
                    <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rdoTakeOffReduce" runat="server">
                        <asp:ListItem Value="N" Selected="True">減免前</asp:ListItem>
                        <asp:ListItem Value="Y">減免後</asp:ListItem>
                    </asp:RadioButtonList>&nbsp;&nbsp;&nbsp;&nbsp;繳費金額計算
                </td>
			</tr>
			<tr>
                <td style="word-break:keep-all;">
                    <asp:DropDownList ID="ddlMemo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMemo_SelectedIndexChanged">
                        <asp:ListItem Value="1">直接從原收入科目扣除金額</asp:ListItem>
                        <asp:ListItem Value="2" Selected="True">將扣除金額放入另一個減項收入科目</asp:ListItem>
                    </asp:DropDownList>
                    <cc:MyLabel ID="labReceiveItem" runat="server" LocationText="減項所屬的收入科目：" />
                    <asp:DropDownList ID="ddlReceiveItem" runat="server"></asp:DropDownList>
                </td>
			</tr>
			</table>
		</td>
	</tr>
</table>          
        </ContentTemplate>
    </asp:UpdatePanel>    
<br />

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
