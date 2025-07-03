<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="采集数据_1688_管理员.aspx.cs" Inherits="WebApplication11.cg.cjt.采集数据_1688_管理员" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>采集数据_1688_管理员</title>
    <style>
        /* 原有样式 */
        .ttt { width: 100%; }
        .ttt tr { }
        .ttt tr td { border: 1px solid #000000; padding: 5px; }
        .ttta { width: 100%; }
        .ttta tr td { border: 1px solid #37cbc5; padding: 5px; }
        .bbb { font-weight: bold; }
        .butt { padding: 0 50px; }
        .anniu1 { border: 1px red solid; }
        
.keyword-manager { 
    background-color: #f0f8ff; 
    border: 2px solid #37cbc5; 
    border-radius: 8px; 
    padding: 15px; 
    margin: 20px 0;
       width: 40%; 
}
.keyword-header { 
    color: #37cbc5; 
    font-size: 18px; 
    font-weight: bold; 
    margin-bottom: 10px;
}
.keyword-textarea {
    width: 100%;
    height: 150px;
    padding: 8px;
    border: 1px solid #37cbc5;
    border-radius: 4px;
    margin-top: 10px;
    font-family: Arial, sans-serif;
}
.keyword-row { 
    display: flex; 
    align-items: center; 
    margin: 10px 0;
}
.keyword-input { 
    width: 300px; 
    padding: 8px; 
    border: 1px solid #37cbc5; 
    border-radius: 4px; 
    margin-right: 10px;
}
.keyword-container { 
    max-height: 300px; 
    overflow-y: auto; 
    margin: 15px 0; 
    padding: 10px; 
    border: 1px solid #ddd; 
    border-radius: 4px;
}
.keyword-item { 
    margin: 8px 0; 
    padding: 5px; 
    background-color: #e9f7fe; 
    border-radius: 4px;
}
.btn-keyword {
    background-color: #37cbc5;
    color: white;
    border: none;
    padding: 8px 15px;
    border-radius: 4px;
    cursor: pointer;
    margin: 0 5px;
    font-weight: bold;
}
.btn-keyword:hover {
    background-color: #2da8a3;
}
.btn-cancel {
    background-color: #6c757d;
}
.btn-cancel:hover {
    background-color: #5a6268;
}
.input-group {
    display: flex;
    align-items: center;
    margin-bottom: 10px;
}
.input-label {
    width: 120px;
    font-weight: bold;
    color: #495057;
}
.status-message {
    padding: 10px;
    margin: 10px 0;
    border-radius: 4px;
    font-weight: bold;
}
.success { background-color: #d4edda; color: #155724; }
.error { background-color: #f8d7da; color: #721c24; }


    /* 弹出层样式 */
    .modal-popup {
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        background: white;
        border: 2px solid #37cbc5;
        border-radius: 8px;
        padding: 20px;
        z-index: 1000;
        box-shadow: 0 0 20px rgba(0,0,0,0.2);
        width: 60%;
        max-height: 80vh;
        overflow-y: auto;
    }
    
    .popup-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        border-bottom: 1px solid #37cbc5;
        padding-bottom: 10px;
        margin-bottom: 15px;
    }
    
    .popup-content {
        max-height: 60vh;
        overflow-y: auto;
        padding: 10px;
    }
    
    .keyword-item {
        padding: 8px 12px;
        margin: 5px 0;
        background-color: #e9f7fe;
        border-radius: 4px;
        border-left: 3px solid #37cbc5;
    }
    </style>
    <script>
        function validateNumbers(input) {
            input.value = input.value.replace(/[^\d]/g, '');
        }
             // 复制链接
        function copyUrl(myurl) {
            var Url2 = document.getElementById(myurl);
            Url2.select();
            document.execCommand("Copy");
        }

        // 关键词输入框自动聚焦
        function focusKeywordInput(index) {
            var input = document.getElementById('txtKeyword_' + index);
            if (input) {
                input.focus();
            }
        }
             // 全选
        function checkAll(obj) {
            var items = document.querySelectorAll("input[id*='chkItem']");
            for (var i = 0; i < items.length; i++) {
                items[i].checked = obj.checked;
            }
        }
    </script>
</head>
<body>

    <form id="form1" runat="server">
            <asp:Panel ID="pnlKeywordsPopup" runat="server" CssClass="modal-popup" Visible="false">
    <div class="popup-header">
        <h3>关键词列表</h3>
        <asp:Button ID="btnCloseKeywords" runat="server" Text="关闭" 
                    CssClass="btn-keyword btn-cancel" OnClick="btnCloseKeywords_Click" />
    </div>
    <div class="popup-content">
        <asp:Repeater ID="rptKeywords" runat="server" OnItemCommand="rptKeywords_ItemCommand">
            <ItemTemplate>
                <div class="keyword-item">
                    
                <span><%# Eval("keywords") %></span>
                    <asp:LinkButton ID="btnDeleteKeywords" runat="server" 
                        CommandName="DeleteKeywords" 
                        CommandArgument='<%# Eval("keywords") %>'
                        OnClientClick="return confirm('确定删除该过滤词吗？');"
                        CssClass="btn-keyword btn-cancel" 
                        style="padding:2px 8px;">×</asp:LinkButton>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Panel>
<asp:Panel ID="pnlFiltersPopup" runat="server" CssClass="modal-popup" Visible="false">
    <div class="popup-header">
        <h3>过滤词列表</h3>
        <asp:Button ID="btnCloseFilters" runat="server" Text="关闭" 
                    CssClass="btn-keyword btn-cancel" OnClick="btnCloseFilters_Click" />
    </div>
    <div class="popup-content">
        <asp:Repeater ID="rptFilters" runat="server" OnItemCommand="rptFilters_ItemCommand">
            <ItemTemplate>
                <div class="keyword-item" style="display:flex; justify-content:space-between; align-items:center;">
                    <span><%# Eval("FilterWords_1688") %></span>
                    <asp:LinkButton ID="btnDeleteFilter" runat="server" 
                        CommandName="DeleteFilter" 
                        CommandArgument='<%# Eval("FilterWords_1688") %>'
                        OnClientClick="return confirm('确定删除该过滤词吗？');"
                        CssClass="btn-keyword btn-cancel" 
                        style="padding:2px 8px;">×</asp:LinkButton>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Panel>
        <div>
            <h3>当前页面【<span style="color: #37cbc5">采集数据_1688_管理员</span>】</h3>
            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>
        
<!-- 在搜索区域上方添加关键词管理区域 -->

        <!-- 搜索区域 -->
        <div>
            
              <span class="input-label">运营编码：</span>
                <asp:DropDownList ID="ddlyybm" runat="server" CssClass="status-filter">
                    <asp:ListItem Value="">请选择</asp:ListItem>
                    <asp:ListItem Value="zyd618">zyd618</asp:ListItem>
                    <asp:ListItem Value="id8897">id8897</asp:ListItem>
                    <asp:ListItem Value="wzm123">wzm123</asp:ListItem>
                    <asp:ListItem Value="cym789">cym789</asp:ListItem>
                    <asp:ListItem Value="th8888">th8888</asp:ListItem>
                </asp:DropDownList>
           
                <span class="input-label">出单率：</span>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="status-filter">
                    <asp:ListItem Value="-1">全部状态</asp:ListItem>
                    <asp:ListItem Value="高到低">高到低</asp:ListItem>
                    <asp:ListItem Value="低到高">低到高</asp:ListItem>
                </asp:DropDownList>
      
            
        
                <span class="input-label">关键词状态：</span>
                <asp:DropDownList ID="ddlkey" runat="server" CssClass="status-filter">
                    <asp:ListItem Value="-1">全部状态</asp:ListItem>
                    <asp:ListItem Value="1">启用</asp:ListItem>
                    <asp:ListItem Value="0">禁用</asp:ListItem>
                    <asp:ListItem Value="2">未处理</asp:ListItem>
                </asp:DropDownList>
        
            
       
                <span class="input-label">卖家性质：</span>
                <asp:DropDownList ID="ddlxz" runat="server" CssClass="status-filter">
                    <asp:ListItem Value="-1">全部状态</asp:ListItem>
                    <asp:ListItem Value="厂">厂</asp:ListItem>
                    <asp:ListItem Value="有限公司">有限公司</asp:ListItem>
                    <asp:ListItem Value="旗舰店">旗舰店</asp:ListItem>
                    <asp:ListItem Value="供应链">供应链</asp:ListItem>
                    <asp:ListItem Value="商行">商行</asp:ListItem>
                    <asp:ListItem Value="店">店</asp:ListItem>
                </asp:DropDownList>
         
              <span class="input-label">优先级别：</span>
                <asp:DropDownList ID="ddlpriority" runat="server" CssClass="status-filter">
                    <asp:ListItem Value="">请选择</asp:ListItem>
                    <asp:ListItem Value="高">高</asp:ListItem>
                    <asp:ListItem Value="低">低</asp:ListItem>
                </asp:DropDownList>
        

     
            
       
                <asp:Button ID="Button1" runat="server" Text="查找" BackColor="#37cbc5" 
                            ForeColor="White" OnClick="Button1_Click" ValidationGroup="searchGroup" 
                            CssClass="btn-keyword" />
                <asp:Button ID="Button2" runat="server" ForeColor="White" BackColor="#28a745"
                            OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页" 
                            OnClick="Button2_Click1" CssClass="btn-keyword" style="margin-left: 10px;" />

            
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </div>
        <div style="display: flex; gap: 20px; flex-wrap: wrap;">
            <!-- 关键词管理区域 -->
            <div class="keyword-manager">
                <div class="keyword-header">关键词管理</div>
                <asp:Panel runat="server" ID="pnlKeywordEditor" Visible="false">
                    <div class="input-group">
                        <span class="input-label">每行输入一个关键词：</span>
                        <asp:TextBox 
                            ID="txtKeywordInput" 
                            runat="server" 
                            TextMode="MultiLine" 
                            CssClass="keyword-textarea"
                            placeholder="输入关键词，每行一个"></asp:TextBox>
                    </div>
                    
                    <div style="margin-top: 15px; display: flex; gap: 10px;">
                        <asp:Button ID="btnSaveKeywords" runat="server" Text="全部保存" 
                                    OnClick="btnSaveKeywords_Click" CssClass="btn-keyword" />
                        <asp:Button ID="btnCancelKeywords" runat="server" Text="取消" 
                                    OnClick="btnCancelKeywords_Click" CssClass="btn-keyword btn-cancel" />
                    </div>
                    
                    <asp:Literal ID="litKeywordMessage" runat="server"></asp:Literal>
                </asp:Panel>
                
                <div style="margin-top: 15px;">
                    <asp:Button ID="btnAddKeywordGroup" runat="server" Text="添加关键词组" 
                                OnClick="btnAddKeywordGroup_Click" CssClass="btn-keyword" />
                      <asp:Button ID="btnAllKeyWords" runat="server" Text="查看关键词组" 
                                OnClick="btnAllKeyWords_Click" CssClass="btn-keyword" />
                </div>
                
                  
            
            </div>
            
            <!-- 过滤词管理区域 -->
            <div class="keyword-manager">
                <div class="keyword-header">过滤词管理</div>
                <asp:Panel runat="server" ID="pnlFilterEditor" Visible="false">
                    <div class="input-group">
                        <span class="input-label">每行输入一个过滤词：</span>
                        <asp:TextBox 
                            ID="txtFilterInput" 
                            runat="server" 
                            TextMode="MultiLine" 
                            CssClass="keyword-textarea"
                            placeholder="输入过滤词，每行一个"></asp:TextBox>
                    </div>
                    
                    <div style="margin-top: 15px; display: flex; gap: 10px;">
                        <asp:Button ID="btnSaveFilters" runat="server" Text="全部保存" 
                                    OnClick="btnSaveFilters_Click" CssClass="btn-keyword" />
                        <asp:Button ID="btnCancelFilters" runat="server" Text="取消" 
                                    OnClick="btnCancelFilters_Click" CssClass="btn-keyword btn-cancel" />
                    </div>
                    
                    <asp:Literal ID="litFilterMessage" runat="server"></asp:Literal>
                </asp:Panel>
                
                <div style="margin-top: 15px;">
                    <asp:Button ID="btnAddFilterGroup" runat="server" Text="添加过滤词组" 
                                OnClick="btnAddFilterGroup_Click" CssClass="btn-keyword" />
                         <asp:Button ID="btnAllFilterWords" runat="server" Text="查看过滤词组" 
                                OnClick="btnAllFilterWords_Click" CssClass="btn-keyword" />
                </div>
              
               
         
            </div>
        </div>

        



        <!-- -->

        <!-- 数据表格 -->
        <br />
        <table class="ttt">
            <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
             <HeaderTemplate>
<tr>
    <td style="width:3%">
        <input type="checkbox" id="chkAll" onclick="checkAll(this)" />
    </td>
    <td colspan="4" > 
        <table style="width:20%; border-collapse: collapse;"> <!-- 嵌套表格实现垂直布局 -->
            <tr style="background-color:#f0f8ff">
                <td style="padding:4px 0;">
                    <asp:DropDownList ID="ddlBatchyybm" runat="server" Width="20%">
                        <asp:ListItem Value="">请选择运营编码</asp:ListItem>
                                <asp:ListItem Value="zyd618">zyd618</asp:ListItem>
                                <asp:ListItem Value="id8897">id8897</asp:ListItem>
                                <asp:ListItem Value="wzm123">wzm123</asp:ListItem>
                                <asp:ListItem Value="cym789">cym789</asp:ListItem>
                                <asp:ListItem Value="th8888">th8888</asp:ListItem>
                    </asp:DropDownList>
                运营编码
                </td>
            </tr>
            <tr style="background-color:#f0f8ff">
                <td style="padding:4px 0;">
                    <asp:DropDownList ID="ddlBatchkey" runat="server" Width="20%">
                        <asp:ListItem Value="">请选择关键词状态</asp:ListItem>
                                <asp:ListItem Value="1">启用</asp:ListItem>
                                <asp:ListItem Value="0">禁用</asp:ListItem>
                                <asp:ListItem Value="2">未处理</asp:ListItem>
                    </asp:DropDownList>
                    关键词状态
                </td>
            </tr>
            <tr style="background-color:#f0f8ff">
                <td style="padding:4px 0;">
                    <asp:DropDownList ID="ddlBatchpriotity" runat="server" Width="20%">
                        <asp:ListItem Value="">请选择优先级别</asp:ListItem>
                                <asp:ListItem Value="高">高</asp:ListItem>
                                <asp:ListItem Value="低">低</asp:ListItem>
                    </asp:DropDownList>
                    优先级别
                </td>
            </tr>
            <tr style="background-color:#f0f8ff">
                <td style="padding:4px 0; text-align:right;">
                    <asp:LinkButton ID="btnApplyBatch" runat="server" 
                        CommandName="ApplyBatch" 
                        Text="应用状态" 
                        OnClientClick="return confirm('确定应用状态到选中行吗？');" 
                        CssClass="butt" />
                </td>
            </tr>
        </table>
    </td>
</tr>


                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width:3%">
                            <asp:CheckBox ID="chkItem" runat="server" />
                        </td>
                        <asp:Literal ID="key" runat="server" Text='<%# Eval("1688采集关键词") %>' Visible="false"/>                   
                        <td style="width: 4%; text-align: center">
                            <%# Container.ItemIndex+1 %><br />
                        </td>
                        <td>
                            <table class="ttta">
                                <tr>
                                    <td style="width: 30%" class="bbb">1688采集关键词</td>
                                    <td><%# Eval("1688采集关键词") %></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">1688采集链接</td>
                                    <td>
                                        <a href="<%# Eval("1688采集链接") %>" target="_blank">打开网址</a>&nbsp;&nbsp;
                                        <input type="text" value='<%# Eval("1688采集链接") %>' style="width: 20px" id="shopeeurl<%# Container.ItemIndex+1 %>" />
                                        <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">已采集产品数量</td>
                                    <td><%# Eval("已采集产品数量") %></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">印尼已导出产品数量</td>
                                    <td><%# Eval("印尼已导出产品数量") %></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">泰国已导出产品数量</td>
                                    <td><%# Eval("泰国已导出产品数量") %></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">印尼未导出产品数量</td>
                                    <td><%# Eval("印尼未导出产品数量") %></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">泰国未导出产品数量</td>
                                    <td><%# Eval("泰国未导出产品数量") %></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">出单率</td>
                                    <td><%# Eval("出单率") %></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">广告订单_链接总数量</td>
                                    <td><%# Eval("广告订单_链接总数量") %></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">广告订单_买家总数量</td>
                                    <td><%# Eval("广告订单_买家总数量") %></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">印尼shopee最高7天平均流量</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">泰国shopee最高7天平均流量</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">1688价格区间</td>
                                    <td>
                                        <asp:TextBox ID="minprice" runat="server" Text='<%# Eval("1688最低价格") %>'></asp:TextBox>-
                                        <asp:TextBox ID="maxprice" runat="server" Text='<%# Eval("1688最高价格") %>'></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">1688销量大于</td>
                                    <td>
                                        <asp:TextBox ID="xiaoliang" runat="server" Text='<%# Eval("1688销量") %>'></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">运营编码</td>
                                    <td>
                                        <div style="display: flex; align-items: center; gap: 10px;">
                                            <asp:DropDownList ID="YYBM" runat="server" CssClass="status-filter"
                                                SelectedValue='<%# Eval("运营编码") %>'>
                                                <asp:ListItem Value="">请选择</asp:ListItem>
                                                <asp:ListItem Value="zyd618">zyd618</asp:ListItem>
                                                <asp:ListItem Value="id8897">id8897</asp:ListItem>
                                                <asp:ListItem Value="wzm123">wzm123</asp:ListItem>
                                                <asp:ListItem Value="cym789">cym789</asp:ListItem>
                                                <asp:ListItem Value="th8888">th8888</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">关键词状态</td>
                                    <td>
                                        <div style="display: flex; align-items: center; gap: 10px;">
                                            <asp:DropDownList ID="keystatus" runat="server" CssClass="status-filter"
                                                SelectedValue='<%# Eval("关键词状态") %>'>
                                                <asp:ListItem Value="">请选择</asp:ListItem>
                                                <asp:ListItem Value="1">启用</asp:ListItem>
                                                <asp:ListItem Value="0">禁用</asp:ListItem>
                                                <asp:ListItem Value="2">未处理</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">优先级别</td>
                                    <td>
                                        <div style="display: flex; align-items: center; gap: 10px;">
                                            <asp:DropDownList ID="priority" runat="server" CssClass="status-filter"
                                                SelectedValue='<%# Eval("优先级别") %>'>
                                                <asp:ListItem Value="">请选择</asp:ListItem>
                                                <asp:ListItem Value="高">高</asp:ListItem>
                                                <asp:ListItem Value="低">低</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:LinkButton ID="btnzd" runat="server" 
                                            OnClientClick="JavaScript:return confirm('确定保存？');" 
                                            Text="保存" ForeColor="White" BackColor="#28a745" CssClass="butt" 
                                            CommandName="qr" CommandArgument='<%# Eval("1688采集关键词") %>' />
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
        
        <!-- 分页控件 -->

            <div class="pager-container">
                <asp:Button ID="btnFirst" runat="server" Text="首页" OnClick="btnFirst_Click" CssClass="btn-keyword" />
                <asp:Button ID="btnPrev" runat="server" Text="上一页" OnClick="btnPrev_Click" CssClass="btn-keyword" />
        <!-- 新增跳转控件 -->
    <span style="margin:0 10px">
        跳转至 
        <asp:TextBox ID="txtJumpPage" runat="server" Width="50px" Text="1"></asp:TextBox>
        <asp:Button ID="btnJump" runat="server" Text="GO" OnClick="btnJump_Click" CssClass="btn-keyword" />
    </span>
                <span style="margin:0 10px">
                    <asp:Literal ID="litCurrentPage" runat="server" /> /
                    <asp:Literal ID="litTotalPages" runat="server" />
                    <asp:Literal ID="litPageInfo" runat="server" Visible="false"></asp:Literal>
                </span>
    
                <asp:Button ID="btnNext" runat="server" Text="下一页" OnClick="btnNext_Click" CssClass="btn-keyword" />
                <asp:Button ID="btnLast" runat="server" Text="尾页" OnClick="btnLast_Click" CssClass="btn-keyword" />
            </div>
    </form>
</body>
</html>