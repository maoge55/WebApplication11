<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jyc.aspx.cs" Inherits="WebApplication11.jyc" %>

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
                        <asp:Literal ID="Literal1" runat="server" Text="禁用词"></asp:Literal></h2>
                </div>
                <div class="ts">
                    <p>
                        <asp:Literal runat="server" ID="lits"></asp:Literal>
                    </p>
                </div>
                <div>
                    <table class="ttt" id="add">

                        <tr>
                            <td colspan="3">
                                <asp:CheckBox ID="ckcf" runat="server" Text="去重复？" />
                                <asp:TextBox runat="server" ID="txtvalue" Width="99%" CssClass="txt" TextMode="MultiLine" Height="270px"></asp:TextBox>
                                <br />
                                <br />
                             
                                <asp:Button ID="btnadddata"  runat="server" Text="保存" CssClass="butt" OnClick="btnadddata_Click" />
                            </td>
                        </tr>

                    </table>
                </div>
            </div>
        </div>

    </form>
</body>
</html>
