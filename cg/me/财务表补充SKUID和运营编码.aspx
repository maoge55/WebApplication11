<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="财务表补充SKUID和运营编码.aspx.cs" Inherits="WebApplication11.cg.财务表补充SKUID和运营编码" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>财务表补充SKUID和运营编码</title>
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
                border: 1px solid #ff4000;
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

        img {
            float: left;
            width: 300px !important;
            height: 300px !important;
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
                <h3>当前页面【<span style="color: #ff4000">财务表补充SKUID和运营编码</span>】</h3>
                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                <asp:Literal ID="litype" Visible="false" runat="server"></asp:Literal>
                <asp:Button ID="Button1" runat="server" Text="加载1条未处理的数据" ForeColor="White" BackColor="Green" OnClick="Button1_Click" />&nbsp;
                 <asp:Button ID="Button3" runat="server" OnClientClick="JavaScript:return confirm('确定跳过？');" Text="跳过" BackColor="Red" ForeColor="White" OnClick="Button3_Click1" />&nbsp;
                 <asp:Button ID="Button2" runat="server" Text="加载1条跳过的数据" BackColor="Blue" ForeColor="White" OnClick="Button2_Click1" />&nbsp;
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />
            <table class="ttt">
                <tr>
                    <td colspan="5" style="background-color: #ebfbcb">财务表数据(id号为:
                        <asp:Literal ID="licid" runat="server"></asp:Literal>)</td>
                </tr>
                <tr>
                    <td style="background-color: #fdd6d6;">Offer ID</td>
                    <td style="background-color: #fdd6d6;">SKU ID</td>
                    <td style="background-color: #fdd6d6;">货品标题</td>
                    <td style="background-color: #fdd6d6;">单价(元)</td>
                    <td style="background-color: #fdd6d6;">1688采购链接</td>
                </tr>
                <tr>

                    <td><%=Offer_ID %></td>
                    <td><%=SKU_ID %></td>
                    <td><%=huopinbiaoti %></td>
                    <td><%=danjia %></td>
                    <td><a href='https://detail.1688.com/offer/<%=Offer_ID%>.html' target="_blank">1688采购链接</a></td>

                </tr>
            </table>
            <br />
            <table class="ttt">
                <tr>
                    <td colspan="8" style="background-color: #ebfbcb">出单表数据</td>
                </tr>
                <tr>
                    <td style="background-color: #8bbde9; width: 10%">SkuID</td>
                    <td style="background-color: #8bbde9; width: 10%">运营编码</td>
                    <td style="background-color: #8bbde9; width: 10%">1688采购链接</td>
                    <td style="background-color: #8bbde9; width: 15%">1688sku1</td>
                    <td style="background-color: #8bbde9; width: 15%">1688sku2</td>
                    <td style="background-color: #8bbde9; width: 15%">1688sku3</td>
                    <td style="background-color: #8bbde9; width: 10%">1688采购价</td>
                    <td style="background-color: #8bbde9; width: 15%">操作</td>
                </tr>

                <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal ID="liskuid" runat="server" Text='<%# Eval("SkuID") %>' Visible="false"></asp:Literal><%# Eval("SkuID") %></td>
                            <td>
                                <asp:Literal ID="liyybm" runat="server" Text='<%# Eval("yunyingbianma") %>' Visible="false"></asp:Literal><%# Eval("yunyingbianma") %></td>
                            <td><a href='<%# Eval("Y_1688url") %>' target="_blank">1688采购链接</a></td>
                            <td><%# Eval("Y_1688sku1") %></td>
                            <td><%# Eval("Y_1688sku2") %></td>
                            <td><%# Eval("Y_1688sku3") %></td>
                            <td><%# Eval("Y_1688price") %></td>
                            <td>
                                <asp:LinkButton ID="LinkButton2" OnClientClick="JavaScript:return confirm('确定匹配？');" runat="server" Text="匹配" ForeColor="White" BackColor="Red" CssClass="butt" CommandName="pp" CommandArgument='<%# Eval("id") %>' /></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>

            </table>
        </div>
    </form>
</body>
</html>
