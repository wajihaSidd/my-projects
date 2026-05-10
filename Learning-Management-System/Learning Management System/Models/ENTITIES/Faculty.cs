using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Management_System.Models.ENTITIES
{
    [Table("Faculty")]
    public class Faculty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // <-- Add this
        [Column("FacultyId")]
        public int FacultyId { get; set; }


        [Required, StringLength(100)]
        [Column("FacultyName")]
        public string Name { get; set; } = null!;

        [Required, StringLength(100)]
        [EmailAddress]
        [Column("FacultyEmail")]
        public string Email { get; set; } = null!;

        [Required, StringLength(50)]
        [Column("FacultyPassword")]
        public string Password { get; set; } = null!;

        [StringLength(20)]
        [Column("PhoneNumber")]
        public string Phone { get; set; } = null!;

        [StringLength(200)]
        [Column("Address")]
        public string Address { get; set; } = null!;

        // Foreign key
        [Required]
        [Column("DepartmentId")]
        public int DepartmentId { get; set; }

        // Navigation property (nullable)
        public Department? Department { get; set; }

        // Navigation collections
        public ICollection<CourseFaculty> CourseFaculty { get; set; } = new List<CourseFaculty>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
