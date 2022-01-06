using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class ApplicationUser:IdentityUser
    {
        //BURASI COK ONEMLI
        public ApplicationUser():base()
        {

        }
        [Column(TypeName="nvarchar(150)")]
        public string FullName { get; set; }
    }
}
