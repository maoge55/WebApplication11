<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pinpaiwhite.aspx.cs" Inherits="WebApplication11.pinpaiwhite" %>

<%@ Register Src="~/left.ascx" TagName="left" TagPrefix="ul" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>品牌白名单管理</title>
    <style>
        .ttttt {
            width: 100%;
        }

            .ttttt tr {
                border: 1px solid;
                margin: 5px;
                padding: 5px;
            }

            .ttttt td {
                border: 1px solid;
                margin: 5px;
                padding: 5px;
            }
    </style>
</head>
<body>
    <ul:left ID="left" runat="server" />
    <form id="form1" runat="server">
        <div class="main" style="width: 980px">
            <div id="zc" runat="server">
                <div class="hdt">
                    <h2>品牌白名单管理</h2>
                </div>
                <div class="ts">
                    <p>
                        <asp:Literal runat="server" ID="lits"></asp:Literal>
                    </p>
                </div>
                <div>
                  
                    <asp:Literal ID="Literal1" runat="server" Visible="false"></asp:Literal>
                    <table style="width:100%">
                        <tr>
                            <td>
                                <h3>新增</h3>
                                <br />
                                <asp:TextBox ID="TextBox1" TextMode="MultiLine" runat="server" Height="143px" Width="158px"></asp:TextBox>
                                <br />
                                <asp:Button ID="Button2" Style="background: green; color: white; border: 1px solid #b7b315;"  OnClientClick="JavaScript:return confirm('确定添加吗？');" runat="server" Text="添加" OnClick="Button2_Click" />
                            </td>
                            <td>
                                <h3>删除</h3>
                                <br />
                                <asp:TextBox ID="TextBox2" TextMode="MultiLine" runat="server" Height="143px" Width="158px"></asp:TextBox>
                                <br />
                                <asp:Button ID="Button3"  Style="background: red; color: white; border: 1px solid #b7b315;"  OnClientClick="JavaScript:return confirm('确定删除吗？');" runat="server" Text="删除" OnClick="Button3_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>

    </form>
</body>
</html>
