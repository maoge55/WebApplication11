using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;

namespace WebApplication11.cg.tb
{
    public partial class 采购单_印尼_管理员_新 : System.Web.UI.Page
    {
        public string u = "";
        public string p = "";
        public string uid = "";
        
        // 表名变量
        private const string TABLE_SHOPEE_ORDER = "ShopeeOrder";
        private const string TABLE_PURCHASE_SALES_WAREHOUSE = "Purchase_Sales_Warehouse";
        private const string TABLE_S1688_ORDER = "S1688Order";
        private const string TABLE_SHOPEE_STOCK = "ShopeeStock";
        private const string TABLE_CAIGOUDAN = "caigoudan";
        private const string TABLE_TOUCHENWULIU = "touchenwuliu";
        
        // 采购单状态常量
        private static readonly string[] VALID_PURCHASE_STATUS = { "需采购" };
        private const string DEFAULT_PURCHASE_STATUS = "需采购";

        // 分页相关属性
        public int CurrentPage
        {
            get { return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1; }
            set { ViewState["CurrentPage"] = value; }
        }

        public int PageSize = 1; // 1页只展示1个offerid_1688下的数据
        
        public int TotalPages
        {
            get { return ViewState["TotalPages"] != null ? (int)ViewState["TotalPages"] : 0; }
            set { ViewState["TotalPages"] = value; }
        }

