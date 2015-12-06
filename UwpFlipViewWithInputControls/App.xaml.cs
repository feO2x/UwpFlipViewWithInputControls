using LightInject;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UwpFlipViewWithInputControls.ExampleViews;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UwpFlipViewWithInputControls
{
    sealed partial class App
    {
        private ServiceContainer _diContainer;

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            if (_diContainer == null)
            {
                _diContainer = new ServiceContainer();
                RegisterViewsWithDiContainer();
                Window.Current.Content = _diContainer.GetInstance<MainPage>();
            }
            Window.Current.Activate();
        }

        private void RegisterViewsWithDiContainer()
        {
            _diContainer.Register(f => new MainPage(_diContainer), new PerContainerLifetime());
            RegisterAllViewsWithinNamespace(typeof (ExampleViewsNamespaceMarker));
        }

        private void RegisterAllViewsWithinNamespace(Type markerType)
        {
            var targetNamespace = markerType.Namespace;
            var typeInfo = markerType.GetTypeInfo();
            var assembly = typeInfo.Assembly;
            foreach (var viewType in assembly.GetTypes().Where(t => t.Namespace == targetNamespace))
            {
                _diContainer.Register(typeof(UserControl), viewType, viewType.Name, new PerContainerLifetime());
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
