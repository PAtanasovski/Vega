using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vega.Controllers.Resources;
using Vega.Core.Models;
using Vega.Persistence;

namespace Vega.Controllers
{
   public class FeaturesController : Controller
   {
      private readonly VegaDbContext context;

      private readonly IMapper mapper;

      public FeaturesController(VegaDbContext context, IMapper mapper)
      {
         this.mapper = mapper;
         this.context = context;

      }

      [HttpGet("/api/features")]
      public IEnumerable<KeyValuePairResource> GetFeatures()
      {
         var features = context.Features.ToList();

         return mapper.Map<List<Feature>, List<KeyValuePairResource>>(features);
      }

      // [HttpGet("/api/features")]
      // public async Task<IEnumerable<FeatureResource>> GetFeatures()
      // {
      //    var features = await context.Features.ToList();

      //    return mapper.Map<List<Feature>, List<FeatureResource>>(features);
      // }
   }
}