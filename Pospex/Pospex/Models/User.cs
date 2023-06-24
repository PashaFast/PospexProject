using Microsoft.AspNetCore.Identity;

namespace Pospex.Models
{
    public class User : IdentityUser
    {
        public byte[] ProfilePicture { get; set; }
    }
}
