<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="订单数据展示.aspx.cs" Inherits="WebApplication11.cg.cjt.订单数据展示" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>订单数据展示</title>
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
        
        .pagination {
            display: flex;
            justify-content: center;
            margin: 20px 0;
        }
        
        .pagination a, .pagination span {
            padding: 8px 16px;
            text-decoration: none;
            border: 1px solid #ddd;
            margin: 0 4px;
        }
        
        .pagination a:hover {
            background-color: #ddd;
        }
        
        .pagination .disabled {
            opacity: 0.5;
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
            <div>
                <h3>当前页面【<span style="color: #37cbc5">订单数据展示</span>】</h3>
                <div>
                输入商家编码：
                <asp:TextBox ID="txtsjbm" runat="server" ValidationGroup="searchGroup"></asp:TextBox>
                <asp:RequiredFieldValidator 
                    ID="rfvSjbm" 
                    runat="server" 
                    ControlToValidate="txtsjbm"
                    ErrorMessage="* 必须输入商家编码"
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

                <br />
                输入浏览器店铺名：
                <asp:TextBox ID="txtBName" runat="server" ValidationGroup="combinedGroup"></asp:TextBox>
                输入产品标题：
                <asp:TextBox ID="txtPName" runat="server" ValidationGroup="combinedGroup"></asp:TextBox>
                <asp:Button ID="btnSearchCombined" runat="server"
                    Text="按店铺和产品查询"
                    BackColor="Purple"
                    ForeColor="White"
                    OnClick="btnSearchCombined_Click"
                    ValidationGroup="combinedGroup" />

                <br />
                <asp:Literal ID="Litera2" runat="server"></asp:Literal>
            </div>



                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>

            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex + 1 %><br />
                            </td>
                            <td style="width: 30%; text-align: center">
                              <img src='<%# Eval("pimage")%>' style="width:300px" /> 
                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 30%" class="bbb">产品链接</td>
                                        <td><a href="<%# Eval("purl") %>" target="_blank">打开网址</a>&nbsp;&nbsp;
                                            <input type="text" value='<%# Eval("purl") %>' style="width: 20px" id="url<%# Container.ItemIndex + 1 %>" />
                                            <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex + 1 %>")' value="复制网址">
                                         </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">下单时间</td>
                                        <td><%# Eval("order_date") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">浏览器店铺名</td>
                                        <td><%# Eval("BName") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">产品标题</td>
                                        <td><%# Eval("pname") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">order_id</td>
                                        <td><%# Eval("order_id") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">shopid</td>
                                        <td><%# Eval("shopid") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">ItemID</td>
                                        <td><%# Eval("ItemID") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">skuid</td>
                                        <td><%# Eval("skuid") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">总价</td>
                                        <td><%# Eval("total_price") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">状态</td>
                                        <td><%# Eval("status") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">订单数量</td>
                                        <td><%# Eval("item_amount") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">GroupName</td>
                                        <td><%# Eval("GroupName") %></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">商家编码</td>
                                        <td><%# Eval("SJBM") %></td>
                                    </tr>
                                    <tr>

                                    </tr>
                                </table>

                            </td>
                        </tr>
                        <tr></tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            
            <!-- 分页部分移动到表单内部 -->
            <div class="pagination">
                <asp:Button ID="btnPrev" runat="server" Text="上一页" 
                    CssClass="pager-btn" CommandName="Prev" OnClick="Page_Changed" 
                    Enabled='<%# CurrentPage > 1 %>' />
                
                <span class="current-page">
                    第 <%= CurrentPage %> 页/共 <%= TotalPages %> 页
                </span>
                    <!-- 新增跳转控件 -->
    <span style="margin:0 10px">
        跳转至 
        <asp:TextBox ID="txtJumpPage" runat="server" Width="50px" Text="1"></asp:TextBox>
        <asp:Button ID="btnJump" runat="server" Text="GO" 
            CssClass="pager-btn" OnClick="BtnJump_Click" />
    </span>
                
                <asp:Button ID="btnNext" runat="server" Text="下一页" 
                    CssClass="pager-btn" CommandName="Next" OnClick="Page_Changed" 
                    Enabled='<%# CurrentPage < TotalPages %>' />
            </div>
        </div>
    </form>
</body>
</html>



    

    <style>
        .pager-btn {
            padding: 5px 15px;
            margin: 0 5px;
            background-color: #007bff;
            color: white;
            border: none;
            border-radius: 4px;
        }
        .pagination {
            margin: 20px 0;
            text-align: center;
        }
    </style>
    