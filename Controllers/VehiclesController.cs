using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vega.Controllers.Resources;
using Vega.Core.Models;
using Vega.Core;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Vega.Controllers
{
   [Route("/api/vehicles")]
   public class VehiclesController : Controller
   {
      private readonly IMapper mapper;

      private readonly IVehicleRepository repository;

      private readonly IUnitOfWork unitOfWork;

      public VehiclesController(
         IMapper mapper,
         IVehicleRepository repository,
         IUnitOfWork unitOfWork
      )
      {
         this.repository = repository;
         this.unitOfWork = unitOfWork;
         this.mapper = mapper;
      }

      [HttpPost]
      [Authorize]
      public IActionResult CreateVehicle([FromBody] SaveVehicleResource vehicleResource)
      {
         if (!ModelState.IsValid)
         {
            return BadRequest(ModelState);
         }

         var vehicle = mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
         vehicle.LastUpdate = DateTime.Now;
         repository.Add(vehicle);
         unitOfWork.Complete();

         vehicle = repository.GetVehicle(vehicle.Id);

         var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
         return Ok(result);
      }

      [HttpPut("{id}")]
      [Authorize]
      public IActionResult UpdateVehicle(int id, [FromBody] SaveVehicleResource vehicleResource)
      {
         if (!ModelState.IsValid)
         {
            return BadRequest(ModelState);
         }

         var vehicle = repository.GetVehicle(id);

         if (vehicle == null)
         {
            return NotFound();
         }

         mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);
         vehicle.LastUpdate = DateTime.Now;

         unitOfWork.Complete();

         vehicle = repository.GetVehicle(vehicle.Id);

         var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
         return Ok(result);
      }

      [HttpDelete("{id}")]
      [Authorize]
      public IActionResult DeleteVehicle(int id)
      {
         var vehicle = repository.GetVehicle(id, includeRelated: false);

         if (vehicle == null)
         {
            return NotFound();
         }

         repository.Remove(vehicle);
         unitOfWork.Complete();

         return Ok(id);
      }

      [HttpGet("{id}")]
      public IActionResult GetVehicle(int id)
      {
         var vehicle = repository.GetVehicle(id);

         if (vehicle == null)
         {
            return NotFound();
         }

         var vehicleResource = mapper.Map<Vehicle, VehicleResource>(vehicle);

         return Ok(vehicleResource);
      }

      [HttpGet]
      public QueryResultResource<VehicleResource> GetVehicles(VehicleQueryResource filterResource)
      {
         var filter = mapper.Map<VehicleQueryResource, VehicleQuery>(filterResource);
         var queryResult = repository.GetVehicles(filter);

         return mapper.Map<QueryResult<Vehicle>, QueryResultResource<VehicleResource>>(queryResult);
      }

      // [HttpPost]
      // public async Task<IActionResult> CreateVehicle([FromBody] VehicleResource vehicleResource)
      // {
      //    if (!ModelState.IsValid)
      //    {
      //       return BadRequest(ModelState);
      //    }
      //
      //    var vehicle = mapper.Map<VehicleResource, Vehicle>(vehicleResource);
      //    vehicle.LastUpdate = DateTime.Now;
      //    context.Vehicles.Add(vehicle);
      //    await context.SaveChangesAsync();
      //    var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
      //    return Ok(result);
      // }
   }
}