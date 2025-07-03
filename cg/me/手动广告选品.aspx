<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="手动广告选品.aspx.cs" Inherits="WebApplication11.cg.手动广告选品" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>手动广告选品</title>
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
                <h3>当前页面【<span style="color: #37cbc5">手动广告选品</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>

            商家编码：<asp:TextBox ID="txtsjbm_in" runat="server" Width="118px"></asp:TextBox><br />
            原始链接：<br />
            <asp:TextBox ID="txtinoffid" runat="server" Height="157px" Width="95%" TextMode="MultiLine" ValidateRequestMode="Disabled"></asp:TextBox>
            <br />
            <asp:Button ID="btncjsj" runat="server" Text="创建数据" Width="150px" OnClientClick="" BackColor="Blue" ForeColor="White" OnClick="btncjsj_Click" />
            <br />
            加载选择：<asp:CheckBox ID="ckbt" runat="server" Text="未添加标题" />&nbsp;
            <asp:CheckBox ID="ckhy" runat="server" Text="未查找货源数据" />&nbsp;
            <asp:CheckBox ID="cksj" runat="server" Text="未上架数据" />&nbsp;
             <asp:CheckBox ID="ck1688" runat="server" Text="有1688货源链接" />&nbsp;
            排序：<asp:DropDownList ID="dporder" runat="server">
                <asp:ListItem Text="shangjiadianpu asc" Value="shangjiadianpu asc">上架店铺</asp:ListItem>
                <asp:ListItem Text="shangjiadianpulianjie asc" Value="shangjiadianpulianjie desc">商家店铺链接排序</asp:ListItem>
            </asp:DropDownList>
            &nbsp;
             商家编码：<asp:TextBox ID="txtsjbm" runat="server" Width="118px"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="加载数据" Width="150px" BackColor="Green" ForeColor="White" OnClick="Button1_Click2" />



            <br />
            <span style="color: red">
                <asp:Literal ID="Literal1" runat="server"></asp:Literal></span>
            <br />
            <br />
            <br />
            <asp:Button ID="Button2" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页数据" OnClick="Button2_Click1" />



            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="liid" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Literal>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />
                                <br />


                            </td>

                            <td>
                                <table class="ttta">

                                    <tr>
                                        <td class="bbb" style="width: 30%">ID</td>
                                        <td>
                                            <%# Eval("id") %></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb" style="width: 30%">商家编码</td>
                                        <td>
                                            <asp:TextBox ID="txtsjbm" Width="70%" Text='<%# Eval("sjbm") %>' runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">原始链接</td>

                                        <td>
                                            <asp:TextBox ID="txtyuanshilianjie" Width="70%" Text='<%# Eval("yuanshilianjie") %>' runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">标题</td>

                                        <td>
                                            <asp:TextBox ID="txtbiaoti" Width="70%" Text='<%# Eval("biaoti") %>' runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">商家店铺链接</td>

                                        <td>
                                            <asp:TextBox ID="txtshangjiadianpulianjie" Width="70%" Text='<%# Eval("shangjiadianpulianjie") %>' runat="server"></asp:TextBox></td>
                                    </tr>



                                    <tr>
                                        <td class="bbb">备注</td>

                                        <td>
                                            <asp:TextBox ID="txtbeizhu" Width="70%" Text='<%# Eval("beizhu") %>' runat="server"></asp:TextBox></td>
                                    </tr>


                                    <tr>
                                        <td class="bbb"><span style="color: red">*查找货源</span></td>

                                        <td>
                                            <asp:Literal ID="lihuoyuan" runat="server" Visible="false" Text='<%# Eval("huoyuan")%>'></asp:Literal>
                                            <asp:RadioButtonList ID="rdhuoyuan" CssClass="rdcss" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="1">是</asp:ListItem>
                                                <asp:ListItem Value="0">否</asp:ListItem>
                                            </asp:RadioButtonList>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">1688货源链接</td>

                                        <td>
                                            <asp:TextBox ID="txtY_1688url" Width="70%" Text='<%# Eval("Y_1688url") %>' runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">1688价格</td>

                                        <td>
                                            <asp:TextBox ID="txtY_1688price" Width="70%" Text='<%# Eval("Y_1688price") %>' runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">印尼虾皮售价</td>

                                        <td>
                                            <asp:TextBox ID="txtyinnixiapishoujia" Width="300%" Text='<%# Eval("yinnixiapishoujia") %>' runat="server"></asp:TextBox>

                                            <asp:LinkButton ID="LinkButton2" runat="server" Text="计算" ForeColor="White" BackColor="blue" CssClass="butt" CommandName="jiage" CommandArgument='<%# Eval("id") %>' />
                                            1688价格*2.1*2250（自动计算）
                                        </td>
                                    </tr>
                            </td>
                        </tr>
                        <tr>
                            <td class="bbb">上架店铺</td>

                            <td>
                                <asp:TextBox ID="txtshangjiadianpu" Width="70%" Text='<%# Eval("shangjiadianpu") %>' runat="server"></asp:TextBox></td>
                        </tr>


                        <tr>
                            <td class="bbb"><span style="color: red">*上架</span></td>

                            <td>
                                <asp:Literal ID="lishangjia" runat="server" Visible="false" Text='<%# Eval("shangjia")%>'></asp:Literal>
                                <asp:RadioButtonList ID="rdshangjia" CssClass="rdcss" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1">是</asp:ListItem>
                                    <asp:ListItem Value="0">否</asp:ListItem>
                                </asp:RadioButtonList>

                            </td>
                        </tr>
                        <tr>
                            <td class="bbb">广告链接</td>

                            <td>
                                <asp:TextBox ID="txtguanggaolianjie" Width="70%" Text='<%# Eval("guanggaolianjie") %>' runat="server"></asp:TextBox></td>
                        </tr>



                        <tr>

                            <td></td>
                            <td>
                                <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确定保存？');" runat="server" Text="保存数据" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="up" CommandArgument='<%# Eval("id") %>' />
                        </tr>
                        </table>





                            </td>

                        </tr>
                        <tr></tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td></td>


                    <td>
                        <asp:Button ID="Button3" runat="server" ForeColor="Blue" Width="200px" Height="40px" Font-Size="20px" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页" OnClick="Button3_Click1" /></td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>
