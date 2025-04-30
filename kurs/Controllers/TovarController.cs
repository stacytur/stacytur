using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kurs.Context;
using kurs.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace kurs.Controllers
{
    public class TovarController : Controller
    {
        private readonly MagazinminContext _context;

        public TovarController(MagazinminContext context)
        {
            _context = context;
        }      
        public IActionResult Index(string sortOrder, string searchName, decimal? searchPrice)
        {
            ViewData["CenaSortParm"] = sortOrder == "cena_asc" ? "cena_desc" : "cena_asc";
            ViewData["KolichestvoSortParm"] = sortOrder == "kolichestvo_asc" ? "kolichestvo_desc" : "kolichestvo_asc";
            ViewData["IdSortParm"] = sortOrder == "id_asc" ? "id_desc" : "id_asc";
            ViewData["TovarNaimenovanieSortParm"] = sortOrder == "tovar_naimenovanie_asc" ? "tovar_naimenovanie_desc" : "tovar_naimenovanie_asc";

            var tovars = from t in _context.Tovar select t;

            // Apply search filters
            if (!string.IsNullOrEmpty(searchName))
            {
                tovars = tovars.Where(t => t.TovarNaimenovanie.Contains(searchName));
            }
            if (searchPrice.HasValue)
            {
                tovars = tovars.Where(t => t.Cena == searchPrice.Value);
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "cena_desc":
                    tovars = tovars.OrderByDescending(t => t.Cena);
                    break;
                case "cena_asc":
                    tovars = tovars.OrderBy(t => t.Cena);
                    break;
                case "kolichestvo_desc":
                    tovars = tovars.OrderByDescending(t => t.Kolichestvo);
                    break;
                case "kolichestvo_asc":
                    tovars = tovars.OrderBy(t => t.Kolichestvo);
                    break;
                case "id_desc":
                    tovars = tovars.OrderByDescending(t => t.IdTovar);
                    break;
                case "id_asc":
                    tovars = tovars.OrderBy(t => t.IdTovar);
                    break;
                case "tovar_naimenovanie_desc":
                    tovars = tovars.OrderByDescending(t => t.TovarNaimenovanie);
                    break;
                case "tovar_naimenovanie_asc":
                    tovars = tovars.OrderBy(t => t.TovarNaimenovanie);
                    break;
                default:
                    tovars = tovars.OrderBy(t => t.IdTovar);
                    break;
            }

            return View(tovars.ToList());
        }
        // GET: Tovar/Add
        public IActionResult Add()
        {
            ViewData["IdZakaz"] = new SelectList(_context.Zakaz, "IdZakaz", "IdZakaz");
            return View();
        }
        [HttpPost]
        public IActionResult Add(Zakaz zakaz)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Добавляем заказ в контекст
                    _context.Zakaz.Add(zakaz);

                    // Для каждого товара в заказе устанавливаем Id заказа
                    foreach (var tovar in zakaz.Tovar)
                    {
                        tovar.IdZakaz = zakaz.IdZakaz;
                    }

                    // Сохраняем изменения
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    // Зарегистрируйте исключение или обработайте его соответственно
                    ModelState.AddModelError("", "Произошла ошибка при сохранении заказа. Пожалуйста, попробуйте позже.");
                    // Вы также можете зарегистрировать подробности исключения для целей отладки
                    Console.WriteLine(ex.InnerException.Message);
                }
            }

            // Если ModelState недействителен, возвращаем представление с данными заказа
            ViewData["IdDostavka"] = new SelectList(_context.Dostavka, "IdDostavka", "IdDostavka", zakaz.IdDostavka);
            ViewData["IdPokupatel"] = new SelectList(_context.Pokupatel, "IdPokupatel", "PokupatelFio", zakaz.IdPokupatel);
            ViewData["IdSborschikZakaza"] = new SelectList(_context.SborschikZakaza, "IdSborschikZakaza", "SborschikZakazaNames", zakaz.IdSborschikZakaza);
            return View(zakaz);
        }
        

        // GET: Tovar/Edit/5
        public IActionResult Edit(int id)
        {
            var tovar = _context.Tovar.Find(id);
            if (tovar == null) return NotFound();

            ViewData["IdZakaz"] = new SelectList(_context.Zakaz, "IdZakaz", "IdZakaz", tovar.IdZakaz);
            return View(tovar);
        }

        // POST: Tovar/Edit/5
        [HttpPost]
        public IActionResult Edit(int id, Tovar tovar)
        {
            if (id != tovar.IdTovar) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Tovar.Update(tovar);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["IdZakaz"] = new SelectList(_context.Zakaz, "IdZakaz", "IdZakaz", tovar.IdZakaz);
            return View(tovar);
        }

        // GET: Tovar/Delete/5
        public IActionResult Delete(int id)
        {
            var tovar = _context.Tovar.Find(id);
            if (tovar == null) return NotFound();

            return View(tovar);
        }

        // POST: Tovar/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var tovar = _context.Tovar.Find(id);
            _context.Tovar.Remove(tovar);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        
    }
}