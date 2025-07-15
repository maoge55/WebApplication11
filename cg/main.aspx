<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="WebApplication11.cg.main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>主页</title>
    <style>
        a {
            font-size: 22px;
        }

        .a1 {
            color: red;
        }

        .a2 {
            color: black;
        }

        .a3 {
            color: cornflowerblue;
        }

        .a4 {
            color: crimson;
        }

        .a5 {
            color: #fd00ff;
        }

        .a6 {
            color: #c01e2f;
        }

        .a7 {
            color: #ff4000;
        }

        .a9 {
            color: #1e63cb;
        }

        .a10 {
            color: #ff692d;
        }

        .a11 {
            color: #cf0000;
        }

        .d1, .d2, .d3, .d4 {
            text-align: left;
        }

            .d1 a {
                color: #003aa3;
            }

            .d2 a {
                color: crimson;
            }

            .d3 a {
                color: black;
            }

            .d4 a {
                color: #fd00ff;
            }

        a {
            text-decoration: none;
            line-height: 50px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 1080px; margin: 0 auto; text-align: center">
            <%--<div>
                <h1 style="color: chocolate">你要去哪?</h1>
            </div>--%>
            <div>

                <div>
                    <div class="d0">
                        <a href="/cg/d2.aspx?type=1" class="a1" runat="server" id="管理员" target="_blank" visible="false">管理员</a><br />
                    </div>
                    <div class="d1">

                        <a href="/cg/me/虾皮更新模版.aspx" class="a10" runat="server" id="虾皮更新模版" target="_blank" visible="false">2-虾皮更新模版</a><br />
                        <a href="/cg/me/找货源.aspx" runat="server" id="找货源" class="a1" visible="false" target="_blank">3-找货源</a><br />
                        <a href="/cg/me/物流审核.aspx" class="a10" runat="server" id="物流审核" target="_blank" visible="false">4-物流审核</a><br />
                        <a href="/cg/me/广告测试数据分配.aspx" class="a2" runat="server" id="广告测试数据分配" target="_blank" visible="false">5-广告测试数据分配</a><br />
                        <a href="/cg/me/管理广告测试产品.aspx" class="a3" runat="server" id="管理广告测试产品" target="_blank" visible="false">6-管理广告测试产品</a><br />

                        <a href="/cg/me/菲律宾虾皮类目链接.aspx" class="a3" runat="server" id="菲律宾虾皮类目链接" target="_blank" visible="false">7-菲律宾虾皮类目链接</a><br />


                        <a href="/cg/me/手动广告选品.aspx" class="a3" runat="server" id="手动广告选品" target="_blank" visible="false">8-手动广告选品</a><br />
                        <a href="/cg/me/虾皮加购成本表.aspx" class="a1" runat="server" id="虾皮加购成本表" target="_blank" visible="false">9-虾皮加购成本表</a><br />
                        <a href="/cg/me/精准匹配广告词.aspx" class="a2" runat="server" id="精准匹配广告词" target="_blank" visible="false">10-精准匹配广告词</a><br />
                        <a href="/cg/me/出单词.aspx" class="a8" runat="server" id="出单词" target="_blank" visible="false">11-出单词</a><br />
                        <a href="/cg/me/初次入仓采购.aspx" runat="server" id="初次入仓采购" class="a3" visible="false" target="_blank">12-初次入仓采购</a><br />
                        <a href="/cg/me/仓库.aspx" class="a4" runat="server" id="仓库" target="_blank" visible="false">13-仓库</a><br />
                        <a href="/cg/me/财务表补充信息.aspx" class="a10" runat="server" id="财务表补充信息" target="_blank" visible="false">14-财务表补充信息</a><br />
                        <a href="/cg/me/财务表补充重量体积.aspx" class="a8" runat="server" id="财务表补充重量体积" target="_blank" visible="false">15-财务表补充重量体积</a><br />
                        <a href="/cg/me/莆田发出信息登记.aspx" class="a5" runat="server" id="莆田发出信息登记" target="_blank" visible="false">16-莆田发出信息登记</a><br />
                        <a href="/cg/me/广东发出信息登记.aspx" class="a10" runat="server" id="广东发出信息登记" target="_blank" visible="false">17-广东发出信息登记</a><br />
                        <a href="/cg/me/海运运费登记.aspx" class="a2" runat="server" id="海运运费登记" target="_blank" visible="false">18-海运运费登记</a><br />
                        <a href="/cg/me/海外仓到仓数量登记.aspx" class="a4" runat="server" id="海外仓到仓数量登记" target="_blank" visible="false">19-海外仓到仓数量登记</a><br />
                        <a href="/cg/me/入仓产品图片视频.aspx" class="a9" runat="server" id="入仓产品图片视频" target="_blank" visible="false">20-入仓产品图片视频</a><br />
                        <a href="/cg/me/财务表.aspx" class="a10" runat="server" id="财务表" target="_blank" visible="false">21-财务表</a><br />
                        <a href="/cg/me/虾皮群控任务表.aspx" class="a2" runat="server" id="虾皮群控任务表" target="_blank" visible="false">22-虾皮群控任务表</a><br />

                        <a href="/cg/me/泰国虾皮群控任务表.aspx" class="a2" runat="server" id="泰国虾皮群控任务表" target="_blank" visible="false">22.1-泰国虾皮群控任务表</a><br />

                        <a href="/cg/me/搜索货源.aspx" runat="server" id="搜索货源" class="a9" visible="false" target="_blank">23-搜索货源</a><br />
                        <a href="/cg/me/虾皮广告任务表.aspx" runat="server" id="虾皮广告任务表" class="a9" visible="false" target="_blank">24-虾皮广告任务表</a><br />
                        <a href="/cg/me/常规任务表.aspx" runat="server" id="常规任务表" class="a9" visible="false" target="_blank">25-常规任务表</a><br />
                        <a href="/cg/me/虾皮创建广告任务.aspx" runat="server" id="虾皮创建广告任务" class="a9" visible="false" target="_blank">26-虾皮创建广告任务</a><br />


                        <a href="/cg/cjt/订单数据展示.aspx" runat="server" id="订单数据展示" visible="false" class="a9" target="_blank">27-所有订单数据展示</a><br />
                        <a href="/cg/cjt/广告测品订单.aspx" runat="server" id="广告测品订单" visible="false" class="a9" target="_blank">28-广告测品订单</a><br />
                        <a href="/cg/tb/本地收货登记.aspx" runat="server" id="本地收货登记" visible="false" class="a9" target="_blank">29-本地收货登记</a><br />
                        <a href="/cg/tb/残次品退货.aspx" runat="server" id="残次品退换货" visible="false" class="a9" target="_blank">30-残次品退换货</a><br />
                        <a href="/cg/cjt/头程物流表.aspx" runat="server" id="头程物流表" visible="false" class="a9" target="_blank">31-头程物流登记</a><br />
                        <a href="/cg/cjt/采购销售海外仓数据匹配.aspx" runat="server" id="采购销售海外仓数据匹配" visible="false" class="a9" target="_blank">32-采购_销售_海外仓数据匹配</a><br />
                        <a href="/cg/cjt/1688采购记录检索.aspx" runat="server" id="_1688采购记录检索" visible="false" class="a9" target="_blank">33-1688采购记录检索</a><br />
                        <a href="/cg/cjt/库存计划表.aspx" runat="server" id="库存计划表" visible="false" class="a9" target="_blank">34-库存计划表</a><br />
                        <a href="/cg/cjt/销量和库存表.aspx" runat="server" id="销量和库存表" visible="false" class="a9" target="_blank">35-销量和库存表</a><br />
                        <a href="/cg/cjt/点击任务表.aspx" runat="server" id="点击任务表" visible="false" class="a9" target="_blank">36-点击任务表</a><br />
                        <a href="/cg/tb/采购订单补充资料.aspx" runat="server" id="货源补充体积_重量_包装等" visible="false" class="a9" target="_blank">37-货源补充体积_重量_包装等</a><br />
                        <a href="/cg/tb/采购单_印尼_管理员_新.aspx" runat="server" id="采购单_印尼_管理员_新" visible="false" class="a9" target="_blank">38-采购单_印尼_管理员</a><br />
                        <a href="/cg/tb/采购单_印尼_采购员_新.aspx" runat="server" id="采购单_印尼_采购员" visible="false" class="a9" target="_blank">39-采购单_印尼_采购员</a><br />


                        <a href="/cg/cjt/采购单_泰国_管理员.aspx" runat="server" id="采购单_泰国_管理员" visible="false" class="a9" target="_blank">40-采购单_泰国_管理员</a><br />
                        <a href="/cg/cjt/采购单_泰国_采购员.aspx" runat="server" id="采购单_泰国_采购员" visible="false" class="a9" target="_blank">41-采购单_泰国_采购员</a><br />

                        <a href="/cg/cjt/采集数据_1688_管理员.aspx" runat="server" id="采集数据_1688_管理员" visible="false" class="a9" target="_blank">42-采集数据_1688_管理员</a><br />
                        <a href="/cg/cjt/采集数据_1688_运营.aspx" runat="server" id="采集数据_1688_运营" visible="false" class="a9" target="_blank">43-采集数据_1688_运营</a><br />
                        <a href="/cg/tb/印尼出单数据_转_泰国广告测品.aspx" runat="server" id="印尼出单数据_转_泰国广告测品" visible="false" class="a9" target="_blank">44-印尼出单数据_转_泰国广告测品</a><br />
                        <a href="/cg/tb/销售状态_库存量_管理员页面.aspx" runat="server" id="销售状态_库存量_管理员页面" visible="false" class="a9" target="_blank">45-销售状态_库存量_管理员页面</a><br />
                        <a href="/cg/tb/SKU优化停售_管理员页面.aspx" runat="server" id="SKU优化停售_管理员页面" visible="false" class="a9" target="_blank">46-SKU优化停售_管理员页面</a><br />
                        <a href="/cg/tb/广告测品出单数据分析_管理员.aspx" runat="server" id="广告测品出单数据分析_管理员" visible="false" class="a9" target="_blank">47-广告测品出单数据分析_管理员</a><br />
                        <a href="/cg/tb/头程物流价格_运营.aspx" runat="server" id="头程物流价格_运营" visible="false" class="a9" target="_blank">48-头程物流价格_运营</a><br />
                        <a href="/cg/tb/产品利润表_印尼_管理员.aspx" runat="server" id="产品利润表_印尼_管理员" visible="false" class="a9" target="_blank">49-产品利润表_印尼_管理员</a><br />
                        <a href="/cg/tb/列名总表_管理员.aspx" runat="server" id="列名总表_管理员" visible="false" class="a9" target="_blank">50-列名总表_管理员</a><br />
                        <a href="/cg/tb/店铺资料管理_运营.aspx" runat="server" id="店铺资料管理_运营" visible="false" class="a9" target="_blank">51-店铺资料管理_运营</a><br />


                        
                    </div>
                    <br />
                    <br />
                    <br />
                    <div class="d2">
                        <a href="/cg/me/POD店铺审核.aspx" class="a5" runat="server" id="POD店铺审核" target="_blank" visible="false">1-POD店铺审核</a><br />
                        <a href="/cg/me/POD产品审核.aspx" class="a9" runat="server" id="POD产品审核" target="_blank" visible="false">2-POD产品审核</a><br />
                        <a href="/cg/me/翻译成印尼语.aspx?type=2" class="a4" runat="server" id="POD翻译成印尼语" target="_blank" visible="false">3-POD翻译成印尼语</a><br />
                        <a href="/cg/me/翻译成泰语.aspx?type=2" class="a1" runat="server" id="POD翻译成泰语" target="_blank" visible="false">4-POD翻译成泰语</a><br />
                        <a href="/cg/me/虾皮更新模版.aspx" class="a10" runat="server" id="虾皮更新模版2" target="_blank" visible="false">5-虾皮更新模版</a><br />
                        <a href="/cg/me/虾皮违规产品.aspx" class="a2" runat="server" id="虾皮违规产品" target="_blank" visible="false">6-虾皮违规产品</a><br />
                        <a href="/cg/me/虾皮我的产品概况.aspx" class="a9" runat="server" id="虾皮我的产品概况" target="_blank" visible="false">7-虾皮我的产品概况</a><br />
                        <a href="/cg/me/虾皮首页信息.aspx" class="a8" runat="server" id="虾皮首页信息" target="_blank" visible="false">8-虾皮首页信息</a><br />
                        <a href="/cg/me/POD原图.aspx" class="a6" runat="server" id="POD原图" target="_blank" visible="false">9-POD原图</a><br />
                        <a href="/cg/me/POD产品审核数量查看与分配.aspx" class="a8" runat="server" id="POD产品审核数量查看与分配" target="_blank" visible="false">10-POD产品审核数量查看与分配</a><br />
                        <a href="/cg/me/虾皮群控任务表.aspx" class="a2" runat="server" id="虾皮群控任务表2" target="_blank" visible="false">11-虾皮群控任务表</a><br />


                        <a href="/cg/me/虾皮菲律宾广告产品找货源.aspx" class="a2" runat="server" id="虾皮菲律宾广告产品找货源" target="_blank" visible="false">12-虾皮菲律宾广告产品找货源</a><br />
                        <a href="/cg/me/虾皮菲律宾广告产品找货源数量查看与分配.aspx" class="a2" runat="server" id="虾皮菲律宾广告产品找货源数量查看与分配" target="_blank" visible="false">13-虾皮菲律宾广告产品找货源数量查看与分配</a><br />


                    </div>
                    <br />
                    <br />
                    <br />

                    <div class="d3">
                        <a href="/cg/me/波兰阿里狗审核体积.aspx" class="a10" runat="server" id="波兰阿里狗审核体积" target="_blank" visible="false">1-阿里狗审核体积</a><br />
                        <a href="/cg/me/阿里狗更新模版.aspx" class="a8" runat="server" id="阿里狗更新模版" target="_blank" visible="false">2-阿里狗更新模版</a>&nbsp;&nbsp;<br />
                        <a href="/cg/me/扩展侵权词处理.aspx" class="a4" runat="server" id="扩展侵权词处理" target="_blank" visible="false">3-扩展侵权词处理</a><br />
                        <a href="/cg/me/阿里狗找货源.aspx" class="a8" runat="server" id="阿里狗找货源" target="_blank" visible="false">4-阿里狗找货源</a><br />
                        <a href="/cg/me/阿里狗拉黑链接.aspx" class="a11" runat="server" id="阿里狗拉黑链接" target="_blank" visible="false">5-阿里狗拉黑链接</a><br />
                        <a href="/cg/me/阿里狗本地库存表.aspx" class="a11" runat="server" id="阿里狗本地库存表" target="_blank" visible="false">6-阿里狗本地库存表</a><br />
                        <a href="/cg/me/阿里狗订单.aspx" class="a11" runat="server" id="阿里狗订单" target="_blank" visible="false">7-阿里狗订单</a><br />
                        <a href="/cg/me/阿里狗买家消息.aspx" class="a11" runat="server" id="阿里狗买家消息" target="_blank" visible="false">8-阿里狗买家消息</a><br />
                        <a href="/cg/me/阿里狗买家纠纷.aspx" class="a11" runat="server" id="阿里狗买家纠纷" target="_blank" visible="false">9-阿里狗买家纠纷</a><br />
                        <a href="/cg/me/阿里狗订单查货源.aspx" class="a1" runat="server" id="阿里狗订单查货源" target="_blank" visible="false">10-阿里狗订单查货源</a><br />
                        <a href="/cg/me/阿里狗首页信息.aspx" class="a1" runat="server" id="阿里狗首页信息" target="_blank" visible="false">11-阿里狗首页信息</a><br />
                        <a href="/cg/me/阿里狗群控任务表.aspx" class="a11" runat="server" id="阿里狗群控任务表" target="_blank" visible="false">12-阿里狗群控任务表</a><br />
                        <a href="/cg/me/阿里狗找货源数量查看与分配.aspx" class="a5" runat="server" id="阿里狗找货源数量查看与分配" target="_blank" visible="false">13-阿里狗找货源数量查看与分配</a><br />
                    </div>



                    <br />
                    <br />
                    <br />


                    <div class="d4">


                        <a href="/cg/me/长期入仓采购.aspx" runat="server" id="长期入仓采购" class="a2" visible="false" target="_blank">长期入仓采购</a><br />

                        <%--   <a href="/cg/me/财务表补充SKUID和运营编码.aspx" class="a7" runat="server" id="财务表补充SKUID和运营编码" target="_blank" visible="false">财务表补充SKUID和运营编码</a>&nbsp;&nbsp;
                    <a href="/cg/me/上架补充SKU.aspx" class="a6" runat="server" id="上架补充SKU" target="_blank" visible="false">上架补充SKU</a>&nbsp;&nbsp;
                        --%>

                        <a href="/cg/me/出错任务.aspx" class="a10" runat="server" id="出错任务" target="_blank" visible="false">出错任务</a>

                        <br />


                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
