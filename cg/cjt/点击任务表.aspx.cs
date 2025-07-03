using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;
using System.Text;

namespace WebApplication11.cg.cjt
{
    public partial class 点击任务表 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 检查是否为API请求

            if (Request.QueryString["action"] == "getTaskData")
            {
                GetApiData();
                Response.End();
            }
            if (!access_sql.yzdlcg())
            {
                Response.Redirect("/cg/clogin.aspx");
            }


            else
            {
                u = HttpContext.Current.Request.Cookies["cu"].Value;
                p = HttpContext.Current.Request.Cookies["cp"].Value;
                uid = HttpContext.Current.Request.Cookies["cuid"].Value;
                if (uid != "9")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }


                else
                {
                    if (!IsPostBack)
                    {
                        // 创建新数据表结构
                        DataTable dt = new DataTable();
                        dt.Columns.Add("ItemID", typeof(string));
                        dt.Columns.Add("click_count", typeof(string));

                        // 添加一行空数据
                        DataRow dr = dt.NewRow();
                        dr["ItemID"] = "";
                        dr["click_count"] = "";
                        dt.Rows.Add(dr);

                        // 绑定数据
                        rplb.DataSource = dt;
                        rplb.DataBind();
                    }
                }



            }



        }

        public string u = "";
        public string p = "";
        public string uid = "";

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {


            if (e.CommandName == "qr")
            {


                TextBox txtY_ItemID = (TextBox)e.Item.FindControl("txtY_ItemID");
                TextBox txtY_click = (TextBox)e.Item.FindControl("txtY_click");



                string itemId = txtY_ItemID.Text.Trim().Replace("'", "''");
                string clickCount = txtY_click.Text.Trim().Replace("'", "''");


                if (string.IsNullOrEmpty(itemId))
                {
                    lits.Text = "ItemID不能为空";
                    return;
                }

                // 修正查询语句（增加N前缀处理中文）
                int count = Convert.ToInt32(access_sql.ExecInt2("SELECT COUNT(*) FROM Click_Task WHERE ItemID = N'" + itemId + "'"));
                if (count > 0)
                {
                    // 存在则更新
                    if ((access_sql.T_Update_ExecSql(new string[] { "click_count" }, new object[] { clickCount }, "Click_Task", "ItemID = N'" + itemId + "'")) > 0)
                    {
                        lits.Text = "更新成功";
                    }
                    else
                    {
                        lits.Text = "更新失败";
                    }

                }
                else
                {
                    // 不存在则插入
                    if (access_sql.T_Insert_ExecSqls(new string[] { "ItemID", "click_count" }, new object[] { itemId, clickCount }, "Click_Task"))
                    {
                        lits.Text = "新建成功";
                    }
                    else
                    {
                        lits.Text = "新建失败";
                    }
                }
            }


        }

        public DataSet GetClickTaskData(string sjbm)
        {
            string safeSjbm = sjbm.Trim().Replace("'", "''");

            string query = string.Format(@"SELECT BID, pname, purl, SJBM, ItemID, BName,
        click_count, pimage FROM Click_Task WHERE click_count>0 AND  SJBM = '{0}'", safeSjbm);

            return access_sql.GreatDs(query);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string sjbm = txtSJBM.Text.Trim().Replace("'", "''");
            string BName = txtBID.Text.Trim().Replace("'", "''"); // 获取BID筛选值
            string status = ddlStatus.SelectedValue; // 获取ItemID状态
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append(@"SELECT DISTINCT BID, pname, purl, SJBM, ItemID, BName,
        click_count, pimage 
        FROM Click_Task 
        WHERE click_count>0 AND SJBM = '" + sjbm + "'");


            if (!string.IsNullOrEmpty(BName))
            {
                queryBuilder.Append(" AND BName LIKE '%" + BName + "%'");
            }


            if (status == "有任务ItemID")
            {
                queryBuilder.Append(" AND ItemID IS NOT NULL AND ItemID <> ''");
            }
            else if (status == "无任务ItemID")
            {
                queryBuilder.Append(" AND (ItemID IS NULL OR ItemID = '')");
            }

            DataSet ds = access_sql.GreatDs(queryBuilder.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                rpModal.DataSource = ds;
                rpModal.DataBind();
                ClientScript.RegisterStartupScript(this.GetType(), "showModal", "document.getElementById('modal').style.display='block';", true);
            }
            else
            {
                lits.Text = "<script>alert('未找到相关记录');</script>";
            }
        }


        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            bool allSuccess = true;
            foreach (RepeaterItem item in rpModal.Items)
            {
                if (item.ItemType != ListItemType.Item &&
                    item.ItemType != ListItemType.AlternatingItem)
                    continue;


                Literal litItemID = item.FindControl("litItemID") as Literal;
                TextBox txtY_click = item.FindControl("txtY_click") as TextBox;


                string safeClick = txtY_click.Text.Trim().Replace("'", "''");
                string safeItemID = litItemID.Text.Trim().Replace("'", "''"); // 使用Literal的Text属性

                string updateSql = string.Format(@"
                        UPDATE Click_Task 
                        SET click_count = '{0}' 
                        WHERE ItemID = '{1}'", safeClick, safeItemID);

                if (!access_sql.ExecSql(updateSql))
                    allSuccess = false;
            }
            lits.Text = allSuccess ? "全部保存成功" : "部分保存失败";
            btnSearch_Click(sender, e); // 刷新模态框
        }


        private void GetApiData()
        {
            try
            {
                int page = 1;
                int pageSize = 100;

                if (!string.IsNullOrEmpty(Request["page"]))
                {
                    int.TryParse(Request["page"], out page);
                    if (page < 1) page = 1;
                }

                if (!string.IsNullOrEmpty(Request["pagesize"]))
                {
                    int.TryParse(Request["pagesize"], out pageSize);
                    if (pageSize < 1) pageSize = 100;
                }

                int offset = (page - 1) * pageSize;

                // 查询总数量
                string countSql = @"
            SELECT COUNT(*) 
            FROM Click_Task c1 
            JOIN Click_Task_KW c2 ON c1.ItemID = c2.itemid where c1.click_count>0";

                int totalCount = Convert.ToInt32(access_sql.GetOneValue(countSql));

                // 分页查询数据
                string dataSql = $@"
            SELECT
                c1.purl,
                c2.KW,
                c2.SearchCount,
                c1.price_min,
                c1.price_max,
                c1.stock,
                c1.click_count
            FROM Click_Task c1
            JOIN Click_Task_KW c2 ON c1.ItemID = c2.itemid
            WHERE c1.click_count>0
            ORDER BY c1.ItemID
            OFFSET {offset} ROWS
            FETCH NEXT {pageSize} ROWS ONLY";

                DataSet ds = access_sql.GreatDs(dataSql);
                DataTable dt = ds.Tables[0];

                var list = new List<Dictionary<string, object>>();
                foreach (DataRow row in dt.Rows)
                {
                    var entry = new Dictionary<string, object>
                    {
                        ["purl"] = row.IsNull("purl") ? "" : row["purl"].ToString(),
                        ["kw"] = row.IsNull("KW") ? "" : row["KW"].ToString(),
                        ["SearchCount"] = row.IsNull("SearchCount") ? 0 : Convert.ToInt32(row["SearchCount"]),
                        ["price_min"] = row.IsNull("price_min") ? 0.0 : Convert.ToDouble(row["price_min"]),
                        ["price_max"] = row.IsNull("price_max") ? 0.0 : Convert.ToDouble(row["price_max"]),
                        ["stock"] = row.IsNull("stock") ? 0 : Convert.ToInt32(row["stock"]),
                        ["click_count"] = row.IsNull("click_count") ? null : (object)Convert.ToInt32(row["click_count"])
                    };
                    list.Add(entry);
                }

                // 包装最终返回数据
                var result = new
                {
                    page,
                    pagesize = pageSize,
                    total = totalCount,
                    list
                };

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.None,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(result, settings));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.StatusCode = 500;
                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(new
                {
                    error = "API请求失败: " + ex.Message
                }));
                Response.End();
            }
        }


    }
}
