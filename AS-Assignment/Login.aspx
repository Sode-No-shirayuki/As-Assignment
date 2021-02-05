<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AS_Assignment.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://www.google.com/recaptcha/api.js?render="></script>
   
    <table style="width:100%;">
        <tr>
            <td style="width: 174px">Email:</td>
            <td style="width: 226px">
                <asp:TextBox ID="tb_emailLogin" runat="server" TextMode="Email"></asp:TextBox>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 174px">Password:</td>
            <td style="width: 226px">
                <asp:TextBox ID="tb_pwdLogin" runat="server" TextMode="Password"></asp:TextBox>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 174px">&nbsp;</td>
            <td style="width: 226px">
                <asp:Button ID="btn_login" runat="server" OnClick="btn_login_Click" Text="Submit" />
                <asp:Button ID="btn_unlock" runat="server" OnClick="btn_unlock_Click" Text="Unlock" Visible="False" />
            </td>
            <td>
                  <asp:Label ID="lbl_error_msg" runat="server" Text="" ViewStateMode="Enabled"></asp:Label>
            </td>
        </tr>
    </table>
    <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
    <script>
        grecaptcha.ready(function (){
            grecaptcha.execute('6Lfen0caAAAAAD9YV-mh8RsrQu2ShOq3SoOiLm6U', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            })
        })
    </script>
</asp:Content>
