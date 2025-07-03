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
    public partial class _out2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!access_sql.yzdl())
            {
                Response.Redirect("/login.aspx");
            }
            else
            {
                u = HttpContext.Current.Request.Cookies["u"].Value;
                p = HttpContext.Current.Request.Cookies["p"].Value;
                uid = HttpContext.Current.Request.Cookies["uid"].Value;
                //if (HttpContext.Current.Request.Cookies["eee"] == null)
                //{
                //   Response.Redirect("/other.aspx?r=pz.aspx");
                //}
            }
            if (!IsPostBack)
            {

                //bind();
                //bind2();
                if (Request.QueryString["cz"] != null)
                {
                    string lrid = Request.QueryString["LRID"];
                    string pids = access_sql.GetOneValue("select pids from rw_v where lrid=" + lrid + "");
                    access_sql.T_Update_ExecSql(new string[] { "LRState", "Lwdpath" }, new object[] { 0, "" }, "rw_v", "lrid=" + lrid);
                    if (pids.Length > 10)
                    {
                        access_sql.T_Update_ExecSql(new string[] { "pgm" }, new object[] { 0 }, "ALLproduct", "pid in (" + pids + ")");
                    }
                    Response.Redirect("out2.aspx");
                }
                else
                {
                    bindrw();
                    if (Request.QueryString["type"] != null)
                    {
                        if (Request.QueryString["type"] == "sczip")
                        {
                            Literal1.Text = "压缩包已经生成，请点击下载";
                        }
                        if (Request.QueryString["rid"] != null)
                        {
                            string zips = access_sql.GetOneValue("select rzip from rw where rid=" + Request.QueryString["rid"] + "");

                            Literal4.Text += "<a  href='" + zips + "' target='_blank'>ZIP下载地址</a>";


                        }
                    }
                }
            }
        }
        public string u = "";
        public string p = "";
        public string uid = "";
        public void bind()
        {
            DataSet ds = access_sql.GreatDs("select * from pz");
            if (access_sql.yzTable(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtcppf.Text = dr["cppf"].ToString();
                txtmspf.Text = dr["mspf"].ToString();
                txtgtpf.Text = dr["gtpf"].ToString();
                txtfhpf.Text = dr["fhpf"].ToString();
                txtkdnx.Text = dr["kdnx"].ToString();
                txtzxl.Text = dr["zxl"].ToString();
                txthl.Text = dr["hl"].ToString();
                txtjgqj.Text = dr["jgqj"].ToString();
            }


        }

        public void bindrw()
        {
            DataSet dsall = access_sql.GreatDs("select rw_v.*,rw.* from rw_v left join rw on rw_v.lmid=rw.RID where rw.ruid=" + uid + " and rw_v.luid=" + uid + " and RState=0");
            if (access_sql.yzTable(dsall))
            {
                Session["mydtout"] = dsall.Tables[0];
                Literal5.Text = dsall.Tables[0].Rows[0]["RID"].ToString();
                yy.Visible = true;
                DataTable dtyy = dsall.Tables[0];
                Session["xlsname"] = dtyy.Rows[0]["Rname"];
                liyy.Text = "<span style='color:blue'>任务名：" + dtyy.Rows[0]["Rname"] + ",任务时间：" + dtyy.Rows[0]["Rtime"] + ",任务数：" + dtyy.Rows.Count + "</span><table style='background: #e7e7e7;'>";
                liyy.Text += "<tr>{0}</tr>";
                List<string> lhead = new List<string>();
                for (int i = 0; i < dtyy.Rows.Count; i++)
                {
                    DataRow dr = dtyy.Rows[i];

                    JObject jb = JsonConvert.DeserializeObject<JObject>(dr["LRjson"].ToString());
                    liyy.Text += "<tr><td>" + (i + 1) + "</td>";
                    foreach (JProperty property in jb.Properties())
                    {
                        if (i == 0)
                        {
                            lhead.Add(property.Name);
                        }
                        liyy.Text += "<td>" + property.Value + "</td>";
                    }
                    liyy.Text += "<td class='wd'>" + dr["Lwdpath"].ToString() + "" + (dr["errorS"].ToString() == "" ? "" : "<span style='color:red'>" + dr["errorS"].ToString() + "<span>") + "</td>";
                    liyy.Text += "<td>" + (dr["LRstate"].ToString() == "0" ? "<span style='color:red'>未完成<span>" : "<span style='color:green'>完成<span>") + "<br><a onclick=\"JavaScript:return confirm('确定重置吗？');\" href='?cz=cz&lrid=" + dr["LRID"].ToString() + "'>重置</a></td>";
                    liyy.Text += "<td  class='lr' style='display:none'>" + dr["LRID"].ToString() + "</td>";
                    liyy.Text += "<td  class='zt' style='display:none'>" + dr["LRstate"].ToString() + "</td>";
                    liyy.Text += "</tr>";


                }

                liyy.Text += "</table>";
                string rep = "<td>序号</td>";
                for (int i = 0; i < lhead.Count; i++)
                {
                    rep += "<td>" + lhead[i] + "</td>";
                }
                rep += "<td>文档</td>";
                rep += "<td>状态</td>";
                rep += "<td style='display:none'>LRID</td>";
                rep += "<td style='display:none'>LRID</td>";
                liyy.Text = liyy.Text.Replace("{0}", rep);
            }
        }
        public void bind2()
        {
            if (Request.QueryString["did"] != null)
            {
                DataSet dsr = access_sql.GreatDs("select * from mb where  did=" + Request.QueryString["did"] + "");
                if (access_sql.yzTable(dsr))
                {
                    did = dsr.Tables[0].Rows[0]["did"].ToString();
                    limid.Text = did;

                    searchtxt = dsr.Tables[0].Rows[0]["dsearchtxt"].ToString();
                    name = dsr.Tables[0].Rows[0]["dname"].ToString();
                    liname.Text = name;
                    list.Text = searchtxt;
                    lipath.Text = dsr.Tables[0].Rows[0]["dfile"].ToString();
                    dtablename = dsr.Tables[0].Rows[0]["dtablename"].ToString();
                    drow = dsr.Tables[0].Rows[0]["drow"].ToString();
                    lits.Text = "模板ID：" + did + "，名称：" + name + "，搜索词：" + searchtxt;
                }
            }
        }
        public string did = "";
        public string searchtxt = "";
        public string name = "";
        public string dtablename = "";
        public string drow = "";
        protected void btnadddata_Click(object sender, EventArgs e)
        {
            if (txtname.Text.Trim() != "" || txtmid.Text.Trim() != "")
            {
                string where = "";
                if (txtname.Text.Trim() != "")
                {
                    where = "dname='" + txtname.Text.Trim().Replace("'", "''") + "'";
                }
                if (txtmid.Text.Trim() != "")
                {
                    where = "did=" + txtmid.Text.Trim() + "";
                }
                if (txtname.Text.Trim() != "" && txtmid.Text.Trim() != "")
                {
                    where = "dname='" + txtname.Text.Trim().Replace("'", "''") + "' and did=" + txtmid.Text.Trim() + "";
                }
                DataSet ds = access_sql.GreatDs("select * from mb where " + where);
                if (access_sql.yzTable(ds))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (dr["dstate"].ToString() != "-1" && dr["dstate"].ToString() != "0")
                    {
                        Response.Redirect("/out.aspx?did=" + dr["did"].ToString());
                    }
                    else
                    {
                        lits.Text = "该模板不能导出，未编辑好或者为不可销售";
                    }
                }
                else
                {
                    lits.Text = "没找到";
                }
            }
            else
            {
                lits.Text = "搜索不能为空";
            }
        }
        public string getsql(string limid, string txtjgqj, int topzs)
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
            ru = "select top " + topzs + " NewSalePrice,pid,pean from ALLproduct where  ((NewSalePrice>=" + minprice + ") and (NewSalePrice<=" + maxprice + ")  or ZongXiaoLiang>0) and (puid=" + uid + ") and pgm=0 and pw=1 and pmbid=" + limid + " and pean is not null and isuse=0 order by daochu,pid";
            return ru;
        }
        public string getsql(string limid, string txtcppf, string txtmspf, string txtgtpf, string txtfhpf, string txtkdnx, string txtzxl, string txtjgqj)
        {
            string ru = "";
            string minprice = "";
            string maxprice = "";
            if (txtjgqj.IndexOf("-") != -1)
            {
                minprice = txtjgqj.Split('-')[0];
                maxprice = txtjgqj.Split('-')[1];
            }
            if (txtcppf != "" && txtmspf != "" && txtgtpf != "" && txtfhpf != "" && txtkdnx != "" && txtzxl != "" && minprice != "" && maxprice != "")
            {



                string wheress = "";
                wheress += " and  ( pkid in (select kid from ysmbkw where mbid=" + limid + ")  )";
                ru = "select * from product where 1=1  " + wheress + " and (ChanPinJiaGe+COALESCE(YunFei,0)>=" + minprice + ") and (ChanPinJiaGe+COALESCE(YunFei,0)<=" + maxprice + ") and (COALESCE(KaiDianShiChang,0)>=" + txtkdnx + ") and (COALESCE(ChanPinPingFen,0)>=" + txtcppf + ") and (COALESCE(ChanPinMiaoShuPingFen,0)>=" + txtmspf + ") and (COALESCE(GouTongPingFen,0)>=" + txtgtpf + ") and (COALESCE(YunSongSuDuPingFen,0)>=" + txtfhpf + ") and (COALESCE(ZongXiaoLiang,0)>=" + txtzxl + ") and (puid=" + uid + ") order by daochu,pid";


            }
            return ru;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {


        }

        public bool testJY(DataSet ds, string[] rr)
        {
            bool ru = true;
            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string item = dt.Rows[i]["wname"].ToString();
                    if (item != "")
                    {
                        foreach (var r in rr)
                        {
                            if (r.ToLower().Contains(item.ToLower()))
                            {
                                ru = false;
                                break;
                            }
                        }
                        if (!ru) { break; }

                    }

                }
            }

            return ru;
        }
        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        public string rrr()
        {
            Random random = new Random();
            char[] chars = new char[10];

            for (int i = 0; i < 10; i++)
            {

                chars[i] = (char)(random.Next('a', 'z' + 1));
            }
            return new string(chars);
        }


        public string getcsv_(string classname, DataTable dttempout, DataTable dtlb, DataTable dtothe, string did, int d, string lipath_, string dtablename_, string drow_, Dictionary<string, string> llsj, Dictionary<string, string> llgd, Dictionary<string, string> llothe, string rwname)
        {
            string ru = "";
            try
            {
                List<string> head = works.gethead(int.Parse(drow_), lipath_, dtablename_);

                DataTable dttemp = new DataTable();
                for (int i = 0; i < head.Count; i++)
                {
                    dttemp.Columns.Add(i.ToString());
                }
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


                    string ChanPinJiaGe = dttempout.Rows[i]["NewSalePrice"].ToString() != "" ? dttempout.Rows[i]["NewSalePrice"].ToString() : "0";
                    string qqq = 价格公式.Replace("原价", ChanPinJiaGe);
                    DataTable table = new DataTable();
                    ChanPinJiaGe = table.Compute(qqq, "").ToString();
                    string jgg = "";
                    if (float.Parse(ChanPinJiaGe) < 1)
                    {
                        jgg = "1";
                    }
                    else
                    {
                        jgg = float.Parse(ChanPinJiaGe).ToString("f2");
                    }
                    if (float.Parse(ChanPinJiaGe) <= 0) { ChanPinJiaGe = "1"; }
                    string pid = dttempout.Rows[i]["pid"].ToString();
                    string pean = dttempout.Rows[i]["pean"].ToString();
                    //string bt = dttempout.Rows[i]["BiaoTi"].ToString();
                    //string zt = dttempout.Rows[i]["ZhuTuWangZhi"].ToString();
                    //string ms = bt+" "+ rrr();
                    // zt = zt.Substring(0, zt.IndexOf('|'));

                    //dtproduct.Rows.Add(new object[] { pid, pean, float.Parse(ChanPinJiaGe).ToString("f2"), bt, zt, ms });
                    dtproduct.Rows.Add(new object[] { pid, pean, jgg });

                }


                foreach (DataRow drp in dtproduct.Rows)
                {
                    try
                    {


                        string[] headtemp = new string[dttemp.Columns.Count];
                        bool ky = true;
                        for (int i = 0; i < headtemp.Length; i++)
                        {

                            DataRow[] rrs = dtlb.Select("lname='" + head[i].Replace("'", "''") + "'");
                            if (rrs.Length > 0)
                            {
                                DataRow drlb = rrs[0];
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
                    catch (Exception)
                    {

                        throw;
                    }

                }
                foreach (KeyValuePair<string, string> item in llsj)
                {

                    if (head.Contains(item.Key))
                    {

                        foreach (DataRow row in dttemp.Rows)
                        {
                            row[head.IndexOf(item.Key)] = item.Value;
                        }
                    }
                }
                if (dttemp.Rows[0][head.IndexOf("Main Category")].ToString() == classname)
                {

                    ru = works.CJCSV(dttemp, did, d, uid, int.Parse(drow_), lipath_, dtablename_, rwname, 店铺备注);
                }
                else
                {
                    ru = "格式出错";
                }
                if (ru.Length > 0)
                {

                }



            }
            catch
            {


            }


            return ru;
        }
        protected void Button31_Click(object sender, EventArgs e)
        {
            if (yy.Visible == false)
            {
                if (fup1.FileName != "")
                {
                    string ext = System.IO.Path.GetExtension(fup1.FileName);
                    string xlsname = Path.GetFileNameWithoutExtension(fup1.PostedFile.FileName);
                    Session["xlsname"] = xlsname;
                    if (ext == ".xlsx")
                    {
                        if (!Directory.Exists(Server.MapPath("/document" + uid + "/")))
                        {
                            Directory.CreateDirectory(Server.MapPath("/document" + uid + "/"));
                        }
                        string path = Server.MapPath("/document" + uid + "/") + fup1.FileName;

                        this.fup1.SaveAs(path);

                        DataTable dtout = works.ReadExcelData_(path);
                        if (dtout != null && dtout.Rows.Count > 0)
                        {



                            string rid = access_sql.T_Insert_ExecSql_top(new string[] { "RName", "RTime", "RUID" }, new object[] { xlsname, DateTime.Now.ToString("yyyyMMdd HH:mm:ss"), uid }, "RW").ToString();




                            for (int i = 0; i < dtout.Rows.Count; i++)
                            {
                                DataRow drrr = dtout.Rows[i];
                                if (drrr[0].ToString() != "")
                                {
                                    string strvalue = "{";
                                    bool jx = true;
                                    for (int c = 0; c < dtout.Columns.Count; c++)
                                    {

                                        if (dtout.Columns[c].ColumnName.IndexOf("Column") == -1)
                                        {
                                            if (dtout.Columns[c].ColumnName == "模版ID")
                                            {
                                                if (drrr[dtout.Columns[c].ColumnName].ToString() == "")
                                                {
                                                    jx = false;
                                                }
                                            }
                                            if (dtout.Columns[c].ColumnName == "导出批量表数量")
                                            {
                                                if (drrr[dtout.Columns[c].ColumnName].ToString() == "0")
                                                {
                                                    jx = false;
                                                }
                                            }
                                            strvalue += "\"" + dtout.Columns[c].ColumnName.Replace("\"", "\\" + "\"") + "\":\"" + drrr[dtout.Columns[c].ColumnName].ToString().Replace("\"", "\\" + "\"") + "\",";
                                        }
                                    }
                                    if (strvalue.Length > 1)
                                    {
                                        strvalue = strvalue.Substring(0, strvalue.Length - 1);
                                    }
                                    strvalue += "}";
                                    if (jx)
                                    {
                                        access_sql.T_Insert_ExecSql(new string[] { "LRJson", "LMID", "LUID" }, new object[] { strvalue, rid, uid }, "RW_V");
                                    }
                                }

                            }
                            bindrw();
                        }
                    }
                    else
                    {
                        lits.Text = "文件格式不对";

                    }

                }
                else
                {
                    lits.Text = "请选择文件";

                }
            }
            else
            {
                lits.Text = "有未完成的任务，清先完成或者删除";
            }
        }
        public bool CreateZip(List<string> allfile, string destinationPath)
        {
            bool ru = true;
            try
            {

                if (File.Exists(destinationPath)) { File.Delete(destinationPath); }
                using (FileStream zipToOpen = new FileStream(destinationPath, FileMode.Create))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                    {
                        for (int i = 0; i < allfile.Count; i++)
                        {
                            string filePath = allfile[i];


                            filePath = Server.MapPath(filePath);
                            string fileName = Path.GetFileName(filePath);
                            ZipArchiveEntry readMeEntry = archive.CreateEntry(fileName);
                            using (System.IO.Stream stream = readMeEntry.Open())
                            {
                                byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                                stream.Write(bytes, 0, bytes.Length);
                            }

                            ZipArchiveEntry entry = archive.CreateEntry(filePath);
                        }
                    }
                }
            }
            catch
            {

                return false;
            }
            return ru;
        }

        protected void Button3_Click(object sender, EventArgs e)
        {



            if (Session["mydtout"] != null)
            {
                DataTable dtout = Session["mydtout"] as DataTable;
                string rwname = Session["xlsname"].ToString();
                Literal3.Text = "开始任务";
                string outtxt = "";
                if (dtout.Rows.Count > 0)
                {


                    DataSet dsother = access_sql.GreatDs("SELECT  a.*,b.* FROM [otmb] as a left join otdata as b on a.oid=b.odmid where a.ouid=" + uid + " and b.odmid<>19 order by a.oid ");
                    if (access_sql.yzTable(dsother))
                    {
                        for (int i = 0; i < dtout.Rows.Count; i++)
                        {
                            DataRow drrr = dtout.Rows[i];
                            string LRJson = drrr["LRJson"].ToString();
                            string LRState = drrr["LRState"].ToString();
                            string LRID = drrr["LRID"].ToString();
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
                                    string mc = dtlb.Select("lname='Main Category'")[0]["lgd"].ToString();
                                    //模板的各种参数
                                    string path = dss.Tables[0].Rows[0]["dfile"].ToString();
                                    string dtablename = dss.Tables[0].Rows[0]["dtablename"].ToString();
                                    string drow = dss.Tables[0].Rows[0]["drow"].ToString();

                                    if (path != "" && File.Exists(path))
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
                                        string new_Lwdpath = "";
                                        for (int a = 0; a < xuyao; a++)
                                        {
                                            string sql = getsql(模版ID, 价格区间, int.Parse(每份批量表产品数量));
                                            DataSet dsout = access_sql.GreatDs(sql);

                                            if (access_sql.yzTable(dsout))
                                            {
                                                DataTable dttout = dsout.Tables[0];
                                                string ru = getcsv_(mc, dttout, dtlb, dsother.Tables[0], 模版ID, a, path, dtablename, drow, lisj, llgd, llothe, rwname);
                                                if (ru != "" && !ru.Contains("格式出错"))
                                                {
                                                    Lwdpath += ru + "|";
                                                }
                                                else
                                                {
                                                    access_sql.T_Update_ExecSql(new string[] { "errorS" }, new object[] { "文档格式错" }, "RW_V", "LRID=" + LRID);
                                                }
                                                new_Lwdpath = old_Lwdpath + Lwdpath;
                                                access_sql.T_Update_ExecSql(new string[] { "Lwdpath" }, new object[] { new_Lwdpath }, "RW_V", "LRID=" + LRID);
                                                string pids = "";
                                                if (!ru.Contains("格式出错"))
                                                {
                                                    for (int u = 0; u < dttout.Rows.Count; u++)
                                                    {
                                                        pids = pids + dttout.Rows[u]["pid"].ToString() + ",";
                                                    }
                                                    if (pids != "")
                                                    {
                                                        pids = pids.Substring(0, pids.Length - 1);
                                                        access_sql.T_Update_ExecSql(new string[] { "pids" }, new object[] { pids }, "RW_V", "LRID=" + LRID);
                                                        access_sql.T_Update_ExecSql(new string[] { "pgm", "gmdp", "gsq", "dtime" }, new object[] { 1, 商家名字 + "镍" + 店铺备注 + "镍" + 店铺用户名, 商家授权码, DateTime.Now.ToString("yyyyMMdd hh:mm:ss") }, "ALLproduct", "pid in (" + pids + ")");

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                access_sql.T_Update_ExecSql(new string[] { "errorS" }, new object[] { "未找到产品" }, "RW_V", "LRID=" + LRID);
                                            }

                                        }

                                        if (new_Lwdpath != "")
                                        {
                                            if (new_Lwdpath.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Length == int.Parse(导出批量表数量))
                                            {
                                                access_sql.T_Update_ExecSql(new string[] { "LRState", "Lwdpath", "errorS" }, new object[] { 1, new_Lwdpath, "" }, "RW_V", "LRID=" + drrr["LRID"].ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        access_sql.T_Update_ExecSql(new string[] { "errorS" }, new object[] { "模板文件不存在" }, "RW_V", "LRID=" + drrr["LRID"].ToString());
                                    }
                                }
                                else
                                {
                                    access_sql.T_Update_ExecSql(new string[] { "errorS" }, new object[] { "模板问题" }, "RW_V", "LRID=" + drrr["LRID"].ToString());
                                }

                            }

                        }
                    }
                    else
                    {
                        Response.Write("1");
                    }
                    Response.Redirect("out2.aspx?rid=" + Literal5.Text);

                }
            }
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            access_sql.DoSql("delete from RW  where rid=" + Literal5.Text);
            access_sql.DoSql("delete from RW_V where LMID=" + Literal5.Text + "");

            Response.Redirect("out2.aspx");
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            access_sql.DoSql("update RW set rstate=1 where rid=" + Literal5.Text);
            Response.Redirect("out2.aspx");
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            Literal3.Text = "打包下载";

            if (!Directory.Exists(Server.MapPath("/zip" + uid + "/")))
            {
                Directory.CreateDirectory(Server.MapPath("/zip" + uid + "/"));
            }


            string dowpath = "/zip" + uid + "/" + Session["xlsname"] + "_" + DateTime.Now.ToString("HHmmss") + ".zip";
            string zippath = Server.MapPath(dowpath);
            List<string> allfile = new List<string>();
            DataSet dssss = access_sql.GreatDs("select Lwdpath from RW_V where Lmid=" + Literal5.Text + "");
            if (access_sql.yzTable(dssss))
            {
                for (int i = 0; i < dssss.Tables[0].Rows.Count; i++)
                {
                    string dtp = dssss.Tables[0].Rows[i]["Lwdpath"].ToString();
                    string[] dsssssssssss = dtp.Split('|');
                    for (int q = 0; q < dsssssssssss.Length; q++)
                    {
                        if (dsssssssssss[q] != "")
                        {
                            allfile.AddRange(new List<string> { dsssssssssss[q] });
                        }
                    }
                }
                if (CreateZip(allfile, zippath))
                {
                    access_sql.DoSql("update RW set RZIP='" + dowpath + "' where rid=" + Literal5.Text);
                    Response.Write("<script>location.href='/out2.aspx?type=sczip&rid=" + Literal5.Text + "';</script>");
                }
            }
        }
    }
}