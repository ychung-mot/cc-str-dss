using AdvSol.Data.Entities;
using AdvSol.Services.Dtos.Audit;
using AdvSol.Services.Dtos.CommonCode;
using AdvSol.Services.Dtos.Notification;
using AdvSol.Services.Dtos.StrApplication;
using AdvSol.Services.Dtos.SystemUser;
using AutoMapper;

namespace AdvSol.Data.Mappings
{
    public class EntityToModelProfile : Profile
    {
        public EntityToModelProfile()
        {
            CreateMap<Audit, AuditDto>();
            CreateMap<CommonCode, CommonCodeDto>();
            CreateMap<StrApplication, StrApplicationDto>();
            CreateMap<SystemUser, SystemUserDto>();
            CreateMap<SystemUser, ApplicantDto>();
            CreateMap<Notification, NotificationDto>();
        }
    }
}
