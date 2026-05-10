using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Management_System.Models.ENTITIES
{
    [Table("Students")]
    public class Students
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // <-- add this
        [Column("StudentId")]
        public int StudentId { get; set; }

        [Required, StringLength(100)]
        [Column("StudentName")]
        public string Name { get; set; } = null!;

        [Required, StringLength(50)]
        [Column("RollNumber")]
        public string RollNumber { get; set; } = null!;

        [Required, StringLength(100)]
        [EmailAddress]
        [Column("EmailAddress")]
        public string Email { get; set; } = null!;

        [Required, StringLength(100)]
        [Column("StudentPassword")]
        public string Password { get; set; } = null!;

        [Range(0, 100)]
        [Column("Percentage")]
        public double Percentage { get; set; }

        [StringLength(200)]
        [Column("Address")]
        public string Address { get; set; } = null!;

        // Foreign Key
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Department is required.")]
        [Column("DepartmentId")]
        public int DepartmentId { get; set; }

        // Navigation property (nullable)
        public Department? Department { get; set; }

        // Related collections
        public ICollection<CourseStudents> CourseStudents { get; set; } = new List<CourseStudents>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        [ValidateNever]
        public ICollection<QuizResult> QuizResults { get; set; } = new List<QuizResult>();

    }
}
