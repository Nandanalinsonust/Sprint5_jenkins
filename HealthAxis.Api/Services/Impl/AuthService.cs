using HealthAxis.Api.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthAxis.Api.Services.Impl
{
    public class AuthService(UserManager<IdentityUser> userManager, IConfiguration config) : IAuthService
    {
        public async Task<(bool Success, string Message, AuthResponse? Data, int ExpiresIn)> Login(LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return (false, "Invalid credentials", null, 0);
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return (false, "Invalid credentials", null, 0);
            }

            var token = await GenerateToken(user);

            var roles = await userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "";

            var expiry = int.Parse(config["Jwt:AccessTokenExpirationMinutes"]!);

            var response = new AuthResponse
            {
                Token = token,
                Role = role,
                UserId = user.Id
            };

            return (true, "User Logged in Successfully", response, expiry);
        }

        public async Task<(bool Success, string Message, string UserId)> Register(RegisterDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return (false, "Password Do Not Match", string.Empty);
            }

            if (request.Role != "Admin" && request.Role != "Patient" && request.Role != "Doctor")
            {
                return (false, "Invalid role", string.Empty);
            }

            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return (false, "User already exists", string.Empty);
            }

            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                return (false, errors, string.Empty);
            }

            await userManager.AddToRoleAsync(user, request.Role);

            return (true, "User Registered Successfully", user.Id);
        }

        private async Task<string> GenerateToken(IdentityUser user)
        {
            var jwtSettings = config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expirationMinutes = int.Parse(jwtSettings["AccessTokenExpirationMinutes"]!);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}