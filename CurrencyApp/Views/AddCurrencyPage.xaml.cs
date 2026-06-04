using CurrencyApp.Services;
using CurrencyApp.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CurrencyApp.Views
{
    public sealed partial class AddCurrencyPage : Page
    {
        public AddCurrencyPage() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var settings = App.LocalSettings;
            ICurrencyStorageService storage = settings.GetStorageType() == StorageType.Json
                ? (ICurrencyStorageService)new JsonCurrencyService()
                : new SqliteCurrencyService();

            DataContext = new AddCurrencyViewModel(storage, settings);
        }
    }
}
