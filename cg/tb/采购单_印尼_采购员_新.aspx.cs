using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using System.Web;

namespace WebApplication11.cg.tb
{
    public partial class 采购单_印尼_采购员_新 : System.Web.UI.Page
    {
        public string u = "";
        public string p = "";
        public string uid = "";

        // 保存数据模型
        private class ItemSaveData
        {
            public string Caigoudanhao { get; set; }
            public string SKUID_ID { get; set; }
            public decimal Shijicaigoushuliang { get; set; }
            public string Status { get; set; }
            public string DingDanBianHao { get; set; }
            public string Beizhu { get; set; }
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }
        }
        
        // 表名变量
        private const string TABLE_CAIGOUDAN = "caigoudan";
        private const string TABLE_SHOPEE_ORDER = "ShopeeOrder";
        private const string TABLE_PURCHASE_SALES_WAREHOUSE = "Purchase_Sales_Warehouse";
        private const string TABLE_S1688_ORDER = "S1688Order";
        
        // 分页相关属性
        public int CurrentPage
        {
            get { return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1; }
            set { ViewState["CurrentPage"] = value; }
        }

        public int PageSize = 1; // 1页只展示1个caigoudanhao下的数据
        
        public int TotalPages
        {
            get { return ViewState["TotalPages"] != null ? (int)ViewState["TotalPages"] : 0; }
            set { ViewState["TotalPages"] = value; }
        }

        public string CurrentCaigoudanhao
        {
            get { return ViewState["CurrentCaigoudanhao"] != null ? ViewState["CurrentCaigoudanhao"].ToString() : ""; }
            set { ViewState["CurrentCaigoudanhao"] = value; }
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
                
                // 检查用户权限（采购员权限）
                if (uid != "8" && uid != "9" && uid != "18" && uid != "19" && uid != "12" && uid != "6")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

            if (!IsPostBack)
            {
                // 同步caigoudan表数据
                SynchronizeCaigoudanData();
            }
        }

        private void SynchronizeCaigoudanData()
        {
            try
            {
                string syncSql = $@"
                    WITH SyncData AS (
                        SELECT 
                            c.caigoudanhao,
                            c.SKUID_ID,
                            MAX(so.pimage) as pimage,
                            MAX(so.itemid) as itemid,
                            MAX(psw.offerid_1688) as offerid_1688,
                            MAX(psw.skuid_1688) as skuid_1688,
                            MAX(s1688.sku1) as sku1,
                            MAX(s1688.sku2) as sku2,
                            MAX(s1688.danjia) as danjia
                        FROM {TABLE_CAIGOUDAN} c
                        LEFT JOIN {TABLE_SHOPEE_ORDER} so ON c.SKUID_ID = so.skuid 
                            AND ('|' + so.caigoudanhao + '|') LIKE '%|' + c.caigoudanhao + '|%'
                        LEFT JOIN {TABLE_PURCHASE_SALES_WAREHOUSE} psw ON so.skuid = psw.rucangitemid
                        LEFT JOIN {TABLE_S1688_ORDER} s1688 ON psw.skuid_1688 = s1688.skuid
                        WHERE so.skuid IS NOT NULL
                        GROUP BY c.caigoudanhao, c.SKUID_ID
                    )
                    UPDATE c
                    SET 
                        c.sku_img = ISNULL(sd.pimage, c.sku_img),
                        c.ItemID_ID = ISNULL(sd.itemid, c.ItemID_ID),
                        c.offerid = ISNULL(sd.offerid_1688, c.offerid),
                        c.skuid = ISNULL(sd.skuid_1688, c.skuid),
                        c.sku1 = ISNULL(sd.sku1, c.sku1),
                        c.sku2 = ISNULL(sd.sku2, c.sku2),
                        c.danjia = ISNULL(sd.danjia, c.danjia)
                    FROM {TABLE_CAIGOUDAN} c
                    INNER JOIN SyncData sd ON c.caigoudanhao = sd.caigoudanhao AND c.SKUID_ID = sd.SKUID_ID";

                bool result = access_sql.ExecSql(syncSql);
                
                if (result)
                {
                    lits.Text = "数据同步成功！请输入运营编码进行查询";
                }
                else
                {
                    lits.Text = "数据同步失败，请联系管理员！";
                }
            }
            catch (Exception ex)
            {
                lits.Text = $"数据同步异常：{ex.Message}";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtYYBM.Text))
            {
                lits.Text = "运营编码为必填项！";
                return;
            }

