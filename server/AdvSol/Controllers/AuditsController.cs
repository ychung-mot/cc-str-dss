using AdvSol.Authorization;
using AdvSol.Services;
using AdvSol.Services.Dtos;
using AdvSol.Services.Dtos.Audit;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdvSol.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuditsController : BaseApiController
    {
        private IAuditService _auditService;

        public AuditsController(IAuditService auditService, ICurrentUser currentUser, IMapper mapper)
            : base(currentUser, mapper)
        {
            _auditService = auditService;
        }

        [AdvAuthorize]
        [HttpGet("{entity}/{id}", Name = "GetAudits")]
        public async Task<ActionResult<AuditDto[]>> GetAuditsAsync(string entity, int id)
        {
            var auditRecords = await _auditService.GetAuditsAsync(entity, id);

            return Ok(auditRecords);
        }
    }
}
