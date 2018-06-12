using Vega.Models;

namespace Vega.Persistence
{
   public interface IVehicleRepository
   {
      Vehicle GetVehicle(int id);
   }
}