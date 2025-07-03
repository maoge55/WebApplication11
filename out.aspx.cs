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
    public partial class _out : System.Web.UI.Page
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

                bind();
                bind2();
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
        public DataTable dttempout = null;
        protected void Button1_Click(object sender, EventArgs e)
        {

            string did_ = limid.Text.Trim();
            string cppf = txtcppf.Text.Trim();
            string mspf = txtmspf.Text.Trim();
            string gtpf = txtgtpf.Text.Trim();
            string fhpf = txtfhpf.Text.Trim();
            string kdnx = txtkdnx.Text.Trim();
            string zxl = txtzxl.Text.Trim();
            string hl = txthl.Text.Trim();
            string jgqj = txtjgqj.Text.Trim();
            string minprice = "";
            string maxprice = "";
            if (jgqj.IndexOf("-") != -1)
            {
                minprice = jgqj.Split('-')[0];
                maxprice = jgqj.Split('-')[1];
            }
            if (cppf != "" && mspf != "" && gtpf != "" && fhpf != "" && kdnx != "" && zxl != "" && minprice != "" && maxprice != "")
            {

                string ssssql = getsql(limid.Text, txtcppf.Text, txtmspf.Text, txtgtpf.Text, txtfhpf.Text, txtkdnx.Text, txtzxl.Text, txtjgqj.Text);

                DataSet dsss = access_sql.GreatDs(ssssql);
                if (access_sql.yzTable(dsss))
                {
                    dttempout = dsss.Tables[0];
                    int skucount = 0;
                    for (int i = 0; i < dttempout.Rows.Count; i++)
                    {
                        JArray objskuprice = JsonConvert.DeserializeObject<JArray>(dttempout.Rows[i]["skuprice"].ToString());
                        skucount = skucount + objskuprice.Count;
                    }
                    lits.Text = " 产品数量：" + dttempout.Rows.Count.ToString() + ",sku数量：" + skucount;

                }
                else
                {
                    lits.Text = "没找到数据";
                }

            }
            else
            {
                lits.Text = "不能为空";
            }
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
            if (txtjgbs.Text != "" && txtpdcount.Text != "" && txtwjcount.Text != "")
            {
                bind2();
                string sql = getsql(limid.Text, txtcppf.Text, txtmspf.Text, txtgtpf.Text, txtfhpf.Text, txtkdnx.Text, txtzxl.Text, txtjgqj.Text);

                if (!File.Exists(lipath.Text))
                {
                    lits.Text = "模板文件不存在";
                }
                else
                {
                    List<string> ru = getcsvvvv(sql, limid.Text, txthl.Text, txtjgbs.Text, txtpdcount.Text, txtwjcount.Text, lipath.Text, dtablename, drow, false, null);
                    for (int i = 0; i < ru.Count; i++)
                    {
                        Response.Write("<script>window.open('" + ru[i] + "', '_blank');</script>");

                    }
                }
            }
            else
            {
                lits.Text = "导出条件不能为空";
            }
        }
        public List<string> getcsvvvv(string sql, string did, string txthl, string txtjgbs, string txtpdcount, string txtwjcount, string lipath_, string dtablename_, string drow_, bool gd, Dictionary<string, string> gdvalue)
        {
            List<string> ru = new List<string>();

            int pdc = int.Parse(txtpdcount);
            int wjs = int.Parse(txtwjcount);
            int zs = pdc * wjs;

            DataSet dsss = access_sql.GreatDs(sql);

            if (access_sql.yzTable(dsss))
            {
                dttempout = dsss.Tables[0];
            }
            if (dttempout != null)
            {
                DataSet dslb = access_sql.GreatDs("select * from lb where ldid =" + did + " order by lid");
                DataSet dsothe = access_sql.GreatDs("SELECT  a.*,b.* FROM [otmb] as a left join otdata as b on a.oid=b.odmid where a.ouid=" + uid + " order by a.oid ");
                if (access_sql.yzTable(dslb) && access_sql.yzTable(dsothe))
                {
                    DataTable dtlb = dslb.Tables[0];
                    DataTable dtothe = dsothe.Tables[0];
                    DataTable dttemp = new DataTable();


                    DataTable dtproduct = new DataTable();
                    dtproduct.Columns.Add("标题");
                    dtproduct.Columns.Add("产品价格");
                    dtproduct.Columns.Add("详细参数");
                    dtproduct.Columns.Add("主图网址");
                    dtproduct.Columns.Add("SKUID");
                    string pids = "";
                    bool gl = false;
                    DataSet dsjyc = access_sql.GreatDs("select * from wj order by wid");
                    for (int i = 0; i < dttempout.Rows.Count; i++)
                    {
                        string skuprice = dttempout.Rows[i]["skuprice"].ToString();
                        string sku = dttempout.Rows[i]["sku"].ToString();

                        string 标题 = (access_sql.jqbt(dttempout.Rows[i]["BiaoTi"].ToString()).Trim());
                        string ChanPinJiaGe = dttempout.Rows[i]["ChanPinJiaGe"].ToString() != "" ? dttempout.Rows[i]["ChanPinJiaGe"].ToString() : "0";
                        string YunFei = dttempout.Rows[i]["YunFei"].ToString() != "" ? dttempout.Rows[i]["YunFei"].ToString() : "0";

                        string 详细参数 = works.zystring(dttempout.Rows[i]["XiangXiCanShu"].ToString()).Replace("|", "\n");
                        string 主图网址 = dttempout.Rows[i]["ZhuTuWangZhi"].ToString();
                        string ChanPinID = dttempout.Rows[i]["ChanPinID"].ToString();

                        string SKUID = "";

                        if (testJY(dsjyc, new string[] { 标题, 详细参数 }))
                        {
                            if (skuprice != "" && sku != "" && sku.Contains("skuPropertyImagePath"))
                            {
                                JArray objskuprice = JsonConvert.DeserializeObject<JArray>(skuprice);
                                JArray objsku = JsonConvert.DeserializeObject<JArray>(sku);
                                if ((objskuprice != null && objskuprice.Count > 0) && (objsku != null && objsku.Count > 0))
                                {
                                    foreach (var item in objskuprice)   //循环生成多个产品
                                    {
                                        if (item["skuId"] != null && item["skuAttr"] != null)
                                        {
                                            SKUID = item["skuId"].ToString();      //skuid
                                            string skuAttr = item["skuAttr"].ToString();   //sku属性
                                            string skuVal = item["skuVal"].ToString();
                                            JObject skuVals = JsonConvert.DeserializeObject<JObject>(skuVal);
                                            string availQuantity = skuVals["availQuantity"].ToString();

                                            if (availQuantity != "0" && skuVals["skuAmount"] != null)
                                            {
                                                string skuimg = "";   //sku主图
                                                string[] attrsz = skuAttr.Split(';');    //拆分多个属性
                                                string 产品价格 = "";
                                                JObject prices = JsonConvert.DeserializeObject<JObject>(skuVals["skuAmount"].ToString());
                                                ChanPinJiaGe = prices["value"].ToString();
                                                foreach (string att in attrsz)         //循环每个属性 比如 颜色：黑色  尺码：40   找sku主图
                                                {
                                                    if (att.Contains(":"))
                                                    {
                                                        string[] att____ = att.Split(':');
                                                        try
                                                        {


                                                            if (att____.Length > 1)
                                                            {
                                                                string aid = att____[0];   //属性id
                                                                string aname = att____[1].Substring(0, att____[1].IndexOf("#")); //属性id对应的值,用来精准匹配图片
                                                                foreach (var item_sku in objsku)
                                                                {
                                                                    if (item_sku["skuPropertyId"] != null)
                                                                    {
                                                                        if (item_sku["skuPropertyId"].ToString() == aid)   //找到属性id对应的表
                                                                        {
                                                                            foreach (var skuvalue in item_sku["skuPropertyValues"])   //循环属性表，查找图片
                                                                            {
                                                                                if (skuvalue["propertyValueId"] != null)
                                                                                {
                                                                                    if (skuvalue["propertyValueId"].ToString() == aname)
                                                                                    {
                                                                                        if (skuvalue["skuPropertyImagePath"] != null)
                                                                                        {
                                                                                            skuimg = skuvalue["skuPropertyImagePath"].ToString();
                                                                                            产品价格 = ((float.Parse(ChanPinJiaGe) + float.Parse(YunFei)) * float.Parse(txthl) * float.Parse(txtjgbs)).ToString();
                                                                                            产品价格 = Math.Round(double.Parse(产品价格), 0).ToString();
                                                                                            break;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }

                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        catch
                                                        {


                                                        }
                                                    }
                                                    if (skuimg != "")
                                                    {
                                                        主图网址 = skuimg + "|" + 主图网址;

                                                        //去除多余图片保留10
                                                        string[] iiiimg = 主图网址.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                                        if (iiiimg.Length > 10)
                                                        {
                                                            for (int ii = 0; ii < 10; ii++)
                                                            {
                                                                主图网址 = iiiimg[ii].Trim() + "|";
                                                            }
                                                            主图网址 = 主图网址.Substring(0, 主图网址.Length - 1);
                                                        }


                                                        dtproduct.Rows.Add(new object[] { 标题, 产品价格, 详细参数, 主图网址, "A" + SKUID });
                                                        if (dtproduct.Rows.Count >= zs)
                                                        {
                                                            gl = true;
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                            if (gl)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            pids = pids + dttempout.Rows[i]["pid"].ToString() + ",";
                            if (gl) { break; }
                        }
                    }

                    if (pids != "")
                    {
                        pids = pids.Substring(0, pids.Length - 1);
                        access_sql.DoSql("update product set daochu=daochu+1 where pid in(" + pids + ")");
                    }

                    if (dtproduct.Rows.Count > 0)
                    {
                        DataTable dttempproduct = new DataTable();

                        if (zs > dtproduct.Rows.Count)
                        {
                            int fs = zs / dtproduct.Rows.Count;
                            int ys = zs - (fs * dtproduct.Rows.Count);
                            if (ys > 0) { fs = fs + 1; }

                            for (int f = 0; f < fs; f++)
                            {
                                dttempproduct.Merge(dtproduct);
                            }

                        }
                        else
                        {
                            dttempproduct.Merge(dtproduct);

                        }



                        string[] head_TTTT = new string[dtlb.Rows.Count];
                        for (int i = 0; i < dtlb.Rows.Count; i++)
                        {
                            string lname = dtlb.Rows[i]["lname"].ToString();
                            head_TTTT[i] = lname;
                            if (dttemp.Columns.Contains(lname))
                            {
                                dttemp.Columns.Add(lname + "|" + i);
                            }
                            else
                            {
                                dttemp.Columns.Add(lname);
                            }
                        }
                        DataTable dtProductID = new DataTable();
                        if (dtothe.Select("otype='一次性'").Length > 0)
                        {
                            DataSet dsProductID = access_sql.GreatDs("select top " + zs + " * from otdata where odmid=" + dtothe.Select("otype='一次性'")[0]["oid"] + " and odstate=0 order by odid");
                            if (access_sql.yzTable(dsProductID))
                            {
                                dtProductID = dsProductID.Tables[0];
                                access_sql.DoSql("update otdata set odstate=1 where odid>=" + dtProductID.Rows[0]["odid"].ToString() + " and odid<=" + dtProductID.Rows[dtProductID.Rows.Count - 1]["odid"].ToString() + "");
                            }

                        }
                        if (dtProductID.Rows.Count >= zs)
                        {
                            int q = 0;
                            foreach (DataRow drp in dttempproduct.Rows)
                            {
                                string[] headtemp = new string[dttemp.Columns.Count];
                                bool ky = true;
                                for (int i = 0; i < headtemp.Length; i++)
                                {


                                    DataRow drlb = dtlb.Rows[i];
                                    string lsmt = drlb["lsmt"].ToString();
                                    string lqt = drlb["lqt"].ToString().Replace("'", "''");
                                    string lgd = drlb["lgd"].ToString();
                                    if (lsmt != "无")
                                    {
                                        if (dttempproduct.Columns.Contains(lsmt))
                                        {
                                            headtemp[i] = drp[lsmt].ToString();
                                        }
                                    }
                                    else if (lqt != "无")
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
                                            else
                                            {

                                                if (dtProductID.Rows.Count >= q)
                                                {
                                                    headtemp[i] = dtProductID.Rows[q]["odvalue"].ToString();
                                                }
                                                else
                                                {
                                                    ky = false;
                                                    break;
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
                                if (ky)
                                {
                                    dttemp.Rows.Add(headtemp);
                                    if (dttemp.Rows.Count == zs)
                                    {
                                        break;
                                    }
                                }
                                q++;
                            }

                            if (gd && gdvalue != null)
                            {
                                foreach (KeyValuePair<string, string> item in gdvalue)
                                {
                                    if (dttemp.Columns.Contains(item.Key))
                                    {
                                        foreach (DataRow row in dttemp.Rows)
                                        {
                                            row[item.Key] = item.Value;
                                        }
                                    }
                                }
                            }
                            string[] dpath = works.CJCSV(head_TTTT, dttemp, pdc, wjs, did, uid, int.Parse(drow_), lipath_, dtablename_);

                            GC.Collect();
                            foreach (string item in dpath)
                            {
                                if (item != "")
                                {
                                    ru.Add(item);
                                    ru.Add(item.Replace(".csv", ".xls"));
                                }
                            }


                        }
                        else
                        {
                            lits.Text = "Ean码不够，请补充";
                        }
                    }
                    else
                    {
                        lits.Text = "没有数据！！";
                    }

                }
            }



            return ru;
        }

        protected void Button31_Click(object sender, EventArgs e)
        {
            if (fup1.FileName != "")
            {
                string ext = System.IO.Path.GetExtension(fup1.FileName);
                Session["xlsname"] = Path.GetFileNameWithoutExtension(fup1.PostedFile.FileName);
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
                        Session["mydtout"] = dtout;
                        Literal2.Text = "需要下载的数据有" + (dtout.Rows.Count - 1) + "行，模板ID为：";

                        foreach (DataRow item in dtout.Rows)
                        {
                            Literal2.Text += item["模版ID"].ToString() + ",";
                        }
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
        public bool CreateZip(List<string> allfile, string destinationPath)
        {
            bool ru = true;
            try
            {


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
                Literal3.Text = "开始任务";

                if (dtout.Rows.Count > 0)
                {

                    List<string> allfile = new List<string>();
                    for (int i = 0; i < dtout.Rows.Count; i++)
                    {
                        DataRow drrr = dtout.Rows[i];


                        string limid = drrr["模版ID"].ToString();
                        string txtcppf = drrr["产品评分"].ToString();
                        string txtmspf = drrr["描述评分"].ToString();
                        string txtgtpf = drrr["沟通评分"].ToString();
                        string txtfhpf = drrr["发货评分"].ToString();
                        string txtkdnx = drrr["开店年限"].ToString();
                        string txtzxl = drrr["历史销量"].ToString();
                        string txthl = drrr["汇率参数"].ToString();
                        string txtjgqj = drrr["价格区间"].ToString();
                        string txtjgbs = drrr["新价格为原价的倍数"].ToString();
                        string txtpdcount = drrr["每份批量表产品数量"].ToString();
                        string txtwjcount = drrr["导出批量表数量"].ToString();
                        Literal3.Text = "当前任务" + limid + "，" + txtwjcount + "份，每份" + txtpdcount + "个产品";
                        DataSet dss = access_sql.GreatDs("select * from mb where did=" + limid + "");
                        DataSet dslb = access_sql.GreatDs("select lname from lb where ldid=" + limid + "");
                        if (access_sql.yzTable(dss) && access_sql.yzTable(dslb))
                        {
                            DataTable dtlb = dslb.Tables[0];
                            string path = dss.Tables[0].Rows[0]["dfile"].ToString();
                            string dtablename = dss.Tables[0].Rows[0]["dtablename"].ToString();
                            string drow = dss.Tables[0].Rows[0]["drow"].ToString();
                            if (path != "")
                            {
                                Dictionary<string, string> lll = new Dictionary<string, string>();
                                for (int t = 0; t < dtout.Columns.Count; t++)
                                {
                                    if (dtlb.Select("lname='" + dtout.Columns[t].ColumnName.ToString().Replace("'", "''") + "'").Length > 0)
                                    {
                                        lll.Add(dtout.Columns[t].ColumnName, drrr[dtout.Columns[t].ColumnName].ToString());
                                    }
                                }

                                string sql = getsql(limid, txtcppf, txtmspf, txtgtpf, txtfhpf, txtkdnx, txtzxl, txtjgqj);
                                List<string> ru = getcsvvvv(sql, limid, txthl, txtjgbs, txtpdcount, txtwjcount, path, dtablename, drow, lll.Count > 0 ? true : false, lll);
                                allfile.AddRange(ru);

                            }
                        }
                    }
                    Literal3.Text = "打包下载";
                    if (!Directory.Exists(Server.MapPath("/zip" + uid + "/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/zip" + uid + "/"));
                    }
                    string dowpath = "/zip" + uid + "/" + Session["xlsname"] + "_" + DateTime.Now.ToString("HHmmss") + ".zip";
                    string zippath = Server.MapPath(dowpath);
                    if (CreateZip(allfile, zippath))
                    {
                        Response.Write("<script>window.open('" + dowpath + "', '_blank');</script>");
                    }
                }
            }
        }


    }
}