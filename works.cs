

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using System.Web;
using System.Text;
using Spire.Xls;
using Microsoft.VisualBasic.FileIO;
using System.Linq;

namespace WebApplication11
{
    public class works
    {
        //2 ，使用Office自带的库


        public static System.Data.DataTable ReadExcelData_(string filePath, string sheetname, int rowIndex)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("lname");
            dt.Columns.Add("lbt");
            try
            {


                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = new XSSFWorkbook(file);
                    ISheet sheet = workbook.GetSheet(sheetname);
                    if (sheet != null)
                    {

                        IRow row = sheet.GetRow(rowIndex - 1);
                        IRow rowjiayi = sheet.GetRow(rowIndex);

                        if (row != null)
                        {
                            for (int i = 0; i < row.LastCellNum; i++)
                            {
                                ICell cell = row.GetCell(i);

                                string b = "0";
                                string r = "";
                                NPOI.SS.UserModel.IComment aaa = cell.CellComment;
                                if (aaa != null)
                                {
                                    b = "1";

                                }



                                if (cell != null)
                                {
                                    r = (cell.ToString().Trim());
                                }

                                ICell celljiayi = rowjiayi.GetCell(i);
                                if (celljiayi != null)
                                {
                                    if (celljiayi.ToString().Trim().ToLower() == "mandatory"|| celljiayi.ToString().Trim().ToLower() == "ps_mandatory" || celljiayi.ToString().Trim().ToLower() == "必填")
                                    {
                                        b = "1";
                                    }
                                }
                                if (r != "")
                                {
                                    dt.Rows.Add(new object[] { r, b });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return dt;
        }
        public static string getshr(string yybm)
        {
            string ru = "";
            switch (yybm)
            {
                case "haicang":
                    ru = "徐先生-海仓";
                    break;
                case "yacang":
                    ru = "徐先生-雅仓";
                    break;
                case "goodslink":
                    ru = "徐先生-goodslink";
                    break;
            }
            return ru;
        }
        public static System.Data.DataTable ReadExcelData_(string filePath)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            try
            {


                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = new XSSFWorkbook(file);
                    ISheet sheet = workbook.GetSheetAt(0);
                    if (sheet != null)
                    {

                        IRow row = sheet.GetRow(0);

                        if (row != null)
                        {
                            for (int i = 0; i < row.LastCellNum; i++)
                            {
                                ICell cell = row.GetCell(i);
                                if (cell != null)
                                {

                                    dt.Columns.Add(cell.ToString().Trim());
                                }
                            }
                        }
                        for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
                        {
                            IRow row1 = sheet.GetRow(rowIndex);
                            if (row1 != null)
                            {
                                object[] obj = new object[dt.Columns.Count];

                                for (int colIndex = 0; colIndex < dt.Columns.Count; colIndex++)
                                {
                                    string cellValue = "";

                                    ICell cell = row1.GetCell(colIndex);

                                    if (cell != null)
                                    {

                                        cellValue = cell.ToString();

                                    }


                                    obj[colIndex] = cellValue;
                                }
                                dt.Rows.Add(obj);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return dt;
        }

        public static List<string> ReadExcelData(string filePath, int rowIndex)
        {
            List<string> ru = new List<string>();
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(file);
                ISheet sheet = workbook.GetSheet("Template");

                IRow row = sheet.GetRow(rowIndex - 1);
                if (row != null)
                {
                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        ICell cell = row.GetCell(i);
                        if (cell != null)
                        {
                            ru.Add(cell.ToString());
                        }
                    }
                }
            }
            return ru;
        }
        public static bool ZHCSV(string xlsmFilePath, string csvFilePath, string Tablename, int rowindex)
        {
            bool ru = true;
            try
            {


                using (FileStream file = new FileStream(xlsmFilePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = WorkbookFactory.Create(file);
                    ISheet sheet = workbook.GetSheet(Tablename);

                    // 打开File.csv文件用于写入
                    using (StreamWriter sw = new StreamWriter(csvFilePath))
                    {

                        // 写入每行每列的值
                        for (int i = (sheet.FirstRowNum); i < rowindex; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            for (int j = 0; j < row.LastCellNum; j++)
                            {
                                if (row.GetCell(j) != null)
                                {
                                    sw.Write(row.GetCell(j).ToString().Replace(",", ""));

                                    if (j < row.LastCellNum - 1)
                                    {
                                        sw.Write(",");
                                    }
                                }
                                else
                                {
                                    sw.Write(",");
                                }
                            }
                            sw.Write(Environment.NewLine);
                        }
                    }
                }
            }
            catch
            {

                ru = false;
            }
            return ru;
        }

        public static string[] CJCSV_A(string[] head, System.Data.DataTable dt, int pc, int wj, string did, string uid, int rowindex, string xlsmFilePath, string Tablename)
        {
            string[] temppathSS = new string[wj];

            try
            {


                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("/documentdownload" + uid + "/")))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("/documentdownload" + uid + "/"));
                }

                for (int d = 0; d < wj; d++)
                {
                    string dnnn = "(" + DateTime.Now.ToString("yyyyMMddhhmmss") + ")__" + did + "__" + (d + 1);
                    string rucsv = "/documentdownload" + uid + "/" + dnnn + ".csv";


                    string savecsv = System.Web.HttpContext.Current.Server.MapPath("/documentdownload" + uid + "/") + dnnn + ".csv";
                    string savexls = System.Web.HttpContext.Current.Server.MapPath("/documentdownload" + uid + "/") + dnnn + ".xls";

                    List<string[]> lll = new List<string[]>();


                    using (FileStream file = new FileStream(xlsmFilePath, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = WorkbookFactory.Create(file);
                        ISheet sheet = workbook.GetSheet(Tablename);

                        // 写入每行每列的值
                        for (int i = (sheet.FirstRowNum); i < rowindex; i++)
                        {
                            IRow row_XLSM = sheet.GetRow(i);
                            string lm = "";
                            string[] temp = new string[row_XLSM.LastCellNum];
                            for (int j = 0; j < row_XLSM.LastCellNum; j++)
                            {
                                lm = "";
                                if (row_XLSM.GetCell(j) != null)
                                {
                                    lm = (row_XLSM.GetCell(j).ToString());
                                }

                                temp[j] = lm;
                            }
                            lll.Add(temp);
                        }
                    }


                    //整理数据源
                    List<string[]> rows = new List<string[]>();
                    //rows.Add(head);    //头部标题
                    int star = d * pc;
                    int end = star + pc;
                    int hh = 0;
                    for (int r = star; r < end; r++)
                    {
                        string[] temp = new string[dt.Columns.Count];
                        DataRow item = dt.Rows[r];
                        string lm = "";
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            lm = "";
                            lm = item[i].ToString().Replace(",", "");

                            if (double.TryParse(lm, out double number))
                            {
                                temp[i] = "\t" + lm;
                            }
                            else
                            {
                                temp[i] = lm;
                            }

                        }
                        lll.Add(temp);
                        hh++;
                    }

                    if (!File.Exists(rucsv))
                    {
                        File.Create(savecsv).Close();

                    }

                    using (var writer = new StreamWriter(savecsv))
                    {
                        for (int i = 0; i < lll.Count; i++)
                        { // 写入标题行
                            writer.WriteLine(string.Join(",", lll[i]));

                        }
                    }







                    temppathSS[d] = rucsv;
                }
            }
            catch (Exception ex)
            {


            }
            return temppathSS;
        }
        public static List<string> gethead(int rowindex, string xlsmFilePath, string Tablename)
        {
            List<string> ru = new List<string>();
            using (FileStream file = new FileStream(xlsmFilePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = WorkbookFactory.Create(file);
                ISheet sheet = workbook.GetSheet(Tablename);
                // 写入每行每列的值

                IRow row_XLSM = sheet.GetRow(rowindex - 1);

                for (int j = 0; j < row_XLSM.LastCellNum; j++)
                {
                    if (row_XLSM.GetCell(j) != null)
                    {
                        string lm = (row_XLSM.GetCell(j).ToString());
                        ru.Add(lm);
                    }

                }

            }
            return ru;
        }
        public static string CJCSV(System.Data.DataTable dt, string did, int d, string uid, int rowindex, string xlsmFilePath, string Tablename, string rwname, string 店铺备注)
        {
            string temppathSS = "";

            try
            {


                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("/documentdownload" + uid + "/")))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("/documentdownload" + uid + "/"));
                }


                string dnnn = 店铺备注 + "__(" + DateTime.Now.ToString("hhmmss") + ")__" + rwname + "__" + did + "__" + (d + 1);
                string rucsv = "/documentdownload" + uid + "/" + dnnn + ".csv";


                string savecsv = System.Web.HttpContext.Current.Server.MapPath("/documentdownload" + uid + "/") + dnnn + ".csv";

                string savexls = System.Web.HttpContext.Current.Server.MapPath("/documentdownload" + uid + "/") + dnnn + ".xls";

                string outcsv = System.Web.HttpContext.Current.Server.MapPath("/documentdownload" + uid + "/") + dnnn + "_o.csv";
                IWorkbook workbook_CSV = new XSSFWorkbook();
                ISheet sheet_CSV = workbook_CSV.CreateSheet("Sheet1");

                using (FileStream file = new FileStream(xlsmFilePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = WorkbookFactory.Create(file);
                    ISheet sheet = workbook.GetSheet(Tablename);





                    // 写入每行每列的值
                    for (int i = (sheet.FirstRowNum); i < rowindex; i++)
                    {
                        IRow row_XLSM = sheet.GetRow(i);
                        IRow row_ = sheet_CSV.CreateRow(i);

                        for (int j = 0; j < row_XLSM.LastCellNum; j++)
                        {
                            if (row_XLSM.GetCell(j) != null)
                            {
                                string lm = (row_XLSM.GetCell(j).ToString());

                                ICell cell = row_.CreateCell(j);

                                cell.SetCellValue(lm);
                            }

                        }
                    }
                }


                //整理数据源
                List<string[]> rows = new List<string[]>();
                int hh = 0;
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    IRow row_ = sheet_CSV.CreateRow(rowindex + hh);

                    DataRow item = dt.Rows[r];
                    string[] temp = new string[dt.Columns.Count];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ICell cell = row_.CreateCell(i);
                        string lm = item[i].ToString().Replace(",", "".Replace("\"", ""));

                        cell.SetCellValue(lm);
                    }
                    hh++;
                }


