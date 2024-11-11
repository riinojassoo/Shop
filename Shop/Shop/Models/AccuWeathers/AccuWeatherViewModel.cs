namespace Shop.Models.AccuWeathers
{
    public class AccuWeatherViewModel
    {
        public string CityName { get; set; }
        public string EffectiveDate { get; set; }
        public Int64 EffectiveEpochDate { get; set; }
        public int Severity { get; set; }
        public string Text { get; set; }
        public string Category {  get; set; }
        public string EndDate { get; set; }
        public Int64 EndEpochDate { get; set; }
        public double TempMinValue { get; set; }
        public double TempMaxValue { get; set; }

    }
}
