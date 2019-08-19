using Portal.Api.Repositories.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Portal.Api.Dtos.Models
{
    public class AccountToCreateDto
    {
        /// <summary>
        /// AccountCode
        /// </summary>
        [Required]
        [MaxLength(10)]
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
        /// The date when the account was opened (YYYY-MM-DD) ISO-8601
        ///  <remarks>example: 2019-06-01</remarks>
        /// </summary>
        [Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:YYYY-MM-DD}")]
        public DateTime OpenDate { get; set; }

        /// <summary>
        /// Whether the account is active or not.
        /// </summary>
        [Required]
        public bool IsActive { get; set; }
    }
}