        public string CurrentOfferID
        {
            get { return ViewState["CurrentOfferID"] != null ? ViewState["CurrentOfferID"].ToString() : ""; }
            set { ViewState["CurrentOfferID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // 验证登录状态
            if (!access_sql.yzdlcg())
            {
                Response.Redirect("/cg/clogin.aspx");
            }
            else
            {
                u = HttpContext.Current.Request.Cookies["cu"].Value;
                p = HttpContext.Current.Request.Cookies["cp"].Value;
                uid = HttpContext.Current.Request.Cookies["cuid"].Value;
                
                // 检查用户权限
                if (uid != "8" && uid != "9" && uid != "18" && uid != "19" && uid != "12" && uid != "6")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

            if (!IsPostBack)
            {
                InitializePage();
            }
        }

        private void InitializePage()
        {
            lits.Text = "请输入商家编码进行查询";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSJBM.Text))
            {
                lits.Text = "商家编码为必填项！";
                return;
            }

            CurrentPage = 1;
            LoadOfferIDsByPurchaseQuantity();
            BindData();
        }

        private string BuildCommonDataQuery(string whereCondition, string finalSelect)
        {
            return $@"
                WITH SalesData AS (
                    -- SKU销量统计子查询，一次性查询3个时间段的销量
                    SELECT 
                        SKUID,
                        ISNULL(SUM(CASE WHEN order_date >= DATEADD(day, -7, CAST(GETDATE() AS DATE)) AND order_date < CAST(GETDATE() AS DATE) THEN amount ELSE 0 END), 0) as SKU7dayamount_id,
                        ISNULL(SUM(CASE WHEN order_date >= DATEADD(day, -14, CAST(GETDATE() AS DATE)) AND order_date < CAST(GETDATE() AS DATE) THEN amount ELSE 0 END), 0) as SKU14dayamount_id,
                        ISNULL(SUM(CASE WHEN order_date >= DATEADD(day, -28, CAST(GETDATE() AS DATE)) AND order_date < CAST(GETDATE() AS DATE) THEN amount ELSE 0 END), 0) as SKU28dayamount_id
                    FROM {TABLE_SHOPEE_ORDER} 
                    WHERE status IN ('Completed', 'Delivered', 'Shipped', 'To Ship', 'Order Received')
                    GROUP BY SKUID
                ),
                PurchaseData AS (
                    -- 采购单数量统计子查询
                    SELECT 
                        skuid_id,
                        ISNULL(SUM(shijicaigoushuliang), 0) as total_purchase_quantity
                    FROM {TABLE_CAIGOUDAN} 
                    WHERE status = '完成采购'
                    GROUP BY skuid_id
                ),
                LogisticsData AS (
                    -- 头程物流发出数量统计子查询
                    SELECT 
                        haiwaicangxitongbianma,
                        ISNULL(SUM(putianfachushuliang), 0) as total_logistics_quantity
                    FROM {TABLE_TOUCHENWULIU} 
                    WHERE (fahuozhuangtai IS NULL OR fahuozhuangtai = '') 
                      AND touchenwuliushan = '宝涵'
                    GROUP BY haiwaicangxitongbianma
                ),
                SkuData AS (
                    SELECT 
                        MAX(so.pimage) as pimage,                                    -- 产品图片
                        MAX(so.BName) as BName,                                      -- 浏览器名称
                        MAX(so.SJBM) as SJBM,                                        -- 商家编码
                        MAX(so.pname) as pname,                                      -- 产品标题_shopee_印尼
                        MAX(psw.pname_1688) as pname_1688,                         -- 产品标题_1688
                        MAX(so.ItemID) as ItemID,                                    -- ItemID_印尼
                        so.SKUID,                                                    -- SKUID_印尼
                        psw.haiwaicangxitongbianma as haiwaicangxitongbianma,  -- 系统编码_海仓_印尼
                        MAX(so.conversions) as conversions,                         -- 3个月广告订单数量
                        MAX(so.ROAS) as ROAS,                                        -- 3个月广告ROARS
                        
                        -- 关联销量统计数据
                        MAX(ISNULL(sd.SKU7dayamount_id, 0)) as SKU7dayamount_id,         -- SKU7天总销量
                        MAX(ISNULL(sd.SKU14dayamount_id, 0)) as SKU14dayamount_id,       -- SKU14天总销量
                        MAX(ISNULL(sd.SKU28dayamount_id, 0)) as SKU28dayamount_id,       -- SKU28天总销量
                        
                        -- 人工设置的最低最高值
                        MAX(so.sku_stock_min_transit_warehouse) as sku_stock_min_transit_warehouse,  -- SkuID最低在途在仓备货量_人工限制
                        MAX(so.sku_stock_max_transit_warehouse) as sku_stock_max_transit_warehouse,  -- SkuID最高在途在仓备货量_人工限制
                        
                        -- 在途采购单数量 = caigoudan.shijicaigoushuliang求和 - touchenwuliu.putianfachushuliang求和（确保不小于0）
                        CASE 
                            WHEN ISNULL(MAX(pd.total_purchase_quantity), 0) - ISNULL(MAX(ld.total_logistics_quantity), 0) < 0 
                            THEN 0 
                            ELSE ISNULL(MAX(pd.total_purchase_quantity), 0) - ISNULL(MAX(ld.total_logistics_quantity), 0) 
                        END as transit_purchase_quantity,    -- 在途_采购单数量_印尼
                        
                        -- 在途头程物流发出数量 = touchenwuliu.putianfachushuliang求和（确保不小于0）
                        CASE 
                            WHEN ISNULL(MAX(ld.total_logistics_quantity), 0) < 0 
                            THEN 0 
                            ELSE ISNULL(MAX(ld.total_logistics_quantity), 0) 
                        END as transit_logistics_quantity,   -- 在途_头程物流发出数量_印尼
                        
                        -- 在仓库存数量
                        ISNULL(MAX(ss.stock), 0) as stock,                         -- 在仓_库存数量_印尼
                        
                        -- 1688相关字段
                        MAX(s1688.offerid) as offerid_1688,                         -- offerid_1688
                        s1688.skuid as skuid_1688,                            -- skuid_1688
                        MAX(s1688.sku1) as sku1_1688,                              -- sku1_1688
                        MAX(s1688.sku2) as sku2_1688,                              -- sku2_1688
                        MAX(s1688.danjia) as jiage_1688,                           -- 价格_1688
                        
                        -- 运营相关字段
                        MAX(so.YYBM) as YYBM,                                       -- 运营编码
                        MAX(so.caigoudanzhuangtai) as current_caigoudanzhuangtai    -- 当前采购单状态
                        
                    FROM {TABLE_SHOPEE_ORDER} so
                    INNER JOIN {TABLE_PURCHASE_SALES_WAREHOUSE} psw ON so.SKUID = psw.rucangSKUID
                    INNER JOIN {TABLE_S1688_ORDER} s1688 ON psw.SkuID_1688 = s1688.Skuid
                    LEFT JOIN {TABLE_SHOPEE_STOCK} ss ON so.SKUID = ss.skuid
                    LEFT JOIN SalesData sd ON so.SKUID = sd.SKUID
                    LEFT JOIN PurchaseData pd ON pd.skuid_id = so.SKUID
                    LEFT JOIN LogisticsData ld ON ld.haiwaicangxitongbianma = psw.haiwaicangxitongbianma
                    WHERE {whereCondition}
                    GROUP BY so.SKUID, s1688.Skuid, psw.haiwaicangxitongbianma
                ),
                CalculatedData AS (
                    SELECT *,
                        -- 计算自动备货量
                        SKU7dayamount_id * 5 as auto_stock_quantity                   -- SkuID在途在仓备货量_自动计算
                    FROM SkuData
                ),
                FinalCalculated AS (
                    SELECT *,
                        -- 计算最终备货量（考虑人工限制）
                        CASE 
                            WHEN sku_stock_min_transit_warehouse IS NOT NULL AND auto_stock_quantity < sku_stock_min_transit_warehouse 
                                THEN sku_stock_min_transit_warehouse
                            WHEN sku_stock_max_transit_warehouse IS NOT NULL AND auto_stock_quantity > sku_stock_max_transit_warehouse 
                                THEN sku_stock_max_transit_warehouse
                            ELSE auto_stock_quantity
                        END as final_stock_quantity
                    FROM CalculatedData
                ),
                WithPurchaseQuantity AS (
                    SELECT *,
                        -- 计算需采购数量
                        CASE 
                            WHEN final_stock_quantity - transit_purchase_quantity - transit_logistics_quantity - stock > 0
                            THEN final_stock_quantity - transit_purchase_quantity - transit_logistics_quantity - stock
                            ELSE 0
                        END as xucaigoushuliang                     -- 该SKU需采购数量
                    FROM FinalCalculated
                )
                {finalSelect}";
        }

        private void LoadOfferIDsByPurchaseQuantity()
        {
            string sjbm = txtSJBM.Text.Trim().Replace("'", "''");
            string bname = txtBName.Text.Trim().Replace("'", "''");

            // 构建查询条件
            string whereCondition = "so.xiaoshouzhuangtai = '在售' AND s1688.offerid IS NOT NULL AND s1688.offerid != ''";
            
            if (!string.IsNullOrEmpty(sjbm))
            {
                whereCondition += $" AND so.SJBM = '{sjbm}'";
            }
            
            if (!string.IsNullOrEmpty(bname))
            {
                whereCondition += $" AND so.BName = '{bname}'";
            }

            string finalSelect = @"
                SELECT offerid_1688, SUM(xucaigoushuliang) as total_purchase_quantity
                FROM WithPurchaseQuantity
                GROUP BY offerid_1688
                ORDER BY total_purchase_quantity DESC";

            string sql = BuildCommonDataQuery(whereCondition, finalSelect);

            DataSet ds = access_sql.GreatDs(sql);
            if (access_sql.yzTable(ds))
            {
                ViewState["OfferIDs"] = ds.Tables[0];
                TotalPages = ds.Tables[0].Rows.Count;
            }
            else
            {
                ViewState["OfferIDs"] = null;
                TotalPages = 0;
            }
        }

        private void BindData()
        {
            // 清空lits
            lits.Text = "";
            DataTable offerIDs = ViewState["OfferIDs"] as DataTable;
            if (offerIDs == null || offerIDs.Rows.Count == 0)
            {
                rplb.DataSource = null;
                rplb.DataBind();
                lits.Text = "没有找到符合条件的数据";
                UpdatePagingInfo();
                return;
            }

            // 获取当前页的offerid
            if (CurrentPage <= offerIDs.Rows.Count)
            {
                CurrentOfferID = offerIDs.Rows[CurrentPage - 1]["offerid_1688"].ToString();
                LoadDataForCurrentOfferID();
            }

            UpdatePagingInfo();
        }

        private void LoadDataForCurrentOfferID()
        {
            string sjbm = txtSJBM.Text.Trim().Replace("'", "''");
            string bname = txtBName.Text.Trim().Replace("'", "''");

            // 构建查询条件
            string whereCondition = $"so.xiaoshouzhuangtai = '在售' AND s1688.offerid = '{CurrentOfferID.Replace("'", "''")}'";
            
            if (!string.IsNullOrEmpty(sjbm))
            {
                whereCondition += $" AND so.SJBM = '{sjbm}'";
            }
            
            if (!string.IsNullOrEmpty(bname))
            {
                whereCondition += $" AND so.BName = '{bname}'";
            }

            string finalSelect = @"
                SELECT *
                FROM WithPurchaseQuantity
                ORDER BY xucaigoushuliang DESC";

            string sql = BuildCommonDataQuery(whereCondition, finalSelect);

            DataSet ds = access_sql.GreatDs(sql);

            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];
                
                // 添加缺失的列（如果不存在）
                if (!dt.Columns.Contains("caigoudanhao"))
                {
                    dt.Columns.Add("caigoudanhao", typeof(string));
                }
                if (!dt.Columns.Contains("caigoudanzhuangtai"))
                {
                    dt.Columns.Add("caigoudanzhuangtai", typeof(string));
                }
                
                // 为每行数据添加采购单号和验证采购单状态
                foreach (DataRow row in dt.Rows)
                {
                    // 生成采购单号
                    string offerid = row["offerid_1688"]?.ToString() ?? "";
                    row["caigoudanhao"] = GenerateCaigoudanhao(offerid);
                    
                    // 验证并设置采购单状态
                    string currentStatus = row["current_caigoudanzhuangtai"]?.ToString() ?? "";
                    row["caigoudanzhuangtai"] = ValidateAndSetPurchaseStatus(currentStatus);
                }
                
                rplb.DataSource = dt;
                rplb.DataBind();
            }
            else
            {
                rplb.DataSource = null;
                rplb.DataBind();
                lits.Text = "查询数据失败";
            }
        }

