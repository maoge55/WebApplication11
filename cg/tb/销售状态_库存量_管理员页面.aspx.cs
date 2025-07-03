using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Serialization;

namespace WebApplication11.cg.tb
{
    public partial class 销售状态_库存量_管理员页面 : System.Web.UI.Page
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
        public const int PageSize = 50;
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

        public void BindData(string sjbm = "", string salesStatus = "", string bname = "", string sortType = "")
        {
            try 
            {
                lits.Text = "";
                
                // 保存筛选条件到ViewState
                ViewState["sjbm"] = sjbm;
                ViewState["salesStatus"] = salesStatus;
                ViewState["bname"] = bname;
                ViewState["sortType"] = sortType;
                
                // 安全处理参数
                string safeSJBM = sjbm.Replace("'", "''");
                string safeSalesStatus = salesStatus.Replace("'", "''");
                string safeBName = bname.Replace("'", "''");
                
                // 构建WHERE条件
                string baseWhereCondition = " WHERE so.SJBM = '" + safeSJBM + "'";
                
                // 浏览器店铺名模糊匹配
                if (!string.IsNullOrEmpty(bname))
                    baseWhereCondition += " AND so.BName LIKE '%" + safeBName + "%'";

                // 销售状态筛选单独处理
                string statusWhereCondition = baseWhereCondition;
                if (!string.IsNullOrEmpty(salesStatus))
                    statusWhereCondition += " AND so.xiaoshouzhuangtai = '" + safeSalesStatus + "'";

                // 构建ORDER BY子句
                string orderByClause = "";
                if (!string.IsNullOrEmpty(sortType))
                {
                    switch (sortType)
                    {
                        case "sales7_desc":
                            orderByClause = " ORDER BY sales_7days DESC";
                            break;
                        case "sales14_desc":
                            orderByClause = " ORDER BY sales_14days DESC";
                            break;
                        case "sales28_desc":
                            orderByClause = " ORDER BY sales_28days DESC";
                            break;
                        case "roas_desc":
                            orderByClause = " ORDER BY ROAS DESC";
                            break;
                        case "roas_asc":
                            orderByClause = " ORDER BY ROAS ASC";
                            break;
                    }
                }
                if (string.IsNullOrEmpty(orderByClause))
                {
                    orderByClause = " ORDER BY itemid";
                }

                // 获取总记录数（使用带销售状态的条件）
                string countSql = string.Format(@"
                    SELECT COUNT(DISTINCT so.itemid)  
                    FROM {0} so 
                    LEFT JOIN {1} psw ON so.ItemID = psw.rucangItemID
                    {2}", SHOPEE_ORDER_TABLE, PURCHASE_SALES_WAREHOUSE_TABLE, statusWhereCondition);
                
                int totalRecords = Convert.ToInt32(access_sql.ExecInt2(countSql));
                TotalPagesCount = (totalRecords + PageSize - 1) / PageSize;

                // 分页参数
                int startRow = (CurrentPageIndex - 1) * PageSize + 1;
                int endRow = CurrentPageIndex * PageSize;

                // 主查询SQL
                string sql = @"
                    WITH BaseData AS (
                        SELECT 
                            so.itemid,
                            MAX(so.BName) as BName,
                            MAX(so.SJBM) as SJBM,
                            MAX(so.pname) as pname,
                            MAX(so.xiaoshouzhuangtai) as xiaoshouzhuangtai
                        FROM " + SHOPEE_ORDER_TABLE + @" so 
                        " + statusWhereCondition + @"
                        GROUP BY so.itemid
                    ),
                    MetricsData AS (
                        SELECT 
                            so.itemid,
                            MAX(so.conversions) as conversions,
                            CAST(ROUND(MAX(so.ROAS), 2) as decimal(10,2)) as ROAS,
                            MAX(so.zuidibeihuoliang_id) as zuidibeihuoliang_id,
                            MAX(so.zuigaobeihuoliang_id) as zuigaobeihuoliang_id,
                            
                            -- 7天销量
                            ISNULL(SUM(CASE 
                                WHEN so.order_date >= DATEADD(day, -7, cast(GETDATE() as date)) 
                                AND so.order_date < cast(GETDATE() as date)
                                AND so.status IN ('To Ship', 'Shipped', 'Delivered', 'Completed')
                                THEN so.amount ELSE 0 END), 0) as sales_7days,
                            
                            -- 14天销量
                            ISNULL(SUM(CASE 
                                WHEN so.order_date >= DATEADD(day, -14, cast(GETDATE() as date)) 
                                AND so.order_date < cast(GETDATE() as date)
                                AND so.status IN ('To Ship', 'Shipped', 'Delivered', 'Completed')
                                THEN so.amount ELSE 0 END), 0) as sales_14days,
                            
                            -- 28天销量
                            ISNULL(SUM(CASE 
                                WHEN so.order_date >= DATEADD(day, -28, cast(GETDATE() as date)) 
                                AND so.order_date < cast(GETDATE() as date)
                                AND so.status IN ('To Ship', 'Shipped', 'Delivered', 'Completed')
                                THEN so.amount ELSE 0 END), 0) as sales_28days
                        FROM " + SHOPEE_ORDER_TABLE + @" so 
                        " + baseWhereCondition + @"
                        GROUP BY so.itemid
                    ),
                    Product1688Info AS (
                        SELECT 
                            so.itemid,
                            MAX(psw.pname_1688) as pname_1688
                        FROM " + SHOPEE_ORDER_TABLE + @" so
                        LEFT JOIN " + PURCHASE_SALES_WAREHOUSE_TABLE + @" psw ON so.ItemID = psw.rucangItemID
                        " + statusWhereCondition + @"
                        GROUP BY so.itemid
                    ),
                    SalesWithAllInfo AS (
                        SELECT 
                            bd.*,
                            md.conversions,
                            md.ROAS,
                            md.zuidibeihuoliang_id,
                            md.zuigaobeihuoliang_id,
                            md.sales_7days,
                            md.sales_14days,
                            md.sales_28days,
                            p1688.pname_1688,
                            img.pimage
                        FROM BaseData bd
                        LEFT JOIN MetricsData md ON bd.itemid = md.itemid
                        LEFT JOIN Product1688Info p1688 ON bd.itemid = p1688.itemid
                        OUTER APPLY (
                            SELECT TOP 1 MAX(so_inner.pimage) as pimage
                            FROM " + SHOPEE_ORDER_TABLE + @" so_inner
                            WHERE so_inner.itemid = bd.itemid
                            GROUP BY so_inner.itemid, so_inner.skuid
                            ORDER BY SUM(so_inner.amount) DESC
                        ) img
                    ),
                    CalculatedData AS (
                        SELECT *,
                            CAST(ROUND(
                                CASE 
                                    WHEN sales_28days > 0 THEN 
                                        sales_7days * sales_14days * 2.0 / sales_28days * 5
                                    ELSE 0 
                                END
                            , 0) as decimal(10,0)) as auto_backup_amount,
                            ROW_NUMBER() OVER (" + orderByClause + @") AS RowNum
                        FROM SalesWithAllInfo
                    )
                    SELECT 
                        itemid,
                        BName,
                        SJBM,
                        pname,
                        pname_1688,
                        conversions,
                        ROAS,
                        sales_7days,
                        sales_14days,
                        sales_28days,
                        auto_backup_amount,
                        zuidibeihuoliang_id,
                        zuigaobeihuoliang_id,
                        xiaoshouzhuangtai,
                        pimage
                    FROM CalculatedData
                    WHERE RowNum BETWEEN " + startRow + " AND " + endRow + @"
                    " + orderByClause;

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
            litPageInfo.Text = string.Format("第{0}页/共{1}页", CurrentPageIndex, TotalPagesCount);
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
            
            BindData(sjbm, salesStatus, bname, sortType);
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
                        ViewState["sortType"] as string);
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
                        ViewState["sortType"] as string);
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
                            ViewState["sortType"] as string);
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
                string itemID = e.CommandArgument.ToString();
                
