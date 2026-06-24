using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using HealthAxis.Api.Models;

namespace HealthAxis.Api.Filters
{
    public class FirstLoginFilter : IAsyncActionFilter
    {
        private readonly UserManager<ApplicationUser> userManager;

        public FirstLoginFilter(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                var user = await userManager.FindByIdAsync(userId);

                if (user != null && user.IsFirstLogin &&
                    !context.HttpContext.Request.Path.Value.Contains("change-password"))
                {
                    context.Result = new UnauthorizedObjectResult("Change password first");
                    return;
                }
            }

            await next();
        }
    }
}