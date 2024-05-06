<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="M0002.aspx.cs" Inherits="eSchoolWeb.M0002" MasterPageFile="~/MasterPage/Mobile.Master" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
<style type="text/css">
html { display: none; }
</style>
<script type="text/javascript" language="javascript">
    if (self === top) {
        document.documentElement.style.display = 'block';
    }
</script>
<script  type="text/javascript">
    function reloadCaptcha() {
        var src = "ValidatePicPage.aspx?" + (new Date()).getTime();
        $("#imgCaptcha").attr("src", src);
    }
</script>

    <div class="title">
        <p></p>
        <span><%= GetLocalized("查詢繳費狀態") %></span>
    </div>

    <div id="loginBox">
        <table>
            <tr>
                <th><%= GetLocalized("虛擬帳號") %>：</th>
                <td><asp:TextBox ID="txtCancelNo" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <th><%= GetLocalized("學號") %>：</th>
                <td><asp:TextBox ID="tbxStuId" runat="server" MaxLength="20" autocomplete="off"></asp:TextBox></td>
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

        <asp:GridView ID="gvResult" runat="server" CssClass="modify"
            AutoGenerateColumns="false" AllowPaging="false"
            RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
            OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender" Visible="False">
            <Columns>
                <cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號">
                    <HeaderStyle Wrap="False"></HeaderStyle>
                </cc:MyBoundField>
                <cc:MyBoundField DataField="ReceiveAmount" LocationHeaderText="應繳金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                    <HeaderStyle Wrap="False"></HeaderStyle>
                </cc:MyBoundField>
                <cc:MyBoundField DataField="StuId" LocationHeaderText="學號">
                    <HeaderStyle Wrap="False"></HeaderStyle>
                </cc:MyBoundField>
                <cc:MyBoundField DataField="MaskedStuName" LocationHeaderText="學生姓名">
                    <HeaderStyle Wrap="False"></HeaderStyle>
                </cc:MyBoundField>
                <cc:MyBoundField DataField="CancelStatus" LocationHeaderText="繳費狀態">
                    <HeaderStyle Wrap="False"></HeaderStyle>
                </cc:MyBoundField>
            </Columns>
        </asp:GridView>
    </div>
</asp:content>
