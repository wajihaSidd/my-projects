using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Management_System.Models.ENTITIES
{
    [Table("SystemSettings")]
    public class SystemSettings
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string SiteName { get; set; } = "My LMS";

        [StringLength(200)]
        public string LogoUrl { get; set; } = "/images/logo.png";

        [StringLength(200)]
        public string FaviconUrl { get; set; } = "/images/favicon.ico";

        [StringLength(50)]
        public string DefaultLanguage { get; set; } = "en";

        [StringLength(50)]
        public string Timezone { get; set; } = "UTC";

        [StringLength(200)]
        public string ContactEmail { get; set; } = "admin@lms.com";

        [StringLength(200)]
        public string FooterText { get; set; } = "© 2025 LMS";

        public bool EnableRegistration { get; set; } = true;
        public bool MaintenanceMode { get; set; } = false;
    }
}
