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
            ATXml atxml = new ATXml();
            atxml.Load("../data/test.xml");
            atxml.SaveTiePoint();

        }
    }
}
