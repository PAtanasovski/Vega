using System;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vega.Controllers.Resources;
using Vega.Core;
using Vega.Core.Models;

namespace Vega.Controllers
{
   [Route("/api/vehicles/{vehicleId}/photos")]
   public class PhotosController : Controller
   {
      private readonly IHostingEnvironment host;

      private readonly IVehicleRepository repository;

      private readonly IUnitOfWork unitOfWork;

      private readonly IMapper mapper;

      public PhotosController(
         IHostingEnvironment host,
         IVehicleRepository repository,
         IUnitOfWork unitOfWork,
         IMapper mapper)
      {
         this.repository = repository;
         this.unitOfWork = unitOfWork;
         this.mapper = mapper;
         this.host = host;
      }

      [HttpPost]
      public IActionResult Upload(int vehicleId, IFormFile file)
      {
         var vehicle = repository.GetVehicle(vehicleId, includeRelated: false);

         if (vehicle == null)
         {
            return NotFound();
         }

         var uploadsFolderPath = Path.Combine(host.WebRootPath, "uploads");

         if (!Directory.Exists(uploadsFolderPath))
         {
            Directory.CreateDirectory(uploadsFolderPath);
         }

         var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
         var filePath = Path.Combine(uploadsFolderPath, fileName);

         using (var stream = new FileStream(filePath, FileMode.Create))
         {
            file.CopyTo(stream);
         }

         var photo = new Photo { FileName = fileName };
         vehicle.Photos.Add(photo);

         unitOfWork.Complete();

         return Ok(mapper.Map<Photo, PhotoResource>(photo));
      }
   }
}