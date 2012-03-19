<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Gopi.Example._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to ASP.NET!
    </h2>
    <body>
        <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnAddEmail" runat="server" Text="Add a Test Email" OnClick="btnAddEmail_Click" /><br />
            <asp:Button ID="btnSendAll" runat="server" Text="Send All Emails" OnClick="btnSendAll_Click" />
            <br />
            <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
        </div>
        </form>
    </body>
</asp:Content>
