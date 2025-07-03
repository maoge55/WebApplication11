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
    public partial class 采购单_印尼_采购员 : System.Web.UI.Page
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
                if (uid != "6" && uid != "9")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public void bindzhy(String YYBM = "",string status="")
        {
            ViewState["YYBM"] = YYBM;
            ViewState["status"] = status;
            string safeYYBM = !string.IsNullOrEmpty(YYBM) ? YYBM.Replace("'", "''") : "";

            string safeStatus = status.Replace("'", "''");
            string whereCondition = "";
            if (!string.IsNullOrEmpty(safeYYBM))
                whereCondition += " AND so.YYBM = '" + safeYYBM + "'";

            if (!string.IsNullOrEmpty(status) && status != "-1")

                whereCondition += " AND so.caigoudanzhuangtai = '" + safeStatus + "'";
                string countSql = string.Format(@"
    SELECT COUNT(so.caigoudanhao)  
    FROM Purchase_YN_User so
    WHERE 1=1 {0}", whereCondition);
      int totalRecords = Convert.ToInt32(access_sql.ExecInt2(countSql));
            TotalPagesCount = (totalRecords + PageSize - 1) / PageSize;

            // 分页查询
            int startRow = (CurrentPageIndex - 1) * PageSize + 1;
            int endRow = CurrentPageIndex * PageSize;
            string sql = @"
WITH MainData AS (
    SELECT 
        so.caigoudanhao AS [采购单号],
        so.caigoudanzhuangtai AS [采购单状态],
        so.xucaigoushuliang AS [需采购数量],
        so.pimage AS [SKU图片],
        so.Y_1688url AS [1688产品链接],
        s.Y_1688sku1 AS [1688SKU1],
        s.Y_1688sku2 AS [1688SKU2],
        s.Y_1688price AS [1688价格],
        so.huopinbiaoti as  [货源标题],
        so.skuid as [SKU],
        ROW_NUMBER() OVER (ORDER BY so.caigoudanhao) AS RowNum
    FROM Purchase_YN_User so
    LEFT JOIN ShopeeOrder s ON so.skuid = s.skuid 
    WHERE 1=1 "+ whereCondition+ @"
    GROUP BY 
        so.caigoudanhao, 
        so.caigoudanzhuangtai, 
        so.xucaigoushuliang, 
        so.pimage, 
        so.Y_1688url,
        s.Y_1688sku1,
        s.Y_1688sku2,
        s.Y_1688price,
        so.huopinbiaoti,
        so.skuid
)
SELECT 
    [采购单号],
    [货源标题],
    [SKU图片],
    [1688产品链接], 
    [1688SKU1], 
    [1688SKU2], 
    [1688价格],
    [需采购数量],
    [采购单状态],
[SKU]
       
FROM MainData
WHERE RowNum BETWEEN " + startRow + " AND " + endRow;


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
            string yybm = txtsjbm.Text.Trim();
            string status = ddlStatus.SelectedValue;


           bindzhy(yybm,  status);
               
        }

            protected void btnPrev_Click(object sender, EventArgs e)
            {
                if (CurrentPageIndex > 1)
                {
                    CurrentPageIndex--;
                    bindzhy(
                        ViewState["YYBM"] as string,
                      
                        ViewState["status"] as string
                       
                    );
                }
            }

            protected void btnNext_Click(object sender, EventArgs e)
            {
                if (CurrentPageIndex < TotalPagesCount)
                {
                    CurrentPageIndex++;
                    bindzhy(
                        ViewState["YYBM"] as string,
                     
                        ViewState["status"] as string
                        
                    );
                }
            }

            protected void Button1_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            string yybm = txtsjbm.Text.Trim();
           
            string status = ddlStatus.SelectedValue;
         
            // 统一调用方式，明确传递三个参数
            bindzhy(YYBM: yybm,status: status);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "qr")
            {
                lits.Text = "";
                ulong 采购单号 = Convert.ToUInt64(e.CommandArgument);
                TextBox sku1 = e.Item.FindControl("sku1") as TextBox;
                TextBox sku2 = e.Item.FindControl("sku2") as TextBox;
                TextBox price = e.Item.FindControl("price") as TextBox;
                Literal skuid = (Literal)e.Item.FindControl("SKU");
                string id = skuid.Text;

                DropDownList caigoudanzhuangtai = e.Item.FindControl("caigoudanzhuangtai") as DropDownList; 
                if (access_sql.T_Update_ExecSql(new string[] { "Y_1688sku1", "Y_1688sku2", "Y_1688price" },
                  new object[] {sku1.Text.Trim().Replace("'", "''"), sku2.Text.Trim().Replace("'", "''"), price.Text.Trim().Replace("'", "''") },
                  "ShopeeOrder",
                 "skuid='" + id + "'") > 0)
                {


                    access_sql.T_Update_ExecSql(new string[] { "caigoudanzhuangtai" },
                  new object[] { caigoudanzhuangtai.SelectedValue },
                  "Purchase_YN_User",
                 "caigoudanhao='" + 采购单号 + "'");
                    bindzhy(ViewState["YYBM"] as string,
                        ViewState["status"] as string);
                    lits.Text =  "更新成功";
                }



            }


        }
        

        public void clzy()
        {
            int cg = 0;


            for (int i = 0; i < rplb.Items.Count; i++)
            {
                Literal 采购单号 = (Literal)rplb.Items[i].FindControl("采购单号");

                String id = 采购单号.Text;
                TextBox sku1 = rplb.Items[i].FindControl("sku1") as TextBox;
                TextBox sku2 = rplb.Items[i].FindControl("sku2") as TextBox;
                TextBox price = rplb.Items[i].FindControl("price") as TextBox;
                DropDownList caigoudanzhuangtai = rplb.Items[i].FindControl("caigoudanzhuangtai") as DropDownList;
                Literal SKU = (Literal)rplb.Items[i].FindControl("SKU");
                string skuid = SKU.Text;

                cg += access_sql.T_Update_ExecSql(new string[] {  "Y_1688sku1", "Y_1688sku2", "Y_1688price" },
                  new object[] {   sku1.Text.Trim().Replace("'", "''"),
                  sku2.Text.Trim().Replace("'", "''"),price.Text.Trim().Replace("'", "''")},
                  "ShopeeOrder",
                 "skuid='" + skuid + "'");
                cg += access_sql.T_Update_ExecSql(new string[] { },
                  new object[] { caigoudanzhuangtai.SelectedValue },
                  "Purchase_YN_User",
                 "caigoudanhao='" + id + "'");


            }
            bindzhy(ViewState["YYBM"] as string,
                        ViewState["status"] as string);
            lits.Text = "更新成功" + cg + "个";

        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            clzy();
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
                        ViewState["YYBM"] as string,
                        ViewState["status"] as string
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