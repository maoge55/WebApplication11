<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="采购单_印尼_管理员_新.aspx.cs" Inherits="WebApplication11.cg.tb.采购单_印尼_管理员_新" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>采购单_印尼_管理员_新</title>
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
            width: 30%;
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

        .input-number {
            width: 80px;
            text-align: center;
        }

        .batch-area {
            background-color: #f8f9fa;
            padding: 15px;
            border: 1px solid #e9ecef;
            border-radius: 6px;
            margin-bottom: 10px;
        }

        .batch-content {
            display: flex;
            align-items: center;
            justify-content: space-between;
            flex-wrap: wrap;
            gap: 15px;
        }

        .batch-item {
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .batch-label {
            font-weight: 500;
            color: #495057;
            font-size: 14px;
        }

        .batch-dropdown {
            padding: 6px 12px;
            border: 1px solid #ced4da;
            border-radius: 4px;
            font-size: 14px;
            background-color: white;
        }

        .batch-dropdown:focus {
            border-color: #80bdff;
            outline: none;
            box-shadow: 0 0 0 2px rgba(0,123,255,0.25);
        }

        .batch-button {
            background-color: #007bff !important;
            border: none !important;
            border-radius: 4px !important;
            padding: 8px 16px !important;
            font-size: 14px !important;
            color: white !important;
            text-decoration: none !important;
            cursor: pointer;
        }

        .batch-button:hover {
            background-color: #0056b3 !important;
        }

        .checkbox-all {
            width: 18px;
            height: 18px;
            cursor: pointer;
        }

        /* 简单响应式支持 */
        @media (max-width: 768px) {
            .batch-content {
                flex-direction: column;
                align-items: stretch;
                gap: 10px;
            }
            
            .batch-item {
                justify-content: space-between;
            }
        }
    </style>
    <script>
        function checkAll(obj) {
            var items = document.querySelectorAll("input[id*='chkItem']");
            for (var i = 0; i < items.length; i++) {
                items[i].checked = obj.checked;
            }
        }

        function validateForm() {
            var sjbm = document.getElementById('<%= txtSJBM.ClientID %>').value.trim();
            if (sjbm === '') {
                alert('商家编码为必填项！');
                return false;
            }
            return true;
        }

        function confirmGenerate() {
            return confirm('确定生成采购单吗？此操作将更新数据库！');
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h3>当前页面【<span style="color: #37cbc5">采购单_印尼_管理员_新</span>】</h3>
            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>
        
        <!-- 筛选区域 -->
        <div>
            商家编码（必填）：
            <asp:TextBox ID="txtSJBM" runat="server" Width="150px"></asp:TextBox>
            <asp:RequiredFieldValidator 
                ID="rfvSJBM" 
                runat="server" 
                ControlToValidate="txtSJBM"
                ErrorMessage="* 必须输入商家编码"
                ForeColor="Red"
                Display="Dynamic"
                ValidationGroup="searchGroup">
            </asp:RequiredFieldValidator>
            &nbsp;&nbsp;
            浏览器名称（选填）：
            <asp:TextBox ID="txtBName" runat="server" Width="150px"></asp:TextBox>
            &nbsp;&nbsp;
            <asp:Button ID="btnSearch" runat="server" Text="查找" BackColor="Red" ForeColor="White" 
                OnClick="btnSearch_Click" ValidationGroup="searchGroup" OnClientClick="return validateForm();" />
            &nbsp;&nbsp;
            <asp:Button ID="btnGenerateAll" runat="server" Text="生成采购单" BackColor="Green" ForeColor="White" 
                OnClick="btnGenerateAll_Click" OnClientClick="return confirmGenerate();" />
            <br />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </div>
        
        <br />
   
        
        <!-- 表格数据 -->
        <table class="ttt">
            <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                <HeaderTemplate>
                    <!-- 批量操作区域 -->
                    <tr>
                        <td style="width: 3%; text-align: center; vertical-align: middle;">
                            <input type="checkbox" id="chkAll" onclick="checkAll(this)" class="checkbox-all" />
                        </td>
                        <td colspan="2" class="batch-area">
                            <div class="batch-content">
                                <div class="batch-item">
                                    <span class="batch-label">采购单状态：</span>
                                    <asp:DropDownList ID="ddlBatchStatus" runat="server" CssClass="batch-dropdown">
                                        <asp:ListItem Value="需采购">需采购</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                
                                <div class="batch-item">
                                    <span class="batch-label">运营编码：</span>
                                    <asp:DropDownList ID="ddlBatchYYBM" runat="server" CssClass="batch-dropdown">
                                        <asp:ListItem Value="">请选择</asp:ListItem>
                                        <asp:ListItem Value="id8897">id8897</asp:ListItem>
                                        <asp:ListItem Value="th8888">th8888</asp:ListItem>
                                        <asp:ListItem Value="cym789">cym789</asp:ListItem>
                                        <asp:ListItem Value="wzm123">wzm123</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                
                                <div class="batch-item">
                                    <asp:LinkButton 
                                        ID="btnQuickApply" 
                                        runat="server" 
                                        CommandName="QuickApply" 
                                        Text="快速应用" 
                                        CssClass="batch-button"
                                        OnClientClick="return confirm('确定应用到勾选的行吗？');" />
                                </div>
                            </div>
                        </td>
                    </tr>
                    
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width: 3%; text-align: center">
                            <asp:CheckBox ID="chkItem" runat="server" />
                        </td>
                        <td style="width: 4%; text-align: center">
                            <%# Container.ItemIndex + 1 %>
                        </td>
                        <td style="width: 25%; text-align: center">
                            <img src="<%# Eval("pimage") %>" style="width: 300px; height: 300px;"/>
                        </td>
                        <td style="width: 68%">
                            <table class="ttta">
                                <tr>
                                    <td class="bbb">浏览器名称</td>
                                    <td><%# Eval("BName") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">商家编码</td>
                                    <td><%# Eval("SJBM") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">产品标题_shopee_印尼</td>
                                    <td><%# Eval("pname") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">产品标题_1688</td>
                                    <td><%# Eval("pname_1688") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">ItemID_印尼</td>
                                    <td><%# Eval("ItemID") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">SKUID_印尼</td>
                                    <td><%# Eval("SKUID") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">系统编码_海仓_印尼</td>
                                    <td><%# Eval("haiwaicangxitongbianma") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">3个月广告订单数量</td>
                                    <td><%# Eval("conversions") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">3个月广告ROARS</td>
                                    <td><%# String.Format("{0:F2}", Eval("ROAS")) %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">SKU7天总销量</td>
                                    <td><%# Eval("SKU7dayamount_id") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">SKU14天总销量</td>
                                    <td><%# Eval("SKU14dayamount_id") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">SKU28天总销量</td>
                                    <td><%# Eval("SKU28dayamount_id") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">SkuID在途在仓备货量_自动计算</td>
                                    <td><%# String.Format("{0:F0}", Eval("auto_stock_quantity")) %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">SkuID最低在途在仓备货量_人工限制</td>
                                    <td>
                                        <asp:TextBox ID="txtMinStock" runat="server" 
                                            Text='<%# Eval("sku_stock_min_transit_warehouse") %>' 
                                            CssClass="input-number" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">SkuID最高在途在仓备货量_人工限制</td>
                                    <td>
                                        <asp:TextBox ID="txtMaxStock" runat="server" 
                                            Text='<%# Eval("sku_stock_max_transit_warehouse") %>' 
                                            CssClass="input-number" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">在途_采购单数量_印尼</td>
                                    <td><%# Eval("transit_purchase_quantity") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">在途_头程物流发出数量_印尼</td>
                                    <td><%# Eval("transit_logistics_quantity") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">在仓_库存数量_印尼</td>
                                    <td><%# Eval("stock") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">offerid_1688</td>
                                    <td><%# Eval("offerid_1688") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">skuid_1688</td>
                                    <td><%# Eval("skuid_1688") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">sku1_1688</td>
                                    <td><%# Eval("sku1_1688") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">sku2_1688</td>
                                    <td><%# Eval("sku2_1688") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">价格_1688</td>
                                    <td><%# Eval("jiage_1688") %></td>
                                </tr>
                                <tr style="background-color: #ffffcc;">
                                    <td class="bbb">该SKU需采购数量</td>
                                    <td>
                                        <asp:TextBox ID="txtPurchaseQuantity" runat="server" 
                                            Text='<%# String.Format("{0:F0}", Eval("xucaigoushuliang")) %>' 
                                            CssClass="input-number" 
                                            BackColor="LightYellow" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">运营编码</td>
                                    <td>
                                        <asp:DropDownList ID="ddlYYBM" runat="server" 
                                            SelectedValue='<%# Eval("YYBM") %>'>
                                            <asp:ListItem Value="">请选择</asp:ListItem>
                                            <asp:ListItem Value="id8897">id8897</asp:ListItem>
                                            <asp:ListItem Value="th8888">th8888</asp:ListItem>
                                            <asp:ListItem Value="cym789">cym789</asp:ListItem>
                                            <asp:ListItem Value="wzm123">wzm123</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">采购单号</td>
                                    <td><%# Eval("caigoudanhao") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">采购单状态</td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" 
                                            SelectedValue='<%# Eval("caigoudanzhuangtai") %>'>
                                            <asp:ListItem Value="需采购">需采购</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center; padding: 10px;">
                                        <!-- 隐藏字段存储需要的数据 -->
                                        <asp:HiddenField ID="hidSkuid" runat="server" Value='<%# Eval("SKUID") %>' />
                                        <asp:HiddenField ID="hidCaigoudanhao" runat="server" Value='<%# Eval("caigoudanhao") %>' />
                                        <asp:HiddenField ID="hidOfferid1688" runat="server" Value='<%# Eval("offerid_1688") %>' />
                                        <asp:HiddenField ID="hidSkuid1688" runat="server" Value='<%# Eval("skuid_1688") %>' />
                                        <asp:HiddenField ID="hidSku11688" runat="server" Value='<%# Eval("sku1_1688") %>' />
                                        <asp:HiddenField ID="hidSku21688" runat="server" Value='<%# Eval("sku2_1688") %>' />
                                        <asp:HiddenField ID="hidJiage1688" runat="server" Value='<%# Eval("jiage_1688") %>' />
                                        
                                        <asp:Button ID="btnSave" runat="server" 
                                            Text="保存" 
                                            CommandName="Save" 
                                            CommandArgument='<%# Eval("SKUID") %>' 
                                            BackColor="Blue" 
                                            ForeColor="White" 
                                            OnClientClick="return confirm('确定保存此项的修改吗？');" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnGenerate" runat="server" 
                                            Text="生成采购单" 
                                            CommandName="Generate" 
                                            CommandArgument='<%# Eval("SKUID") %>' 
                                            BackColor="Green" 
                                            ForeColor="White" 
                                            OnClientClick="return confirm('确定生成此项的采购单吗？');" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        
        <br />
        <!-- 分页信息 -->
        <div>
            <asp:Literal ID="litPageInfo" runat="server"></asp:Literal>
            &nbsp;&nbsp;
            <asp:Button ID="btnPrev" runat="server" Text="上一页" CssClass="pager-btn" OnClick="btnPrev_Click" />
            <asp:Button ID="btnNext" runat="server" Text="下一页" CssClass="pager-btn" OnClick="btnNext_Click" />
            &nbsp;&nbsp;
            跳转到第 <asp:TextBox ID="txtJumpPage" runat="server" Width="50px" CssClass="input-number"></asp:TextBox> 页
            <asp:Button ID="btnJump" runat="server" Text="跳转" CssClass="pager-btn" OnClick="btnJump_Click" />
        </div>
    </form>
</body>
</html>