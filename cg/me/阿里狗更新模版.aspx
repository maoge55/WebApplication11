<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="阿里狗更新模版.aspx.cs" Inherits="WebApplication11.cg.阿里狗更新模版" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>阿里狗更新模版</title>
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
                border: 1px solid #37cbc5;
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

        .ttt td {
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
    <form id="form1" runat="server">
        <div>
            <div>
                <h3>当前页面【<span style="color: #37cbc5">阿里狗更新模版</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <asp:Button ID="btnsearch" runat="server" Text="查找需要更新的类目ID" Width="150px" OnClientClick="" BackColor="Blue" ForeColor="White" OnClick="btnsearch_Click" />
            <br />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>


            <br />
            <table class="ttt">
                <tr class="aaa4">
                    <td>序号</td>
                    <td>类目id</td>
                    <td>出错类型</td>
                    <td>时间</td>
                    <td>处理总次数</td>
                    <td>报表下载</td>
                    <td>操作</td>

                </tr>
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="lileimu" runat="server" Text='<%# Eval("leimu") %>' Visible="false"></asp:Literal>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />

                            </td>
                            <td style="width: 15%; text-align: center">
                                <%# Eval("leimu") %>
                            </td>
                            <td style="width: 15%; text-align: center">
                                <%# Eval("type") %>
                            </td>
                            <td style="width: 15%; text-align: center">
                                <%# Eval("time") %>
                            </td>
                            <td style="width: 15%; text-align: center">
                                <%# Eval("cl_count") %>
                            </td>
                            <td style="width: 15%; text-align: center">
                                
                              <%# Eval("downloadPath").ToString().Replace("D:/mmmm/","").ToString()%>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确定已更新？');" runat="server" Text=" 已更新" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="up" CommandArgument='<%# Eval("leimu") %>' />
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
