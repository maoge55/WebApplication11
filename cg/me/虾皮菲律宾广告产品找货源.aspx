<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="虾皮菲律宾广告产品找货源.aspx.cs" Inherits="WebApplication11.cg.虾皮菲律宾广告产品找货源" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>找货源-虾皮菲律宾广告产品</title>
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
                border: 1px solid #37cbc5;
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
                <h3>当前页面【<span style="color: #37cbc5">虾皮菲律宾广告产品找货源</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                加载数量： 
                <asp:TextBox ID="txtcount" runat="server" Text="10" Width="61px">10</asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="加载" OnClick="Button1_Click" />&nbsp;
                 <asp:Button ID="Button2" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页" OnClick="Button2_Click1" />
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
                                <%# Container.ItemIndex+1 %><br />
                                <br />
                                <%# Eval("itemID")%>-   <%# Eval("SkuID")%>
                            </td>
                            <td style="width: 30%; text-align: center">
                              <img src='<%# Eval("image")%>' style="width:300px" /> 
                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 20%" class="bbb">产品ID</td>
                                        <td style="width: 50%"><%# Eval("itemID") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">SkuID</td>
                                        <td style="width: 50%"><%# Eval("SkuID") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">产品链接</td>
                                        <td style="width: 50%"><a href="<%# Eval("url") %>" target="_blank">打开网址</a>
                                            <input type="text" value='<%# Eval("url") %>' style="width: 20px" id="url<%# Container.ItemIndex+1 %>" />
                                            <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">第一级SKU</td>
                                        <td><%# Eval("SKU1") %></td>

                                    </tr>
                                    <tr>
                                        <td class="bbb">第二级SKU</td>
                                        <td><%# Eval("SKU2") %></td>

                                    </tr>

                                    <tr>
                                        <td class="bbb">标题</td>
                                        <td><%# Eval("pname") %></td>

                                    </tr>

                                    <tr>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">1688采购链接</td>
                                        <td>
                                            <asp:TextBox ID="txtY_1688url" Text='' runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">1688sku1</td>

                                        <td>
                                            <asp:TextBox ID="txtY_1688sku1" Text='' runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">1688sku2</td>

                                        <td>
                                            <asp:TextBox ID="txtY_1688sku2" Text='' runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">1688sku3</td>

                                        <td>
                                            <asp:TextBox ID="txtY_1688sku3" Text='' runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">1688采购价</td>

                                        <td>
                                            <asp:TextBox ID="txtY_1688price" Text='' runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>

                                        <td>
                                            <asp:LinkButton ID="btnzd" OnClientClick="JavaScript:return confirm('确定找不到？');" runat="server" Text="找不到" ForeColor="White" BackColor="Red" CssClass="butt" CommandName="xd" CommandArgument='<%# Eval("id") %>' />

                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确定保存？');" runat="server" Text="保存数据" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="up" CommandArgument='<%# Eval("id") %>' />
                                        </td>
                                    </tr>
                                </table>





                            </td>

                        </tr>
                        <tr></tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td></td>
                    <td></td>

                    <td>
                        <asp:Button ID="Button3" runat="server" ForeColor="Blue" Width="200px" Height="40px" Font-Size="20px" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页" OnClick="Button3_Click1" /></td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>
