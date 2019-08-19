namespace Portal.Api.Repositories.Models
{
    /// <summary>
    /// A POCO representation of the physical file
    /// CONF_CERT_AN_T~^fr-CA~^A1082~^20181231~^ADDN Test Document_PPP
    /// </summary>
    public class DocumentFileDto
    {
        /// <summary>
        /// The name of the file as exported
        /// CONF_CERT_AN_T~^fr-CA~^A1082~^20181231~^ADDN Test Document_PPP.pdf
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// The file name as it downloaded
        /// ex:ADDN Test Document_PPP.pdf
        /// </summary>
        public string LogicalName { get; set; }
    }
}