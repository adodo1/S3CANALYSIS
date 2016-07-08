using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S3CLook
{
    public class Photo3D
    {
        // 相关资料:
        // http://blog.sina.com.cn/s/blog_548f1a050100utef.html
        // http://wenku.baidu.com/link?url=if5-lcvx7ky5EXaUu8k_4IBlhauvjC08y3PeaqWp31CUBtNDfA_MpOq2Lx6SYktyZHP7oC2wH2uhVJN9zOQI7-sh8flgeIDUEYmrkFRrdoy
        // 1台相机传感器尺寸是不变的,但是可以选择照片的比例
        // 照片比例 3:2 / 16:9
        // 16:9 的时候上下被裁减了 但是宽度是一样的
        // 照片的比例最好跟传感器长宽比一致 否则计算是有误的
        // 简单的说就是别整这么多幺蛾子 老老实实来
        // 画幅根据对角线长度来计算
        // ---------------------------------------------------
        // 格式          长度（mm） 宽度（mm） 对角线 焦距系数
        // ---------------------------------------------------
        // 全画幅        36         24         43.27  1
        // 佳能APS-H     27.9       18.6       33.53  1.3
        // 尼康APS-C     23.6       15.8       28.4   1.5
        // 佳能APS-C     22.3       14.9       26.82  1.6
        // 佳能1.5英寸*1 18.7       14         23.36  1.85
        // 4/3系统*2     17.3       13         21.64  2
        // 尼康 1系列*3  13.2       8.8        15.86  2.73
        // 1/1.7英寸     7.76       5.82       9.7    4.46
        // 1/1.8英寸     7.2        5.3        8.94   4.84
        // 1/2.0英寸     6.4        4.8        8      5.41
        // 1/2.3英寸     6.16       4.62       7.7    5.62
        // 1/2.5英寸     5.76       4.29       7.18   6.02
        // 1/2.7英寸     5.3        4          6.64   6.52
        // ---------------------------------------------------

        // S3C里默认的35mm焦距对应的传感器宽度为35mm 
        // 但是标准的35mm焦距对应的传感器宽度为36mm
        // 本程序安装标准的来认定传感器宽度为36mm
        // 为了保证精度 程序运算使用单位统一为毫米

        // ☆☆☆☆☆
        // 注意2套直角坐标
        // 第1套直角坐标(用于照片姿态)
        // X轴朝右
        // Y轴朝里
        // Z轴朝上 ← 注意这里
        // 对应参数:
        // X轴: omega
        // Y轴: phi
        // Z轴: kappa
        // ---------------------------------------------------
        // 第2套直角坐标(用于飞机飞行姿态)
        // X轴朝右
        // Y轴朝上
        // Z轴朝外
        // 对应参数:
        // X轴: pitch 飞机俯仰
        // Y轴: yaw 飞机航向
        // Z轴: roll 飞机横滚


        private double _sensor_width = 0;       // 传感器宽度(单位: 毫米)
        private double _sensor_height = 0;      // 传感器高度(单位: 毫米)
        private double _focal = 0;              // 相机焦距(单位: 毫米)
        private double _focal35 = 0;            // 35毫米等效焦距(单位: 毫米) 此时 宽度x高度 36x24 (mm x mm)

        private double _image_width = 0;        // 照片宽度(单位: 像素) <<用来计算地面分辨率的 同时用来框定传感器尺寸
        private double _image_height = 0;       // 照片高度(单位: 像素) <<用来计算地面分辨率的 同时用来框定传感器尺寸

        private double _omega = 0;              // X(单位: 角度0-360 向右)
        private double _phi = 0;                // Y(单位: 角度0-360 向里)
        private double _kappa = 0;              // Z(单位: 角度0-360 向上)

        private double _z = 0;                  // Z 相机焦点Z坐标(单位: 米)
        private double _y = 0;                  // Y 相机焦点Y坐标(单位: 米)
        private double _x = 0;                  // X 相机焦点X坐标(单位: 米)

        private const double PI = 3.14159265358979323846;   //
        private const double FOCAL35_SENSOR_WIDTH = 36.0;   // 标准35mm的传感器宽度为36mm 注意S3C里用的是35mm
        private const double FOCAL35_SENSOR_HEIGHT = 24.0;  // 标准35mm的传感器高度为24mm
        private const double FOCAL35_DIAGONAL_LEN = 43.266615305567875;     // 标准35mm的传感器对角线长度

        private Vector3 _sensor1 = new Vector3();   // 传感器空间坐标1
        private Vector3 _sensor2 = new Vector3();   // 传感器空间坐标2
        private Vector3 _sensor3 = new Vector3();   // 传感器空间坐标3
        private Vector3 _sensor4 = new Vector3();   // 传感器空间坐标4
        private Vector3 _focus = new Vector3();     // 相机焦点空间坐标


        /// <summary>
        /// 空中三维照片
        /// ☆注意☆: 直角坐标系采用Z轴朝上的坐标系
        /// </summary>
        /// <param name="focal35">35毫米等效焦距 单位: 毫米</param>
        /// <param name="image_width">照片宽度 单位: 像素</param>
        /// <param name="image_height">照片高度 单位: 像素</param>
        /// <param name="omega">空中姿态: x轴(向右) 单位: 度</param>
        /// <param name="phi">空中姿态: y轴(向里) 单位: 度</param>
        /// <param name="kappa">空中姿态: z轴(向上) 单位: 度</param>
        /// <param name="x">焦点位置: x 单位: 米</param>
        /// <param name="y">焦点位置: y 单位: 米</param>
        /// <param name="z">焦点位置: z 单位: 米</param>
        public Photo3D(double focal35, double image_width, double image_height, double omega, double phi, double kappa, double x, double y, double z)
        {
            _focal35 = focal35;             // 
            _image_width = image_width;     // 
            _image_height = image_height;   // 
            _omega = omega;                 // 
            _phi = phi;                     // 
            _kappa = kappa;                 // 
            _x = x;                         // 
            _y = y;                         // 
            _z = z;                         // 

            InitPhoto();                    // 初始化
        }
        /// <summary>
        /// 初始化 单位: 毫米
        /// </summary>
        private void InitPhoto()
        {
            // 1. 根据对角线计算出传等效感器尺寸
            // 2. 根据35mm等效焦距和图片高宽比计算传等效感器4个点位置
            // 3. 焦点位置刚好位于x=0 y=0 z=0的位置
            // 4. 传感器4个点 + 焦点(0,0,0)刚好5个空间坐标点
            // 5. 按照空中姿态进行xyz轴的角度旋转
            // 6. 将5个点进行xyz的平移
            double atan = Math.Atan(_image_height / _image_width);
            _sensor_height = FOCAL35_DIAGONAL_LEN * Math.Sin(atan);
            _sensor_width = FOCAL35_DIAGONAL_LEN * Math.Cos(atan);

            // 传感器4个角点
            Vector3 sensor1 = new Vector3((float)_sensor_width / 2, (float)_sensor_height / 2, (float)_focal35);
            Vector3 sensor2 = new Vector3((float)_sensor_width / 2, (float)_sensor_height / -2, (float)_focal35);
            Vector3 sensor3 = new Vector3((float)_sensor_width / -2, (float)_sensor_height / -2, (float)_focal35);
            Vector3 sensor4 = new Vector3((float)_sensor_width / -2, (float)_sensor_height / 2, (float)_focal35);
            // 焦点
            Vector3 focus = new Vector3(0, 0, 0);
            // 旋转 把5个点都旋转
            float pitch = (float)(_omega * PI / 180.0);         // 绕X轴转
            float yaw = (float)(_phi * PI / 180.0);             // 绕Y轴转
            float roll = (float)(_kappa * PI / 180);            // 绕Z轴转
            Matrix rotation = Matrix.RotationYawPitchRoll(yaw, pitch, roll);    // 构造旋转矩阵
            Vector4 sensor1_ro = Vector3.Transform(sensor1, rotation);
            Vector4 sensor2_ro = Vector3.Transform(sensor2, rotation);
            Vector4 sensor3_ro = Vector3.Transform(sensor3, rotation);
            Vector4 sensor4_ro = Vector3.Transform(sensor4, rotation);
            Vector4 focus_ro = Vector3.Transform(focus, rotation);
            // 平移坐标
            Matrix tran = Matrix.Translation((float)_x * 1000, (float)_y * 1000, (float)_z * 1000); // 构造平移矩阵
            Vector4 sensor1_mv = Vector3.Transform(new Vector3(sensor1_ro.X, sensor1_ro.Y, sensor1_ro.Z), tran);
            Vector4 sensor2_mv = Vector3.Transform(new Vector3(sensor2_ro.X, sensor2_ro.Y, sensor2_ro.Z), tran);
            Vector4 sensor3_mv = Vector3.Transform(new Vector3(sensor3_ro.X, sensor3_ro.Y, sensor3_ro.Z), tran);
            Vector4 sensor4_mv = Vector3.Transform(new Vector3(sensor4_ro.X, sensor4_ro.Y, sensor4_ro.Z), tran);
            Vector4 focus_mv = Vector3.Transform(new Vector3(focus_ro.X, focus_ro.Y, focus_ro.Z), tran);
            // 保存
            _sensor1 = new Vector3(sensor1_mv.X, sensor1_mv.Y, sensor1_mv.Z);
            _sensor2 = new Vector3(sensor2_mv.X, sensor2_mv.Y, sensor2_mv.Z);
            _sensor3 = new Vector3(sensor3_mv.X, sensor3_mv.Y, sensor3_mv.Z);
            _sensor4 = new Vector3(sensor4_mv.X, sensor4_mv.Y, sensor4_mv.Z);
            _focus = new Vector3(focus_mv.X, focus_mv.Y, focus_mv.Z);
        }
        /// <summary>
        /// 获取照片投影在地面上的区域 
        /// 如果无解返回空
        /// </summary>
        /// <param name="h">地平面高度 单位: 米</param>
        /// <returns>地面区域4个角点 单位: 米</returns>
        public Vector3[] GetArea(double h)
        {
            // 检查无解
            // 只要传感器角点Z值 ≤ 焦点Z值 就无解
            if (_sensor1.Z < _focus.Z || _sensor1.Z == _focus.Z ||
                _sensor2.Z < _focus.Z || _sensor2.Z == _focus.Z ||
                _sensor3.Z < _focus.Z || _sensor3.Z == _focus.Z ||
                _sensor4.Z < _focus.Z || _sensor4.Z == _focus.Z) {
                return null;
            }
            // 利用空间2点求直线方程 然后用Z值带入方程求出XY
            // 求出地面投影角点
            Vector3? ground1 = GetXPoint(_sensor1, _focus, h * 1000);
            Vector3? ground2 = GetXPoint(_sensor2, _focus, h * 1000);
            Vector3? ground3 = GetXPoint(_sensor3, _focus, h * 1000);
            Vector3? ground4 = GetXPoint(_sensor4, _focus, h * 1000);

            if (ground1 == null ||
                ground2 == null ||
                ground3 == null ||
                ground4 == null) return null;

            return new Vector3[] {
                new Vector3(ground1.Value.X /1000.0f, ground1.Value.Y /1000.0f, ground1.Value.Z /1000.0f),
                new Vector3(ground2.Value.X /1000.0f, ground2.Value.Y /1000.0f, ground2.Value.Z /1000.0f),
                new Vector3(ground3.Value.X /1000.0f, ground3.Value.Y /1000.0f, ground3.Value.Z /1000.0f),
                new Vector3(ground4.Value.X /1000.0f, ground4.Value.Y /1000.0f, ground4.Value.Z /1000.0f)};
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        private Vector3? GetXPoint(Vector3 p1, Vector3 p2, double h)
        {
            // 与平面上的直线方程的求法类似，平面上的直线方程有“点斜式”
            // 空间内的直线方程有“点向式”，“向”指的是与直线平行的一个向量
            // 在这里，方向向量就是向量AB＝(x2-x1，y2-y1，z2-z1)
            // 直线的方程是：(x－x1)/(x2－x1)＝(y－y1)/(y2－y1)＝(z－z1)/(z2－z1) 
            // http://wenku.baidu.com/link?url=awsnrAP1wCdOfdtxpcf3Ycq7g6inP2LwOLDcwHPBxFLjHMAWf9NCxNftBmfNeXtFWwKm6bXQfm-o3emDbSFHcKavd4VZc4x_HJuWL2CaJ9i
            // http://wenku.baidu.com/link?url=c185ld01WGGWETSghwvCq4qmTP_vP4aLsc2bIy6ThEl1cHoKwGaJ69K1iNcNNWYWfVNffj9kpwp9fxM9b62nHFRxs17SwFEE7Am73IdeuiO
            double u = p2.Z - p1.Z;
            if (u == 0) return null;
            double t = (h - p1.Z) / (p2.Z - p1.Z);
            double x = t * (p2.X - p1.X) + p1.X;
            double y = t * (p2.Y - p1.Y) + p1.Y;
            return new Vector3((float)x, (float)y, (float)h);
        }
        /// <summary>
        /// 求算视角
        /// </summary>
        /// <param name="focal35">35mm等效焦距 单位:毫米</param>
        /// <returns>视角 单位: 度</returns>
        public double GetVisualAngle(double focal35)
        {
            // 35mm 标注画幅为 36mm x 24mm 长边为 36mm
            // 根据三角函数计算
            return Math.Atan((36 / 2.0) / focal35) / PI * 180 * 2;
        }
        

    }
}
