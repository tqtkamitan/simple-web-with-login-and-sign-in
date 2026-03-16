using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<User> _hasher;

        public UserService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _hasher = new PasswordHasher<User>();
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
                throw new Exception("Username already exists.");

            var user = new User
            {
                Username = registerDto.Username,
                Role = registerDto.Role
            };
            user.PasswordHash = _hasher.HashPassword(user, registerDto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto { Id = user.Id, Username = user.Username, Role = user.Role };
        }

        public async Task<UserDto> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null)
                throw new Exception("Invalid credentials.");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid credentials.");

            return new UserDto { Id = user.Id, Username = user.Username, Role = user.Role };

            // TODO: for jwt token web api
            //var claims = new[]
            //{
            //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //    new Claim(ClaimTypes.Name, user.Username),
            //    new Claim(ClaimTypes.Role, user.Role.ToString())
            //};

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var token = new JwtSecurityToken(
            //    issuer: _config["Jwt:Issuer"],
            //    audience: _config["Jwt:Audience"],
            //    claims: claims,
            //    expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpireMinutes"])),
            //    signingCredentials: creds
            //);

            //return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _context.Users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role
            }).ToListAsync();
            return users;
        }

    }
}
