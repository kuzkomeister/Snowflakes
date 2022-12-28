using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Snowflakes.Models
{
    public class Contour
    {
        public Contour()
        {

        }
        public Contour(params Point[] points)
        {
            Points = new List<Point>(points);
        }

        public List<Point> Points { get; private set; } = new List<Point>();

        /// <summary>
        /// Вычисление статуса пересечения контуров
        /// </summary>
        /// <param name="contour"></param>
        /// <returns></returns>
        public bool IsIntersect(Contour contour)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                for (int j = 0; j < contour.Points.Count; j++)
                {
                    int ni = (i + 1) % Points.Count;
                    int nj = (j + 1) % contour.Points.Count;
                    if (intersection(Points[i], Points[ni], contour.Points[j], contour.Points[nj]))
                        return true;
                }
            }
            return false;
        }

        private bool intersection(Point start1, Point end1, Point start2, Point end2) //источник: [url]http://users.livejournal.com/_winnie/152327.html[/url]
        {
            Point dir1 = new Point();
            dir1.X = end1.X - start1.X;
            dir1.Y = end1.Y - start1.Y;
            Point dir2 = new Point();
            dir2.X = end2.X - start2.X;
            dir2.Y = end2.Y - start2.Y;

            //считаем уравнения прямых проходящих через отрезки
            double a1 = -dir1.Y;
            double b1 = +dir1.X;
            double d1 = -(a1 * start1.X + b1 * start1.Y);

            double a2 = -dir2.Y;
            double b2 = +dir2.X;
            double d2 = -(a2 * start2.X + b2 * start2.Y);

            //подставляем концы отрезков, для выяснения в каких полуплоскотях они
            double seg1_line2_start = a2 * start1.X + b2 * start1.Y + d2;
            double seg1_line2_end = a2 * end1.X + b2 * end1.Y + d2;

            double seg2_line1_start = a1 * start2.X + b1 * start2.Y + d1;
            double seg2_line1_end = a1 * end2.X + b1 * end2.Y + d1;

            //если концы одного отрезка имеют один знак, значит он в одной полуплоскости и пересечения нет.
            if (seg1_line2_start * seg1_line2_end >= 0 || seg2_line1_start * seg2_line1_end >= 0)
                return false;
            else
                return true;
        }

    }
}
