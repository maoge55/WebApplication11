using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11
{
    public partial class pz : System.Web.UI.Page
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
                    Response.Redirect("/other.aspx?r=pz.aspx");
                }
            }
            if (!IsPostBack)
            {

                bind();
            }
        }
        public string u = "";
        public string p = "";
        public string uid = "";
        public void bind()
        {
            DataSet ds = access_sql.GreatDs("select * from pz");
            if (access_sql.yzTable(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtcppf.Text = dr["cppf"].ToString();
                txtmspf.Text = dr["mspf"].ToString();
                txtgtpf.Text = dr["gtpf"].ToString();
                txtfhpf.Text = dr["fhpf"].ToString();
                txtkdnx.Text = dr["kdnx"].ToString();
                txtfssl.Text = dr["fssl"].ToString();
                txthl.Text = dr["hl"].ToString();
                txtjgqj.Text = dr["jgqj"].ToString();
                txtzxl.Text = dr["zxl"].ToString();
            }
        }

        public bool yzfloat(object[] aaa)
        {
            bool ru = true;

            try
            {
                for (int i = 0; i < aaa.Length; i++)
                {
                    if (ru)
                    {
                        TextBox temp = aaa[i] as TextBox;
                        float ff = float.Parse(temp.Text.Trim());
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch
            {
                ru = false;
            }

            return ru;

        }


        protected void btnadddata_Click(object sender, EventArgs e)
        {
            if (yzfloat(new object[] { txtcppf, txtmspf, txtgtpf, txtfhpf, txtkdnx, txtfssl, txthl, txtzxl }))
            {
                if (access_sql.T_Update_ExecSql(new string[] { "cppf", "mspf", "gtpf", "fhpf", "kdnx", "fssl", "hl", "jgqj", "zxl" }, new object[] { txtcppf.Text.Trim(), txtmspf.Text.Trim(), txtgtpf.Text.Trim(), txtfhpf.Text.Trim(), txtkdnx.Text.Trim(), txtfssl.Text.Trim(), txthl.Text.Trim(), txtjgqj.Text.Trim(), txtzxl.Text.Trim() }, "pz", "1=1") != 0)
                {
                    lits.Text = ("修改成功");
                }
                else
                {
                    lits.Text = ("修改失败");
                }
            }
            else
            {
                lits.Text = ("输入有误");
            }
        }

    }
}