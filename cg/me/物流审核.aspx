<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="物流审核.aspx.cs" Inherits="WebApplication11.cg.物流审核" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>物流审核</title>
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
                border: 1px solid crimson;
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



        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <h3>当前页面【<span style="color: crimson">物流审核</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>

                <asp:Button ID="Button1" runat="server" OnClientClick="JavaScript:return confirm('确定自动补充同随机码的物流商吗？');" Text="自动补充同随机码的物流商" OnClick="Button1_Click1" />&nbsp;<br />
                <br />
                <br />
&nbsp;<br />
                <asp:Button ID="Button2" runat="server" OnClientClick="JavaScript:return confirm('确定查询需审核产品数量吗？');" Text="查询需审核产品数量" OnClick="Button2_Click3"  />&nbsp;
                 <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>
            <br />


        </div>
    </form>
</body>
</html>
