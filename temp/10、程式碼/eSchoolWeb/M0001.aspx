<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="M0001.aspx.cs" Inherits="eSchoolWeb.M0001" MasterPageFile="MasterPage/Mobile.Master" %>

<%@ Register Src="~/UserControl/UCPageNews.ascx" TagPrefix="uc" TagName="UCPageNews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript">
$(function() {
    var datas = [
      <%=_SchoolItemsJson%>
    ];

    function changeSchType() {
        var schType = $('#selSchType').val().trim();
        var jqSch = $('#selSchIdenty');
        if (jqSch) {
            jqSch.empty();
            $.each(datas, function (index, data) {
                if (data.schType == schType) {
                    jqSch.append($("<option/>", {
                        value: data.value,
                        text: data.label
                    }));
                }
            });
            changeSchIdenty();
        }
    }

    function changeSchIdenty() {
        var schIdenty = $('#selSchIdenty').val().trim();
        $.each(datas, function (index, data) {
            if (data.value == schIdenty) {
                var loginKeyKind = data.pwdkind;
                if (loginKeyKind == "B") {
                    $("#thPWDKind").html("<%= GetLocalized("生日_Mobile") %>：");
                    $("#spnPWDMemo").html("(<%= GetLocalized("例如生日為2015年1月20日請輸入20150120") %>)");
                } else if (loginKeyKind == "I") {
                    $("#thPWDKind").html("<%= GetLocalized("身份證字號_Mobile") %>：");
                    $("#spnPWDMemo").html("");
                } else if (loginKeyKind == "B2P" || loginKeyKind == "I2P") {
                    $("#thPWDKind").html("<%= GetLocalized("使用者密碼_Mobile") %>：");
                    $("#spnPWDMemo").html("");
                } else {
                    $("#thPWDKind").html("<%= GetLocalized("使用者密碼_Mobile") %>：");
                    $("#spnPWDMemo").html("");
                }
                return false;
            }
        });
    }

    $('#selSchType').change(changeSchType);
    $('#selSchIdenty').change(changeSchIdenty);

    changeSchType();
});
</script>
<script  type="text/javascript">
    function reloadCaptcha() {
        var src = "ValidatePicPage.aspx?" + (new Date()).getTime();
        $("#imgCaptcha").attr("src", src);
    }
</script>

    <div class="title">
        <p></p>
        <span><%= GetLocalized("學生專區登入_Mobile") %></span>
    </div>

    <div id="loginBox">
        <table>
        <tr>
            <th><%= GetLocalized("學校名稱_Mobile") %>：</th>
            <th>
                <asp:TextBox ID="tbxSchName" runat="server" ClientIDMode="Static" Visible="false"></asp:TextBox>
                <asp:HiddenField ID="hidSchIdenty" runat="server" ClientIDMode="Static" Visible="false" />
                <select id="selSchType" name="selSchType" style="font-size:12pt;">
                    <option value="1" selected="selected"><%= GetLocalized("大專院校") %></option>
                    <option value="2"><%= GetLocalized("高中職") %></option>
                    <option value="3"><%= GetLocalized("國中小") %></option>
                    <option value="4"><%= GetLocalized("幼兒園") %></option>
                </select><br />
                <select id="selSchIdenty" name="selSchIdenty" style="font-size:12pt;"></select>
            </th>
        </tr>
        <tr>
            <th><%= GetLocalized("學號_Mobile") %>：</th>
            <td><asp:TextBox ID="tbxStuID" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <th id="thPWDKind"><%= GetLocalized("使用者密碼_Mobile") %>：</th>
            <td>
                <asp:TextBox ID="tbxPWord" runat="server" TextMode="Password"></asp:TextBox><br />
                <span id="spnPWDMemo"></span>
            </td>
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
                    <a href="javascript:void(0)" onclick="document.forms['form1'].reset();"><%= GetLocalized("重填") %></a>
                </div>
            </td>
        </tr>
        </table>

        <uc:UCPageNews runat="server" ID="UCPageNews" />
    </div>
</asp:Content>