
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using kurs.Models;
//using kurs.Context;

//namespace kurs.Controllers
//{
//    public class DostavkaController : Controller
//    {
//        private readonly MagazinminContext _context;

//        public DostavkaController(MagazinminContext context)
//        {
//            _context = context;
//        }

//        //public async Task<IActionResult> Index()
//        //{
//        //    return View(await _context.Dostavka.ToListAsync());
//        //}
//        public async Task<IActionResult> Index(string sortOrder)
//        {
//            ViewData["DataPolucheniyaSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
//            ViewData["OjdaemayaDataSortParm"] = sortOrder == "date" ? "date_desc" : "date";
//            ViewData["StatusDostavkiSortParm"] = sortOrder == "status" ? "status_desc" : "status";

//            var dostavkas = from d in _context.Dostavka
//                            select d;

//            switch (sortOrder)
//            {
//                case "date_desc":
//                    dostavkas = dostavkas.OrderByDescending(d => d.DataPolucheniya);
//                    break;
//                case "date":
//                    dostavkas = dostavkas.OrderBy(d => d.OjdaemayaData);
//                    break;
//                case "status_desc":
//                    dostavkas = dostavkas.OrderByDescending(d => d.StatusDostavki);
//                    break;
//                case "status":
//                    dostavkas = dostavkas.OrderBy(d => d.StatusDostavki);
//                    break;
//                default:
//                    dostavkas = dostavkas.OrderBy(d => d.DataPolucheniya);
//                    break;
//            }

//            return View(await dostavkas.AsNoTracking().ToListAsync());
//        }
//        public IActionResult Create()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("DataPolucheniya,OjdaemayaData,StatusDostavki")] Dostavka dostavka)
//        {
//            if (ModelState.IsValid)
//            {
//                dostavka.DataPolucheniya = DateTime.SpecifyKind(dostavka.DataPolucheniya, DateTimeKind.Utc);
//                dostavka.OjdaemayaData = DateTime.SpecifyKind(dostavka.OjdaemayaData, DateTimeKind.Utc);
//                _context.Add(dostavka);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(dostavka);
//        }

//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var dostavka = await _context.Dostavka.FindAsync(id);
//            if (dostavka == null)
//            {
//                return NotFound();
//            }
//            return View(dostavka);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("IdDostavka,DataPolucheniya,OjdaemayaData,StatusDostavki")] Dostavka dostavka)
//        {
//            if (id != dostavka.IdDostavka)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    dostavka.DataPolucheniya = DateTime.SpecifyKind(dostavka.DataPolucheniya, DateTimeKind.Utc);
//                    dostavka.OjdaemayaData = DateTime.SpecifyKind(dostavka.OjdaemayaData, DateTimeKind.Utc);
//                    _context.Update(dostavka);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!DostavkaExists(dostavka.IdDostavka))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(dostavka);
//        }

//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var dostavka = await _context.Dostavka
//                .FirstOrDefaultAsync(m => m.IdDostavka == id);
//            if (dostavka == null)
//            {
//                return NotFound();
//            }

//            return View(dostavka);
//        }

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var dostavka = await _context.Dostavka.FindAsync(id);
//            _context.Dostavka.Remove(dostavka);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool DostavkaExists(int id)
//        {
//            return _context.Dostavka.Any(e => e.IdDostavka == id);
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kurs.Models;
using kurs.Context;
using Npgsql;

namespace kurs.Controllers
{
    public class DostavkaController : Controller
    {
        private readonly MagazinminContext _context;

        public DostavkaController(MagazinminContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string sortOrder, string searchReceivedDate, string searchExpectedDate, string searchStatus)
        {
            ViewData["DataPolucheniyaSortParm"] = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["OjdaemayaDataSortParm"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["StatusDostavkiSortParm"] = sortOrder == "status" ? "status_desc" : "status";
            ViewData["IdSortParm"] = sortOrder == "id_asc" ? "id_desc" : "id_asc";


            ViewData["CurrentSearchReceivedDate"] = searchReceivedDate;
            ViewData["CurrentSearchExpectedDate"] = searchExpectedDate;
            ViewData["CurrentSearchStatus"] = searchStatus;

            var dostavkas = from d in _context.Dostavka select d;

            // Apply search filters
          

            if (!String.IsNullOrEmpty(searchStatus))
            {
                dostavkas = dostavkas.Where(d => d.StatusDostavki.Contains(searchStatus));
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "id_asc":
                    dostavkas = dostavkas.OrderBy(t => t.IdDostavka);
                    break;
                case "date_desc":
                    dostavkas = dostavkas.OrderByDescending(d => d.DataPolucheniya);
                    break;
                case "date":
                    dostavkas = dostavkas.OrderBy(d => d.OjdaemayaData);
                    break;
                case "status_desc":
                    dostavkas = dostavkas.OrderByDescending(d => d.StatusDostavki);
                    break;
                case "status":
                    dostavkas = dostavkas.OrderBy(d => d.StatusDostavki);
                    break;
                default:
                    dostavkas = dostavkas.OrderBy(d => d.DataPolucheniya);
                    break;
            }

            return View(await dostavkas.AsNoTracking().ToListAsync());
        }

        // GET: Dostavka/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DataPolucheniya,OjdaemayaData,StatusDostavki")] Dostavka dostavka)
        {
            if (ModelState.IsValid)
            {
                dostavka.DataPolucheniya = DateTime.SpecifyKind(dostavka.DataPolucheniya, DateTimeKind.Utc);
                dostavka.OjdaemayaData = DateTime.SpecifyKind(dostavka.OjdaemayaData, DateTimeKind.Utc);
                _context.Add(dostavka);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dostavka);
        }

        // GET: Dostavka/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dostavka = await _context.Dostavka.FindAsync(id);
            if (dostavka == null)
            {
                return NotFound();
            }
            return View(dostavka);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDostavka,DataPolucheniya,OjdaemayaData,StatusDostavki")] Dostavka dostavka)
        {
            if (id != dostavka.IdDostavka)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dostavka.DataPolucheniya = DateTime.SpecifyKind(dostavka.DataPolucheniya, DateTimeKind.Utc);
                    dostavka.OjdaemayaData = DateTime.SpecifyKind(dostavka.OjdaemayaData, DateTimeKind.Utc);
                    _context.Update(dostavka);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DostavkaExists(dostavka.IdDostavka))
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
            return View(dostavka);
        }

        // GET: Dostavka/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dostavka = await _context.Dostavka
                .FirstOrDefaultAsync(m => m.IdDostavka == id);
            if (dostavka == null)
            {
                return NotFound();
            }

            return View(dostavka);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dostavka = await _context.Dostavka.FindAsync(id);
            if (dostavka != null)
            {
                try
                {
                    _context.Dostavka.Remove(dostavka);
                    await _context.SaveChangesAsync();
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

        private bool DostavkaExists(int id)
        {
            return _context.Dostavka.Any(e => e.IdDostavka == id);
        }


        public async Task<IActionResult> Orders(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dostavka = await _context.Dostavka
                .Include(d => d.Zakaz) // Включаем связанные заказы
                    //.ThenInclude(z => z.Tovar) // Включаем связанные товары для каждого заказа
                .FirstOrDefaultAsync(m => m.IdDostavka == id);

            if (dostavka == null)
            {
                return NotFound();
            }

            return View(dostavka);
        }
    }
}




