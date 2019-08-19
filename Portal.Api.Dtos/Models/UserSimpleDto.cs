using System.ComponentModel.DataAnnotations;

namespace Portal.Api.Repositories.Models
{
    /// <summary>
    /// User DTO
    /// </summary>
    public class UserSimpleDto
    {
        /// <summary>
        /// Unique Identifier of a user
        /// </summary>
        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(11)]
        public string UserCode { get; set; }
    }
}