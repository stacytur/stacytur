using kurs.Context;
using kurs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kurs.Controllers
{
    public class ReportController : Controller
    {
        private readonly MagazinminContext _context;

        public ReportController(MagazinminContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reports = await _context.Pokupatel
                .Include(p => p.Zakaz)
                .Select(p => new ReportViewModel
                {
                    PokupatelFio = p.PokupatelFio,
                    TotalSum = p.Zakaz.Sum(z => (decimal?)z.ZakazSumma) ?? 0,
                    AverageOrderSum = p.Zakaz.Average(z => (decimal?)z.ZakazSumma) ?? 0,
                    OrderCount = p.Zakaz.Count(),
                    MaxOrderSum = p.Zakaz.Max(z => (decimal?)z.ZakazSumma) ?? 0,
                    MinOrderSum = p.Zakaz.Min(z => (decimal?)z.ZakazSumma) ?? 0
                })
                .ToListAsync();

            return View(reports);
        }
        public async Task<IActionResult> DeliveryReport()
        {
            var deliveryReports = await _context.Dostavka
                .Include(d => d.Zakaz)
                .Select(d => new DeliveryReportViewModel
                {
                    IdDostavka = d.IdDostavka,
                    DataPolucheniya = d.DataPolucheniya,
                    OjdaemayaData = d.OjdaemayaData,
                    StatusDostavki = d.StatusDostavki,
                    OrderCount = d.Zakaz.Count()
                })
                .ToListAsync();

            return View(deliveryReports);
        }

        public async Task<IActionResult> SborschikReport()
        {
            var sborschikReports = await _context.SborschikZakaza
                .Include(s => s.Zakaz)
                .Select(s => new SborschikReportViewModel
                {
                    IdSborschikZakaza = s.IdSborschikZakaza,
                    SborschikZakazaNames = s.SborschikZakazaNames,
                    SborschikZakazaTelefon = s.SborschikZakazaTelefon,
                    OrderCount = s.Zakaz.Count()
                })
                .ToListAsync();

            return View(sborschikReports);
        }
        public async Task<IActionResult> TovarReport()
        {
            var tovars = await _context.Tovar
                .Include(t => t.IdZakazNavigation)
                .ToListAsync();

            return View(tovars);
        }


        public IActionResult Index1()
        {
            return View();
        }
    }
}
    
    
