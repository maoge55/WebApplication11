using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 阿里狗买家消息 : System.Web.UI.Page
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
                if (uid != "12" && uid != "9" && uid != "10" && uid != "13" && uid != "15" && uid != "19" && uid != "20")
                {

                    Response.Redirect("/cg/clogin.aspx");
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
            string sql = "SELECT [bid],[bname],[GroupName], max([update_time]) as lasttime FROM [SuMaiTongPol].[dbo].[ALMessage] where lastMessage_isInterlocutor=1 and SJBM='" + txtsjbm.Text.Trim().Replace("'", "''") + "' group by [bid],[bname],[GroupName]";
            DataSet dser = access_sql.GreatDs(sql);
            if (access_sql.yzTable(dser))
            {
                rplb.DataSource = dser.Tables[0];
                rplb.DataBind();
                Literal1.Text = "加载数据" + dser.Tables[0].Rows.Count;


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
            if (txtsjbm.Text.Trim() != "")
            {
                lits.Text = "";
                Literal1.Text = "";
              
                bindzhy();
            }
            else
            {
                lits.Text = "请输入商家编码";
            }
        }
    }
}