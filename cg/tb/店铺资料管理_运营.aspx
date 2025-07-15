<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="店铺资料管理_运营.aspx.cs" Inherits="WebApplication11.cg.tb.店铺资料管理_运营" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>店铺资料管理_运营</title>
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
            var yybm = document.getElementById('<%= txtSJBM.ClientID %>').value.trim();
            if (yybm === '') {
                alert('商家编码为必填项！');
                return false;
            }
            return true;
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
            <h3>当前页面【<span style="color: #37cbc5">店铺资料管理_运营</span>】</h3>
            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>
        
        <!-- 筛选区域 -->
        <div>
            商家编码<span style="color: red;">*</span>：
            <asp:TextBox ID="txtSJBM" runat="server" ValidationGroup="searchGroup"></asp:TextBox>
            <asp:RequiredFieldValidator 
                ID="rfvSJBM" 
                runat="server" 
                ControlToValidate="txtSJBM"
                ErrorMessage="* 必须输入商家编码"
                ForeColor="Red"
                Display="Dynamic"
                ValidationGroup="searchGroup">
            </asp:RequiredFieldValidator>
            &nbsp;
            &nbsp;
            浏览器ID：
            <asp:TextBox ID="txtBrowserID" runat="server"></asp:TextBox>
            &nbsp;
            筛选条件：
            <asp:DropDownList ID="ddlProxyIP" runat="server" Width="150px">
                <asp:ListItem Value="" Selected="True">全部</asp:ListItem>
                <asp:ListItem Value="补充代理IP">补充代理IP</asp:ListItem>
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
        <br />
        <div style="padding:10px; border:1px solid #EDF2FA;">
            <asp:HiddenField ID="_hidId" runat="server" Value='' />
                登录网址：
                <asp:TextBox ID="_txtPlatform" runat="server"></asp:TextBox>
                &nbsp;
                国家：
                <asp:DropDownList ID="_ddlCountry" runat="server" Width="150px">
                    <asp:ListItem Value="印尼">印尼</asp:ListItem>
                    <asp:ListItem Value="泰国">泰国</asp:ListItem>
                    <asp:ListItem Value="">未知</asp:ListItem>
                </asp:DropDownList>
                &nbsp;
                平台：
                <asp:DropDownList ID="_ddlPingTai" runat="server" Width="150px">
                    <asp:ListItem Value="shopee">shopee</asp:ListItem>
                    <asp:ListItem Value="allegro">allegro</asp:ListItem>
                    <asp:ListItem Value="">未知</asp:ListItem>
                </asp:DropDownList>
                &nbsp;
                商家编码：
                <asp:TextBox ID="_txtSJBM" runat="server"></asp:TextBox>
                &nbsp;
                运营编码：
                <asp:TextBox ID="_txtYYBM" runat="server"></asp:TextBox>
                &nbsp;
                <input type="checkbox" id="chkAll" onclick="checkAll(this)" class="checkbox-all" />
            选择全部
            &nbsp;
                <asp:Button ID="btnUpdate" runat="server" Text="选中修改" 
                    BackColor="Red" 
                    ForeColor="White" OnClick="btnUpdate_Click"
                    ValidationGroup="searchGroup" OnClientClick="return applyBatchStatus();" />
            </div>
        <!-- 新增数据 -->
        <div style="width:100%; display:flex; border:1px solid #EDF2FA;align-content:center; align-items:center;">
            
            <!-- 新增数据 -->
            <div style="width:50%; padding:10px;">
                商家编码：
                <asp:TextBox ID="txtSjbmValue" runat="server"></asp:TextBox>
                &nbsp;
                <asp:FileUpload ID="fup1" runat="server" CssClass="butt" />
                &nbsp;
                <asp:Button ID="btnImport" runat="server" Text="数据导入" 
                    BackColor="Red" 
                    ForeColor="White" OnClick="btnImport_Click" ValidationGroup="searchGroup" OnClientClick="return validateUpload();" />
            </div>
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
                        <td style="width: 94%">
                            <!-- 隐藏字段存储关键信息 -->
                            <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("id") %>' />
                            
                            <table class="ttta">
                                <tr>
                                    <td class="bbb">浏览器ID</td>
                                    <td><%# Eval("BrowserID") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">浏览器名称</td>
                                    <td><%# Eval("DpName") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">分组名称</td>
                                    <td><%# Eval("GroupName") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">登录网址</td>
                                    <td>
                                        <asp:TextBox ID="txtPlatform" runat="server" 
                                            Text='<%# Eval("Platform") %>' 
                                            width="90%" Rows="10" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">国家</td>
                                    <td>
                                       
                                        <asp:DropDownList ID="ddlCountry" runat="server" SelectedValue='<%# StrToNull(Eval("Country")) %>' Width="150px">
                                            <asp:ListItem Value="印尼">印尼</asp:ListItem>
                                            <asp:ListItem Value="泰国">泰国</asp:ListItem>
                                            <asp:ListItem Value="">未知</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">平台</td>
                                    <td>
                                       
                                        <asp:DropDownList ID="ddlPingTai" runat="server" SelectedValue='<%# StrToNull(Eval("PingTai")) %>' Width="150px">
                                            <asp:ListItem Value="shopee">shopee</asp:ListItem>
                                            <asp:ListItem Value="allegro">allegro</asp:ListItem>
                                            <asp:ListItem Value="">未知</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">用户名</td>
                                    <td>
                                        <asp:TextBox ID="txtUserName" runat="server" 
                                            Text='<%# Eval("UserName") %>' 
                                            width="90%" Rows="10" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">密码</td>
                                    <td>
                                        <asp:TextBox ID="txtPassword" runat="server" 
                                            Text='<%# Eval("Password") %>' 
                                            width="90%" Rows="10" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">商家编码</td>
                                    <td>
                                        <asp:TextBox ID="txtSJBM" runat="server" 
                                            Text='<%# Eval("SJBM") %>' 
                                            width="90%" Rows="10" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">运营编码</td>
                                    <td>
                                        <asp:TextBox ID="txtYYBM" runat="server" 
                                            Text='<%# Eval("YYBM") %>' 
                                            width="90%" Rows="10" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">代理IP</td>
                                    <td>
                                        <asp:TextBox ID="txtProxyIP" runat="server" 
                                            Text='<%# Eval("ProxyIP") %>' 
                                            width="90%" Rows="10" />
                                        <div>
                                            * IP格式：socks5://14a2cb0090bc2:bf8e5a1479@91.124.210.25:12324 socks://代理账号:代理密码@主机 : 端口）
                                        </div>
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
