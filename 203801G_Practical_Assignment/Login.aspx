<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="_203801G_Practical_Assignment.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://www.google.com/recaptcha/api.js?render=6LfoDGAeAAAAABEqaSO6APz9McEcLt50dM5dbmD0"></script>
</head>
<body>
    <form id="form1" runat="server">
        <h2>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Login"></asp:Label>
        <br />
        <br />
   </h2>
        <table class="style1">
            <tr>
                <td class="style3">
        <asp:Label ID="Label2" runat="server" Text="Email"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_userid" runat="server" Height="32px" Width="280px" ValidationGroup="Val"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server" ErrorMessage="Invalid email format" ValidationExpression="^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$" ControlToValidate="tb_userid" ForeColor="Red" ValidationGroup="Val"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="style3">
        <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_pwd" runat="server" Height="32px" Width="281px" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3">
       
                </td>
                <td class="style2">
    <asp:Button ID="btn_Submit" runat="server" Height="32px" OnClick="btn_submit_click" Text="Submit" Width="288px" ValidationGroup="Val"/>
                </td>
            </tr>
            <tr>
                <td>

                </td>
                <td>
                    <asp:Label ID="lb_error" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    -------------------------or-------------------------
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="btn_register" runat="server" Height="32px" OnClick="btn_register_click" Text="Register" Width="288px" />
                </td>
            </tr>
    </table>
        <br />
    <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LfoDGAeAAAAABEqaSO6APz9McEcLt50dM5dbmD0', { action: 'Login' }).then(function (token) {
            document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>
