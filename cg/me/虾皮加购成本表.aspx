<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="虾皮加购成本表.aspx.cs" Inherits="WebApplication11.cg.虾皮加购成本表" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>虾皮加购成本表</title>
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



        .anniu1 {
            border: 1px red solid;
        }

        .aaa4 td {
            background: #fb686842;
            text-align: center;
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
            <h3>当前页面【<span style="color: #cba537">虾皮加购成本表</span>】
             
            </h3>

            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>
        <div>
            <form id="form2" action="虾皮加购成本表.aspx" method="get">
                商家编码：
                  <input type="text" name="txtsjbm" id="txtsjbm" />&nbsp;
                排序规则：<select id="order" name="order">
                    <option value="avg_cost">avg_cost</option>
                    <option value="add_cart_30day">add_cart_30day</option>
                    <option value="Product_ID 大-小">Product_ID 大-小</option>
                    <option value="Product_ID 小-大">Product_ID 小-大</option>
                    <option value="BName">BName</option>
                    <option value="cart_to_conversion_rate 大-小">cart_to_conversion_rate 大-小</option>
                    <option value="cart_to_conversion_rate 小-大">cart_to_conversion_rate 小-大</option>
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
                    <td style="width: 7%">浏览器名称</td>
                    <td>浏览器ID</td>
                    <td style="width: 7%">浏览器分组</td>
                    <td>Product_ID</td>
                    <td>产品标题</td>
                    <td>产品链接</td>
                    <td>conversions</td>
                    <td>创建天数</td>
                    <td>30天广告费</td>
                    <td>30天加购数量</td>
                    <td>每个加购成本</td>
                    <td>加购率</td>
                    <td>购物车转化率</td>
                    <td>每个订单广告费</td>
                    <td>状态</td>
                    <td>操作</td>

                </tr>
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("BName")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("bid")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("GroupName")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("Product_ID")%>
                            </td>
                            <td style="text-align: center; width: 20%">
                                <%# Eval("Product_Name_Ad_Name")%>
                            </td>
                            <td style="text-align: center">
                                <a href='<%# Eval("purl")%>' target="_blank">PURL</a>

                            </td>
                            <td style="text-align: center">
                                <%# Eval("conversions")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("create_days")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("cost_30day")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("add_cart_30day")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("avg_cost")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("conversion_rate")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("cart_to_conversion_rate")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("cost_per_conversion")%>
                            </td>
                            <td style="text-align: center; width: 5%">
                                <%# getzt(Eval("paused").ToString())%>
                            </td>
                            <td style="text-align: center; width: 4%">
                                <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确定暂停？');" runat="server" Text="暂停" ForeColor="White" BackColor="Red" CssClass="butt" CommandName="up" CommandArgument='<%# Eval("Product_ID") %>' />

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



            const ckpaused = document.getElementById('ckpaused');
            const order = document.getElementById('order');
            const txtsjbm = document.getElementById('txtsjbm');
            if (txtsjbm && params.txtsjbm) {
                txtsjbm.value = params.txtsjbm;
            }
            if (ckpaused && params.ckpaused) {

                ckpaused.checked = true;
            }

            if (order && params.order) {
                order.value = params.order;
            }


        });
    </script>
</body>
</html>
