<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="点击任务表.aspx.cs" Inherits="WebApplication11.cg.cjt.点击任务表" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>刷点击任务表</title>
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
                <h3>当前页面【<span style="color: #37cbc5">刷点击任务表</span>】</h3>

<div>
    <asp:TextBox ID="txtSJBM" runat="server" PlaceHolder="输入商家编码" style="margin-right:10px;"></asp:TextBox>

    <asp:TextBox ID="txtBID" runat="server" PlaceHolder="输入浏览器名称" style="margin-right:10px;"></asp:TextBox>
   
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="status-filter">
                    <asp:ListItem Value="有任务ItemID">有任务ItemID</asp:ListItem>
                    <asp:ListItem Value="-1">全部</asp:ListItem>
                
                     <asp:ListItem Value="无任务ItemID">无任务ItemID</asp:ListItem>
                       
                </asp:DropDownList>
                &nbsp;
    <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
</div>

<div id="modal" style="display:none; position:fixed; top:50%; left:50%; transform:translate(-50%,-50%); background:#ffffcc; border:2px solid #37cbc5; padding:20px; box-shadow:0 0 10px rgba(0,0,0,0.3);">
    <div style="position: relative;border-bottom:1px solid #37cbc5;margin-bottom:10px;">
        <h3 style="color: #37cbc5;margin:0 0 10px 0;">查询结果</h3>
        <button onclick="closeModal()" 
            style="position:absolute; top:-10px; right:-10px; padding:0; background:none; border:none; cursor:pointer; font-size:28px; color:#ff0000; width:40px; height:40px;">
            ×
        </button>
    </div>
    <div id="modalContent" style="margin:10px 0;height:400px;overflow-y:auto;">
        <asp:Repeater ID="rpModal" runat="server">
            <HeaderTemplate>
                <table class="ttta" style="width:100%">
                    <tr style="background:#f5f5f5;">
                         <th>序号</th>
                        <th>图片</th>
                        <th>浏览器名称</th>
                        <th>产品标题</th>
                        <th>产品链接</th>
                        <th>商家编码</th>
                        <th>ItemID</th>
                        <th>每天点击量</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td style="width: 4%; text-align: center">
                                <%# Container.ItemIndex + 1 %><br />
                     </td>
                     <td style="width: 30%; text-align: center">
                              <img src='<%# Eval("pimage")%>' style="width:300px" /> 
                            </td>
                    <td><%# Eval("BID") %></td>
                    <td><%# Eval("pname") %></td>
                    <td><a href="<%# Eval("purl") %>" target="_blank" style="color:#37cbc5; text-decoration:underline;">查看链接</a></td>
                    <td><%# Eval("SJBM") %></td>
                               <td>
                <asp:Literal ID="litItemID" runat="server" Text='<%# Eval("ItemID") %>' />
            </td>
                     <td><asp:TextBox ID="txtY_click" runat="server" Text='<%# Eval("click_count") %>'></asp:TextBox></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div style="text-align:right;margin-top:10px;">
        <asp:Button ID="btnSaveAll" runat="server" Text="全部保存" 
            OnClick="btnSaveAll_Click" 
            CssClass="butt" 
            OnClientClick="return confirm('确定保存所有修改吗？');"
            style="margin-right:10px;"/>
        <button onclick="closeModal()" 
            style="padding:5px 15px; background:#37cbc5; color:white; border:none;">
            关闭
        </button>
    </div>
</div>


                <h2 style="color: blue">
                    <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
            </div>
            <div>


                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            </div>
            <br />
         
                <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                    <ItemTemplate>
                        <tr>
                          
                     
                      
                            <td>
                                <table class="ttta">

                                   <tr>
                                        <td style="width: 30%" class="bbb">ItemID</td>
                                        <td>
                                            <asp:TextBox ID="txtY_ItemID" runat="server" Text='<%# Eval("ItemID") %>'></asp:TextBox>
                                           
                                        </td>
                                    </tr>

                                 <tr>
                                        <td style="width: 30%" class="bbb">每天点击数量</td>
                                        <td>
                                            <asp:TextBox ID="txtY_click" runat="server" Text='<%# Eval("click_count") %>'></asp:TextBox>
                                        
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
                                                    CommandArgument='<%# Eval("ItemID") %>' />
                                                &nbsp;&nbsp;
                                            </td>
                                        </tr>
                                </table>  

                            </td>

                        </tr>
                        <tr></tr>
                    </ItemTemplate>
                </asp:Repeater>

        </div>
    </form>
</body>
</html>
<script>
    let isDragging = false;
    let currentX;
    let currentY;
    let initialX;
    let initialY;
    let xOffset = 0;
    let yOffset = 0;
    let modal = null;

    function initDrag() {
        modal = document.getElementById('modal');
        modal.addEventListener('mousedown', dragStart);
        document.addEventListener('mousemove', drag);
        document.addEventListener('mouseup', dragEnd);
    }

    function dragStart(e) {
        if(e.target.tagName !== 'BUTTON') { // 排除按钮的拖动
            initialX = e.clientX - xOffset;
            initialY = e.clientY - yOffset;
            
            if (e.target === modal) {
                isDragging = true;
            }
        }
    }

    function drag(e) {
        if (isDragging) {
            e.preventDefault();
            currentX = e.clientX - initialX;
            currentY = e.clientY - initialY;
            
            xOffset = currentX;
            yOffset = currentY;
            
            setTranslate(currentX, currentY, modal);
        }
    }

    function setTranslate(xPos, yPos, el) {
        el.style.transform = "translate3d(" + xPos + "px, " + yPos + "px, 0)";
    }

    function dragEnd(e) {
        initialX = currentX;
        initialY = currentY;
        isDragging = false;
    }

    function closeModal() {
        document.getElementById('modal').style.display = 'none';
        // 重置位置
        setTranslate(0, 0, modal);
        xOffset = 0;
        yOffset = 0;
    }

    // 初始化拖动功能
    window.addEventListener('load', initDrag);
</script>
