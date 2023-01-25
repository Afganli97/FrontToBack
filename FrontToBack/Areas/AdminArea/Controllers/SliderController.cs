using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FrontToBack.DAL;
using FrontToBack.Helpers.Extensions;
using FrontToBack.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FrontToBack.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SliderController : Controller
    {
        private readonly DataBase _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(DataBase context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_context.Sliders.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (ModelState["Photo"].ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                return View();
            if(!slider.Photo.CheckFile("image"))
                ModelState.AddModelError("Photo", "Select Photo");
            if(slider.Photo.CheckFileLength(1000))
                ModelState.AddModelError("Photo", "Selected photo length is so much");

            slider.Photo.SaveFile(_env, "img");
            
            Slider newSlider = new Slider();
            newSlider.ImageUrl = slider.Photo.FileName;

            _context.Sliders.Add(newSlider);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int? id)
        {
            if(id == null) return NotFound();
            Slider slider = _context.Sliders.Find(id);
            if(slider == null) return NotFound();

            slider.ImageUrl.DeleteFile(_env, "img");

            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Update(int? id)
        {
            if(id == null) return NotFound();
            if(_context.Sliders.Find(id)== null) return NotFound();

            return View(_context.Sliders.Find(id));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, Slider slider)
        {
            if(id == null) return NotFound();
            Slider existSlider = _context.Sliders.Find(id);
            if(existSlider == null) return NotFound();

            if (slider.Photo != null)
            {
                if(!slider.Photo.CheckFile("image"))
                    ModelState.AddModelError("Photo", "Select Photo");
                if(slider.Photo.CheckFileLength(1000))
                    ModelState.AddModelError("Photo", "Selected photo length is so much");

                existSlider.ImageUrl.DeleteFile(_env, "img");
                slider.Photo.SaveFile(_env, "img");
                
                _context.Sliders.Find(id).ImageUrl = slider.Photo.FileName;
                _context.SaveChanges();
            }
            
            return RedirectToAction("index");
        }


    }
}