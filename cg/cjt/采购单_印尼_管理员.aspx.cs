using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
namespace WebApplication11.cg.cjt
{
    public partial class 采购单_印尼_管理员 : System.Web.UI.Page
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
                if (uid != "9")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }
        

        }
        private void Rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                TextBox txtPO = (TextBox)e.Item.FindControl("caigoudanhao");
                Literal litItemID = (Literal)e.Item.FindControl("销售链接ItemIDLiteral");


                    string newPO = DateTime.Now.ToString("yyMMdd") + litItemID.Text;
                    txtPO.Text = newPO;
                    row["采购单号"] = newPO; // 更新数据源
                
            }
        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public void bindzhy(String SJBM = "",string BName="",string status="", string needPurchase = "-1")
        {
            ViewState["SJBM"] = SJBM;
            ViewState["status"] = status;
            ViewState["BName"] = BName;
            ViewState["needPurchase"] = needPurchase;
            string safeSJBM = !string.IsNullOrEmpty(SJBM) ? SJBM.Replace("'", "''") : "";
            string safeBName = !string.IsNullOrEmpty(BName) ? BName.Replace("'", "''") : "";

            string safeStatus = status.Replace("'", "''");
            string whereCondition = "";
            whereCondition += " AND so.status IN ('Shipped', 'Delivered', 'Completed')";
            if (!string.IsNullOrEmpty(safeBName))
                whereCondition += " AND so.BName LIKE '%" + safeBName + "%'";
            if (!string.IsNullOrEmpty(safeSJBM))
                whereCondition += " AND so.SJBM = '" + safeSJBM + "'";

            if (!string.IsNullOrEmpty(status) && status != "-1")

                whereCondition += " AND so.caigoudanzhuangtai = '" + safeStatus + "'";


            string Condition = "";
            if (!string.IsNullOrEmpty(needPurchase) && needPurchase != "-1")
            {
                if (needPurchase == "1") // 需采购
                {
                    Condition += " AND (COALESCE([该SKU需维持在仓在途数量], 0) "
                              + " - COALESCE([实际在仓数量], 0) "
                              + " - COALESCE([实际在途总数], 0) > 0)";
                }
                else if (needPurchase == "0") // 不需采购
                {
                    Condition += " AND (COALESCE([该SKU需维持在仓在途数量], 0) "
                              + " - COALESCE([实际在仓数量], 0) "
                              + " - COALESCE([实际在途总数], 0) <= 0)";
                }
            }

            string countSql = string.Format(@"
    SELECT COUNT(DISTINCT so.skuid)  
    FROM ShopeeOrder so
    LEFT JOIN caiwu c ON so.ItemID = c.rucangITEMID
    WHERE 1=1 {0}", whereCondition);
      int totalRecords = Convert.ToInt32(access_sql.ExecInt2(countSql));
            TotalPagesCount = (totalRecords + PageSize - 1) / PageSize;

            // 分页查询
            int startRow = (CurrentPageIndex - 1) * PageSize + 1;
            int endRow = CurrentPageIndex * PageSize;
            string sql = @"
WITH InTransitData AS (
    SELECT 
        pw.rucangSKUID,
        SUM(
            COALESCE(CAST(TRY_CAST(NULLIF(REPLACE(tc.putianfachushuliang, '.', ''), '') AS FLOAT) AS INT), 0) + 
            COALESCE(CAST(TRY_CAST(NULLIF(REPLACE(tc.putianweifachushuliang, '.', ''), '') AS FLOAT) AS INT), 0)
        ) AS [实际在途数量]
    FROM touchenwuliu AS tc 
    LEFT JOIN Purchase_Sales_Warehouse pw
        ON tc.haiwaicangxitongbianma = pw.haiwaicangxitongbianma
    GROUP BY pw.rucangSKUID
),
SKU1688 AS (
    SELECT 
        psw.rucangSKUID,
        MAX('https://detail.1688.com/offer/' + s1.itemid + '.html') AS [1688采购链接],
        MAX(s1.sku1) AS [1688SKU1],
        MAX(s1.sku2) AS [1688SKU2],
        MAX(s1.sku_price) AS [1688价格]
    FROM Purchase_Sales_Warehouse psw 
    LEFT JOIN S1688ProSKU s1 ON psw.SkuID_1688 = s1.skuid
    GROUP BY psw.rucangSKUID
),
Item14DayOrders AS (
    SELECT 
        so.ItemID,
        COUNT(DISTINCT so.order_id) AS [该ItemID14天总销量] 
    FROM ShopeeOrder so
    WHERE so.order_date >= DATEADD(day, -14, CAST(GETDATE() AS DATE))
      AND so.order_date <= CAST(GETDATE() AS DATE)
    GROUP BY so.ItemID
),
MainData AS (
    SELECT 
        ROW_NUMBER() OVER (ORDER BY so.skuid) AS RowNum,
        MAX(so.BName) AS [浏览器店铺名],
        MAX(so.SJBM) AS [商家编码],
        MAX(so.pname) AS [销售链接产品标题],
        MAX(cw.huopinbiaoti) AS [货源链接产品标题],
        MAX(so.ItemID) AS [销售链接ItemID],
        so.skuid AS [销售链接SKU],
        MAX(psw.haiwaicangxitongbianma) AS [系统编码],
        MAX(so.pimage) AS [SKU图片],
        MAX(so.xucaigoushuliang) as [实际需采购数量],
        MAX('https://detail.1688.com/offer/' +psw.OfferID_1688+ '.html') as [1688产品链接],
        COUNT(DISTINCT CASE 
            WHEN so.order_date >= DATEADD(day, -7, CAST(GETDATE() AS DATE)) 
                 AND so.order_date <= CAST(GETDATE() AS DATE) 
            THEN so.buyer_id 
        END) AS [该SKU7天总销量],
        COUNT(DISTINCT CASE 
            WHEN so.order_date >= DATEADD(day, -14, CAST(GETDATE() AS DATE)) 
                 AND so.order_date <= CAST(GETDATE() AS DATE) 
            THEN so.buyer_id 
        END) AS [该SKU14天总销量],
        COUNT(DISTINCT CASE 
            WHEN so.order_date >= DATEADD(day, -30, CAST(GETDATE() AS DATE)) 
                 AND so.order_date <= CAST(GETDATE() AS DATE) 
            THEN so.buyer_id 
        END) AS [该SKU30天总销量],
        MAX(ss.stock) AS [实际在仓数量],
        MAX(it.[实际在途数量]) AS [实际在途总数],
        CAST(
            CASE 
                WHEN COUNT(DISTINCT CASE 
                            WHEN so.order_date >= DATEADD(day, -30, CAST(GETDATE() AS DATE)) 
                                 AND so.order_date <= CAST(GETDATE() AS DATE) 
                            THEN so.buyer_id 
                        END) >= 60 
                THEN COUNT(DISTINCT CASE 
                            WHEN so.order_date >= DATEADD(day, -30, CAST(GETDATE() AS DATE)) 
                                 AND so.order_date <= CAST(GETDATE() AS DATE) 
                            THEN so.buyer_id 
                        END) * 1.5
                WHEN COUNT(DISTINCT CASE 
                            WHEN so.order_date >= DATEADD(day, -30, CAST(GETDATE() AS DATE)) 
                                 AND so.order_date <= CAST(GETDATE() AS DATE) 
                            THEN so.buyer_id 
                        END) >= 30 
                THEN COUNT(DISTINCT CASE 
                            WHEN so.order_date >= DATEADD(day, -30, CAST(GETDATE() AS DATE)) 
                                 AND so.order_date <= CAST(GETDATE() AS DATE) 
                            THEN so.buyer_id 
                        END) * 1.2
                ELSE 
                    CASE 
                        WHEN (COUNT(DISTINCT CASE 
                                    WHEN so.order_date >= DATEADD(day, -7, CAST(GETDATE() AS DATE)) 
                                         AND so.order_date <= CAST(GETDATE() AS DATE) 
                                    THEN so.buyer_id 
                                END) * 4) >= COUNT(DISTINCT CASE 
                                    WHEN so.order_date >= DATEADD(day, -30, CAST(GETDATE() AS DATE)) 
                                         AND so.order_date <= CAST(GETDATE() AS DATE) 
                                    THEN so.buyer_id 
                                END)
                        THEN COUNT(DISTINCT CASE 
                                    WHEN so.order_date >= DATEADD(day, -7, CAST(GETDATE() AS DATE)) 
                                         AND so.order_date <= CAST(GETDATE() AS DATE) 
                                    THEN so.buyer_id 
                                END) * 4
                        ELSE COUNT(DISTINCT CASE 
                                    WHEN so.order_date >= DATEADD(day, -30, CAST(GETDATE() AS DATE)) 
                                         AND so.order_date <= CAST(GETDATE() AS DATE) 
                                    THEN so.buyer_id 
                                END)
                    END
            END 
        AS DECIMAL(18, 0)) AS [该SKU需维持在仓在途数量],
        MAX(s8.[1688采购链接]) AS [1688采购链接],
        MAX(COALESCE(so.Y_1688SKU1, s8.[1688SKU1])) AS [1688SKU1],
        MAX(COALESCE(so.Y_1688SKU2, s8.[1688SKU2])) AS [1688SKU2],
        MAX(COALESCE(so.Y_1688price, s8.[1688价格])) AS [1688价格],
        COALESCE(MAX(io.[该ItemID14天总销量]), 0) AS [该ItemID14天总销量],
        MAX(so.caigoudanhao) AS [采购单号],
        MAX(so.caigoudanzhuangtai) AS [采购单状态],
        MAX(so.YYBM) AS [运营编码]
    FROM ShopeeOrder so 
    LEFT JOIN Purchase_Sales_Warehouse psw ON so.skuid = psw.rucangSKUID
    LEFT JOIN caiwu cw ON psw.SkuID_1688 = cw.SKU_ID
    LEFT JOIN ShopeeStock ss ON so.skuid = ss.skuid
    LEFT JOIN InTransitData it ON so.skuid = it.rucangSKUID
    LEFT JOIN SKU1688 s8 ON so.skuid = s8.rucangSKUID
    LEFT JOIN Item14DayOrders io ON so.ItemID = io.ItemID  
    WHERE so.xiaoshouzhuangtai='在售' " + whereCondition + @" 
    GROUP BY so.skuid
)
SELECT 
    [浏览器店铺名], [商家编码], [销售链接产品标题], [货源链接产品标题],
    [销售链接ItemID], [销售链接SKU], [系统编码], [SKU图片],
    [该SKU7天总销量], [该SKU14天总销量], [该SKU30天总销量],
    [实际在仓数量], [实际在途总数], [该SKU需维持在仓在途数量],
    [1688产品链接], [1688SKU1], [1688SKU2], [1688价格],
    CAST(
        CASE 
            WHEN COALESCE([该SKU需维持在仓在途数量], 0) 
                - COALESCE([实际在仓数量], 0) 
                - COALESCE([实际在途总数], 0) < 0 
            THEN 0 
            ELSE COALESCE([该SKU需维持在仓在途数量], 0) 
                - COALESCE([实际在仓数量], 0) 
                - COALESCE([实际在途总数], 0)
        END 
    AS INT) AS [需采购数量],
    [采购单号], [采购单状态], [运营编码],
    SUM(CAST(
        CASE 
            WHEN COALESCE([该SKU需维持在仓在途数量], 0) 
                - COALESCE([实际在仓数量], 0) 
                - COALESCE([实际在途总数], 0) < 0 
            THEN 0 
            ELSE COALESCE([该SKU需维持在仓在途数量], 0) 
                - COALESCE([实际在仓数量], 0) 
                - COALESCE([实际在途总数], 0)
        END 
    AS INT)) OVER (PARTITION BY [销售链接ItemID]) AS ItemTotalPurchaseQty
FROM MainData
WHERE RowNum BETWEEN " + startRow + " AND " + endRow + Condition + @"
ORDER BY ItemTotalPurchaseQty DESC, [销售链接ItemID] ASC, [销售链接SKU] ASC";


            DataSet ds = access_sql.GreatDs(sql);

            if (access_sql.yzTable(ds))
            {


                DataTable dt = ds.Tables[0];
                // 新增：为采购单号为空的数据生成采购单号
                foreach (DataRow row in dt.Rows)
                {
             
                        string itemId = row["销售链接ItemID"].ToString();
                        row["采购单号"] = GeneratePurchaseOrderNumber(itemId);
            
                }
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
            string sjbm = txtsjbm.Text.Trim();
            string status = ddlStatus.SelectedValue;
            string BName = txtBName.Text.Trim();

            string needPurchase = ddlNeedPurchase.SelectedValue; // 获取新筛选条件

            bindzhy(sjbm, BName, status, needPurchase);
               
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
        protected void Button1_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            string sjbm = txtsjbm.Text.Trim();
            string BName = txtBName.Text.Trim();
            string status = ddlStatus.SelectedValue;

            string needPurchase = ddlNeedPurchase.SelectedValue; // 获取新筛选条件

            bindzhy(SJBM: sjbm, BName: BName, status: status, needPurchase: needPurchase);


        }
        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "qr")
            {
                lits.Text = "";
                

                ulong 销售链接SKU = Convert.ToUInt64(e.CommandArgument);
                TextBox caigoudanhao = e.Item.FindControl("caigoudanhao") as TextBox;
                if (caigoudanhao == null) // 重点：添加空值检查
                {
                    lits.Text = "错误：未找到采购单号输入框。";
                    return;
                }

            

                TextBox xucaigoushuliang = e.Item.FindControl("xucaigoushuliang") as TextBox;
                TextBox sku1 = e.Item.FindControl("sku1") as TextBox;
                TextBox sku2 = e.Item.FindControl("sku2") as TextBox;
                TextBox price = e.Item.FindControl("price") as TextBox;
                DropDownList caigoudanzhuangtai = e.Item.FindControl("caigoudanzhuangtai") as DropDownList; 
                
                 DropDownList YYBM = e.Item.FindControl("YYBM") as DropDownList;
                Literal Y_1688url = (Literal)e.Item.FindControl("URL");
                string url = Y_1688url.Text;

                Literal skuid = (Literal)e.Item.FindControl("销售链接SKU");
                string id = skuid.Text;

                Literal skutupian = (Literal)e.Item.FindControl("SKU图片");
                string tupian = skutupian.Text;


                Literal huopinbiaoti = (Literal)e.Item.FindControl("货源链接产品标题");
                string biaoti = huopinbiaoti.Text;


                string existingPOs = GetExistingPOsForSKU(销售链接SKU);

                // 验证日期差
                string errorMsg;
                string newPurchaseOrderNumber = caigoudanhao.Text.Trim().Replace("'", "''");
                // 获取该SKU已有的采购单号

                Literal litItemID = e.Item.FindControl("销售链接ItemID") as Literal;
                string itemId = litItemID.Text;
                if (YYBM == null || string.IsNullOrEmpty(YYBM.SelectedValue))
                {
                    lits.Text = "错误：请选择运营编码";
                    return; // 阻止保存操作
                }
                if (!string.IsNullOrEmpty(YYBM.SelectedValue)
                         && caigoudanzhuangtai.SelectedValue == "待生成采购单")
                {
                    caigoudanzhuangtai.SelectedValue = "已生成采购单待采购";
                    if (!ValidatePurchaseOrderDate(existingPOs, newPurchaseOrderNumber, out errorMsg))
                    {
                        lits.Text = errorMsg;

                        return; // 验证不通过，不执行保存
                    }
                

                    //if (!ValidateItemTotalPurchaseQty(itemId))
                    //{
                    //    lits.Text = "错误：该ItemID下所有SKU需采购数量总和必须≥5";
                    //    return; // 终止保存
                    //}
                }




                // 3. 构造插入参数（列名与 Purchase_YN_User 表字段严格匹配）
                string[] columns = {
            "skuid", "caigoudanhao", "xucaigoushuliang",
            "Y_1688sku1", "Y_1688sku2", "Y_1688price",
            "caigoudanzhuangtai", "YYBM","Y_1688url","pimage","huopinbiaoti"
        };
                object[] values = {
            id,
            caigoudanhao.Text.Trim(),          // 采购单号
            xucaigoushuliang.Text.Trim(),      // 需采购数量
            sku1.Text.Trim(),                  // 1688SKU1
            sku2.Text.Trim(),                  // 1688SKU2
            price.Text.Trim(),                 // 1688价格
            caigoudanzhuangtai.SelectedValue,  // 采购单状态
            YYBM.SelectedValue ,                // 运营编码
            url,tupian,biaoti
        };

                access_sql.T_Insert_ExecSqls(columns, values, "Purchase_YN_User");

                if (access_sql.T_Update_ExecSql(new string[] {  "caigoudanzhuangtai", "YYBM", "xucaigoushuliang" , "Y_1688sku1", "Y_1688sku2", "Y_1688price" },
                  new object[] {  caigoudanzhuangtai.SelectedValue, YYBM.SelectedValue, xucaigoushuliang.Text.Trim().Replace("'", "''"),
                  sku1.Text.Trim().Replace("'", "''"), sku2.Text.Trim().Replace("'", "''"), price.Text.Trim().Replace("'", "''")
                  },
                  "ShopeeOrder",
                 "skuid='" + 销售链接SKU + "'") > 0)
                {

                    AppendPurchaseOrderNumber(销售链接SKU, caigoudanhao.Text.Trim().Replace("'", "''")); 
                    bindzhy(ViewState["SJBM"] as string, ViewState["BName"] as string,
                        ViewState["status"] as string,ViewState["needPurchase"] as string);
                    lits.Text = "skuid:" + 销售链接SKU + "更新成功";
                }
            }
            else if(e.CommandName== "ApplyBatch")
            {
                ApplyBatchUpdate();
            }

        }


        public void clzy()
        {
            List<string> errorMessages = new List<string>();
            Dictionary<string, bool> validatedItems = new Dictionary<string, bool>();
            int successCount = 0;

            for (int i = 0; i < rplb.Items.Count; i++)
            {
                Literal 销售链接SKU = (Literal)rplb.Items[i].FindControl("销售链接SKU");
                String id = 销售链接SKU.Text;
                TextBox caigoudanhao = rplb.Items[i].FindControl("caigoudanhao") as TextBox;
                TextBox xucaigoushuliang = rplb.Items[i].FindControl("xucaigoushuliang") as TextBox;
                TextBox sku1 = rplb.Items[i].FindControl("sku1") as TextBox;
                TextBox sku2 = rplb.Items[i].FindControl("sku2") as TextBox;
                TextBox price = rplb.Items[i].FindControl("price") as TextBox;
                DropDownList caigoudanzhuangtai = rplb.Items[i].FindControl("caigoudanzhuangtai") as DropDownList;
                DropDownList YYBM = rplb.Items[i].FindControl("YYBM") as DropDownList;
                Literal Y_1688url = (Literal)rplb.Items[i].FindControl("URL");
                string url = Y_1688url.Text;

                Literal skutupian = (Literal)rplb.Items[i].FindControl("SKU图片");
                string tupian = skutupian.Text;
                string newPO = caigoudanhao.Text.Trim().Replace("'", "''");
                string existingPOs = GetExistingPOsForSKU(Convert.ToUInt64(id));
                Literal litItemID = rplb.Items[i].FindControl("销售链接ItemID") as Literal;
                string itemId = litItemID.Text;
                Literal huopinbiaoti = rplb.Items[i].FindControl("货源链接产品标题") as Literal;
                string biaoti = huopinbiaoti.Text;
                if (!string.IsNullOrEmpty(YYBM.SelectedValue) &&
                    caigoudanzhuangtai.SelectedValue == "待生成采购单")
                {
                    caigoudanzhuangtai.SelectedValue = "已生成采购单待采购";
                    string errorMsg;
                    if (!ValidatePurchaseOrderDate(existingPOs, newPO, out errorMsg))
                    {
                        // 修改点：使用字符串拼接代替字符串插值
                        errorMessages.Add("SKU " + id + ": " + errorMsg);
                        continue;
                    }
                    // 每个ItemID仅验证一次
                    //if (!validatedItems.ContainsKey(itemId))
                    //{
                    //    if (!ValidateItemTotalPurchaseQty(itemId))
                    //    {
                    //        errorMessages.Add("错误：ItemID " + itemId + " 需采购总量不足5");
                    //        continue; // 终止批量保存
                    //    }
                    //    validatedItems[itemId] = true;
                    //}
                }
                if (YYBM == null || string.IsNullOrEmpty(YYBM.SelectedValue))
                {
                    errorMessages.Add("错误：第" + (i + 1).ToString() + "行 运营编码未选择");
                    continue; // 阻止保存操作
                }





                access_sql.T_Update_ExecSql(
                    new string[] { "caigoudanzhuangtai", "YYBM", "xucaigoushuliang", "Y_1688sku1", "Y_1688sku2", "Y_1688price" },
                    new object[] {
                caigoudanzhuangtai.SelectedValue,
                YYBM.SelectedValue,
                xucaigoushuliang.Text.Trim().Replace("'", "''"),
                sku1.Text.Trim().Replace("'", "''"),
                sku2.Text.Trim().Replace("'", "''"),
                price.Text.Trim().Replace("'", "''")
                    },
                    "ShopeeOrder",
                    "skuid='" + id + "'");

                AppendPurchaseOrderNumber(Convert.ToUInt64(id), newPO);


                // 3. 构造插入参数（列名与 Purchase_YN_User 表字段严格匹配）
                string[] columns = {
            "skuid", "caigoudanhao", "xucaigoushuliang",
            "Y_1688sku1", "Y_1688sku2", "Y_1688price",
            "caigoudanzhuangtai", "YYBM","Y_1688url","pimage","huopinbiaoti"
        };
                object[] values = {
            id,
            caigoudanhao.Text.Trim(),          // 采购单号
            xucaigoushuliang.Text.Trim(),      // 需采购数量
            sku1.Text.Trim(),                  // 1688SKU1
            sku2.Text.Trim(),                  // 1688SKU2
            price.Text.Trim(),                 // 1688价格
            caigoudanzhuangtai.SelectedValue,  // 采购单状态
            YYBM.SelectedValue ,                // 运营编码
            url,tupian,biaoti
        };

                bool insertSuccess = access_sql.T_Insert_ExecSqls(columns, values, "Purchase_YN_User");
                if (insertSuccess)
                {
                    successCount++;
                }
            }

            if (errorMessages.Count > 0)
            {
                lits.Text = string.Join("<br/>", errorMessages);
            }
            else
            {
                bindzhy(ViewState["SJBM"] as string, ViewState["BName"] as string,
                       ViewState["status"] as string, ViewState["needPurchase"] as string);
                lits.Text = "更新成功" + successCount + "个";
            }
        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            clzy();
        }

        private bool ValidateItemTotalPurchaseQty(string itemId)
        {
            int totalQty = 0;

            foreach (RepeaterItem item in rplb.Items)
            {
                if (item.ItemType == ListItemType.Item ||
                    item.ItemType == ListItemType.AlternatingItem)
                {
                    Literal litItemID = item.FindControl("销售链接ItemID") as Literal;
                    if (litItemID != null && litItemID.Text == itemId)
                    {
                        // 修复这里：使用正确的控件ID "xucaigoushuliang"
                        TextBox txtQty = item.FindControl("xucaigoushuliang") as TextBox;
                        if (txtQty != null && !string.IsNullOrEmpty(txtQty.Text))
                        {
                            int qty;
                            if (int.TryParse(txtQty.Text, out qty))
                            {
                                totalQty += qty;
                            }
                        }
                    }
                }
            }

            return totalQty >= 5;
        }
        // 修改后的更新逻辑 - 追加而非覆盖
        private void AppendPurchaseOrderNumber(ulong skuID, string newPurchaseOrderNumber)
        {
            // 1. 查询原有采购单号
            string sql = "SELECT caigoudanhao FROM ShopeeOrder WHERE skuid = '" + skuID + "'";
            DataSet ds = access_sql.GreatDs(sql);
            string oldValue = "";
            if (access_sql.yzTable(ds) && ds.Tables[0].Rows.Count > 0)
            {
                oldValue = ds.Tables[0].Rows[0]["caigoudanhao"].ToString().Trim();
            }

            // 2. 拼接新值（新值在前）
            string newValue = string.IsNullOrEmpty(oldValue)
                ? newPurchaseOrderNumber
                : newPurchaseOrderNumber + ";" + oldValue; // 用分号分隔历史记录

            // 3. 更新数据库
            access_sql.T_Update_ExecSql(
                new string[] { "caigoudanhao" },
                new object[] { newValue },
                "ShopeeOrder",
                "skuid='" + skuID + "'"
            );
        }
        public string GeneratePurchaseOrderNumber(string itemId)
                    {
                        
                        string timePart = DateTime.Now.ToString("yyMMdd");
                        // 拼接销售链接ItemID
                        return timePart + itemId;
                    }

        private void ApplyBatchUpdate()
        {

            DropDownList ddlBatchStatus = rplb.Controls[0] .FindControl("ddlBatchStatus") as DropDownList;

            if (ddlBatchStatus == null || string.IsNullOrEmpty(ddlBatchStatus.SelectedValue))
            {
                lits.Text = "请选择要应用的运营编码";
                return;
            }

            bool anyChecked = false;
            int updatedCount = 0;
            string batchStatus = ddlBatchStatus.SelectedValue;

            // 遍历所有行，只处理选中的行
            foreach (RepeaterItem item in rplb.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox chk = item.FindControl("chkItem") as CheckBox;
                    Literal litSKU = item.FindControl("销售链接SKU") as Literal; // 使用Literal控件

                    if (chk != null && chk.Checked && litSKU != null)
                    {
                        anyChecked = true;
                        ulong skuID = Convert.ToUInt64(litSKU.Text); // 从Literal获取SKU值

                        string condition = "skuid='" + skuID+"'";

                        if (access_sql.T_Update_ExecSql(
                            new string[] { "YYBM" },
                            new object[] { batchStatus },
                            "ShopeeOrder",
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
                ViewState["SJBM"] as string,
                ViewState["BName"] as string,
                ViewState["status"] as string,
                ViewState["needPurchase"] as string
            );
        }
        protected string GetDisplayQuantity(object calcQty)
        {
            // 无实际值时使用计算值
            int needValue = Convert.ToInt32(calcQty);
            return needValue < 0 ? "" : needValue.ToString();
        }
        private bool ValidatePurchaseOrderDate(string existingPOs, string newPO, out string errorMessage)
        {
            errorMessage = "";
            if (string.IsNullOrEmpty(existingPOs))
                return true;

            // 获取最近一次保存的采购单号（分号分隔的第一个）
            string[] poList = existingPOs.Split(';');
            string lastPO = poList[0];

            // 检查采购单号格式是否正确（至少6位日期）
            if (lastPO.Length < 6 || newPO.Length < 6)
            {
                errorMessage = "采购单号格式错误";
                return false;
            }

            string lastDateStr = lastPO.Substring(0, 6);
            string newDateStr = newPO.Substring(0, 6);

            // 解析日期
            DateTime lastDate;
            DateTime newDate;
            if (!DateTime.TryParseExact(lastDateStr, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out lastDate) ||
                !DateTime.TryParseExact(newDateStr, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out newDate))
            {
                errorMessage = "采购单号日期解析错误";
                return false;
            }

            // 计算日期差（取绝对值）[1,7](@ref)
            TimeSpan difference = newDate - lastDate;
            int daysDiff = Math.Abs(difference.Days);

            if (daysDiff < 7)
            {
                errorMessage = "新采购单日期与上一次仅差" + daysDiff + "天，需间隔至少7天";
                return false;
            }

            return true;
        }
        private string GetExistingPOsForSKU(ulong skuID)
        {
            string sql = "SELECT caigoudanhao FROM ShopeeOrder WHERE skuid = '" + skuID + "'";
            DataSet ds = access_sql.GreatDs(sql);
            return (access_sql.yzTable(ds) && ds.Tables[0].Rows.Count > 0)
                ? ds.Tables[0].Rows[0]["caigoudanhao"].ToString()
                : string.Empty;
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
            bindzhy(
                ViewState["SJBM"] as string,
                ViewState["BName"] as string,
                ViewState["status"] as string,
                ViewState["needPurchase"] as string
            );
        }
    }
}