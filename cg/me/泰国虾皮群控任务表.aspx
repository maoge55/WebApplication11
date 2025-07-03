<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="泰国虾皮群控任务表.aspx.cs" Inherits="WebApplication11.cg.泰国虾皮群控任务表" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>泰国虾皮群控任务表</title>
    <style>
        .ttt {
            width: 100%;
        }

            .ttt tr {
            }

                .ttt tr td {
                    border: 1px solid #c0c0d9;
                    padding: 5px;
                }

        .ttta {
            width: 100%;
        }

            .ttta tr td {
                border: 1px solid #cba537;
                padding: 5px;
            }

        .bbb {
            font-weight: bold;
        }

        .butt {
        }

        .anniu1 {
            border: 1px red solid;
        }

        .aaa4 td {
            background: #fb686842;
            text-align: center;
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
            <h3>当前页面【<span style="color: #cba537">泰国虾皮群控任务表</span>】
              
            </h3>

            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>
        <div>
            <form id="form2" action="泰国虾皮群控任务表.aspx" method="get">
                国家：
                 <select id="Country" name="Country">
                     <option value="all">全部</option>
                     <option value="th">泰国</option>
                 </select>
                &nbsp;
                平台：
                 <select id="PingTai" name="PingTai">
                     <option value="all">全部</option>
                     <option value="shopee">虾皮</option>
                 </select>
                &nbsp;
                商家编码：<input type="text" name="sjbm" id="sjbm" />
                &nbsp;
                 任务类型：<select id="renwu" name="renwu">
                     <option value="no">无</option>
                     <option value="is_task_baned">关闭所有任务</option>
                     <option value="is_cj_ShopInfo">采集店铺资料(首页+订单+库存)</option>
                     <option value="is_create_ad">创建广告</option>
                     <option value="is_youhua">广告优化</option>
                     <option value="is_tz_kw">广告词拓展</option>

                 </select>&nbsp;
                 任务状态：
                 <select id="renwu_zt" name="renwu_zt">
                     <option value="1">有任务</option>
                     <option value="0">无任务</option>
                 </select>
                &nbsp;
                 <button type="submit" id="btns1">搜索查找</button>
            </form>
            <br />

            <asp:Literal ID="Literal1" runat="server"></asp:Literal>

        </div>

        <br />
        <form id="form1" runat="server">
            <div>
                <asp:Button ID="Button5" CssClass="gg" BackColor="Green" ForeColor="White" runat="server" Text="全选1" OnClick="Button5_Click" />
                &nbsp;
                <asp:Button ID="Button6" CssClass="gg" BackColor="Red" ForeColor="White" runat="server" Text="全选0" OnClick="Button6_Click" />
                &nbsp;  &nbsp;  &nbsp;  &nbsp;  &nbsp;  &nbsp;  &nbsp;
               
               
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="Button1" CssClass="gg" BackColor="blue" ForeColor="White" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" runat="server" Text="保存整页" OnClick="Button1_Click1" />
            </div>
            <table class="ttt">
                <tr class="aaa4">
                    <td>序号</td>
                    <td>浏览器名称</td>
                    <td>浏览器分组</td>
                    <td>登录网址</td>
                    <td>登录用户名</td>
                    <td>登录密码</td>
                    <td>商家编码</td>
                    <td>运营编码</td>
                    <td style="width: 10%">选中任务</td>
                    <td>更新</td>
                </tr>
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">

                    <ItemTemplate>
                        <asp:Literal ID="liid" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Literal>
                      
                        <tr>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %>
                                <br />
                                <%# Eval("id")%>
                            </td>
                            <td style="text-align: center">
                                <%# Eval("DpName")%>
                                <br />
                                <asp:LinkButton ID="LinkButton2" OnClientClick="JavaScript:return confirm('确定改为被封？');" runat="server" Text="封" ForeColor="White" BackColor="Red" CssClass="butt" CommandName="bf" CommandArgument='<%# Eval("id") %>' />
                            </td>
                            <td style="text-align: center">
                                <%# Eval("GroupName")%>
                            </td>
                            <td style="text-align: center">
                                <asp:TextBox ID="txtPlatform" runat="server" Text='<%# Eval("Platform")%>'></asp:TextBox>
                            </td>

                            <td style="text-align: center">
                                <asp:TextBox ID="txtUserName" runat="server" Text='<%# Eval("UserName")%>'></asp:TextBox>
                            </td>
                            <td style="text-align: center">
                                <asp:TextBox ID="txtPassword" runat="server" Text='<%# Eval("Password")%>'></asp:TextBox>
                            </td>
                            <td style="text-align: center">
                                <asp:TextBox ID="txtSJBM" runat="server" Text='<%# Eval("SJBM")%>'></asp:TextBox>
                            </td>
                            <td style="text-align: center">
                                <asp:TextBox ID="txtYYBM" runat="server" Text='<%# Eval("YYBM")%>'></asp:TextBox>
                            </td>






                            <td style="text-align: center">
                                <asp:Literal ID="linowrw" runat="server" Visible="false" Text='<%# Eval("nowrw")%>'></asp:Literal>
                                <asp:RadioButtonList ID="rdjg" CssClass="rdcss" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="0">0</asp:ListItem>
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                </asp:RadioButtonList>


                            </td>
                            <td style="text-align: center">

                                <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确定更新？');" runat="server" Text="更新" ForeColor="White" BackColor="Green" CssClass="butt" CommandName="up" CommandArgument='<%# Eval("id") %>' />
                            </td>



                        </tr>

                    </ItemTemplate>
                </asp:Repeater>

            </table>
        </form>
        <br />

    </div>
    <script>
        function getQueryParams() {
            const queryString = window.location.search;
            const urlParams = new URLSearchParams(queryString);
            const params = {};
            for (const [key, value] of urlParams.entries()) {
                params[key] = value;

            }
            return params;
        }
        document.addEventListener("DOMContentLoaded", function () {
            const params = getQueryParams();


            const sjbm = document.getElementById('sjbm');
            const Country = document.getElementById('Country');
            const PingTai = document.getElementById('PingTai');
            const renwu = document.getElementById('renwu');
            const renwu_zt = document.getElementById('renwu_zt');

            if (sjbm && params.sjbm) {
                sjbm.value = params.sjbm;
            }
            if (Country && params.Country) {
                Country.value = params.Country;
            }
            if (PingTai && params.PingTai) {
                PingTai.value = params.PingTai;
            }
            if (renwu && params.renwu) {
                renwu.value = params.renwu;
            }
            if (renwu_zt && params.renwu_zt) {
                renwu_zt.value = params.renwu_zt;
            }

        });
    </script>
</body>
</html>
