<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="M0003.aspx.cs" Inherits="eSchoolWeb.M0003" MasterPageFile="MasterPage/Mobile.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script  type="text/javascript">
    function reloadCaptcha() {
        var src = "ValidatePicPage.aspx?" + (new Date()).getTime();
        $("#imgCaptcha").attr("src", src);
    }
</script>

    <div class="title">
        <p></p>
        <span><%= GetLocalized("信用卡繳費") %></span>
    </div>

    <div id="loginBox">
        <table id="tabStep1" runat="server">
            <tr>
                <th><%= GetLocalized("持卡人身分證字號") %>： </th>
                <td><asp:TextBox ID="tbxS1PID" runat="server" MaxLength="10"></asp:TextBox></td>
            </tr>
            <tr>
                <th><%= GetLocalized("發卡行") %>：</th>
                <td><asp:DropDownList ID="ddlS1Bank" runat="server" Height="24px" Width="164px" style="font-size: 12pt;"></asp:DropDownList></td>
            </tr>
            <tr>
                <th><%= GetLocalized("虛擬帳號") %>：</th>
                <td><asp:TextBox ID="tbxS1CancelNo" runat="server" MaxLength="16"></asp:TextBox></td>
            </tr>
            <tr>
                <th><%= GetLocalized("圖型驗證碼") %>：</th>
                <td>
                    <img id="imgCaptcha" src="ValidatePicPage.aspx?<%= DateTime.Now.Ticks %>" style="vertical-align:bottom" alt="圖形驗證" onclick="reloadCaptcha()" />
                    <asp:TextBox ID="txtValidateCode" runat="server" MaxLength="6" Width="90px" style="text-transform: uppercase;" EnableViewState="false"></asp:TextBox><br />
                    <span style="font-size:10pt;">(<%= GetLocalized("點上圖可更新驗證碼") %>)</span>
                </td>
            </tr>
            <tr>
                <th>&nbsp;</th>
                <td>
                    <div class="button">
                        <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>
                        <a href="javascript:form1.reset();"><%= GetLocalized("重填") %></a>
                    </div>
                </td>
            </tr>
        </table>

        <table id="tabStep2" runat="server">
            <tr>
                <th><%= GetLocalized("學校名稱") %>： </th>
                <td><asp:TextBox ID="tbxS2School" runat="server" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <th><%= GetLocalized("學生姓名") %>： </th>
                <td><asp:TextBox ID="tbxS2Student" runat="server" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <th><%= GetLocalized("虛擬帳號") %>： </th>
                <td><asp:TextBox ID="tbxS2CancelNo" runat="server" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <th><%= GetLocalized("應繳金額") %>： </th>
                <td><asp:TextBox ID="tbxS2Amount" runat="server" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <th><%= GetLocalized("發卡銀行") %>：</th>
                <td><asp:TextBox ID="tbxS2Bank" runat="server" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <th><%= GetLocalized("持卡人身分證字號") %>： </th>
                <td><asp:TextBox ID="tbxS2PayerId" runat="server" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="button">
                        <cc:MyLinkButton ID="ccbtnPay" runat="server" OnClick="ccbtnPay_Click" LocationText="繳費"></cc:MyLinkButton>&nbsp;&nbsp;
                        <cc:MyLinkButton ID="ccbtnGoBack" runat="server" OnClick="ccbtnGoBack_Click" LocationText="回上一頁"></cc:MyLinkButton>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2"><asp:Label ID="labErrMsg" runat="server" Text="" Font-Size="16px"></asp:Label></td>
            </tr>
        </table>
    </div>
</asp:Content>
