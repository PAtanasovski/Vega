using System.Collections.Generic;
using Vega.Core.Models;

namespace Vega.Core
{
   public interface IVehicleRepository
   {
      Vehicle GetVehicle(int id, bool includeRelated = true);

      void Add(Vehicle vehicle);

      void Remove(Vehicle vehicle);

      QueryResult<Vehicle> GetVehicles(VehicleQuery filter);
   }
}