using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Vega.Controllers.Resources;
using Vega.Models;

namespace Vega.Mapping
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         // Domain to API Resource
         CreateMap<Make, MakeResource>();
         CreateMap<Model, ModelResource>();
         CreateMap<Feature, FeatureResource>();
         CreateMap<Vehicle, VehicleResource>()
            .ForMember(
               vr => vr.Contact,
               opt => opt.MapFrom(v => new ContactResource
               {
                  Name = v.ContactName,
                  Email = v.ContactEmail,
                  Phone = v.ContactPhone
               }))
            .ForMember(
               vr => vr.Features,
               opt => opt.MapFrom(v => v.Features.Select(vf => vf.FeatureId))
            );

         // API Resource to Domain
         CreateMap<VehicleResource, Vehicle>()
            .ForMember(
               vehicle => vehicle.Id,
               opt => opt.Ignore()
            )
            .ForMember(
               vehicle => vehicle.ContactName,
               opt => opt.MapFrom(vr => vr.Contact.Name)
            )
            .ForMember(
               vehicle => vehicle.ContactEmail,
               opt => opt.MapFrom(vr => vr.Contact.Email)
            )
            .ForMember(
               vehicle => vehicle.ContactPhone,
               opt => opt.MapFrom(vr => vr.Contact.Phone)
            )
            .ForMember(
               vehicle => vehicle.Features,
               opt => opt.Ignore()
            )
            .AfterMap((vr, v) =>
            {
               // Remove unselected features
               var removedFeatures = v.Features
                  .Where(f => !vr.Features.Contains(f.FeatureId));
               foreach (var rf in removedFeatures.ToList())
               {
                  v.Features.Remove(rf);
               }

               // Add new features
               var addedFeatures = vr.Features
                  .Where(id => !v.Features.Any(vf => vf.FeatureId == id))
                  .Select(id => new VehicleFeature { FeatureId = id });

               foreach (var f in addedFeatures.ToList())
               {
                  v.Features.Add(f);
               }
            });
      }
   }
}