using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Text;

/// <summary>
/// access_sql 的摘要说明
/// </summary>
public class access_sql
{
    public access_sql()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    /// <summary>
    /// MD5加密
    /// </summary>
    /// <param name="strSource">需要加密的明文</param>
    /// <returns>返回32位加密结果</returns>
    public static string Get_MD5(string strSource, string sEncode)
    {
        //new 
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

        //获取密文字节数组
        byte[] bytResult = md5.ComputeHash(System.Text.Encoding.GetEncoding(sEncode).GetBytes(strSource));

        //转换成字符串，并取9到25位 
        //string strResult = BitConverter.ToString(bytResult, 4, 8);  
        //转换成字符串，32位 

        string strResult = BitConverter.ToString(bytResult);

        //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉 
        strResult = strResult.Replace("-", "");

        return strResult.ToLower();
    }
    public static string SetHeaderHtml(string Title, string KeyWords, string Description, Literal lititle)
    {

        string title = access_sql.GetOneValue("select txtTitle from siteconfig where txtid=1");

        if (Title != null && Title != "")
        {
            lititle.Text = Title + ", " + title.Replace("\r\n", "");
        }
        if (KeyWords != null && KeyWords != "")
        {
            KeyWords = KeyWords + ", ";
        }
        if (Description != null && Description != "")
        {
            Description = Description + ", ";
        }

        StringBuilder StrHeaderHtml = new StringBuilder();



        StrHeaderHtml.Append("<meta name=\"keywords\" content=\"");
        StrHeaderHtml.Append(KeyWords);
        StrHeaderHtml.Append(title);
        StrHeaderHtml.Append("\" />\n");

        StrHeaderHtml.Append("<meta name=\"description\" content=\"");
        StrHeaderHtml.Append(Description);
        StrHeaderHtml.Append(title);
        StrHeaderHtml.Append("\" />");

        return StrHeaderHtml.ToString();
    }



    public static readonly string connstring = ConfigurationSettings.AppSettings["SQLConnectionString"];
    private static SqlConnection conn;
    //public static string rep(string str) 
    //{

    //}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string sub(string str, int length)
    {
        return str.Length < length ? str : str.Substring(0, length) + "...";
    }


