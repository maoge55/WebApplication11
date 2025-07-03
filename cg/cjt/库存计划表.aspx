<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="库存计划表.aspx.cs" Inherits="WebApplication11.cg.cjt.库存计划表" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>库存计划表</title>
    <style>
        .ttt {
            width: 100%;
            border-collapse: collapse;
        }
        .ttt tr td {
            border: 1px solid #000000;
            padding: 5px;
        }
        .ttta {
            width: 100%;
            border-collapse: collapse;
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
        .pager-container {
            text-align: center;
            margin: 20px 0;
            padding: 15px;
            background-color: #f8f9fa;
            border-top: 1px solid #e9ecef;
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
        .status-filter {
            padding: 3px;
        }
    </style>
    <script>
        function checkAll(obj) {
            var items = document.querySelectorAll("input[id*='chkItem']");
            for (var i = 0; i < items.length; i++) {
                items[i].checked = obj.checked;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h3>当前页面【<span style="color: #37cbc5">库存计划表</span>】</h3>
            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>
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
            <%-- 新增排序下拉框 --%>
            排序方式：
            <asp:DropDownList ID="ddlSort" runat="server">
                <asp:ListItem Value="DESC" Selected="True">数量高到低</asp:ListItem>
                <asp:ListItem Value="ASC">数量低到高</asp:ListItem>
            </asp:DropDownList>
            &nbsp;
            销售状态：
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="status-filter">
                <asp:ListItem Value="在售">在售</asp:ListItem>
                <asp:ListItem Value="-1">全部状态</asp:ListItem>
                <asp:ListItem Value="未处理">未处理</asp:ListItem>
                <asp:ListItem Value="临时停止销售">临时停止销售</asp:ListItem>
                <asp:ListItem Value="永久停止销售">永久停止销售</asp:ListItem>
            </asp:DropDownList>
            浏览器店铺名：
            <asp:TextBox ID="txtBName" runat="server" ValidationGroup="searchGroup"></asp:TextBox>
            &nbsp;
            <asp:Button ID="btnSearch" runat="server" 
                Text="查找" 
                BackColor="Red" 
                ForeColor="White" 
                OnClick="btnSearch_Click" 
                ValidationGroup="searchGroup" />
            <asp:Button ID="btnSaveAll" runat="server" 
                ForeColor="Blue" 
                OnClientClick="return confirm('确定保存整页吗？');" 
                Text="保存整页" 
                OnClick="btnSaveAll_Click" />
            <br />
            <asp:Literal ID="litPageInfo" runat="server"></asp:Literal>
        </div>
        <br />
        <table class="ttt">
            <asp:Repeater ID="rptItems" runat="server" OnItemCommand="rptItems_ItemCommand">
                <HeaderTemplate>
                    <tr>
                        <td style="width:3%">
                            <input type="checkbox" id="chkAll" onclick="checkAll(this)" />
                        </td>
                        <td colspan="4">批量操作</td>
                        <td>
                            <asp:DropDownList ID="ddlBatchStatus" runat="server">
                                <asp:ListItem Value="">请选择状态</asp:ListItem>
                                <asp:ListItem Value="在售">在售</asp:ListItem>
                                <asp:ListItem Value="未处理">未处理</asp:ListItem>
                                <asp:ListItem Value="临时停止销售">临时停止销售</asp:ListItem>
                                <asp:ListItem Value="永久停止销售">永久停止销售</asp:ListItem>
                            </asp:DropDownList>
                            <asp:LinkButton ID="btnApplyBatch" runat="server" 
                                CommandName="ApplyBatch" 
                                Text="应用状态" 
                                OnClientClick="return confirm('确定应用状态到选中行吗？');" 
                                CssClass="butt" />
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width:3%">
                            <asp:CheckBox ID="chkItem" runat="server" />
                        </td>
                        <asp:HiddenField ID="hdnItemID" runat="server" Value='<%# Eval("ItemID") %>' />
                        <td style="width: 4%; text-align: center">
                            <%# Container.ItemIndex + 1 %><br />
                        </td>
                        <td style="width: 30%; text-align: center">
                            <img src='<%# Eval("pimage") %>' style="width:300px" /> 
                        </td>
                        <td>
                            <table class="ttta">
                                <tr>
                                    <td style="width: 30%" class="bbb">浏览器店铺名</td>
                                    <td><%# Eval("BName") %></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">商家编码</td>
                                    <td><%# Eval("SJBM") %></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">销售链接产品标题</td>
                                    <td><%# Eval("pname") %></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">货源链接产品标题</td>
                                    <td><%# Eval("huopinbiaoti") %></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">销售链接ItemID</td>
                                    <td><%# Eval("ItemID") %></td>
                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">3个月广告订单数量</td>
                                    <td><%# Eval("conversions") %></td>
                                </tr>                             
                                <tr>
                                    <td style="width: 30%" class="bbb">3个月广告ROARS</td>
                                    <td><%# FormatROAS(Eval("ROAS")) %></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">该ItemID7天总销量</td>
                                    <td><%# Eval("day_sales_7") %></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">该ItemID14天总销量</td>
                                    <td><%# Eval("day_sales_14") %></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">该ItemID30天总销量</td>
                                    <td><%# Eval("day_sales_30") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">该ITEMID需维持在仓在途数量</td>
                                    <td>
                                        <!-- 优先显示已保存的 ItemIDxuweichi，不存在时显示计算值 -->
                                            <%# string.Format("{0:0}", GetDisplayMaintainQuantity(Eval("ItemIDxuweichi"), Eval("MaintainQuantity"))) %>
                                        <asp:HiddenField ID="hdnMaintainQty" runat="server" 
                                             Value='<%# GetActualMaintainQuantity(Eval("ItemIDxuweichi"), Eval("MaintainQuantity")) %>' />
                                        <asp:TextBox ID="txtMaintainQuantity" runat="server" 
                                             Text='<%# GetTextBoxValue(Eval("ItemIDxuweichi")) %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">销售状态</td>
                                    <td>
                                        <asp:DropDownList ID="ddlSaleStatus" runat="server" 
                                            CssClass="status-filter"
                                            SelectedValue='<%# Eval("xiaoshouzhuangtai") %>'>
                                            <asp:ListItem Value="">请选择</asp:ListItem>
                                            <asp:ListItem Value="在售">在售</asp:ListItem>
                                            <asp:ListItem Value="未处理">未处理</asp:ListItem>
                                            <asp:ListItem Value="临时停止销售">临时停止销售</asp:ListItem>
                                            <asp:ListItem Value="永久停止销售">永久停止销售</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:LinkButton ID="btnSave" runat="server" 
                                            Text="保存" 
                                            ForeColor="White" 
                                            BackColor="Green" 
                                            CssClass="butt" 
                                            CommandName="SaveItem" 
                                            CommandArgument='<%# Eval("ItemID") %>' 
                                            OnClientClick="return confirm('确定保存？');" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <div class="pager-container">
            <asp:Button ID="btnPrev" runat="server" Text="上一页" OnClick="btnPrev_Click" CssClass="pager-btn" />
                                <!-- 新增跳转控件 -->
    <!-- 新增跳转控件 -->
    <span style="margin-left:15px;">
        跳转至 
        <asp:TextBox ID="txtJumpPage" runat="server" Width="50px" Text="1"></asp:TextBox>
        <asp:Button ID="btnJump" runat="server" Text="GO" 
                    OnClick="BtnJump_Click" CssClass="pager-btn" />
    </span>
            <span style="margin:0 10px">
                <asp:Literal ID="litCurrentPage" runat="server" /> / 
                <asp:Literal ID="litTotalPages" runat="server" />
            </span>
            <asp:Button ID="btnNext" runat="server" Text="下一页" OnClick="btnNext_Click" CssClass="pager-btn" />
        </div>
    </form>
</body>
</html>