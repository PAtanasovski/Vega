using System.Linq;
using Microsoft.EntityFrameworkCore;
using Vega.Models;

namespace Vega.Persistence
{
   public class VehicleRepository : IVehicleRepository
   {
      private readonly VegaDbContext context;

      public VehicleRepository(VegaDbContext context)
      {
         this.context = context;
      }
      public Vehicle GetVehicle(int id)
      {
         return context.Vehicles
            .Include(v => v.Features)
               .ThenInclude(vf => vf.Feature)
            .Include(v => v.Model)
               .ThenInclude(m => m.Make)
            .SingleOrDefault(v => v.Id == id);
      }
   }
}