<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="印尼出单数据_转_泰国广告测品.aspx.cs" Inherits="WebApplication11.cg.tb.印尼出单数据_转_泰国广告测品" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>印尼出单数据_转_泰国广告测品</title>
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

        .checkbox-group {
            display: flex;
            align-items: center;
            gap: 15px;
            padding: 8px;
            background-color: #f9f9f9;
            border-radius: 5px;
            border: 1px solid #ddd;
        }

        .checkbox-item {
            display: flex;
            align-items: center;
            gap: 5px;
            padding: 4px 8px;
            border-radius: 3px;
            transition: background-color 0.2s;
        }

        .checkbox-item:hover {
            background-color: #e8f4fd;
        }

        .checkbox-item input[type="checkbox"] {
            transform: scale(1.2);
            margin-right: 5px;
        }

        .checkbox-item span {
            font-weight: bold;
        }

        .checkbox-item.yes {
            color: #28a745;
            font-weight: bold;
        }

        .checkbox-item.no {
            color: #dc3545;
            font-weight: bold;
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
    </style>
    <script>
        // 复制URL函数保持不变
        function copyUrl(myurl) {
            var Url2 = document.getElementById(myurl);
            Url2.select();
            document.execCommand("Copy");
        }
        //全选功能
        function checkAll(obj) {
            var items = document.querySelectorAll("input[id*='chkItem']");
            for (var i = 0; i < items.length; i++) {
                items[i].checked = obj.checked;
            }
        }

        // 表头checkbox互斥控制
        function toggleBatchStatus(checkbox, isYes) {
            var batchYes = document.getElementById('batchYes');
            var batchNo = document.getElementById('batchNo');
            
            if (isYes) {
                if (checkbox.checked) {
                    batchNo.checked = false;
                }
            } else {
                if (checkbox.checked) {
                    batchYes.checked = false;
                }
            }
        }

        // 准备批量状态并提交
        function prepareBatchStatus() {
            var batchYes = document.getElementById('batchYes');
            var batchNo = document.getElementById('batchNo');
            
            if (!batchYes.checked && !batchNo.checked) {
                alert('请先选择要设置的状态（是或否）');
                return false;
            }
            
            var selectedItems = document.querySelectorAll("input[id*='chkItem']:checked");
            if (selectedItems.length === 0) {
                alert('请先选择要修改的行');
                return false;
            }
            
            if (!confirm('确定要应用状态到选中的 ' + selectedItems.length + ' 行吗？')) {
                return false;
            }
            
            // 将批量状态保存到隐藏字段
            var hiddenField = document.querySelector("input[id*='hdnBatchStatus']");
            if (hiddenField) {
                hiddenField.value = batchYes.checked ? "1" : "-1";
            }
            
            // 应用状态到前端显示（可选，用于视觉反馈）
            var isYes = batchYes.checked;
            selectedItems.forEach(function(item) {
                var row = item.closest('tr');
                var yesCheckbox = row.querySelector("input[id*='chkIsToThadYes']");
                var noCheckbox = row.querySelector("input[id*='chkIsToThadNo']");
                
                if (yesCheckbox && noCheckbox) {
                    if (isYes) {
                        yesCheckbox.checked = true;
                        noCheckbox.checked = false;
                    } else {
                        yesCheckbox.checked = false;
                        noCheckbox.checked = true;
                    }
                }
            });
            
            return true;
        }

        // 确保单行内两个checkbox互斥
        function toggleSingleAdStatus(checkbox, isYes) {
            var row = checkbox.closest('tr');
            var yesCheckbox = row.querySelector("input[id*='chkIsToThadYes']");
            var noCheckbox = row.querySelector("input[id*='chkIsToThadNo']");
            
            if (isYes) {
                if (checkbox.checked) {
                    noCheckbox.checked = false;
                }
            } else {
                if (checkbox.checked) {
                    yesCheckbox.checked = false;
                }
            }
        }

        // 验证单行保存时至少选择一个状态
        function validateSingleSave(button) {
            var row = button.closest('tr');
            var yesCheckbox = row.querySelector("input[id*='chkIsToThadYes']");
            var noCheckbox = row.querySelector("input[id*='chkIsToThadNo']");
            
            if (!yesCheckbox.checked && !noCheckbox.checked) {
                alert('请至少选择一个状态（是 或 否）');
                return false;
            }
            
            return confirm('确定保存？');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <div>
            <h3>当前页面【<span style="color: #37cbc5">印尼出单数据_转_泰国广告测品</span>】</h3>

            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
        </div>
        <div>
            泰国广告测品状态：
            <asp:DropDownList ID="ddlAdStatus" runat="server" CssClass="status-filter">
                <asp:ListItem Value="">全部</asp:ListItem>
                <asp:ListItem Value="1">可泰国广告测品</asp:ListItem>
                <asp:ListItem Value="-1">不可泰国广告测品</asp:ListItem>
                <asp:ListItem Value="0">未处理</asp:ListItem>
            </asp:DropDownList>

            <asp:Button ID="Button1" runat="server"
                Text="查找"
                BackColor="Red"
                ForeColor="White"
                OnClick="Button1_Click" />

            <br />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </div>

        <br />

        <table class="ttt">
            <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                <HeaderTemplate>
                    <tr>
                        <td style="width: 3%">
                            <input type="checkbox" id="chkAll" onclick="checkAll(this)" />
                        </td>
                        <td colspan="4">
                            <div class="batch-controls">
                                <div class="batch-title">批量设置泰国广告测品状态：</div>
                                <div style="display: flex; align-items: center; gap: 20px;">
                                    <div class="checkbox-group">
                                        <div class="checkbox-item yes">
                                            <input type="checkbox" id="batchYes" onchange="toggleBatchStatus(this, true)" />
                                            <label for="batchYes">是</label>
                                        </div>
                                        <div class="checkbox-item no">
                                            <input type="checkbox" id="batchNo" onchange="toggleBatchStatus(this, false)" />
                                            <label for="batchNo">否</label>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="hdnBatchStatus" runat="server" />
                                    <asp:LinkButton ID="btnApplyBatch" runat="server"
                                        CommandName="ApplyBatch"
                                        Text="应用到选中行"
                                        OnClientClick="return prepareBatchStatus();"
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
                        <td style="width: 4%; text-align: center">
                            <%# Container.ItemIndex+1 %><br />
                        </td>
                        <td style="width: 30%; text-align: center">
                            <img src='<%# Eval("pimage")%>' style="width: 300px" />
                        </td>
                        <td>
                            <table class="ttta">
                                <tr>
                                    <td style="width: 30%" class="bbb">产品链接</td>
                                    <td>
                                        <a href="<%# Eval("purl") %>" target="_blank">打开网址</a>&nbsp;&nbsp;
                                        <input type="text" value='<%# Eval("purl") %>' style="width: 20px" id="url<%# Container.ItemIndex+1 %>" />
                                        <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                    </td>
                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">ItemId</td>
                                    <td><%# Eval("itemid") %></td>
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
                                    <td><%# Eval("total_amount") %></td>
                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">泰国广告测品</td>
                                    <td>
                                        <div class="checkbox-group">
                                            <div class="checkbox-item yes">
                                                <input type="checkbox" 
                                                       id="chkIsToThadYes_<%# Container.ItemIndex %>" 
                                                       name="chkIsToThadYes_<%# Container.ItemIndex %>"
                                                       <%# Eval("is_to_thad").ToString() == "1" ? "checked" : "" %>
                                                       onchange="toggleSingleAdStatus(this, true)" />
                                                <label for="chkIsToThadYes_<%# Container.ItemIndex %>">是</label>
                                            </div>
                                            <div class="checkbox-item no">
                                                <input type="checkbox" 
                                                       id="chkIsToThadNo_<%# Container.ItemIndex %>" 
                                                       name="chkIsToThadNo_<%# Container.ItemIndex %>"
                                                       <%# Eval("is_to_thad").ToString() == "-1" ? "checked" : "" %>
                                                       onchange="toggleSingleAdStatus(this, false)" />
                                                <label for="chkIsToThadNo_<%# Container.ItemIndex %>">否</label>
                                            </div>
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:LinkButton ID="btnzd"
                                            OnClientClick="return validateSingleSave(this);"
                                            runat="server"
                                            Text="保存"
                                            ForeColor="White"
                                            BackColor="Green"
                                            CssClass="butt"
                                            CommandName="qr"
                                            CommandArgument='<%# Eval("itemid") %>' />
                                        &nbsp;&nbsp;
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
            <!-- 新增跳转控件 -->
            <span style="margin: 0 10px">跳转至 
                <asp:TextBox ID="txtJumpPage" runat="server" Width="50px" Text="1"></asp:TextBox>
                <asp:Button ID="btnJump" runat="server" Text="GO" OnClick="BtnJump_Click" CssClass="pager-btn" />
            </span>
            <span style="margin: 0 10px">
                <asp:Literal ID="litCurrentPage" runat="server" />
                /
                <asp:Literal ID="litPageInfo" runat="server"></asp:Literal>
                <asp:Literal ID="litTotalPages" runat="server" />
            </span>
            <asp:Button ID="btnNext" runat="server" Text="下一页" OnClick="btnNext_Click" CssClass="pager-btn" />
        </div>
    </form>
</body>
</html>