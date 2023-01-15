using System;
using FrontToBack.Models;

namespace FrontToBack.ViewModels
{
	public class ProductVM
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

