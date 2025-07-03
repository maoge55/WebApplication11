<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YGL.aspx.cs" Inherits="WebApplication11.YGL" %>

<%@ Register Src="~/left.ascx" TagName="left" TagPrefix="ul" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>所有预加载列表</title>
</head>
<body>

    <ul:left ID="left" runat="server" />
    <form id="form1" runat="server">
        <div class="main" style="width: 980px">

            <div id="zc" runat="server" visible="true">
                <div class="hdt">
                    <h2>所有预加载列表<a href="addyjz.aspx" style="color: red">[去添加]</a></h2>
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
                            <td>名字</td>
                            <td>操作</td>
                        </tr>
                        <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 5%"><%#Eval("yid") %></td>
                                    <td style="width: 20%">
                                        <asp:Literal ID="liyname" runat="server" Text='<%#Eval("yname") %>'></asp:Literal>
                                        <asp:TextBox ID="txtyname" CssClass="txt" Width="80%" runat="server" Visible="false" Text='<%#Eval("yname") %>'></asp:TextBox>
                                    </td>


                                    <td style="width: 25%">
                                        <asp:LinkButton ID="btnup" runat="server" Text="修改" Width="60px" ForeColor="Green" CssClass="butt" CommandName="update" CommandArgument='<%# Eval("yid") %>' />
                                        <asp:LinkButton ID="btnsave" Visible="false" runat="server" Text="保存" Width="60px" ForeColor="#0600ff" CssClass="butt" CommandName="save" CommandArgument='<%# Eval("yid") %>' />
                                        &nbsp;
                                          <asp:LinkButton ID="btnuplm" runat="server" Text="编辑列名" Width="100px" ForeColor="Plum" CssClass="butt" CommandName="uplm" CommandArgument='<%# Eval("yid") %>' />
                                        &nbsp;<asp:LinkButton ID="btnadd" runat="server" Text="指定数据" Width="100px" ForeColor="#b513f5" CssClass="butt" CommandName="upsj" CommandArgument='<%# Eval("yid") %>' />
                                        &nbsp;
                                    <asp:LinkButton ID="btndell" runat="server" Text="删除" OnClientClick="JavaScript:return confirm('确定删除吗？');" Width="60px" ForeColor="Red" CssClass="butt" CommandName="del" CommandArgument='<%# Eval("yid") %>' />
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
