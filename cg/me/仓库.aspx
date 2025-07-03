<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="仓库.aspx.cs" Inherits="WebApplication11.cg.仓库" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>仓库</title>
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
                <h3>当前页面【<span style="color: crimson">仓库</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                商家编码：<asp:TextBox ID="txtsjbm" runat="server"></asp:TextBox>&nbsp;
                订单状态：<asp:DropDownList ID="dpzt" runat="server">
                    <asp:ListItem Value="全部">全部</asp:ListItem>
                    <asp:ListItem Value="等待买家付款">等待买家付款</asp:ListItem>
                    <asp:ListItem Value="等待买家确认收货">等待买家确认收货</asp:ListItem>
                    <asp:ListItem Value="等待卖家发货">等待卖家发货</asp:ListItem>
                    <asp:ListItem Value="交易成功">交易成功</asp:ListItem>
                    <asp:ListItem Value="交易关闭">交易关闭</asp:ListItem>
                </asp:DropDownList>
                <br />
                <br />
                输入快递单号：<asp:TextBox ID="txtkddh" runat="server"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="查找" BackColor="Red" ForeColor="White" OnClick="Button1_Click" />&nbsp;
                输入标题搜索词：<asp:TextBox ID="txtbtkey" runat="server"></asp:TextBox>
                <asp:Button ID="Button2" runat="server" Text="查找" BackColor="Green" ForeColor="White" OnClick="Button2_Click2" Style="height: 21px" />&nbsp;
                 输入1688订单号：<asp:TextBox ID="txtddh" runat="server"></asp:TextBox>
                <asp:Button ID="Button3" runat="server" Text="查找" BackColor="Blue" ForeColor="White" OnClick="Button3_Click2" />&nbsp;
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                <br />
                <br />
                <br />
                <asp:Button ID="Button4" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存整页的4个信息吗？');" Text="保存整页的4个信息" OnClick="Button4_Click" />

            </div>
            <br />

            <table class="ttt">

                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="liid" runat="server" Text='<%# Eval("cid") %>' Visible="false"></asp:Literal>
                            <asp:Literal ID="liimages" runat="server" Text='<%# Eval("tuihuotupian_1688") %>' Visible="false"></asp:Literal>
                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />
                                CID:(<%# Eval("cid")%>)
                                
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
                                        <td style="width: 20%" class="bbb">订单编号</td>
                                        <td style="width: 50%"><%# Eval("dingdanbianhao") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">Offer ID</td>
                                        <td style="width: 50%"><%# Eval("Offer_ID") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">SKU ID</td>
                                        <td style="width: 50%"><%# Eval("SKU_ID") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">货品标题</td>
                                        <td style="width: 50%">

                                            <%# Eval("huopinbiaoti") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">1688采购链接</td>

                                        <td style="width: 50%"><a href="https://detail.1688.com/offer/<%# Eval("Offer_ID") %>.html" target="_blank">打开网址</a>&nbsp;&nbsp;
                                           <input type="text" value='https://detail.1688.com/offer/<%# Eval("Offer_ID") %>.html' style="width: 20px" id="url<%# Container.ItemIndex+1 %>" />
                                            <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="bbb">产品图片上传</td>
                                        <td>


                                            <asp:FileUpload ID="FileUpload2" runat="server" AllowMultiple="true" />
                                            <asp:LinkButton ID="LinkButton4" OnClientClick="JavaScript:return confirm('确认上传？');" runat="server" Text="确认上传" ForeColor="White" BackColor="Blue" CssClass="butt" CommandName="upmainimg" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">数量</td>
                                        <td><%# Eval("shuliang") %></td>

                                    </tr>
                                    <tr>
                                        <td class="bbb"><span style="color: red; font-weight: bold">*商家编码</span></td>
                                        <td>
                                            <asp:TextBox ID="txtshangjiabianma" Text='<%# Eval("shangjiabianma") %>' Width="150px" runat="server"></asp:TextBox>

                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="bbb">实际收货数量</td>
                                        <td>
                                            <asp:TextBox ID="txtshijishouhuoshuliang" Text='<%# Eval("shijishouhuoshuliang") %>' Width="50px" runat="server"></asp:TextBox>

                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="bbb">需退货数量</td>
                                        <td>
                                            <asp:TextBox ID="txtxutuihuoshuliang" Text='<%# Eval("xutuihuoshuliang") %>' Width="50px" runat="server"></asp:TextBox>

                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="bbb">订单备注</td>
                                        <td>
                                            <asp:TextBox ID="txtdingdanbeizhu" Text='<%# Eval("dingdanbeizhu") %>' Width="60%" runat="server"></asp:TextBox>

                                        </td>

                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton5" OnClientClick="JavaScript:return confirm('确定更新以上信息？');" runat="server" Text="更新以上信息" ForeColor="White" BackColor="Red" CssClass="" CommandName="upxx" />

                                        </td>
                                    </tr>


                                    <tr>
                                        <td class="bbb">单位</td>
                                        <td><%# Eval("danwei") %></td>

                                    </tr>
                                    <tr>
                                        <td class="bbb">单价(元)</td>
                                        <td><%# Eval("danjia") %></td>

                                    </tr>
                                    <tr>
                                        <td class="bbb">货号</td>
                                        <td><%# Eval("huohao") %></td>

                                    </tr>
                                    <tr>
                                        <td class="bbb">型号</td>
                                        <td style="width: 50%">

                                            <%#Eval("xinghao")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">材质</td>
                                        <td>
                                            <asp:Literal ID="licid" runat="server" Visible="false" Text='<%# Eval("cid")%>'></asp:Literal>
                                            <%# Eval("caizhi") %>&nbsp;
                                            <asp:TextBox ID="txt_caizhi" Text='' Width="50px" runat="server"></asp:TextBox>
                                            <asp:LinkButton ID="LinkButton3" OnClientClick="JavaScript:return confirm('确定更新材质？');" runat="server" Text="更新材质" ForeColor="White" BackColor="Red" CssClass="" CommandName="upcz" CommandArgument='<%# Eval("cid").ToString()+"-"+(Container.ItemIndex+1).ToString() %>' />

                                            <span style="color: red; font-weight: bold">
                                                <asp:Literal ID="liishavecaiwu" runat="server" Text='<%# Eval("rucangSKUID").ToString()==""?"财务表未找到数据，请先匹配skuid":"" %>'></asp:Literal>
                                            </span>


                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>


                                    <tr>

                                        <td>
                                            <asp:LinkButton ID="btnzd" OnClientClick="JavaScript:return confirm('确定确认收货？');" runat="server" Text="确认收货" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="qr" CommandArgument='<%# Eval("cid") %>' />

                                        </td>
                                        <td>退款备注<asp:TextBox ID="txttkbz" runat="server"></asp:TextBox>&nbsp;<asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确定需退款？');" runat="server" Text="需退款" ForeColor="White" BackColor="Red" CssClass="butt" CommandName="tk" CommandArgument='<%# Eval("cid") %>' />
                                    </tr>
                                    <tr>
                                        <td colspan="2">

                                            <asp:DataList ID="DataList1" DataKeyField="imgname" runat="server"
                                                OnItemCommand="DataList1_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:Literal Text='<%# Eval("cid").ToString() %>' runat="server" ID="licid" Visible="false"></asp:Literal>
                                                    <table bgcolor="black" cellpadding="4" cellspacing="1">
                                                        <tr>
                                                            <td bgcolor="white" width="60px">
                                                                <a href='<%# "/Uploads/"+Eval("imgname").ToString()%>' target="_blank">
                                                                    <img id="img" src='<%# "/Uploads/"+Eval("imgname").ToString()%>' height="60" width="60"
                                                                        border="0" /></a>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td bgcolor="white" align="center">
                                                                <asp:Button ID="del" Text="del" Width="50px" runat="server" CommandName="del" CommandArgument='<%# Eval("imgname") %>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>

                                            <asp:Literal ID="liimgxs" runat="server"></asp:Literal>
                                            <br />
                                            <span style="color: red; font-weight: bold">*请上传快递包裹称重图</span>
                                            <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" />
                                            <asp:LinkButton ID="LinkButton2" OnClientClick="JavaScript:return confirm('确认上传？');" runat="server" Text="确认上传" ForeColor="White" BackColor="Blue" CssClass="butt" CommandName="up" CommandArgument='<%# Eval("cid") %>' />
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
