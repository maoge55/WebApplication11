using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication11
{
    public partial class c : System.Web.UI.Page
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
                if (HttpContext.Current.Request.Cookies["eee"] == null)
                {
                    Response.Redirect("/other.aspx?r=addother.aspx");
                }
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["oid"] != null)
                {
                    DataSet ds = access_sql.GreatDs("select * from otmb where oid=" + Request.QueryString["oid"] + " and ouid=" + uid + "");
                    if (access_sql.yzTable(ds))
                    {
                        lits.Text = ds.Tables[0].Rows[0]["oname"].ToString();
                        name = ds.Tables[0].Rows[0]["oname"].ToString();
                        lioid.Text = ds.Tables[0].Rows[0]["oid"].ToString();
                        oid = ds.Tables[0].Rows[0]["oid"].ToString();
                        dddrrr.Visible = false;
                        btnadddata.Visible = true;
                        DataSet dsdata = access_sql.GreatDs("select top 500 * from otdata where odmid=" + Request.QueryString["oid"]);
                        showmewss("现有" + dsdata.Tables[0].Rows.Count + "个");
                        Literal1.Text = "编辑" + name;
                        if (access_sql.yzTable(dsdata))
                        {
                            int rrr = 0;
                            foreach (DataRow dr in dsdata.Tables[0].Rows)
                            {
                                rrr++;
                                txtvalue.Text += dr["odvalue"].ToString().Trim() + "\r\n";
                                
                            }
                        }
                    }
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";
        public string oid = "";
        public string name = "";
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (works.yzkong(new List<TextBox> { txtoname, txtvalue }))
            {
                if (access_sql.GetOneValue("select count(*) from otmb where oname='" + txtoname.Text.Trim().Replace("'", "''") + "' and ouid=" + uid + "") == "0")
                {
                    if (access_sql.T_Insert_ExecSql(new string[] { "oname", "ouid", "otype" }, new object[] { txtoname.Text.Trim(), uid, dptype.SelectedValue }, "otmb") == 1)
                    {
                        string oid = access_sql.GetOneValue("select oid from otmb where oname='" + txtoname.Text.Trim().Replace("'", "''") + "' and ouid=" + uid + "");
                        string values = txtvalue.Text.Trim();
                        values = values.Replace("\r\n", "镍");
                        string[] aaaa = values.Split('镍');
                        int cg = 0;
                        int cf = 0;
                        foreach (string item in aaaa)
                        {
                            if (item != "")
                            {
                                if (ckcf.Checked)
                                {
                                    if (access_sql.GetOneValue("select count(*) from otdata where odvalue='" + item.Replace("'", "''") + "' and odmid=" + oid + "") == "0")
                                    {
                                        if (access_sql.T_Insert_ExecSql(new string[] { "odmid", "odvalue" }, new object[] { oid, item.Trim() }, "otdata") == 1)
                                        {
                                            cg++;
                                        }
                                    }
                                    else
                                    {
                                        cf++;
                                    }
                                }
                                else
                                {
                                    if (access_sql.T_Insert_ExecSql(new string[] { "odmid", "odvalue" }, new object[] { oid, item.Trim() }, "otdata") == 1)
                                    {
                                        cg++;
                                    }
                                }

                            }
                        }
                        showmewss("添加成功" + cg + "，重复" + cf + "个");
                    }
                }
                else
                {
                    showmewss("名字已经存在");
                }
            }
            else
            {
                showmewss("不能为空");
            }
        }
        public void showmewss(string mess)
        {
            lits.Text = mess;
        }

        protected void btnadddata_Click(object sender, EventArgs e)
        {
            if (txtvalue.Text.Trim() != "")
            {
                string values = txtvalue.Text.Trim();
                values = values.Replace("\r\n", "镍");
                string[] aaaa = values.Split('镍');
                oid = lioid.Text.Trim();
                access_sql.DoSql("delete from otdata where odmid=" + oid + "");
                int cg = 0;
                int cf = 0;
                foreach (string item in aaaa)
                {
                    if (item != "")
                    {
                        if (ckcf.Checked)
                        {
                            if (access_sql.GetOneValue("select count(*) from otdata where odvalue='" + item.Replace("'", "''") + "' and odmid=" + oid + "") == "0")
                            {
                                if (access_sql.T_Insert_ExecSql(new string[] { "odmid", "odvalue" }, new object[] { oid, item.Trim() }, "otdata") == 1)
                                {
                                    cg++;
                                }
                            }
                            else
                            {
                                cf++;
                            }
                        }
                        else
                        {
                            if (access_sql.T_Insert_ExecSql(new string[] { "odmid", "odvalue" }, new object[] { oid, item.Trim() }, "otdata") == 1)
                            {
                                cg++;
                            }
                        }

                    }
                }
                showmewss("添加成功" + cg + "，重复" + cf + "个");
            }
            else
            {
                showmewss("不能为空");
            }
        }


    }
}