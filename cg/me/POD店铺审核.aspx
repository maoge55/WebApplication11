<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="POD店铺审核.aspx.cs" Inherits="WebApplication11.cg.POD店铺审核" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>POD店铺审核</title>
    <style>
        .ttt {
            width: 100%;
        }

            .ttt tr {
            }

                .ttt tr td {
                    border: 1px solid #000000;
                    padding: 5px;
                }

        .ttta {
            width: 100%;
        }

            .ttta tr td {
                border: 1px solid #fd00ff;
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

        img {
            float: left;
            width: 300px !important;
            height: 300px !important;
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
                <h3>当前页面【<span style="color: #fd00ff">POD店铺审核</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                类型：<asp:DropDownList ID="dptype" runat="server">
                    <asp:ListItem Value="all">全部</asp:ListItem>
                    <asp:ListItem Value="Hoodie">Hoodie</asp:ListItem>
                    <asp:ListItem Value="T-shirt">T-shirt</asp:ListItem>

                </asp:DropDownList>&nbsp;
                <asp:Button ID="Button1" runat="server" Text="加载10个店铺" OnClick="Button1_Click" />&nbsp;
                <asp:Button ID="Button2" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存审核结果吗？');" Text="保存审核结果" OnClick="Button2_Click2" />
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                &nbsp;&nbsp;
            
            </div>
            <br />

            <table class="ttt">

                <asp:Repeater ID="rplb" runat="server">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="lishopid" runat="server" Text='<%# Eval("shopid") %>' Visible="false"></asp:Literal>

                            <td style="width: 4%;"><%# Eval("shopid") %></td>
                            <td>
                                <%# Eval("imgs") %>
                            </td>

                            <td>
                                <asp:RadioButtonList ID="rdjg" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="yes"><span style="color:green">是</span></asp:ListItem>
                                    <asp:ListItem Value="or"><span style="color:blue">部分</span></asp:ListItem>
                                    <asp:ListItem Value="no"><span style="color:red">否</span></asp:ListItem>
                                </asp:RadioButtonList>

                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td></td>
                    <td></td>

                    <td>
                        <asp:Button ID="Button3" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存审核结果吗？');" Text="保存审核结果" OnClick="Button3_Click2" /></td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>
