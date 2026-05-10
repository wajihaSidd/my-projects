using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Management_System.Models.ENTITIES
{
    [Table("Certificates")]
    public class Certificate
    {
        [Key]
        public int CertificateId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required, StringLength(200)]
        public string CertificateType { get; set; } // "Course Completion" or "Graduation"

        [Required]
        public DateTime IssueDate { get; set; } // Always UTC

        // Navigation properties
        public Students Student { get; set; } = null!;
        public Courses Course { get; set; } = null!;
    }
}
