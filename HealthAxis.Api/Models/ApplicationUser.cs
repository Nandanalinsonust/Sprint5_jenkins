using Microsoft.AspNetCore.Identity;

namespace HealthAxis.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsFirstLogin { get; set; } = true;
        public bool IsActive { get; set; } = true;
    }
}