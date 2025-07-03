<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="left.ascx.cs" Inherits="WebApplication11.left" %>
<style>
    .txt {
        border: 1px solid #333;
        line-height: 18px;
        font-size: 16px;
    }

    .butt {
        border: 1px solid #333;
        background: #e9e9f9;
        color: #333;
        text-align: center;
        text-decoration: none;
    }

    .ttt {
        width: 100%;
    }

        .ttt tr {
        }

            .ttt tr td {
                border: 1px solid #c0c0d9;
                padding: 5px;
            }

    .ts p {
        text-align: center;
        font-size: 25px;
        color: red;
        font-weight: bold;
    }

    .main {
        margin: 0 auto;
        width: 1280px;
    }

    .head td {
        color: #0600ff;
        background: #cbdbcd;
    }

    .auto-style1 {
        width: 10%;
        height: 35px;
    }

    .auto-style2 {
        width: 30%;
        height: 35px;
    }

    .rightdiv {
        width: 180px;
        position: fixed;
        background: #cbdbcd;
    }

        .rightdiv a {
            color: black;
            text-decoration: none;
        }

    .hdt h2 {
    }

        .hdt h2 span {
            color: red;
        }

    .t0 {
    }

    .t1 {
        background-color: #00dfff;
    }

    .t2 {
        background-color: #e9cee9;
    }

    .t3 {
        background-color: #ed356a;
    }
</style>
<script>
    function closeleft() {
        document.getElementById('leftdh').style.display = 'none';
    }
</script>
<div class="rightdiv" id="leftdh">
    <table class="ttt">
        <tr class="t0">
            <td>
                <a onclick="closeleft()">关闭
                </a>
            </td>
        </tr>
        <tr class="t0">
            <td>
                <a href="/mb.aspx">模板管理
                </a>
            </td>
        </tr>
        <tr class="t0">
            <td>
                <a href="/uplb.aspx">指定批量修改
                </a>
            </td>
        </tr>
        <tr class="t1">
            <td>
                <a href="/other.aspx">其他数据管理
                </a>
            </td>
        </tr>
        <tr class="t1">
            <td>
                <a href="/addother.aspx">添加其他数据
                </a>
            </td>
        </tr>
        <tr class="t2">
            <td>
                <a href="/YGL.aspx">预加载自动管理
                </a>
            </td>
        </tr>
        <tr class="t2">
            <td>
                <a href="/addyjz.aspx">添加预加载
                </a>
            </td>
        </tr>
        <tr class="t1">
            <td>
                <a href="/jyc.aspx">禁用词表
                </a>
            </td>
        </tr>
        <tr class="t1">
            <td>
                <a href="/pz.aspx">导出配置管理
                </a>
            </td>
        </tr>
        <tr class="t3" style="display: none">
            <td>
                <a href="/out.aspx">导出数据
                </a>
            </td>
        </tr>
        <tr class="t3" runat="server" id="gm">
            <td>
                <a href="/out2.aspx" target="_blank">导出跟卖数据
                </a>
            </td>
        </tr>
        <tr class="t3">
            <td>
                <a href="/DT.aspx" target="_blank">导出结果统计
                </a>
            </td>
        </tr>
        <tr class="t3">
            <td>
                <a href="/fc.aspx">反查产品
                </a>
            </td>
        </tr>
        <tr class="t3">
            <td>
                <a href="/pipei.aspx">产品匹配模板id
                </a>
            </td>
        </tr>
        <tr class="t3">
            <td>
                <a href="/pm.aspx">产品统计
                </a>
            </td>
        </tr>
        <tr class="t3">
            <td>
                <a href="/pinpai.aspx">品牌统计和管理
                </a>
            </td>
        </tr>
        <tr class="t">
            <td><a href="/login.aspx?type=out"><%=u %>退出账号
            </a>
            </td>
        </tr>

    </table>
</div>
