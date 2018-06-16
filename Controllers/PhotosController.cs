using System;
using System.IO;
using System.Linq;
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
      private readonly int MAX_BYTES = 1 * 1024 * 1024;

      private readonly string[] ACCEPTED_FILE_TYPES = new[] { ".jpg", ".jpeg", ".png" };

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

         if (file == null)
         {
            return BadRequest("Null file.");
         }

         if (file.Length == 0)
         {
            return BadRequest("Empty file.");
         }

         if (file.Length > MAX_BYTES)
         {
            return BadRequest("Max file size exceeded.");
         }

         if (!ACCEPTED_FILE_TYPES.Any(s => s == Path.GetExtension(file.FileName)))
         {
            return BadRequest("Invalid file type.");
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