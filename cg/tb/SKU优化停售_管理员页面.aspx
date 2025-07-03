<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SKU优化停售_管理员页面.aspx.cs" Inherits="WebApplication11.cg.tb.SKU优化停售_管理员页面" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>SKU优化停售_管理员页面</title>
    <style>
        .ttt {
            width: 100%;
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

        .filter-section {
            padding: 15px 15px 10px 15px;
            margin-bottom: 0;
            border-bottom: 1px solid #eee;
        }

        .filter-row {
            display: flex;
            align-items: center;
            gap: 15px;
            margin-bottom: 5px;
            flex-wrap: wrap;
        }

        .filter-item {
            display: flex;
            align-items: center;
            gap: 5px;
            height: 30px;
        }

        .filter-item label {
            font-weight: bold;
            min-width: 80px;
            line-height: 30px;
            margin: 0;
        }

        .filter-item input,
        .filter-item select {
            height: 24px;
            line-height: 24px;
            vertical-align: middle;
            margin: 0;
        }

        .required {
            color: red;
            vertical-align: middle;
        }

        .filter-item input[type="submit"],
        .quick-action input[type="submit"] {
            height: 24px;
            line-height: 20px;
            padding: 0 15px;
            vertical-align: middle;
            cursor: pointer;
        }

        .quick-action {
            padding: 10px 15px;
            border-bottom: 1px solid #eee;
        }

        .batch-controls {
            background-color: #f0f8ff;
            padding: 12px;
            border-radius: 5px;
            border: 1px solid #b3d9ff;
        }

        .batch-title {
            font-weight: bold;
            color: #0066cc;
            margin-bottom: 8px;
        }

        .sales-status-select {
            padding: 4px 8px;
            border-radius: 3px;
            border: 1px solid #ccc;
        }

        .pager-container {
            margin-top: 20px;
            text-align: center;
        }

        .pager-btn {
            margin: 0 5px;
            padding: 5px 15px;
        }
    </style>
    <script>
        // 全选功能
        function checkAll(obj) {
            var items = document.querySelectorAll("input[id*='chkItem']");
            for (var i = 0; i < items.length; i++) {
                items[i].checked = obj.checked;
            }
        }

        // 验证单行保存
        function validateSingleSave(button) {
            var currentRow = button.closest('tr').parentNode.parentNode;
            var ddlSalesStatus = currentRow.querySelector("select[id*='ddlSalesStatus']");
            
            if (!ddlSalesStatus) {
                alert('找不到销售状态下拉框');
                return false;
            }
            
            if (ddlSalesStatus.value === "") {
                alert('请选择销售状态');
                ddlSalesStatus.focus();
                return false;
            }
            
            return confirm('确定保存？');
        }

        // 批量设置销售状态
        function applyBatchSalesStatus() {
            var batchStatusSelect = document.getElementById('ddlBatchSalesStatus');
            
            if (batchStatusSelect.value === "") {
                alert('请先选择要设置的销售状态');
                return false;
            }
            
            var selectedItems = document.querySelectorAll("input[id*='chkItem']:checked");
            if (selectedItems.length === 0) {
                alert('请先选择要修改的行');
                return false;
            }
            
            if (!confirm('确定要应用销售状态到选中的 ' + selectedItems.length + ' 行吗？')) {
                return false;
            }
            
            var hiddenField = document.querySelector("input[id*='hdnBatchSalesStatus']");
            if (hiddenField) {
                hiddenField.value = batchStatusSelect.value;
            }
            
            return true;
        }

        // 验证快速停售
        function validateQuickStop() {
            var skuid = document.getElementById('<%= txtQuickStopSKUID.ClientID %>').value.trim();
            if (!skuid) {
                alert('请输入要停售的SKUID');
                return false;
            }
            return confirm('确定要停售SKUID: ' + skuid + ' 吗？');
        }

        // 验证必填项和SKU数量
        function validateFilters() {
            var sjbm = document.getElementById('<%= txtSJBM.ClientID %>');
            if (!sjbm.value.trim()) {
                alert('商家编码为必填项');
                sjbm.focus();
                return false;
            }

            var skuCount = document.getElementById('<%= txtSkuCount.ClientID %>');
            if (skuCount.value.trim()) {
                if (!/^\d+$/.test(skuCount.value.trim())) {
                    alert('SKU数量必须是非负整数');
                    skuCount.focus();
                    return false;
                }
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h3>当前页面【<span style="color: #37cbc5">SKU优化停售_管理员页面</span>】</h3>
            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>

        <!-- 筛选条件区域 -->
        <div class="filter-section">
            <div class="filter-row">
                <div class="filter-item">
                    <label><span class="required">*</span>商家编码：</label>
                    <asp:TextBox ID="txtSJBM" runat="server" Width="150px" placeholder="必填"></asp:TextBox>
                </div>
                
                <div class="filter-item">
                    <label>销售状态：</label>
                    <asp:DropDownList ID="ddlSalesStatusFilter" runat="server">
                        <asp:ListItem Value="未处理">未处理</asp:ListItem>
                        <asp:ListItem Value="在售">在售</asp:ListItem>
                        <asp:ListItem Value="停售">停售</asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="filter-item">
                    <label>SKU数量大于：</label>
                    <asp:TextBox ID="txtSkuCount" runat="server" Width="80px" placeholder="非负数"></asp:TextBox>
                </div>

                <div class="filter-item">
                    <label>浏览器店铺名：</label>
                    <asp:TextBox ID="txtBName" runat="server" Width="150px" placeholder="模糊匹配"></asp:TextBox>
                </div>
                
                <div class="filter-item">
                    <label>排序方式：</label>
                    <asp:DropDownList ID="ddlSortType" runat="server">
                        <asp:ListItem Value="sku_sales7_desc">SKU7天总销量降序</asp:ListItem>
                        <asp:ListItem Value="sku_sales14_desc">SKU14天总销量降序</asp:ListItem>
                        <asp:ListItem Value="sku_sales28_desc">SKU28天总销量降序</asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="filter-item">
                    <asp:Button ID="btnSearch" runat="server"
                        Text="查找"
                        BackColor="Red"
                        ForeColor="White"
                        OnClick="btnSearch_Click"
                        OnClientClick="return validateFilters();" />
                </div>
            </div>
        </div>

        <!-- 快速停售区域 -->
        <div class="quick-action">
            <div class="filter-row">
                <div class="filter-item">
                    <label>SKUID：</label>
                    <asp:TextBox ID="txtQuickStopSKUID" runat="server" Width="150px" placeholder="输入要停售的SKUID"></asp:TextBox>
                    <asp:Button ID="btnQuickStop" runat="server"
                        Text="停售该SKUID"
                        BackColor="Yellow"
                        ForeColor="Black"
                        OnClick="btnQuickStop_Click"
                        OnClientClick="return validateQuickStop();" />
                </div>
            </div>
        </div>

        <br />

        <table class="ttt">
            <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                <HeaderTemplate>
                    <tr>
                        <td style="width: 3%">
                            <input type="checkbox" id="chkAll" onclick="checkAll(this)" />
                        </td>
                        <td colspan="3">
                            <div class="batch-controls">
                                <div class="batch-title">批量设置销售状态：</div>
                                <div style="display: flex; align-items: center; gap: 20px;">
                                    <div style="display: flex; align-items: center; gap: 10px;">
                                        <label>销售状态：</label>
                                        <select id="ddlBatchSalesStatus" class="sales-status-select">
                                            <option value="">请选择</option>
                                            <option value="未处理">未处理</option>
                                            <option value="在售">在售</option>
                                            <option value="停售">停售</option>
                                        </select>
                                    </div>
                                    <asp:HiddenField ID="hdnBatchSalesStatus" runat="server" />
                                    <asp:LinkButton ID="btnApplyBatch" runat="server"
                                        CommandName="ApplyBatch"
                                        Text="应用到选中行"
                                        OnClientClick="return applyBatchSalesStatus();"
                                        CssClass="butt"
                                        style="background-color: #007bff; color: white; padding: 6px 12px; border-radius: 4px; text-decoration: none;" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width: 3%">
                            <asp:CheckBox ID="chkItem" runat="server" />
                        </td>
                        <asp:HiddenField ID="ItemID" runat="server" Value='<%# Eval("itemid") %>' />
                        <asp:HiddenField ID="SKUID" runat="server" Value='<%# Eval("skuid") %>' />
                        <td style="width: 4%; text-align: center">
                            <%# Container.ItemIndex+1 %><br />
                        </td>
                        <td style="width: 30%; text-align: center">
                            <img src='<%# Eval("pimage")%>' style="width: 300px" />
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
                                    <td style="width: 30%" class="bbb">印尼shopee产品标题</td>
                                    <td><%# Eval("pname") %></td>
                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">1688产品标题</td>
                                    <td><%# Eval("pname_1688") %></td>
                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">ItemID</td>
                                    <td><%# Eval("itemid") %></td>
                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">SKUID</td>
                                    <td><%# Eval("skuid") %></td>
                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">3个月广告订单数量</td>
                                    <td><%# Eval("conversions") %></td>
                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">3个月广告ROARS</td>
                                    <td><%# Eval("ROAS") %></td>
                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">SKU7天总销量</td>
                                    <td><%# Eval("sales_7days") %></td>
                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">SKU14天总销量</td>
                                    <td><%# Eval("sales_14days") %></td>
                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">SKU28天总销量</td>
                                    <td><%# Eval("sales_28days") %></td>
                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">销售状态</td>
                                    <td>
                                        <asp:DropDownList ID="ddlSalesStatus" runat="server" 
                                            SelectedValue='<%# Eval("xiaoshouzhuangtai") %>' CssClass="sales-status-select">
                                            <asp:ListItem Value="未处理">未处理</asp:ListItem>
                                            <asp:ListItem Value="在售">在售</asp:ListItem>
                                            <asp:ListItem Value="停售">停售</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:LinkButton ID="btnSave"
                                            OnClientClick="return validateSingleSave(this);"
                                            runat="server"
                                            Text="保存"
                                            ForeColor="White"
                                            BackColor="Green"
                                            CssClass="butt"
                                            CommandName="Save"
                                            CommandArgument='<%# Eval("itemid") + "|" + Eval("skuid") %>' />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr></tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

        <div class="pager-container">
            <asp:Button ID="btnPrev" runat="server" Text="上一页" OnClick="btnPrev_Click" CssClass="pager-btn" />
            <span style="margin: 0 10px">跳转至 
                <asp:TextBox ID="txtJumpPage" runat="server" Width="50px" Text="1"></asp:TextBox>
                <asp:Button ID="btnJump" runat="server" Text="GO" OnClick="btnJump_Click" CssClass="pager-btn" />
            </span>
            <span style="margin: 0 10px">
                <asp:Literal ID="litCurrentPage" runat="server" />
                /
                <asp:Literal ID="litTotalPages" runat="server" />
            </span>
            <asp:Button ID="btnNext" runat="server" Text="下一页" OnClick="btnNext_Click" CssClass="pager-btn" />
        </div>
    </form>
</body>
</html> 