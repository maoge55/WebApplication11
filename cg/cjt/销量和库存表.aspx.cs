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
    public partial class 销量和库存表 : System.Web.UI.Page
    {
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
                if ( uid != "9")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public void bindzhy(String SJBM = "",string BName="",string status="")
        {

            string safeSJBM = !string.IsNullOrEmpty(SJBM) ? SJBM.Replace("'", "''") : "";
   
            string safeBName = !string.IsNullOrEmpty(BName) ? BName.Replace("'", "''") : "";
            string safeStatus = status.Replace("'", "''");
            string whereCondition = "";

            if (!string.IsNullOrEmpty(safeSJBM))
                whereCondition += " AND so.SJBM = '" + safeSJBM + "'";

            if (!string.IsNullOrEmpty(safeBName))
                whereCondition += " AND so.BName LIKE '%" + safeBName + "%'";
            if (!string.IsNullOrEmpty(status) && status != "-1")

                whereCondition += " AND so.xiaoshouzhuangtai = '" + safeStatus + "'";
            string sql = @"
WITH Prepared1688 AS (
    SELECT 
        c.SKU_ID AS bridge_skuid,
        sp.sku1,
        sp.sku2,
        sp.sku_price
    FROM caiwu c
    INNER JOIN S1688ProSKU sp ON c.SKU_ID = sp.skuid
    WHERE c.SKU_ID IS NOT NULL
),
SalesAgg AS (
    SELECT 
        skuid,
        COUNT(DISTINCT CASE 
            WHEN order_date >= DATEADD(DAY, -7, GETDATE()) THEN buyer_id 
        END) AS sales_7d,
        COUNT(DISTINCT CASE 
            WHEN order_date >= DATEADD(DAY, -14, GETDATE()) THEN buyer_id 
        END) AS sales_14d,
        COUNT(DISTINCT CASE 
            WHEN order_date >= DATEADD(DAY, -30, GETDATE()) THEN buyer_id 
        END) AS sales_30d
    FROM ShopeeOrder
    WHERE status != 'Cancelled'
    GROUP BY skuid
),
InTransitData AS (
    SELECT 
        cw.SKU_ID,
        SUM(tc.putianfachushuliang) AS fachu_total,
        SUM(tc.putianweifachushuliang) AS weifachu_total,
        SUM(tc.putianfachushuliang + tc.putianweifachushuliang) AS zongzaitu
    FROM touchenwuliu tc
    INNER JOIN Purchase_Sales_Warehouse psw 
        ON tc.haiwaicangrukudanhao = psw.haiwaicangxitongbianma
    INNER JOIN caiwu cw 
        ON cw.rucangITEMID = psw.rucangItemID
    GROUP BY cw.SKU_ID
),
ItemMaintenance AS (
    SELECT 
        ItemID,
        MAX(TRY_CAST(ItemIDxuweichi AS DECIMAL(18, 2))) AS required_stock,
        COUNT(DISTINCT CASE 
            WHEN order_date >= DATEADD(DAY, -14, GETDATE()) 
            THEN buyer_id 
        END) AS item_14d_sales
    FROM ShopeeOrder
    WHERE status != 'Cancelled'
    GROUP BY ItemID
),
BaseData AS (
    SELECT 
        so.skuid,
        so.BName AS [浏览器店铺名],
        so.SJBM AS [商家编码],
        so.pname AS [销售链接产品标题],
        c.huopinbiaoti AS [货源链接产品标题],
        so.pimage,
        COALESCE(sa.sales_7d, 0) AS [该SKU7天总销量],
        COALESCE(sa.sales_14d, 0) AS [该SKU14天总销量],
        COALESCE(sa.sales_30d, 0) AS [该SKU30天总销量],
        st.stock AS [实际在仓数量],
        COALESCE(it.fachu_total, 0) AS [莆田已发货数量],
        COALESCE(it.weifachu_total, 0) AS [莆田未发货数量],
        COALESCE(it.zongzaitu, 0) AS [实际在途总数],
        'https://detail.1688.com/offer/' + so.ItemID + '.html' AS [1688采购链接],
        p8.sku1 AS [1688SKU1],
        p8.sku2 AS [1688SKU2],
        p8.sku_price AS [1688价格],
        CASE 
            WHEN im.item_14d_sales > 0 
            THEN CAST((COALESCE(im.required_stock, 0) * 1.0 / NULLIF(im.item_14d_sales, 0)) * sa.sales_14d AS DECIMAL(18,2))
            ELSE 0 
        END AS [该SKU需维持在仓在途数],
        ROW_NUMBER() OVER (
            PARTITION BY so.skuid 
            ORDER BY so.order_date DESC
        ) AS rn
    FROM ShopeeOrder so
    LEFT JOIN caiwu c ON so.ItemID = c.rucangITEMID
    LEFT JOIN Prepared1688 p8 ON c.SKU_ID = p8.bridge_skuid
    LEFT JOIN SalesAgg sa ON so.skuid = sa.skuid
    LEFT JOIN ShopeeStock st ON so.skuid = st.skuid
    LEFT JOIN InTransitData it ON c.SKU_ID = it.SKU_ID
    LEFT JOIN ItemMaintenance im ON so.ItemID = im.ItemID
    WHERE 1=1
    " + whereCondition + @"
)
SELECT 
    [浏览器店铺名],
    [商家编码],
    [销售链接产品标题],
    [货源链接产品标题],
    pimage,
    skuid AS [销售链接SKU],
    [该SKU7天总销量],
    [该SKU14天总销量],
    [该SKU30天总销量],
    [实际在仓数量],
    [莆田已发货数量],
    [莆田未发货数量],
    [实际在途总数],
    [1688采购链接],
    [1688SKU1],
    [1688SKU2],
    [1688价格],
    [该SKU需维持在仓在途数]
FROM BaseData
WHERE rn = 1;";

            DataSet ds = access_sql.GreatDs(sql,300);

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
            string bname = txtBName.Text.Trim();
            string status = ddlStatus.SelectedValue;

            // 统一调用方式，明确传递三个参数
            bindzhy(SJBM: sjbm, BName: bname, status: status);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {


        }
        public void clzy()
        {


        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            clzy();
        }
    }
}