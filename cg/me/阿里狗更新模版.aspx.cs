using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 阿里狗更新模版 : System.Web.UI.Page
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

                access_sql.DoSql("update ErrAlLeiMu set iscl=1,cl_count=cl_count+1 where leimu='" + leimu + "'");
                access_sql.DoSql("update AlFileReport set isupdate=1,cl_count=cl_count+1 where leimu='" + leimu + "'");


                lits.Text = "类目:" + leimu + "更新成功";
                bind();

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
            string sql = "SELECT leimu,  DATEADD(HOUR, 8, DATEADD(SECOND, max(uploadDate), '1970-01-01')) as uploadDate,max(cl_count) as cl_count,max(downloadPath) as downloadPath,min(err_des) as err_des FROM [SuMaiTongPol].[dbo].[AlFileReport] where status='UNSUCCESSFUL' and isupdate=0 and leimu<>'正则解析类目为空' and leimu<> '报告没填写类目' group by leimu order by uploadDate desc";

            string sql2 = "SELECT [leimu],min(des),max(uoload_time) as uoload_time ,max(cl_count) as cl_count FROM [SuMaiTongPol].[dbo].[ErrAlLeiMu] where iscl=0 GROUP BY LEIMU";
            DataSet ds = access_sql.GreatDs(sql);
            DataSet ds2 = access_sql.GreatDs(sql2);
            DataTable ddd = new DataTable();
            ddd.Columns.Add("leimu");
            ddd.Columns.Add("type");
            ddd.Columns.Add("time");
            ddd.Columns.Add("cl_count");
            ddd.Columns.Add("downloadPath");
            if (access_sql.yzTable(ds2))
            {
                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                {
                    string lll = ds2.Tables[0].Rows[i][0].ToString();
                    string des = ds2.Tables[0].Rows[i][1].ToString();
                    string time = ds2.Tables[0].Rows[i][2].ToString();
                    string cl_count = ds2.Tables[0].Rows[i][3].ToString();
                    if (ddd.Select("leimu='" + lll + "'").Length == 0)
                    {
                        ddd.Rows.Add(new object[] { lll, des, time, cl_count });
                    }
                }
            }
            if (access_sql.yzTable(ds))
            {


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    string lll = ds.Tables[0].Rows[i][0].ToString();
                    string time = ds.Tables[0].Rows[i][1].ToString();
                    string cl_count = ds.Tables[0].Rows[i][2].ToString();
                    string downloadPath = ds.Tables[0].Rows[i][3].ToString();
                    string errdes= ds.Tables[0].Rows[i][4].ToString();
                    if (downloadPath != "")
                    {
                        downloadPath = "<a href='/" + downloadPath + "' target='_blank' >下载<a>";
                    }
                    if (ddd.Select("leimu='" + lll + "'").Length == 0)
                    {
                        ddd.Rows.Add(new object[] { lll, errdes, time, cl_count, downloadPath });
                    }
                }
            }

            if (ddd != null && ddd.Rows.Count > 0)
            {

                Literal1.Text = "加载数据" + ddd.Rows.Count + "条";
                rplb.DataSource = ddd;
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