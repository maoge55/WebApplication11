<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="uplb.aspx.cs" Inherits="WebApplication11.uplb" %>

<%@ Register Src="~/left.ascx" TagName="left" TagPrefix="ul" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>指定数据批量修改</title>
</head>
<body>
    <ul:left ID="left" runat="server" />
    <form id="form1" runat="server">

        <div class="main">
            <div id="lg" runat="server">
                <div class="hdt">
                    <h2>输入单独的管理员密码</h2>
                </div>
                <table class="ttt" id="add">
                    <tr>

                        <td>密码：<asp:TextBox TextMode="Password" runat="server" ID="txtpwd" Width="119px" CssClass="txt"></asp:TextBox>

                            &nbsp;<asp:Button ID="Button3" runat="server" Text="Login" CssClass="butt" OnClick="Button3_Click" /></td>
                    </tr>
                </table>
            </div>
            <div id="zc" runat="server" visible="false">
                <div class="hdt">
                    <h2>指定数据批量修改</h2>
                </div>
                <div class="ts">
                    <p>
                        <asp:Literal runat="server" ID="lits"></asp:Literal>
                    </p>
                </div>
                <div>
                    <asp:TextBox ID="txtlbm" runat="server"></asp:TextBox>
                    <asp:DropDownList ID="DropDownList1" runat="server">
                        <asp:ListItem Value="lgd">固定</asp:ListItem>
                        <asp:ListItem Value="lqt">其他</asp:ListItem>
                        <asp:ListItem Value="lsmt">电商平台数据</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtvalue" runat="server"></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" Text="查询" OnClick="Button1_Click" />
                    &nbsp;
            <asp:Button ID="Button2" runat="server" Text="修改" OnClick="Button2_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
