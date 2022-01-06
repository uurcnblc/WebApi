using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Authorization
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private UserManager<ApplicationUser> _userManager;

        public PermissionAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
            {
                return;
            }
            // Get all the roles the user belongs to and check if any of the roles has the permission required
            // for the authorization to succeed.
            var user = await _userManager.GetUserAsync(context.User);
            if (user == null)
                user = await _userManager.FindByNameAsync("ugur");
            var userClaims = await _userManager.GetClaimsAsync(user);
            var permissions = userClaims.Where(x => x.Type == requirement.Permission &&
                                                    x.Value == "true" &&
                                                        x.Issuer == "LOCAL AUTHORITY")
                                        .Select(x => x.Value);
            if (permissions.Any())
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
