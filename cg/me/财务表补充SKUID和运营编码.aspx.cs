using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 财务表补充SKUID和运营编码 : System.Web.UI.Page
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
                if (uid != "8" && uid != "9")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";


        public string Offer_ID = "";
        public string SKU_ID = "";
        public string huopinbiaoti = "";
        public string danjia = "";


        public void bindzhy()
        {
            string sql = "";

            if (litype.Text == "0")
            {
                sql = "select top 1 * from caiwu  where (rucangSKUID is null or rucangSKUID='' or rucangSKUID='0') and istg=0 order by cid ";
            }
            else
            {
                sql = "select top 1 * from caiwu  where (rucangSKUID is null or rucangSKUID='' or rucangSKUID='0') and istg>0 order by istg,cid ";
            }


            DataSet dscw = access_sql.GreatDs(sql);
            if (access_sql.yzTable(dscw))
            {


                DataRow dr = dscw.Tables[0].Rows[0];
                licid.Text = dr["cid"].ToString();
                Offer_ID = dr["Offer_ID"].ToString();
                SKU_ID = dr["SKU_ID"].ToString();
                huopinbiaoti = dr["huopinbiaoti"].ToString();
                danjia = dr["danjia"].ToString();
                DataSet dscdb = access_sql.GreatDs("select * from YNBigData where Y_1688url like '%" + Offer_ID + "%' order by id");
                if (access_sql.yzTable(dscdb))
                {
                    lits.Text = "";
                    Repeater1.DataSource = dscdb.Tables[0];
                    Repeater1.DataBind();




                }
                else
                {
                    Repeater1.DataSource = null;
                    Repeater1.DataBind();
                    lits.Text = "出单表无对应数据！请选择跳过";
                }

                string zs = access_sql.GetOneValue("select count(*) from caiwu  where (rucangSKUID is null or rucangSKUID='' or rucangSKUID='0')");
                string tg = access_sql.GetOneValue("select count(*) from caiwu  where (rucangSKUID is null or rucangSKUID='' or rucangSKUID='0') and istg>0 ");

                Literal1.Text = "<span style='color:red'>" + "共有" + zs + "条数据需要审核，跳过的有" + tg + "条</span>";







            }
            else
            {
                lits.Text = "无数据"; ;
            }
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            lits.Text = "";
            litype.Text = "0";
            bindzhy();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }







        protected void Button2_Click2(object sender, EventArgs e)
        {
        }

        protected void Button3_Click2(object sender, EventArgs e)
        {

        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "pp")
            {
                lits.Text = "";
                string id = licid.Text;
                Literal liskuid = (Literal)e.Item.FindControl("liskuid");
                Literal liyybm = (Literal)e.Item.FindControl("liyybm");



                if (access_sql.T_Update_ExecSql(new string[] { "yunyingbianma", "rucangSKUID" }, new object[] { liyybm.Text, liskuid.Text }, "caiwu", "cid=" + id + "") > 0)
                {
                    bindzhy();
                    lits.Text = "ID:" + id + "匹配成功";
                }

            }
        }

        protected void Button3_Click1(object sender, EventArgs e)
        {
            access_sql.DoSql("update caiwu set istg=istg+1 where cid=" + licid.Text + "");
            bindzhy();
        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            litype.Text = "1";
            bindzhy();
        }
    }
}