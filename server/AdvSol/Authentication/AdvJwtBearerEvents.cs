using AdvSol.Services;
using AdvSol.Services.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AdvSol.Authentication
{
    public class AdvJwtBearerEvents : JwtBearerEvents
    {
        private readonly ICurrentUser _currentUser;
        private readonly ISystemUserService _systemUserService;

        public AdvJwtBearerEvents(ICurrentUser currentUser, ISystemUserService systemUserService) : base()
        {
            _currentUser = currentUser;
            _systemUserService = systemUserService;
        }

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            _currentUser.LoadUserSession(context.Principal);

            var systemUser = await _systemUserService.GetSystemUserByUsernameAsync(_currentUser.Username);

            if (systemUser == null)
            {
                var errorMessage = $"{_currentUser.Username} is not registered.";
                context.Response.StatusCode = 401;
                context.Fail($"Unauthorized: {errorMessage}");
                return;
            }

            _currentUser.Id = systemUser.Id;
            _currentUser.Username = systemUser.Username;
            _currentUser.LastName = systemUser.LastName ?? "";
            _currentUser.Role = systemUser.Role.CodeValue;
            _currentUser.AddClaim(context.Principal, "role", systemUser.Role.CodeValue);

            await Task.CompletedTask;
        }
    }
}
