using Application.Interfaces;
using System.Security.Claims;

namespace MyWebApplication.User
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var id = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value;

                return int.TryParse(id, out var parsed) ? parsed : null;
            }
        }

        public string? Username =>
            _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }
}
