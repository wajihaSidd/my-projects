using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Learning_Management_System.Models.ENTITIES
{
    [Table("Courses")]
    public class Courses
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        // ✅ FIX 1: Make Description nullable
        [StringLength(500)]
        public string? Description { get; set; }

        // ✅ FK used in form
        [Required]
        public int DepartmentId { get; set; }

        // ✅ FIX 2: Prevent MVC validation on navigation property
        [ValidateNever]
        public Department Department { get; set; } = null!;

        // ✅ Navigation collections (never validated in forms)
        [ValidateNever]
        public ICollection<CourseStudents> CourseStudents { get; set; } = new List<CourseStudents>();

        [ValidateNever]
        public ICollection<CourseFaculty> CourseFaculty { get; set; } = new List<CourseFaculty>();

        [ValidateNever]
        public ICollection<Attendance> Attendance { get; set; } = new List<Attendance>();
        [ValidateNever]
        public ICollection<CourseOutline> CourseOutlines { get; set; } = new List<CourseOutline>();

        [ValidateNever]
        public ICollection<CourseFiles> CourseFiles { get; set; } = new List<CourseFiles>();

        [ValidateNever]
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    }
}
