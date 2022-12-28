using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Snowflakes.Models
{
    public abstract class SnowflakeHelper
    {
        #region [ Настройки ]

        /// <summary>
        /// Количество сегментов в снежинке
        /// </summary>
        public static int AmountSegments { get; set; }

        /// <summary>
        /// Угол сегмента снежинки
        /// </summary>
        public static double AngleSegment { get; set; }

        /// <summary>
        /// Радиус описанной окружности снежинки
        /// </summary>
        public static double RadiusSegment { get; set; } = 100;

        #endregion

        #region [ Создание точек ]

        /// <summary>
        /// Создать точку в полярных координатах в радианах
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="radAngle"></param>
        /// <returns></returns>
        public Point CreatePointInRadians(double radius, double radAngle) =>
            new Point(radius * Math.Cos(radAngle),
                       radius * Math.Sin(radAngle));

        /// <summary>
        /// Создать точку в полярных координатах в градусах
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="gradAngle"></param>
        /// <returns></returns>
        public Point CreatePointInGradus(double radius, double gradAngle) => CreatePointInRadians(radius, ConvertGradToRad(gradAngle));

        /// <summary>
        /// Получить симметричную точку origP относительно прямой проходящей через начало координат и точку lineP
        /// </summary>
        /// <param name="origP"></param>
        /// <param name="lineP"></param>
        /// <returns></returns>
        public Point CreateSymmetricPoint(Point origP, Point lineP)
        {
            double A1 = -lineP.Y / lineP.X;
            double B2 = -A1;
            double C2 = -origP.X + A1 * origP.Y;
            double Xp = C2 / (A1 * B2 - 1);
            double Yp = (-C2 * A1) / (A1 * B2 - 1);
            double Xv = (Xp - origP.X) * 2;
            double Yv = (Yp - origP.Y) * 2;
            return new Point(origP.X + Xv, origP.Y + Yv);
        }

        /// <summary>
        /// Создать точку на отрезке на определенной длине
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public Point CreatePointOnLine(Point A, Point B, double length)
        {
            double lengthAB = Math.Sqrt(Math.Pow(B.X - A.X, 2) + Math.Pow(B.Y - A.Y, 2));
            double k = length / lengthAB;
            return new Point(A.X + (B.X - A.X) * k, 
                             A.Y + (B.Y - A.Y) * k);
        }

        /// <summary>
        /// Создать точку на отрезке на определенной длине в процентах
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="lengthPercent"></param>
        /// <returns></returns>
        public Point CreatePointOnLinePercent(Point A, Point B, double lengthPercent)
        {
            double lengthAB = Math.Sqrt(Math.Pow(B.X - A.X, 2) + Math.Pow(B.Y - A.Y, 2));
            return CreatePointOnLine(A, B, lengthAB * lengthPercent);
        }

        #endregion

        #region [ Конвертеры угла ]

        /// <summary>
        /// Конвертировать градусы в радианы
        /// </summary>
        /// <param name="grad"></param>
        /// <returns></returns>
        public double ConvertGradToRad(double grad) => (grad * Math.PI) / 180.0;

        /// <summary>
        /// Конвертировать радианы в градусы
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public double ConvertRadToGrad(double rad) => (rad * 180.0) / Math.PI;

        #endregion

        #region [ Служебные методы ]

        /// <summary>
        /// Проверка. Есть ли уже точка в списке
        /// </summary>
        /// <param name="points"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool HavePoint(List<Point> points, Point point)
        {
            return points.Any(p => Math.Round(p.X, 2) == Math.Round(point.X, 2) &&
                                   Math.Round(p.Y, 2) == Math.Round(point.Y, 2));
        }

        /// <summary>
        /// Получить радиус и угол точки в полярных координатах
        /// </summary>
        /// <param name="point"></param>
        /// <param name="radius"></param>
        /// <param name="angle"></param>
        public void GetRadiusAngleGrad(Point point, out double radius, out double angle)
        {
            var rad = -Math.Atan(point.X / point.Y);
            angle = ConvertRadToGrad(rad);
            radius = Math.Sqrt(point.X * point.X + point.Y * point.Y);
        }

        /// <summary>
        /// Повернуть точку относительно начала координат в градусах
        /// </summary>
        /// <param name="point"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public Point RotatePointGrad(Point point, double angle)
        {
            return new Point(point.X * Math.Cos(ConvertGradToRad(angle)) - point.Y * Math.Sin(ConvertGradToRad(angle)),
                             point.X * Math.Sin(ConvertGradToRad(angle)) + point.Y * Math.Cos(ConvertGradToRad(angle)));
        }

        #endregion

    }
}
