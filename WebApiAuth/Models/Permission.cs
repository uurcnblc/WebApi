namespace WebApiAuth.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public int ParentPermissionId { get; set; }
        public string PermissionDescription { get; set; }
        public string PermissionType { get; set; }
    }
}
