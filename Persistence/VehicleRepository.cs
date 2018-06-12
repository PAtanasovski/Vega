using System.Linq;
using Microsoft.EntityFrameworkCore;
using Vega.Core;
using Vega.Core.Models;

namespace Vega.Persistence
{
   public class VehicleRepository : IVehicleRepository
   {
      private readonly VegaDbContext context;

      public VehicleRepository(VegaDbContext context)
      {
         this.context = context;
      }

      public Vehicle GetVehicle(int id, bool includeRelated = true)
      {
         if (!includeRelated)
         {
            return context.Vehicles.Find(id);
         }
         return context.Vehicles
            .Include(v => v.Features)
               .ThenInclude(vf => vf.Feature)
            .Include(v => v.Model)
               .ThenInclude(m => m.Make)
            .SingleOrDefault(v => v.Id == id);
      }

      public void Add(Vehicle vehicle)
      {
         context.Vehicles.Add(vehicle);
      }

      public void Remove(Vehicle vehicle)
      {
         context.Remove(vehicle);
      }
   }
}