    public static void RepeaterBind(string[] strsql, Repeater[] repeat)
    {
        using (SqlConnection conn = new SqlConnection(connstring))
        {
            conn.Open();
            SqlDataAdapter oda = null;
            for (int i = 0; i < strsql.Length; i++)
            {
                if (strsql[i] != null && repeat[i] != null)
                {
                    using (oda = new SqlDataAdapter(strsql[i], conn))
                    {
                        using (DataTable dt = new DataTable(i.ToString()))
                        {
                            try
                            {
                                oda.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    repeat[i].DataSource = dt;
                                    repeat[i].DataBind();
                                }
                            }
                            catch
                            {
                                throw new Exception(i.ToString());
                            }
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// 根据得到的最大行数,来获取序号
    /// </summary>
    /// <param name="FileName"></param>
    /// <param name="TableName"></param>
    /// <returns></returns>
    public static int getMaxID(string FileName, string TableName)
    {
        string strsql = string.Format("SELECT   max({0}) AS Expr1 FROM  {1}", FileName, TableName);
        return ExecInt(strsql);
    }



    /// <summary>
    /// 通用数据添加方法
    /// </summary>
    /// <param name="CloumsName">列名称数组</param>
    /// <param name="parms">参数数组</param>
    /// <param name="tableName">要添加的表名称</param>
    /// <returns></returns>
    public static bool T_Insert_ExecSqls(string[] CloumsName, object[] parms, string tableName)
    {
        bool result = false;
        using (SqlConnection conn = new SqlConnection(connstring))
        {
            //增加一条数据
            conn.Open();
            StringBuilder strbul = new StringBuilder();
            strbul.Append(string.Format("insert into {0}", tableName));
            //insert into user(1,2,3) values(1,2,3)
            for (int i = 0; i < CloumsName.Length; i++)
            {
                if (i == 0)
                {
                    strbul.Append(string.Format("({0}", CloumsName[i]));
                    if (CloumsName.Length - 1 == 0)
                    {
                        strbul.Append(") ");
                    }
                }
                else
                {
                    if (i == CloumsName.Length - 1)
                    {
                        strbul.Append(string.Format(",{0}", CloumsName[i] + ") "));
                    }
                    else
                    {
                        strbul.Append(string.Format(",{0}", CloumsName[i]));
                    }
                }
            }
            for (int i = 0; i < parms.Length; i++)
            {
                if (i == 0)
                {
                    strbul.Append(string.Format("values(@{0}", CloumsName[i]));
                    if (parms.Length - 1 == 0)
                    {
                        strbul.Append(")");
                    }
                }
                else
                {
                    if (i == parms.Length - 1)
                    {
                        strbul.Append(string.Format(",@{0})", CloumsName[i]));
                    }
                    else
                    {
                        strbul.Append(string.Format(",@{0}", CloumsName[i]));
                    }
                }
            }
            using (SqlCommand cmd = new SqlCommand(strbul.ToString(), conn))
            {
                cmd.CommandText = strbul.ToString();//添加到SQL语句
                cmd.CommandTimeout = 0;
                for (int i = 0; i < parms.Length; i++)
                {
                    cmd.Parameters.Add(new SqlParameter("@" + CloumsName[i], parms[i]));//添加参数
                }

                if (cmd.ExecuteNonQuery() > 0)
                    result = true;
            }
        }
        return result;
    }





    /// <summary>
    /// 返回当前第一行第一列2(object)
    /// </summary>
    /// <param name="StrSql"></param>
    /// <returns></returns>
    public static object ExecInt2(string StrSql)
    {
        using (SqlConnection conn = new SqlConnection(connstring))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(StrSql, conn))
            {
                cmd.CommandTimeout = 0;
                return cmd.ExecuteScalar();
            }
        }
    }
    public static object ExecIntGetCount(string StrSql)
    {
        using (SqlConnection conn = new SqlConnection(connstring))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(StrSql, conn))
            {
                cmd.CommandTimeout = 0;
                object obj = cmd.ExecuteScalar();
                int i = 0;
                if (obj != DBNull.Value)
                { i = Convert.ToInt32(obj); }
                return i;
            }
        }
    }


    public static string gettitle(string str, string pagename)
    {
        string strtitle = "";
        if (pagename == "pd")
        {
            strtitle = (str != "" ? str : "");
        }
        else if (pagename == "pc")
        {
            strtitle = (str != "" ? str : "");

        }
        return strtitle;
    }
    public static string getdesciption(string str)
    {
        str = str.Trim();
        string getdescription = "<meta name='description' content='" + (str == "" ? "" : str) + "' />";
        return getdescription;

    }
    public static string getkeywords(string str)
    {
        str = str.Trim();
        string keywords = "<meta name='keywords' content='" + (str == "" ? "" : str) + "' />";
        return keywords;
    }
    public static DataTable picpath(string path)
    {
        string[] img = path.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        DataTable oDT = new DataTable();
        oDT.Columns.Add("picpath");
        DataRow oDR;
        for (int i = 0; i < img.Length; i++)
        {
            oDR = oDT.NewRow();
            oDR["picpath"] = "/" + img[i];
            oDT.Rows.Add(oDR);

        }
        return oDT;
    }
    public static string GetFirstImg(string path)
    {
        DataTable dtimg = access_sql.picpath(path);
        return dtimg.Rows[0]["picpath"].ToString() == "" ? "" : dtimg.Rows[0]["picpath"].ToString();
        //return dtimg.Rows[dtimg.Rows.Count - 1]["picpath"].ToString() == "" ? "" : dtimg.Rows[dtimg.Rows.Count - 1]["picpath"].ToString();
    }
    //public static string GetFirstImg(string path)
    //{
    //    DataTable dtimg = access_sql.picpath(path);
    //    return dtimg.Rows[dtimg.Rows.Count - 1]["picpath"].ToString() == "" ? "" : dtimg.Rows[dtimg.Rows.Count - 1]["picpath"].ToString();


    public static string GetsmallImg(string path)
    {
        DataTable dtimg = access_sql.picpath(path);
        return dtimg.Rows[0]["picpath"].ToString() == "" ? "" : dtimg.Rows[0]["picpath"].ToString();
        //return dtimg.Rows[0]["picpath"].ToString() == "" ? "" : dtimg.Rows[0]["picpath"].ToString();
    }
    public static string GetImg(string path)
    {
        string[] img = path.Split('|');
        return img[0].ToString() == "" ? "" : img[0].ToString();

    }
    public static string GetsmallImgBG(string path)
    {
        DataTable dtimg = access_sql.picpathBG(path);

        return dtimg.Rows[0]["picpath"].ToString().Replace('s', 's') == "" ? "" : dtimg.Rows[0]["picpath"].ToString().Replace('s', 's');

    }
    public static DataTable picpathBG(string path)
    {
        string[] img = path.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        DataTable oDT = new DataTable();
        oDT.Columns.Add("picpath");
        DataRow oDR;
        for (int i = 0; i < img.Length; i++)
        {
            oDR = oDT.NewRow();
            oDR["picpath"] = img[i];
            oDT.Rows.Add(oDR);

        }
        return oDT;
    }
    public static DataSet GreatDs(string sql)
    {
        DataSet ds = new DataSet();
        SqlConnection conn = new SqlConnection();//创建连接对象
        conn.ConnectionString = connstring;//给连接字符串赋值
        conn.Open();//打开数据库
        try
        {
            SqlDataAdapter Dar = new SqlDataAdapter(sql, conn);
            Dar.SelectCommand.CommandTimeout = 300; //单位秒
            Dar.Fill(ds);
        }
        catch (Exception ex)
        {

        }
        finally
        {
            conn.Close();
        }
        return ds;
    }
    public static DataSet GreatDs(string sql,int timeout)
    {
        DataSet ds = new DataSet();
        SqlConnection conn = new SqlConnection();//创建连接对象
        conn.ConnectionString = connstring;//给连接字符串赋值
        conn.Open();//打开数据库
        try
        {
            SqlDataAdapter Dar = new SqlDataAdapter(sql, conn);
            Dar.SelectCommand.CommandTimeout = timeout; //单位秒
            Dar.Fill(ds);
        }
        catch (Exception ex)
        {

        }
        finally
        {
            conn.Close();
        }
        return ds;
    }
    public static float getnum22222(string rrrr)
    {
        float ru = 0;
        string[] cf = rrrr.Split('.');
        if (cf.Length == 2)
        {
            string hhh = cf[1];
            if (hhh.Length > 2)
            {
                hhh = hhh.Substring(0, 2);
            }
            ru = float.Parse(cf[0] + "." + hhh);
        }
        else
        {
            ru = float.Parse(rrrr);

        }
        return ru;
    }
    public static int fhint(string instr)
    {
        try
        {
            return int.Parse(instr);
        }
        catch
        {

            return -1;
        }
    }
    public static void DoSql(string sql)
    {
        SqlConnection conn = new SqlConnection();//创建连接对象
        conn.ConnectionString = connstring;//给连接字符串赋值
        conn.Open();//打开数据库
        try
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandTimeout = 0;
            cmd.ExecuteNonQuery();//
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            conn.Close();//关闭数据库
        }
    }
    public static bool ExecSql(string sql)
    {
        bool result = false;
        SqlConnection conn = new SqlConnection();//创建连接对象
        conn.ConnectionString = connstring;//给连接字符串赋值
        conn.Open();//打开数据库
        try
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandTimeout = 0;
            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }
        }
        catch (Exception ex)
        {
            //throw (ex);
        }
        finally
        {
            conn.Close();//关闭数据库
        }
        return result;
    }
    public static bool yzdlcg()
    {
        bool ru = false;
        if (HttpContext.Current.Request.Cookies["cu"] != null && HttpContext.Current.Request.Cookies["cp"] != null && HttpContext.Current.Request.Cookies["cuid"] != null)
        {
            string u = HttpContext.Current.Request.Cookies["cu"].Value;
            string p = HttpContext.Current.Request.Cookies["cp"].Value;
            string uid = HttpContext.Current.Request.Cookies["cuid"].Value;
            DataSet ds = access_sql.GreatDs("select * from yn_user where us='" + u + "' and pass='" + p + "' and id=" + uid + "");
            if (access_sql.yzTable(ds))
            {
                ru = true;
            }
        }

        return ru;
    }
    public static bool yzdl()
    {
        bool ru = false;
        if (HttpContext.Current.Request.Cookies["u"] != null && HttpContext.Current.Request.Cookies["p"] != null && HttpContext.Current.Request.Cookies["uid"] != null)
        {
            string u = HttpContext.Current.Request.Cookies["u"].Value;
            string p = HttpContext.Current.Request.Cookies["p"].Value;
            string uid = HttpContext.Current.Request.Cookies["uid"].Value;
            DataSet ds = access_sql.GreatDs("select * from users where uname='" + u + "' and upwd='" + p + "' and uid=" + uid + " and uzt=0 ");
            if (access_sql.yzTable(ds))
            {
                ru = true;
            }
        }

        return ru;
    }
    public static string GetMD5_32(string input)
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] data = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(input));
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            sb.Append(data[i].ToString("x2"));
        }
        return sb.ToString();
    }
    public static string GetOneValue(string sql)
    {
        string result = "";
        SqlConnection conn = new SqlConnection();//创建连接对象
        conn.ConnectionString = connstring;//给连接字符串赋值
        conn.Open();//打开数据库
        try
        {


            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandTimeout = 0;
            result = cmd.ExecuteScalar().ToString();

        }
        catch (Exception ex)
        {
            //throw (ex);
        }
        finally
        {
            conn.Close();//关闭数据库
        }
        return result;
    }
    public static string getimg(string ZhuTuWangZhi)
    {
        string ru = "";
        if (ZhuTuWangZhi != "")
        {
            ru = ZhuTuWangZhi;
            if (ru.Contains("|"))
            {
                ru = ru.Split('|')[0];
            }
        }
        return ru;
    }
    public static string getimg2(string ZhuTuWangZhi, bool ck)
    {
        string ru = "";
        if (ck)
        {
            if (ZhuTuWangZhi != "")
            {

                if (ZhuTuWangZhi.Contains("|"))
                {
                    string[] aaa = ZhuTuWangZhi.Split('|');
                    for (int i = 0; i < aaa.Length; i++)
                    {
                        if (i != 0)
                        {
                            ru += " <a href='" + aaa[i] + "' target='_blank'><img src='" + aaa[i] + "' style='width: 50px; height: 50px' /></a>";
                        }
                    }
                }
            }
        }
        return ru;
    }

    public static string GetOneValue(string sql, string connstring)
    {
        string result = "";
        SqlConnection conn = new SqlConnection();//创建连接对象
        conn.ConnectionString = connstring;
        conn.Open();//打开数据库
        try
        {


            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandTimeout = 0;
            result = cmd.ExecuteScalar().ToString();

        }
        catch (Exception ex)
        {
            //throw (ex);
        }
        finally
        {
            conn.Close();//关闭数据库
        }
        return result;
    }

    public DataSet GetDs(string sql)
    {
        SqlConnection conn = new SqlConnection();//创建连接对象
        conn.ConnectionString = connstring;//给连接字符串赋值
        conn.Open();//打开数据库
        DataSet ds = new DataSet();
        try
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);


            da.Fill(ds);
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            conn.Close();//关闭数据库
        }

        return ds;
    }
    public static DataSet GetDs(string sql, string tablename)
    {
        SqlConnection conn = new SqlConnection();//创建连接对象
        conn.ConnectionString = connstring;//给连接字符串赋值
        conn.Open();//打开数据库
        DataSet ds = new DataSet();
        try
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);


            da.Fill(ds, tablename);
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            conn.Close();//关闭数据库
        }
        return ds;
    }


    public static string GetD_ID()
    {
        return GetOneValue("select D_ID from leaguer where D_username='" + System.Web.HttpContext.Current.Session["uid"].ToString() + "'");
    }

    public static DataTable Get_DataStr(string datastr)
    {
        string[] ppath = datastr.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        DataTable oDT = new DataTable();
        oDT.Columns.Add("str");
        DataRow oDR;

        for (int i = 0; i < ppath.Length; i++)
        {
            oDR = oDT.NewRow();
            oDR["str"] = ppath[i];
            oDT.Rows.Add(oDR);

        }
        return oDT;
    }

    public static bool InsetCommand(string Sqltext, string[] inset, string[] value)
    {
        bool result = false;
        try
        {
            conn = new SqlConnection(connstring);
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand com = new SqlCommand(Sqltext, conn);
            for (int i = 0; i < inset.Length; i++)
            {
                com.Parameters.Add(new SqlParameter(inset[i], SqlDbType.NText));
                com.Parameters[inset[i]].Value = value[i];
            }
            if (com.ExecuteNonQuery() > 0)
            {
                result = true;
            }
            if (conn.State == ConnectionState.Open) conn.Close();
            //MessageBox.Show("插入或修改成功");
        }
        catch (SqlException se)
        {
            if (conn.State == ConnectionState.Open) conn.Close();
            result = false;

        }
        return result;
    }
    public static SqlCommand GetCmd(SqlCommand cmd, string sql)
    {

        SqlConnection conn = new SqlConnection();//创建连接对象
        conn.ConnectionString = connstring;//给连接字符串赋值
        conn.Open();//打开数据库
        cmd = new SqlCommand(sql, conn);
        return cmd;
    }
    public static bool yzTable(DataSet ds)
    {
        bool re = false;
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                re = true;
            }
        }
        return re;
    }
    public static string Getzhekou(double xj, double yj)
    {

        if (xj != null && yj != null)
        {
            double s = Convert.ToDouble((xj / yj));
            string z = (100 - (s * 100)).ToString("0.0");
            return z + "%";
        }
        else
        { return ""; }

    }
    public static DataTable GetAddress(string username, int type)
    {
        string set = "";
        string Oset = "";
        if (type == 1)
        {
            Oset = "D_set";
            set = "Y_set";
        }
        else
        {
            Oset = "D_bet";
            set = "Y_bet";
        }
        int D_ID = Convert.ToInt32(access_sql.GetOneValue("select D_ID from Leaguer where D_UserName='" + username + "'"));
        string strsql = "select D_FirstName,D_LastName,D_Address,city,D_State,D_Code,D_Country,D_Address2,D_set,D_bet,D_ID from Leaguer where D_ID = " + D_ID + "";
        if (access_sql.yzTable(access_sql.GreatDs(strsql)))
        {
            DataTable dt = access_sql.GreatDs(strsql).Tables[0];
            if (dt.Rows[0]["" + Oset + ""].ToString() == "1")
            {
                return dt;
            }
            else
            {
                strsql = "select Y_FirstName,Y_LastName,Y_Address,Y_City,Y_State,Y_Zip,Y_Country,Y_Address2 from Leaguer_Two where I_ID=" + dt.Rows[0]["D_ID"] + " and " + set + "=1";
                if (access_sql.yzTable(access_sql.GreatDs(strsql)))
                {
                    return access_sql.GreatDs(strsql).Tables[0];
                }
                else
                {
                    return null;
                }

            }
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 执行Sql语句,返回表 结构
    /// </summary>
    /// <param name="StrSql">Sql查询语句</param>
    /// <returns></returns>
    public static DataTable GetTable(string StrSql)
    {
        using (SqlConnection conn = new SqlConnection(connstring))
        {
            conn.Open();
            using (SqlDataAdapter oda = new SqlDataAdapter(StrSql, conn))
            {
                using (DataTable dt = new DataTable("Table"))
                {
                    oda.Fill(dt);
                    return dt;
                }
            }
        }
    }


    /// <summary>       /// 处理：字符串过滤html标签    
    /// </summary>      
    /// <paramname="text">要过滤的字符串</param>      
    /// <returns></returns>      
    public static string stringDelHtml(string text)
    {
        text = Regex.Replace(text, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"-->", "", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"<!--.*", "", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"&#(\d+);", "", RegexOptions.IgnoreCase);
        text.Replace("<", "");
        text.Replace(">", "");
        text.Replace("\r\n", "");
        text = HttpContext.Current.Server.HtmlEncode(text).Trim();
        return text;
    }
    public static string stringDelHtml__(string text)
    {
        text = Regex.Replace(text, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
        text.Replace("<", "");
        text.Replace(">", "");
        text.Replace("\r\n", "");
        //text = HttpContext.Current.Server.HtmlEncode(text).Trim();
        return text;
    }
    public static string SetcanonicalByPid(string pid)
    {
        string pcanonical = access_sql.GetOneValue("select p_canonical from product where p_id = " + pid + "");
        if (pcanonical != "")
        {
            return "<link rel=\"canonical\" href=\"" + pcanonical + "\"/>";
        }
        else
        {
            return "";
        }
    }
    public static string SetcanonicalByCid(string cid)
    {
        string ccanonical = access_sql.GetOneValue("select c_canonical from Content_Class where cid = '" + cid + "'");
        if (ccanonical != "")
        {

            return "<link rel=\"canonical\" href=\"" + ccanonical + "\"/>";
        }
        else
        {
            return "";
        }
    }
    public static string Setcanonical(string canonical)
    {

        if (canonical != "")
        {
            return "<link rel=\"canonical\" href=\"" + canonical + "\"/>";
        }
        else
        {
            return "";
        }
    }



    public static void Gouwu(string pid, float price, int shuliang, string sizeindex, string size, string colorindex, string color)
    {
        HttpCookie hc = null;
        if (HttpContext.Current.Request.Cookies["ShoppingCart"] == null)
        {
            //如果Cookies中不存在ShoppingCart，则创建
            hc = new HttpCookie("ShoppingCart");
        }
        else
        {
            //如果Cookies中存在ShoppingCart，则取出
            hc = HttpContext.Current.Request.Cookies["ShoppingCart"];

        }
        bool flag = true;//标记在购物车中是否存在本次选择的物品

        //在购物车的Cookies中查找是否存在这次要选择的物品
        foreach (string item in hc.Values)
        {
            if (item == pid + "-" + sizeindex + "-" + colorindex)
            {
                flag = false;
                break;
            }
        }
        if (flag)
        {
            //如果选择的内容在购物车中没有，则创建一个新的子键
            hc.Values.Add(pid + "-" + sizeindex + "-" + colorindex, pid + "|" + price + "|" + shuliang + "|" + size + "|" + sizeindex + "|" + color + "|" + colorindex);
        }
        else
        {
            int num = int.Parse(hc.Values[pid + "-" + sizeindex + "-" + colorindex].Split(new char[] { '|' })[2]) + shuliang;
            hc.Values.Remove(pid + "-" + sizeindex + "-" + colorindex);
            hc.Values.Add(pid + "-" + sizeindex + "-" + colorindex, pid + "|" + price + "|" + num + "|" + size + "|" + sizeindex + "|" + color + "|" + colorindex);

        }
        //hc.Expires = DateTime.Now.AddDays(1);
        HttpContext.Current.Response.Cookies.Add(hc);
    }


    public static void Gouwu(string pid, float price, int shuliang, string sizeindex, string size)
    {
        HttpCookie hc = null;
        if (HttpContext.Current.Request.Cookies["ShoppingCart"] == null)
        {
            //如果Cookies中不存在ShoppingCart，则创建
            hc = new HttpCookie("ShoppingCart");
        }
        else
        {
            //如果Cookies中存在ShoppingCart，则取出
            hc = HttpContext.Current.Request.Cookies["ShoppingCart"];

        }
        bool flag = true;//标记在购物车中是否存在本次选择的物品

        //在购物车的Cookies中查找是否存在这次要选择的物品
        foreach (string item in hc.Values)
        {
            if (item == pid + "-" + sizeindex)
            {
                flag = false;
                break;
            }
        }
        if (flag)
        {
            //如果选择的内容在购物车中没有，则创建一个新的子键
            hc.Values.Add(pid + "-" + sizeindex, pid + "|" + price + "|" + shuliang + "|" + size + "|" + sizeindex);
        }
        else
        {
            int num = int.Parse(hc.Values[pid + "-" + sizeindex].Split(new char[] { '|' })[2]) + shuliang;
            hc.Values.Remove(pid + "-" + sizeindex);
            hc.Values.Add(pid + "-" + sizeindex, pid + "|" + price + "|" + num + "|" + size + "|" + sizeindex);

        }
        //hc.Expires = DateTime.Now.AddDays(1);
        HttpContext.Current.Response.Cookies.Add(hc);
    }
    public static void UpdateGouwu(string pid, float price, int shuliang, string sizeindex, string size, string YsizeID, string cz, string oldsizeid)
    {

        HttpCookie hc = null;
        hc = HttpContext.Current.Request.Cookies["ShoppingCart"];
        hc.Values.Remove(pid + "-" + YsizeID);
        bool flag = true;

        foreach (string item in hc.Values)
        {
            if (item == pid + "-" + sizeindex + "-" + cz)
            {
                flag = false;
                break;
            }
        }
        if (flag)
        {
            hc.Values.Remove(pid + "-" + oldsizeid + "-" + cz);
            //如果选择的内容在购物车中没有，则创建一个新的子键
            hc.Values.Add(pid + "-" + sizeindex + "-" + cz, pid + "|" + price + "|" + shuliang + "|" + size + "|" + sizeindex + "|" + cz);
        }
        else
        {
            int num = shuliang;
            hc.Values.Remove(pid + "-" + sizeindex + "-" + cz);
            hc.Values.Add(pid + "-" + sizeindex + "-" + cz, pid + "|" + price + "|" + num + "|" + size + "|" + sizeindex + "|" + cz);

        }
        hc.Expires = DateTime.Now.AddDays(1);
        HttpContext.Current.Response.Cookies.Add(hc);
    }
    public static void UpdateGouwu(string pid, float price, int shuliang, string sizeindex, string size, string YsizeID)
    {

        HttpCookie hc = null;
        hc = HttpContext.Current.Request.Cookies["ShoppingCart"];
        hc.Values.Remove(pid + "-" + YsizeID);
        bool flag = true;
        foreach (string item in hc.Values)
        {
            if (item == pid + "-" + sizeindex)
            {
                flag = false;
                break;
            }
        }
        if (flag)
        {
            //如果选择的内容在购物车中没有，则创建一个新的子键
            hc.Values.Add(pid + "-" + sizeindex, pid + "|" + price + "|" + shuliang + "|" + size + "|" + sizeindex);
        }
        else
        {
            int num = int.Parse(hc.Values[pid + "-" + sizeindex].Split(new char[] { '|' })[2]) + shuliang;
            hc.Values.Remove(pid + "-" + sizeindex);
            hc.Values.Add(pid + "-" + sizeindex, pid + "|" + price + "|" + num + "|" + size + "|" + sizeindex);

        }
        hc.Expires = DateTime.Now.AddDays(1);
        HttpContext.Current.Response.Cookies.Add(hc);
    }

    public static void DeleteGouwu(string pid, string sizeindex)
    {
        HttpCookie hc = null;
        hc = HttpContext.Current.Request.Cookies["ShoppingCart"];
        hc.Values.Remove(pid + "-" + sizeindex);
        hc.Expires = DateTime.Now.AddDays(1);
        HttpContext.Current.Response.Cookies.Add(hc);
    }
    public static void DeleteGouwu(string pid, string sizeindex, string colorindex)
    {
        HttpCookie hc = null;
        hc = HttpContext.Current.Request.Cookies["ShoppingCart"];
        hc.Values.Remove(pid + "-" + sizeindex + "-" + colorindex);
        hc.Expires = DateTime.Now.AddDays(1);
        HttpContext.Current.Response.Cookies.Add(hc);
    }
    public static string jqbt(string bt)
    {
        string ru = "";

        ru = SubstringTonum(bt, 75).Trim();
        while (ru.Length > 75)
        {
            ru = ru.Substring(0, ru.LastIndexOf(" "));
        }
        return ru;
    }
    public static string SubstringTonum(string str, int num)
    {
        str = str.Replace("&amp;amp;", "&").Replace("&amp;#180;", "'").Replace("&amp;", "&");
        bool flag = false;
        if (num > str.Length || num == str.Length)
        {
            num = str.Length - 2;
            return str;
        }
        else
        {
            string rustr = "";
            int position = 0;//要截取的位置

            while (!flag)
            {
                if (position < str.Length - 1)
                {
                    string s = str.Substring(position, 1);//要截取的位置的字符
                    byte a = ASCIIEncoding.ASCII.GetBytes(s)[0];
                    if ((a < 65 || a > 122) && position >= num)
                    {
                        flag = true;
                    }

                    else
                    {
                        position += 1;
                    }
                }
                else
                {
                    flag = true;

                }
            }
            rustr = str.Substring(0, position + 1);
            return rustr;
        }

    }


    public static float GetYunFei(float p_jiage)
    {
        float ru = 0;
        DataRow dr = access_sql.GreatDs("select Y_OutPrice,Y_YunFei from N_YunFei where Y_YID=1").Tables[0].Rows[0];
        float outprice = float.Parse(dr[0].ToString());
        float yunfei = float.Parse(dr[1].ToString());

        if (outprice > p_jiage)
        {
            ru = yunfei;
        }
        return ru;

    }


    public static object T_Insert_ExecSql_top(string[] Cloums, object[] CloumsValue, string tableName)
    {
        using (SqlConnection conn = new SqlConnection(connstring))
        {
            //增加一条数据
            conn.Open();
            StringBuilder strbul = new StringBuilder();
            strbul.Append(string.Format("insert into {0}", tableName));
            //insert into user(1,2,3) values(1,2,3)
            for (int i = 0; i < Cloums.Length; i++)
            {
                if (i == 0)
                {
                    strbul.Append(string.Format("({0}", Cloums[i]));
                    if (Cloums.Length - 1 == 0)
                    {
                        strbul.Append(") ");
                    }
                }
                else
                {
                    if (i == Cloums.Length - 1)
                    {
                        strbul.Append(string.Format(",{0}", Cloums[i] + ") "));
                    }
                    else
                    {
                        strbul.Append(string.Format(",{0}", Cloums[i]));
                    }
                }
            }
            for (int i = 0; i < CloumsValue.Length; i++)
            {
                if (i == 0)
                {
                    strbul.Append(string.Format("values(@{0}", Cloums[i]));
                    if (CloumsValue.Length - 1 == 0)
                    {
                        strbul.Append(")");
                    }
                }
                else
                {
                    if (i == CloumsValue.Length - 1)
                    {
                        strbul.Append(string.Format(",@{0})", Cloums[i]));
                    }
                    else
                    {
                        strbul.Append(string.Format(",@{0}", Cloums[i]));
                    }
                }
            }
            using (SqlCommand cmd = new SqlCommand(strbul.ToString() + "; SELECT SCOPE_IDENTITY();", conn))
            {
                // cmd.CommandText = strbul.ToString();//添加到SQL语句
                cmd.CommandTimeout = 0;
                for (int i = 0; i < CloumsValue.Length; i++)
                {
                    cmd.Parameters.Add(new SqlParameter("@" + Cloums[i], CloumsValue[i]));//添加参数
                }

                return cmd.ExecuteScalar();//执行
            }
        }
    }
    public static int T_Insert_ExecSql(string[] Cloums, object[] CloumsValue, string tableName)
    {
        using (SqlConnection conn = new SqlConnection(connstring))
        {
            //增加一条数据
            conn.Open();
            StringBuilder strbul = new StringBuilder();
            strbul.Append(string.Format("insert into {0}", tableName));
            //insert into user(1,2,3) values(1,2,3)
            for (int i = 0; i < Cloums.Length; i++)
            {
                if (i == 0)
                {
                    strbul.Append(string.Format("({0}", Cloums[i]));
                    if (Cloums.Length - 1 == 0)
                    {
                        strbul.Append(") ");
                    }
                }
                else
                {
                    if (i == Cloums.Length - 1)
                    {
                        strbul.Append(string.Format(",{0}", Cloums[i] + ") "));
                    }
                    else
                    {
                        strbul.Append(string.Format(",{0}", Cloums[i]));
                    }
                }
            }
            for (int i = 0; i < CloumsValue.Length; i++)
            {
                if (i == 0)
                {
                    strbul.Append(string.Format("values(@{0}", Cloums[i]));
                    if (CloumsValue.Length - 1 == 0)
                    {
                        strbul.Append(")");
                    }
                }
                else
                {
                    if (i == CloumsValue.Length - 1)
                    {
                        strbul.Append(string.Format(",@{0})", Cloums[i]));
                    }
                    else
                    {
                        strbul.Append(string.Format(",@{0}", Cloums[i]));
                    }
                }
            }
            using (SqlCommand cmd = new SqlCommand(strbul.ToString(), conn))
            {
                cmd.CommandText = strbul.ToString();//添加到SQL语句
                cmd.CommandTimeout = 0;
                for (int i = 0; i < CloumsValue.Length; i++)
                {
                    cmd.Parameters.Add(new SqlParameter("@" + Cloums[i], CloumsValue[i]));//添加参数
                }

                return cmd.ExecuteNonQuery();//执行
            }
        }
    }

    public static int T_Insert_ExecSql1(string[] Cloums, object[] CloumsValue, string tableName)
    {
        using (SqlConnection conn = new SqlConnection(connstring))
        {
            //增加一条数据
            conn.Open();
            StringBuilder strbul = new StringBuilder();
            strbul.Append(string.Format("insert into {0}", tableName));
            //insert into user(1,2,3) values(1,2,3)
            for (int i = 0; i < Cloums.Length; i++)
            {
                if (i == 0)
                {
                    strbul.Append(string.Format("({0}", Cloums[i]));
                    if (Cloums.Length - 1 == 0)
                    {
                        strbul.Append(") ");
                    }
                }
                else
                {
                    if (i == Cloums.Length - 1)
                    {
                        strbul.Append(string.Format(",{0}", Cloums[i] + ") "));
                    }
                    else
                    {
                        strbul.Append(string.Format(",{0}", Cloums[i]));
                    }
                }
            }
            for (int i = 0; i < CloumsValue.Length; i++)
            {
                if (i == 0)
                {
                    strbul.Append(string.Format("values(@{0}", Cloums[i]));
                    if (CloumsValue.Length - 1 == 0)
                    {
                        strbul.Append(")");
                    }
                }
                else
                {
                    if (i == CloumsValue.Length - 1)
                    {
                        strbul.Append(string.Format(",@{0})", Cloums[i]));
                    }
                    else
                    {
                        strbul.Append(string.Format(",@{0}", Cloums[i]));
                    }
                }
            }
            using (SqlCommand cmd = new SqlCommand(strbul.ToString(), conn))
            {
                cmd.CommandText = strbul.ToString();//添加到SQL语句
                cmd.CommandTimeout = 0;
                for (int i = 0; i < CloumsValue.Length; i++)
                {
                    cmd.Parameters.Add(new SqlParameter("@" + Cloums[i], CloumsValue[i]));//添加参数
                }

                return cmd.ExecuteNonQuery();//执行
            }
        }
    }
    public static int T_Update_ExecSql(string[] Cloums, object[] CloumsValue, string TableName, string strWhere)
    {
        using (SqlConnection conn = new SqlConnection(connstring))
        {

            //修改一条数据
            conn.Open();

            StringBuilder strbul = new StringBuilder();
            strbul.Append(string.Format("update {0} set ", TableName));
            //update news set name='',body='' where id=
            for (int i = 0; i < Cloums.Length; i++)
            {
                if (i == Cloums.Length - 1)
                {
                    strbul.Append(string.Format("{0}=@{0}", Cloums[i]));
                }
                else
                {
                    strbul.Append(string.Format("{0}=@{0},", Cloums[i]));
                }
            }
            if (strWhere != "")
            {
                strbul.Append(string.Format(" where {0}", strWhere));
            }

            using (SqlCommand cmd = new SqlCommand(strbul.ToString(), conn))
            {
                cmd.CommandText = strbul.ToString();//添加到SQL语句
                cmd.CommandTimeout = 0;
                for (int i = 0; i < CloumsValue.Length; i++)
                {
                    cmd.Parameters.Add(new SqlParameter("@" + Cloums[i], CloumsValue[i]));//添加参数
                }

                return cmd.ExecuteNonQuery();//执行
            }
        }
    }
    public static DataTable getcwbycid(int cid)
    {
        DataTable dt = new DataTable();
        string zuidaname = "";
        DataRow drchu = access_sql.GreatDs("select classtj,classpre,classname from content_class where cid=" + cid + "").Tables[0].Rows[0];

        if (drchu[0].ToString() == "2")
        {
            zuidaname = access_sql.GetOneValue("select classname from content_class where classid='" + drchu[1].ToString() + "'");
        }
        else if (drchu[0].ToString() == "3")
        {
            zuidaname = access_sql.GetOneValue("select classname from content_class where classid='" + access_sql.GetOneValue("select classpre from content_class where classid='" + drchu[1].ToString() + "'") + "'");

        }
        else
        {
            zuidaname = drchu["classname"].ToString();
        }

        if (zuidaname.Equals("NIKE MAX"))
        {
            dt = access_sql.GreatDs("select * from nike_max_cc").Tables[0];
        }
        else if (zuidaname.Equals("NIKE FREE"))
        {
            dt = access_sql.GreatDs("select * from nike_free_cc").Tables[0];
        }
        else if (zuidaname.Equals("NIKE JORDAN"))
        {
            dt = access_sql.GreatDs("select * from nike_jordan_cc").Tables[0];
        }
        else if (zuidaname.Equals("NIKE FOOTBALL"))
        {
            dt = access_sql.GreatDs("select * from nike_football_cc").Tables[0];
        }
        else if (zuidaname.Equals("NIKE BASKET"))
        {
            dt = access_sql.GreatDs("select * from nike_basket_cc").Tables[0];
        }
        return dt;
    }
    public static int ExecInt(string StrSql)
    {
        using (SqlConnection conn = new SqlConnection(connstring))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(StrSql, conn))
            {
                cmd.CommandTimeout = 0;
                object obj = cmd.ExecuteScalar();
                int i = 1;
                if (obj != DBNull.Value)
                { i = Convert.ToInt32(obj); }
                return i;
            }
        }
    }
    public static string bubian(string cs)
    {
        string ru = "";
        ru = cs.Replace("&#x2154", "⅔").Replace("&#x2153", "⅓");
        return ru;
    }
    public static string bianhui(string cs)
    {
        string ru = "";
        ru = cs.Replace("⅔", "&#x2154").Replace("⅓", "&#x2153");
        return ru;
    }
}
