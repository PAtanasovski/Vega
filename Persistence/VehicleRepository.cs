using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Vega.Core;
using Vega.Core.Models;
using Vega.Extensions;

namespace Vega.Persistence
{
   public class VehicleRepository : IVehicleRepository
   {
      private readonly VegaDbContext context;

      public VehicleRepository(VegaDbContext context)
      {
         this.context = context;
      }

      public Vehicle GetVehicle(int id, bool includeRelated = true)
      {
         if (!includeRelated)
         {
            return context.Vehicles.Find(id);
         }
         return context.Vehicles
            .Include(v => v.Features)
               .ThenInclude(vf => vf.Feature)
            .Include(v => v.Model)
               .ThenInclude(m => m.Make)
            .SingleOrDefault(v => v.Id == id);
      }

      public void Add(Vehicle vehicle)
      {
         context.Vehicles.Add(vehicle);
      }

      public void Remove(Vehicle vehicle)
      {
         context.Remove(vehicle);
      }

      public QueryResult<Vehicle> GetVehicles(VehicleQuery queryObj)
      {
         var result = new QueryResult<Vehicle>();

         var query = context.Vehicles
            .Include(v => v.Model)
               .ThenInclude(m => m.Make)
            .Include(v => v.Features)
               .ThenInclude(vf => vf.Feature)
            .AsQueryable();

         if (queryObj.MakeId.HasValue)
         {
            query = query.Where(v => v.Model.MakeId == queryObj.MakeId.Value);
         }

         if (queryObj.ModelId.HasValue)
         {
            query = query.Where(v => v.ModelId == queryObj.ModelId.Value);
         }

         var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
         {
            ["make"] = v => v.Model.Make.Name,
            ["model"] = v => v.Model.Name,
            ["contactName"] = v => v.ContactName
         };

         query = query.ApplyOrdering(queryObj, columnsMap);

         #region 
         // if (queryObj.IsSortAscending)
         // {
         //    query = query.OrderBy(columnsMap[queryObj.SortBy]);
         // }
         // else
         // {
         //    query = query.OrderByDescending(columnsMap[queryObj.SortBy]);
         // }

         // if (queryObj.SortBy == "make")
         // {
         //    query = (queryObj.IsSortAscending)
         //       ? query.OrderBy(v => v.Model.Make.Name) : query.OrderByDescending(v => v.Model.Make.Name);
         // }

         // if (queryObj.SortBy == "model")
         // {
         //    query = (queryObj.IsSortAscending)
         //       ? query.OrderBy(v => v.Model.Name) : query.OrderByDescending(v => v.Model.Name);
         // }

         // if (queryObj.SortBy == "contactName")
         // {
         //    query = (queryObj.IsSortAscending)
         //       ? query.OrderBy(v => v.ContactName) : query.OrderByDescending(v => v.ContactName);
         // }

         // if (queryObj.SortBy == "id")
         // {
         //    query = (queryObj.IsSortAscending)
         //    ? query.OrderBy(v => v.Id) : query.OrderByDescending(v => v.Id);
         // }
         #endregion

         result.TotalItems = query.Count();

         query = query.ApplyPaging(queryObj);

         result.Items = query.ToList();

         return result;
      }
   }
}