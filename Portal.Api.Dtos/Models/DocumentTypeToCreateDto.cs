using System.ComponentModel.DataAnnotations;

namespace Portal.Api.Repositories.Models
{
    /// <summary>
    /// A representation of a Document Type to create DTO
    /// </summary>
    public class DocumentTypeToCreateDto
    {
        /// <summary>
        /// DocTypeCode
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string DocTypeCode { get; set; }

        /// <summary>
        /// Represents a name in english
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// Array of translations for Name
        /// </summary>
        [Required]
        public TranslatableName[] TranslatableNames { get; set; }

        /// <summary>
        /// The unique code for the category.
        /// </summary>
        [Required]
        public string CategoryCode { get; set; }
     
        /// <summary>
        /// The language of the documentType.
        /// </summary>
        [Required]
        [MaxLength(4)]
        public string Language { get; set; }
    }
}