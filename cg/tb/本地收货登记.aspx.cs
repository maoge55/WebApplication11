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
    public partial class 本地收货登记 : System.Web.UI.Page
    {
        public string u = "";
        public string p = "";
        public string uid = "";

        // 保存数据模型
        private class ItemSaveData
        {
            public string id { get; set; }
            public string Skuid { get; set; }
            public string ShiJiShouHuoCount { get; set; }
            public string CanCiPinCount { get; set; }
            public string DingDanBeiZhu { get; set; }
            public string ShiJiShouHuoCountOld { get; set; }
            public string CanCiPinCountOld { get; set; }
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }
            public string sku_img { get; set; }
            public string pimage { get; set; }
            public string video { get; set; }
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
                if (uid != "8" && uid != "9" && uid != "18" && uid != "19" && uid != "12" && uid != "6" && uid!="22")
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
            if (string.IsNullOrWhiteSpace(txtYYBM.Text))
            {
                lits.Text = "运营编码为必填项！";
                return;
            }

            CurrentPage = 1;
            BindData();
        }

        private void loadData()
        {
            try
            {
                string yybm = txtYYBM.Text.Trim();
                string status = ddlSearchStatus.SelectedValue;
                string kddh = txtKuaididanhao.Text;
                string title = txtBiaoti.Text;
                string orderNo = txt1688OrderNo.Text.Trim();

                string whereCondition = $"c.YYBM = '{yybm}'";

                if (!string.IsNullOrEmpty(status))
                {
                    whereCondition += $" AND ISNULL(c.DingDanZhuangTai, '') = '{status}'";
                }

                if (!string.IsNullOrEmpty(orderNo))
                {
                    whereCondition += $" AND c.DingDanBianHao LIKE '%{orderNo}%'";
                }

                if (!string.IsNullOrEmpty(kddh))
                {
                    whereCondition += $" AND c.YunDanHao LIKE '%{kddh}%'";
                }
                if (!string.IsNullOrEmpty(title))
                {
                    whereCondition += $" AND c.HuoPinBiaoTi LIKE '%{title}%'";
                }
                int bPage= (CurrentPage-1)*PageSize+1;
                int ePage = bPage + PageSize-1;
                string sql = $@"select * from(
                    SELECT *,ROW_NUMBER() OVER (ORDER BY c.YunDanHao DESC) AS RowNum FROM {TABLE_S1688_ORDER} c
                    WHERE {whereCondition} ) bb WHERE RowNum BETWEEN "+bPage+" AND "+ePage;
                string sqlCount = $@"SELECT count(*) num FROM S1688Order c
                    WHERE {whereCondition}  ";

                DataSet ds = access_sql.GreatDs(sql);
                DataTable dt = ds.Tables[0];
                DataSet dsCount = access_sql.GreatDs(sqlCount);
                DataTable dtCount = dsCount.Tables[0];

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
                int pages= int.Parse(dtCount.Rows[0]["NUM"].ToString()) / PageSize;
                if (pages == 0) {
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
                lits.Text = "查询采购单号失败：" + ex.Message;
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
                case "SaveItemSkuImg":
                    SaveSingleItemImg(e.Item);
                    break;

            }
        }
        private void SaveSingleItemImg(RepeaterItem item)
        {
            try
            {
                var id = item.FindControl("hidId") as HiddenField;
                var hidSkuid = item.FindControl("hidSkuid") as HiddenField;
                var fup1 = item.FindControl("fup1") as FileUpload;
                var fup2 = item.FindControl("fup2") as FileUpload;
                var fup3 = item.FindControl("fup3") as FileUpload;
                string ext1 = System.IO.Path.GetExtension(fup1.FileName);
                string ext2 = System.IO.Path.GetExtension(fup2.FileName);
                string ext3 = System.IO.Path.GetExtension(fup3.FileName);
                if (ext1.ToUpper() == ".JPG" || ext1.ToUpper() == ".PNG" || ext1.ToUpper() == ".BMP")
                {
                }
                else
                {
                    lits.Text = "文件格式不对，图片支持JPG|PNG|BMP,视频支持MP4！";
                    return;
                }
                if (ext2.ToUpper() == ".JPG" || ext2.ToUpper() == ".PNG" || ext2.ToUpper() == ".BMP")
                {
                }
                else
                {
                    lits.Text = "文件格式不对，图片支持JPG|PNG|BMP,视频支持MP4！";
                    return;
                }
                if (ext3.ToUpper() != ".MP4")
                {
                    lits.Text = "文件格式不对，图片支持JPG|PNG|BMP,视频支持MP4！";
                    return;
                }
                string path1 = "/upload/S1688Order/skuimg" + hidSkuid.Value + "/";
                string path2 = "/upload/S1688Order/pimage" + hidSkuid.Value + "/";
                string path3 = "/upload/S1688Order/video" + hidSkuid.Value + "/";
                if (!Directory.Exists(Server.MapPath(path1)))
                {
                    Directory.CreateDirectory(Server.MapPath(path1));
                }
                if (!Directory.Exists(Server.MapPath(path2)))
                {
                    Directory.CreateDirectory(Server.MapPath(path2));
                }
                if (!Directory.Exists(Server.MapPath(path3)))
                {
                    Directory.CreateDirectory(Server.MapPath(path3));
                }
                string newName1 = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext1;
                string newName2 = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext2;
                string newName3 = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext3;
                string save1 = Server.MapPath(path1) + newName1;
                string save2 = Server.MapPath(path2) + newName2;
                string save3 = Server.MapPath(path3) + newName3;
                fup1.SaveAs(save1);
                fup2.SaveAs(save2);
                fup3.SaveAs(save3);
                string sql = "update S1688Order set sku_img='"+path1+newName1+"',pimage='"+path2+newName2+"',video='"+path3+newName3+"' where id=" + id.Value;
                access_sql.DoSql(sql);
                BindData();
                lits.Text = "添加成功";
            }
            catch (Exception ex)
            {
                lits.Text = "保存失败：" + ex.Message;
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
                var hidSkuid = item.FindControl("hidSkuid") as HiddenField;

                // 获取用户输入控件
                var txtShiJiShouHuoCount = item.FindControl("txtShiJiShouHuoCount") as TextBox;
                var txtCanCiPinCount = item.FindControl("txtCanCiPinCount") as TextBox;
                var txtDingDanBeiZhu = item.FindControl("txtDingDanBeiZhu") as TextBox;
                var txtShiJiShouHuoCountOld = item.FindControl("txtShiJiShouHuoCountOld") as HiddenField;
                var txtCanCiPinCountOld = item.FindControl("txtCanCiPinCountOld") as HiddenField;

                var fup1 = item.FindControl("fup1") as FileUpload;
                var fup2 = item.FindControl("fup2") as FileUpload;
                var fup3 = item.FindControl("fup3") as FileUpload;
                // 验证控件是否存在
                if (hidId == null || hidSkuid == null ||
                    txtShiJiShouHuoCount == null || txtCanCiPinCount == null ||
                    txtDingDanBeiZhu == null )
                {
                    saveData.IsValid = false;
                    saveData.ErrorMessage = "找不到必要的输入控件";
                    return saveData;
                }

                // 验证实际采购数量
                if (string.IsNullOrWhiteSpace(txtShiJiShouHuoCount.Text))
                {
                    saveData.IsValid = false;
                    saveData.ErrorMessage = "实际收货数量不能为空";
                    return saveData;
                }
                if (string.IsNullOrWhiteSpace(txtCanCiPinCount.Text))
                {
                    saveData.IsValid = false;
                    saveData.ErrorMessage = "残次品数量不能为空";
                    return saveData;
                }
                if (string.IsNullOrWhiteSpace(txtDingDanBeiZhu.Text))
                {
                    saveData.IsValid = false;
                    saveData.ErrorMessage = "订单备注不能为空";
                    return saveData;
                }


                // 设置数据
                saveData.ShiJiShouHuoCount = StrToInt(txtShiJiShouHuoCount.Text);
                saveData.CanCiPinCount = StrToInt(txtCanCiPinCount.Text);
                saveData.DingDanBeiZhu = txtDingDanBeiZhu.Text?.Trim() ?? "";
                saveData.ShiJiShouHuoCountOld = StrToInt(txtShiJiShouHuoCountOld.Value);
                saveData.CanCiPinCountOld = StrToInt(txtCanCiPinCountOld.Value);
                saveData.Skuid = hidSkuid.Value?.Trim() ?? "";
                saveData.id = hidId.Value?.Trim() ?? "";
                saveData.IsValid = true;
                saveData.ErrorMessage = "";

                #region 文件操作    
                try
                {
                    string ext1 = System.IO.Path.GetExtension(fup1.FileName);
                    string ext2 = System.IO.Path.GetExtension(fup2.FileName);
                    string ext3 = System.IO.Path.GetExtension(fup3.FileName);
                    string path1 = "/upload/S1688Order/skuimg" + hidSkuid.Value + "/";
                    string path2 = "/upload/S1688Order/pimage" + hidSkuid.Value + "/";
                    string path3 = "/upload/S1688Order/video" + hidSkuid.Value + "/";
                    if (!Directory.Exists(Server.MapPath(path1)))
                    {
                        Directory.CreateDirectory(Server.MapPath(path1));
                    }
                    if (!Directory.Exists(Server.MapPath(path2)))
                    {
                        Directory.CreateDirectory(Server.MapPath(path2));
                    }
                    if (!Directory.Exists(Server.MapPath(path3)))
                    {
                        Directory.CreateDirectory(Server.MapPath(path3));
                    }
                    if (ext1.ToUpper() == ".JPG" || ext1.ToUpper() == ".PNG" || ext1.ToUpper() == ".BMP")
                    {
                        string newName1 = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext1;
                        string save1 = Server.MapPath(path1) + newName1;
                        fup1.SaveAs(save1);
                        saveData.sku_img = path1 + newName1;
                    }
                    else
                    {
                        lits.Text = "文件格式不对，图片支持JPG|PNG|BMP,视频支持MP4！";
                        
                    }
                    if (ext2.ToUpper() == ".JPG" || ext2.ToUpper() == ".PNG" || ext2.ToUpper() == ".BMP")
                    {
                        string newName2 = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext2;
                        string save2 = Server.MapPath(path2) + newName2;
                        fup2.SaveAs(save2);
                        saveData.pimage = path2 + newName2;
                    }
                    else
                    {
                        lits.Text = "文件格式不对，图片支持JPG|PNG|BMP,视频支持MP4！";
                        
                    }
                    if (ext3.ToUpper() != ".MP4")
                    {
                        lits.Text = "文件格式不对，图片支持JPG|PNG|BMP,视频支持MP4！";

                    }
                    else
                    {
                        string newName3 = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext3;
                        string save3 = Server.MapPath(path3) + newName3;
                        fup3.SaveAs(save3);
                        saveData.video = path3 + newName3;
                    }
                }
                catch (Exception)
                {
                    saveData.ErrorMessage = "文件异常";
                }
                #endregion

                return saveData;
            }
            catch (Exception ex)
            {
                saveData.IsValid = false;
                saveData.ErrorMessage = "数据提取失败：" + ex.Message;
                return saveData;
            }
        }
        private string StrToInt(string str) {
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

        private bool ExecuteSaveItem(ItemSaveData saveData)
        {
            try
            {
                if (!saveData.IsValid)
                {
                    return false;
                }
                int sjsh = int.Parse(saveData.ShiJiShouHuoCount) + int.Parse(saveData.ShiJiShouHuoCountOld);
                int ccp = int.Parse(saveData.CanCiPinCount) + int.Parse(saveData.CanCiPinCountOld);
                string[] columns = { "ShiJiShouHuoCount", "CanCiPinCount", "DingDanBeiZhu" };
                object[] values = { sjsh,ccp, saveData.DingDanBeiZhu };
                string whereCondition = $"id = '{saveData.id}'";
                #region 更新采购单-skuid
                string sqlCaigoudan = "update caigoudan set ShiJiShouHuoCount="+sjsh+",CanCiPinCount="+ccp +" where skuid="+saveData.Skuid;
                access_sql.DoSql(sqlCaigoudan);
                #endregion
                //string whereCondition = $"caigoudanhao = '{saveData.Caigoudanhao.Replace("'", "''")}' AND SKUID_ID = '{saveData.SKUID_ID.Replace("'", "''")}'";

                try
                {
                    if (saveData.sku_img != null && saveData.sku_img.Length > 4)
                    {
                        string sql = "update S1688Order set sku_img='" + saveData.sku_img + "',pimage='" + saveData.pimage + "',video='" + saveData.video + "' where id=" + saveData.id;
                        access_sql.DoSql(sql);
                    }
                }
                catch (Exception)
                {

                }

                int result = access_sql.T_Update_ExecSql(columns, values, TABLE_S1688_ORDER, whereCondition);
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