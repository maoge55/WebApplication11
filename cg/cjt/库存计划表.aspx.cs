using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;


namespace WebApplication11.cg.cjt
{
    public partial class 库存计划表 : System.Web.UI.Page
    {
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
                if (uid != "9")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
                if (!IsPostBack)
                {
                    ViewState["SortDirection"] = "DESC"; // 默认降序
                }
            }
        }
        public string u = "";
        public string p = "";
        public string uid = "";


        public void BindData(string SJBM = "", string status = "", string BName = "")
        {
            ViewState["SJBM"] = SJBM;
            ViewState["status"] = status;

            ViewState["BName"] = BName;

            string safeSJBM = !string.IsNullOrEmpty(SJBM) ? SJBM.Replace("'", "''") : "";
            string safeStatus = !string.IsNullOrEmpty(status) ? status.Replace("'", "''") : "";
            string safeBName = !string.IsNullOrEmpty(BName) ? BName.Replace("'", "''") : "";
            string sortDirection = ViewState["SortDirection"] as string ?? "DESC";
            string whereCondition = " WHERE 1=1";
            whereCondition += " AND s.status IN ('Shipped', 'Delivered', 'Completed')";

            if (!string.IsNullOrEmpty(safeSJBM))
                whereCondition += " AND s.SJBM = '" + safeSJBM + "'";

            if (!string.IsNullOrEmpty(safeBName))
                whereCondition += " AND s.BName LIKE '%" + safeBName + "%'";

            if (!string.IsNullOrEmpty(safeStatus) && status != "-1")
                whereCondition += " AND s.xiaoshouzhuangtai = '" + safeStatus + "'";

            // 查询总记录数
            string countSql = string.Format(@"
                SELECT COUNT(DISTINCT s.ItemID)
                FROM ShopeeOrder s
                LEFT JOIN caiwu c ON s.ItemID = c.rucangITEMID
                {0}", whereCondition);

            int totalRecords = Convert.ToInt32(access_sql.ExecInt2(countSql));
            TotalPagesCount = (totalRecords + PageSize - 1) / PageSize;

            // 分页查询
            int startRow = (CurrentPageIndex - 1) * PageSize + 1;
            int endRow = CurrentPageIndex * PageSize;
            string sql = string.Format(@"
    SELECT * FROM (
        SELECT 
            FinalResults.*,
            ROW_NUMBER() OVER (ORDER BY MaintainQuantity {3}) AS RowNum
        FROM (
            SELECT 
                
                MAX(Sub.SJBM) AS SJBM,
                MAX(Sub.pimage) AS pimage,
                MAX(Sub.BName) AS BName,
                MAX(Sub.pname) AS pname,
                MAX(Sub.huopinbiaoti) AS huopinbiaoti,
                Sub.ItemID AS ItemID,
                MAX(Sub.ItemIDxuweichi) AS ItemIDxuweichi,
                MAX(Sub.xiaoshouzhuangtai) AS xiaoshouzhuangtai,
                MAX(Sub.conversions) AS  conversions,
                MAX(Sub.ROAS) AS  ROAS,
                COUNT(DISTINCT CASE 
                    WHEN Sub.order_date >= DATEADD(DAY, -7, CAST(GETDATE() AS DATE)) 
                    THEN Sub.order_id 
                END) AS day_sales_7,
                COUNT(DISTINCT CASE 
                    WHEN Sub.order_date >= DATEADD(DAY, -14, CAST(GETDATE() AS DATE)) 
                    THEN Sub.order_id 
                END) AS day_sales_14,
                COUNT(DISTINCT CASE 
                    WHEN Sub.order_date >= DATEADD(DAY, -30, CAST(GETDATE() AS DATE)) 
                    THEN Sub.order_id 
                END) AS day_sales_30,
                MAX(Sub.status) AS status,
                -- 计算维持数量作为排序字段
CAST(
    ROUND(
        CASE 
            WHEN COUNT(DISTINCT CASE 
                    WHEN Sub.order_date >= DATEADD(DAY, -30, CAST(GETDATE() AS DATE)) 
                    THEN Sub.order_id 
                END) >= 60 
                THEN COUNT(DISTINCT CASE 
                    WHEN Sub.order_date >= DATEADD(DAY, -30, CAST(GETDATE() AS DATE)) 
                    THEN Sub.order_id 
                END) * 1.5
            WHEN COUNT(DISTINCT CASE 
                    WHEN Sub.order_date >= DATEADD(DAY, -30, CAST(GETDATE() AS DATE)) 
                    THEN Sub.order_id 
                END) >= 30 
                THEN COUNT(DISTINCT CASE 
                    WHEN Sub.order_date >= DATEADD(DAY, -30, CAST(GETDATE() AS DATE)) 
                    THEN Sub.order_id 
                END) * 1.2
            ELSE 
                CASE 
                    WHEN (COUNT(DISTINCT CASE 
                            WHEN Sub.order_date >= DATEADD(DAY, -7, CAST(GETDATE() AS DATE)) 
                            THEN Sub.order_id 
                        END) * 4) < COUNT(DISTINCT CASE 
                            WHEN Sub.order_date >= DATEADD(DAY, -30, CAST(GETDATE() AS DATE)) 
                            THEN Sub.order_id 
                        END) 
                    THEN COUNT(DISTINCT CASE 
                            WHEN Sub.order_date >= DATEADD(DAY, -30, CAST(GETDATE() AS DATE)) 
                            THEN Sub.order_id 
                        END)
                    ELSE COUNT(DISTINCT CASE 
                            WHEN Sub.order_date >= DATEADD(DAY, -7, CAST(GETDATE() AS DATE)) 
                            THEN Sub.order_id 
                        END) * 4.0
                END
        END, 
        0
    ) AS INT
) AS MaintainQuantity
            FROM (
                SELECT DISTINCT 
                    s.SJBM,
                    s.pimage,
                    s.BName,
                    s.pname,
                    c.huopinbiaoti,
                    s.ItemID,
                    s.ItemIDxuweichi,
                    s.xiaoshouzhuangtai,
                    s.status,
                    s.order_id,
                    s.order_date,
                    s.conversions,
                    s.ROAS
                FROM ShopeeOrder s
                LEFT JOIN caiwu c ON s.ItemID = c.rucangITEMID
                {0}
            ) AS Sub
            GROUP BY Sub.ItemID 
        ) AS FinalResults
    ) AS SortedResults
    WHERE RowNum BETWEEN {1} AND {2}",
            whereCondition, startRow, endRow, sortDirection);

            DataSet ds = access_sql.GreatDs(sql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                rptItems.DataSource = ds;
                rptItems.DataBind();
            }
            else
            {
                rptItems.DataSource = null;
                rptItems.DataBind();
                lits.Text = "无数据";
            }

            UpdatePagerInfo();
        }

        private void UpdatePagerInfo()
        {
            btnPrev.Enabled = CurrentPageIndex > 1;
            btnNext.Enabled = CurrentPageIndex < TotalPagesCount;
            litCurrentPage.Text = CurrentPageIndex.ToString();
            litTotalPages.Text = TotalPagesCount.ToString();
            litPageInfo.Text = string.Format("第{0}页/共{1}页", CurrentPageIndex, TotalPagesCount);
            // 新增：同步跳转输入框的值
            txtJumpPage.Text = CurrentPageIndex.ToString();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            string sjbm = txtsjbm.Text.Trim();
            string status = ddlStatus.SelectedValue;
            string bname = txtBName.Text.Trim();
            ViewState["SortDirection"] = ddlSort.SelectedValue;
            BindData(sjbm, status, bname);
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex > 1)
            {
                CurrentPageIndex--;
                BindData(
                    ViewState["SJBM"] as string,
                    ViewState["status"] as string,
                    ViewState["BName"] as string
                );
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex < TotalPagesCount)
            {
                CurrentPageIndex++;
                BindData(
                    ViewState["SJBM"] as string,
                    ViewState["status"] as string,
                    ViewState["BName"] as string
                );
            }
        }

        protected void rptItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SaveItem")
            {
                SaveSingleItem(e);
            }
            else if (e.CommandName == "ApplyBatch")
            {
                ApplyBatchUpdate();
            }
        }

        private void SaveSingleItem(RepeaterCommandEventArgs e)
        {
            ulong ItemID = Convert.ToUInt64(e.CommandArgument);
            TextBox txtMaintainQuantity = e.Item.FindControl("txtMaintainQuantity") as TextBox;
            DropDownList ddlSaleStatus = e.Item.FindControl("ddlSaleStatus") as DropDownList;
            HiddenField hdnMaintainQty = e.Item.FindControl("hdnMaintainQty") as HiddenField;
            if (txtMaintainQuantity != null && ddlSaleStatus != null && hdnMaintainQty != null)
            {
                
                string maintainQty = !string.IsNullOrWhiteSpace(txtMaintainQuantity.Text)
                    ? txtMaintainQuantity.Text.Trim().Replace("'", "''")
                    : hdnMaintainQty.Value; 

                string saleStatus = ddlSaleStatus.SelectedValue;



                if (access_sql.T_Update_ExecSql(new string[] { "ItemIDxuweichi", "xiaoshouzhuangtai" }, new object[] { maintainQty, saleStatus }, "ShopeeOrder", "ItemID='" + ItemID + "'") > 0)
                {
                    lits.Text = "更新成功";
                    BindData(
                        ViewState["SJBM"] as string,
                        ViewState["status"] as string,
                        ViewState["BName"] as string
                    );
                }
            }
        }

