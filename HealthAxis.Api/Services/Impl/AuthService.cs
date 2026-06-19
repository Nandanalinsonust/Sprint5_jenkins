using HealthAxis.Api.Models;
using HealthAxis.Api.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthAxis.Api.Services.Impl
{
    public class AuthService(UserManager<ApplicationUser> userManager, IConfiguration config) : IAuthService
    {
        public async Task<(bool Success, string Message, AuthResponse? Data, int ExpiresIn)> Login(LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return (false, "Invalid credentials", null, 0);

            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
                return (false, "Invalid credentials", null, 0);

            if (user.IsFirstLogin)
            {
                return (false, "FirstLogin", null, 0);
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
                return (false, "Password Do Not Match", "");

            if (request.Role == "Doctor")
                return (false, "Doctors must be created by Admin", "");

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                IsFirstLogin = false
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return (false, "Error creating user", "");

            await userManager.AddToRoleAsync(user, request.Role);

            return (true, "User Registered Successfully", user.Id);
        }

        public async Task<(bool Success, string Message)> CreateDoctorUser(string email)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                IsFirstLogin = true
            };

            var result = await userManager.CreateAsync(user, "Doctor@123");

            if (!result.Succeeded)
                return (false, "Doctor creation failed");

            await userManager.AddToRoleAsync(user, "Doctor");

            return (true, "Doctor user created");
        }

        public async Task<(bool Success, string Message)> ChangePassword(string email, string oldPassword, string newPassword)
        {
            var user = await userManager.FindByEmailAsync(email);

            var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (!result.Succeeded)
                return (false, "Password change failed");

            user.IsFirstLogin = false;
            await userManager.UpdateAsync(user);

            return (true, "Password changed successfully");
        }

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var jwtSettings = config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(jwtSettings["AccessTokenExpirationMinutes"]!)
                ),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}