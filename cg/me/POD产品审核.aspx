<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="POD产品审核.aspx.cs" Inherits="WebApplication11.cg.POD产品审核" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>POD产品审核</title>
    <style>
        .ttt {
            width: 100%;
            text-align: center
        }

            .ttt tr {
            }

                .ttt tr td {
                    border: 1px solid #ede8e8;
                    padding: 10px;
                }

        .ttta {
            width: 100%;
        }

            .ttta tr td {
                border: 1px solid #1e63cb;
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
            width: 200px !important;
            height: 200px !important;
        }

        .ttt table {
            width: 100%
        }

        .rdcss td:nth-child(1) {
            background: green;
        }

        .rdcss td:nth-child(2) {
            background: red;
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
                <h3>当前页面【<span style="color: #1e63cb">POD产品审核</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                <asp:Button ID="Button1" runat="server" Text="加载18个产品" OnClick="Button1_Click" />&nbsp;
                <asp:Button ID="Button2" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存审核结果吗？');" Text="保存审核结果" OnClick="Button2_Click2" />
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                &nbsp;&nbsp;
            
            </div>
            <br />

            <table class="ttt">

                <asp:Repeater ID="rplb" runat="server">
                    <ItemTemplate>
                        <%# Container.ItemIndex%6==0?"<tr>":"" %>
                        <td>
                            <asp:Literal ID="liitemid" runat="server" Text='<%# Eval("itemid") %>' Visible="false"></asp:Literal>
                            <img src='<%# Eval("image") %>'>
                            <br />
                          <%# Eval("shopid") %>-<%# Eval("itemid") %>
                            <asp:RadioButtonList ID="rdjg" CssClass="rdcss" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="yes" class="gg"><span style="color:white">是</span></asp:ListItem>
                                <asp:ListItem Value="no" class="rr"><span style="color:white">否</span></asp:ListItem>
                            </asp:RadioButtonList>

                        </td>

                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <br />
            <asp:Button ID="Button4" Width="100px" Height="40px" Font-Size="15px" BackColor="green" runat="server" ForeColor="wheat" Text="全选是" OnClick="Button4_Click" />
            &nbsp;
            <asp:Button ID="Button5" Width="100px" Height="40px" Font-Size="15px" BackColor="red" runat="server" ForeColor="wheat" Text="全选否" OnClick="Button5_Click" />
            <br />
            <br />
            <br />
            <asp:Button ID="Button3" Width="200px" Height="80px" Font-Size="25px" BackColor="blue" runat="server" ForeColor="wheat" OnClientClick="JavaScript:return confirm('确定保存审核结果吗？');" Text="保存审核结果" OnClick="Button3_Click2" />


        </div>
    </form>
</body>
</html>
