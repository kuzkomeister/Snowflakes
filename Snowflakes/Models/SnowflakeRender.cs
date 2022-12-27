using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Snowflakes.Models
{
    public class SnowflakeRender
    {
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
        /// Отображение сегмента снежинки. 1/8
        /// </summary>
        public void RenderNewSnoflakeSegment(Path pathSegment, SnowflakeSegment segment)
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
        /// Отображение всей снежинки.
        /// </summary>
        public void RenderNewSnowflakeFull(Canvas canvasSnowflake, List<Contour> contours)
        {
            canvasSnowflake.Children.Clear();
            foreach (var contour in contours)
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
    }
}
