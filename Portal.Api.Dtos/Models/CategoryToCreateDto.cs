using System.ComponentModel.DataAnnotations;

namespace Portal.Api.Repositories.Models
{
    /// <summary>
    /// A model for a category of document types to create.
    /// </summary>
    public class CategoryToCreateDto 
    {
        /// <summary>
        /// The unique code for the category.
        /// </summary>
        public string Code { get; set; }
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

    }
}       