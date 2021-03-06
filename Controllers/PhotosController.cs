using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Vega.Controllers.Resources;
using Vega.Core;
using Vega.Core.Models;

namespace Vega.Controllers
{
   [Route("/api/vehicles/{vehicleId}/photos")]
   public class PhotosController : Controller
   {
      private readonly IHostingEnvironment host;

      private readonly IVehicleRepository vehicleRepository;

      private readonly IPhotoRepository photoRepository;

      private readonly IUnitOfWork unitOfWork;

      private readonly IMapper mapper;

      private readonly PhotoSettings photoSettings;

      public PhotosController(
         IHostingEnvironment host,
         IVehicleRepository vehicleRepository,
         IPhotoRepository photoRepository,
         IUnitOfWork unitOfWork,
         IMapper mapper,
         IOptionsSnapshot<PhotoSettings> options)
      {
         this.photoSettings = options.Value;
         this.vehicleRepository = vehicleRepository;
         this.photoRepository = photoRepository;
         this.unitOfWork = unitOfWork;
         this.mapper = mapper;
         this.host = host;
      }

      [HttpGet]
      public IEnumerable<PhotoResource> GetPhotos(int vehicleId)
      {
         var photos = photoRepository.GetPhotos(vehicleId);

         return mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
      }

      [HttpPost]
      public IActionResult Upload(int vehicleId, IFormFile file)
      {
         var vehicle = vehicleRepository.GetVehicle(vehicleId, includeRelated: false);

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

         if (file.Length > photoSettings.MaxBytes)
         {
            return BadRequest("Max file size exceeded.");
         }

         if (!photoSettings.isSupported(file.FileName))
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