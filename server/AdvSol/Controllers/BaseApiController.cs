using AdvSol.Services.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AdvSol.Controllers
{
    public class BaseApiController : ControllerBase
    {
        protected ICurrentUser _currentUser;
        protected IMapper _mapper;

        public BaseApiController(ICurrentUser currentUser, IMapper mapper)
        {
            _currentUser = currentUser;
            _mapper = mapper;
        }
    }
}
