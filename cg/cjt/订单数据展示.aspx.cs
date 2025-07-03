using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg.cjt
{
    public partial class 订单数据展示 : System.Web.UI.Page
    {
        private int PageSize = 50; // 每页50条
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
        public int CurrentPage
        {
            get { return ViewState["CurrentPage"] == null ? 1 : (int)ViewState["CurrentPage"]; }
            set { ViewState["CurrentPage"] = value; }
        }
        public int TotalPages
        {
            get { return ViewState["TotalPages"] == null ? 1 : (int)ViewState["TotalPages"]; }
            set { ViewState["TotalPages"] = value; }
        }

        // 修改现有的bindzhy方法
        public void bindzhy(String SJBM = "")
        {
            string safeSJBM = !string.IsNullOrEmpty(SJBM) ? SJBM.Replace("'", "''") : "";
            string whereCondition = !string.IsNullOrEmpty(safeSJBM)
                ? " AND o.SJBM = '" + safeSJBM + "'"
                : "";

            string sql = @"
                WITH ItemCount AS ( 
                    SELECT 
                        ItemID, 
                        COUNT(DISTINCT buyer_id) AS item_amount 
                    FROM 
                        SuMaiTongPol.dbo.ShopeeOrder 
                    GROUP BY 
                        ItemID 
                ),
                OrderedData AS (
                    SELECT 
                        o.*,
                        s.item_amount,
                        ROW_NUMBER() OVER (
                            ORDER BY 
                                SUBSTRING(o.order_sn, 1, 6) DESC,  
                                o.BName ASC,                     
                                s.item_amount DESC
                        ) AS RowNum
                    FROM SuMaiTongPol.dbo.ShopeeOrder o
                    INNER JOIN ItemCount s ON o.ItemID = s.ItemID
                    WHERE 1=1 " + whereCondition + @"
                )
                SELECT * 
                FROM OrderedData
                WHERE RowNum BETWEEN " + ((CurrentPage - 1) * PageSize + 1) + " AND " + (CurrentPage * PageSize);
            DataSet ds = access_sql.GreatDs(sql);
            string sql1 = "SELECT COUNT(*) FROM SuMaiTongPol.dbo.ShopeeOrder o WHERE 1=1 " + whereCondition;
            int totalCount = (int)access_sql.ExecInt(sql1);

            TotalPages = (int)Math.Ceiling((double)totalCount / PageSize);

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
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            string sjbm = txtsjbm.Text.Trim();
            ViewState["SearchType"] = "SJBM";
            ViewState["SearchValue"] = sjbm;
            bindzhy(sjbm);
        }

        // 新增店铺查询方法
        protected void btnSearchBName_Click(object sender, EventArgs e)
        {
            string bname = txtBName.Text.Trim();
            ViewState["SearchType"] = "BName";
            ViewState["SearchValue"] = bname;
            bindByBName(bname);
        }

        // 新增产品标题查询方法
        protected void btnSearchPName_Click(object sender, EventArgs e)
        {
            string pname = txtPName.Text.Trim();
            ViewState["SearchType"] = "PName";
            ViewState["SearchValue"] = pname;
            bindByPName(pname);
        }

        // 合并查询事件处理
        protected void btnSearchCombined_Click(object sender, EventArgs e)
        {
            string bname = txtBName.Text.Trim();
            string pname = txtPName.Text.Trim();
            ViewState["SearchType"] = "Combined";
            ViewState["SearchValue"] = "{bname}|{pname}";
            bindCombinedSearch(bname, pname);
        }

        // 合并查询绑定方法
        public void bindCombinedSearch(string BName = "", string PName = "")
        {
            string safeBName = !string.IsNullOrEmpty(BName) ? BName.Replace("'", "''") : "";
            string safePName = !string.IsNullOrEmpty(PName) ? PName.Replace("'", "''") : "";
            string whereCondition = "";
            if (!string.IsNullOrEmpty(safeBName)) whereCondition += " AND o.BName = '" + safeBName + "'";
            if (!string.IsNullOrEmpty(safePName)) whereCondition += " AND o.pname LIKE '%" + safePName + "%'";

            string sql = @"
                WITH ItemCount AS (
                    SELECT 
                        ItemID, 
                        COUNT(DISTINCT buyer_id) AS item_amount 
                    FROM 
                        SuMaiTongPol.dbo.ShopeeOrder 
                    GROUP BY 
                        ItemID 
                ),
                OrderedData AS (
                    SELECT 
                        o.*,
                        s.item_amount,
                        ROW_NUMBER() OVER (
                            ORDER BY 
                                SUBSTRING(o.order_sn, 1, 6) DESC,  
                                o.BName ASC,                      
                                s.item_amount DESC
                        ) AS RowNum
                    FROM SuMaiTongPol.dbo.ShopeeOrder o
                    INNER JOIN ItemCount s ON o.ItemID = s.ItemID
                    WHERE 1=1 " + whereCondition + @"
                )
                SELECT * 
                FROM OrderedData
                WHERE RowNum BETWEEN " + ((CurrentPage - 1) * PageSize + 1) + " AND " + (CurrentPage * PageSize);
            DataSet ds = access_sql.GreatDs(sql);
            string sql1 = "SELECT COUNT(*) FROM SuMaiTongPol.dbo.ShopeeOrder o WHERE 1=1 " + whereCondition;
            int totalCount = (int)access_sql.ExecInt(sql1);

            TotalPages = (int)Math.Ceiling((double)totalCount / PageSize);

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
        }


        public void bindByBName(string BName = "")
        {
            string safeBName = !string.IsNullOrEmpty(BName) ? BName.Replace("'", "''") : "";
            string whereCondition = !string.IsNullOrEmpty(safeBName)
                ? " AND o.BName = '" + safeBName + "'"
                : "";


            string sql = @"
            WITH OrderedData AS (
                SELECT 
                    *,
                     s.item_amount,
                    ROW_NUMBER() OVER (
                        ORDER BY 
                            SUBSTRING(o.order_sn, 1, 6) DESC,  
                            o.BName ASC,                      
                            s.item_amount DESC
                    ) AS RowNum
                FROM SuMaiTongPol.dbo.ShopeeOrder o
                INNER JOIN (
                    SELECT 
                        ItemID, 
                        COUNT(DISTINCT buyer_id) AS item_amount 
                    FROM SuMaiTongPol.dbo.ShopeeOrder 
                    GROUP BY ItemID
                ) s ON o.ItemID = s.ItemID
                WHERE 1=1 " + whereCondition + @"
            )
            SELECT * 
            FROM OrderedData
            WHERE RowNum BETWEEN " + ((CurrentPage - 1) * PageSize + 1) + " AND " + (CurrentPage * PageSize);
            DataSet ds = access_sql.GreatDs(sql);
            string sql1 = @"
            SELECT COUNT(*) 
            FROM SuMaiTongPol.dbo.ShopeeOrder o
            INNER JOIN (
                SELECT 
                    ItemID, 
                    COUNT(DISTINCT buyer_id) AS item_amount 
                FROM SuMaiTongPol.dbo.ShopeeOrder 
                GROUP BY ItemID
            ) s ON o.ItemID = s.ItemID
            WHERE 1=1 " + whereCondition;
            int totalCount = (int)access_sql.ExecInt(sql1);

            TotalPages = (int)Math.Ceiling((double)totalCount / PageSize);

            string sql2 = @"
                                WITH ItemCount AS ( 
                SELECT 
                    ItemID, 
                    COUNT(DISTINCT buyer_id) AS item_amount 
                FROM 
                    SuMaiTongPol.dbo.ShopeeOrder 
                GROUP BY 
                    ItemID 
            ) 
            SELECT 
                o.BName, 
                o.ItemID, 
                s.item_amount  ,  
                SUBSTRING(o.order_sn, 1, 6) AS order_date,  
                o.shopid, 
                o.order_id, 
                o.skuid, 
                o.purl, 
                o.pname, 
                o.total_price, 
                o.status, 
                o.GroupName, 
                o.SJBM, 
                o.pimage 
            FROM SuMaiTongPol.dbo.ShopeeOrder o 
            INNER JOIN ItemCount s ON o.ItemID = s.ItemID 
            WHERE 1=1" + whereCondition + @"
            GROUP BY 
                o.BName, 
                o.ItemID, 
                SUBSTRING(o.order_sn, 1, 6),  
                s.item_amount  ,
                o.shopid, 
                o.order_id, 
                o.skuid, 
                o.purl, 
                o.pname, 
                o.total_price, 
                o.status, 
                o.GroupName, 
                o.SJBM, 
                o.pimage 
            ORDER BY 
                SUBSTRING(o.order_sn, 1, 6) DESC,  
                o.BName ASC,                      
                s.item_amount DESC";

            DataSet ds1 = access_sql.GreatDs(sql2);


            if (access_sql.yzTable(ds1))
            {
                DataTable dt = ds1.Tables[0];
                rplb.DataSource = dt;
                rplb.DataBind();

            }
            else
            {
                rplb.DataSource = null;
                rplb.DataBind();
                lits.Text = "无数据";
            }
        }

        // 新增产品标题绑定方法
        public void bindByPName(string PName = "")
        {
            string safePName = !string.IsNullOrEmpty(PName) ? PName.Replace("'", "''") : "";
            string whereCondition = !string.IsNullOrEmpty(safePName)
                ? " AND o.pname LIKE '%" + safePName + "%'"
                : "";

            string sql = @"
            WITH OrderedData AS (
                SELECT 
                    *,
                    s.item_amount,
                    ROW_NUMBER() OVER (
                        ORDER BY 
                            SUBSTRING(o.order_sn, 1, 6) DESC,  
                            o.BName ASC,                      
                            s.item_amount DESC
                    ) AS RowNum
                FROM SuMaiTongPol.dbo.ShopeeOrder o
                INNER JOIN (
                    SELECT 
                        ItemID, 
                        COUNT(DISTINCT buyer_id) AS item_amount 
                    FROM SuMaiTongPol.dbo.ShopeeOrder 
                    GROUP BY ItemID
                ) s ON o.ItemID = s.ItemID
                WHERE 1=1 " + whereCondition + @"
            )
            SELECT * 
            FROM OrderedData
            WHERE RowNum BETWEEN " + ((CurrentPage - 1) * PageSize + 1) + " AND " + (CurrentPage * PageSize);
            DataSet ds = access_sql.GreatDs(sql);
            string sql1 = @"
            SELECT COUNT(*) 
            FROM SuMaiTongPol.dbo.ShopeeOrder o
            INNER JOIN (
                SELECT 
                    ItemID, 
                    COUNT(DISTINCT buyer_id) AS item_amount 
                FROM SuMaiTongPol.dbo.ShopeeOrder 
                GROUP BY ItemID
            ) s ON o.ItemID = s.ItemID
            WHERE 1=1 " + whereCondition;
            int totalCount = (int)access_sql.ExecInt(sql1);

            TotalPages = (int)Math.Ceiling((double)totalCount / PageSize);

            string sql2 = @"
                WITH ItemCount AS ( 
                    SELECT 
                        ItemID, 
                        COUNT(DISTINCT buyer_id) AS item_amount 
                    FROM 
                        SuMaiTongPol.dbo.ShopeeOrder 
                    GROUP BY 
                        ItemID 
                ) 
                SELECT 
                    o.BName, 
                    o.ItemID, 
                    s.item_amount  ,  
                    SUBSTRING(o.order_sn, 1, 6) AS order_date,  
                    o.shopid, 
                    o.order_id, 
                    o.skuid, 
                    o.purl, 
                    o.pname, 
                    o.total_price, 
                    o.status, 
                    o.GroupName, 
                    o.SJBM, 
                    o.pimage 
                FROM SuMaiTongPol.dbo.ShopeeOrder o 
                INNER JOIN ItemCount s ON o.ItemID = s.ItemID 
                WHERE 1=1" + whereCondition + @"
                GROUP BY 
                    o.BName, 
                    o.ItemID, 
                    SUBSTRING(o.order_sn, 1, 6),  
                    s.item_amount  ,
                    o.shopid, 
                    o.order_id, 
                    o.skuid, 
                    o.purl, 
                    o.pname, 
                    o.total_price, 
                    o.status, 
                    o.GroupName, 
                    o.SJBM, 
                    o.pimage 
                ORDER BY 
                    SUBSTRING(o.order_sn, 1, 6) DESC,  
                    o.BName ASC,                      
                    s.item_amount DESC";

            DataSet ds1 = access_sql.GreatDs(sql2);

            if (access_sql.yzTable(ds1))
            {
                DataTable dt = ds1.Tables[0];
                rplb.DataSource = dt;
                rplb.DataBind();
            }
            else
            {
                rplb.DataSource = null;
                rplb.DataBind();
                lits.Text = "无数据";
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {


            if (e.CommandName == "qr")
            {
                lits.Text = "";
                int id = Convert.ToInt32(e.CommandArgument);
                TextBox txtY_1688url = e.Item.FindControl("txtY_1688url") as TextBox;
                TextBox txtY_1688sku1 = e.Item.FindControl("txtY_1688sku1") as TextBox;
                TextBox txtY_1688sku2 = e.Item.FindControl("txtY_1688sku2") as TextBox;
                TextBox txtY_1688sku3 = e.Item.FindControl("txtY_1688sku3") as TextBox;
                TextBox txtY_1688price = e.Item.FindControl("txtY_1688price") as TextBox;
                CheckBox cbShifou = e.Item.FindControl("cbShifou") as CheckBox;



                if (access_sql.T_Update_ExecSql(new string[] { "Y_1688url", "Y_1688sku1", "Y_1688sku2", "Y_1688sku3", "Y_1688price", "shi_fou" }, new object[] { txtY_1688url.Text.Trim().Replace("'", "''"), txtY_1688sku1.Text.Trim().Replace("'", "''"), txtY_1688sku2.Text.Trim().Replace("'", "''"), txtY_1688sku3.Text.Trim().Replace("'", "''"), txtY_1688price.Text.Trim().Replace("'", "''"), cbShifou.Checked ? 1 : 0 }, "ShopeeOrder", "id=" + id + "") > 0)
                {
                    bindzhy();
                    lits.Text = "ID:" + id + "更新成功";
                }

            }
        }
        public void clzy()
        {
            int cg = 0;


            for (int i = 0; i < rplb.Items.Count; i++)
            {
                Literal id = (Literal)rplb.Items[i].FindControl("id");
                TextBox txtY_1688url = rplb.Items[i].FindControl("txtY_1688url") as TextBox;
                TextBox txtY_1688sku1 = rplb.Items[i].FindControl("txtY_1688sku1") as TextBox;
                TextBox txtY_1688sku2 = rplb.Items[i].FindControl("txtY_1688sku2") as TextBox;
                TextBox txtY_1688sku3 = rplb.Items[i].FindControl("txtY_1688sku3") as TextBox;
                TextBox txtY_1688price = rplb.Items[i].FindControl("txtY_1688price") as TextBox;
                CheckBox cbShifou = rplb.Items[i].FindControl("cbShifou") as CheckBox;
                cg += access_sql.T_Update_ExecSql(new string[] { "Y_1688url", "Y_1688sku1", "Y_1688sku2", "Y_1688sku3", "Y_1688price", "shi_fou" }, new object[] { txtY_1688url.Text.Trim().Replace("'", "''"), txtY_1688sku1.Text.Trim().Replace("'", "''"), txtY_1688sku2.Text.Trim().Replace("'", "''"), txtY_1688sku3.Text.Trim().Replace("'", "''"), txtY_1688price.Text.Trim().Replace("'", "''"), cbShifou.Checked ? 1 : 0 }, "ShopeeOrder", "id=" + id + "");
            }
            bindzhy();
            lits.Text = "更新成功" + cg + "个";

        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            clzy();
        }

        // 添加分页按钮事件
        protected void Page_Changed(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.CommandName)
            {
                case "Prev":
                    if (CurrentPage > 1) CurrentPage--;
                    break;
                case "Next":
                    if (CurrentPage < TotalPages) CurrentPage++;
                    break;
            }
            // 更新跳转输入框的值
            txtJumpPage.Text = CurrentPage.ToString();

            ReloadData();


        }
        protected void BtnJump_Click(object sender, EventArgs e)
        {
            int pageNumber;
            if (int.TryParse(txtJumpPage.Text.Trim(), out pageNumber))
            {
                // 验证页码范围
                if (pageNumber >= 1 && pageNumber <= TotalPages)
                {
                    CurrentPage = pageNumber;
                    ReloadData();
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

        private void ReloadData()
        {
            // 获取存储的查询参数
            string searchType = ViewState["SearchType"] as string;
            string searchValue = ViewState["SearchValue"] as string;

            if (!string.IsNullOrEmpty(searchType) && !string.IsNullOrEmpty(searchValue))
            {
                if (searchType == "SJBM")
                    bindzhy(searchValue);
                else if (searchType == "BName")
                    bindByBName(searchValue);
                else if (searchType == "PName")
                    bindByPName(searchValue);
                else if (searchType == "Combined")
                {
                    string[] values = searchValue.Split('|');
                    if (values.Length == 2)
                        bindCombinedSearch(values[0], values[1]);
                }
            }
            else
            {
                bindzhy();
            }
        }
    }
}