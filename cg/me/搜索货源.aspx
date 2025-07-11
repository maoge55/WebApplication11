﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="搜索货源.aspx.cs" Inherits="WebApplication11.cg.搜索货源" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>搜索货源</title>
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
                border: 1px solid #cba537;
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
                <h3>当前页面【<span style="color: #cba537">搜索货源</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal>
                </h2>
            </div>
            <div>
                输入关键词：<asp:TextBox ID="txtkjc" runat="server"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="搜索" OnClick="Button1_Click" />&nbsp;
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="liid" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Literal>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %>
                            </td>
                            <td style="width: 30%; text-align: center">
                                <img src='<%# Eval("MainImage")%>' style="width: 300px" />

                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 20%" class="bbb">Purl</td>
                                        <td style="width: 50%"><a href="<%# Eval("purl") %>" target="_blank">打开网址</a>&nbsp;&nbsp;
                                           <input type="text" value='<%# Eval("purl") %>' style="width: 20px" id="url<%# Container.ItemIndex+1 %>" />
                                            <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">标题</td>
                                        <td><%# Eval("Title") %></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">itemID</td>
                                        <td><%# Eval("itemID") %></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">SkuID</td>
                                        <td><%# Eval("SkuID") %></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">随机码</td>
                                        <td><%# Eval("code") %></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">1688采购链接</td>
                                        <td>
                                            
                                                <a href="<%# Eval("Y_1688url") %>" target="_blank">点击打开</a>&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="LinkButton4" OnClientClick="JavaScript:return confirm('确定拉黑该链接？');" runat="server" Text="拉黑该链接" ForeColor="White" BackColor="Black" CssClass="" CommandName="lhurl" CommandArgument='<%# Eval("Y_1688url").ToString() %>' />
                                           
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtY_1688url" Text='' runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">1688sku1</td>
                                        <td><%# Eval("Y_1688sku1") %></td>
                                        <td>
                                            <asp:TextBox ID="txtY_1688sku1" Text='' runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">1688sku2</td>
                                        <td><%# Eval("Y_1688sku2") %></td>
                                        <td>
                                            <asp:TextBox ID="txtY_1688sku2" Text='' runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">1688sku3</td>
                                        <td><%# Eval("Y_1688sku3") %></td>
                                        <td>
                                            <asp:TextBox ID="txtY_1688sku3" Text='' runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">1688采购价</td>
                                        <td><%# Eval("Y_1688price") %></td>
                                        <td>
                                            <asp:TextBox ID="txtY_1688price" Text='' runat="server"></asp:TextBox></td>
                                    </tr>





                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确定更新采购链接？');" runat="server" Text="更新采购链接" ForeColor="White" BackColor="Blue" CssClass="butt" CommandName="up" CommandArgument='<%# Eval("id") %>' />
                                    </tr>
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
