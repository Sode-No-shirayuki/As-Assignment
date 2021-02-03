<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="AS_Assignment.Registration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function validate() {
            var pwd = document.getElementById("<%=tb_pwd.ClientID %>").value;
            if (pwd.length < 8) {
                document.getElementById("<%=lbl_pwdCheck.ClientID %>").innerHTML = "Password length must be at least 8 characters";
                document.getElementById("<%=lbl_pwdCheck.ClientID %>").style.color = "Red";
                return("too_short")
            }
            else if (pwd.search(/[0-9]/) == -1) {
                document.getElementById("<%=lbl_pwdCheck.ClientID %>").innerHTML = "Password require at least 1 number";
                document.getElementById("<%=lbl_pwdCheck.ClientID %>").style.color = "Red";
                return ("no_number");
            }
            else if (pwd.search(/[a-z]/) == -1) {
                document.getElementById("<%=lbl_pwdCheck.ClientID %>").innerHTML = "Password require at least 1 lowercase";
                document.getElementById("<%=lbl_pwdCheck.ClientID %>").style.color = "Red";
                return ("no_uppercase");
            }
            else if (pwd.search(/[A-Z]/) == -1) {
                document.getElementById("<%=lbl_pwdCheck.ClientID %>").innerHTML = "Password require at least 1 uppercase";
                document.getElementById("<%=lbl_pwdCheck.ClientID %>").style.color = "Red";
                return ("no_lowercase");
            }
            else if (pwd.search(/[^A-Za-z0-9]/) == -1) {
                document.getElementById("<%=lbl_pwdCheck.ClientID %>").innerHTML = "Password require at least special characters";
                document.getElementById("<%=lbl_pwdCheck.ClientID %>").style.color = "Red";
                return ("no_specialchar");
            }

            document.getElementById("<%=lbl_pwdCheck.ClientID %>").innerHTML = "Ok";
            document.getElementById("<%=lbl_pwdCheck.ClientID %>").style.color = "Green";
        }
    </script>
    <table class="nav-justified">
        <tr>
            <td>First Name:</td>
            <td style="width: 174px">
                <asp:TextBox ID="tb_fname" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Last Name:</td>
            <td style="width: 174px">
                <asp:TextBox ID="tb_lname" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Credit Card Number:</td>
            <td style="width: 174px">
                <asp:TextBox ID="tb_credit_card" runat="server" TextMode="Number"></asp:TextBox>
            </td>
            <td style="width:100px;">Valid Date:</td>
            <td>
                <asp:TextBox ID="tb_validDate" runat="server" style="margin-left: 0"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 174px">Email:</td>
            <td class="modal-sm" style="width: 174px">
                <asp:TextBox ID="tb_email" runat="server"></asp:TextBox>
            </td>
            <td style="width: 138px">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 174px; height: 22px">Password:</td>
            <td class="modal-sm" style="width: 174px; height: 22px">
                <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password" ></asp:TextBox>
            </td>
            <td style="height: 22px; width: 100px;">
                <asp:Label ID="lbl_pwdCheck" runat="server" ViewStateMode="Enabled"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Date of Birth:</td>
            <td style="width: 174px">
                <asp:TextBox ID="tb_dob" runat="server" TextMode="Date"></asp:TextBox>
            </td>
        </tr>
    </table>
    <asp:Label ID="lbl_errorMsg" runat="server" ForeColor="Red"></asp:Label>
    <br />
    <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" Text="Submit" />
</asp:Content>
