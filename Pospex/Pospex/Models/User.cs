using Microsoft.AspNetCore.Identity;

namespace Pospex.Models
{
    public class User : IdentityUser
    {
        //TODO - подумать нужно ли тут = null
        public string Role { get; set; } = null;
        public byte[] Avatar { get; set; } = null;
    }
}
