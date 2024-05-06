<%@ Page Title="土地銀行 - 代收學雜費服務網 - 財金QRCode支付相關設定" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600017.aspx.cs" Inherits="eSchoolWeb.S.S5600017" %>

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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table id="result" class="result" summary="查詢結果" width="100%">
                <tr>
                    <th width="30%"><cc:MyLabel ID="cclabMerchantName" runat="server" LocationText="特店名稱"></cc:MyLabel>：</th>
                    <td>
                        <asp:TextBox ID="tbxMerchantName" runat="server" Width="80%" MaxLength="30"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><cc:MyLabel ID="cclabMerchantId" runat="server" LocationText="特店代號"></cc:MyLabel>：</th>
                    <td>
                        <asp:TextBox ID="tbxMerchantId" runat="server" Width="200px" MaxLength="15" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><cc:MyLabel ID="cclabTerminalId" runat="server" LocationText="端末代號"></cc:MyLabel>：</th>
                    <td>
                        <asp:TextBox ID="tbxTerminalId" runat="server" Width="100px" MaxLength="8" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><cc:MyLabel ID="cclabCostId" runat="server" LocationText="費用代號"></cc:MyLabel>：</th>
                    <td>
                        <asp:TextBox ID="tbxCostId" runat="server" Width="100px" MaxLength="8" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <cc:MyLabel ID="cclabSecureCode" runat="server" LocationText="安全碼"></cc:MyLabel>：</th>
                    <td>
                        <asp:TextBox ID="tbxSecureCode" runat="server" Width="200px" MaxLength="12" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><cc:MyLabel ID="cclabPaymentType" runat="server" LocationText="支付工具型態"></cc:MyLabel>：</th>
                    <td>
                        <asp:TextBox ID="tbxPaymentType" runat="server" Width="100px" MaxLength="2" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><cc:MyLabel ID="cclabCountryCode" runat="server" LocationText="國別碼"></cc:MyLabel>：</th>
                    <td>
                        <asp:TextBox ID="tbxCountryCode" runat="server" Width="100px" MaxLength="3" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><cc:MyLabel ID="cclabCharge" runat="server" LocationText="使用者支付手續費"></cc:MyLabel>：</th>
                    <td>
                        <asp:TextBox ID="tbxCharge" runat="server" Width="100px" MaxLength="7" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><cc:MyLabel ID="cclabPayUrl" runat="server" LocationText="繳費網址"></cc:MyLabel>：</th>
                    <td>
                        <asp:TextBox ID="tbxPayUrl" runat="server" Width="90%" MaxLength="100" ></asp:TextBox>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="button">
        <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
        <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
    </div>
</asp:Content>
