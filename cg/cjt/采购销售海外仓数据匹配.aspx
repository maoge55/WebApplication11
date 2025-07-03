<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="采购销售海外仓数据匹配.aspx.cs" Inherits="WebApplication11.cg.tb.采购销售海外仓数据匹配" MaintainScrollPositionOnPostback="true"  %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>采购销售海外仓数据匹配</title>
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
                <h3>当前页面【<span style="color: #37cbc5">采购销售海外仓数据匹配</span>】</h3>

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
                海外仓系统编码：
                <asp:TextBox ID="txtHaiWaiCangBianMa" runat="server"></asp:TextBox>
                &nbsp;
                 筛选：
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="status-filter">
                    <asp:ListItem Value="需补充1688OfferID和1688SKUID">需补充1688OfferID和1688SKUID</asp:ListItem>
                    <asp:ListItem Value="需补充1688SKUID">需补充1688SKUID</asp:ListItem>
                    <asp:ListItem Value="补充泰国海外仓系统编码">补充泰国海外仓系统编码</asp:ListItem>
                    <asp:ListItem Value="-1">全部</asp:ListItem>
                </asp:DropDownList>
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
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>
                           <asp:Literal ID="rucangSKUID" runat="server" Text='<%# Eval("rucangSKUID") %>' Visible="False"></asp:Literal>
                                
                           <asp:Literal ID="rucangItemID" runat="server" Text='<%# Eval("rucangItemID") %>' Visible="False"></asp:Literal>
                            <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex+1 %><br />

                            </td>
                            <td style="width: 30%; text-align: center">
                              <img src='<%# Eval("sku_img")%>' style="width:300px" /> 
                            </td>
                            <td>
                                <table class="ttta">

                                    <tr>
                                        <td style="width: 30%" class="bbb">印尼语标题</td>
                                        <td><%# Eval("pname") %></td>

                                    </tr>
                               
                                    <tr>
                                        <td style="width: 30%" class="bbb">海外仓系统编码</td>
                                        <td><%# Eval("haiwaicangxitongbianma") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">店铺名称</td>
                                        <td><%# Eval("BName") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">入仓ItemID</td>
                                        <td><%# Eval("rucangItemID") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">入仓SKUID</td>
                                        <td><%# Eval("rucangSKUID") %></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="bbb">入仓SKUID前台名称</td>
                                        <td><%# Eval("sku_name") %></td>

                                    </tr>
                                    
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688标题</td>
                                        <td><%# Eval("pname_1688") %></td>

                                    </tr>
  
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688OfferID</td>
                                        <td>
                                            <asp:TextBox ID="txtY_1688OfferID" runat="server" Text='<%# Bind("OfferID_1688") %>'  Width="300px"></asp:TextBox>
                                            <asp:RegularExpressionValidator runat="server" 
                                                ControlToValidate="txtY_1688OfferID"
                                                    ValidationExpression="^\d+$"
                                                    ErrorMessage="只能输入数字！"
                                                    ForeColor="Red"
                                                    Display="Dynamic"
                                                />
                                            <span style="color:red; font-size:12px;">（*如需重新找货源，只需先填写1688OfferID）</span>
                                        </td>

                                    </tr>
                                                                        
                                    <tr>
                                        <td style="width: 30%" class="bbb">1688SKUID</td>
                                        <td>
                                            <asp:TextBox ID="txtY_1688SKUID" runat="server" Text='<%# Bind("SkuID_1688") %>'  Width="300px"></asp:TextBox>
                                                                              <asp:RegularExpressionValidator runat="server" 
                                                ControlToValidate="txtY_1688SKUID"
                                                    ValidationExpression="^\d+$"
                                                    ErrorMessage="只能输入数字！"
                                                    ForeColor="Red"
                                                    Display="Dynamic"
                                                />
                                            <span style="color:red; font-size:13px;">（*如果产品是单SKU，那么这里填写1688OfferID）</span>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td style="width: 30%" class="bbb">泰语标题</td>
                                        <td><%# Eval("pname_th") %></td>

                                    </tr>
                                    
                                    <tr>
                                        <td style="width: 30%" class="bbb">海外仓系统编码_泰国</td>
                                        <td>
                                            <asp:TextBox ID="txtHwid_th" runat="server" Text='<%# Bind("hwid_th") %>'  Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td style="width: 30%" class="bbb">店铺名称_泰国</td>
                                        <td><%# Eval("BName_th") %></td>

                                    </tr>
                                    
                                    <tr>
                                        <td style="width: 30%" class="bbb">入仓ItemID_泰国</td>
                                        <td><%# Eval("rucangItemID_th") %></td>

                                    </tr>
                                    
                                    <tr>
                                        <td style="width: 30%" class="bbb">入仓SKUID_泰国</td>
                                        <td><%# Eval("rucangSKUID_th") %></td>

                                    </tr>
                                    
                                    <tr>
                                        <td style="width: 30%" class="bbb">入仓SKUID前台名称_泰国</td>
                                        <td><%# Eval("sku_name_th") %></td>

                                    </tr>
                                    
                                    <tr>
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
                                                    CommandArgument='<%# Eval("rucangSKUID") %>' />
                                                <span style="color:blue; font-size:12px;">（*保存时将同时更新1688信息和泰国海外仓系统编码）</span>
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

    </form>
</body>
</html>
