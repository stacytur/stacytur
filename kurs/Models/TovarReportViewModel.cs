namespace kurs.Models
{
    public class TovarReportViewModel
    {
        public int IdTovar { get; set; }
        public string TovarNaimenovanie { get; set; }
        public decimal? Cena { get; set; }
        public int? Kolichestvo { get; set; }
        public int IdZakaz { get; set; }


    }
}
