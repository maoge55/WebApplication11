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

namespace WebApplication11.cg.cjt
{
    public partial class 采集数据_1688_运营 : System.Web.UI.Page
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
        public void bindzhy(String YYBM = "", string status = "", string keywords = "")
        {
            ViewState["YYBM"] = YYBM;
            ViewState["status"] = status;
            ViewState["keywords"] = keywords;

            string safeYYBM = !string.IsNullOrEmpty(YYBM) ? YYBM.Replace("'", "''") : "";
            string safeStatus = status.Replace("'", "''");
            string safekeywords = !string.IsNullOrEmpty(keywords) ? keywords.Replace("'", "''") : "";
            //运营编码选择
            string whereCondition = "";
            if (!string.IsNullOrEmpty(safeYYBM))
                whereCondition += " AND ss.YYBM = '" + safeYYBM + "'";
            //数据状态筛选
            if (!string.IsNullOrEmpty(status) && status != "-1")
                whereCondition += " AND sp.shujuzhuangtai = '" + safeStatus + "'";
            //标题模糊查找
            if (!string.IsNullOrEmpty(safekeywords))
            {
                // 1. 白名单过滤：仅保留中文、字母、数字、空格
                string safeInput = Regex.Replace(safekeywords, @"[^\u4e00-\u9fa5a-zA-Z0-9\s]", "");

                // 2. 拆分关键词并移除空项
                string[] parts = safeInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // 3. 构建条件（传统字符串拼接）
                var conditions = new List<string>();
                foreach (string part in parts)
                {
                    if (part.Length > 0)
                    {
                        // 关键步骤：双重转义单引号（防御注入核心）
                        string sanitizedPart = part.Replace("'", "''");

                        // 改用传统拼接（无$符号）
                        conditions.Add("CHARINDEX('" + sanitizedPart + "', sp.pname) > 0");
                    }
                }

                // 4. 组合条件
                if (conditions.Count > 0)
                {
                    whereCondition += " AND " + string.Join(" AND ", conditions);
                }
            }
            // 获取总记录数


            string countSql = string.Format(@"
        SELECT COUNT(DISTINCT sp.itemid)  
        FROM S1688SearchUrl ss
        INNER JOIN S1688Pro sp ON ss.id = sp.from_sid
        INNER JOIN Filter_Words_1688 fw ON ss.keywords != fw.FilterWords_1688
        WHERE 1=1 {0}", whereCondition);


            // 获取总记录数
            int totalRecords = Convert.ToInt32(access_sql.ExecInt2(countSql));
            TotalPagesCount = (totalRecords + PageSize - 1) / PageSize;

            // 分页参数
            int startRow = (CurrentPageIndex - 1) * PageSize + 1;
            int endRow = CurrentPageIndex * PageSize;

            // 核心修改：根据关键词是否为空决定是否限制每个关键词的记录数




            string sql = @"
WITH RankedKeywords AS (
    SELECT 
        ss.keywords,
        sp.itemid,
        sp.url,
        sp.image,
        sp.pname,
        ss.YYBM,
        sp.price,
        sp.historical_sold,
        sp.shujuzhuangtai,
        ROW_NUMBER() OVER (
            PARTITION BY ss.keywords 
            ORDER BY sp.itemid DESC 
        ) AS GroupRank
    FROM S1688SearchUrl ss
    INNER JOIN S1688Pro sp ON ss.id = sp.from_sid
    WHERE NOT EXISTS (
        SELECT 1 FROM Filter_Words_1688 fw 
        WHERE sp.pname LIKE '%' + fw.FilterWords_1688 + '%'
    )
    AND 1=1 " + whereCondition + @"
),
SegmentedKeywords AS (
    SELECT 
        keywords,
        itemid,
        url,
        image,
        pname,
        YYBM,
        price,
        historical_sold,
        shujuzhuangtai,
        GroupRank,
        DENSE_RANK() OVER (ORDER BY keywords) AS KeywordSeq,
    
        CEILING(CAST(GroupRank AS FLOAT) / 10) AS Segment
    FROM RankedKeywords
),
GlobalItems AS (
    SELECT 
        *,
        ROW_NUMBER() OVER (
           
            ORDER BY Segment, KeywordSeq, GroupRank 
        ) AS GlobalRowNum
    FROM SegmentedKeywords
)
SELECT 
    keywords AS [1688采集关键词],
    url AS [1688产品链接],
    image AS [图片],
    itemid AS [OfferID],
    pname AS [标题],
    YYBM AS [运营编码],
    price AS [单价],
    historical_sold AS [销量],
    shujuzhuangtai AS [数据状态]
FROM GlobalItems
WHERE GlobalRowNum BETWEEN " + startRow + " AND " + endRow + @"
ORDER BY GlobalRowNum;";




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
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            string yybm = txtsjbm.Text.Trim();
            string status = ddlStatus.SelectedValue;
            string keywords = ddlkeywords.Text.Trim();

            bindzhy(yybm, status, keywords);

        }
        // 上一页按钮
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex > 1)
            {
                CurrentPageIndex--;
                bindzhy(
                    ViewState["YYBM"] as string,

                    ViewState["status"] as string,
                    ViewState["keywords"] as string

                );
            }
        }
        // 下一页按钮
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex < TotalPagesCount)
            {
                CurrentPageIndex++;
                bindzhy(
                    ViewState["YYBM"] as string,

                    ViewState["status"] as string,
                    ViewState["keywords"] as string

                );
            }
        }
        // 查找按钮，接收运营编码，状态，关键词
        protected void Button1_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            string yybm = txtsjbm.Text.Trim();

            string status = ddlStatus.SelectedValue;
            string keywords = ddlkeywords.Text.Trim();

            // 统一调用方式，明确传递三个参数
            bindzhy(YYBM: yybm, status: status, keywords: keywords);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "qr")
            {
                lits.Text = "";
                // 前端传的OfferID
                ulong OfferID = Convert.ToUInt64(e.CommandArgument);
                // 数据状态
                DropDownList shujuzhuangtai = e.Item.FindControl("shujuzhuangtai") as DropDownList;
                // 更新语句
                if (access_sql.T_Update_ExecSql(new string[] { "shujuzhuangtai" },
                  new object[] { shujuzhuangtai.SelectedValue },
                  "S1688Pro",
                 "itemid='" + OfferID + "'") > 0)
                {


                    bindzhy(ViewState["YYBM"] as string,
                        ViewState["status"] as string, ViewState["keywords"] as string);
                    lits.Text = "itemid:" + OfferID + "更新成功";
                }
            }
            else if (e.CommandName == "ApplyBatch")
            {
                // 批量应用状态
                ApplyBatchUpdate();
            }


        }

        // 批量保存
        public void clzy()
        {
            int cg = 0;


            for (int i = 0; i < rplb.Items.Count; i++)
            {
                HiddenField OfferID = rplb.Items[i].FindControl("OfferID") as HiddenField;
                // ItemID
                ulong itemId = ulong.Parse(OfferID.Value);
                // 数据状态
                DropDownList shujuzhuangtai = rplb.Items[i].FindControl("shujuzhuangtai") as DropDownList;
                string zt = shujuzhuangtai.SelectedValue;

                cg += access_sql.T_Update_ExecSql(new string[] { "shujuzhuangtai" },
                  new object[] { shujuzhuangtai.SelectedValue },
                  "S1688Pro",
                 "itemid='" + itemId + "'");


            }
            bindzhy(ViewState["YYBM"] as string,
                        ViewState["status"] as string, ViewState["keywords"] as string);
            lits.Text = "更新成功" + cg + "个";

        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            clzy();
        }

        private void ApplyBatchUpdate()
        {

            DropDownList ddlBatchsj = rplb.Controls[0].FindControl("ddlBatchsj") as DropDownList;

            if (ddlBatchsj == null || string.IsNullOrEmpty(ddlBatchsj.SelectedValue))
            {
                lits.Text = "请选择要应用的数据状态";
                return;
            }

            bool anyChecked = false;
            int updatedCount = 0;
            string safeddlBatchsj = ddlBatchsj.SelectedValue;

            // 遍历所有行，只处理选中的行
            foreach (RepeaterItem item in rplb.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox chk = item.FindControl("chkItem") as CheckBox;
                    HiddenField OfferID = item.FindControl("OfferID") as HiddenField;




                    if (chk != null && chk.Checked && OfferID != null)
                    {
                        anyChecked = true;
                        ulong itemId = ulong.Parse(OfferID.Value);

                        string condition = "itemid='" + itemId + "'";

                        if (access_sql.T_Update_ExecSql(
                            new string[] { "shujuzhuangtai", },
                            new object[] { safeddlBatchsj, },
                            "S1688Pro",
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

            lits.Text = "成功更新了 " + updatedCount + " 条记录的运营编码";

            // 刷新数据（需保留分页状态）
            bindzhy(
                ViewState["YYBM"] as string,

                ViewState["status"] as string

            );
        }
        protected void btnPopupSave_Click(object sender, EventArgs e)
        {
            try
            {
                string keywords = txtKeywords.Text.Trim();
                string yybm = txtPopupYYBM.Text.Trim();
                string bid = txtbid.Text.Trim();
                int quantity = Convert.ToInt32(txtQuantity.Text);
                int fileCount = Convert.ToInt32(txtFileCount.Text);
                string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string pt = "1688";
                string insertSql = "INSERT INTO TaskDC (kws, YYBM, per_count, file_count,PingTai,bid,start_time) VALUES ('" +
                          keywords.Replace("'", "''") + "', '" +   // 转义keywords并添加逗号
                          yybm.Replace("'", "''") + "', " +      // 转义yybm并添加逗号
                          quantity + ", " +                      // 整数值不加引号
                          fileCount + ", '" +
                          pt + "', '" +
                          bid + "', '" +
                          currentTime + "')";

                if (access_sql.ExecSql(insertSql))
                {
                    lits.Text = "任务添加成功！";
                    // 清空输入框
                    txtKeywords.Text = "";
                    txtPopupYYBM.Text = "";
                    txtQuantity.Text = "";
                    txtFileCount.Text = "";
                    txtbid.Text = "";
                }
                else
                {
                    lits.Text = "任务添加失败，请重试";
                }
            }
            catch (Exception ex)
            {
                lits.Text = "发生错误: " + ex.Message;
            }
        }

        protected string GetStatusText(object statusObj)
        {
            int status = 0;
            if (statusObj != null && int.TryParse(statusObj.ToString(), out status))
            {
                switch (status)
                {
                    case 0:
                        return "任务未启动";
                    case 1:
                        return "任务正在进行";
                    default:
                        return "已生成下载链接";
                }
            }
            return "未知状态";
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
                    bindzhy(
                        ViewState["YYBM"] as string,
                        ViewState["status"] as string,
                        ViewState["keywords"] as string
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

        public class DownloadInfo
        {
            public string yybm { get; set; }
            public string download_url { get; set; }
            public int status { get; set; }
            public string start_time { get; set; }  // 添加
            public string end_time { get; set; }    // 添加
        }


        [WebMethod]
        public static List<DownloadInfo> GetDownloadList(string yybm)
        {
            string safeYybm = yybm.Replace("'", "''");
            string sql = $"SELECT yybm, download_url, status,start_time,end_time FROM TaskDC WHERE YYBM='{safeYybm}' ORDER BY start_time DESC";

            DataSet ds = access_sql.GreatDs(sql);
            List<DownloadInfo> list = new List<DownloadInfo>();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(new DownloadInfo
                    {
                        yybm = row["yybm"].ToString(),
                        download_url = row["download_url"].ToString(),
                        status = Convert.ToInt32(row["status"]),
                        start_time = row["start_time"].ToString(),
                        end_time = row["end_time"].ToString()
                    });
                }
            }

            return list;
        }
    }
}