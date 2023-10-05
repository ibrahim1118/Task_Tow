using Microsoft.AspNetCore.Identity;

namespace AuthenticationTask.Models
{
    public class AppUser : IdentityUser
    {
        public string FName { get; set; }

        public string LName { get; set; }

    }
}
