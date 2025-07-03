<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="阿里狗买家消息.aspx.cs" Inherits="WebApplication11.cg.阿里狗买家消息" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>阿里狗买家消息</title>
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
                <h3>当前页面【<span style="color: #cba537">阿里狗买家消息</span>】
               
                </h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal>
                </h2>
            </div>
            <div>
                商家：<asp:TextBox ID="txtsjbm" runat="server"></asp:TextBox>&nbsp;
                 <asp:Button ID="Button1" runat="server" Text="查找" BackColor="Green" ForeColor="White" OnClick="Button1_Click1" />
                <br />

                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>

            <br />

            <table class="ttt">
                <tr class="aaa4" style="text-align: center">
                    <td>序号</td>
                    <td>浏览器ID</td>
                    <td>店铺名称</td>
                    <td>分组名称</td>
                    <td>消息状态</td>
                    <td>采集时间</td>
                    <td>处理</td>


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
                            <td style="text-align: center">需回复
                            </td>
                            <td style="text-align: center">
                                <%# Eval("lasttime")%>
                            </td>
                            <td style="text-align: center">
                                <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确定已处理？');" runat="server" Text="已处理" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="up" CommandArgument='<%# Eval("bid") %>' />
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
