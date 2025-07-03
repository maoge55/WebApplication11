<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fc.aspx.cs" Inherits="WebApplication11.fc" %>

<%@ Register Src="~/left.ascx" TagName="left" TagPrefix="ul" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>反查产品</title>
</head>
<body>
    <ul:left ID="left" runat="server" />
    <form id="form1" runat="server">
        <div class="main" style="width: 980px">
            <div id="zc" runat="server" visible="true">
                <div class="hdt">
                    <h2>
            
                        <asp:Literal ID="Literal1" runat="server" Text="反查产品"></asp:Literal></h2>
                </div>
                <div class="ts">
                    <p>
                        <asp:Literal runat="server" ID="lits"></asp:Literal>
                        <asp:Literal ID="lipath" runat="server" Visible="false"></asp:Literal>
                    </p>
                </div>
                标题：<asp:TextBox ID="txtname" runat="server"></asp:TextBox>
                SKUID：<asp:TextBox ID="txtsku" runat="server"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="查询" OnClick="Button1_Click" />
                <br />
                <div runat="server" id="jg" visible="false">
                    <table class="ttt" id="add">
                        <tr>
                            <td>PID</td>
                            <td><%=pid %></td>
                        </tr>
                        <tr>
                            <td>URL</td>
                            <td><%=purl %></td>
                        </tr>
                        <tr>
                            <td>属性</td>
                            <td><%=skuAttr %></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
