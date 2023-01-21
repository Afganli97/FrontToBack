using FrontToBack.DAL;
using FrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CategoryController : Controller
    {
        private readonly DataBase _context;

        public CategoryController(DataBase context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            return View(_context.Categories.ToList());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Add(Category category)
        {
            if (!ModelState.IsValid) return View();
            if (_context.Categories.Any(c=>c.Name.ToLower() == category.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This name already exist!");
                return View();
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Info(int? id)
        {
            if (id == null) return NotFound();
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            return View(category);
        }

        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(Category category)
        {
            if (!ModelState.IsValid) return View();
            if (_context.Categories.Any(c=>c.Name.ToLower() == category.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This name already exist!");
                return View(category);
            }
            _context.Categories.Find(category.Id).Name = category.Name;
            _context.Categories.Find(category.Id).Description = category.Description;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
