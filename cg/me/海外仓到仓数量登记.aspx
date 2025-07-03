<asp:Literal ID="Literal2" runat="server"></asp:Literal><%@ Page Language="C#" AutoEventWireup="true" CodeBehind="海外仓到仓数量登记.aspx.cs" Inherits="WebApplication11.cg.海外仓到仓数量登记" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>海外仓到仓数量登记</title>
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
                border: 1px solid crimson;
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
                <h3>当前页面【<span style="color: crimson">海外仓到仓数量登记</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                商家编码：<asp:TextBox ID="txtsjbm" TextMode="Password" runat="server"></asp:TextBox>&nbsp;
                海外仓入库单号：<asp:TextBox ID="txthaiwaicangrukudanhao" runat="server"></asp:TextBox>
                &nbsp;
                广东发出单号：<asp:TextBox ID="txtguangdongfachudanhao" runat="server"></asp:TextBox>
                &nbsp;
                <asp:Button ID="Button1" runat="server" Text="查找数据" BackColor="Green" ForeColor="White" OnClick="Button1_Click" />
                &nbsp;
                <asp:Button ID="Button2" runat="server" OnClientClick="JavaScript:return confirm('确定保存整页？');" Text="保存整页" BackColor="Blue" ForeColor="White" OnClick="Button2_Click1" />
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />

            <table class="ttt">

                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />


                            </td>
                            <td style="width: 30%; text-align: center">
                                <img src='/Uploads/<%#Eval("mainimg") %>' style='width: 300px; height: 300px' />
                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 30%" class="bbb">货品标题</td>
                                        <td><%# (Eval("huopinbiaoti").ToString()) %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">莆田发出数量</td>
                                        <td><%# (Eval("putianfachushuliang").ToString()) %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">莆田发出日期</td>
                                        <td><%# (Eval("putianfachuriqi").ToString()) %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">图片网址</td>
                                        <td>
                                            <%# getiiiiimg(Eval("tupianwangzhi").ToString()) %>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">广东发出日期</td>
                                        <td><%# Eval("guangdongfachuriqi") %></td>
                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">广东发出单号</td>
                                        <td><%# Eval("guangdongfachudanhao") %></td>
                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">海外仓入库单号</td>
                                        <td><%# Eval("haiwaicangrukudanhao") %></td>
                                    </tr>

                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>



                                    <tr>
                                        <td style="width: 30%" class="bbb">海外仓上架数量</td>
                                        <td>
                                            <asp:TextBox ID="txthaiwaicangshangjiashuliang" Text='<%# Eval("haiwaicangshangjiashuliang") %>' Width="10%" runat="server"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">财务状态</td>
                                        <td>
                                            <asp:TextBox ID="txtcaiwuzhuangtai" Text='<%# Eval("caiwuzhuangtai") %>' Width="10%" runat="server"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">财务审核备注</td>
                                        <td>
                                            <asp:TextBox ID="txtcaiwushenhebeizhu" Text='<%# Eval("caiwushenhebeizhu") %>' Width="30%" runat="server"></asp:TextBox>
                                        </td>

                                    </tr>




                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>


                                    <tr>
                                        <td></td>

                                        <td>
                                            <asp:Literal ID="licid" Text='<%# Eval("cid") %>' Visible="false" runat="server"></asp:Literal>
                                            <asp:LinkButton ID="btnzd" OnClientClick="JavaScript:return confirm('确定保存？');" runat="server" Text="保存" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="qr" CommandArgument='<%# Eval("cid") %>' />

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
