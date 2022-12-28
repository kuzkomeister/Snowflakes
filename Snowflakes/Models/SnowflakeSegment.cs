using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Snowflakes.Models
{
    public class SnowflakeSegment
    {
        public SnowflakeSegment()
        {
            Left = StandartLeft; 
            Right = StandartRight;
        }
        public SnowflakeSegment(Point left, Point right)
        {
            Left = left;
            Right = right;
        }

        #region [ Угловые точки ]

        /// <summary>
        /// Левая угловая точка
        /// </summary>
        public Point Left { get; set; }

        /// <summary>
        /// Правая угловая точка
        /// </summary>
        public Point Right { get; set; }

        #endregion

        #region [ Точки на гранях ]

        public List<Point> All 
        { 
            get
            {
                var res = new List<Point>();

                foreach(var left in LeftContours)
                {
                    res.AddRange(left.Points);
                }
                res.Add(Left);
                foreach (var top in TopContours)
                {
                    res.AddRange(top.Points);
                }
                res.Add(Right);
                foreach (var right in RightContours)
                {
                    res.AddRange(right.Points);
                }
                return res;
            } 
        }

        /// <summary>
        /// Точки на внешней грани. Вверх.
        /// </summary>
        public List<Contour> TopContours { get; set; } = new List<Contour>();

        /// <summary>
        /// Точки на внутренней грани. Лево.
        /// </summary>
        public List<Contour> LeftContours { get; set; } = new List<Contour>();

        /// <summary>
        /// Точки на внутренней грани. Право.
        /// </summary>
        public List<Contour> RightContours { get; set; } = new List<Contour>();

        #endregion


        public static Point StandartLeft;
        public static Point StandartRight;
    }
}
