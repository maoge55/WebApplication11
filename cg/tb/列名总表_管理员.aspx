<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="列名总表_管理员.aspx.cs" Inherits="WebApplication11.cg.tb.列名总表_管理员" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>列名总表_管理员</title>
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
            checkedItems.forEach(function (checkbox) {
                var row = checkbox.closest('tr');
                var statusDropdown = row.querySelector("select[id*='ddlStatus']");
                if (statusDropdown) {
                    statusDropdown.value = batchStatus;
                }
            });

            alert('批量状态已应用到选中行，请点击保存按钮保存到数据库！');
            return false; // 防止提交
        }
        function validateForm() {
            let lm = document.getElementById("txtLm").value.trim();
            let table = document.getElementById("txtTa").value.trim();
            if (lm.length < 1) {
                alert("列名必填");
                document.getElementById("txtLm").focus();
                return false;
            } else if (table.length < 1) {
                alert("表必填");
                document.getElementById("txtTa").focus();
                return false;
            }
        }
        function validateUpload() {
            let fup1 = document.getElementById("fup1").value.trim();
            let txtTableName = document.getElementById("txtTableName").value.trim();
            if (fup1.length < 1) {
                alert("请选择导入的数据");
                document.getElementById("fup1").focus();
                return false;
            } else if (txtTableName.length < 1) {
                alert("表必填");
                document.getElementById("txtTableName").focus();
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h3>当前页面【<span style="color: #37cbc5">列名总表_管理员</span>】</h3>
            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>
        
        <!-- 筛选区域 -->
        <div>
            网页名称：
            <asp:DropDownList ID="ddlName" runat="server" Width="150px">
                <asp:ListItem Value="" Selected="True">全部</asp:ListItem>
                <asp:ListItem Value="有">有</asp:ListItem>
                <asp:ListItem Value="无">无</asp:ListItem>
            </asp:DropDownList>
            &nbsp;
            创建状态：
            <asp:DropDownList ID="ddlStauts" runat="server" Width="150px">
                <asp:ListItem Value="" Selected="True">全部</asp:ListItem>
                <asp:ListItem Value="有">有</asp:ListItem>
                <asp:ListItem Value="无">无</asp:ListItem>
            </asp:DropDownList>
            &nbsp;
            检索：
            <asp:TextBox ID="txtFind" runat="server"></asp:TextBox>
            &nbsp;
            <asp:Button ID="btnSearch" runat="server" Text="查找" 
                BackColor="Red" 
                ForeColor="White"
                OnClick="btnSearch_Click" ValidationGroup="searchGroup"  />

            <asp:Button ID="btnSaveAll" runat="server" Text="保存整页" 
                BackColor="Blue" 
                ForeColor="White"
                OnClick="btnSaveAll_Click" OnClientClick="return confirmSave();" />

            <br />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </div>
        <br />
        <!-- 新增数据 -->
        <div style="width:100%; display:flex; border:1px solid #EDF2FA;">
            <div style="width:50%; padding:10px;">
                列名：
                <asp:TextBox ID="txtLm" runat="server"></asp:TextBox>
                &nbsp;
                表：
                <asp:TextBox ID="txtTa" runat="server"></asp:TextBox>
                &nbsp;
                <asp:Button ID="btnAdd" runat="server" Text="添加" 
                    BackColor="Red" 
                    ForeColor="White"
                    OnClick="btnAdd_Click" ValidationGroup="searchGroup" OnClientClick="return validateForm();" />
            </div>
            <!-- 新增数据 -->
            <div style="width:50%; padding:10px;">
                <asp:FileUpload ID="fup1" runat="server" CssClass="butt" />
                &nbsp;
                表：
                <asp:TextBox ID="txtTableName" runat="server"></asp:TextBox>
                &nbsp;
                <asp:Button ID="btnImport" runat="server" Text="上传" 
                    BackColor="Red" 
                    ForeColor="White" OnClick="btnImport_Click" ValidationGroup="searchGroup" OnClientClick="return validateUpload();" />
            </div>
        </div>
        
        <!-- 表格数据 -->
        <table class="ttt">
            <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                
                <ItemTemplate>
                    <tr>
                        <td style="width: 4%; text-align: center">
                            <%# Container.ItemIndex + 1 %>
                        </td>
                        <td style="width: 96%">
                            <!-- 隐藏字段存储关键信息 -->
                            <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("id") %>' />
                            
                            <table class="ttta">
                                <tr>
                                    <td class="bbb">网页名称</td>
                                    <td>
                                        <asp:TextBox ID="txtPageName" runat="server" 
                                            Text='<%# Eval("page_name") %>' 
                                            width="90%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">列名编号</td>
                                    <td><%# Eval("id") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">列名</td>
                                    <td><%# Eval("column_name") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">表</td>
                                    <td><%# Eval("table_name") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">创建状态</td>
                                    <td>
                                       
                                        <asp:DropDownList ID="ddlCreationStatus" runat="server" SelectedValue='<%# StrToInt(Eval("creation_status").ToString()) %>' Width="150px">
                                            <asp:ListItem Value="1">已创建</asp:ListItem>
                                            <asp:ListItem Value="0">未创建</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">关联名称</td>
                                    <td>
                                        <asp:TextBox ID="txtDataSource" runat="server" 
                                            Text='<%# Eval("data_source") %>' 
                                            width="90%" Rows="10" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">基础数据</td>
                                    <td>
                                       
                                        <asp:DropDownList ID="ddlIsBasicData" runat="server" SelectedValue='<%# StrToInt(Eval("is_basic_data").ToString()) %>' Width="150px">
                                            <asp:ListItem Value="1">是</asp:ListItem>
                                            <asp:ListItem Value="0">否</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">关联/计算规则</td>
                                    <td>
                                        <asp:TextBox ID="txtCalculationRule" runat="server"  TextMode="MultiLine"
                                            Text='<%# Eval("calculation_rule") %>' 
                                            width="90%" Rows="10" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb"></td>
                                    <td>
                                        <asp:Button ID="Button1" runat="server" 
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