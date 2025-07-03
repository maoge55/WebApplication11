using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 入仓产品图片视频 : System.Web.UI.Page
    {
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
                if (uid != "8" && uid != "9" && uid != "6" && uid != "18" && uid != "19" && uid != "12" && uid != "21")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";







        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }
        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                string filename = e.CommandArgument.ToString();
                Literal liimgSKU_ID = e.Item.FindControl("liimgSKU_ID") as Literal;
                string ddimg = access_sql.GetOneValue("select tupianwangzhi from caiwu where SKU_ID='" + liimgSKU_ID.Text + "'");
                ddimg = ddimg.Replace(filename, "").Replace("||", "|");
                access_sql.T_Update_ExecSql(new string[] { "tupianwangzhi" }, new object[] { ddimg }, "caiwu", "SKU_ID='" + liimgSKU_ID.Text + "'");
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
                string ddimg = access_sql.GetOneValue("select shipinwangzhi from caiwu where SKU_ID='" + liimgSKU_ID.Text + "'");
                ddimg = ddimg.Replace(filename, "").Replace("||", "|");
                access_sql.T_Update_ExecSql(new string[] { "shipinwangzhi" }, new object[] { ddimg }, "caiwu", "SKU_ID='" + liimgSKU_ID.Text + "'");
                RepeaterItem item = (RepeaterItem)((DataList)source).NamingContainer;
                DataList liimgxs = item.FindControl("DataList2") as DataList;
                bindimgs(liimgxs, ddimg, liimgSKU_ID.Text);
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
        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {


            if (e.CommandName == "upimg")
            {
                lits.Text = "";
                string SKU_ID = (e.CommandArgument).ToString();
                FileUpload ff = e.Item.FindControl("FileUpload1") as FileUpload;
                DataList liimgxs = e.Item.FindControl("DataList1") as DataList;
                string imgs = access_sql.GetOneValue("select top 1 tupianwangzhi from caiwu where SKU_ID='" + SKU_ID + "'") + "|";
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
                    access_sql.T_Update_ExecSql(new string[] { "tupianwangzhi" }, new object[] { imgs }, "caiwu", "SKU_ID='" + SKU_ID + "'");
                    bindimgs(liimgxs, imgs, SKU_ID);
                }

            }


            if (e.CommandName == "upsp")
            {
                lits.Text = "";
                string SKU_ID = (e.CommandArgument).ToString();
                FileUpload ff = e.Item.FindControl("FileUpload2") as FileUpload;
                DataList liimgxs = e.Item.FindControl("DataList2") as DataList;
                string imgs = access_sql.GetOneValue("select top 1 shipinwangzhi from caiwu where SKU_ID='" + SKU_ID + "'") + "|";
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
                    access_sql.T_Update_ExecSql(new string[] { "shipinwangzhi" }, new object[] { imgs }, "caiwu", "SKU_ID='" + SKU_ID + "'");
                    bindimgs(liimgxs, imgs, SKU_ID);
                }

            }

        }





        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataList dttupianwangzhi = e.Item.FindControl("DataList1") as DataList;
            Literal lirucangitemid = e.Item.FindControl("lirucangitemid") as Literal;
            Literal litupianwangzhi = e.Item.FindControl("litupianwangzhi") as Literal;
            if (litupianwangzhi.Text.Length > 0)
            {

                bindimgs(dttupianwangzhi, litupianwangzhi.Text, lirucangitemid.Text);
            }


            DataList dtshipinwangzhi = e.Item.FindControl("DataList2") as DataList;

            Literal lishipinwangzhi = e.Item.FindControl("lishipinwangzhi") as Literal;
            if (litupianwangzhi.Text.Length > 0)
            {

                bindimgs(dtshipinwangzhi, lishipinwangzhi.Text, lirucangitemid.Text);
            }





        }

        protected void Button2_Click2(object sender, EventArgs e)
        {


        }
        public void bindbtkey()
        {
            string where = " ";
            string sql = "";
            where = " where dingdanzhuangtai<>'交易关闭' and isbctg=0 and shangjiabianma='" + txtsjbm.Text.Trim().Replace("'", "''") + "' ";
            if (txtrucangITEMID.Text.Trim() != "")
            {

                where += " and  rucangITEMID = '" + txtrucangITEMID.Text.Trim().Replace("'", "''") + "' ";
            }
            if (txtrucangyinnibiaoti.Text.Trim() != "")
            {
                where += " and  rucangyinnibiaoti like '%" + txtrucangyinnibiaoti.Text.Trim().Replace("'", "''") + "%' ";
            }

            DataSet ds = access_sql.GreatDs("SELECT c1.* FROM caiwu c1 INNER JOIN (SELECT SKU_ID,MIN(cid) AS min_id FROM caiwu WHERE rucangitemid in (select rucangitemid from caiwu " + where + " group by rucangitemid) GROUP BY SKU_ID) c2 ON c1.cid = c2.min_id");
            if (access_sql.yzTable(ds))
            {
                if (access_sql.yzTable(ds))
                {
                    DataTable dt = ds.Tables[0];
                 
                    rplb.DataSource = dt;
                    rplb.DataBind();
                    Literal1.Text = "<span style='color:red'>加载数据" + dt.Rows.Count + "条</span>";

                }

            }
            else
            {
                lits.Text = "无数据"; ;
            }
        }

        protected void Button3_Click2(object sender, EventArgs e)
        {

        }


        protected void Button3_Click3(object sender, EventArgs e)
        {
            lits.Text = "";

            if (txtsjbm.Text.Trim() != "" && (txtrucangITEMID.Text.Trim() != "" || txtrucangyinnibiaoti.Text.Trim() != ""))
            {
                rplb.DataSource = null;
                rplb.DataBind();
                Literal1.Text = "";
                bindbtkey();
            }
            else
            {
                lits.Text = "请输入搜索";

            }
        }
    }
}