<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="财务表补充信息.aspx.cs" Inherits="WebApplication11.cg.财务表补充信息" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>财务表补充信息</title>
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
                <h3>当前页面【<span style="color: crimson">财务表补充信息</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                输入商家编码：<asp:TextBox ID="txtsjbm" runat="server"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="查找一条信息为空的数据" BackColor="Green" ForeColor="White" OnClick="Button1_Click" />
                &nbsp;
                <asp:Button ID="Button2" runat="server" Text="查找一条信息为0的数据" BackColor="Blue" ForeColor="White" OnClick="Button2_Click2" />
                &nbsp;
                <asp:Button ID="Button3" runat="server" Text="查找跳过的数据" BackColor="Red" ForeColor="White" OnClick="Button3_Click1" />
                <br />
                <br />
                 输入标题：<asp:TextBox ID="txthuopinbiaoti" runat="server"></asp:TextBox> &nbsp;
                <asp:Button ID="Button4" runat="server" Text="查找" BackColor="Red" ForeColor="White" OnClick="Button4_Click"  />
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />

            <table class="ttt">

                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">
                    <ItemTemplate>
                        <tr>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />
                                <asp:Literal ID="licid" runat="server" Visible="false" Text='<%# Eval("cid")%>'></asp:Literal>
                                <asp:Literal ID="liOffer_ID" runat="server" Visible="false" Text='<%# Eval("Offer_ID")%>'></asp:Literal>
                                <asp:Literal ID="liSKU_ID" runat="server" Visible="false" Text='<%# Eval("SKU_ID")%>'></asp:Literal>
                                 <asp:Literal ID="limainimg222" runat="server" Visible="false" Text='<%# Eval("mainimg")%>'></asp:Literal>
                            </td>
                            <td style="width: 30%; text-align: center">
                                <asp:Literal ID="limainimg" runat="server" Visible="false"></asp:Literal>
                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 30%" class="bbb">Offer ID</td>
                                        <td><%# Eval("Offer_ID") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">SKU ID</td>
                                        <td><%# Eval("SKU_ID") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688采购链接</td>

                                        <td><a href="<%# Eval("1688链接") %>" target="_blank">打开网址</a>&nbsp;&nbsp;
                                           <input type="text" value='<%# Eval("1688链接") %>' style="width: 20px" id="url<%# Container.ItemIndex+1 %>" />
                                            <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">货品标题</td>
                                        <td>

                                            <%# Eval("huopinbiaoti") %>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="bbb">单价(元)</td>
                                        <td><%# Eval("danjia") %></td>

                                    </tr>
                                    <tr>
                                        <td class="bbb">图片网址：</td>
                                        <td>


                                            <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" />
                                            <asp:LinkButton ID="LinkButton2" OnClientClick="JavaScript:return confirm('确认上传？');" runat="server" Text="确认上传" ForeColor="White" BackColor="Blue" CssClass="butt" CommandName="upimg" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">运营编码<span style="color: red">(*)</span></td>

                                        <td>
                                            <asp:TextBox ID="txtyunyingbianma" Text='<%# Eval("yunyingbianma")%>' Width="80%" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">入仓ITEMID<span style="color: red">(*)</span></td>

                                        <td>
                                            <asp:TextBox ID="txtrucangITEMID" Text='<%# Eval("rucangITEMID")%>' Width="80%" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">入仓卖家SKU<span style="color: red">(*)</span></td>

                                        <td>
                                            <asp:TextBox ID="txtrucangSKUID" Text='<%# Eval("rucangSKUID")%>' Width="80%" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">入仓印尼标题<span style="color: red">(*)</span></td>

                                        <td>
                                            <asp:TextBox ID="txtrucangyinnibiaoti" Text='<%# Eval("rucangyinnibiaoti")%>' Width="80%" runat="server"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="bbb">数据类型</td>
                                        <td>
                                            <asp:Literal ID="lisjtype" runat="server" Visible="false" Text='<%# Eval("sjtype")%>'></asp:Literal>
                                            <asp:RadioButtonList ID="rdsjtype" CssClass="rdcss" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="发仓货物">发仓货物</asp:ListItem>
                                                <asp:ListItem Value="阿里狗">阿里狗</asp:ListItem>
                                                <asp:ListItem Value="耗材">耗材</asp:ListItem>
                                            </asp:RadioButtonList>
                                            &nbsp;<asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确定跳过？');" runat="server" Text="跳过" ForeColor="White" BackColor="Red" CssClass="butt" CommandName="tg" CommandArgument='<%# Eval("SKU_ID").ToString()+"|"+Eval("Offer_ID").ToString() %>' />
                                        </td>

                                    </tr>


                                    <tr>
                                        <td colspan="3"></td>
                                    </tr>


                                    <tr>
                                        <td></td>

                                        <td>
                                            <asp:LinkButton ID="btnzd" OnClientClick="JavaScript:return confirm('确定保存？');" runat="server" Text="保存" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="qr" CommandArgument='<%# Eval("SKU_ID").ToString()+"|"+Eval("Offer_ID").ToString() %>' />

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
