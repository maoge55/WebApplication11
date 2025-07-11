﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="虾皮创建广告任务.aspx.cs" Inherits="WebApplication11.cg.虾皮创建广告任务" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>虾皮创建广告任务</title>
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
    <form id="form1" runat="server">
        <div>
            <div>
                <h3>当前页面【<span style="color: #cba537">虾皮创建广告任务</span>】
               
                </h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal>
                </h2>
            </div>
            <div>
                店铺名称：<asp:TextBox ID="txtbname" runat="server"></asp:TextBox>&nbsp;
             
              
                执行状态：
                <asp:DropDownList ID="dpstate" runat="server">
                    <asp:ListItem Value="全部">全部</asp:ListItem>
                    <asp:ListItem Value="1">执行成功</asp:ListItem>
                    <asp:ListItem Value="0">未执行成功</asp:ListItem>
                </asp:DropDownList>
                排序：
                <asp:DropDownList ID="dporder" runat="server">
                    <asp:ListItem Value="店铺名称">店铺名称</asp:ListItem>
                    <asp:ListItem Value="更新时间-大到小">更新时间-大到小</asp:ListItem>
                    <asp:ListItem Value="更新时间-小到大">更新时间-小到大</asp:ListItem>
                </asp:DropDownList>
                &nbsp;
                 <asp:Button ID="Button1" runat="server" Text="查找" BackColor="Green" ForeColor="White" OnClick="Button1_Click1" />

                <br />
                <span style="color: red">
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                </span>
            </div>

            <br />

            <table class="ttt">
                <tr class="aaa4" style="text-align: center">
                    <td>序号</td>

                    <td>浏览器ID</td>
                    <td>浏览器名字</td>
                    <td>浏览器分组</td>
                    <td>商家编码</td>
                    <td>运营编码</td>
                    <td>执行任务前需创建广告数</td>
                    <td>执行任务后需创建广告数</td>
                    <td>执行任务前已有广告数</td>
                    <td>执行任务后已有广告数</td>
                    <td>任务执行状态</td>
                    <td>数据更新时间</td>
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
                                <%# Eval("SJBM")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("YYBM")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("need_create_before")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("need_create_after")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("has_created_before")%>
                            </td>
                              <td style="text-align: center">
                                <%# Eval("has_created_after")%>
                            </td>
                              <td style="text-align: center">
                                <%# Eval("rw_msg")%>
                            </td>
                              <td style="text-align: center">
                                <%# Eval("update_time")%>
                            </td>
                           
                        </tr>

                    </ItemTemplate>
                </asp:Repeater>

            </table>


        </div>
    </form>
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
