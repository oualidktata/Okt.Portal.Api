using System.ComponentModel.DataAnnotations;

namespace Portal.Api.Dtos.Models
{
    /// <summary>
    /// A simplified representation of a document
    /// </summary>
    public class DocumentSimpleDto
    {
        /// <summary>
        /// Unique identifier in the system
        /// </summary>
        public int documentId { get; set; }
        /// <summary>
        /// Document Type Code (CONF_CERT_AN_T)
        /// </summary>
        [Key]
        [Required]
        public string DocumentTypeCode { get; set; }
        
        /// <summary>
        /// Name of the document
        /// EXTRPT_A1153-CONF_CERT_AN_T-2018-09-30.pdf
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
