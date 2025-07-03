<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="莆田发出信息登记.aspx.cs" Inherits="WebApplication11.cg.莆田发出信息登记" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>莆田发出信息登记</title>
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
                <h3>当前页面【<span style="color: crimson">莆田发出信息登记</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                商家编码：<asp:TextBox ID="txtsjbm" runat="server"></asp:TextBox>&nbsp;
                订单状态：<asp:DropDownList ID="dpddzt" runat="server">
                    <asp:ListItem Value="等待买家确认收货">等待买家确认收货</asp:ListItem>
                    <asp:ListItem Value="交易成功">交易成功</asp:ListItem>
                </asp:DropDownList>&nbsp;
                  中文或者印尼语标题：<asp:TextBox ID="txttitle" runat="server" OnTextChanged="txttitle_TextChanged" Width="205px"></asp:TextBox>&nbsp;
                  1688订单号：<asp:TextBox ID="txtdingdanbianhao" runat="server" Width="150px"></asp:TextBox>&nbsp;
                <asp:Button ID="Button1" runat="server" Text="查找数据" BackColor="Green" ForeColor="White" OnClick="Button1_Click" />

                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="Button2" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页" OnClick="Button2_Click1" />
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
                                <asp:Literal ID="licid" Text='<%# Eval("cid") %>' Visible="false" runat="server"></asp:Literal>

                            </td>
                            <td style="width: 30%; text-align: center">
                                <img src='/Uploads/<%#Eval("mainimg") %>' style='width: 300px; height: 300px' />
                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 30%" class="bbb">订单编号</td>
                                        <td><%# Eval("dingdanbianhao") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">订单创建时间</td>
                                        <td><%# Eval("dingdanchuangjianshijian") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">货品标题</td>
                                        <td><%# (Eval("huopinbiaoti").ToString()) %></td>
                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">可发数量</td>
                                        <td><%#getkefa( Eval("shuliang").ToString(),Eval("putianfachushuliang").ToString(),Eval("xutuihuoshuliang").ToString()) %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">Offer ID</td>
                                        <td><%# Eval("Offer_ID") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">SKU ID</td>
                                        <td><%# Eval("SKU_ID") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">印尼语标题</td>
                                        <td><%# (Eval("rucangyinnibiaoti").ToString()) %></td>

                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">莆田发出数量<span style="color: red">(*)</span></td>
                                        <td>
                                            <asp:TextBox ID="txtputianfachushuliang" Text='<%# Eval("putianfachushuliang") %>' Width="40%" runat="server"></asp:TextBox>
                                            <span style="color: red">(*)多次用 | 号隔开</span>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">莆田发出日期<span style="color: red">(*)</span></td>
                                        <td>
                                            <asp:TextBox ID="txtputianfachuriqi" Text='<%# Eval("putianfachuriqi") %>' Width="40%" runat="server"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">莆田发出单号<span style="color: red">(*)</span></td>
                                        <td>
                                            <asp:TextBox ID="txtputianfachudanhao" Text='<%# Eval("putianfachudanhao") %>' Width="40%" runat="server"></asp:TextBox>
                                            <span style="color: red">(*)多次用 | 号隔开</span>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">莆田发出快递费用<span style="color: red">(*)</span></td>
                                        <td>
                                            <asp:TextBox ID="txtputianfachukuaidifeiyong" Text='<%# Eval("putianfachukuaidifeiyong") %>' Width="40%" runat="server"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">莆田发出备注</td>
                                        <td>
                                            <asp:TextBox ID="txtputianfachubeizhu" Text='<%# Eval("putianfachubeizhu") %>' Width="80%" runat="server"></asp:TextBox>
                                        </td>

                                    </tr>



                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>


                                    <tr>
                                        <td></td>

                                        <td>
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
