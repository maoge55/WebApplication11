using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 财务表 : System.Web.UI.Page
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
                if (uid != "9" && uid != "18" && uid != "19")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public string getannengyunfei(string zhongliang)
        {
            string ru = "";
            if (zhongliang != "" && zhongliang != "0")
            {
                int rounded = (int)Math.Round((float.Parse(zhongliang) * 1.6), MidpointRounding.AwayFromZero);
                ru = rounded.ToString();
                if(ru=="0")
                {
                    ru = "1";
                }
            }
            return ru;
        }
        public void bindzhy()
        {
            string where = "";
            if (dpzt.SelectedValue != "全部")
            {
                where = " dingdanzhuangtai ='" + dpzt.SelectedValue + "'  and ";
            }
            if (txtsjbm.Text.Trim() != "")
            {
                where += " shangjiabianma ='" + txtsjbm.Text.Trim().Replace("'", "''") + "'  ";
            }
            if (txtrucangyinnibiaoti.Text.Trim() != "")
            {
                if (where != "")
                {
                    where += " and";
                }
                where += " rucangyinnibiaoti like '%" + txtrucangyinnibiaoti.Text.Trim().Replace("'", "''") + "%'";
            }
            if (txtrucangSKUID.Text.Trim() != "")
            {
                if (where != "")
                {
                    where += " and";
                }
                where += " rucangSKUID = '" + txtrucangSKUID.Text.Trim().Replace("'", "''") + "'";
            }

            string sql = "select * from caiwu where " + where;


            DataSet ds = access_sql.GreatDs(sql);
            if (access_sql.yzTable(ds))
            {
                if (access_sql.yzTable(ds))
                {
                    DataTable dt = ds.Tables[0];

                    rplb.DataSource = dt;
                    rplb.DataBind();
                    Literal1.Text = "<span style='color:red'>加载：" + dt.Rows.Count + "条</span>";

                }

            }
            else
            {
                lits.Text = "无数据"; ;
            }
        }

        public string getyfhydp(string yunfeihaiyundanpin, string yunfeihaiyunmeifang, string chang, string kuan, string gao)
        {
            string ru = "";
            if (yunfeihaiyundanpin != "")
            {
                ru = yunfeihaiyundanpin;
            }
            else
            {
                if (yunfeihaiyunmeifang != "" && chang != "" && kuan != "" && gao != "")
                {
                    try
                    {
                        ru = float.Parse((float.Parse(yunfeihaiyunmeifang) * float.Parse(chang) * float.Parse(kuan) * float.Parse(gao) / 1000000).ToString()).ToString("0.00");
                    }
                    catch
                    {


                    }
                }
            }
            return ru;
        }
        public string gettitle(string r)
        {
            string ru = r;
            if (txtrucangyinnibiaoti.Text.Trim() != "")
            {
                if (ru.IndexOf(txtrucangyinnibiaoti.Text.Trim().Replace("'", "''")) != -1)
                {
                    ru = ru.Replace(txtrucangyinnibiaoti.Text.Trim().Replace("'", "''"), "<span style='color:red;font-weight: bold;'>" + txtrucangyinnibiaoti.Text.Trim().Replace("'", "''") + "</span>");
                }
            }
            return ru;
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




        }


        protected void Button1_Click1(object sender, EventArgs e)
        {
            lits.Text = "";
            rplb.DataSource = null;
            rplb.DataBind();
            Literal1.Text = "";
            bindzhy();

        }
    }
}