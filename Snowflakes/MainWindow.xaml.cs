using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Snowflakes.Models;

namespace Snowflakes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int amountSegments;
        double angleSegment;
        public double RadiusSegment = 100;

        private void _SetSettings()
        {
            amountSegments = 12;
            angleSegment = 360 / amountSegments;

            SnowflakeSegment.StandartLeft = CreatePointInGradus(RadiusSegment, -90 - angleSegment / 2);
            SnowflakeSegment.StandartRight = CreatePointInGradus(RadiusSegment, -90 + angleSegment / 2);
        }

        private void _TypoMain()
        {
            SnowflakeSegment segment = new SnowflakeSegment();
            segment.TopPoints.Add(CreatePointInGradus(RadiusSegment - 20, -95));
            List<Contour> contours = _CreateFull(segment);

            _RenderNewSnoflakeSegment(segment);
            _RenderNewSnowflakeFull(contours);
        }

        #region [ Создание контура снежинки ]

        /// <summary>
        /// Создать снежинку по шаблону сегмента
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        private List<Contour> _CreateFull(SnowflakeSegment segment)
        {
            double angle = 0;
            List<Contour> res = new List<Contour>();
            // Создание сегментов
            List<Point> topContour = new List<Point>();
            for (int i = 0; i < amountSegments; i++)
            {
                // Чередование сегментов для зеркализации
                if (i % 2 == 0)
                {
                    // Поворот
                    //var rotatedLeftPoint = RotatePointGrad(segment.Left, angle);
                    //if (HavePoint(topContour, rotatedLeftPoint) is false)
                    //{
                    //    topContour.Add(rotatedLeftPoint);
                    //}

                    foreach (var point in segment.TopPoints)
                    {
                        var rotatedPoint = RotatePointGrad(point, angle);
                        if (HavePoint(topContour, rotatedPoint) is false)
                        {
                            topContour.Add(rotatedPoint);
                        }
                    }

                    //var rotatedRightPoint = RotatePointGrad(segment.Right, angle);
                    //if (HavePoint(topContour, rotatedRightPoint) is false)
                    //{
                    //    topContour.Add(rotatedRightPoint);
                    //}
                }
                else
                {
                    // Зеркально
                    var axisPoint = CreatePointInRadians(RadiusSegment, ConvertGradToRad(-90 + angle));

                    var symRightPoint = CreateSymmetricPoint(RotatePointGrad(segment.Right, angle), axisPoint);
                    if (HavePoint(topContour, symRightPoint) is false)
                    {
                        topContour.Add(symRightPoint);
                    }

                    for (int ip = segment.TopPoints.Count - 1; ip >= 0; ip--)
                    {
                        var point = segment.TopPoints[ip];

                        var rotatedPoint = RotatePointGrad(point, angle);
                        var symetricPoint = CreateSymmetricPoint(rotatedPoint, axisPoint);
                        if (HavePoint(topContour, symetricPoint) is false)
                        {
                            topContour.Add(symetricPoint);
                        }
                    }

                    var symLeftPoint = CreateSymmetricPoint(RotatePointGrad(segment.Left, angle), axisPoint);
                    if (HavePoint(topContour, symLeftPoint) is false)
                    {
                        topContour.Add(symLeftPoint);
                    }
                }

                angle += angleSegment;
            }

            var externalContour = new Contour(topContour.ToArray());
             
            res.Add(externalContour);
            return res;
        }

        #endregion

        #region [ Отображение ]

        /// <summary>
        /// Отображение сегмента снежинки. 1/8
        /// </summary>
        private void _RenderNewSnoflakeSegment(SnowflakeSegment segment)
        {
            var lss = new List<LineSegment>();
            var all = segment.All;
            for (int i = 0; i < all.Count; i++)
            {
                var p = all[i];
                lss.Add(new LineSegment(p, true));
            }
            var figure = new PathFigure(new Point(), lss, true);
            pathSegment.Data = new PathGeometry(new PathFigureCollection(new List<PathFigure> { figure }));
        }

        /// <summary>
        /// Отображение контура
        /// </summary>
        /// <param name="contour"></param>
        /// <returns></returns>
        private PathGeometry _RenderContour(Contour contour)
        {
            var lss = new List<LineSegment>();
            for (int i = 1; i < contour.Points.Count; i++)
            {
                var p = contour.Points[i];
                lss.Add(new LineSegment(p, true));
            }
            var figure = new PathFigure(contour.Points.Count > 0 ? contour.Points[0] : new Point(), lss, true);
            return new PathGeometry(new PathFigureCollection(new List<PathFigure> { figure }));
        }

        /// <summary>
        /// Отображение всей снежинки.
        /// </summary>
        private void _RenderNewSnowflakeFull(List<Contour> contours)
        {
            canvasSnowflake.Children.Clear();
            foreach(var contour in contours)
            {
                var path = new Path()
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Fill = Brushes.Transparent,
                    Data = _RenderContour(contour)
                };
                canvasSnowflake.Children.Add(path);
            }
        }

        #endregion

        #region [ Создание точек ]

        /// <summary>
        /// Создать точку в полярных координатах в радианах
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="radAngle"></param>
        /// <returns></returns>
        private Point CreatePointInRadians(double radius, double radAngle) =>
            new Point(radius * Math.Cos(radAngle),
                       radius * Math.Sin(radAngle));

        /// <summary>
        /// Создать точку в полярных координатах в градусах
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="gradAngle"></param>
        /// <returns></returns>
        private Point CreatePointInGradus(double radius, double gradAngle) => CreatePointInRadians(radius, ConvertGradToRad(gradAngle));

        /// <summary>
        /// Получить симметричную точку origP относительно прямой проходящей через начало координат и точку lineP
        /// </summary>
        /// <param name="origP"></param>
        /// <param name="lineP"></param>
        /// <returns></returns>
        private Point CreateSymmetricPoint(Point origP, Point lineP)
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

        #endregion

        #region [ Конвертеры угла ]

        /// <summary>
        /// Конвертировать градусы в радианы
        /// </summary>
        /// <param name="grad"></param>
        /// <returns></returns>
        private double ConvertGradToRad(double grad) => (grad * Math.PI) / 180.0;

        /// <summary>
        /// Конвертировать радианы в градусы
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        private double ConvertRadToGrad(double rad) => (rad * 180.0) / Math.PI;

        #endregion

        #region [ Методы поворота точки ]

        /// <summary>
        /// Повернуть точку относительно начала координат в градусах
        /// </summary>
        /// <param name="point"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private Point RotatePointGrad(Point point, double angle)
        {
            return new Point(point.X * Math.Cos(ConvertGradToRad(angle)) - point.Y * Math.Sin(ConvertGradToRad(angle)),
                             point.X * Math.Sin(ConvertGradToRad(angle)) + point.Y * Math.Cos(ConvertGradToRad(angle)));
        }

        /// <summary>
        /// Повернуть точку относительно начала координат в радианах
        /// </summary>
        /// <param name="point"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private Point RotatePointRad(Point point, double angle)
        {
            return new Point(point.X * Math.Cos(angle) - point.Y * Math.Sin(angle),
                             point.X * Math.Sin(angle) + point.Y * Math.Cos(angle));
        }

        #endregion

        #region [ Служебные методы ]

        /// <summary>
        /// Проверка. Есть ли уже точка в списке
        /// </summary>
        /// <param name="points"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool HavePoint(List<Point> points, Point point)
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
        private void GetRadiusAngleGrad(Point point, out double radius, out double angle)
        {
            var rad = -Math.Atan(point.X / point.Y);
            angle = ConvertRadToGrad(rad);
            radius = Math.Sqrt(point.X * point.X + point.Y * point.Y);
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            _SetSettings();
            _TypoMain();
        }

        private void Click_NewSnow(object sender, RoutedEventArgs e)
        {
            _TypoMain();
        }
    }
}
