using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PracticeProjectWebAPI.DataBaseConnection;
using PracticeProjectWebAPI.Models;
using PracticeProjectWebAPI.services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PracticeProjectWebAPI.services.ServiceExtension
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        public AuthService(ApplicationDbContext context,IConfiguration configuration,IPasswordHasher<AppUser> passwordHasher)
        {
            _configuration = configuration;
            _context = context;
            _passwordHasher = passwordHasher;
        }

      
        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Email==request.Email);
            if (user == null)
                throw new Exception("User Does not exist, Please Register");
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Wrong password");
            return GenerateJwtToken(user);
        }

        public async Task<string> RegisterAsync(RegisterRequest request)
        {
            if(await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new Exception("user already exists");
            }
            var user = new AppUser
            {
                Email = request.Email
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return GenerateJwtToken(user);
        }
        private string GenerateJwtToken(AppUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role)
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                );
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials:creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<string> ForgetPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Email==email);
            if (user == null)
                return ("user not found, Register");
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            user.PasswordResetTokenHash = HashToken(token);
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(30);
            await _context.SaveChangesAsync();
            return token;
        }

       private string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }

        public async Task<bool> ResetPassword(string token, string newPassword)
        {
            var hash = HashToken(token);
            var user = await _context.Users
                .FirstOrDefaultAsync(u=>u.PasswordResetTokenHash==hash && u.PasswordResetTokenExpiry>DateTime.UtcNow);
            if (user == null)
                return false;
            var hasher = new PasswordHasher<AppUser>();
            user.PasswordHash = hasher.HashPassword(user,newPassword);
            user.PasswordResetTokenExpiry = null;
            user.PasswordResetTokenHash = null;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
