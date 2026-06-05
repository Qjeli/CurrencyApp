using CurrencyApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;


namespace CurrencyApp.Services
{
    public class JsonCurrencyService : ICurrencyStorageService
    {
        private const string JsonFile = "currencies.json";
        private List<Currency> _cache;
        private int _nextId = 1;

        public async Task InitializeAsync()
        {
            if (_cache != null) return;
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder; // Get the local folder for the application
                var item = await folder.TryGetItemAsync(JsonFile); // Check if the JSON file exists
                if (item is StorageFile file)
                {
                    var json = await FileIO.ReadTextAsync(file);
                    _cache = JsonConvert.DeserializeObject<List<Currency>>(json) ?? new List<Currency>();
                    _nextId = _cache.Count > 0 ? _cache.Max(c => c.Id) + 1 : 1; // Set next ID based on existing currencies
                }
                else
                {
                    _cache = new List<Currency>();
                }
            }
            catch
            {
                _cache = new List<Currency>();
            }
        }

        private async Task SaveAsync()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(JsonFile, CreationCollisionOption.ReplaceExisting);
            var json = JsonConvert.SerializeObject(_cache, Formatting.Indented);
            await FileIO.WriteTextAsync(file, json);
        }

        public async Task<List<Currency>> GetAllCurrenciesAsync()
        {
            await InitializeAsync();
            return new List<Currency>(_cache);
        }

        public async Task SaveApiCurrenciesAsync(List<Currency> apiCurrencies)
        {
            await InitializeAsync();
            var userRows = _cache.Where(c => c.IsUserAdded).ToList();
            _cache = apiCurrencies.Select(c => { c.Id = _nextId++; return c; }).ToList();
            _cache.AddRange(userRows);
            await SaveAsync();
        }

        public async Task AddCurrencyAsync(Currency currency)
        {
            await InitializeAsync();
            currency.Id = _nextId++;
            _cache.Add(currency);
            await SaveAsync();
        }

        public async Task DeleteCurrencyAsync(int id)
        {
            await InitializeAsync();
            _cache.RemoveAll(c => c.Id == id);
            await SaveAsync();
        }

        public async Task<List<Currency>> GetUserCurrenciesAsync()
        {
            await InitializeAsync();
            return _cache.Where(c => c.IsUserAdded).ToList();
        }
    }
}