<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Oops.aspx.cs" Inherits="eSchoolWeb.Oops" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>土地銀行 - 代收學雜費服務網</title>
    <style type="text/css">
        body {
            font-family: 蘋果儷中黑, 微軟正黑體, 新細明體, Arial, Helvetica, sans-serif;
        }
        section {
            width: 500px;
            height: 400px;
            position: absolute;
            top: 50%;
            left: 50%;
            margin: -150px 0 0 -250px;
            display: table;
        }
        section .text-center {
            display: table-cell;
            vertical-align: middle;
            text-align:center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="container">
        <div id="header">
            <div id="logo"><img src="/img/logo.gif" /></div>
        </div>
        <div style="height:20px; width:100%; background-color:greenyellow;"></div>
        <section>
            <div class="text-center">
                <img src="/img/Oops.png" border="0" />
                <h2>抱歉！找不到您指定的網頁內容。<br/>請使用正常順序操作。</h2>
            </div>
        </section>
    </div>
    </form>
</body>
</html>
