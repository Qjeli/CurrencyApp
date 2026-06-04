using SQLite;
using Windows.UI;

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

        [Ignore]
        public string ValueFormatted => Value.ToString("F4");

        [Ignore]
        public string ChangeStr
        {
            get
            {
                var change = Change;
                var sign = change > 0 ? "+" : "";
                return $"{sign}{change:F4}";
            }
        }

        [Ignore]
        public Color BadgeColor
        {
            get
            {
                if (Change > 0)
                    return Colors.Green;
                else if (Change < 0)
                    return Colors.Red;
                else
                    return Colors.Gray;
            }
        }

        [Ignore]
        public Color ChangeColor => BadgeColor;
    }
}