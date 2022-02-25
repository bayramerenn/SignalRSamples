using System;

namespace CovidChart.API.Models
{
    public class Covid
    {
        public int Id { get; set; }
        public ECity ECity { get; set; }
        public int Count { get; set; }
        public DateTime CovidDate{ get; set; }

    }
    public enum ECity
    {
        Istanbul = 1,
        Ankara = 2,
        Izmir = 3,
        Giresun = 4,
        Antalya = 5
    }
}
