namespace kurs.Models
{
    public class ReportViewModel
    {
        public string PokupatelFio { get; set; }
        public decimal TotalSum { get; set; }
        public decimal AverageOrderSum { get; set; }
        public int OrderCount { get; set; }
        public decimal MaxOrderSum { get; set; }
        public decimal MinOrderSum { get; set; }
    }
}
