<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="d2.aspx.cs" Inherits="WebApplication11.cg.d2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>管理员</title>
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
                color: #036807;
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
                    <div class="d1">
                        <div>
                            <h2>shopee普货运营郑雨蝶</h2>
                            <a href="/cg/me/翻译成印尼语.aspx?type=1" class="a1" runat="server" id="A1" target="_blank">1-普货翻译成印尼语</a><br />
                            <a href="/cg/me/虾皮更新模版.aspx" class="a10" runat="server" id="A2" target="_blank">2-虾皮更新模版</a><br />
                            <a href="/cg/me/找货源.aspx" runat="server" id="A3" class="a1" target="_blank">3-找货源</a><br />
                            <a href="/cg/me/物流审核.aspx" class="a10" runat="server" id="A4" target="_blank">4-物流审核</a><br />
                            <a href="/cg/me/菲律宾虾皮类目链接.aspx" class="a3" runat="server" id="A5" target="_blank">7-菲律宾虾皮类目链接</a><br />
                            <a href="/cg/me/手动广告选品.aspx" class="a3" runat="server" id="A6" target="_blank">8-手动广告选品</a><br />
                            <a href="/cg/me/初次入仓采购.aspx" runat="server" id="A7" class="a3" target="_blank">12-初次入仓采购</a><br />
                            <a href="/cg/me/虾皮我的产品概况.aspx" class="a9" runat="server" id="A26" target="_blank">P7-虾皮我的产品概况</a><br />
                            <a href="/cg/me/仓库.aspx" class="a4" runat="server" id="仓库" target="_blank">13-仓库</a><br />
                            <a href="/cg/me/搜索货源.aspx" runat="server" id="搜索货源" class="a9" target="_blank">23-搜索货源</a><br />
                            <a href="/cg/me/虾皮菲律宾广告产品找货源.aspx" runat="server" id="A54" class="a9" target="_blank">12-虾皮菲律宾广告产品找货源</a><br />
                            <a href="/cg/cjt/本地收货登记.aspx" runat="server" id="A58" class="a9" target="_blank">29-本地收货登记</a><br />
                            <a href="/cg/cjt/残次品退换货.aspx" runat="server" id="A59" class="a9" target="_blank">30-残次品退换货</a><br />

                        </div>
                        <br />
                        <br />
                        <br />
                        <div>
                            <h2>shopee普货运营夏鸿飞</h2>
                            <a href="/cg/me/财务表补充信息.aspx" class="a10" runat="server" id="A8" target="_blank">14-财务表补充信息</a><br />
                            <a href="/cg/me/财务表补充重量体积.aspx" class="a8" runat="server" id="A9" target="_blank">15-财务表补充重量体积</a><br />
                            <a href="/cg/me/莆田发出信息登记.aspx" class="a5" runat="server" id="A10" target="_blank">16-莆田发出信息登记</a><br />
                            <a href="/cg/me/广东发出信息登记.aspx" class="a10" runat="server" id="A11" target="_blank">17-广东发出信息登记</a><br />
                            <a href="/cg/me/海运运费登记.aspx" class="a2" runat="server" id="A12" target="_blank">18-海运运费登记</a><br />
                            <a href="/cg/me/海外仓到仓数量登记.aspx" class="a4" runat="server" id="A13" target="_blank">19-海外仓到仓数量登记</a><br />
                            <a href="/cg/me/入仓产品图片视频.aspx" class="a9" runat="server" id="A14" target="_blank">20-入仓产品图片视频</a><br />
                            <a href="/cg/me/仓库.aspx" class="a4" runat="server" id="A52" target="_blank">13-仓库</a><br />
                            <a href="/cg/me/搜索货源.aspx" runat="server" id="A53" class="a9" target="_blank">23-搜索货源</a><br />
                        </div>
                        <br />
                        <br />
                        <br />
                        <div>
                            <h2>shopee普货管理员</h2>
                            <a href="/cg/me/广告测试数据分配.aspx" class="a2" runat="server" id="A15" target="_blank">5-广告测试数据分配</a><br />
                            <a href="/cg/me/管理广告测试产品.aspx" class="a3" runat="server" id="A16" target="_blank">6-管理广告测试产品</a><br />

                            <a href="/cg/me/菲律宾虾皮类目链接.aspx" class="a3" runat="server" id="A17" target="_blank">7-菲律宾虾皮类目链接</a><br />


                            <a href="/cg/me/手动广告选品.aspx" class="a3" runat="server" id="A18" target="_blank">8-手动广告选品</a><br />
                            <a href="/cg/me/虾皮加购成本表.aspx" class="a1" runat="server" id="A19" target="_blank">9-虾皮加购成本表</a><br />
                            <a href="/cg/me/精准匹配广告词.aspx" class="a2" runat="server" id="A20" target="_blank">10-精准匹配广告词</a><br />
                            <a href="/cg/me/出单词.aspx" class="a8" runat="server" id="A21" target="_blank">11-出单词</a><br />
                            <a href="/cg/me/财务表.aspx" class="a10" runat="server" id="A22" target="_blank">21-财务表</a><br />
                            <a href="/cg/me/虾皮群控任务表.aspx" class="a2" runat="server" id="A23" target="_blank">22-虾皮群控任务表</a><br />
                            <a href="/cg/me/虾皮我的产品概况.aspx" class="a9" runat="server" id="A24" target="_blank">P7-虾皮我的产品概况</a><br />
                            <a href="/cg/me/虾皮首页信息.aspx" class="a8" runat="server" id="A25" target="_blank">P8-虾皮首页信息</a><br />
                            <a href="/cg/me/虾皮广告任务表.aspx" runat="server" id="虾皮广告任务表" class="a9" target="_blank">24-虾皮广告任务表</a><br />
                            <a href="/cg/me/常规任务表.aspx" runat="server" id="常规任务表" class="a9" target="_blank">25-常规任务表</a><br />
                            <a href="/cg/me/虾皮菲律宾广告产品找货源数量查看与分配.aspx" runat="server" id="A55" class="a9" target="_blank">13-虾皮菲律宾广告产品找货源数量查看与分配</a><br />
                             <a href="/cg/cjt/订单数据展示.aspx" runat="server" id="A56" class="a9" target="_blank">26-所有订单数据展示</a><br />
                             <a href="/cg/cjt/广告测品订单.aspx" runat="server" id="A57" class="a9" target="_blank">27-广告测品订单</a><br />
                        </div>


                        <%-- <a href="/cg/me/翻译成印尼语.aspx?type=1" class="a1" runat="server" id="普货翻译成印尼语" target="_blank" >1-普货翻译成印尼语</a><br />
                        <a href="/cg/me/虾皮更新模版.aspx" class="a10" runat="server" id="虾皮更新模版" target="_blank" >2-虾皮更新模版</a><br />
                        <a href="/cg/me/找货源.aspx" runat="server" id="找货源" class="a1"  target="_blank">3-找货源</a><br />
                        <a href="/cg/me/物流审核.aspx" class="a10" runat="server" id="物流审核" target="_blank" >4-物流审核</a><br />
                        <a href="/cg/me/广告测试数据分配.aspx" class="a2" runat="server" id="广告测试数据分配" target="_blank" >5-广告测试数据分配</a><br />
                        <a href="/cg/me/管理广告测试产品.aspx" class="a3" runat="server" id="管理广告测试产品" target="_blank" >6-管理广告测试产品</a><br />

                        <a href="/cg/me/菲律宾虾皮类目链接.aspx" class="a3" runat="server" id="菲律宾虾皮类目链接" target="_blank" >7-菲律宾虾皮类目链接</a><br />


                        <a href="/cg/me/手动广告选品.aspx" class="a3" runat="server" id="手动广告选品" target="_blank" >8-手动广告选品</a><br />
                        <a href="/cg/me/虾皮加购成本表.aspx" class="a1" runat="server" id="虾皮加购成本表" target="_blank" >9-虾皮加购成本表</a><br />
                        <a href="/cg/me/精准匹配广告词.aspx" class="a2" runat="server" id="精准匹配广告词" target="_blank" >10-精准匹配广告词</a><br />
                        <a href="/cg/me/出单词.aspx" class="a8" runat="server" id="出单词" target="_blank" >11-出单词</a><br />
                        <a href="/cg/me/初次入仓采购.aspx" runat="server" id="初次入仓采购" class="a3"  target="_blank">12-初次入仓采购</a><br />
                        <a href="/cg/me/仓库.aspx" class="a4" runat="server" id="仓库" target="_blank" >13-仓库</a><br />
                        <a href="/cg/me/财务表补充信息.aspx" class="a10" runat="server" id="财务表补充信息" target="_blank" >14-财务表补充信息</a><br />
                        <a href="/cg/me/财务表补充重量体积.aspx" class="a8" runat="server" id="财务表补充重量体积" target="_blank" >15-财务表补充重量体积</a><br />
                        <a href="/cg/me/莆田发出信息登记.aspx" class="a5" runat="server" id="莆田发出信息登记" target="_blank" >16-莆田发出信息登记</a><br />
                        <a href="/cg/me/广东发出信息登记.aspx" class="a10" runat="server" id="广东发出信息登记" target="_blank" >17-广东发出信息登记</a><br />
                        <a href="/cg/me/海运运费登记.aspx" class="a2" runat="server" id="海运运费登记" target="_blank" >18-海运运费登记</a><br />
                        <a href="/cg/me/海外仓到仓数量登记.aspx" class="a4" runat="server" id="海外仓到仓数量登记" target="_blank" >19-海外仓到仓数量登记</a><br />
                        <a href="/cg/me/入仓产品图片视频.aspx" class="a9" runat="server" id="入仓产品图片视频" target="_blank" >20-入仓产品图片视频</a><br />
                        <a href="/cg/me/财务表.aspx" class="a10" runat="server" id="财务表" target="_blank" >21-财务表</a><br />
                        <a href="/cg/me/虾皮群控任务表.aspx" class="a2" runat="server" id="虾皮群控任务表" target="_blank" >22-虾皮群控任务表</a><br />--%>
                    </div>












                    <br />
                    <br />
                    <br />
                    <div class="d2">
                        <div>
                            <h2>POD运营郑雨蝶</h2>
                            <a href="/cg/me/POD店铺审核.aspx" class="a5" runat="server" id="A27" target="_blank">P1-POD店铺审核</a><br />
                            <a href="/cg/me/POD产品审核.aspx" class="a9" runat="server" id="A28" target="_blank">P2-POD产品审核</a><br />
                            <a href="/cg/me/翻译成印尼语.aspx?type=2" class="a4" runat="server" id="A29" target="_blank">P3-POD翻译成印尼语</a><br />
                            <a href="/cg/me/翻译成泰语.aspx?type=2" class="a1" runat="server" id="A30" target="_blank">P4-POD翻译成泰语</a><br />
                            <a href="/cg/me/虾皮更新模版.aspx" class="a10" runat="server" id="A31" target="_blank">P5-虾皮更新模版</a><br />
                            <a href="/cg/me/虾皮违规产品.aspx" class="a2" runat="server" id="A32" target="_blank">P6-虾皮违规产品</a><br />
                            <a href="/cg/me/虾皮我的产品概况.aspx" class="a9" runat="server" id="A33" target="_blank">P7-虾皮我的产品概况</a><br />
                            <a href="/cg/me/POD原图.aspx" class="a6" runat="server" id="A35" target="_blank">P9-POD原图</a><br />

                        </div>

                        <br />
                        <br />
                        <br />

                        <div>
                            <h2>POD管理员</h2>
                            <a href="/cg/me/虾皮首页信息.aspx" class="a8" runat="server" id="A34" target="_blank">P8-虾皮首页信息</a><br />
                            <a href="/cg/me/POD产品审核数量查看与分配.aspx" class="a8" runat="server" id="A37" target="_blank">P10-POD产品审核数量查看与分配</a><br />
                            <a href="/cg/me/虾皮群控任务表.aspx" class="a2" runat="server" id="A38" target="_blank">P11-虾皮群控任务表</a><br />

                        </div>



                        <%--<a href="/cg/me/POD店铺审核.aspx" class="a5" runat="server" id="POD店铺审核" target="_blank" >P1-POD店铺审核</a><br />
                        <a href="/cg/me/POD产品审核.aspx" class="a9" runat="server" id="POD产品审核" target="_blank" >P2-POD产品审核</a><br />
                        <a href="/cg/me/翻译成印尼语.aspx?type=2" class="a4" runat="server" id="POD翻译成印尼语" target="_blank" >P3-POD翻译成印尼语</a><br />
                        <a href="/cg/me/翻译成泰语.aspx?type=2" class="a1" runat="server" id="POD翻译成泰语" target="_blank" >P4-POD翻译成泰语</a><br />
                        <a href="/cg/me/虾皮更新模版.aspx" class="a10" runat="server" id="虾皮更新模版2" target="_blank" >P5-虾皮更新模版</a><br />
                        <a href="/cg/me/虾皮违规产品.aspx" class="a2" runat="server" id="虾皮违规产品" target="_blank" >P6-虾皮违规产品</a><br />
                        <a href="/cg/me/虾皮我的产品概况.aspx" class="a9" runat="server" id="虾皮我的产品概况" target="_blank" >P7-虾皮我的产品概况</a><br />
                        <a href="/cg/me/虾皮首页信息.aspx" class="a8" runat="server" id="虾皮首页信息" target="_blank" >P8-虾皮首页信息</a><br />
                        <a href="/cg/me/POD原图.aspx" class="a6" runat="server" id="POD原图" target="_blank" >P9-POD原图</a><br />
                        <a href="/cg/me/POD产品审核数量查看与分配.aspx" class="a8" runat="server" id="POD产品审核数量查看与分配" target="_blank" >P10-POD产品审核数量查看与分配</a><br />
                        <a href="/cg/me/虾皮群控任务表.aspx" class="a2" runat="server" id="虾皮群控任务表2" target="_blank" >P11-虾皮群控任务表</a><br />--%>
                    </div>
                    <br />
                    <br />
                    <br />

                    <div class="d3">



                        <div>
                            <h2>Allegro运营夏鸿飞</h2>
                            <a href="/cg/me/波兰阿里狗审核体积.aspx" class="a10" runat="server" id="A36" target="_blank">A1-阿里狗审核体积</a><br />
                            <a href="/cg/me/阿里狗更新模版.aspx" class="a8" runat="server" id="A39" target="_blank">A2-阿里狗更新模版</a><br />
                            <a href="/cg/me/阿里狗找货源.aspx" class="a8" runat="server" id="A41" target="_blank">A4-阿里狗找货源</a><br />
                            <a href="/cg/me/阿里狗拉黑链接.aspx" class="a11" runat="server" id="A42" target="_blank">A5-阿里狗拉黑链接</a><br />
                            <a href="/cg/me/阿里狗本地库存表.aspx" class="a11" runat="server" id="A43" target="_blank">A6-阿里狗本地库存表</a><br />
                            <a href="/cg/me/阿里狗订单.aspx" class="a11" runat="server" id="A44" target="_blank">A7-阿里狗订单</a><br />
                            <a href="/cg/me/阿里狗买家消息.aspx" class="a11" runat="server" id="A45" target="_blank">A8-阿里狗买家消息</a><br />
                            <a href="/cg/me/阿里狗买家纠纷.aspx" class="a11" runat="server" id="A46" target="_blank">A9-阿里狗买家纠纷</a><br />
                            <a href="/cg/me/阿里狗订单查货源.aspx" class="a1" runat="server" id="A47" target="_blank">A10-阿里狗订单查货源</a><br />
                            <a href="/cg/me/阿里狗首页信息.aspx" class="a1" runat="server" id="A48" target="_blank">A11-阿里狗首页信息</a><br />

                        </div>
                        <br />
                        <br />
                        <br />
                        <div>
                            <h2>Allegro管理员</h2>
                            <a href="/cg/me/阿里狗订单.aspx" class="a11" runat="server" id="A40" target="_blank">A7-阿里狗订单</a><br />
                            <a href="/cg/me/阿里狗首页信息.aspx" class="a1" runat="server" id="A49" target="_blank">A11-阿里狗首页信息</a><br />
                            <a href="/cg/me/阿里狗群控任务表.aspx" class="a11" runat="server" id="A50" target="_blank">A12-阿里狗群控任务表</a><br />
                            <a href="/cg/me/阿里狗找货源数量查看与分配.aspx" class="a5" runat="server" id="A51" target="_blank">A13-阿里狗找货源数量查看与分配</a><br />
                        </div>

                        <div>
                            <h2>Allegro合作方</h2>
                            <a href="/cg/me/阿里狗找货源.aspx" class="a8" runat="server" id="阿里狗找货源" target="_blank">A4-阿里狗找货源</a><br />
                            <a href="/cg/me/阿里狗拉黑链接.aspx" class="a11" runat="server" id="阿里狗拉黑链接" target="_blank">A5-阿里狗拉黑链接</a><br />
                            <a href="/cg/me/阿里狗本地库存表.aspx" class="a11" runat="server" id="阿里狗本地库存表" target="_blank">A6-阿里狗本地库存表</a><br />
                            <a href="/cg/me/阿里狗订单.aspx" class="a11" runat="server" id="阿里狗订单" target="_blank">A7-阿里狗订单</a><br />
                            <a href="/cg/me/阿里狗买家消息.aspx" class="a11" runat="server" id="阿里狗买家消息" target="_blank">A8-阿里狗买家消息</a><br />
                            <a href="/cg/me/阿里狗买家纠纷.aspx" class="a11" runat="server" id="阿里狗买家纠纷" target="_blank">A9-阿里狗买家纠纷</a><br />
                            <a href="/cg/me/阿里狗订单查货源.aspx" class="a1" runat="server" id="阿里狗订单查货源" target="_blank">A10-阿里狗订单查货源</a><br />
                            <a href="/cg/me/阿里狗首页信息.aspx" class="a1" runat="server" id="阿里狗首页信息" target="_blank">A11-阿里狗首页信息</a><br />
                        </div>





                        <%--    <a href="/cg/me/波兰阿里狗审核体积.aspx" class="a10" runat="server" id="波兰阿里狗审核体积" target="_blank" >A1-阿里狗审核体积</a><br />
                        <a href="/cg/me/阿里狗更新模版.aspx" class="a8" runat="server" id="阿里狗更新模版" target="_blank" >A2-阿里狗更新模版</a>&nbsp;&nbsp;<br />
                        <a href="/cg/me/扩展侵权词处理.aspx" class="a4" runat="server" id="扩展侵权词处理" target="_blank" >A3-扩展侵权词处理</a><br />
                        <a href="/cg/me/阿里狗找货源.aspx" class="a8" runat="server" id="阿里狗找货源" target="_blank" >A4-阿里狗找货源</a><br />
                        <a href="/cg/me/阿里狗拉黑链接.aspx" class="a11" runat="server" id="阿里狗拉黑链接" target="_blank" >A5-阿里狗拉黑链接</a><br />
                        <a href="/cg/me/阿里狗本地库存表.aspx" class="a11" runat="server" id="阿里狗本地库存表" target="_blank" >A6-阿里狗本地库存表</a><br />
                        <a href="/cg/me/阿里狗订单.aspx" class="a11" runat="server" id="阿里狗订单" target="_blank" >A7-阿里狗订单</a><br />
                        <a href="/cg/me/阿里狗买家消息.aspx" class="a11" runat="server" id="阿里狗买家消息" target="_blank" >A8-阿里狗买家消息</a><br />
                        <a href="/cg/me/阿里狗买家纠纷.aspx" class="a11" runat="server" id="阿里狗买家纠纷" target="_blank" >A9-阿里狗买家纠纷</a><br />
                        <a href="/cg/me/阿里狗订单查货源.aspx" class="a1" runat="server" id="阿里狗订单查货源" target="_blank" >A10-阿里狗订单查货源</a><br />
                        <a href="/cg/me/阿里狗首页信息.aspx" class="a1" runat="server" id="阿里狗首页信息" target="_blank" >A11-阿里狗首页信息</a><br />
                        <a href="/cg/me/阿里狗群控任务表.aspx" class="a11" runat="server" id="阿里狗群控任务表" target="_blank" >A12-阿里狗群控任务表</a><br />
                        <a href="/cg/me/阿里狗找货源数量查看与分配.aspx" class="a5" runat="server" id="阿里狗找货源数量查看与分配" target="_blank" >A13-阿里狗找货源数量查看与分配</a><br />--%>
                    </div>






                    <%--<div class="d4">


                        <a href="/cg/me/长期入仓采购.aspx" runat="server" id="长期入仓采购" class="a2"  target="_blank">长期入仓采购</a><br />

                        <a href="/cg/me/财务表补充SKUID和运营编码.aspx" class="a7" runat="server" id="财务表补充SKUID和运营编码" target="_blank" >财务表补充SKUID和运营编码</a>&nbsp;&nbsp;
                    <a href="/cg/me/上架补充SKU.aspx" class="a6" runat="server" id="上架补充SKU" target="_blank" >上架补充SKU</a>&nbsp;&nbsp;
                        

                        <a href="/cg/me/出错任务.aspx" class="a10" runat="server" id="出错任务" target="_blank" >出错任务</a><br />

                        <a href="/cg/me/搜索货源.aspx" runat="server" id="搜索货源" class="a9"  target="_blank">搜索货源</a><br />
                    </div>--%>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
