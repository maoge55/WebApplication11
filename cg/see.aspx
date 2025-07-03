<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="see.aspx.cs" Inherits="WebApplication11.cg.see" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>宝涵审核</title>
    <style>
        .ttt {
            width: 100%;
        }

            .ttt tr {
            }

                .ttt tr td {
                    border: 1px solid #c0c0d9;
                    padding: 5px;
                }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <h3>当前页面【<span style="color: #37cbc5">宝涵审核</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                <asp:Button ID="Button1" runat="server" BackColor="Green" ForeColor="White" Text="加载正常数据1条" OnClick="Button1_Click" />&nbsp;
                <asp:Button ID="Button4" runat="server" BackColor="Blue" ForeColor="White" Text="加载跳过数据1条" OnClick="Button4_Click" />&nbsp;
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                &nbsp;&nbsp;
            <asp:Button ID="Button2" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存审核结果吗？');" Text="保存审核结果" OnClick="Button2_Click" />
            </div>
            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="licode" runat="server" Text='<%# Eval("code") %>' Visible="false"></asp:Literal>
                            <td style="width: 4%;"><%# Container.ItemIndex+1 %></td>
                            <td style="width: 30%;">
                                <img src="<%# Eval("MainImage") %>" style="width: 300px" /></td>
                            <td style="width: 40%;"><%# Eval("Title") %>|<%# Eval("code") %><br /><%# Eval("Y_1688url").ToString()%></td>
                            <td>
                                <asp:RadioButtonList ID="rdjg" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="yes"><span style="color:blue">能发</span></asp:ListItem>
                                    <asp:ListItem Value="no"><span style="color:red">不能发</span></asp:ListItem>
                                    <asp:ListItem Value="tg"><span style="color:green">跳过</span></asp:ListItem>
                                </asp:RadioButtonList>
                                <div id="bh" runat="server">
                                    <br />
                                    宝涵海运价格：<asp:TextBox ID="txtbhjg" runat="server"></asp:TextBox>
                                    <br />
                                    宝涵海运备注：<asp:TextBox ID="txtbhbz" runat="server"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:Button ID="Button3" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存审核结果吗？');" Text="保存审核结果" OnClick="Button3_Click" /></td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>
