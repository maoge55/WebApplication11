using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 虾皮加购成本表 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!access_sql.yzdlcg())
            {
                Response.Redirect("/cg/clogin.aspx");
            }
            else
            {
                Session.Timeout = 180;
                u = HttpContext.Current.Request.Cookies["cu"].Value;
                p = HttpContext.Current.Request.Cookies["cp"].Value;
                uid = HttpContext.Current.Request.Cookies["cuid"].Value;
                if (uid != "9")
                {

                    Response.Redirect("/cg/clogin.aspx");
                }





                if (Request.QueryString["order"] != null && Request.QueryString["order"] != "")
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
                            lify.Text += "<a href='?txtsjbm=" + Request.QueryString["txtsjbm"] + "&ckpaused=" + Request.QueryString["ckpaused"] + "&order=" + Request.QueryString["order"] + "&page=" + i + "'>" + i + "</a>&nbsp;&nbsp;";
                        }
                        lify2.Text = lify.Text;
                        string order = "";
                        if (Request.QueryString["order"] != null && Request.QueryString["order"] != "")
                        {
                            order = Request.QueryString["order"];

                        }
                        lits.Text = "加载数据" + count + "共有" + pg + "页。当前页面" + pageindex + "，排序：" + order + ",商家编码" + Request.QueryString["txtsjbm"];
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
        public string u = "";
        public string p = "";
        public string uid = "";
        public int count = 0;
        public int pg = 0;
        public int is_paused = 0;


        public string getzt(string ppp)
        {
            string ru = "";
            if (ppp == "1")
            {
                ru = "<span style='color:red'>已暂停<span>";
            }
            else if (ppp == "0")
            {
                ru = "<span style='color:green'>进行中<span>";
            }
            else if (ppp == "2")
            {
                ru = "<span style='color:blue'>缺货<span>";
            }
            return ru;
        }
        public void bindzhy()
        {
            string sql = "";
            rplb.DataSource = null;
            rplb.DataBind();
            string where = "";
            bool kycx = true;

            where += " is_paused=" + is_paused + " and add_cart_30day<>0 ";

            if (Request.QueryString["txtsjbm"] != null && Request.QueryString["txtsjbm"] != "")
            {
                where += " and(SJBM='" + Request.QueryString["txtsjbm"].Trim().Replace("'", "''") + "') ";


            }


            string order = "";

            if (Request.QueryString["order"] != null && Request.QueryString["order"] != "")
            {
                order = Request.QueryString["order"];
            }





            string sqler = "select * from ShopeeADCost where " + where;

            string oldslq = "";
            string oldorder = "";
            if (Session["sqler"] != null)
            {
                oldslq = Session["sqler"].ToString();
            }
            if (Session["order"] != null)
            {
                oldorder = Session["order"].ToString();
            }

            Session["sqler"] = sqler;
            Session["order"] = order;
            if (oldslq == sqler && Session["dtall"] != null && oldorder == order)
            {
                kycx = false;
            }
            if (kycx)
            {
                DataSet dser = access_sql.GreatDs(sqler);
                if (access_sql.yzTable(dser))
                {
                    //DataSet dsbypd = access_sql.GreatDs("select * from YN_AD_CDC");
                    DataTable dttemp = dser.Tables[0];
                    //dttemp.Columns.Add("conversions", typeof(float));
                    //dttemp.Columns.Add("cart_to_conversion_rate", typeof(float));
                    //dttemp.Columns.Add("cost_per_conversion", typeof(int));
                    //dttemp.Columns.Add("Product_Name_Ad_Name", typeof(string));

                    //for (int i = 0; i < dttemp.Rows.Count; i++)
                    //{
                    //    DataRow dr = dttemp.Rows[i];
                    //    string Product_ID = dr["Product_ID"].ToString();
                    //    //string Product_Name_Ad_Name = "";
                    //    float cost_30day = float.Parse(dr["cost_30day"].ToString());
                    //    float add_cart_30day = float.Parse(dr["add_cart_30day"].ToString());
                    //    float conversions = 0;
                    //    // float cart_to_conversion_rate = 0;
                    //    float cost_per_conversion = 0;

                    //    if (access_sql.yzTable(dsbypd))
                    //    {
                    //        DataRow[] dtbypd = dsbypd.Tables[0].Select("Product_ID='" + Product_ID + "'");

                    //        foreach (DataRow drbypd in dtbypd)
                    //        {
                    //            //if (Product_Name_Ad_Name == "")
                    //            //{
                    //            //    Product_Name_Ad_Name = drbypd["Product_Name_Ad_Name"].ToString();
                    //            //}
                    //            if (drbypd["conversions"].ToString() != "")
                    //            {
                    //                conversions += int.Parse(drbypd["conversions"].ToString());
                    //            }
                    //        }

                    //    }
                    //    if (conversions != 0)
                    //    {
                    //        //cart_to_conversion_rate = conversions / add_cart_30day;
                    //        cost_per_conversion = cost_30day / conversions;
                    //    }
                    //    //  dr["Product_Name_Ad_Name"] = Product_Name_Ad_Name;
                    //    dr["conversions"] = conversions;
                    //    // dr["cart_to_conversion_rate"] = access_sql.getnum22222(cart_to_conversion_rate.ToString());
                    //    dr["cost_per_conversion"] = (int)Math.Round(cost_per_conversion);

                    //}
                    if (order != "")
                    {
                        if (order == "avg_cost")
                        {
                            var sortedData = dttemp.AsEnumerable()
                                 .OrderBy(row => row.Field<int>("avg_cost"))
                                 .CopyToDataTable();

                            Session["dtall"] = sortedData;
                        }
                        if (order == "add_cart_30day")
                        {
                            var sortedData = dttemp.AsEnumerable()
                                 .OrderByDescending(row => row.Field<int>("add_cart_30day"))
                                 .CopyToDataTable();

                            Session["dtall"] = sortedData;
                        }
                        if (order == "BName")
                        {
                            var sortedData = dttemp.AsEnumerable()
                                 .OrderBy(row => row.Field<string>("BName"))
                                 .CopyToDataTable();

                            Session["dtall"] = sortedData;
                        }
                        if (order == "cart_to_conversion_rate 大-小")
                        {
                            var sortedData = dttemp.AsEnumerable()
                                 .OrderByDescending(row => row.Field<float>("cart_to_conversion_rate"))
                                 .CopyToDataTable();

                            Session["dtall"] = sortedData;
                        }
                        if (order == "cart_to_conversion_rate 小-大")
                        {
                            var sortedData = dttemp.AsEnumerable()
                                 .OrderBy(row => row.Field<float>("cart_to_conversion_rate"))
                                 .CopyToDataTable();

                            Session["dtall"] = sortedData;
                        }
                        if (order == "Product_ID 大-小")
                        {
                            var sortedData = dttemp.AsEnumerable()
                                 .OrderByDescending(row => row.Field<string>("Product_ID"))
                                 .CopyToDataTable();

                            Session["dtall"] = sortedData;
                        }
                        if (order == "Product_ID 小-大")
                        {
                            var sortedData = dttemp.AsEnumerable()
                                 .OrderBy(row => row.Field<string>("Product_ID"))
                                 .CopyToDataTable();

                            Session["dtall"] = sortedData;
                        }

                    }

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
                string Product_ID = e.CommandArgument.ToString();
                int a = access_sql.T_Update_ExecSql(new string[] { "is_paused" }, new object[] { 1 }, "ShopeeADCost", "Product_ID='" + Product_ID + "'");

                if (a > 0)
                {
                    lits.Text = "Product_ID:" + Product_ID + "暂停成功";

                }
                else
                {
                    lits.Text = "Product_ID:" + Product_ID + "暂停失败";
                }
            }
            if (e.CommandName == "up2")
            {
                lits.Text = "";
                string Product_ID = e.CommandArgument.ToString();
                int a = access_sql.T_Update_ExecSql(new string[] { "is_paused" }, new object[] { 0 }, "ShopeeADCost", "Product_ID='" + Product_ID + "'");

                if (a > 0)
                {
                    lits.Text = "Product_ID:" + Product_ID + "启动成功";

                }
                else
                {
                    lits.Text = "Product_ID:" + Product_ID + "启动失败";
                }
            }
        }
    }
}