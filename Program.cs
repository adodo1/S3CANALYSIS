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
            //atxml.Load("../data/test2.xml");
            //atxml.SaveTiePoint();

            Photo3D photo = new Photo3D(24, 36, 24, 0, 30, 0, 0, 0, 100);
            Vector3[] result = photo.GetArea(0);

        }
    }
}