private void ApplyBatchUpdate()
{
    // 正确获取批量操作的状态值
    DropDownList ddlBatchStatus = (DropDownList)rptItems.Controls[0].Controls[0].FindControl("ddlBatchStatus");
    
    if (ddlBatchStatus == null || string.IsNullOrEmpty(ddlBatchStatus.SelectedValue))
    {
        lits.Text = "请选择要应用的状态";
        return;
    }

    string batchStatus = ddlBatchStatus.SelectedValue;
    int updatedCount = 0;
    
    foreach (RepeaterItem item in rptItems.Items)
    {
        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox chk = item.FindControl("chkItem") as CheckBox;
            if (chk != null && chk.Checked)
            {
                HiddenField hdnItemID = item.FindControl("hdnItemID") as HiddenField;
                TextBox txtMaintainQuantity = item.FindControl("txtMaintainQuantity") as TextBox;

                if (hdnItemID != null && !string.IsNullOrEmpty(hdnItemID.Value))
                {
                    ulong itemID = Convert.ToUInt64(hdnItemID.Value);
                    string maintainQty = txtMaintainQuantity != null ?
                        txtMaintainQuantity.Text.Trim().Replace("'", "''") : "";
                    
                    
                    if (access_sql.T_Update_ExecSql(
                        new string[] { "ItemIDxuweichi", "xiaoshouzhuangtai" }, 
                        new object[] { maintainQty, batchStatus }, 
                        "ShopeeOrder", 
                        "ItemID='" + itemID + "'") > 0)
                    {
                        updatedCount++;
                    }
                }
            }
        }
    }

    lits.Text = "成功更新 " + updatedCount + " 条记录";
    BindData(
        ViewState["SJBM"] as string,
        ViewState["status"] as string,
        ViewState["BName"] as string
    );
}
        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            int updatedCount = 0;
            foreach (RepeaterItem item in rptItems.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hdnItemID = item.FindControl("hdnItemID") as HiddenField;
                    TextBox txtMaintainQuantity = item.FindControl("txtMaintainQuantity") as TextBox;
                    DropDownList ddlSaleStatus = item.FindControl("ddlSaleStatus") as DropDownList;
                    HiddenField hdnMaintainQty = item.FindControl("hdnMaintainQty") as HiddenField;
                    if (hdnItemID != null && !string.IsNullOrEmpty(hdnItemID.Value) && hdnMaintainQty != null)
                    {
                        ulong itemID = Convert.ToUInt64(hdnItemID.Value);

                        // 判断输入框是否有内容
                        string maintainQty = !string.IsNullOrWhiteSpace(txtMaintainQuantity.Text)
                            ? txtMaintainQuantity.Text.Trim().Replace("'", "''")
                            : hdnMaintainQty.Value; // 使用隐藏字段的原始值

                        string saleStatus = ddlSaleStatus != null ?
                            ddlSaleStatus.SelectedValue : "";


                        if (access_sql.T_Update_ExecSql(new string[] { "ItemIDxuweichi", "xiaoshouzhuangtai" }, new object[] { maintainQty, saleStatus }, "ShopeeOrder", "ItemID='" + itemID + "'") > 0)
                        {
                            updatedCount++;
                        }
                    }
                }
            }

            lits.Text = "整页保存完成，更新 " + updatedCount + " 条记录";
            BindData(
                ViewState["SJBM"] as string,
                ViewState["status"] as string,
                ViewState["BName"] as string
            );
        }


        protected string CalculateMaintainQuantity(object sales30Obj, object sales7Obj)
        {
            int sales30 = sales30Obj is DBNull ? 0 : Convert.ToInt32(sales30Obj);
            int sales7 = sales7Obj is DBNull ? 0 : Convert.ToInt32(sales7Obj);

            double result;

            if (sales30 >= 60)
            {
                result = sales30 * 1.5;
            }
            else if (sales30 >= 30)
            {
                result = sales30 * 1.2;
            }
            else
            {
                result = sales7 * 4;
                if (result < sales30) result = sales30;
            }

            // 将四舍五入改为强制转换为int实现取整
            return ((int)result).ToString("0");
        }

        protected string GetMaintainQuantityText(object dataItem)
        {
            // 默认返回空字符串，由前端显示原始值
            return string.Empty;
        }
        // 获取显示在页面上的值（优先显示已保存的 ItemIDxuweichi）
      
        protected string GetDisplayMaintainQuantity(object itemIDxuweichi, object maintainQuantity)
        {
            if (itemIDxuweichi != null && !string.IsNullOrWhiteSpace(itemIDxuweichi.ToString()))
            {
                return itemIDxuweichi.ToString();
            }
            else if (maintainQuantity != null)
            {
                return maintainQuantity.ToString();
            }
            return "";
        }

        // 获取实际保存到隐藏字段的值（用于保存逻辑）
        protected string GetActualMaintainQuantity(object itemIDxuweichi, object maintainQuantity)
        {
            if (itemIDxuweichi != null && !string.IsNullOrWhiteSpace(itemIDxuweichi.ToString()))
            {
                return itemIDxuweichi.ToString();
            }
            else if (maintainQuantity != null)
            {
                return  maintainQuantity.ToString();
            }
            return "";
        }

        // 获取文本框初始值（只显示已保存的值）
        protected string GetTextBoxValue(object itemIDxuweichi)
        {
            if (itemIDxuweichi != null && !string.IsNullOrWhiteSpace(itemIDxuweichi.ToString()))
            {
                return itemIDxuweichi.ToString();
            }
            return string.Empty; // 没有保存值时文本框留空
        }

        protected string FormatROAS(object value)
        {
            if (value == DBNull.Value || value == null)
                return "0.0"; // 处理空值情况

            decimal roas;
            if (decimal.TryParse(value.ToString(), out roas))
            {
                // 保留1位小数，四舍五入
                return roas.ToString("F1");
            }

            return "0.0"; // 解析失败时的默认值
        }
        protected void BtnJump_Click(object sender, EventArgs e)
        {
            int pageNumber;
            if (int.TryParse(txtJumpPage.Text.Trim(), out pageNumber))
            {
                // 验证页码范围
                if (pageNumber >= 1 && pageNumber <= TotalPagesCount)
                {
                    CurrentPageIndex = pageNumber;
                    BindData(
                        ViewState["SJBM"] as string,
                        ViewState["status"] as string,
                        ViewState["BName"] as string
                    );
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
    }
}