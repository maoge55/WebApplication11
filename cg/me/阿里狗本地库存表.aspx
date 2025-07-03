<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="阿里狗本地库存表.aspx.cs" Inherits="WebApplication11.cg.阿里狗本地库存表" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>阿里狗本地库存表</title>
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
                <h3>当前页面【<span style="color: #cba537">阿里狗本地库存表</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal>
                </h2>
            </div>
            <div>

                <asp:Button ID="Button1" runat="server" Text="添加新数据" OnClick="Button1_Click1" OnClientClick="JavaScript:return confirm('确定添加新数据？');" />
                <span style="color: blue; font-weight: bold">（商家编码+EAN组合去重复创建新数据，出单量大于等于2）</span>
                <br />
                <br />
                商家编码：<asp:TextBox ID="txtsjbm" runat="server" Width="118px"></asp:TextBox>&nbsp;
                产品链接：<asp:TextBox ID="txturl" runat="server" Width="218px"></asp:TextBox>&nbsp;
                 状态选择：
                <asp:DropDownList ID="dpzt" runat="server">
                    <asp:ListItem Value="全部">全部</asp:ListItem>
                    <asp:ListItem Value="未备库存">未备库存</asp:ListItem>
                    <asp:ListItem Value="已备库存">已备库存</asp:ListItem>
                </asp:DropDownList>&nbsp;
                <asp:Button ID="Button2" runat="server" Text="搜索加载数据" OnClientClick="JavaScript:return confirm('确定加载数据？');" OnClick="Button2_Click1" />
                <br />
                <span style="color: red; font-weight: bold">
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal></span>
                <br />
                <span style="color: blue; font-weight: bold">
                    <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                </span>

            </div>
            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="liid" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Literal>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />
                                <br />
                                <%# Eval("id")%>
                            </td>
                            <td style="width: 30%; text-align: center">
                                <img src='<%# Eval("chanpintupianwangzhi") %>' style="width: 300px; height: 300px" />
                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 30%" class="bbb">商家编码</td>
                                        <td style="width: 70%">
                                            <%# Eval("sjbm") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">EAN</td>
                                        <td style="width: 70%">
                                            <%# Eval("ean") %>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">标题</td>
                                        <td style="width: 70%">
                                            <%# Eval("biaoti") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">产品链接</td>
                                        <td style="width: 70%">

                                            <a href="<%# Eval("chanpinlianjie") %>" target="_blank">打开网址</a>&nbsp;&nbsp;
                                           <input type="text" value='<%# Eval("chanpinlianjie") %>' style="width: 20px" id="url<%# Container.ItemIndex+1 %>" />
                                            <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                            &nbsp;
                                            <a href="阿里狗订单查货源.aspx?url=<%# Eval("chanpinlianjie") %>" style="color: blue; font-weight: bold" target="_blank">查找已有货源
                                            </a>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">总单量</td>
                                        <td style="width: 70%">
                                            <%# Eval("zongdanliang") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">最近订单最后时间</td>
                                        <td style="width: 70%">
                                            <%# Eval("lasttime") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">最近订单最早时间</td>
                                        <td style="width: 70%">
                                            <%# Eval("firsttime") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">平均订单间隔时间</td>
                                        <td style="width: 70%">
                                            <%# Eval("pingjundingdanjiangeshijian") %>
                                            <span style="color: red">(最多统计最近的2-5个订单的总时间/单量)</span>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">建议库存量</td>
                                        <td style="width: 70%">
                                            <%# Eval("jianyikucunliang") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">库存编号</td>
                                        <td style="width: 70%">
                                            <asp:TextBox ID="txtkucunbianhao" runat="server" Width="80%" Text=' <%# Eval("kucunbianhao") %>'></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">实际库存量</td>
                                        <td style="width: 70%">
                                            <asp:TextBox ID="txtshijikucunliang" runat="server" Width="80%" Text=' <%# Eval("shijikucunliang") %>'></asp:TextBox>
                                        </td>

                                    </tr>




                                    <tr>
                                        <td></td>
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

            </table>

        </div>
    </form>
</body>
</html>
