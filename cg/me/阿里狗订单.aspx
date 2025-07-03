<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="阿里狗订单.aspx.cs" Inherits="WebApplication11.cg.阿里狗订单" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>阿里狗订单</title>
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
                <h3>当前页面【<span style="color: #cba537">阿里狗订单</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal>
                </h2>
            </div>
            <div>
                商家编码：<asp:TextBox ID="txtsjbm" runat="server" Width="118px"></asp:TextBox>&nbsp;
                状态选择：
                <asp:DropDownList ID="dpzt" runat="server">
                    <asp:ListItem Value="待处理">待处理</asp:ListItem>
                    <asp:ListItem Value="已采购">已采购</asp:ListItem>
                    <asp:ListItem Value="已发货">已发货</asp:ListItem>
                    <asp:ListItem Value="异常订单">异常订单</asp:ListItem>

                </asp:DropDownList>&nbsp;
                <asp:Button ID="Button1" runat="server" Text="加载数据" OnClick="Button1_Click" /><br />
                <span style="color: red; font-weight: bold">
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal></span>
                <br />
                <span style="color: blue; font-weight: bold">
                    <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                </span>

            </div>
            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="liid" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Literal>
                            <asp:Literal ID="lizt" runat="server" Text='<%# Eval("zt") %>' Visible="false"></asp:Literal>
                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />
                                <br />
                                <%# Eval("id")%>
                            </td>
                            <td style="width: 30%; text-align: center">
                                <img src='<%# Eval("lineItems_0_offer_imageUrl") %>' style="width: 300px; height: 300px" />
                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 30%" class="bbb">浏览器ID</td>
                                        <td style="width: 70%">
                                            <%# Eval("bid") %>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">店铺名称</td>
                                        <td style="width: 70%">
                                            <%# Eval("bname") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">订单号</td>
                                        <td style="width: 70%">
                                            <%# Eval("order_id") %>
                                            <%# Eval("isMutilOffer").ToString()=="1"?"<strong style='color:red'>**(合并订单)</strong>":"" %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">名</td>
                                        <td style="width: 70%">
                                            <%# Eval("delivery_address_firstName") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">姓</td>
                                        <td style="width: 70%">
                                            <%# Eval("delivery_address_lastName") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">详细地址</td>
                                        <td style="width: 70%">
                                            <%# Eval("delivery_address_street") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">城市</td>
                                        <td style="width: 70%">
                                            <%# Eval("delivery_address_city") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">邮编</td>
                                        <td style="width: 70%">
                                            <%# Eval("delivery_address_zipCode") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">国家</td>
                                        <td style="width: 70%">
                                            <%# Eval("delivery_address_countryCode") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">电话</td>
                                        <td style="width: 70%">
                                            <%# Eval("delivery_address_phoneNumber") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">买家备注</td>
                                        <td style="width: 70%">
                                            <%# Eval("buyerNote") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">订单金额PLN</td>
                                        <td style="width: 70%">
                                            <%# Eval("payments_0_paid_amount") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">订单金额CNY</td>
                                        <td style="width: 70%">
                                            <%#  getcnjg(Eval("payments_0_paid_amount").ToString()) %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">下单时间</td>
                                        <td style="width: 70%">
                                            <%# Eval("orderDate") %>
                                        </td>

                                    </tr>



                                    <tr>
                                        <td style="width: 30%" class="bbb">订单状态</td>
                                        <td style="width: 70%">
                                            <%# Eval("status") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">支付状态</td>
                                        <td style="width: 70%">
                                            <%# Eval("summary_paymentStatus") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">产品标题</td>
                                        <td style="width: 70%">
                                            <%# Eval("lineItems_0_offer_name") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">产品链接</td>
                                        <td style="width: 70%">

                                            <a href="<%# Eval("lineItems_0_offer_offerUrl") %>" target="_blank">打开网址</a>&nbsp;&nbsp;
                                           <input type="text" value='<%# Eval("lineItems_0_offer_offerUrl") %>' style="width: 20px" id="url<%# Container.ItemIndex+1 %>" />
                                            <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                            &nbsp;
                                            <a href="阿里狗订单查货源.aspx?url=<%# Eval("lineItems_0_offer_offerUrl") %>" style="color: blue; font-weight: bold" target="_blank">查找已有货源
                                            </a>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">产品数量</td>
                                        <td style="width: 70%">
                                            <%# Eval("lineItems_0_quantity") %>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">状态</td>
                                        <td style="width: 70%">

                                            <asp:DropDownList ID="dpzt2" runat="server">


                                                <asp:ListItem Value="待处理">待处理</asp:ListItem>
                                                <asp:ListItem Value="已采购">已采购</asp:ListItem>
                                                <asp:ListItem Value="已发货">已发货</asp:ListItem>
                                                <asp:ListItem Value="异常订单">异常订单</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">备注</td>
                                        <td style="width: 70%">
                                            <asp:TextBox ID="txtbz" runat="server" Width="80%" Text=' <%# Eval("beizhu") %>'></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确定保存？');" runat="server" Text="保存数据" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="up" CommandArgument='<%# Eval("id") %>' />
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
