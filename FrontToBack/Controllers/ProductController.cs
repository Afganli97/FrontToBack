
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
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
            var products = _context.Products.Where(p=>p.IsDeleted==false).Include(p => p.Category).Include(p=>p.Images).Take(2).ToList();
            return View(products);
        }

        public IActionResult Loadmore(int skip)
        {
            if (skip >= _context.Products.ToList().Count)
            {
                return null;
            }
            var products = _context.Products.Include(p =>p.Category).Include(p=>p.Images).Skip(skip).Take(2).ToList();
            return PartialView("_ProductPartial",products);
        }

        public IActionResult GetCount()
        {
            return Ok(_context.Products.Where(p=>p.IsDeleted==false).Count());
        }

    }
}

