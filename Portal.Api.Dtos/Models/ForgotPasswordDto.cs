using System.ComponentModel.DataAnnotations;

namespace Portal.Api.Dtos.Models
{
    /// <summary>
    /// Model used for the Forgot password feature
    /// </summary>
    public class ForgotPasswordDto
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}