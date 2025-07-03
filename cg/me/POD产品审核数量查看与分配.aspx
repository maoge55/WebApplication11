<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="POD产品审核数量查看与分配.aspx.cs" Inherits="WebApplication11.cg.POD产品审核数量查看与分配" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>POD产品审核数量查看与分配</title>
    <style>
        .ttt {
        }

            .ttt tr {
            }

                .ttt tr td {
                    border: 1px solid #000000;
                    padding: 5px;
                }

        .ttta {
        }

            .ttta tr td {
                border: 1px solid #cf0000;
                padding: 5px;
            }

        .bbb {
            font-weight: bold;
        }

        .butt {
            padding: 0 50px;
        }

        .anniu1 {
            border: 1px red solid;
        }

        .auto-style1 {
            width: 50%;
        }

        .auto-style2 {
            width: 189px;
            background: #cf0000;
            color: white;
        }

        .auto-style3 {
            width: 165px;
            background: #cf0000;
            color: white;
        }

        .auto-style4 {
            width: 132px;
            background: #cf0000;
            color: white;
        }

        .ccc1 {
            color: blue;
            font-weight: bold;
        }

        .ccc2 {
            color: blue;
            font-weight: bold;
            text-align: center;
        }
    </style>
    <script>
        function copyUrl(myurl) {

            var Url2 = document.getElementById(myurl);
            Url2.select();
            document.execCommand("Copy");



        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <h3>当前页面【<span style="color: #cf0000">POD产品审核数量查看与分配</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                类型：<asp:DropDownList ID="dptype" runat="server">
                    <asp:ListItem Value="all">全部</asp:ListItem>
                    <asp:ListItem Value="Hoodie">Hoodie</asp:ListItem>
                    <asp:ListItem Value="T-shirt">T-shirt</asp:ListItem>

                </asp:DropDownList>
                <asp:Button ID="Button1" runat="server" Text="统计数据" BackColor="Green" ForeColor="White" OnClick="Button1_Click" />
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />
            <div>
                <table class="ttt">

                    <tr>

                        <td>
                            <table class="ttta">
                                <tr>
                                    <td class="auto-style2">账号</td>
                                    <td class="auto-style3">数量</td>

                                </tr>
                                <%=html %>
                            </table>
                        </td>

                    </tr>
                </table>
            </div>
            <br />
            从：<asp:DropDownList ID="drf" runat="server">
                <asp:ListItem Value="0">无人</asp:ListItem>
                <asp:ListItem Value="6">郑雨蝶</asp:ListItem>
                <asp:ListItem Value="8">徐震雄</asp:ListItem>
                <asp:ListItem Value="11">陈高乐</asp:ListItem>
                <asp:ListItem Value="12">夏鸿飞</asp:ListItem>
                <asp:ListItem Value="13">胡伟国</asp:ListItem>
                <asp:ListItem Value="14">张志伟POD</asp:ListItem>
                <asp:ListItem Value="16">林嘉华</asp:ListItem>
            </asp:DropDownList>
            移动<asp:TextBox ID="txtcount" Width="50px" CssClass="ccc2" runat="server"></asp:TextBox>条数据到
            <asp:DropDownList ID="drin" runat="server">

                <asp:ListItem Value="6">郑雨蝶</asp:ListItem>
                <asp:ListItem Value="8">徐震雄</asp:ListItem>
                <asp:ListItem Value="11">陈高乐</asp:ListItem>
                <asp:ListItem Value="12">夏鸿飞</asp:ListItem>
                <asp:ListItem Value="13">胡伟国</asp:ListItem>
                <asp:ListItem Value="14">张志伟POD</asp:ListItem>
                <asp:ListItem Value="16">林嘉华</asp:ListItem>
            </asp:DropDownList>&nbsp;
            <asp:Button ID="Button2" runat="server" OnClientClick="JavaScript:return confirm('确定移动？');" Text="移动" BackColor="Green" ForeColor="White" OnClick="Button2_Click" />

        </div>
    </form>
</body>
</html>
