using SharpDX;
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





            // 大文件处理参考
            // 关键字 SetProcessWorkingSetSize 内存映射将文件分块 BufferStream MemoryMappedFile 超大 XML
            // http://zhidao.baidu.com/link?url=rkHCtOXUCVjZsyH-K9fKlIpVfGjdQdUkUqK5RDSMbccuOucgcIx61ukOcDV2ThN_lmHHXjIrgw6Ip0BV-xkBbVMS19HBF7PGAGJ16BPaZOa
            // http://q.cnblogs.com/q/52734/

        }
        /// <summary>
        /// 保存所有同名点文件
        /// </summary>
        public void SaveTiePoint()
        {

            // 能解析但是太慢了 必须动用正则表达式
            //_doc = new XmlDocument();
            //_doc.Load(file);
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


            Matrix mx = new Matrix();
            //Matrix.PerspectiveFovLH(

            StringBuilder buffer = new StringBuilder();
            buffer.Append("ID\tX\tY\tZ\r\n");
            int num = 0;
            using (XmlReader reader = XmlReader.Create(_file)) {
                reader.MoveToContent();
                while (reader.Read()) {
                    if (reader.NodeType == XmlNodeType.Element) {

                        if (reader.Name == "TiePoints") {
                            // 所有同名点
                        }
                        else if (reader.Name == "TiePoint") {
                            // 单个同名点
                            reader.Read();      // Whitespace
                            reader.Read();      // Position 节点
                            reader.Read();      // Whitespace
                            reader.Read();      // x 节点
                            reader.Read();      // text 文本
                            string x = reader.Value;
                            reader.Read();      // EndElement
                            reader.Read();      // Whitespace
                            reader.Read();      // y 节点
                            reader.Read();      // text 文本
                            string y = reader.Value;
                            reader.Read();      // EndElement
                            reader.Read();      // 空白节点
                            reader.Read();      // z 节点
                            reader.Read();      // text 文本
                            string z = reader.Value;
                            buffer.AppendFormat("{0}\t{1}\t{2}\t{3}\r\n", num++, x, y, z);
                        }

                        //var block = XElement.ReadFrom(reader) as XElement;
                        
                    }
                }
            }

            // 保存
            string path = Path.GetDirectoryName(_file);
            string name = Path.GetFileNameWithoutExtension(_file);
            string file = path + "\\" + name + ".tiepoints.txt";
            using (StreamWriter writer = new StreamWriter(file)) {
                writer.Write(buffer.ToString());
                writer.Flush();
                writer.Close();
            }

            

        }

    }
}

