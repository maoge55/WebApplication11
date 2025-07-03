<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="海运运费登记.aspx.cs" Inherits="WebApplication11.cg.海运运费登记" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>海运运费登记</title>
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
                <h3>当前页面【<span style="color: crimson">海运运费登记</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                商家编码：<asp:TextBox ID="txtsjbm" runat="server"></asp:TextBox>&nbsp;
                物流单号：<asp:TextBox ID="txtwldh" runat="server"></asp:TextBox>
                &nbsp;
                <asp:Button ID="Button1" runat="server" Text="查找数据" BackColor="Green" ForeColor="White" OnClick="Button1_Click" style="height: 21px" />
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />

            <table class="ttt">

                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">
                    <ItemTemplate>
                         <asp:Literal ID="liguangdongwuliufeiyong_img" runat="server" Text='<%# Eval("guangdongwuliufeiyong_img") %>' Visible="false"></asp:Literal>
                            <asp:Literal ID="liguangdongfachudanhao" runat="server" Text='<%# Eval("guangdongfachudanhao") %>' Visible="false"></asp:Literal>
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
                                        <td style="width: 30%" class="bbb">莆田发出单号</td>
                                        <td><%# (Eval("putianfachudanhao").ToString()) %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">莆田发出备注</td>
                                        <td><%# (Eval("putianfachubeizhu").ToString()) %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">莆田发出日期</td>
                                        <td><%# (Eval("putianfachuriqi").ToString()) %></td>
                                    </tr>

                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>



                                    <tr>
                                        <td style="width: 30%" class="bbb">广东发出日期</td>
                                        <td>
                                            <%# (Eval("guangdongfachuriqi").ToString()) %>
                                          
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">广东发出单号</td>
                                        <td>
                                            <%# (Eval("guangdongfachudanhao").ToString()) %>
                                       
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">广东物流类型</td>
                                        <td>
                                            <%# (Eval("guangdongwuliuleixing").ToString()) %>
                                          
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">广东物流费用<span style="color: red">(*)</span></td>
                                        <td>
                                            <asp:TextBox ID="txtguangdongwuliufeiyong" Text='<%# Eval("guangdongwuliufeiyong") %>' Width="80%" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">图片网址：</td>
                                        <td>

                                            <asp:DataList ID="DataList1" DataKeyField="imgname" runat="server"
                                                OnItemCommand="DataList1_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:Literal Text='<%# Eval("guangdongfachudanhao").ToString() %>' runat="server" ID="liguangdongfachudanhao" Visible="false"></asp:Literal>
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

                                         
                                            <br />
                                            <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" />
                                            <asp:LinkButton ID="LinkButton2" OnClientClick="JavaScript:return confirm('确认上传？');" runat="server" Text="确认上传" ForeColor="White" BackColor="Blue" CssClass="butt" CommandName="upimg" CommandArgument='<%# Eval("guangdongfachudanhao") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">物流商名称</td>
                                        <td>
                                            <asp:TextBox ID="txtwuliushangmingcheng" Text='<%# Eval("wuliushangmingcheng") %>' Width="80%" runat="server"></asp:TextBox>
                                        </td>

                                    </tr>



                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>


                                    <tr>
                                        <td></td>

                                        <td>
                                            <asp:LinkButton ID="btnzd" OnClientClick="JavaScript:return confirm('确定保存？');" runat="server" Text="保存" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="qr" CommandArgument='<%# Eval("guangdongfachudanhao") %>' />

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
