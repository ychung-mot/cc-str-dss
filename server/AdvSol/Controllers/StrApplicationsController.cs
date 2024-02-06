using AdvSol.Authorization;
using AdvSol.Services;
using AdvSol.Services.Dtos;
using AdvSol.Services.Dtos.StrApplication;
using AdvSol.Utils;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AdvSol.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class StrApplicationsController : BaseApiController
    {
        private IStrApplicationService _strApplicationService;

        public StrApplicationsController(IStrApplicationService strApplicationService, ICurrentUser currentUser, IMapper mapper)
            : base(currentUser, mapper) 
        {
            _strApplicationService = strApplicationService;
        }

        [AdvAuthorize()]
        [HttpGet("", Name = "GetStrApplications")]
        public async Task<ActionResult<StrApplicationDto[]>> GetStrApplications()
        {
            bool isAdmin = _currentUser.Role == Roles.Admin;
            var strApplications = isAdmin ? 
                await _strApplicationService.GetStrApplicationsAsync() : 
                await _strApplicationService.GetStrApplicationsByApplicantAsync(_currentUser.Id);

            return Ok(strApplications);
        }

        [AdvAuthorize()]
        [HttpGet("{id}", Name = "GetStrApplication")]
        public async Task<ActionResult<StrApplicationDto>> GetStrApplication(int id)
        {
            var strApplications = await _strApplicationService.GetStrApplicationAsync(id);

            if (strApplications == null)
                return NotFound();

            return Ok(strApplications);
        }

        [AdvAuthorize(Roles.Applicant)]
        [HttpPost]
        public async Task<ActionResult<StrApplicationDto>> CreateStrApplication(StrApplicationCreateDto dtoCreate)
        {
            var dto = _mapper.Map<StrApplicationDto>(dtoCreate);

            var response = await _strApplicationService.CreateStrApplicationAsync(dto);

            if (response.errors.Count > 0)
            {
                return ValidationUtils.GetValidationErrorResult(response.errors, ControllerContext);
            }

            return Ok(response.dto);
        }

        [AdvAuthorize(Roles.Applicant, Roles.Admin)]
        [HttpPut]
        public async Task<ActionResult> UpdatetrApplication(StrApplicationUpdateDto dtoUpdate)
        {
            var dto = _mapper.Map<StrApplicationDto>(dtoUpdate);

            var response = await _strApplicationService.UpdateStrApplicationAsync(dto);

            if (response.notFound)
            {
                return NotFound();
            }

            if (response.errors.Count > 0)
            {
                return ValidationUtils.GetValidationErrorResult(response.errors, ControllerContext);
            }

            return NoContent();
        }
    }
}
