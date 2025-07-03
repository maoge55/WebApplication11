using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 波兰阿里狗审核体积 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!access_sql.yzdlcg())
            {
                Response.Redirect("/cg/clogin.aspx");
            }
            else
            {
                Session.Timeout = 240;
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

        public void bindzhy(string type)
        {
            string sql = "";
            string where = "";
            string ssss = "SELECT  top 1000 leimu, COUNT(1) AS leimu_count  FROM ALLGoodPro where pw=1  GROUP BY leimu  ORDER BY leimu_count DESC";
            if (type == "正常")
            {
                where = " shenhetiji=0  and pw=1 and (sold>=1 or added>=1 or visits>=1 or PingLunShuLiang>3 or ZongXiaoLiang>3)";
            }
            else
            {
                where = " shenhetiji=100";
            }
            string order = " order by mb_counts.leimu_count DESC";
            sql = "select top 20 * from ALLGoodPro  as ap JOIN (" + ssss + ") mb_counts on ap.leimu = mb_counts.leimu where " + where + order;

            string sql2 = "select count(1) from ALLGoodPro  as ap JOIN(" + ssss + ") mb_counts on ap.leimu = mb_counts.leimu where " + where;


            DataSet ds = access_sql.GreatDs(sql);
            if (access_sql.yzTable(ds))
            {
                if (access_sql.yzTable(ds))
                {

                    DataTable dt = ds.Tables[0];

                    rplb.DataSource = dt;
                    rplb.DataBind();
                    Literal1.Text = "<span style='color:red'>加载" + type + "数据" + dt.Rows.Count + "条，共有" + type + "数据" + access_sql.GetOneValue(sql2) + "条 </span>";

                }

            }
            else
            {
                Literal1.Text = "无数据";

            }
        }

        public void deleteyt(int pid)
        {

            DataTable dt = (DataTable)Session["mdt"];
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["pid"].ToString() == pid.ToString())
                    {
                        dt.Rows.Remove(dt.Rows[i]);
                        break;
                    }
                }


                rplb.DataSource = dt;
                rplb.DataBind();
                Literal1.Text = "<span style='color:red'>加载数据" + dt.Rows.Count + "条</span>";
            }
            else
            {
                lits.Text = "会话状态过期，请关闭浏览器重新打开";
            }



        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            rplb.DataSource = null;
            rplb.DataBind();
            bindzhy("正常");
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
        public void cl()
        {
            lits.Text = "";
            string yes_dp = "";
            string or_dp = "";
            string no_dp = "";
            string tg_dp = "";
            bool ky = true;
            for (int i = 0; i < rplb.Items.Count; i++)
            {

                Literal liid = (Literal)rplb.Items[i].FindControl("liid");
                RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");
                if (rdjg.SelectedIndex == -1)
                {

                    ky = false;
                    Response.Write("<script>alert('第" + (i + 1) + "个还未审核，请检查');</script>");
                    break;
                }
                else
                {
                    if (rdjg.SelectedIndex == 0)
                    {
                        yes_dp += liid.Text + ",";
                    }

                    else if (rdjg.SelectedIndex == 1)
                    {
                        no_dp += liid.Text + ",";
                    }
                    else if (rdjg.SelectedIndex == 2)
                    {
                        tg_dp += liid.Text + ",";
                    }
                }
            }
            if (ky)
            {
                int cg = 0;
                if (tg_dp != "")
                {
                    tg_dp = tg_dp.Substring(0, tg_dp.Length - 1);


                    cg += access_sql.T_Update_ExecSql(new string[] { "shenhetiji" }, new object[] { 100 }, "ALLGoodPro", " pid in (" + tg_dp + ")");

                }
                if (yes_dp != "")
                {
                    yes_dp = yes_dp.Substring(0, yes_dp.Length - 1);


                    cg += access_sql.T_Update_ExecSql(new string[] { "shenhetiji" }, new object[] { 1 }, "ALLGoodPro", " pid in (" + yes_dp + ")");

                }

                if (no_dp != "")
                {
                    no_dp = no_dp.Substring(0, no_dp.Length - 1);
                    cg += access_sql.T_Update_ExecSql(new string[] { "shenhetiji", "ky" }, new object[] { -1, -2 }, "ALLGoodPro", "pid in (" + no_dp + ")");
                }



                lits.Text = "成功更新" + cg + "个产品";
                rplb.DataSource = null;
                rplb.DataBind();
                Literal1.Text = "<span style='color:red'>请点击按钮，加载新数据</span>";
            }
        }


        protected void Button2_Click1(object sender, EventArgs e)
        {
            cl();
        }

        protected void Button3_Click1(object sender, EventArgs e)
        {
            cl();
        }

        protected void Button4_Click(object sender, EventArgs e)
        {

        }

        protected void Button4_Click1(object sender, EventArgs e)
        {
            rplb.DataSource = null;
            rplb.DataBind();
            bindzhy("跳过");
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rplb.Items.Count; i++)
            {


                RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");
                rdjg.SelectedIndex = 0;

            }
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rplb.Items.Count; i++)
            {


                RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");
                rdjg.SelectedIndex = 1;

            }
        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rplb.Items.Count; i++)
            {


                RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");
                rdjg.SelectedIndex = 2;

            }
        }
    }
}