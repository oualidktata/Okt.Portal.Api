using System.ComponentModel.DataAnnotations;

namespace Portal.Api.Repositories.Models
{
    /// <summary>
    /// A representation of a Document Type DTO.
    /// </summary>
    public class DocumentTypeDto
    {

        /// <summary>
        /// Unique identifier of the document Type
        /// </summary>
        [Key]
        [Required]
        public int DocumentTypeId { get; set; }
        /// <summary>
        /// DocumentTypeCode
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
        /// The category of the document Type
        /// </summary>
        [Required]
        public CategoryDto Category { get; set; }
        /// <summary>
        /// The language of the document Type
        /// </summary>
        [Required]
        [MaxLength(4)]
        public string Language { get; set; }
    }
}