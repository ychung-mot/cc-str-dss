using AdvSol.Services;
using AdvSol.Services.Dtos;
using AdvSol.Services.Dtos.CommonCode;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdvSol.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class CommonCodesController : BaseApiController
    {
        private ICommonCodeService _commonCodeService;

        public CommonCodesController(ICommonCodeService commonCodeService, ICurrentUser currentUser, IMapper mapper)
            : base(currentUser, mapper)
        {
            _commonCodeService = commonCodeService;
        }

        [AllowAnonymous]
        [HttpGet("", Name = "GetCommonCodes")]
        public async Task<ActionResult<CommonCodeDto[]>> GetCommonCodesAsync()
        {
            var codes = await _commonCodeService.GetCommonCodesAsync();

            return Ok(codes);
        }
    }
}