                using (FileStream file = new FileStream(savexls, FileMode.Create, FileAccess.Write))
                {
                    workbook_CSV.Write(file,true);
                }



                Spire.Xls.Workbook workbooka = new Spire.Xls.Workbook();
                workbooka.LoadFromFile(savexls);
                Spire.Xls.Worksheet sheeta = workbooka.Worksheets[0];
                sheeta.SaveToFile(savecsv, ",", Encoding.UTF8);


                temppathSS = rucsv;


            }
            catch (Exception ex)
            {


            }
            return temppathSS;
        }
        public static string[] CJCSV(string[] aa, System.Data.DataTable dt, int pc, int wj, string did, string uid, int rowindex, string xlsmFilePath, string Tablename)
        {
            string[] temppathSS = new string[wj];

            try
            {


                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("/documentdownload" + uid + "/")))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("/documentdownload" + uid + "/"));
                }

                for (int d = 0; d < wj; d++)
                {
                    string dnnn = "(" + DateTime.Now.ToString("yyyyMMddhhmmss") + ")__" + did + "__" + (d + 1);
                    string rucsv = "/documentdownload" + uid + "/" + dnnn + ".csv";


                    string savecsv = System.Web.HttpContext.Current.Server.MapPath("/documentdownload" + uid + "/") + dnnn + ".csv";

                    string savexls = System.Web.HttpContext.Current.Server.MapPath("/documentdownload" + uid + "/") + dnnn + ".xls";

                    string outcsv = System.Web.HttpContext.Current.Server.MapPath("/documentdownload" + uid + "/") + dnnn + "_o.csv";
                    IWorkbook workbook_CSV = new XSSFWorkbook();
                    ISheet sheet_CSV = workbook_CSV.CreateSheet("Sheet1");

                    using (FileStream file = new FileStream(xlsmFilePath, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = WorkbookFactory.Create(file);
                        ISheet sheet = workbook.GetSheet(Tablename);





                        // 写入每行每列的值
                        for (int i = (sheet.FirstRowNum); i < rowindex; i++)
                        {
                            IRow row_XLSM = sheet.GetRow(i);
                            IRow row_ = sheet_CSV.CreateRow(i);

                            for (int j = 0; j < row_XLSM.LastCellNum; j++)
                            {
                                if (row_XLSM.GetCell(j) != null)
                                {
                                    string lm = (row_XLSM.GetCell(j).ToString());

                                    ICell cell = row_.CreateCell(j);

                                    cell.SetCellValue(lm);
                                }

                            }
                        }
                    }


                    //整理数据源
                    List<string[]> rows = new List<string[]>();
                    int star = d * pc;
                    int end = star + pc;
                    int hh = 0;
                    for (int r = star; r < end; r++)
                    {
                        IRow row_ = sheet_CSV.CreateRow(rowindex + hh);
                        DataRow item = dt.Rows[r];
                        string[] temp = new string[dt.Columns.Count];
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            ICell cell = row_.CreateCell(i);
                            string lm = item[i].ToString().Replace(",", "".Replace("\"", ""));

                            cell.SetCellValue(lm);




                        }
                        hh++;
                    }


                    using (FileStream file = new FileStream(savexls, FileMode.Create, FileAccess.Write))
                    {
                        workbook_CSV.Write(file,true);
                    }



                    Spire.Xls.Workbook workbooka = new Spire.Xls.Workbook();
                    workbooka.LoadFromFile(savexls);
                    Spire.Xls.Worksheet sheeta = workbooka.Worksheets[0];
                    sheeta.SaveToFile(savecsv, ",", Encoding.UTF8);



                    temppathSS[d] = rucsv;
                }
            }
            catch (Exception ex)
            {


            }
            return temppathSS;
        }

        public static string zystring(string rr)
        {
            return rr.Replace("\"", "&quot;").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace(" ", "&nbsp;").Replace("'", "&apos;");
        }
        public static string CJCSV(string[] head, System.Data.DataTable dt)
        {
            string temppath = "/documenttemp/" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";
            string savepath = System.Web.HttpContext.Current.Server.MapPath(temppath);
            try
            {


                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("/documenttemp/")))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("/documenttemp/"));
                }

                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Sheet1");

                List<string[]> rows = new List<string[]>();
                rows.Add(head);

                foreach (DataRow item in dt.Rows)
                {
                    string[] temp = new string[dt.Columns.Count];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        temp[i] = item[i].ToString();
                    }
                    rows.Add(temp);
                }
                int rownum = 0;
                foreach (var row in rows)
                {
                    IRow row_ = sheet.CreateRow(rownum++); // 创建新行并添加到sheet中
                    for (int i = 0; i < row.Length; i++) // 对于每行的每个元素
                    {
                        row_.CreateCell(i).SetCellValue(row[i]); // 创建单元格并设置值
                    }
                }


                using (FileStream file = new FileStream(savepath, FileMode.Create, FileAccess.Write)) // 指定文件名和打开模式
                {
                    workbook.Write(file,true);
                }
            }
            catch
            {

                temppath = "";
            }
            return temppath;
        }
        public static bool ReadExcelToDataTable(string filePath, out System.Data.DataTable dt, out ArrayList excelTitles, int startIndex = 1)
        {
            //excel内数据（无标题）
            dt = new System.Data.DataTable();
            //excel内标题
            excelTitles = new ArrayList();
            try
            {
                using (FileStream fsRead = File.OpenRead(filePath))
                {
                    IWorkbook wk = null;
                    //获取后缀名
                    string extension = filePath.Substring(filePath.LastIndexOf(".")).ToString().ToLower();
                    //判断是否是excel文件
                    if (extension == ".xlsx" || extension == ".xlsm")
                    {
                        try
                        {
                            wk = new XSSFWorkbook(fsRead);
                        }
                        catch (Exception error)
                        {
                            wk = new HSSFWorkbook(fsRead);
                        }

                        //获取第一个sheet TODO 若有多个Sheet这里需要循环
                        ISheet sheet = wk.GetSheetAt(3);
                        //获取第一行
                        IRow headrow = sheet.GetRow(4);

                        for (int i = headrow.FirstCellNum; i < headrow.Cells.Count; i++)
                        {
                            ICell cell = headrow.GetCell(i);
                            var tempV = GetCellValue(cell);
                            //创建列
                            DataColumn datacolum = new DataColumn(tempV);
                            dt.Columns.Add(datacolum);
                            //读取标题
                            if ((!string.IsNullOrEmpty(tempV)) && startIndex != 0)
                            {
                                excelTitles.Add(tempV);
                            }
                        }

                        ////读取每行
                        //for (int r = startIndex; r <= sheet.LastRowNum; r++)
                        //{
                        //    bool result = false;
                        //    DataRow dr = dt.NewRow();
                        //    //获取当前行
                        //    IRow row = sheet.GetRow(r);
                        //    //读取每列 采用headrow行数原因为，row.Cells.Count遇空会跳过
                        //    for (int j = 0; j < headrow.Cells.Count; j++)
                        //    {
                        //        //获取单元格的值
                        //        dr[j] = GetCellValue(row.GetCell(j));
                        //        //全为空则不取
                        //        if (dr[j].ToString() != "") result = true;
                        //    }
                        //    if (result == true)
                        //    {
                        //        dt.Rows.Add(dr); //把每行追加到DataTable
                        //    }
                        //}
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 读取Cell数据
        /// </summary>
        private static string GetCellValue(ICell cell)
        {
            if (cell == null) return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank: //空数据类型 这里类型注意一下，不同版本NPOI大小写可能不一样,有的版本是Blank（首字母大写)
                    return string.Empty;
                case CellType.Boolean: //bool类型
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric: //数字类型
                                       //日期类型 其它数字
                    return (DateUtil.IsCellDateFormatted(cell) ? cell.DateCellValue.ToString() : cell.NumericCellValue.ToString());
                case CellType.String: //string 类型
                    return cell.StringCellValue;
                case CellType.Formula: //带公式类型
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
                case CellType.Unknown: //无法识别类型
                default: //默认类型
                    return cell.ToString();//
            }
        }



        public static DropDownList getxl(DropDownList rr, string uid)
        {
            string aa = "";
            if (uid == "4")
            {
                aa = "无|标题|价格|月销量|总销量|评分|评价数量|类目|描述|主图网址|SKU名称|SKU图片网址|视频网址|SKUID|产品ID";
            }
            else
            {
                aa = "无|标题|产品价格|运费|运输方式|主图网址|SKU图网址|SKU名称|产品详情图|详细参数|开店时长|产品描述评分|沟通评分|运送速度评分|产品评分|评论数量|总销量|产品ID|SKUID";
            }
            int i = 0;
            foreach (string item in aa.Split('|'))
            {
                if (item != "")
                {
                    i++;
                    rr.Items.Add(new ListItem(item, item));
                }
            }
            return rr;
        }
        public static DropDownList getyjz(DropDownList rr, string uid)
        {


            rr.Items.Add(new ListItem("无", "无"));
            DataSet ds = access_sql.GreatDs("select * from ymb where yuid=" + uid + " order by yid");
            if (access_sql.yzTable(ds))
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    rr.Items.Add(new ListItem(item["yname"].ToString(), item["yid"].ToString()));
                }

            }

            return rr;
        }
        public static DropDownList getother(DropDownList rr, string uid)
        {


            rr.Items.Add(new ListItem("无", "无"));
            DataSet ds = access_sql.GreatDs("select * from otmb where ouid=" + uid + "  order by oid");
            if (access_sql.yzTable(ds))
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    rr.Items.Add(new ListItem(item["oname"].ToString(), item["oname"].ToString()));
                }

            }

            return rr;
        }
        public static bool yzkong(List<System.Web.UI.WebControls.TextBox> aa)
        {
            bool ru = true;
            foreach (System.Web.UI.WebControls.TextBox item in aa)
            {
                if (item.Text.Trim() == "")
                {
                    ru = false;
                    break;
                }
            }
            return ru;
        }
    }
}