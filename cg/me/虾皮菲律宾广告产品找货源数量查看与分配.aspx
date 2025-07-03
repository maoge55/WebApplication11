<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="虾皮菲律宾广告产品找货源数量查看与分配.aspx.cs" Inherits="WebApplication11.cg.虾皮菲律宾广告产品找货源数量查看与分配" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>虾皮菲律宾广告产品找货源数量查看与分配</title>
    <style>
        .ttt {
        }

            .ttt tr {
            }

                .ttt tr td {
                    border: 1px solid #000000;
                    padding: 5px;
                }

        .ttta {
        }

            .ttta tr td {
                border: 1px solid #cf0000;
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

        .auto-style2 {
            width: 189px;
            background: #cf0000;
            color: white;
        }

        .auto-style3 {
            width: 165px;
            background: #cf0000;
            color: white;
        }

        .ccc1 {
            color: blue;
            font-weight: bold;
        }

        .ccc2 {
            color: blue;
            font-weight: bold;
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
                <h3>当前页面【<span style="color: #cf0000">虾皮菲律宾广告产品找货源数量查看与分配</span>】（月销量>30 且 未查找货源 且 500<菲律宾价格 在 500 - 1200）</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                <asp:Button ID="Button1" runat="server" Text="统计数据" BackColor="Green" ForeColor="White" OnClick="Button1_Click" />
                <br />
                <asp:Button ID="Button3" runat="server" Text="重置" Visible="false" BackColor="Green" ForeColor="White" OnClick="Button3_Click" Style="height: 21px" />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />
            <div>
                <table class="ttt">

                    <tr>

                        <td>
                            <table class="ttta">
                                <tr>
                                    <td class="auto-style2">账号</td>
                                    <td class="auto-style3">数量</td>

                                </tr>
                                <%=html %>
                            </table>
                        </td>

                    </tr>
                </table>
            </div>
            <br />
            从：<asp:DropDownList ID="drf" runat="server">
                <asp:ListItem Value="0">无人</asp:ListItem>
                <asp:ListItem Value="6">郑雨蝶</asp:ListItem>
             
                <asp:ListItem Value="19">王志敏</asp:ListItem>
             
            </asp:DropDownList>
            移动<asp:TextBox ID="txtcount" Width="50px" CssClass="ccc2" runat="server"></asp:TextBox>条数据到
            <asp:DropDownList ID="drin" runat="server">

                  <asp:ListItem Value="6">郑雨蝶</asp:ListItem>
             
                <asp:ListItem Value="19">王志敏</asp:ListItem>
                <asp:ListItem Value="0">无人</asp:ListItem>
            </asp:DropDownList>&nbsp;
            <asp:Button ID="Button2" runat="server" OnClientClick="JavaScript:return confirm('确定移动？');" Text="移动" BackColor="Green" ForeColor="White" OnClick="Button2_Click" />

        </div>
    </form>
</body>
</html>