                DropDownList ddlSalesStatus = e.Item.FindControl("ddlSalesStatus") as DropDownList;
                TextBox txtMinBackupAmount = e.Item.FindControl("txtMinBackupAmount") as TextBox;
                TextBox txtMaxBackupAmount = e.Item.FindControl("txtMaxBackupAmount") as TextBox;
                
                if (ddlSalesStatus == null || string.IsNullOrEmpty(ddlSalesStatus.SelectedValue))
                {
                    lits.Text = "错误：请选择销售状态";
                    return;
                }

                string salesStatusValue = ddlSalesStatus.SelectedValue;
                string minBackupAmount = txtMinBackupAmount != null ? txtMinBackupAmount.Text.Trim() : "";
                string maxBackupAmount = txtMaxBackupAmount != null ? txtMaxBackupAmount.Text.Trim() : "";

                bool updated = false;
                
                // 先更新备货量
                if (access_sql.T_Update_ExecSql(
                    new string[] { "zuidibeihuoliang_id", "zuigaobeihuoliang_id" },
                    new object[] { minBackupAmount, maxBackupAmount },
                    SHOPEE_ORDER_TABLE,
                    "itemid='" + itemID + "'") > 0)
                {
                    updated = true;
                }

                // 更新状态（带优先级规则）
                string statusCondition = $"itemid='{itemID}' AND (xiaoshouzhuangtai <> '{salesStatusValue}' AND " + 
                    $"(xiaoshouzhuangtai = '未处理' OR " +
                    $"(xiaoshouzhuangtai = '在售' AND '{salesStatusValue}' = '停售') OR " +
                    $"(xiaoshouzhuangtai = '停售' AND '{salesStatusValue}' = '停售')))";

