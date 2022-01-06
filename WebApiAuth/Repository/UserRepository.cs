using System.Collections.Generic;
using System.Linq;
using WebApiAuth.DB;
using WebApiAuth.Models;
using WebApiAuth.Models.Dto;
using WebApiAuth.Repository.Interfaces;

namespace WebApiAuth.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private MarketContext _db;
        public UserRepository(MarketContext db) : base(db)
        {
            _db = db;
        }

        public List<PermissionDto> GetAllPermissions(User user)
        {
            var permissions = GetRolePermissions(user);
            permissions.AddRange(GetUserPermissions(user));
            return permissions;
        }

        public List<PermissionDto> GetRolePermissions(User user)
        {
            var permissions = (from userRole in _db.UserRoles.Where(x => x.UserId == user.Id).ToList()
                               from rolePermission in _db.RolePermissions.Where(x => x.RoleId == userRole.Id).ToList()
                               select new PermissionDto()
                               {
                                   PermissionType = rolePermission.PermissionType,
                                   PermissionValue = rolePermission.PermissionValue
                               }
                               )

                               .ToList();
            return permissions;
        }

        public List<Role> GetRoles(User user)
        {
            var roles = (from userRole in _db.UserRoles.Where(x => x.UserId == user.Id).ToList()
                         from role in _db.Roles.Where(x => x.Id == userRole.RoleId).ToList()
                         select role).ToList();
            return roles;
        }

        public List<PermissionDto> GetUserPermissions(User user)
        {
            var permissions = (from userPermission in _db.UserPermissions.Where(x => x.UserId == user.Id).ToList()
                               select new PermissionDto()
                               {
                                   PermissionType = userPermission.PermissionType,
                                   PermissionValue = userPermission.PermissionValue
                               }).ToList();
            return permissions;
        }

        public User Login(LoginDto loginDto)
        {
            var user = _db.Users.FirstOrDefault(x => x.Email == loginDto.Email &&
                            x.Password == loginDto.Password);
            return user;
        }

        public void Register(RegisterDto registerDto)
        {
            var user = _db.Users.FirstOrDefault(x => x.Email == registerDto.Email);
            if (user != null)
                throw new System.Exception("Aynı mail ile kullanıcı bulunmaktadır");
            _db.Users.Add(new User
            {
                Email = registerDto.Email,
                Name = registerDto.Name,
                Password = registerDto.Password
            });
            _db.SaveChanges();
        }
    }
}
