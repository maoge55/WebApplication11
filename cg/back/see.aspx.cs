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
        public void bind()
        {
            string where = getwhere(uid);
            if (where != "")
            {
                string sql = "SELECT  id,itemid,Title,Purl,Y_1688url,phitemid,ROW_NUMBER() OVER(PARTITION BY itemid ORDER BY itemid) AS rn  FROM [YNBigData] " + where;
                string ssss = "WITH RankedImages AS(" + sql + ") SELECT top 15 id,itemid,Title,Purl,Y_1688url,phitemid FROM RankedImages WHERE rn = 1 order by id";
                DataSet dscount = access_sql.GreatDs("SELECT itemid  FROM [YNBigData] " + where + " group by itemid");
                int cc = 0;
                if (access_sql.yzTable(dscount)) { cc = dscount.Tables[0].Rows.Count; }
                Literal1.Text = "&nbsp;&nbsp;共有数据<span style='color:red;font-weight: bold;'>" + cc + "</span>条";
                DataSet ds = access_sql.GreatDs(ssss);
                if (access_sql.yzTable(ds))
                {
                    DataTable dt = ds.Tables[0];
                    dt.Columns.Add("img");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string phitemid = dt.Rows[i]["phitemid"].ToString();

                        if (phitemid != "")
                        {
                            string img = access_sql.GetOneValue("select image from ProShopeePh where itemID='" + phitemid + "'");
                            dt.Rows[i]["img"] = img;
                        }


                    }
                    rplb.DataSource = dt;
                    rplb.DataBind();
                }
            }
            else
            {
                Literal1.Text = "&nbsp;&nbsp;查询语句出错";
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
            bind();
        }
        public void cl()
        {
            lits.Text = "";
            bool wwc = false;
            string jg_no = "";
            string jg_yes = "";
            string[] bhlist = new string[rplb.Items.Count];
            int cc = 0;
            string type = uid;
            for (int i = 0; i < rplb.Items.Count; i++)
            {
                string itemid = "";
                Literal litemid = (Literal)rplb.Items[i].FindControl("litemid");
                if (litemid != null)
                {
                    itemid = litemid.Text;
                }
                RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");
                string selectvalue = rdjg.SelectedValue;
                if (selectvalue != "")
                {
                    if (type == "2" || type == "7")
                    {
                        string adnr = "";
                        TextBox txtbhjg = (TextBox)rplb.Items[i].FindControl("txtbhjg");
                        TextBox txtbhbz = (TextBox)rplb.Items[i].FindControl("txtbhbz");
                        adnr = itemid + "镍{0}镍" + txtbhjg.Text.Trim().Replace("'", "''") + "镍" + txtbhbz.Text.Trim().Replace("'", "''");
                        if (selectvalue == "yes")
                        {
                            adnr = adnr.Replace("{0}", "YES");
                        }
                        else
                        {
                            adnr = adnr.Replace("{0}", "NO");
                        }
                        bhlist[i] = adnr;
                    }
                    else
                    {
                        if (selectvalue == "yes")
                        {
                            jg_yes += "'" + itemid + "',";
                        }
                        else
                        {
                            jg_no += "'" + itemid + "',";
                        }
                    }
                }
                else
                {
                    cc = (i + 1);
                    wwc = true;
                    break;
                }


            }
            if (wwc)
            {
                lits.Text = "还有未选择的行,第" + cc + "行";
            }
            else
            {
                if (jg_yes != "")
                {
                    jg_yes = jg_yes.Substring(0, jg_yes.Length - 1);
                }
                if (jg_no != "")
                {
                    jg_no = jg_no.Substring(0, jg_no.Length - 1);
                }
                int hyjd = int.Parse(access_sql.GetOneValue("select us from yn_user where id=5"));


                string yesnr = "";
                string nonr = "";
                int a = 0; int b = 0;
                if ((type == "2" && hyjd == 1) || (type == "7"))
                {

                    for (int i = 0; i < bhlist.Length; i++)
                    {
                        string[] q = bhlist[i].Split('镍');
                        string itemid = q[0];
                        string tg = q[1] == "YES" ? "宝涵" : "宝涵不能发";

                        string tg2 = q[1] == "YES" ? "goodslink" : "";
                        string jg = q[2];
                        string bz = q[3];

                        if (type == "2")
                        {
                            string[] str = new string[] { "kefadehaiyun", "baohanhaiyunjiage", "baohanhaiyunbeizhu" };
                            object[] obj = new object[] { tg, jg, bz };


                            a += access_sql.T_Update_ExecSql(str, obj, "YNBigData", "itemid='" + itemid + "'");
                        }
                        else
                        {
                            string[] str = new string[] { "kefadehaiyun", "shangjiamingzi", "baohanhaiyunjiage", "baohanhaiyunbeizhu" };
                            object[] obj = new object[] { tg, tg2, jg, bz };


                            a += access_sql.T_Update_ExecSql(str, obj, "YNBigData", "itemid='" + itemid + "'");
                        }
                    }

                }
                else
                {
                    string[] str = new string[1];
                    object[] ob_yes = new object[1];
                    object[] ob_no = new object[1];


                    if (type == "1" && hyjd == 0)
                    {

                        str[0] = "kefadehaiyun";
                        ob_yes[0] = "轻舟";
                        ob_no[0] = "轻舟不能发";

                    }
                    else if (type == "3" && hyjd == 2)
                    {


                        str[0] = "shangjiamingzi";
                        ob_yes[0] = "雅仓";
                        ob_no[0] = "雅仓不能入";

                    }
                    else if (type == "4" && hyjd == 3)
                    {


                        str[0] = "shangjiamingzi";
                        ob_yes[0] = "海仓";
                        ob_no[0] = "goodslink";
                    }



                    if (jg_yes != "")
                    {
                        a = access_sql.T_Update_ExecSql(str, ob_yes, "YNBigData", "itemid in(" + jg_yes + ")");


                    }
                    if (jg_no != "")
                    {
                        b = access_sql.T_Update_ExecSql(str, ob_no, "YNBigData", "itemid in(" + jg_no + ")");
                    }


                }

                lits.Text = "成功修改," + (a + b) + "条";

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
    }
}