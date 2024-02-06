using AdvSol.Utils;
using System.Security.Claims;

namespace AdvSol.Services.Dtos
{
    public interface ICurrentUser
    {
        int Id { get; set; }
        string Username { get; set; }
        string LastName { get; set; }
        string Role { get; set; }
        string Token { get; set; }
        void LoadUserSession(ClaimsPrincipal user);
        void AddClaim(ClaimsPrincipal user, string claimType, string value);
    }

    public class CurrentUser : ICurrentUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Role { get; set; }
        public string Token { get; set; } = "";
        public void LoadUserSession(ClaimsPrincipal user)
        {
            if (user == null)
                return;

            Username = user.GetCustomClaim(ClaimTypes.NameIdentifier);
        }
        public void AddClaim(ClaimsPrincipal user, string claimType, string value)
        {
            if (user == null || claimType.IsEmpty() || value.IsEmpty() || user.HasClaim(claimType, value)) return;

            user.Identities.FirstOrDefault().AddClaim(new Claim(claimType, value));
        }
    }
}
