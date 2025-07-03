<%@ Page Language="C#" AutoEventWireup="true"  ValidateRequest="false" CodeBehind="翻译成泰语.aspx.cs" Inherits="WebApplication11.cg.翻译成泰语" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%=name %></title>
    <style>
        .ttt {
            width: 100%;
        }

            .ttt tr {
            }

                .ttt tr td {
                    border: 1px solid #000000;
                    padding: 5px;
                }

        .ttta {
            width: 100%;
        }

            .ttta tr td {
                border: 1px solid #37cbc5;
                padding: 5px;
            }

        .bbb {
            font-weight: bold;
        }

        .butt {
            padding: 0 50px;
        }

        .anniu1 {
            border: 1px red solid;
        }
    </style>
    <script>
        function copyUrl(myurl) {

            var Url2 = document.getElementById(myurl);
            Url2.select();
            document.execCommand("Copy");
            alert("复制成功");


        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <h3>当前页面【<span style="color: #37cbc5"><%=name %></span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>

            导出：
            <br />
            <asp:TextBox ReadOnly="true" ID="txtoutfy" runat="server" Height="257px" Width="95%" TextMode="MultiLine" ValidateRequestMode="Disabled"></asp:TextBox>
            <br />
            <asp:Button ID="btnsearch" runat="server" Text="加载标题" Width="150px" OnClientClick="" BackColor="Blue" ForeColor="White" OnClick="btnsearch_Click" />
            &nbsp;&nbsp;
             <input type="button" class="anniu1" onclick="copyUrl('txtoutfy')" value="复制内容">
            <br />
            导入：
            <br />
            <asp:TextBox ID="txtinfy" runat="server" Height="257px" Width="95%" TextMode="MultiLine" ValidateRequestMode="Disabled"></asp:TextBox>
            <br />
            <asp:Button ID="Button1" runat="server" Text="更新标题" OnClientClick="JavaScript:return confirm('确定更新标题吗？');" Width="150px" BackColor="Green" ForeColor="White" OnClick="Button1_Click" />
            <br />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>

        </div>
    </form>
</body>
</html>
