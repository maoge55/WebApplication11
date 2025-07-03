<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="阿里狗拉黑链接.aspx.cs" Inherits="WebApplication11.cg.阿里狗拉黑链接" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>阿里狗拉黑链接</title>
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
                <h3>当前页面【<span style="color: #cf0000">阿里狗拉黑链接</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                <table class="ttt">

                    <tr>
                        <td class="auto-style1">
                            <asp:TextBox ID="txtinoffid" runat="server" Height="457px" Width="95%" TextMode="MultiLine" ValidateRequestMode="Disabled"></asp:TextBox>
                            <br />
                            <asp:Button ID="btnsearch" runat="server" Text="搜索拉黑处理" Width="150px" BackColor="Blue" ForeColor="White" OnClick="btnsearch_Click" />
                        </td>
                        <td>
                            <table class="ttta">
                                <tr>
                                    <td class="auto-style2">offer id</td>
                                    <td class="auto-style3">pean</td>
                                    <td class="auto-style4">处理结果</td>
                                </tr>
                                <asp:Repeater ID="Repeater1" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("SelfOfferID") %></td>
                                            <td><%# Eval("pean") %></td>
                                            <td><%# Eval("jg") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    </tr>
                </table>
            </div>


        </div>
    </form>
</body>
</html>
