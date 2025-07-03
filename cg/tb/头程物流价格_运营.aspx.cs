using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication11.cg.tb
{
    [Serializable]
    public class NewRecordData
    {
        public string Id { get; set; }
        public string LogisticsCode { get; set; }
        public string FirstLogisticsPrice { get; set; }
        public string PricingUnit { get; set; }
        public string ProductCategory { get; set; }
    }

    public partial class 头程物流价格_运营 : System.Web.UI.Page
    {
        // 数据表名变量
        private const string HEAD_LOGISTICS_PRICE_TABLE = "HeadLogisticsPrice";
        
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
            
            // 每次页面加载都重新查询数据
            BindData();
        }
        
        public string u = "";
        public string p = "";
        public string uid = "";

        private void ClearNewRecords()
        {
            ViewState.Remove("NewRecords");
        }

        public void BindData()
        {
            try 
            {
                lits.Text = "";
                
                // 创建一个新的DataTable来存储数据
                DataTable dt = new DataTable();
                dt.Columns.Add("id");
                dt.Columns.Add("logistics_product_type_code");
                dt.Columns.Add("first_logistics_price");
                dt.Columns.Add("pricing_unit");
                dt.Columns.Add("product_category");
                dt.Columns.Add("IsNewRecord");

                // 查询SQL，按物流商品种编号的拼音排序
                string sql = @"
                    SELECT 
                        id,
                        logistics_product_type_code,
                        first_logistics_price,
                        pricing_unit,
                        product_category,
                        0 as IsNewRecord
                    FROM " + HEAD_LOGISTICS_PRICE_TABLE + @"
                    ORDER BY logistics_product_type_code COLLATE Chinese_PRC_CI_AS";

                DataSet ds = access_sql.GreatDs(sql, 300);

                // 如果查询结果不为空，将数据填充到DataTable中
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["id"] = row["id"];
                        newRow["logistics_product_type_code"] = row["logistics_product_type_code"];
                        newRow["first_logistics_price"] = row["first_logistics_price"];
                        newRow["pricing_unit"] = row["pricing_unit"];
                        newRow["product_category"] = row["product_category"];
                        newRow["IsNewRecord"] = 0;
                        dt.Rows.Add(newRow);
                    }
                }

                // 添加新记录（如果有）
                if (ViewState["NewRecords"] != null)
                {
                    List<NewRecordData> newRecords = ViewState["NewRecords"] as List<NewRecordData>;
                    if (newRecords != null && newRecords.Count > 0)
                    {
                        foreach (var record in newRecords.OrderByDescending(r => r.Id))
                        {
                            DataRow newRow = dt.NewRow();
                            newRow["id"] = record.Id;
                            newRow["logistics_product_type_code"] = record.LogisticsCode;
                            newRow["first_logistics_price"] = record.FirstLogisticsPrice;
                            newRow["pricing_unit"] = record.PricingUnit;
                            newRow["product_category"] = record.ProductCategory;
                            newRow["IsNewRecord"] = 1;
                            dt.Rows.InsertAt(newRow, 0);
                        }
                    }
                }

                // 始终绑定DataTable，即使它是空的
                rplb.DataSource = dt;
                rplb.DataBind();
            }
            catch (Exception ex)
            {
                lits.Text = "查询出错：" + ex.Message;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // 生成临时ID
                string tempId = "NEW_" + Guid.NewGuid().ToString("N").Substring(0, 8);
                
                // 创建新记录
                List<NewRecordData> newRecords = ViewState["NewRecords"] as List<NewRecordData> ?? new List<NewRecordData>();
                
                var newRecord = new NewRecordData
                {
                    Id = tempId,
                    LogisticsCode = "",
                    FirstLogisticsPrice = "",
                    PricingUnit = "",
                    ProductCategory = ""
                };
                
                newRecords.Add(newRecord);
                ViewState["NewRecords"] = newRecords;
                
                // 重新绑定数据
                BindData();
                
                lits.Text = "请填写物流商品种编号等信息";
            }
            catch (Exception ex)
            {
                lits.Text = "添加新行出错：" + ex.Message;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                RepeaterItem item = (RepeaterItem)btn.NamingContainer;
                
                // 获取表单数据
                TextBox txtLogisticsCode = (TextBox)item.FindControl("txtLogisticsCode");
                TextBox txtFirstLogisticsPrice = (TextBox)item.FindControl("txtFirstLogisticsPrice");
                TextBox txtPricingUnit = (TextBox)item.FindControl("txtPricingUnit");
                TextBox txtProductCategory = (TextBox)item.FindControl("txtProductCategory");
                HiddenField hdnIsNewRecord = (HiddenField)item.FindControl("hdnIsNewRecord");
                HiddenField hdnRowId = (HiddenField)item.FindControl("hdnRowId");
                
                if (txtLogisticsCode == null)
                {
                    lits.Text = "错误：找不到输入控件";
                    return;
                }

                // 验证必填项
                if (string.IsNullOrEmpty(txtLogisticsCode.Text.Trim()))
                {
                    lits.Text = "错误：物流商品种编号为必填项";
                    return;
                }

                string logisticsCode = txtLogisticsCode.Text.Trim();
                string firstLogisticsPrice = txtFirstLogisticsPrice.Text.Trim();
                string pricingUnit = txtPricingUnit.Text.Trim();
                string productCategory = txtProductCategory.Text.Trim();
                string recordId = hdnRowId.Value;
                bool isNewRecord = hdnIsNewRecord != null && hdnIsNewRecord.Value == "True";

                int result = 0;
                
                if (isNewRecord || recordId.StartsWith("NEW_"))
                {
                    // 新增记录
                    result = access_sql.T_Insert_ExecSql(
                        new string[] { "logistics_product_type_code", "first_logistics_price", "pricing_unit", "product_category" },
                        new object[] { logisticsCode, firstLogisticsPrice, pricingUnit, productCategory },
                        HEAD_LOGISTICS_PRICE_TABLE);
                        
                    if (result > 0)
                    {
                        // 保存成功后清除所有新增记录
                        ClearNewRecords();
                        // 重定向到当前页面，避免刷新时重复提交
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lits.Text = "新增失败";
                    }
                }
                else
                {
                    // 更新现有记录
                    result = access_sql.T_Update_ExecSql(
                        new string[] { "logistics_product_type_code", "first_logistics_price", "pricing_unit", "product_category" },
                        new object[] { logisticsCode, firstLogisticsPrice, pricingUnit, productCategory },
                        HEAD_LOGISTICS_PRICE_TABLE,
                        "id = " + recordId);
                        
                    if (result > 0)
                    {
                        // 重定向到当前页面，避免刷新时重复提交
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lits.Text = "更新失败";
                    }
                }

                // 重新绑定数据
                BindData();
            }
            catch (Exception ex)
            {
                lits.Text = "保存出错：" + ex.Message;
            }
        }
    }
} 