using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace FrontToBack.Controllers
{
    public class BasketController : Controller
    {
        private readonly DataBase _context;

        public BasketController(DataBase context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add(int? id)
        {
            string test = Request.Cookies["basket"];
            List<BasketVM> baskets;
            if (test == null)
            {
                Response.Cookies.Append("basket", JsonConvert.SerializeObject(new List<BasketVM>()));
                baskets = new List<BasketVM>();
            }
            else
            {
                baskets = JsonConvert.DeserializeObject<List<BasketVM>>(test);
            }

            if (id == null) return NotFound();
            Product existProduct = _context.Products.Include(p=>p.Category).Include(p=>p.Images).FirstOrDefault(p=>p.Id == id); 
            if (existProduct == null) return NotFound();
            
            BasketVM basketVM = new BasketVM {
                Id = existProduct.Id,
                Name = existProduct.Name,
                Price = existProduct.Price,
                ImageUrl = existProduct.Images.FirstOrDefault(i => i.IsMain).ImageUrl,
                Count = 1,
                CategoryName = existProduct.Category.Name
            };

            if (baskets.Find(p=>p.Id == basketVM.Id) != null)
            {
                baskets.Find(p => p.Id == basketVM.Id).Count++;
            }
            else
            {
                baskets.Add(basketVM);
            }

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(baskets));


            return RedirectToAction("index", "home");
        }


        public IActionResult ShowBasket()
        {
            string list = Request.Cookies["basket"];
            List<BasketVM> baskets;
            if(list == null) 
            {
                baskets= new List<BasketVM>();
                JsonConvert.SerializeObject(baskets);
            }
            else
            {
                baskets = JsonConvert.DeserializeObject<List<BasketVM>>(list);
            }
            return View(baskets);
        }

        public IActionResult Minus(int? id)
        {
            if(id == null) return RedirectToAction("showbasket");

            string list = Request.Cookies["basket"];
            List<BasketVM> baskets = JsonConvert.DeserializeObject<List<BasketVM>>(list);
            if(!baskets.Any(b=>b.Id == id)) return RedirectToAction("showbasket");

            baskets.Find(x=>x.Id == id).Count--;
            if (baskets.Find(x=>x.Id == id).Count == 0)
            {
                 baskets.Remove(baskets.Find(x=>x.Id == id));
            }
            Response.Cookies.Append("basket",JsonConvert.SerializeObject(baskets));
            return RedirectToAction("showbasket");
        }

        public IActionResult Plus(int? id)
        {   
            if(id == null) return RedirectToAction("showbasket");

            string list = Request.Cookies["basket"];
            List<BasketVM> baskets = JsonConvert.DeserializeObject<List<BasketVM>>(list);
            if(!baskets.Any(b=>b.Id == id)) return RedirectToAction("showbasket");

            baskets.Find(x=>x.Id == id).Count++;
            Response.Cookies.Append("basket",JsonConvert.SerializeObject(baskets));
            return RedirectToAction("showbasket");
        }

        public IActionResult Delete(int? id)
        {
            if(id == null) return RedirectToAction("showbasket");

             string list = Request.Cookies["basket"];
            List<BasketVM> baskets = JsonConvert.DeserializeObject<List<BasketVM>>(list);
            if(!baskets.Any(b=>b.Id == id)) return RedirectToAction("showbasket");

            baskets.Remove(baskets.Find(x=>x.Id == id));
            Response.Cookies.Append("basket",JsonConvert.SerializeObject(baskets));
            return RedirectToAction("showbasket");
        }

    }
}
