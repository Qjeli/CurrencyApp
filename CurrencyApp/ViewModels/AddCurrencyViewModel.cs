using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using CurrencyApp.Models;
using CurrencyApp.Services;

namespace CurrencyApp.ViewModels
{
    public class AddCurrencyViewModel : BaseViewModel
    {
        private ICurrencyStorageService _storage;

        // ── Form fields ───────────────────────────────────────────────
        private string _charCode;
        private string _numCode;
        private string _name;
        private string _valueText;
        private string _nominalText = "1";
        private string _errorMessage;
        private string _successMessage;

        // ── Properties ───────────────────────────────────────────────
        public string CharCode { get => _charCode; set => SetProperty(ref _charCode, value); }
        public string NumCode { get => _numCode; set => SetProperty(ref _numCode, value); }
        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public string ValueText { get => _valueText; set => SetProperty(ref _valueText, value); }
        public string NominalText { get => _nominalText; set => SetProperty(ref _nominalText, value); }
        public string ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }
        public string SuccessMessage { get => _successMessage; set => SetProperty(ref _successMessage, value); }

        public ICommand AddCommand { get; }

        public AddCurrencyViewModel(ICurrencyStorageService storage, LocalSettingsService _)
        {
            _storage = storage;
            AddCommand = new RelayCommand(async () => await AddAsync());
        }

        public void SwitchStorage(ICurrencyStorageService newStorage) => _storage = newStorage;

        private async Task AddAsync()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            // Validation
            if (string.IsNullOrWhiteSpace(CharCode)) { ErrorMessage = "Укажите код валюты (например, USDT)"; return; }
            if (string.IsNullOrWhiteSpace(Name)) { ErrorMessage = "Укажите название валюты"; return; }

            // Changing comma to dot for invariant parsing, and validating that it's a positive number
            var valueStr = ValueText?.Replace(',', '.') ?? "";
            if (!double.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) || value <= 0)
            { ErrorMessage = "Введите корректный курс (число > 0)"; return; }

            if (!int.TryParse(NominalText, out int nominal) || nominal <= 0)
            { ErrorMessage = "Номинал должен быть целым числом > 0"; return; }

            var currency = new Currency
            {
                CharCode = CharCode.ToUpperInvariant().Trim(),
                NumCode = NumCode?.Trim() ?? "000",
                Name = Name.Trim(),
                Value = value,
                Nominal = nominal,
                Previous = value,
                IsUserAdded = true
            };

            await _storage.InitializeAsync();
            await _storage.AddCurrencyAsync(currency);

            SuccessMessage = $"Валюта {currency.CharCode} добавлена";
            CharCode = NumCode = Name = ValueText = string.Empty;
            NominalText = "1";
        }

    }
}