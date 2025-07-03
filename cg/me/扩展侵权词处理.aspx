<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="扩展侵权词处理.aspx.cs" Inherits="WebApplication11.cg.扩展侵权词处理" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>扩展侵权词处理</title>
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

        .rdcss td:nth-child(1) {
            background: White;
           
        }

        .rdcss td:nth-child(2) {
            background: black;
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
                <h3>当前页面【<span style="color: #37cbc5">扩展侵权词处理</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <asp:Button ID="btnsearch" runat="server" Text="加载100个" Width="150px" OnClientClick="" BackColor="Blue" ForeColor="White" OnClick="btnsearch_Click" />
            &nbsp;
            <asp:Button ID="Button2" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存审核结果吗？');" Text="保存审核结果" OnClick="Button2_Click1" />
            &nbsp;
            <asp:Button ID="Button4"  BackColor="White" runat="server" ForeColor="Black" Text="全选白" OnClick="Button4_Click"/>
            &nbsp;
            <asp:Button ID="Button5" BackColor="Black" runat="server" ForeColor="White" Text="全选黑" OnClick="Button5_Click" />
            <br />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>


            <br />
            <table class="ttt">
                <tr class="aaa4" style="text-align: center">
                    <td>序号</td>
                    <td>原侵权词</td>
                    <td>扩展侵权词</td>
                    <td>数量</td>
                    <td>处理</td>
                </tr>
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="linewcc" runat="server" Text='<%# Eval("newcc") %>' Visible="false"></asp:Literal>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />

                            </td>
                            <td style="width: 30%; text-align: center">
                                <%# Eval("oldcc") %>
                            </td>
                            <td style="width: 30%; text-align: center">
                                <%# Eval("newcc") %>
                            </td>
                            <td style="width: 30%; text-align: center">
                                <%# Eval("count") %>
                            </td>
                            <td style="width: 30%; text-align: center">
                                <asp:RadioButtonList ID="rdjg" CssClass="rdcss" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" class="gg"><span style="color:black">白</span></asp:ListItem>
                                    <asp:ListItem Value="-1" class="rr"><span style="color:white">黑</span></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>

                        </tr>
                        <tr>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td colspan="5" style="text-align: right">
                        <asp:Button ID="Button1" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存审核结果吗？');" Text="保存审核结果" OnClick="Button1_Click2" />
                    </td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>
