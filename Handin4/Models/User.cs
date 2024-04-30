using Microsoft.AspNetCore.Identity;

namespace Handin4.Models
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
    }
}
