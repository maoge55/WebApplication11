<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="阿里狗首页信息.aspx.cs" Inherits="WebApplication11.cg.阿里狗首页信息" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>阿里狗首页信息</title>
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

        .aaa4 td {
            background: #fb686842;
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

    <div>
        <div>
            <h3>当前页面【<span style="color: #cba537">阿里狗首页信息</span>】
               
            </h3>

            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>
        <div>
            <form id="form2" action="阿里狗首页信息.aspx" method="get">
                商家编码：
                  <input type="text" name="txtsjbm" id="txtsjbm" />
                &nbsp;
               
                排序规则：<select id="order" name="order">
                    <option value="DpStatus">店铺状态</option>
                    <option value="Available_funds">可提现波兰币</option>
                    <option value="Settlements_with_Allegro">需缴费</option>
                    <option value="views_day7">第7天流量</option>
                </select>&nbsp;
                 <button type="submit" id="btns1">搜索查找</button>
            </form>
            <br />
            <asp:Literal ID="lify" runat="server"></asp:Literal><br />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>

        </div>

        <br />
        <form id="form1" runat="server">
            <table class="ttt">
                <tr class="aaa4">
                    <td>序号</td>
                    <td>浏览器ID</td>
                    <td>店铺名称</td>
                    <td>分组名称</td>
                    <td>店铺状态</td>
                    <td>可提现波兰币</td>
                    <td>需缴费</td>
                    <td>计费时间段</td>
                    <td>销售额</td>
                    <td>平台佣金</td>
                    <td>店铺质量分</td>
                    <td>未解决纠纷</td>
                    <td>准时发货</td>
                    <td>推荐评分</td>
                    <td>不推荐评分</td>
                    <td>产品数量</td>
                    <td>第1天流量</td>
                    <td>第2天流量</td>
                    <td>第3天流量</td>
                    <td>第4天流量</td>
                    <td>第5天流量</td>
                    <td>第6天流量</td>
                    <td>第7天流量</td>
                    <td>更新时间</td>

                </tr>
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("bid")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("bname")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("GroupName")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("DpStatus")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("Available_funds")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("Settlements_with_Allegro")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("Summary_of_costs")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("Sales_value")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("Sales_costs")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("Sales_quality")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("Unresolved_discussions")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("On_time_dispatch")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("Reconmended_ratings")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("Not_Reconmended_ratings")%>
                            </td>
                             <td style="text-align: center">
                                <%# Eval("pcount")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("views_day1")%>
                                
                            </td>
                            <td style="text-align: center">
                                <%# Eval("views_day2")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("views_day3")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("views_day4")%>
                               
                            </td>
                            <td style="text-align: center">
                                <%# Eval("views_day5")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("views_day6")%>
                               
                            </td>
                            <td style="text-align: center">
                                <%# Eval("views_day7")%>
                            </td>
                            <td style="text-align: center">
                                <%#  gettime( Eval("update_time").ToString())%>
                            </td>


                        </tr>

                    </ItemTemplate>
                </asp:Repeater>

            </table>
        </form>
        <br />
        <asp:Literal ID="lify2" runat="server"></asp:Literal>
    </div>
    <script>
        function getQueryParams() {
            const queryString = window.location.search;
            const urlParams = new URLSearchParams(queryString);
            const params = {};
            for (const [key, value] of urlParams.entries()) {
                params[key] = value;

            }
            return params;
        }
        document.addEventListener("DOMContentLoaded", function () {
            const params = getQueryParams();


            const txtsjbm = document.getElementById('txtsjbm');
            const txtyybm = document.getElementById('txtyybm');
            const order = document.getElementById('order');

            if (txtsjbm && params.txtsjbm) {
                txtsjbm.value = params.txtsjbm;
            }
            if (txtyybm && params.txtyybm) {
                txtyybm.value = params.txtyybm;
            }
            if (order && params.order) {
                order.value = params.order;
            }


        });
    </script>
</body>
</html>
