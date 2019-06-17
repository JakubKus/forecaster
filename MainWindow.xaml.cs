using Forecaster.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Forecaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new SearchViewModel();
        }
    }
}
