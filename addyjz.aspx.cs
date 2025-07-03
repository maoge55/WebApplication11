using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication11
{
    public partial class addyjz : System.Web.UI.Page
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
                    Response.Redirect("/other.aspx?r=addyjz.aspx");
                }
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["yid"] != null)
                {
                    DataSet ds = access_sql.GreatDs("select * from ymb where yid=" + Request.QueryString["yid"] + " and yuid=" + uid + "");
                    if (access_sql.yzTable(ds))
                    {
                        lits.Text = ds.Tables[0].Rows[0]["yname"].ToString();
                        name = ds.Tables[0].Rows[0]["yname"].ToString();
                        lioid.Text = ds.Tables[0].Rows[0]["yid"].ToString();
                        oid = ds.Tables[0].Rows[0]["yid"].ToString();
                        dddrrr.Visible = false;
                        btnadddata.Visible = true;
                        DataSet dsdata = access_sql.GreatDs("select * from ydata where jyid=" + Request.QueryString["yid"]);
                        showmewss("现有" + dsdata.Tables[0].Rows.Count + "个");
                        Literal1.Text = "编辑" + name;
                        if (access_sql.yzTable(dsdata))
                        {
                            foreach (DataRow dr in dsdata.Tables[0].Rows)
                            {
                                txtvalue.Text += dr["jname"].ToString().Trim() + "\r\n";
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
            if (works.yzkong(new List<TextBox> { txtyname, txtvalue }))
            {
                if (access_sql.GetOneValue("select count(*) from ymb where yname='" + txtyname.Text.Trim().Replace("'", "''") + "' and yuid=" + uid + "") == "0")
                {
                    if (access_sql.T_Insert_ExecSql(new string[] { "yname", "yuid" }, new object[] { txtyname.Text.Trim(), uid }, "ymb") == 1)
                    {
                        string yid = access_sql.GetOneValue("select yid from ymb where yname='" + txtyname.Text.Trim().Replace("'", "''") + "'  and yuid=" + uid + "");
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
                                    if (access_sql.GetOneValue("select count(*) from ydata where jname='" + item.Replace("'", "''") + "' and jyid=" + yid + "") == "0")
                                    {
                                        if (access_sql.T_Insert_ExecSql(new string[] { "jyid", "jname" }, new object[] { yid, item.Trim() }, "ydata") == 1)
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
                                    if (access_sql.T_Insert_ExecSql(new string[] { "jyid", "jname" }, new object[] { yid, item.Trim() }, "ydata") == 1)
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
                string yid = lioid.Text.Trim();

                int cg = 0;
                int cf = 0;
                foreach (string item in aaaa)
                {
                    if (item != "")
                    {
                        if (access_sql.GetOneValue("select count(*) from ydata where jname='" + item.Replace("'", "''") + "' and jyid=" + yid + "") == "0")
                        {
                            if (access_sql.T_Insert_ExecSql(new string[] { "jyid", "jname" }, new object[] { yid, item.Trim() }, "ydata") == 1)
                            {
                                cg++;
                            }
                        }
                        else
                        {
                            cf++;
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