using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Forecaster
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ScaleTransform transormFast = new ScaleTransform();
            ScaleTransform transformSlow = new ScaleTransform();
            DoubleAnimation fastAnim = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(1000));
            DoubleAnimation slowAnim = new DoubleAnimation(0.3, 1, TimeSpan.FromMilliseconds(1000));
            transormFast.BeginAnimation(ScaleTransform.ScaleYProperty, fastAnim);
            transformSlow.BeginAnimation(ScaleTransform.ScaleYProperty, slowAnim);

            CitiesPanel.RenderTransform = transormFast;
            HourlyPanel.RenderTransform = transformSlow;
            DailyPanel.RenderTransform = transformSlow;
        }

        private void ChangeCity (object sender, RoutedEventArgs e)
        {
            ScaleTransform transformSlow = new ScaleTransform();
            DoubleAnimation slowAnim = new DoubleAnimation(0.3, 1, TimeSpan.FromMilliseconds(500));
            transformSlow.BeginAnimation(ScaleTransform.ScaleYProperty, slowAnim);

            HourlyPanel.RenderTransform = transformSlow;
            DailyPanel.RenderTransform = transformSlow;

            var citiesButtons = CitiesPanel.Children;
            foreach (Button cityButton in citiesButtons)
                cityButton.Background = Brushes.PaleGreen;

            LinearGradientBrush gradientBg = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop { Color = Colors.PaleGreen, Offset = 0 },
                    new GradientStop { Color = Colors.Cyan, Offset = 1 }
                }
            };

            Button clickedButton = sender as Button;
            clickedButton.Background = gradientBg;
        }
    }
}
