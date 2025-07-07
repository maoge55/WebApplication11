<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="采购单_印尼_采购员_新.aspx.cs" Inherits="WebApplication11.cg.tb.采购单_印尼_采购员_新" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>采购单_印尼_采购员_新</title>
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

        .anniu1 {
            border: 1px red solid;
        }

        /* 输入框和下拉框样式 */
        .input-number {
            width: 60px;
            text-align: center;
            padding: 2px;
        }

        .batch-dropdown {
            padding: 3px;
            margin: 0 5px;
        }

        /* 统一所有输入框和下拉框的高度 */
        input[type="text"], 
        select, 
        .input-number, 
        .batch-dropdown {
            height: 30px;
            padding: 4px;
            box-sizing: border-box;
            vertical-align: middle;
        }

        /* 单行输入框样式（排除多行文本框） */
        .filter-input:not(textarea) {
            height: 30px;
            padding: 4px;
            box-sizing: border-box;
            vertical-align: middle;
        }

        /* 备注输入框保持原样 */
        textarea {
            height: auto !important;
            padding: 4px;
            box-sizing: border-box;
        }

        /* 批量操作区域样式 */
        .batch-content {
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .batch-item {
            display: flex;
            align-items: center;
            gap: 5px;
        }

        .batch-button {
            padding: 5px 15px;
            background-color: #2196F3;
            color: white;
            border: none;
            border-radius: 3px;
            cursor: pointer;
            font-size: 14px;
        }

        .batch-button:hover {
            background-color: #1976D2;
        }

        /* 分页按钮样式 */
        .pager-btn {
            padding: 3px 8px;
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
        function checkAll(obj) {
            var items = document.querySelectorAll("input[id*='chkItem']");
            for (var i = 0; i < items.length; i++) {
                items[i].checked = obj.checked;
            }
        }

        function validateForm() {
            var yybm = document.getElementById('<%= txtYYBM.ClientID %>').value.trim();
            if (yybm === '') {
                alert('运营编码为必填项！');
                return false;
            }
            return true;
        }

        function confirmSave() {
            return confirm('确定保存当前页面所有数据吗？');
        }

        function applyBatchStatus() {
            // 直接通过名称查找批量状态下拉框（在Repeater的HeaderTemplate中）
            var batchStatusDropdown = document.querySelector("select[id*='ddlBatchStatus']");
            if (!batchStatusDropdown) {
                alert('找不到批量状态选择框！');
                return false;
            }
            
            var batchStatus = batchStatusDropdown.value;
            if (batchStatus === '') {
                alert('请选择要批量应用的采购单状态！');
                return false;
            }
            
            var checkedItems = document.querySelectorAll("input[id*='chkItem']:checked");
            if (checkedItems.length === 0) {
                alert('请至少选择一行进行批量操作！');
                return false;
            }
            
            // 获取所有被选中行的状态下拉框
            checkedItems.forEach(function(checkbox) {
                var row = checkbox.closest('tr');
                var statusDropdown = row.querySelector("select[id*='ddlStatus']");
                if (statusDropdown) {
                    statusDropdown.value = batchStatus;
                }
            });
            
            alert('批量状态已应用到选中行，请点击保存按钮保存到数据库！');
            return false; // 防止提交
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h3>当前页面【<span style="color: #37cbc5">采购单_印尼_采购员_新</span>】</h3>
            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>
        
        <!-- 筛选区域 -->
        <div>
            运营编码<span style="color: red;">*</span>：
            <asp:TextBox ID="txtYYBM" runat="server" ValidationGroup="searchGroup"></asp:TextBox>
            <asp:RequiredFieldValidator 
                ID="rfvYYBM" 
                runat="server" 
                ControlToValidate="txtYYBM"
                ErrorMessage="* 必须输入运营编码"
                ForeColor="Red"
                Display="Dynamic"
                ValidationGroup="searchGroup">
            </asp:RequiredFieldValidator>
            采购单状态：
            <asp:DropDownList ID="ddlSearchStatus" runat="server">
                <asp:ListItem Value="需采购" Selected="True">需采购</asp:ListItem>
                <asp:ListItem Value="完成采购">完成采购</asp:ListItem>
                <asp:ListItem Value="放弃采购">放弃采购</asp:ListItem>
                <asp:ListItem Value="">全部</asp:ListItem>
            </asp:DropDownList>
            &nbsp;
            1688订单号：
            <asp:TextBox ID="txt1688OrderNo" runat="server" Width="150px"></asp:TextBox>
            &nbsp;
            <asp:Button ID="btnSearch" runat="server" Text="查找" 
                BackColor="Red" 
                ForeColor="White"
                OnClick="btnSearch_Click" ValidationGroup="searchGroup" OnClientClick="return validateForm();" />

            <asp:Button ID="btnSaveAll" runat="server" Text="保存整页" 
                BackColor="Blue" 
                ForeColor="White"
                OnClick="btnSaveAll_Click" OnClientClick="return confirmSave();" />

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
                        <td style="width: 2%; text-align: center; vertical-align: middle;">
                            <input type="checkbox" id="chkAll" onclick="checkAll(this)" class="checkbox-all" />
                        </td>
                        <td colspan="2" class="batch-area">
                            <div class="batch-content">
                                <div class="batch-item">
                                    <span class="batch-label">采购单状态：</span>
                                    <asp:DropDownList ID="ddlBatchStatus" runat="server" CssClass="batch-dropdown">
                                        <asp:ListItem Value="">请选择</asp:ListItem>
                                        <asp:ListItem Value="需采购">需采购</asp:ListItem>
                                        <asp:ListItem Value="完成采购">完成采购</asp:ListItem>
                                        <asp:ListItem Value="放弃采购">放弃采购</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                
                                <div class="batch-item">
                                    <button type="button" onclick="applyBatchStatus()" class="batch-button">
                                        快速应用
                                    </button>
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
                            <img src="<%# Eval("sku_img") %>" style="width: 300px; height: 300px;""/>
                        </td>
                        <td style="width: 68%">
                            <!-- 隐藏字段存储关键信息 -->
                            <asp:HiddenField ID="hidCaigoudanhao" runat="server" Value='<%# Eval("caigoudanhao") %>' />
                            <asp:HiddenField ID="hidSKUID_ID" runat="server" Value='<%# Eval("SKUID_ID") %>' />
                            <table class="ttta">
                                <tr>
                                    <td class="bbb">采购单单号</td>
                                    <td><%# Eval("caigoudanhao") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">ItemID_印尼</td>
                                    <td><%# Eval("ItemID_ID") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">SKUID_印尼</td>
                                    <td><%# Eval("SKUID_ID") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">offerid_1688</td>
                                    <td><%# Eval("offerid") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">skuid_1688</td>
                                    <td><%# Eval("skuid") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">1688采购链接</td>
                                    <td>
                                        <a href="https://detail.1688.com/offer/<%# Eval("offerid") %>.html" target="_blank">
                                            https://detail.1688.com/offer/<%# Eval("offerid") %>.html
                                        </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">1688SKU1</td>
                                    <td><%# Eval("sku1") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">1688SKU2</td>
                                    <td><%# Eval("sku2") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">1688价格</td>
                                    <td><%# String.Format("{0:F2}", Eval("danjia")) %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">需采购数量</td>
                                    <td><%# Eval("xucaigoushuliang") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">实际采购数量</td>
                                    <td>
                                        <asp:TextBox ID="txtShijicaigoushuliang" runat="server" 
                                            Text='<%# Eval("shijicaigoushuliang") %>' 
                                            CssClass="input-number" />
                                        <span style="color: red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">采购单状态</td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="batch-dropdown" SelectedValue='<%# Eval("status").ToString() ?? "需采购" %>'>
                                            <asp:ListItem Value="需采购">需采购</asp:ListItem>
                                            <asp:ListItem Value="完成采购">完成采购</asp:ListItem>
                                            <asp:ListItem Value="放弃采购">放弃采购</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">1688订单编号</td>
                                    <td>
                                        <asp:TextBox ID="txtDingDanBianHao" runat="server" 
                                            Text='<%# Eval("DingDanBianHao") %>' 
                                            CssClass="filter-input" Width="200px" />
                                        <span style="color: red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">备注</td>
                                    <td>
                                        <asp:TextBox ID="txtBeizhu" runat="server" 
                                            Text='<%# Eval("beizhu") %>' 
                                            CssClass="filter-input" Width="600px" 
                                            TextMode="MultiLine" Rows="4" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb"></td>
                                    <td>
                                        <asp:Button ID="btnSaveItem" runat="server" 
                                            Text="保存" 
                                            BackColor="Green" 
                                            ForeColor="White"
                                            CommandName="SaveItem"
                                            OnClientClick="return confirm('确定保存吗？');" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
            </asp:Repeater>
        </table>
        
        <br />
        <!-- 分页信息 -->
        <div style="margin-bottom: 10px;">
            <asp:Literal ID="litPageInfo" runat="server"></asp:Literal>
            <asp:Button ID="btnPrev" runat="server" Text="上一页" CssClass="pager-btn" OnClick="btnPrev_Click" />
            <asp:Button ID="btnNext" runat="server" Text="下一页" CssClass="pager-btn" OnClick="btnNext_Click" />
            跳转到第 <asp:TextBox ID="txtJumpPage" runat="server" CssClass="input-number" Width="50px"></asp:TextBox> 页
            <asp:Button ID="btnJump" runat="server" Text="跳转" CssClass="pager-btn" OnClick="btnJump_Click" />
        </div>
    </form>
</body>
</html> 