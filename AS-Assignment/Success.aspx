<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="AS_Assignment.Success" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:100%;">
        <tr>
            <td>User Profile</td>
        </tr>
        <tr>
            <td style="width: 228px">Email:</td>
            <td>
                <asp:Label ID="lbl_email" runat="server"></asp:Label>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 228px">Credit Card Number: </td>
            <td>
                <asp:Label ID="lbl_credit_card" runat="server"></asp:Label>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 228px">Date of Birth:</td>
            <td>
                <asp:Label ID="lbl_dob" runat="server"></asp:Label>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="btn_logout" runat="server" OnClick="btn_logout_Click" Text="Logout" />
            </td>
        </tr>
    </table>
</asp:Content>
