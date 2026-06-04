using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CurrencyApp.Models;
using CurrencyApp.Services;

namespace CurrencyApp.ViewModels
{
    public class CurrencyListViewModel : BaseViewModel
    {
        private readonly CurrencyApiService _api;
        private readonly LocalSettingsService _settings;
        private ICurrencyStorageService _storage;

        // Observable state
        private ObservableCollection<Currency> _currencies;
        private bool _isLoading;
        private string _statusMessage = "Загрузка...";
        private string _lastUpdated;

        public ObservableCollection<Currency> Currencies
        {
            get => _currencies;
            set => SetProperty(ref _currencies, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public string LastUpdated
        {
            get => _lastUpdated;
            set => SetProperty(ref _lastUpdated, value);
        }

        // Comands

        public ICommand RefreshCommand { get; }
        public ICommand DeleteCommand { get; }

        public CurrencyListViewModel(ICurrencyStorageService storage, LocalSettingsService settings)
        {
            _storage = storage;
            _settings = settings;
            _api = new CurrencyApiService();
            _currencies = new ObservableCollection<Currency>();

            RefreshCommand = new RelayCommand(async () => await RefreshFromApiAsync());
            DeleteCommand = new RelayCommand(async obj => await DeleteAsync(obj));
        }

        // Public API
        public async Task LoadAsync()
        {
            IsLoading = true;
            try
            {
                await _storage.InitializeAsync();
                var list = await _storage.GetAllCurrenciesAsync();

                if (list.Count == 0)
                {
                    await RefreshFromApiAsync();
                    return;
                }

                Reload(list);
                StatusMessage = $"Загружено {Currencies.Count} валют из хранилища.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task RefreshFromApiAsync()
        {
            IsLoading = true;
            StatusMessage = "Загрузка данных из API...";
            try
            {
                var apiData = await _api.FetchCurrenciesAsync();
                await _storage.SaveApiCurrenciesAsync(apiData);

                var all = await _storage.GetAllCurrenciesAsync();
                Reload(all);

                LastUpdated = $"Последнее обновление: {DateTime.Now:dd.MM.yyyy HH:mm:ss}";
                StatusMessage = $"Данные обновлены. Всего валют: {Currencies.Count}.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Нет сети, берем данные из хранилища: {ex.Message}";
                var local = await _storage.GetAllCurrenciesAsync();
                Reload(local);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void SwitchStorage(ICurrencyStorageService newStorage)
        {
            _storage = newStorage;
        }

        // Internal helpers
        private void Reload(System.Collections.Generic.List<Currency> list)
        {
            Currencies.Clear();
            foreach (var item in list)
            {
                Currencies.Add(item);
            }
        }

        private async Task DeleteAsync(object obj)
        {
            if (obj is Currency c)
            {
                await _storage.DeleteCurrencyAsync(c.Id);
                Currencies.Remove(c);
                StatusMessage = $"Валюта {c.CharCode} удалена.";
            }
        }
    }
}