<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="财务表补充重量体积.aspx.cs" Inherits="WebApplication11.cg.财务表补充重量体积" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>财务表补充重量体积</title>
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
                <h3>当前页面【<span style="color: crimson">财务表补充重量体积</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                输入商家编码：<asp:TextBox ID="txtsjbm" runat="server"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="查找一条重量体积为空的数据" BackColor="Green" ForeColor="White" OnClick="Button1_Click" />
                &nbsp;
                <asp:Button ID="Button2" runat="server" Text="查找一条重量体积为0的数据" BackColor="Blue" ForeColor="White" OnClick="Button2_Click2" />
                <br />
                中文或者印尼语标题：<asp:TextBox ID="txttitle" runat="server" Width="205px"></asp:TextBox>&nbsp;
                <asp:Button ID="Button3" runat="server" Text="查找数据" BackColor="Green" ForeColor="White" OnClick="Button3_Click1" />
                &nbsp;
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                <br />
                <br />
                <asp:Button ID="Button4" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页" OnClick="Button4_Click" />

            </div>
            <br />

            <table class="ttt">

                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />
                                <asp:Literal ID="licid" runat="server" Visible="false" Text='<%# Eval("cid")%>'></asp:Literal>
                                <asp:Literal ID="liOffer_ID" runat="server" Visible="false" Text='<%# Eval("Offer_ID")%>'></asp:Literal>
                                <asp:Literal ID="liSKU_ID" runat="server" Visible="false" Text='<%# Eval("SKU_ID")%>'></asp:Literal>

                            </td>
                            <td style="width: 30%; text-align: center">
                                <img src='/Uploads/<%#Eval("mainimg") %>' style='width: 300px; height: 300px' />
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
                                        <td colspan="2"></td>
                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">长cm<span style="color: red">(*无法确定，可以填写“0”)</span></td>

                                        <td>
                                            <asp:TextBox ID="txtchang" Text='<%# Eval("chang") %>' Width="80%" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">宽cm<span style="color: red">(*无法确定，可以填写“0”)</span></td>

                                        <td>
                                            <asp:TextBox ID="txtkuan" Text='<%# Eval("kuan") %>' Width="80%" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">高cm<span style="color: red">(*无法确定，可以填写“0”)</span></td>

                                        <td>
                                            <asp:TextBox ID="txtgao" Text='<%# Eval("gao") %>' Width="80%" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">重量kg<span style="color: red">(*无法确定，可以填写“0”)</span></td>

                                        <td>
                                            <asp:TextBox ID="txtzhongliang" Text='<%# Eval("zhongliang") %>' Width="80%" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">体积重量kg<span style="color: red">(*无法确定，可以填写“0”)</span></td>

                                        <td>
                                            <asp:TextBox ID="txttijizhongliang" Text='<%# Eval("tijizhongliang") %>' Width="80%" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">运费海运每方<span style="color: red">(*无法确定，可以填写“0”)</span></td>

                                        <td>
                                            <asp:TextBox ID="txtyunfeihaiyunmeifang" Text='<%# Eval("yunfeihaiyunmeifang") %>' Width="80%" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">运费空运每公斤<span style="color: red">(*无法确定，可以填写“0”)</span></td>

                                        <td>
                                            <asp:TextBox ID="txtyunfeikongyunmeigongjin" Text='<%# Eval("yunfeikongyunmeigongjin") %>' Width="80%" runat="server"></asp:TextBox></td>
                                    </tr>





                                    <tr>
                                        <td colspan="3"></td>
                                    </tr>


                                    <tr>
                                        <td></td>

                                        <td>
                                            <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('填写整页？');" runat="server" Text="填写整页" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="txzy" CommandArgument='<%# Eval("SKU_ID").ToString()+"|"+Eval("Offer_ID").ToString() %>' />

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
