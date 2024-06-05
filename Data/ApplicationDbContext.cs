using Microsoft.EntityFrameworkCore;
using DotnetNBA.Models;

namespace DotnetNBA.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
            {                
            }

            public DbSet<PlayerDataTotals> PlayerDataTotals {get; set;}
    }
}