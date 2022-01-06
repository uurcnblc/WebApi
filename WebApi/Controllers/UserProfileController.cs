using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;

        public UserProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<Object> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return new
            {
                user.FullName,
                user.Email,
                user.UserName
            };
        }

        [HttpGet]
        [Route("SeedData")]
        public async Task<Object> SeedData()
        {
            var applicationUser = new ApplicationUser()
            {
                UserName = "ugur",
                FullName = "ugur",
                Email = "ugur@gmail.com",
            };
            var result = await _userManager.CreateAsync(applicationUser, "Ugur.1234");
            await _userManager.AddToRoleAsync(applicationUser, "Admin");
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("ForAdmin")]
        public async Task<Object> GetForAdmin()
        {
            return "For Admin";
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        [Route("GetForCustomer")]
        public async Task<Object> GetForCustomer()
        {
            return "For Customer";
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        [Route("GetForAdminOrCustomer")]
        public async Task<Object> GetForAdminOrCustomer()
        {
            return "For Adnmin or Customer";
        }
    }
}
