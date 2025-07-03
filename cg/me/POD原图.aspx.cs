using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class POD原图 : System.Web.UI.Page
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
                if (uid != "11" && uid != "9" && uid != "6" && uid != "16")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public string bindzzzzimg(string instr)
        {
            string ru = "";
            instr = instr.Replace("[", "").Replace("]", "").Trim().Replace(" ", "").Replace("\"", "");
            string[] iiiii = instr.Split(',');
            for (int i = 0; i < iiiii.Length; i++)
            {
                if(iiiii[i].Trim()!="")
                {
                    ru += "<a href='" + iiiii[i].Trim() + "' target='_blank'><img src='" + iiiii[i].Trim() + "' style='width:150px !important;height:auto!important'></a>";
                }
            }
            return ru;
        }
        public void bindzhy()
        {

            DataSet dsdp = access_sql.GreatDs("select id,image,images, yntitlercode,PODyuantu from ProShopeePh where datatype=2 and yntitlercode like '%" + txtsjm.Text.Trim().Replace("'", "''") + "%'");
            if (access_sql.yzTable(dsdp))
            {

                rplb.DataSource = dsdp.Tables[0];
                rplb.DataBind();
                Literal1.Text = "<span style='color:red'>加载数据" + dsdp.Tables[0].Rows.Count + "条</span>";



            }
            else
            {
                lits.Text = "无数据"; ;
            }
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





        }
        public void bindimgs(DataList liimgxs, string imgs, int id)
        {
            string[] iiii = imgs.Split('|');
            DataTable dtimgs = new DataTable();
            dtimgs.Columns.Add("id");
            dtimgs.Columns.Add("imgname");

            for (int i = 0; i < iiii.Length; i++)
            {
                if (iiii[i] != "")
                {
                    dtimgs.Rows.Add(new object[] { id, iiii[i] });
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
        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                string filename = e.CommandArgument.ToString();
                Literal liid = e.Item.FindControl("liid") as Literal;
                string ddimg = access_sql.GetOneValue("select PODyuantu from ProShopeePh where id=" + liid.Text + "");
                ddimg = ddimg.Replace(filename, "").Replace("||", "|");
                access_sql.T_Update_ExecSql(new string[] { "PODyuantu" }, new object[] { ddimg }, "ProShopeePh", "id=" + liid.Text + "");
                RepeaterItem item = (RepeaterItem)((DataList)source).NamingContainer;
                DataList liimgxs = item.FindControl("DataList1") as DataList;
                bindimgs(liimgxs, ddimg, int.Parse(liid.Text));
            }

        }
        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {


            if (e.CommandName == "up")
            {
                lits.Text = "";
                int id = Convert.ToInt32(e.CommandArgument);
                FileUpload ff = e.Item.FindControl("FileUpload1") as FileUpload;
                DataList liimgxs = e.Item.FindControl("DataList1") as DataList;
                string imgs = access_sql.GetOneValue("select PODyuantu from ProShopeePh where id=" + id + "") + "|";
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
                    access_sql.T_Update_ExecSql(new string[] { "PODyuantu" }, new object[] { imgs }, "ProShopeePh", "id=" + id + "");
                    bindimgs(liimgxs, imgs, id);
                }

            }
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            lits.Text = "";
            if (txtsjm.Text.Trim() != "" && txtsjm.Text.Trim().Length == 8)
            {
                bindzhy();
            }
            else
            {
                lits.Text = "请输入8位数随机码";
                Response.Write("<script>alert('" + lits.Text + "');</script>");
            }

        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }







        protected void Button2_Click2(object sender, EventArgs e)
        {
            cl();
        }

        protected void Button3_Click2(object sender, EventArgs e)
        {
            cl();
        }
        public void cl()
        {
            lits.Text = "";
            string yes_dp = "";
            string or_dp = "";
            string no_dp = "";
            bool ky = true;
            for (int i = 0; i < rplb.Items.Count; i++)
            {

                Literal lishopid = (Literal)rplb.Items[i].FindControl("lishopid");
                RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");
                if (rdjg.SelectedIndex == -1)
                {
                    ky = false;
                    Response.Write("<script>alert('还有未审核的数据，请检查');</script>");
                    break;
                }
                else
                {
                    if (rdjg.SelectedIndex == 0)
                    {
                        yes_dp += "'" + lishopid.Text + "',";
                    }
                    else if (rdjg.SelectedIndex == 1)
                    {
                        or_dp += "'" + lishopid.Text + "',";
                    }
                    else if (rdjg.SelectedIndex == 2)
                    {
                        no_dp += "'" + lishopid.Text + "',";
                    }
                }
            }
            if (ky)
            {
                int cg = 0;
                if (yes_dp != "")
                {
                    yes_dp = yes_dp.Substring(0, yes_dp.Length - 1);
                    cg += access_sql.T_Update_ExecSql(new string[] { "ispoddp" }, new object[] { 1 }, "ProShopeePh", "datatype=2 and shopid in (" + yes_dp + ")");
                    string[] dps = yes_dp.Split(',');
                    for (int i = 0; i < dps.Length; i++)
                    {
                        if (dps[i] != "")
                        {

                            if (!IsNumeric(dps[i]))
                            {
                                try
                                {
                                    access_sql.T_Insert_ExecSql(new string[] { "ShopID" }, new object[] { dps[i].ToString().Replace("'", "") }, "AmazonShop");
                                }
                                catch
                                {


                                }
                            }

                        }
                    }

                }
                if (or_dp != "")
                {
                    or_dp = or_dp.Substring(0, or_dp.Length - 1);
                    cg += access_sql.T_Update_ExecSql(new string[] { "ispoddp" }, new object[] { 2 }, "ProShopeePh", "datatype=2 and shopid in (" + or_dp + ")");
                }
                if (no_dp != "")
                {
                    no_dp = no_dp.Substring(0, no_dp.Length - 1);
                    cg += access_sql.T_Update_ExecSql(new string[] { "ispoddp" }, new object[] { -1 }, "ProShopeePh", "datatype=2 and shopid in (" + no_dp + ")");
                }
                lits.Text = "成功更新" + cg + "个店铺";
                rplb.DataSource = null;
                rplb.DataBind();
                Literal1.Text = "<span style='color:red'>请点击按钮，加载新数据</span>";
            }
        }
        public bool IsNumeric(string str)
        {
            Regex regex = new Regex(@"^\d+(\.\d+)?$");
            return regex.IsMatch(str);
        }
    }
}