<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mb.aspx.cs" Inherits="WebApplication11.a" EnableEventValidation="false" %>

<%@ Register Src="~/left.ascx" TagName="left" TagPrefix="ul" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>模板管理</title>
    <style type="text/css">
        .auto-style3 {
            width: 653px;
        }
    </style>
</head>
<body>
    <ul:left ID="left" runat="server" />
    <form id="form1" runat="server">
        <div class="main" style="width: 1480px !important;">
            <div id="lg" runat="server">
                <div class="hdt">
                    <h2>输入单独的管理员密码</h2>
                </div>
                <table class="ttt" id="add">
                    <tr>

                        <td>密码：<asp:TextBox TextMode="Password" runat="server" ID="txtpwd" Width="119px" CssClass="txt"></asp:TextBox>

                            &nbsp;<asp:Button ID="Button8" runat="server" Text="Login" CssClass="butt" OnClick="Button8_Click" /></td>
                    </tr>
                </table>
            </div>
            <div id="zc" runat="server" visible="false">
                <div class="hdt">
                    <h2>模板管理</h2>
                </div>
                <div class="ts">
                    <p>
                        <asp:Literal runat="server" ID="lits"></asp:Literal>
                    </p>
                </div>
                <div>
                    <table class="ttt" id="adda">
                        <tr>
                            <td>模板名称：<asp:TextBox runat="server" ID="txtmbname" Width="119px" CssClass="txt"></asp:TextBox></td>
                            <td>表名：<asp:TextBox runat="server" ID="txttablename" Width="100px" CssClass="txt">et_sheet_template</asp:TextBox></td>
                            <td>列名行：<asp:TextBox runat="server" ID="txtlmrow" Width="50px" CssClass="txt">4</asp:TextBox></td>
                            <td>搜索词：<asp:TextBox runat="server" ID="txtsearchtxt" Width="150px" CssClass="txt"></asp:TextBox></td>
                            <td>模板文件上传：
                        <asp:FileUpload ID="fup1" runat="server" CssClass="butt" /></td>
                            <td>
                                <asp:Button ID="Button1" runat="server" Text="保存" CssClass="butt" OnClick="Button1_Click" />

                            </td>
                        </tr>
                    </table>
                    <br />
                    <table class="ttt" id="lb2">
                        <tr>

                            <td class="auto-style3">
                                <asp:DropDownList ID="drtype" runat="server">
                                    <asp:ListItem Text="所有模版" Value="999">所有模版</asp:ListItem>
                                    <asp:ListItem Text="需处理的模版" Value="0">需处理的模版</asp:ListItem>
                                    <asp:ListItem Text="不能销售的模版" Value="-1">不能销售的模版</asp:ListItem>
                                    <asp:ListItem Text="已设置搜索词" Value="1">已设置搜索词</asp:ListItem>
                                    <asp:ListItem Text="已完成的模版" Value="2">已完成的模版</asp:ListItem>

                                </asp:DropDownList>
                                &nbsp;
                              <asp:DropDownList ID="drpd" runat="server">
                                  <asp:ListItem Text="所有模版" Value="999">所有模版</asp:ListItem>
                                  <asp:ListItem Text="有产品的模板" Value="1">有产品的模板</asp:ListItem>
                                  <asp:ListItem Text="无产品的模板" Value="0">无产品的模板</asp:ListItem>

                              </asp:DropDownList>
                                <asp:DropDownList ID="drxl" runat="server">
                                    <asp:ListItem Text="所有模版" Value="999">所有模版</asp:ListItem>
                                    <asp:ListItem Text="有销量的模板" Value="1">有销量的模板</asp:ListItem>
                                    <asp:ListItem Text="无销量的模板" Value="0">无销量的模板</asp:ListItem>

                                </asp:DropDownList>

                                <asp:DropDownList ID="drcolor" runat="server">
                                    <asp:ListItem Text="所有模版" Value="999">所有模版</asp:ListItem>
                                      <asp:ListItem Text="所有模版" Value="0">未分类模版</asp:ListItem>
                                    <asp:ListItem Text="白名单模板" Value="1">白名单模板</asp:ListItem>
                                    <asp:ListItem Text="黑名单模板" Value="2">黑名单模板</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="DDDtype" runat="server">
                                    <asp:ListItem Text="所有模版" Value="999">所有模版</asp:ListItem>
                                    <asp:ListItem Text="普货模版" Value="1">普货模版</asp:ListItem>
                                    <asp:ListItem Text="POD模版" Value="2">POD模版</asp:ListItem>
                                    <asp:ListItem Text="有产品的模板" Value="0">未分类模版</asp:ListItem>
                                </asp:DropDownList>
                                <br />
                                区间：<asp:TextBox runat="server" ID="txtqj" Width="50px" CssClass="txt"></asp:TextBox>
                                名称：<asp:TextBox runat="server" ID="txtsname" Width="76px" CssClass="txt"></asp:TextBox>&nbsp;
                             <asp:Button ID="btnqjjz" runat="server" Text="查找" CssClass="butt" OnClick="btnqjjz_Click" />
                            </td>
                            <td>
                                <asp:Button ID="Button2" runat="server" Text="全选" CssClass="butt" OnClick="Button2_Click4" />&nbsp;
                            <asp:Button ID="Button3" runat="server" Text="反选" CssClass="butt" OnClick="Button3_Click" />&nbsp;
                              <asp:Button ID="Button4" OnClientClick="JavaScript:return confirm('确定改成已设置搜索词吗？');" runat="server" Text="改成已设置搜索词" BackColor="#00ffc1" CssClass="butt" OnClick="Button4_Click" />&nbsp;
                              <asp:Button ID="Button5" OnClientClick="JavaScript:return confirm('确定改成已完成吗？');" runat="server" Text="改成已完成" BackColor="#72b5d3" CssClass="butt" OnClick="Button5_Click" />&nbsp;
                             <asp:Button ID="Button7" OnClientClick="JavaScript:return confirm('确定改成需处理的模版吗？');" runat="server" Text="改成需处理的模版" BackColor="#ffcf2b" CssClass="butt" OnClick="Button7_Click" />&nbsp;
                              <asp:Button ID="Button6" OnClientClick="JavaScript:return confirm('确定改成不能销售吗？');" runat="server" Text="改成不能销售" BackColor="#f94db6" CssClass="butt" OnClick="Button6_Click" />&nbsp;
                              <asp:Button ID="Button9" OnClientClick="JavaScript:return confirm('确定改成普货模版吗？');" runat="server" Text="改成普货模版" BackColor="#dadbdb" ForeColor="red" CssClass="butt" OnClick="Button9_Click" />&nbsp;
                                <asp:Button ID="Button10" OnClientClick="JavaScript:return confirm('确定改成POD模版吗？');" runat="server" Text="改成POD模版" BackColor="#040404" ForeColor="white" CssClass="butt" OnClick="Button10_Click" />&nbsp;
                                  <asp:Button ID="Button11" OnClientClick="JavaScript:return confirm('确定改成白名单吗？');" runat="server" Text="改成白名单" BackColor="White" ForeColor="Black" CssClass="butt" OnClick="Button11_Click" />&nbsp;
                                 <asp:Button ID="Button12" OnClientClick="JavaScript:return confirm('确定改成黑名单吗？');" runat="server" Text="改成黑名单" BackColor="#040404" ForeColor="white" CssClass="butt" OnClick="Button12_Click" />&nbsp;
                                <asp:Button ID="Button13" OnClientClick="JavaScript:return confirm('确定去掉黑白吗？');" runat="server" Text="去掉黑白" BackColor="#0033CC" ForeColor="white" CssClass="butt" OnClick="Button13_Click"  />&nbsp;
                            </td>
                        </tr>
                    </table>
                    <table class="ttt" id="lb">
                        <tr class="head">
                            <td>ID</td>
                            <td>模板名字</td>
                            <td>文档名字</td>
                            <td>表格名字</td>
                            <td>字段行</td>
                            <td>搜索词</td>
                            <td>状态</td>
                            <td>操作</td>
                        </tr>
                        <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand" OnItemDataBound="rplb_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 5%">
                                        <asp:CheckBox ID="ckxz" runat="server" />
                                        <%#Eval("did") %></td>
                                    <td style="width: 15%">
                                        <asp:Literal ID="lidid" Visible="false" runat="server" Text='<%#Eval("did") %>'></asp:Literal>
                                        <asp:Literal ID="liname" runat="server" Text='<%#Eval("dname") %>'></asp:Literal>
                                        <span style="color: blue"><%# Eval("pcount") %>-<%# Eval("pxl") %></span>
                                        <asp:Literal ID="liysj" runat="server" Visible='<%#Eval("count").ToString()=="0"?false:true %>' Text="<span style='color:red'>(*)</span>"></asp:Literal>
                                        <asp:TextBox ID="txtname" CssClass="txt" Width="80%" runat="server" Visible="false" Text='<%#Eval("dname") %>'></asp:TextBox>

                                    </td>
                                    <td style="width: 20%">
                                        <a href="/document<%# uid+"/"+ getpathname(Eval("dfile").ToString()) %>" target="_blank">
                                            <asp:Literal ID="lipath" runat="server" Text='<%# getpathname(Eval("dfile").ToString()) %>'></asp:Literal></a>
                                        <asp:FileUpload ID="fpp" Visible="false" runat="server" CssClass="butt" CommandName="upimg" CommandArgument='<%# Eval("did") %>' />
                                        <asp:Literal ID="liallpath" Visible="false" runat="server" Text='<%# Eval("dfile").ToString()%>'></asp:Literal>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:Literal ID="litablename" runat="server" Text='<%#Eval("dtablename") %>'></asp:Literal>
                                        <asp:TextBox ID="txttablename" CssClass="txt" Width="90%" runat="server" Visible="false" Text='<%#Eval("dtablename") %>'></asp:TextBox>

                                    </td>
                                    <td style="width: 5%">
                                        <asp:Literal ID="lirow" runat="server" Text='<%#Eval("drow") %>'></asp:Literal>
                                        <asp:TextBox ID="txtrow" CssClass="txt" Width="50%" runat="server" Visible="false" Text='<%#Eval("drow") %>'></asp:TextBox>

                                    </td>
                                    <td style="width: 15%; word-wrap: break-word; word-break: break-all;">
                                        <asp:Literal ID="lisearchtxt" runat="server" Text='<%#getpathname_(Eval("dsearchtxt").ToString()) %>'></asp:Literal>
                                        <asp:TextBox TextMode="MultiLine" ID="txtsearchtxt" CssClass="txt" Width="90%" runat="server" Visible="false" Text='<%#Eval("dsearchtxt") %>'></asp:TextBox>

                                    </td>
                                    <td style="width: 10%">
                                        <%#getzt(Eval("dstate").ToString()) %>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:LinkButton ID="btnup" runat="server" Text="修改" ForeColor="Green" CssClass="butt" CommandName="update" CommandArgument='<%# Eval("did") %>' />
                                        <asp:LinkButton ID="btnsave" Visible="false" runat="server" Text="保存" ForeColor="#0600ff" CssClass="butt" CommandName="save" CommandArgument='<%# Eval("did") %>' />
                                        &nbsp;
                                    <asp:LinkButton ID="btnzd" runat="server" Text="指定" ForeColor="#b513f5" CssClass="butt" CommandName="zd" CommandArgument='<%# Eval("did") %>' />
                                        &nbsp;
                                    <asp:LinkButton ID="btndell" runat="server" Text="删除" OnClientClick="JavaScript:return confirm('确定删除吗？');" ForeColor="Red" CssClass="butt" CommandName="del" CommandArgument='<%# Eval("did") %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
