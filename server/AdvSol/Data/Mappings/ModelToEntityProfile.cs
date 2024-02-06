using AdvSol.Data.Entities;
using AdvSol.Services.Dtos.Audit;
using AdvSol.Services.Dtos.CommonCode;
using AdvSol.Services.Dtos.StrApplication;
using AdvSol.Services.Dtos.SystemUser;
using AutoMapper;

namespace AdvSol.Data.Mappings
{
    public class ModelToEntityProfile : Profile
    {
        public ModelToEntityProfile()
        {
            CreateMap<AuditDto, Audit>();
            CreateMap<CommonCodeDto, CommonCode>();
            CreateMap<StrApplicationDto, StrApplication>();
            CreateMap<SystemUserDto, SystemUser>();
        }
    }
}
