using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FrontToBack.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FrontToBack.Areas.AdminArea.Controllers
{
    [Route("[controller]")]
    [Area("AdminArea")]
    public class SliderController : Controller
    {
        private readonly DataBase _context;
        public SliderController(DataBase context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}