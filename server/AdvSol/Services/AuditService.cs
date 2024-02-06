using AdvSol.Data.Repositories;
using AdvSol.Data;
using AdvSol.Services.Dtos.Audit;
using AdvSol.Services.Dtos;
using AutoMapper;

namespace AdvSol.Services
{
    public interface IAuditService
    {
        Task<AuditDto[]> GetAuditsAsync(string entity, int id);
    }
    public class AuditService : ServiceBase, IAuditService
    {
        private readonly IAuditRepository _auditRepo;

        public AuditService(ICurrentUser currentUser, IFieldValidatorService validator, IAuditRepository auditRepo, IUnitOfWork unitOfWork, IMapper mapper)
            : base(currentUser, validator, unitOfWork, mapper)
        {
            _auditRepo = auditRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuditDto[]> GetAuditsAsync(string entity, int id)
        {
            return _mapper.Map<AuditDto[]>(await _auditRepo.GetAuditRecords(entity, id));
        } 
    }
}
