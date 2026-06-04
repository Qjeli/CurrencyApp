using CurrencyApp.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CurrencyApp.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = new SettingsViewModel(App.LocalSettings);
        }
    }
}
