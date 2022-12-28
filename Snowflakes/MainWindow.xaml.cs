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

using netDxf;
using netDxf.Entities;

using Snowflakes.Models;

namespace Snowflakes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region [ Модули работы со снежинками ]

        private SnowflakeRender Render = new SnowflakeRender();
        
        private SnowflakeCreator Creator = new SnowflakeCreator();

        private SnowflakeGenerator Generator = new SnowflakeGenerator();

        #endregion

        public SnowflakeSegment CurrentSnowflakeSegment { get; private set; }
        public List<Contour> CurrentSnowflake { get; private set; }

        public int Seed { get; private set; }

        private void _Main()
        {
            Seed = new Random().Next();
            txtTemplate.Text = $"Шаблон снежинки #{Seed}:";
            CurrentSnowflakeSegment = new SnowflakeSegment();
            Generator.GenerateSnowflakeSegment(CurrentSnowflakeSegment, Seed);
            CurrentSnowflake = Creator.CreateSnowflakeContours(CurrentSnowflakeSegment);

            Render.RenderNewSnoflakeSegment(pathSegment, CurrentSnowflakeSegment);
            Render.RenderNewSnowflakeFull(canvasSnowflake, CurrentSnowflake);
        }








        #region [ Служебные методы ]
        
        public MainWindow()
        {
            InitializeComponent();

            _Main();
        }

        private void Click_NewSnow(object sender, RoutedEventArgs e)
        {
            _Main();
        }

        private bool isFirst = true;
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if(sender is RadioButton RB)
            {
                SnowflakeGenerator.NumberTemplate = Convert.ToInt32(RB.Tag);
                if (isFirst)
                {
                    isFirst = !isFirst;
                }
                else
                {
                    _Main();
                }
            }
        }

        #endregion

        private void BtnClick_CreateDxf(object sender, RoutedEventArgs e)
        {
            string fileName = $"C:\\Users\\peace\\Desktop\\snowflake_{Seed}.dxf";

            DxfDocument doc = new DxfDocument();

            foreach(var contour in CurrentSnowflake)
            {
                var polyline = new netDxf.Entities.Polyline() { IsClosed = true };
                foreach(var point in contour.Points)
                {
                    polyline.Vertexes.Add(new PolylineVertex(point.X, point.Y, 0));
                }
                doc.AddEntity(polyline);
            }
            doc.Save(fileName);
        }
    }
}
