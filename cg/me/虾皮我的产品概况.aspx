<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="虾皮我的产品概况.aspx.cs" Inherits="WebApplication11.cg.虾皮我的产品概况" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>虾皮我的产品概况</title>
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
            <h3>当前页面【<span style="color: #cba537">虾皮我的产品概况</span>】
              
            </h3>

            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>
        <div>
            <form id="form2" action="虾皮我的产品概况.aspx" method="get">
                商家编码：
                  <input type="text" name="txtsjbm" id="txtsjbm" />

                &nbsp;
                排序规则：<select id="order" name="order">
                    <option value="GroupName">分组排序</option>
                    <option value="BName">店铺名称排序</option>
                    <option value="live">在售</option>
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
                    <td>产品额度</td>
                    <td>在售</td>
                    <td>屏蔽</td>
                    <td>降权</td>
                    <td>虾皮删除</td>
                    <td>虾皮审核</td>
                    <td>下架</td>
                    <td>草稿</td>
                    <td>店铺ID</td>
                    <td>浏览器ID</td>
                    <td>店铺名称</td>
                    <td>浏览器分组</td>
                    <td>更新时间</td>
                    
                </tr>
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("listing_limit")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("live")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("banned")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("deboosted")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("shopee_deleted")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("under_shopee_review")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("delisted")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("draft")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("shopid")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("bid")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("BName")%>
                            </td>
                           <td style="text-align: center">
                                <%# Eval("GroupName")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("update_time")%>
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
            
            const order = document.getElementById('order');

            if (txtsjbm && params.txtsjbm) {
                txtsjbm.value = params.txtsjbm;
            }

            if (order && params.order) {
                order.value = params.order;
            }


        });
    </script>
</body>
</html>
