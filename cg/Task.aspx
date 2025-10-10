<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Task.aspx.cs" Inherits="WebApplication11.cg.Task" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>任务管理</title>
    <style>
        table { border-collapse: collapse; width: 100%; }
        th, td { border: 1px solid #ccc; padding: 6px; text-align: center; }
        th { background-color: #f3f3f3; }
        input[type=text] { width: 95%; }
        select { width: 100px; }
        .btn { margin: 2px; padding: 3px 8px; border: none; border-radius: 3px; cursor: pointer; }
        .start { background: #4CAF50; color: #fff; }
        .continue { background: #2196F3; color: #fff; }
        .close { background: #f44336; color: #fff; }
        .save { background: #ff9800; color: #fff; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h2>任务管理</h2>
        <asp:Repeater ID="repTask" runat="server" OnItemCommand="repTask_ItemCommand">
            <HeaderTemplate>
                <table>
                    <tr>
                        <th>ID</th>
                        <th>任务名称</th>
                        <th>ShopClass</th>
                        <th>tcount</th>
                        <th>state</th>
                        <th>pt</th>
                        <th>logPath</th>
                        <th>LastRunTime</th>
                        <th>LastEndTime</th>
                        <th>LastHouTaiID</th>
                        <th>iszd</th>
                        <th>FuncName</th>
                        <th>isMulti</th>
                        <th>extraParam</th>
                        <th>timing</th>
                        <th>gtName</th>
                        <th>position</th>
                        <th>mutilTiming</th>
                        <th>操作</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("id") %></td>
                    <td><asp:TextBox ID="txttName" runat="server" Text='<%# Eval("tName") %>' /></td>
                    <td><asp:TextBox ID="txtShopClass" runat="server" Text='<%# Eval("ShopClass") %>' /></td>
                    <td><asp:TextBox ID="txttcount" runat="server" Text='<%# Eval("tcount") %>' /></td>
                    <td>
                        <asp:Button CommandName="start" CssClass="btn start" Text="开始" runat="server" CommandArgument='<%# Eval("id") %>' />
                        <asp:Button CommandName="continue" CssClass="btn continue" Text="继续" runat="server" CommandArgument='<%# Eval("id") %>' />
                        <asp:Button CommandName="close" CssClass="btn close" Text="关闭" runat="server" CommandArgument='<%# Eval("id") %>' />
                    </td>
                    <td><asp:TextBox ID="txtpt" runat="server" Text='<%# Eval("pt") %>' /></td>
                    <td><asp:TextBox ID="txtlogPath" runat="server" Text='<%# Eval("logPath") %>' /></td>
                    <td><asp:TextBox ID="txtLastRunTime" runat="server" Text='<%# Eval("LastRunTime") %>' /></td>
                    <td><asp:TextBox ID="txtLastEndTime" runat="server" Text='<%# Eval("LastEndTime") %>' /></td>
                    <td><asp:TextBox ID="txtLastHouTaiID" runat="server" Text='<%# Eval("LastHouTaiID") %>' /></td>
                    <td><asp:TextBox ID="txtiszd" runat="server" Text='<%# Eval("iszd") %>' /></td>
                    <td><asp:TextBox ID="txtFuncName" runat="server" Text='<%# Eval("FuncName") %>' /></td>
                    <td><asp:TextBox ID="txtisMulti" runat="server" Text='<%# Eval("isMulti") %>' /></td>
                    <td><asp:TextBox ID="txtextraParam" runat="server" Text='<%# Eval("extraParam") %>' /></td>
                    <td><asp:TextBox ID="txttiming" runat="server" Text='<%# Eval("timing") %>' /></td>
                    <td><asp:TextBox ID="txtgtName" runat="server" Text='<%# Eval("gtName") %>' /></td>
                    <td>
                        <asp:DropDownList ID="ddlPosition" runat="server" SelectedValue='<%# Eval("position") %>'>
                            <asp:ListItem Text="显示" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="隐藏" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td><asp:TextBox ID="txtmutilTiming" runat="server" Text='<%# Eval("mutilTiming") %>' /></td>
                    <td>
                        <asp:Button CommandName="save" CssClass="btn save" Text="保存" runat="server" CommandArgument='<%# Eval("id") %>' />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
