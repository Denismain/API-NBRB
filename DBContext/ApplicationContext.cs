using Firstapp.Entities;
using Microsoft.EntityFrameworkCore;

namespace Firstapp.DBContext
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
 
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.Migrate();
        }
    }
}
