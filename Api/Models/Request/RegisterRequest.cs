using System.ComponentModel.DataAnnotations;

namespace Api.Models.Request
{
    public class RegisterRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)] 
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public DateTime Dob { get; set; }
    }
}
