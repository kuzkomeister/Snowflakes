﻿using System;
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

        private void _Main()
        {
            int seed = new Random().Next();
            txtTemplate.Text = $"Шаблон снежинки #{seed}:";
            SnowflakeSegment segment = new SnowflakeSegment();
            Generator.GenerateSnowflakeSegment(segment, seed);
            List<Contour> contours = Creator.CreateSnowflakeContours(segment);

            Render.RenderNewSnoflakeSegment(pathSegment, segment);
            Render.RenderNewSnowflakeFull(canvasSnowflake, contours);
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
    }
}
