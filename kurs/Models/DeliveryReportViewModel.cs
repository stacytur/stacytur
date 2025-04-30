namespace kurs.Models
{
    public class DeliveryReportViewModel
    {
        public int IdDostavka { get; set; }
        public DateTime DataPolucheniya { get; set; }
        public DateTime OjdaemayaData { get; set; }
        public string? StatusDostavki { get; set; }
        public int OrderCount { get; set; }
    }
}
