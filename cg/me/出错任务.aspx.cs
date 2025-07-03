using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 出错任务 : System.Web.UI.Page
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

        public void bindzhy()
        {
            lits.Text = "";
            Literal1.Text = "";
            rplb.DataSource = null;
            rplb.DataBind();
            string where = " iscl=0 and loginTime>'" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00' and loginTime<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00'";


            DataSet ds = access_sql.GreatDs("SELECT ErrBid.id as 'eid',ErrBid.BID as'浏览器id', ErrBid.loginTime as 时间,ErrBid.des as '出错内容',bname,gname,pintai FROM [SuMaiTongPol].[dbo].[ErrBid]  where " + where + " order by loginTime desc");
            if (access_sql.yzTable(ds))
            {
                if (access_sql.yzTable(ds))
                {
                  

                    rplb.DataSource = ds.Tables[0];
                    rplb.DataBind();
                    Literal1.Text = "<span style='color:red'>加载数据" + ds.Tables[0].Rows.Count + "条</span>";

                }

            }
            else
            {
                lits.Text = "无数据"; ;
            }
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            bindzhy();
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
                if (access_sql.T_Update_ExecSql(new string[] { "iscl" }, new object[] { "1" }, "ErrBid", "id=" + id + "") > 0)
                {
                    bindzhy();
                    lits.Text = "ID:" + id + "改成已处理";
                }

            }
        }

        

       
    }
}