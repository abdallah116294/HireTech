using AutoMapper;
using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
using HireTech.Core.Specifications;
using HireTech.Repository.Data;
using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HireTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProfileController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HireTechDbContext _dbContext;
        
        public ProfileController(IUnitOfWork unitOfWork, IMapper mapper, HireTechDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dbContext = dbContext;
        }
        [Authorize(Roles = "CANDIDATE")]
        [HttpPost("AddProfile")]
        public async Task<IActionResult> AddProfile(CreateCandidateProfileDTO dto)
        {
            try
            {
                var userId = GetUserID();
                if (userId == null)
                    return Unauthorized("Please Login to add Profile Details");

                // Check if profile already exists for this user
                var spec = new ProfileWithSpecification(userId);
                var profileRepo = _unitOfWork.Repository<CandidateProfile>();
                var existingProfile = await profileRepo.GetByIdWithSpecAsync(spec);

                if (existingProfile != null)
                {
                    return CreateResponse(new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "Profile already exists for this user. Use update endpoint instead.",
                        ErrorCode = ErrorCodes.BadRequest // or appropriate error code
                    });
                }

                // Map the DTO to entity
                var mappedProfile = _mapper.Map<CandidateProfile>(dto);
                mappedProfile.UserId = userId; // Set the UserId explicitly

                // Handle Skills
                if (dto.Skills != null && dto.Skills.Any())
                {
                    mappedProfile.Skills = new List<Skill>();

                    foreach (var skillName in dto.Skills)
                    {
                        // Check if skill exists in the database
                        var skill = await _dbContext.Skills
                            .FirstOrDefaultAsync(s => s.Name.ToLower() == skillName.ToLower());

                        if (skill == null)
                        {
                            // Create new skill if it doesn't exist
                            skill = new Skill
                            {
                                Name = skillName,
                                // Add other required properties if any
                            };
                            _dbContext.Skills.Add(skill);
                            // Save immediately to get the ID
                            await _dbContext.SaveChangesAsync();
                        }

                        mappedProfile.Skills.Add(skill);
                    }
                }

                // Add the profile
                await profileRepo.AddAsync(mappedProfile);
                await _unitOfWork.CompleteAsync();

                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Profile Added Successfully",
                    Data = new
                    {
                        Id = mappedProfile.Id,
                        UserId = mappedProfile.UserId,
                        Skills = mappedProfile.Skills?.Select(s => s.Name).ToList()
                    }
                });
            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"An Error Occurred: {ex.Message}",
                    ErrorCode = ErrorCodes.Excptions
                });
            }
           
        }
        [Authorize(Roles = "CANDIDATE")]
        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = GetUserID();
                if (userId == null)
                    return Unauthorized("Please Login to view Profile Details");
                var spec = new ProfileWithSpecification(userId);
                var profileRepo = _unitOfWork.Repository<CandidateProfile>();
                var profile = await profileRepo.GetByIdWithSpecAsync(spec);
                //var profile = await _dbContext.CandidateProfiles
                //    .Include(p => p.Skills)
                //    .Include(p => p.Applications)
                //        .ThenInclude(a => a.Vacancy)
                //    .FirstOrDefaultAsync(p => p.UserId == userId);

                if (profile == null)
                {
                    return CreateResponse(new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "Profile not found",
                        ErrorCode = ErrorCodes.NotFound
                    });
                }

                //  var profileDto = _mapper.Map<CandidateProfileResponseDTO>(profile);
                var profileDto = new CandidateProfileResponseDTO
                {
                    Id = profile.Id,
                    FullName = profile.User.FullName ?? string.Empty, // Handle null
                    Gender = profile.Gender ?? string.Empty,
                    Nationality = profile.Nationality ?? string.Empty,
                    HighestQualification = profile.HighestQualification ?? string.Empty,
                    CurrentJobTitle = profile.CurrentJobTitle ?? string.Empty,
                    CurrentCompany = profile.CurrentCompany ?? string.Empty,
                    CurrentSalary = profile.CurrentSalary,
                    ExpectedSalary = profile.ExpectedSalary,
                    LinkedInProfile = profile.LinkedInProfile ?? string.Empty,
                    PortfolioUrl = profile.PortfolioUrl ?? string.Empty,
                    Skills = profile.Skills?.Select(s => s.Name).Where(name => !string.IsNullOrEmpty(name)).ToList() ?? new List<string>(),
                    ApplicationsCount = profile.Applications?.Count ?? 0
                };

                return CreateResponse(new ResponseDTO<CandidateProfileResponseDTO>
                {
                    IsSuccess = true,
                    Message = "Profile Retrieved Successfully",
                    Data = profileDto
                });
            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"An Error Occurred: {ex.Message}",
                    ErrorCode = ErrorCodes.Excptions
                });
            }
        }
    }
}
