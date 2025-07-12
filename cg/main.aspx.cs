using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!access_sql.yzdlcg())
            {
                Response.Redirect("/cg/clogin.aspx");
            }
            else
            {
                u = HttpContext.Current.Request.Cookies["cu"].Value;
                p = HttpContext.Current.Request.Cookies["cp"].Value;
                uid = HttpContext.Current.Request.Cookies["cuid"].Value;
                if (uid != "6" && uid != "8" && uid != "9" && uid != "10" && uid != "11" && uid != "13" && uid != "14" && uid != "15" && uid != "16" && uid != "17" && uid != "18" && uid != "19" && uid != "21" && uid != "22")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
                if (uid == "9")
                {
                    找货源.Visible = true;
                    长期入仓采购.Visible = true;
                    初次入仓采购.Visible = true;
                    仓库.Visible = true;
                    POD店铺审核.Visible = true;
                    POD产品审核.Visible = true;
                    //上架补充SKU.Visible = true;
                    //财务表补充SKUID和运营编码.Visible = true;
                    阿里狗找货源.Visible = true;
                    波兰阿里狗审核体积.Visible = true;
                    阿里狗拉黑链接.Visible = true;
                    阿里狗订单查货源.Visible = true;
                    广告测试数据分配.Visible = true;
                    管理广告测试产品.Visible = true;
                    物流审核.Visible = true;
                    //普货翻译成印尼语.Visible = true;
                    POD翻译成印尼语.Visible = true;
                    阿里狗更新模版.Visible = true;
                    财务表补充信息.Visible = true;
                    搜索货源.Visible = true;
                    虾皮首页信息.Visible = true;
                    财务表补充重量体积.Visible = true;
                    莆田发出信息登记.Visible = true;
                    POD原图.Visible = true;
                    财务表.Visible = true;
                    出错任务.Visible = true;
                    虾皮我的产品概况.Visible = true;
                    入仓产品图片视频.Visible = true;
                    虾皮群控任务表.Visible = true;
                    阿里狗群控任务表.Visible = true;
                    阿里狗找货源数量查看与分配.Visible = true;
                    POD产品审核数量查看与分配.Visible = true;
                    POD翻译成泰语.Visible = true;
                    广东发出信息登记.Visible = true;
                    虾皮违规产品.Visible = true;
                    阿里狗首页信息.Visible = true;
                    精准匹配广告词.Visible = true;
                    扩展侵权词处理.Visible = true;
                    出单词.Visible = true;
                    虾皮更新模版.Visible = true;
                    虾皮加购成本表.Visible = true;
                    海运运费登记.Visible = true;
                    海外仓到仓数量登记.Visible = true;
                    虾皮更新模版2.Visible = true;
                    虾皮群控任务表2.Visible = true;
                    阿里狗订单.Visible = true;
                    阿里狗买家消息.Visible = true;
                    阿里狗买家纠纷.Visible = true;
                    手动广告选品.Visible = true;
                    阿里狗本地库存表.Visible = true;
                    菲律宾虾皮类目链接.Visible = true;
                    管理员.Visible = true;
                    虾皮广告任务表.Visible = true;
                    常规任务表.Visible = true;
                    虾皮创建广告任务.Visible = true;
                    虾皮菲律宾广告产品找货源.Visible = true;
                    虾皮菲律宾广告产品找货源数量查看与分配.Visible = true;
                    广告测品订单.Visible = true;
                    订单数据展示.Visible = true;
                    本地收货登记.Visible = true;
                    残次品退换货.Visible = true;
                    头程物流表.Visible = true;
                    泰国虾皮群控任务表.Visible = true;
                    采购销售海外仓数据匹配.Visible = true;
                    _1688采购记录检索.Visible = true;
                    库存计划表.Visible = true;
                    销量和库存表.Visible = true;
                    点击任务表.Visible = true;
                    货源补充体积_重量_包装等.Visible = true;
                    采购单_印尼_管理员_新.Visible = true;
                    采购单_印尼_采购员.Visible = true;

                    采购单_泰国_管理员.Visible = true;
                    采购单_泰国_采购员.Visible = true;
                    采集数据_1688_管理员.Visible = true;
                    采集数据_1688_运营.Visible = true;
                    印尼出单数据_转_泰国广告测品.Visible = true;
                    销售状态_库存量_管理员页面.Visible = true;
                    SKU优化停售_管理员页面.Visible = true;
                    广告测品出单数据分析_管理员.Visible = true;
                    头程物流价格_运营.Visible = true;
                    产品利润表_印尼_管理员.Visible = true;
                }
                if (uid == "6")
                {






                    //普货翻译成印尼语.Visible = true;


                    搜索货源.Visible = true;


                    虾皮我的产品概况.Visible = true;


                    虾皮更新模版.Visible = true;


                    入仓产品图片视频.Visible = true;
                    虾皮首页信息.Visible = true;
                    财务表补充信息.Visible = true;

                    //财务表补充重量体积.Visible = true;
                    财务表补充重量体积.Visible = true;
                    广告测品订单.Visible = true;
                    本地收货登记.Visible = true;
                    残次品退换货.Visible = true;
                    头程物流表.Visible = true;
                    采购销售海外仓数据匹配.Visible = true;
                    _1688采购记录检索.Visible = true;
                    货源补充体积_重量_包装等.Visible = true;
                    采购单_印尼_采购员.Visible = true;
                    采购单_泰国_采购员.Visible = true;
                    采集数据_1688_运营.Visible = true;
                    印尼出单数据_转_泰国广告测品.Visible = true;
                    头程物流价格_运营.Visible = true;
                }
                if (uid == "8")
                {
                    仓库.Visible = true;
                    //上架补充SKU.Visible = true;
                    //财务表补充信息.Visible = true;
                    //财务表补充重量体积.Visible = true;
                    //  财务表补充SKUID和运营编码.Visible = true;
                    //莆田发出信息登记.Visible = true;
                    虾皮我的产品概况.Visible = true;
                    入仓产品图片视频.Visible = true;
                    POD产品审核.Visible = true;
                    //广东发出信息登记.Visible = true;
                    //普货翻译成印尼语.Visible = true;
                    POD翻译成印尼语.Visible = true;
                    阿里狗找货源.Visible = true;
                    搜索货源.Visible = true;
                }
                if (uid == "10")
                {
                    初次入仓采购.Visible = true;
                    //上架补充SKU.Visible = true;
                    阿里狗拉黑链接.Visible = true;
                    阿里狗找货源.Visible = true;
                    管理广告测试产品.Visible = true;
                    阿里狗订单查货源.Visible = true;
                    搜索货源.Visible = true;
                    虾皮首页信息.Visible = true;
                    阿里狗首页信息.Visible = true;
                    阿里狗订单.Visible = true;
                    阿里狗买家消息.Visible = true;
                    阿里狗买家纠纷.Visible = true;
                }
                if (uid == "11")
                {
                    虾皮首页信息.Visible = true;
                    POD原图.Visible = true;
                    虾皮我的产品概况.Visible = true;
                }
                if (uid == "12")
                {
                    阿里狗找货源.Visible = true;

                    阿里狗拉黑链接.Visible = true;
                    阿里狗订单查货源.Visible = true;
                    阿里狗更新模版.Visible = true;
                    虾皮首页信息.Visible = true;
                    出错任务.Visible = true;
                    阿里狗首页信息.Visible = true;


                    阿里狗订单.Visible = true;
                    阿里狗买家消息.Visible = true;
                    阿里狗买家纠纷.Visible = true;

                    财务表补充信息.Visible = true;

                    财务表补充重量体积.Visible = true;
                    入仓产品图片视频.Visible = true;
                    仓库.Visible = true;
                    搜索货源.Visible = true;

                    本地收货登记.Visible = true;
                    残次品退换货.Visible = true;
                    头程物流表.Visible = true;
                    货源补充体积_重量_包装等.Visible = true;
                }
                if (uid == "13")
                {
                    阿里狗找货源.Visible = true;
                    阿里狗订单查货源.Visible = true;
                    管理广告测试产品.Visible = true;
                    虾皮首页信息.Visible = true;
                    阿里狗拉黑链接.Visible = true;
                    虾皮我的产品概况.Visible = true;
                    POD产品审核.Visible = true;
                    阿里狗首页信息.Visible = true;
                    扩展侵权词处理.Visible = true;
                    阿里狗订单.Visible = true;
                    阿里狗买家消息.Visible = true;
                    阿里狗买家纠纷.Visible = true;

                }
                if (uid == "14")
                {
                    POD产品审核.Visible = true;

                }
                if (uid == "15")
                {
                    阿里狗找货源.Visible = true;
                    阿里狗拉黑链接.Visible = true;
                    阿里狗订单查货源.Visible = true;
                    阿里狗首页信息.Visible = true;
                    阿里狗订单.Visible = true;
                    阿里狗买家消息.Visible = true;
                    阿里狗买家纠纷.Visible = true;
                }
                if (uid == "16")
                {
                    POD店铺审核.Visible = true;
                    POD产品审核.Visible = true;
                    POD原图.Visible = true;
                    虾皮我的产品概况.Visible = true;
                    虾皮首页信息.Visible = true;
                }
                if (uid == "17")
                {

                    虾皮我的产品概况.Visible = true;
                    虾皮首页信息.Visible = true;
                }
                if (uid == "18")
                {

                    虾皮首页信息.Visible = true;
                    //虾皮我的产品概况.Visible = true;
                    //财务表补充信息.Visible = true;
                    //财务表补充重量体积.Visible = true;
                    //莆田发出信息登记.Visible = true;
                    //广东发出信息登记.Visible = true;
                    //海运运费登记.Visible = true;
                    //海外仓到仓数量登记.Visible = true;
                    //入仓产品图片视频.Visible = true;
                    财务表.Visible = true;
                    广告测品订单.Visible = true;
                    //手动广告选品.Visible = true;
                    头程物流表.Visible = true;
                    采集数据_1688_运营.Visible = true;

                }
                if (uid == "19")
                {
                    虾皮首页信息.Visible = true;
                    虾皮我的产品概况.Visible = true;
                    //财务表补充信息.Visible = true;
                    //财务表补充重量体积.Visible = true;
                    //莆田发出信息登记.Visible = true;
                    //广东发出信息登记.Visible = true;
                    //海运运费登记.Visible = true;
                    //海外仓到仓数量登记.Visible = true;
                    入仓产品图片视频.Visible = true;
                    财务表.Visible = true;

                    阿里狗找货源.Visible = true;
                    阿里狗拉黑链接.Visible = true;
                    阿里狗订单.Visible = true;
                    阿里狗订单查货源.Visible = true;
                    阿里狗首页信息.Visible = true;
                    阿里狗买家消息.Visible = true;
                    阿里狗买家纠纷.Visible = true;
                    手动广告选品.Visible = true;
                    虾皮菲律宾广告产品找货源.Visible = true;
                    广告测品订单.Visible = true;
                    头程物流表.Visible = true;
                    采购销售海外仓数据匹配.Visible = true;
                    _1688采购记录检索.Visible = true;
                    货源补充体积_重量_包装等.Visible = true;
                    采集数据_1688_运营.Visible = true;
                }
                if (uid == "20")
                {


                    阿里狗找货源.Visible = true;
                    阿里狗拉黑链接.Visible = true;
                    阿里狗订单.Visible = true;
                    阿里狗买家消息.Visible = true;
                    阿里狗买家纠纷.Visible = true;
                    阿里狗订单查货源.Visible = true;
                    阿里狗首页信息.Visible = true;






                }
                if (uid == "22")
                {


                    本地收货登记.Visible = true;
                    残次品退换货.Visible = true;
                    头程物流表.Visible = true;
                    _1688采购记录检索.Visible = true;


                }
            }
        }
        public string u = "";
        public string p = "";
        public string uid = "";
    }
}