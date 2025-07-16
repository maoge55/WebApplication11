<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="采购订单补充资料.aspx.cs" Inherits="WebApplication11.cg.cjt.采购订单补充资料"   %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>货源补充体积_重量_包装</title>
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
    
        /* 批量应用样式 */
        .batch-apply-container {
            padding: 15px;
            margin: 10px 0;
        }
    
        .batch-apply-row {
            display: flex;
            align-items: center;
            gap: 15px;
            flex-wrap: wrap;
        }
    
        .batch-field {
            display: flex;
            align-items: center;
            gap: 5px;
        }
    
        .batch-label {
            font-weight: bold;
            color: #333;
            white-space: nowrap;
        }
    
        .batch-dropdown {
            min-width: 150px;
            padding: 5px;
            border: 1px solid #ccc;
            border-radius: 3px;
        }
    
        .checkbox-column {
            width: 40px;
            text-align: center;
        }
    </style>
    <script>
        function copyUrl(myurl) {

            var Url2 = document.getElementById(myurl);
            Url2.select();
            document.execCommand("Copy");
        }
        
        // 全选/取消全选功能
        function toggleAll(source) {
            var checkboxes = document.querySelectorAll('input[type="checkbox"][id*="chkSelect"]:not(#chkSelectAll)');
            for (var i = 0; i < checkboxes.length; i++) {
                checkboxes[i].checked = source.checked;
            }
        }
        
        // 检查是否需要更新全选状态
        function updateSelectAllState() {
            var selectAllCheckbox = document.getElementById('chkSelectAll');
            var checkboxes = document.querySelectorAll('input[type="checkbox"][id*="chkSelect"]:not(#chkSelectAll)');
            var checkedCount = 0;
            
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    checkedCount++;
                }
            }
            
            // 如果所有行都选中，则选中全选框；如果有任何一行未选中，则取消全选框
            selectAllCheckbox.checked = (checkedCount === checkboxes.length && checkboxes.length > 0);
        }
        
        // 获取选中的行
        function getSelectedRows() {
            var selectedRows = [];
            var checkboxes = document.querySelectorAll('input[type="checkbox"][id*="chkSelect"]:not(#chkSelectAll)');
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    selectedRows.push(i);
                }
            }
            return selectedRows;
        }
        
        // 绑定checkbox事件
        function bindCheckboxEvents() {
            // 获取数据行中的checkbox（排除全选checkbox）
            var checkboxes = document.querySelectorAll('input[type="checkbox"][id*="chkSelect"]:not(#chkSelectAll)');
            for (var i = 0; i < checkboxes.length; i++) {
                // 移除之前的事件监听器（如果存在）
                checkboxes[i].removeEventListener('change', updateSelectAllState);
                // 添加新的事件监听器
                checkboxes[i].addEventListener('change', updateSelectAllState);
            }
        }
        
        // 页面加载完成后绑定事件
        document.addEventListener('DOMContentLoaded', function() {
            setTimeout(bindCheckboxEvents, 100);
        });
        
        // 在页面回发后重新绑定事件（ASP.NET会重新生成控件）
        function pageLoad() {
            setTimeout(bindCheckboxEvents, 100);
        }
        
        // 兼容ASP.NET的pageLoad事件
        if (typeof(Sys) !== "undefined") {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(pageLoad);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        
            <div>
                <h3>当前页面【<span style="color: #37cbc5">货源补充体积_重量_包装</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>

                输入运营编码：
                <asp:TextBox ID="txtsjbm" runat="server" ValidationGroup="searchGroup"></asp:TextBox>
                <asp:RequiredFieldValidator 
                    ID="rfvSjbm" 
                    runat="server" 
                    ControlToValidate="txtsjbm"
                    ErrorMessage="* 必须输入运营编码"
                    ForeColor="Red"
                    Display="Dynamic"
                    ValidationGroup="searchGroup">
                </asp:RequiredFieldValidator>
                &nbsp;
                标题_OfferID_SkuID：
                <asp:TextBox ID="txtTitleOfferSku" runat="server" ValidationGroup="searchGroup"></asp:TextBox>
                &nbsp;
                 状态：
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="status-filter">
                    <asp:ListItem Value="-1">全部</asp:ListItem>
                    <asp:ListItem Value="补充长宽高">补充长宽高</asp:ListItem>
                    <asp:ListItem Value="补充重量">补充重量</asp:ListItem>
                    <asp:ListItem Value="补充包装盒链接">补充包装盒链接</asp:ListItem>
                    <asp:ListItem Value="补充物流商品种编号">补充物流商品种编号</asp:ListItem>
                </asp:DropDownList>

                &nbsp;&nbsp;
                <asp:Button ID="Button1" runat="server" 
                    Text="查找" 
                    BackColor="Red" 
                    ForeColor="White" 
                    OnClick="Button1_Click" 
                    ValidationGroup="searchGroup" 
                    style="margin-left: 10px;" />
                 &nbsp;&nbsp;
                 <asp:Button ID="Button2" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页" OnClick="Button2_Click1" />
                 &nbsp;

                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            </div>
            
            <br />
            <table class="ttt">
                <!-- 表头 -->
                <tr>
                    <td class="checkbox-column" style="width: 1%;">
                        <input type="checkbox" id="chkSelectAll" onchange="toggleAll(this)" />
                    </td>
                    <td colspan="2" style="width: 25%; text-align: center; vertical-align: top;">
                        <!-- 批量应用区域 -->
                        <div class="batch-apply-container" style="margin: 0;">
                            <div class="batch-apply-row" style="flex-direction: column; gap: 8px; align-items: flex-start;">
                                <div class="batch-field" style="width: 100%;">
                                    <span class="batch-label" style="font-size: 13px; display: block; margin-bottom: 2px;">物流商品种编号_莆田广东:</span>
                                    <asp:DropDownList ID="ddlBatchPtGd" runat="server" CssClass="batch-dropdown" style="width: 120px; font-size: 13px;">
                                    </asp:DropDownList>
                                </div>
                                <div class="batch-field" style="width: 100%;">
                                    <span class="batch-label" style="font-size: 13px; display: block; margin-bottom: 2px;">物流商品种编号_广东印尼:</span>
                                    <asp:DropDownList ID="ddlBatchGdId" runat="server" CssClass="batch-dropdown" style="width: 120px; font-size: 13px;">
                                    </asp:DropDownList>
                                </div>
                                <div class="batch-field" style="width: 100%;">
                                    <span class="batch-label" style="font-size: 13px; display: block; margin-bottom: 2px;">物流商品种编号_广东泰国:</span>
                                    <asp:DropDownList ID="ddlBatchGdTh" runat="server" CssClass="batch-dropdown" style="width: 120px; font-size: 13px;">
                                    </asp:DropDownList>
                                </div>
                                <div class="batch-field" style="width: 100%; margin-top: 5px;">
                                    <asp:Button ID="btnBatchApply" runat="server" Text="应用" 
                                                OnClientClick="return applyBatchSettings();" 
                                                style="width: 80px; font-size: 13px; padding: 4px 8px; background-color: #007BFF; color: white; border: none; border-radius: 3px; cursor: pointer;" />
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <asp:Literal ID="OfferID_1688" runat="server" Text='<%# Eval("OfferID_1688") %>' Visible="False"></asp:Literal>
                            <asp:Literal ID="SkuID_1688" runat="server" Text='<%# Eval("SkuID_1688") %>' Visible="False"></asp:Literal>
                            <td class="checkbox-column">
                                <asp:CheckBox ID="chkSelect" runat="server" />
                            </td>
                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />

                            </td>
                               <td style="width: 30%; text-align: center">
                                 <img 
                                    src='<%# 
                                        Eval("sku_img") != null && Eval("sku_img").ToString().ToLower().StartsWith("http") 
                                        ?"/cg/cjt/ImageProxy.aspx?url=" + Convert.ToString(Eval("sku_img")) 
                                        : ResolveUrl(Convert.ToString(Eval("sku_img")))
                                    %>' 
                                    style="width:300px" />
                         </td>
                            <td>
                                <table class="ttta">
                                
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688 Offer ID</td>
                                        <td>
                                            <%# Eval("OfferID_1688") %>
                                                <asp:LinkButton ID="btnSync" OnClientClick="JavaScript:return confirm('确定同步该Offer下所有SKU数据？');"
                                                    runat="server" Text="套用同1688_Offer_ID已填写信息" BackColor="Green" ForeColor="White" CssClass="butt"
                                                    CommandName="sync" CommandArgument='<%# Eval("OfferID_1688") %>' />
                                        </td>
                                
                                    </tr>
                               
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688 SKU ID</td>
                                        <td><%# Eval("SkuID_1688") %></td>

                                    </tr>


                                        <tr>
                                            <td style="width: 30%" class="bbb">1688采购链接</td>
                                            <td>
                             
                                                <a href="https://detail.1688.com/offer/<%# Eval("OfferID_1688") %>.html" target="_blank">打开网址</a>&nbsp;&nbsp;
        
                                                <input type="text" value="https://detail.1688.com/offer/<%# Eval("OfferID_1688") %>.html" style="width: 20px" id="url<%# Container.ItemIndex+1 %>" />
        
                                                <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                            </td>
                                        </tr>

                                   <tr>
                                        <td style="width: 30%" class="bbb">货品标题</td>
                                        <td><%# Eval("huopinbiaoti") %></td>

                                    </tr>

                                    
                                   <tr>
                                        <td style="width: 30%" class="bbb">单价</td>
                                        <td><%# Eval("danjia") %>元</td>

                                    </tr>

                                                                        
                                   <tr>
                                        <td style="width: 30%" class="bbb">运营编码</td>
                                        <td><%# Eval("yunyingbianma") %></td>

                                    </tr>

                                     <tr>
                                        <td style="width: 30%" class="bbb">长</td>
                                         <td>
                                            <asp:TextBox ID="txtY_length" runat="server" Text='<%# Bind("chang") %>'>cm</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">宽</td>
                                         <td>
                                            <asp:TextBox ID="txtY_width" runat="server" Text='<%# Bind("kuan") %>'>cm</asp:TextBox>
                                        </td>

                                    </tr>

                          
                                    <tr>
                                        <td style="width: 30%" class="bbb">高</td>
                                                 <td>
                                            <asp:TextBox ID="txtY_hight" runat="server" Text='<%# Bind("gao") %>'>cm</asp:TextBox>
                                        </td>

                                    </tr>
                                  <tr>
                                        <td style="width: 30%" class="bbb">重量</td>
                                        <td>
                                            <asp:TextBox ID="txtY_weight" runat="server" Text='<%# Bind("zhongliang") %>'>KG</asp:TextBox>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">体积重量</td>
                                        <td>
                                            <%# Eval("tijizhongliang") %>KG
                                        </td>
                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">数据类型</td>
                                        <td>
                                            <div style="display: flex; align-items: center; gap: 10px;">
                                                <asp:DropDownList 
                                                        ID="sjtype" 
                                                        runat="server" 
                                                        CssClass="status-filter"
                                                        SelectedValue='<%# Eval("sjtype") %>'>
                                                        <asp:ListItem Value="">请选择</asp:ListItem>
                                                        <asp:ListItem Value="发仓货物">发仓货物</asp:ListItem>
                                                        <asp:ListItem Value="阿里狗">阿里狗</asp:ListItem>
                                                        <asp:ListItem Value="耗材">耗材</asp:ListItem>
                                                    </asp:DropDownList>
                                            </div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">物流商品种编号_莆田广东</td>
                                        <td>
                                            <asp:DropDownList ID="ddlLogisticsPtGd" runat="server" 
                                                CssClass="status-filter">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">物流商品种编号_广东印尼</td>
                                        <td>
                                            <asp:DropDownList ID="ddlLogisticsGdId" runat="server" 
                                                CssClass="status-filter">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">物流商品种编号_广东泰国</td>
                                        <td>
                                            <asp:DropDownList ID="ddlLogisticsGdTh" runat="server" 
                                                CssClass="status-filter">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">莆田发广东物流费</td>
                                        <td><%# Eval("logistics_fee_pt_gd", "{0:F2}") %>元</td>
                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">广东发海外仓物流费_印尼</td>
                                        <td><%# Eval("logistics_fee_gd_id", "{0:F2}") %>元</td>
                                    </tr>

                                    <tr>
                                        <td style="width: 30%" class="bbb">广东发海外仓物流费_泰国</td>
                                        <td><%# Eval("logistics_fee_gd_th", "{0:F2}") %>元</td>
                                    </tr>

                                       <tr>
                                        <td style="width: 30%" class="bbb">包装盒</td>
                                        <td>
                                            <div style="display: flex; align-items: center; gap: 10px;">
                                                <asp:DropDownList 
                                                        ID="baozhuanghe" 
                                                        runat="server" 
                                                        CssClass="status-filter"
                                                        SelectedValue='<%# Eval("baozhuanghe") %>'>
                                                    <asp:ListItem Value="">请选择</asp:ListItem>
                                                        <asp:ListItem Value="是">是</asp:ListItem>
                                                    <asp:ListItem Value="否">否</asp:ListItem>
        
                                                    </asp:DropDownList>
                                            </div>
                                        </td>
                                    </tr>
                                    
                                  <tr>
                                        <td style="width: 30%" class="bbb">包装盒规格</td>
                                        <td>
                                           长: <asp:TextBox ID="baozhuanghechang" runat="server" Text='<%# Bind("baozhuanghechang") %>'></asp:TextBox>cm
                                            宽: <asp:TextBox ID="baozhuanghekuan" runat="server" Text='<%# Bind("baozhuanghekuan") %>'></asp:TextBox>cm
                                             高:<asp:TextBox ID="baozhuanghegao" runat="server" Text='<%# Bind("baozhuanghegao") %>'></asp:TextBox>cm
                                        </td>

                                    </tr>
                                  <tr>
                                        <td style="width: 30%" class="bbb">包装盒1688链接1</td>
                                        <td>
                                            <asp:TextBox ID="baozhuanghe1688lianjie1" runat="server" Text='<%# Bind("baozhuanghe1688lianjie1") %>'></asp:TextBox>
                                        </td>

                                    </tr>
                                 <tr>
                                        <td style="width: 30%" class="bbb">最低起批量1</td>
                                        <td>
                                            <asp:TextBox ID="zuidiqipiliang1" runat="server" Text='<%# Bind("zuidiqipiliang1") %>'></asp:TextBox>
                                        </td>

                                    </tr>
                                      <tr>
                                        <td style="width: 30%" class="bbb">包装盒价格1</td>
                                        <td>
                                            <asp:TextBox ID="baozhuanghe1688jiage1" runat="server" Text='<%# Bind("baozhuanghe1688jiage1") %>'></asp:TextBox>
                                        </td>

                                    </tr>

                                   <tr>
                                        <td  style="width: 30%;background-color:aquamarine" class="bbb">包装盒1688链接2</td>
                                        <td style="background-color:aquamarine">
                                            <asp:TextBox ID="baozhuanghe1688lianjie2" runat="server" Text='<%# Bind("baozhuanghe1688lianjie2") %>'></asp:TextBox>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td style="width: 30%;background-color:aquamarine" class="bbb" >最低起批量2</td>
                                        <td style="background-color:aquamarine">
                                            <asp:TextBox ID="zuidiqipiliang2" runat="server" Text='<%# Bind("zuidiqipiliang2") %>'></asp:TextBox>
                                        </td>

                                    </tr>
                                      <tr>
                                        <td  style="width: 30%;background-color:aquamarine" class="bbb">包装盒价格2</td>
                                        <td style="background-color:aquamarine">
                                            <asp:TextBox ID="baozhuanghe1688jiage2" runat="server" Text='<%# Bind("baozhuanghe1688jiage2") %>'></asp:TextBox>
                                        </td>

                                    </tr>

                                      <tr>
                                        <td style="width: 30%" class="bbb">备注</td>
                                        <td>
                                            <asp:TextBox ID="beizhu" runat="server" Text='<%# Bind("beizhu") %>'></asp:TextBox>
                                        </td>

                                    </tr>
                                        <td></td>
                                        <td>
                                            <div style="display: flex; align-items: center; gap: 10px;">
               
                                                <asp:LinkButton ID="btnzd" 
                                                    OnClientClick="JavaScript:return confirm('确定保存？');" 
                                                    runat="server" 
                                                    Text="保存" 
                                                    ForeColor="White" 
                                                    BackColor="Green" 
                                                    CssClass="butt" 
                                                    CommandName="qr" 
                                                    CommandArgument='<%# string.Format("{0}|{1}", Eval("OfferID_1688"), Eval("SkuID_1688")) %>' />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
               
            </table>
            
            <br/>
            <div class="pager-container">
                <asp:Button ID="btnPrev" runat="server" Text="上一页" OnClick="btnPrev_Click" CssClass="pager-btn" />
                <!-- 新增跳转控件 -->
                <span style="margin-left:15px;">
                    跳转至
                    <asp:TextBox ID="txtJumpPage" runat="server" Width="50px" Text="1"></asp:TextBox>
                    <asp:Button ID="btnJump" runat="server" Text="GO" OnClick="btnJump_Click" CssClass="pager-btn" />
                </span>
                <span style="margin:0 10px">
                    <asp:Literal ID="litCurrentPage" runat="server" /> /
                    <asp:Literal ID="litTotalPages" runat="server" />
                    <asp:Literal ID="litPageInfo" runat="server"></asp:Literal>
                </span>
                <asp:Button ID="btnNext" runat="server" Text="下一页" OnClick="btnNext_Click" CssClass="pager-btn" />
            </div>
    </form>
    
    <script>
        // 批量应用功能
        function applyBatchSettings() {
            var selectedRows = getSelectedRows();
            if (selectedRows.length === 0) {
                alert('请先选择要应用的行');
                return false;
            }
            
            var ptGd = document.getElementById('<%= ddlBatchPtGd.ClientID %>').value;
            var gdId = document.getElementById('<%= ddlBatchGdId.ClientID %>').value;
            var gdTh = document.getElementById('<%= ddlBatchGdTh.ClientID %>').value;
            
            if (!ptGd || !gdId || !gdTh) {
                alert('请选择要应用的物流商品种编号');
                return false;
            }
            
            // 获取所有选中的checkbox（排除全选checkbox）
            var checkboxes = document.querySelectorAll('input[type="checkbox"][id*="chkSelect"]:not(#chkSelectAll)');
            
            // 应用到选中的行
            for (var i = 0; i < selectedRows.length; i++) {
                var rowIndex = selectedRows[i];
                var checkbox = checkboxes[rowIndex];
                
                if (checkbox && checkbox.checked) {
                    // 找到checkbox所在的行（tr元素）
                    var row = checkbox.closest('tr');
                    if (row) {
                        // 在该行中查找对应的下拉框
                        if (ptGd) {
                            var ptGdDropdown = row.querySelector('select[id*="ddlLogisticsPtGd"]');
                            if (ptGdDropdown) {
                                setDropdownValue(ptGdDropdown, ptGd);
                            }
                        }
                        
                        if (gdId) {
                            var gdIdDropdown = row.querySelector('select[id*="ddlLogisticsGdId"]');
                            if (gdIdDropdown) {
                                setDropdownValue(gdIdDropdown, gdId);
                            }
                        }
                        
                        if (gdTh) {
                            var gdThDropdown = row.querySelector('select[id*="ddlLogisticsGdTh"]');
                            if (gdThDropdown) {
                                setDropdownValue(gdThDropdown, gdTh);
                            }
                        }
                    }
                }
            }
            
            alert('已应用到选中的 ' + selectedRows.length + ' 行');
            return false; // 防止回发
        }
        
        // 设置下拉框值
        function setDropdownValue(dropdown, value) {
            for (var i = 0; i < dropdown.options.length; i++) {
                if (dropdown.options[i].value === value) {
                    dropdown.selectedIndex = i;
                    break;
                }
            }
        }
    </script>
</body>

</html>
