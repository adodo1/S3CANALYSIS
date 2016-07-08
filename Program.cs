using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S3CLook
{
    class Program
    {
        static void Main(string[] args)
        {

            // 处理空中三角XML
            //ATXml atxml = new ATXml();
            //atxml.Load("../data/test1.xml");
            //atxml.SaveTiePoint();

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

            double image_width = 6000;      // 照片宽度
            double image_height = 4000;     // 照片高度
            double foacl35 = 51.9404;       // 35mm等效焦距
            double omega = 180.0 - 175.5202967407976;
            double phi = -28.47765948934057;
            double kappa = 0.9386577561617924;
            double x = 344425.1889122094;
            double y = 2685902.18347278;
            double z = 293.6589041985571;


            Photo3D photo = new Photo3D(foacl35, image_width, image_height, omega, phi, kappa, 0, 0, z);
            Vector3[] result = photo.GetArea(88);

            string text = "";
            foreach (var item in result) {
                text += string.Format("X:{0} Y:{1} Z:{2}\r\n", item.X + x, item.Y + y, item.Z);

            }

        }
    }
}
