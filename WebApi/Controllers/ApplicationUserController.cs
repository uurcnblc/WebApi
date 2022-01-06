using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Authorization;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private ApplicationSettings _appSettings;
        public SignInManager<ApplicationUser> _signInManager;
        public ApplicationUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("Register")]
        //POST: /api/ApplicationUser/Register
        public async Task<Object> PostApplicationUser(ApplicationUserDto model)
        {
            var applicationUser = new ApplicationUser()
            {
                UserName = model.UserName,
                FullName = model.FullName,
                Email = model.Email,
            };
            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                await _userManager.AddToRoleAsync(applicationUser, model.Role);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                var roles = await _userManager.GetRolesAsync(user);
                var principal = await _signInManager.CreateUserPrincipalAsync(user);
                var ticket = new AuthenticationTicket(
                    principal,
                    new AuthenticationProperties(),
                    JwtBearerDefaults.AuthenticationScheme);
                var value = SignIn(principal);
                //return SignIn(principal);
                return Ok(TokenHelper.GenerateToken(user, roles.ToList(), userClaims.ToList(), _appSettings));
            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect." });
            }
        }

    }
}
