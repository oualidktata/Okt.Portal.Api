using Portal.Api.Repositories.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Portal.Api.Dtos.Models
{
    /// <summary>
    /// Account DTO
    /// </summary>
    public class AccountDto
    {

        /// <summary>
        /// Unique identifier of the account
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// AccountCode
        /// </summary>
        [Required]
        [MaxLength(7)]
        public string Code { get; set; }

        /// <summary>
        /// Represents a name in english
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Array of translations for Name
        /// </summary>
        [Required]
        public TranslatableName[] TranslatableNames { get; set; }

        /// <summary>
        /// The date the account was opened (Short Date)
        /// </summary>
        [Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:YYYY-MM-DD}")]
        public DateTime OpenDate { get; set; }

        /// <summary>
        /// Indicates whether the account is active
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

    }
}