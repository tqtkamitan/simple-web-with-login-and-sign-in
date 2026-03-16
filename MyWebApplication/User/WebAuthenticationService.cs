using Application.DTOs;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace MyWebApplication.User
{
    public class WebAuthenticationService
    {
        private readonly IHttpContextAccessor _http;

        public WebAuthenticationService(IHttpContextAccessor http)
        {
            _http = http;
        }

        public async Task SignInAsync(UserDto user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

            var identity = new ClaimsIdentity(claims, "Cookies");

            await _http.HttpContext!.SignInAsync(
                "Cookies",
                new ClaimsPrincipal(identity)
            );
        }
    }
}
