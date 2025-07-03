<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="出错任务.aspx.cs" Inherits="WebApplication11.cg.出错任务" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>出错任务</title>
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
                <h3>当前页面【<span style="color: #37cbc5">出错任务</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                <asp:Button ID="Button1" runat="server" Text="加载今日出错店铺" OnClick="Button1_Click" />&nbsp;
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="liid" runat="server" Text='<%# Eval("eid") %>' Visible="false"></asp:Literal>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %>
                                <br />
                                <%# Eval("eid")%>
                            </td>
                           
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 20%" class="bbb">浏览器id：</td>
                                        <td ><%# Eval("浏览器id") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">时间：</td>
                                        <td ><%# Eval("时间") %></td>
                                    </tr>
                                     <tr>
                                        <td style="width: 20%" class="bbb">出错内容：</td>
                                        <td ><span style="color:red"><%# Eval("出错内容") %></span></td>
                                    </tr>
                                     <tr>
                                        <td style="width: 20%" class="bbb">店铺名字：</td>
                                        <td ><%# Eval("bname") %></td>
                                    </tr>
                                     <tr>
                                        <td style="width: 20%" class="bbb">分组：</td>
                                        <td ><%# Eval("gname") %></td>
                                    </tr>
                                      <tr>
                                        <td style="width: 20%" class="bbb">平台：</td>
                                        <td ><%# Eval("pintai") %></td>
                                    </tr>
                                     <tr>
                                        <td style="width: 20%" class="bbb"></td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确定已经处理？');" runat="server" Text="已经处理" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="up" CommandArgument='<%# Eval("eid") %>' />
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
