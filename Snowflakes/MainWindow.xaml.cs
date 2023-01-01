using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

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

        public string pathFile { get; private set; } = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\";

        public SnowflakeSegment CurrentSnowflakeSegment { get; private set; }
        public List<Contour> CurrentSnowflake { get; private set; }

        public int Seed { get; private set; }

        private void _Main()
        {
            var r = new Random();
            List<string> snowflakes = Directory.GetFiles(pathFile, "*.dxf").ToList();
            List<int> seeds = new List<int>(snowflakes.Where(s => Convert.ToInt32(s.Split('_')[1]) == SnowflakeGenerator.NumberTemplate)
                .Select(s => Convert.ToInt32(s.Split('_', '.')[2])));
            do
            {
                Seed = r.Next();
            } 
            while (seeds.Contains(Seed));
            
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
            if(sender is System.Windows.Controls.RadioButton RB)
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
            string fileName = $"{pathFile}snowflake_{SnowflakeGenerator.NumberTemplate}_{Seed}.dxf";

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

        private void BtnClick_SetPath(object sender, RoutedEventArgs e)
        {
            using(var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();
                if(result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    pathFile = fbd.SelectedPath + "\\";
                }
            }
        }
    }
}
