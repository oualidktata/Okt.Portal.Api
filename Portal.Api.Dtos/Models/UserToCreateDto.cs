using System.ComponentModel.DataAnnotations;

namespace Portal.Api.Repositories.Models
{
    /// <summary>
    /// User DTO
    /// </summary>
    public class UserToCreateDto
    {
        [Required]
        [MaxLength(11)]
        public string UserCode { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string LanguageCode { get; set; }
        [Required]
        public bool IsActive { get; set; }

    }
}