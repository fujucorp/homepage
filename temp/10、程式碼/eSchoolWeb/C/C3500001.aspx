<%@ Page Title="土地銀行 - 代收學雜費服務網 - 繳費銷帳報表" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3500001.aspx.cs" Inherits="eSchoolWeb.C.C3500001" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Option" AutoPostBack="true" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" />

<br/>
<!--//表格_修改----------------------------------------------------------------->
<table class="result" summary="查詢條件" width="100%">
<tr>
    <td colspan="2">
        <div align="left"><%=this.GetLocalized("報表名稱") %>：
            <asp:TextBox ID="tbxReportName" runat="server" MaxLength="50" Width="80%"></asp:TextBox>
        </div>
    </td>
</tr>
<tr>
    <td width="50%">
        <div align="left"><%=this.GetLocalized("批號") %>：
            <asp:DropDownList ID="ddlUpNo" runat="server"></asp:DropDownList>
        </div>
    </td>
    <td width="50%">
        <div align="left" id="divReceiveStatus"><%=this.GetLocalized("繳費狀態") %>：
            <asp:DropDownList ID="ddlReceiveStatus" runat="server">
                <asp:ListItem Value="0">未繳</asp:ListItem>
                <asp:ListItem Value="1">已繳</asp:ListItem>
            </asp:DropDownList>
        </div>
    </td>
</tr>
<tr id="trReceiveWay" style="display:none;">
    <td colspan="2">
        <div align="left"><%=this.GetLocalized("繳款方式") %>：
            <asp:DropDownList ID="ddlReceiveWay" runat="server">
                <asp:ListItem Value="1">超商</asp:ListItem>
                <asp:ListItem Value="2">ATM</asp:ListItem>
                <asp:ListItem Value="3">臨櫃</asp:ListItem>
            </asp:DropDownList>
        </div>
    </td>
</tr>
<tr>
    <td colspan="2">
        <div align="left"><%=this.GetLocalized("檔案格式") %>：
            <asp:RadioButtonList ID="rblFileFormat" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Text="Excel 檔 (XLS)　" Value="XLS" Selected="True" />
                <asp:ListItem Text="Calc 檔 (ODS)　" Value="ODS" />
            </asp:RadioButtonList>
        </div>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyLinkButton ID="ccbtnGenRpeortA1" runat="server" LocationText="下載繳費銷帳總表" OnClick="ccbtnGenRpeort_Click"></cc:MyLinkButton>&nbsp;&nbsp;&nbsp;
    <cc:MyLinkButton ID="ccbtnGenRpeortB1" runat="server" LocationText="下載繳費銷帳明細表" OnClick="ccbtnGenRpeort_Click"></cc:MyLinkButton><br /><br />
    <cc:MyLinkButton ID="ccbtnGenRpeortA2" runat="server" LocationText="下載繳費失敗總表(遲繳)" OnClick="ccbtnGenRpeort_Click"></cc:MyLinkButton>&nbsp;&nbsp;&nbsp;
    <cc:MyLinkButton ID="ccbtnGenRpeortB2" runat="server" LocationText="下載繳費失敗明細表(遲繳)" OnClick="ccbtnGenRpeort_Click"></cc:MyLinkButton><br /><br />
    <cc:MyLinkButton ID="ccbtnGenRpeortE1" runat="server" LocationText="下載繳費收費項目明細分析表" OnClick="ccbtnGenRpeort_Click"></cc:MyLinkButton>&nbsp;&nbsp;&nbsp;
    <cc:MyLinkButton ID="ccbtnGenRpeortE2" runat="server" LocationText="下載繳費收費項目分類統計表" OnClick="ccbtnGenRpeort_Click"></cc:MyLinkButton><br /><br />
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
