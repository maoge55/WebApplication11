<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="精准匹配广告词.aspx.cs" Inherits="WebApplication11.cg.精准匹配广告词" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>精准匹配广告词</title>
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

        .aaa4 td {
            background: #fb686842;
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
                <h3>当前页面【<span style="color: #37cbc5">精准匹配广告词</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>

                <asp:Button ID="Button1" runat="server" Text="1.加载今日csv数据" OnClick="Button1_Click" />&nbsp;&nbsp;
                 <asp:Button ID="Button2" runat="server" Text="2.保存到数据库" OnClientClick="JavaScript:return confirm('确定保存？');" OnClick="Button2_Click2" />&nbsp;&nbsp;
                &nbsp;&nbsp;
               搜索类型：<asp:DropDownList ID="drtype" runat="server">
                   <asp:ListItem Value="0">在用精准关键词</asp:ListItem>
                   <asp:ListItem Value="1">停用精准关键词</asp:ListItem>
               </asp:DropDownList>&nbsp;
                排序：
                <asp:DropDownList ID="drporder" runat="server">
                    <asp:ListItem Value="0">点击率高-低</asp:ListItem>
                    <asp:ListItem Value="1">点击率低-高</asp:ListItem>
                </asp:DropDownList>&nbsp;
                 <asp:Button ID="Button3" runat="server" Text="搜索加载" BackColor="Green" ForeColor="White" OnClick="Button3_Click3" />&nbsp;&nbsp;
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />
            <br />
            广告标题关键词操作:
            <br />
            Product_ID:<asp:TextBox ID="txtProduct_ID" runat="server" Width="118px"></asp:TextBox>&nbsp;
            <asp:CheckBox ID="ck3" runat="server" Text="三词" />&nbsp;
            <asp:CheckBox ID="ck4" runat="server" Text="四词" />&nbsp;
            <asp:CheckBox ID="ck5" runat="server" Text="五词" />&nbsp;
            <asp:CheckBox ID="ck6" runat="server" Text="六词" />&nbsp;
            <asp:CheckBox ID="ck7" runat="server" Text="七词" />&nbsp;
            搜索量大于:<asp:TextBox ID="txtSearchCount" Text="50" runat="server" Width="30px"></asp:TextBox>&nbsp;
            <asp:CheckBox ID="ckall" runat="server" Text="所有Product_ID" />&nbsp;
              <asp:Button ID="Button4" runat="server" Text="搜索数据" Width="100px" BackColor="Blue" ForeColor="White" OnClick="Button4_Click1" />
            <br />
            <span style="color: red">
                <asp:Literal ID="Literal2" runat="server"></asp:Literal></span>
            <asp:Button ID="Button5" runat="server" OnClientClick="JavaScript:return confirm('确定保存？');" Text="保存" Width="100px" BackColor="Green" ForeColor="White" OnClick="Button5_Click" />

            

            <br />
            <table class="ttt">
                <tr class="aaa4">
                    <td>序号</td>
                    <td>浏览器id</td>
                    <td>店铺名称</td>
                    <td>分组</td>
                    <td>Product_ID</td>
                    <td>Search_Query</td>
                    <td>Clicks</td>
                    <td>IMPRESSION</td>
                    <td>CTR</td>
                    <td>User_Name</td>
                    <td>Shop_Name</td>
                    <td>Shop_ID</td>
                    <td>Product_Name_Ad_Name</td>
                </tr>
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %>
                            </td>
                            <td>
                                <%#Eval("BrowserID") %>
                            </td>
                            <td>
                                <%#Eval("DpName") %>
                            </td>
                            <td>
                                <%#Eval("GroupName") %>
                            </td>
                            <td>
                                <%#Eval("Product_ID") %>
                            </td>
                            <td style="width: 20%">
                                <%#Eval("Search_Query") %>
                            </td>
                            <td>
                                <%#Eval("Clicks") %>
                            </td>
                            <td>
                                <%#Eval("IMPRESSION") %>
                            </td>
                            <td>
                                <%# Eval("CTR") %>
                            </td>

                            <td>
                                <%#Eval("User_Name") %>
                            </td>
                            <td>
                                <%#Eval("Shop_Name") %>
                            </td>
                            <td>
                                <%#Eval("Shop_ID") %>
                            </td>
                            <td>
                                <%#Eval("Product_Name_Ad_Name") %>
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
