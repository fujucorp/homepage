<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5100001D.aspx.cs" Inherits="eSchoolWeb.S.S5100001D" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
    <th width="30%">商家代號：</th>
    <td colspan="3"><asp:Label ID="labAPPNO" runat="server" ></asp:Label></td>
</tr>
<tr>
    <th width="30%">實際轉存帳號：</th>
    <td width="20%"><asp:Label ID="labACCTID" runat="server" ></asp:Label></td>
    <th width="30%">營利事業統一編號：</th>
    <td width="20%"><asp:Label ID="labUNINO" runat="server" ></asp:Label></td>
</tr>
<tr>
    <th width="30%">申請日期：</th>
    <td width="20%"><asp:Label ID="labAPYDAY" runat="server" ></asp:Label></td>
    <th width="30%">註銷日期：</th>
    <td width="20%"><asp:Label ID="labCNLDAY" runat="server" ></asp:Label></td>
</tr>
<tr>
    <th width="30%">檢碼方式：</th>
    <td width="20%"><asp:Label ID="labCHKTYPE" runat="server" ></asp:Label></td>
    <th width="30%">交易日期：</th>
    <td width="20%"><asp:Label ID="labTRNDT" runat="server" ></asp:Label></td>
</tr>
<tr>
    <th width="30%">廠商名稱：</th>
    <td width="20%"><asp:Label ID="labCUSTNAME" runat="server" ></asp:Label></td>
    <th width="30%">IP位址：</th>
    <td width="20%"><asp:Label ID="labIPADDR" runat="server" ></asp:Label></td>
</tr>
<tr>
    <th width="30%">E-MAIL位址：</th>
    <td width="20%"><asp:Label ID="labMAILADDR" runat="server" ></asp:Label></td>
    <th width="30%">入金通知記號：</th>
    <td width="20%"><asp:Label ID="labSVCFLG" runat="server" ></asp:Label></td>
</tr>
<tr>
    <th width="30%">入金通知申請日期：</th>
    <td width="20%"><asp:Label ID="labRGDT" runat="server" ></asp:Label></td>
    <th width="30%">入金通知啟用日期：</th>
    <td width="20%"><asp:Label ID="labSTARTDT" runat="server" ></asp:Label></td>
</tr>
<tr>
    <th width="30%">入金通知暫停日期：</th>
    <td width="20%"><asp:Label ID="labSTPDT" runat="server" ></asp:Label></td>
    <th width="30%">入金通知恢復日期：</th>
    <td width="20%"><asp:Label ID="labRTNDT" runat="server" ></asp:Label></td>
</tr>
<tr>
    <th width="30%">入金通知註銷日期：</th>
    <td width="20%"><asp:Label ID="labCNCLDT" runat="server" ></asp:Label></td>
    <th width="30%">虛擬帳號種類：</th>
    <td width="20%"><asp:Label ID="labVIRFLG" runat="server" ></asp:Label></td>
</tr>
<tr>
    <th width="30%">收款期間異動單位：</th>
    <td width="20%"><asp:Label ID="labUPDBRNO" runat="server" ></asp:Label></td>
    <th width="30%">收款期間異動日期：</th>
    <td width="20%"><asp:Label ID="labUPDDATE" runat="server" ></asp:Label></td>
</tr>
<tr>
    <th width="30%">收款期間異動時間：</th>
    <td width="20%"><asp:Label ID="labUPDTIME" runat="server" ></asp:Label></td>
    <th width="30%">收款啟用日期：</th>
    <td width="20%"><asp:Label ID="labOPDATE" runat="server" ></asp:Label></td>
</tr>
<tr>
    <th width="30%">收款啟用時間：</th>
    <td width="20%"><asp:Label ID="labOPTIME" runat="server" ></asp:Label></td>
    <th width="30%">收款停用日期：</th>
    <td width="20%"><asp:Label ID="labSTPDATE" runat="server" ></asp:Label></td>
</tr>
<tr>
    <th width="30%">收款停用時間：</th>
    <td width="20%"><asp:Label ID="labSTPTIME" runat="server" ></asp:Label></td>
    <th width="30%">併登記號：</th>
    <td width="20%"><asp:Label ID="labCMBFLG" runat="server" ></asp:Label></td>
</tr>
<tr>
    <th width="30%">廠商種類：</th>
    <td width="20%"><asp:Label ID="labKIND" runat="server" ></asp:Label></td>
    <th width="30%">繳款通道_臨櫃：</th>
    <td width="20%"><asp:Label ID="labCHNLFGT" runat="server" ></asp:Label></td>
</tr>
<tr>
    <th width="30%">繳款通道_匯款：</th>
    <td width="20%"><asp:Label ID="labCHNLFGR" runat="server" ></asp:Label></td>
    <th width="30%">繳款通道_自動化設備：</th>
    <td width="20%"><asp:Label ID="labCHNLFGM" runat="server" ></asp:Label></td>
</tr>
<tr>
    <th width="30%">FLLER：</th>
    <td colspan="3"><asp:Label ID="labFILLER" runat="server" ></asp:Label></td>
</tr>
</table>

<div class="button">
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
