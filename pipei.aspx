<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pipei.aspx.cs" Inherits="WebApplication11.pipei" %>

<%@ Register Src="~/left.ascx" TagName="left" TagPrefix="ul" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>产品匹配模板id</title>
</head>
<body>
    <ul:left ID="left" runat="server" />
    <form id="form1" runat="server">
        <div class="main" style="width: 980px">
            <div id="zc" runat="server">
                <div class="hdt">
                    <h2>产品匹配模板id</h2>
                </div>
                <div class="ts">
                    <p>
                        <asp:Literal runat="server" ID="lits"></asp:Literal>
                    </p>
                </div>
                <div>
                    <asp:Button ID="Button1" runat="server" Text="查询" OnClick="Button1_Click" />
                    &nbsp;
                    <asp:Button ID="Button2" runat="server" Text="开始匹配" OnClick="Button2_Click" />
                </div>
            </div>
        </div>

    </form>
</body>
</html>
