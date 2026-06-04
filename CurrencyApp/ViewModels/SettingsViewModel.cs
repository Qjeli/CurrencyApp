using System;
using CurrencyApp.Services;

namespace CurrencyApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly LocalSettingsService _settings;

        private string _lastSession;
        private bool   _isSqlite;
        private bool   _isJson;

        public string LastSession { get => _lastSession; set => SetProperty(ref _lastSession, value); }

        public bool IsSqlite
        {
            get => _isSqlite;
            set
            {
                if (!SetProperty(ref _isSqlite, value)) return;
                if (value)
                {
                    _isJson = false;
                    OnPropertyChanged(nameof(IsJson));
                    _settings.SetStorageType(StorageType.Sqlite);
                    StorageTypeChanged?.Invoke(this, StorageType.Sqlite);
                }
            }
        }

        public bool IsJson
        {
            get => _isJson;
            set
            {
                if (!SetProperty(ref _isJson, value)) return;
                if (value)
                {
                    _isSqlite = false;
                    OnPropertyChanged(nameof(IsSqlite));
                    _settings.SetStorageType(StorageType.Json);
                    StorageTypeChanged?.Invoke(this, StorageType.Json);
                }
            }
        }

        public event EventHandler<StorageType> StorageTypeChanged;

        public SettingsViewModel(LocalSettingsService settings)
        {
            _settings = settings;

            var last = _settings.GetLastSession();
            LastSession = last.HasValue
                ? last.Value.ToString("dd.MM.yyyy HH:mm:ss")
                : "Нет данных — это первый запуск приложения";

            var type = _settings.GetStorageType();
            _isSqlite = type == StorageType.Sqlite;
            _isJson   = type == StorageType.Json;
        }
    }
}
