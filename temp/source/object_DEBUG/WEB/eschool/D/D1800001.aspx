<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1800001.aspx.cs" Inherits="eSchoolWeb.D.D1800001" %>
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
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Option" />
<br />

<!--//表格_修改----------------------------------------------------------------->
<table id="modify" class="modify" summary="表格_修改" width="100%">
<asp:Panel ID="panList" runat="server">
<tr>
    <th width="100%" colspan="2"><div style="color:white" align="center">請選擇欲刪除的代碼檔：</div></th>
</tr>
<tr>
    <td width="50%" ><asp:CheckBox ID="chkDeptList" runat="server" />部別代碼</td>
    <td width="50%" ><asp:CheckBox ID="chkCollegeList" runat="server" />院別代碼</td>
</tr>
<tr>
    <td width="50%" ><asp:CheckBox ID="chkMajorList" runat="server" />科系代碼</td>
    <td width="50%" ><asp:CheckBox ID="chkClassList" runat="server" />班別代碼</td>
</tr>
<tr>
    <td width="50%" ><asp:CheckBox ID="chkReduceList" runat="server" />減免類別代碼</td>
    <td width="50%" ><asp:CheckBox ID="chkDormList" runat="server" />住宿代碼</td>
</tr>
<tr>
    <td width="50%" ><asp:CheckBox ID="chkLoanList" runat="server" />就貸代碼</td>
    <td width="50%" ><asp:CheckBox ID="chkReturnList" runat="server" />退費代碼</td>
</tr>
<tr>
    <td width="50%" ><asp:CheckBox ID="chkIdentifyList" runat="server" />身分註記代碼</td>
    <td width="50%" ><asp:CheckBox ID="chkReceiveList" runat="server" />代收費用別代碼</td>
</tr>
<tr>
    <td width="100%" colspan="2"><asp:CheckBox ID="chkSchoolRid" runat="server" />代收費用別檔 (商家代號費用)</td>
</tr>
</asp:Panel>

<asp:Panel ID="panStandard" runat="server">
<tr>
    <th width="100%" colspan="2"><div style="color:white" align="center">請選擇欲刪除的標準檔：</div></th>
</tr>
<tr>
    <td width="50%" ><asp:CheckBox ID="chkGeneralStandard" runat="server" />一般收費標準</td>
    <td width="50%" ><asp:CheckBox ID="chkCredit2Standard" runat="server" />小於基數其他收費標準</td>
</tr>
<tr>
    <td width="50%" ><asp:CheckBox ID="chkCreditStandard" runat="server" />學分費收費標準</td>
    <td width="50%" ><asp:CheckBox ID="chkDormStandard" runat="server" />住宿費收費標準</td>
</tr>
<tr>
    <td width="50%" ><asp:CheckBox ID="chkCreditbStandard" runat="server" />學分費基準收費標準</td>
    <td width="50%" ><asp:CheckBox ID="chkIdentifyStandard" runat="server" />身分註記收費標準</td>
</tr>
<tr>
    <td width="50%" ><asp:CheckBox ID="chkClassStandard" runat="server" />課程收費標準</td>
    <td width="50%" ><asp:CheckBox ID="chkReduceStandard" runat="server" />減免收費標準</td>
</tr>
<tr>
    <td width="50%" ><asp:CheckBox ID="chkLoanStandard" runat="server" />就貸收費標準</td>
    <td width="50%" ><asp:CheckBox ID="chkReturnStandard" runat="server" />退費收費標準</td>
</tr>
</asp:Panel>
<tr>
    <th width="100%" colspan="2"><div style="color:white" align="center">處理結果：</div></th>
</tr>
<tr>
    <td width="100%" colspan="2"><asp:Label ID="labResultMsg" runat="server" ></asp:Label></td>
</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>
</div>

</asp:Content>
