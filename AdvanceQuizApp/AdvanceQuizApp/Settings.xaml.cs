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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdvanceQuizApp
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Window br = new MainWindow();
            br.Show();
            br.WindowState = WindowState.Maximized;

        }
        private void ClearData(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("sai a");
        }

        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("sai a");

        }
    }
}
