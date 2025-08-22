using AutoMapper;
using HireTech.Core.Entities;
using HireTech.Uitilities.DTO.Application;
using HireTech.Uitilities.DTO.Company;
using HireTech.Uitilities.DTO.EventNote;
using HireTech.Uitilities.DTO.Profile;
using HireTech.Uitilities.DTO.Vacancy;
using Microsoft.EntityFrameworkCore;

namespace HireTech.API
{
    public class MappingProfile:Profile
    {
        public MappingProfile() 
        {
            //User 
             CreateMap<User, UserBasicInfoDTO>();
            //Company 
            //1.Create Company 
            CreateMap<Company, CreateCompanyDTO>().ReverseMap();
            CreateMap<Company, UpdateCompanyDTO>().ReverseMap();
            CreateMap<Company, CompanyBasicInfoDTO>();
            //Vacancy
            CreateMap<Vacancy,CreateVacancyDTO>().ReverseMap();
            CreateMap<Vacancy, VacancyResponseDTO>();
            //Application
            CreateMap<Application, CandidateApplicationDTO>()
                            .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.VacancyId, opt => opt.MapFrom(src => src.Vacancy.Id))
            .ForMember(dest => dest.VacancyTitle, opt => opt.MapFrom(src => src.Vacancy.Title))
            .ForMember(dest => dest.VacancyDescription, opt => opt.MapFrom(src => src.Vacancy.Description))
            .ForMember(dest => dest.VacancyStatus, opt => opt.MapFrom(src => src.Vacancy.Status))
            .ForMember(dest => dest.SalaryMin, opt => opt.MapFrom(src => src.Vacancy.SalaryMin))
            .ForMember(dest => dest.SalaryMax, opt => opt.MapFrom(src => src.Vacancy.SalaryMax))
            .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Vacancy.Company.Id))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Vacancy.Company.Name))
            .ForMember(dest => dest.DaysSinceApplication,
                opt => opt.MapFrom(src => EF.Functions.DateDiffDay(src.AppliedOn, DateTime.Now)))
            .ForMember(dest => dest.SalaryRange,
                opt => opt.MapFrom(src =>
                    src.Vacancy.SalaryMin.HasValue && src.Vacancy.SalaryMax.HasValue
                        ? $"{src.Vacancy.SalaryMin.Value} - {src.Vacancy.SalaryMax.Value}"
                        : null));
            //Profile
            CreateMap<CreateCandidateProfileDTO, CandidateProfile>()
            .ForMember(dest => dest.Skills, opt => opt.Ignore()) // Handle skills manually
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore()); // Set manually

            CreateMap<CandidateProfile, CandidateProfileResponseDTO>()
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills.Select(s => s.Name).ToList()))
                .ForMember(dest => dest.ApplicationsCount, opt => opt.MapFrom(src => src.Applications.Count))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName ?? string.Empty))
                ;
            CreateMap<UpdateCandidateProfileDTO, CandidateProfile>()
    .ForMember(dest => dest.Skills, opt => opt.Ignore())
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.UserId, opt => opt.Ignore())
     .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            //EventNote
            CreateMap<EventNoteRequestDTO, EventNote>().ReverseMap();
            CreateMap<EventNote, EventNoteResponseDto>().ReverseMap();
        }
    }
}
