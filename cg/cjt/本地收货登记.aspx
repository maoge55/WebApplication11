<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="本地收货登记.aspx.cs" Inherits="WebApplication11.cg.cjt.本地收货登记" MaintainScrollPositionOnPostback="true"  %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>本地收货登记</title>
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
                <h3>当前页面【<span style="color: #37cbc5">本地收货登记</span>】</h3>

                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>
                    状态筛选：
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="status-filter">
                    <asp:ListItem Value="等待买家确认收货">等待买家确认收货</asp:ListItem>
                    <asp:ListItem Value="-1">全部状态</asp:ListItem>
                     <asp:ListItem Value="等待买家付款">等待买家付款</asp:ListItem>
                      
                       <asp:ListItem Value="等待卖家发货">等待卖家发货</asp:ListItem>
                        <asp:ListItem Value="交易成功">交易成功</asp:ListItem>
                        <asp:ListItem Value="交易关闭">交易关闭</asp:ListItem>
                    <asp:ListItem Value="退款中">退款中</asp:ListItem>
                    <asp:ListItem Value="terminated">terminated</asp:ListItem>
                </asp:DropDownList>
                输入运营编码：
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
                快递单号：
                <asp:TextBox ID="txtYundanhao" runat="server"></asp:TextBox>
                &nbsp;
                标题搜索词：
                <asp:TextBox ID="txtHuopinbiaoti" runat="server"></asp:TextBox>
                &nbsp;
                1688单号：
                <asp:TextBox ID="txtDingdanbianhao" runat="server"></asp:TextBox>
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
                      
                         <asp:Literal ID="litTupianwangzhi" runat="server" Text='<%# Eval("图片网址") %>' Visible="false" />
                        <asp:Literal ID="litShipinwangzhi" runat="server" Text='<%# Eval("视频网址") %>' Visible="false" />
                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />

                            </td>
                          <td style="width: 30%; text-align: center">
                        <img 
                            src='<%# 
                                Eval("SKU图片") != null && Eval("SKU图片").ToString().ToLower().StartsWith("http") 
                                ?"ImageProxy.aspx?url=" + Convert.ToString(Eval("SKU图片")) 
                                : ResolveUrl(Convert.ToString(Eval("SKU图片")))
                            %>' 
                            style="width:300px" />

                         </td>
                            <td>
                                <table class="ttta">
                                    <tr>
                                        <td style="width: 30%" class="bbb">订单号</td>
                                        <td><%# Eval("订单号") %></td>
                                         <asp:Literal ID="订单号" runat="server" Text='<%# Eval("订单号") %>' Visible="False" ></asp:Literal>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">物流公司</td>
                                        <td><%# Eval("物流公司") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">运单号</td>
                                        <td><%# Eval("运单号") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">标题</td>
                                        <td><%# Eval("标题") %></td>

                                    </tr>

                                  <tr>
                                            <td style="width: 30%" class="bbb">1688链接</td>
                                            <td>
                             
                                                <a href="https://detail.1688.com/offer/<%# Eval("OfferID") %>.html" target="_blank">打开网址</a>&nbsp;&nbsp;
        
                                                <input type="text" value="https://detail.1688.com/offer/<%# Eval("OfferID") %>.html" style="width: 20px" id="url<%# Container.ItemIndex+1 %>" />
        
                                                <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">


                                                                                                                         
                                     <asp:LinkButton runat="server" 
                                        PostBackUrl='<%# "~/cg/cjt/采购订单补充资料.aspx?OfferID=" + Eval("OfferID") %>'
                                        Text="补充1688_Offer_ID长宽高_包装盒" 
                                        BackColor="Green" 
                                        ForeColor="White"
                                        CssClass="butt" />
                                       
                                            </td>

                                        </tr>

                                   <tr>
     
                                          <tr>
                                        <td style="width: 30%" class="bbb">1688_SKU_ID</td>
                                        <td><%# Eval("SKU_ID") %>                                            
                                            <asp:LinkButton runat="server" 
                                                PostBackUrl='<%# "~/cg/cjt/采购订单补充资料.aspx?SKU_ID=" + Eval("SKU_ID") %>'
                                                Text="补充1688_SKU_ID长宽高_包装盒" 
                                                       BackColor="Green" 
                                        ForeColor="White"
                                        CssClass="butt"/></td>
                                      

                                        
                                    </tr>
                                    
                                    </tr>
                                   <tr>
                                        <td style="width: 30%" class="bbb">数量</td>
                                        <td><%# Eval("数量") %></td>

                                    </tr>
                                     
                 
                                    
                                    <tr>
                                        <td style="width: 30%" class="bbb">单位</td>
                                        <td><%# Eval("单位") %></td>
                                    </tr>
                                                                        
                                    <tr>
                                        <td style="width: 30%" class="bbb">实际收货数量</td>
                                        <td>
                                            <asp:TextBox ID="txtY_true" runat="server"  Text='<%# Eval("实际收货数量") %>'></asp:TextBox>
                                        </td>
                                    </tr>
                                                                        
                                    <tr>
                                        <td style="width: 30%" class="bbb">残次品数量</td>
                                        <td>
                                            <asp:TextBox ID="txtY_pool" runat="server" Text='<%# Eval("残次品数量") %>'></asp:TextBox>
                                        </td>
                                    </tr>
                                                                                 
                                    <tr>
                                        <td style="width: 30%" class="bbb">订单备注</td>
                                        <td>
                                            <asp:TextBox ID="txtY_ding" runat="server" Text='<%# Eval("订单备注") %>'></asp:TextBox>
                                        </td>
                                    </tr>

                                     <tr>
                                        <td style="width: 30%" class="bbb">数据类型</td>
                                        <td>
                                            <div style="display: flex; align-items: center; gap: 10px;">
                                                <asp:DropDownList 
                                                        ID="shujuleixing" 
                                                        runat="server" 
                                                        CssClass="status-filter"
                                                        SelectedValue='<%# Eval("数据类型") %>'>
                                                        <asp:ListItem Value="">请选择</asp:ListItem>
                                                        <asp:ListItem Value="发仓货物">发仓货物</asp:ListItem>
                                                        <asp:ListItem Value="阿里狗">阿里狗</asp:ListItem>
                                                        
                                                        <asp:ListItem Value="耗材">耗材</asp:ListItem>
                                                    </asp:DropDownList>
                                            </div>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td></td>
                                        <td>
                                            <div style="display: flex; align-items: center; gap: 10px;">
                                                <asp:DropDownList 
                                                        ID="dingdanzhuangtai" 
                                                        runat="server" 
                                                        CssClass="status-filter"
                                                        SelectedValue='<%# Eval("订单状态") %>'>
                                                        <asp:ListItem Value="等待买家确认收货">等待买家确认收货</asp:ListItem>
                                                        <asp:ListItem Value="等待买家付款">等待买家付款</asp:ListItem>
                                                        
                                                        <asp:ListItem Value="等待卖家发货">等待卖家发货</asp:ListItem>
                                                        <asp:ListItem Value="交易成功">交易成功</asp:ListItem>
                                                        <asp:ListItem Value="交易关闭">交易关闭</asp:ListItem>
                                                                        <asp:ListItem Value="退款中">退款中</asp:ListItem>
                                                         <asp:ListItem Value="terminated">terminated</asp:ListItem>
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
                        
                                   <tr>
                                        <td class="bbb">SKU图片上传：</td>
                                        <td>
                                            <asp:FileUpload ID="FileUpload3" runat="server" AllowMultiple="true" />
                                            <asp:LinkButton ID="LinkButton3" 
                                                OnClientClick="JavaScript:return confirm('确认上传SKU图片？');" 
                                                runat="server" 
                                                Text="上传SKU图片" 
                                                ForeColor="White" 
                                                BackColor="Purple" 
                                                CssClass="butt" 
                                                CommandName="upskuimg" 
                                                CommandArgument='<%# Eval("SKU_ID") %>' />
                                        </td>
                                    </tr>

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
                                            <asp:LinkButton ID="LinkButton2" OnClientClick="JavaScript:return confirm('确认上传？');" runat="server" Text="确认上传" ForeColor="White" BackColor="Blue" CssClass="butt" CommandName="upimg" CommandArgument='<%# Eval("SKU_ID") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="bbb">视频网址：</td>
                                          <td>

                                            <asp:DataList ID="DataList2" DataKeyField="imgname" runat="server"
                                                OnItemCommand="DataList2_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:Literal Text='<%# Eval("SKU_ID").ToString() %>' runat="server" ID="liimgSKU_ID" Visible="false"></asp:Literal>
                                                    <table bgcolor="black" cellpadding="4" cellspacing="1">
                                                        <tr>
                                                            <td bgcolor="white" width="60px">
                                                                <a href='<%# "/Uploads/"+Eval("imgname").ToString()%>' target="_blank">
                                                                    <%# "/Uploads/"+Eval("imgname").ToString()%></a>
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
                                            <asp:LinkButton ID="LinkButton1" OnClientClick="JavaScript:return confirm('确认上传？');" runat="server" Text="确认上传" ForeColor="White" BackColor="Blue" CssClass="butt" CommandName="upsp" CommandArgument='<%# Eval("SKU_ID") %>' />
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
