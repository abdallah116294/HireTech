using AutoMapper;
using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
using HireTech.Core.IServices;
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
        private readonly IProfileService _profileService;

        public ProfileController(IUnitOfWork unitOfWork, IMapper mapper, HireTechDbContext dbContext, IProfileService profileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dbContext = dbContext;
            _profileService = profileService;
        }
        [Authorize(Roles = "CANDIDATE")]
        [HttpPost("AddProfile")]
        public async Task<IActionResult> AddProfile(CreateCandidateProfileDTO dto)
        {
            var userId = GetUserID();
            if (userId == null)
                return Unauthorized("Please Login to add Profile Details");
            var result = await _profileService.AddProfile(dto, userId);
            return CreateResponse(result);
         
           
        }
        [Authorize(Roles = "CANDIDATE")]
        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetUserID();
            if (userId == null)
                return Unauthorized("Please Login to view Profile Details");
            var result = await _profileService.GetProfile(userId);
            return CreateResponse(result);
          
        }
        [Authorize(Roles = "CANDIDATE")]
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(UpdateCandidateProfileDTO dto)
        {
            var userId = GetUserID();
            if (userId == null)
                return Unauthorized("Please Login to view Profile Details");
            var result = await _profileService.UpdateProfile(dto, userId);
            return CreateResponse(result);
        }
    }
}
