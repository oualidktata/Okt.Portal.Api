using System.ComponentModel.DataAnnotations;

namespace Portal.Api.Repositories.Models
{
    public class AssociationSimpleDto
    {
        [Required]
        [MaxLength(11)]
        public string userCode { get; set; }
        [Required]
        [MaxLength(7)]
        public string AccountCode { get; set; }
        [Required]
        [MaxLength(20)]
        public string DocumentTypeCode { get; set; }

    }
}