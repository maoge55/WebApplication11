using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication11.cg.tb
{
    public partial class SKU优化停售_管理员页面 : System.Web.UI.Page
    {
        // 数据表名变量
        private const string SHOPEE_ORDER_TABLE = "ShopeeOrder";
        private const string PURCHASE_SALES_WAREHOUSE_TABLE = "Purchase_Sales_Warehouse";
        
        // 分页属性
        private int CurrentPageIndex
        {
            get
            {
                if (ViewState["CurrentPage"] != null)
                    return (int)ViewState["CurrentPage"];
                return 1;
            }
            set
            {
                ViewState["CurrentPage"] = value;
            }
        }
        public const int PageSize = 1;
        private int TotalPagesCount
        {
            get
            {
                if (ViewState["TotalPages"] != null)
                    return (int)ViewState["TotalPages"];
                return 0;
            }
            set
            {
                ViewState["TotalPages"] = value;
            }
        }

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
                if (uid != "6" && uid != "9")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }
        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public void BindData(string sjbm = "", string salesStatus = "", string bname = "", string sortType = "", string skuCount = "")
        {
            try 
            {
                lits.Text = "";
                
                // 保存筛选条件到ViewState
                ViewState["sjbm"] = sjbm;
                ViewState["salesStatus"] = salesStatus;
                ViewState["bname"] = bname;
                ViewState["sortType"] = sortType;
                ViewState["skuCount"] = skuCount;
                
                // 安全处理参数
                string safeSJBM = sjbm.Replace("'", "''");
                string safeSalesStatus = salesStatus.Replace("'", "''");
                string safeBName = bname.Replace("'", "''");
                int minSkuCount = 0;
                if (!string.IsNullOrEmpty(skuCount))
                {
                    minSkuCount = Convert.ToInt32(skuCount);
                }
                
                // 构建WHERE条件
                string baseWhereCondition = " WHERE so.SJBM = '" + safeSJBM + "'";
                
                // 浏览器店铺名模糊匹配
                if (!string.IsNullOrEmpty(bname))
                    baseWhereCondition += " AND so.BName LIKE '%" + safeBName + "%'";

                // 销售状态筛选单独处理
                string statusWhereCondition = baseWhereCondition;
                if (!string.IsNullOrEmpty(salesStatus))
                    statusWhereCondition += " AND so.xiaoshouzhuangtai = '" + safeSalesStatus + "'";

                // SKU数量筛选
                string skuCountFilter = "";
                if (minSkuCount > 0)
                {
                    skuCountFilter = @"
                    HAVING COUNT(DISTINCT so.skuid) > " + minSkuCount;
                }

                // 构建ORDER BY子句
                string orderByClause = "";
                if (!string.IsNullOrEmpty(sortType))
                {
                    switch (sortType)
                    {
                        case "sku_sales7_desc":
                            orderByClause = " ORDER BY sales_7days DESC";
                            break;
                        case "sku_sales14_desc":
                            orderByClause = " ORDER BY sales_14days DESC";
                            break;
                        case "sku_sales28_desc":
                            orderByClause = " ORDER BY sales_28days DESC";
                            break;
                    }
                }
                if (string.IsNullOrEmpty(orderByClause))
                {
                    orderByClause = " ORDER BY itemid";
                }

                // 获取总记录数（使用带销售状态和SKU数量的条件）
                string countSql = string.Format(@"
                    SELECT COUNT(t.itemid)
                    FROM (
                        SELECT so.itemid
                        FROM {0} so 
                        LEFT JOIN {1} psw ON so.ItemID = psw.rucangItemID
                        {2}
                        GROUP BY so.itemid
                        {3}
                    ) t", 
                    SHOPEE_ORDER_TABLE, 
                    PURCHASE_SALES_WAREHOUSE_TABLE, 
                    statusWhereCondition,
                    skuCountFilter);
                
                int totalRecords = Convert.ToInt32(access_sql.ExecInt2(countSql));
                TotalPagesCount = (totalRecords + PageSize - 1) / PageSize;

                // 分页参数
                int startRow = (CurrentPageIndex - 1) * PageSize + 1;
                int endRow = CurrentPageIndex * PageSize;

                // 主查询SQL
                string sql = @"
                    WITH ItemSales AS (
                        -- 计算ItemID级别的28天销量（不受销售状态过滤影响）
                        SELECT 
                            itemid,
                            ISNULL(SUM(CASE 
                                WHEN order_date >= DATEADD(day, -28, cast(GETDATE() as date)) 
                                AND order_date < cast(GETDATE() as date)
                                AND status IN ('To Ship', 'Shipped', 'Delivered', 'Completed')
                                THEN amount ELSE 0 END), 0) as item_sales_28days
                        FROM " + SHOPEE_ORDER_TABLE + @" so
                        " + baseWhereCondition + @"
                        GROUP BY itemid
                    ),
                    FilteredBaseData AS (
                        -- 获取符合销售状态和SKU数量条件的基础数据
                        SELECT itemid
                        FROM " + SHOPEE_ORDER_TABLE + @" so
                        " + statusWhereCondition + @"
                        GROUP BY itemid
                        " + skuCountFilter + @"
                    ),
                    RankedItems AS (
                        -- 按ItemID28天销量降序排序，但只包含符合销售状态条件的ItemID
                        SELECT 
                            i.itemid,
                            ROW_NUMBER() OVER (ORDER BY i.item_sales_28days DESC,i.itemid) as ItemRank
                        FROM ItemSales i
                        INNER JOIN FilteredBaseData f ON i.itemid = f.itemid
                    ),
                    BaseData AS (
                        SELECT 
                            so.itemid,
                            so.skuid,
                            MAX(so.BName) as BName,
                            MAX(so.SJBM) as SJBM,
                            MAX(so.pname) as pname,
                            MAX(so.xiaoshouzhuangtai) as xiaoshouzhuangtai
                        FROM " + SHOPEE_ORDER_TABLE + @" so 
                        " + statusWhereCondition + @"
                        GROUP BY so.itemid, so.skuid
                    ),
                    MetricsData AS (
                        SELECT 
                            so.itemid,
                            so.skuid,
                            MAX(so.conversions) as conversions,
                            CAST(ROUND(MAX(so.ROAS), 2) as decimal(10,2)) as ROAS,
                            -- SKU7天销量
                            ISNULL(SUM(CASE 
                                WHEN so.order_date >= DATEADD(day, -7, cast(GETDATE() as date)) 
                                AND so.order_date < cast(GETDATE() as date)
                                AND so.status IN ('To Ship', 'Shipped', 'Delivered', 'Completed')
                                THEN so.amount ELSE 0 END), 0) as sales_7days,
                            
                            -- SKU14天销量
                            ISNULL(SUM(CASE 
                                WHEN so.order_date >= DATEADD(day, -14, cast(GETDATE() as date)) 
                                AND so.order_date < cast(GETDATE() as date)
                                AND so.status IN ('To Ship', 'Shipped', 'Delivered', 'Completed')
                                THEN so.amount ELSE 0 END), 0) as sales_14days,
                            
                            -- SKU28天销量
                            ISNULL(SUM(CASE 
                                WHEN so.order_date >= DATEADD(day, -28, cast(GETDATE() as date)) 
                                AND so.order_date < cast(GETDATE() as date)
                                AND so.status IN ('To Ship', 'Shipped', 'Delivered', 'Completed')
                                THEN so.amount ELSE 0 END), 0) as sales_28days
                        FROM " + SHOPEE_ORDER_TABLE + @" so 
                        " + baseWhereCondition + @"
                        GROUP BY so.itemid, so.skuid
                    ),
                    Product1688Info AS (
                        SELECT 
                            so.itemid,
                            so.skuid,
                            MAX(psw.pname_1688) as pname_1688
                        FROM " + SHOPEE_ORDER_TABLE + @" so
                        LEFT JOIN " + PURCHASE_SALES_WAREHOUSE_TABLE + @" psw ON so.SKUID = psw.rucangSKUID
                        " + statusWhereCondition + @"
                        GROUP BY so.itemid, so.skuid
                    ),
                    SalesWithAllInfo AS (
                        SELECT 
                            bd.*,
                            md.conversions,
                            md.ROAS,
                            md.sales_7days,
                            md.sales_14days,
                            md.sales_28days,
                            p1688.pname_1688,
                            img.pimage,
                            ri.ItemRank,
                            is2.item_sales_28days
                        FROM BaseData bd
                        LEFT JOIN MetricsData md ON bd.itemid = md.itemid AND bd.skuid = md.skuid
                        LEFT JOIN Product1688Info p1688 ON bd.itemid = p1688.itemid AND bd.skuid = p1688.skuid
                        INNER JOIN RankedItems ri ON bd.itemid = ri.itemid
                        LEFT JOIN ItemSales is2 ON bd.itemid = is2.itemid
                        OUTER APPLY (
                            SELECT TOP 1 MAX(so_inner.pimage) as pimage
                            FROM " + SHOPEE_ORDER_TABLE + @" so_inner
                            WHERE so_inner.itemid = bd.itemid AND so_inner.skuid = bd.skuid
                            GROUP BY so_inner.itemid, so_inner.skuid
                            ORDER BY SUM(so_inner.amount) DESC
                        ) img
                    ),
                    CalculatedData AS (
                        SELECT *,
                            ROW_NUMBER() OVER (PARTITION BY itemid " + orderByClause + @") AS RowNum
                        FROM SalesWithAllInfo
                    )
                    SELECT 
                        itemid,
                        skuid,
                        BName,
                        SJBM,
                        pname,
                        pname_1688,
                        conversions,
                        ROAS,
                        sales_7days,
                        sales_14days,
                        sales_28days,
                        xiaoshouzhuangtai,
                        pimage
                    FROM CalculatedData cd
                    WHERE cd.ItemRank BETWEEN " + startRow + " AND " + endRow + @"
                    ORDER BY cd.ItemRank, " + orderByClause.Replace("ORDER BY ", "");

                DataSet ds = access_sql.GreatDs(sql, 300);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    rplb.DataSource = ds.Tables[0];
                    rplb.DataBind();
                    
                    // 更新分页信息
                    UpdatePagerInfo();
                }
                else
                {
                    rplb.DataSource = null;
                    rplb.DataBind();
                    lits.Text = "无数据";
                }
            }
            catch (Exception ex)
            {
                lits.Text = "查询出错：" + ex.Message;
            }
        }

        // 页面刷新，分页需要
        private void UpdatePagerInfo()
        {
            btnPrev.Enabled = CurrentPageIndex > 1;
            btnNext.Enabled = CurrentPageIndex < TotalPagesCount;
            litCurrentPage.Text = CurrentPageIndex.ToString();
            litTotalPages.Text = TotalPagesCount.ToString();
            // 同步跳转输入框的值
            txtJumpPage.Text = CurrentPageIndex.ToString();
        }

        // 查找按钮
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string sjbm = txtSJBM.Text.Trim();
            
            if (string.IsNullOrEmpty(sjbm))
            {
                lits.Text = "商家编码为必填项";
                return;
            }
            
            CurrentPageIndex = 1;
            string salesStatus = ddlSalesStatusFilter.SelectedValue;
            string bname = txtBName.Text.Trim();
            string sortType = ddlSortType.SelectedValue;
            string skuCount = txtSkuCount.Text.Trim();
            
            BindData(sjbm, salesStatus, bname, sortType, skuCount);
        }

        // 上一页按钮
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex > 1)
            {
                CurrentPageIndex--;
                BindData(ViewState["sjbm"] as string, 
                        ViewState["salesStatus"] as string,
                        ViewState["bname"] as string,
                        ViewState["sortType"] as string,
                        ViewState["skuCount"] as string);
            }
        }

        // 下一页按钮
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex < TotalPagesCount)
            {
                CurrentPageIndex++;
                BindData(ViewState["sjbm"] as string, 
                        ViewState["salesStatus"] as string,
                        ViewState["bname"] as string,
                        ViewState["sortType"] as string,
                        ViewState["skuCount"] as string);
            }
        }

        // 跳转按钮
        protected void btnJump_Click(object sender, EventArgs e)
        {
            int pageNumber;
            if (int.TryParse(txtJumpPage.Text.Trim(), out pageNumber))
            {
                // 验证页码范围
                if (pageNumber >= 1 && pageNumber <= TotalPagesCount)
                {
                    CurrentPageIndex = pageNumber;
                    // 刷新数据（保留搜索条件）
                    BindData(ViewState["sjbm"] as string, 
                            ViewState["salesStatus"] as string,
                            ViewState["bname"] as string,
                            ViewState["sortType"] as string,
                            ViewState["skuCount"] as string);
                }
                else
                {
                    lits.Text = "页码必须在1到" + TotalPagesCount + "之间";
                }
            }
            else
            {
                lits.Text = "请输入有效的数字页码";
            }
        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Save")
            {
                lits.Text = "";
                string[] args = e.CommandArgument.ToString().Split('|');
                if (args.Length != 2)
                {
                    lits.Text = "参数错误";
                    return;
                }

                string itemID = args[0];
                string skuID = args[1];
                
                DropDownList ddlSalesStatus = e.Item.FindControl("ddlSalesStatus") as DropDownList;
                
                if (ddlSalesStatus == null || string.IsNullOrEmpty(ddlSalesStatus.SelectedValue))
                {
                    lits.Text = "错误：请选择销售状态";
                    return;
                }

                string salesStatusValue = ddlSalesStatus.SelectedValue;

                // 更新状态（带优先级规则）
                string statusCondition = $"itemid='{itemID}' AND skuid='{skuID}' AND (xiaoshouzhuangtai <> '{salesStatusValue}' AND " + 
                    $"(xiaoshouzhuangtai = '未处理' OR " +
                    $"(xiaoshouzhuangtai = '在售' AND '{salesStatusValue}' = '停售') OR " +
                    $"(xiaoshouzhuangtai = '停售' AND '{salesStatusValue}' = '停售')))";

                int statusUpdateResult = access_sql.T_Update_ExecSql(
                    new string[] { "xiaoshouzhuangtai" },
                    new object[] { salesStatusValue },
                    SHOPEE_ORDER_TABLE,
                    statusCondition);

                BindData(ViewState["sjbm"] as string, 
                        ViewState["salesStatus"] as string,
                        ViewState["bname"] as string,
                        ViewState["sortType"] as string,
                        ViewState["skuCount"] as string);

                if (statusUpdateResult > 0)
                {
                    lits.Text = $"itemid:{itemID}, skuid:{skuID} 更新成功";
                }
                else
                {
                    lits.Text = "更新失败（状态未更新：不满足优先级规则或状态未变）";
                }
            }
            else if (e.CommandName == "ApplyBatch")
            {
                ApplyBatchUpdate();
            }
        }

        private void ApplyBatchUpdate()
        {
            // 从隐藏字段获取批量设置的状态
            HiddenField hdnBatchSalesStatus = rplb.Controls[0].FindControl("hdnBatchSalesStatus") as HiddenField;
            
            if (hdnBatchSalesStatus == null || string.IsNullOrEmpty(hdnBatchSalesStatus.Value))
            {
                lits.Text = "请先在表头选择要设置的销售状态";
                return;
            }

            string batchValue = hdnBatchSalesStatus.Value;
            string statusText = batchValue;

            bool anyChecked = false;
            int updatedCount = 0;

            // 遍历所有行，只处理选中的行
            foreach (RepeaterItem item in rplb.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox chk = item.FindControl("chkItem") as CheckBox;
                    HiddenField ItemID = item.FindControl("ItemID") as HiddenField;
                    HiddenField SKUID = item.FindControl("SKUID") as HiddenField;

                    if (chk != null && chk.Checked && ItemID != null && SKUID != null)
                    {
                        anyChecked = true;
                        string itemId = ItemID.Value;
                        string skuId = SKUID.Value;

                        // 更新状态（带优先级规则）
                        string statusCondition = $"itemid='{itemId}' AND skuid='{skuId}' AND (xiaoshouzhuangtai <> '{batchValue}' AND " + 
                            $"(xiaoshouzhuangtai = '未处理' OR " +
                            $"(xiaoshouzhuangtai = '在售' AND '{batchValue}' = '停售') OR " +
                            $"(xiaoshouzhuangtai = '停售' AND '{batchValue}' = '停售')))";

                        if (access_sql.T_Update_ExecSql(
                            new string[] { "xiaoshouzhuangtai" },
                            new object[] { batchValue },
                            SHOPEE_ORDER_TABLE,
                            statusCondition) > 0)
                        {
                            updatedCount++;
                        }
                    }
                }
            }

            if (!anyChecked)
            {
                lits.Text = "请至少选择一项";
                return;
            }

            // 清空隐藏字段
            hdnBatchSalesStatus.Value = "";

            // 刷新数据（需保留分页状态）
            BindData(ViewState["sjbm"] as string, 
                    ViewState["salesStatus"] as string,
                    ViewState["bname"] as string,
                    ViewState["sortType"] as string,
                    ViewState["skuCount"] as string);

            lits.Text = "成功更新了 " + updatedCount + " 条记录的销售状态为：" + statusText;
        }

        // 快速停售按钮
        protected void btnQuickStop_Click(object sender, EventArgs e)
        {
            string skuid = txtQuickStopSKUID.Text.Trim();
            if (string.IsNullOrEmpty(skuid))
            {
                lits.Text = "请输入要停售的SKUID";
                return;
            }

            // 更新状态（带优先级规则）
            string statusCondition = $"skuid='{skuid}' AND (xiaoshouzhuangtai <> '停售' AND " + 
                $"(xiaoshouzhuangtai = '未处理' OR xiaoshouzhuangtai = '在售'))";

            int result = access_sql.T_Update_ExecSql(
                new string[] { "xiaoshouzhuangtai" },
                new object[] { "停售" },
                SHOPEE_ORDER_TABLE,
                statusCondition);

            if (result > 0)
            {
                // 刷新数据
                BindData(ViewState["sjbm"] as string, 
                        ViewState["salesStatus"] as string,
                        ViewState["bname"] as string,
                        ViewState["sortType"] as string,
                        ViewState["skuCount"] as string);
                lits.Text = $"成功将SKUID: {skuid} 设置为停售状态";                        
            }
            else
            {
                lits.Text = $"SKUID: {skuid} 更新失败（不存在或不满足优先级规则）";
            }
        }
    }
} 