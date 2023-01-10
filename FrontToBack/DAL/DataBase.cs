using FrontToBack.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.DAL
{
    public class DataBase:DbContext
    {
        public DataBase(DbContextOptions<DataBase> options):base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; }
    }
}
