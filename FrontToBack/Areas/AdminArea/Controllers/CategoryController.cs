using FrontToBack.DAL;
using FrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Areas.AdminArea.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataBase _context;

        public CategoryController(DataBase context)
        {
            _context = context;
        }

        [Area("AdminArea")]
        public IActionResult Index()
        {
            return View(_context.Categories.ToList());
        }

        [Area("AdminArea")]
        public IActionResult Add()
        {
            return View();
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult Add(Category category)
        {
            if (!ModelState.IsValid) return View();
            if (_context.Categories.FirstOrDefault(c=>c.Name.ToLower() == category.Name.ToLower()) != null)
            {
                return View();
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Area("AdminArea")]
        public IActionResult Info(int? id)
        {
            if (id == null) return NotFound();
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [Area("AdminArea")]
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult Update(Category category)
        {
            _context.Categories.Find(category.Id).Name = category.Name;
            _context.Categories.Find(category.Id).Description = category.Description;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Area("AdminArea")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
