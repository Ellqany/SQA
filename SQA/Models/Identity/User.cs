using Microsoft.AspNetCore.Identity;

namespace SQA.Models.Identity
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Department { get; set; }
    }
}
