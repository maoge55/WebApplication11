<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="残次品退换货.aspx.cs" Inherits="WebApplication11.cg.残次品退换货" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>残次品退换货</title>
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
                <h3>当前页面【<span style="color: #37cbc5">残次品退换货</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                    状态筛选：
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="status-filter">
                    <asp:ListItem Value="0">未处理</asp:ListItem>
                    <asp:ListItem Value="-1">全部状态</asp:ListItem>
                     <asp:ListItem Value="申请退货">申请退货</asp:ListItem>
                       <asp:ListItem Value="申请换货">申退换货</asp:ListItem>
                        <asp:ListItem Value="退款成功">退款成功</asp:ListItem>
                        <asp:ListItem Value="换货成功">换货成功</asp:ListItem>
                </asp:DropDownList>
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

                <asp:Button ID="Button2" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页" OnClick="Button2_Click1" />

                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            </div>
            <br />
            <table class="ttt">
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                           <asp:Literal ID="SKU_ID" runat="server" Text='<%# Eval("SKU_ID") %>' Visible="False"></asp:Literal>
                           <asp:Literal ID="littuikuanjietu" runat="server" Text='<%# Eval("tuikuanjietu") %>' Visible="false" />
                            <asp:Literal ID="litcancipintupian" runat="server" Text='<%# Eval("cancipintupian") %>' Visible="false" />
                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />

                            </td>
                            <td style="width: 30%; text-align: center">
                              <img src='<%# Eval("SKU_Picture")%>' style="width:300px" /> 
                            </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 30%" class="bbb">订单号</td>
                                        <td><%# Eval("dingdanbianhao") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">物流公司</td>
                                        <td><%# Eval("wuliugongsi_1688") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">运单号</td>
                                        <td><%# Eval("yundanhao_1688") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">标题</td>
                                        <td><%# Eval("huopinbiaoti") %></td>

                                    </tr>
                                          <tr>
                                        <td style="width: 30%" class="bbb">SKU_ID</td>
                                        <td><%# Eval("SKU_ID") %></td>

                                    </tr>
                                    
                                    </tr>
                                   <tr>
                                        <td style="width: 30%" class="bbb">单位</td>
                                        <td><%# Eval("danwei") %></td>

                                    </tr>
                                       <tr>
                                           <td style="width: 30%" class="bbb">残次品数量</td> 
                                            <td><%# Eval("cancipinshuliang") %></td>
                                        </tr>
 
                                   <tr>
                                        <td style="width: 30%" class="bbb">单价</td>
                                        <td><%# Eval("danjia") %></td>

                                    </tr>
                         
                                   <tr>
                                        <td style="width: 30%" class="bbb">单批次累计退款金额</td>
                                        <td><%# Eval("tuikuanjiner") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">第一次收到正常换货数量</td>
                                        <td>
                                            <asp:TextBox ID="txtY_diyici" runat="server" Text='<%# Bind("diyicihuanhuoshuliang") %>'></asp:TextBox>
                                        </td>
                                    </tr>
                                                                        
                                    <tr>
                                        <td style="width: 30%" class="bbb">第二次收到正常换货数量</td>
                                        <td>
                                            <asp:TextBox ID="txtY_dierci" runat="server" Text='<%# Bind("diercihuanhuoshuliang") %>'></asp:TextBox>
                                        </td>
                                    </tr>
                                                                        
                                    <tr>
                                        <td style="width: 30%" class="bbb">第三次收到正常换货数量</td>
                                        <td>
                                            <asp:TextBox ID="txtY_disanci" runat="server" Text='<%# Bind("disancihuanhuoshuliang") %>'></asp:TextBox>
                                        </td>
                                    </tr>
                               <tr>
                                   <td style="width: 30%" class="bbb">累计收到正常货物数量</td>
                                    <td><%# Eval("leijishoudaozhengchanghuowushuliang") %></td>
                               </tr>                                 
                                    <tr>
                                        <td style="width: 30%" class="bbb">备注</td>
                                        <td>
                                            <asp:TextBox ID="txtY_beizhu" runat="server" Text='<%# Bind("beizhu") %>'></asp:TextBox>
                                        </td>
                                    </tr>

                                      <tr>
                                    <tr>
                                        <td class="bbb">图片网址：</td>
                                        <td>

                                            <asp:DataList ID="DataList1" DataKeyField="imgname" runat="server"
                                                OnItemCommand="DataList1_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:Literal Text='<%# Eval("SKU_ID").ToString() %>' runat="server" ID="liimgSKU_ID" Visible="false"></asp:Literal>
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
                                            <asp:LinkButton ID="LinkButton2" OnClientClick="JavaScript:return confirm('确认上传？');" runat="server" Text="确认上传" ForeColor="White" BackColor="Blue" CssClass="butt" CommandName="uptuikuanimg" CommandArgument='<%# Eval("SKU_ID") %>' />
                                        </td>
                                    </tr>

                                        <td class="bbb">残次品图片上传：</td>
                                          <td>

                                            <asp:DataList ID="DataList2" DataKeyField="imgname" runat="server"
                                                OnItemCommand="DataList2_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:Literal Text='<%# Eval("SKU_ID").ToString() %>' runat="server" ID="liimgSKU_ID" Visible="false"></asp:Literal>
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

                                            <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                                            <br />
                                            <asp:FileUpload ID="FileUpload2" runat="server" AllowMultiple="true" />
                                            <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确认上传？');" runat="server" Text="确认上传" ForeColor="White" BackColor="Blue" CssClass="butt" CommandName="upcancipinimg" CommandArgument='<%# Eval("SKU_ID") %>' />
                                        </td>
                                    </tr>
                                        <td></td>
                                        <td>
                                            <div style="display: flex; align-items: center; gap: 10px;">
                                                <asp:DropDownList 
                                                        ID="cbShifou" 
                                                        runat="server" 
                                                        CssClass="status-filter"
                                                        SelectedValue='<%# Eval("cancipinchulijieguo") %>'>
                                                        <asp:ListItem Value="0">未处理</asp:ListItem>
                                                         <asp:ListItem Value="申请退货">申请退货</asp:ListItem>
                                                         <asp:ListItem Value="申请换货">申退换货</asp:ListItem>
                                                         <asp:ListItem Value="退款成功">退款成功</asp:ListItem>
                                                         <asp:ListItem Value="换货成功">换货成功</asp:ListItem>
                                                    </asp:DropDownList>
                                                <asp:LinkButton ID="btnzd" 
                                                    OnClientClick="JavaScript:return confirm('确定保存？');" 
                                                    runat="server" 
                                                    Text="保存" 
                                                    ForeColor="White" 
                                                    BackColor="Green" 
                                                    CssClass="butt" 
                                                    CommandName="qr" 
                                                    CommandArgument='<%# Eval("SKU_ID") %>' />
                                            </div>
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
