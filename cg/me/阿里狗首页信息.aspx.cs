using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 阿里狗首页信息 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!access_sql.yzdlcg())
            {
                Response.Redirect("/cg/clogin.aspx");
            }
            else
            {
                Session.Timeout = 640;
                u = HttpContext.Current.Request.Cookies["cu"].Value;
                p = HttpContext.Current.Request.Cookies["cp"].Value;
                uid = HttpContext.Current.Request.Cookies["cuid"].Value;
                if (uid != "9" && uid != "12" && uid != "10" && uid != "13" && uid != "15" && uid != "19" && uid != "20")
                {

                    Response.Redirect("/cg/clogin.aspx");
                }


                if (Request.QueryString["order"] != null && Request.QueryString["order"] != "")
                {
                    if ((Request.QueryString["txtsjbm"] != null && Request.QueryString["txtsjbm"] != "") || uid == "9")
                    {
                        rplb.DataSource = null;
                        rplb.DataBind();
                        lify.Text = "";
                        lify2.Text = "";
                        lits.Text = "";
                        bindzhy();
                        if (Session["dtall"] != null)
                        {
                            DataTable dtbd = (DataTable)Session["dtall"];

                            count = dtbd.Rows.Count;
                            pg = count / 50;

                            if (pg == 0) { pg = 1; }
                            if (count % 50 > 0 && count > 50) { pg = pg + 1; }



                            int star = 1;
                            int end = 50;
                            int pageindex = 1;
                            if (Request.QueryString["page"] != null && Request.QueryString["page"] != "")
                            {
                                pageindex = int.Parse(Request.QueryString["page"]);

                            }

                            lify.Text = "当前<span style='color:red'>" + pageindex + "</span>共" + pg + "页    分页：";
                            for (int i = 1; i <= pg; i++)
                            {
                                lify.Text += "<a href='?txtsjbm=" + Request.QueryString["txtsjbm"] + "&order=" + Request.QueryString["order"] + "&page=" + i + "'>" + i + "</a>&nbsp;&nbsp;";
                            }
                            lify2.Text = lify.Text;
                            string order = "";
                            if (Request.QueryString["order"] != null && Request.QueryString["order"] != "")
                            {
                                if (Request.QueryString["order"] == "DpStatus")
                                {
                                    order = "店铺状态";
                                }
                                if (Request.QueryString["order"] == "Available_funds")
                                {
                                    order = "可提现波兰币";
                                }
                                if (Request.QueryString["order"] == "Settlements_with_Allegro")
                                {
                                    order = "需缴费";
                                }
                                if (Request.QueryString["order"] == "views_day7")
                                {
                                    order = "第7天流量";
                                }
                            }
                            lits.Text = "加载数据" + count + "共有" + pg + "页。当前页面" + pageindex + ",商家编码" + Request.QueryString["txtsjbm"] + "，排序：" + order;
                            star = (pageindex - 1) * 50 + 1;
                            end = 50 * pageindex;

                            DataTable dtls = dtbd.Clone();
                            dtls.Clear();
                            for (int i = star - 1; i <= end - 1; i++)
                            {
                                if (dtbd.Rows.Count > i)
                                {
                                    dtls.Rows.Add(dtbd.Rows[i].ItemArray);
                                }
                            }
                            rplb.DataSource = dtls;
                            rplb.DataBind();
                        }
                        else
                        {
                            lits.Text = "无数据";

                        }

                    }
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";
        public int count = 0;
        public int pg = 0;

        public string gettime(string tttt)
        {
            string ru = "";
            if (tttt != "")
            {
                try
                {
                    ru = DateTime.Parse(tttt).ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch
                {

                }
            }
            return ru;
        }


        public void bindzhy()
        {
            string sql = "";
            rplb.DataSource = null;
            rplb.DataBind();
            string where = " bid in(select browserid from houtai_alg where browserid <>'' and is_close=0 ) and ";
            bool kycx = true;
            if (Request.QueryString["txtsjbm"] != null && Request.QueryString["txtsjbm"] != "")
            {
                where += " SJBM='" + Request.QueryString["txtsjbm"].Trim().Replace("'", "''") + "'";


            }
            if (uid == "9" && Request.QueryString["txtsjbm"] == "")
            {
                where = where.Replace(") and", ")");
            }

            string order = "";
            if (Request.QueryString["order"] != null && Request.QueryString["order"] != "")
            {
                order = " " + Request.QueryString["order"] + " desc";
            }




            string sqler = "select * from ALHome where " + where + " order by " + order;
            string oldslq = "";
            if (Session["sqler"] != null)
            {
                oldslq = Session["sqler"].ToString();
            }
            Session["sqler"] = sqler;
            if (oldslq == sqler && Session["dtall"] != null)
            {
                kycx = false;
            }
            if (kycx)
            {
                DataSet dser = access_sql.GreatDs(sqler);
                if (access_sql.yzTable(dser))
                {
                    //DataView view = new DataView(dser.Tables[0]);
                    //view.Sort = "update_time desc";
                    //lasttime = view.ToTable().Rows[0]["update_time"].ToString();
                    //if (lasttime != "" && lasttime.ToLower() != "null")
                    //{
                    //    lasttime = DateTime.Parse(lasttime).ToString("yyyy-MM-dd HH:mm");
                    //    lasttime = "【最后更新时间】" + lasttime;
                    //    lilasttime.Text = lasttime;
                    //}
                    Session["dtall"] = dser.Tables[0];


                }
                else
                {
                    Session["dtall"] = null;
                }
            }
            else
            {
                lits.Text = "无数据"; ;
            }
        }
        public string lasttime = "";
        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
    }
}