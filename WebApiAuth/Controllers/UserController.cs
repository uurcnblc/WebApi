using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApiAuth.Models;
using WebApiAuth.Models.Dto;
using WebApiAuth.Repository.Interfaces;

namespace WebApiAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("Register")]
        //POST: /api/ApplicationUser/Register
        public async Task<Object> Register(RegisterDto model)
        {
            try
            {
                _userRepository.Register(model);
                return Ok();
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
            var user = _userRepository.GetFirstOrDefault(x=>x.Email== model.Email,null);
            if (user != null && user.Password == model.Password)
            {
                var userPermissions = _userRepository.GetAllPermissions(user);
                var roles = _userRepository.GetRoles(user);
                return Ok(TokenHelper.GenerateToken(user, roles.ToList(), userClaims.ToList(), _appSettings));
            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect." });
            }
        }
    }
}
