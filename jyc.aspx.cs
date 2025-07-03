using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication11
{
    public partial class jyc : System.Web.UI.Page
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
                    Response.Redirect("/other.aspx?r=jyc.aspx");
                }
            }
            if (!IsPostBack)
            {

                DataSet ds = access_sql.GreatDs("select * from wj order by wid");
                if (access_sql.yzTable(ds))
                {


                    btnadddata.Visible = true;

                    showmewss("现有" + ds.Tables[0].Rows.Count.ToString() + "个");
                    Literal1.Text = "编辑" + name;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        txtvalue.Text += dr["wname"].ToString().Trim() + "\r\n";
                    }

                }

            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";
        public string oid = "";
        public string name = "";
      
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
                access_sql.DoSql("delete from wj");
                int cg = 0;
                int cf = 0;
                foreach (string item in aaaa)
                {
                    if (item != "")
                    {
                        if (ckcf.Checked)
                        {
                            if (access_sql.GetOneValue("select count(*) from wj where wname='" + item.Replace("'", "''") + "'") == "0")
                            {
                                if (access_sql.T_Insert_ExecSql(new string[] {  "wname" }, new object[] {item.Trim() }, "wj") == 1)
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
                            if (access_sql.T_Insert_ExecSql(new string[] { "wname" }, new object[] { item.Trim() }, "wj") == 1)
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