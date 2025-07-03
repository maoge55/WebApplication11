using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace WebApplication11.cg.tb
{
    public partial class 采购销售海外仓数据匹配 : System.Web.UI.Page
    {
        // 表名常量
        private const string TABLE_NAME = "Purchase_Sales_Warehouse";
   
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
        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public void bindzhy(String SJBM = "", string status = "-1", string haiWaiCangBianMa = "")
        {

            string safeSJBM = !string.IsNullOrEmpty(SJBM) ? SJBM.Replace("'", "''") : "";
            string safeHaiWaiCangBianMa = !string.IsNullOrEmpty(haiWaiCangBianMa) ? haiWaiCangBianMa.Replace("'", "''") : "";
            string whereCondition = "";
            if (!string.IsNullOrEmpty(safeSJBM))
                whereCondition += " AND o.yunyingbianma = '" + safeSJBM + "'";
            
            if (!string.IsNullOrEmpty(safeHaiWaiCangBianMa))
                whereCondition += " AND (o.haiwaicangxitongbianma = '" + safeHaiWaiCangBianMa + "' OR o.hwid_th = '" + safeHaiWaiCangBianMa + "')";
                
            if(!string.IsNullOrEmpty(status)&& status != "-1")
            {
                switch (status)
                {
                    case "需补充1688OfferID和1688SKUID":
                        whereCondition += " AND ((o.OfferID_1688 IS NULL OR o.OfferID_1688 = '') AND (o.SkuID_1688 IS NULL OR o.SkuID_1688 = '') )";
                        break;
                    case "需补充1688SKUID":
                        whereCondition += " AND( (o.SkuID_1688 IS NULL OR o.SkuID_1688 = '') AND (o.OfferID_1688 IS NOT NULL))";
                        break;
                    case "补充泰国海外仓系统编码":
                        whereCondition += " AND (o.hwid_th IS NULL OR o.hwid_th = '')";
                        break;

                }
            }



            string sql = string.Format(@"
                SELECT * 
                FROM {0} AS o
                where o.haiwaicangxitongbianma!=''
                {1}", TABLE_NAME, whereCondition);
            

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

        }



  





        protected void Button1_Click(object sender, EventArgs e)
        {

            string sjbm = txtsjbm.Text.Trim();
            string haiWaiCangBianMa = txtHaiWaiCangBianMa.Text.Trim();
            string status = ddlStatus.SelectedValue;  // 获取状态筛选值

            bindzhy(SJBM: sjbm, status: status, haiWaiCangBianMa: haiWaiCangBianMa);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {


            if (e.CommandName == "qr")
            {
                lits.Text = "";
                ulong rucangSKUID = Convert.ToUInt64(e.CommandArgument);
                Literal rucangItemID = e.Item.FindControl("rucangItemID") as Literal;

                string ItemID = rucangItemID.Text;
                TextBox txtY_1688OfferID = e.Item.FindControl("txtY_1688OfferID") as TextBox;
                TextBox txtY_1688SKUID = e.Item.FindControl("txtY_1688SKUID") as TextBox;
                TextBox txtHwid_th = e.Item.FindControl("txtHwid_th") as TextBox;




                if (access_sql.T_Update_ExecSql(new string[] { "OfferID_1688", "SkuID_1688", "hwid_th" }, new object[] { txtY_1688OfferID.Text.Trim().Replace("'", "''"), txtY_1688SKUID.Text.Trim().Replace("'", "''"), txtHwid_th.Text.Trim().Replace("'", "''") }, TABLE_NAME, "rucangSKUID='" + rucangSKUID +  "' AND　rucangItemID='"+ ItemID+"' ") > 0)
                {
                    bindzhy();
                    lits.Text = "rucangSKUID:" + rucangSKUID + "更新成功";
                }

            }
        }
        public void clzy()
        {
            int cg = 0;


            for (int i = 0; i < rplb.Items.Count; i++)
            {
                Literal id = (Literal)rplb.Items[i].FindControl("rucangSKUID");
                String skuid = id.Text;
                Literal rucangItemID = (Literal)rplb.Items[i].FindControl("rucangItemID") as Literal;
                String ItemID = rucangItemID.Text;
                TextBox txtY_1688OfferID = rplb.Items[i].FindControl("txtY_1688OfferID") as TextBox;
                TextBox txtY_1688SKUID = rplb.Items[i].FindControl("txtY_1688SKUID") as TextBox;
                TextBox txtHwid_th = rplb.Items[i].FindControl("txtHwid_th") as TextBox;
     
                cg += access_sql.T_Update_ExecSql(new string[] { "OfferID_1688", "SkuID_1688", "hwid_th" }, new object[] { txtY_1688OfferID.Text.Trim().Replace("'", "''"), txtY_1688SKUID.Text.Trim().Replace("'", "''"), txtHwid_th.Text.Trim().Replace("'", "''") }, TABLE_NAME, "rucangSKUID='" + skuid + "' AND　rucangItemID='"+ItemID+"' ");
            }
            bindzhy();
            lits.Text = "更新成功" + cg + "个";

        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            clzy();
        }

       


 
    }
}