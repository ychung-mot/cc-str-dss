using AdvSol.Data;
using AdvSol.Services.Dtos;
using AutoMapper;

namespace AdvSol.Services
{
    public class ServiceBase
    {
        protected ICurrentUser _currentUser;
        protected IFieldValidatorService _validator;
        protected IUnitOfWork _unitOfWork;
        protected IMapper _mapper;

        public ServiceBase(ICurrentUser currentUser, IFieldValidatorService validator, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _currentUser = currentUser;
            _validator = validator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
