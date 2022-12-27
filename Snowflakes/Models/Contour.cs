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
    }
}
