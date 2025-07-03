using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 广东发出信息登记 : System.Web.UI.Page
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


        public void bindzhy()
        {
            string where = " ";


            if (txttitle.Text.Trim() != "")
            {
                string title = txttitle.Text.Trim().Replace("'", "''");
                where += " and (huopinbiaoti like '%" + title + "%' or rucangyinnibiaoti like '%" + title + "%') ";
            }
            if(txtputianfachudanhao.Text.Trim()!="")
            {
                where += " and (putianfachudanhao like '%"+ txtputianfachudanhao.Text.Trim().Replace("'", "''")+ "%') ";
            }

            string sql = "SELECT mainimg,haiwaicangrukudanhao,cid,rucangyinnibiaoti,dingdanbianhao,dingdanchuangjianshijian,Offer_ID,SKU_ID,huopinbiaoti,shuliang,putianfachushuliang,putianfachuriqi,putianfachudanhao,putianfachukuaidifeiyong,putianfachubeizhu,guangdongfachuriqi,guangdongfachudanhao,guangdongwuliuleixing,guangdongwuliufeiyong,wuliushangmingcheng FROM [SuMaiTongPol].[dbo].[caiwu] where dingdanzhuangtai<>'交易关闭' and isbctg=0 and  shangjiabianma='" + txtsjbm.Text.Trim().Replace("'", "''") + "' and (putianfachudanhao<>'' and putianfachudanhao is not null) " + where + " order by " + dporder.SelectedValue;
            string lx = "";
            string zs = "";


            DataSet ds = access_sql.GreatDs(sql);
            if (access_sql.yzTable(ds))
            {
                if (access_sql.yzTable(ds))
                {
                    DataTable dt = ds.Tables[0];
                    //DataTable dttemp = new DataTable();
                    //dttemp = dt.Clone();
                    //dttemp.Clear();
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    DataRow dr = dt.Rows[i];
                    //    string[] guangdongfachudanhao = dr["guangdongfachudanhao"].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries); ;
                    //    string[] putianfachudanhao = dr["putianfachudanhao"].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries); ;
                    //    if (guangdongfachudanhao.Length < putianfachudanhao.Length)
                    //    {
                    //        dttemp.Rows.Add(dr.ItemArray);
                    //    }

                    //}


                    rplb.DataSource = dt;
                    rplb.DataBind();
                    Literal1.Text = "<span style='color:red'>加载" + dt.Rows.Count + "条</span>";

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





                TextBox txtguangdongfachuriqi = e.Item.FindControl("txtguangdongfachuriqi") as TextBox;
                TextBox txtguangdongfachudanhao = e.Item.FindControl("txtguangdongfachudanhao") as TextBox;
                TextBox txtguangdongwuliuleixing = e.Item.FindControl("txtguangdongwuliuleixing") as TextBox;
                TextBox txthaiwaicangrukudanhao = e.Item.FindControl("txthaiwaicangrukudanhao") as TextBox;
                TextBox txtwuliushangmingcheng = e.Item.FindControl("txtwuliushangmingcheng") as TextBox;

                if (txthaiwaicangrukudanhao.Text.Trim() != "")
                {
                    string[] strs = new string[] { "guangdongfachuriqi", "guangdongfachudanhao", "guangdongwuliuleixing", "wuliushangmingcheng", "haiwaicangrukudanhao" };
                    object[] objs = new object[] {
                        txtguangdongfachuriqi.Text.Trim().Replace("'", "''"),
                        txtguangdongfachudanhao.Text.Trim().Replace("'", "''"),
                        txtguangdongwuliuleixing.Text.Trim().Replace("'", "''"),
                        txtwuliushangmingcheng.Text.Trim().Replace("'", "''"),

                        txthaiwaicangrukudanhao.Text.Trim().Replace("'", "''")
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
                else
                {
                    lits.Text = cid + "请输入必填信息";
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





                TextBox txtguangdongfachuriqi = rplb.Items[i].FindControl("txtguangdongfachuriqi") as TextBox;
                TextBox txtguangdongfachudanhao = rplb.Items[i].FindControl("txtguangdongfachudanhao") as TextBox;
                TextBox txtguangdongwuliuleixing = rplb.Items[i].FindControl("txtguangdongwuliuleixing") as TextBox;
                TextBox txthaiwaicangrukudanhao = rplb.Items[i].FindControl("txthaiwaicangrukudanhao") as TextBox;
                TextBox txtwuliushangmingcheng = rplb.Items[i].FindControl("txtwuliushangmingcheng") as TextBox;




                if (txthaiwaicangrukudanhao.Text == "")
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


                    TextBox txtguangdongfachuriqi = rplb.Items[i].FindControl("txtguangdongfachuriqi") as TextBox;
                    TextBox txtguangdongfachudanhao = rplb.Items[i].FindControl("txtguangdongfachudanhao") as TextBox;
                    TextBox txtguangdongwuliuleixing = rplb.Items[i].FindControl("txtguangdongwuliuleixing") as TextBox;
                    TextBox txthaiwaicangrukudanhao = rplb.Items[i].FindControl("txthaiwaicangrukudanhao") as TextBox;
                    TextBox txtwuliushangmingcheng = rplb.Items[i].FindControl("txtwuliushangmingcheng") as TextBox;


                    string[] strs = new string[] { "guangdongfachuriqi", "guangdongfachudanhao", "guangdongwuliuleixing", "wuliushangmingcheng", "haiwaicangrukudanhao" };
                    object[] objs = new object[] {
                        txtguangdongfachuriqi.Text.Trim().Replace("'", "''"),
                        txtguangdongfachudanhao.Text.Trim().Replace("'", "''"),
                        txtguangdongwuliuleixing.Text.Trim().Replace("'", "''"),
                        txtwuliushangmingcheng.Text.Trim().Replace("'", "''"),
                        txthaiwaicangrukudanhao.Text.Trim().Replace("'", "''")
                    };

                    cg += access_sql.T_Update_ExecSql(strs, objs, "caiwu", "cid=" + licid + "");
                   








                }
            }


            lits.Text = "更新成功" + cg + "个";
        }
    }
}