using Application.DTOs;
using Repository.Entities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<UserDto> LoginAsync(LoginDto loginDto);
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto> GoogleLoginAsync(RegisterDto loginDto);
    }
}
