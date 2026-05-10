using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Management_System.Models.ENTITIES
{
    [Table("CourseFaculty")]
    public class CourseFaculty
    {
        [Column("CourseId")]
        public int CourseId { get; set; }
        public Courses Course { get; set; } = null!;

        [Column("FacultyId")]
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; } = null!;
    }
}
