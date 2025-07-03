<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="产品利润表_印尼_管理员.aspx.cs" Inherits="WebApplication11.cg.tb.产品利润表_印尼_管理员" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>产品利润表_印尼_管理员</title>
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
            width: 30%;
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
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h3>当前页面【<span style="color: #37cbc5">产品利润表_印尼_管理员</span>】
                <span style="color: #666666; font-size: 13px; margin-left: 10px; font-weight: normal;">每天0点数据自动更新，此页面展示的是当天0点的数据</span>
            </h3>
            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>
        <div>
            海外仓系统编码：
            <asp:TextBox ID="txtHWCCode" runat="server"></asp:TextBox>
            &nbsp;&nbsp;
            浏览器店铺名：
            <asp:TextBox ID="txtBName" runat="server"></asp:TextBox>
            &nbsp;&nbsp;
            排序方式：
            <asp:DropDownList ID="ddlSort" runat="server">
                <asp:ListItem Value="dingdanshuliang_id ASC">SKUID_订单数量_三个月_印尼 升序</asp:ListItem>
                <asp:ListItem Value="dingdanshuliang_id DESC">SKUID_订单数量_三个月_印尼 降序</asp:ListItem>
                <asp:ListItem Value="conversions ASC">conversions_三个月_印尼 升序</asp:ListItem>
                <asp:ListItem Value="conversions DESC">conversions_三个月_印尼 降序</asp:ListItem>
                <asp:ListItem Value="roars ASC">ROARS_三个月_印尼 升序</asp:ListItem>
                <asp:ListItem Value="roars DESC">ROARS_三个月_印尼 降序</asp:ListItem>
            </asp:DropDownList>
            &nbsp;&nbsp;
            <asp:Button ID="btnSearch" runat="server" Text="查找" BackColor="Red" ForeColor="White" OnClick="btnSearch_Click" />
            <br />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </div>
        <br />
        <table class="ttt">
            <asp:Repeater ID="rplb" runat="server">
                <ItemTemplate>
                    <tr>
                        <td style="width: 4%; text-align: center">
                            <%# Container.ItemIndex + 1 %>
                        </td>
                        <td style="width: 30%; text-align: center">
                            <img src="<%# Eval("sku_img") %>" style="width: 300px; height: 300px;" />
                        </td>
                        <td style="width: 66%">
                            <table class="ttta">
                                <tr>
                                    <td class="bbb">OfferID_1688</td>
                                    <td><%# Eval("OfferID_1688") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">SkuID_1688</td>
                                    <td><%# Eval("SkuID_1688") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">1688采购链接</td>
                                    <td>
                                        <a href='<%# Eval("s1688url") %>' target="_blank"><%# Eval("s1688url") %></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbb">货品标题</td>
                                    <td><%# Eval("huopinbiaoti") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">单价</td>
                                    <td><%# Eval("danjia") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">包装盒价格</td>
                                    <td><%# Eval("baozhuanghe1688jiage1") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">莆田发广东物流费</td>
                                    <td><%# Eval("logistics_pt_to_gd") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">广东发海外仓物流费_印尼</td>
                                    <td><%# Eval("guangdong_warehouse_cost") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">运营编码</td>
                                    <td><%# Eval("yunyingbianma") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">系统编码_海仓_印尼</td>
                                    <td><%# Eval("haiwaicangxitongbianma") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">浏览器店铺名</td>
                                    <td><%# Eval("BName") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">ItemID_印尼</td>
                                    <td><%# Eval("rucangItemID") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">SKUID_印尼</td>
                                    <td><%# Eval("rucangSKUID") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">ItemID_订单数量_三个月_印尼</td>
                                    <td><%# Eval("dingdanshuliang_itemid_id") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">SKUID_订单数量_三个月_印尼</td>
                                    <td><%# Eval("dingdanshuliang_id") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">conversions_三个月_印尼</td>
                                    <td><%# Eval("conversions") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">ACOS_三个月_印尼</td>
                                    <td><%# Eval("acos_3months_id") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">ROARS_三个月_印尼</td>
                                    <td><%# Eval("roars") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">销售价格_印尼</td>
                                    <td><%# Eval("sale_price_id") == DBNull.Value ? "" : String.Format("{0:0}", Convert.ToDouble(Eval("sale_price_id"))) %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">退货数量_itemid_三个月_印尼</td>
                                    <td><%# Eval("tuihuoshuliang_id") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">退货率_itemid_印尼</td>
                                    <td><%# Eval("tuihuolv_id") == DBNull.Value ? "" : String.Format("{0:0.##}", Convert.ToDouble(Eval("tuihuolv_id"))) + "%" %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">综合成本</td>
                                    <td><%# Eval("total_cost") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">毛利润_印尼</td>
                                    <td><%# Eval("maolirun_id") == DBNull.Value ? "" : String.Format("{0:0.##}", Convert.ToDouble(Eval("maolirun_id"))) %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">毛利润率_印尼</td>
                                    <td><%# Eval("maolirunlv_id") == DBNull.Value ? "" : String.Format("{0:0.##}", Convert.ToDouble(Eval("maolirunlv_id")) * 100.0) + "%" %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">数据同步时间</td>
                                    <td><%# Eval("update_time") %></td>
                                </tr>
                                <tr>
                                    <td class="bbb">数据创建时间</td>
                                    <td><%# Eval("upload_time") %></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <div class="pager-container" style="text-align: center; margin: 20px 0; padding: 15px; background-color: #f8f9fa; border-top: 1px solid #e9ecef;">
            <asp:Button ID="btnPrev" runat="server" Text="上一页" OnClick="btnPrev_Click" CssClass="pager-btn" />
            <span style="margin:0 10px">
                跳转至 
                <asp:TextBox ID="txtJumpPage" runat="server" Width="50px" Text="1"></asp:TextBox>
                <asp:Button ID="btnJump" runat="server" Text="GO" OnClick="btnJump_Click" CssClass="pager-btn" />
            </span>
            <span style="margin:0 10px"><asp:Literal ID="litPageInfo" runat="server"></asp:Literal></span>
            <asp:Button ID="btnNext" runat="server" Text="下一页" OnClick="btnNext_Click" CssClass="pager-btn" />
        </div>
    </form>
</body>
</html> 