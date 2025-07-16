<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="菲律宾shopee数据处理_运营.aspx.cs" Inherits="WebApplication11.cg.tb.菲律宾shopee数据处理_运营" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>菲律宾shopee数据处理_运营</title>
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
        .mybutton {
            border-radius:6px;
            background-color:darkcyan;color:white;
            padding:1px 10px;
            margin-left:10px;
            text-decoration:none;
        }
        .mybutton:hover {
            border-radius:6px;
            background-color:chocolate;color:white;
            font-weight:bold;
            padding:1px 10px;
            margin-left:10px;
            text-decoration:none;
        }
            .ttta tr td {
                border: 1px solid #37cbc5;
                padding: 5px;
            }

        .bbb {
            font-weight: bold;
            width:20%;
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
            
            var checkedItems = document.querySelectorAll("input[id*='chkItem']:checked");
            if (checkedItems.length === 0) {
                alert('请至少选择一行进行批量操作！');
                return false;
            }
            let newV = "";
            // 获取所有被选中行的状态下拉框
            checkedItems.forEach(function (checkbox) {
                var row = checkbox.closest('tr');
                var statusDropdown = row.querySelector("input[id*='hidId']");
                if (newV == "") {
                    newV = statusDropdown.value;
                } else {
                    newV += ","+statusDropdown.value;
                }
            });
            document.getElementById("_hidId").value = newV;

            //alert('批量状态已应用到选中行，请点击保存按钮保存到数据库！');
            //return false; // 防止提交
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

        function validateFindForm() {

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
            <h3>当前页面【<span style="color: #37cbc5">菲律宾shopee数据处理_运营</span>】</h3>
            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>
        
        <!-- 筛选区域 -->
        <div style="line-height:40px;padding:10px; border:1px solid #EDF2FA; border-bottom:0px;">
            价格区间：
            <asp:TextBox ID="txtPriceLow" runat="server" width="60px" ></asp:TextBox>
            -
            <asp:TextBox ID="txtPriceHigh" runat="server" width="60px" ></asp:TextBox>
            &nbsp;
            月销量大于：
            <asp:TextBox ID="txtMonthSold" runat="server" width="80px" ></asp:TextBox>
            &nbsp;
            历史销量大于：
            <asp:TextBox ID="txtHistoricalSold" runat="server" width="80px" ></asp:TextBox>
            &nbsp;
            评分大于：
            <asp:TextBox ID="txtRatingStar" runat="server" width="80px" ></asp:TextBox>
            &nbsp;
            检索标题：
            <asp:TextBox ID="txtPname" runat="server"></asp:TextBox>
            &nbsp;
            体积状态：
            <asp:DropDownList ID="ddlVolumeStatus" runat="server" Width="150px">
                <asp:ListItem Value="" Selected="True">未处理</asp:ListItem>
                <asp:ListItem Value="全部">全部</asp:ListItem>
                <asp:ListItem Value="体积审核通过">体积审核通过</asp:ListItem>
                <asp:ListItem Value="体积审核不通过">体积审核不通过</asp:ListItem>
            </asp:DropDownList>
            &nbsp;
            货源状态：
            <asp:DropDownList ID="ddlY1688url" runat="server" Width="150px">
                <asp:ListItem Value="" Selected="True">未处理</asp:ListItem>
                <asp:ListItem Value="全部">全部</asp:ListItem>
                <asp:ListItem Value="有货源">1688有货源</asp:ListItem>
                <asp:ListItem Value="无货源">1688无货源</asp:ListItem>
            </asp:DropDownList>
            &nbsp;
            <asp:Button ID="btnSearch" runat="server" Text="查找" 
                BackColor="Red" 
                ForeColor="White"
                OnClick="btnSearch_Click" OnClientClick="return validateFindForm();" ValidationGroup="searchGroup"  />

            <asp:Button ID="btnSaveAll" runat="server" Text="保存整页" 
                BackColor="Blue" 
                ForeColor="White"
                OnClick="btnSaveAll_Click" OnClientClick="return confirmSave();" />

            <br />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </div>
        <div style="padding:10px; border:1px solid #EDF2FA;">
            <asp:HiddenField ID="_hidId" runat="server" Value='' />
                数据状态：
                <asp:DropDownList ID="_ddlStatus" runat="server" Width="150px">
                    <asp:ListItem Value="体积审核通过">体积审核通过</asp:ListItem>
                    <asp:ListItem Value="体积审核不通过">体积审核不通过</asp:ListItem>
                    <asp:ListItem Value="无货源">1688无货源</asp:ListItem>
                </asp:DropDownList>
                &nbsp;
                <input type="checkbox" id="chkAll" onclick="checkAll(this)" class="checkbox-all" />
            选择全部
            &nbsp;
                <asp:Button ID="btnUpdate" runat="server" Text="选中修改" 
                    BackColor="Red" 
                    ForeColor="White" OnClick="btnUpdate_Click"
                    ValidationGroup="searchGroup" OnClientClick="return applyBatchStatus();" />
            </div>

        <!-- 表格数据 -->
        <table class="ttt">
            <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                
                <ItemTemplate>
                    <tr>
                        <td style="width: 2%; text-align: center; vertical-align: middle;">
                            <asp:CheckBox ID="chkItem" name='<%# Eval("id") %>' runat="server" />
                        </td>
                        <td style="width: 4%; text-align: center">
                            <%# Container.ItemIndex + 1 %>
                        </td>
                        <td style="width: 15%; text-align: center">
                            <img src='<%# Eval("image") %>' style="width:100%;"/>
                        </td>
                        <td style="width: 79%">
                            <!-- 隐藏字段存储关键信息 -->
                            <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("id") %>' />
                            
                            <table class="ttta">
                                <tr>
                                    <td class="bbb">标题_shopee_菲律宾</td>
                                    <td><%# Eval("pname") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">主图网址</td>
                                    <td>
                                        <a href='<%# Eval("image") %>' target="_blank">
                                            <%# Eval("image") %>
                                        </a>
                                        <a class="mybutton" href="javascript:void(0);" onclick="copyTextToClipboard('<%# Eval("image") %>');">复制网址</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">价格_shopee_菲律宾</td>
                                    <td><%# Eval("price") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">月销量</td>
                                    <td><%# Eval("month_sold") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">历史销量</td>
                                    <td><%# Eval("historical_sold") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">评分星级</td>
                                    <td><%# Eval("rating_star") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">体积状态</td>
                                    <td>
                                       
                                        <asp:DropDownList ID="ddlVolumeStatus" runat="server" SelectedValue='<%# StrToNull(Eval("VolumeStatus")) %>' Width="150px">
                                            <asp:ListItem Value="体积审核通过">体积审核通过</asp:ListItem>
                                            <asp:ListItem Value="体积审核不通过">体积审核不通过</asp:ListItem>
                                            <asp:ListItem Value="">未知</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">1688货源链接</td>
                                    <td>
                                        无货源：<asp:CheckBox ID="chkY1688url" runat="server" class="checkbox-all" Checked='<%# Eval("Y_1688url").ToString() =="无货源" %>' ></asp:CheckBox> <br />
                                        有货源：<asp:TextBox ID="txtY1688url" runat="server" 
                                            Text='<%# Eval("Y_1688url") %>' 
                                            width="80%" Rows="10" />
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
    <script type="text/javascript">
        // 复制固定文字的函数
        function copyTextToClipboard(text) {
            // 使用现代 Clipboard API
            navigator.clipboard.writeText(text)
                .then(() => {
                    console.log('文字已复制:', text);
                    alert('复制成功！');
                })
                .catch(err => {
                    console.error('复制失败:', err);
                    alert('复制失败，请手动复制');
                });
        }
                                    </script>
</body>
</html> 
