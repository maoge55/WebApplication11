<%@ Page Language="C#" AutoEventWireup="true" ViewStateMode="Enabled" CodeBehind="管理广告测试产品.aspx.cs" Inherits="WebApplication11.cg.管理广告测试产品" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>管理广告测试产品</title>
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



        }
    </script>
</head>
<body>

    <div>
        <div>
            <h3>当前页面【<span style="color: #37cbc5">管理广告测试产品</span>】</h3>

            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
        </div>
        <div>
            <form id="form2" action="管理广告测试产品.aspx" method="get">
                商家编码：
            <input type="text" name="txtsjbm" id="txtsjbm" />

                <input id="rdno" type="radio" name="rdno" title="无入仓链接" value="no" checked="checked" />无入仓链接
          <input id="rdyes" name="rdno" type="radio" title="有入仓链接" value="yes" />有入仓链接
             &nbsp;
           <button type="submit" id="btns1">搜索查找</button>
            </form>
            <br />

            <form id="form3" action="管理广告测试产品.aspx" method="get">
                &nbsp;&nbsp;&nbsp;&nbsp;随机码：
            <input type="text" name="txtcode" id="txtcode" />&nbsp;
           <button type="submit" id="btns2">搜索查找</button>
            </form>
            <br />
            <br />
            <asp:Literal ID="lify" runat="server"></asp:Literal><br />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>

        </div>
        <br />
        <form id="form1" runat="server">
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="licode" runat="server" Text='<%# Eval("code") %>' Visible="false"></asp:Literal>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />

                            </td>
                            <td style="width: 20%; text-align: center">
                                <img src='<%# Eval("MainImage")%>' style="width: 200px; height: 200px" />
                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 20%" class="bbb">随机码</td>
                                        <td style="width: 40%"><%# Eval("code") %></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">标题</td>
                                        <td style="width: 40%"><%# Eval("Title") %></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">商家编码</td>
                                        <td style="width: 40%"><%# Eval("shangjiabianma") %></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">1688采购链接</td>
                                        <td style="width: 40%"><a href="<%# Eval("Y_1688url") %>" target="_blank">打开网址</a>
                                            <input type="text" value='<%# Eval("Y_1688url") %>' style="width: 20px" id="url<%# Container.ItemIndex+1 %>" />

                                        </td>
                                        <td>
                                            <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">二次铺货链接</td>
                                        <td><%# Eval("ercipuhuoURL") %></td>
                                        <td>

                                            <asp:TextBox ID="txtercipuhuoURL" Text='' runat="server"></asp:TextBox>
                                            <asp:LinkButton ID="LinkButton2" OnClientClick="JavaScript:return confirm('确定保存？');" runat="server" Text="保存数据" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="erci" CommandArgument='<%# Eval("code") %>' />
                                        </td>
                                    </tr>


                                    <tr>
                                        <td class="bbb">可发的海运</td>
                                        <td>
                                            <%# Eval("kefadehaiyun") %></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">产品链接</td>
                                        <td style="width: 40%"><%# Eval("purl") %></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">印尼新售价</td>
                                        <td style="width: 40%"><%# Eval("NewPrice_shopeeid") %></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" class="bbb">入仓链接</td>
                                        <td style="width: 40%"><%# Eval("rucanglianjie") %></td>
                                        <td>

                                            <asp:TextBox ID="txtrucanglianjie" Text='' runat="server"></asp:TextBox>
                                            <asp:LinkButton ID="LinkButton3"  OnClientClick="JavaScript:return confirm('确定保存？');" runat="server" Text="保存数据" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="rucang" CommandArgument='<%# Eval("code") %>' />
                                        </td>
                                    </tr>



                                    <tr>
                                        <td style="width: 20%" class="bbb">印尼入仓店铺备注</td>
                                        <td style="width: 40%"><%# Eval("dianpubeizhu") %></td>
                                        <td>

                                            <asp:TextBox ID="txtdianpubeizhu" Text='' runat="server"></asp:TextBox>
                                            <asp:LinkButton ID="LinkButton4" OnClientClick="JavaScript:return confirm('确定保存？');" runat="server" Text="保存数据" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="bz" CommandArgument='<%# Eval("code") %>' />
                                        </td>
                                    </tr>


                                </table>





                            </td>

                        </tr>
                        <tr></tr>
                    </ItemTemplate>
                </asp:Repeater>

            </table>
        </form>
        <br />
        <asp:Literal ID="lify2" runat="server"></asp:Literal>

    </div>

</body>
</html>
