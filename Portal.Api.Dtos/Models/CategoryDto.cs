using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Portal.Api.Repositories.Models
{
    /// <summary>
    /// Category of document types.
    /// </summary>
    public class CategoryDto 
    {

        /// <summary>
        /// Internal Id of the CategoryId.
        /// </summary>
        public int Id { get; set; }
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

        /// <summary>
        /// List of child document types.
        /// </summary>
        public IEnumerable<DocumentTypeDto> DocumentTypes { get; set; }

    }
}       