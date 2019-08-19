using System;
using System.ComponentModel.DataAnnotations;

namespace Portal.Api.Dtos.Models
{
    public class DocumentDto
    {
        /// <summary>
        /// Unique identifier in the system
        /// </summary>
        public string documentId { get; set; }
        /// <summary>
        /// Document Type Code (CONF_CERT_AN_T)
        /// </summary>
        [Key]
        [Required]
        public string DocumentTypeCode { get; set; }
        /// <summary>
        /// Language preferred by the client(en-US)
        /// </summary>
        [Required]
        public string Language { get; set; }
        ///// <summary>
        ///// Account code (A1153)
        ///// </summary>
        //[Required]
        //public string AccountCode { get; set; }
        /// <summary>
        /// As of Date (2018-09-30)
        /// </summary>
        [Required]
        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{yyyyMMdd}")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:YYYY-MM-DD}")]
        public DateTime AsOfDate { get; set; }
        /// <summary>
        /// Name of the document
        /// EXTRPT_A1153-CONF_CERT_AN_T-2018-09-30.pdf
        /// </summary>
        [Required]
        public string Name { get; set; }
        ///// <summary>
        ///// Physical name as sent 
        ///// Should be Unique in nthe system
        ///// i.e.: CONF_CERT_AN_T~^en-US~^A1153~^20180930~^EXTRPT_A1153-CONF_CERT_AN_T-2018-09-30.pdf
        ///// </summary>
        //[Required]
        //public string OriginalFileName { get; set; }
    }
}
