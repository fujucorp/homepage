<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCLoginBox.ascx.cs" Inherits="eSchoolWeb.UCLoginBox" %>

<div class="cote">
    <p id="pTitle" runat="server"></p>
</div>
<div id="loginBox">
    <div id="divBank" runat="server" visible="false">
        <table>
        <tr>
            <th><asp:Label ID="Label2" runat="server" Text="使用者代號"></asp:Label>：</th>
            <td><asp:TextBox ID="tbxBankUID" runat="server" MaxLength="20"  Width="60px"></asp:TextBox></td>
        </tr>
        <tr>
            <th><asp:Label ID="Label3" runat="server" Text="使用者密碼"></asp:Label>：</th>
            <td><asp:TextBox ID="tbxBankPXX" runat="server" MaxLength="8"  Width="60px"></asp:TextBox></td>
        </tr>
        <tr>
            <th>&nbsp;</th>
            <td class="button">
                <asp:LinkButton ID="lbtnBankLogon" runat="server" CssClass="btn" Text="確認" OnClick="lbtnBankLogon_Click"></asp:LinkButton>
            </td>
        </tr>
        </table>
    </div>
    <div id="divSchool" runat="server" visible="false">
        <table>
        <tr>
            <th><asp:Label ID="labUnitId" runat="server" Text="統一編號"></asp:Label>： </th>
            <td><asp:DropDownList ID="ddlUnit" runat="server" Width="90px"></asp:DropDownList></td>
        </tr>
        <tr>
            <th><asp:Label ID="labUserId" runat="server" Text="使用者代號"></asp:Label>：</th>
            <td><input type="text" id="tbxUserId" runat="server" maxlength="20" size="10" /></td>
        </tr>
        <tr>
            <th><asp:Label ID="labUserPXX" runat="server" Text="使用者密碼"></asp:Label>：</th>
            <td><input type="password" id="tbxUserPXX" runat="server" maxlength="32" size="8" /></td>
        </tr>
        <tr>
            <th>&nbsp;</th>
            <td class="button">
                <asp:LinkButton ID="lbtnSchoolLogon" runat="server" CssClass="btn" Text="確認" OnClick="lbtnSchoolLogon_Click"></asp:LinkButton>
            </td>
        </tr>
        </table>
    </div>
    <div id="divStudent" runat="server" visible="false">
        <table>
        <tr>
            <th><asp:Label ID="labSchool" runat="server" Text="學校"></asp:Label>： </th>
            <td id="tdSchool"><asp:DropDownList ID="ddlSchool" runat="server" Width="90px"></asp:DropDownList></td>
        </tr>
        <tr>
            <th><asp:Label ID="labStuId" runat="server" Text="學號"></asp:Label>：</th>
            <td><asp:TextBox ID="tbxStuId" MaxLength="20" Width="60px" size="10" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <th id="thLoginKeyName"><asp:Label ID="labLoginKey" runat="server" Text="使用者密碼"></asp:Label>：</th>
            <td><asp:TextBox ID="tbxLoginKey" TextMode="Password" MaxLength="20" Width="60px" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <th>&nbsp;</th>
            <td class="button">
                <asp:LinkButton ID="lbtnStudentLogon" runat="server" CssClass="btn" Text="確認" OnClick="lbtnStudentLogon_Click"></asp:LinkButton>
            </td>
        </tr>
        </table>
        <script type="text/javascript">
            <%--var keyTypeNames = [<%= this.GetKeyTypeNames() %>];
            $(function () {
                $('#tdSchool > select').change(function () {
                    var code = $(this).val();
                    var keyType = code.substr(0, 2);
                    var nameIndex = 0;
                    switch (keyType) {
                        case "0-":
                            nameIndex = 0;
                            break;
                        case "1-":
                            nameIndex = 1;
                            break;
                        case "2-":
                            nameIndex = 2
                            break;
                    }
                    $('#thLoginKeyName > span').text(keyTypeNames[nameIndex]);
                });
            });--%>
        </script>
    </div>
</div>
