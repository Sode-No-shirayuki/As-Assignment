<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="unlock.aspx.cs" Inherits="AS_Assignment.unlock" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Label ID="Label2" runat="server" Text="Enter email: "></asp:Label>
<asp:TextBox ID="tb_email" runat="server" TextMode="Email"></asp:TextBox>
<br />
<asp:Label ID="Label1" runat="server" Text="Enter Date of birth: "></asp:Label>
<asp:TextBox ID="tb_dob" runat="server" TextMode="Date"></asp:TextBox>
<br />
<asp:Label ID="lbl_errorMsg" runat="server"></asp:Label>
<br />
<asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" Text="Submit" />

</asp:Content>
