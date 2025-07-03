<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication11.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Login</title>
    <style>
        .txt {
            border: 1px solid #333;
            line-height: 18px;
            font-size: 16px;
        }

        .butt {
            border: 1px solid #333;
            background: #e9e9f9;
            color: #333;
            text-align: center;
            text-decoration: none;
        }

        .ttt {
            width: 100%;
        }

            .ttt tr {
            }

                .ttt tr td {
                    border: 1px solid #c0c0d9;
                    padding: 5px;
                }

        .ts p {
            text-align: center;
            font-size: 25px;
            color: red;
            font-weight: bold;
        }

        .main {
            margin: 10% auto;
            width: 680px;
        }

        .head td {
            color: #0600ff;
            background: #cbdbcd;
        }

        </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div>
                <div class="ts">
                    <p>
                        <asp:Literal runat="server" ID="lits"></asp:Literal>
                    </p>
                </div>
                <table class="ttt" id="add">
                    <tr>
                        <td>用户名：<asp:TextBox runat="server" ID="txtname" Width="119px" CssClass="txt"></asp:TextBox></td>
                        <td>密码：<asp:TextBox TextMode="Password" runat="server" ID="txtpwd" Width="119px" CssClass="txt"></asp:TextBox></td>
                        <td>
                            <asp:Button ID="Button1" runat="server" Text="Login" CssClass="butt" OnClick="Button1_Click" style="height: 19px" /></td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
