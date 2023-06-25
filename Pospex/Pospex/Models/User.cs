using Microsoft.AspNetCore.Identity;

namespace Pospex.Models
{
    public class User : IdentityUser
    {
        public string Role { get; set; } 
        public byte[] Avatar { get; set; } 
    }
}