            CurrentPage = 1;
            LoadCaigoudanhaosByPurchaseQuantity();
            BindData();
        }

        private void LoadCaigoudanhaosByPurchaseQuantity()
        {
            try
            {
                string yybm = txtYYBM.Text.Trim();
                string status = ddlSearchStatus.SelectedValue;
                string orderNo = txt1688OrderNo.Text.Trim();
                
                string whereCondition = $"c.YYBM = '{yybm}'";
                
                if (!string.IsNullOrEmpty(status))
                {
                    whereCondition += $" AND ISNULL(c.status, '需采购') = '{status}'";
                }
                
                if (!string.IsNullOrEmpty(orderNo))
                {
                    whereCondition += $" AND c.DingDanBianHao LIKE '%{orderNo}%'";
                }

                string sql = $@"
                    SELECT c.caigoudanhao
                    FROM {TABLE_CAIGOUDAN} c
                    WHERE {whereCondition}
                    GROUP BY c.caigoudanhao
                    ORDER BY SUM(ISNULL(TRY_CAST(c.xucaigoushuliang AS DECIMAL(18,2)), 0)) DESC";

                DataSet ds = access_sql.GreatDs(sql);
                DataTable dt = ds.Tables[0];
                
                var caigoudanhaoList = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    caigoudanhaoList.Add(row["caigoudanhao"].ToString());
                }
                
                ViewState["CaigoudanhaoList"] = caigoudanhaoList;
                TotalPages = caigoudanhaoList.Count;
                
                if (TotalPages > 0 && CurrentPage <= TotalPages)
                {
                    CurrentCaigoudanhao = caigoudanhaoList[CurrentPage - 1];
                }
                else
                {
                    CurrentCaigoudanhao = "";
                }
            }
            catch (Exception ex)
            {
                lits.Text = "查询采购单号失败：" + ex.Message;
            }
        }

        private void BindData()
        {
            // 清空提示信息
            lits.Text = "";
            if (string.IsNullOrEmpty(CurrentCaigoudanhao))
            {
                rplb.DataSource = null;
                rplb.DataBind();
                lits.Text = "未找到符合条件的数据";
                UpdatePagingInfo();
                return;
            }

            LoadDataForCurrentCaigoudanhao();
            UpdatePagingInfo();
        }

        private void LoadDataForCurrentCaigoudanhao()
        {
            try
            {
                string sql = $@"
                    SELECT 
                        c.caigoudanhao,
                        c.ItemID_ID,
                        c.SKUID_ID,
                        c.offerid,
                        c.skuid,
                        c.sku_img,
                        c.sku1,
                        c.sku2,
                        c.danjia,
                        c.xucaigoushuliang,
                        c.shijicaigoushuliang,
                        ISNULL(c.status, '需采购') as status,
                        c.DingDanBianHao,
                        c.beizhu
                    FROM {TABLE_CAIGOUDAN} c
                    WHERE c.caigoudanhao = '{CurrentCaigoudanhao}'
                    ORDER BY ISNULL(TRY_CAST(c.xucaigoushuliang AS DECIMAL(18,2)), 0) DESC";

                DataSet ds = access_sql.GreatDs(sql);
                DataTable dt = ds.Tables[0];
                
                rplb.DataSource = dt;
                rplb.DataBind();
            }
            catch (Exception ex)
            {
                lits.Text = "加载数据失败：" + ex.Message;
            }
        }

        private void UpdatePagingInfo()
        {
            string pageInfo = $"第 {CurrentPage} 页 / 共 {TotalPages} 页";
            litPageInfo.Text = pageInfo;
            
            btnPrev.Enabled = CurrentPage > 1;
            btnNext.Enabled = CurrentPage < TotalPages;
            
            // 设置跳转输入框显示当前页码
            txtJumpPage.Text = CurrentPage.ToString();
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                UpdateCurrentCaigoudanhao();
                BindData();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                UpdateCurrentCaigoudanhao();
                BindData();
            }
        }

        protected void btnJump_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtJumpPage.Text, out int targetPage))
            {
                if (targetPage >= 1 && targetPage <= TotalPages)
                {
                    CurrentPage = targetPage;
                    UpdateCurrentCaigoudanhao();
                    BindData();
                }
                else
                {
                    lits.Text = $"页码超出范围，请输入 1 到 {TotalPages} 之间的数字";
                }
            }
            else
            {
                lits.Text = "请输入有效的页码数字";
            }
        }

        private void UpdateCurrentCaigoudanhao()
        {
            var caigoudanhaoList = ViewState["CaigoudanhaoList"] as List<string>;
            if (caigoudanhaoList != null && CurrentPage <= caigoudanhaoList.Count)
            {
                CurrentCaigoudanhao = caigoudanhaoList[CurrentPage - 1];
            }
        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string commandName = e.CommandName;
            
            switch (commandName)
            {
                case "SaveItem":
                    SaveSingleItem(e.Item);
                    break;
            }
        }

        private void SaveSingleItem(RepeaterItem item)
        {
            try
            {
                var saveData = ExtractItemData(item);
                if (!saveData.IsValid)
                {
                    lits.Text = $"保存失败：{saveData.ErrorMessage}";
                    return;
                }

                if (ExecuteSaveItem(saveData))
                {
                    BindData(); // 重新加载数据以显示最新状态
                    lits.Text = "保存成功！";
                }
                else
                {
                    lits.Text = "保存失败：数据库更新失败";
                }
            }
            catch (Exception ex)
            {
                lits.Text = "保存失败：" + ex.Message;
            }
        }

        private ItemSaveData ExtractItemData(RepeaterItem item)
        {
            var saveData = new ItemSaveData();
            
            try
            {
                // 获取隐藏字段值
                var hidCaigoudanhao = item.FindControl("hidCaigoudanhao") as HiddenField;
                var hidSKUID_ID = item.FindControl("hidSKUID_ID") as HiddenField;
                
                // 获取用户输入控件
                var txtShijicaigoushuliang = item.FindControl("txtShijicaigoushuliang") as TextBox;
                var ddlStatus = item.FindControl("ddlStatus") as DropDownList;
                var txtDingDanBianHao = item.FindControl("txtDingDanBianHao") as TextBox;
                var txtBeizhu = item.FindControl("txtBeizhu") as TextBox;
                
                // 验证控件是否存在
                if (hidCaigoudanhao == null || hidSKUID_ID == null || 
                    txtShijicaigoushuliang == null || ddlStatus == null || 
                    txtDingDanBianHao == null || txtBeizhu == null)
                {
                    saveData.IsValid = false;
                    saveData.ErrorMessage = "找不到必要的输入控件";
                    return saveData;
                }
                
                // 验证实际采购数量
                if (string.IsNullOrWhiteSpace(txtShijicaigoushuliang.Text))
                {
                    saveData.IsValid = false;
                    saveData.ErrorMessage = "实际采购数量不能为空";
                    return saveData;
                }
                
                if (!decimal.TryParse(txtShijicaigoushuliang.Text.Trim(), out decimal quantity) || quantity <= 0)
                {
                    saveData.IsValid = false;
                    saveData.ErrorMessage = "实际采购数量必须为大于0的数字";
                    return saveData;
                }
                
                // 验证1688订单编号
                if (string.IsNullOrWhiteSpace(txtDingDanBianHao.Text))
                {
                    saveData.IsValid = false;
                    saveData.ErrorMessage = "1688订单编号不能为空";
                    return saveData;
                }
                
                // 设置数据
                saveData.Caigoudanhao = hidCaigoudanhao.Value?.Trim() ?? "";
                saveData.SKUID_ID = hidSKUID_ID.Value?.Trim() ?? "";
                saveData.Shijicaigoushuliang = quantity;
                saveData.Status = ddlStatus.SelectedValue?.Trim() ?? "";
                saveData.DingDanBianHao = txtDingDanBianHao.Text?.Trim() ?? "";
                saveData.Beizhu = txtBeizhu.Text?.Trim() ?? "";
                saveData.IsValid = true;
                saveData.ErrorMessage = "";
                
                return saveData;
            }
            catch (Exception ex)
            {
                saveData.IsValid = false;
                saveData.ErrorMessage = "数据提取失败：" + ex.Message;
                return saveData;
            }
        }
        
        private bool ExecuteSaveItem(ItemSaveData saveData)
        {
            try
            {
                if (!saveData.IsValid)
                {
                    return false;
                }
                
                string[] columns = { "shijicaigoushuliang", "status", "DingDanBianHao", "beizhu" };
                object[] values = { saveData.Shijicaigoushuliang, saveData.Status, saveData.DingDanBianHao, saveData.Beizhu };
                string whereCondition = $"caigoudanhao = '{saveData.Caigoudanhao.Replace("'", "''")}' AND SKUID_ID = '{saveData.SKUID_ID.Replace("'", "''")}'";
                
                int result = access_sql.T_Update_ExecSql(columns, values, TABLE_CAIGOUDAN, whereCondition);
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                int successCount = 0;
                int errorCount = 0;
                var errorMessages = new List<string>();
                
                foreach (RepeaterItem item in rplb.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        var saveData = ExtractItemData(item);
                        if (saveData.IsValid && ExecuteSaveItem(saveData))
                        {
                            successCount++;
                        }
                        else
                        {
                            errorCount++;
                            if (!saveData.IsValid)
                            {
                                errorMessages.Add($"第{item.ItemIndex + 1}行：{saveData.ErrorMessage}");
                            }
                        }
                    }
                }
                
                // 重新加载数据
                BindData();

                if (errorCount == 0)
                {
                    lits.Text = $"保存成功！共保存 {successCount} 条记录";
                }
                else
                {
                    string errorDetail = errorMessages.Count > 0 ? 
                        $"<br/>错误详情：{string.Join("<br/>", errorMessages.Take(3))}" + 
                        (errorMessages.Count > 3 ? "<br/>..." : "") : "";
                    lits.Text = $"保存完成！成功 {successCount} 条，失败 {errorCount} 条{errorDetail}";
                }
            }
            catch (Exception ex)
            {
                lits.Text = "保存失败：" + ex.Message;
            }
        }
    }
} 