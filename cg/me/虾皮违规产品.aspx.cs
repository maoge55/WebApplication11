using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 虾皮违规产品 : System.Web.UI.Page
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
                if (uid != "6" && uid != "9")
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
                string id = e.CommandArgument.ToString();
                if (access_sql.T_Update_ExecSql(new string[] { "iscl" }, new object[] { 1 }, "ShopeeDeletePro", "id=" + id + "") > 0)
                {
                    lits.Text = "id:" + id + "更新成功";
                    bind();
                }
                else
                {
                    lits.Text = "id:" + id + "更新失败";
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
            string sql = "select id,Product_name,Violation_type,Violation_reason,Suggestion from ShopeeDeletePro where iscl=0";


            DataSet ds = access_sql.GreatDs(sql);

            if (access_sql.yzTable(ds))
            {


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
    }

}