<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addyjz.aspx.cs" Inherits="WebApplication11.addyjz" %>

<%@ Register Src="~/left.ascx" TagName="left" TagPrefix="ul" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>添加预加载数据</title>
    <style type="text/css">
        .auto-style3 {
            width: 429px;
        }

        .auto-style4 {
            width: 172px;
        }
    </style>
</head>
<body>
    <ul:left ID="left" runat="server" />
    <form id="form1" runat="server">
        <div class="main" style="width: 980px">

            <div id="zc" runat="server" visible="true">
                <div class="hdt">
                    <h2>
                        <asp:Literal ID="Literal1" runat="server" Text="添加预加载数据"></asp:Literal></h2>
                </div>
                <div class="ts">
                    <p>
                        <asp:Literal runat="server" ID="lits"></asp:Literal>
                    </p>
                </div>
                <div>
                    <table class="ttt" id="add">
                        <tr runat="server" id="dddrrr">
                            <td class="auto-style3">预加载数据名字：<asp:TextBox runat="server" ID="txtyname" Width="173px" CssClass="txt"></asp:TextBox>
                            </td>
                            <td>

                                <asp:Button ID="Button1" runat="server" Text="添加" CssClass="butt" OnClick="Button1_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:CheckBox ID="ckcf" runat="server" Text="去重复？" Checked="true" Visible="false" />
                                <asp:TextBox runat="server" ID="txtvalue" Width="99%" CssClass="txt" TextMode="MultiLine" Height="270px"></asp:TextBox>
                                <br />
                                <br />
                                <asp:Literal ID="lioid" Visible="false" runat="server"></asp:Literal>
                                <asp:Button ID="btnadddata" Visible="false" runat="server" Text="保存" CssClass="butt" OnClick="btnadddata_Click" />
                            </td>
                        </tr>

                    </table>
                </div>
            </div>
        </div>

    </form>
</body>
</html>
