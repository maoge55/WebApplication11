<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="广告测品订单.aspx.cs" Inherits="WebApplication11.cg.cjt.广告测品订单" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>广告测品订单</title>
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
            <div>
                <h3>当前页面【<span style="color: #37cbc5">广告测品订单</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                    状态筛选：
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="status-filter">
                    <asp:ListItem Value="-1">全部状态</asp:ListItem>
                    <asp:ListItem Value="0">0-未处理</asp:ListItem>
                    <asp:ListItem Value="1">1-已下架原始链接</asp:ListItem>
                    <asp:ListItem Value="2">2-不能销售</asp:ListItem>
                    <asp:ListItem Value="3">3-已下单1688</asp:ListItem>
                    <asp:ListItem Value="4">4-已收货</asp:ListItem>
                    <asp:ListItem Value="5">5-需重新采购</asp:ListItem>
                    <asp:ListItem Value="6">6-已发仓</asp:ListItem>
                </asp:DropDownList>
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
                 <asp:Button ID="Button1" runat="server" 
                    Text="查找" 
                    BackColor="Red" 
                    ForeColor="White" 
                    OnClick="Button1_Click" 
                    ValidationGroup="searchGroup" />
                &nbsp;
           <asp:Button ID="btnSyncStatus" runat="server" Text="同步状态" ForeColor="Green" 
                    OnClientClick="return confirm('确定同步所有记录的状态吗？');" 
                OnClick="btnSyncStatus_Click" />


                <asp:Button ID="Button2" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页" OnClick="Button2_Click1" />

                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            </div>
            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>
                           <asp:Literal ID="skuid" runat="server" Text='<%# Eval("skuid") %>' Visible="False"></asp:Literal>
                                
                           <asp:Literal ID="litItemID" runat="server" Text='<%# Eval("ItemID") %>' Visible="False"></asp:Literal>
                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />

                            </td>
                            <td style="width: 30%; text-align: center">
                              <img src='<%# Eval("pimage")%>' style="width:300px" /> 
                            </td>
                            <td>
                                <table class="ttta">

                                    <tr>
                                        <td style="width: 30%" class="bbb">产品链接</td>
                                        <td><a href="<%# Eval("purl") %>" target="_blank">打开网址</a>&nbsp;&nbsp;
                                            <input type="text" value='<%# Eval("purl") %>' style="width: 20px" id="url<%# Container.ItemIndex+1 %>" />
                                            <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                         </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">SKU名称</td>
                                        <td><%# Eval("sku_name") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">浏览器ID</td>
                                        <td><%# Eval("bid") %></td>

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
                                        <td style="width: 30%" class="bbb">订单数量</td>
                                        <td><%# Eval("sku_amount") %></td>

                                    </tr>
                                    
                                    </tr>
                                   <tr>
                                        <td style="width: 30%" class="bbb">广告单数量</td>
                                        <td><%# Eval("conversions") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">需采购量</td> 
                                        <td>
                                            <%# CalculateRequiredQuantity(Eval("conversions"), Eval("sku_amount")) %>
                                        </td>
                                    </tr>
                                           <tr>
                                            <td style="width: 30%" class="bbb">1688采购链接</td>
                                            <td>
                                                <a href='<%# Eval("Y_1688url") %>' target="_blank" class="anniu1">打开链接</a>
                                                <asp:TextBox 
                                                    ID="txtY_1688url" 
                                                    runat="server" 
                                                    Text='<%# Eval("Y_1688url") %>' 
                                                    Width="300px"
                                                    style="margin-left:5px; vertical-align:middle;">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                    
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688SKU1</td>
                                        <td>
                                            <asp:TextBox ID="txtY_1688sku1" runat="server" Text='<%# Bind("Y_1688sku1") %>' Width="400px" ></asp:TextBox>
                                        </td>
                                    </tr>
                                                                        
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688SKU2</td>
                                        <td>
                                            <asp:TextBox ID="txtY_1688sku2" runat="server" Text='<%# Bind("Y_1688sku2") %>' Width="400px"></asp:TextBox>
                                        </td>
                                    </tr>
                                                                        
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688SKU3</td>
                                        <td>
                                            <asp:TextBox ID="txtY_1688sku3" runat="server" Text='<%# Bind("Y_1688sku3") %>' Width="400px"></asp:TextBox>
                                        </td>
                                    </tr>
                                                                    
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688采购价</td>
                                        <td>
                                            <asp:TextBox ID="txtY_1688price" runat="server" Text='<%# Bind("Y_1688price") %>' Width="400px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <div style="display: flex; align-items: center; gap: 10px;">
                                                <asp:DropDownList 
                                                        ID="cbShifou" 
                                                        runat="server" 
                                                        CssClass="status-filter"
                                                        SelectedValue='<%# Eval("shi_fou") %>'>
                                                        <asp:ListItem Value="0">0-未处理</asp:ListItem>
                                                        <asp:ListItem Value="1">1-已下架原始链接</asp:ListItem>
                                                        <asp:ListItem Value="2">2-不能销售</asp:ListItem>
                                                        <asp:ListItem Value="3">3-已下单1688</asp:ListItem>
                                                        <asp:ListItem Value="4">4-已收货</asp:ListItem>
                                                        <asp:ListItem Value="5">5-需重新采购</asp:ListItem>
                                                        <asp:ListItem Value="6">6-已发仓</asp:ListItem>
                                                    </asp:DropDownList>
                                                <asp:LinkButton ID="btnzd" 
                                                    OnClientClick="JavaScript:return confirm('确定保存？');" 
                                                    runat="server" 
                                                    Text="保存" 
                                                    ForeColor="White" 
                                                    BackColor="Green" 
                                                    CssClass="butt" 
                                                    CommandName="qr" 
                                                    CommandArgument='<%# Eval("skuid") %>' />
                                            </div>
                                        </td>
                                    </tr>

                                </table>

                                

                            </td>

                        </tr>
                        <tr></tr>
                    </ItemTemplate>
                </asp:Repeater>
               
            </table>
            <div style="margin:10px 0; padding:10px; background:#f5f5f5;">
    <asp:Button ID="btnFirst" runat="server" Text="首页" OnClick="BtnFirst_Click" CssClass="pager-btn" />
    <asp:Button ID="btnPrev" runat="server" Text="上一页" OnClick="BtnPrev_Click" CssClass="pager-btn" />
    <span style="margin:0 15px; color:#333;">
        第 <asp:Literal ID="litCurrentPage" runat="server"></asp:Literal> 页/共 <asp:Literal ID="litTotalPages" runat="server"></asp:Literal> 页
    </span>
                    <!-- 新增跳转控件 -->
    <span style="margin-left:15px;">
        跳转至 
        <asp:TextBox ID="txtJumpPage" runat="server" Width="50px" Text="1"></asp:TextBox>
        <asp:Button ID="btnJump" runat="server" Text="GO" 
                    OnClick="BtnJump_Click" CssClass="pager-btn" />
    </span>
    <asp:Button ID="btnNext" runat="server" Text="下一页" OnClick="BtnNext_Click" CssClass="pager-btn" />
    <asp:Button ID="btnLast" runat="server" Text="末页" OnClick="BtnLast_Click" CssClass="pager-btn" />
    <span style="margin-left:20px; color:#666;">
        共 <asp:Literal ID="litTotalRecords" runat="server"></asp:Literal> 条记录
    </span>
</div>
        </div>
    </form>
</body>
</html>
