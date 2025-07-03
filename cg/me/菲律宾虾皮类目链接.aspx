<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="菲律宾虾皮类目链接.aspx.cs" Inherits="WebApplication11.cg.菲律宾虾皮类目链接" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>菲律宾虾皮类目链接</title>
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
                <h3>当前页面【<span style="color: #cba537">菲律宾虾皮类目链接</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal>
                </h2>
            </div>
            <div>

                <asp:Button ID="Button1" runat="server" Text="添加新数据" OnClick="Button1_Click1" OnClientClick="JavaScript:return confirm('确定添加新数据？');" />
                <span style="color: blue; font-weight: bold">（所有普货大数据出过单的ITEMID都提取）</span>

                <br />
                <br />

                订单量大于><asp:TextBox ID="txtQuantity" runat="server" Width="118px"></asp:TextBox>&nbsp;
               
                 状态选择
                <asp:DropDownList ID="dpzt" runat="server">
                    <asp:ListItem Value="全部">全部</asp:ListItem>
                    <asp:ListItem Value="已添加类目链接">已添加类目链接</asp:ListItem>
                    <asp:ListItem Value="未添加类目链接">未添加类目链接</asp:ListItem>
                </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                是否广告测品
                 <asp:DropDownList ID="dpisadpd" runat="server">
                     <asp:ListItem Value="全部">全部</asp:ListItem>
                     <asp:ListItem Value="1">是</asp:ListItem>
                     <asp:ListItem Value="-1">否</asp:ListItem>
                 </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                 是否发仓
                 <asp:DropDownList ID="dpisfc" runat="server">
                     <asp:ListItem Value="全部">全部</asp:ListItem>
                     <asp:ListItem Value="1">是</asp:ListItem>
                     <asp:ListItem Value="-1">否</asp:ListItem>
                 </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                 是否有货源
                 <asp:DropDownList ID="dpishy" runat="server">
                     <asp:ListItem Value="全部">全部</asp:ListItem>
                     <asp:ListItem Value="1" Selected="True">是</asp:ListItem>
                     <asp:ListItem Value="-1">否</asp:ListItem>
                 </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                 是否有运营编码
                 <asp:DropDownList ID="dpyybm" runat="server">
                     <asp:ListItem Value="全部" Selected="True">全部</asp:ListItem>
                     <asp:ListItem Value="1">是</asp:ListItem>
                     <asp:ListItem Value="-1">否</asp:ListItem>
                 </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                运营编码
                <asp:TextBox ID="txtyybm" runat="server" Width="118px"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;
                排序
                 <asp:DropDownList ID="dporder" runat="server">
                     <asp:ListItem Value="Quantity desc">出单总数高-低</asp:ListItem>

                 </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                    加载数量
                <asp:TextBox ID="txttop" runat="server" Text="100" Width="38px"></asp:TextBox>&nbsp;&nbsp;
                <asp:Button ID="Button2" runat="server" BackColor="Blue" ForeColor="White" Text="加载数据" OnClientClick="JavaScript:return confirm('确定加载数据？');" OnClick="Button2_Click1" />

                <br />

                <span style="color: red; font-weight: bold">
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal></span>
                <br />
                <span style="color: blue; font-weight: bold">
                    <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                </span>
                <br />
                <asp:Button ID="Button3" BackColor="Green" runat="server" ForeColor="White" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页" OnClick="Button3_Click1" />

            </div>
            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="licode" runat="server" Text='<%# Eval("code") %>' Visible="false"></asp:Literal>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />

                            </td>
                            <td style="width: 30%; text-align: center">
                                <img src='<%# Eval("MainImage") %>' style="width: 200px; height: 200px" />
                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 30%" class="bbb">随机码</td>
                                        <td style="width: 70%">
                                            <%# Eval("code") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">订单量</td>
                                        <td style="width: 70%">
                                            <%# Eval("Quantity") %>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">标题</td>
                                        <td style="width: 70%">
                                            <%# Eval("title") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688采购链接</td>
                                        <td style="width: 70%">

                                            <a href="<%# Eval("Y_1688url") %>" target="_blank">打开网址</a>&nbsp;&nbsp;
                                           <input type="text" value='<%# Eval("Y_1688url") %>' style="width: 220px" id="url<%# Container.ItemIndex+1 %>" />
                                            <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                            <br />
                                            <asp:TextBox ID="txtY_1688url" runat="server" Width="80%" Text=' <%# Eval("Y_1688url") %>'></asp:TextBox>
                                        </td>

                                    </tr>


                                    <tr>
                                        <td style="width: 30%" class="bbb">菲律宾虾皮原始链接</td>
                                        <td style="width: 70%">

                                            <a href="<%# Eval("phurl") %>" target="_blank">打开网址</a>&nbsp;&nbsp;
                                           <input type="text" value='<%# Eval("phurl") %>' style="width: 220px" id="phurl<%# Container.ItemIndex+1 %>" />
                                            <input type="button" class="anniu1" onclick='copyUrl("phurl<%# Container.ItemIndex+1 %>")' value="复制网址">
                                        </td>

                                    </tr>


                                    <tr>
                                        <td>是否有货源</td>
                                        <td>
                                            <asp:Literal ID="liishy" runat="server" Text='<%# Eval("ishy") %>' Visible="false"></asp:Literal>
                                            <asp:RadioButtonList ID="rbishy" CssClass="rdcss" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="1" class="gg">是</asp:ListItem>
                                                <asp:ListItem Value="-1" class="rr">否</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td>是否广告测品</td>
                                        <td>
                                            <asp:Literal ID="liisadpd" runat="server" Text='<%# Eval("isadpd") %>' Visible="false"></asp:Literal>
                                            <asp:RadioButtonList ID="rbisadpd" CssClass="rdcss" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="1" class="gg">是</asp:ListItem>
                                                <asp:ListItem Value="-1" class="rr">否</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>是否发仓</td>
                                        <td>
                                            <asp:Literal ID="liisfc" runat="server" Text='<%# Eval("isfc") %>' Visible="false"></asp:Literal>
                                            <asp:RadioButtonList ID="rbisfc" CssClass="rdcss" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="1" class="gg">是</asp:ListItem>
                                                <asp:ListItem Value="-1" class="rr">否</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">菲律宾虾皮类目链接</td>
                                        <td style="width: 70%">
                                            <asp:TextBox ID="txtlmurl" runat="server" Width="80%" Text=' <%# Eval("lmurl") %>'></asp:TextBox>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">类目采集日期</td>
                                        <td style="width: 70%">
                                            <asp:TextBox ID="txtlmcjtime" runat="server" Width="80%" Text=' <%# Eval("lmcjtime") %>'></asp:TextBox>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">原始店铺链接</td>
                                        <td style="width: 70%">
                                            <asp:TextBox ID="txtysdpurl" runat="server" Width="80%" Text=' <%# Eval("ysdpurl") %>'></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">店铺采集日期</td>
                                        <td style="width: 70%">
                                            <asp:TextBox ID="txtdpcjtime" runat="server" Width="80%" Text=' <%# Eval("dpcjtime") %>'></asp:TextBox>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">运营编码</td>
                                        <td style="width: 70%">
                                            <asp:TextBox ID="txtyybm" runat="server" Width="80%" Text=' <%# Eval("yybm") %>'></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确定保存？');" runat="server" Text="保存数据" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="up" CommandArgument='<%# Eval("code") %>' />
                                        </td>

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
