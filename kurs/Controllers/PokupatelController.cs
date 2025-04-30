using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using kurs.Context;
using kurs.Models;
using Npgsql;

namespace kurs.Controllers
{
    public class PokupatelController : Controller
    {
        private readonly MagazinminContext _context;

        public PokupatelController(MagazinminContext context)
        {
            _context = context;
        }

        // GET: Pokupatel

        
        public async Task<IActionResult> Index(string sortOrder, string searchPhone, string searchName, string searchAddress)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PhoneSortParm"] = sortOrder == "phone" ? "phone_desc" : "phone";
            ViewData["AddressSortParm"] = sortOrder == "address" ? "address_desc" : "address";
            ViewData["IdSortParm"] = sortOrder == "id_asc" ? "id_desc" : "id_asc";


            ViewData["CurrentSearchPhone"] = searchPhone;
            ViewData["CurrentSearchName"] = searchName;
            ViewData["CurrentSearchAddress"] = searchAddress;

            var pokupatels = from p in _context.Pokupatel select p;

            // Apply search filters
            if (!String.IsNullOrEmpty(searchPhone))
            {
                pokupatels = pokupatels.Where(p => p.PokupatelTelefon.ToString().Contains(searchPhone));
            }

            if (!String.IsNullOrEmpty(searchName))
            {
                pokupatels = pokupatels.Where(p => p.PokupatelFio.Contains(searchName));
            }

            if (!String.IsNullOrEmpty(searchAddress))
            {
                pokupatels = pokupatels.Where(p => p.PokupatelAdresDostavki.Contains(searchAddress));
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "name_desc":
                    pokupatels = pokupatels.OrderByDescending(p => p.PokupatelFio);
                    break;
                case "phone":
                    pokupatels = pokupatels.OrderBy(p => p.PokupatelTelefon);
                    break;
                case "phone_desc":
                    pokupatels = pokupatels.OrderByDescending(p => p.PokupatelTelefon);
                    break;
                case "address":
                    pokupatels = pokupatels.OrderBy(p => p.PokupatelAdresDostavki);
                    break;
                case "id_asc":
                    pokupatels = pokupatels.OrderBy(z => z.IdPokupatel);
                    break;
                case "address_desc":
                    pokupatels = pokupatels.OrderByDescending(p => p.PokupatelAdresDostavki);
                    break;
                default:
                    pokupatels = pokupatels.OrderBy(p => p.PokupatelFio);
                    break;
            }

            return View(await pokupatels.ToListAsync());
        }
        // GET: Pokupatel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pokupatel == null)
            {
                return NotFound();
            }

            var pokupatel = await _context.Pokupatel
                .FirstOrDefaultAsync(m => m.IdPokupatel == id);
            if (pokupatel == null)
            {
                return NotFound();
            }

            return View(pokupatel);
        }

        // GET: Pokupatel/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pokupatel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPokupatel,PokupatelFio,PokupatelTelefon,PokupatelAdresDostavki")] Pokupatel pokupatel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pokupatel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pokupatel);
        }


        // GET: Pokupatel/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var pokupatel = await _context.Pokupatel.FindAsync(id);
        //    if (pokupatel == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(pokupatel);
        //}

        //// POST: Pokupatel/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("IdPokupatel,PokupatelFio,PokupatelTelefon,PokupatelAdresDostavki")] Pokupatel pokupatel)
        //{
        //    if (id != pokupatel.IdPokupatel)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(pokupatel);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PokupatelExists(pokupatel.IdPokupatel))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(pokupatel);
        //}

        public IActionResult Edit(int id)
        {
            var pokupatel = _context.Pokupatel.Find(id);
            if (pokupatel == null) return NotFound();

           
            return View(pokupatel);
        }

        // POST: Tovar/Edit/5
        [HttpPost]
        public IActionResult Edit(int id, Pokupatel pokupatel)
        {
            if (id != pokupatel.IdPokupatel) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Pokupatel.Update(pokupatel);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
          
            return View(pokupatel);
        }

        // GET: Pokupatel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pokupatel == null)
            {
                return NotFound();
            }

            var pokupatel = await _context.Pokupatel
                .FirstOrDefaultAsync(m => m.IdPokupatel == id);
            if (pokupatel == null)
            {
                return NotFound();
            }

            return View(pokupatel);
        }

        // POST: Pokupatel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pokupatel == null)
            {
                return Problem("Entity set 'MagazinminContext.Pokupatel' is null.");
            }

            var pokupatel = await _context.Pokupatel.FindAsync(id);
            if (pokupatel != null)
            {
                try
                {
                    _context.Pokupatel.Remove(pokupatel);
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

        private bool PokupatelExists(int id)
        {
            return (_context.Pokupatel?.Any(e => e.IdPokupatel == id)).GetValueOrDefault();
        }


        public async Task<IActionResult> Orders(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pokupatel = await _context.Pokupatel
                .Include(p => p.Zakaz)
                .FirstOrDefaultAsync(m => m.IdPokupatel == id);

            if (pokupatel == null)
            {
                return NotFound();
            }

            return View(pokupatel);
        }
    }
}
