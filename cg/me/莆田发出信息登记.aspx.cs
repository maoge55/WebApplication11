using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 莆田发出信息登记 : System.Web.UI.Page
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
                if (uid != "8" && uid != "9" && uid != "18" && uid != "19" && uid != "12")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public string getkefa(string shuliang, string putianfachushuliang, string xutuihuoshuliang)
        {
            string ru = "";


            try
            {


                if (putianfachushuliang == "")
                {
                    if (xutuihuoshuliang == "")
                    {


                        ru = shuliang + "-0-0=<span style='color:blue;font-weight: bold;'>" + shuliang + "</span>";
                    }
                    else
                    {
                        ru = shuliang + "-0-" + xutuihuoshuliang + "=<span style='color:blue;font-weight: bold;'>" + (int.Parse(shuliang) - int.Parse(xutuihuoshuliang)) + "</span>";
                    }
                }
                else
                {
                    int shuliang_ = access_sql.fhint(shuliang);
                    int fhsl = 0;
                    string[] cf = putianfachushuliang.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int q = 0; q < cf.Length; q++)
                    {
                        int d = access_sql.fhint(cf[q].Trim());
                        if (d != -100)
                        {
                            fhsl += d;
                        }
                    }
                    if (xutuihuoshuliang == "")
                    {
                        ru = shuliang_ + "-" + fhsl + "=<span style='color:blue;font-weight: bold;'>" + (shuliang_ - fhsl) + "</span>";
                    }
                    else
                    {
                        ru = shuliang_ + "-" + fhsl + "-" + xutuihuoshuliang + "=<span style='color:blue;font-weight: bold;'>" + (shuliang_ - fhsl - int.Parse(xutuihuoshuliang)) + "</span>";
                    }
                }
            }
            catch
            {

                ru = "信息有误";
            }
            return ru;
        }
        public void bindzhy()
        {
            string where = " dingdanzhuangtai<>'交易关闭' and isbctg=0 and";
            if (txttitle.Text.Trim() != "")
            {
                string title = txttitle.Text.Trim().Replace("'", "''");
                where += " (huopinbiaoti like '%" + title + "%' or rucangyinnibiaoti like '%" + title + "%') and ";
            }
            if (txtdingdanbianhao.Text.Trim() != "")
            {
                where += " (dingdanbianhao = '" + txtdingdanbianhao.Text.Trim().Replace("'", "''") + "') and ";
            }
            string sql = "SELECT xutuihuoshuliang,mainimg,cid,rucangyinnibiaoti,dingdanbianhao,dingdanchuangjianshijian,Offer_ID,SKU_ID,huopinbiaoti,shuliang,putianfachushuliang,putianfachuriqi,putianfachudanhao,putianfachukuaidifeiyong,putianfachubeizhu FROM [SuMaiTongPol].[dbo].[caiwu] where " + where + " dingdanzhuangtai='" + dpddzt.Text + "' and shangjiabianma='" + txtsjbm.Text.Trim().Replace("'", "''") + "'";
            string lx = "";
            string zs = "";


            DataSet ds = access_sql.GreatDs(sql);
            if (access_sql.yzTable(ds))
            {
                if (access_sql.yzTable(ds))
                {
                    DataTable dt = ds.Tables[0];
                    DataTable dttemp = new DataTable();
                    dttemp = dt.Clone();
                    dttemp.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        int shuliang = access_sql.fhint(dr["shuliang"].ToString());
                        string putianfachushuliang = dr["putianfachushuliang"].ToString();
                        int fhsl = 0;
                        if (putianfachushuliang != "")
                        {
                            string[] cf = putianfachushuliang.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                            for (int q = 0; q < cf.Length; q++)
                            {
                                int d = access_sql.fhint(cf[q].Trim());
                                if (d != -100)
                                {
                                    fhsl += d;
                                }
                            }
                        }
                        if (shuliang > fhsl)
                        {
                            dttemp.Rows.Add(dr.ItemArray);
                        }


                    }


                    rplb.DataSource = dttemp;
                    rplb.DataBind();
                    Literal1.Text = "<span style='color:red'>加载" + dttemp.Rows.Count + "条</span>";

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
            if (txttitle.Text.Trim().Length > 0)
            {
                if (ru.IndexOf(txttitle.Text.Trim().Replace("'", "''")) != -1)
                {
                    ru = ru.Replace(txttitle.Text.Trim().Replace("'", "''"), "<span style='color:red;font-weight: bold;'>" + txttitle.Text.Trim().Replace("'", "''") + "</span>");
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





                TextBox txtputianfachushuliang = e.Item.FindControl("txtputianfachushuliang") as TextBox;
                TextBox txtputianfachuriqi = e.Item.FindControl("txtputianfachuriqi") as TextBox;
                TextBox txtputianfachudanhao = e.Item.FindControl("txtputianfachudanhao") as TextBox;
                TextBox txtputianfachukuaidifeiyong = e.Item.FindControl("txtputianfachukuaidifeiyong") as TextBox;
                TextBox txtputianfachubeizhu = e.Item.FindControl("txtputianfachubeizhu") as TextBox;

                if (txtputianfachushuliang.Text != "" && txtputianfachuriqi.Text != "" && txtputianfachudanhao.Text != "" && txtputianfachukuaidifeiyong.Text != "")
                {
                    string[] strs = new string[] { "putianfachushuliang", "putianfachuriqi", "putianfachudanhao", "putianfachukuaidifeiyong", "putianfachubeizhu" };
                    object[] objs = new object[] { txtputianfachushuliang.Text.Trim().Replace("'", "''"), txtputianfachuriqi.Text.Trim().Replace("'", "''"), txtputianfachudanhao.Text.Trim().Replace("'", "''"), txtputianfachukuaidifeiyong.Text.Trim().Replace("'", "''"), txtputianfachubeizhu.Text.Trim().Replace("'", "''") };

                    int sl = access_sql.T_Update_ExecSql(strs, objs, "caiwu", "cid=" + cid + "");
                    lits.Text = cid + "更新数据成功";
                }
                else
                {
                    lits.Text = "信息不能为空，不确定可以先填写“0”";
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
            int cg = 0;
            bool ky = true;

            for (int i = 0; i < rplb.Items.Count; i++)
            {





                TextBox txtputianfachushuliang = rplb.Items[i].FindControl("txtputianfachushuliang") as TextBox;
                TextBox txtputianfachuriqi = rplb.Items[i].FindControl("txtputianfachuriqi") as TextBox;
                TextBox txtputianfachudanhao = rplb.Items[i].FindControl("txtputianfachudanhao") as TextBox;
                TextBox txtputianfachukuaidifeiyong = rplb.Items[i].FindControl("txtputianfachukuaidifeiyong") as TextBox;
                TextBox txtputianfachubeizhu = rplb.Items[i].FindControl("txtputianfachubeizhu") as TextBox;




                if (txtputianfachushuliang.Text == "" || txtputianfachuriqi.Text == "" || txtputianfachudanhao.Text == "" || txtputianfachukuaidifeiyong.Text == "")
                {
                    ky = false;
                    lits.Text = "不能为空";
                    Response.Write("<script>alert('第" + (i + 1) + "需先填写信息');</script>");
                    break;
                }



            }
            if (ky)
            {
                for (int i = 0; i < rplb.Items.Count; i++)
                {

                    Literal licid = (Literal)rplb.Items[i].FindControl("licid");

                    TextBox txtputianfachushuliang = rplb.Items[i].FindControl("txtputianfachushuliang") as TextBox;
                    TextBox txtputianfachuriqi = rplb.Items[i].FindControl("txtputianfachuriqi") as TextBox;
                    TextBox txtputianfachudanhao = rplb.Items[i].FindControl("txtputianfachudanhao") as TextBox;
                    TextBox txtputianfachukuaidifeiyong = rplb.Items[i].FindControl("txtputianfachukuaidifeiyong") as TextBox;
                    TextBox txtputianfachubeizhu = rplb.Items[i].FindControl("txtputianfachubeizhu") as TextBox;


                    string[] strs = new string[] { "putianfachushuliang", "putianfachuriqi", "putianfachudanhao", "putianfachukuaidifeiyong", "putianfachubeizhu" };
                    object[] objs = new object[] { txtputianfachushuliang.Text.Trim().Replace("'", "''"), txtputianfachuriqi.Text.Trim().Replace("'", "''"), txtputianfachudanhao.Text.Trim().Replace("'", "''"), txtputianfachukuaidifeiyong.Text.Trim().Replace("'", "''"), txtputianfachubeizhu.Text.Trim().Replace("'", "''") };

                    cg += access_sql.T_Update_ExecSql(strs, objs, "caiwu", "cid=" + licid.Text + "");









                }
            }


            lits.Text = "更新成功" + cg + "个";
        }
    }
}