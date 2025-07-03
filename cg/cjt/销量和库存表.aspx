<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="销量和库存表.aspx.cs" Inherits="WebApplication11.cg.cjt.销量和库存表" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>销量和库存表</title>
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
            <div>
                <h3>当前页面【<span style="color: #37cbc5">销量和库存表</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>

                输入商家编码：
                <asp:TextBox ID="txtsjbm" runat="server" ValidationGroup="searchGroup"></asp:TextBox>
                <asp:RequiredFieldValidator 
                    ID="rfvSjbm" 
                    runat="server" 
                    ControlToValidate="txtsjbm"
                    ErrorMessage="* 必须输入商家编码"
                    ForeColor="Red"
                    Display="Dynamic"
                    ValidationGroup="searchGroup">
                </asp:RequiredFieldValidator>
                &nbsp;
                <asp:Button ID="Button1" runat="server" 
                    Text="查找" 
                    BackColor="Red" 
                    ForeColor="White" 
                    OnClick="Button1_Click" 
                    ValidationGroup="searchGroup" />
                    输入浏览器名称：
               <asp:TextBox ID="txtBName" runat="server"></asp:TextBox>
                   &nbsp;
                 销售状态：
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="status-filter">
                    <asp:ListItem Value="在售">在售</asp:ListItem>
                    <asp:ListItem Value="-1">全部状态</asp:ListItem>
                    <asp:ListItem Value="未处理">未处理</asp:ListItem>
                     <asp:ListItem Value="临时停止销售">临时停止销售</asp:ListItem>
                       <asp:ListItem Value="永久停止销售">永久停止销售</asp:ListItem>
                </asp:DropDownList>
                 &nbsp;
                <asp:Button ID="Button2" runat="server" 
                    Text="查找" 
                    BackColor="Red" 
                    ForeColor="White" 
                    OnClick="Button1_Click" 
                    ValidationGroup="searchGroup" />
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            </div>
            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>
                           <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />

                            </td>
                      <td style="width: 30%; text-align: center">
                           <img src='<%# Eval("pimage")%>' style="width:300px" /> 
                         </td>
                            <td>
                                <table class="ttta">
             
      
                                    <tr>
                                        <td style="width: 30%" class="bbb">浏览器名称</td>
                                        <td><%# Eval("浏览器店铺名") %></td>

                                    </tr>
    
                                    <tr>
                                        <td style="width: 30%" class="bbb">商家编码</td>
                                        <td><%# Eval("商家编码") %></td>

                                    </tr>
                                          <tr>
                                        <td style="width: 30%" class="bbb">标题</td>
                                        <td><%# Eval("销售链接产品标题") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">货源链接产品标题</td>
                                        <td><%# Eval("货源链接产品标题") %></td>
                                    </tr>
                                    
                                    </tr>
                         
                                       <tr>
                                           <td style="width: 30%" class="bbb">SKU</td> 
                                           <td><%# Eval("销售链接SKU") %></td>
                                        </tr>

                                      <tr>
                                           <td style="width: 30%" class="bbb">7天总销量</td> 
                                           <td><%# Eval("该SKU7天总销量") %></td>
                                        </tr>
                                       <tr>
                                           <td style="width: 30%" class="bbb">14天总销量</td> 
                                           <td><%# Eval("该SKU14天总销量") %></td>
                                        </tr>
                                        <tr>
                                           <td style="width: 30%" class="bbb">30天总销量</td> 
                                           <td><%# Eval("该SKU30天总销量") %></td>
                                        </tr>
                                        <tr>
                                           <td style="width: 30%" class="bbb">在仓数量</td> 
                                           <td><%# Eval("实际在仓数量") %></td>
                                        </tr>
                                        <tr>
                                           <td style="width: 30%" class="bbb">在途数量</td> 
                                           <td><%# Eval("实际在途总数") %></td>
                                        </tr>
                                                                <tr>
                                           <td style="width: 30%" class="bbb">该SKU需维持在仓在途数</td> 
                                           <td><%# Eval("该SKU需维持在仓在途数") %></td>
                                        </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">产品链接</td>
                                        <td><a href="<%# Eval("1688采购链接") %>" target="_blank">打开网址</a>&nbsp;&nbsp;
                                            <input type="text" value='<%# Eval("1688采购链接") %>' style="width: 20px" id="url<%# Container.ItemIndex+1 %>" />
                                            <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                         </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688SKU1</td>
                                        <td><%# Eval("1688SKU1") %></td>
                
                                    </tr>
                                                                        
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688SKU2</td>
                                     <td><%# Eval("1688SKU2") %></td>
                                    </tr>
                                                                        
                                                                    
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688采购价</td>
                                     <td><%# Eval("1688价格") %></td>
                                    </tr>

                           <tr>
                                        <td style="width: 30%" class="bbb">需采购数量</td>
                                        <td>
                                            <%# Math.Max(0, 
                                                (Eval("该SKU需维持在仓在途数") is DBNull ? 0 : Convert.ToInt64(Eval("该SKU需维持在仓在途数")) ) 
                                                - (Eval("实际在仓数量") is DBNull ? 0 : Convert.ToInt64(Eval("实际在仓数量")) )              
                                                - (Eval("实际在途总数") is DBNull ? 0 : Convert.ToInt64(Eval("实际在途总数")) )             
                                            ) %>
                                        </td>
                                    </tr>

                                </table>

                                

                            </td>

                        </tr>
                        <tr></tr>
                    </ItemTemplate>
                </asp:Repeater>
               
            </table>

        </div>
    </form>
</body>
</html>