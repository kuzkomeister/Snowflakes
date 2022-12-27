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
        /// Обработать шаблон снежинки
        /// </summary>
        /// <param name="segment"></param>
        public void GenerateSnowflakeSegment(SnowflakeSegment segment)
        {
            switch (NumberTemplate)
            {
                case 1:
                    GenerateTemplate1(segment);
                    break;

            }
        }

        /// <summary>
        /// Сгенерировать по сценарию 1
        /// </summary>
        /// <param name="segment"></param>
        private void GenerateTemplate1(SnowflakeSegment segment) 
        {
            segment.TopPoints.Add(CreatePointInGradus(RadiusSegment - 40, -90));
        }
    }
}
