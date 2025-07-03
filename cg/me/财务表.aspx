<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="财务表.aspx.cs" Inherits="WebApplication11.cg.财务表" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>财务表</title>
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
                border: 1px solid crimson;
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
                <h3>当前页面【<span style="color: crimson">财务表</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                商家编码：<asp:TextBox ID="txtsjbm" runat="server" Width="118px"></asp:TextBox>&nbsp;
                入仓印尼标题：<asp:TextBox ID="txtrucangyinnibiaoti" runat="server" Width="418px"></asp:TextBox>&nbsp;
                 系统编码：<asp:TextBox ID="txtrucangSKUID" runat="server" Width="118px"></asp:TextBox>&nbsp;
                订单状态：<asp:DropDownList ID="dpzt" runat="server">
                    <asp:ListItem Value="全部">全部</asp:ListItem>
                    <asp:ListItem Value="等待买家确认收货">等待买家确认收货</asp:ListItem>
                    <asp:ListItem Value="等待卖家发货">等待卖家发货</asp:ListItem>
                    <asp:ListItem Value="交易成功">交易成功</asp:ListItem>
                    <asp:ListItem Value="交易关闭">交易关闭</asp:ListItem>
                    <asp:ListItem Value="已收货">已收货</asp:ListItem>





                </asp:DropDownList>
                <asp:Button ID="Button1" runat="server" Text="查找" BackColor="Green" ForeColor="White" OnClick="Button1_Click1" />

                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />

            <table class="ttt">

                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />


                            </td>

                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 30%" class="bbb">入仓印尼标题</td>
                                        <td><%#gettitle( Eval("rucangyinnibiaoti").ToString()) %></td>

                                    </tr>



                                    <tr>
                                        <td style="width: 30%" class="bbb">货品标题</td>
                                        <td><%# Eval("huopinbiaoti") %></td>

                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">原始数据SKUID</td>
                                        <td><%# Eval("rucangSKUID") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">订单状态</td>
                                        <td><%# Eval("dingdanzhuangtai") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%; color: red; font-weight: bold" class="bbb">单价(元)</td>
                                        <td><%# Eval("danjia") %></td>

                                    </tr>



                                    <tr>
                                        <td style="width: 30%" class="bbb">数量</td>
                                        <td><%# Eval("shuliang") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">长cm</td>
                                        <td><%# Eval("chang") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">宽cm</td>
                                        <td><%# Eval("kuan") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">高cm</td>
                                        <td><%# Eval("gao") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">重量kg</td>
                                        <td><%# Eval("zhongliang") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%; color: red; font-weight: bold" class="bbb">安能运费</td>
                                        <td><%# getannengyunfei(Eval("zhongliang").ToString()) %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">体积重量kg</td>
                                        <td><%# Eval("tijizhongliang") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">运费海运每方</td>
                                        <td><%# Eval("yunfeihaiyunmeifang") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">运费空运每公斤</td>
                                        <td><%# Eval("yunfeikongyunmeigongjin") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%; color: red; font-weight: bold" class="bbb">运费海运单品</td>
                                        <td><%# getyfhydp(Eval("yunfeihaiyundanpin").ToString(),Eval("yunfeihaiyunmeifang").ToString(),Eval("chang").ToString(),Eval("kuan").ToString(),Eval("gao").ToString()) %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">运费空运单品</td>
                                        <td><%# Eval("yunfeikongyundanpin") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">莆田发出数量</td>
                                        <td><%# Eval("putianfachushuliang") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">莆田发出日期</td>
                                        <td><%# Eval("putianfachuriqi") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">莆田发出单号</td>
                                        <td><%# Eval("putianfachudanhao") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">莆田发出快递费用</td>
                                        <td><%# Eval("putianfachukuaidifeiyong") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">莆田发出备注</td>
                                        <td><%# Eval("putianfachubeizhu") %></td>

                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">图片网址</td>
                                        <td><%# Eval("tupianwangzhi") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">视频网址</td>
                                        <td><%# Eval("shipinwangzhi") %></td>

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
