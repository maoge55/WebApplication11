<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="阿里狗群控任务表.aspx.cs" Inherits="WebApplication11.cg.阿里狗群控任务表" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>阿里狗群控任务表</title>
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
            <h3>当前页面【<span style="color: #cba537">阿里狗群控任务表</span>】
              
            </h3>

            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>
        <div>
            <form id="form2" action="阿里狗群控任务表.aspx" method="get">
                国家：
                 <select id="Country" name="Country">
                     <option value="all">全部</option>
                     <option value="pl">波兰</option>
                 </select>
                &nbsp;
                平台：
                 <select id="PingTai" name="PingTai">
                     <option value="all">全部</option>
                     <option value="Allegro">阿里狗</option>
                 </select>
                &nbsp;
                商家编码：<input type="text" name="sjbm" id="sjbm" />
                &nbsp;
                 文本类型：<select id="wenben" name="wenben">
                     <option value="no">无</option>
                     <option value="jiagegongshi">价格公式</option>
                     <option value="gudinglie">固定列</option>
                     <option value="jiagequjian">价格区间</option>
                     <option value="meifenshuliang">每份数量</option>
                     <option value="biaogeshuliang">表格数量</option>
                     <option value="MinSalePrice">最低售价</option>
                     <option value="DataGroup">数据分组</option>
                     <option value="YHYGgongShi">新价格公式</option>
                 </select>&nbsp;
               
                 任务类型：<select id="renwu" name="renwu">
                     <option value="no">无</option>
                     <option value="is_task_baned">关闭所有任务</option>
                     <option value="isUpload">上传跟卖产品</option>
                     <option value="isDownloadBg">下载跟卖报告</option>
                     <option value="isCjll">采集跟卖流量</option>
                     <%--   <option value="isEnd">下架跟卖产品</option>
                     <option value="isActive">上架跟卖产品</option>--%>
                     <option value="ProxyIP">ProxyIP</option>
                     <option value="isCJPay">采集付费链接</option>
                     <option value="isDelNoll">删除无流量产品</option>
                     <option value="is_end_qq">下架侵权产品</option>
                     <option value="isGetOrder">采集阿里狗订单信息</option>
                     <option value="isGetMes">采集阿里狗买家消息</option>
                     <option value="isGetDiscussion">采集阿里狗纠纷</option>
                     <option value="isCJALHome">采集 首页+订单+消息+纠纷+付费</option>

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
                &nbsp;
                <asp:Button ID="Button1" CssClass="gg" BackColor="blue" ForeColor="White" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" runat="server" Text="保存整页" OnClick="Button1_Click1" />
            </div>
            <table class="ttt">
                <tr class="aaa4">
                    <td>序号</td>
                    <td>浏览器名称</td>
                    <td style="width: 10%">浏览器分组</td>
                    <td>登录网址</td>
                    <td>国家</td>
                    <td>平台</td>
                    <td>登录用户名</td>
                    <td>登录密码</td>
                    <td>商家编码</td>
                    <td>运营编码</td>
                    <%=wenben!=""?"<td style=\"width:10%\">"+wenben+"</td>":"" %>
                    <%=renwu!=""?"<td style=\"width:10%\">"+renwu+"</td>":"" %>

                    <td>更新</td>
                </tr>
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="liid" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Literal>
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
                                <asp:TextBox ID="txtCountry" runat="server" Text='<%# Eval("Country")%>'></asp:TextBox>
                            </td>
                            <td style="text-align: center">
                                <asp:TextBox ID="txtPingTai" runat="server" Text='<%# Eval("PingTai")%>'></asp:TextBox>
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
                            <td style='text-align: center; display: <%# Eval("wb").ToString()!="镍"?"123":"none"%>'>
                                <asp:TextBox ID="txtwb" runat="server" Text='<%# Eval("wb")%>' Visible='<%# Eval("wb").ToString()!="镍"?true:false%>'></asp:TextBox>
                            </td>
                            <td style='text-align: center; display: <%# Eval("nowrw").ToString()!="-1"?"123":"none"%>'>
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
            const wenben = document.getElementById('wenben');
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
            if (wenben && params.wenben) {
                wenben.value = params.wenben;
            }

        });
    </script>
</body>
</html>
