using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 仓库 : System.Web.UI.Page
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
                if (uid != "8" && uid != "9" && uid != "6" && uid != "12")
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

            string where = " shangjiabianma='" + txtsjbm.Text.Trim().Replace("'", "''") + "' ";

            if (dpzt.SelectedValue != "全部")
            {
                where += " and dingdanzhuangtai='" + dpzt.SelectedValue + "' and";
            }
            else
            {
                where += " and";
            }
            DataSet ds = access_sql.GreatDs("select * from caiwu where " + where + "  yundanhao_1688 like '%" + txtkddh.Text.Trim().Replace("'", "''") + "%' and (caigouxucaozuo='' or caigouxucaozuo is null)");
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



        protected void Button1_Click(object sender, EventArgs e)
        {
            lits.Text = "";
            if (txtsjbm.Text.Trim() != "")
            {
                if (txtkddh.Text.Trim() != "")
                {
                    txtbtkey.Text = "";
                    txtddh.Text = "";
                    rplb.DataSource = null;
                    rplb.DataBind();
                    Literal1.Text = "";
                    bindzhy();
                }
                else
                {
                    lits.Text = "请输入快递单号";
                }
            }
            else
            {
                lits.Text = "请输入商家编码";
            }
        }

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
                Literal licid = e.Item.FindControl("licid") as Literal;
                string ddimg = access_sql.GetOneValue("select tuihuotupian_1688 from caiwu where cid=" + licid.Text + "");
                ddimg = ddimg.Replace(filename, "").Replace("||", "|");
                access_sql.T_Update_ExecSql(new string[] { "tuihuotupian_1688" }, new object[] { ddimg }, "caiwu", "cid=" + licid.Text + "");
                RepeaterItem item = (RepeaterItem)((DataList)source).NamingContainer;
                DataList liimgxs = item.FindControl("DataList1") as DataList;
                bindimgs(liimgxs, ddimg, int.Parse(licid.Text));
            }

        }
        public void bindimgs(DataList liimgxs, string imgs, int cid)
        {
            string[] iiii = imgs.Split('|');
            DataTable dtimgs = new DataTable();
            dtimgs.Columns.Add("cid");
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

            if (e.CommandName == "upmainimg")
            {
                lits.Text = "";
                Literal licid = e.Item.FindControl("licid") as Literal;
                Literal limainimg = e.Item.FindControl("limainimg") as Literal;
                Literal liOffer_ID = e.Item.FindControl("liOffer_ID") as Literal;
                Literal liSKU_ID = e.Item.FindControl("liSKU_ID") as Literal;
                string where = "";
                if (licid.Text != "")
                {
                    where = " cid=" + licid.Text + "";
                }
                if (liOffer_ID.Text != "")
                {
                    where = " Offer_ID='" + liOffer_ID.Text + "'";
                }
                if (liSKU_ID.Text != "")
                {
                    where = " SKU_ID='" + liSKU_ID.Text + "'";
                }
                FileUpload ff = e.Item.FindControl("FileUpload2") as FileUpload;

                string imgs = access_sql.GetOneValue("select top 1  mainimg from caiwu where " + where) + "|";
                for (int i = 0; i < ff.PostedFiles.Count; i++)
                {
                    string type = ff.PostedFiles[i].FileName.Split('.')[ff.PostedFiles[i].FileName.Split('.').Length - 1];
                    string imgname = DateTime.Now.ToString("yyyyMMddHHmmss") + i + "." + type;
                    string savename = Server.MapPath("~/Uploads/") + imgname;
                    ff.PostedFiles[i].SaveAs(savename);
                    imgs = imgname;
                }
                if (imgs != "")
                {
                    imgs = imgs.Replace("||", "|");
                    access_sql.T_Update_ExecSql(new string[] { "mainimg" }, new object[] { imgs }, "caiwu", where);
                    limainimg.Visible = true;
                    limainimg.Text = "";
                    limainimg.Text = " <img src='/Uploads/" + imgs + "' style='width:300px; height:300px' />";
                }

            }
            if (e.CommandName == "upxx")
            {
                lits.Text = "";

                Literal licid = e.Item.FindControl("licid") as Literal;
                TextBox txtshangjiabianma = e.Item.FindControl("txtshangjiabianma") as TextBox;
                TextBox txtshijishouhuoshuliang = e.Item.FindControl("txtshijishouhuoshuliang") as TextBox;
                TextBox txtxutuihuoshuliang = e.Item.FindControl("txtxutuihuoshuliang") as TextBox;
                TextBox txtdingdanbeizhu = e.Item.FindControl("txtdingdanbeizhu") as TextBox;


                if (licid.Text != "" && txtshangjiabianma.Text != "")
                {
                    List<string> listr = new List<string>();
                    List<object> liobj = new List<object>();

                    listr.Add("shangjiabianma");
                    liobj.Add(txtshangjiabianma.Text.Trim().Replace("'", "''"));


                    listr.Add("shijishouhuoshuliang");
                    liobj.Add(txtshijishouhuoshuliang.Text.Trim().Replace("'", "''"));

                    listr.Add("xutuihuoshuliang");
                    liobj.Add(txtxutuihuoshuliang.Text.Trim().Replace("'", "''"));


                    listr.Add("dingdanbeizhu");
                    liobj.Add(txtdingdanbeizhu.Text.Trim().Replace("'", "''"));


                    if (access_sql.T_Update_ExecSql(listr.ToArray(), liobj.ToArray(), "caiwu", "cid=" + licid.Text + "") > 0)
                    {

                        Response.Write("<script>alert('ID:" + licid.Text + "修改信息成功')</script>");

                    }
                }
                else
                {
                    Response.Write("<script>alert('商家编码必填')</script>");
                }


            }
            if (e.CommandName == "updingdanbeizhu")
            {
                lits.Text = "";
                string id = e.CommandArgument.ToString().Split('-')[0];
                string md = e.CommandArgument.ToString().Split('-')[1];
                Literal licid = e.Item.FindControl("licid") as Literal;
                TextBox txtdingdanbeizhu = e.Item.FindControl("txtdingdanbeizhu") as TextBox;

                if (licid.Text != "")
                {
                    if (access_sql.T_Update_ExecSql(new string[] { "dingdanbeizhu" }, new object[] { txtdingdanbeizhu.Text.Trim().Replace("'", "''") }, "caiwu", "cid=" + licid.Text + "") > 0)
                    {

                        Response.Write("<script>alert('ID:" + licid.Text + "修改订单备注成功')</script>");

                    }
                }
                else
                {
                    Response.Write("<script>alert('改id还没匹配skuid')</script>");
                }


            }



            if (e.CommandName == "upcz")
            {
                lits.Text = "";
                string id = e.CommandArgument.ToString().Split('-')[0];
                string md = e.CommandArgument.ToString().Split('-')[1];
                Literal licid = e.Item.FindControl("licid") as Literal;
                TextBox txt_caizhi = e.Item.FindControl("txt_caizhi") as TextBox;

                if (licid.Text != "")
                {
                    if (access_sql.T_Update_ExecSql(new string[] { "caizhi" }, new object[] { txt_caizhi.Text.Trim().Replace("'", "''") }, "caiwu", "cid=" + licid.Text + "") > 0)
                    {

                        Response.Write("<script>alert('ID:" + licid.Text + "修改材质成功')</script>");

                    }
                }
                else
                {
                    Response.Write("<script>alert('改id还没匹配skuid')</script>");
                }


            }
            if (e.CommandName == "up")
            {
                lits.Text = "";
                int cid = Convert.ToInt32(e.CommandArgument);
                FileUpload ff = e.Item.FindControl("FileUpload1") as FileUpload;
                DataList liimgxs = e.Item.FindControl("DataList1") as DataList;
                string imgs = access_sql.GetOneValue("select tuihuotupian_1688 from caiwu where cid=" + cid + "") + "|";
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
                    access_sql.T_Update_ExecSql(new string[] { "tuihuotupian_1688" }, new object[] { imgs }, "caiwu", "cid=" + cid + "");
                    bindimgs(liimgxs, imgs, cid);
                }

            }
            if (e.CommandName == "qr")
            {
                lits.Text = "";
                int cid = Convert.ToInt32(e.CommandArgument);
                if (access_sql.T_Update_ExecSql(new string[] { "caigouxucaozuo" }, new object[] { "可以确认收货" }, "caiwu", "cid=" + cid + "") > 0)
                {
                    bindzhy();
                    lits.Text = "ID:" + cid + "改成确认收货";
                }

            }
            if (e.CommandName == "tk")
            {
                lits.Text = "";
                int cid = Convert.ToInt32(e.CommandArgument);
                TextBox txttkbz = e.Item.FindControl("txttkbz") as TextBox;




                if (access_sql.T_Update_ExecSql(new string[] { "caigouxucaozuo", "tuikuanbeizhu" }, new object[] { "申请退款", txttkbz.Text.Trim().Replace("'", "''") }, "caiwu", "cid=" + cid + "") > 0)
                {
                    bindzhy();
                    lits.Text = "ID:" + cid + "改成需退款成功";
                }

            }
        }

        public void clzy()
        {
            int cg = 0;
            for (int i = 0; i < rplb.Items.Count; i++)
            {
                Literal id = (Literal)rplb.Items[i].FindControl("liid");
                TextBox txtY_1688url = rplb.Items[i].FindControl("txtY_1688url") as TextBox;
                TextBox txtY_1688sku1 = rplb.Items[i].FindControl("txtY_1688sku1") as TextBox;
                TextBox txtY_1688sku2 = rplb.Items[i].FindControl("txtY_1688sku2") as TextBox;
                TextBox txtY_1688sku3 = rplb.Items[i].FindControl("txtY_1688sku3") as TextBox;
                TextBox txtY_1688price = rplb.Items[i].FindControl("txtY_1688price") as TextBox;
                cg += access_sql.T_Update_ExecSql(new string[] { "Y_1688url", "Y_1688sku1", "Y_1688sku2", "Y_1688sku3", "Y_1688price" }, new object[] { txtY_1688url.Text.Trim().Replace("'", "''"), txtY_1688sku1.Text.Trim().Replace("'", "''"), txtY_1688sku2.Text.Trim().Replace("'", "''"), txtY_1688sku3.Text.Trim().Replace("'", "''"), txtY_1688price.Text.Trim().Replace("'", "''") }, "YNBigData", "id=" + id + "");
            }
            bindzhy();
            lits.Text = "更新成功" + cg + "个";
        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            clzy();
        }

        protected void Button3_Click1(object sender, EventArgs e)
        {
            clzy();
        }

        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataList liimgxs = e.Item.FindControl("DataList1") as DataList;
            Literal liid = e.Item.FindControl("liid") as Literal;
            Literal liimages = e.Item.FindControl("liimages") as Literal;
            if (liimages.Text.Length > 0)
            {

                bindimgs(liimgxs, liimages.Text, int.Parse(liid.Text));
            }

            Literal limainimg = e.Item.FindControl("limainimg") as Literal;
            Literal limainimg222 = e.Item.FindControl("limainimg222") as Literal;
            limainimg.Visible = true;
            limainimg.Text = " <img src='/Uploads/" + limainimg222.Text + "' style='width:300px; height:300px' />";



        }

        protected void Button2_Click2(object sender, EventArgs e)
        {
            lits.Text = "";
            if (txtsjbm.Text.Trim() != "")
            {
                if (txtbtkey.Text.Trim() != "")
                {
                    txtkddh.Text = "";
                    txtddh.Text = "";
                    rplb.DataSource = null;
                    rplb.DataBind();
                    Literal1.Text = "";
                    bindbtkey();
                }
                else
                {
                    lits.Text = "请输入搜索词";
                }
            }
            else
            {
                lits.Text = "请输入商家编码";
            }

        }
        public void bindbtkey()
        {

            string where = " shangjiabianma='" + txtsjbm.Text.Trim().Replace("'", "''") + "' ";

            if (dpzt.SelectedValue != "全部")
            {
                where += " and dingdanzhuangtai='" + dpzt.SelectedValue + "' and";
            }
            else
            {
                where += " and";
            }
            DataSet ds = access_sql.GreatDs("select * from caiwu where " + where + " huopinbiaoti like '%" + txtbtkey.Text.Trim().Replace("'", "''") + "%' and (caigouxucaozuo='' or caigouxucaozuo is null) order by cid desc");
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
            lits.Text = "";
            if (txtsjbm.Text.Trim() != "")
            {
                if (txtddh.Text.Trim() != "")
                {
                    txtkddh.Text = "";
                    txtbtkey.Text = "";
                    rplb.DataSource = null;
                    rplb.DataBind();
                    Literal1.Text = "";
                    bindddh();
                }
                else
                {
                    lits.Text = "输入1688订单号";
                }
            }
            else
            {
                lits.Text = "请输入商家编码";
            }
        }
        public void bindddh()
        {
            string where = " shangjiabianma='" + txtsjbm.Text.Trim().Replace("'", "''") + "' ";

            if (dpzt.SelectedValue != "全部")
            {
                where += " and dingdanzhuangtai='" + dpzt.SelectedValue + "' and";
            }
            else
            {
                where += " and";
            }

            DataSet ds = access_sql.GreatDs("select * from caiwu where  " + where + " dingdanbianhao = '" + txtddh.Text.Trim().Replace("'", "''") + "'");
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

        protected void Button4_Click(object sender, EventArgs e)
        {
            int cg = 0;
            bool ky = true;




            for (int i = 0; i < rplb.Items.Count; i++)
            {

                TextBox txtshangjiabianma = rplb.Items[i].FindControl("txtshangjiabianma") as TextBox;




                if (txtshangjiabianma.Text == "")
                {
                    ky = false;
                    lits.Text = "第" + (i + 1) + "的商家编码没填";
                    Response.Write("<script>alert('" + lits.Text + "');</script>");
                    break;
                }



            }
            if (ky)
            {

                for (int i = 0; i < rplb.Items.Count; i++)
                {
                    Literal licid = rplb.Items[i].FindControl("licid") as Literal;
                    TextBox txtshangjiabianma = rplb.Items[i].FindControl("txtshangjiabianma") as TextBox;
                    TextBox txtshijishouhuoshuliang = rplb.Items[i].FindControl("txtshijishouhuoshuliang") as TextBox;
                    TextBox txtxutuihuoshuliang = rplb.Items[i].FindControl("txtxutuihuoshuliang") as TextBox;
                    TextBox txtdingdanbeizhu = rplb.Items[i].FindControl("txtdingdanbeizhu") as TextBox;


                    List<string> listr = new List<string>();
                    List<object> liobj = new List<object>();

                    listr.Add("shangjiabianma");
                    liobj.Add(txtshangjiabianma.Text.Trim().Replace("'", "''"));


                    listr.Add("shijishouhuoshuliang");
                    liobj.Add(txtshijishouhuoshuliang.Text.Trim().Replace("'", "''"));

                    listr.Add("xutuihuoshuliang");
                    liobj.Add(txtxutuihuoshuliang.Text.Trim().Replace("'", "''"));


                    listr.Add("dingdanbeizhu");
                    liobj.Add(txtdingdanbeizhu.Text.Trim().Replace("'", "''"));


                    cg += access_sql.T_Update_ExecSql(listr.ToArray(), liobj.ToArray(), "caiwu", "cid=" + licid.Text + "");

                }


                Response.Write("<script>alert('修改信息成功" + cg + "条')</script>");


            }
        }
    }
}