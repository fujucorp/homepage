<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mobile.Master" AutoEventWireup="true" CodeBehind="SMMBarcode.aspx.cs" Inherits="eSchoolWeb.student.SMMBarcode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div style="padding:10px 5px; width:95%; margin: auto;">
	<span style="float: left; display:inline-block; margin:2px 10px;"><%=this.GetLocalized("學號") %>：<asp:Label ID="labStuId" runat="server" Text=""></asp:Label></span>
	<span style="float: left; display:inline-block; margin:2px 10px;"><%=this.GetLocalized("姓名") %>：<asp:Label ID="labStuName" runat="server" Text=""></asp:Label></span>
	<span style="float: left; display:inline-block; margin:2px 10px;"><%=this.GetLocalized("繳費期限") %>：<asp:Label ID="labSMPayDueDate" runat="server" Text=""></asp:Label></span>
	<span style="float: left; display:inline-block; margin:2px 10px;"><%=this.GetLocalized("繳費金額") %>：<asp:Label ID="labReceiveAmount" runat="server" Text=""></asp:Label></span>
	<span style="float: left; display:inline-block; margin:2px 10px;"><%=this.GetLocalized("虛擬帳號") %>：<asp:Label ID="labCancelNo" runat="server" Text=""></asp:Label></span>
</div>
<div style="clear: both; padding:10px 5px; width:500px; margin: auto;">
	<span style="display:inline-block; padding:5px 0px 15px 20px; font-size:12pt;"><%=this.GetLocalized("便利商店繳款") %></span>
	<span style="padding:0px 10px;"><asp:Image ID="imgSMMBarcode" runat="server" /></span>
	<span style="display:inline-block; padding:5px 0px 15px 20px; font-size:12pt;">＊<%=this.GetLocalized("便利商店收據請保留六個月") %></span>
</div>
</asp:Content>
