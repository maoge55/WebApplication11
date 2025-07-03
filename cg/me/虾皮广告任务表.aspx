<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="虾皮广告任务表.aspx.cs" Inherits="WebApplication11.cg.虾皮广告任务表" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>虾皮广告任务表</title>
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
                <h3>当前页面【<span style="color: #cba537">虾皮广告任务表</span>】
               
                </h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal>
                </h2>
            </div>
            <div>
                浏览器名字：<asp:TextBox ID="txtbname" runat="server"></asp:TextBox>&nbsp;
                广告ID：<asp:TextBox ID="txtcampaign_id" runat="server"></asp:TextBox>&nbsp;
                  关键词：<asp:TextBox ID="txtSearch_Query" runat="server"></asp:TextBox>&nbsp;
                任务类型：
                <asp:DropDownList ID="dpaction_type" runat="server">
                    <asp:ListItem Value="全部">全部</asp:ListItem>
                    <asp:ListItem Value="新增">新增</asp:ListItem>
                    <asp:ListItem Value="修改">修改</asp:ListItem>
                    <asp:ListItem Value="删除">删除</asp:ListItem>
                    <asp:ListItem Value="任务增加异常">任务增加异常</asp:ListItem>
                </asp:DropDownList>
                &nbsp;
                执行状态：
                <asp:DropDownList ID="dpgt_state" runat="server">
                    <asp:ListItem Value="全部">全部</asp:ListItem>
                    <asp:ListItem Value="1">完成</asp:ListItem>
                    <asp:ListItem Value="0">未执行</asp:ListItem>
                </asp:DropDownList>
                &nbsp;
                验证状态：
                <asp:DropDownList ID="dpck_state" runat="server">
                    <asp:ListItem Value="全部">全部</asp:ListItem>
                    <asp:ListItem Value="1">完成</asp:ListItem>
                    <asp:ListItem Value="0">未执行</asp:ListItem>
                </asp:DropDownList>
                &nbsp;
                时间选择：
                <asp:DropDownList ID="dptask_date" runat="server">
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
                    <td>广告ID</td>
                    <td>标题</td>
                    <td>搜索词</td>
                    <td>词来源</td>
                    <td>执行动作</td>
                    <td>达标条件</td>
                    <td>导入任务日期</td>
                    <td>软件执行结果</td>
                    <td>软件验证结果</td>
                    <td>创建时间</td>
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
                                <%# Eval("campaign_id")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("Product_Name_Ad_Name")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("Search_Query")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("kw_type")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("action_type")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("action_des")%>
                            </td>

                            <td style="text-align: center">
                                <%# Eval("task_date")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("gt_state")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("ck_state")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("upload_time")%>
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
