using CurrencyApp.Services;
using CurrencyApp.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CurrencyApp.Views
{
    public sealed partial class CurrencyListPage : Page
    {
        private CurrencyListViewModel _vm;

        public CurrencyListPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var settings = App.LocalSettings;
            ICurrencyStorageService storage = settings.GetStorageType() == StorageType.Json
                ? (ICurrencyStorageService)new JsonCurrencyService()
                : new SqliteCurrencyService();

            _vm          = new CurrencyListViewModel(storage, settings);
            DataContext  = _vm;

            await _vm.LoadAsync();
        }
    }
}
