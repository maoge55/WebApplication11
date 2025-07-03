using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 虾皮广告任务表 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
                    if (uid != "9")
                    {

                        Response.Redirect("/cg/clogin.aspx");
                    }
                    string nowddd = DateTime.Now.ToString("yyyy-MM-dd");

                    for (int i = 0; i < 10; i++)
                    {
                        DateTime dtemp = DateTime.Parse(nowddd).AddDays(-i);
                        dptask_date.Items.Add(new ListItem(dtemp.ToString("yyyy-MM-dd"), dtemp.ToString("yyyy-MM-dd")));
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
            rplb.DataSource = null;
            rplb.DataBind();

            string where = " task_date='" + dptask_date.SelectedValue + "' ";
            if (txtbname.Text.Trim() != "")
            {
                where += " and bname='" + txtbname.Text.Trim().Replace("'", "''") + "'";
            }
            if (txtcampaign_id.Text.Trim() != "")
            {
                where += " and campaign_id='" + txtcampaign_id.Text.Trim().Replace("'", "''") + "'";
            }
            if (txtSearch_Query.Text.Trim() != "")
            {
                where += " and Search_Query='" + txtSearch_Query.Text.Trim().Replace("'", "''") + "'";
            }
            if (dpgt_state.SelectedValue != "全部")
            {

                where += " and gt_state=" + dpgt_state.SelectedValue;

            }
            if (dpck_state.SelectedValue != "全部")
            {

                where += " and ck_state=" + dpck_state.SelectedValue;

            }
            if (dpaction_type.SelectedValue != "全部")
            {

                where += " and action_type='" + dpaction_type.SelectedValue + "'";

            }
            string getbnamesql = "SELECT bname FROM [SuMaiTongPol].[dbo].[ShopeeADTask] where " + where + " group by bname order by bname";
            DataSet dsbname = access_sql.GreatDs(getbnamesql);
            int dbcount = 0;
            if (access_sql.yzTable(dsbname))
            {
                DataTable dtbname = dsbname.Tables[0];
                dbcount = dtbname.Rows.Count;
                DataTable dt = new DataTable();
                for (int i = 0; i < dtbname.Rows.Count; i++)
                {
                    string bname = dtbname.Rows[i]["bname"].ToString();
                    string where2222 = where + " and bname='" + bname + "'";
                    string getallbydname = "select * from ShopeeADTask where" + where2222 + " order by product_id,Search_Query";
                    DataSet dssss = access_sql.GreatDs(getallbydname);
                    if (access_sql.yzTable(dssss))
                    {
                        dt.Merge(dssss.Tables[0]);
                    }
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    rplb.DataSource = dt;
                    rplb.DataBind();
                    Literal1.Text = "加载店铺" + dbcount + "个，数据" + dt.Rows.Count + "条";
                }
                else
                {
                    Literal1.Text = "无数据";
                }

            }
            else
            {
                Literal1.Text = "无数据";
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
            if (e.CommandName == "up")
            {
                lits.Text = "";
                string bid = e.CommandArgument.ToString();

                if (access_sql.T_Update_ExecSql(new string[] { "lastMessage_isInterlocutor" }, new object[] { 0 }, "ALMessage", "bid='" + bid + "'") > 0)
                {
                    bindzhy();
                    lits.Text = "店铺:" + bid + "更新成功";
                }

            }
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {

            lits.Text = "";
            Literal1.Text = "";

            bindzhy();

        }
    }
}