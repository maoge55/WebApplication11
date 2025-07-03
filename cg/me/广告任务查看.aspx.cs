using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 广告任务查看 : System.Web.UI.Page
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
                string mbid = e.CommandArgument.ToString();
                int a = access_sql.T_Update_ExecSql(new string[] { "isupdate" }, new object[] { 1 }, "ShopeeReport", "mbid='" + mbid + "'");

                if (a > 0)
                {
                    lits.Text = "类目:" + mbid + "更新成功";
                    bind();
                }
                else
                {
                    lits.Text = "类目:" + mbid + "更新失败";
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

            string sql1 = "SELECT product_id,Search_Query,DpName FROM YN_AD_CSV  where ((Search_volume>=100 and wordnum is null) or (Search_volume>=50 and wordnum is not null)) and Product_ID in (select product_id from campaign where is_download_bb=1 and update_time >= CAST(GETDATE() AS DATE)) and is_added=0 and is_edited=0 and DpName is not null";
            string sql2 = "SELECT product_id,Search_Query,DpName FROM YN_AD_CDC where  is_added=0 and is_edited=0 and Product_ID in (select product_id from campaign where is_download_bb=1 and update_time >= CAST(GETDATE() AS DATE)) and is_added=0 and is_edited=0 and DpName is not null";


            DataSet ds1 = access_sql.GreatDs(sql1);
            DataSet ds2 = access_sql.GreatDs(sql2);

            if (access_sql.yzTable(ds1) || access_sql.yzTable(ds2))
            {
                // 合并 DataSet（假设已执行）
                ds1.Merge(ds2, true, MissingSchemaAction.AddWithKey);
                DataTable mergedTable = ds1.Tables[0];

                // 多级分组去重
                var groupedData = mergedTable.AsEnumerable()
                    .Where(row => !row.IsNull("DpName") && !row.IsNull("product_id") && !row.IsNull("Search_Query"))
                    .GroupBy(row => new {
                        DpName = row.Field<string>("DpName"),
                        ProductId = row.Field<int>("product_id")
                    })
                    .Select(g => new {
                        DpName = g.Key.DpName,
                        ProductId = g.Key.ProductId,
                        UniqueQueries = g.Select(r => r.Field<string>("Search_Query")).Distinct().ToList()
                    });

                // 构建结果表
                DataTable resultTable = new DataTable();
                resultTable.Columns.Add("DpName", typeof(string));
                resultTable.Columns.Add("product_id", typeof(int));
                resultTable.Columns.Add("ALLstr", typeof(string));
                resultTable.Columns.Add("Tongji", typeof(int));

                foreach (var group in groupedData)
                {
                    DataRow newRow = resultTable.NewRow();
                    newRow["DpName"] = group.DpName;
                    newRow["product_id"] = group.ProductId;
                    newRow["ALLstr"] = string.Join(";", group.UniqueQueries);
                    newRow["Tongji"] = group.UniqueQueries.Count;
                    resultTable.Rows.Add(newRow);
                }

              

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