using Microsoft.AspNetCore.Identity;

namespace Api.Data
{
    public class User : IdentityUser
    {
        public DateTime? Dob { get; set; }
    }
}
