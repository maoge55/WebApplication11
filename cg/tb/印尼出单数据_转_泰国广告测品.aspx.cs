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
    public partial class 印尼出单数据_转_泰国广告测品 : System.Web.UI.Page
    {
        // 数据表名变量 - 方便后续修改
        private const string TABLE_NAME = "ShopeeOrder";
        
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

        public void bindzhy(string adStatus = "")
        {
            lits.Text = "";
            ViewState["adStatus"] = adStatus;
            
            string safeAdStatus = adStatus.Replace("'", "''");
            
            // 构建WHERE条件
            string whereCondition = " WHERE status = 'Completed'";
            
            // 泰国广告测品状态筛选
            if (!string.IsNullOrEmpty(adStatus))
                whereCondition += " AND is_to_thad = '" + safeAdStatus + "'";

            // 获取总记录数
            string countSql = string.Format(@"
                SELECT COUNT(DISTINCT itemid)  
                FROM {0} 
                {1}", TABLE_NAME, whereCondition);

            int totalRecords = Convert.ToInt32(access_sql.ExecInt2(countSql));
            TotalPagesCount = (totalRecords + PageSize - 1) / PageSize;

            // 分页参数
            int startRow = (CurrentPageIndex - 1) * PageSize + 1;
            int endRow = CurrentPageIndex * PageSize;

            // 主查询SQL - 按itemid分组并使用聚合函数
            string sql = @"
                WITH GroupedData AS (
                    SELECT 
                        itemid,
                        MAX(pimage) as pimage,
                        MAX(purl) as purl,
                        MAX(BName) as BName,
                        MAX(pname) as pname,
                        SUM(amount) as total_amount,
                        MIN(is_to_thad) as is_to_thad,
                        ROW_NUMBER() OVER (ORDER BY SUM(amount) DESC) AS RowNum
                    FROM " + TABLE_NAME + @" 
                    " + whereCondition + @"
                    GROUP BY itemid
                )
                SELECT 
                    itemid,
                    pimage,
                    purl,
                    BName,
                    pname,
                    total_amount,
                    is_to_thad
                FROM GroupedData
                WHERE RowNum BETWEEN " + startRow + " AND " + endRow + @"
                ORDER BY RowNum;";

            DataSet ds = access_sql.GreatDs(sql, 300);

            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];
                rplb.DataSource = dt;
                rplb.DataBind();
            }
            else
            {
                rplb.DataSource = null;
                rplb.DataBind();
                lits.Text = "无数据";
            }
            UpdatePagerInfo();
        }

        // 页面刷新，分页需要
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

        // 查找按钮
        protected void Button1_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            string adStatus = ddlAdStatus.SelectedValue;
            bindzhy(adStatus: adStatus);
        }

        // 上一页按钮
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex > 1)
            {
                CurrentPageIndex--;
                bindzhy(ViewState["adStatus"] as string);
            }
        }

        // 下一页按钮
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex < TotalPagesCount)
            {
                CurrentPageIndex++;
                bindzhy(ViewState["adStatus"] as string);
            }
        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "qr")
            {
                lits.Text = "";
                // 前端传的ItemID
                string itemID = e.CommandArgument.ToString();
                
                // 从Request中获取checkbox状态
                string yesCheckboxName = "chkIsToThadYes_" + e.Item.ItemIndex;
                string noCheckboxName = "chkIsToThadNo_" + e.Item.ItemIndex;
                
                bool isYesChecked = Request.Form[yesCheckboxName] != null;
                bool isNoChecked = Request.Form[noCheckboxName] != null;
                
                // 验证至少选择一个状态
                if (!isYesChecked && !isNoChecked)
                {
                    lits.Text = "错误：请至少选择一个状态（是 或 否）";
                    return;
                }
                
                // 确定最终状态
                string thadValue;
                if (isYesChecked && !isNoChecked)
                {
                    thadValue = "1";  // 可泰国广告测品
                }
                else if (!isYesChecked && isNoChecked)
                {
                    thadValue = "-1"; // 不可泰国广告测品
                }
                else
                {
                    // 如果都选择了（理论上不应该发生，因为前端已经互斥），默认为未处理
                    thadValue = "0";  // 未处理
                }
                
                // 更新语句 - 根据itemid更新所有相关记录
                if (access_sql.T_Update_ExecSql(new string[] { "is_to_thad" },
                  new object[] { thadValue },
                  TABLE_NAME,
                 "itemid='" + itemID + "'") > 0)
                {
                    bindzhy(ViewState["adStatus"] as string);
                    lits.Text = "itemid:" + itemID + "更新成功";
                }
            }
            else if (e.CommandName == "ApplyBatch")
            {
                // 批量应用状态
                ApplyBatchUpdate();
            }
        }



        private void ApplyBatchUpdate()
        {
            // 从隐藏字段获取批量设置的状态
            HiddenField hdnBatchStatus = rplb.Controls[0].FindControl("hdnBatchStatus") as HiddenField;
            
            if (hdnBatchStatus == null || string.IsNullOrEmpty(hdnBatchStatus.Value))
            {
                lits.Text = "请先在表头选择要设置的状态（是或否）";
                return;
            }

            string batchValue = hdnBatchStatus.Value;
            string statusText = batchValue == "1" ? "可泰国广告测品" : (batchValue == "-1" ? "不可泰国广告测品" : "未处理");

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

                        string condition = "itemid='" + itemId + "'";

                        if (access_sql.T_Update_ExecSql(
                            new string[] { "is_to_thad" },
                            new object[] { batchValue },
                            TABLE_NAME,
                            condition) > 0)
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
            hdnBatchStatus.Value = "";

            // 刷新数据（需保留分页状态）
            bindzhy(ViewState["adStatus"] as string);

            lits.Text = "成功更新了 " + updatedCount + " 条记录为：" + statusText;
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
                    // 刷新数据（保留搜索条件）
                    bindzhy(ViewState["adStatus"] as string);
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