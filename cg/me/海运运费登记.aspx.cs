using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 海运运费登记 : System.Web.UI.Page
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
                if (uid != "12" && uid != "9" && uid != "18" && uid != "19" && uid != "12")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";


        public void bindzhy()
        {
            string where = "";

            string sql = "SELECT top 1 mainimg,cid,rucangyinnibiaoti,dingdanbianhao,dingdanchuangjianshijian,Offer_ID,SKU_ID,huopinbiaoti,shuliang,putianfachushuliang,putianfachuriqi,putianfachudanhao,putianfachukuaidifeiyong,putianfachubeizhu,guangdongfachuriqi,guangdongfachudanhao,guangdongwuliuleixing,guangdongwuliufeiyong,wuliushangmingcheng,guangdongwuliufeiyong_img FROM [SuMaiTongPol].[dbo].[caiwu] where dingdanzhuangtai<>'交易关闭' and isbctg=0 and shangjiabianma='" + txtsjbm.Text.Trim().Replace("'", "''") + "' and (guangdongfachudanhao='" + txtwldh.Text.Trim().Replace("'", "''") + "') ";
            string lx = "";
            string zs = "";


            DataSet ds = access_sql.GreatDs(sql);
            if (access_sql.yzTable(ds))
            {
                if (access_sql.yzTable(ds))
                {
                    DataTable dt = ds.Tables[0];



                    rplb.DataSource = dt;
                    rplb.DataBind();
                    Literal1.Text = "<span style='color:red'>加载" + dt.Rows.Count + "条</span>";

                }

            }
            else
            {
                lits.Text = "无数据"; ;
            }
        }
        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                string filename = e.CommandArgument.ToString();
                Literal liguangdongfachudanhao = e.Item.FindControl("liguangdongfachudanhao") as Literal;
                string ddimg = access_sql.GetOneValue("select top 1 guangdongwuliufeiyong_img from caiwu where guangdongfachudanhao='" + liguangdongfachudanhao.Text + "'");
                ddimg = ddimg.Replace(filename, "").Replace("||", "|");
                access_sql.T_Update_ExecSql(new string[] { "guangdongwuliufeiyong_img" }, new object[] { ddimg }, "caiwu", "guangdongfachudanhao='" + liguangdongfachudanhao.Text + "'");
                RepeaterItem item = (RepeaterItem)((DataList)source).NamingContainer;
                DataList liimgxs = item.FindControl("DataList1") as DataList;
                bindimgs(liimgxs, ddimg, liguangdongfachudanhao.Text);
            }

        }
        public string gettitle(string r)
        {
            string ru = r;
            //if (txttitle.Text.Trim().Length > 0)
            //{
            //    if (ru.IndexOf(txttitle.Text.Trim().Replace("'", "''")) != -1)
            //    {
            //        ru = ru.Replace(txttitle.Text.Trim().Replace("'", "''"), "<span style='color:red;font-weight: bold;'>" + txttitle.Text.Trim().Replace("'", "''") + "</span>");
            //    }
            //}
            return ru;
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            lits.Text = "";
            if (txtsjbm.Text.Trim() != "" && txtwldh.Text.Trim() != "")
            {
                rplb.DataSource = null;
                rplb.DataBind();
                Literal1.Text = "";
                bindzhy();
            }
            else
            {
                lits.Text = "请输入商家编码和物流单号";
                Response.Write("<script>alert('" + lits.Text + "');</script>");
            }


        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        public void bindimgs(DataList liimgxs, string imgs, string cid)
        {
            string[] iiii = imgs.Split('|');
            DataTable dtimgs = new DataTable();
            dtimgs.Columns.Add("guangdongfachudanhao");
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
                string guangdongfachudanhao = (e.CommandArgument).ToString();
                FileUpload ff = e.Item.FindControl("FileUpload1") as FileUpload;
                DataList liimgxs = e.Item.FindControl("DataList1") as DataList;
                string imgs = access_sql.GetOneValue("select top 1  guangdongwuliufeiyong_img from caiwu where guangdongfachudanhao='" + guangdongfachudanhao + "'") + "|";
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
                    access_sql.T_Update_ExecSql(new string[] { "guangdongwuliufeiyong_img" }, new object[] { imgs }, "caiwu", "guangdongfachudanhao='" + guangdongfachudanhao + "'");
                    bindimgs(liimgxs, imgs, guangdongfachudanhao);
                }

            }

            if (e.CommandName == "qr")
            {

                string guangdongfachudanhao = e.CommandArgument.ToString();






                TextBox txtguangdongwuliufeiyong = e.Item.FindControl("txtguangdongwuliufeiyong") as TextBox;
                TextBox txtwuliushangmingcheng = e.Item.FindControl("txtwuliushangmingcheng") as TextBox;
                if (txtguangdongwuliufeiyong.Text.Trim() != "")
                {

                    string[] strs = new string[] { "guangdongwuliufeiyong", "wuliushangmingcheng" };
                    object[] objs = new object[] {
                        txtguangdongwuliufeiyong.Text.Trim().Replace("'", "''"),
                        txtwuliushangmingcheng.Text.Trim().Replace("'", "''")
                    };
                    int cg = access_sql.T_Update_ExecSql(strs, objs, "caiwu", "guangdongfachudanhao='" + guangdongfachudanhao + "'");
                    if (cg > 0)
                    {
                        lits.Text = guangdongfachudanhao + "更新数据成功" + cg + "条";
                    }
                    else
                    {
                        lits.Text = guangdongfachudanhao + "更新数据失败";
                        Response.Write("<script>alert('" + lits.Text + "');</script>");
                    }
                }
                else
                {
                    lits.Text = "请输入费用";
                    Response.Write("<script>alert('" + lits.Text + "');</script>");
                }
            }

        }
        protected void Button2_Click2(object sender, EventArgs e)
        {

        }

        protected void txttitle_TextChanged(object sender, EventArgs e)
        {

        }

        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataList dttupianwangzhi = e.Item.FindControl("DataList1") as DataList;
            Literal liguangdongfachudanhao = e.Item.FindControl("liguangdongfachudanhao") as Literal;
            Literal liguangdongwuliufeiyong_img = e.Item.FindControl("liguangdongwuliufeiyong_img") as Literal;
            if (liguangdongwuliufeiyong_img.Text.Length > 0)
            {

                bindimgs(dttupianwangzhi, liguangdongwuliufeiyong_img.Text, liguangdongfachudanhao.Text);
            }



        }
    }
}