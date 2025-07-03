<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="采集数据_1688_运营.aspx.cs" Inherits="WebApplication11.cg.cjt.采集数据_1688_运营" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>采集数据_1688_运营</title>
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
        /* 新增弹出框样式 - 关键修复 */
        .popup-overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
            display: none; /* 确保默认隐藏 */
            z-index: 1000;
        }

        .popup-content {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background: white;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0,0,0,0.3);
            width: 400px;
        }

        .popup-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-bottom: 1px solid #eee;
            padding-bottom: 10px;
            margin-bottom: 15px;
        }

        .close-btn {
            cursor: pointer;
            font-size: 20px;
        }

        .form-group {
            margin-bottom: 15px;
        }

            .form-group label {
                display: block;
                margin-bottom: 5px;
                font-weight: bold;
            }

            .form-group input {
                width: 95%;
                padding: 8px;
                border: 1px solid #ddd;
                border-radius: 4px;
            }

        .popup-footer {
            display: flex;
            justify-content: flex-end;
            gap: 10px;
            margin-top: 20px;
        }


        #downloadLinkTable {
            width: 100%;
            border-collapse: collapse;
            table-layout: fixed; /* 均匀分配宽度 */
        }

        #downloadLinkTable th,
        #downloadLinkTable td {
            padding: 10px 12px;
            text-align: center;
            border: 1px solid #ddd;
            font-size: 14px;
            word-break: break-all;
        }

        /* 可选：设置每列比例（可调） */
        #downloadLinkTable th:nth-child(1),
        #downloadLinkTable td:nth-child(1) {
            width: 15%;
        }

        #downloadLinkTable th:nth-child(2),
        #downloadLinkTable td:nth-child(2) {
            width: 15%;
        }

        #downloadLinkTable th:nth-child(3),
        #downloadLinkTable td:nth-child(3) {
            width: 20%;
        }

        #downloadLinkTable th:nth-child(4),
        #downloadLinkTable td:nth-child(4) {
            width: 20%;
        }

        #downloadLinkTable th:nth-child(5),
        #downloadLinkTable td:nth-child(5) {
            width: 20%;
        }

        #downloadLinkTable th:nth-child(6),
        #downloadLinkTable td:nth-child(6) {
            width: 10%;
            }
    </style>
    <script>

        let downloadData = []; // 全部数据
        let currentPage = 1;
        const pageSize = 3;
        // 复制URL函数保持不变
        function copyUrl(myurl) {
            var Url2 = document.getElementById(myurl);
            Url2.select();
            document.execCommand("Copy");
        }
        //全选功能
        function checkAll(obj) {
            var items = document.querySelectorAll("input[id*='chkItem']");
            for (var i = 0; i < items.length; i++) {
                items[i].checked = obj.checked;
            }
        }
        // 新增弹出框控制函数 - 确保页面加载时隐藏
        document.addEventListener("DOMContentLoaded", function () {
            // 页面加载时确保弹出框隐藏
            document.getElementById('popupOverlay').style.display = 'none';
        });

        function showPopup() {
            document.getElementById('popupOverlay').style.display = 'block';
        }

        function hidePopup() {
            document.getElementById('popupOverlay').style.display = 'none';
        }

        // 点击窗口外部关闭弹出框
        window.onclick = function (event) {
            var popup = document.getElementById('popupOverlay');
            if (event.target == popup) {
                popup.style.display = 'none';
            }
        }

        function renderDownloadTable(page = 1) {
            currentPage = page;
            const start = (page - 1) * pageSize;
            const end = start + pageSize;
            const pageData = downloadData.slice(start, end);

            let html = "";

            if (pageData.length === 0) {
                html = "<p style='color:#999;'>暂无下载链接</p>";
            } else {
                html += "<table id='downloadLinkTable' style='width:100%; border-collapse:collapse;' border='1'>";
                html += "<tr style='background:#f5f5f5; font-weight:bold;'>";
                html += "<td style='padding:5px;'>运营编码</td><td style='padding:5px;'>任务状态</td><td style='padding:5px;'>提交时间</td><td style='padding:5px;'>完成时间</td><td style='padding:5px;'>下载链接</td><td style='padding:5px;'>操作</td></tr>";

                pageData.forEach(item => {
                    let statusText = "未知状态";
                    if (item.status === 0) statusText = "任务未启动";
                    else if (item.status === 1) statusText = "任务正在进行";
                    else statusText = "已生成下载链接";

                    html += "<tr>";
                    html += `<td style='padding:5px;'>${item.yybm}</td>`;
                    html += `<td style='padding:5px;'>${statusText}</td>`;
                    html += `<td style='padding:5px;'>${item.start_time || ''}</td>`;
                    html += `<td style='padding:5px;'>${item.end_time || ''}</td>`;
                    html += `<td style='padding:5px; word-break: break-all;'><a href='${item.download_url}' target='_blank'>${item.download_url}</a></td>`;
                    html += `<td style='padding:5px;'><button type="button" onclick="window.open('${item.download_url}', '_blank')">下载</button></td>`;
                    html += "</tr>";
                });

                html += "</table>";

                // 分页控制区
                const totalPages = Math.ceil(downloadData.length / pageSize);
                html += `<div style="margin-top:10px; text-align:right;">`;
                html += `<button onclick="renderDownloadTable(${Math.max(1, currentPage - 1)})">上一页</button>`;
                html += ` <span>第 ${currentPage} 页 / 共 ${totalPages} 页</span> `;
                html += `<button onclick="renderDownloadTable(${Math.min(totalPages, currentPage + 1)})">下一页</button> `;
                html += `<input type="number" id="jumpPageInput" min="1" max="${totalPages}" value="${currentPage}" style="width:50px;"/>`;
                html += `<button onclick="jumpToPage()">跳转</button>`;
                html += `</div>`;
            }

            document.getElementById("downloadContent").innerHTML = html;
        }


        function jumpToPage() {
            const input = document.getElementById("jumpPageInput");
            const page = parseInt(input.value);
            const totalPages = Math.ceil(downloadData.length / pageSize);
            if (!isNaN(page) && page >= 1 && page <= totalPages) {
                renderDownloadTable(page);
            } else {
                alert("请输入有效页码！");
            }
        }


        function showDownloadPopup() {
            var yybm = document.getElementById("txtsjbm").value.trim();
            if (!yybm) {
                alert("请输入运营编码！");
                return;
            }

            fetch('采集数据_1688_运营.aspx/GetDownloadList', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ yybm: yybm })
            })
                .then(response => response.json())
                .then(data => {
                    downloadData = data.d || [];
                    renderDownloadTable(1); // 渲染第1页
                    document.getElementById("popupDownloadOverlay").style.display = "block";
                })
                .catch(error => {
                    console.error("获取失败", error);
                    alert("加载下载链接失败");
                });
        }

        function hideDownloadPopup() {
            document.getElementById('popupDownloadOverlay').style.display = 'none';
        }
        function copyToClipboard(text) {
            navigator.clipboard.writeText(text)
                .then(() => alert("链接已复制！"))
                .catch(err => console.error('复制失败:', err));
        }

        function checkAndShowPopup() {
            var yybm = document.getElementById("txtsjbm").value.trim();
            if (!yybm) {
                alert("请输入运营编码！");
                return false;
            }
            document.getElementById("txtPopupYYBM").value = yybm
            showPopup();
            return false; // 阻止默认提交
        }

        function checkAndShowDownloadPopup() {
            var yybm = document.getElementById("txtsjbm").value.trim();
            if (!yybm) {
                alert("请输入运营编码！");
                return false;
            }
            showDownloadPopup();
            return false; // 阻止默认提交
        }
        function is_pulish_stask() {
            var yybmElement = document.getElementById("txtPopupYYBM");
            var perCountElement = document.getElementById("txtQuantity");
            var fileCountElement = document.getElementById("txtFileCount");

            if (!yybmElement || !perCountElement || !fileCountElement) {
                alert("找不到输入框，请检查控件 ID 设置是否正确！");
                return false;
            }

            var yybm = yybmElement.value.trim();
            var per_count = perCountElement.value.trim();
            var file_count = fileCountElement.value.trim();
            

            if (!yybm || !per_count || !file_count) {
                alert("请输入必填字段！");
                return false;
            }
            return true;
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">

        <div>
            <h3>当前页面【<span style="color: #37cbc5">采集数据_1688_运营</span>】</h3>

            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal></h2>
        </div>
        <div>
            输入运营编码：
                <asp:TextBox ID="txtsjbm" runat="server" ClientIDMode="Static" ValidationGroup="searchGroup"></asp:TextBox>
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
                 数据状态：
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="status-filter">
                    <asp:ListItem Value="未处理">未处理</asp:ListItem>
                    <asp:ListItem Value="-1">全部状态</asp:ListItem>
                    <asp:ListItem Value="可导出">可导出</asp:ListItem>
                    <asp:ListItem Value="不可导出">不可导出</asp:ListItem>
                    <asp:ListItem Value="不确定">不确定</asp:ListItem>
                    <asp:ListItem Value="多型号标题">多型号标题</asp:ListItem>
                    <asp:ListItem Value="可广告测品">可广告测品</asp:ListItem>
                </asp:DropDownList>

            输入标题：
                <asp:TextBox ID="ddlkeywords" runat="server" ValidationGroup="searchGroup"></asp:TextBox>
            <asp:Button ID="Button1" runat="server"
                Text="查找"
                BackColor="Red"
                ForeColor="White"
                OnClick="Button1_Click"
                ValidationGroup="searchGroup" />

            <asp:Button ID="Button2" runat="server" ForeColor="Blue" OnClientClick="JavaScript:return confirm('确定保存整页吗？');" Text="保存整页" OnClick="Button2_Click1" />

            <asp:Button ID="btnAddTask" runat="server" Text="导出用于广告测品_印尼Shopee"
                OnClientClick="return checkAndShowPopup();"
                ForeColor="White" BackColor="#37cbc5" />
            <!-- 新增的查看下载链接按钮 -->
            <asp:Button ID="btnShowDownloads" runat="server" Text="查看下载链接"
                OnClientClick="return checkAndShowDownloadPopup();"
                ForeColor="White" BackColor="#37cbc5" />

            <br />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </div>

        <br />
        <!-- 表格和其他内容保持不变 -->
        <div id="popupOverlay" class="popup-overlay">
            <div class="popup-content">
                <div class="popup-header">
                    <h3>添加导出任务</h3>
                    <span class="close-btn" onclick="hidePopup()">×</span>
                </div>
                <div class="form-group">
                    <label for="txtPopupKeywords">关键词(选填)：</label>
                    <asp:TextBox ID="txtKeywords"  runat="server"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtbid">ADS浏览器ID(选填)：</label>
                    <asp:TextBox ID="txtbid"  ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtPopupYYBM">运营编码(必填)：</label>
                    <asp:TextBox ID="txtPopupYYBM"  ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtQuantity">数量(必填)：</label>
                    <asp:TextBox ID="txtQuantity"  ClientIDMode="Static" runat="server" TextMode="Number"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtFileCount">文件数量(必填)：</label>
                    <asp:TextBox ID="txtFileCount"  ClientIDMode="Static" runat="server" TextMode="Number"></asp:TextBox>
                </div>



                <div class="popup-footer">
                    <asp:Button ID="btnPopupSave" runat="server" Text="上传任务"
                        OnClientClick="return is_pulish_stask();"  OnClick="btnPopupSave_Click" CssClass="butt" BackColor="#4CAF50" ForeColor="White" />
                    <asp:Button ID="btnPopupCancel" runat="server" Text="取消"
                        OnClientClick="hidePopup(); return false;" CssClass="butt" />
                </div>
            </div>
        </div>
        <!-- 下载链接弹出层 -->
        <div id="popupDownloadOverlay" class="popup-overlay">
            <div class="popup-content">
                <div class="popup-header">
                    <h3>下载链接列表</h3>
                    <span class="close-btn" onclick="hideDownloadPopup()">×</span>
                </div>
                <div id="downloadContent" class="form-group">
                </div>
                <div class="popup-footer">
                    <asp:Button ID="btnCloseDownload" runat="server" Text="刷新"
                        OnClientClick="showDownloadPopup(); return false;" CssClass="butt" />
                </div>
            </div>
        </div>

        <table class="ttt">
            <asp:Repeater ID="rplb" runat="server" OnItemCommand="rplb_ItemCommand">
                <HeaderTemplate>
                    <tr>
                        <td style="width: 3%">
                            <input type="checkbox" id="chkAll" onclick="checkAll(this)" />
                        </td>
                        <td colspan="4">
                            <table style="width: 20%; border-collapse: collapse;">
                                <!-- 嵌套表格实现垂直布局 -->
                                <tr style="background-color: #f0f8ff">
                                    <td style="padding: 4px 0;">
                                        <asp:DropDownList ID="ddlBatchsj" runat="server" Width="20%">
                                            <asp:ListItem Value="">请选择数据状态</asp:ListItem>

                                            <asp:ListItem Value="可导出">可导出</asp:ListItem>
                                            <asp:ListItem Value="不可导出">不可导出</asp:ListItem>
                                            <asp:ListItem Value="未处理">未处理</asp:ListItem>
                                            <asp:ListItem Value="不确定">不确定</asp:ListItem>
                                            <asp:ListItem Value="多型号标题">多型号标题</asp:ListItem>
                                            <asp:ListItem Value="可广告测品">可广告测品</asp:ListItem>

                                        </asp:DropDownList>
                                        数据状态
                </td>
                                </tr>
                                <tr style="background-color: #f0f8ff">
                                    <td style="padding: 4px 0; text-align: right;">
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
                        <td style="width: 3%">
                            <asp:CheckBox ID="chkItem" runat="server" />
                        </td>
                        <asp:HiddenField ID="OfferID" runat="server" Value='<%# Eval("OfferID") %>' />
                        <td style="width: 4%; text-align: center">
                            <%# Container.ItemIndex+1 %><br />

                        </td>
                        <td style="width: 30%; text-align: center">
                            <img src='ImageProxy.aspx?url=<%# Eval("图片")%>' style="width: 300px"  alt="商品图片"/>
                        </td>
                        <td>
                            <table class="ttta">
                                <tr>
                                    <td style="width: 30%" class="bbb">1688采集关键词</td>
                                    <td><%# Eval("1688采集关键词") %></td>

                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">1688产品链接</td>
                                    <td><a href="<%# Eval("1688产品链接") %>" target="_blank">打开网址</a>&nbsp;&nbsp;
                                            <input type="text" value='<%# Eval("1688产品链接") %>' style="width: 20px" id="shopeeurl<%# Container.ItemIndex+1 %>" />
                                        <input type="button" class="anniu1" onclick='copyUrl("url<%# Container.ItemIndex+1 %>")' value="复制网址">
                                    </td>

                                </tr>



                                <tr>
                                    <td style="width: 30%" class="bbb">标题</td>
                                    <td><%# Eval("标题") %></td>

                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">单价</td>
                                    <td><%# Eval("单价") %></td>

                                </tr>
                                <tr>
                                    <td style="width: 30%" class="bbb">销量</td>
                                    <td><%# Eval("销量") %></td>

                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">数据状态</td>
                                    <td>
                                        <div style="display: flex; align-items: center; gap: 10px;">
                                            <asp:DropDownList
                                                ID="shujuzhuangtai"
                                                runat="server"
                                                CssClass="status-filter"
                                                SelectedValue='<%# Eval("数据状态") %>'>

                                                <asp:ListItem Value="可导出">可导出</asp:ListItem>
                                                <asp:ListItem Value="不可导出">不可导出</asp:ListItem>
                                                <asp:ListItem Value="未处理">未处理</asp:ListItem>
                                                <asp:ListItem Value="不确定">不确定</asp:ListItem>

                                                <asp:ListItem Value="多型号标题">多型号标题</asp:ListItem>
                                                <asp:ListItem Value="可广告测品">可广告测品</asp:ListItem>
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
                                            CommandArgument='<%# Eval("OfferID") %>' />
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
            <span style="margin: 0 10px">跳转至 
        <asp:TextBox ID="txtJumpPage" runat="server" Width="50px" Text="1"></asp:TextBox>
                <asp:Button ID="btnJump" runat="server" Text="GO" OnClick="BtnJump_Click" CssClass="pager-btn" />
            </span>
            <span style="margin: 0 10px">
                <asp:Literal ID="litCurrentPage" runat="server" />
                /
                <asp:Literal ID="litPageInfo" runat="server"></asp:Literal>
                <asp:Literal ID="litTotalPages" runat="server" />
            </span>
            <asp:Button ID="btnNext" runat="server" Text="下一页" OnClick="btnNext_Click" CssClass="pager-btn" />
        </div>
    </form>
</body>
</html>
