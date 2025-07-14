using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace WebApplication11.cg.cjt
{
    public partial class 广告测品订单 : System.Web.UI.Page
    {
        // 分页属性
        private int CurrentPage
        {
            get { return ViewState["_CurrentPage"] != null ? (int)ViewState["_CurrentPage"] : 1; }
            set { ViewState["_CurrentPage"] = value; }
        }

        private int PageSize
        {
            get { return 50; }
        }

        private int TotalRecords
        {
            get { return ViewState["_TotalRecords"] != null ? (int)ViewState["_TotalRecords"] : 0; }
            set { ViewState["_TotalRecords"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            if (!access_sql.yzdlcg())
            {
                Response.Redirect("/cg/clogin.aspx");
            }
            else
            {
                u = HttpContext.Current.Request.Cookies["cu"].Value;
                p = HttpContext.Current.Request.Cookies["cp"].Value;
                uid = HttpContext.Current.Request.Cookies["cuid"].Value;
                if (uid != "19" && uid != "6" && uid != "18" && uid != "9")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }
        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public void bindzhy(String SJBM = "", string status = "", bool keepPage = false)
        {

            if (!keepPage) CurrentPage = 1;
            string safeSJBM = !string.IsNullOrEmpty(SJBM) ? SJBM.Replace("'", "''") : "";
            string whereCondition = !string.IsNullOrEmpty(safeSJBM)
                ? " AND o.SJBM = '" + safeSJBM + "'"
                : "";

            string statusCondition = "";
            if (!string.IsNullOrEmpty(status) && status != "-1")
            {
                statusCondition = " AND o.shi_fou = " + status;
            }

            string sql = string.Format(@"
            WITH 
              SKUCount AS ( 
                  SELECT 
                      skuid, 
                      COUNT(DISTINCT buyer_id) AS sku_amount 
                  FROM 
                      SuMaiTongPol.dbo.ShopeeOrder 
                  GROUP BY 
                      skuid 
              ), 
              ItemCount AS ( 
                  SELECT 
                      ItemID, 
                      COUNT(DISTINCT buyer_id) AS item_amount 
                  FROM 
                      SuMaiTongPol.dbo.ShopeeOrder 
                  GROUP BY 
                      ItemID 
              ), 
              RankedData AS ( 
                  SELECT 
                      o.SJBM, 
                      o.purl, 
                      o.pimage, 
                      o.sku_name, 
                      o.skuid, 
                      o.bid, 
                      o.BName, 
                      o.pname, 
                      o.ItemID, 
                      o.shi_fou, 
                      o.id, 
                      o.conversions,
                      sc.sku_amount, 
                      ic.item_amount,
                      o.Y_1688url,
                      o.Y_1688sku1, 
                      o.Y_1688sku2, 
                      o.Y_1688sku3, 
                      o.Y_1688price,
                      ROW_NUMBER() OVER ( 
                          PARTITION BY o.skuid 
                          ORDER BY o.ItemID 
                      ) AS rn  
                  FROM 
                      SuMaiTongPol.dbo.ShopeeOrder AS o 
                  INNER JOIN SKUCount AS sc ON o.skuid = sc.skuid 
                  INNER JOIN ItemCount AS ic ON ic.ItemID = o.ItemID 
                  LEFT JOIN YNBigData AS y ON y.itemID = o.ItemID 
                  WHERE 
                      o.BName LIKE '%广告%' 
                      AND ic.item_amount >= 3 
                      {0} {1}
              ),
              PagedData AS (
                  SELECT 
                      *,
                      ROW_NUMBER() OVER (ORDER BY item_amount DESC, ItemID) AS RowNumber,
                      COUNT(*) OVER() AS TotalCount
                  FROM (
                      SELECT 
                          SJBM, purl, pimage, sku_name, skuid, bid, BName, pname, 
                          sku_amount, ItemID, item_amount, shi_fou, id, conversions,
                          Y_1688url, Y_1688sku1, Y_1688sku2, Y_1688sku3, Y_1688price
                      FROM 
                          RankedData 
                      WHERE 
                          rn = 1 
                      GROUP BY 
                          SJBM, purl, pimage, sku_name, skuid, bid, BName, pname, 
                          ItemID, item_amount, shi_fou, id, conversions,
                          Y_1688url, Y_1688sku1, 
                          Y_1688sku2, Y_1688sku3, Y_1688price, sku_amount 
                  ) AS FilteredData
              )
              SELECT 
                  SJBM, purl, pimage, sku_name, skuid, bid, BName, pname, 
                  sku_amount, ItemID, item_amount, shi_fou, id, conversions,
                  Y_1688url, Y_1688sku1, Y_1688sku2, Y_1688sku3, Y_1688price,
                  TotalCount
              FROM 
                  PagedData
              WHERE 
                  RowNumber BETWEEN {2} AND {3}",
              statusCondition, whereCondition,
              (CurrentPage - 1) * PageSize + 1,
              CurrentPage * PageSize);

            DataSet ds = access_sql.GreatDs(sql);

            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    TotalRecords = Convert.ToInt32(dt.Rows[0]["TotalCount"]);
                }

                foreach (DataRow row in dt.Rows)
                {
                    string skuName = row["sku_name"] as string;
                    string y1688url = row["Y_1688url"] as string;

                    // 提取第二个数字作为skuid
                    string skuid1688 = "";
                    if (!string.IsNullOrEmpty(skuName))
                    {
                        Match match = Regex.Match(skuName, @"\[(\d{12})\s+(\d{13})\]");
                        if (match.Success)
                        {
                            string num1 = match.Groups[1].Value;  // 12位数字
                            string num2 = match.Groups[2].Value;  // 13位数字

                            // 精确验证数字长度
                            if (num1.Length == 12 && num2.Length == 13)
                            {
                                skuid1688 = num2; // 使用第二个13位数字作为skuid


                                string newUrl = $"https://detail.1688.com/offer/{num2}.html";
                                if (string.IsNullOrEmpty(y1688url))
                                    row["Y_1688url"] = newUrl;
                            }
                        }
                    }
                    // 关联查询S1688ProSKU表
                    if (!string.IsNullOrEmpty(skuid1688))
                    {
                        string query = @"SELECT TOP 1 sku1, sku2, sku_price, itemID 
                                       FROM S1688ProSKU 
                                       WHERE skuid = @skuid";

                        using (SqlConnection conn = new SqlConnection(access_sql.connstring))
                        {
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.Add(new SqlParameter("@skuid", SqlDbType.VarChar, 200)).Value = skuid1688;

                                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                                {
                                    DataTable proData = new DataTable();
                                    adapter.Fill(proData);

                                    if (proData.Rows.Count > 0)
                                    {
                                        DataRow sourceRow = proData.Rows[0];

                                        // 改进：仅当原始值为空时赋值
                                        string originalSku1 = row["Y_1688sku1"] as string;
                                        row["Y_1688sku1"] = string.IsNullOrEmpty(originalSku1)
                                            ? (sourceRow["sku1"] is DBNull ? string.Empty : sourceRow["sku1"].ToString().Trim())
                                            : originalSku1;

                                        string originalSku2 = row["Y_1688sku2"] as string;
                                        row["Y_1688sku2"] = string.IsNullOrEmpty(originalSku2)
                                            ? (sourceRow["sku2"] is DBNull ? string.Empty : sourceRow["sku2"].ToString().Trim())
                                            : originalSku2;

                                        // 处理 Y_1688price（需类型转换）
                                        object originalPrice = row["Y_1688price"];
                                        if (originalPrice == null || originalPrice is DBNull || Convert.ToDecimal(originalPrice) == 0)
                                        {
                                            try
                                            {
                                                decimal price = Convert.ToDecimal(sourceRow["sku_price"]);
                                                row["Y_1688price"] = price.ToString("0.00");
                                            }
                                            catch
                                            {
                                                row["Y_1688price"] = 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    row.AcceptChanges();
                }

                rplb.DataSource = dt;
                rplb.DataBind();
            }
            else
            {
                rplb.DataSource = null;
                rplb.DataBind();
                lits.Text = "无数据";
                TotalRecords = 0;
            }

            UpdatePagerControls();
        }

        private void UpdatePagerControls()
        {
            int totalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);

            btnFirst.Enabled = CurrentPage > 1;
            btnPrev.Enabled = CurrentPage > 1;
            btnNext.Enabled = CurrentPage < totalPages;
            btnLast.Enabled = CurrentPage < totalPages;

            litCurrentPage.Text = CurrentPage.ToString();
            litTotalPages.Text = totalPages.ToString("N0");
            litTotalRecords.Text = TotalRecords.ToString("N0");
            // 新增：同步跳转输入框的值
            txtJumpPage.Text = CurrentPage.ToString();
        }

        // 分页按钮事件
        protected void BtnFirst_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            // 添加true参数保持搜索条件
            bindzhy(txtsjbm.Text.Trim(), ddlStatus.SelectedValue, true);
        }

        protected void BtnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                bindzhy(txtsjbm.Text.Trim(), ddlStatus.SelectedValue, true);
            }
        }

        protected void BtnNext_Click(object sender, EventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
            if (CurrentPage < totalPages)
            {
                CurrentPage++;
                bindzhy(txtsjbm.Text.Trim(), ddlStatus.SelectedValue, true);
            }
        }

        protected void BtnLast_Click(object sender, EventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
            CurrentPage = totalPages;
            bindzhy(txtsjbm.Text.Trim(), ddlStatus.SelectedValue, true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            bindzhy(txtsjbm.Text.Trim(), ddlStatus.SelectedValue);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {


            if (e.CommandName == "qr")
            {
                lits.Text = "";
                ulong skuid = Convert.ToUInt64(e.CommandArgument);
                TextBox txtY_1688url = e.Item.FindControl("txtY_1688url") as TextBox;
                TextBox txtY_1688sku1 = e.Item.FindControl("txtY_1688sku1") as TextBox;
                TextBox txtY_1688sku2 = e.Item.FindControl("txtY_1688sku2") as TextBox;
                TextBox txtY_1688sku3 = e.Item.FindControl("txtY_1688sku3") as TextBox;
                TextBox txtY_1688price = e.Item.FindControl("txtY_1688price") as TextBox;
                DropDownList cbShifou = e.Item.FindControl("cbShifou") as DropDownList;



                if (access_sql.T_Update_ExecSql(new string[] { "Y_1688url", "Y_1688sku1", "Y_1688sku2", "Y_1688sku3", "Y_1688price", "shi_fou" }, new object[] { txtY_1688url.Text.Trim().Replace("'", "''"), txtY_1688sku1.Text.Trim().Replace("'", "''"), txtY_1688sku2.Text.Trim().Replace("'", "''"), txtY_1688sku3.Text.Trim().Replace("'", "''"), txtY_1688price.Text.Trim().Replace("'", "''"), cbShifou.SelectedValue }, "ShopeeOrder", "skuid='" + skuid + "'") > 0)
                {

                  bindzhy();
                    lits.Text = "skuid:" + skuid + "更新成功";
                }

            }
        }
        public void clzy()
        {
            int cg = 0;


            for (int i = 0; i < rplb.Items.Count; i++)
            {
                Literal id = (Literal)rplb.Items[i].FindControl("skuid");
                String skuid = id.Text;
                TextBox txtY_1688url = rplb.Items[i].FindControl("txtY_1688url") as TextBox;
                TextBox txtY_1688sku1 = rplb.Items[i].FindControl("txtY_1688sku1") as TextBox;
                TextBox txtY_1688sku2 = rplb.Items[i].FindControl("txtY_1688sku2") as TextBox;
                TextBox txtY_1688sku3 = rplb.Items[i].FindControl("txtY_1688sku3") as TextBox;
                TextBox txtY_1688price = rplb.Items[i].FindControl("txtY_1688price") as TextBox;
                DropDownList cbShifou = rplb.Items[i].FindControl("cbShifou") as DropDownList;
                cg += access_sql.T_Update_ExecSql(new string[] { "Y_1688url", "Y_1688sku1", "Y_1688sku2", "Y_1688sku3", "Y_1688price", "shi_fou" }, new object[] { txtY_1688url.Text.Trim().Replace("'", "''"), txtY_1688sku1.Text.Trim().Replace("'", "''"), txtY_1688sku2.Text.Trim().Replace("'", "''"), txtY_1688sku3.Text.Trim().Replace("'", "''"), txtY_1688price.Text.Trim().Replace("'", "''"), cbShifou.SelectedValue }, "ShopeeOrder", "skuid='" + skuid + "'");
            }
            bindzhy();
            lits.Text = "更新成功" + cg + "个";

        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            clzy();
        }

        protected void btnSyncStatus_Click(object sender, EventArgs e)
        {
            int successCount = 0;
            foreach (RepeaterItem item in rplb.Items)
            {
                // 获取隐藏的ItemID（需要在ASPX中添加隐藏控件存储ItemID）
                Literal litItemID = item.FindControl("litItemID") as Literal;
                if (litItemID == null || string.IsNullOrEmpty(litItemID.Text)) continue;

                string itemId = litItemID.Text.Trim();
                // 查询同ItemID的最大非0状态
                string targetStatus = GetSyncStatus(itemId);
                if (string.IsNullOrEmpty(targetStatus)) continue;

                // 获取当前记录的skuid（假设skuid在隐藏控件中）
                Literal skuidLit = item.FindControl("skuid") as Literal;
                if (skuidLit == null || string.IsNullOrEmpty(skuidLit.Text)) continue;

                ulong skuid = Convert.ToUInt64(skuidLit.Text);
                // 更新当前记录的shi_fou状态
                if (UpdateShiFou(skuid, targetStatus))
                {
                    successCount++;
                }
            }
            bindzhy();
            lits.Text = "成功同步" + successCount + "条记录的状态";
        }

        // 获取同ItemID的最大非0状态
        private string GetSyncStatus(string itemId)
        {
            string sql = @"SELECT TOP 1 shi_fou 
                           FROM ShopeeOrder 
                           WHERE ItemID = @ItemID AND shi_fou <> 0 
                           ORDER BY shi_fou DESC";

            using (SqlConnection conn = new SqlConnection(access_sql.connstring))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ItemID", itemId);
                    object result = cmd.ExecuteScalar();
                    return result != null && result != DBNull.Value ? result.ToString() : null;
                }
            }
        }

        // 更新指定skuid的shi_fou状态
        private bool UpdateShiFou(ulong skuid, string newStatus)
        {
            return access_sql.T_Update_ExecSql(
                new string[] { "shi_fou" },
                new object[] { newStatus },
                "ShopeeOrder",
                "skuid=" + skuid + ""
            ) > 0;
        }

        protected string CalculateRequiredQuantity(object conversionsObj, object skuAmountObj)
        {
            int conversions = conversionsObj is DBNull ? 0 : Convert.ToInt32(conversionsObj);
            int skuAmount = skuAmountObj is DBNull ? 0 : Convert.ToInt32(skuAmountObj);

            return conversions >= 3
                ? Math.Min(skuAmount * 2, 6).ToString()
                : Math.Min(skuAmount, 3).ToString();
        }
        protected void BtnJump_Click(object sender, EventArgs e)
        {
            int pageNumber;
            if (int.TryParse(txtJumpPage.Text.Trim(), out pageNumber))
            {
                int totalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);

                // 验证页码范围
                if (pageNumber >= 1 && pageNumber <= totalPages)
                {
                    CurrentPage = pageNumber;
                    bindzhy(txtsjbm.Text.Trim(), ddlStatus.SelectedValue, true);
                }
                else
                {
                    lits.Text = "页码超出有效范围 (1-" + totalPages + ")";
                }
            }
            else
            {
                lits.Text = "请输入有效的数字页码";
            }
        }
    }
}