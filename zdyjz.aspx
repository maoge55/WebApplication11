<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="zdyjz.aspx.cs" Inherits="WebApplication11.zdyjz" %>

<%@ Register Src="~/left.ascx" TagName="left" TagPrefix="ul" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>列名指定</title>
    <style>
        .leftdiv {
            width: 180px;
            position: fixed;
            background: #cbdbcd;
        }

            .leftdiv a {
                color: black;
                text-decoration: none;
            }

        tr:hover {
            background: #b6ff00;
        }
    </style>
</head>
<body>
    <ul:left ID="left" runat="server" />
    <form id="form1" runat="server">
        <div class="main">
            <div class="hdt">
                <h2>预加载<span><%=name %></span>数据指定</h2>
            </div>
            <div class="ts">
                <p>
                    <asp:Literal runat="server" ID="lits"></asp:Literal>
                </p>
            </div>
            <div>
                <asp:Literal ID="liyid" runat="server" Visible="false"></asp:Literal>
                <table class="ttt">
                    <tr class="head">
                        <td>列名称</td>
                        <td>电商平台数据</td>
                        <td>其它来源数据-变量</td>
                        <td>其它来源数据-固定值</td>
                    </tr>
                    <asp:Repeater ID="rplb" runat="server" OnItemDataBound="rplb_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtjid" Visible="false" runat="server" Text='<%#Eval("jid") %>'></asp:TextBox>
                                    <%#Eval("jname") %>
                                </td>
                                <td>
                                    <asp:DropDownList ID="dpxl" Width="100%" runat="server"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="dpother" Width="100%" runat="server"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtzdy" Width="233px" CssClass="txt"></asp:TextBox>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td colspan="4" style="text-align: right">

                            <asp:Button ID="btnadddata" Width="200px" Font-Size="18px" BackColor="Red" ForeColor="White" runat="server" Text="保存" CssClass="butt" OnClick="btnadddata_Click" />


                        </td>
                    </tr>
                </table>
            </div>
        </div>

    </form>

</body>
</html>
