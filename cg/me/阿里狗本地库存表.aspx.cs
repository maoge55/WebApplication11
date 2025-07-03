using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 阿里狗本地库存表 : System.Web.UI.Page
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
                if (uid != "12" && uid != "9")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public void bindd()
        {
            rplb.DataSource = null;
            rplb.DataBind();
            Literal1.Text = "";
            Literal2.Text = "";
            lits.Text = "";
            string a = "";
            bool ky = true;
            string where = " sjbm='" + txtsjbm.Text.Trim().Replace("'", "''") + "' ";
            if (dpzt.SelectedValue == "未备库存")
            {
                where += " and (shijikucunliang is null or shijikucunliang=0)";
            }
            else if (dpzt.SelectedValue == "已备库存")
            {
                where += " and (shijikucunliang is not null  and shijikucunliang<>0)";
            }
            if (txturl.Text != "")
            {
                string url = txturl.Text.Trim().Replace("'", "''");
                string offer_id = "";
                if (url.IndexOf("-") != -1)
                {
                    offer_id = (url.Split('-')[url.Split('-').Length - 1]).Trim();
                }
                else if (url.IndexOf("oferta") != -1)
                {
                    offer_id = (url.Split('/')[url.Split('/').Length - 1]).Trim();
                }
                if (offer_id.IndexOf("?") != -1)
                {
                    offer_id = offer_id.Split('?')[0].Trim();
                }
                if (offer_id != "")
                {
                    string ean = access_sql.GetOneValue("select pean from AllGM where SelfOfferID='" + offer_id + "'");
                    if (ean != "")
                    {
                        where += " and (ean='" + ean + "')";
                    }
                    else
                    {
                        ky = false;
                    }
                }
                else
                {
                    ky = false;
                }
            }

            if (ky)
            {

                DataSet ds = access_sql.GreatDs("select * from ALBDKC where " + where + " order by kucunbianhao");

                if (access_sql.yzTable(ds))
                {
                    rplb.DataSource = ds.Tables[0];
                    rplb.DataBind();
                    Literal1.Text = "加载到数据" + ds.Tables[0].Rows.Count + "条";
                }
                else
                {
                    Literal1.Text = "无数据";
                }
            }
            else
            {
                Literal1.Text = "输入链接未能找到数据";
            }

        }



        public string getcnjg(string payments_0_paid_amount)
        {
            string ru = "";
            if (payments_0_paid_amount != "")
            {
                ru = (double.Parse(payments_0_paid_amount) * 1.83).ToString("0.00");
            }
            return ru;
        }



        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            if (e.CommandName == "up")
            {
                lits.Text = "";
                int id = Convert.ToInt32(e.CommandArgument);

                TextBox txtkucunbianhao = e.Item.FindControl("txtkucunbianhao") as TextBox;
                TextBox txtshijikucunliang = e.Item.FindControl("txtshijikucunliang") as TextBox;
                int shijikucunliang = 0;
                if (txtshijikucunliang.Text != "")
                {
                    shijikucunliang = int.Parse(txtshijikucunliang.Text.Trim().Replace("'", "''"));
                }

                if (access_sql.T_Update_ExecSql(new string[] { "kucunbianhao", "shijikucunliang" }, new object[] { txtkucunbianhao.Text.Trim().Replace("'", "''"), shijikucunliang }, "ALBDKC", "id=" + id + "") > 0)
                {
                    bindd();
                    lits.Text = "ID:" + id + "更新成功";
                }


            }
        }

        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            DataSet dsnotean = access_sql.GreatDs("SELECT id,lineItems_0_offer_externalId,lineItems_0_offer_offerUrl FROM [SuMaiTongPol].[dbo].[ALOrder] where lineItems_0_offer_externalId is null or lineItems_0_offer_externalId ='' ");
            if (access_sql.yzTable(dsnotean))
            {
                for (int i = 0; i < dsnotean.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dsnotean.Tables[0].Rows[i];
                    string id = dr["id"].ToString();
                    string url = dr["lineItems_0_offer_offerUrl"].ToString();
                    string offer_id = "";
                    if (url.IndexOf("-") != -1)
                    {
                        offer_id = (url.Split('-')[url.Split('-').Length - 1]).Trim();
                    }
                    else if (url.IndexOf("oferta") != -1)
                    {
                        offer_id = (url.Split('/')[url.Split('/').Length - 1]).Trim();
                    }
                    if (offer_id.IndexOf("?") != -1)
                    {
                        offer_id = offer_id.Split('?')[0].Trim();
                    }
                    if (offer_id != "")
                    {
                        access_sql.DoSql("update ALOrder set lineItems_0_offer_externalId=(select pean from AllGM where SelfOfferID='" + offer_id + "' ) where id=" + id + "");
                    }
                }
            }
            DataSet dtfhtj = access_sql.GreatDs("SELECT sjbm,lineItems_0_offer_externalId,count(*) as cc FROM [SuMaiTongPol].[dbo].[ALOrder] where payments_0_status='PAID' group by sjbm,lineItems_0_offer_externalId HAVING COUNT(*) > 1");
            if (access_sql.yzTable(dtfhtj))
            {
                int addcg = 0;
                int upcg = 0;
                int wxup = 0;
                for (int i = 0; i < dtfhtj.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dtfhtj.Tables[0].Rows[i];
                    string sjbm = dr["sjbm"].ToString();
                    string ean = dr["lineItems_0_offer_externalId"].ToString();
                    DataSet dsbysjbmandean = access_sql.GreatDs("select * from ALOrder where sjbm='" + sjbm + "' and lineItems_0_offer_externalId='" + ean + "' and payments_0_status='PAID' order by orderDate desc");
                    if (access_sql.yzTable(dsbysjbmandean))
                    {
                        DataTable dtbysjbmandean = dsbysjbmandean.Tables[0];
                        int 总单量 = dtbysjbmandean.Rows.Count;
                        string 目前订单总量 = access_sql.GetOneValue("select zongdanliang from ALBDKC where sjbm='" + sjbm + "' and ean='" + ean + "'");
                        bool kf = true;
                        if (目前订单总量 != "")
                        {
                            if (总单量.ToString() == 目前订单总量)
                            {
                                kf = false;
                            }
                        }
                        if (kf)
                        {

                            string 产品链接 = dtbysjbmandean.Rows[0]["lineItems_0_offer_offerUrl"].ToString();
                            string 标题 = dtbysjbmandean.Rows[0]["lineItems_0_offer_name"].ToString();
                            string 产品图片网址 = dtbysjbmandean.Rows[0]["lineItems_0_offer_imageUrl"].ToString();
                            DateTime 最后一单时间 = DateTime.Parse(dtbysjbmandean.Rows[0]["orderDate"].ToString());
                            DateTime 最早一单时间 = new DateTime();
                            for (int o = 1; o < dtbysjbmandean.Rows.Count; o++)
                            {
                                最早一单时间 = DateTime.Parse(dtbysjbmandean.Rows[o]["orderDate"].ToString());
                                if (o == 4)
                                {
                                    break;
                                }
                            }
                            TimeSpan span = 最后一单时间 - 最早一单时间;
                            double totalDays = span.TotalDays;
                            int 平均订单间隔时间 = (int)Math.Round(totalDays, MidpointRounding.AwayFromZero);
                            if (平均订单间隔时间 == 0) { 平均订单间隔时间 = 1; }
                            int 建议库存量 = (int)Math.Round(((double)5 / 平均订单间隔时间), MidpointRounding.AwayFromZero);
                            string ttt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            if (access_sql.GetOneValue("select count(1) from ALBDKC where sjbm='" + sjbm + "' and ean='" + ean + "'") == "0") //新增
                            {
                                string[] name = new string[] { "sjbm", "ean", "biaoti", "chanpintupianwangzhi", "chanpinlianjie", "zongdanliang", "pingjundingdanjiangeshijian", "jianyikucunliang", "createtime", "lasttime", "firsttime" };
                                object[] obj = new object[] { sjbm, ean, 标题, 产品图片网址, 产品链接, 总单量, 平均订单间隔时间, 建议库存量, ttt, 最后一单时间, 最早一单时间 };
                                if (access_sql.T_Insert_ExecSql(name, obj, "ALBDKC") > 0)
                                {
                                    addcg++;
                                }
                            }
                            else//更新
                            {
                                string[] name = new string[] { "biaoti", "chanpintupianwangzhi", "chanpinlianjie", "zongdanliang", "pingjundingdanjiangeshijian", "jianyikucunliang", "updatetime", "lasttime", "firsttime" };
                                object[] obj = new object[] { 标题, 产品图片网址, 产品链接, 总单量, 平均订单间隔时间, 建议库存量, ttt, 最后一单时间, 最早一单时间 };
                                if (access_sql.T_Update_ExecSql(name, obj, "ALBDKC", "sjbm='" + sjbm + "' and ean='" + ean + "'") > 0)
                                {
                                    upcg++;
                                }
                            }
                        }
                        else
                        {
                            wxup++;
                        }
                    }

                }
                lits.Text = "新增" + addcg + ",修改" + upcg + ",没有变动" + wxup;
            }
        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            if (txtsjbm.Text.Trim() != "")
            {
                bindd();
            }
            else
            {
                lits.Text = "请输入商家编码";
            }
        }
    }
}