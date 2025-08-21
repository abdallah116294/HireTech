using AutoMapper;
using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
using HireTech.Core.IServices;
using HireTech.Core.Specifications;
using HireTech.Repository.Data;
using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.Profile;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Service
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HireTechDbContext _dbContext;

        public ProfileService(IUnitOfWork unitOfWork, IMapper mapper, HireTechDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<ResponseDTO<object>> AddProfile(CreateCandidateProfileDTO dto, string userId)
        {
            try
            {
                // Check if profile already exists for this user
                var spec = new ProfileWithSpecification(userId);
                var profileRepo = _unitOfWork.Repository<CandidateProfile>();
                var skilRepo = _unitOfWork.Repository<Skill>();
                var existingProfile = await profileRepo.GetByIdWithSpecAsync(spec);

                if (existingProfile != null)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "Profile already exists for this user. Use update endpoint instead.",
                        ErrorCode = ErrorCodes.BadRequest // or appropriate error code
                    };
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
                           //await skilRepo.AddAsync(skill);
                           // await _unitOfWork.CompleteAsync();
                        }

                        mappedProfile.Skills.Add(skill);
                    }
                }
                // Add the profile
                await profileRepo.AddAsync(mappedProfile);
                await _unitOfWork.CompleteAsync();

                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Profile Added Successfully",
                    Data = new
                    {
                        Id = mappedProfile.Id,
                        UserId = mappedProfile.UserId,
                        Skills = mappedProfile.Skills?.Select(s => s.Name).ToList()
                    }
                };
            }
            catch(Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = $"Error Accured {ex}",
                    ErrorCode = ErrorCodes.Excptions
                };
            }
        }

        public async Task<ResponseDTO<object>> GetProfile(string userId)
        {
            try
            {
                var spec = new ProfileWithSpecification(userId);
                var profileRepo = _unitOfWork.Repository<CandidateProfile>();
                var profile = await profileRepo.GetByIdWithSpecAsync(spec);
                if (profile == null)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "Profile not found",
                        ErrorCode = ErrorCodes.NotFound
                    };
                }

                
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

                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Profile Retrieved Successfully",
                    Data = profileDto
                };
            }
            catch(Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = $"Error Accured {ex}",
                    ErrorCode = ErrorCodes.Excptions
                };
            }
        }

        public async Task<ResponseDTO<object>> UpdateProfile(UpdateCandidateProfileDTO dto, string userId)
        {
            try
            {
                var profileRepo = _unitOfWork.Repository<CandidateProfile>();
                var existingProfile = await _dbContext.CandidateProfiles
             .Include(p => p.Skills)
             .FirstOrDefaultAsync(p => p.UserId == userId);
                if (existingProfile == null)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "Profile not found. Please create a profile first.",
                        ErrorCode = ErrorCodes.NotFound
                    };
                }
                // Update profile properties

                _mapper.Map(dto, existingProfile);
                await profileRepo.UpdateAsync(existingProfile.Id, entity =>
                {
                    if (!string.IsNullOrEmpty(dto.Nationality))entity.Nationality=dto.Nationality;
                    if (!string.IsNullOrEmpty(dto.HighestQualification)) entity.HighestQualification = dto.HighestQualification;
                    if (!string.IsNullOrEmpty(dto.Address)) entity.Address = dto.Address;
                    if (dto.CurrentSalary==0) entity.CurrentSalary = dto.CurrentSalary;
                    if (!string.IsNullOrEmpty(dto.CurrentCompany)) entity.CurrentCompany = dto.CurrentCompany;
                    if (!string.IsNullOrEmpty(dto.Address)) entity.Address = dto.Address;
                    if (dto.ExpectedSalary==0) entity.ExpectedSalary = dto.ExpectedSalary;
                    if (!string.IsNullOrEmpty(dto.CurrentJobTitle)) entity.CurrentJobTitle = dto.CurrentJobTitle;
                    if (!string.IsNullOrEmpty(dto.PortfolioUrl)) entity.PortfolioUrl = dto.PortfolioUrl;
                    if (!string.IsNullOrEmpty(dto.LinkedInProfile)) entity.LinkedInProfile = dto.LinkedInProfile;
                   
                });
                // 3. Handle the Skills update
                if (dto.Skills != null)
                {
                    // Identify skills to be removed
                    var skillsToRemove = existingProfile.Skills
                        .Where(s => !dto.Skills.Any(ds => ds.ToLower() == s.Name.ToLower()))
                        .ToList();

                    foreach (var skill in skillsToRemove)
                    {
                        existingProfile.Skills.Remove(skill);
                    }

                    // Identify and add new skills
                    var existingSkillNames = existingProfile.Skills.Select(s => s.Name.ToLower()).ToList();

                    foreach (var skillName in dto.Skills)
                    {
                        if (!existingSkillNames.Contains(skillName.ToLower()))
                        {
                            // Check if the skill already exists in the database
                            var skillToAdd = await _dbContext.Skills
                                .FirstOrDefaultAsync(s => s.Name.ToLower() == skillName.ToLower());

                            if (skillToAdd == null)
                            {
                                // Create a new skill if it doesn't exist in the database
                                skillToAdd = new Skill { Name = skillName };
                            }

                            // Add the skill to the in-memory collection
                            existingProfile.Skills.Add(skillToAdd);
                        }
                    }
                }
                await profileRepo.UpdateAsync(existingProfile);
                await _unitOfWork.CompleteAsync();
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Profile Updated Successfully",
                    Data = new
                    {
                        Id = existingProfile.Id,
                        UserId = existingProfile.UserId,
                        Skills = existingProfile.Skills?.Select(s => s.Name).ToList()
                    }
                };
            }
            catch(Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"An Error Occurred: {ex.Message}",
                    ErrorCode = ErrorCodes.Excptions
                };
            }
        }
    }
}
