using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using TakeDeal.DTOs;
using TakeDeal.Models;
using TakeDeal.Services;

namespace TakeDeal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context,TokenService tokenService)
        {
            this._context = context;
            this._tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Check if email already exists
            bool exist = await _context.Users.AnyAsync(u => u.Email == registerDto.Email);
            if(exist)
            {
                return BadRequest("Email already registered");
            }
            // Create new user
            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Phone = registerDto.Phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password) // Hash the password
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1️⃣ Find user by email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return Unauthorized("Invalid email or password");

            // 2️⃣ Verify password
            bool isPasswordValid =
                BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!isPasswordValid)
                return Unauthorized("Invalid email or password");

            var token = _tokenService.CreateToken(user);

            // 3️⃣ SUCCESS (temporary response — JWT next)
            return Ok(new
            {
                message = "Login successful",
                token = token,
                userId = user.Id,
                name = user.Name,
                email = user.Email
            });
        }
    }
}
