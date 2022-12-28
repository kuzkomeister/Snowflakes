using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Snowflakes.Models
{
    public class SnowflakeGenerator : SnowflakeHelper
    {
        /// <summary>
        /// Номер шаблона
        /// </summary>
        public static int NumberTemplate = 0;

        /// <summary>
        /// Количество попыток создать контур
        /// </summary>
        public static int ContourTryes = 10;

        /// <summary>
        /// Обработать шаблон снежинки
        /// </summary>
        /// <param name="segment"></param>
        public void GenerateSnowflakeSegment(SnowflakeSegment segment, int seed)
        {
            switch (NumberTemplate)
            {
                case 1:
                    GenerateTemplate1(segment, seed);
                    break;

            }
        }

        /// <summary>
        /// Сгенерировать по сценарию 1
        /// </summary>
        /// <param name="segment"></param>
        private void GenerateTemplate1(SnowflakeSegment segment, int seed) 
        {
            Random r = new Random(seed);

            int amountLeftContours = 1;
            int amountRightContours = 1;
            int amountTopContours = 1;

            int amountAverageLeftPoints = 1;
            int amountAverageRightPoints = 1;
            int amountAverageTopPoints = 1;

            int amountEpsLeftPoints = 0;
            int amountEpsRightPoints = 0;
            int amountEpsTopPoints = 0;


            // Создание контуров левой грани
            for(int i = 0; i < amountLeftContours; i++)
            {
                var contour = new Contour();

                double startPercent = r.Next(10, (int)RadiusSegment / 2 - 10) / RadiusSegment;
                double endPercent = r.Next((int)RadiusSegment / 2 + 10, (int)RadiusSegment - 10) / RadiusSegment;
                double midPercent = r.NextDouble() * (endPercent - startPercent) + startPercent;

                contour.Points.Add(CreatePointOnLinePercent(new Point(), segment.Left, startPercent));
                var p = CreatePointOnLinePercent(new Point(), segment.Left, midPercent);
                contour.Points.Add(RotatePointGrad(p, r.Next(5, 15)));
                contour.Points.Add(CreatePointOnLinePercent(new Point(), segment.Left, endPercent));

                segment.LeftContours.Add(contour);
            }

            // Создание контуров внешней грани
            for (int i = 0; i < amountTopContours; i++)
            {
                var contour = new Contour();
                bool isAgain = false;
                int numTry = 0;
                do
                {
                    isAgain = false;
                    double startPercent = 0;    //r.NextDouble() * 0.25;
                    double endPercent = 1;      //1 - r.NextDouble() * 0.25;
                    double midPercent = r.NextDouble() * (endPercent - startPercent) + startPercent;

                    contour.Points.Add(CreatePointOnLinePercent(segment.Left, segment.Right, startPercent));
                    var p = CreatePointOnLinePercent(segment.Left, segment.Right, midPercent);
                    contour.Points.Add(new Point(p.X, p.Y + r.Next(5, 40)));
                    contour.Points.Add(CreatePointOnLinePercent(segment.Left, segment.Right, endPercent));

                    foreach (var c in segment.LeftContours)
                    {
                        if (c.IsIntersect(contour))
                        {
                            isAgain = true;
                        }
                    }
                    numTry++;
                }
                while (isAgain && numTry < ContourTryes);

                if (numTry < ContourTryes)
                {
                    segment.TopContours.Add(contour);
                }
            }

            // Создание контуров правой грани
            for (int i = 0; i < amountRightContours; i++)
            {
                var contour = new Contour();
                bool isAgain = false;
                int numTry = 0;
                do
                {
                    double endPercent = r.Next(10, (int)RadiusSegment / 2 - 10) / RadiusSegment;
                    double startPercent = r.Next((int)RadiusSegment / 2 + 10, (int)RadiusSegment - 10) / RadiusSegment;
                    double midPercent = r.NextDouble() * (endPercent - startPercent) + startPercent;

                    contour.Points.Add(CreatePointOnLinePercent(segment.Right, new Point(), endPercent));
                    var p = CreatePointOnLinePercent(segment.Right, new Point(), midPercent);
                    contour.Points.Add(RotatePointGrad(p, -r.Next(5, 15)));
                    contour.Points.Add(CreatePointOnLinePercent(segment.Right, new Point(), startPercent));

                    foreach (var c in segment.LeftContours)
                    {
                        if (c.IsIntersect(contour))
                        {
                            isAgain = true;
                        }
                    }
                    if(isAgain is false)
                    {
                        foreach (var c in segment.TopContours)
                        {
                            if (c.IsIntersect(contour))
                            {
                                isAgain = true;
                            }
                        }
                    }
                    numTry++;
                }
                while (isAgain && numTry < ContourTryes);

                if(numTry < ContourTryes)
                {
                    segment.RightContours.Add(contour);
                }

               

            }


            //segment.TopPoints.Add(CreatePointInGradus(RadiusSegment - 40, -90));

            //var left = new Contour();
            //left.Points.Add(CreatePointInGradus(RadiusSegment - 50, -90 - AngleSegment / 2));
            //left.Points.Add(CreatePointInGradus(RadiusSegment - 40, -90 - AngleSegment / 2 + 10));
            //left.Points.Add(CreatePointInGradus(RadiusSegment - 20, -90 - AngleSegment / 2));
            //segment.LeftContours.Add(left);
        }
    }
}
