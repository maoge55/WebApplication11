using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg.tb
{
    public partial class 菲律宾shopee数据处理_运营 : System.Web.UI.Page
    {
        public string u = "";
        public string p = "";
        public string uid = "";

        // 保存数据模型
        private class ItemSaveData
        {
            public string id { get; set; }
            public string VolumeStatus { get; set; }
            public string Y_1688url { get; set; }
            public string is_basic_data { get; set; }
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

        public int PageSize = 10; // 1页只展示1个caigoudanhao下的数据

        public int TotalPages
        {
            get { return ViewState["TotalPages"] != null ? (int)ViewState["TotalPages"] : 0; }
            set { ViewState["TotalPages"] = value; }
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

            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            BindData();
        }

        private void loadData()
        {
            try
            {
                string price1 = txtPriceLow.Text;
                string price2 = txtPriceHigh.Text;
                string month_sold = txtMonthSold.Text;
                string historical_sold = txtHistoricalSold.Text;
                string rating_star = txtRatingStar.Text;
                string pname = txtPname.Text;
                string VolumeStatus = ddlVolumeStatus.SelectedValue;
                string Y_1688url = ddlY1688url.SelectedValue;

                string whereCondition = $" 1=1 ";

                if (!string.IsNullOrEmpty(price1))
                {
                    whereCondition += $" AND price > "+ StrToInt2(price1);
                }
                if (!string.IsNullOrEmpty(price2))
                {
                    whereCondition += $" AND price < " + StrToInt2(price2);
                }
                if (!string.IsNullOrEmpty(month_sold))
                {
                    whereCondition += $" AND month_sold > " + StrToInt2(month_sold);
                }
                if (!string.IsNullOrEmpty(historical_sold))
                {
                    whereCondition += $" AND historical_sold > " + StrToInt2(historical_sold);
                }
                if (!string.IsNullOrEmpty(rating_star))
                {
                    whereCondition += $" AND rating_star > " + StrToDouble(rating_star);
                }

                if (!string.IsNullOrEmpty(pname))
                {
                    whereCondition += $" AND pname LIKE '%{pname}%'";
                }
                if (VolumeStatus == "" || VolumeStatus == null)
                {
                    whereCondition += $" AND VolumeStatus is null ";
                }
                else if (VolumeStatus == "体积审核通过")
                {
                    whereCondition += $" AND VolumeStatus ='体积审核通过' ";
                }
                else if (VolumeStatus == "体积审核不通过")
                {
                    whereCondition += $" AND VolumeStatus ='体积审核不通过' ";
                }
                else
                {

                }
                if (Y_1688url == "" || Y_1688url == null)
                {
                    whereCondition += $" AND Y_1688url is null ";
                }
                else if (Y_1688url == "有货源")
                {
                    whereCondition += $" AND Y_1688url like '%1688.com%' ";
                }
                else if (Y_1688url == "无货源")
                {
                    whereCondition += $" AND Y_1688url ='无货源' ";
                }
                else
                {

                }

                int bPage = (CurrentPage - 1) * PageSize + 1;
                int ePage = bPage + PageSize - 1;
                string sql = $@"select * from(
                    SELECT *,ROW_NUMBER() OVER (ORDER BY c.month_sold desc) AS RowNum FROM ShopeePHADPro c
                    WHERE {whereCondition} ) bb WHERE RowNum BETWEEN " + bPage + " AND " + ePage;
                string sqlCount = $@"SELECT count(*) num FROM ShopeePHADPro c
                    WHERE {whereCondition}  ";

                DataSet ds = access_sql.GreatDs(sql);
                System.Data.DataTable dt = ds.Tables[0];
                DataSet dsCount = access_sql.GreatDs(sqlCount);
                System.Data.DataTable dtCount = dsCount.Tables[0];

                if (dt.Rows.Count <= 0)
                {
                    rplb.DataSource = null;
                    rplb.DataBind();
                    lits.Text = "未找到符合条件的数据";
                    CurrentPage = 1;
                    TotalPages = 1;
                    UpdatePagingInfo();
                    return;
                }

                rplb.DataSource = dt;
                rplb.DataBind();
                int pages = int.Parse(dtCount.Rows[0]["NUM"].ToString()) / PageSize;
                if (pages == 0)
                {
                    TotalPages = 1;
                }
                else
                {
                    TotalPages = pages;
                }

                //if (TotalPages > 0 && CurrentPage <= TotalPages)
                //{
                //    CurrentCaigoudanhao = caigoudanhaoList[CurrentPage - 1];
                //}
                //else
                //{
                //    CurrentCaigoudanhao = "";
                //}
            }
            catch (Exception ex)
            {
                lits.Text = "查询列名总表_管理员失败：" + ex.Message;
            }
        }
        private void BindData()
        {
            loadData();
            UpdatePagingInfo();
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
            if (int.TryParse(txtJumpPage.Text, out int targetPage))
            {
                if (targetPage >= 1 && targetPage <= TotalPages)
                {
                    CurrentPage = targetPage;
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
                var hidId = item.FindControl("hidId") as HiddenField;

                // 获取用户输入控件
                var txtY1688url = item.FindControl("txtY1688url") as System.Web.UI.WebControls.TextBox;
                var chkY1688url = item.FindControl("chkY1688url") as CheckBox;
                var ddlVolumeStatus = item.FindControl("ddlVolumeStatus") as DropDownList;

                // 验证控件是否存在
                if (hidId == null || txtY1688url == null || chkY1688url == null ||
                    ddlVolumeStatus == null)
                {
                    saveData.IsValid = false;
                    saveData.ErrorMessage = "找不到必要的输入控件";
                    return saveData;
                }

                // 设置数据
                if (chkY1688url.Checked)
                {
                    saveData.Y_1688url = "无货源";
                }
                else {
                    saveData.Y_1688url = txtY1688url.Text;
                }
                saveData.VolumeStatus = ddlVolumeStatus.Text;
                saveData.id = hidId.Value?.Trim() ?? "";
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
        public double StrToDouble(string str)
        {
            double result = 0;
            try
            {
                double num = double.Parse(str);
                result = num;
            }
            catch (Exception e)
            {
                result = 0;
            }
            return result;
        }
        public int StrToInt2(string str)
        {
            int result = 0;
            try
            {
                int num = int.Parse(str);
                result = num;
            }
            catch (Exception e)
            {
                result = 0;
            }
            return result;
        }
        public string StrToInt(string str)
        {
            string result = "";
            try
            {
                int num = int.Parse(str);
                result = num + "";
            }
            catch (Exception e)
            {
                result = "0";
            }
            return result;
        }
        public string StrToNull(Object obj)
        {
            string result = "";
            try
            {
                if (obj == null || obj.ToString() == "")
                {
                    result = "";
                }
                else
                {
                    result = obj.ToString();
                }
            }
            catch (Exception e)
            {
                result = "";
            }
            return result;
        }

        private bool ExecuteSaveItem(ItemSaveData saveData)
        {
            try
            {
                if (!saveData.IsValid)
                {
                    return false;
                }

                string sql = "update ShopeePHADPro set VolumeStatus='" + saveData.VolumeStatus + "',Y_1688url='" + saveData.Y_1688url + "' where id='" + saveData.id + "'";
                access_sql.DoSql(sql);
                return true;
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string id = _hidId.Value;
                string zt = _ddlStatus.SelectedValue;
                string sql = "";
                if (zt == "体积审核通过")
                {
                    sql = "update ShopeePHADPro set VolumeStatus='体积审核通过' where id in("+id+")";
                }
                else if (zt == "体积审核不通过") {
                    sql = "update ShopeePHADPro set VolumeStatus='体积审核不通过' where id in(" + id+")";
                }
                else if (zt == "无货源")
                {
                    sql = "update ShopeePHADPro set Y_1688url='无货源' where id in(" + id+")";
                }
                access_sql.DoSql(sql);
                lits.Text = "处理成功" + id + "数据!";
            }
            catch (Exception ex)
            {
                lits.Text = "处理异常!";
            }
        }
    }
}