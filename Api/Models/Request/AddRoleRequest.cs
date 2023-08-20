using System.ComponentModel.DataAnnotations;

namespace Api.Models.Request
{
    public class AddRoleRequest
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
