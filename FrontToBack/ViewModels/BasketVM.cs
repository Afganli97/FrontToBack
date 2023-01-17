using FrontToBack.Models;

namespace FrontToBack.ViewModels
{
    public class BasketVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
    }
}
