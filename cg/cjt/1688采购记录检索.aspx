<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="1688采购记录检索.aspx.cs" Inherits="WebApplication11.cg.cjt._1688采购记录检索" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>1688采购记录检索</title>
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
            .pager-btn {
        padding: 5px 15px;
        margin: 0 2px;
        background: #4CAF50;
        color: white;
        border: none;
        border-radius: 3px;
        cursor: pointer;
    }
    .pager-btn:disabled {
        background: #cccccc;
        cursor: not-allowed;
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
                <h3>当前页面【<span style="color: #37cbc5">1688采购记录检索</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>

                输入运营编码：
                <asp:TextBox ID="txtsjbm" runat="server" ValidationGroup="searchGroup"></asp:TextBox>
                <asp:RequiredFieldValidator 
                    ID="rfvSjbm" 
                    runat="server" 
                    ControlToValidate="txtsjbm"
                    ErrorMessage="* 必须输入运营编码"
                    ForeColor="Red"
                    Display="Dynamic"
                    ValidationGroup="searchGroup">
                </asp:RequiredFieldValidator>
                &nbsp;

                <asp:Button ID="Button1" runat="server" 
                    Text="查找" 
                    BackColor="Red" 
                    ForeColor="White" 
                    OnClick="Button1_Click" 
                    ValidationGroup="searchGroup" />
                 输入产品标题：
                <asp:TextBox ID="txtPName" runat="server" ValidationGroup="combinedGroup"></asp:TextBox>
                <asp:Button ID="btnSearchCombined" runat="server"
                    Text="按标题查询"
                    BackColor="Purple"
                    ForeColor="White"
                    OnClick="btnSearchCombined_Click"
                    ValidationGroup="combinedGroup" />
                 输入Offer_ID：
                <asp:TextBox ID="txtOfferID" runat="server" ValidationGroup="offerIDGroup"></asp:TextBox>
                <asp:Button ID="btnSearchOfferID" runat="server"
                    Text="按Offer_ID查询"
                    BackColor="Orange"
                    ForeColor="White"
                    OnClick="btnSearchOfferID_Click"
                    ValidationGroup="offerIDGroup" />
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            </div>
            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>

                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />

                            </td>
                            <td style="width: 30%; text-align: center">
                              <img src='<%# Eval("sku_img")%>' style="width:300px" /> 
                            </td>
                            <td>
                                <table class="ttta">

                                    <tr>
                                        <td style="width: 30%" class="bbb">1688标题</td>
                                        <td><%# Eval("huopinbiaoti") %></td>

                                    </tr>
                               
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688产品ID</td>
                                        <td><%# Eval("Offer_ID") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688SKU_ID</td>
                                        <td><%# Eval("SKU_ID") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688链接</td>
                                                                            <td>
                                        <a href='<%# "https://detail.1688.com/offer/" + Eval("Offer_ID") + ".html" %>' target="_blank">
                                            https://detail.1688.com/offer/<%# Eval("Offer_ID") %>.html
                                        </a>
                                    </td>

                                    </tr>


                                </table>

                                

                            </td>

                        </tr>
                        <tr></tr>
                    </ItemTemplate>
                </asp:Repeater>
               
            </table>
 <div class="pager-container" style="text-align: center; margin: 20px 0; padding: 15px; background-color: #f8f9fa; border-top: 1px solid #e9ecef;">
    <asp:Button ID="btnPrev" runat="server" Text="上一页" OnClick="btnPrev_Click" CssClass="pager-btn" />
       <!-- 新增跳转控件 -->
    <span style="margin:0 10px">
        跳转至 
        <asp:TextBox ID="txtJumpPage" runat="server" Width="50px" Text="1"></asp:TextBox>
        <asp:Button ID="btnJump" runat="server" Text="GO" 
                    OnClick="btnJump_Click" CssClass="pager-btn" />
    </span>
     <span style="margin:0 10px"><asp:Literal ID="litPageInfo" runat="server"></asp:Literal></span>
    <asp:Button ID="btnNext" runat="server" Text="下一页" OnClick="btnNext_Click" CssClass="pager-btn" />
</div>
    </form>
</body>

</html>
