namespace WebApiAuth.Models
{
    public class UserPermission
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PermissionType { get; set; }
        public string PermissionValue { get; set; }
    }
}
