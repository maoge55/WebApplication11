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
    public partial class 采购订单补充资料 : System.Web.UI.Page
    {
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

        private DataTable _logisticsProductTypes = null;
        private DataTable LogisticsProductTypes
        {
            get
            {
                if (_logisticsProductTypes == null)
                {
                    string sql = "SELECT logistics_product_type_code FROM HeadLogisticsPrice group by logistics_product_type_code ORDER BY logistics_product_type_code COLLATE Chinese_PRC_Stroke_CI_AI_KS";
                    DataSet ds = access_sql.GreatDs(sql);
                    if (access_sql.yzTable(ds))
                    {
                        _logisticsProductTypes = ds.Tables[0];
                    }
                    else
                    {
                        _logisticsProductTypes = new DataTable();
                    }
                }
                return _logisticsProductTypes;
            }
        }

        private void ResetLogisticsProductTypes()
        {
            _logisticsProductTypes = null;
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


            // 新增：检查URL中的SKU_ID参数
            if (!IsPostBack)
            {
                // 加载物流商品种编号数据
                LoadLogisticsProductTypes();
                
                // 初始化批量应用下拉框
                LoadBatchDropdowns();

                string skuID = Request.QueryString["SKU_ID"];
                string OfferID = Request.QueryString["OfferID"];
                if (!string.IsNullOrEmpty(skuID))
                {
                    // 自动填充SKU_ID到搜索框
                    txtsjbm.Text = skuID;
                    // 自动触发搜索
                    CurrentPageIndex = 1;
                    BindWithSkuId(skuID); // 使用新的搜索方法
                }
                if (!string.IsNullOrEmpty(OfferID))
                {
                    // 自动填充SKU_ID到搜索框
                    txtsjbm.Text = OfferID;
                    // 自动触发搜索
                    CurrentPageIndex = 1;
                    BindWithOfferID(OfferID); // 使用新的搜索方法
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";


        public void BindWithSkuId(string skuID ) {
            // 安全处理输入
            string safeSkuID = skuID.Replace("'", "''");

            // 构建查询条件
            string whereCondition = " WHERE p.SkuID_1688 = '" + safeSkuID + "'";

            // 查询总记录数
            string countSql = "SELECT COUNT(*) " +
                              "FROM Purchase_Sales_Warehouse p " +
                              whereCondition;

            int totalRecords = Convert.ToInt32(access_sql.ExecInt2(countSql));
            TotalPagesCount = (totalRecords + PageSize - 1) / PageSize;

            // 分页查询
            int startRow = (CurrentPageIndex - 1) * PageSize + 1;
            int endRow = CurrentPageIndex * PageSize;

            string sql = "SELECT * FROM (" +
                         "   SELECT ROW_NUMBER() OVER (ORDER BY T.SkuID_1688, T.OfferID_1688) AS RowNum, T.* " +
                         "  FROM (SELECT DISTINCT " +
                         "           p.pname_1688 as huopinbiaoti, p.sku_img, p.danjia, p.yunyingbianma, p.chang, p.kuan, p.gao, p.zhongliang, p.sjtype, p.tijizhongliang, " +
                         "           p.baozhuanghe, p.baozhuanghe1688lianjie1, p.baozhuanghe1688jiage1, p.baozhuanghe1688lianjie2, " +
                         "           p.baozhuanghe1688jiage2, p.beizhu, " +
                         "           p.SkuID_1688, p.OfferID_1688, p.baozhuanghechang, p.baozhuanghekuan, p.baozhuanghegao, " +
                         "           p.zuidiqipiliang1, p.zuidiqipiliang2, " +
                         "           p.logistics_product_type_code_pt_gd, " +
                         "           p.logistics_product_type_code_gd_id, " +
                         "           p.logistics_product_type_code_gd_th, " +
                         "           ROUND(ISNULL(CAST(NULLIF(p.zhongliang, '') AS decimal(18,2)), 0) * h1.first_logistics_price, 2) as logistics_fee_pt_gd, " +
                         "           ROUND(ISNULL(CAST(NULLIF(p.chang, '') AS decimal(18,2)), 0) * " +
                         "                 ISNULL(CAST(NULLIF(p.kuan, '') AS decimal(18,2)), 0) * " +
                         "                 ISNULL(CAST(NULLIF(p.gao, '') AS decimal(18,2)), 0) * " +
                         "                 h2.first_logistics_price / 1000000, 2) as logistics_fee_gd_id, " +
                         "           ROUND(ISNULL(CAST(NULLIF(p.chang, '') AS decimal(18,2)), 0) * " +
                         "                 ISNULL(CAST(NULLIF(p.kuan, '') AS decimal(18,2)), 0) * " +
                         "                 ISNULL(CAST(NULLIF(p.gao, '') AS decimal(18,2)), 0) * " +
                         "                 h3.first_logistics_price / 1000000, 2) as logistics_fee_gd_th " +
                         "    FROM Purchase_Sales_Warehouse p " +
                         "    LEFT JOIN HeadLogisticsPrice h1 ON p.logistics_product_type_code_pt_gd = h1.logistics_product_type_code " +
                         "    LEFT JOIN HeadLogisticsPrice h2 ON p.logistics_product_type_code_gd_id = h2.logistics_product_type_code " +
                         "    LEFT JOIN HeadLogisticsPrice h3 ON p.logistics_product_type_code_gd_th = h3.logistics_product_type_code " +
                         "    " + whereCondition +
                         ") AS T ) AS T2 " +
                         "WHERE RowNum BETWEEN " + startRow + " AND " + endRow;

            DataSet ds = access_sql.GreatDs(sql);

            if (access_sql.yzTable(ds))
            {
                LoadLogisticsProductTypes(); // 先注册事件
                rplb.DataSource = ds.Tables[0];
                rplb.DataBind();
            }
            else
            {
                rplb.DataSource = null;
                rplb.DataBind();
                lits.Text = "未找到指定的SKU信息";
            }

            UpdatePagerInfo();
        }
        public void BindWithOfferID(string OfferID)
        {
            // 安全处理输入
            string safeOfferID = OfferID.Replace("'", "''");

            // 构建查询条件
            string whereCondition = " WHERE p.OfferID_1688 = '" + safeOfferID + "'";

            // 查询总记录数
            string countSql = "SELECT COUNT(*) " +
                              "FROM Purchase_Sales_Warehouse p " +
                              whereCondition;

            int totalRecords = Convert.ToInt32(access_sql.ExecInt2(countSql));
            TotalPagesCount = (totalRecords + PageSize - 1) / PageSize;

            // 分页查询
            int startRow = (CurrentPageIndex - 1) * PageSize + 1;
            int endRow = CurrentPageIndex * PageSize;

            string sql = "SELECT * FROM (" +
                         "   SELECT ROW_NUMBER() OVER (ORDER BY T.SkuID_1688, T.OfferID_1688) AS RowNum, T.* " +
                         "  FROM (SELECT DISTINCT " +
                         "           p.pname_1688 as huopinbiaoti, p.sku_img, p.danjia, p.yunyingbianma, p.chang, p.kuan, p.gao, p.zhongliang, p.sjtype, p.tijizhongliang, " +
                         "           p.baozhuanghe, p.baozhuanghe1688lianjie1, p.baozhuanghe1688jiage1, p.baozhuanghe1688lianjie2, " +
                         "           p.baozhuanghe1688jiage2, p.beizhu, " +
                         "           p.SkuID_1688, p.OfferID_1688, p.baozhuanghechang, p.baozhuanghekuan, p.baozhuanghegao, " +
                         "           p.zuidiqipiliang1, p.zuidiqipiliang2, " +
                         "           p.logistics_product_type_code_pt_gd, " +
                         "           p.logistics_product_type_code_gd_id, " +
                         "           p.logistics_product_type_code_gd_th, " +
                         "           ROUND(ISNULL(CAST(NULLIF(p.zhongliang, '') AS decimal(18,2)), 0) * h1.first_logistics_price, 2) as logistics_fee_pt_gd, " +
                         "           ROUND(ISNULL(CAST(NULLIF(p.chang, '') AS decimal(18,2)), 0) * " +
                         "                 ISNULL(CAST(NULLIF(p.kuan, '') AS decimal(18,2)), 0) * " +
                         "                 ISNULL(CAST(NULLIF(p.gao, '') AS decimal(18,2)), 0) * " +
                         "                 h2.first_logistics_price / 1000000, 2) as logistics_fee_gd_id, " +
                         "           ROUND(ISNULL(CAST(NULLIF(p.chang, '') AS decimal(18,2)), 0) * " +
                         "                 ISNULL(CAST(NULLIF(p.kuan, '') AS decimal(18,2)), 0) * " +
                         "                 ISNULL(CAST(NULLIF(p.gao, '') AS decimal(18,2)), 0) * " +
                         "                 h3.first_logistics_price / 1000000, 2) as logistics_fee_gd_th " +
                         "    FROM Purchase_Sales_Warehouse p " +
                         "    LEFT JOIN HeadLogisticsPrice h1 ON p.logistics_product_type_code_pt_gd = h1.logistics_product_type_code " +
                         "    LEFT JOIN HeadLogisticsPrice h2 ON p.logistics_product_type_code_gd_id = h2.logistics_product_type_code " +
                         "    LEFT JOIN HeadLogisticsPrice h3 ON p.logistics_product_type_code_gd_th = h3.logistics_product_type_code " +
                         "    " + whereCondition +
                         ") AS T ) AS T2 " +
                         "WHERE RowNum BETWEEN " + startRow + " AND " + endRow;

            DataSet ds = access_sql.GreatDs(sql);

            if (access_sql.yzTable(ds))
            {
                LoadLogisticsProductTypes(); // 先注册事件
                rplb.DataSource = ds.Tables[0];
                rplb.DataBind();
            }
            else
            {
                rplb.DataSource = null;
                rplb.DataBind();
                lits.Text = "未找到指定的信息";
            }

            UpdatePagerInfo();
        }
        public void bindzhy(string YYBM = "", string status = "-1")
        {
            ViewState["YYBM"] = YYBM;
            ViewState["status"] = status;
            string safeYYBM = !string.IsNullOrEmpty(YYBM) ? YYBM.Replace("'", "''") : "";
            string titleOfferSku = txtTitleOfferSku.Text.Trim().Replace("'", "''");

            string whereCondition = " WHERE 1=1";

            if (!string.IsNullOrEmpty(safeYYBM))
                whereCondition += " AND p.yunyingbianma = '" + safeYYBM + "'";

            if (!string.IsNullOrEmpty(titleOfferSku))
                whereCondition += " AND (p.pname_1688 LIKE '%" + titleOfferSku + "%' OR p.OfferID_1688 LIKE '%" + titleOfferSku + "%' OR p.SkuID_1688 LIKE '%" + titleOfferSku + "%')";

            string skuID = Request.QueryString["SKU_ID"];
            if (!string.IsNullOrEmpty(skuID))
            {
                whereCondition += " AND p.SkuID_1688 = '" + skuID.Replace("'", "''") + "'";
            }
            string OfferID = Request.QueryString["OfferID"];
            if (!string.IsNullOrEmpty(OfferID))
            {
                whereCondition += " AND p.OfferID_1688 = '" + OfferID.Replace("'", "''") + "'";
            }
            if (!string.IsNullOrEmpty(status) && status != "-1")
            {
                switch (status)
                {
                    case "补充长宽高":
                        whereCondition += " AND ((p.chang IS NULL OR p.chang = '') OR (p.kuan IS NULL OR p.kuan = '') OR (p.gao IS NULL OR p.gao = ''))";
                        break;
                    case "补充重量":
                        whereCondition += " AND (p.zhongliang IS NULL OR p.zhongliang = '')";
                        break;
                    case "补充包装盒链接":
                        whereCondition += @" AND ((p.baozhuanghe1688lianjie1 IS NULL OR p.baozhuanghe1688lianjie1 = '') 
                          OR (p.baozhuanghe1688lianjie2 IS NULL OR p.baozhuanghe1688lianjie2 = ''))";
                        break;
                    case "补充物流商品种编号":
                        whereCondition += @" AND ((p.logistics_product_type_code_pt_gd IS NULL OR p.logistics_product_type_code_pt_gd = '') 
                          OR (p.logistics_product_type_code_gd_id IS NULL OR p.logistics_product_type_code_gd_id = '') 
                          OR (p.logistics_product_type_code_gd_th IS NULL OR p.logistics_product_type_code_gd_th = ''))";
                        break;
                }
            }
            // 查询总记录数
            string countSql = string.Format(@"
                SELECT COUNT(*)
                FROM Purchase_Sales_Warehouse p
                {0}", whereCondition);

            int totalRecords = Convert.ToInt32(access_sql.ExecInt2(countSql));
            TotalPagesCount = (totalRecords + PageSize - 1) / PageSize;

            // 分页查询
            int startRow = (CurrentPageIndex - 1) * PageSize + 1;
            int endRow = CurrentPageIndex * PageSize;
            string sql = string.Format(@"
        SELECT * FROM (
             SELECT ROW_NUMBER() OVER (ORDER BY T.SkuID_1688, T.OfferID_1688) AS RowNum, T.* FROM (SELECT DISTINCT  
                   p.pname_1688 as huopinbiaoti, p.sku_img, p.danjia, p.yunyingbianma, p.chang, p.kuan, p.gao, p.zhongliang, p.sjtype, p.tijizhongliang,
                   p.baozhuanghe, p.baozhuanghe1688lianjie1, p.baozhuanghe1688jiage1, p.baozhuanghe1688lianjie2,
                   p.baozhuanghe1688jiage2, p.beizhu,
                   p.SkuID_1688, p.OfferID_1688, p.baozhuanghechang, p.baozhuanghekuan, p.baozhuanghegao,
                   p.zuidiqipiliang1, p.zuidiqipiliang2,
                   p.logistics_product_type_code_pt_gd,
                   p.logistics_product_type_code_gd_id,
                   p.logistics_product_type_code_gd_th,
                   ROUND(ISNULL(CAST(NULLIF(p.zhongliang, '') AS decimal(18,2)), 0) * h1.first_logistics_price, 2) as logistics_fee_pt_gd,
                   ROUND(ISNULL(CAST(NULLIF(p.chang, '') AS decimal(18,2)), 0) * 
                         ISNULL(CAST(NULLIF(p.kuan, '') AS decimal(18,2)), 0) * 
                         ISNULL(CAST(NULLIF(p.gao, '') AS decimal(18,2)), 0) * 
                         h2.first_logistics_price / 1000000, 2) as logistics_fee_gd_id,
                   ROUND(ISNULL(CAST(NULLIF(p.chang, '') AS decimal(18,2)), 0) * 
                         ISNULL(CAST(NULLIF(p.kuan, '') AS decimal(18,2)), 0) * 
                         ISNULL(CAST(NULLIF(p.gao, '') AS decimal(18,2)), 0) * 
                         h3.first_logistics_price / 1000000, 2) as logistics_fee_gd_th
            FROM Purchase_Sales_Warehouse p 
            LEFT JOIN HeadLogisticsPrice h1 ON p.logistics_product_type_code_pt_gd = h1.logistics_product_type_code
            LEFT JOIN HeadLogisticsPrice h2 ON p.logistics_product_type_code_gd_id = h2.logistics_product_type_code
            LEFT JOIN HeadLogisticsPrice h3 ON p.logistics_product_type_code_gd_th = h3.logistics_product_type_code
            {0}
        ) AS T) AS T2
        WHERE RowNum BETWEEN {1} AND {2}",
                  whereCondition, startRow, endRow);


            DataSet ds = access_sql.GreatDs(sql);



            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];
                LoadLogisticsProductTypes(); // 先注册事件
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            string searchValue = txtsjbm.Text.Trim();
            string status = ddlStatus.SelectedValue;

            // 判断是搜索运营编码还是SKU ID
            if (IsSkuIdSearch(searchValue))
            {
                BindWithSkuId(searchValue);
            }
            else
            {
                bindzhy(YYBM: searchValue, status: status);
            }
        }
        // 辅助方法：判断输入是否是SKU ID
        private bool IsSkuIdSearch(string value)
        {
            // 假设SKU ID是纯数字，运营编码包含字母或其他字符
            // 根据实际业务逻辑调整
            long result;
            return long.TryParse(value, out result);
        }
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex > 1)
            {
                CurrentPageIndex--;
                BindWithCurrentFilters();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex < TotalPagesCount)
            {
                CurrentPageIndex++;
                BindWithCurrentFilters();
            }
        }
        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "qr")
            {
                lits.Text = "";
                string[] args = e.CommandArgument.ToString().Split('|');
                string offerId = args[0];
                string skuId = args.Length > 1 ? args[1] : null;

                TextBox txtY_length = e.Item.FindControl("txtY_length") as TextBox;
                TextBox txtY_width = e.Item.FindControl("txtY_width") as TextBox;
                TextBox txtY_hight = e.Item.FindControl("txtY_hight") as TextBox;
                TextBox txtY_weight = e.Item.FindControl("txtY_weight") as TextBox;
                DropDownList baozhuanghe = e.Item.FindControl("baozhuanghe") as DropDownList;
                DropDownList sjtype = e.Item.FindControl("sjtype") as DropDownList;

                TextBox baozhuanghechang = e.Item.FindControl("baozhuanghechang") as TextBox;
                TextBox baozhuanghekuan = e.Item.FindControl("baozhuanghekuan") as TextBox;
                TextBox baozhuanghegao = e.Item.FindControl("baozhuanghegao") as TextBox;
                TextBox zuidiqipiliang1 = e.Item.FindControl("zuidiqipiliang1") as TextBox;
                TextBox zuidiqipiliang2 = e.Item.FindControl("zuidiqipiliang2") as TextBox;
                TextBox zuidiqipiliang3 = e.Item.FindControl("zuidiqipiliang3") as TextBox;

                TextBox baozhuanghe1688lianjie1 = e.Item.FindControl("baozhuanghe1688lianjie1") as TextBox;
                TextBox baozhuanghe1688jiage1 = e.Item.FindControl("baozhuanghe1688jiage1") as TextBox;
                TextBox baozhuanghe1688lianjie2 = e.Item.FindControl("baozhuanghe1688lianjie2") as TextBox;
                TextBox baozhuanghe1688jiage2 = e.Item.FindControl("baozhuanghe1688jiage2") as TextBox;
                TextBox baozhuanghe1688lianjie3 = e.Item.FindControl("baozhuanghe1688lianjie3") as TextBox;
                TextBox baozhuanghe1688jiage3 = e.Item.FindControl("baozhuanghe1688jiage3") as TextBox;
                TextBox beizhu = e.Item.FindControl("beizhu") as TextBox;

                DropDownList ddlLogisticsPtGd = e.Item.FindControl("ddlLogisticsPtGd") as DropDownList;
                DropDownList ddlLogisticsGdId = e.Item.FindControl("ddlLogisticsGdId") as DropDownList;
                DropDownList ddlLogisticsGdTh = e.Item.FindControl("ddlLogisticsGdTh") as DropDownList;

                string whereCondition;
                if (string.IsNullOrEmpty(skuId))
                {
                    // 如果SkuID为空，更新对应OfferID且SkuID为null或空的记录
                    whereCondition = "OfferID_1688='" + offerId + "' AND (SkuID_1688 IS NULL OR SkuID_1688 = '')";
                }
                else
                {
                    // 如果SkuID不为空，按OfferID和SkuID一起筛选
                    whereCondition = "OfferID_1688='" + offerId + "' AND SkuID_1688='" + skuId + "'";
                }

                if (access_sql.T_Update_ExecSql(
                    new string[] {
                        "chang", "kuan", "gao", "zhongliang", "sjtype", "baozhuanghe",
                        "baozhuanghe1688lianjie1", "baozhuanghe1688jiage1",
                        "baozhuanghe1688lianjie2", "baozhuanghe1688jiage2",
                        "beizhu", "baozhuanghechang", "baozhuanghekuan", "baozhuanghegao",
                        "zuidiqipiliang1", "zuidiqipiliang2",
                        "logistics_product_type_code_pt_gd",
                        "logistics_product_type_code_gd_id",
                        "logistics_product_type_code_gd_th"
                    },
                    new object[] {
                        txtY_length.Text.Trim().Replace("'", "''"),
                        txtY_width.Text.Trim().Replace("'", "''"),
                        txtY_hight.Text.Trim().Replace("'", "''"),
                        txtY_weight.Text.Trim().Replace("'", "''"),
                        sjtype.SelectedValue,
                        baozhuanghe.SelectedValue,
                        baozhuanghe1688lianjie1.Text.Trim().Replace("'", "''"),
                        baozhuanghe1688jiage1.Text.Trim().Replace("'", "''"),
                        baozhuanghe1688lianjie2.Text.Trim().Replace("'", "''"),
                        baozhuanghe1688jiage2.Text.Trim().Replace("'", "''"),
                        beizhu.Text.Trim().Replace("'", "''"),
                        baozhuanghechang.Text.Trim().Replace("'", "''"),
                        baozhuanghekuan.Text.Trim().Replace("'", "''"),
                        baozhuanghegao.Text.Trim().Replace("'", "''"),
                        zuidiqipiliang1.Text.Trim().Replace("'", "''"),
                        zuidiqipiliang2.Text.Trim().Replace("'", "''"),
                        ddlLogisticsPtGd.SelectedValue,
                        ddlLogisticsGdId.SelectedValue,
                        ddlLogisticsGdTh.SelectedValue
                    },
                    "Purchase_Sales_Warehouse",
                    whereCondition
                ) > 0)
                {
                    ResetLogisticsProductTypes(); // 重置缓存
                    lits.Text = "更新成功";
                    RefreshData(); // 使用新方法刷新数据
                }
            }

            if (e.CommandName == "sync")
            {
                string offerId = e.CommandArgument.ToString();
                Literal litSkuId = e.Item.FindControl("SkuID_1688") as Literal;
                string currentSkuId = litSkuId.Text;

                // 获取当前行的数据
                TextBox txtY_length = e.Item.FindControl("txtY_length") as TextBox;
                TextBox txtY_width = e.Item.FindControl("txtY_width") as TextBox;
                TextBox txtY_hight = e.Item.FindControl("txtY_hight") as TextBox;
                TextBox txtY_weight = e.Item.FindControl("txtY_weight") as TextBox;
                DropDownList baozhuanghe = e.Item.FindControl("baozhuanghe") as DropDownList;
                DropDownList sjtype = e.Item.FindControl("sjtype") as DropDownList;
                TextBox baozhuanghechang = e.Item.FindControl("baozhuanghechang") as TextBox;
                TextBox baozhuanghekuan = e.Item.FindControl("baozhuanghekuan") as TextBox;
                TextBox baozhuanghegao = e.Item.FindControl("baozhuanghegao") as TextBox;
                TextBox zuidiqipiliang1 = e.Item.FindControl("zuidiqipiliang1") as TextBox;
                TextBox zuidiqipiliang2 = e.Item.FindControl("zuidiqipiliang2") as TextBox;
                TextBox baozhuanghe1688lianjie1 = e.Item.FindControl("baozhuanghe1688lianjie1") as TextBox;
                TextBox baozhuanghe1688jiage1 = e.Item.FindControl("baozhuanghe1688jiage1") as TextBox;
                TextBox baozhuanghe1688lianjie2 = e.Item.FindControl("baozhuanghe1688lianjie2") as TextBox;
                TextBox baozhuanghe1688jiage2 = e.Item.FindControl("baozhuanghe1688jiage2") as TextBox;
                TextBox beizhu = e.Item.FindControl("beizhu") as TextBox;

                DropDownList ddlLogisticsPtGd = e.Item.FindControl("ddlLogisticsPtGd") as DropDownList;
                DropDownList ddlLogisticsGdId = e.Item.FindControl("ddlLogisticsGdId") as DropDownList;
                DropDownList ddlLogisticsGdTh = e.Item.FindControl("ddlLogisticsGdTh") as DropDownList;

                // 查询同Offer下的所有SKU（排除当前行）
                string sql;
                if (string.IsNullOrEmpty(currentSkuId))
                {
                    // 如果当前SKU为空，查询其他所有非空SKU  理论应该不存在这种情况
                    sql = "SELECT SkuID_1688 FROM Purchase_Sales_Warehouse " +
                          "WHERE OfferID_1688 = '" + offerId + "' " +
                          "AND SkuID_1688 IS NOT NULL AND SkuID_1688 <> ''";
                }
                else
                {
                    // 如果当前SKU不为空，排除当前SKU
                    sql = "SELECT SkuID_1688 FROM Purchase_Sales_Warehouse " +
                          "WHERE OfferID_1688 = '" + offerId + "' " +
                          "AND SkuID_1688 <> '" + currentSkuId + "'";
                }

                DataSet ds = access_sql.GreatDs(sql);
                int updateCount = 0;

                if (access_sql.yzTable(ds))
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string skuId = row["SkuID_1688"].ToString();

                        // 更新Purchase_Sales_Warehouse表
                        int cg2 = access_sql.T_Update_ExecSql(
                            new string[] {
                                "chang", "kuan", "gao", "zhongliang", "sjtype", "baozhuanghe",
                                "baozhuanghechang", "baozhuanghekuan", "baozhuanghegao",
                                "baozhuanghe1688lianjie1", "baozhuanghe1688jiage1",
                                "baozhuanghe1688lianjie2", "baozhuanghe1688jiage2",
                                "zuidiqipiliang1", "zuidiqipiliang2",
                                "logistics_product_type_code_pt_gd", "logistics_product_type_code_gd_id", "logistics_product_type_code_gd_th",
                                "beizhu"
                            },
                            new object[] {
                                txtY_length.Text.Trim().Replace("'", "''"),
                                txtY_width.Text.Trim().Replace("'", "''"),
                                txtY_hight.Text.Trim().Replace("'", "''"),
                                txtY_weight.Text.Trim().Replace("'", "''"),
                                sjtype.SelectedValue,
                                baozhuanghe.SelectedValue,
                                baozhuanghechang.Text.Trim().Replace("'", "''"),
                                baozhuanghekuan.Text.Trim().Replace("'", "''"),
                                baozhuanghegao.Text.Trim().Replace("'", "''"),
                                baozhuanghe1688lianjie1.Text.Trim().Replace("'", "''"),
                                baozhuanghe1688jiage1.Text.Trim().Replace("'", "''"),
                                baozhuanghe1688lianjie2.Text.Trim().Replace("'", "''"),
                                baozhuanghe1688jiage2.Text.Trim().Replace("'", "''"),
                                zuidiqipiliang1.Text.Trim().Replace("'", "''"),
                                zuidiqipiliang2.Text.Trim().Replace("'", "''"),
                                ddlLogisticsPtGd.SelectedValue,
                                ddlLogisticsGdId.SelectedValue,
                                ddlLogisticsGdTh.SelectedValue,
                                beizhu.Text.Trim().Replace("'", "''")
                            },
                            "Purchase_Sales_Warehouse",
                            "SkuID_1688='" + skuId + "'"
                        );

                        if (cg2 > 0) updateCount++;
                    }
                }

                ResetLogisticsProductTypes(); // 重置缓存
                lits.Text = "同步成功，更新了" + updateCount + "个SKU";
                RefreshData(); // 使用新方法刷新数据
            }
        }
        public void clzy()
        {
            int cg = 0;

            for (int i = 0; i < rplb.Items.Count; i++)
            {
                Literal id = (Literal)rplb.Items[i].FindControl("SkuID_1688");
                Literal offerId = (Literal)rplb.Items[i].FindControl("OfferID_1688");
                String skuid = id.Text;
                String offerid = offerId.Text;

                TextBox txtY_length = rplb.Items[i].FindControl("txtY_length") as TextBox;
                TextBox txtY_width = rplb.Items[i].FindControl("txtY_width") as TextBox;
                TextBox txtY_hight = rplb.Items[i].FindControl("txtY_hight") as TextBox;
                TextBox txtY_weight = rplb.Items[i].FindControl("txtY_weight") as TextBox;
                DropDownList baozhuanghe = rplb.Items[i].FindControl("baozhuanghe") as DropDownList;
                DropDownList sjtype = rplb.Items[i].FindControl("sjtype") as DropDownList;
                TextBox baozhuanghe1688lianjie1 = rplb.Items[i].FindControl("baozhuanghe1688lianjie1") as TextBox;
                TextBox baozhuanghe1688jiage1 = rplb.Items[i].FindControl("baozhuanghe1688jiage1") as TextBox;
                TextBox baozhuanghe1688lianjie2 = rplb.Items[i].FindControl("baozhuanghe1688lianjie2") as TextBox;
                TextBox baozhuanghe1688jiage2 = rplb.Items[i].FindControl("baozhuanghe1688jiage2") as TextBox;

                TextBox baozhuanghechang = rplb.Items[i].FindControl("baozhuanghechang") as TextBox;
                TextBox baozhuanghekuan = rplb.Items[i].FindControl("baozhuanghekuan") as TextBox;
                TextBox baozhuanghegao = rplb.Items[i].FindControl("baozhuanghegao") as TextBox;
                TextBox zuidiqipiliang1 = rplb.Items[i].FindControl("zuidiqipiliang1") as TextBox;
                TextBox zuidiqipiliang2 = rplb.Items[i].FindControl("zuidiqipiliang2") as TextBox;

                DropDownList ddlLogisticsPtGd = rplb.Items[i].FindControl("ddlLogisticsPtGd") as DropDownList;
                DropDownList ddlLogisticsGdId = rplb.Items[i].FindControl("ddlLogisticsGdId") as DropDownList;
                DropDownList ddlLogisticsGdTh = rplb.Items[i].FindControl("ddlLogisticsGdTh") as DropDownList;

                TextBox beizhu = rplb.Items[i].FindControl("beizhu") as TextBox;

                string whereCondition;
                if (string.IsNullOrEmpty(skuid))
                {
                    // 如果SkuID为空，更新对应OfferID且SkuID为null或空的记录
                    whereCondition = "OfferID_1688='" + offerid + "' AND (SkuID_1688 IS NULL OR SkuID_1688 = '')";
                }
                else
                {
                    // 如果SkuID不为空，按OfferID和SkuID一起筛选
                    whereCondition = "OfferID_1688='" + offerid + "' AND SkuID_1688='" + skuid + "'";
                }
                
                cg += access_sql.T_Update_ExecSql(
                   new string[] {
                       "chang", "kuan", "gao", "zhongliang", "sjtype", "baozhuanghe", "baozhuanghe1688lianjie1",
                       "baozhuanghe1688jiage1", "baozhuanghe1688lianjie2", "baozhuanghe1688jiage2", "beizhu",
                       "baozhuanghechang","baozhuanghekuan","baozhuanghegao","zuidiqipiliang1","zuidiqipiliang2",
                       "logistics_product_type_code_pt_gd", "logistics_product_type_code_gd_id", "logistics_product_type_code_gd_th"
                   },
                   new object[] {
                       txtY_length.Text.Trim().Replace("'", "''"),
                       txtY_width.Text.Trim().Replace("'", "''"),
                       txtY_hight.Text.Trim().Replace("'", "''"),
                       txtY_weight.Text.Trim().Replace("'", "''"),
                       sjtype.SelectedValue,
                       baozhuanghe.SelectedValue,
                       baozhuanghe1688lianjie1.Text.Trim().Replace("'", "''"),
                       baozhuanghe1688jiage1.Text.Trim().Replace("'", "''"),
                       baozhuanghe1688lianjie2.Text.Trim().Replace("'", "''"),
                       baozhuanghe1688jiage2.Text.Trim().Replace("'", "''"),
                       beizhu.Text.Trim().Replace("'", "''"),
                       baozhuanghechang.Text.Trim().Replace("'", "''"),
                       baozhuanghekuan.Text.Trim().Replace("'", "''"),
                       baozhuanghegao.Text.Trim().Replace("'", "''"),
                       zuidiqipiliang1.Text.Trim().Replace("'", "''"),
                       zuidiqipiliang2.Text.Trim().Replace("'", "''"),
                       ddlLogisticsPtGd.SelectedValue,
                       ddlLogisticsGdId.SelectedValue,
                       ddlLogisticsGdTh.SelectedValue
                   },
                   "Purchase_Sales_Warehouse",
                   whereCondition
               );
            }
            ResetLogisticsProductTypes(); // 重置缓存
            RefreshData(); // 使用新方法刷新数据
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
                    BindWithCurrentFilters();
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

        private void BindWithCurrentFilters()
        {
            // 获取存储的搜索参数
            string searchValue = ViewState["YYBM"] as string;
            string status = ViewState["status"] as string;

            // 判断是搜索运营编码还是SKU ID
            if (IsSkuIdSearch(searchValue))
            {
                BindWithSkuId(searchValue);
            }
            else
            {
                bindzhy(YYBM: searchValue, status: status);
            }
        }

        private void LoadLogisticsProductTypes()
        {
            // 移除之前的事件处理器(如果有)以避免重复注册
            rplb.ItemDataBound -= Rplb_ItemDataBound;
            // 注册新的事件处理器
            rplb.ItemDataBound += Rplb_ItemDataBound;
        }

        private void LoadBatchDropdowns()
        {
            // 清空批量应用下拉框
            ddlBatchPtGd.Items.Clear();
            ddlBatchGdId.Items.Clear();
            ddlBatchGdTh.Items.Clear();

            // 添加默认选项
            ddlBatchPtGd.Items.Add(new ListItem("请选择", ""));
            ddlBatchGdId.Items.Add(new ListItem("请选择", ""));
            ddlBatchGdTh.Items.Add(new ListItem("请选择", ""));

            // 添加物流商品种编号选项
            foreach (DataRow row in LogisticsProductTypes.Rows)
            {
                string code = row["logistics_product_type_code"].ToString();
                ddlBatchPtGd.Items.Add(new ListItem(code, code));
                ddlBatchGdId.Items.Add(new ListItem(code, code));
                ddlBatchGdTh.Items.Add(new ListItem(code, code));
            }
        }

        private void Rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                try
                {
                    DropDownList ddlPtGd = e.Item.FindControl("ddlLogisticsPtGd") as DropDownList;
                    DropDownList ddlGdId = e.Item.FindControl("ddlLogisticsGdId") as DropDownList;
                    DropDownList ddlGdTh = e.Item.FindControl("ddlLogisticsGdTh") as DropDownList;

                    if (ddlPtGd != null && ddlGdId != null && ddlGdTh != null)
                    {
                        // 先清空所有选项
                        ddlPtGd.Items.Clear();
                        ddlGdId.Items.Clear();
                        ddlGdTh.Items.Clear();

                        // 添加默认选项
                        ddlPtGd.Items.Add(new ListItem("请选择", ""));
                        ddlGdId.Items.Add(new ListItem("请选择", ""));
                        ddlGdTh.Items.Add(new ListItem("请选择", ""));

                        // 添加物流商品种编号选项
                        foreach (DataRow row in LogisticsProductTypes.Rows)
                        {
                            string code = row["logistics_product_type_code"].ToString();
                            ddlPtGd.Items.Add(new ListItem(code, code));
                            ddlGdId.Items.Add(new ListItem(code, code));
                            ddlGdTh.Items.Add(new ListItem(code, code));
                        }

                        // 获取当前数据行
                        DataRowView drv = e.Item.DataItem as DataRowView;
                        if (drv != null)
                        {
                            try
                            {
                                // 设置选中值
                                string ptGdValue = drv["logistics_product_type_code_pt_gd"]?.ToString() ?? "";
                                string gdIdValue = drv["logistics_product_type_code_gd_id"]?.ToString() ?? "";
                                string gdThValue = drv["logistics_product_type_code_gd_th"]?.ToString() ?? "";

                                // 设置选中值前先检查值是否存在
                                if (!string.IsNullOrEmpty(ptGdValue) && ddlPtGd.Items.FindByValue(ptGdValue) != null)
                                {
                                    ddlPtGd.SelectedValue = ptGdValue;
                                }
                                if (!string.IsNullOrEmpty(gdIdValue) && ddlGdId.Items.FindByValue(gdIdValue) != null)
                                {
                                    ddlGdId.SelectedValue = gdIdValue;
                                }
                                if (!string.IsNullOrEmpty(gdThValue) && ddlGdTh.Items.FindByValue(gdThValue) != null)
                                {
                                    ddlGdTh.SelectedValue = gdThValue;
                                }
                            }
                            catch
                            {
                                // 如果设置选中值失败，使用默认空值
                                ddlPtGd.SelectedValue = "";
                                ddlGdId.SelectedValue = "";
                                ddlGdTh.SelectedValue = "";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 记录错误但不中断执行
                    System.Diagnostics.Debug.WriteLine("Error in Rplb_ItemDataBound: " + ex.Message);
                }
            }
        }

        private void RefreshData()
        {
            // 从控件获取当前的筛选条件
            string searchValue = txtsjbm.Text.Trim();
            string status = ddlStatus.SelectedValue;

            // 重新加载批量应用下拉框
            LoadBatchDropdowns();

            // 判断是搜索运营编码还是SKU ID
            if (IsSkuIdSearch(searchValue))
            {
                BindWithSkuId(searchValue);
            }
            else
            {
                bindzhy(YYBM: searchValue, status: status);
            }
        }
    }
}