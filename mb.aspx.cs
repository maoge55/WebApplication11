using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication11
{
    public partial class a : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!access_sql.yzdl())
            {
                Response.Redirect("/login.aspx");
            }
            else
            {
                u = HttpContext.Current.Request.Cookies["u"].Value;
                p = HttpContext.Current.Request.Cookies["p"].Value;
                uid = HttpContext.Current.Request.Cookies["uid"].Value;
                if (uid == "4" || uid == "6" || uid == "9")
                {
                    txtlmrow.Text = "3";
                    txttablename.Text = "Template";
                }
                if (uid == "10")
                {
                    txtlmrow.Text = "1";
                    txttablename.Text = "TShirts";
                }
                if (HttpContext.Current.Request.Cookies["eee"] != null)
                {
                    lg.Visible = false;
                    zc.Visible = true;
                }
            }
            if (!IsPostBack)
            {

                bind();
            }
        }
        public string getpathname(string ppp)
        {
            string rr = "";
            string aaa = (ppp.Split('\\')[ppp.ToString().Split('\\').Length - 1]);
            //if (aaa.Length > 25)
            //{
            //    aaa = aaa.Substring(0, 200) + "...";
            //}
            rr = aaa;
            return rr;

        }
        public string getpathname_(string ppp)
        {
            string rr = "";
            rr = ppp;
            if (rr.Length > 40)
            {
                rr = rr.Substring(0, 40) + "...";
            }

            return rr;

        }
        public string u = "";
        public string p = "";
        public string uid = "";
        public void bind()
        {
            string where = "";
            string sql = "";
            string xsss = "";
            string order = " order by pcount desc";
            if (Request.QueryString["type"] != null && Request.QueryString["type"] != "")
            {
                if (Request.QueryString["type"] != "999")
                {
                    drtype.SelectedValue = Request.QueryString["type"];

                    where += " and (dstate='" + Request.QueryString["type"] + "')";

                    xsss += "<br>模板状态：" + drtype.SelectedItem.Text;
                }
            }
            if (Request.QueryString["pdtype"] != null && Request.QueryString["pdtype"] != "")
            {
                if (Request.QueryString["pdtype"] != "999")
                {
                    drpd.SelectedValue = Request.QueryString["pdtype"];
                    if (drpd.SelectedValue == "1")
                    {
                        where += " and (pcount>0)";
                        order = " order by pcount desc";
                    }
                    else
                    {
                        where += " and (pcount=0)";
                    }

                    xsss += "<br>采集状态：" + drpd.SelectedItem.Text;
                }
            }
            if (Request.QueryString["xltype"] != null && Request.QueryString["xltype"] != "")
            {
                if (Request.QueryString["xltype"] != "999")
                {
                    drxl.SelectedValue = Request.QueryString["xltype"];
                    if (drxl.SelectedValue == "1")
                    {
                        where += " and (pxl>0)";
                        order = " order by pxl desc";
                    }
                    else
                    {
                        where += " and (pxl=0)";
                        order = " order by pcount desc"; ;
                    }
                    xsss += "<br>销量状态：" + drxl.SelectedItem.Text;
                }
            }
            if (Request.QueryString["color"] != null && Request.QueryString["color"] != "")
            {
                if (Request.QueryString["color"] != "999")
                {
                    drcolor.SelectedValue = Request.QueryString["color"];



                    where += " and (color=" + drcolor.SelectedValue + ")";

                    xsss += "<br>黑白状态：" + drcolor.SelectedItem.Text;
                }
            }
            if (Request.QueryString["dddtype"] != null && Request.QueryString["dddtype"] != "")
            {
                if (Request.QueryString["dddtype"] != "999")
                {
                    DDDtype.SelectedValue = Request.QueryString["dddtype"];

                    where += " and (dtype=" + Request.QueryString["dddtype"] + ")";

                    xsss += "<br>状态：" + DDDtype.SelectedItem.Text;
                }
            }

            if (Request.QueryString["name"] != null && Request.QueryString["name"] != "")
            {
                txtsname.Text = Request.QueryString["name"];
                where += " and (dname like'%" + Request.QueryString["name"].Replace("'", "''") + "%')";
                xsss += "<br>名称：" + txtsname.Text;
            }
            if (Request.QueryString["qj"] != null && Request.QueryString["qj"] != "")
            {
                string qj = Request.QueryString["qj"];
                txtqj.Text = qj;
                where += " and did BETWEEN 开始 AND 结束";
                if (Request.QueryString["qj"].IndexOf("-") != -1)
                {
                    where = where.Replace("开始", qj.Split('-')[0]).Replace("结束", qj.Split('-')[1]);
                }
                else
                {
                    where = where.Replace("开始", qj).Replace("结束", qj);
                }
                xsss += "<br>区间：" + Request.QueryString["qj"];
            }

            if (where == "")
            {
                sql = "SELECT top 20 mb.did,mb.dname,mb.dfile,mb.drow,mb.dsearchtxt,mb.duid,mb.dxs,mb.dzd,mb.dtablename,mb.dstate,count(lb.lid) AS count,mb.pcount,mb.pxl FROM mb LEFT JOIN lb ON mb.did = lb.ldid  where mb.dxs=1 and mb.duid=" + uid + " GROUP BY  mb.did, mb.dname, mb.dfile, mb.drow, mb.dsearchtxt, mb.duid, mb.dxs, mb.dzd,mb.dtablename,mb.dstate,mb.pcount,mb.pxl  " + order;
            }
            else
            {
                sql = "SELECT  mb.did,mb.dname,mb.dfile,mb.drow,mb.dsearchtxt,mb.duid,mb.dxs,mb.dzd,mb.dtablename,mb.dstate,count(lb.lid) AS count,mb.pcount,mb.pxl FROM mb LEFT JOIN lb ON mb.did = lb.ldid  where mb.dxs=1 and mb.duid=" + uid + " " + where + " GROUP BY  mb.did, mb.dname, mb.dfile, mb.drow, mb.dsearchtxt, mb.duid, mb.dxs, mb.dzd,mb.dtablename,mb.dstate,mb.pcount,mb.pxl   " + order;
            }

            DataSet ds = access_sql.GreatDs(sql);
            if (access_sql.yzTable(ds))
            {
                if (xsss != "") { xsss = "<br>检索条件" + xsss; }
                showmewss("显示数量" + ds.Tables[0].Rows.Count.ToString() + "<br>" + xsss);
                rplb.DataSource = ds;
                rplb.DataBind();
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {

            if (fup1.FileName != "")
            {
                if (access_sql.GetOneValue("select count(*) from mb where dname='" + txtmbname.Text.Trim().Replace("'", "''") + "' and duid=" + uid + "") == "0")
                {
                    string ext = System.IO.Path.GetExtension(fup1.FileName);
                    if (ext == ".xlsm" || ext == ".xlsx" || ext == ".csv")
                    {
                        if (!Directory.Exists(Server.MapPath("/document" + uid + "/")))
                        {
                            Directory.CreateDirectory(Server.MapPath("/document" + uid + "/"));
                        }
                        string path = Server.MapPath("/document" + uid + "/") + fup1.FileName;
                        if (works.yzkong(new List<TextBox> { txtlmrow, txtmbname, txtsearchtxt, txttablename }))
                        {
                            this.fup1.SaveAs(path);
                            string dname = txtmbname.Text.Trim();
                            string leimu = "";
                            if (dname.Contains("(") && dname.Contains(")"))
                            {
                                dname = dname.Replace(")", "");
                                leimu = dname.Split('(')[dname.Split('(').Length - 1];

                            }

                            if (access_sql.T_Insert_ExecSql(new string[] { "dname", "drow", "dfile", "duid", "dsearchtxt", "dtablename", "leimu" }, new object[] { txtmbname.Text.Trim(), txtlmrow.Text.Trim(), path, uid, txtsearchtxt.Text.Trim(), txttablename.Text.Trim(), leimu }, "mb") != 0)
                            {
                                bind();
                                txtmbname.Text = "";
                                txtsearchtxt.Text = "";
                                showmewss("添加成功");
                            }
                        }
                        else
                        {
                            showmewss("不能为空");
                        }


                    }
                    else
                    {
                        showmewss("文件格式不对");
                    }
                }
                else
                {
                    showmewss("模板名称已经存在");
                }
            }
            else
            {
                showmewss("请选择文件");
            }
        }

        public void showmewss(string mess)
        {
            lits.Text = mess;
            // Response.Write("<script>alert(\"" + mess + "\")</script>");
        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "zd")
            {
                int did = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("/b.aspx?did=" + did);
                // Response.Write("<script>window.open('/b.aspx?did=" + did + "')</script>");
                //string path = access_sql.GetOneValue("select  dfile from mb where did=" + did + " order by did");
                //List<string> ru = works.GetHeadnames(path);
                //foreach (var item in ru)
                //{
                //    lits.Text += item + "|";
                //}

            }
            if (e.CommandName == "del")
            {
                int did = Convert.ToInt32(e.CommandArgument);
                access_sql.DoSql("update mb set dxs=0 where did=" + did + "");
                bind();
                showmewss("删除成功");
            }
            if (e.CommandName == "update")
            {
                int did = Convert.ToInt32(e.CommandArgument);
                Literal liname = e.Item.FindControl("liname") as Literal;
                liname.Visible = false;
                Literal lipath = e.Item.FindControl("lipath") as Literal;
                lipath.Visible = false;

                Literal lirow = e.Item.FindControl("lirow") as Literal;
                lirow.Visible = false;

                Literal lisearchtxt = e.Item.FindControl("lisearchtxt") as Literal;
                lisearchtxt.Visible = false;

                TextBox txtname = e.Item.FindControl("txtname") as TextBox;
                txtname.Visible = true;
                TextBox txtrow = e.Item.FindControl("txtrow") as TextBox;
                txtrow.Visible = true;

                TextBox txtsearchtxt = e.Item.FindControl("txtsearchtxt") as TextBox;
                txtsearchtxt.Visible = true;

                FileUpload fpp = e.Item.FindControl("fpp") as FileUpload;
                fpp.Visible = true;
                LinkButton btnsave = e.Item.FindControl("btnsave") as LinkButton;
                btnsave.Visible = true;
                LinkButton btnup = e.Item.FindControl("btnup") as LinkButton;
                btnup.Visible = false;


                Literal litablename = e.Item.FindControl("litablename") as Literal;
                litablename.Visible = false;

                TextBox txttablename = e.Item.FindControl("txttablename") as TextBox;
                txttablename.Visible = true;


                showmewss(did.ToString());
            }

            if (e.CommandName == "save")
            {
                int did = Convert.ToInt32(e.CommandArgument);
                TextBox txtname = e.Item.FindControl("txtname") as TextBox;
                Literal liname = e.Item.FindControl("liname") as Literal;
                Literal lipath = e.Item.FindControl("lipath") as Literal;
                Literal liallpath = e.Item.FindControl("liallpath") as Literal;
                Literal lirow = e.Item.FindControl("lirow") as Literal;
                TextBox txtrow = e.Item.FindControl("txtrow") as TextBox;


                FileUpload fpp = e.Item.FindControl("fpp") as FileUpload;
                LinkButton btnsave = e.Item.FindControl("btnsave") as LinkButton;
                LinkButton btnup = e.Item.FindControl("btnup") as LinkButton;

                Literal lisearchtxt = e.Item.FindControl("lisearchtxt") as Literal;
                TextBox txtsearchtxt = e.Item.FindControl("txtsearchtxt") as TextBox;



                Literal litablename = e.Item.FindControl("litablename") as Literal;


                TextBox txttablename = e.Item.FindControl("txttablename") as TextBox;



                string name = txtname.Text.Trim();
                string searchtxt = txtsearchtxt.Text.Trim();
                string row = txtrow.Text.Trim();
                string path = "";
                bool next = true;
                if (fpp.FileName != "")
                {
                    string ext = System.IO.Path.GetExtension(fpp.FileName);
                    if (ext == ".xlsm" || ext == ".xlsx" || ext == ".csv")
                    {
                        if (!Directory.Exists(Server.MapPath("/document" + uid + "/")))
                        {
                            Directory.CreateDirectory(Server.MapPath("/document" + uid + "/"));
                        }
                        path = Server.MapPath("/document" + uid + "/") + fpp.FileName;
                        File.Delete(path);
                        fpp.SaveAs(path);

                    }
                    else
                    {
                        next = false;
                        showmewss("文件格式不对");
                    }
                }
                if (next)
                {
                    if (path == "")
                    {
                        path = liallpath.Text.Trim();
                    }

                    if (access_sql.T_Update_ExecSql(new string[] { "dname", "drow", "dfile", "dsearchtxt", "dtablename" }, new object[] { name, row, path, searchtxt, txttablename.Text.Trim() }, "mb", "did=" + did) != 0)
                    {

                        access_sql.DoSql("delete from lb where ldid=" + did + "");
                        lipath.Visible = true;
                        liname.Visible = true;
                        lirow.Visible = true;
                        lisearchtxt.Visible = true;
                        txtname.Visible = false;
                        txtrow.Visible = false;
                        txtsearchtxt.Visible = false;
                        fpp.Visible = false;
                        btnsave.Visible = false;
                        btnup.Visible = true;
                        litablename.Visible = true;
                        txttablename.Visible = false;
                        bind();
                        showmewss("修改成功");
                    }

                }




            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button2_Click1(object sender, EventArgs e)
        {


        }

        protected void Button2_Click2(object sender, EventArgs e)
        {
            works.ReadExcelData_(access_sql.GetOneValue("select dfile from mb where did=1"), "Template", 4);
        }

        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void Button2_Click3(object sender, EventArgs e)
        {
            works.ReadExcelData_(access_sql.GetOneValue("select dfile from mb where did=1"), "Template", 4);
        }

        protected void btnqjjz_Click(object sender, EventArgs e)
        {
            Response.Redirect("/mb.aspx?type=" + drtype.SelectedValue + "&qj=" + txtqj.Text.Trim() + "&name=" + txtsname.Text.Trim() + "&pdtype=" + drpd.SelectedValue + "&xltype=" + drxl.SelectedValue + "&dddtype=" + DDDtype.SelectedValue + "&color=" + drcolor.SelectedValue);
        }

        protected void Button2_Click4(object sender, EventArgs e)
        {
            for (int i = 0; i < rplb.Items.Count; i++)
            {
                CheckBox ckk = (CheckBox)rplb.Items[i].FindControl("ckxz");
                ckk.Checked = true;
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rplb.Items.Count; i++)
            {
                CheckBox ckk = (CheckBox)rplb.Items[i].FindControl("ckxz");
                if (ckk.Checked) { ckk.Checked = false; } else { ckk.Checked = true; }

            }
        }
        public string getck()
        {
            string ru = "";
            for (int i = 0; i < rplb.Items.Count; i++)
            {
                CheckBox ckk = (CheckBox)rplb.Items[i].FindControl("ckxz");

                if (ckk.Checked)
                {
                    Literal lidid = (Literal)rplb.Items[i].FindControl("lidid");
                    ru += lidid.Text.Trim() + ",";
                }

            }
            if (ru != "")
            {
                ru = ru.Substring(0, ru.Length - 1);
            }
            return ru;
        }
        protected void Button4_Click(object sender, EventArgs e)
        {
            string where = getck();
            if (where != "")
            {
                int ll = where.Split(',').Length;
                access_sql.DoSql("update mb set dstate=1 where did in (" + where + ")");
                showmewss("处理" + ll + "个");
                bind();
            }
            else
            {
                showmewss("没有任何选择");
            }
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            string where = getck();
            if (where != "")
            {
                int ll = where.Split(',').Length;
                access_sql.DoSql("update mb set dstate=2 where did in (" + where + ")");
                showmewss("处理" + ll + "个");
                bind();
            }
            else
            {
                showmewss("没有任何选择");
            }
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            string where = getck();
            if (where != "")
            {
                int ll = where.Split(',').Length;
                access_sql.DoSql("update mb set dstate=-1 where did in (" + where + ")");
                showmewss("处理" + ll + "个");
                bind();
            }
            else
            {
                showmewss("没有任何选择");
            }
        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            string where = getck();
            if (where != "")
            {
                int ll = where.Split(',').Length;
                access_sql.DoSql("update mb set dstate=0 where did in (" + where + ")");
                showmewss("处理" + ll + "个");
                bind();
            }
            else
            {
                showmewss("没有任何选择");
            }
        }
        public string getzt(string ii)
        {
            string ru = "";
            if (ii == "0")
            {
                ru = "需处理";
            }
            if (ii == "-1")
            {
                ru = "不能销售";
            }
            if (ii == "1")
            {
                ru = "已设置搜索词";
            }
            if (ii == "2")
            {
                ru = "已完成";
            }
            return ru;
        }
        protected void Button8_Click(object sender, EventArgs e)
        {
            if (txtpwd.Text.Trim() == "cai-8897")
            {
                zc.Visible = true;
                lg.Visible = false;

                HttpCookie eee = null;

                if (HttpContext.Current.Request.Cookies["eee"] != null)
                {
                    eee = HttpContext.Current.Request.Cookies["eee"];
                    eee.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Add(eee);

                }
                else
                {
                    eee = new HttpCookie("eee");

                }
                eee.Value = txtpwd.Text.Trim();
                eee.Expires = DateTime.Now.AddDays(10);
                HttpContext.Current.Response.Cookies.Add(eee);
                if (Request.QueryString["r"] != null)
                {
                    Response.Redirect("/" + Request.QueryString["r"]);
                }
            }
            else
            {
                Response.Write("<script>alert(\"密码错误\")</script>");
            }
        }

        protected void Button9_Click(object sender, EventArgs e)
        {
            string where = getck();
            if (where != "")
            {
                int ll = where.Split(',').Length;
                access_sql.DoSql("update mb set dtype=1 where did in (" + where + ")");
                showmewss("处理" + ll + "个");
                bind();
            }
            else
            {
                showmewss("没有任何选择");
            }
        }

        protected void Button10_Click(object sender, EventArgs e)
        {
            string where = getck();
            if (where != "")
            {
                int ll = where.Split(',').Length;
                access_sql.DoSql("update mb set dtype=2 where did in (" + where + ")");
                showmewss("处理" + ll + "个");
                bind();
            }
            else
            {
                showmewss("没有任何选择");
            }
        }

        protected void Button11_Click(object sender, EventArgs e)
        {
            string where = getck();
            if (where != "")
            {
                int ll = where.Split(',').Length;
                access_sql.DoSql("update mb set color=1 where did in (" + where + ")");
                showmewss("处理" + ll + "个");
                bind();
            }
            else
            {
                showmewss("没有任何选择");
            }
        }

        protected void Button12_Click(object sender, EventArgs e)
        {
            string where = getck();
            if (where != "")
            {
                int ll = where.Split(',').Length;
                access_sql.DoSql("update mb set color=2 where did in (" + where + ")");
                showmewss("处理" + ll + "个");
                bind();
            }
            else
            {
                showmewss("没有任何选择");
            }
        }

        protected void Button13_Click(object sender, EventArgs e)
        {
            string where = getck();
            if (where != "")
            {
                int ll = where.Split(',').Length;
                access_sql.DoSql("update mb set color=0 where did in (" + where + ")");
                showmewss("处理" + ll + "个");
                bind();
            }
            else
            {
                showmewss("没有任何选择");
            }
        }
    }
}