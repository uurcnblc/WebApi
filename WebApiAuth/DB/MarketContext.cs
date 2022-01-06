using Microsoft.EntityFrameworkCore;
using WebApiAuth.Models;

namespace WebApiAuth.DB
{
    public class MarketContext : DbContext
    {
        public MarketContext(DbContextOptions<MarketContext> options)
: base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

    }
}
