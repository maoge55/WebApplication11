<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="阿里狗找货源.aspx.cs" Inherits="WebApplication11.cg.阿里狗找货源" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>阿里狗找货源</title>
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
            padding: 0 20px;
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
                <h3>当前页面【<span style="color: #37cbc5">阿里狗找货源</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                <asp:CheckBox ID="ckimg" runat="server" Text="加载细节图" />
                <asp:Button ID="Button1" BackColor="Green" ForeColor="White" runat="server" Text="加载10条正常数据" OnClick="Button1_Click" />&nbsp;
                <asp:Button ID="Button4" BackColor="Bisque" ForeColor="Black" runat="server" Text="加载所有没保存重量的数据" OnClick="Button4_Click" />&nbsp;
                <asp:Button ID="Button5" BackColor="Plum" ForeColor="White" runat="server" Text="加载10条跳过的数据" OnClick="Button5_Click" />&nbsp;
                 <asp:Button ID="Button2" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页" OnClick="Button2_Click1" />
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="liid" runat="server" Text='<%# Eval("pid") %>' Visible="false"></asp:Literal>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />
                                <br />
                                <%# Eval("pid")%>
                            </td>
                            <td style="width: 30%; text-align: center">
                                <img src='<%# access_sql.getimg( Eval("ZhuTuWangZhi").ToString())%>' style="width: 300px; height: 300px" />
                                <br />
                                <%# access_sql.getimg2( Eval("ZhuTuWangZhi").ToString(),ckimg.Checked)%>
                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 20%" class="bbb">产品ID</td>
                                        <td style="width: 50%"><%# Eval("pid") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">EAN</td>
                                        <td style="width: 50%"><%# Eval("pean") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">类目ID和数量</td>
                                        <td style="width: 50%"><%# Eval("leimu") %>/<%# Eval("leimu_count") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">产品链接</td>
                                        <td style="width: 50%">

                                            <a href='  <%# (Eval("purl").ToString()) %>' target='_blank'>打开网址</a>



                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">标题</td>
                                        <td><%# Eval("biaoti") %></td>

                                    </tr>

                                    <tr>
                                        <td colspan="3"></td>
                                    </tr>
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
                                        <td class="bbb">重量</td>


                                        <td>
                                            <asp:TextBox ID="txt_zhongliang" Text='<%# Eval("zhongliang") %>' runat="server"></asp:TextBox>kg</td>
                                    </tr>
                                    <tr>
                                        <td class="bbb"><span style="color: red">*产品类型</span></td>


                                        <td>
                                            <asp:Literal ID="licptype" runat="server" Text='<%# Eval("cptype") %>' Visible="false"></asp:Literal>
                                            <asp:RadioButtonList ID="rbcptype" CssClass="rdcss" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="ph" class="gg">普货</asp:ListItem>
                                                <asp:ListItem Value="dd" class="rr">带电</asp:ListItem>
                                            </asp:RadioButtonList>

                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="bbb">运费</td>


                                        <td>
                                            <asp:TextBox ID="txt_yf" Text='<%# Eval("shipping_cost") %>' runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>

                                        <td>
                                            <asp:LinkButton ID="btnzd" OnClientClick="JavaScript:return confirm('确定找不到？');" runat="server" Text="找不到" ForeColor="White" BackColor="Red" CssClass="butt" CommandName="xd" CommandArgument='<%# Eval("pid") %>' />

                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确定保存？');" runat="server" Text="保存数据" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="up" CommandArgument='<%# Eval("pid") %>' />
                                            &nbsp;&nbsp;
                                            <asp:LinkButton ID="LinkButton2" OnClientClick="JavaScript:return confirm('确定拉黑该产品？');" runat="server" Text="拉黑该产品" ForeColor="White" BackColor="Black" CssClass="butt" CommandName="bc" CommandArgument='<%# Eval("pid") %>' />
                                            &nbsp;&nbsp;
                                            <asp:LinkButton ID="LinkButton3" OnClientClick="JavaScript:return confirm('确定跳过该产品？');" runat="server" Text="跳过" ForeColor="White" BackColor="Plum" CssClass="butt" CommandName="tg" CommandArgument='<%# Eval("pid") %>' />
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
