<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="重新采购无货订单.aspx.cs" Inherits="WebApplication11.cg.重新采购无货订单" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>初次入仓采购</title>
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
                border: 1px solid #1e05a18a;
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
                <h3>当前页面【<span style="color: #1e05a18a">重新采购无货订单</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal>
                </h2>
            </div>
            <div>
                商家编码：
                <asp:TextBox ID="txtsjbm" Text='' runat="server"></asp:TextBox>
                1688采购链接：
                <asp:TextBox ID="txt1688url" Text='' runat="server"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="加载数据" OnClick="Button1_Click" />&nbsp;
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="liid" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Literal>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />
                                <br />
                                <%# Eval("itemID")%>-   <%# Eval("SkuID")%>
                                <br />
                                <%# Eval("code") %>
                            </td>
                            <td style="width: 30%; text-align: center">
                                <%# Eval("img")%>
                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 20%" class="bbb">产品链接</td>
                                        <td style="width: 50%"><a href="<%# Eval("purl") %>" target="_blank">打开网址</a>&nbsp;&nbsp;
                                           <input type="text" value='<%# Eval("purl") %>' style="width: 20px" id="url<%# Container.ItemIndex+1 %>" />
                                            <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">产品ID</td>
                                        <td style="width: 50%"><%# Eval("itemID") %></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td style="width: 20%" class="bbb">SkuID</td>
                                        <td style="width: 50%"><%# Eval("SkuID") %></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td style="width: 20%" class="bbb">sku1</td>
                                        <td style="width: 50%"><%# Eval("sku1") %></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td style="width: 20%" class="bbb">sku2</td>
                                        <td style="width: 50%"><%# Eval("sku2") %></td>
                                        <td></td>
                                    </tr>


                                    <tr>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">1688采购链接</td>
                                        <td><a href="<%# Eval("Y_1688url") %>" target="_blank">点击打开</a></td>
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
                                        <td class="bbb">发仓数量_自动</td>
                                        <td><%# Eval("发仓数量_自动") %></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">收货人姓名</td>
                                        <td><%#  WebApplication11.works.getshr(Eval("yunyingbianma").ToString()) %></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="LinkButton2" OnClientClick="JavaScript:return confirm('确定不能采购？');" runat="server" Text="不能采购" ForeColor="White" BackColor="Red" CssClass="butt" CommandName="no" CommandArgument='<%# Eval("id") %>' />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="btnzd" OnClientClick="JavaScript:return confirm('确定已下单？');" runat="server" Text="已下单" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="yes" CommandArgument='<%# Eval("id") %>' />

                                        </td>
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
