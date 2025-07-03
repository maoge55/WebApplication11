<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="阿里狗订单查货源.aspx.cs" Inherits="WebApplication11.cg.阿里狗订单查货源" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>阿里狗订单查货源</title>
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
                <h3>当前页面【<span style="color: #37cbc5">阿里狗订单查货源</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>


            <asp:TextBox ID="txtinoffid" runat="server" Height="157px" Width="95%" TextMode="MultiLine" ValidateRequestMode="Disabled"></asp:TextBox>
            <br />
            <asp:Button ID="btnsearch" runat="server" Text="查找已有货源" Width="150px" OnClientClick="" BackColor="Blue" ForeColor="White" OnClick="btnsearch_Click" />
            &nbsp;&nbsp;
                <asp:Button ID="Button1" runat="server" Text="补充订单货源" Width="150px" BackColor="Green" ForeColor="White" OnClick="Button1_Click1" />

            &nbsp;&nbsp;
                <asp:Button ID="Button2" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页" OnClick="Button2_Click1" />
                
            <br />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>


            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="liid" runat="server" Text='<%# Eval("pid") %>' Visible="false"></asp:Literal>
                            <asp:Literal ID="lipean" runat="server" Text='<%# Eval("pean") %>' Visible="false"></asp:Literal>
                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />
                                <br />
                                <%# Eval("pid")%>
                                <br />
                                <%# Eval("pean")%>
                                <br />
                                 <%# Eval("yuse")%>
                            </td>
                            <td style="width: 30%; text-align: center">
                                <img src='<%# Eval("ZhuTuWangZhi")%>' style="width: 300px; height: 300px" />
                            </td>
                            <td>
                                <table class="ttta">



                                    <tr>
                                        <td class="bbb">1688采购链接</td>
                                        <td>
                                            <asp:TextBox ID="txtY_1688url" Text='<%# Eval("Y_1688url") %>' runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">1688sku1</td>

                                        <td>
                                            <asp:TextBox ID="txtY_1688sku1" Text='<%# Eval("Y_1688sku1") %>' runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">1688sku2</td>

                                        <td>
                                            <asp:TextBox ID="txtY_1688sku2" Text='<%# Eval("Y_1688sku2") %>' runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">1688sku3</td>

                                        <td>
                                            <asp:TextBox ID="txtY_1688sku3" Text='<%# Eval("Y_1688sku3") %>' runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb"><span style="color: red">*1688采购价</span></td>

                                        <td>
                                            <asp:TextBox ID="txtY_1688price" Text='<%# Eval("Y_1688price") %>' runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="bbb"><span style="color: red">*空运发波兰运费</span></td>

                                        <td>
                                            <asp:TextBox ID="txt_yf" Text='<%# Eval("shipping_cost") %>'  runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>


                                        <td colspan="2">
                                            <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确定保存？');" runat="server" Text="保存数据" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="up" CommandArgument='<%# Eval("pean") %>' />
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
