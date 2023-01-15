using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontToBack.DAL;
using FrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontToBack.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataBase _context;

        public ProductController(DataBase context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var products = _context.Products.Where(p=>p.IsDeleted==false).Include(p => p.Category).Include(p=>p.Images).ToList();
            return View(products);
        }

        public IActionResult LoadMore()
        {
            var products = _context.Products.Select(p=> new Category {
                Id = p.Id,
                Name = p.Name
            }).ToList() ;
            return View();
        }
    }
}

