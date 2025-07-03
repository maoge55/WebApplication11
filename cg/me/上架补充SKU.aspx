<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="上架补充SKU.aspx.cs" Inherits="WebApplication11.cg.上架补充SKU" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>上架补充SKU</title>
    <style>
        .ttt {
            width: 100%;
        }

            .ttt tr {
            }

                .ttt tr td {
                    border: 1px solid #c0c0d9;
                    padding: 5px;
                }

        .ttta {
            width: 100%;
        }

            .ttta tr td {
                border: 1px solid #c01e2f;
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
                <h3>当前页面【<span style="color: #c01e2f">上架补充SKU</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal>
                </h2>
            </div>
            <div>

                <asp:TextBox ID="txtsearch" Text='' runat="server"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="搜索" OnClick="Button1_Click" /><span style="color: red">*输入随机码、部分标题搜索产品</span><br />

                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="liitemid" runat="server" Text='<%# Eval("itemid") %>' Visible="false"></asp:Literal>

                            <td style="width: 2%; text-align: center">
                                <%# Container.ItemIndex+1 %>
                            </td>
                            <td>
                                <%# getimg(Eval("itemid").ToString()) %>
                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td colspan="5">
                                            <%# gettitle(Eval("itemid").ToString()) %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; background-color: #fdd6d6;">itemid</td>
                                        <td style="width: 15%; background-color: #fdd6d6;">sku1</td>
                                        <td style="width: 15%; background-color: #fdd6d6;">sku2</td>
                                        <td style="width: 15%; background-color: #fdd6d6;">价格</td>
                                        <td style="background-color: #fdd6d6;">sku图片网址</td>
                                    </tr>
                                    <asp:Repeater ID="rpej" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%# Eval("itemid") %></td>
                                                <td><%# Eval("sku1") %></td>
                                                <td><%# Eval("sku2") %></td>
                                                <td><%# Eval("newprice_shopeeid") %></td>
                                                <td><%# Eval("skuimg") %></td>

                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </td>
                        </tr>
                        <tr></tr>
                    </ItemTemplate>
                </asp:Repeater>

            </table>

        </div>
    </form>
</body>
</html>
