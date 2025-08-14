using AutoMapper;
using HireTech.Core.Entities;
using HireTech.Uitilities.DTO.Company;

namespace HireTech.API
{
    public class MappingProfile:Profile
    {
        public MappingProfile() 
        {
            //Company 
            //1.Create Company 
            CreateMap<Company, CreateCompanyDTO>().ReverseMap();
            CreateMap<Company, UpdateCompanyDTO>().ReverseMap();
        }
    }
}
