<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="虾皮首页信息.aspx.cs" Inherits="WebApplication11.cg.虾皮首页信息" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>虾皮首页信息</title>
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
            <h3>当前页面【<span style="color: #cba537">虾皮首页信息</span>】
             
            </h3>

            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>
        <div>
            <form id="form2" action="虾皮首页信息.aspx" method="get">
                商家编码：
                  <input type="text" name="txtsjbm" id="txtsjbm" />
                &nbsp;
                运营编码：<input type="text" name="txtyybm" id="txtyybm" />
                &nbsp;
                排序规则：<select id="order" name="order">
                    <option value="orders">当日订单</option>
                    <option value="visitors">访客</option>
                    <option value="My_Penalty">处罚</option>
                    <option value="to_process_shipment">需处理订单</option>
                    <option value="ad_count">在用广告数量</option>
                     <option value="ads_credit">广告余额</option>
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
                    <td>浏览器名称</td>
                    <td>浏览器ID</td>
                    <td>浏览器分组</td>
                    <td>在用广告数量</td>
                    <td>广告余额</td>
                    <td>待付款订单</td>
                    <td>需处理订单</td>
                    <td>已处理订单</td>
                    <td>待取消订单</td>
                    <td>待退货订单</td>
                    <td>被屏蔽产品</td>
                    <td>无库存产品</td>
                    <td>可参与竞价</td>
                    <td>访客</td>
                    <td>访客趋势</td>
                    <td>页面访问量</td>
                    <td>页面访问量趋势</td>
                    <td>当日订单</td>
                    <td>当日订单趋势</td>
                    <td>转化率</td>
                    <td>转化率趋势</td>
                    <td>未履约订单比例</td>
                    <td>延迟发货比例</td>
                    <td>订单准备时间</td>
                    <td>处罚</td>
                    <td>更新时间</td>
                </tr>
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("shopName")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("bid")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("GroupName")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("ad_count")%>
                            </td>
                             <td style="text-align: center">
                                <%# Eval("ads_credit")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("unpaid")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("to_process_shipment")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("processed_shipment")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("pending_cancellation")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("pending_return")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("banned_products")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("sold_out_products")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("to_join_bidding")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("visitors")%>
                            </td>
                            <td style="text-align: center">
                                <%# (float.Parse( Eval("visitors_trend").ToString())*100).ToString("0.00")%>% 
                               
                            </td>
                            <td style="text-align: center">
                                <%# Eval("page_views")%>
                            </td>
                            <td style="text-align: center">
                                <%# (float.Parse( Eval("page_views_trend").ToString())*100).ToString("0.00")%>% 
                            
                                
                            </td>
                            <td style="text-align: center">
                                <%# Eval("orders")%>
                            </td>
                            <td style="text-align: center">
                                <%# (float.Parse( Eval("orders_trend").ToString())*100).ToString("0.00")%>% 
                          
                               
                            </td>
                            <td style="text-align: center">
                                <%# (float.Parse( Eval("conversion_rate").ToString())*100).ToString("0.00")%>% 
                            </td>
                            <td style="text-align: center">
                                <%# (float.Parse( Eval("conversion_rate_trend").ToString())*100).ToString("0.00")%>% 
                            
                               
                            </td>



                            <td style="text-align: center">
                                <%# Eval("Non_fulfilment_Rate")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("Late_Shipment_Rate")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("Preparation_Time")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("My_Penalty")%>
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
