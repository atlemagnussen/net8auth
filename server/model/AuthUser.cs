using Microsoft.AspNetCore.Identity;

namespace net8auth.model
{
    public class AuthUser : IdentityUser
    {
        public bool IsDeleted { get; set; }
    }
}