using SharpDX;
using System;
using System.Collections.Generic;
using System.Data;
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
        /// <summary>
        /// 保存所有照片数据
        /// </summary>
        public DataTable SavePhotos()
        {
            DataTable result = new DataTable();
            result.Columns.Add("id", typeof(int));
            result.Columns.Add("name", typeof(string));
            result.Columns.Add("path", typeof(string));
            result.Columns.Add("omega", typeof(double));
            result.Columns.Add("phi", typeof(double));
            result.Columns.Add("kappa", typeof(double));
            result.Columns.Add("x", typeof(double));
            result.Columns.Add("y", typeof(double));
            result.Columns.Add("z", typeof(double));

            using (XmlReader reader = XmlReader.Create(_file)) {
                reader.MoveToContent();
                while (reader.Read()) {
                    if (reader.NodeType == XmlNodeType.Element) {

                        if (reader.Name == "Photo") {
                            // 单张照片
                            //for (int i = 0; i < 50; i++) {
                            //    Console.WriteLine("{0}[{1}]: {2} {3}", i, reader.Name, reader.NodeType, reader.Value);
                            //    reader.Read();
                            //}

                            string id = "";
                            string file = "";
                            string omega = "";
                            string phi = "";
                            string kappa = "";
                            string x = "";
                            string y = "";
                            string z = "";

                            while (reader.Read()) {
                                if (reader.Name == "Photo") break;
                                else if (reader.NodeType != XmlNodeType.Element &&
                                         reader.NodeType != XmlNodeType.Text) {
                                    // 
                                    continue;
                                }
                                else if (reader.Name == "Id") {
                                    reader.Read();
                                    id = reader.Value;
                                }
                                else if (reader.Name == "ImagePath") {
                                    reader.Read();
                                    file = reader.Value;
                                }
                                else if (reader.Name == "Omega") {
                                    reader.Read();
                                    omega = reader.Value;
                                }
                                else if (reader.Name == "Phi") {
                                    reader.Read();
                                    phi = reader.Value;
                                }
                                else if (reader.Name == "Kappa") {
                                    reader.Read();
                                    kappa = reader.Value;
                                }
                                else if (reader.Name == "x") {
                                    reader.Read();
                                    x = reader.Value;
                                }
                                else if (reader.Name == "y") {
                                    reader.Read();
                                    y = reader.Value;
                                }
                                else if (reader.Name == "z") {
                                    reader.Read();
                                    z = reader.Value;
                                }
                            }
                            if (x == "" || y == "" || z == "") continue;


                            DataRow row = result.NewRow();
                            row["id"] = int.Parse(id);
                            row["name"] = Path.GetFileName(file);
                            row["path"] = Path.GetDirectoryName(file);
                            row["omega"] = double.Parse(omega);
                            row["phi"] = double.Parse(phi);
                            row["kappa"] = double.Parse(kappa);
                            row["x"] = double.Parse(x);
                            row["y"] = double.Parse(y);
                            row["z"] = double.Parse(z);
                            result.Rows.Add(row);
                        }
                    }
                }
            }
            return result;
        }

    }
}

