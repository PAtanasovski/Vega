using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vega.Controllers.Resources;
using Vega.Models;
using Vega.Persistence;

namespace Vega.Controllers
{
   [Route("/api/vehicles")]
   public class VehiclesController : Controller
   {
      private readonly IMapper mapper;

      private readonly VegaDbContext context;

      private readonly IVehicleRepository repository;

      public VehiclesController(
         IMapper mapper,
         VegaDbContext context,
         IVehicleRepository repository
      )
      {
         this.context = context;
         this.repository = repository;
         this.mapper = mapper;
      }

      [HttpPost]
      public IActionResult CreateVehicle([FromBody] SaveVehicleResource vehicleResource)
      {
         if (!ModelState.IsValid)
         {
            return BadRequest(ModelState);
         }

         var vehicle = mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
         vehicle.LastUpdate = DateTime.Now;
         context.Vehicles.Add(vehicle);
         context.SaveChanges();

         vehicle = repository.GetVehicle(vehicle.Id);

         var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
         return Ok(result);
      }

      [HttpPut("{id}")]
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

         context.SaveChanges();

         var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
         return Ok(result);
      }

      [HttpDelete("{id}")]
      public IActionResult DeleteVehicle(int id)
      {
         var vehicle = context.Vehicles.Find(id);

         if (vehicle == null)
         {
            return NotFound();
         }

         context.Remove(vehicle);
         context.SaveChanges();

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