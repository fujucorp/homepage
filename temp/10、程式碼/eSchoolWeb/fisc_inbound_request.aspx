<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fisc_inbound_request.aspx.cs" Inherits="eSchoolWeb.fisc_inbound_request" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
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
            top.location = decodeURIComponent(self.location);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Literal ID="litMessage" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
