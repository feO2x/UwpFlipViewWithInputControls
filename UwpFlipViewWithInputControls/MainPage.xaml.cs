using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LightInject;

namespace UwpFlipViewWithInputControls
{
    public sealed partial class MainPage
    {
        private readonly IServiceContainer _serviceContainer;

        public MainPage(IServiceContainer serviceContainer)
            : this()
        {
            if (serviceContainer == null) throw new ArgumentNullException(nameof(serviceContainer));

            _serviceContainer = serviceContainer;
            CurrentView.Content = _serviceContainer.GetInstance<UserControl>(nameof(ExampleViews.FlipViewWithImages));
        }

        public MainPage()
        {
            InitializeComponent();
        }

        private void ShowNewView(object sender, RoutedEventArgs e)
        {
            var sourceButton = (Button) sender;
            CurrentView.Content = _serviceContainer.GetInstance<UserControl>((string) sourceButton.Tag);
        }
    }
}
