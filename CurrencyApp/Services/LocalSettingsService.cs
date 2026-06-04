using SQLitePCL;
using System;
using Windows.Storage;

namespace CurrencyApp.Services
{
    public enum StorageType { Sqlite, Json}

    public class LocalSettingsService
    {
        private const string KeyLastSession = "LastSession";
        private const string KeyStorageType = "StorageType";

        private readonly ApplicationDataContainer _settings =
            ApplicationData.Current.LocalSettings;

        // Last Session
        public DateTime? GetLastSession()
        {
            if (_settings.Values.TryGetValue(KeyLastSession, out var value) && value is string s
                && DateTime.TryParse(s, out var dt))
                return dt;

            return null;
        }

        public void SaveCurrentSession()
            => _settings.Values[KeyLastSession] = DateTime.Now.ToString("O");

        // Storage Type
        public StorageType GetStorageType()
        {
            if (_settings.Values.TryGetValue(KeyStorageType, out var value) && value is string s
                && Enum.TryParse<StorageType>(s, out var t))
                return t;
            return StorageType.Sqlite;
        }
        public void SetStorageType(StorageType type)
            => _settings.Values[KeyStorageType] = type.ToString();
    }
}