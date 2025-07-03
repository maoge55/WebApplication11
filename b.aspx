<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="b.aspx.cs" Inherits="WebApplication11.b" %>

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
                <h2>模板<span><%=dname %></span>列名指定</h2>
            </div>
            <div class="ts">
                <p>
                    <asp:Literal runat="server" ID="lits"></asp:Literal>
                </p>
            </div>
            <div>
                <table class="ttt" id="add">
                    <tr>
                        <td>模板名字：  <%=dname %>
                        </td>
                        <td>模板文件：  <%=dfile %>
                        </td>
                        <td>搜索词：<%=dsearchtxt %>
                            <asp:Literal ID="lidid" runat="server" Visible="false"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">预加载选择：<asp:DropDownList ID="dpyjz" runat="server" Width="200"></asp:DropDownList>
                            <asp:Button ID="Button1" Width="80"  BackColor="Red" ForeColor="White" runat="server" Text="加载" CssClass="butt" OnClick="Button1_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table class="ttt">
                    <tr class="head">
                        <td>批量表列名称</td>
                        <td>电商平台数据</td>
                        <td>其它来源数据-变量</td>
                        <td>其它来源数据-固定值</td>
                    </tr>
                    <asp:Repeater ID="rplb" runat="server" OnItemDataBound="rplb_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtlindex" Visible="false" runat="server" Text='<%#Container.ItemIndex %>'></asp:TextBox>

                                    <%#Eval("lname") %>
                                    <asp:Literal ID="libt" runat="server" Visible='<%# Eval("lbt").ToString()=="1"?true:false %>' Text="<span style='color:red'>(*)</span>"></asp:Literal>
                                    <asp:TextBox ID="txtlbname" runat="server" Visible="false" Text='<%#Eval("lname") %>'></asp:TextBox>
                                    <asp:TextBox ID="txtbt" runat="server" Visible="false" Text='<%#Eval("lbt") %>'></asp:TextBox>
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