        private void UpdatePagingInfo()
        {
            btnPrev.Enabled = CurrentPage > 1;
            btnNext.Enabled = CurrentPage < TotalPages;
            
            string pageInfo = $"第 {CurrentPage} 页 / 共 {TotalPages} 页";
            
            litPageInfo.Text = pageInfo;
            txtJumpPage.Text = CurrentPage.ToString();
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

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string commandName = e.CommandName;
            string skuid = e.CommandArgument.ToString();

            switch (commandName)
            {
                case "Save":
                    SaveItem(e.Item, skuid);
                    break;
                case "Generate":
                    GenerateItem(e.Item, skuid);
                    break;
                case "QuickApply":
                    QuickApplyBatch();
                    break;
            }
        }

        private void SaveItem(RepeaterItem item, string skuid)
        {
            try
            {
                TextBox txtMinStock = item.FindControl("txtMinStock") as TextBox;
                TextBox txtMaxStock = item.FindControl("txtMaxStock") as TextBox;

                object minStock = string.IsNullOrWhiteSpace(txtMinStock.Text) ? (object)DBNull.Value : Convert.ToDecimal(txtMinStock.Text.Trim());
                object maxStock = string.IsNullOrWhiteSpace(txtMaxStock.Text) ? (object)DBNull.Value : Convert.ToDecimal(txtMaxStock.Text.Trim());

                // 使用封装的更新方法
                if (access_sql.T_Update_ExecSql(
                    new string[] { "sku_stock_min_transit_warehouse", "sku_stock_max_transit_warehouse" },
                    new object[] { minStock, maxStock },
                    TABLE_SHOPEE_ORDER,
                    "SKUID='" + skuid.Replace("'", "''") + "'") > 0)
                {
                    BindData(); // 重新加载数据以更新计算结果
                    lits.Text = "保存成功！";
                }
                else
                {
                    lits.Text = "保存失败！";
                }
            }
            catch (Exception ex)
            {
                lits.Text = "保存出错：" + ex.Message;
            }
        }

