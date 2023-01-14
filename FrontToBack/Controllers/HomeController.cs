using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FrontToBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataBase _context;

        public HomeController(DataBase context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM();
            homeVM.Sliders = _context.Sliders.ToList();
            homeVM.SliderContext = _context.SliderContexts.FirstOrDefault();
            homeVM.Categories = _context.Categories.ToList();
            homeVM.Products = _context.Products.ToList();

            return View(homeVM);
        }
    }
}
