using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyApp.Models;

namespace CurrencyApp.Services
{
    public interface ICurrencyStorageService
    {
        Task InitializeAsync();
        Task<List<Currency>> GetAllCurrenciesAsync();
        Task SaveApiCurrenciesAsync(List<Currency> apiCurrencies);
        Task AddCurrencyAsync(Currency currency);
        Task DeleteCurrencyAsync(int id);
        Task<List<Currency>> GetUserCurrenciesAsync();
    }
}
