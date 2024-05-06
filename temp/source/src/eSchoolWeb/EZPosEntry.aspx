<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EZPosEntry.aspx.cs" Inherits="eSchoolWeb.EZPosEntry" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=big5"/>
    <title></title>
    <style type="text/css">
        html {
            display: none;
        }
    </style>
    <script  type="text/javascript">
        if (self === top) {
            document.documentElement.style.display = 'initial';
        }
        else {
            top.location = '/EZPosEntry.aspx';
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    </form>
    <div id="divForm" runat="server" >

    </div>
</body>
</html>