        private void GenerateItem(RepeaterItem item, string skuid)
        {
            try
            {
                // 获取表单数据
                TextBox txtPurchaseQuantity = item.FindControl("txtPurchaseQuantity") as TextBox;
                DropDownList ddlYYBM = item.FindControl("ddlYYBM") as DropDownList;
                DropDownList ddlStatus = item.FindControl("ddlStatus") as DropDownList;
                
                // 从隐藏字段获取数据
                HiddenField hidCaigoudanhao = item.FindControl("hidCaigoudanhao") as HiddenField;
                HiddenField hidOfferid1688 = item.FindControl("hidOfferid1688") as HiddenField;
                HiddenField hidSkuid1688 = item.FindControl("hidSkuid1688") as HiddenField;
                HiddenField hidSku11688 = item.FindControl("hidSku11688") as HiddenField;
                HiddenField hidSku21688 = item.FindControl("hidSku21688") as HiddenField;
                HiddenField hidJiage1688 = item.FindControl("hidJiage1688") as HiddenField;

                if (hidCaigoudanhao == null || hidOfferid1688 == null || hidSkuid1688 == null || 
                    hidSku11688 == null || hidSku21688 == null || hidJiage1688 == null)
                {
                    lits.Text = "找不到隐藏字段控件！";
                    return;
                }

                string frontendCaigoudanhao = hidCaigoudanhao.Value?.Trim() ?? "";
                string offerid_1688 = hidOfferid1688.Value?.Trim() ?? "";
                string skuid_1688 = hidSkuid1688.Value?.Trim() ?? "";
                string sku1_1688 = hidSku11688.Value?.Trim() ?? "";
                string sku2_1688 = hidSku21688.Value?.Trim() ?? "";
                string jiage_1688 = hidJiage1688.Value?.Trim() ?? "0";
                
                if (string.IsNullOrWhiteSpace(frontendCaigoudanhao))
                {
                    lits.Text = "采购单号为空！";
                    return;
                }



                double purchaseQuantity;
                if (!double.TryParse(txtPurchaseQuantity.Text.Trim(), out purchaseQuantity) || purchaseQuantity <= 0)
                {
                    lits.Text = "需采购数量必须大于0！";
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(offerid_1688) || string.IsNullOrWhiteSpace(skuid_1688))
                {
                    lits.Text = "1688数据不完整！";
                    return;
                }


                // 调用核心操作方法
                try
                {
                    bool success = ExecuteGeneratePurchaseOrder(skuid, purchaseQuantity, ddlYYBM.SelectedValue, ddlStatus.SelectedValue,
                        frontendCaigoudanhao, offerid_1688, skuid_1688, sku1_1688, sku2_1688, jiage_1688);
                    
                    if (success)
                    {
                        BindData(); // 重新加载数据
                        lits.Text = $"采购单生成成功！";
                    }
                    else
                    {
                        lits.Text = "采购单生成失败：数据库更新失败";
                    }
                }
                catch (Exception coreEx)
                {
                    lits.Text = "采购单生成失败：" + coreEx.Message;
                }
            }
            catch (Exception ex)
            {
                lits.Text = "生成采购单失败：" + ex.Message;
            }
        }



