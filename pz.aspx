<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pz.aspx.cs" Inherits="WebApplication11.pz" %>

<%@ Register Src="~/left.ascx" TagName="left" TagPrefix="ul" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>编辑导出配置</title>
    <style type="text/css">
        .auto-style3 {
            width: 82px;
            text-align: right;
        }
    </style>
</head>
<body>
    <ul:left ID="left" runat="server" />
    <form id="form1" runat="server">
        <div class="main" style="width: 980px">

            <div id="zc" runat="server" visible="true">
                <div class="hdt">
                    <h2>
                        <asp:Literal ID="Literal1" runat="server" Text="编辑导出配置"></asp:Literal></h2>
                </div>
                <div class="ts">
                    <p>
                        <asp:Literal runat="server" ID="lits"></asp:Literal>
                    </p>
                </div>
                <div>
                    <table class="ttt" id="add">
                        <tr>
                            <td class="auto-style3">产品评分：</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtcppf" Width="100px" CssClass="txt"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="auto-style3">描述评分：</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtmspf" Width="100px" CssClass="txt"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="auto-style3">沟通评分：</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtgtpf" Width="100px" CssClass="txt"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="auto-style3">发货评分：</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtfhpf" Width="100px" CssClass="txt"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="auto-style3">开店年限：</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtkdnx" Width="100px" CssClass="txt"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="auto-style3">粉丝数量：</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtfssl" Width="100px" CssClass="txt"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="auto-style3">总销量：</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtzxl" Width="100px" CssClass="txt"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="auto-style3">价格区间：</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtjgqj" Width="100px" CssClass="txt"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="auto-style3">汇率：</td>
                            <td>
                                <asp:TextBox runat="server" ID="txthl" Width="100px" CssClass="txt"></asp:TextBox>

                                <span style="color:red">(新价格=原价*倍数*汇率)</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style3"></td>
                            <td>
                                <asp:Button ID="btnadddata" runat="server" Text="保存" ForeColor="Red" CssClass="butt" OnClick="btnadddata_Click" /></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
