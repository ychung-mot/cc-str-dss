using AdvSol.Authentication;
using AdvSol.Authorization;
using AdvSol.Services;
using AdvSol.Services.Dtos;
using AdvSol.Services.Dtos.SystemUser;
using AdvSol.Utils;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdvSol.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class SystemUsersController : BaseApiController
    {
        private ISystemUserService _systemUserService;
        private IConfiguration _config;

        public SystemUsersController(ISystemUserService systemUserService, IConfiguration config, ICurrentUser currentUser, IMapper mapper)
            : base(currentUser, mapper)
        {
            _systemUserService = systemUserService;
            _config = config;
        }

        [HttpGet("", Name = "GetSystemUsers")]
        [AdvAuthorize(Roles.Admin)]
        public async Task<ActionResult<SystemUserDto[]>> GetSystemUsersAsync()
        {
            var users = await _systemUserService.GetSystemUsersAsync();

            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetSystemUser")]
        [AdvAuthorize(Roles.Admin)]
        public async Task<ActionResult<SystemUserDto>> GetUserAsync(int id)
        {
            var user = await _systemUserService.GetSystemUserAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<SystemUserDto>> CreateUser(SystemUserCreateDto dtoCreate)
        {
            var dto = _mapper.Map<SystemUserDto>(dtoCreate);

            var response = await _systemUserService.CreateSystemUserAsync(dto);

            if (response.errors.Count > 0)
            {
                return ValidationUtils.GetValidationErrorResult(response.errors, ControllerContext);
            }

            return Ok(response.dto);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ICurrentUser>> Login(CredentialDto credential)
        {
            var errors = new Dictionary<string, List<string>> { ["Password"] = new List<string> { "Invalid username or password." } };

            var tokenSecret = _config.GetValue<string>("InternalTokenSecret") ?? "";
            var currentUser = new CurrentUser();

            var systemUser = await _systemUserService.GetSystemUserByUsernameAsync(credential.Username);

            if (systemUser == null)
            {
                return ValidationUtils.GetValidationErrorResult(errors, ControllerContext);
            }

            var pwdHashed = credential.Password.ComputeSHA256Hash();

            if (systemUser.Password != pwdHashed)
            {
                return ValidationUtils.GetValidationErrorResult(errors, ControllerContext);
            }

            currentUser.Id = systemUser.Id; ;
            currentUser.Username = credential.Username;
            currentUser.LastName = systemUser.LastName ?? "";
            currentUser.Token = TokenHelper.GenerateAccessToken(credential.Username, tokenSecret);
            currentUser.Role = systemUser.Role?.CodeValue;

            return Ok(currentUser);
        }
    }
}
