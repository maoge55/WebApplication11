using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 翻译成泰语 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!access_sql.yzdlcg())
            {
                Response.Redirect("/cg/clogin.aspx");
            }
            else
            {
                Session.Timeout = 240;
                u = HttpContext.Current.Request.Cookies["cu"].Value;
                p = HttpContext.Current.Request.Cookies["cp"].Value;
                uid = HttpContext.Current.Request.Cookies["cuid"].Value;
                if (uid != "6" && uid != "9")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
                if (Request.QueryString["type"] != null && Request.QueryString["type"] != "")
                {
                    if (Request.QueryString["type"] == "1")
                    {
                        name = "普货翻译成泰语";
                        datatype = "1";
                    }
                    else if (Request.QueryString["type"] == "2")
                    {
                        name = "POD翻译成泰语";
                        datatype = "2";
                    }
                }

            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";
        public string name = "";
        public string datatype = "";

        protected void btnsearch_Click(object sender, EventArgs e)
        {

            lits.Text = "";
            string where = "";
            if (datatype == "2")
            {
                where = " and ispodcp=1 ";
            }
            DataSet dsall = access_sql.GreatDs("select itemid from ProShopeePh where datatype=" + datatype + where + " and (ThTitle is null or ThTitle ='') group by itemid ");
            string count = "0";
            if (access_sql.yzTable(dsall))
            {
                count = dsall.Tables[0].Rows.Count.ToString();
                DataSet ds = access_sql.GreatDs("select itemid+'|'+pname as jg from ProShopeePh where itemid in(select top 100 itemid from ProShopeePh where datatype=" + datatype + where + " and (ThTitle is null or ThTitle ='') group by itemid) group by itemid,pname");
                if (access_sql.yzTable(ds))
                {
                    DataTable dt = ds.Tables[0];
                    string outstr = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string paname = dt.Rows[i][0].ToString();
                        if ((outstr + paname + "\r\n").Length < 3990)
                        {
                            outstr += paname + "\r\n";
                        }
                        else
                        {
                            break;
                        }
                    }
                    outstr = outstr.Trim();
                    if (outstr != "")
                    {
                        txtoutfy.Text = outstr;
                    }
                }
                lits.Text = "共有" + count + "条数据，需要翻译";

            }
            else
            {
                lits.Text = name + "没有数据";
                Response.Write("<script>alert('" + lits.Text + "');</script>");
            }
        }
        public string getcode()
        {
            string ru = "";
            do
            {


                byte[] r = new byte[8];
                Random rand = new Random((int)(DateTime.Now.Ticks % 1000000));

                int ran = 0;
                for (int i = 0; i < 8; i++)

                    do
                    {

                        ran = rand.Next(48, 122);
                        r[i] = Convert.ToByte(ran);
                    } while ((ran >= 58 && ran <= 64) || (ran >= 91 && ran <= 96));

                ru = Encoding.ASCII.GetString(r);
                if (access_sql.GetOneValue("select count(*) from RandomCodes where RandomCode='" + ru + "'") != "0")
                {
                    ru = "";
                }
            } while (ru == "");
            return ru;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (txtinfy.Text.Trim() != "")
            {
                string fystr = txtinfy.Text.Trim().Replace("\r\n", "镍");
                string[] fz = fystr.Split(new char[] { '镍' }, StringSplitOptions.RemoveEmptyEntries);
                if (fz.Length > 0)
                {
                    int cgcp = 0;
                    int cgit = 0;
                    foreach (string item in fz)
                    {
                        if (item != "")
                        {
                            string[] btfz = item.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                            string code = getcode();
                            if (btfz.Length == 2)
                            {
                                string itemid = btfz[0].Trim();
                                string ThTitle = btfz[1].Trim().Replace("'", "''");
                                if (itemid != "" && ThTitle != "")
                                {
                                    int cc = access_sql.T_Update_ExecSql(new string[] { "ThTitle", "ThTitleRcode" }, new object[] { ThTitle, ThTitle + " |" + code + "|" }, "ProShopeePh", "itemid='" + itemid + "'");
                                    if (cc > 0)
                                    {
                                        access_sql.T_Insert_ExecSql(new string[] { "itemid", "RandomCode" }, new object[] { itemid, code }, "RandomCodes");
                                        cgcp = cgcp + cc;
                                        cgit++;
                                    }
                                }

                            }
                            else
                            {
                                string itemid = btfz[0].Trim();
                                string ThTitle = "";
                                for (int i = 1; i < btfz.Length; i++)
                                {
                                    ThTitle += btfz[i] + "|";
                                }
                                ThTitle = ThTitle.Replace("'", "''");
                                if (ThTitle != "")
                                {
                                    ThTitle = ThTitle.Substring(0, ThTitle.Length - 1);
                                }
                                if (itemid != "" && ThTitle != "")
                                {
                                    int cc = access_sql.T_Update_ExecSql(new string[] { "ThTitle", "ThTitleRcode" }, new object[] { ThTitle, ThTitle + " |" + code + "|" }, "ProShopeePh", "itemid='" + itemid + "'");
                                    if (cc > 0)
                                    {
                                        access_sql.T_Insert_ExecSql(new string[] { "itemid", "RandomCode" }, new object[] { itemid, code }, "RandomCodes");
                                        cgcp = cgcp + cc;
                                        cgit++;
                                    }
                                }
                            }
                        }
                    }
                    lits.Text = "成功更新itemid" + cgit + "个,sku" + cgcp + "个";
                    Response.Write("<script>alert('" + lits.Text + "');</script>");
                    txtinfy.Text = "";
                    txtoutfy.Text = "";
                }
            }
            else
            {
                lits.Text = "请输入翻译后的标题";
                Response.Write("<script>alert('" + lits.Text + "');</script>");
            }
        }
    }

}