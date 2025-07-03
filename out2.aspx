<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="out2.aspx.cs" Inherits="WebApplication11._out2" %>

<%@ Register Src="~/left.ascx" TagName="left" TagPrefix="ul" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.6.1.min.js"></script>
    <title>导出跟卖数据</title>
    <style type="text/css">
        .auto-style3 {
            width: 407px;
        }

        .auto-style4 {
            width: 257px;
        }

        .txt {
            font-weight: bold;
        }

        .rightdiv {
            display: none;
        }
    </style>
</head>
<body>
    <ul:left ID="left" runat="server" />

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="main" style="width: 100%">

            <div id="zc" runat="server" visible="true">
                <div class="hdt">
                    <h2>
                        <asp:Literal ID="Literal5" runat="server" Text=""></asp:Literal>
                        <asp:Literal ID="lisql" runat="server" Visible="false" Text="导出数据"></asp:Literal>
                        <asp:Literal ID="Literal1" runat="server" Text="导出跟卖数据"></asp:Literal></h2>
                    <span><a href="/mb.aspx" style="color: red">模板管理</a></span>
                    <span><a href="/pm.aspx" style="color: blue">产品统计</a></span>
                </div>
                <div class="ts">
                    <p>
                        <div id="rrrrx">xxxx</div>
                        <asp:Literal runat="server" ID="lits"></asp:Literal>
                        <asp:Literal ID="lipath" runat="server" Visible="false"></asp:Literal>
                    </p>
                </div>
                <div>
                    <div style="display: none">
                        <table class="ttt" id="add" style="background: beige;">
                            <tr>
                                <td colspan="4">
                                    <b style="color: red">导出目标</b>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style4">模板ID：
                                <asp:TextBox runat="server" ID="txtmid" Width="100px" CssClass="txt"></asp:TextBox>
                                    <asp:Label ID="limid" Visible="false" runat="server" Text="Label"></asp:Label>

                                </td>

                                <td class="auto-style4">模板名称：
                                <asp:TextBox runat="server" ID="txtname" Width="100px" CssClass="txt"></asp:TextBox>
                                    <asp:Label ID="liname" Visible="false" runat="server" Text="Label"></asp:Label>
                                    <asp:Label ID="list" Visible="false" runat="server" Text="Label"></asp:Label>
                                </td>
                                <td class="auto-style4" colspan="2">
                                    <asp:Button ID="btnadddata" runat="server" Text="搜索" ForeColor="Red" CssClass="butt" OnClick="btnadddata_Click" />
                                    <span style="color: red">(*一个条件即可)</span>

                                </td>
                            </tr>
                        </table>
                        <br />
                        <table class="ttt" id="add2" style="background: darkseagreen;">
                            <tr>
                                <td colspan="4">
                                    <b style="color: red">筛选过滤条件</b>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style3">产品评分：
                                <asp:TextBox runat="server" ID="txtcppf" Width="100px" CssClass="txt"></asp:TextBox></td>

                                <td class="auto-style3">描述评分：
                                <asp:TextBox runat="server" ID="txtmspf" Width="100px" CssClass="txt"></asp:TextBox></td>


                                <td class="auto-style3">沟通评分：
                                <asp:TextBox runat="server" ID="txtgtpf" Width="100px" CssClass="txt"></asp:TextBox></td>
                                <td class="auto-style3">发货评分：
                                <asp:TextBox runat="server" ID="txtfhpf" Width="100px" CssClass="txt"></asp:TextBox></td>
                            </tr>
                            <tr>


                                <td class="auto-style3">开店年限：
                                <asp:TextBox runat="server" ID="txtkdnx" Width="100px" CssClass="txt"></asp:TextBox></td>

                                <td class="auto-style3">历史销量：
                                <asp:TextBox runat="server" ID="txtzxl" Width="100px" CssClass="txt"></asp:TextBox></td>
                                <td class="auto-style3">汇率参数：
                                <asp:TextBox runat="server" ID="txthl" Width="100px" CssClass="txt"></asp:TextBox></td>
                                <td class="auto-style3">价格区间：
                                <asp:TextBox runat="server" ID="txtjgqj" Width="100px" CssClass="txt"></asp:TextBox></td>
                            </tr>
                        </table>
                        <br />

                        <table class="ttt" id="add1" style="background: lavender;">
                            <tr>
                                <td colspan="4">
                                    <b style="color: red">导出条件</b>
                                </td>
                            </tr>
                            <tr>
                                <td>新价格为原价的倍数：
                                <asp:TextBox runat="server" ID="txtjgbs" Width="100px" CssClass="txt" Text="2.5"></asp:TextBox>
                                    <br />
                                    <span style="color: red">提示：新价格=原价*倍数*汇率</span>
                                </td>

                                <td>每份批量表产品数量：
                                <asp:TextBox runat="server" ID="txtpdcount" Width="100px" CssClass="txt" Text="500"></asp:TextBox>
                                    <br />
                                    <span style="color: red">提示：默认500，不可修改</span>
                                </td>


                                <td>导出批量表数量：
                                <asp:TextBox runat="server" ID="txtwjcount" Width="100px" CssClass="txt" Text="1"></asp:TextBox>
                                    <br />
                                    <span style="color: red">&nbsp;</span>
                                </td>
                                <td>
                                    <asp:Button ID="Button1" runat="server" Text="查询" ForeColor="Blue" CssClass="butt" OnClick="Button1_Click" />
                                    &nbsp;
                                  <asp:Button ID="Button2" runat="server" Text="导出" ForeColor="Red" CssClass="butt" OnClick="Button2_Click" />
                                    <br />
                                    <span style="color: red">&nbsp;</span>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>
                    <div>
                        <table class="ttt" id="add3" style="background: #ebf1ff;">
                            <tr>
                                <td>
                                    <b style="color: red">批量导出</b>
                                    <input style="display: none" type="button" id="btna" class="butt" value="开始导出" />
                                    <script>

                                        document.querySelector('#btna').addEventListener('click', function () {
                                            var wd = document.querySelectorAll('.wd');
                                            var lr = document.querySelectorAll('.lr');
                                            var zt = document.querySelectorAll('.zt');
                                            (async function foo() {
                                                for (i = 0; i < lr.length; ++i) {
                                                    if (zt[i].innerHTML == "0") {

                                                        await p(lr[i].innerHTML).then(function () {
                                                            document.querySelector('#rrrrx').innerHTML = i;
                                                        });

                                                    }
                                                }
                                            })();
                                        })
                                        function q(time) {
                                            return new Promise((resolve, reject) => {
                                                setTimeout(() => {
                                                    resolve(time);
                                                }, time);
                                            });
                                        }
                                        function p(LRID) {
                                            return new Promise((resolve, reject) => {

                                                $.ajax({
                                                    url: "/ca.asmx/HelloWorld",
                                                    type: 'Post',
                                                    async: false,
                                                    data: {
                                                        LRID: LRID,
                                                        uid: 2
                                                    },
                                                    dataType: 'text',
                                                    success: function (data) {

                                                        resolve(data);

                                                    }
                                                });
                                            });
                                        }

                                    </script>
                                    <asp:Button ID="Button3" runat="server" ForeColor="White" BackColor="Green" CssClass="butt" Text="开始导出" OnClick="Button3_Click" />
                                    &nbsp;
                                    &nbsp;
                                    <asp:Button OnClientClick="JavaScript:return confirm('确定改为成功？');" ID="Button5" ForeColor="White" BackColor="Blue" CssClass="butt" runat="server" Text="改为成功" OnClick="Button5_Click" />

                                    &nbsp;
                                    &nbsp;
                                     <asp:Button OnClientClick="JavaScript:return confirm('确定删除吗？');" ID="Button4" ForeColor="White" BackColor="Red" CssClass="butt" runat="server" Text="删除任务" OnClick="Button4_Click" />


                                    <asp:Literal ID="Literal4" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr runat="server" id="yy" visible="false">
                                <td>当前任务：<asp:Literal ID="liyy" runat="server"></asp:Literal>

                                </td>
                            </tr>
                            <tr>
                                <td>上传表格：
                                <asp:FileUpload ID="fup1" runat="server" CssClass="butt" />
                                    <asp:Button ID="Button31" runat="server" Text="上传" CssClass="butt" OnClick="Button31_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span style="color: red">
                                        <asp:Literal ID="Literal2" runat="server"></asp:Literal></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button OnClientClick="JavaScript:return confirm('生成压缩包？');" ID="Button6" ForeColor="White" BackColor="Blue" CssClass="butt" runat="server" Text="生成压缩包" OnClick="Button6_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>

                                    <span style="color: red">
                                        <asp:Literal ID="Literal3" runat="server"></asp:Literal></span>

                                </td>
                            </tr>
                        </table>

                    </div>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
