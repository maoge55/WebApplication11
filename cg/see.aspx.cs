using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class see : System.Web.UI.Page
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
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";
        public void bind(string type)
        {
            string sql = "";

            if (type == "正常")
            {
                sql = "select id,skuid,itemid,code,Quantity,kefadehaiyun,ROW_NUMBER() OVER(PARTITION BY code ORDER BY code) AS rn from [YNBigData] where code in(SELECT code FROM [YNBigData] where baohanTiaoguo=0 group by code  HAVING SUM(Quantity)>=2) order by code";
            }
            else
            {
                sql = "select id,skuid,itemid,code,Quantity,kefadehaiyun,ROW_NUMBER() OVER(PARTITION BY code ORDER BY code) AS rn from [YNBigData] where code in(SELECT code FROM [YNBigData] where baohanTiaoguo=1 group by code  HAVING SUM(Quantity)>=2) order by code";
            }





            DataSet ds = access_sql.GreatDs(sql);
            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];
                DataRow[] codeList = dt.Select("rn=1");
                int cg = 0;
                DataTable dtls = new DataTable();
                dtls.Columns.Add("code");
                for (int i = 0; i < codeList.Length; i++)
                {
                    string code = codeList[i]["code"].ToString();
                    DataRow[] allrowsbycode = dt.Select("code='" + code + "'");

                    string kefadehaiyun = "";
                    foreach (DataRow dr in allrowsbycode)
                    {
                        if (dr["kefadehaiyun"].ToString() != "" && dr["kefadehaiyun"].ToString() != "0" && dr["kefadehaiyun"].ToString().ToLower() != "null")
                        {
                            kefadehaiyun = dr["kefadehaiyun"].ToString();
                            break;
                        }
                    }
                    if (kefadehaiyun == "")
                    {
                        dtls.Rows.Add(new object[] { code });
                        cg++;
                    }


                }

                lits.Text = "查询到" + (type) + "需审核的总数量为" + cg + "个";


                if (cg > 0)
                {

                    string codes = "";
                    for (int i = 0; i < 1; i++)
                    {
                        if (dtls.Rows.Count >= i)
                        {
                            codes += "'" + dtls.Rows[i][0] + "',";
                        }
                    }
                    if (codes != "") { codes = codes.Substring(0, codes.Length - 1); }

                    string sqlcp = "SELECT  id,itemid,Title,Purl,Y_1688url,phitemid,MainImage,code,ROW_NUMBER() OVER(PARTITION BY code ORDER BY code) AS rn  FROM YNBigData where code in(" + codes + ")";
                    DataSet dspd = access_sql.GreatDs(sqlcp);

                    if (access_sql.yzTable(dspd))
                    {
                        DataTable dtpd = dspd.Tables[0];
                        DataRow[] codeListpd = dtpd.Select("rn=1");

                        DataTable dtbdpd = new DataTable();

                        dtbdpd.Columns.Add("code");
                        dtbdpd.Columns.Add("Title");
                        dtbdpd.Columns.Add("Y_1688url");
                        dtbdpd.Columns.Add("MainImage");



                        for (int i = 0; i < codeListpd.Length; i++)
                        {
                            string code = codeListpd[i]["code"].ToString();
                            DataRow[] allrowsbycode = dtpd.Select("code='" + code + "'");

                            string title = "";
                            string Y_1688url = "";
                            string MainImage = "";
                            foreach (DataRow dr in allrowsbycode)
                            {
                                if (dr["Y_1688url"].ToString().IndexOf("1688") != -1)
                                {
                                    Y_1688url = dr["Y_1688url"].ToString();
                                    break;
                                }
                            }
                            foreach (DataRow dr in allrowsbycode)
                            {
                                if (dr["Title"].ToString() != "" && dr["Title"].ToString() != "0" && dr["Title"].ToString().ToLower() != "null")
                                {
                                    title = dr["Title"].ToString();
                                    break;
                                }
                            }
                            foreach (DataRow dr in allrowsbycode)
                            {
                                if (dr["MainImage"].ToString() != "" && dr["MainImage"].ToString() != "0" && dr["MainImage"].ToString().ToLower() != "null")
                                {
                                    MainImage = dr["MainImage"].ToString();
                                    break;
                                }
                            }
                            if (MainImage == "" && code != "")
                            {
                                MainImage = access_sql.GetOneValue("select  top 1 image from ProShopeePh where itemid =(select top 1 PHItemid from YNBigData where code='" + code + "')");
                            }
                            if (Y_1688url != "")
                            {
                                Y_1688url = "<a href='" + Y_1688url + "' target='_blank'>1688采购链接</a>";
                            }
                            else
                            {
                                Y_1688url = "<span style='color:red'>无1688链接</span>";
                            }
                            dtbdpd.Rows.Add(new object[] { code, title, Y_1688url, MainImage });
                        }
                        if (dtbdpd != null && dtbdpd.Rows.Count > 0)
                        {
                            rplb.DataSource = dtbdpd;
                            rplb.DataBind();
                        }
                    }



                }
            }
            else
            {
                lits.Text = "查询到" + (type) + "需审核的总数量为0个";
                rplb.DataSource = null;
                rplb.DataBind();
            }



         

        }


        public string getwhere(string type)
        {
            string ru = "";
            if (uid != "")
            {
                int hyjd = int.Parse(access_sql.GetOneValue("select us from yn_user where id=5"));



                string where = "";
                if (type == "1" && hyjd == 0)
                {

                    where += "where   shangjiabianma='HB8897' ";
                    where += " and (code in (SELECT  code FROM [YNBigData] group by code  HAVING SUM(Quantity)>=5))";
                    where += " and (Y_1688url like '%1688%')";
                    where += " and (kefadehaiyun is null or kefadehaiyun='' or kefadehaiyun='0')";
                }
                else if (type == "2" && hyjd == 1)
                {
                    where += " where   shangjiabianma='HB8897' ";
                    where += " and ((Y_1688url like '%1688%' and code in (SELECT  code FROM [YNBigData] group by code  HAVING SUM(Quantity)>=3) and (kefadehaiyun is null or kefadehaiyun='' or kefadehaiyun='0')) ";
                    where += " or (code in (SELECT  code FROM [YNBigData] group by code  HAVING SUM(Quantity)>=5) and Y_1688url like '%1688%' and kefadehaiyun='轻舟不能发')";
                    where += " or (Y_1688url like '%1688%' and kefadehaiyun='轻舟' and shangjiamingzi='雅仓不能入'))";
                }

                else if (type == "3" && hyjd == 2)
                {
                    where += "where (Y_1688url like '%1688%'  and kefadehaiyun='轻舟')  and (shangjiamingzi is null or shangjiamingzi='' or shangjiamingzi='0')  and shangjiabianma='HB8897'";

                }
                else if (type == "4" && hyjd == 3)
                {
                    where += " where   shangjiabianma='HB8897' ";
                    where += "  and((Y_1688url like '%1688%'  and kefadehaiyun='宝涵' and (shangjiamingzi is null or shangjiamingzi='' or shangjiamingzi='0'))";
                    where += "  or (Y_1688url like '%1688%'  and shangjiamingzi='雅仓不能入'))";
                }
                else if (type == "7")
                {
                    where += "where (Y_1688url like '%1688%' and code in (SELECT  code FROM [YNBigData] group by code  HAVING SUM(Quantity)>=3) and (kefadehaiyun is null or kefadehaiyun='' or kefadehaiyun='0') and shangjiabianma='zzw888')";
                }

                string sql = "" + where;

                if (where != "")
                {
                    ru = sql;
                }
            }


            return ru;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            bind("正常");
        }
        public void cl()
        {
            int cg = 0;
            int tgcount = 0;
            bool ky = true;
            for (int i = 0; i < rplb.Items.Count; i++)
            {

                RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");
                if (rdjg.SelectedValue == "")
                {
                    ky = false;
                    lits.Text = "第" + (i + 1) + "还未选择";
                    Response.Write("<script>alert('第" + (i + 1) + "还未选择');</script>");
                    break;
                }
            }
            if (ky)
            {
                for (int i = 0; i < rplb.Items.Count; i++)
                {
                    Literal licode = (Literal)rplb.Items[i].FindControl("licode");
                    RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");
                    string selectvalue = rdjg.SelectedValue;
                    TextBox txtbhjg = (TextBox)rplb.Items[i].FindControl("txtbhjg");
                    TextBox txtbhbz = (TextBox)rplb.Items[i].FindControl("txtbhbz");
                    string kefadehaiyun = "";
                    bool tg = false;
                    if (rdjg.SelectedValue == "yes")
                    {
                        kefadehaiyun = "宝涵";
                    }
                    else if (rdjg.SelectedValue == "no")
                    {
                        kefadehaiyun = "宝涵不能发";
                    }
                    else
                    {
                        tg = true;
                    }

                    if (!tg)
                    {
                        string[] str = new string[] { "kefadehaiyun", "baohanhaiyunjiage", "baohanhaiyunbeizhu", "baohanTiaoguo" };
                        object[] obj = new object[] { kefadehaiyun, txtbhjg.Text.Trim().Replace("'", "''"), txtbhbz.Text.Trim().Replace("'", "''"), 0 };


                        if (access_sql.T_Update_ExecSql(str, obj, "YNBigData", "code='" + licode.Text + "'") > 0)
                        {
                            cg++;
                        }
                    }
                    else
                    {
                        if (access_sql.T_Update_ExecSql(new string[] { "baohanTiaoguo" }, new object[] { 1 }, "YNBigData", "code='" + licode.Text + "'") > 0)
                        {
                            tgcount++;
                        }

                    }
                }
                lits.Text = "成功保存" + cg + "个,跳过" + tgcount + "个";
            }
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            cl();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            cl();
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            bind("跳过");
        }
    }
}