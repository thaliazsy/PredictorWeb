<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Predictor.aspx.cs" Inherits="PredictorWeb.Predictor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
<style>
    #ListViewBox
    {
        width:1650px;
        height:1200px;
    }
</style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:FileUpload ID="FileUpload1" runat="server" />
            <asp:Button ID="Button1" runat="server" Text="Predict" OnClick="Button1_Click" />
            <asp:DropDownList ID="DropDownList1" runat="server" style="margin-bottom: 0px">
            </asp:DropDownList>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <asp:Image ID="Image1" runat="server"/>
            <br />
            <asp:Repeater ID="RepeaterImages" runat="server">
                    <ItemTemplate>
                        <asp:Image ID="Image" runat="server" ImageUrl='<%# Container.DataItem %>' />
                    </ItemTemplate>
                </asp:Repeater>
            <br />
            <div id="ListViewBox">
                <asp:ListView ID="ListView1" runat="server" >
                    <ItemTemplate>
                        <asp:Image ID="Image" runat="server" ImageUrl='<%# Container.DataItem %>' />
                        <%--<asp:Image ID="ItemImage" runat="server" ImageUrl='<%# BuildItemImageUrl(Eval("Id", "{0}")) %>'/>--%>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            
        </div>
    </form>
</body>
</html>
