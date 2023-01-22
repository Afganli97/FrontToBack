using System;
using System.Collections.Generic;
using FrontToBack.Services.Interfaces;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FrontToBack.Services
{
	public class BasketCountService : IBasketCount
	{
        private readonly IHttpContextAccessor _contextAccessor;

        public BasketCountService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        int IBasketCount.GetBasketCount()
        {
            string basket = _contextAccessor.HttpContext.Request.Cookies["basket"];
            if (basket == null) return 0;
            List<BasketVM> list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            return list.Count;
        }
    }
}

