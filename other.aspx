<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="other.aspx.cs" Inherits="WebApplication11.other" %>

<%@ Register Src="~/left.ascx" TagName="left" TagPrefix="ul" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>所有数据源列表</title>
</head>
<body>

    <ul:left ID="left" runat="server" />
    <form id="form1" runat="server">
        <div class="main" style="width: 980px">
            <div id="lg" runat="server">
                <div class="hdt">
                    <h2>输入单独的管理员密码</h2>
                </div>
                <table class="ttt" id="add">
                    <tr>

                        <td>密码：<asp:TextBox TextMode="Password" runat="server" ID="txtpwd" Width="119px" CssClass="txt"></asp:TextBox>

                            &nbsp;<asp:Button ID="Button1" runat="server" Text="Login" CssClass="butt" OnClick="Button1_Click" /></td>
                    </tr>
                </table>
            </div>
            <div id="zc" runat="server" visible="false">
                <div class="hdt">
                    <h2>所有数据源列表<a href="addother.aspx" style="color: red">[去添加]</a></h2>
                </div>
                <div class="ts">
                    <p>
                        <asp:Literal runat="server" ID="lits"></asp:Literal>
                    </p>
                </div>
                <div>
                    <table class="ttt" id="lb">
                       
                        <tr class="head">
                            <td>ID</td>
                            <td>数据源名字</td>
                            <td>类型</td>
                            <td>数据量</td>
                            <td>操作</td>
                        </tr>
                        <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 5%"><%#Eval("oid") %></td>
                                    <td style="width: 20%">
                                        <asp:Literal ID="lioname" runat="server" Text='<%#Eval("oname") %>'></asp:Literal>
                                        <asp:TextBox ID="txtoname" CssClass="txt" Width="80%" runat="server" Visible="false" Text='<%#Eval("oname") %>'></asp:TextBox>
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Literal ID="liotype" runat="server" Text='<%#Eval("otype") %>'></asp:Literal>

                                        <asp:DropDownList ID="dptype" Visible="false" runat="server">
                                            <asp:ListItem Text="重复" Value="重复"></asp:ListItem>
                                            <asp:ListItem Text="一次性" Value="一次性"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 20%">
                                      
                                            <%#Eval("count") %>
                                       
                                    </td>
                                    <td style="width: 25%">
                                        <asp:LinkButton ID="btnup" runat="server" Text="修改" Width="60px" ForeColor="Green" CssClass="butt" CommandName="update" CommandArgument='<%# Eval("oid") %>' />
                                        <asp:LinkButton ID="btnsave" Visible="false" runat="server" Text="保存" Width="60px" ForeColor="#0600ff" CssClass="butt" CommandName="save" CommandArgument='<%# Eval("oid") %>' />

                                        &nbsp;
                                     <asp:LinkButton ID="btnadd" runat="server" Text="编辑数据" Width="100px" ForeColor="#b513f5" CssClass="butt" CommandName="add" CommandArgument='<%# Eval("oid") %>' />
                                        &nbsp;
                                    <asp:LinkButton ID="btndell" runat="server" Text="删除" OnClientClick="JavaScript:return confirm('确定删除吗？');" Width="60px" ForeColor="Red" CssClass="butt" CommandName="del" CommandArgument='<%# Eval("oid") %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
        </div>
    </form>

</body>
</html>
