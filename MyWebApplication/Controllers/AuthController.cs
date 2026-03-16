using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApplication.Models;
using MyWebApplication.User;
using Repository.Entities;
using System.Security.Claims;
using System.Security.Principal;

namespace MyWebApplication.Controllers
{
    // This for outside view
    //[ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly WebAuthenticationService _authService;

        public AuthController(IUserService userService, WebAuthenticationService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new RegisterDto
            {
                Username = model.UserName,
                Password = model.Password
            };

            var user = await _userService.RegisterAsync(dto);

            await _authService.SignInAsync(user);

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new LoginDto
            {
                Username = model.UserName,
                Password = model.Password
            };

            var user = await _userService.LoginAsync(dto);

            await _authService.SignInAsync(user);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login");
        }

        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }
    }
}
