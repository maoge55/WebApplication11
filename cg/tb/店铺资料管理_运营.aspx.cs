using NPOI.SS.Formula.Functions;
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
    public partial class 店铺资料管理_运营 : System.Web.UI.Page
    {
        public string u = "";
        public string p = "";
        public string uid = "";

        // 保存数据模型
        private class ItemSaveData
        {
            public string id { get; set; }
            public string Platform { get; set; }
            public string Country { get; set; }
            public string PingTai { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string SJBM { get; set; }
            public string YYBM { get; set; }
            public string ProxyIP { get; set; }
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
                string SJBM = txtSJBM.Text;
                string BrowserID = txtBrowserID.Text;
                string ProxyIP = ddlProxyIP.SelectedValue;

                string whereCondition = $" SJBM='"+SJBM+"' ";


                if (!string.IsNullOrEmpty(ProxyIP))
                {
                    if (ProxyIP == "补充代理IP")
                    {
                        whereCondition += $" AND ProxyIP is null ";
                    }
                }

                if (!string.IsNullOrEmpty(BrowserID))
                {
                    whereCondition += $" AND BrowserID LIKE '%{BrowserID}%'";
                }
                int bPage = (CurrentPage - 1) * PageSize + 1;
                int ePage = bPage + PageSize - 1;
                string sql = $@"select * from(
                    SELECT *,ROW_NUMBER() OVER (ORDER BY c.DpName asc) AS RowNum FROM Houtai c
                    WHERE {whereCondition} ) bb WHERE RowNum BETWEEN " + bPage + " AND " + ePage;
                string sqlCount = $@"SELECT count(*) num FROM Houtai c
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
                    //UpdatePagingInfo();
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
                var txtPlatform = item.FindControl("txtPlatform") as System.Web.UI.WebControls.TextBox;
                var txtUserName = item.FindControl("txtUserName") as System.Web.UI.WebControls.TextBox;
                var txtPassword = item.FindControl("txtPassword") as System.Web.UI.WebControls.TextBox;
                var txtSJBM = item.FindControl("txtSJBM") as System.Web.UI.WebControls.TextBox;
                var txtYYBM = item.FindControl("txtYYBM") as System.Web.UI.WebControls.TextBox;
                var txtProxyIP = item.FindControl("txtProxyIP") as System.Web.UI.WebControls.TextBox;
                var ddlCountry = item.FindControl("ddlCountry") as DropDownList;
                var ddlPingTai = item.FindControl("ddlPingTai") as DropDownList;

                // 验证控件是否存在
                if (hidId == null || txtPlatform == null || txtUserName == null ||
                    txtPassword == null || txtSJBM == null || txtYYBM == null
                    || txtProxyIP == null || ddlCountry == null || ddlPingTai == null)
                {
                    saveData.IsValid = false;
                    saveData.ErrorMessage = "找不到必要的输入控件";
                    return saveData;
                }

                // 设置数据
                saveData.Platform = txtPlatform.Text;
                saveData.UserName = txtUserName.Text;
                saveData.Password = txtPassword.Text;
                saveData.SJBM = txtSJBM.Text;
                saveData.YYBM = txtYYBM.Text;
                saveData.ProxyIP = txtProxyIP.Text;
                saveData.Country = ddlCountry.Text;
                saveData.PingTai = ddlPingTai.Text;
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
                else {
                    result= obj.ToString();
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

                string sql = "update Houtai set Platform='" + saveData.Platform + "',Country='" + saveData.Country + "',PingTai='" + saveData.PingTai + "',UserName='" + saveData.UserName + "',Password='" + saveData.Password + "',SJBM='" + saveData.SJBM + "',YYBM='" + saveData.YYBM + "',ProxyIP='" + saveData.ProxyIP + "' where id='" + saveData.id + "'";
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

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (fup1.FileName != "")
            {
                string ext = System.IO.Path.GetExtension(fup1.FileName);
                if (ext == ".xlsx")
                {
                    if (!Directory.Exists(Server.MapPath("/upload/Houtai/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/upload/Houtai/"));
                    }
                    string path = Server.MapPath("/upload/Houtai/") + fup1.FileName;

                    this.fup1.SaveAs(path);

                    System.Data.DataTable dtout = ReadExcelData_(path);
                    string sjbm = txtSjbmValue.Text;
                    if (dtout != null && dtout.Rows.Count > 0)
                    {
                        DataSet ds = access_sql.GreatDs("select BrowserID from Houtai");
                        System.Data.DataTable dt = ds.Tables[0];
                        for (int i = 1; i < dtout.Rows.Count; i++) {
                            string BrowserID = dtout.Rows[i][1].ToString();
                            string GroupName = dtout.Rows[i][2].ToString();
                            string DpName = dtout.Rows[i][3].ToString();
                            bool johnExists = dt.AsEnumerable()
                            .Any(row => row.Field<string>("BrowserID") == BrowserID);
                            if (johnExists)
                            {
                                string sql = "update Houtai set SJBM='" + sjbm + "',GroupName='" + GroupName + "',DpName='" + DpName + "' where BrowserID='" + BrowserID + "'";
                                access_sql.DoSql(sql);
                            }
                            else
                            {
                                string sql = "insert into Houtai(SJBM,BrowserID,GroupName,DpName)";
                                sql += "values('" + sjbm + "','" + BrowserID + "','" + GroupName + "','" + DpName + "')";
                                access_sql.DoSql(sql);
                            }
                        }
                        lits.Text = "处理成功" + dtout.Rows.Count + "条数据!";
                    }
                    BindData();
                }
                else
                {
                    lits.Text = "文件格式不对(支持xlsx)";

                }

            }
            else
            {
                lits.Text = "请选择文件";

            }
        }
        public static System.Data.DataTable ReadExcelData_(string filePath)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            try
            {


                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = new XSSFWorkbook(file);
                    ISheet sheet = workbook.GetSheetAt(0);
                    if (sheet != null)
                    {

                        IRow row = sheet.GetRow(0);

                        if (row != null)
                        {
                            for (int i = 0; i < row.LastCellNum; i++)
                            {
                                ICell cell = row.GetCell(i);
                                if (cell != null)
                                {

                                    dt.Columns.Add(cell.ToString().Trim());
                                }
                            }
                        }
                        for (int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
                        {
                            IRow row1 = sheet.GetRow(rowIndex);
                            if (row1 != null)
                            {
                                object[] obj = new object[dt.Columns.Count];

                                for (int colIndex = 0; colIndex < dt.Columns.Count; colIndex++)
                                {
                                    string cellValue = "";

                                    ICell cell = row1.GetCell(colIndex);

                                    if (cell != null)
                                    {

                                        cellValue = cell.ToString();

                                    }


                                    obj[colIndex] = cellValue;
                                }
                                dt.Rows.Add(obj);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return dt;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string id=_hidId.Value;
                string Platform = _txtPlatform.Text;
                string Country = _ddlCountry.SelectedValue;
                string PingTai = _ddlPingTai.SelectedValue;
                string SJBM = _txtSJBM.Text;
                string YYBM = _txtYYBM.Text;
                string sql = "update Houtai set Platform='"+ Platform + "',Country='" + Country + "',PingTai='" + PingTai + "',SJBM='" + SJBM + "',YYBM='" + YYBM + "' where id in(" + id+")";
                access_sql.DoSql(sql);
                lits.Text = "处理成功" + id + "数据!";
            }
            catch (Exception ex) { 
            
            }
        }
    }
}