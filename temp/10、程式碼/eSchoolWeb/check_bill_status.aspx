<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="check_bill_status.aspx.cs" Inherits="eSchoolWeb.check_bill_status" %>

<%@ Register Src="~/UserControl/UCPageNews.ascx" TagPrefix="uc" TagName="UCPageNews" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/whole.css" rel="stylesheet" type="text/css" />
    <link href="/css/index.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #webTitle a, .title p, .title span, .masthead p, .masthead_s p, #channel a, #menu a, #logOut a, .button {
            font-family: 蘋果儷中黑, 微軟正黑體, 新細明體, Arial, Helvetica, sans-serif;
        }
        .uppercase {
            text-transform: uppercase;
        }
        html {
            display: none;
        }
    </style>
    <script type="text/javascript" language="javascript" src="/js/jquery-3.5.1.min.js"></script>
    <script type="text/javascript" language="javascript" src="/js/jquery.blockUI.js"></script>
    <script type="text/javascript" language="javascript" src="/js/common.js"></script>
    <script  type="text/javascript">
        if (self === top) {
            document.documentElement.style.display = 'initial';
        }
        else {
            top.location = decodeURIComponent(self.location);
        }
    </script>
    <script  type="text/javascript">
        function checkInput(event) {
            if (event.keyCode == 13) {
                var oInput1 = $("#<%=txtCancelNo.ClientID%>");
                if (oInput1.val().trim() == "") {
                    oInput1.focus();
                    return false;
                }
                var oInput2 = $("#<%=tbxStuId.ClientID%>");
                if (oInput2.val().trim() == "") {
                    oInput2.focus();
                    return false;
                }
                var oInput3 = $("#<%=txtValidateCode.ClientID%>");
                if (Input3.val().trim() == "") {
                    oInput3.focus();
                    return false;
                }

                $("#<%=ccbtnOK.ClientID%>")[0].click();
                return false;
            }
        }
        function reloadCaptcha() {
            var src = "ValidatePicPage.aspx?" + (new Date()).getTime();
            $("#imgCaptcha").attr("src", src);
        }
    </script>
</head>

<body>
    <form id="form1" runat="server" onkeypress="javascript:return checkInput(event);">
        <div id="container">

            <!--begin of header--------------------------------------------------------------------------------------------------------------------------------------------->
            <div id="header">
                <uc:UCEntryPageHeader runat="server" ID="UCEntryPageHeader" />
            </div>
            <!--end of header-->

            <!--begin of content-------------------------------------------------------------------------------------------------------------------------------------------->
            <div id="bg">
                <div id="content">

                    <!--begin of menu------------------------------------------------------------------>
                    <div id="menu">
                        <uc:UCEntryPageMenu runat="server" ID="UCEntryPageMenu" />
                    </div>
                    <!--end of menu-->

                    <!--begin of main------------------------------------------------------------------>
                    <div id="main">
                        <div class="title">
                            <p></p>
                            <span><%= GetLocalized("查詢繳費狀態") %></span>
                        </div>

                        <div id="loginBox">
                            <table id="result" class="result" summary="查詢結果" width="100%">
                                <tr>
                                    <th><%= GetLocalized("虛擬帳號") %>：</th>
                                    <td><asp:TextBox ID="txtCancelNo" runat="server" MaxLength="16" autocomplete="off"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <th><%= GetLocalized("學號") %>：</th>
                                    <td><asp:TextBox ID="tbxStuId" runat="server" MaxLength="20" autocomplete="off"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <th><%= GetLocalized("圖型驗證碼") %>：</th>
                                    <td>
                                        <img id="imgCaptcha" src="ValidatePicPage.aspx?<%= DateTime.Now.Ticks %>" style="vertical-align:bottom" alt="圖形驗證" onclick="reloadCaptcha()" />
                                        <asp:TextBox ID="txtValidateCode" runat="server" MaxLength="6" Width="90px" CssClass="uppercase" EnableViewState="false"></asp:TextBox><br />
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

                        <uc:UCPageNews runat="server" ID="UCPageNews" />
                    </div>
                    <!--end of main-->

                </div>
                <!--end of content-->
            </div>
            <!--end of bg-->

            <uc:UCFooter runat="server" ID="UCFooter" />

        </div>
    </form>
</body>
</html>
