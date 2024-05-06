<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="file_download.aspx.cs" Inherits="eSchoolWeb.file_download" %>

<%@ Register Src="~/UserControl/UCPageNews.ascx" TagPrefix="uc" TagName="UCPageNews" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/whole.css" rel="stylesheet" type="text/css" />
    <link href="/css/index.css" rel="stylesheet" type="text/css" />
    <link href="/css/content.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #webTitle a, .title p, .title span, .masthead p, .masthead_s p, #channel a, #menu a, #logOut a, .button {
            font-family: 蘋果儷中黑, 微軟正黑體, 新細明體, Arial, Helvetica, sans-serif;
        }
        html {
            display: none;
        }
    </style>
    <script type="text/javascript" language="javascript" src="/js/jquery-3.5.1.min.js"></script>
    <script type="text/javascript" language="javascript" src="/js/jquery.blockUI.js"></script>
    <script  type="text/javascript">
        if (self === top) {
            document.documentElement.style.display = 'initial';
        }
        else {
            top.location = '/file_download.aspx';
        }
    </script>
</head>

<body>
    <form id="form2" runat="server">

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
                            <span><%= GetLocalized("檔案下載") %></span>
                        </div>

                        <asp:GridView ID="gvResult" runat="server" CssClass="modify"
                            AutoGenerateColumns="false" AllowPaging="false"
                            RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
                            OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender">
                            <Columns>
                                <cc:MyBoundField DataField="Explain" LocationHeaderText="檔案說明">
                                </cc:MyBoundField>
                                <cc:MyTemplateField LocationHeaderText="下載">
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    <ItemTemplate>
                                        <cc:MyDownloadButton ID="ccbtndownload" runat="server" CssClass="btn"></cc:MyDownloadButton>
                                    </ItemTemplate>
                                </cc:MyTemplateField>
                            </Columns>
                        </asp:GridView>

                        
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
