<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="_203801G_Practical_Assignment.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 29px;
        }

    </style>

    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_pwd.ClientID %>').value;

            if (str.length < 12) {
                document.getElementById("lbl_pwdcheck").innerHTML = "Password Length must be at least 12 characters";
                document.getElementById("lbl_pwdcheck").style.color = "Red";
                return ("too_short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_pwdcheck").innerHTML = "Password require at least 1 number";
                document.getElementById("lbl_pwdcheck").style.color = "Red";
                return ("no_number");
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_pwdcheck").innerHTML = "Password require at least 1 Uppercase Character";
                document.getElementById("lbl_pwdcheck").style.color = "Red";
                return ("no_uppercase");
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_pwdcheck").innerHTML = "Password require at least 1 Lowercase Character";
                document.getElementById("lbl_pwdcheck").style.color = "Red";
                return ("no_lowercase");
            }
            else if (str.search(/[^a-zA-Z0-9]/) == -1) {
                document.getElementById("lbl_pwdcheck").innerHTML = "Password require at least 1 Special Character";
                document.getElementById("lbl_pwdcheck").style.color = "Red";
                return ("no_special");
            }
            document.getElementById("lbl_pwdcheck").innerHTML = "";
        }
    </script>
    <script src="https://www.google.com/recaptcha/api.js?render=6LfoDGAeAAAAABEqaSO6APz9McEcLt50dM5dbmD0"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Registration</h1>
                <table class="style1">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_FirstName" runat="server" Text="First Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tb_FirstName" runat="server" Height="32px" Width="280px" ValidationGroup="Val"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorFN" runat="server" ErrorMessage="First Name Required" ControlToValidate="tb_FirstName" ValidationGroup="Val" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_LastName" runat="server" Text="Last Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tb_LastName" runat="server" Height="32px" Width="280px" ValidationGroup="Val"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorLN" runat="server" ErrorMessage="Last Name Required" ControlToValidate="tb_LastName" ValidationGroup="Val" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        
                        <td>
                            <asp:Label ID="lbl_CCName" runat="server" Text="Cardholder Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tb_CCName" runat="server" Height="32px" Width="280px" ValidationGroup="Val"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorCCName" runat="server" ErrorMessage="Cardholder Name Required" ControlToValidate="tb_CCName" ValidationGroup="Val" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_CCNo" runat="server" Text="Credit Card No"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tb_CCNo" runat="server" Height="32px" Width="280px" onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);" maxLength="16" ValidationGroup="Val"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorCCNo" runat="server" ErrorMessage="Credit Card Number Required" ControlToValidate="tb_CCNo" ValidationGroup="Val" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_ExpDate" runat="server" Text="Expiry Date"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_ExpMonth" runat="server" Height="32px" Width="134px">
                                <asp:ListItem default="selected">MM</asp:ListItem>
                                <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label runat="server">/</asp:Label>
                            <asp:TextBox ID="tb_ExpYear" runat="server" Height="32px" Width="134px" placeholder="YY" onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);" maxLength="2" ValidationGroup="Val"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorExp" runat="server" ErrorMessage="Expiry Date Required" ControlToValidate="tb_ExpYear" ValidationGroup="Val" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_CVV" runat="server" Text="CVV"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tb_CVV" runat="server" Height="32px" Width="280px" onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);" maxLength="3" ValidationGroup="Val"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorCVV" runat="server" ErrorMessage="CVV Required" ControlToValidate="tb_CVV" ValidationGroup="Val" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            <asp:Label ID="lbl_Email" runat="server" Text="Email Address"></asp:Label>
                        </td>
                        <td class="auto-style1">
                            <asp:TextBox ID="tb_Email" runat="server" Height="32px" Width="280px" TextMode="Email" ValidationGroup="Val"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmail" runat="server" ErrorMessage="Email Required" ControlToValidate="tb_Email" ValidationGroup="Val" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server" ErrorMessage="Invalid email format" ValidationExpression="^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$" ControlToValidate="tb_Email" ForeColor="Red" ValidationGroup="Val"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            <asp:Label ID="lbl_pwd" runat="server" Text="Password" ValidationGroup="Val"></asp:Label>
                        </td>
                        <td class="auto-style1">
                            <asp:TextBox ID="tb_pwd" runat="server" Height="32px" Width="280px" TextMode="Password" onkeyup="javascript:validate()"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorpwd" runat="server" ErrorMessage="Password Required" ControlToValidate="tb_pwd" ValidationGroup="Val" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Password must be at least 12 characters, contains at least one digit, lowercase, uppercase and special character" ValidationExpression="(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,15}$" ControlToValidate="tb_pwd" ForeColor="Red" ValidationGroup="Val"></asp:RegularExpressionValidator>
                            <br />
                            <asp:Label ID="lbl_pwdcheck" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_DOB" runat="server" Text="Date of Birth"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tb_DOB" runat="server" Height="32px" Width="280px" TextMode="Date" ValidationGroup="Val"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorDOB" runat="server" ErrorMessage="Date Of Birth Required" ControlToValidate="tb_DOB" ValidationGroup="Val" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Photo" runat="server" Text="Upload Photo"></asp:Label>
                        </td>
                        <td>
                            <asp:FileUpload ID="PhotoUpload" runat="server" Height="32px" Width="280px" ValidationGroup="Val" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorPU" runat="server" ErrorMessage="Photo Required" ControlToValidate="PhotoUpload" ValidationGroup="Val" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Only jpg or png allowed" ValidationExpression="^.*\.(jpg|png)$" ControlToValidate="PhotoUpload" ForeColor="Red" ValidationGroup="Val"></asp:RegularExpressionValidator>
                            <asp:Label runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btn_submit" runat="server" Text="Submit" Height="32px" Width="285px" OnClick="btn_submit_click" ValidationGroup="Val"/>
                        </td>
                    </tr>
                </table>             
        </div>
        <asp:Label runat="server" ID="lb_error1" ForeColor="Red"></asp:Label>
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