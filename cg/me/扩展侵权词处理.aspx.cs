using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 扩展侵权词处理 : System.Web.UI.Page
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
                if (uid != "13" && uid != "9")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";






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
                string leimu = e.CommandArgument.ToString();
                int a = access_sql.T_Update_ExecSql(new string[] { "iscl" }, new object[] { 1 }, "ErrAlLeiMu", "leimu='" + leimu + "'");
                int b = access_sql.T_Update_ExecSql(new string[] { "isupdate" }, new object[] { 1 }, "AlFileReport", "leimu='" + leimu + "'");
                if (a > 0 || b > 0)
                {
                    lits.Text = "类目:" + leimu + "更新成功";
                    bind();
                }
                else
                {
                    lits.Text = "类目:" + leimu + "更新失败";
                }
            }
        }







        protected void Button1_Click1(object sender, EventArgs e)
        {


        }
        public void bind()
        {
            Literal1.Text = "加载数据";
            rplb.DataSource = null;
            rplb.DataBind();
            string sql = "SELECT top 100 * FROM [SuMaiTongPol].[dbo].[qq_kz]  where state=0 order by count desc";


            DataSet ds = access_sql.GreatDs(sql);



            if (access_sql.yzTable(ds))
            {
                Literal1.Text = "加载数据" + ds.Tables[0].Rows.Count + "条";
                rplb.DataSource = ds.Tables[0];
                rplb.DataBind();


            }



            else
            {
                lits.Text = "未找到数据";

            }
        }
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            bind();
        }

        public void cl()
        {
            lits.Text = "";
            string yes_dp = "";
            string or_dp = "";
            string no_dp = "";
            bool ky = true;
            for (int i = 0; i < rplb.Items.Count; i++)
            {

                Literal linewcc = (Literal)rplb.Items[i].FindControl("linewcc");
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
                        yes_dp += "'" + linewcc.Text.Replace("'", "''") + "',";
                    }

                    else if (rdjg.SelectedIndex == 1)
                    {
                        no_dp += "'" + linewcc.Text.Replace("'", "''") + "',";
                    }
                }
            }
            if (ky)
            {
                int cg = 0;
                if (yes_dp != "")
                {
                    yes_dp = yes_dp.Substring(0, yes_dp.Length - 1);
                    cg += access_sql.T_Update_ExecSql(new string[] { "state" }, new object[] { 1 }, "qq_kz", "newcc in (" + yes_dp + ")");

                }

                if (no_dp != "")
                {
                    no_dp = no_dp.Substring(0, no_dp.Length - 1);
                    cg += access_sql.T_Update_ExecSql(new string[] { "state" }, new object[] { -1 }, "qq_kz", "newcc in (" + no_dp + ")");
                }
                lits.Text = "成功更新数据";
                rplb.DataSource = null;
                rplb.DataBind();
                Literal1.Text = "<span style='color:red'>请点击按钮，加载新数据</span>";
            }
        }

        protected void Button2_Click2(object sender, EventArgs e)
        {

        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            cl();
        }

        protected void Button1_Click2(object sender, EventArgs e)
        {
            cl();
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rplb.Items.Count; i++)
            {


                RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");
                rdjg.SelectedIndex = 0;

            }
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rplb.Items.Count; i++)
            {


                RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");
                rdjg.SelectedIndex = 1;

            }
        }
    }

}