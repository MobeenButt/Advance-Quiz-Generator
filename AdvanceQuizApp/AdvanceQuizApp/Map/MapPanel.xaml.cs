using System;
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

namespace AdvanceQuizApp.Map
{
    /// <summary>
    /// Interaction logic for MapPanel.xaml
    /// </summary>
    public partial class MapPanel : Window
    {
        public MapPanel()
        {
            InitializeComponent();
            //webBrowser.Navigate("D:\\Advance-Quiz-Generator\\AdvanceQuizApp\\AdvanceQuizApp\\Map\\map.html");

            webBrowser.Navigate(new Uri(@"file:///D:/Advance-Quiz-Generator/AdvanceQuizApp/AdvanceQuizApp/Map/map.html"));

        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.InvokeScript("zoomIn");
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.InvokeScript("zoomOut");
        }
    }
}
