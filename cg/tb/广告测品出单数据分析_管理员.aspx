<%@ Page Language="C#" AutoEventWireup="true" validateRequest="false" CodeBehind="广告测品出单数据分析_管理员.aspx.cs" Inherits="WebApplication11.cg.tb.广告测品出单数据分析_管理员" %>

<!DOCTYPE html>

<html lang="zh-CN">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>广告测品出单数据分析_管理员</title>
    <script src="https://cdn.tailwindcss.com"></script>
    <link href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" rel="stylesheet">
    <script>
        tailwind.config = {
            theme: {
                extend: {
                    colors: {
                        primary: '#3b82f6',
                        secondary: '#64748b',
                        success: '#10b981',
                        warning: '#f59e0b',
                        danger: '#ef4444',
                        info: '#06b6d4',
                    },
                    fontFamily: {
                        inter: ['Inter', 'system-ui', 'sans-serif'],
                    },
                }
            }
        }
    </script>
    <style type="text/tailwindcss">
        @layer utilities {
            .content-auto {
                content-visibility: auto;
            }
            .card-shadow {
                box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
            }
        }
    </style>
    <style>
        .page-header {
            background: linear-gradient(to right, #4F46E5, #3B82F6);
            padding: 1rem 2rem;
            margin-bottom: 2rem;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        .header-title {
            color: white;
            font-size: 1.5rem;
            font-weight: bold;
            text-shadow: 1px 1px 2px rgba(0,0,0,0.1);
        }
        .data-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
        }
        .data-table th,
        .data-table td {
            padding: 12px;
            text-align: left;
            border: 1px solid #e5e7eb;
        }
        .data-table th {
            background-color: #f3f4f6;
            font-weight: 600;
        }
        .data-table tr:nth-child(even) {
            background-color: #f9fafb;
        }
    </style>
    <script>
        function validateForm() {
            var operationCodes = document.getElementById('<%= txtOperationCodes.ClientID %>').value.trim();
            if (!operationCodes) {
                alert('请输入商家编码！');
                return false;
            }
            
            // 检查每一行是否为空
            var lines = operationCodes.split('\n');
            for (var i = 0; i < lines.length; i++) {
                if (!lines[i].trim()) {
                    alert('商家编码不能包含空行！');
                    return false;
                }
            }
            return true;
        }
    </script>
</head>
<body class="bg-gray-50 font-inter text-gray-800 min-h-screen flex flex-col">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <header class="page-header">
            <div class="header-title">当前页面【<span style="color: #37cbc5">广告测品出单数据分析_管理员</span>】</div>
        </header>

        <!-- 主内容区 -->
        <main class="flex-grow container mx-auto px-4 py-6">
            <!-- 筛选区 -->
            <section class="bg-white rounded-lg card-shadow p-6 mb-8">
                <h2 class="text-lg font-semibold mb-4 flex items-center">
                    <i class="fa fa-filter text-primary mr-2"></i>商家编码筛选
                </h2>
                <div class="flex flex-col md:flex-row gap-4">
                    <div class="flex-grow">
                        <label for="operationCodes" class="block text-sm font-medium text-gray-700 mb-1">商家编码（每行一个，可输入多个）<span class="text-red-500">*</span></label>
                        <asp:TextBox ID="txtOperationCodes" runat="server" TextMode="MultiLine" Rows="4" 
                            CssClass="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary/50 focus:border-primary transition-all" 
                            placeholder="请输入商家编码">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvOperationCodes" runat="server" 
                            ControlToValidate="txtOperationCodes"
                            ErrorMessage="请输入商家编码"
                            Display="Dynamic"
                            CssClass="text-red-500 text-sm mt-1">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="flex items-end">
                        <asp:Button ID="btnFilter" runat="server" Text="筛选数据" OnClick="btnFilter_Click"
                            OnClientClick="return validateForm();"
                            CssClass="bg-primary hover:bg-primary/90 text-white px-6 py-2 rounded-lg transition-all flex items-center" />
                    </div>
                </div>
            </section>

            <!-- 无数据提示 -->
            <asp:Panel ID="pnlNoData" runat="server" CssClass="text-center py-8 bg-white rounded-lg card-shadow" Visible="false">
                <i class="fa fa-info-circle text-gray-400 text-4xl mb-4"></i>
                <p class="text-gray-500">暂无数据</p>
            </asp:Panel>

            <!-- 数据表格区域 -->
            <asp:Panel ID="pnlDataArea" runat="server" CssClass="grid grid-cols-1 lg:grid-cols-2 gap-8" Visible="false">
                <!-- 历史销量 -->
                <div class="bg-white rounded-lg card-shadow p-6">
                    <h3 class="font-semibold text-lg mb-4">历史销量</h3>
                    <table class="data-table">
                        <tbody>
                            <asp:Repeater ID="rptSalesData" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Range") %></td>
                                        <td><%# Eval("Count") %></td>
                                        <td><%# Eval("Percentage") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>

                <!-- 评分星级 -->
                <div class="bg-white rounded-lg card-shadow p-6">
                    <h3 class="font-semibold text-lg mb-4">评分星级</h3>
                    <table class="data-table">
                        <tbody>
                            <asp:Repeater ID="rptRatingData" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Range") %></td>
                                        <td><%# Eval("Count") %></td>
                                        <td><%# Eval("Percentage") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>

                <!-- 预定数 -->
                <div class="bg-white rounded-lg card-shadow p-6">
                    <h3 class="font-semibold text-lg mb-4">预定数</h3>
                    <table class="data-table">
                        <tbody>
                            <asp:Repeater ID="rptBookingData" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Range") %></td>
                                        <td><%# Eval("Count") %></td>
                                        <td><%# Eval("Percentage") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>

                <!-- 价格 -->
                <div class="bg-white rounded-lg card-shadow p-6">
                    <h3 class="font-semibold text-lg mb-4">价格</h3>
                    <table class="data-table">
                        <tbody>
                            <asp:Repeater ID="rptPriceData" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Range") %></td>
                                        <td><%# Eval("Count") %></td>
                                        <td><%# Eval("Percentage") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>

                <!-- 店铺名称 -->
                <div class="bg-white rounded-lg card-shadow p-6">
                    <h3 class="font-semibold text-lg mb-4">店铺名称</h3>
                    <table class="data-table">
                        <tbody>
                            <asp:Repeater ID="rptShopData" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Range") %></td>
                                        <td><%# Eval("Count") %></td>
                                        <td><%# Eval("Percentage") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>

                <!-- 回头率 -->
                <div class="bg-white rounded-lg card-shadow p-6">
                    <h3 class="font-semibold text-lg mb-4">回头率</h3>
                    <table class="data-table">
                        <tbody>
                            <asp:Repeater ID="rptReturnRateData" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Range") %></td>
                                        <td><%# Eval("Count") %></td>
                                        <td><%# Eval("Percentage") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </asp:Panel>
        </main>
    </form>
</body>
</html> 