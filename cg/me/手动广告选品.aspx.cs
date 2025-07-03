using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 手动广告选品 : System.Web.UI.Page
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
                if (uid != "6" && uid != "9" && uid != "18" && uid != "19")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }

            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";



        public void deleteyt(int pid)
        {

            DataTable dt = (DataTable)Session["mdt"];
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["pid"].ToString() == pid.ToString())
                    {
                        dt.Rows.Remove(dt.Rows[i]);
                        break;
                    }
                }


                rplb.DataSource = dt;
                rplb.DataBind();
                Literal1.Text = "<span style='color:red'>加载数据" + dt.Rows.Count + "条</span>";
            }
            else
            {
                lits.Text = "会话状态过期，请关闭浏览器重新打开";
            }



        }




        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }
        public string getnuuu(TextBox rrr)
        {
            return rrr.Text.Trim().Replace("'", "''");
        }
        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "jiage")
            {
                TextBox txtY_1688price = e.Item.FindControl("txtY_1688price") as TextBox;
                TextBox txtyinnixiapishoujia = e.Item.FindControl("txtyinnixiapishoujia") as TextBox;
                double yinnixiapishoujia = 0;
                if (txtY_1688price.Text != "")
                {
                    yinnixiapishoujia = float.Parse(getnuuu(txtY_1688price)) * 2.1 * 2250;
                    if (yinnixiapishoujia < 169000)
                    {
                        yinnixiapishoujia = 169000;
                    }
                    txtyinnixiapishoujia.Text = yinnixiapishoujia.ToString();
                }
            }

            if (e.CommandName == "up")
            {
                lits.Text = "";
                int id = int.Parse(e.CommandArgument.ToString());
                TextBox txtsjbm = e.Item.FindControl("txtsjbm") as TextBox;
                TextBox txtyuanshilianjie = e.Item.FindControl("txtyuanshilianjie") as TextBox;
                TextBox txtbiaoti = e.Item.FindControl("txtbiaoti") as TextBox;
                TextBox txtshangjiadianpulianjie = e.Item.FindControl("txtshangjiadianpulianjie") as TextBox;
                TextBox txtbeizhu = e.Item.FindControl("txtbeizhu") as TextBox;
                TextBox txtY_1688url = e.Item.FindControl("txtY_1688url") as TextBox;
                TextBox txtY_1688price = e.Item.FindControl("txtY_1688price") as TextBox;
                TextBox txtshangjiadianpu = e.Item.FindControl("txtshangjiadianpu") as TextBox;
                TextBox txtguanggaolianjie = e.Item.FindControl("txtguanggaolianjie") as TextBox;
                RadioButtonList rdhuoyuan = (RadioButtonList)e.Item.FindControl("rdhuoyuan");
                RadioButtonList rdshangjia = (RadioButtonList)e.Item.FindControl("rdshangjia");
                TextBox txtyinnixiapishoujia = e.Item.FindControl("txtyinnixiapishoujia") as TextBox;
                string yinnixiapishoujia = "0";
                if (txtyinnixiapishoujia.Text != "")
                {
                    yinnixiapishoujia = txtyinnixiapishoujia.Text.Trim();
                }

                //if (txtY_1688price.Text != "")
                //{
                //    yinnixiapishoujia = float.Parse(getnuuu(txtY_1688price)) * 2.1 * 2250;
                //    if (yinnixiapishoujia < 169000)
                //    {
                //        yinnixiapishoujia = 169000;
                //    }
                //}

                string[] name = new string[] { "sjbm", "yuanshilianjie", "biaoti", "shangjiadianpulianjie", "beizhu", "huoyuan", "Y_1688url", "Y_1688price", "shangjiadianpu", "yinnixiapishoujia", "shangjia", "guanggaolianjie" };
                object[] ooo = new object[] { getnuuu(txtsjbm), getnuuu(txtyuanshilianjie), getnuuu(txtbiaoti), getnuuu(txtshangjiadianpulianjie), getnuuu(txtbeizhu), rdhuoyuan.SelectedValue, getnuuu(txtY_1688url), getnuuu(txtY_1688price), getnuuu(txtshangjiadianpu), yinnixiapishoujia, rdshangjia.SelectedValue, getnuuu(txtguanggaolianjie) };




                if (access_sql.T_Update_ExecSql(name, ooo, "mapd", "id=" + id + "") > 0)
                {

                    lits.Text = "ID:" + id + "更新成功";
                }
                else
                {
                    lits.Text = "ID:" + id + "更新失败";
                }





            }
        }

        public void clzy()
        {
            int cg = 0;
            int sb = 0;
            for (int i = 0; i < rplb.Items.Count; i++)
            {
                Literal liid = (Literal)rplb.Items[i].FindControl("liid");



                int id = int.Parse(liid.Text);
                TextBox txtsjbm = rplb.Items[i].FindControl("txtsjbm") as TextBox;
                TextBox txtyuanshilianjie = rplb.Items[i].FindControl("txtyuanshilianjie") as TextBox;
                TextBox txtbiaoti = rplb.Items[i].FindControl("txtbiaoti") as TextBox;
                TextBox txtshangjiadianpulianjie = rplb.Items[i].FindControl("txtshangjiadianpulianjie") as TextBox;
                TextBox txtbeizhu = rplb.Items[i].FindControl("txtbeizhu") as TextBox;
                TextBox txtY_1688url = rplb.Items[i].FindControl("txtY_1688url") as TextBox;
                TextBox txtY_1688price = rplb.Items[i].FindControl("txtY_1688price") as TextBox;
                TextBox txtshangjiadianpu = rplb.Items[i].FindControl("txtshangjiadianpu") as TextBox;
                TextBox txtguanggaolianjie = rplb.Items[i].FindControl("txtguanggaolianjie") as TextBox;
                RadioButtonList rdhuoyuan = (RadioButtonList)rplb.Items[i].FindControl("rdhuoyuan");
                RadioButtonList rdshangjia = (RadioButtonList)rplb.Items[i].FindControl("rdshangjia");
                double yinnixiapishoujia = 0;
                if (txtY_1688price.Text != "")
                {
                    yinnixiapishoujia = float.Parse(getnuuu(txtY_1688price)) * 2.1 * 2250;
                    if (yinnixiapishoujia < 169000)
                    {
                        yinnixiapishoujia = 169000;
                    }
                }

                string[] name = new string[] { "sjbm", "yuanshilianjie", "biaoti", "shangjiadianpulianjie", "beizhu", "huoyuan", "Y_1688url", "Y_1688price", "shangjiadianpu", "yinnixiapishoujia", "shangjia", "guanggaolianjie" };
                object[] ooo = new object[] { getnuuu(txtsjbm), getnuuu(txtyuanshilianjie), getnuuu(txtbiaoti), getnuuu(txtshangjiadianpulianjie), getnuuu(txtbeizhu), rdhuoyuan.SelectedValue, getnuuu(txtY_1688url), getnuuu(txtY_1688price), getnuuu(txtshangjiadianpu), yinnixiapishoujia, rdshangjia.SelectedValue, getnuuu(txtguanggaolianjie) };




                if (access_sql.T_Update_ExecSql(name, ooo, "mapd", "id=" + id + "") > 0)
                {

                    cg++;
                }
                else
                {
                    sb++;

                }

            }



            lits.Text = "更新成功" + cg + "个，失败" + sb + "个";
        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            clzy();
        }

        protected void Button3_Click1(object sender, EventArgs e)
        {
            clzy();
        }




        protected void Button1_Click1(object sender, EventArgs e)
        {

        }



        protected void btncjsj_Click(object sender, EventArgs e)
        {
            if (txtinoffid.Text.Trim() != "" && txtsjbm_in.Text.Trim() != "")
            {
                string[] urls = txtinoffid.Text.Trim().Replace("\r\n", "|").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                string sjbm = txtsjbm_in.Text.Trim().Replace("'", "''");
                int cg = 0;
                int cz = 0;

                for (int i = 0; i < urls.Length; i++)
                {
                    string url = urls[i].Trim().Replace("'", "''");
                    string pid = "";
                    if (url.IndexOf("?") != -1 && url.IndexOf("-i.") != -1)
                    {
                        string aaa = url.Split('?')[0];
                        aaa = aaa.Replace("-i.", "镍");
                        pid = "-i." + aaa.Split('镍')[1];
                    }
                    bool ky = true;
                    if (pid != "")
                    {
                        if (access_sql.GetOneValue("select count(1) from mapd where  iid='" + pid + "'") != "0")
                        {
                            ky = false;
                        }
                    }
                    else
                    {
                        if (access_sql.GetOneValue("select count(1) from mapd where  yuanshilianjie='" + url + "'") != "0")
                        {
                            ky = false;
                        }
                    }
                    if (access_sql.GetOneValue("select count(1) from phlmurl where  phurl like '%" + pid + "%'") != "0")
                    {
                        ky = false;
                    }

                    if (ky)
                    {
                        if (access_sql.T_Insert_ExecSql(new string[] { "sjbm", "yuanshilianjie", "iid" }, new object[] { sjbm, url, pid }, "mapd") != 0)
                        {
                            cg++;
                        }
                    }
                    else
                    {
                        cz++;
                    }
                }
                lits.Text = "成功添加" + cg + "条，重复" + cz + "条";

            }
            else
            {
                lits.Text = "请输入商家编码和原始链接";
                Response.Write("<script>alert('"+ lits.Text + "');</script>");
            }
        }

        protected void Button1_Click2(object sender, EventArgs e)
        {
            if (txtsjbm.Text.Trim() != "" || uid == "9")
            {
                rplb.DataSource = null;
                rplb.DataBind();
                lits.Text = "";
                Literal1.Text = "";
                string where = "where  1=1 ";
                string orrrr = "";
                if (txtsjbm.Text.Trim() != "")
                {
                    where += "and (sjbm='" + txtsjbm.Text.Trim().Replace("'", "''") + "')  ";
                }
                if (ckbt.Checked)
                {
                    where += "and ( biaoti is null or biaoti='') ";
                }
                if (ckhy.Checked)
                {
                    where += "and ( huoyuan=0 ) ";
                }
                if (cksj.Checked)
                {
                    where += "and ( shangjia=0 ) ";
                }
                if (ck1688.Checked)
                {
                    where += "and ( Y_1688url is not null and Y_1688url<>'') ";
                }



                DataSet ds = access_sql.GreatDs("select * from mapd " + where + " order by " + dporder.SelectedValue);
                if (access_sql.yzTable(ds))
                {
                    rplb.DataSource = ds.Tables[0];
                    rplb.DataBind();
                    Literal1.Text = "加载数据" + ds.Tables[0].Rows.Count + "条";
                }
            }
            else
            {
                Literal1.Text = "输入商家编码";
            }
        }

        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RadioButtonList rdhuoyuan = e.Item.FindControl("rdhuoyuan") as RadioButtonList;
            Literal lihuoyuan = e.Item.FindControl("lihuoyuan") as Literal;

            rdhuoyuan.SelectedValue = lihuoyuan.Text;



            RadioButtonList rdshangjia = e.Item.FindControl("rdshangjia") as RadioButtonList;
            Literal lishangjia = e.Item.FindControl("lishangjia") as Literal;
            rdshangjia.SelectedValue = lishangjia.Text;


        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            DataTable dt = access_sql.GreatDs("select * from mapd where yuanshilianjie like '%-i.%' and iid is null").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string id = dt.Rows[i]["id"].ToString();
                string yuanshilianjie = dt.Rows[i]["yuanshilianjie"].ToString();
                string aaa = yuanshilianjie.Split('?')[0];
                aaa = aaa.Replace("-i.", "镍");
                string p = "-i." + aaa.Split('镍')[1];
                access_sql.T_Update_ExecSql(new string[] { "iid" }, new object[] { p }, "mapd", "id=" + id + "");
            }
            lits.Text = "111";
        }
    }

}