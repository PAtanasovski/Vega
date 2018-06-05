using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vega.Models;
using Vega.Persistence;

namespace Vega.Controllers
{
   public class MakesController : Controller
   {
      private readonly VegaDbContext context;

      public MakesController(VegaDbContext context)
      {
         this.context = context;
      }

      [HttpGet("/api/makes")]
      public IEnumerable<Make> GetMakes()
      {
         return context.Makes.Include(m => m.Models).ToList();
      }

      // [HttpGet("/api/makes")]
      // public async Task<IEnumerable<Make>> GetMakes()
      // {
      //    return await context.Makes.Include(m => m.Models).ToListAsync();
      // }
   }
}