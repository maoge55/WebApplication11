using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 虾皮创建广告任务 : System.Web.UI.Page
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

            string where = " 1=1 ";
            if (txtbname.Text.Trim() != "")
            {
                where += " and bname='" + txtbname.Text.Trim().Replace("'", "''") + "'";
            }

            if (dpstate.SelectedValue != "全部")
            {
                if (dpstate.SelectedValue == "1")
                {
                    where += " and rw_msg='执行成功'";
                }
                else
                {
                    where += " and rw_msg<>'执行成功'";
                }
            }

            string order = " order by bname";
            if (dporder.SelectedValue == "更新时间-大到小")
            {
                order = " order by update_time desc";
            }
            else if (dporder.SelectedValue == "更新时间-小到大")
            {
                order = " order by update_time ";
            }

            string stringsql = "SELECT * FROM [SuMaiTongPol].[dbo].[ShopeeCreadTask] where " + where + order;
            DataSet dsbname = access_sql.GreatDs(stringsql);

            if (access_sql.yzTable(dsbname))
            {

                rplb.DataSource = dsbname.Tables[0];
                rplb.DataBind();
                Literal1.Text = "加载数据" + dsbname.Tables[0].Rows.Count + "条";


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