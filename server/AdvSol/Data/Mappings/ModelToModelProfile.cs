using AdvSol.Services.Dtos.StrApplication;
using AdvSol.Services.Dtos.SystemUser;
using AutoMapper;

namespace AdvSol.Data.Mappings
{
    public class ModelToModelProfile : Profile
    {
        public ModelToModelProfile()
        {
            CreateMap<StrApplicationCreateDto, StrApplicationDto>();
            CreateMap<StrApplicationUpdateDto, StrApplicationDto>();
            CreateMap<SystemUserCreateDto, SystemUserDto>();
        }
    }
}
