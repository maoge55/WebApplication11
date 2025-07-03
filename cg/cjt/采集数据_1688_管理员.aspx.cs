using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebApplication11.cg.cjt
{
    public partial class 采集数据_1688_管理员 : System.Web.UI.Page
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
        // 分页属性
        public const int PageSize = 50;
        // 分页属性
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
                if (uid != "9")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

            if (!IsPostBack)
            {
                bindzhy();
                pnlKeywordEditor.Visible = false; // 初始隐藏关键词面板
                pnlFilterEditor.Visible = false;   // 初始隐藏过滤词面板
            }
    


        }

        public string u = "";
        public string p = "";
        public string uid = "";

        public void bindzhy(String priority = "", string orderBy = "", string xz = "",string key = "",string yybm="")
        {
            //优先级别
            ViewState["priority"] = priority;
            //出单率
            ViewState["orderBy"] = orderBy;
            //性质
            ViewState["xz"] = xz;
            //关键词
            ViewState["key"] = key;
            //优先级别
            ViewState["yybm"] = yybm;
 
            string safeOrderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "";
            string safexz = !string.IsNullOrEmpty(xz) && xz != "-1" ? xz.Replace("'", "''") : "";
            string safekey = !string.IsNullOrEmpty(key) && key != "-1" ? key.Replace("'", "''") : "";
            string safeyybm = !string.IsNullOrEmpty(yybm) && yybm != "-1" ? yybm.Replace("'", "''") : "";
            string safepriority = !string.IsNullOrEmpty(priority) && priority != "-1" ? priority.Replace("'", "''") : "";
            string whereCondition = "";

            if (!string.IsNullOrEmpty(safexz))
            {
                whereCondition += " AND  RIGHT(sp.shop_name, 4) LIKE '%" + safexz + "%'";
            }
            if (!string.IsNullOrEmpty(safekey))
            {
                whereCondition += " AND ss.state = " + key;
            }
            if (!string.IsNullOrEmpty(safeyybm))
            {
                whereCondition += " AND ss.YYBM = '" + yybm+"'";
            }
            if (!string.IsNullOrEmpty(safepriority))
            {
                whereCondition += " AND ss.Priority = '" + priority + "'";
            }

            string orderByClause = "";
            switch (safeOrderBy)
            {
                case "高到低":
                    orderByClause = "[出单率] DESC";
                    break;
                case "低到高":
                    orderByClause = "[出单率] ASC";
                    break;
                default:
                    orderByClause = "[1688采集关键词]"; 
                    break;
            }
            string countSql = @"
    SELECT COUNT(DISTINCT ss.keywords)
    FROM S1688SearchUrl ss
    LEFT JOIN S1688Pro sp ON ss.id = sp.from_sid 
    WHERE 1=1 " + whereCondition;
            int totalRecords = Convert.ToInt32(access_sql.ExecInt2(countSql));
            TotalPagesCount = (totalRecords + PageSize - 1) / PageSize;

            // 分页查询
            int startRow = (CurrentPageIndex - 1) * PageSize + 1;
            int endRow = CurrentPageIndex * PageSize;
            string sql = @"
WITH random AS (
    SELECT 
        SUBSTRING(
            pname,
            CHARINDEX('|', pname) + 1,
            CASE 
                WHEN CHARINDEX('|', pname, CHARINDEX('|', pname)+1) > 0 
                THEN CHARINDEX('|', pname, CHARINDEX('|', pname)+1) - CHARINDEX('|', pname) - 1
                ELSE LEN(pname)
            END
        ) AS code,
        ItemID
    FROM ShopeeOrder
    WHERE pname LIKE '%|'
),
buyer AS (
    SELECT 
        SUBSTRING(
            pname,
            CHARINDEX('|', pname) + 1,
            CASE 
                WHEN CHARINDEX('|', pname, CHARINDEX('|', pname)+1) > 0 
                THEN CHARINDEX('|', pname, CHARINDEX('|', pname)+1) - CHARINDEX('|', pname) - 1
                ELSE LEN(pname)
            END
        ) AS code,
        buyer_id
    FROM ShopeeOrder
    WHERE pname LIKE '%|'
),

MainData AS (
            select  
            ss.keywords as [1688采集关键词],
            MAX(ss.url) as [1688采集链接],
			SUM(CASE WHEN ss.is_cj = 1 THEN 1 ELSE 0 END) AS [已采集产品数量],
			SUM(CASE WHEN sp.is_yn_dc = 1 THEN 1 ELSE 0 END) AS [印尼已导出产品数量],
			SUM(CASE WHEN sp.is_ph_ad = 1 THEN 1 ELSE 0 END) AS [泰国已导出产品数量],
            SUM(CASE WHEN sp.is_yn_dc = 0 AND sp.is_fy_sku = 1 THEN 1 ELSE 0 END) AS [印尼未导出产品数量],
SUM(CASE WHEN sp.is_ph_ad = 0 AND sp.is_fy_thsku = 1 THEN 1 ELSE 0 END) AS [泰国未导出产品数量],
            CAST(
                    ROUND(
                        COUNT(DISTINCT rn.itemid) * 100.0 / NULLIF(COUNT(ss.is_cj), 0), 
                        2
                    ) AS DECIMAL(10,2)
                ) AS [出单率],

            COUNT(DISTINCT rn.itemid) AS [广告订单_链接总数量],
            COUNT(DISTINCT be.buyer_id) AS [广告订单_买家总数量],
            MAX(ss.minprice) as [1688最低价格],
            MAX(ss.maxprice) as [1688最高价格],
            MAX(ss.sold) as [1688销量],
            MAX(ss.sjbm) as [商家编码],
            MAX(sp.shop_name) as [卖家性质],
            MAX(ss.state) as [关键词状态],
            MAX(ss.YYBM) as [运营编码],
            MAX(ss.Priority) as [优先级别],
ROW_NUMBER() OVER (ORDER BY ss.keywords) AS RowNum
            from S1688SearchUrl ss 
            LEFT JOIN S1688Pro sp on ss.id=sp.from_sid
            LEFT JOIN random rn ON sp.random_code = rn.code 
            LEFT JOIN buyer be ON sp.random_code = be.code 
    WHERE 1=1 " + whereCondition+ @"
    Group By ss.keywords
)
SELECT 
    [1688采集关键词],
    [1688采集链接],
[已采集产品数量],
[印尼已导出产品数量],
[泰国已导出产品数量],
 [印尼未导出产品数量],
 [泰国未导出产品数量],
     [出单率],  
[广告订单_链接总数量],
[广告订单_买家总数量],
[1688最低价格],
[1688最高价格],
[1688销量],
[商家编码],
[关键词状态],
[卖家性质],
[运营编码],
[优先级别]
FROM MainData

WHERE RowNum BETWEEN " + startRow + " AND " + endRow+@"
ORDER BY " + orderByClause ;


            DataSet ds = access_sql.GreatDs(sql);

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
        // 辅助方法：使用当前筛选条件绑定数据
        private void BindDataWithCurrentFilters()
        {
            bindzhy(
                ViewState["priority"] as string,
                ViewState["orderBy"] as string,
                ViewState["xz"] as string,
                ViewState["key"] as string,
                ViewState["yybm"] as string
            );
        }

        // 更新分页信息（修改后）
        private void UpdatePagerInfo()
        {
            // 更新按钮状态
            btnFirst.Enabled = CurrentPageIndex > 1;
            btnPrev.Enabled = CurrentPageIndex > 1;
            btnNext.Enabled = CurrentPageIndex < TotalPagesCount;
            btnLast.Enabled = CurrentPageIndex < TotalPagesCount;

            // 更新页码显示
            litCurrentPage.Text = CurrentPageIndex.ToString();
            litTotalPages.Text = TotalPagesCount.ToString();
            litPageInfo.Text = string.Format("第{0}页/共{1}页", CurrentPageIndex, TotalPagesCount);
            // 同步跳转输入框的值
            txtJumpPage.Text = CurrentPageIndex.ToString();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            string priority = ddlpriority.Text.Trim();


            string orderBy = ddlStatus.SelectedValue;

            string xz = ddlxz.SelectedValue;
            string key = ddlkey.SelectedValue;
            string yybm = ddlyybm.SelectedValue;
            bindzhy(priority, orderBy, xz,key,yybm);

        }

        // 首页按钮事件
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            BindDataWithCurrentFilters();
        }

        // 尾页按钮事件
        protected void btnLast_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = TotalPagesCount;
            BindDataWithCurrentFilters();
        }
        //向前翻页
        protected void btnPrev_Click(object sender, EventArgs e)
            {
                if (CurrentPageIndex > 1)
                {
                    CurrentPageIndex--;
                    bindzhy(
                        ViewState["priority"] as string,
                           ViewState["orderBy"] as string,
                           ViewState["xz"] as string,
                           ViewState["key"] as string,

                           ViewState["yybm"] as string

                    );
                }
            }

        //向后翻页
        protected void btnNext_Click(object sender, EventArgs e)
            {
                if (CurrentPageIndex < TotalPagesCount)
                {
                    CurrentPageIndex++;
                    bindzhy(
                        ViewState["priority"] as string,
                        ViewState["orderBy"] as string,
                        ViewState["xz"] as string,
                        ViewState["key"] as string,
                        ViewState["yybm"] as string

                    );
                }
            }
    //查找按钮
            protected void Button1_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            string priority = ddlpriority.Text.Trim();
            string orderBy = ddlStatus.SelectedValue;
            string xz = ddlxz.SelectedValue;
            string key = ddlkey.SelectedValue;
            string yybm = ddlyybm.SelectedValue;
            // 统一调用方式，明确传递三个参数
            bindzhy(priority: priority, orderBy:orderBy,xz:xz,key:key, yybm: yybm);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }
        //保存功能
        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "qr")
            {
                lits.Text = "";
            //关键词
                string keywords = e.CommandArgument.ToString().Replace("'", "''");
                //最小价格
                TextBox minprice = e.Item.FindControl("minprice") as TextBox;
                //最高价格
                TextBox maxprice = e.Item.FindControl("maxprice") as TextBox;
                //销量大于
                TextBox xiaoliang = e.Item.FindControl("xiaoliang") as TextBox;
                //运营编码
                DropDownList YYBM = e.Item.FindControl("YYBM") as DropDownList;
                //关键词状态
                DropDownList keystatus = e.Item.FindControl("keystatus") as DropDownList;
                //优先级别
                DropDownList priority = e.Item.FindControl("priority") as DropDownList;

                //更新到数据库中
                if (access_sql.T_Update_ExecSql(
                    new string[] { "minprice", "maxprice", "sold", "YYBM", "state", "Priority" },
                    new object[] {
                minprice.Text.Trim().Replace("'", "''"),
                maxprice.Text.Trim().Replace("'", "''"),
                xiaoliang.Text.Trim().Replace("'", "''"),
                YYBM.SelectedValue,
                keystatus.SelectedValue,
                priority.SelectedValue
                    },
                    "S1688SearchUrl",
                    "keywords='" + keywords + "'") > 0)
                {
                    bindzhy(ViewState["priority"] as string, ViewState["orderBy"] as string, ViewState["xz"] as string, ViewState["key"] as string, ViewState["yybm"] as string);
                    lits.Text = keywords.Replace("''", "'") + "更新成功"; 
                }
            }
            else if (e.CommandName == "ApplyBatch")
            {
                ApplyBatchUpdate();
            }
        }

        public void clzy()
        {
            int cg = 0;
            for (int i = 0; i < rplb.Items.Count; i++)
            {
                // 正确查找Literal控件
                Literal key = (Literal)rplb.Items[i].FindControl("key");
                if (key == null) continue;

                string id = key.Text.Replace("'", "''"); // 转义关键词

                TextBox minprice = rplb.Items[i].FindControl("minprice") as TextBox;
                TextBox maxprice = rplb.Items[i].FindControl("maxprice") as TextBox;
                TextBox xiaoliang = rplb.Items[i].FindControl("xiaoliang") as TextBox;
                DropDownList YYBM = rplb.Items[i].FindControl("YYBM") as DropDownList;
                DropDownList keystatus = rplb.Items[i].FindControl("keystatus") as DropDownList;
                DropDownList priority = rplb.Items[i].FindControl("priority") as DropDownList;

                if (minprice != null && maxprice != null && xiaoliang != null &&
                    YYBM != null && keystatus != null && priority != null)
                {
                    cg += access_sql.T_Update_ExecSql(
                        new string[] { "minprice", "maxprice", "sold", "YYBM", "state", "Priority" },
                        new object[] {
                    minprice.Text.Trim().Replace("'", "''"),
                    maxprice.Text.Trim().Replace("'", "''"),
                    xiaoliang.Text.Trim().Replace("'", "''"),
                    YYBM.SelectedValue,
                    keystatus.SelectedValue,
                    priority.SelectedValue
                        },
                        "S1688SearchUrl",
                        "keywords='" + id + "'");
                }
            }

            // 修复：使用正确的ViewState键名
            bindzhy(ViewState["priority"] as string, ViewState["orderBy"] as string, ViewState["xz"] as string, ViewState["key"] as string, ViewState["yybm"] as string);
            lits.Text = "更新成功" + cg + "个";
        }
        protected void Button2_Click1(object sender, EventArgs e)
        {
            clzy();
        }

        //批量应用运营编码，关键词状态，优先级别
        private void ApplyBatchUpdate()
        {
            // 直接从Repeater的Header中查找控件
            DropDownList ddlBatchyybm = rplb.Controls[0].FindControl("ddlBatchyybm") as DropDownList;
            DropDownList ddlBatchkey = rplb.Controls[0].FindControl("ddlBatchkey") as DropDownList;
            DropDownList ddlBatchpriotity = rplb.Controls[0].FindControl("ddlBatchpriotity") as DropDownList;

            // C# 5.0兼容的空值检查
            string batchyybm = (ddlBatchyybm != null) ? ddlBatchyybm.SelectedValue : null;
            string batchkey = (ddlBatchkey != null) ? ddlBatchkey.SelectedValue : null;
            string batchpriotity = (ddlBatchpriotity != null) ? ddlBatchpriotity.SelectedValue : null;

            int updatedCount = 0;
            foreach (RepeaterItem item in rplb.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox chk = item.FindControl("chkItem") as CheckBox;
                    if (chk != null && chk.Checked)
                    {
                        Literal key = item.FindControl("key") as Literal;
                        if (key == null) continue;

                        string id = key.Text.Replace("'", "''");

                        // 获取当前项的原始值
                        DropDownList currentYYBM = item.FindControl("YYBM") as DropDownList;
                        DropDownList currentKeyStatus = item.FindControl("keystatus") as DropDownList;
                        DropDownList currentPriority = item.FindControl("priority") as DropDownList;

                        // 确定最终使用的值
                        string finalYYBM = !string.IsNullOrEmpty(batchyybm) ? batchyybm :
                                          (currentYYBM != null ? currentYYBM.SelectedValue : null);

                        string finalKeyStatus = !string.IsNullOrEmpty(batchkey) ? batchkey :
                                              (currentKeyStatus != null ? currentKeyStatus.SelectedValue : null);

                        string finalPriority = !string.IsNullOrEmpty(batchpriotity) ? batchpriotity :
                                             (currentPriority != null ? currentPriority.SelectedValue : null);

                        // 执行更新
                        if (access_sql.T_Update_ExecSql(
                            new string[] { "YYBM", "state", "Priority" },
                            new object[] { finalYYBM, finalKeyStatus, finalPriority },
                            "S1688SearchUrl",
                            "keywords='" + id + "'") > 0)
                        {
                            updatedCount++;
                        }
                    }
                }
            }

            // 更新显示
            lits.Text = "成功更新 " + updatedCount + " 条记录";
            bindzhy(
                ViewState["priority"] as string,
                ViewState["orderBy"] as string,
                ViewState["xz"] as string,
                ViewState["key"] as string,
                ViewState["yybm"] as string
            );
        }



        // 修改关键词保存方法
        protected void btnSaveKeywords_Click(object sender, EventArgs e)
        {

            string keywordText = txtKeywordInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(keywordText))
            {
                litKeywordMessage.Text = "<div class='status-message error'>请输入关键词</div>";
                return;
            }

            // 按行分割关键词
            string[] keywords = keywordText.Split(
                new[] { "\r\n", "\n" },
                StringSplitOptions.RemoveEmptyEntries
            );

            if (keywords.Length == 0)
            {
                litKeywordMessage.Text = "<div class='status-message error'>请输入至少一个关键词</div>";
                return;
            }

            int successCount = 0;
            int duplicateCount = 0;
            StringBuilder insertedKeywords = new StringBuilder();

            foreach (string keyword in keywords)
            {
                string cleanKeyword = keyword.Trim();
                if (string.IsNullOrWhiteSpace(cleanKeyword)) continue;

                // 检查关键词是否已存在
                string checkSql = "SELECT COUNT(*) FROM S1688SearchUrl WHERE keywords = '" +
                                 cleanKeyword.Replace("'", "''") + "'";
                int exists = Convert.ToInt32(access_sql.ExecInt2(checkSql));

                if (exists > 0)
                {
                    duplicateCount++;
                    continue;
                }

                // 插入新关键词
                string insertSql = "INSERT INTO S1688SearchUrl (keywords) VALUES ('" +
                                         cleanKeyword.Replace("'", "''") + "' )";

                if (access_sql.ExecSql(insertSql))
                {
                    successCount++;
                    insertedKeywords.Append(cleanKeyword + "<br/>");
                }
            }

            // 显示结果消息
            StringBuilder message = new StringBuilder();
            message.Append("<div class='status-message ");
            message.Append(successCount > 0 ? "success" : "error");
            message.Append("'>");

            message.AppendFormat("成功添加 {0} 个关键词", successCount);

            if (duplicateCount > 0)
            {
                message.AppendFormat("，跳过 {0} 个重复关键词", duplicateCount);
            }

            message.Append("</div>");

            if (successCount > 0)
            {
                message.Append("<div class='status-message info'>添加的关键词：<br/>");
                message.Append(insertedKeywords.ToString());
                message.Append("</div>");
            }

            litKeywordMessage.Text = message.ToString();

            // 清空输入框
            txtKeywordInput.Text = "";
        }

        // 修改过滤词保存方法
        protected void btnSaveFilters_Click(object sender, EventArgs e)
        {
            string filterText = txtFilterInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(filterText))
            {
                litFilterMessage.Text = "<div class='status-message error'>请输入过滤词</div>";
                return;
            }

            // 按行分割过滤词
            string[] filters = filterText.Split(
                new[] { "\r\n", "\n" },
                StringSplitOptions.RemoveEmptyEntries
            );

            if (filters.Length == 0)
            {
                litFilterMessage.Text = "<div class='status-message error'>请输入至少一个过滤词</div>";
                return;
            }

            int successCount = 0;
            int duplicateCount = 0;
            StringBuilder insertedFilters = new StringBuilder();

            foreach (string filter in filters)
            {
                string cleanFilter = filter.Trim();
                if (string.IsNullOrWhiteSpace(cleanFilter)) continue;

                // 检查过滤词是否已存在
                string checkSql = "SELECT COUNT(*) FROM Filter_Words_1688 WHERE FilterWords_1688 = '" +
                                 cleanFilter.Replace("'", "''") + "'";
                int exists = Convert.ToInt32(access_sql.ExecInt2(checkSql));

                if (exists > 0)
                {
                    duplicateCount++;
                    continue;
                }

                // 插入新过滤词
                string insertSql = "INSERT INTO Filter_Words_1688 (FilterWords_1688) VALUES ('" +
                                         cleanFilter.Replace("'", "''") + "')";

                if (access_sql.ExecSql(insertSql))
                {
                    successCount++;
                    insertedFilters.Append(cleanFilter + "<br/>");
                }
            }

            // 显示结果消息
            StringBuilder message = new StringBuilder();
            message.Append("<div class='status-message ");
            message.Append(successCount > 0 ? "success" : "error");
            message.Append("'>");

            message.AppendFormat("成功添加 {0} 个过滤词", successCount);

            if (duplicateCount > 0)
            {
                message.AppendFormat("，跳过 {0} 个重复过滤词", duplicateCount);
            }
            message.Append("</div>");

            if (successCount > 0)
            {
                message.Append("<div class='status-message info'>添加的过滤词：<br/>");
                message.Append(insertedFilters.ToString());
                message.Append("</div>");
            }

            litFilterMessage.Text = message.ToString();

            // 清空输入框
            txtFilterInput.Text = "";
        }

        // 修改取消按钮事件
        // 添加关键词组按钮点击事件
        protected void btnAddKeywordGroup_Click(object sender, EventArgs e)
        {
            pnlKeywordEditor.Visible = true;
            btnAddKeywordGroup.Visible = false;
        }

        // 添加过滤词组按钮点击事件
        protected void btnAddFilterGroup_Click(object sender, EventArgs e)
        {
            pnlFilterEditor.Visible = true;
            btnAddFilterGroup.Visible = false;
        }

        // 关键词取消按钮事件
        protected void btnCancelKeywords_Click(object sender, EventArgs e)
        {
            pnlKeywordEditor.Visible = false;
            btnAddKeywordGroup.Visible = true;
            txtKeywordInput.Text = "";
            litKeywordMessage.Text = "";
        }

        // 过滤词取消按钮事件
        protected void btnCancelFilters_Click(object sender, EventArgs e)
        {
            pnlFilterEditor.Visible = false;
            btnAddFilterGroup.Visible = true;
            txtFilterInput.Text = "";
            litFilterMessage.Text = "";
        }
        protected void btnAllKeyWords_Click(object sender, EventArgs e)
        {
            // 查询关键词
            string sql = "SELECT DISTINCT keywords FROM S1688SearchUrl";
            DataSet ds = access_sql.GreatDs(sql);

            if (access_sql.yzTable(ds))
            {
                rptKeywords.DataSource = ds.Tables[0];
                rptKeywords.DataBind();
                pnlKeywordsPopup.Visible = true;
            }
            else
            {
                litKeywordMessage.Text = "<div class='status-message error'>未找到关键词数据</div>";
            }

            // 确保过滤词弹出层关闭
            pnlFiltersPopup.Visible = false;
        }

        // 修改btnAllFilterWords_Click方法
        protected void btnAllFilterWords_Click(object sender, EventArgs e)
        {
            BindFilterWords();
            pnlFiltersPopup.Visible = true;
        }

        private void BindFilterWords()
        {
            string sql = "SELECT FilterWords_1688 FROM Filter_Words_1688";
            DataSet ds = access_sql.GreatDs(sql);

            if (access_sql.yzTable(ds))
            {
                rptFilters.DataSource = ds.Tables[0];
                rptFilters.DataBind();
            }
            else
            {
                // 处理无数据情况
                rptFilters.DataSource = null;
                rptFilters.DataBind();
            }
        }

        protected void btnCloseKeywords_Click(object sender, EventArgs e)
        {
            pnlKeywordsPopup.Visible = false;
        }

        protected void btnCloseFilters_Click(object sender, EventArgs e)
        {
            pnlFiltersPopup.Visible = false;
        }
        // 在类中添加ItemCommand事件处理方法
        protected void rptFilters_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteFilter")
            {
                string filterWord = e.CommandArgument.ToString();
                DeleteFilterWord(filterWord);
            }
        }
        protected void rptKeywords_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteKeywords")
            {
                string keyword = e.CommandArgument.ToString();
                DeleteKeyword(keyword);
            }
        }
        private void BindKeywords()
        {
            string sql = "SELECT DISTINCT keywords FROM S1688SearchUrl";
            DataSet ds = access_sql.GreatDs(sql);

            DataTable dt = new DataTable();
            dt.Columns.Add("keywords");

            if (access_sql.yzTable(ds))
            {
                dt = ds.Tables[0];
            }

            rptKeywords.DataSource = dt;
            rptKeywords.DataBind();
            pnlKeywordsPopup.Visible = true;

            // 清空消息
            SetKeywordPopupMessage("");
        }
        private void DeleteFilterWord(string filterWord)
        {
            try
            {
                // 安全处理字符串（防止SQL注入）
                string safeFilter = filterWord.Replace("'", "''");
                string deleteSql = "DELETE FROM Filter_Words_1688 WHERE FilterWords_1688 = '" + safeFilter + "'";

                if (access_sql.ExecSql(deleteSql))
                {
                    // 删除成功后重新绑定过滤词列表
                    BindFilterWords();
                    litFilterMessage.Text = "<div class='status-message success'>删除成功!</div>";
                }
                else
                {
                    litFilterMessage.Text = "<div class='status-message error'>删除失败，请重试!</div>";
                }
            }
            catch (Exception ex)
            {
                litFilterMessage.Text = "<div class='status-message error'>删除出错: " + ex.Message + "</div>";
            }
        }
        private void DeleteKeyword(string keyword)
        {
            try
            {
                string safeKeyword = keyword.Replace("'", "''");
                string deleteSql = "DELETE FROM S1688SearchUrl WHERE keywords = '" + safeKeyword + "'";

                if (access_sql.ExecSql(deleteSql))
                {
                    BindKeywords(); // 重新绑定关键词列表
                                    // 在弹出层显示成功消息
                    SetKeywordPopupMessage("<div class='status-message success'>删除成功!</div>");
                }
                else
                {
                    SetKeywordPopupMessage("<div class='status-message error'>删除失败，请重试!</div>");
                }
            }
            catch (Exception ex)
            {
                SetKeywordPopupMessage("<div class='status-message error'>删除出错: " + ex.Message + "</div>");
            }
        }

        private void SetKeywordPopupMessage(string message)
        {
            Literal litPopupMsg = pnlKeywordsPopup.FindControl("litPopupKeywordMessage") as Literal;
            if (litPopupMsg != null)
            {
                litPopupMsg.Text = message;
            }
        }
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
                    bindzhy(
                        ViewState["priority"] as string,
                        ViewState["orderBy"] as string,
                        ViewState["xz"] as string,
                        ViewState["key"] as string,
                        ViewState["yybm"] as string
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