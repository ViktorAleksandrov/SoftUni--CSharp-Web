using Microsoft.AspNetCore.Identity;

namespace CHUSHKA.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
    }
}
