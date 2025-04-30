
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kurs.Models;
using kurs.Context;
using Npgsql;

namespace kurs.Controllers
{
    public class SborschikZakazaController : Controller
    {
        private readonly MagazinminContext _context;

        public SborschikZakazaController(MagazinminContext context)
        {
            _context = context;
        }

       

        public async Task<IActionResult> Index(string sortOrder, string searchPhone, string searchName)
        {
            ViewData["PhoneSortParm"] = String.IsNullOrEmpty(sortOrder) ? "phone_desc" : "";
            ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["IdSortParm"] = sortOrder == "id_asc" ? "id_desc" : "id_asc";


            ViewData["CurrentSearchPhone"] = searchPhone;
            ViewData["CurrentSearchName"] = searchName;

            var sborschikZakazas = from s in _context.SborschikZakaza select s;

            if (!String.IsNullOrEmpty(searchPhone))
            {
                sborschikZakazas = sborschikZakazas.Where(s => s.SborschikZakazaTelefon.ToString().Contains(searchPhone));
            }

            if (!String.IsNullOrEmpty(searchName))
            {
                sborschikZakazas = sborschikZakazas.Where(s => s.SborschikZakazaNames.Contains(searchName));
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "phone_desc":
                    sborschikZakazas = sborschikZakazas.OrderByDescending(s => s.SborschikZakazaTelefon);
                    break;
                case "name":
                    sborschikZakazas = sborschikZakazas.OrderBy(s => s.SborschikZakazaNames);
                    break;
                case "name_desc":
                    sborschikZakazas = sborschikZakazas.OrderByDescending(s => s.SborschikZakazaNames);
                    break;
                case "id_asc":
                    sborschikZakazas = sborschikZakazas.OrderBy(z => z.IdSborschikZakaza);
                    break;
                default:
                    sborschikZakazas = sborschikZakazas.OrderBy(s => s.SborschikZakazaTelefon);
                    break;
            }

            return View(await sborschikZakazas.AsNoTracking().ToListAsync());
        }
        // GET: SborschikZakaza/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SborschikZakaza == null)
            {
                return NotFound();
            }

            var sborschikZakaza = await _context.SborschikZakaza
                .FirstOrDefaultAsync(m => m.IdSborschikZakaza == id);
            if (sborschikZakaza == null)
            {
                return NotFound();
            }

            return View(sborschikZakaza);
        }

        // GET: SborschikZakaza/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SborschikZakaza/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSborschikZakaza,SborschikZakazaTelefon,SborschikZakazaNames")] SborschikZakaza sborschikZakaza)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sborschikZakaza);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sborschikZakaza);
        }

        // GET: SborschikZakaza/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SborschikZakaza == null)
            {
                return NotFound();
            }

            var sborschikZakaza = await _context.SborschikZakaza.FindAsync(id);
            if (sborschikZakaza == null)
            {
                return NotFound();
            }
            return View(sborschikZakaza);
        }

        // POST: SborschikZakaza/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSborschikZakaza,SborschikZakazaTelefon,SborschikZakazaNames")] SborschikZakaza sborschikZakaza)
        {
            if (id != sborschikZakaza.IdSborschikZakaza)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sborschikZakaza);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SborschikZakazaExists(sborschikZakaza.IdSborschikZakaza))
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
            return View(sborschikZakaza);
        }

        // GET: SborschikZakaza/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SborschikZakaza == null)
            {
                return NotFound();
            }

            var sborschikZakaza = await _context.SborschikZakaza
                .FirstOrDefaultAsync(m => m.IdSborschikZakaza == id);
            if (sborschikZakaza == null)
            {
                return NotFound();
            }

            return View(sborschikZakaza);
        }

        // POST: SborschikZakaza/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SborschikZakaza == null)
            {
                return Problem("Entity set 'MagazinminContext.SborschikZakaza' is null.");
            }

            var sborschikZakaza = await _context.SborschikZakaza.FindAsync(id);
            if (sborschikZakaza != null)
            {
                try
                {
                    _context.SborschikZakaza.Remove(sborschikZakaza);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23503")
                {
                    // Перенаправление на страницу с предупреждением об ошибке
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

        private bool SborschikZakazaExists(int id)
        {
            return (_context.SborschikZakaza?.Any(e => e.IdSborschikZakaza == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> Orders(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sborschikZakaza = await _context.SborschikZakaza
                .Include(p => p.Zakaz)
                .FirstOrDefaultAsync(m => m.IdSborschikZakaza == id);

            if (sborschikZakaza == null)
            {
                return NotFound();
            }

            return View(sborschikZakaza);
        }
    }
}
