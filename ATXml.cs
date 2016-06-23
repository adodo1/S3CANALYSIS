using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace S3CLook
{
    // 解析空中三角的XML
    public class ATXml
    {
        private XmlDocument _doc = null;
        private string _file = null;
        private string _xmlstr = null;


        /// <summary>
        /// 加载XML文件
        /// </summary>
        /// <param name="file"></param>
        public void Load(string file)
        {
            _file = file;
            _doc = new XmlDocument();
            _doc.Load(file);
            using (StreamReader reader = new StreamReader(file)) {
                StringBuilder builder = new StringBuilder();
                int size = 4096;
                int index = 0;
                int num = 0;
                do {
                    char[] buffer = new char[size];
                    num = reader.ReadBlock(buffer, 0, size);
                    index += num;
                    builder.Append(buffer);
                } while (num > 0);
                _xmlstr = builder.ToString();
                reader.Close();
            }
        }
        /// <summary>
        /// 保存所有同名点文件
        /// </summary>
        public void SaveTiePoint()
        {
            // 能解析但是太慢了 必须动用正则表达式
            //string result = "ID\tX\tY\tZ\r\n";
            //int num = 0;
            //XmlNodeList xnl = _doc.SelectNodes("/BlocksExchange/Block/TiePoints");
            //foreach (XmlNode tiePointNode in xnl[0]) {
            //    XmlNode position = tiePointNode["Position"];
            //    string x = position["x"].InnerText;
            //    string y = position["y"].InnerText;
            //    string z = position["z"].InnerText;
            //    result += string.Format("{0}\t{1}\t{2}\t{3}\r\n", num++, x, y, z);
            //}
            //// 保存
            //string path = Path.GetDirectoryName(_file);
            //string name = Path.GetFileNameWithoutExtension(_file);
            //string file = path + "\\" + name + ".tiepoints.txt";
            //using (StreamWriter writer = new StreamWriter(file)) {
            //    writer.Write(result);
            //    writer.Flush();
            //    writer.Close();
            //}


        }

    }
}
