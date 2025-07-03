using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Web;

namespace WebApplication11
{
    /// <summary>
    /// ca 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class ca : System.Web.Services.WebService
    {

        [WebMethod]
        public void HelloWorld(string LRID, string uid)
        {
            string new_Lwdpath = "";
            DataSet dsother = access_sql.GreatDs("SELECT  a.*,b.* FROM [otmb] as a left join otdata as b on a.oid=b.odmid where a.ouid=" + uid + " and b.odmid<>19 order by a.oid ");
            DataSet dsss = access_sql.GreatDs("select RW_V.*,RW.* from RW_V left join RW on RW_V.LMID=RW.RID where RW_V.LRID=" + LRID);
            if (access_sql.yzTable(dsss))
            {
                DataRow drrr = dsss.Tables[0].Rows[0];
                string LRJson = drrr["LRJson"].ToString();
                string LRState = drrr["LRState"].ToString();
                string rwname = drrr["RNAME"].ToString();
                JObject JJJJJ = JsonConvert.DeserializeObject<JObject>(LRJson);
                string 商家名字 = JJJJJ["商家名字"] != null ? JJJJJ["商家名字"].ToString() : "";
                string 店铺备注 = JJJJJ["店铺备注"] != null ? JJJJJ["店铺备注"].ToString() : "";
                string 店铺用户名 = JJJJJ["店铺用户名"] != null ? JJJJJ["店铺用户名"].ToString() : "";
                string 价格公式 = JJJJJ["价格公式"] != null ? JJJJJ["价格公式"].ToString() : "";
                string SPL = JJJJJ["Shipping Price List"] != null ? JJJJJ["Shipping Price List"].ToString() : "";
                string RT = JJJJJ["Returns Terms"] != null ? JJJJJ["Returns Terms"].ToString() : "";
                string CT = JJJJJ["Complaints Terms"] != null ? JJJJJ["Complaints Terms"].ToString() : "";
                string WO = JJJJJ["Warranties (optional)"] != null ? JJJJJ["Warranties (optional)"].ToString() : "";
                string PC = JJJJJ["Person responsible for product compliance"] != null ? JJJJJ["Person responsible for product compliance"].ToString() : "";
                string 固定列 = JJJJJ["固定列"] != null ? JJJJJ["固定列"].ToString() : "";
                string 价格区间 = JJJJJ["价格区间"] != null ? JJJJJ["价格区间"].ToString() : "";
                string 模版ID = JJJJJ["模版ID"] != null ? JJJJJ["模版ID"].ToString() : "";
                string 每份批量表产品数量 = JJJJJ["每份批量表产品数量"] != null ? JJJJJ["每份批量表产品数量"].ToString() : "";
                string 导出批量表数量 = JJJJJ["导出批量表数量"] != null ? JJJJJ["导出批量表数量"].ToString() : "";
                string 商家授权码 = JJJJJ["商家授权码"] != null ? JJJJJ["商家授权码"].ToString() : "";
                if (LRState == "0")
                {

                    DataSet dss = access_sql.GreatDs("select * from mb where did=" + 模版ID + "");
                    DataSet dslb = access_sql.GreatDs("select * from lb where ldid=" + 模版ID + "");

                    if (access_sql.yzTable(dss) && access_sql.yzTable(dslb))
                    {
                        DataTable dtlb = dslb.Tables[0];
                        //模板的各种参数
                        string path = dss.Tables[0].Rows[0]["dfile"].ToString();
                        string dtablename = dss.Tables[0].Rows[0]["dtablename"].ToString();
                        string drow = dss.Tables[0].Rows[0]["drow"].ToString();

                        if (path != "")
                        {
                            Dictionary<string, string> lisj = new Dictionary<string, string>();
                            if (SPL != "") { lisj.Add("Shipping Price List", SPL); }
                            if (RT != "") { lisj.Add("Returns Terms", RT); }
                            if (CT != "") { lisj.Add("Complaints Terms", CT); }
                            if (WO != "") { lisj.Add("Warranties (optional)", WO); }
                            if (PC != "") { lisj.Add("Person responsible for product compliance", PC); }

                            string[] gdcs = 固定列.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                            Dictionary<string, string> llgd = new Dictionary<string, string>();
                            foreach (var item in gdcs)
                            {
                                llgd.Add(item, item);
                            }
                            Dictionary<string, string> llothe = new Dictionary<string, string>();
                            if (商家名字 != "") { llothe.Add("商家名字", 商家名字); }
                            if (店铺备注 != "") { llothe.Add("店铺备注", 店铺备注); }
                            if (店铺用户名 != "") { llothe.Add("店铺用户名", 店铺用户名); }
                            if (价格公式 != "") { llothe.Add("价格公式", 价格公式); }
                            if (商家授权码 != "") { llothe.Add("商家授权码", 商家授权码); }
                            string old_Lwdpath = drrr["Lwdpath"].ToString();
                            int dqy = old_Lwdpath.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Length;
                            int xuyao = int.Parse(导出批量表数量) - dqy;
                            string Lwdpath = "";

                            for (int a = 0; a < xuyao; a++)
                            {
                                string sql = getsql(模版ID, 价格区间, int.Parse(每份批量表产品数量), uid);
                                DataSet dsout = access_sql.GreatDs(sql);

                                if (access_sql.yzTable(dsout))
                                {
                                    DataTable dttout = dsout.Tables[0];
                                    string ru = getcsv_(dttout, dtlb, dsother.Tables[0], 模版ID, a, path, dtablename, drow, lisj, llgd, llothe, rwname, uid);
                                    if (ru != "")
                                    {
                                        Lwdpath += ru + "|";
                                    }
                                    new_Lwdpath = old_Lwdpath + Lwdpath;
                                    access_sql.T_Update_ExecSql(new string[] { "Lwdpath" }, new object[] { new_Lwdpath }, "RW_V", "LRID=" + drrr["LRID"].ToString());
                                    string pids = "";
                                    for (int u = 0; u < dttout.Rows.Count; u++)
                                    {
                                        pids = pids + dttout.Rows[u]["pid"].ToString() + ",";
                                    }
                                    if (pids != "")
                                    {
                                        pids = pids.Substring(0, pids.Length - 1);

                                        //access_sql.T_Update_ExecSql(new string[] { "pgm", "gmdp", "gsq", "dtime" }, new object[] { 1, 商家名字 + "镍" + 店铺备注 + "镍" + 店铺用户名, 商家授权码, DateTime.Now.ToString("yyyyMMdd hh:mm:ss") }, "ALLproduct", "pid in (" + pids + ")");

                                    }
                                }

                            }
                            if (new_Lwdpath != "")
                            {
                                if (new_Lwdpath.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Length == int.Parse(导出批量表数量))
                                {
                                    access_sql.T_Update_ExecSql(new string[] { "LRState", "Lwdpath" }, new object[] { 1, new_Lwdpath }, "RW_V", "LRID=" + drrr["LRID"].ToString());
                                }
                            }
                        }
                    }

                }
            }
            Context.Response.Write(new_Lwdpath);

        }
        public string getsql(string limid, string txtjgqj, int topzs, string uid)
        {
            string ru = "";
            string minprice = "";
            string maxprice = "";
            if (txtjgqj.IndexOf("-") != -1)
            {
                minprice = txtjgqj.Split('-')[0];
                maxprice = txtjgqj.Split('-')[1];
            }
            //ru = "select top " + topzs + " ChanPinJiaGe,pid,pean,ZhuTuWangZhi,BiaoTi from ALLproduct where  (ChanPinJiaGe>=" + minprice + ") and (ChanPinJiaGe<=" + maxprice + ") and (puid=" + uid + ") and pgm=0 and pw=1 and pmbid=" + limid + " and pean is not null order by daochu,pid";
            ru = "select top " + topzs + " ChanPinJiaGe,pid,pean from ALLproduct where  (ChanPinJiaGe>=" + minprice + ") and (ChanPinJiaGe<=" + maxprice + ") and (puid=" + uid + ") and pgm=0 and pw=1 and pmbid=" + limid + " and pean is not null and isuse=0 order by daochu,pid";
            return ru;
        }
        public string getcsv_(DataTable dttempout, DataTable dtlb, DataTable dtothe, string did, int d, string lipath_, string dtablename_, string drow_, Dictionary<string, string> llsj, Dictionary<string, string> llgd, Dictionary<string, string> llothe, string rwname, string uid)
        {
            string ru = "";
            try
            {

                DataTable dttemp = new DataTable();
                DataTable dtproduct = new DataTable();

                string 商家名字 = llothe["商家名字"];
                string 店铺备注 = llothe["店铺备注"];
                string 店铺用户名 = llothe["店铺用户名"];
                string 价格公式 = llothe["价格公式"];
                string 商家授权码 = llothe["商家授权码"];


                dtproduct.Columns.Add("pid");
                dtproduct.Columns.Add("Product ID");
                dtproduct.Columns.Add("产品价格");
                //dtproduct.Columns.Add("标题");
                //dtproduct.Columns.Add("主图网址");
                //dtproduct.Columns.Add("详细参数");

                ;
                bool gl = false;
                //DataSet dsjyc = access_sql.GreatDs("select * from wj order by wid");
                for (int i = 0; i < dttempout.Rows.Count; i++)
                {


                    string ChanPinJiaGe = dttempout.Rows[i]["ChanPinJiaGe"].ToString() != "" ? dttempout.Rows[i]["ChanPinJiaGe"].ToString() : "0";
                    string qqq = 价格公式.Replace("原价", ChanPinJiaGe);
                    DataTable table = new DataTable();
                    ChanPinJiaGe = table.Compute(qqq, "").ToString();


                    if (float.Parse(ChanPinJiaGe) <= 0) { ChanPinJiaGe = "1"; }
                    string pid = dttempout.Rows[i]["pid"].ToString();
                    string pean = dttempout.Rows[i]["pean"].ToString();
                    //string bt = dttempout.Rows[i]["BiaoTi"].ToString();
                    //string zt = dttempout.Rows[i]["ZhuTuWangZhi"].ToString();               
                    //string ms = bt+" "+ rrr();
                    // zt = zt.Substring(0, zt.IndexOf('|'));

                    //dtproduct.Rows.Add(new object[] { pid, pean, float.Parse(ChanPinJiaGe).ToString("f2"), bt, zt, ms });
                    dtproduct.Rows.Add(new object[] { pid, pean, float.Parse(ChanPinJiaGe).ToString("f2") });

                }
                for (int i = 0; i < dtlb.Rows.Count; i++)
                {
                    string lname = dtlb.Rows[i]["lname"].ToString();
                    if (dttemp.Columns.Contains(lname))
                    {
                        dttemp.Columns.Add(lname + "|" + i);
                    }
                    else
                    {
                        dttemp.Columns.Add(lname);
                    }
                }

                foreach (DataRow drp in dtproduct.Rows)
                {

                    string[] headtemp = new string[dttemp.Columns.Count];
                    bool ky = true;
                    for (int i = 0; i < headtemp.Length; i++)
                    {


                        DataRow drlb = dtlb.Rows[i];
                        string lname = drlb["lname"].ToString();
                        string lsmt = drlb["lsmt"].ToString();
                        string lqt = drlb["lqt"].ToString().Replace("'", "''");
                        string lgd = drlb["lgd"].ToString();
                        if (llgd.ContainsKey(lname))
                        {
                            if (lsmt != "无")
                            {
                                if (dtproduct.Columns.Contains(lsmt))
                                {
                                    headtemp[i] = drp[lsmt].ToString();
                                }
                            }
                            else if (lqt != "无")
                            {
                                if (dtproduct.Columns.Contains(lqt))
                                {
                                    headtemp[i] = drp[lqt].ToString();
                                }
                                else
                                {
                                    DataRow[] drothe = dtothe.Select("oname='" + lqt + "'");
                                    if (drothe.Length > 0)
                                    {
                                        string otype = drothe[0]["otype"].ToString();
                                        string oid = drothe[0]["oid"].ToString();


                                        if (otype == "重复")
                                        {
                                            string odvalue = drothe[0]["odvalue"].ToString();
                                            headtemp[i] = odvalue;
                                        }
                                    }
                                }
                            }
                            else if (lgd != "无")
                            {
                                headtemp[i] = lgd;
                            }
                            else
                            {
                                headtemp[i] = "";
                            }
                        }
                        else
                        {
                            headtemp[i] = "";
                        }
                    }
                    if (ky)
                    {
                        dttemp.Rows.Add(headtemp);
                    }

                }
                foreach (KeyValuePair<string, string> item in llsj)
                {
                    if (dttemp.Columns.Contains(item.Key))
                    {
                        foreach (DataRow row in dttemp.Rows)
                        {
                            row[item.Key] = item.Value;
                        }
                    }
                }
                ru = works.CJCSV(dttemp, did, d, uid, int.Parse(drow_), lipath_, dtablename_, rwname, 店铺备注);
                if (ru.Length > 0)
                {

                }



            }
            catch
            {


            }


            return ru;
        }
    }


}

