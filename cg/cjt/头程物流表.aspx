<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="头程物流表.aspx.cs" Inherits="WebApplication11.cg.cjt.头程物流表" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>头程物流表</title>
    <style>
        .ttt { width: 100%; }
        .ttt tr td { border: 1px solid #000000; padding: 5px; }
        .ttta { width: 100%; }
        .ttta tr td { border: 1px solid #37cbc5; padding: 5px; }
        .col1 { background-color: #ffe5ec; }
        .col2 { background-color: #e6f9ff; }
        .col3 { background-color: #f0fff0; }
        .bbb { font-weight: bold; }
        .butt { padding: 0 50px; }
        .putian-table { background-color: #ffe5ec; }
        .guangdong-table { background-color: #e6f9ff; }
        .haiwai-table { background-color: #f0fff0; }
    </style>
    <script>
        function setCurrentTime() {
            var now = new Date();
            var year = now.getFullYear();
            var month = (now.getMonth() + 1).toString().padStart(2, '0');
            var day = now.getDate().toString().padStart(2, '0');
            var hours = now.getHours().toString().padStart(2, '0');
            var minutes = now.getMinutes().toString().padStart(2, '0');
            var seconds = now.getSeconds().toString().padStart(2, '0');
            var currentTime = year + '-' + month + '-' + day + ' ' + hours + ':' + minutes + ':' + seconds;
            document.getElementById('<%= txtCommonPtsj.ClientID %>').value = currentTime;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        
            <div>
                <h3>当前页面【<span style="color: #37cbc5">头程物流登记</span>】</h3>
                <h2 style="color: blue"><asp:Literal ID="lits" runat="server"></asp:Literal></h2>
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
                <asp:Button ID="Button1" runat="server"
                    Text="进入"
                    BackColor="Red"
                    ForeColor="White"
                    OnClick="Button1_Click"
                    ValidationGroup="searchGroup" />
                &nbsp;
                海外仓入库单号：
                <asp:TextBox ID="txtHaiwaiCangRuKu" runat="server" ValidationGroup="searchGroup"></asp:TextBox>
                <asp:Button ID="btnPutianLeft" runat="server" Text="莆田准备发出"  ForeColor="Black" OnClick="btnPutianLeft_Click" />
                 <asp:Button ID="btnGuangdong" runat="server" Text="广东已发出"  ForeColor="Black" OnClick="btnGuangdong_Click" />
                <div style="display: flex; justify-content: space-between; align-items: center; margin-top: 10px;">
                    <div>
                        
                        <asp:Button ID="btnPutian" runat="server" Text="莆田已发出"  ForeColor="Black" OnClick="btnPutian_Click" />
                       <asp:Button ID="Button5" runat="server" Text="广东已发出"  ForeColor="Black" OnClick="btnGuangdong_Click" />
                        <asp:Button ID="btnHaiwai" runat="server" Text="海外仓已上架"  ForeColor="Black" OnClick="btnHaiwai_Click" />
                    </div>
                    <div >
                                    <asp:FileUpload ID="fuExcel" runat="server" accept=".xls,.xlsx" />
                <asp:Button ID="btnImport" runat="server" Text="导入Excel" 
                  OnClick="btnImport_Click" BackColor="#33cc33" ForeColor="White" />
                    </div>


                    <asp:Button ID="btnSaveAll" runat="server" Text="全部保存" OnClick="btnSaveAll_Click" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" 
                        style="padding: 10px 50px; font-size: 16px; background-color: #37cbc5; color: white; border: none;" />
                </div>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            </div>
            <br />

            <!-- 在Repeater控件前新增公共字段区域 -->
            <div style="margin-bottom: 20px;">
                <table class="ttta">
                    
                <%-- 新增Excel上传控件 --%>


                    <tr>
                       
                         <td style="width: 15%" class="bbb">莆田发货时间</td>
                        <td>
    <asp:TextBox ID="txtCommonPtsj" runat="server"></asp:TextBox>
    <asp:Button ID="btnCurrentTime" runat="server" Text="生成当前时间" OnClientClick="setCurrentTime();return false;" Style="padding: 0 10px; margin-left: 5px; background-color: #37cbc5; color: white; border: none;" />
</td>
                        <td style="width: 15%" class="bbb">海外仓入库单号</td>
                        <td><asp:TextBox ID="txtCommonHaiwaicangRuku" runat="server"></asp:TextBox></td>

                    </tr>

                </table>
            </div>
            
            <table class="ttt">
                <div style="margin-bottom: 10px;">
                    <table class="ttta">
                       <tr>
                            <td style="width:4%;text-align:center"></td>
            <td class="bbb " style="width: 33%;">海外仓系统编码</td>
            <td class="bbb " style="width: 33%;">莆田发出数量</td>
            <td class="bbb " style="width: 34%;">备注</td>
                        </tr>
                    </table>
                </div>
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>

                        <tr>
                            <asp:Literal ID="haiwaicangxitongbianma" runat="server" Text='<%# Eval("haiwaicangxitongbianma") %>' Visible="False"></asp:Literal>
                            <asp:Literal ID="litImportStatus" runat="server"></asp:Literal>
                            <h2 style="font-size:14px; line-height:1.5;">
    <asp:Literal ID="lits" runat="server"></asp:Literal>
</h2>
                            <td>
                               
                                <!-- 初始填写表格 -->
                                <table class="ttta">
                                        <tr>
                                         <td style="width: 4%; text-align: center"><%# Container.ItemIndex+1 %><br /></td>
                                            <br />

                                            <td class="bbb"><asp:TextBox ID="txtY_haiwaicangxitongbianma" runat="server" Text='<%# Eval("haiwaicangxitongbianma") %>'></asp:TextBox></td>
                                        <td class="bbb"><asp:TextBox ID="txtY_ptfachu" runat="server" Text='<%# Eval("putianfachushuliang") %>'></asp:TextBox></td>
                                        <td class="bbb"><asp:TextBox ID="txtY_Beizhu" runat="server" Text='<%# Eval("beizhu") %>'></asp:TextBox></td>
                                        </tr>
                                </table>
                            </td>
                        </tr>
                        <!-- 删除多余的 <tr></tr> 标签 -->
                    </ItemTemplate>
                </asp:Repeater>
            </table>
    
            <!-- 莆田模态框修改 -->
            <div id="divPutianModal" runat="server" style="display:none;position:fixed;top:20%;left:10%;width:80%;max-height:80vh;background:white;border:2px solid #37cbc5;padding:20px;z-index:1000;overflow:auto;">
                <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 15px;">
                    <h3>待补全莆田发出信息</h3>

                    <div>
                                           <asp:Button ID="Button2" runat="server" Text="全部保存" OnClick="btnSaveAllPutian_Click" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" 
                        style="padding: 10px 50px; font-size: 16px; background-color: #37cbc5; color: white; border: none;" />
                        <asp:Button ID="btnCloseModal" runat="server" Text="关闭" OnClick="btnCloseModal_Click" BackColor="Gray" ForeColor="White" />
                    </div>
                </div>
                <table class="ttt putian-table">
                    <asp:Repeater ID="rptPutianPending" runat="server" OnItemCommand="rptPutianPending_ItemCommand">
                        <ItemTemplate>
                            <tr>
                                <asp:Literal ID="haiwaicangrukudanhao" runat="server" Text='<%# Eval("haiwaicangrukudanhao") %>' Visible="False" />
                                <td style="width:4%;text-align:center"><%# Container.ItemIndex+1 %></td>
                                <td>
                                    <table class="ttta">
                                  <tr>
                                            <td class="bbb">海外仓入库单号</td>
                                            <td><asp:TextBox ID="TextBox1" runat="server" Text='<%# Eval("haiwaicangrukudanhao") %>' /></td>
                                        </tr>
                                        <tr>
                                            <td class="bbb">海外仓系统编码</td>
                                            <td><asp:TextBox ID="txtY_haiwaicangxitongbianma" runat="server" Text='<%# Eval("haiwaicangxitongbianma") %>' /></td>
                                        </tr>
                                        <tr>
                                            <td class="bbb">莆田发出数量</td>
                                            <td><asp:TextBox ID="txtY_ptfachu" runat="server" Text='<%# Eval("putianfachushuliang") %>' /></td>
                                        </tr>
              
                                        <tr>
                                            <td class="bbb">莆田发出快递单号</td>
                                            <td><asp:TextBox ID="txtKddh" runat="server" Text='<%# Eval("putianfachukuaididanghao") %>' /></td>
                                        </tr>

                                        <tr>
                                            <td class="bbb">快递费</td>
                                            <td><asp:TextBox ID="txtPrice" runat="server" Text='<%# Eval("kuaidifei") %>' /></td>
                                        </tr>
                                       <tr>
                                            <td class="bbb">备注</td>
                                            <td><asp:TextBox ID="txtPtBeizhu" runat="server" Text='<%# Eval("beizhu") %>' /></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:LinkButton ID="btnSave" runat="server" Text="保存" 
                                                    CommandName="savePutian" 
                                                    CommandArgument='<%# Eval("haiwaicangrukudanhao") %>'
                                                    BackColor="Green" ForeColor="White" CssClass="butt"
                                                    OnClientClick="JavaScript:return confirm('确定保存此条记录？');" />
                                                &nbsp;&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <!-- 广东模态框修改（移除底部重复的关闭按钮） -->
            <div id="divGuangdongModal" runat="server" style="display:none;position:fixed;top:20%;left:10%;width:80%;max-height:80vh;background:white;border:2px solid #37cbc5;padding:20px;z-index:1000;overflow:auto;"> 
                <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 15px;">
                    <h3>待补全广东发出信息</h3>

                    <div>
                                             
                                         
                                            <td><a href=" https://tool2.baohanscm.com/index.html" target="_blank">宝涵物流查询网址</a>&nbsp;&nbsp;</td>
                                     <td><a href=" https://oms.seacang.com/fore/logistics/trackList.html" target="_blank">海仓派物流查询网址</a>&nbsp;&nbsp;</td>
                                           <asp:Button ID="Button3" runat="server" Text="全部保存" OnClick="btnSaveAllGuangdong_Click" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" 
                        style="padding: 10px 50px; font-size: 16px; background-color: #37cbc5; color: white; border: none;" />
                        <asp:Button ID="btnCloseGdModal" runat="server" Text="关闭" OnClick="btnCloseGdModal_Click" BackColor="Gray" ForeColor="White" />
                    </div>
                </div>
                <table class="ttt guangdong-table"> 
                    <asp:Repeater ID="rptGuangdongPending" runat="server" OnItemCommand="rptGuangdongPending_ItemCommand"> 
                        <ItemTemplate>  
                            <tr> 
                                <asp:Literal ID="haiwaicangrukudanhao" runat="server" Text='<%# Eval("haiwaicangrukudanhao") %>' Visible="False" /> 
                                <td style="width:4%;text-align:center"><%# Container.ItemIndex+1 %></td> 
                                <td> 

                                    <table class="ttta"> 
                                        <tr>
                                            <td class="bbb">海外仓入库单号</td>
                                            <td><%# Eval("haiwaicangrukudanhao") %></td>
                                        </tr>
                                        <tr>
                                            <td class="bbb">广东发出运单号</td>
                                            <td><asp:TextBox ID="txtGdYundanhao" runat="server" Text='<%# Eval("guandongfachuyundanhao") %>' /></td>
                                        </tr>

                                        <tr> 
                                            <td class="bbb">头程物流费</td> 
                                            <td><asp:TextBox ID="txtTlFei" runat="server" Text='<%# Eval("touchenwuliufei") %>' /></td> 
                                        </tr> 
                                     <tr>
                                        <td style="width: 30%" class="bbb">头程物流商</td>
                                        <td>
                                            <div style="display: flex; align-items: center; gap: 10px;">
                                                <asp:DropDownList 
                                                        ID="touchenwuliushan" 
                                                        runat="server" 
                                                        CssClass="status-filter"
                                                        SelectedValue='<%# Eval("touchenwuliushan") %>'>
                                                        <asp:ListItem Value="">请选择</asp:ListItem>
                                                        <asp:ListItem Value="宝涵">宝涵</asp:ListItem>
                                                        <asp:ListItem Value="海仓派">海仓派</asp:ListItem>
                                                    </asp:DropDownList>
                                            </div>
                                        </td>
                                    </tr>

                                          <tr>
                                            <td class="bbb">备注</td>
                                            <td><asp:TextBox ID="txtGdBeizhu" runat="server" Text='<%# Eval("beizhu") %>' /></td>
                                        </tr> 

                                                                         <tr>
                                        <td class="bbb">图片网址：</td>
                                        <td>

                                            <asp:DataList ID="DataList2" DataKeyField="imgname" runat="server"
                                                OnItemCommand="DataList2_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:Literal Text='<%# Eval("haiwaicangrukudanhao").ToString() %>' runat="server" ID="haiwaicangrukudanhao" Visible="false"></asp:Literal>
                                                    <table bgcolor="black" cellpadding="4" cellspacing="1">
                                                        <tr>
                                                            <td bgcolor="white" width="60px">
                                                                <a href='<%# "/Uploads/"+Eval("imgname").ToString()%>' target="_blank">
                                                                    <img id="img" src='<%# "/Uploads/"+Eval("imgname").ToString()%>' height="60" width="60"
                                                                        border="0" /></a>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td bgcolor="white" align="center">
                                                                <asp:Button ID="del" Text="del" Width="50px" runat="server" CommandName="del" CommandArgument='<%# Eval("imgname") %>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>

                                            <br />
                                            <asp:FileUpload ID="FileUpload2" runat="server" AllowMultiple="true" />
                                            <asp:LinkButton ID="LinkButton2" OnClientClick="JavaScript:return confirm('确认上传？');" runat="server" Text="确认上传" ForeColor="White" BackColor="Blue" CssClass="butt" CommandName="upimg" CommandArgument='<%# Eval("haiwaicangrukudanhao") %>' />
                                        </td>
                                    </tr>
              

                                        <tr> 
                                            <td></td> 
                                            <td> 
                                                <asp:LinkButton ID="btnSaveGd" runat="server" Text="保存" 
                                                    CommandName="saveGuangdong" 
                                                    CommandArgument='<%# Eval("haiwaicangrukudanhao") %>' 
                                                    BackColor="Green" ForeColor="White" CssClass="butt" 
                                                    OnClientClick="JavaScript:return confirm('确定保存此条记录？');" />
        
                                            </td> 
                                        </tr> 
                                    </table> 
                                </td> 
                            </tr> 
                        </ItemTemplate>  
                    </asp:Repeater> 
                </table> 
                <!-- 移除原底部重复的关闭按钮 -->
            </div>

<!-- 海外模态框结构修改 -->
<div id="divHaiwaiModal" runat="server" style="display:none;position:fixed;top:20%;left:10%;width:80%;max-height:80vh;background:white;border:2px solid #37cbc5;padding:20px;z-index:1000;overflow:auto;">
    <!-- 新增flex布局容器（标题+关闭按钮） -->
    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 15px;">
        <h3>待补全海外仓信息</h3>
        <div>
            <td><a href=" https://tool2.baohanscm.com/index.html" target="_blank" >宝涵物流查询网址</a>&nbsp;&nbsp;</td>
            <td><a href=" https://oms.seacang.com/fore/logistics/trackList.html" target="_blank">海仓派物流查询网址</a>&nbsp;&nbsp;</td>
          <asp:Button ID="Button4" runat="server" Text="全部保存" OnClick="btnSaveAllHaiwai_Click" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" 
             style="padding: 10px 50px; font-size: 16px; background-color: #37cbc5; color: white; border: none;" />
            <asp:Button ID="btnCloseHaiwaiModal" runat="server" Text="关闭" OnClick="btnCloseHaiwaiModal_Click" BackColor="Gray" ForeColor="White" />
        </div>
    </div>
    <table class="ttt haiwai-table">
        <asp:Repeater ID="rptHaiwaiPending" runat="server" OnItemCommand="rptHaiwaiPending_ItemCommand">
            <ItemTemplate>
                <tr>
                    <asp:Literal ID="haiwaicangrukudanhao" runat="server" Text='<%# Eval("haiwaicangrukudanhao") %>' Visible="False" />
                    <td style="width:4%;text-align:center"><%# Container.ItemIndex+1 %></td>
                    <td>
                        <table class="ttta">
                            <tr>
                                            <td class="bbb">海外仓入库单号</td>
                                            <td><%# Eval("haiwaicangrukudanhao") %></td>
                                        </tr>

                            <tr>
                                            <td class="bbb">广东发出运单号 </td>
                                            <td><%# Eval("guandongfachuyundanhao") %></td>
                            </tr>
                           <tr>
                                            <td class="bbb">头程物流商 </td>
                                            <td><%# Eval("touchenwuliushan") %></td>
                            </tr>
                           <tr>
                                            <td class="bbb">头程物流费 </td>
                                            <td><%# Eval("touchenwuliufei") %><asp:TextBox ID="touchenwuliufei" runat="server" Text='<%# Bind("touchenwuliufei") %>'  />
</td>
                            </tr>
                            <tr>
                                <td class="bbb">海外仓已上架数量</td>
                                <td><asp:TextBox ID="txtShoudao" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="bbb">丢失数量</td>
                                <td><asp:TextBox ID="txtDiuShi" runat="server" /></td>
                            </tr>
                                <tr>
                                     <td class="bbb">备注</td>
                                    <td><asp:TextBox ID="txtHaiwaiBeizhu" runat="server" /></td>
                               </tr>

                                 <tr>
                                        <td class="bbb">上传图片：</td>
                                        <td>

                                            <asp:DataList ID="DataList1" DataKeyField="imgname" runat="server"
                                                OnItemCommand="DataList1_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:Literal Text='<%# Eval("haiwaicangrukudanhao").ToString() %>' runat="server" ID="haiwaicangrukudanhao" Visible="false"></asp:Literal>
                                                    <table bgcolor="black" cellpadding="4" cellspacing="1">
                                                        <tr>
                                                            <td bgcolor="white" width="60px">
                                                                <a href='<%# "/Uploads/"+Eval("imgname").ToString()%>' target="_blank">
                                                                    <img id="img" src='<%# "/Uploads/"+Eval("imgname").ToString()%>' height="60" width="60"
                                                                        border="0" /></a>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td bgcolor="white" align="center">
                                                                <asp:Button ID="del" Text="del" Width="50px" runat="server" CommandName="del" CommandArgument='<%# Eval("imgname") %>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>

                                         
                                            <br />
                                            <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" />
                                            <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确认上传？');" runat="server" Text="确认上传" ForeColor="White" BackColor="Blue" CssClass="butt" CommandName="upimg" CommandArgument='<%# Eval("haiwaicangrukudanhao") %>' />
                                        </td>
                                    </tr>
                                       <tr>
                                        <td style="width: 30%" class="bbb">发货状态</td>
                                        <td>
                                            <div style="display: flex; align-items: center; gap: 10px;">
                                                <asp:DropDownList 
                                                        ID="fahuozhuangtai" 
                                                        runat="server" 
                                                        CssClass="status-filter"
                                                        SelectedValue='<%# Eval("fahuozhuangtai") %>'>
                                                        <asp:ListItem Value="">请选择</asp:ListItem>
                                                        <asp:ListItem Value="发货完成">发货完成</asp:ListItem>
                                                        <asp:ListItem Value="发货未完成">发货未完成</asp:ListItem>
                                                    </asp:DropDownList>
                                            </div>
                                        </td>
                                    </tr>
       
                            <tr>
                                <td></td>
                                <td>
                                    <asp:LinkButton ID="btnSaveHaiwai" runat="server" Text="保存" 
                                        CommandName="saveHaiwai" 
                                        CommandArgument='<%# Eval("haiwaicangrukudanhao") %>'
                                        BackColor="Green" ForeColor="White" CssClass="butt"
                                        OnClientClick="JavaScript:return confirm('确定保存此条记录？');" />

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
   
</div>
            <!-- 底部全部保存按钮 -->
        <div style="margin: 30px auto; text-align: center;">
            <asp:Button ID="btnSaveAllBottom" runat="server" Text="全部保存" OnClick="btnSaveAll_Click" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" 
                style="padding: 10px 50px; font-size: 16px; background-color: #37cbc5; color: white; border: none;" />
        </div>
    </form>
</body>
</html>


<script>
    function initDraggable(modalId) {
        var modal = document.getElementById(modalId);
        if (!modal) return;

        var isDragging = false;
        var offset = { x: 0, y: 0 };

        // 添加可拖拽标题栏（样式与莆田模态框一致）
        var dragHandle = document.createElement('div');
        dragHandle.style.cssText = 'cursor:move;background:#37cbc5;color:white;padding:5px;margin:-20px -20px 15px -20px;text-align:center;';
        dragHandle.textContent = '拖拽标题栏移动';
        modal.insertBefore(dragHandle, modal.firstChild);

        // 鼠标按下事件
        dragHandle.addEventListener('mousedown', function (e) {
            isDragging = true;
            var rect = modal.getBoundingClientRect();
            offset.x = e.clientX - rect.left;
            offset.y = e.clientY - rect.top;
        });

        // 鼠标移动事件
        document.addEventListener('mousemove', function (e) {
            if (isDragging) {
                modal.style.left = (e.clientX - offset.x) + 'px';
                modal.style.top = (e.clientY - offset.y) + 'px';
            }
        });

        // 鼠标释放事件
        document.addEventListener('mouseup', function () {
            isDragging = false;
        });
    }

    // 初始化所有三个模态框的拖拽功能
    initDraggable('divPutianModal');
    initDraggable('divGuangdongModal');
    initDraggable('divHaiwaiModal');
</script>