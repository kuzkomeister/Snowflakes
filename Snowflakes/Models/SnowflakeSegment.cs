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

                res.AddRange(LeftPoints);
                res.Add(Left);
                res.AddRange(TopPoints);
                res.Add(Right);
                res.AddRange(RightPoints);
                return res;
            } 
        }

        /// <summary>
        /// Точки на внешней грани. Вверх.
        /// </summary>
        public List<Point> TopPoints { get; set; } = new List<Point>();
        
        /// <summary>
        /// Точки на внутренней грани. Лево.
        /// </summary>
        public List<Point> LeftPoints { get; set; } = new List<Point>();
        
        /// <summary>
        /// Точки на внутренней грани. Право.
        /// </summary>
        public List<Point> RightPoints { get; set; } = new List<Point>();

        #endregion


        public static Point StandartLeft;
        public static Point StandartRight;
    }
}
