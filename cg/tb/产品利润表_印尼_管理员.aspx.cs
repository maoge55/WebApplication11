using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg.tb
{
    public partial class 产品利润表_印尼_管理员 : System.Web.UI.Page
    {
        public int CurrentPage
        {
            get { return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1; }
            set { ViewState["CurrentPage"] = value; }
        }

        public int PageSize = 50;
        public int TotalPages
        {
            get { return ViewState["TotalPages"] != null ? (int)ViewState["TotalPages"] : 0; }
            set { ViewState["TotalPages"] = value; }
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
                if (uid != "8" && uid != "9" && uid != "18" && uid != "19" && uid != "12" && uid != "6")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }
            if (!IsPostBack)
            {
                BindData();
            }
        }
        
        public string u = "";
        public string p = "";
        public string uid = "";

        private void BindData()
        {
            string hwcCode = txtHWCCode.Text.Trim();
            string bName = txtBName.Text.Trim();
            string sortExpression = ddlSort.SelectedValue;

            string whereCondition = "1=1";
            if (!string.IsNullOrEmpty(hwcCode))
            {
                whereCondition += " AND haiwaicangxitongbianma = '" + hwcCode.Replace("'", "''") + "'";
            }
            if (!string.IsNullOrEmpty(bName))
            {
                whereCondition += " AND BName = '" + bName.Replace("'", "''") + "'";
            }

            // 查询总记录数
            string countSql = $"SELECT COUNT(*) FROM ProductProfit WHERE {whereCondition}";
            int totalRecords = Convert.ToInt32(access_sql.ExecInt2(countSql));
            TotalPages = (totalRecords + PageSize - 1) / PageSize;

            // 分页查询
            int startRow = (CurrentPage - 1) * PageSize + 1;
            int endRow = CurrentPage * PageSize;

            string sql = $@"
                SELECT * FROM (
                    SELECT ROW_NUMBER() OVER (ORDER BY {sortExpression}) AS RowNum,
                           p.OfferID_1688, p.SkuID_1688, p.s1688url, p.huopinbiaoti, p.danjia,
                           p.baozhuanghe1688jiage1, p.logistics_pt_to_gd, p.guangdong_warehouse_cost,
                           p.yunyingbianma, p.haiwaicangxitongbianma, p.BName, p.rucangItemID,
                           p.rucangSKUID, p.dingdanshuliang_itemid_id, p.dingdanshuliang_id, p.conversions, p.acos_3months_id,
                           p.roars, p.sale_price_id, p.tuihuoshuliang_id, p.tuihuolv_id,
                           p.total_cost, p.maolirun_id, p.maolirunlv_id, p.update_time, p.upload_time,
                           w.sku_img
                    FROM ProductProfit p
                    LEFT JOIN (
                        SELECT rucangSKUID, MAX(sku_img) as sku_img
                        FROM Purchase_Sales_Warehouse 
                        GROUP BY rucangSKUID
                    ) w ON p.rucangSKUID = w.rucangSKUID
                    WHERE {whereCondition}
                ) AS Temp
                WHERE RowNum BETWEEN {startRow} AND {endRow}";

            DataSet ds = access_sql.GreatDs(sql);

            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];
                rplb.DataSource = dt;
                rplb.DataBind();

                btnPrev.Enabled = CurrentPage > 1;
                btnNext.Enabled = CurrentPage < TotalPages;
                litPageInfo.Text = string.Format("第{0}页/共{1}页", CurrentPage, TotalPages);
                txtJumpPage.Text = CurrentPage.ToString();
            }
            else
            {
                rplb.DataSource = null;
                rplb.DataBind();
                lits.Text = "无数据";
                btnPrev.Enabled = false;
                btnNext.Enabled = false;
                litPageInfo.Text = "";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            BindData();
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                BindData();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                BindData();
            }
        }

        protected void btnJump_Click(object sender, EventArgs e)
        {
            int pageNumber;
            if (int.TryParse(txtJumpPage.Text.Trim(), out pageNumber))
            {
                if (pageNumber >= 1 && pageNumber <= TotalPages)
                {
                    CurrentPage = pageNumber;
                    BindData();
                }
                else
                {
                    lits.Text = "页码必须在1到" + TotalPages + "之间";
                }
            }
            else
            {
                lits.Text = "请输入有效的数字页码";
            }
        }
    }
} 