using CurrencyApp.Services;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CurrencyApp
{
    sealed partial class App : Application
    {
        private static LocalSettingsService _localSettings;
        public static LocalSettingsService LocalSettings
        {
            get
            {
                if (_localSettings == null)
                {
                    _localSettings = new LocalSettingsService();
                }
                return _localSettings;
            }
        }

        public App() => InitializeComponent();

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // Save current session to LocalSettings
            LocalSettings.SaveCurrentSession();

            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += (s, ex) =>
                    throw new System.Exception("Navigation failed: " + ex.SourcePageType.FullName);

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated) return;

            if (rootFrame.Content == null)
                rootFrame.Navigate(typeof(MainPage), e.Arguments);

            Window.Current.Activate();
        }
    }
}
