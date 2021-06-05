using System.ComponentModel.DataAnnotations;

namespace aspnet_core_3_jwt_authentication_tutorial_with_example_api.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
