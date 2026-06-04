using CurrencyApp.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyApp.Services
{
    public class SqliteCurrencyService : ICurrencyStorageService
    {
        private SQLiteAsyncConnection _db;
        private const string DbFile = "currencies.db";

        // Initializes the SQLite connection and ensures the CurrencyModel table exists
        public async Task InitializeAsync()
        {
            if (_db != null) return;
            var path = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, DbFile); // Get the path to the local folder and combine it with the database file name
            _db = new SQLiteAsyncConnection(path);
            await _db.CreateTableAsync<Currency>();
        }

        // Retrieves all currency records from the database
        public async Task<List<Currency>> GetAllCurrenciesAsync()
        {
            await InitializeAsync();
            return await _db.Table<Currency>().ToListAsync();
        }

        // Saves the provided list of API currencies to the database, replacing any existing non-user-added records
        public async Task SaveApiCurrenciesAsync(List<Currency> apiCurrencies)
        {
            await InitializeAsync();
            // Delete existing non-user rows
            var oldApiRows = await _db.Table<Currency>().Where(c => !c.IsUserAdded).ToListAsync();
            foreach (var row in oldApiRows)
                await _db.DeleteAsync(row);
            // Insert fresh API rows
            foreach (var c in apiCurrencies)
            {
                c.Id = 0; // let autoincrement handle it
                await _db.InsertAsync(c);
            }
        }

        // Adds a new currency record to the database
        public async Task AddCurrencyAsync(Currency currency)
        {
            await InitializeAsync();
            currency.Id = 0;
            await _db.InsertAsync(currency);
        }

        // Deletes a currency record from the database based on its ID
        public async Task DeleteCurrencyAsync(int id)
        {
            await InitializeAsync();
            await _db.DeleteAsync<Currency>(id);
        }

        // Retrieves only user-added currency records from the database
        public async Task<List<Currency>> GetUserCurrenciesAsync()
        {
            await InitializeAsync();
            return await _db.Table<Currency>().Where(c => c.IsUserAdded).ToListAsync();
        }
    }
}