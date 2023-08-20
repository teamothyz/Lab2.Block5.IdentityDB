using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Request
{
    public class DeleteRoleRequest
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        [FromRoute(Name = "email")]
        public string Email { get; set; }

        [Required]
        [FromRoute(Name = "role")]
        public string Role { get; set; }
    }
}
