using System;
using CurrencyApp.Views;
using Windows.UI.Xaml.Controls;

namespace CurrencyApp
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                // Select first item on load
                NavView.SelectedItem = NavView.MenuItems[0];
                ContentFrame.Navigate(typeof(CurrencyListPage));
            };
        }

        private void NavView_SelectionChanged(NavigationView sender,
                                              NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is NavigationViewItem item)
            {
                var tag = item.Tag?.ToString();
                Type pageType;

                if (tag == "CurrencyListPage")
                    pageType = typeof(CurrencyListPage);
                else if (tag == "AddCurrencyPage")
                    pageType = typeof(AddCurrencyPage);
                else if (tag == "SettingsPage")
                    pageType = typeof(SettingsPage);
                else
                    pageType = typeof(CurrencyListPage);

                ContentFrame.Navigate(pageType);
            }
        }
    }
}
