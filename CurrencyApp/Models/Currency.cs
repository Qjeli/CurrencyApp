using SQLite;

namespace CurrencyApp.Models
{
    [Table("Currencies")]
    public class Currency
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string CharCode { get; set; } = string.Empty;
        public string NumCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Value { get; set; }
        public int Nominal { get; set; } = 1;
        public double Previous { get; set; }
        public bool IsUserAdded { get; set; }

        [Ignore]
        public double Change => Value - Previous;

        [Ignore]
        public string NominalStr => Nominal > 1 ? $"Номинал: {Nominal}" : "Номинал: 1";
    }
}