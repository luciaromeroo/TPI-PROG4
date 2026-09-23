using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
    
{
   public class ApplicationContext: DbContext
    {
        public DbSet<User> Users{get; set;}
        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options)
        {
            
        }
    }
}

