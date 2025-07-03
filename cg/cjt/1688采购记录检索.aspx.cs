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
    public partial class _1688采购记录检索 : System.Web.UI.Page
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
                if (uid != "8" && uid != "9" && uid != "18" && uid != "19" && uid != "12" && uid != "6")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }
        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public void bindzhy(string SJBM = "", string PName = "", string OfferID = "")
        {
            // 保存查询条件到ViewState
            ViewState["SJBM"] = SJBM;
            ViewState["PName"] = PName;
            ViewState["OfferID"] = OfferID;

            string safeSJBM = !string.IsNullOrEmpty(SJBM) ? SJBM.Replace("'", "''") : "";
            string whereCondition = !string.IsNullOrEmpty(safeSJBM)
                ? " AND p.SJBM = '" + safeSJBM + "'"
                : "";


            string pNameCondition = "";
            if (!string.IsNullOrEmpty(PName))
            {
                string safePName = PName.Replace("'", "''");
                pNameCondition = " AND o.HuoPinBiaoTi LIKE '%" + safePName + "%'";
            }

            string offerIDCondition = "";
            if (!string.IsNullOrEmpty(OfferID))
            {
                string safeOfferID = OfferID.Replace("'", "''");
                offerIDCondition = " AND o.Offerid = '" + safeOfferID + "'";
            }

            // 组合所有查询条件
            string fullWhere = whereCondition + pNameCondition + offerIDCondition;

            // 查询总记录数
            string countSql = string.Format(@"
                SELECT COUNT(*) FROM (
                    SELECT o.Offerid, o.Skuid
                    FROM s1688order AS o 
                    LEFT　JOIN Purchase_Sales_Warehouse as p
                    on o.Offerid=p.OfferID_1688
                    WHERE 1=1 {0}
                    GROUP BY o.Offerid, o.Skuid
                ) AS grouped", fullWhere);
            int totalRecords = Convert.ToInt32(access_sql.ExecInt2(countSql));
            TotalPages = (totalRecords + PageSize - 1) / PageSize;

            // 分页查询
            int startRow = (CurrentPage - 1) * PageSize + 1;
            int endRow = CurrentPage * PageSize;
            string sql = string.Format(@"
                SELECT * FROM (
                    SELECT ROW_NUMBER() OVER (ORDER BY o.Offerid, o.Skuid) AS RowNum,
                    MAX(COALESCE(p.sku_img, o.sku_img)) AS sku_img, 
                    MAX(o.HuoPinBiaoTi) as huopinbiaoti, 
                    o.Offerid as Offer_ID, 
                    o.Skuid as SKU_ID
                    FROM s1688order AS o 
                    LEFT　JOIN Purchase_Sales_Warehouse as p
                    on o.Offerid=p.OfferID_1688
                    WHERE 1=1 {0}
                    GROUP BY o.Offerid, o.Skuid
                ) AS Temp
                WHERE RowNum BETWEEN {1} AND {2}", fullWhere, startRow, endRow);

            DataSet ds = access_sql.GreatDs(sql);

            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];
                rplb.DataSource = dt;
                rplb.DataBind();
                // 更新分页显示
                btnPrev.Enabled = CurrentPage > 1;
                btnNext.Enabled = CurrentPage < TotalPages;
                litPageInfo.Text = string.Format("第{0}页/共{1}页", CurrentPage, TotalPages);
                // 新增：同步跳转输入框的值
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









        protected void Button1_Click(object sender, EventArgs e)
        {
            CurrentPage = 1; // 新查询时重置为第一页、
            string sjbm = txtsjbm.Text.Trim();
            if (sjbm == "zyd618")
            {
                sjbm = "cai-8897";
            }
            bindzhy(sjbm, "", "");
        }

        protected void btnSearchCombined_Click(object sender, EventArgs e)
        {
            CurrentPage = 1; // 新查询时重置为第一页
            bindzhy("", txtPName.Text.Trim(), ""); // 新标题查询
        }

        protected void btnSearchOfferID_Click(object sender, EventArgs e)
        {
            CurrentPage = 1; // 新查询时重置为第一页
            bindzhy("", "", txtOfferID.Text.Trim()); // 新OfferID查询
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                bindzhy((string)ViewState["SJBM"], (string)ViewState["PName"], (string)ViewState["OfferID"]);
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                bindzhy((string)ViewState["SJBM"], (string)ViewState["PName"], (string)ViewState["OfferID"]);
            }
        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {


            if (e.CommandName == "qr")
            {
                lits.Text = "";
                ulong rucangSKUID = Convert.ToUInt64(e.CommandArgument);
                Literal rucangItemID = e.Item.FindControl("rucangItemID") as Literal;

                string ItemID = rucangItemID.Text;
                TextBox txtY_1688OfferID = e.Item.FindControl("txtY_1688OfferID") as TextBox;
                TextBox txtY_1688SKUID = e.Item.FindControl("txtY_1688SKUID") as TextBox;




                if (access_sql.T_Update_ExecSql(new string[] { "OfferID_1688", "SkuID_1688" }, new object[] { txtY_1688OfferID.Text.Trim().Replace("'", "''"), txtY_1688SKUID.Text.Trim().Replace("'", "''") }, "Purchase_Sales_Warehouse", "rucangSKUID='" + rucangSKUID + "' AND　rucangItemID='" + ItemID + "' ") > 0)
                {
                    bindzhy((string)ViewState["SJBM"], (string)ViewState["PName"], (string)ViewState["OfferID"]);
                    lits.Text = "rucangSKUID:" + rucangSKUID + "更新成功";
                }

            }
        }
        protected void btnJump_Click(object sender, EventArgs e)
        {
            int pageNumber;
            if (int.TryParse(txtJumpPage.Text.Trim(), out pageNumber))
            {
                // 验证页码范围
                if (pageNumber >= 1 && pageNumber <= TotalPages)
                {
                    CurrentPage = pageNumber;
                    // 刷新数据（保留搜索条件）
                    bindzhy(
                        ViewState["SJBM"] as string,
                        ViewState["PName"] as string,
                        ViewState["OfferID"] as string
                    );
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