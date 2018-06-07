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

      public VehiclesController(IMapper mapper, VegaDbContext context)
      {
         this.context = context;
         this.mapper = mapper;
      }

      [HttpPost]
      public IActionResult CreateVehicle([FromBody] VehicleResource vehicleResource)
      {
         if (!ModelState.IsValid)
         {
            return BadRequest(ModelState);
         }

         var vehicle = mapper.Map<VehicleResource, Vehicle>(vehicleResource);
         vehicle.LastUpdate = DateTime.Now;
         context.Vehicles.Add(vehicle);
         context.SaveChanges();

         var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
         return Ok(result);
      }

      [HttpPut("{id}")]
      public IActionResult UpdateVehicle(int id, [FromBody] VehicleResource vehicleResource)
      {
         if (!ModelState.IsValid)
         {
            return BadRequest(ModelState);
         }

         var vehicle = context.Vehicles
            .Include(v => v.Features).SingleOrDefault(v => v.Id == id);
         mapper.Map<VehicleResource, Vehicle>(vehicleResource, vehicle);
         vehicle.LastUpdate = DateTime.Now;

         context.SaveChanges();

         var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
         return Ok(result);
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