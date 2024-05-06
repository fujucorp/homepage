<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Unionpay.aspx.cs" Inherits="eSchoolWeb.Unionpay" %>

<%@ Register Src="~/UserControl/UCPageNews.ascx" TagPrefix="uc" TagName="UCPageNews" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>
<link href="css/whole.css" rel="stylesheet" type="text/css" />
<link href="css/index.css" rel="stylesheet" type="text/css" />
<style type="text/css">
#webTitle a, .title p, .title span, .masthead p, .masthead_s p, #channel a, #menu a, #logOut a, .button  {
	font-family: 蘋果儷中黑, 微軟正黑體, 新細明體, Arial, Helvetica, sans-serif;
	}
html {
    display: none;
}
</style>
<script type="text/javascript" language="javascript" src="./js/jquery-3.5.1.min.js"></script>
<script type="text/javascript" language="javascript" src="./js/jquery.blockUI.js"></script>
    <script  type="text/javascript">
        if (self === top) {
            document.documentElement.style.display = 'initial';
        }
        else {
            top.location = decodeURIComponent(self.location);
        }
    </script>
</head>

<body>
<form id="form1" runat="server">
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
                    <p></p><span><%= GetLocalized("銀聯卡繳費") %></span>
                </div>
                
                <div id="loginBox">
                	<table>
                      <tr>
                        <th><%= GetLocalized("虛擬帳號") %>：</th>
                        <td><asp:TextBox ID="txtCancelNo" runat="server" TextMode="Password"></asp:TextBox></td>
                      </tr>
                      <tr>
                        <th><%= GetLocalized("圖型驗證碼") %>：</th>
                        <td>
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/ValidatePicPage.aspx" />
                            <asp:TextBox ID="txtValidateNum" runat="server" MaxLength="4" Width="90px"></asp:TextBox><br />
                            <span style="font-size:10pt;">(<%= GetLocalized("點上圖可更新驗證碼") %>)</span>
                        </td>
                      </tr>
                      <tr>
                        <th>&nbsp;</th>
                        <td>
                        	<div class="button"><cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton><a href="javascript:form1.reset();"><%= GetLocalized("重填") %></a></div>
                        </td>
                      </tr>
                    </table>
                </div> 
                
                <uc:UCPageNews runat="server" ID="UCPageNews" />
            </div>
            <!--end of main-->
        
        </div><!--end of content-->
    </div><!--end of bg-->
   
    <uc:UCFooter runat="server" ID="UCFooter" />


</div>
    </form>
</body>
</html>
