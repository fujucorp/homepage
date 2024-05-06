<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="eSchoolWeb.ErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="respond">
<span>注意</span>
<asp:Literal ID="litMessage" runat="server"></asp:Literal>
</div>
</asp:Content>