        private char GetTodayLetter()
        {
            // 根据当前是月的第几天对26取余，获取对应字母
            int dayOfMonth = DateTime.Now.Day;
            int letterIndex = (dayOfMonth - 1) % 26; // 0-25
            return (char)('a' + letterIndex);
        }


        private bool ExecuteGeneratePurchaseOrder(string skuid, double purchaseQuantity, string yybm, string status,
            string frontendCaigoudanhao, string offerid_1688, string skuid_1688, string sku1_1688, string sku2_1688, string jiage_1688)
        {
            try
            {
                // 更新ShopeeOrder表，采购单号使用SQL逻辑实现条件更新（避免重复追加相同单号）
                string finalCaigoudanhao = frontendCaigoudanhao.Replace("'", "''");
                string updateShopeeOrderSql = $@"
                    UPDATE {TABLE_SHOPEE_ORDER} 
                    SET 
                        xucaigoushuliang = {purchaseQuantity},
                        YYBM = '{yybm.Replace("'", "''")}',
                        caigoudanzhuangtai = '{status.Replace("'", "''")}',
                        caigoudanhao = CASE 
                            WHEN caigoudanhao IS NULL OR caigoudanhao = '' 
                            THEN '{finalCaigoudanhao}'
                            WHEN caigoudanhao LIKE '{finalCaigoudanhao}%'
                                 OR caigoudanhao LIKE '%|{finalCaigoudanhao}%'
                            THEN caigoudanhao  -- 单号已存在，不更新
                            ELSE caigoudanhao + '|{finalCaigoudanhao}'
                        END
                    WHERE SKUID = '{skuid.Replace("'", "''")}'";
                
                bool success1 = access_sql.ExecSql(updateShopeeOrderSql);

                // 使用MERGE语法插入/更新caigoudan表
                string skuidValue = string.IsNullOrWhiteSpace(skuid_1688) ? "NULL" : $"'{skuid_1688.Replace("'", "''")}'";
                string mergeCaigoudanSql = $@"
                    MERGE {TABLE_CAIGOUDAN} AS target
                    USING (SELECT 
                        '{frontendCaigoudanhao.Replace("'", "''")}' as caigoudanhao, 
                        '{skuid.Replace("'", "''")}' as skuid_id, 
                        {skuidValue} as skuid,
                        {purchaseQuantity} as xucaigoushuliang,
                        '{yybm.Replace("'", "''")}' as YYBM,
                        '{status.Replace("'", "''")}' as status,
                        '{offerid_1688.Replace("'", "''")}' as offerid,
                        '{sku1_1688.Replace("'", "''")}' as sku1,
                        '{sku2_1688.Replace("'", "''")}' as sku2,
                        {jiage_1688} as danjia
                    ) AS source
                    ON target.caigoudanhao = source.caigoudanhao AND 
                       target.skuid_id = source.skuid_id
                    WHEN MATCHED THEN
                        UPDATE SET 
                            xucaigoushuliang = source.xucaigoushuliang,
                            YYBM = source.YYBM,
                            skuid = source.skuid,
                            status = source.status,
                            offerid = source.offerid,
                            sku1 = source.sku1,
                            sku2 = source.sku2,
                            danjia = source.danjia,
                            update_time = GETDATE()
                    WHEN NOT MATCHED THEN
                        INSERT (xucaigoushuliang, skuid_id, YYBM, caigoudanhao, status, offerid, skuid, sku1, sku2, danjia,update_time,upload_time)
                        VALUES (source.xucaigoushuliang, source.skuid_id, source.YYBM, source.caigoudanhao, source.status, 
                                source.offerid, source.skuid, source.sku1, source.sku2, source.danjia,GETDATE(),GETDATE());";

                bool success2 = access_sql.ExecSql(mergeCaigoudanSql);
                
                return success1 && success2;
            }
            catch (Exception ex)
            {
                // 在批量操作时，不直接设置lits.Text，而是抛出异常让调用方处理
                throw new Exception("数据库操作失败：" + ex.Message);
            }
        }


