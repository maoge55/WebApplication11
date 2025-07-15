<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="本地收货登记.aspx.cs" Inherits="WebApplication11.cg.tb.本地收货登记" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>本地收货登记</title>
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
            <h3>当前页面【<span style="color: #37cbc5">本地收货登记</span>】</h3>
            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>
        
        <!-- 筛选区域 -->
        <div>
            
            状态筛选：
            <asp:DropDownList ID="ddlSearchStatus" runat="server">
                <asp:ListItem Value="等待买家确认收货" Selected="True">等待买家确认收货</asp:ListItem>
                <asp:ListItem Value="">全部状态</asp:ListItem>
                <asp:ListItem Value="等待买家付款">等待买家付款</asp:ListItem>
                <asp:ListItem Value="等待卖家发货">等待卖家发货</asp:ListItem>
                <asp:ListItem Value="交易成功">交易成功</asp:ListItem>
                <asp:ListItem Value="交易关闭">交易关闭</asp:ListItem>
                <asp:ListItem Value="退款中">退款中</asp:ListItem>
            </asp:DropDownList>
            &nbsp;
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
                                <img 
                                src='<%# 
                                    Eval("SKU图片") != null && Eval("SKU图片").ToString().ToLower().StartsWith("http") 
                                    ?"ImageProxy.aspx?url=" + Convert.ToString(Eval("SKU图片")) 
                                    : ResolveUrl(Convert.ToString(Eval("SKU图片")))
                                %>' 
                                style="width:300px" />
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
                                    <td class="bbb">1688_SKU_ID</td>
                                    <td><%# Eval("Skuid") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">1688链接</td>
                                    <td>
                                        <a href="https://detail.1688.com/offer/<%# Eval("OfferID") %>.html" target="_blank">
                                          https://detail.1688.com/offer/<%# Eval("OfferID") %>.html
                                        </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">数量</td>
                                    <td><%# Eval("ShuLiang") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">单位</td>
                                    <td><%# Eval("DanWei") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">实际收货数量</td>
                                    <td>
                                        【<%# Eval("ShiJiShouHuoCount") %>】
                                        <asp:TextBox ID="txtShiJiShouHuoCount" runat="server" 
                                            Text='0' 
                                            CssClass="input-number" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">残次品数量</td>
                                    <td>
                                        【<%# Eval("CanCiPinCount") %>】
                                        <asp:TextBox ID="txtCanCiPinCount" runat="server" 
                                            Text='0' 
                                            CssClass="input-number" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">订单备注</td>
                                    <td>
                                        <asp:TextBox ID="txtDingDanBeiZhu" runat="server" 
                                            Text='<%# Eval("DingDanBeiZhu") %>' 
                                            width="90%" />
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
                                    <td class="bbb">SKU图片上传</td>
                                    <td>
                                        
                                         <asp:FileUpload ID="fup1" runat="server" CssClass="butt" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">图片网址</td>
                                    <td>
                                         <asp:FileUpload ID="fup2" runat="server" CssClass="butt" />
                                         <a href="<%# Eval("pimage") %>" target="_blank"><img src="<%# Eval("pimage") %>" style="width:120px;height:60px;" /></a>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">视频网址</td>
                                    <td>
                                         <asp:FileUpload ID="fup3" runat="server" CssClass="butt" />
                                        <a href="<%# Eval("video") %>" target="_blank"><img src="../../upload/video.png" style="width:120px;height:60px;" /></a>
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