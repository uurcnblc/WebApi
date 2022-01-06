namespace WebApiAuth.Models
{
    public class RolePermission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string PermissionType { get; set; }
        public string PermissionValue { get; set; }
    }
}
