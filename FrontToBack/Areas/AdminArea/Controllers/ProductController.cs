using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontToBack.DAL;
using FrontToBack.Helpers.Extensions;
using FrontToBack.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.Areas.AdminArea.Controllers
{
    [Area("adminarea")]
    public class ProductController : Controller
    {
        private readonly DataBase _context;
private readonly IWebHostEnvironment _env;
        public ProductController(DataBase context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_context.Products.Include(p=>p.Images).ToList());
        }

        public IActionResult Detail(int? id)
        {
            if(id == null) return NotFound();
            Product product = _context.Products.Include(p=>p.Images).Include(p=>p.Category).FirstOrDefault(x=>x.Id == id);
            if(product == null) return NotFound();

            return View(product);
        }

        public IActionResult Delete(int? id)
        {
            if(id == null) return NotFound();
            Product product = _context.Products.Include(p=>p.Images).Include(p=>p.Category).FirstOrDefault(x=>x.Id == id);
            if(product == null) return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }

        public IActionResult Update(int? id)
        {
            if(id == null) return NotFound();
            Product product = _context.Products.Include(p=>p.Images).Include(p=>p.Category).FirstOrDefault(x=>x.Id == id);
            if(product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        public IActionResult Update(int? id, Product product)
        {
            if(!ModelState.IsValid) return View();
            var existProduct = _context.Products.Find(id);
            if (!_context.Products.Any(x=>x.Name.ToLower() == product.Name.ToLower())&&existProduct.Name.ToLower()!=product.Name.ToLower()) return View();

            var mainImage = product.Images.FirstOrDefault(x=>x.IsMain);

            existProduct.Name = product.Name;
            existProduct.Price = product.Price;
            existProduct.Count = product.Count;

            if (mainImage.Photo != null)
            {
                if(!mainImage.Photo.CheckFile("image"))
                    ModelState.AddModelError("Photo", "Select Photo");
                if(mainImage.Photo.CheckFileLength(1000))
                    ModelState.AddModelError("Photo", "Selected photo length is so much");

                existProduct.Images.FirstOrDefault(x=>x.IsMain).ImageUrl.DeleteFile(_env, "img");
                mainImage.Photo.SaveFile(_env, "img");
                
                _context.Products.Find(id).Images.FirstOrDefault(x=>x.IsMain).ImageUrl = mainImage.Photo.FileName;
                _context.SaveChanges();
            }

            _context.Products.Update(existProduct);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        
    }
}