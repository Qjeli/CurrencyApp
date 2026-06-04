using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace CurrencyApp.Models
{
    public class CbrResponse
    {
        [JsonProperty("Date")]
        public DateTime Date { get; set; }
        [JsonProperty("PreviousDate")]
        public DateTime PreviousDate { get; set; }
        [JsonProperty("Timestamp")]
        public DateTime Timestamp { get; set; }
        [JsonProperty("Valute")]
        public Dictionary<string, CbrValute> Valute { get; set; }
    }
    public class CbrValute
    {
        [JsonProperty("ID")]
        public string ID { get; set; }
        [JsonProperty("NumCode")]
        public string NumCode { get; set; }
        [JsonProperty("CharCode")]
        public string CharCode { get; set; }
        [JsonProperty("Nominal")]
        public int Nominal { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Value")]
        public double Value { get; set; }
        [JsonProperty("Previous")]
        public double Previous { get; set; }
    }
}