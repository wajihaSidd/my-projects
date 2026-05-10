using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learning_Management_System.Models.ENTITIES
{
    [Table("CourseStudents")]
    public class CourseStudents
    {
        [Key]
        [Column("CourseStudentId")]
        public int CourseStudentId { get; set; }

        [Column("CourseId")]
        public int CourseId { get; set; }
        public Courses Course { get; set; } = null!;

        [Column("StudentId")]
        public int StudentId { get; set; }
        public Students Student { get; set; } = null!;
    }
}