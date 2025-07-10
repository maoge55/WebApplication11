<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="残次品退货.aspx.cs" Inherits="WebApplication11.cg.tb.残次品退货" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>残次品退货</title>
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

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h3>当前页面【<span style="color: #37cbc5">残次品退货</span>】</h3>
            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>
        
        <!-- 筛选区域 -->
        <div>
            输入运营编码<span style="color: red;">*</span>：
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
            &nbsp;
            快递单号：
            <asp:TextBox ID="txtKuaididanhao" runat="server"></asp:TextBox>
            &nbsp;
            标题搜索词：
            <asp:TextBox ID="txtBiaoti" runat="server"></asp:TextBox>
            &nbsp;
            1688订单号：
            <asp:TextBox ID="txt1688OrderNo" runat="server" Width="150px"></asp:TextBox>
            &nbsp;
            
            筛选条件：
            <asp:DropDownList ID="ddlSearchStatus" runat="server">
                <asp:ListItem Value="未处理" Selected="True">未处理</asp:ListItem>
                <asp:ListItem Value="申请退货">申请退货</asp:ListItem>
                <asp:ListItem Value="退款成功">退款成功</asp:ListItem>
                <asp:ListItem Value="">全部</asp:ListItem>
            </asp:DropDownList>
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
                
                <ItemTemplate>
                    <tr>
                        <td style="width: 4%; text-align: center">
                            <%# Container.ItemIndex + 1 %>
                        </td>
                        <td style="width: 28%; text-align: center">
                            <a href="<%# Eval("sku_img") %>" target="_blank">
                                <img src="<%# Eval("sku_img") %>" style="width: 300px; height: 300px;""/>
                            </a>
                        </td>
                        <td style="width: 68%">
                            <!-- 隐藏字段存储关键信息 -->
                            <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("id") %>' />
                            <asp:HiddenField ID="hidSkuid" runat="server" Value='<%# Eval("Skuid") %>' />
                            <asp:HiddenField ID="txtShiJiShouHuoCountOld" runat="server" Value='<%# Eval("ShiJiShouHuoCount") %>' />
                            <asp:HiddenField ID="txtCanCiPinCountOld" runat="server" Value='<%# Eval("CanCiPinCount") %>' />
                            
                            <table class="ttta">
                                <tr>
                                    <td class="bbb">订单号</td>
                                    <td><%# Eval("DingDanBianHao") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">物流公司</td>
                                    <td><%# Eval("WuLiuGongSi") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">运单号</td>
                                    <td><%# Eval("YunDanHao") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">标题</td>
                                    <td><%# Eval("HuoPinBiaoTi") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">SKU_ID</td>
                                    <td><%# Eval("Skuid") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">单位</td>
                                    <td><%# Eval("DanWei") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">残次品数量</td>
                                    <td>
                                        <%# Eval("CanCiPinCount") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">单价</td>
                                    <td><%# Eval("DanJia") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">单批次累计退款金额</td>
                                    <td><%# Eval("TuiKuanZongJin") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">备注</td>
                                    <td>
                                        <asp:TextBox ID="txtDingDanBeiZhu" runat="server" 
                                            Text='<%# Eval("beizhu_cancipin") %>' 
                                            width="90%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">状态</td>
                                    <td>
                                        
                                        <asp:DropDownList ID="ddlStatusCancipin" runat="server" SelectedValue='<%# Eval("status_cancipin") %>'>
                                            <asp:ListItem Value="退款成功">退款成功</asp:ListItem>
                                            <asp:ListItem Value="未处理">未处理</asp:ListItem>
                                            <asp:ListItem Value="申请退货">申请退货</asp:ListItem>
                                        </asp:DropDownList>
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
                                <tr>
                                    <td class="bbb">残次品图片上传</td>
                                    <td>
                                         <asp:FileUpload ID="fup1" runat="server" CssClass="butt" />
                                         <a href="<%# Eval("image_cancipin") %>" target="_blank"><img src="<%# Eval("image_cancipin") %>" style="width:120px;height:60px;" /></a>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb"></td>
                                    <td>
                                        <asp:Button ID="btnSaveItem" runat="server" 
                                            Text="上传文件" 
                                            BackColor="Green" 
                                            ForeColor="White"
                                            CommandName="SaveItemSkuImg"
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