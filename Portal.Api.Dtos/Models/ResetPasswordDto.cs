using System.ComponentModel.DataAnnotations;

namespace Portal.Api.Repositories.Models
{
    public class ResetPasswordDto
    {
         [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}