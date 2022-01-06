using System.Collections.Generic;
using WebApiAuth.Models;
using WebApiAuth.Models.Dto;

namespace WebApiAuth.Repository.Interfaces
{
    public interface IUserRepository:IBaseRepository<User>
    {
        List<PermissionDto> GetUserPermissions(User user);
        List<PermissionDto> GetRolePermissions(User user);
        List<PermissionDto> GetAllPermissions(User user);
        User Login(LoginDto loginDto);
        void Register(RegisterDto registerDto);
        List<Role> GetRoles(User user);


    }
}
