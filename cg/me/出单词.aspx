<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="出单词.aspx.cs" Inherits="WebApplication11.cg.出单词" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>出单词</title>
    <style>
        .ttt {
            width: 100%;
        }

            .ttt tr {
            }

                .ttt tr td {
                    border: 1px solid #000000;
                    padding: 5px;
                    text-align: center;
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



        .anniu1 {
            border: 1px red solid;
        }

        .aaa4 td {
            background: #fb686842;
            text-align: center;
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
                <h3>当前页面【<span style="color: #37cbc5">出单词</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>

                <asp:Button ID="Button1" runat="server" Text="1.加载今日csv数据" OnClick="Button1_Click" Style="height: 21px" />&nbsp;&nbsp;
                 <asp:Button ID="Button2" runat="server" Text="2.保存更新到数据库" OnClientClick="JavaScript:return confirm('确定保存更新到数据库？');" OnClick="Button2_Click2" style="height: 21px" />&nbsp;&nbsp;
                <br />
                <br />
                排序：
                <asp:DropDownList ID="dporder" runat="server">
                    <asp:ListItem Value="Product_ID" Selected="True">Product_ID</asp:ListItem>
                    <asp:ListItem Value="Conversions">单个关键词的出单量排序</asp:ListItem>
                    <asp:ListItem Value="onepid">单品销量</asp:ListItem>
                </asp:DropDownList>&nbsp;
                有无模板id：
                <asp:CheckBox runat="server" ID="ckymbid" Text="无" />&nbsp;
                <asp:Button ID="Button3" runat="server" Text="加载现有数据" OnClick="Button3_Click3" />&nbsp;&nbsp;
                <asp:Button ID="Button4" runat="server" Text="保存整页模板id" OnClientClick="JavaScript:return confirm('确定保存模板id？');" OnClick="Button4_Click2" />
                &nbsp;&nbsp;

              

                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />
            <table class="ttt">
                <tr class="aaa4">
                    <td>序号</td>

                    <td>店铺名称</td>
                    <td>分组</td>
                    <td>Product_ID</td>
                    <td>Search_Query</td>
                    <td>Conversions</td>
                    <td>ROAS</td>
                    <td>Product_Name_Ad_Name</td>
                    <td>类目网址</td>
                    <td>类目选品日期</td>
                    <td>搜索量</td>
                    <td>模板id</td>
                    <td style="width: 40px;">更新</td>
                </tr>
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %>
                            </td>

                            <td>
                                <%#Eval("DpName") %>
                            </td>
                            <td>
                                <%#Eval("GroupName") %>
                            </td>
                            <td>
                                <%#Eval("Product_ID") %>
                                <br />
                                <span style="color: red"><%#Eval("sumccc") %></span>
                            </td>
                            <td style="width: 15%">
                                <%#Eval("Search_Query") %>
                            </td>
                            <td>
                                <%#Eval("Conversions") %>
                            </td>

                             <td>
                                <%#Eval("ROAS") %>
                            </td>

                            <td>
                                <%#Eval("Product_Name_Ad_Name") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtleimuwangzhi" runat="server" Width="50px" Text='<%#Eval("leimuwangzhi") %>'></asp:TextBox>

                            </td>
                            <td>
                                <asp:TextBox ID="txtleimuxuanpinriqi" runat="server" Width="50px" Text='<%#Eval("leimuxuanpinriqi") %>'></asp:TextBox>

                            </td>
                            <td>
                                <%#Eval("search_count") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtmbid" runat="server" Width="50px" Text='<%#Eval("mid") %>'></asp:TextBox>
                            </td>
                            <td>

                                <asp:Literal ID="lipid" runat="server" Visible="false" Text='<%#Eval("Product_ID") %>'></asp:Literal>
                                <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确定更新？');" runat="server" Text="更新" ForeColor="White" BackColor="Red" CssClass="butt" CommandName="up" CommandArgument='<%# Eval("Product_ID") %>' />
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
