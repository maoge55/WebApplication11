<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="入仓产品图片视频.aspx.cs" Inherits="WebApplication11.cg.入仓产品图片视频" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>入仓产品图片视频</title>
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
                <h3>当前页面【<span style="color: crimson">入仓产品图片视频</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                商家编码：<asp:TextBox ID="txtsjbm" runat="server"></asp:TextBox>
                &nbsp;
                入仓ITEMID：<asp:TextBox ID="txtrucangITEMID" runat="server"></asp:TextBox>
                &nbsp;
                入仓印尼标题：<asp:TextBox ID="txtrucangyinnibiaoti" runat="server"></asp:TextBox>&nbsp;
                <asp:Button ID="Button3" runat="server" Text="查找" OnClick="Button3_Click3" />
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />

            <table class="ttt">

                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="lirucangitemid" runat="server" Text='<%# Eval("rucangitemid") %>' Visible="false"></asp:Literal>
                            <asp:Literal ID="liskuid" runat="server" Text='<%# Eval("SKU_ID") %>' Visible="false"></asp:Literal>
                            <asp:Literal ID="litupianwangzhi" runat="server" Text='<%# Eval("tupianwangzhi") %>' Visible="false"></asp:Literal>
                            <asp:Literal ID="lishipinwangzhi" runat="server" Text='<%# Eval("shipinwangzhi") %>' Visible="false"></asp:Literal>
                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %>

                            </td>
                            <td style="width: 30%; text-align: center">
                                <img src='/Uploads/<%#Eval("mainimg") %>' style='width: 300px; height: 300px' />
                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 20%" class="bbb">入仓ITEMID：</td>
                                        <td style="width: 50%"><%# Eval("rucangitemid") %></td>
                                    </tr>
                                     <tr>
                                        <td style="width: 20%" class="bbb">Offer_ID：</td>
                                        <td style="width: 50%"><%# Eval("Offer_ID") %></td>
                                    </tr>
                                     <tr>
                                        <td style="width: 20%" class="bbb">SKU_ID：</td>
                                        <td style="width: 50%"><%# Eval("SKU_ID") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">入仓印尼标题：</td>
                                        <td style="width: 50%"><%# Eval("rucangyinnibiaoti") %></td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">长cm：</td>
                                        <td><%# Eval("chang") %></td>

                                    </tr>
                                    <tr>
                                        <td class="bbb">宽cm：</td>
                                        <td><%# Eval("kuan") %></td>

                                    </tr>
                                    <tr>
                                        <td class="bbb">高cm：</td>
                                        <td><%# Eval("gao") %></td>

                                    </tr>
                                    <tr>
                                        <td class="bbb">重量kg：</td>
                                        <td><%# Eval("zhongliang") %></td>

                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>





                                    <tr>
                                        <td class="bbb">图片网址：</td>
                                        <td>

                                            <asp:DataList ID="DataList1" DataKeyField="imgname" runat="server"
                                                OnItemCommand="DataList1_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:Literal Text='<%# Eval("SKU_ID").ToString() %>' runat="server" ID="liimgSKU_ID" Visible="false"></asp:Literal>
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
                                            <asp:LinkButton ID="LinkButton2" OnClientClick="JavaScript:return confirm('确认上传？');" runat="server" Text="确认上传" ForeColor="White" BackColor="Blue" CssClass="butt" CommandName="upimg" CommandArgument='<%# Eval("SKU_ID") %>' />
                                        </td>
                                    </tr>



                                    <tr>
                                        <td class="bbb">视频网址：</td>
                                          <td>

                                            <asp:DataList ID="DataList2" DataKeyField="imgname" runat="server"
                                                OnItemCommand="DataList2_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:Literal Text='<%# Eval("SKU_ID").ToString() %>' runat="server" ID="liimgSKU_ID" Visible="false"></asp:Literal>
                                                    <table bgcolor="black" cellpadding="4" cellspacing="1">
                                                        <tr>
                                                            <td bgcolor="white" width="60px">
                                                                <a href='<%# "/Uploads/"+Eval("imgname").ToString()%>' target="_blank">
                                                                    <%# "/Uploads/"+Eval("imgname").ToString()%></a>
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

                                            <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                                            <br />
                                            <asp:FileUpload ID="FileUpload2" runat="server" AllowMultiple="true" />
                                            <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确认上传？');" runat="server" Text="确认上传" ForeColor="White" BackColor="Blue" CssClass="butt" CommandName="upsp" CommandArgument='<%# Eval("SKU_ID") %>' />
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
