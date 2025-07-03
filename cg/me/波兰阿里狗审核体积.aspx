<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="波兰阿里狗审核体积.aspx.cs" Inherits="WebApplication11.cg.波兰阿里狗审核体积" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>阿里狗审核体积</title>
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
                border: 1px solid #ff692d;
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

        .rdcss td:nth-child(1) {
            background: green;
            width: 200px;
        }

        .rdcss td:nth-child(2) {
            background: black;
            width: 200px;
        }

        .rdcss td:nth-child(3) {
            background: blue;
            width: 200px;
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
                <h3>当前页面【<span style="color: #ff692d">阿里狗审核体积</span>】(评论大于5 或者 总销量大于5)</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                <asp:Button ID="Button1" BackColor="Green" ForeColor="White" runat="server" Text="加载20条正常数据" OnClick="Button1_Click" />&nbsp;
                <asp:Button ID="Button4" BackColor="Blue" ForeColor="White" runat="server" Text="加载20条跳过数据" OnClick="Button4_Click1" />&nbsp;
                <br />
                <br />
                <asp:Button ID="Button5" CssClass="gg"  BackColor="Green" ForeColor="White"  runat="server" Text="全选 可以发货" OnClick="Button5_Click" />&nbsp;
                <asp:Button ID="Button6" CssClass="gg" BackColor="black" ForeColor="White"  runat="server" Text="全选 拉黑产品" OnClick="Button6_Click" />&nbsp;
                <asp:Button ID="Button7" CssClass="gg" BackColor="blue" ForeColor="White"  runat="server" Text="全选 跳过" OnClick="Button7_Click" />&nbsp;

                 <asp:Button ID="Button2" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页" OnClick="Button2_Click1" />
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="liid" runat="server" Text='<%# Eval("pid") %>' Visible="false"></asp:Literal>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />
                                <br />
                                <%# Eval("pid")%>
                            </td>
                            <td style="width: 30%; text-align: center">
                                <img src='<%# access_sql.getimg( Eval("ZhuTuWangZhi").ToString())%>' style="width: 200px; height: 200px" />
                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 10%" class="bbb">产品ID</td>
                                        <td style="width: 50%"><%# Eval("pid") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">类目ID和产品数量</td>
                                        <td style="width: 50%"><%# Eval("leimu") %>/<%# Eval("leimu_count") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%" class="bbb">产品链接</td>
                                        <td style="width: 50%"><a href="<%# Eval("purl") %>" target="_blank">打开网址</a>
                                            <input type="text" value='<%# Eval("purl") %>' style="width: 20px" id="url<%# Container.ItemIndex+1 %>" />
                                            <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">标题</td>
                                        <td><%# Eval("biaoti") %></td>

                                    </tr>


                                    <tr>

                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rdjg" CssClass="rdcss" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="yes" class="gg"><span style="color:white">可以发货</span></asp:ListItem>
                                                <asp:ListItem Value="no" class="rr"><span style="color:white">拉黑该产品</span></asp:ListItem>
                                                <asp:ListItem Value="tg" class="tt"><span style="color:white">跳过</span></asp:ListItem>
                                            </asp:RadioButtonList>

                                        </td>

                                    </tr>
                                </table>





                            </td>

                        </tr>
                        <tr></tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td></td>
                    <td></td>

                    <td>
                        <asp:Button ID="Button3" runat="server" ForeColor="Blue" Width="200px" Height="40px" Font-Size="20px" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页" OnClick="Button3_Click1" /></td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>
