using Vega.Models;

namespace Vega.Persistence
{
   public interface IVehicleRepository
   {
      Vehicle GetVehicle(int id, bool includeRelated = true);

      void Add(Vehicle vehicle);

      void Remove(Vehicle vehicle);
   }
}