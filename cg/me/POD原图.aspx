<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="POD原图.aspx.cs" Inherits="WebApplication11.cg.POD原图" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>POD原图</title>
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
                border: 1px solid #fd00ff;
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

        img {
            float: left;
            width: 300px !important;
            height: 300px !important;
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
                <h3>当前页面【<span style="color: #fd00ff">POD原图</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                随机码：<asp:TextBox ID="txtsjm" runat="server"></asp:TextBox>&nbsp;
                <asp:Button ID="Button1" runat="server" Text="查找" OnClick="Button1_Click" />
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                &nbsp;&nbsp;
            
            </div>
            <br />

            <table class="ttt">

                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="liid" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Literal>
                            <asp:Literal ID="liimages" runat="server" Text='<%# Eval("PODyuantu") %>' Visible="false"></asp:Literal>
                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />
                                id(<%# Eval("id")%>)

                            </td>
                            <td style="width: 30%; text-align: center">
                                <a href='<%# Eval("image")%>' target='_blank'>
                                <img src='<%# Eval("image")%>' style="width: 300px" />
                                   </a>

                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 10%" class="bbb">标题</td>
                                        <td style=""><%# Eval("yntitlercode") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%" class="bbb">所有主图</td>
                                        <td style=""><%#bindzzzzimg(Eval("images").ToString()) %></td>
                                    </tr>


                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>



                                    <tr>
                                        <td class="bbb">POD原图</td>
                                        <td>

                                            <asp:DataList ID="DataList1" DataKeyField="imgname" runat="server"
                                                OnItemCommand="DataList1_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:Literal Text='<%# Eval("id").ToString() %>' runat="server" ID="liid" Visible="false"></asp:Literal>
                                                    <table bgcolor="black" cellpadding="4" cellspacing="1">
                                                        <tr>
                                                            <td bgcolor="white" width="60px">
                                                                <a href='<%# "/Uploads/"+Eval("imgname").ToString()%>' target="_blank">
                                                                    <img id="img" src='<%# "/Uploads/"+Eval("imgname").ToString()%>' height="60" width="60"
                                                                        border="0" /></a>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td bgcolor="white" style="display:none" align="center">
                                                                <asp:Button ID="del" Text="del" Width="50px" runat="server" CommandName="del" CommandArgument='<%# Eval("imgname") %>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>

                                            <asp:Literal ID="liimgxs" runat="server"></asp:Literal>
                                            <br />
                                            <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" />
                                            <asp:LinkButton ID="LinkButton2" OnClientClick="JavaScript:return confirm('确认上传？');" runat="server" Text="确认上传" ForeColor="White" BackColor="Blue" CssClass="butt" CommandName="up" CommandArgument='<%# Eval("id") %>' />
                                        </td>
                                    </tr>
                                </table>





                            </td>

                        </tr>
                        <tr></tr>
                    </ItemTemplate>
                </asp:Repeater>

            </table>

        </div>
    </form>
</body>
</html>
