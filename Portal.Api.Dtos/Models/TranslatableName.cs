using System.ComponentModel.DataAnnotations;

namespace Portal.Api.Repositories.Models
{
    /// <summary>
    /// Represents a generic bilingual Name object
    /// </summary>
    public class TranslatableName
    {
        [Required]
        [MaxLength(5)]
        public string CultureCode { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
    }
}