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

         // API Resource to Domain
         CreateMap<VehicleResource, Vehicle>()
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
               opt => opt.MapFrom(vr =>
                  vr.Features.Select(id => new VehicleFeature { FeatureId = id }))
            );
      }
   }
}