                int statusUpdateResult = access_sql.T_Update_ExecSql(
                    new string[] { "xiaoshouzhuangtai" },
                    new object[] { salesStatusValue },
                    SHOPEE_ORDER_TABLE,
                    statusCondition);

                if (statusUpdateResult > 0)
                {
                    updated = true;
                }

                BindData(ViewState["sjbm"] as string, 
                        ViewState["salesStatus"] as string,
                        ViewState["bname"] as string,
                        ViewState["sortType"] as string);

                if (updated)
                {
                    lits.Text = "itemid:" + itemID + " 更新成功" + 
                        (statusUpdateResult == 0 ? "（状态未更新：不满足优先级规则或状态未变）" : "");
                }
                else
                {
                    lits.Text = "更新失败";
                }
            }
            else if (e.CommandName == "ApplyBatch")
            {
                ApplyBatchUpdate();
            }
        }

        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            int updatedCount = 0;
            int statusNotUpdatedCount = 0;
            
            foreach (RepeaterItem item in rplb.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField ItemID = item.FindControl("ItemID") as HiddenField;
                    DropDownList ddlSalesStatus = item.FindControl("ddlSalesStatus") as DropDownList;
                    TextBox txtMinBackupAmount = item.FindControl("txtMinBackupAmount") as TextBox;
                    TextBox txtMaxBackupAmount = item.FindControl("txtMaxBackupAmount") as TextBox;

                    if (ItemID != null && !string.IsNullOrEmpty(ItemID.Value))
                    {
                        string itemId = ItemID.Value;
                        string newStatus = ddlSalesStatus?.SelectedValue ?? "";
                        string minBackupAmount = txtMinBackupAmount != null ? txtMinBackupAmount.Text.Trim() : "";
                        string maxBackupAmount = txtMaxBackupAmount != null ? txtMaxBackupAmount.Text.Trim() : "";

                        bool updated = false;

                        // 先更新备货量
                        if (access_sql.T_Update_ExecSql(
                            new string[] { "zuidibeihuoliang_id", "zuigaobeihuoliang_id" },
                            new object[] { minBackupAmount, maxBackupAmount },
                            SHOPEE_ORDER_TABLE,
                            "itemid='" + itemId + "'") > 0)
                        {
                            updated = true;
                        }

                        // 更新状态（带优先级规则）
                        string statusCondition = $"itemid='{itemId}' AND (xiaoshouzhuangtai <> '{newStatus}' AND " + 
                            $"(xiaoshouzhuangtai = '未处理' OR " +
                            $"(xiaoshouzhuangtai = '在售' AND '{newStatus}' = '停售') OR " +
                            $"(xiaoshouzhuangtai = '停售' AND '{newStatus}' = '停售')))";

                        int statusUpdateResult = access_sql.T_Update_ExecSql(
                            new string[] { "xiaoshouzhuangtai" },
                            new object[] { newStatus },
                            SHOPEE_ORDER_TABLE,
                            statusCondition);

                        if (statusUpdateResult == 0)
                        {
                            statusNotUpdatedCount++;
                        }

                        if (updated || statusUpdateResult > 0)
                        {
                            updatedCount++;
                        }
                    }
                }
            }

            BindData(ViewState["sjbm"] as string, 
                    ViewState["salesStatus"] as string,
                    ViewState["bname"] as string,
                    ViewState["sortType"] as string);
            
            lits.Text = $"整页保存完成，更新 {updatedCount} 条记录" + 
                (statusNotUpdatedCount > 0 ? $"（其中 {statusNotUpdatedCount} 条记录的状态因不满足优先级规则或状态未变而未更新）" : "");
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
            string statusText = batchValue; // 直接使用文本值作为状态文本

            bool anyChecked = false;
            int updatedCount = 0;

            // 遍历所有行，只处理选中的行
            foreach (RepeaterItem item in rplb.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox chk = item.FindControl("chkItem") as CheckBox;
                    HiddenField ItemID = item.FindControl("ItemID") as HiddenField;

                    if (chk != null && chk.Checked && ItemID != null)
                    {
                        anyChecked = true;
                        string itemId = ItemID.Value;

                        // 更新状态（带优先级规则）
                        string statusCondition = $"itemid='{itemId}' AND (xiaoshouzhuangtai <> '{batchValue}' AND " + 
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
                    ViewState["sortType"] as string);

            lits.Text = "成功更新了 " + updatedCount + " 条记录的销售状态为：" + statusText;
        }
    }
} 