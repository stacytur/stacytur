
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using kurs.Context;
using kurs.Models;
using Npgsql;
using System;

namespace kurs.Controllers
{
    public class ZakazController : Controller
    {
        private readonly MagazinminContext _context;

        public ZakazController(MagazinminContext context)
        {
            _context = context;
        }

        
        public IActionResult Index(string sortOrder, DateTime? searchDate, decimal? searchSum)
        {
            ViewData["DateSortParm"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewData["SumSortParm"] = sortOrder == "sum_asc" ? "sum_desc" : "sum_asc";
            ViewData["IdSortParm"] = sortOrder == "id_asc" ? "id_desc" : "id_asc";

            var zakazs = from z in _context.Zakaz select z;

            // Apply search filters
            if (searchDate.HasValue)
            {
                // Преобразуйте дату поиска в UTC перед сравнением
                var utcSearchDate = searchDate.Value.ToUniversalTime();
                zakazs = zakazs.Where(z => z.ZakazData.Date == utcSearchDate.Date);
            }
            if (searchSum.HasValue)
            {
                // Необходимо преобразовать сумму также, если она хранится как decimal
                zakazs = zakazs.Where(z => z.ZakazSumma == searchSum.Value);
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "date_desc":
                    zakazs = zakazs.OrderByDescending(z => z.ZakazData);
                    break;
                case "date_asc":
                    zakazs = zakazs.OrderBy(z => z.ZakazData);
                    break;
                case "sum_desc":
                    zakazs = zakazs.OrderByDescending(z => z.ZakazSumma);
                    break;
                case "sum_asc":
                    zakazs = zakazs.OrderBy(z => z.ZakazSumma);
                    break;
                case "id_desc":
                    zakazs = zakazs.OrderByDescending(z => z.IdZakaz);
                    break;
                case "id_asc":
                    zakazs = zakazs.OrderBy(z => z.IdZakaz);
                    break;
                default:
                    zakazs = zakazs.OrderBy(z => z.ZakazData);
                    break;
            }

            return View(zakazs.ToList());
        }

        // GET: Zakaz/Add
        public IActionResult Add()
        {
            ViewData["IdDostavka"] = new SelectList(_context.Dostavka, "IdDostavka", "IdDostavka");
            ViewData["IdPokupatel"] = new SelectList(_context.Pokupatel, "IdPokupatel", "PokupatelFio");
            ViewData["IdSborschikZakaza"] = new SelectList(_context.SborschikZakaza, "IdSborschikZakaza", "SborschikZakazaNames");
            return View();
        }

        [HttpPost]
        public IActionResult Add(Zakaz zakaz)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Убедитесь, что ZakazData установлен как UTC
                    zakaz.ZakazData = DateTime.SpecifyKind(zakaz.ZakazData, DateTimeKind.Utc);

                    _context.Zakaz.Add(zakaz);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    // Зарегистрируйте исключение или обработайте его соответственно
                    ModelState.AddModelError("", "Произошла ошибка при сохранении заказа. Пожалуйста, попробуйте позже.");
                    // Вы также можете зарегистрировать подробности исключения для целей отладки
                    Console.WriteLine(ex.InnerException?.Message);
                }
            }
            ViewData["IdDostavka"] = new SelectList(_context.Dostavka, "IdDostavka", "IdDostavka", zakaz.IdDostavka);
            ViewData["IdPokupatel"] = new SelectList(_context.Pokupatel, "IdPokupatel", "PokupatelFio", zakaz.IdPokupatel);
            ViewData["IdSborschikZakaza"] = new SelectList(_context.SborschikZakaza, "IdSborschikZakaza", "SborschikZakazaNames", zakaz.IdSborschikZakaza);
            return View(zakaz);
        }

       
        // GET: Zakaz/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zakaz = await _context.Zakaz
                .Include(z => z.IdDostavkaNavigation)
                .Include(z => z.IdPokupatelNavigation)
                .Include(z => z.IdSborschikZakazaNavigation)
                .FirstOrDefaultAsync(m => m.IdZakaz == id);

            if (zakaz == null)
            {
                return NotFound();
            }

            ViewData["IdDostavka"] = new SelectList(_context.Dostavka, "IdDostavka", "IdDostavka", zakaz.IdDostavka);
            ViewData["IdPokupatel"] = new SelectList(_context.Pokupatel, "IdPokupatel", "PokupatelFio", zakaz.IdPokupatel);
            ViewData["IdSborschikZakaza"] = new SelectList(_context.SborschikZakaza, "IdSborschikZakaza", "SborschikZakazaNames", zakaz.IdSborschikZakaza);
            return View(zakaz);
        }

        // POST: Zakaz/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdZakaz,ZakazData,ZakazSumma,IdPokupatel,IdSborschikZakaza,IdDostavka")] Zakaz zakaz)
        {
            if (id != zakaz.IdZakaz)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Убедитесь, что ZakazData установлен как UTC
                    zakaz.ZakazData = DateTime.SpecifyKind(zakaz.ZakazData, DateTimeKind.Utc);

                    _context.Update(zakaz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZakazExists(zakaz.IdZakaz))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDostavka"] = new SelectList(_context.Dostavka, "IdDostavka", "IdDostavka", zakaz.IdDostavka);
            ViewData["IdPokupatel"] = new SelectList(_context.Pokupatel, "IdPokupatel", "PokupatelFio", zakaz.IdPokupatel);
            ViewData["IdSborschikZakaza"] = new SelectList(_context.SborschikZakaza, "IdSborschikZakaza", "SborschikZakazaNames", zakaz.IdSborschikZakaza);
            return View(zakaz);
        }

        private bool ZakazExists(int id)
        {
            return _context.Zakaz.Any(e => e.IdZakaz == id);
        }

        // GET: Zakaz/Delete/5
        public IActionResult Delete(int id)
        {
            var zakaz = _context.Zakaz.Find(id);
            if (zakaz == null) return NotFound();

            return View(zakaz);
        }

        // POST: Zakaz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var zakaz = _context.Zakaz.Find(id);
            if (zakaz != null)
            {
                try
                {
                    _context.Zakaz.Remove(zakaz);
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23503")
                {
                    // Перенаправление на страницу с предупреждением об ошибке
                    return RedirectToAction(nameof(DeleteError));
                }
                catch (Exception ex)
                {
                    // Логирование и обработка других исключений
                    Console.WriteLine("Unexpected error details: " + ex.ToString());
                    return RedirectToAction(nameof(DeleteError));
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // Страница для отображения ошибки удаления
        public IActionResult DeleteError()
        {
            return View();
        }

        public async Task<IActionResult> Orders(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zakaz = await _context.Zakaz
                .Include(p => p.Tovar)
                .FirstOrDefaultAsync(m => m.IdZakaz == id);

            if (zakaz == null)
            {
                return NotFound();
            }

            return View(zakaz);
        }

        public IActionResult Reports()
        {
            // Общая сумма заказов
            var totalSum = _context.Zakaz.Sum(z => z.ZakazSumma);
            ViewData["TotalSum"] = totalSum;

            // Количество заказов по дате
            var orderCountByDate = _context.Zakaz
                .GroupBy(z => z.ZakazData.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToList();
            ViewData["OrderCountByDate"] = orderCountByDate;

            // Средняя сумма заказов
            var averageSum = _context.Zakaz.Average(z => z.ZakazSumma);
            ViewData["AverageSum"] = averageSum;

           
            // Количество заказов по покупателю
            var orderCountByCustomer = _context.Zakaz
                .GroupBy(z => new { z.IdPokupatel, z.IdPokupatelNavigation.PokupatelFio })
                .Select(g => new
                {
                    g.Key.PokupatelFio,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();
            ViewData["OrderCountByCustomer"] = orderCountByCustomer;

            return View();
        }
        // GET: Zakaz/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zakaz = await _context.Zakaz
                .Include(z => z.IdPokupatelNavigation)
                .Include(z => z.IdSborschikZakazaNavigation)
                .Include(z => z.IdDostavkaNavigation)
                .FirstOrDefaultAsync(z => z.IdZakaz == id);

            if (zakaz == null)
            {
                return NotFound();
            }

            return View(zakaz);
        }
    }
}

