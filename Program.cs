using MonoShapelib;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace S3CLook
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test1();
            //Test2();
            //Test3();
            Test4();
            //Test5();
        }

        /// <summary>
        /// 测试1
        /// </summary>
        private static void Test1()
        {
            double image_width = 6000;      // 照片宽度
            double image_height = 4000;     // 照片高度
            double foacl35 = 51.9404;       // 35mm等效焦距
            double omega = 180.0 - 175.5202967407976;
            double phi = 28.47765948934057;
            double kappa = -0.9386577561617924;
            double x = 344425.1889122094;
            double y = 2685902.18347278;
            double z = 293.6589041985571;


            Photo3D photo = new Photo3D(foacl35, image_width, image_height, omega, phi, kappa, 0, 0, z);
            Vector3[] result = photo.GetArea(88);


            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));               // 序号
            table.Columns.Add("NAME", typeof(string));          // 照片名称
            table.Columns.Add("PATH", typeof(string));          // 照片路径
            table.Columns.Add("IMGWIDTH", typeof(double));      // 照片宽度
            table.Columns.Add("IMGHEIGHT", typeof(double));     // 照片高度
            table.Columns.Add("FOACL35", typeof(double));       // 35mm等效焦距
            table.Columns.Add("OMEGA", typeof(double));         // X轴旋转
            table.Columns.Add("PHI", typeof(double));           // Y轴旋转
            table.Columns.Add("KAPPA", typeof(double));         // Z轴旋转
            table.Columns.Add("X", typeof(double));             // X坐标
            table.Columns.Add("Y", typeof(double));             // Y坐标
            table.Columns.Add("Z", typeof(double));             // Z坐标
            table.Columns.Add("SHAPE", typeof(SHPRecord));      // 图形

            // 构造图形
            SHPRecord shprecord = new SHPRecord() { ShapeType = SHPT.ARC };
            shprecord.Parts.Add(0);                         // 第一个分段 从0号点开始
            foreach (var item in result) {
                double xx = item.X + x;
                double yy = item.Y + y;
                double zz = item.Z;
                shprecord.Points.Add(new double[] { xx, yy, zz, 0 });
            }
            shprecord.Points.Add(shprecord.Points[0]);      // 闭合
            // 添加焦点连线 如果不想要注释掉就好
            double[] focus = new double[] { x, y, z, 0 };
            shprecord.Parts.Add(shprecord.Points.Count);
            shprecord.Points.Add(shprecord.Points[0]);
            shprecord.Points.Add(focus);
            shprecord.Parts.Add(shprecord.Points.Count);
            shprecord.Points.Add(shprecord.Points[1]);
            shprecord.Points.Add(focus);
            shprecord.Parts.Add(shprecord.Points.Count);
            shprecord.Points.Add(shprecord.Points[2]);
            shprecord.Points.Add(focus);
            shprecord.Parts.Add(shprecord.Points.Count);
            shprecord.Points.Add(shprecord.Points[3]);
            shprecord.Points.Add(focus);
            
            // 添加图形
            DataRow row = table.NewRow();
            row["ID"] = 0;
            row["NAME"] = "TEST.JPG";
            row["PATH"] = "./";
            row["IMGWIDTH"] = image_width;
            row["IMGHEIGHT"] = image_height;
            row["FOACL35"] = foacl35;
            row["OMEGA"] = omega;
            row["PHI"] = phi;
            row["KAPPA"] = kappa;
            row["X"] = x;
            row["Y"] = y;
            row["Z"] = z;
            row["SHAPE"] = shprecord;

            table.Rows.Add(row);
            
            bool success = ExportSHP("./data/test.shp", table, SHPT.ARCZ);
        }
        /// <summary>
        /// 测试2
        /// </summary>
        private static void Test2()
        {

            // 处理空中三角XML
            //ATXml atxml = new ATXml();
            //atxml.Load("../data/test1.xml");
            //atxml.SaveTiePoint();

            ATXml atxml = new ATXml();
            atxml.Load("../data/WGS84_UTM_49_32649.xml");
            DataTable photos = atxml.SavePhotos();

            //

            //double image_width = 6000;      // 照片宽度
            //double image_height = 4000;     // 照片高度
            //double foacl35 = 52.0943;       // 35mm等效焦距
            //double omega = 180.0 - 143.8773937335135;
            //double phi = -0.6382670099782131;
            //double kappa = 0.3455460181283488;
            //double x = 344339.9862200458;
            //double y = 2685756.97231997;
            //double z = 295.9032780230045;

            //double image_width = 6000;      // 照片宽度
            //double image_height = 4000;     // 照片高度
            //double foacl35 = 51.9404;       // 35mm等效焦距
            //double omega = 180.0 - 175.5202967407976;   // x
            //double phi = 28.47765948934057;             // y
            //double kappa = -0.9386577561617924;         // z
            //double x = 344425.1889122094;
            //double y = 2685902.18347278;
            //double z = 293.6589041985571;


            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));               // 序号
            table.Columns.Add("NAME", typeof(string));          // 照片名称
            table.Columns.Add("PATH", typeof(string));          // 照片路径
            table.Columns.Add("IMGWIDTH", typeof(double));      // 照片宽度
            table.Columns.Add("IMGHEIGHT", typeof(double));     // 照片高度
            table.Columns.Add("FOACL35", typeof(double));       // 35mm等效焦距
            table.Columns.Add("OMEGA", typeof(double));         // X轴旋转
            table.Columns.Add("PHI", typeof(double));           // Y轴旋转
            table.Columns.Add("KAPPA", typeof(double));         // Z轴旋转
            table.Columns.Add("X", typeof(double));             // X坐标
            table.Columns.Add("Y", typeof(double));             // Y坐标
            table.Columns.Add("Z", typeof(double));             // Z坐标
            table.Columns.Add("SHAPE", typeof(SHPRecord));      // 图形

            foreach (DataRow photorow in photos.Rows) {
                // 
                double image_width = 6000;      // 照片宽度
                double image_height = 4000;     // 照片高度
                double foacl35 = 52.0;          // 35mm等效焦距

                double omega = 0 - (double)photorow["omega"];  // x
                double phi = 0 - (double)photorow["phi"];          // y
                double kappa = (double)photorow["kappa"];      // z
                double x = (double)photorow["x"];
                double y = (double)photorow["y"];
                double z = (double)photorow["z"];
                string name = (string)photorow["name"];
                string path = (string)photorow["path"];
                int id = (int)photorow["id"];


                x = x - 344741;
                y = y - 2686064;


                Photo3D photo = new Photo3D(foacl35, image_width, image_height, omega, phi, kappa, 0, 0, z);
                Vector3[] result = photo.GetArea(87);

                if (result == null) continue;

                // 构造图形
                SHPRecord shprecord = new SHPRecord() { ShapeType = SHPT.ARC };
                shprecord.Parts.Add(0);                         // 第一个分段 从0号点开始
                foreach (var item in result) {
                    // 这里是引用类型不能把item的类型写出来
                    double xx = item.X + x;
                    double yy = item.Y + y;
                    double zz = item.Z;
                    shprecord.Points.Add(new double[] { xx, yy, zz, 0 });
                }
                shprecord.Points.Add(shprecord.Points[0]);      // 闭合
                // 添加焦点连线 如果不想要注释掉就好
                double[] focus = new double[] { x, y, z, 0 };
                shprecord.Parts.Add(shprecord.Points.Count);
                shprecord.Points.Add(shprecord.Points[0]);
                shprecord.Points.Add(focus);
                shprecord.Parts.Add(shprecord.Points.Count);
                shprecord.Points.Add(shprecord.Points[1]);
                shprecord.Points.Add(focus);
                shprecord.Parts.Add(shprecord.Points.Count);
                shprecord.Points.Add(shprecord.Points[2]);
                shprecord.Points.Add(focus);
                shprecord.Parts.Add(shprecord.Points.Count);
                shprecord.Points.Add(shprecord.Points[3]);
                shprecord.Points.Add(focus);

                // 添加图形
                DataRow row = table.NewRow();
                row["ID"] = id;
                row["NAME"] = name;
                row["PATH"] = path;
                row["IMGWIDTH"] = image_width;
                row["IMGHEIGHT"] = image_height;
                row["FOACL35"] = foacl35;
                row["OMEGA"] = omega;
                row["PHI"] = phi;
                row["KAPPA"] = kappa;
                row["X"] = x;
                row["Y"] = y;
                row["Z"] = z;
                row["SHAPE"] = shprecord;

                table.Rows.Add(row);
            }
            


            bool success = ExportSHP("./data/test.shp", table, SHPT.ARCZ);

        }
        /// <summary>
        /// 测试3
        /// </summary>
        private static void Test3()
        {

            // 处理空中三角XML
            //ATXml atxml = new ATXml();
            //atxml.Load("../data/test1.xml");
            //atxml.SaveTiePoint();


            DataTable photos = new DataTable();
            photos.Columns.Add("id", typeof(int));
            photos.Columns.Add("name", typeof(string));
            photos.Columns.Add("path", typeof(string));
            photos.Columns.Add("omega", typeof(double));
            photos.Columns.Add("phi", typeof(double));
            photos.Columns.Add("kappa", typeof(double));
            photos.Columns.Add("x", typeof(double));
            photos.Columns.Add("y", typeof(double));
            photos.Columns.Add("z", typeof(double));

            // omega
            for (int i = -180; i < 0; i += 5) {
                DataRow row = photos.NewRow();
                row["id"] = i;
                row["name"] = "OMEGA";
                row["path"] = "";
                row["omega"] = i;
                row["phi"] = 0;
                row["kappa"] = 0;
                row["x"] = 344425.1889122094;
                row["y"] = 2685902.18347278;
                row["z"] = 293.6589041985571;
                photos.Rows.Add(row);
            }
            // phi
            for (int i = -45; i < 45; i += 5) {
                DataRow row = photos.NewRow();
                row["id"] = i;
                row["name"] = "PHI";
                row["path"] = "";
                row["omega"] = -180;
                row["phi"] = i;
                row["kappa"] = 0;
                row["x"] = 344425.1889122094;
                row["y"] = 2685902.18347278;
                row["z"] = 293.6589041985571;
                photos.Rows.Add(row);
            }
            // kappa
            for (int i = -45; i < 45; i += 5) {
                DataRow row = photos.NewRow();
                row["id"] = i;
                row["name"] = "KAPPA";
                row["path"] = "";
                row["omega"] = -180;
                row["phi"] = 0;
                row["kappa"] = i;
                row["x"] = 344425.1889122094;
                row["y"] = 2685902.18347278;
                row["z"] = 293.6589041985571;
                photos.Rows.Add(row);
            }


            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));               // 序号
            table.Columns.Add("NAME", typeof(string));          // 照片名称
            table.Columns.Add("PATH", typeof(string));          // 照片路径
            table.Columns.Add("IMGWIDTH", typeof(double));      // 照片宽度
            table.Columns.Add("IMGHEIGHT", typeof(double));     // 照片高度
            table.Columns.Add("FOACL35", typeof(double));       // 35mm等效焦距
            table.Columns.Add("OMEGA", typeof(double));         // X轴旋转
            table.Columns.Add("PHI", typeof(double));           // Y轴旋转
            table.Columns.Add("KAPPA", typeof(double));         // Z轴旋转
            table.Columns.Add("X", typeof(double));             // X坐标
            table.Columns.Add("Y", typeof(double));             // Y坐标
            table.Columns.Add("Z", typeof(double));             // Z坐标
            table.Columns.Add("SHAPE", typeof(SHPRecord));      // 图形

            foreach (DataRow photorow in photos.Rows) {
                // 
                double image_width = 6000;      // 照片宽度
                double image_height = 4000;     // 照片高度
                double foacl35 = 52.0;          // 35mm等效焦距

                double omega = 180.0 - (double)photorow["omega"];  // x
                double phi = (double)photorow["phi"];          // y
                double kappa = (double)photorow["kappa"];      // z
                double x = (double)photorow["x"];
                double y = (double)photorow["y"];
                double z = (double)photorow["z"];
                string name = (string)photorow["name"];
                string path = (string)photorow["path"];
                int id = (int)photorow["id"];


                x = x - 344741;
                y = y - 2686064;


                Photo3D photo = new Photo3D(foacl35, image_width, image_height, omega, phi, kappa, 0, 0, z);
                Vector3[] result = photo.GetArea(87);

                if (result == null) continue;

                // 构造图形
                SHPRecord shprecord = new SHPRecord() { ShapeType = SHPT.ARC };
                shprecord.Parts.Add(0);                         // 第一个分段 从0号点开始
                foreach (var item in result) {
                    // 这里是引用类型不能把item的类型写出来
                    double xx = item.X + x;
                    double yy = item.Y + y;
                    double zz = item.Z;
                    shprecord.Points.Add(new double[] { xx, yy, zz, 0 });
                }
                shprecord.Points.Add(shprecord.Points[0]);      // 闭合
                // 添加焦点连线 如果不想要注释掉就好
                double[] focus = new double[] { x, y, z, 0 };
                shprecord.Parts.Add(shprecord.Points.Count);
                shprecord.Points.Add(shprecord.Points[0]);
                shprecord.Points.Add(focus);
                shprecord.Parts.Add(shprecord.Points.Count);
                shprecord.Points.Add(shprecord.Points[1]);
                shprecord.Points.Add(focus);
                shprecord.Parts.Add(shprecord.Points.Count);
                shprecord.Points.Add(shprecord.Points[2]);
                shprecord.Points.Add(focus);
                shprecord.Parts.Add(shprecord.Points.Count);
                shprecord.Points.Add(shprecord.Points[3]);
                shprecord.Points.Add(focus);

                // 添加图形
                DataRow row = table.NewRow();
                row["ID"] = id;
                row["NAME"] = name;
                row["PATH"] = path;
                row["IMGWIDTH"] = image_width;
                row["IMGHEIGHT"] = image_height;
                row["FOACL35"] = foacl35;
                row["OMEGA"] = omega;
                row["PHI"] = phi;
                row["KAPPA"] = kappa;
                row["X"] = x;
                row["Y"] = y;
                row["Z"] = z;
                row["SHAPE"] = shprecord;

                table.Rows.Add(row);
            }
            


            bool success = ExportSHP("./data/test.shp", table, SHPT.ARCZ);

        }
        /// <summary>
        /// 测试4
        /// </summary>
        private static void Test4()
        {
            // 处理空中三角XML
            //ATXml atxml = new ATXml();
            //atxml.Load("../data/test1.xml");
            //atxml.SaveTiePoint();

            ATXml atxml = new ATXml();
            //atxml.Load("../data/LIANHUASHAN_2_XIAN80.xml");
            atxml.Load("../data/WGS84_UTM_49_32649.xml");
            DataTable photos = atxml.SavePhotos();

            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));               // 序号
            table.Columns.Add("NAME", typeof(string));          // 照片名称
            table.Columns.Add("PATH", typeof(string));          // 照片路径
            table.Columns.Add("IMGWIDTH", typeof(double));      // 照片宽度
            table.Columns.Add("IMGHEIGHT", typeof(double));     // 照片高度
            table.Columns.Add("FOACL35", typeof(double));       // 35mm等效焦距
            table.Columns.Add("OMEGA", typeof(double));         // X轴旋转
            table.Columns.Add("PHI", typeof(double));           // Y轴旋转
            table.Columns.Add("KAPPA", typeof(double));         // Z轴旋转
            table.Columns.Add("X", typeof(double));             // X坐标
            table.Columns.Add("Y", typeof(double));             // Y坐标
            table.Columns.Add("Z", typeof(double));             // Z坐标
            table.Columns.Add("SHAPE", typeof(SHPRecord));      // 图形

            foreach (DataRow photorow in photos.Rows) {
                // 
                double image_width = 6000;      // 照片宽度
                double image_height = 4000;     // 照片高度
                double foacl35 = 52.0;          // 35mm等效焦距

                double omega = 180 + (double)photorow["omega"];     // x
                double phi = 0 - (double)photorow["phi"];          // y
                double kappa = 0 - (double)photorow["kappa"];      // z
                double x = (double)photorow["x"];
                double y = (double)photorow["y"];
                double z = (double)photorow["z"];
                string name = (string)photorow["name"];
                string path = (string)photorow["path"];
                int id = (int)photorow["id"];

                //if (Math.Abs(omega) < 179 ||
                //    Math.Abs(phi) > 1) continue;

                //if (name != "DSC00538.JPG") continue;

                

                Photo3D photo = new Photo3D(foacl35, image_width, image_height, omega, phi, kappa, 0, 0, z);
                Vector3[] result = photo.GetArea(0);

                if (result == null) continue;

                // 构造图形
                SHPRecord shprecord = new SHPRecord() { ShapeType = SHPT.ARC };
                shprecord.Parts.Add(0);                         // 第一个分段 从0号点开始
                foreach (var item in result) {
                    // 这里是引用类型不能把item的类型写出来
                    double xx = item.X + x;
                    double yy = item.Y + y;
                    double zz = item.Z;
                    shprecord.Points.Add(new double[] { xx, yy, zz, 0 });
                }
                shprecord.Points.Add(shprecord.Points[0]);      // 闭合
                // 添加焦点连线 如果不想要注释掉就好
                double[] focus = new double[] { x, y, z, 0 };
                shprecord.Parts.Add(shprecord.Points.Count);
                shprecord.Points.Add(shprecord.Points[0]);
                shprecord.Points.Add(focus);
                shprecord.Parts.Add(shprecord.Points.Count);
                shprecord.Points.Add(shprecord.Points[1]);
                shprecord.Points.Add(focus);
                shprecord.Parts.Add(shprecord.Points.Count);
                shprecord.Points.Add(shprecord.Points[2]);
                shprecord.Points.Add(focus);
                shprecord.Parts.Add(shprecord.Points.Count);
                shprecord.Points.Add(shprecord.Points[3]);
                shprecord.Points.Add(focus);

                // 添加图形
                DataRow row = table.NewRow();
                row["ID"] = id;
                row["NAME"] = name;
                row["PATH"] = path;
                row["IMGWIDTH"] = image_width;
                row["IMGHEIGHT"] = image_height;
                row["FOACL35"] = foacl35;
                row["OMEGA"] = omega;
                row["PHI"] = phi;
                row["KAPPA"] = kappa;
                row["X"] = x;
                row["Y"] = y;
                row["Z"] = z;
                row["SHAPE"] = shprecord;

                table.Rows.Add(row);
            }
            


            bool success = ExportSHP("./data/test.shp", table, SHPT.ARCZ);

        }
        /// <summary>
        /// 测试5
        /// </summary>
        private static void Test5()
        {
            DataTable photos = new DataTable();
            photos.Columns.Add("id", typeof(int));
            photos.Columns.Add("name", typeof(string));
            photos.Columns.Add("path", typeof(string));
            photos.Columns.Add("omega", typeof(double));
            photos.Columns.Add("phi", typeof(double));
            photos.Columns.Add("kappa", typeof(double));
            photos.Columns.Add("x", typeof(double));
            photos.Columns.Add("y", typeof(double));
            photos.Columns.Add("z", typeof(double));


            //double oo = -161.71783395000455;
            //double pp = 18.987139550088642;
            //double kk = 111.29725261667032;
            //double xx = 99404.530169771184;
            //double yy = 2702586.2241647677;
            //double zz = 348.57052615936846;

            double oo = 148.20431305857122;
            double pp = 4.6638374299675993;
            double kk = -106.68806535618991;
            double xx = 98672.510971207768;
            double yy = 2701719.236410012;
            double zz = 345.28511809650809;

            photos.Rows.Add(new object[] {
                1, "", "",
                180 - oo,
                pp,
                kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                2, "", "",
                180 - oo,
                -pp,
                kk,
                 xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                3, "", "",
                180 - oo,
                pp,
                -kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                4, "", "",
                180 - oo,
                -pp,
                -kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                5, "", "",
                180 - oo,
                180 - pp,
                kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                6, "", "",
                180 - oo,
                pp,
                180 - kk,
                 xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                7, "", "",
                180 - oo,
                180 - pp,
                -kk,
                 xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                8, "", "",
                180 - oo,
                -pp,
                180 - kk,
                xx, yy, zz
            });
            ////////
            photos.Rows.Add(new object[] {
                11, "", "",
                oo,
                pp,
                kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                22, "", "",
                oo,
                -pp,
                kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                33, "", "",
                oo,
                pp,
                -kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                44, "", "",
                oo,
                -pp,
                -kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                55, "", "",
                oo,
                180 - pp,
                kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                66, "", "",
                oo,
                pp,
                180 - kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                77, "", "",
                oo,
                180 - pp,
                -kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                88, "", "",
                oo,
                -pp,
                180 - kk,
                xx, yy, zz
            });
            ////////
            photos.Rows.Add(new object[] {
                111, "", "",
                -oo,
                pp,
                kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                222, "", "",
                -oo,
                -pp,
                kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                333, "", "",
                -oo,
                pp,
                -kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                444, "", "",
                -oo,
                -pp,
                -kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                555, "", "",
                -oo,
                180 - pp,
                kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                666, "", "",
                -oo,
                pp,
                180 - kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                777, "", "",
                -oo,
                180 - pp,
                -kk,
                xx, yy, zz
            });
            photos.Rows.Add(new object[] {
                888, "", "",
                -oo,
                -pp,
                180 - kk,
                xx, yy, zz
            });

            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));               // 序号
            table.Columns.Add("NAME", typeof(string));          // 照片名称
            table.Columns.Add("PATH", typeof(string));          // 照片路径
            table.Columns.Add("IMGWIDTH", typeof(double));      // 照片宽度
            table.Columns.Add("IMGHEIGHT", typeof(double));     // 照片高度
            table.Columns.Add("FOACL35", typeof(double));       // 35mm等效焦距
            table.Columns.Add("OMEGA", typeof(double));         // X轴旋转
            table.Columns.Add("PHI", typeof(double));           // Y轴旋转
            table.Columns.Add("KAPPA", typeof(double));         // Z轴旋转
            table.Columns.Add("X", typeof(double));             // X坐标
            table.Columns.Add("Y", typeof(double));             // Y坐标
            table.Columns.Add("Z", typeof(double));             // Z坐标
            table.Columns.Add("SHAPE", typeof(SHPRecord));      // 图形

            foreach (DataRow photorow in photos.Rows) {
                // 
                double image_width = 6000;      // 照片宽度
                double image_height = 4000;     // 照片高度
                double foacl35 = 52.0;          // 35mm等效焦距

                double omega = 180.0 - (double)photorow["omega"];  // x
                double phi = (double)photorow["phi"];          // y
                double kappa = (double)photorow["kappa"];      // z
                double x = (double)photorow["x"];
                double y = (double)photorow["y"];
                double z = (double)photorow["z"];
                string name = (string)photorow["name"];
                string path = (string)photorow["path"];
                int id = (int)photorow["id"];

                Photo3D photo = new Photo3D(foacl35, image_width, image_height, omega, phi, kappa, 0, 0, z);
                Vector3[] result = photo.GetArea(0);

                if (result == null) continue;

                // 构造图形
                SHPRecord shprecord = new SHPRecord() { ShapeType = SHPT.ARC };
                shprecord.Parts.Add(0);                         // 第一个分段 从0号点开始
                foreach (var item in result) {
                    // 这里是引用类型不能把item的类型写出来
                    double xxx = item.X + x;
                    double yyy = item.Y + y;
                    double zzz = item.Z;
                    shprecord.Points.Add(new double[] { xxx, yyy, zzz, 0 });
                }
                shprecord.Points.Add(shprecord.Points[0]);      // 闭合
                // 添加焦点连线 如果不想要注释掉就好
                double[] focus = new double[] { x, y, z, 0 };
                shprecord.Parts.Add(shprecord.Points.Count);
                shprecord.Points.Add(shprecord.Points[0]);
                shprecord.Points.Add(focus);
                shprecord.Parts.Add(shprecord.Points.Count);
                shprecord.Points.Add(shprecord.Points[1]);
                shprecord.Points.Add(focus);
                shprecord.Parts.Add(shprecord.Points.Count);
                shprecord.Points.Add(shprecord.Points[2]);
                shprecord.Points.Add(focus);
                shprecord.Parts.Add(shprecord.Points.Count);
                shprecord.Points.Add(shprecord.Points[3]);
                shprecord.Points.Add(focus);

                // 添加图形
                DataRow row = table.NewRow();
                row["ID"] = id;
                row["NAME"] = name;
                row["PATH"] = path;
                row["IMGWIDTH"] = image_width;
                row["IMGHEIGHT"] = image_height;
                row["FOACL35"] = foacl35;
                row["OMEGA"] = omega;
                row["PHI"] = phi;
                row["KAPPA"] = kappa;
                row["X"] = x;
                row["Y"] = y;
                row["Z"] = z;
                row["SHAPE"] = shprecord;

                table.Rows.Add(row);
            }
            


            bool success = ExportSHP("./data/test.shp", table, SHPT.ARCZ);

        }

        /// <summary>
        /// 导出SHP数据
        /// </summary>
        /// <param name="nameWithoutExt">SHP文件名 不包含扩展名</param>
        /// <param name="table">属性表</param>
        /// <param name="shptype">SHP类别</param>
        /// <returns></returns>
        private static bool ExportSHP(string nameWithoutExt, DataTable table, SHPT shptype)
        {
            // TABLE里包含SHPRecord字段
            // 创建SHP文件
            // 创建DBF文件
            // 写入图形
            // 写入属性
            try {
                // 1.创建SHP文件
                SHPHandle hSHP = SHPHandle.Create(nameWithoutExt, shptype);
                if (hSHP == null) throw new Exception("Unable to create SHP:" + nameWithoutExt);
                // 2.创建DBF文件
                DBFHandle hDBF = DBFHandle.Create(nameWithoutExt);
                if (hDBF == null) throw new Exception("Unable to create DBF:" + nameWithoutExt);
                string shapeField = "";
                foreach (DataColumn column in table.Columns) {
                    if (column.DataType == typeof(string)) {
                        if (hDBF.AddField(column.ColumnName, FT.String, 50, 0) < 0)
                            throw new Exception("DBFHandle.AddField(" + column.ColumnName + ",FTString,50,0) failed.");
                    }
                    else if (column.DataType == typeof(int)) {
                        if (hDBF.AddField(column.ColumnName, FT.Double, 8, 0) < 0)
                            throw new Exception("DBFHandle.AddField(" + column.ColumnName + ",Integer,8,0) failed.");
                    }
                    else if (column.DataType == typeof(double)) {
                        if (hDBF.AddField(column.ColumnName, FT.Double, 16, 8) < 0)
                            throw new Exception("DBFHandle.AddField(" + column.ColumnName + ",Double,16,8) failed.");
                    }
                    else if (column.DataType == typeof(DateTime)) {
                        // 不支持
                        //if (hDBF.AddField(column.ColumnName, FT.Logical, 16, 0) < 0)
                        //    throw new Exception("DBFHandle.AddField(" + column.ColumnName + ",Double,16,0) failed.");
                    }
                    else if (column.DataType == typeof(SHPRecord)) {
                        // 图形字段
                        shapeField = column.ColumnName;
                    }
                }
                if (shapeField == "") throw new Exception("Can not found shape field.");
                // 3.写数据
                int record_index = 0;
                foreach (DataRow row in table.Rows) {
                    // 处理图形
                    SHPRecord record = row[shapeField] as SHPRecord;
                    int num_parts = record.NumberOfParts;       // 总共分为几段
                    int[] parts = record.Parts.ToArray();       // 每一个分段的起始节点索引
                    int num_points = record.NumberOfPoints;     // 所有节点总数
                    double[] xs = new double[num_points];       // 所有节点X坐标
                    double[] ys = new double[num_points];       // 所有节点Y坐标
                    double[] zs = new double[num_points];       // 所有节点Z坐标
                    double[] ms = new double[num_points];       // 所有节点M坐标
                    for (int i = 0; i < num_points; i++) {
                        xs[i] = record.Points[i][0];            // X坐标
                        ys[i] = record.Points[i][1];            // Y坐标
                        zs[i] = record.Points[i][2];            // Z值
                        ms[i] = record.Points[i][3];            // M值
                    }
                    // 
                    int field_index = 0;
                    foreach (DataColumn column in table.Columns) {
                        object val = row[column];
                        if (val == null || Convert.IsDBNull(val)) {
                            hDBF.WriteNULLAttribute(record_index, field_index);
                            field_index++;
                        }
                        else if (column.DataType == typeof(string)) {
                            hDBF.WriteStringAttribute(record_index, field_index, Convert.ToString(val));
                            field_index++;
                        }
                        else if (column.DataType == typeof(int)) {
                            hDBF.WriteDoubleAttribute(record_index, field_index, Convert.ToInt32(val));
                            field_index++;
                        }
                        else if (column.DataType == typeof(double)) {
                            hDBF.WriteDoubleAttribute(record_index, field_index, Convert.ToDouble(val));
                            field_index++;
                        }
                        else if (column.DataType == typeof(DateTime)) {
                            // 不支持
                        }
                    }
                    // PS: 节点 "逆时针"是加 "顺时针"是减
                    SHPObject shpobj = SHPObject.Create(shptype,    // 图形类别
                                                        -1,         // 图形ID -1表示新增
                                                        num_parts,  // 总共分为几段
                                                        parts,      // 每一个分段的起始节点索引
                                                        null,       // 每段的类别
                                                        num_points, // 所有节点总数
                                                        xs,         // 所有节点的X坐标
                                                        ys,         // 所有节点的Y坐标
                                                        zs,         // 所有节点的Z值
                                                        ms);        // 所有节点的M值
                    hSHP.WriteObject(-1, shpobj);
                    record_index++;
                }

                hDBF.Close();
                hSHP.Close();
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }
        /// <summary>
        /// 导入SHP数据
        /// </summary>
        /// <param name="nameWithoutExt"></param>
        /// <returns></returns>
        private static bool ImportSHP(string nameWithoutExt)
        {
            // Open the passed shapefile.
            SHPHandle hSHP = SHPHandle.Open(nameWithoutExt, "rb");
            int nEntities;
            SHPT nShapeType;
            double[] adfMinBound = new double[4];
            double[] adfMaxBound = new double[4];
            hSHP.GetInfo(out nEntities, out nShapeType, adfMinBound, adfMaxBound);

            for (int i = 0; i < nEntities; i++) {
                //if (i != 0) continue;
                //List<PointLatLng> points = new List<PointLatLng>();
                SHPObject psShape = hSHP.ReadObject(i);
                for (int n = 0; n < psShape.nVertices; n++) {
                    double lat = psShape.padfY[n];
                    double lng = psShape.padfX[n];
                    //points.Add(new PointLatLng(lat, lng));
                    Console.WriteLine("{0}, {1}", lat, lng);
                }

            }
            return true;
        }



    }
}
