using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

namespace WebApplication11
{
    public class CSVReader : IEnumerable
    {

        public CSVReader() { }
        bool HasTitle = false;
        string[] mTitles;
        List<string[]> mLines = new List<string[]>();

        /// <summary>
        /// 读取CSV文件
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <param name="hasTitle">是否有标题行，若为True 则将第一行作为标题行</param>
        public List<string[]> Reader(string file, bool hasTitle = false)
        {
            HasTitle = hasTitle;
            return Parse(File.ReadAllText(file, Encoding.UTF8));
        }


        /// <summary>
        /// CSV文件解析
        /// </summary>
        /// <param name="cvsLineTexts">All Lines</param>
        public List<string[]> Parse(string csvText)
        {
            Clear();
            bool flag = false;//判断是否为内容的一部分
            string buffer = string.Empty;//文本缓存
            int k = 0;
            bool isDescript = false;
            //CSVLine line = new CSVLine();
            List<string> values = new List<string>();//列数据集合

            while (k < csvText.Length)
            {
                char c = csvText[k];
                switch (c)
                {
                    case '#':
                        if (flag)
                            goto Tip_ADD;
                        if (values.Count == 0 && buffer == string.Empty)
                        {
                            isDescript = true;
                            break;
                        }
                        goto Tip_ADD;
                    case '"':
                        if (isDescript)
                            break;
                        flag ^= true; //对flag进行异或运算  flag = !flag
                        goto Tip_ADD;
                    case ',':
                        if (isDescript)
                            break;
                        if (flag)
                        {
                            //当前逗号为内容部分，不执行分割功能
                            goto Tip_ADD;
                        }
                        //逗号为分隔符
                        buffer = buffer.Trim();
                        if (buffer.StartsWith("\"") && buffer.EndsWith("\""))
                        {
                            //消除文本中多余的双引号
                            buffer = buffer.Substring(1, buffer.Length - 2);
                            buffer = buffer.Replace("\"\"", "\"");
                        }
                        //添加到列集合
                        values.Add(buffer.Trim());
                        //清空缓存字符串
                        buffer = string.Empty;
                        break;
                    case '\n':

                        if (flag)
                        {
                            //换行符
                            goto Tip_ADD;
                        }
                        //行尾结束标识
                        if (isDescript)
                        {
                            isDescript = false;
                            break;
                        }
                        buffer = buffer.Trim();

                        if (buffer.StartsWith("\"") && buffer.EndsWith("\""))
                        {
                            buffer = buffer.Substring(1, buffer.Length - 2);
                            buffer = buffer.Replace("\"\"", "\"");
                        }
                        if (!string.IsNullOrWhiteSpace(buffer.Trim()))
                        {
                            values.Add(buffer);
                        }
                        //添加行数据
                        if (values.Count > 0)
                        {
                            mLines.Add(values.ToArray());
                        }
                        values.Clear();
                    tip_end:
                        buffer = string.Empty;
                        break;
                    default:
                        if (isDescript)
                            break;
                        Tip_ADD:
                        buffer += c;
                        break;
                }
                k++;
                if (k == csvText.Length && !isDescript)
                {
                    //最后一行数据
                    buffer = buffer.Trim();
                    if (buffer.StartsWith("\"") && buffer.EndsWith("\""))
                    {
                        buffer = buffer.Substring(1, buffer.Length - 2);
                        buffer = buffer.Replace("\"\"", "\"");
                    }
                    if (!string.IsNullOrWhiteSpace(buffer.Trim()))
                    {
                        values.Add(buffer);
                    }
                    if (values.Count > 0)
                    {
                        mLines.Add(values.ToArray());
                    }
                }
            }

            if (HasTitle)
            {
                //移除标题行
                mTitles = mLines[0];
                mLines.RemoveAt(0);
            }
            return mLines;
        }

        /// <summary>
        /// 清空内容
        /// </summary>
        public void Clear()
        {
            mLines.Clear();
        }

        /// <summary>
        /// 获取对应行的内容数组
        /// </summary>
        /// <param name="row">行下标</param>
        /// <returns></returns>
        public string[] this[int row]
        {
            get
            {
                return mLines[row];
            }
        }

        /// <summary>
        /// 获取对应列的数据集合
        /// </summary>
        /// <param name="column">列下标</param>
        /// <returns></returns>
        public string[] GetColumns(int column)
        {
            List<string> list = new List<string>();
            //返回对应列的所有数据集合
            for (int i = 0; i < mLines.Count; i++)
            {
                list.Add(mLines[i][column]);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 根据列名获取对应列的内容数组
        /// </summary>
        /// <param name="column">列名</param>
        /// <returns></returns>
        public string[] GetColumns(string column)
        {
            List<string> list = new List<string>();
            int col = -1;
            //遍历标题数组，找到名称对应的数组下标
            for (int i = 0; i < mTitles.Length; i++)
            {
                if (mTitles[i] == column)
                {
                    col = i;
                    break;
                }
            }
            if (col == -1)
                throw new Exception("无效列名");
            //返回对应列的所有数据集合
            for (int i = 0; i < mLines.Count; i++)
            {
                list.Add(mLines[i][col]);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 行数
        /// </summary>
        public int RowCount
        {
            get
            {
                return mLines.Count;
            }
        }

        /// <summary>
        /// 获取对应列名
        /// </summary>
        /// <param name="colidx">列下标</param>
        /// <returns></returns>
        public string GetTitle(int colidx)
        {
            if (colidx >= mTitles.Length)
                throw new IndexOutOfRangeException();
            return mTitles[colidx];
        }

        /// <summary>
        /// 根据行列坐标查找内容
        /// </summary>
        /// <param name="rowidx">行下标</param>
        /// <param name="colIndex">列下标</param>
        /// <returns></returns>
        public string GetValue(int rowidx, int colIndex)
        {
            if (rowidx >= mLines.Count)
                throw new IndexOutOfRangeException("rowidx 超出数组长度");
            if (colIndex >= mLines[rowidx].Length)
                throw new IndexOutOfRangeException("colIndex 超出数组长度");
            return mLines[rowidx][colIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            foreach (string[] N in this.mLines)
            {
                yield return N;
            }
            yield break;
        }
    }
}


