using System.Collections.Generic;
using Vega.Core.Models;

namespace Vega.Core
{
   public interface IPhotoRepository
   {
      IEnumerable<Photo> GetPhotos(int vehicleId);
   }
}