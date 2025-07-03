<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="采购单_印尼_采购员.aspx.cs" Inherits="WebApplication11.cg.cjt.采购单_印尼_采购员" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>采购单_印尼_采购员</title>
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
    </style>
    <script>


        // 复制URL函数保持不变
        function copyUrl(myurl) {
            var Url2 = document.getElementById(myurl);
            Url2.select();
            document.execCommand("Copy");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
      
            <div>
                <h3>当前页面【<span style="color: #37cbc5">采购单_印尼_采购员</span>】</h3>

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
                 采购单状态：
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="status-filter">
                    <asp:ListItem Value="已生成采购单待采购">已生成采购单待采购</asp:ListItem>
                    <asp:ListItem Value="-1">全部状态</asp:ListItem>
                    <asp:ListItem Value="已下单">已下单</asp:ListItem>
                     <asp:ListItem Value="放弃采购">放弃采购</asp:ListItem>
                       <asp:ListItem Value="完成采购">完成采购</asp:ListItem>
                </asp:DropDownList>
              <asp:Button ID="Button1" runat="server" 
                    Text="查找" 
                    BackColor="Red" 
                    ForeColor="White" 
                    OnClick="Button1_Click" 
                    ValidationGroup="searchGroup" />

                <asp:Button ID="Button2" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页" OnClick="Button2_Click1" />

                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            </div>
            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            
                            <asp:Literal ID="采购单号" runat="server" Text='<%# Eval("采购单号") %>' Visible="False"></asp:Literal>
                            <asp:Literal ID="SKU" runat="server" Text='<%# Eval("SKU") %>' Visible="False"></asp:Literal>
                           <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />

                            </td>
                      <td style="width: 30%; text-align: center">
                           <img src='<%# Eval("SKU图片")%>' style="width:300px" /> 
                         </td>
                            <td>
                                <table class="ttta">
             
          
                                  <tr>
                                        <td style="width: 30%" class="bbb">采购单号</td>
                                        <td><%# Eval("采购单号") %></td>
                                      <asp:Literal ID="单号" runat="server" Text='<%# Eval("采购单号") %>' Visible="False"></asp:Literal>
                                    </tr>
    

                                    

                                    <tr>
                                        <td style="width: 30%" class="bbb">1688产品链接</td>
                                        <td><a href="<%# Eval("1688产品链接") %>" target="_blank">打开网址</a>&nbsp;&nbsp;
                                            <input type="text" value='<%# Eval("1688产品链接") %>' style="width: 20px" id="url<%# Container.ItemIndex+1 %>" />
                                            <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                         </td>

                                    </tr>
                                   <tr>
                                        <td style="width: 30%" class="bbb">货源标题</td>
                                        <td><%# Eval("货源标题") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688SKU1</td>
                                      
                                       <td >  <asp:TextBox ID="sku1" runat="server" Text='<%# Eval("1688SKU1") %>'> </asp:TextBox></td>
                
                                    </tr>
                                                                        
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688SKU2</td>
                                    <td ><asp:TextBox ID="sku2" runat="server" Text='<%# Eval("1688SKU2") %>'> </asp:TextBox></td>
                                    </tr>
                                                                        
                                                                    
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688采购价</td>
                                     <td ><asp:TextBox ID="price" runat="server" Text='<%# Eval("1688价格") %>'> </asp:TextBox></td>
                                    </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">需采购数量</td>
                                    <td><%# Eval("需采购数量") %></td>
                                </tr>


                                     <tr>
                                        <td style="width: 30%" class="bbb">采购单状态</td>
                                        <td>
                                            <div style="display: flex; align-items: center; gap: 10px;">
                                                <asp:DropDownList 
                                                        ID="caigoudanzhuangtai" 
                                                        runat="server" 
                                                        CssClass="status-filter"
                                                        SelectedValue='<%# Eval("采购单状态") %>'>
                                                        
                                                        <asp:ListItem Value="已生成采购单待采购">已生成采购单待采购</asp:ListItem>
                                                        <asp:ListItem Value="已下单">已下单</asp:ListItem>
                                                        <asp:ListItem Value="放弃采购">放弃采购</asp:ListItem>
                                                        <asp:ListItem Value="完成采购">完成采购</asp:ListItem>
                                                        <asp:ListItem Value="">请选择</asp:ListItem>
                                                    </asp:DropDownList>
                                            </div>
                                        </td>
                                    </tr>

                                          <tr>
                                            <td></td>
                                            <td>
                                                   <asp:LinkButton ID="btnzd" 
                                                    OnClientClick="JavaScript:return confirm('确定保存？');" 
                                                    runat="server" 
                                                    Text="保存" 
                                                    ForeColor="White" 
                                                    BackColor="Green" 
                                                    CssClass="butt" 
                                                    CommandName="qr" 
                                                    CommandArgument='<%# Eval("采购单号") %>' />
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
    <span style="margin:0 10px">
        跳转至 
        <asp:TextBox ID="txtJumpPage" runat="server" Width="50px" Text="1"></asp:TextBox>
        <asp:Button ID="btnJump" runat="server" Text="GO" 
                    OnClick="btnJump_Click" CssClass="pager-btn" />
    </span>
            <span style="margin:0 10px">
                <asp:Literal ID="litCurrentPage" runat="server" /> /
                <asp:Literal ID="litPageInfo" runat="server"></asp:Literal>
                <asp:Literal ID="litTotalPages" runat="server" />
            </span>
            <asp:Button ID="btnNext" runat="server" Text="下一页" OnClick="btnNext_Click" CssClass="pager-btn" />
        </div>
    </form>
</body>
</html>