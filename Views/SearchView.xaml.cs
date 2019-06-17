using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Forecaster.Views
{
    /// <summary>
    /// Interaction logic for SearchView.xaml
    /// </summary>
    public partial class SearchView : UserControl
    {
        public SearchView()
        {
            InitializeComponent();
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            Duration animationDuration = new Duration(TimeSpan.FromMilliseconds(600));

            DoubleAnimation widthAnimation = new DoubleAnimation(300, animationDuration);
            SearchCityBar.BeginAnimation(WidthProperty, widthAnimation);

            DoubleAnimation opacityAnimation = new DoubleAnimation(1, animationDuration);
            SearchButton.BeginAnimation(OpacityProperty, opacityAnimation);

            DoubleAnimation fontSizeAnimation = new DoubleAnimation(18, animationDuration);
            SearchButton.BeginAnimation(FontSizeProperty, fontSizeAnimation);
        }
    }
}
