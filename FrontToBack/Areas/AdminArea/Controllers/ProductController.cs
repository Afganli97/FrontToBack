using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontToBack.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.Areas.AdminArea.Controllers
{
    [Area("adminarea")]
    public class ProductController : Controller
    {
        private readonly DataBase _context;

        public ProductController(DataBase context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Products.Include(p=>p.Images).Include(p=>p.Category).ToList());
        }

        public IActionResult Detail(int? id)
        {
            return View(_context.Products.Include(p=>p.Images).Include(p=>p.Category).ToList());
        }
    }
}