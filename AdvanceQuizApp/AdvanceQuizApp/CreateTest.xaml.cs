﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdvanceQuizApp
    {
    /// <summary>
    /// Interaction logic for CreateTest.xaml
    /// </summary>
    public partial class CreateTest : Window
        {
        public CreateTest()
            {
            InitializeComponent();
            }
        private void Button_ReturnButtton_Click(object sender, RoutedEventArgs e)
            {
            Window win = new MainWindow();
            win.Show();
            win.WindowState = WindowState.Maximized;
            this.Visibility = Visibility.Hidden;
            }
        }
    }