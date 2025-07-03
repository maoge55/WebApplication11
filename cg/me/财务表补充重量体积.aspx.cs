using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 财务表补充重量体积 : System.Web.UI.Page
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
                if (uid != "8" && uid != "9" && uid != "18" && uid != "19" && uid != "12" && uid != "6")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public void bindzhy(int type)
        {
            string sql = "";
            string lx = "";
            string zs = "";
            if (type == 1)
            {
                lx = "重量体积为空数据";
                string and = " and dingdanzhuangtai<>'交易关闭' and isbctg=0 and ((chang is null or chang='')  or (kuan is null or kuan='') or (gao is null or gao='')  or (zhongliang is null or zhongliang='') or (tijizhongliang is null or tijizhongliang='') or (yunfeihaiyunmeifang is null or yunfeihaiyunmeifang='') or (yunfeikongyunmeigongjin is null or yunfeikongyunmeigongjin=''))";


                zs = access_sql.GetOneValue("select  count(*) from caiwu where shangjiabianma='" + txtsjbm.Text.Replace("'", "''") + "' " + and + "  ");


                sql = "select top 1 *,'https://detail.1688.com/offer/'+[Offer_ID]+'.html' as [1688链接] from caiwu where Offer_ID is not null and shangjiabianma='" + txtsjbm.Text.Replace("'", "''") + "' " + and;


            }
            else
            {
                lx = "重量体积为0数据";
                string and = " and isbctg=0  and ((chang='0')  or (kuan='0') or (gao='0')  or (zhongliang='0') or (tijizhongliang='0') or (yunfeihaiyunmeifang='0') or (yunfeikongyunmeigongjin='0'))";

                zs = access_sql.GetOneValue("select  count(*) from caiwu where shangjiabianma='" + txtsjbm.Text.Replace("'", "''") + "' " + and + "  ");


                sql = "select top 1 *,'https://detail.1688.com/offer/'+[Offer_ID]+'.html' as [1688链接] from caiwu where Offer_ID is not null and shangjiabianma='" + txtsjbm.Text.Replace("'", "''") + "' " + and;

            }

            DataSet ds = access_sql.GreatDs(sql);
            if (access_sql.yzTable(ds))
            {
                if (access_sql.yzTable(ds))
                {
                    DataTable dt = ds.Tables[0];

                    rplb.DataSource = dt;
                    rplb.DataBind();
                    Literal1.Text = "<span style='color:red'>加载" + lx + "：" + dt.Rows.Count + "条，共有数据" + zs + "条</span>";

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
                rplb.DataSource = null;
                rplb.DataBind();
                Literal1.Text = "";
                bindzhy(1);
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
            if (e.CommandName == "txzy")
            {

                string cs = e.CommandArgument.ToString();
                bool ky = true;

                string chang = "";
                string kuan = "";
                string gao = ""; string zhongliang = ""; string tijizhongliang = ""; string yunfeihaiyunmeifang = ""; string yunfeikongyunmeigongjin = "";






                TextBox txtchang = e.Item.FindControl("txtchang") as TextBox;
                TextBox txtkuan = e.Item.FindControl("txtkuan") as TextBox;
                TextBox txtgao = e.Item.FindControl("txtgao") as TextBox;
                TextBox txtzhongliang = e.Item.FindControl("txtzhongliang") as TextBox;
                TextBox txttijizhongliang = e.Item.FindControl("txttijizhongliang") as TextBox;
                TextBox txtyunfeihaiyunmeifang = e.Item.FindControl("txtyunfeihaiyunmeifang") as TextBox;
                TextBox txtyunfeikongyunmeigongjin = e.Item.FindControl("txtyunfeikongyunmeigongjin") as TextBox;


                chang = txtchang.Text.Trim().Replace("'", "''");
                kuan = txtkuan.Text.Trim().Replace("'", "''");
                gao = txtgao.Text.Trim().Replace("'", "''");
                zhongliang = txtzhongliang.Text.Trim().Replace("'", "''");
                tijizhongliang = txttijizhongliang.Text.Trim().Replace("'", "''");
                yunfeihaiyunmeifang = txtyunfeihaiyunmeifang.Text.Trim().Replace("'", "''");
                yunfeikongyunmeigongjin = txtyunfeikongyunmeigongjin.Text.Trim().Replace("'", "''");

                for (int i = 0; i < rplb.Items.Count; i++)
                {
                    (rplb.Items[i].FindControl("txtchang") as TextBox).Text = chang;
                    (rplb.Items[i].FindControl("txtkuan") as TextBox).Text = kuan;
                    (rplb.Items[i].FindControl("txtgao") as TextBox).Text = gao;
                    (rplb.Items[i].FindControl("txtzhongliang") as TextBox).Text = zhongliang;
                    (rplb.Items[i].FindControl("txttijizhongliang") as TextBox).Text = tijizhongliang;
                    (rplb.Items[i].FindControl("txtyunfeihaiyunmeifang") as TextBox).Text = yunfeihaiyunmeifang;
                    (rplb.Items[i].FindControl("txtyunfeikongyunmeigongjin") as TextBox).Text = yunfeikongyunmeigongjin;
                }

            }

            if (e.CommandName == "qr")
            {

                string cs = e.CommandArgument.ToString();
                bool ky = true;

                string chang = ""; string kuan = ""; string gao = ""; string zhongliang = ""; string tijizhongliang = ""; string yunfeihaiyunmeifang = ""; string yunfeikongyunmeigongjin = "";






                TextBox txtchang = e.Item.FindControl("txtchang") as TextBox;
                TextBox txtkuan = e.Item.FindControl("txtkuan") as TextBox;
                TextBox txtgao = e.Item.FindControl("txtgao") as TextBox;
                TextBox txtzhongliang = e.Item.FindControl("txtzhongliang") as TextBox;
                TextBox txttijizhongliang = e.Item.FindControl("txttijizhongliang") as TextBox;
                TextBox txtyunfeihaiyunmeifang = e.Item.FindControl("txtyunfeihaiyunmeifang") as TextBox;
                TextBox txtyunfeikongyunmeigongjin = e.Item.FindControl("txtyunfeikongyunmeigongjin") as TextBox;


                chang = txtchang.Text.Trim().Replace("'", "''");
                kuan = txtkuan.Text.Trim().Replace("'", "''");
                gao = txtgao.Text.Trim().Replace("'", "''");
                zhongliang = txtzhongliang.Text.Trim().Replace("'", "''");
                tijizhongliang = txttijizhongliang.Text.Trim().Replace("'", "''");
                yunfeihaiyunmeifang = txtyunfeihaiyunmeifang.Text.Trim().Replace("'", "''");
                yunfeikongyunmeigongjin = txtyunfeikongyunmeigongjin.Text.Trim().Replace("'", "''");

                if (chang != "" && kuan != "" && gao != "" && zhongliang != "" && tijizhongliang != "" && yunfeihaiyunmeifang != "" && yunfeikongyunmeigongjin != "")
                {
                    ky = true;
                }
                else
                {
                    ky = false;
                }
                if (ky)
                {
                    string upstr = "";

                    string[] strs = new string[] { "chang", "kuan", "gao", "zhongliang", "tijizhongliang", "yunfeihaiyunmeifang", "yunfeikongyunmeigongjin" };
                    object[] objs = new object[] { chang, kuan, gao, zhongliang, tijizhongliang, yunfeihaiyunmeifang, yunfeikongyunmeigongjin };



                    string where = "";

                    Literal licid = e.Item.FindControl("licid") as Literal;
                    Literal liOffer_ID = e.Item.FindControl("liOffer_ID") as Literal;
                    Literal liSKU_ID = e.Item.FindControl("liSKU_ID") as Literal;

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



                    int sl = access_sql.T_Update_ExecSql(strs, objs, "caiwu", where);

                    lits.Text = "更新数据成功" + sl + "条,请手动加载下一条";


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
            lits.Text = "";
            if (txtsjbm.Text.Trim() != "")
            {
                rplb.DataSource = null;
                rplb.DataBind();
                Literal1.Text = "";
                bindzhy(0);
            }
            else
            {
                lits.Text = "请输入商家编码";
            }
        }

        protected void Button3_Click1(object sender, EventArgs e)
        {
            lits.Text = "";
            if (txttitle.Text.Trim() != "" && txtsjbm.Text.Trim() != "")
            {
                rplb.DataSource = null;
                rplb.DataBind();
                Literal1.Text = "";
                bindtitle();
            }
            else
            {
                lits.Text = "请输入标题和商家编码";
                Response.Write("<script>alert('" + lits.Text + "');</script>");
            }


        }

        public void bindtitle()
        {
            string sql = "";
            string lx = "";
            string zs = "";
            string where = "";
            if (txttitle.Text.Trim() != "")
            {
                string title = txttitle.Text.Trim().Replace("'", "''");
                where += " 1=1  and dingdanzhuangtai<>'交易关闭' and isbctg=0  and (huopinbiaoti like '%" + title + "%' or rucangyinnibiaoti like '%" + title + "%')  ";
            }

            lx = "标题搜索";


            zs = access_sql.GetOneValue("select  count(*) from caiwu where " + where);
            sql = " select *,'https://detail.1688.com/offer/'+[Offer_ID]+'.html' as [1688链接] from caiwu where " + where;

            DataSet ds = access_sql.GreatDs(sql);
            if (access_sql.yzTable(ds))
            {
                if (access_sql.yzTable(ds))
                {
                    DataTable dt = ds.Tables[0];

                    rplb.DataSource = dt;
                    rplb.DataBind();
                    Literal1.Text = "<span style='color:red'>加载" + lx + "：" + dt.Rows.Count + "条，共有数据" + zs + "条</span>";

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


                string chang = ""; string kuan = ""; string gao = ""; string zhongliang = ""; string tijizhongliang = ""; string yunfeihaiyunmeifang = ""; string yunfeikongyunmeigongjin = "";

                TextBox txtchang = rplb.Items[i].FindControl("txtchang") as TextBox;
                TextBox txtkuan = rplb.Items[i].FindControl("txtkuan") as TextBox;
                TextBox txtgao = rplb.Items[i].FindControl("txtgao") as TextBox;
                TextBox txtzhongliang = rplb.Items[i].FindControl("txtzhongliang") as TextBox;
                TextBox txttijizhongliang = rplb.Items[i].FindControl("txttijizhongliang") as TextBox;
                TextBox txtyunfeihaiyunmeifang = rplb.Items[i].FindControl("txtyunfeihaiyunmeifang") as TextBox;
                TextBox txtyunfeikongyunmeigongjin = rplb.Items[i].FindControl("txtyunfeikongyunmeigongjin") as TextBox;


                chang = txtchang.Text.Trim().Replace("'", "''");
                kuan = txtkuan.Text.Trim().Replace("'", "''");
                gao = txtgao.Text.Trim().Replace("'", "''");
                zhongliang = txtzhongliang.Text.Trim().Replace("'", "''");
                tijizhongliang = txttijizhongliang.Text.Trim().Replace("'", "''");
                yunfeihaiyunmeifang = txtyunfeihaiyunmeifang.Text.Trim().Replace("'", "''");
                yunfeikongyunmeigongjin = txtyunfeikongyunmeigongjin.Text.Trim().Replace("'", "''");

                if (chang != "" && kuan != "" && gao != "" && zhongliang != "" && tijizhongliang != "" && yunfeihaiyunmeifang != "" && yunfeikongyunmeigongjin != "")
                {
                    ky = true;
                }
                else
                {
                    ky = false;
                    lits.Text = "";
                    Response.Write("<script>alert('信息不能为空');</script>");
                    break;
                }
            }
            if (ky)
            {
                for (int i = 0; i < rplb.Items.Count; i++)
                {
                    string chang = ""; string kuan = ""; string gao = ""; string zhongliang = ""; string tijizhongliang = ""; string yunfeihaiyunmeifang = ""; string yunfeikongyunmeigongjin = "";

                    TextBox txtchang = rplb.Items[i].FindControl("txtchang") as TextBox;
                    TextBox txtkuan = rplb.Items[i].FindControl("txtkuan") as TextBox;
                    TextBox txtgao = rplb.Items[i].FindControl("txtgao") as TextBox;
                    TextBox txtzhongliang = rplb.Items[i].FindControl("txtzhongliang") as TextBox;
                    TextBox txttijizhongliang = rplb.Items[i].FindControl("txttijizhongliang") as TextBox;
                    TextBox txtyunfeihaiyunmeifang = rplb.Items[i].FindControl("txtyunfeihaiyunmeifang") as TextBox;
                    TextBox txtyunfeikongyunmeigongjin = rplb.Items[i].FindControl("txtyunfeikongyunmeigongjin") as TextBox;


                    chang = txtchang.Text.Trim().Replace("'", "''");
                    kuan = txtkuan.Text.Trim().Replace("'", "''");
                    gao = txtgao.Text.Trim().Replace("'", "''");
                    zhongliang = txtzhongliang.Text.Trim().Replace("'", "''");
                    tijizhongliang = txttijizhongliang.Text.Trim().Replace("'", "''");
                    yunfeihaiyunmeifang = txtyunfeihaiyunmeifang.Text.Trim().Replace("'", "''");
                    yunfeikongyunmeigongjin = txtyunfeikongyunmeigongjin.Text.Trim().Replace("'", "''");


                    string[] strs = new string[] { "chang", "kuan", "gao", "zhongliang", "tijizhongliang", "yunfeihaiyunmeifang", "yunfeikongyunmeigongjin" };
                    object[] objs = new object[] { chang, kuan, gao, zhongliang, tijizhongliang, yunfeihaiyunmeifang, yunfeikongyunmeigongjin };



                    string where = "";

                    Literal licid = rplb.Items[i].FindControl("licid") as Literal;
                    Literal liOffer_ID = rplb.Items[i].FindControl("liOffer_ID") as Literal;
                    Literal liSKU_ID = rplb.Items[i].FindControl("liSKU_ID") as Literal;

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



                    int sl = access_sql.T_Update_ExecSql(strs, objs, "caiwu", where);
                    cg += sl;

                }
            }


            lits.Text = "更新成功" + cg + "个";
        }
    }
}