        private string GenerateCaigoudanhao(string offerid)
        {
            string today = DateTime.Now.ToString("yyyyMMdd");
            char letter = GetTodayLetter();
            return $"{offerid}{letter}{today}";
        }


        private string ValidateAndSetPurchaseStatus(string currentStatus)
        {
            if (string.IsNullOrWhiteSpace(currentStatus) || !VALID_PURCHASE_STATUS.Contains(currentStatus.Trim()))
            {
                return DEFAULT_PURCHASE_STATUS;
            }
            return currentStatus.Trim();
        }

        private void QuickApplyBatch()
        {
            try
            {
                // 获取批量操作的下拉框控件
                DropDownList ddlBatchYYBM = null;
                DropDownList ddlBatchStatus = null;
                
                // 从Repeater的HeaderTemplate中查找控件
                if (rplb.Controls.Count > 0)
                {
                    // 找到HeaderTemplate控件
                    foreach (Control item in rplb.Controls)
                    {
                        RepeaterItem repeaterItem = item as RepeaterItem;
                        if (repeaterItem != null && repeaterItem.ItemType == ListItemType.Header)
                        {
                            ddlBatchYYBM = repeaterItem.FindControl("ddlBatchYYBM") as DropDownList;
                            ddlBatchStatus = repeaterItem.FindControl("ddlBatchStatus") as DropDownList;
                            break;
                        }
                    }
                }
                
                if (ddlBatchYYBM == null)
                {
                    lits.Text = "找不到批量运营编码下拉框控件！请确保数据已加载。";
                    return;
                }
                
                if (ddlBatchStatus == null)
                {
                    lits.Text = "找不到批量采购单状态下拉框控件！请确保数据已加载。";
                    return;
                }
                
                string batchYYBM = ddlBatchYYBM.SelectedValue;
                string batchStatus = ddlBatchStatus.SelectedValue;
                
                if (string.IsNullOrWhiteSpace(batchYYBM))
                {
                    lits.Text = "请选择批量设置的运营编码！";
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(batchStatus))
                {
                    lits.Text = "请选择批量设置的采购单状态！";
                    return;
                }
                
                // 验证批量状态是否有效
                string validatedBatchStatus = ValidateAndSetPurchaseStatus(batchStatus);
                
                int updatedCount = 0;
                
                // 遍历 Repeater 中的所有项
                foreach (RepeaterItem item in rplb.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        // 查找复选框
                        CheckBox chkItem = item.FindControl("chkItem") as CheckBox;
                        
                        if (chkItem != null && chkItem.Checked)
                        {
                            // 查找当前行的运营编码和状态下拉框
                            DropDownList ddlYYBM = item.FindControl("ddlYYBM") as DropDownList;
                            DropDownList ddlStatus = item.FindControl("ddlStatus") as DropDownList;
                            
                            if (ddlYYBM != null && ddlStatus != null)
                            {
                                // 更新运营编码
                                if (ddlYYBM.Items.FindByValue(batchYYBM) != null)
                                {
                                    ddlYYBM.SelectedValue = batchYYBM;
                                }
                                else
                                {
                                    // 如果选项不存在，添加新选项
                                    ddlYYBM.Items.Add(new ListItem(batchYYBM, batchYYBM));
                                    ddlYYBM.SelectedValue = batchYYBM;
                                }
                                
                                // 更新采购单状态
                                if (ddlStatus.Items.FindByValue(validatedBatchStatus) != null)
                                {
                                    ddlStatus.SelectedValue = validatedBatchStatus;
                                }
                                else
                                {
                                    // 如果选项不存在，添加新选项
                                    ddlStatus.Items.Add(new ListItem(validatedBatchStatus, validatedBatchStatus));
                                    ddlStatus.SelectedValue = validatedBatchStatus;
                                }
                                
                                updatedCount++;
                            }
                        }
                    }
                }
                
                if (updatedCount > 0)
                {
                    lits.Text = $"批量应用成功！已更新 {updatedCount} 行数据。运营编码：{batchYYBM}，采购单状态：{validatedBatchStatus}";
                }
                else
                {
                    lits.Text = "没有选中任何行进行批量操作！";
                }
            }
            catch (Exception ex)
            {
                lits.Text = "批量应用失败：" + ex.Message;
            }
        }

        protected void btnGenerateAll_Click(object sender, EventArgs e)
        {
            // 生成当前页面所有需要采购的SKU的采购单
            try
            {
                int successCount = 0;
                int totalCount = 0;
                List<string> failedItems = new List<string>();

                foreach (RepeaterItem item in rplb.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        totalCount++;
                        
                        try
                        {
                            // 获取必要的控件
                            TextBox txtPurchaseQuantity = item.FindControl("txtPurchaseQuantity") as TextBox;
                            DropDownList ddlYYBM = item.FindControl("ddlYYBM") as DropDownList;
                            DropDownList ddlStatus = item.FindControl("ddlStatus") as DropDownList;
                            
                            // 从隐藏字段获取数据
                            HiddenField hidSkuid = item.FindControl("hidSkuid") as HiddenField;
                            HiddenField hidCaigoudanhao = item.FindControl("hidCaigoudanhao") as HiddenField;
                            HiddenField hidOfferid1688 = item.FindControl("hidOfferid1688") as HiddenField;
                            HiddenField hidSkuid1688 = item.FindControl("hidSkuid1688") as HiddenField;
                            HiddenField hidSku11688 = item.FindControl("hidSku11688") as HiddenField;
                            HiddenField hidSku21688 = item.FindControl("hidSku21688") as HiddenField;
                            HiddenField hidJiage1688 = item.FindControl("hidJiage1688") as HiddenField;

                            // 验证控件和数据
                            if (txtPurchaseQuantity == null || ddlYYBM == null || ddlStatus == null ||
                                hidSkuid == null || hidCaigoudanhao == null || hidOfferid1688 == null || 
                                hidSkuid1688 == null || hidSku11688 == null || hidSku21688 == null || hidJiage1688 == null)
                            {
                                failedItems.Add($"第{totalCount}行：控件缺失");
                                continue;
                            }

                            double purchaseQuantity;
                            if (!double.TryParse(txtPurchaseQuantity.Text.Trim(), out purchaseQuantity) || purchaseQuantity <= 0)
                            {
                                failedItems.Add($"第{totalCount}行：需采购数量无效");
                                continue;
                            }

                            // 获取数据
                            string skuid = hidSkuid.Value?.Trim() ?? "";
                            string frontendCaigoudanhao = hidCaigoudanhao.Value?.Trim() ?? "";
                            string offerid_1688 = hidOfferid1688.Value?.Trim() ?? "";
                            string skuid_1688 = hidSkuid1688.Value?.Trim() ?? "";
                            string sku1_1688 = hidSku11688.Value?.Trim() ?? "";
                            string sku2_1688 = hidSku21688.Value?.Trim() ?? "";
                            string jiage_1688 = hidJiage1688.Value?.Trim() ?? "0";

                            if (string.IsNullOrWhiteSpace(skuid) || string.IsNullOrWhiteSpace(frontendCaigoudanhao) || 
                                string.IsNullOrWhiteSpace(offerid_1688) || string.IsNullOrWhiteSpace(skuid_1688))
                            {
                                failedItems.Add($"第{totalCount}行：关键数据缺失");
                                continue;
                            }

                            // 调用核心操作方法
                            try
                            {
                                bool success = ExecuteGeneratePurchaseOrder(skuid, purchaseQuantity, ddlYYBM.SelectedValue, ddlStatus.SelectedValue,
                                    frontendCaigoudanhao, offerid_1688, skuid_1688, sku1_1688, sku2_1688, jiage_1688);
                                
                                if (success)
                                {
                                    successCount++;
                                }
                                else
                                {
                                    failedItems.Add($"第{totalCount}行：数据库操作失败");
                                }
                            }
                            catch (Exception coreEx)
                            {
                                failedItems.Add($"第{totalCount}行：{coreEx.Message}");
                            }
                        }
                        catch (Exception itemEx)
                        {
                            failedItems.Add($"第{totalCount}行：{itemEx.Message}");
                        }
                    }
                }

                if (totalCount == 0)
                {
                    lits.Text = "当前页面没有可以生成采购单的记录！";
                }
                else
                {
                    string message = $"批量生成采购单完成！成功 {successCount} 条，共 {totalCount} 条";
                    if (failedItems.Count > 0)
                    {
                        message += $"<br/>失败原因：<br/>{string.Join("<br/>", failedItems)}";
                    }
                    if (successCount > 0)
                    {
                        BindData(); // 重新加载数据
                    }
                    lits.Text = message;
                }
            }
            catch (Exception ex)
            {
                lits.Text = "批量生成采购单失败：" + ex.Message;
            }
        }
    }
}