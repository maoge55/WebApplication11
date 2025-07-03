using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 财务表补充信息 : System.Web.UI.Page
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
                lx = "信息为空数据";

                zs = access_sql.GetOneValue("select count(*) from caiwu where dingdanzhuangtai<>'交易关闭' and isbctg=0 and  Offer_ID<>'' and Offer_ID is not null and shangjiabianma='" + txtsjbm.Text.Replace("'", "''") + "' and ((yunyingbianma is null or yunyingbianma='')  or (rucangSKUID is null or rucangSKUID='') or (rucangITEMID is null or rucangITEMID='')  or (rucangyinnibiaoti is null or rucangyinnibiaoti=''))");

                sql = "select top 1 *,'https://detail.1688.com/offer/'+[Offer_ID]+'.html' as [1688链接] from caiwu where dingdanzhuangtai<>'交易关闭' and isbctg=0 and  Offer_ID<>'' and Offer_ID is not null and shangjiabianma='" + txtsjbm.Text.Replace("'", "''") + "' and ((yunyingbianma is null or yunyingbianma='')  or (rucangSKUID is null or rucangSKUID='') or (rucangITEMID is null or rucangITEMID='')  or (rucangyinnibiaoti is null or rucangyinnibiaoti='')) order by maijiagongsiming_m";
            }
            else if (type == 0)
            {

                lx = "信息为0数据";

                zs = access_sql.GetOneValue("select count(*) from caiwu where  isbctg=0 and  Offer_ID<>'' and Offer_ID is not null and shangjiabianma='" + txtsjbm.Text.Replace("'", "''") + "' and ((yunyingbianma='0')  or (rucangSKUID='0') or (rucangITEMID='0')  or (rucangyinnibiaoti='0'))");

                sql = "select top 1 *,'https://detail.1688.com/offer/'+[Offer_ID]+'.html' as [1688链接] from caiwu where dingdanzhuangtai<>'交易关闭' and isbctg=0 and  Offer_ID<>'' and Offer_ID is not null and shangjiabianma='" + txtsjbm.Text.Replace("'", "''") + "'  and ((yunyingbianma='0')  or (rucangSKUID='0') or (rucangITEMID='0')  or (rucangyinnibiaoti='0')) order by maijiagongsiming_m";

            }
            else if (type == 2)
            {

                lx = "跳过的数据";

                zs = access_sql.GetOneValue("select count(*) from caiwu where isbctg=1");

                sql = "select  *,'https://detail.1688.com/offer/'+[Offer_ID]+'.html' as [1688链接] from caiwu where isbctg=1  order by maijiagongsiming_m";

            }
            else if (type ==3)
            {

                lx = "标题搜索";

                zs = access_sql.GetOneValue("select count(*) from caiwu where isbctg=0 and dingdanzhuangtai<>'交易关闭' and  huopinbiaoti like '%"+txthuopinbiaoti.Text.Trim().Replace("'","''")+"%'");

                sql = "select  *,'https://detail.1688.com/offer/'+[Offer_ID]+'.html' as [1688链接] from caiwu where  isbctg=0 and dingdanzhuangtai<>'交易关闭' and  huopinbiaoti like '%" + txthuopinbiaoti.Text.Trim().Replace("'", "''") + "%'";

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
        public void bindimgs(DataList liimgxs, string imgs, string cid)
        {
            string[] iiii = imgs.Split('|');
            DataTable dtimgs = new DataTable();
            dtimgs.Columns.Add("guangdongfachudanhao");
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
            if (e.CommandName == "upimg")
            {
                lits.Text = "";
                Literal licid = e.Item.FindControl("licid") as Literal;
                Literal limainimg = e.Item.FindControl("limainimg") as Literal;
                Literal liOffer_ID = e.Item.FindControl("liOffer_ID") as Literal;
                Literal liSKU_ID = e.Item.FindControl("liSKU_ID") as Literal;
                string where = "";
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
                FileUpload ff = e.Item.FindControl("FileUpload1") as FileUpload;

                string imgs = access_sql.GetOneValue("select top 1  mainimg from caiwu where " + where) + "|";
                for (int i = 0; i < ff.PostedFiles.Count; i++)
                {
                    string type = ff.PostedFiles[i].FileName.Split('.')[ff.PostedFiles[i].FileName.Split('.').Length - 1];
                    string imgname = DateTime.Now.ToString("yyyyMMddHHmmss") + i + "." + type;
                    string savename = Server.MapPath("~/Uploads/") + imgname;
                    ff.PostedFiles[i].SaveAs(savename);
                    imgs = imgname;
                }
                if (imgs != "")
                {
                    imgs = imgs.Replace("||", "|");
                    access_sql.T_Update_ExecSql(new string[] { "mainimg" }, new object[] { imgs }, "caiwu", where);
                    limainimg.Visible = true;
                    limainimg.Text = "";
                    limainimg.Text = " <img src='/Uploads/" + imgs + "' style='width:300px; height:300px' />";
                }

            }
            if (e.CommandName == "tg")
            {
                RadioButtonList rdsjtype = e.Item.FindControl("rdsjtype") as RadioButtonList;
                if (rdsjtype.SelectedValue != "发仓货物")
                {

                    Literal licid = e.Item.FindControl("licid") as Literal;
                    Literal liOffer_ID = e.Item.FindControl("liOffer_ID") as Literal;
                    Literal liSKU_ID = e.Item.FindControl("liSKU_ID") as Literal;
                    string where = "";
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




                    string[] strs = new string[] { "sjtype", "isbctg" };
                    object[] objs = new object[] { rdsjtype.SelectedValue, 1 };





                    int sl = access_sql.T_Update_ExecSql(strs, objs, "caiwu", where);
                    lits.Text = "跳过成功,请手动加载下一条数据";
                    // bindzhy(1);
                }
                else
                {
                    lits.Text = "想要跳过，必须选择阿里狗或者耗材";
                    Response.Write("<script>alert('" + lits.Text + "');</script>");
                }
            }

            if (e.CommandName == "qr")
            {

                Literal licid = e.Item.FindControl("licid") as Literal;
                Literal liOffer_ID = e.Item.FindControl("liOffer_ID") as Literal;
                Literal liSKU_ID = e.Item.FindControl("liSKU_ID") as Literal;
                string where = "";
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

                bool ky = true;

                string yunyingbianma = "";
                string rucangITEMID = "";
                string rucangSKUID = "";
                string rucangyinnibiaoti = "";





                TextBox txtyunyingbianma = e.Item.FindControl("txtyunyingbianma") as TextBox;
                TextBox txtrucangITEMID = e.Item.FindControl("txtrucangITEMID") as TextBox;
                TextBox txtrucangSKUID = e.Item.FindControl("txtrucangSKUID") as TextBox;
                TextBox txtrucangyinnibiaoti = e.Item.FindControl("txtrucangyinnibiaoti") as TextBox;


                yunyingbianma = txtyunyingbianma.Text.Trim().Replace("'", "''");
                rucangITEMID = txtrucangITEMID.Text.Trim().Replace("'", "''");
                rucangSKUID = txtrucangSKUID.Text.Trim().Replace("'", "''");
                rucangyinnibiaoti = txtrucangyinnibiaoti.Text.Trim().Replace("'", "''");
                int isbctg = 0;
                RadioButtonList rdsjtype = e.Item.FindControl("rdsjtype") as RadioButtonList;
                if (rdsjtype.SelectedValue != "发仓货物")
                {
                    isbctg = 1;
                }

                if (yunyingbianma != "" && rucangITEMID != "" && rucangSKUID != "" && rucangyinnibiaoti != "")
                {
                    string upstr = "";

                    string[] strs = new string[] { "yunyingbianma", "rucangITEMID", "rucangSKUID", "rucangyinnibiaoti", "sjtype", "isbctg" };
                    object[] objs = new object[] { yunyingbianma, rucangITEMID, rucangSKUID, rucangyinnibiaoti, rdsjtype.SelectedValue, isbctg };




                    int sl = access_sql.T_Update_ExecSql(strs, objs, "caiwu", where);

                    lits.Text = "更新数据成功,请手动加载下一条数据";
                    //bindzhy(1);

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

        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RadioButtonList rdsjtype = e.Item.FindControl("rdsjtype") as RadioButtonList;



            Literal lisjtype = e.Item.FindControl("lisjtype") as Literal;

            rdsjtype.SelectedValue = lisjtype.Text;


            Literal limainimg = e.Item.FindControl("limainimg") as Literal;
            Literal limainimg222 = e.Item.FindControl("limainimg222") as Literal;
            limainimg.Visible = true;
            limainimg.Text = " <img src='/Uploads/" + limainimg222.Text + "' style='width:300px; height:300px' />";
        }

        protected void Button3_Click1(object sender, EventArgs e)
        {
            lits.Text = "";
            if (txtsjbm.Text.Trim() != "")
            {
                rplb.DataSource = null;
                rplb.DataBind();
                Literal1.Text = "";
                bindzhy(2);
            }
            else
            {
                lits.Text = "请输入商家编码";
            }
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            lits.Text = "";
            if (txtsjbm.Text.Trim() != "")
            {
                rplb.DataSource = null;
                rplb.DataBind();
                Literal1.Text = "";
                bindzhy(3);
            }
            else
            {
                lits.Text = "请输入商家编码";
            }
        }
    }
}