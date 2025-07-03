<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pinpai.aspx.cs" Inherits="WebApplication11.pinpai" %>

<%@ Register Src="~/left.ascx" TagName="left" TagPrefix="ul" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>所有品牌统计和管理</title>
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
        <div class="main" style="width: 1280px">
            <div id="zc" runat="server">
                <div class="hdt">
                    <h2>所有品牌统计和管理</h2>
                </div>
                <div class="ts">
                    <p>
                        <asp:Literal runat="server" ID="lits"></asp:Literal>
                    </p>
                </div>
                <div>
                    <table class="ttttt">
                        <tr>
                            <td>
                                <asp:Button ID="Button1" runat="server" Style="background: blue; color: white; border: 1px solid #b7b315;" Text="统计所有" OnClick="Button1_Click1" />
                                &nbsp;
                     <asp:Button ID="Button2" runat="server" Style="background: red; color: white; border: 1px solid #b7b315;"
                         Text="白名单管理" OnClick="Button2_Click" /></td>
                            <td>

                                <asp:Button ID="Button3" Style="background: green; color: white; border: 1px solid #b7b315;" runat="server" Text="修改产品状态" OnClientClick="JavaScript:return confirm('确定修改产品状态吗？');" OnClick="Button3_Click" />
                            </td>
                        </tr>
                    </table>



                    <br />
                    <asp:Literal ID="Literal1" runat="server" Visible="false"></asp:Literal>
                </div>
            </div>
        </div>

    </form>
</body>
</html>
