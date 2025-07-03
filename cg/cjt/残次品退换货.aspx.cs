using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace WebApplication11.cg
{
    public partial class 残次品退换货 : System.Web.UI.Page
    {
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
                if (uid != "6" && uid != "12" && uid != "9")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public void bindzhy(String SJBM = "", string status = "")
        {

            string safeSJBM = !string.IsNullOrEmpty(SJBM)? SJBM.Replace("'", "''"): "";
            string whereCondition = !string.IsNullOrEmpty(safeSJBM)
                ? " AND o.shangjiabianma = '" + safeSJBM + "'"
                : "";

            string statusCondition = "";
            if (!string.IsNullOrEmpty(status) && status != "-1")
            {
                statusCondition = " AND o.cancipinchulijieguo = " + status;  // 修复字符串拼接错误
            }

            string sql = @"
           select * from cancipin as o where 1=1
                      " + statusCondition + whereCondition ;
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
            string status = ddlStatus.SelectedValue;
            bindzhy(SJBM: sjbm, status: status);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {


            if (e.CommandName == "qr")
            {
                lits.Text = "";
                ulong SKU_ID = Convert.ToUInt64(e.CommandArgument);
                TextBox txtY_diyici = e.Item.FindControl("txtY_diyici") as TextBox;
                TextBox txtY_dierci = e.Item.FindControl("txtY_dierci") as TextBox;
                TextBox txtY_disanci = e.Item.FindControl("txtY_disanci") as TextBox;
                TextBox txtY_beizhu = e.Item.FindControl("txtY_beizhu") as TextBox;
                DropDownList cbShifou = e.Item.FindControl("cbShifou") as DropDownList;
                int temp1, temp2, temp3;
                int.TryParse(txtY_diyici.Text, out temp1);
                int.TryParse(txtY_dierci.Text, out temp2);
                int.TryParse(txtY_disanci.Text, out temp3);

                int diyici = temp1;
                int dierci = temp2;
                int disanci = temp3;
                int total =diyici+dierci+disanci;
                if (access_sql.T_Update_ExecSql(new string[] { "diyicihuanhuoshuliang", "diercihuanhuoshuliang", "disancihuanhuoshuliang", "beizhu", "cancipinchulijieguo", "leijishoudaozhengchanghuowushuliang" }, new object[] { txtY_diyici.Text.Trim().Replace("'", "''"), txtY_dierci.Text.Trim().Replace("'", "''"), txtY_disanci.Text.Trim().Replace("'", "''"), txtY_beizhu.Text.Trim().Replace("'", "''"),  cbShifou.SelectedValue, total.ToString() }, "cancipin", "SKU_ID='" + SKU_ID + "'") > 0)
                    {
                       bindzhy();
                            lits.Text = "SKU_ID:" + SKU_ID + "更新成功";
                    }

            }
            if (e.CommandName == "uptuikuanimg")
            {
                lits.Text = "";
                string SKU_ID = (e.CommandArgument).ToString();
                FileUpload ff = e.Item.FindControl("FileUpload1") as FileUpload;
                DataList liimgxs = e.Item.FindControl("DataList1") as DataList;
                string imgs = access_sql.GetOneValue("select top 1 tuikuanjietu from cancipin where SKU_ID='" + SKU_ID + "'") + "|";
                for (int i = 0; i < ff.PostedFiles.Count; i++)
                {
                    string type = ff.PostedFiles[i].FileName.Split('.')[ff.PostedFiles[i].FileName.Split('.').Length - 1];
                    string imgname = DateTime.Now.ToString("yyyyMMddHHmmss") + i + "." + type;
                    string savename = Server.MapPath("~/Uploads/") + imgname;
                    ff.PostedFiles[i].SaveAs(savename);
                    imgs += imgname + "|";
                }
                if (imgs != "")
                {
                    imgs = imgs.Replace("||", "|");
                    access_sql.T_Update_ExecSql(new string[] { "tuikuanjietu" }, new object[] { imgs }, "cancipin", "SKU_ID='" + SKU_ID + "'");
                    bindimgs(liimgxs, imgs, SKU_ID);
                }

            }
            if (e.CommandName == "upcancipinimg")
            {
                lits.Text = "";
                string SKU_ID = (e.CommandArgument).ToString();
                FileUpload ff = e.Item.FindControl("FileUpload2") as FileUpload;
                DataList liimgxs = e.Item.FindControl("DataList2") as DataList;
                string imgs = access_sql.GetOneValue("select top 1 cancipintupian from cancipin where SKU_ID='" + SKU_ID + "'") + "|";
                for (int i = 0; i < ff.PostedFiles.Count; i++)
                {
                    string type = ff.PostedFiles[i].FileName.Split('.')[ff.PostedFiles[i].FileName.Split('.').Length - 1];
                    string imgname = DateTime.Now.ToString("yyyyMMddHHmmss") + i + "." + type;
                    string savename = Server.MapPath("~/Uploads/") + imgname;
                    ff.PostedFiles[i].SaveAs(savename);
                    imgs += imgname + "|";
                }
                if (imgs != "")
                {
                    imgs = imgs.Replace("||", "|");
                    access_sql.T_Update_ExecSql(new string[] { "cancipintupian" }, new object[] { imgs }, "cancipin", "SKU_ID='" + SKU_ID + "'");
                    bindimgs(liimgxs, imgs, SKU_ID);
                }

            }

        }
        public void clzy()
        {
            int cg = 0;


            for (int i = 0; i < rplb.Items.Count; i++)
            {
                Literal id = (Literal)rplb.Items[i].FindControl("SKU_ID");
                String skuid =id.Text;
                TextBox txtY_diyici = rplb.Items[i].FindControl("txtY_diyici") as TextBox;
                TextBox txtY_dierci = rplb.Items[i].FindControl("txtY_dierci") as TextBox;
                TextBox txtY_disanci = rplb.Items[i].FindControl("txtY_disanci") as TextBox;
                TextBox txtY_beizhu = rplb.Items[i].FindControl("txtY_beizhu") as TextBox;
                DropDownList cbShifou = rplb.Items[i].FindControl("cbShifou") as DropDownList;
                int temp1, temp2, temp3;
                int.TryParse(txtY_diyici.Text, out temp1);
                int.TryParse(txtY_dierci.Text, out temp2);
                int.TryParse(txtY_disanci.Text, out temp3);

                int diyici = temp1;
                int dierci = temp2;
                int disanci = temp3;
                int total = diyici + dierci + disanci;
                cg += access_sql.T_Update_ExecSql(new string[] { "diyicihuanhuoshuliang", "diercihuanhuoshuliang", "disancihuanhuoshuliang", "beizhu", "cancipinchulijieguo", "leijishoudaozhengchanghuowushuliang" }, new object[] { txtY_diyici.Text.Trim().Replace("'", "''"), txtY_dierci.Text.Trim().Replace("'", "''"), txtY_disanci.Text.Trim().Replace("'", "''"), txtY_beizhu.Text.Trim().Replace("'", "''"), cbShifou.SelectedValue, total.ToString() }, "cancipin", "SKU_ID='" + skuid + "'");
            }
            bindzhy();
            lits.Text = "更新成功" + cg + "个";
            
        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            clzy();
        }
        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                string filename = e.CommandArgument.ToString();
                Literal liimgSKU_ID = e.Item.FindControl("liimgSKU_ID") as Literal;
                string ddimg = access_sql.GetOneValue("select tuikuanjietu from caiwu where SKU_ID='" + liimgSKU_ID.Text + "'");
                ddimg = ddimg.Replace(filename, "").Replace("||", "|");
                access_sql.T_Update_ExecSql(new string[] { "tuikuanjietu" }, new object[] { ddimg }, "cancipin", "SKU_ID='" + liimgSKU_ID.Text + "'");
                RepeaterItem item = (RepeaterItem)((DataList)source).NamingContainer;
                DataList liimgxs = item.FindControl("DataList1") as DataList;
                bindimgs(liimgxs, ddimg, liimgSKU_ID.Text);
            }

        }
        protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                string filename = e.CommandArgument.ToString();
                Literal liimgSKU_ID = e.Item.FindControl("liimgSKU_ID") as Literal;
                string ddimg = access_sql.GetOneValue("select cancipintupian from caiwu where SKU_ID='" + liimgSKU_ID.Text + "'");
                ddimg = ddimg.Replace(filename, "").Replace("||", "|");
                access_sql.T_Update_ExecSql(new string[] { "cancipintupian" }, new object[] { ddimg }, "cancipin", "SKU_ID='" + liimgSKU_ID.Text + "'");
                RepeaterItem item = (RepeaterItem)((DataList)source).NamingContainer;
                DataList liimgxs = item.FindControl("DataList2") as DataList;
                bindimgs(liimgxs, ddimg, liimgSKU_ID.Text);
            }

        }
        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                Literal litSKU_ID = e.Item.FindControl("SKU_ID") as Literal;
                Literal litTupian = e.Item.FindControl("littuikuanjietu") as Literal;
                DataList dlImages = e.Item.FindControl("DataList1") as DataList;

                Literal litShipin = e.Item.FindControl("litcancipintupian") as Literal;
                DataList dlVideos = e.Item.FindControl("DataList2") as DataList;

                // 绑定图片
                if (litTupian != null && !string.IsNullOrEmpty(litTupian.Text))
                {
                    bindimgs(dlImages, litTupian.Text, litSKU_ID.Text);
                }

                // 绑定视频
                if (litShipin != null && !string.IsNullOrEmpty(litShipin.Text))
                {
                    bindimgs(dlVideos, litShipin.Text, litSKU_ID.Text);
                }


            }
        }
        public void bindimgs(DataList liimgxs, string imgs, string cid)
        {
            string[] iiii = imgs.Split('|');
            DataTable dtimgs = new DataTable();
            dtimgs.Columns.Add("SKU_ID");
            dtimgs.Columns.Add("imgname");

            for (int i = 0; i < iiii.Length; i++)
            {
                if (iiii[i] != "")
                {
                    dtimgs.Rows.Add(new object[] { cid, iiii[i] });
                }
            }
            if (dtimgs.Rows.Count > 0)
            {
                liimgxs.RepeatColumns = dtimgs.Rows.Count;
                liimgxs.DataSource = dtimgs;
                liimgxs.DataBind();
            }
            else
            {
                liimgxs.DataSource = null;
                liimgxs.DataBind();
            }
        }

    }
}