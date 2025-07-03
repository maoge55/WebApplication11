<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DT.aspx.cs" Inherits="WebApplication11.DT" %>

<%@ Register Src="~/left.ascx" TagName="left" TagPrefix="ul" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>导出结果统计</title>
    <style>
        .ttttt {
            width:100%;
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
                    <h2>导出结果统计</h2>
                </div>
                <div class="ts">
                    <p>
                        <asp:Literal runat="server" ID="lits"></asp:Literal>
                    </p>
                </div>
                <div>
                    <asp:Button ID="Button1" runat="server" Text="开始统计" OnClick="Button1_Click1" />
                    <br />
                    <asp:Literal ID="Literal1" runat="server" Visible="false"></asp:Literal>
                </div>
            </div>
        </div>

    </form>
</body>
</html>
