<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="虾皮违规产品.aspx.cs" Inherits="WebApplication11.cg.虾皮违规产品" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>虾皮违规产品</title>
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
                <h3>当前页面【<span style="color: #37cbc5">虾皮违规产品</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <asp:Button ID="btnsearch" runat="server" Text="查找数据" Width="150px" OnClientClick="" BackColor="Blue" ForeColor="White" OnClick="btnsearch_Click" />
            <br />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>


            <br />
            <table class="ttt">

                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="lileimu" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Literal>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />

                            </td>

                            <td>

                                <table class="ttta">

                                    <tr>
                                        <td class="bbb">Product_name</td>

                                        <td>
                                            <%# Eval("Product_name") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">Violation_type</td>

                                        <td>
                                            <%# Eval("Violation_type") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">Violation_reason</td>

                                        <td>
                                            <%# Eval("Violation_reason") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">Suggestion</td>

                                        <td>
                                            <%# Eval("Suggestion") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确定已处理？');" runat="server" Text=" 已处理" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="up" CommandArgument='<%# Eval("id") %>' />
                                        </td>
                                    </tr>
                                </table>
                        </tr>
                        <tr></tr>
                    </ItemTemplate>
                </asp:Repeater>

            </table>

        </div>
    </form>
</body>
</html>
