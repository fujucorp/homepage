<%@ Page Title="土地銀行 - 代收學雜費服務網 - 商家代號費用小記定義檔" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1200002M.aspx.cs" Inherits="eSchoolWeb.D.D1200002M" %>

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
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" AutoGetDataBound="false" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Label" AutoGetDataBound="false" />
<table id="condition" class="condition" summary="表格_修改" width="100%">
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="cclabSumId" runat="server" LocationText="小計代碼"></cc:MyLabel>：</td>
        <td style="text-align:left;" colspan="3">
            <asp:DropDownList ID="ddlSumId" runat="server"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="text-align:right; width:20%;"><cc:MyLabel ID="cclabSumName" runat="server" LocationText="小計中文名稱"></cc:MyLabel>：</td>
        <td style="text-align:left; width:30%;"><asp:TextBox ID="tbxSumName" runat="server" MaxLength="40"></asp:TextBox></td>
        <td style="text-align:right; width:20%;"><cc:MyLabel ID="cclabSumEName" runat="server" LocationText="小計英文名稱"></cc:MyLabel>：</td>
        <td style="text-align:left; width:30%;"><asp:TextBox ID="tbxSumEName" runat="server" MaxLength="40"></asp:TextBox></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="cclabItem01" runat="server" LocationText="收入科目01"></cc:MyLabel>：</td>
        <td style="text-align:left;"><asp:CheckBox ID="cbxReceiveItem01" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="cclabItem02" runat="server" LocationText="收入科目02"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem02" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel5" runat="server" LocationText="收入科目03"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem03" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel6" runat="server" LocationText="收入科目04"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem04" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel7" runat="server" LocationText="收入科目05"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem05" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel8" runat="server" LocationText="收入科目06"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem06" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel9" runat="server" LocationText="收入科目07"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem07" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel10" runat="server" LocationText="收入科目08"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem08" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel11" runat="server" LocationText="收入科目09"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem09" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel12" runat="server" LocationText="收入科目10"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem10" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel13" runat="server" LocationText="收入科目11"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem11" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel14" runat="server" LocationText="收入科目12"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem12" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel15" runat="server" LocationText="收入科目13"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem13" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel16" runat="server" LocationText="收入科目14"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem14" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel17" runat="server" LocationText="收入科目15"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem15" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel18" runat="server" LocationText="收入科目16"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem16" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel19" runat="server" LocationText="收入科目17"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem17" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel20" runat="server" LocationText="收入科目18"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem18" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel21" runat="server" LocationText="收入科目19"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem19" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel22" runat="server" LocationText="收入科目20"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem20" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel23" runat="server" LocationText="收入科目21"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem21" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel24" runat="server" LocationText="收入科目22"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem22" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel25" runat="server" LocationText="收入科目23"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem23" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel26" runat="server" LocationText="收入科目24"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem24" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel27" runat="server" LocationText="收入科目25"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem25" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel28" runat="server" LocationText="收入科目26"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem26" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel29" runat="server" LocationText="收入科目27"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem27" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel30" runat="server" LocationText="收入科目28"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem28" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel31" runat="server" LocationText="收入科目29"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem29" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel32" runat="server" LocationText="收入科目30"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem30" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel33" runat="server" LocationText="收入科目31"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem31" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel34" runat="server" LocationText="收入科目32"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem32" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel35" runat="server" LocationText="收入科目33"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem33" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel36" runat="server" LocationText="收入科目34"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem34" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel37" runat="server" LocationText="收入科目35"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem35" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel38" runat="server" LocationText="收入科目36"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem36" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel39" runat="server" LocationText="收入科目37"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem37" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel40" runat="server" LocationText="收入科目38"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem38" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel41" runat="server" LocationText="收入科目39"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem39" runat="server" /></td>
        <td style="text-align:right;"><cc:MyLabel ID="MyLabel42" runat="server" LocationText="收入科目40"></cc:MyLabel>：</td>
        <td style="text-align:left"><asp:CheckBox ID="cbxReceiveItem40" runat="server" /></td>
    </tr>
</table>

<div class="button">
    <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
