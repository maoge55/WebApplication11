using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 海外仓到仓数量登记 : System.Web.UI.Page
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

            if (txthaiwaicangrukudanhao.Text != "")
            {
                where += " and haiwaicangrukudanhao like '%" + txthaiwaicangrukudanhao.Text.Trim().Replace("'", "''") + "%' ";
            }
            if (txtguangdongfachudanhao.Text != "")
            {
                where += " and guangdongfachudanhao like '%" + txtguangdongfachudanhao.Text.Trim().Replace("'", "''") + "%'";
            }
            string sql = "SELECT * FROM caiwu where dingdanzhuangtai<>'交易关闭' and isbctg=0 and shangjiabianma='" + txtsjbm.Text.Trim().Replace("'", "''") + "'  " + where;
            string lx = "";
            string zs = "";


            DataSet ds = access_sql.GreatDs(sql);
            if (access_sql.yzTable(ds))
            {
                if (access_sql.yzTable(ds))
                {


                    rplb.DataSource = ds.Tables[0];
                    rplb.DataBind();
                    Literal1.Text = "<span style='color:red'>加载" + ds.Tables[0].Rows.Count + "条</span>";

                }

            }
            else
            {
                lits.Text = "无数据"; ;
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

        public string getiiiiimg(string tupianwangzhi)
        {
            string ru = "";
            if (tupianwangzhi != "")
            {
                string[] aa = tupianwangzhi.Split('|');
                for (int i = 0; i < aa.Length; i++)
                {
                    if (aa[i].Trim() != "")
                    {
                        ru += "<a href='/Uploads/" + aa[i] + "' target='_blank'><img src='/Uploads/" + aa[i] + "' style='width:100px;'/></a>&nbsp;";
                    }
                }
            }
            return ru;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            lits.Text = "";
            if (txtsjbm.Text.Trim() != "")
            {
                rplb.DataSource = null;
                rplb.DataBind();
                Literal1.Text = "";
                lits.Text = "";
                bindzhy();
            }
            else
            {
                lits.Text = "请输入商家编码";
                Response.Write("<script>alert('" + lits.Text + "');</script>");
            }


        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

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


            if (e.CommandName == "qr")
            {

                string cid = e.CommandArgument.ToString();





                TextBox txthaiwaicangshangjiashuliang = e.Item.FindControl("txthaiwaicangshangjiashuliang") as TextBox;
                TextBox txtcaiwuzhuangtai = e.Item.FindControl("txtcaiwuzhuangtai") as TextBox;
                TextBox txtcaiwushenhebeizhu = e.Item.FindControl("txtcaiwushenhebeizhu") as TextBox;


                string[] strs = new string[] { "haiwaicangshangjiashuliang", "caiwuzhuangtai", "caiwushenhebeizhu" };
                object[] objs = new object[] {
                        txthaiwaicangshangjiashuliang.Text.Trim().Replace("'", "''"),
                        txtcaiwuzhuangtai.Text.Trim().Replace("'", "''"),
                        txtcaiwushenhebeizhu.Text.Trim().Replace("'", "''")
                    };

                if (access_sql.T_Update_ExecSql(strs, objs, "caiwu", "cid=" + cid + "") > 0)
                {
                    lits.Text = cid + "更新数据成功";
                }
                else
                {
                    lits.Text = cid + "更新数据失败";
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

        protected void Button2_Click1(object sender, EventArgs e)
        {
            for (int i = 0; i < rplb.Items.Count; i++)
            {
                Literal licid = (Literal)rplb.Items[i].FindControl("licid");



                TextBox txthaiwaicangshangjiashuliang = rplb.Items[i].FindControl("txthaiwaicangshangjiashuliang") as TextBox;
                TextBox txtcaiwuzhuangtai = rplb.Items[i].FindControl("txtcaiwuzhuangtai") as TextBox;
                TextBox txtcaiwushenhebeizhu = rplb.Items[i].FindControl("txtcaiwushenhebeizhu") as TextBox;


                string[] strs = new string[] { "haiwaicangshangjiashuliang", "caiwuzhuangtai", "caiwushenhebeizhu" };
                object[] objs = new object[] {
                        txthaiwaicangshangjiashuliang.Text.Trim().Replace("'", "''"),
                        txtcaiwuzhuangtai.Text.Trim().Replace("'", "''"),
                        txtcaiwushenhebeizhu.Text.Trim().Replace("'", "''")
                    };

                if (access_sql.T_Update_ExecSql(strs, objs, "caiwu", "cid=" + licid.Text + "") > 0)
                {
                    lits.Text = licid.Text + "更新数据成功";
                }
                else
                {
                    lits.Text = licid.Text + "更新数据失败";
                    Response.Write("<script>alert('" + lits.Text + "');</script>");
                }
            }
        }
    }
}