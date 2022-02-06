<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="_203801G_Practical_Assignment.Success" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>User Profile</h2>
            <h2>First Name: <asp:Label ID="lbl_FirstName" runat="server"></asp:Label></h2>
            <h2>Last Name: <asp:Label ID="lbl_LastName" runat="server"></asp:Label></h2>
            <h2>Credit Card Number ending with: <asp:Label ID="lbl_ccNo" runat="server"></asp:Label></h2>
            <h2>Expires: <asp:Label ID="lbl_ExpMonth" runat="server"/>/<asp:Label ID="lbl_ExpYear" runat="server"/></h2>
            <h2>Email Address: <asp:Label ID="lbl_email" runat="server"></asp:Label></h2>
            <h2>Date of Birth: <asp:Label ID="lbl_DOB" runat="server"></asp:Label></h2>
            <h2>Photo: <asp:Image runat="server" ID="PhotoImg" Width="100px" Height="100px" ImageUrl="~/Photo"/></h2>
        </div>
        <asp:Button ID="LogOutBtn" runat="server" OnClick="LogOutClick" Text="Log Out" Width="288px"/>
    </form>
</body>
</html>
