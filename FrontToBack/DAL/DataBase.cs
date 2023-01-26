using FrontToBack.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.DAL
{
    public class DataBase : IdentityDbContext<AppUser>
    {
        public DataBase(DbContextOptions<DataBase> options):base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderContext> SliderContexts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Expert> Experts { get; set; }

    }
}
