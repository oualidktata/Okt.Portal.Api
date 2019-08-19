using Sieve.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Portal.Api.Repositories.Models
{
    /// <summary>
    /// An Association DTO has a unique Key: userCode-AccountCode-DocumentType
    /// </summary>
    public class AssociationDto
    {
        [Required]
        [MaxLength(11)]
        [Sieve(CanFilter = true, CanSort = true)]
        public string  UserCode { get; set; }
        [Required]
        [MaxLength(7)]
        [Sieve(CanFilter = true, CanSort = true)]
        public string AccountCode { get; set; }
        [Required]
        [MaxLength(20)]
        [Sieve(CanFilter = true, CanSort = true)]
        public string DocumentTypeCode { get; set; }

        [Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:YYYY-MM-DD}")]
        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime CreatedOn { get; set; }

        [Required]
        public bool IsActive { get; set; }

    }
}
    