using Microsoft.EntityFrameworkCore;

namespace Vega.Persistence
{
   public class VegaDbContext : DbContext
   {
      public VegaDbContext(DbContextOptions<VegaDbContext> options) : base(options)
      {

      }
   }
}