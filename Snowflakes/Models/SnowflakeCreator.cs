using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Snowflakes.Models
{
    public class SnowflakeCreator : SnowflakeHelper
    {
        public SnowflakeCreator()
        {
            SnowflakeHelper.AmountSegments = 12;
            SnowflakeHelper.AngleSegment = 360 / AmountSegments;

            SnowflakeSegment.StandartLeft = CreatePointInGradus(RadiusSegment, -90 - AngleSegment / 2);
            SnowflakeSegment.StandartRight = CreatePointInGradus(RadiusSegment, -90 + AngleSegment / 2);
        }

        /// <summary>
        /// Создать снежинку по шаблону сегмента
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public List<Contour> CreateSnowflakeContours(SnowflakeSegment segment)
        {
            double angle = 0;
            List<Contour> res = new List<Contour>();
            // Создание сегментов
            List<Point> topContour = new List<Point>();
            for (int i = 0; i < AmountSegments; i++)
            {
                // Чередование сегментов для зеркализации
                if (i % 2 == 0)
                {
                    // Поворот

                    // Вычисление точек внешней грани
                    var rotatedLeftPoint = RotatePointGrad(segment.Left, angle);
                    if (HavePoint(topContour, rotatedLeftPoint) is false)
                    {
                        topContour.Add(rotatedLeftPoint);
                    }

                    foreach (var point in segment.TopPoints)
                    {
                        var rotatedPoint = RotatePointGrad(point, angle);
                        if (HavePoint(topContour, rotatedPoint) is false)
                        {
                            topContour.Add(rotatedPoint);
                        }
                    }

                    var rotatedRightPoint = RotatePointGrad(segment.Right, angle);
                    if (HavePoint(topContour, rotatedRightPoint) is false)
                    {
                        topContour.Add(rotatedRightPoint);
                    }
                }
                else
                {
                    // Зеркально
                    var axisPoint = CreatePointInGradus(RadiusSegment, -90 + angle);

                    // Вычисление точек внешней грани
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

                    // Вычисление точек внутренней правой грани и соседней левой
                    var rightContour = new Contour();
                    foreach (var p in segment.RightPoints)
                    {
                        var symetricPoint = CreateSymmetricPoint(RotatePointGrad(p, angle), axisPoint);
                        if (HavePoint(rightContour.Points, symetricPoint) is false)
                        {
                            rightContour.Points.Add(symetricPoint);
                        }
                    }
                    for (int ip = segment.RightPoints.Count - 1; ip >= 0; ip--)
                    {
                        var point = segment.RightPoints[ip];
                        var rotatedPoint = RotatePointGrad(point, angle - AngleSegment);
                        if (HavePoint(rightContour.Points, rotatedPoint) is false)
                        {
                            rightContour.Points.Add(rotatedPoint);
                        }
                    }
                    if (rightContour.Points.Count > 2)
                    {
                        res.Add(rightContour);
                    }

                    // Вычисление точек внутренней левой грани и соседней правой
                    var leftContour = new Contour();
                    foreach (var p in segment.LeftPoints)
                    {
                        var symetricPoint = CreateSymmetricPoint(RotatePointGrad(p, angle), axisPoint);
                        if (HavePoint(leftContour.Points, symetricPoint) is false)
                        {
                            leftContour.Points.Add(symetricPoint);
                        }
                    }
                    for (int ip = segment.LeftPoints.Count - 1; ip >= 0; ip--)
                    {
                        var point = segment.LeftPoints[ip];
                        var rotatedPoint = RotatePointGrad(point, angle + AngleSegment);
                        if (HavePoint(leftContour.Points, rotatedPoint) is false)
                        {
                            leftContour.Points.Add(rotatedPoint);
                        }
                    }
                    if (leftContour.Points.Count > 2)
                    {
                        res.Add(leftContour);
                    }
                }

                angle += AngleSegment;
            }

            var externalContour = new Contour(topContour.ToArray());

            res.Add(externalContour);
            return res;
        }
        
    }
}
