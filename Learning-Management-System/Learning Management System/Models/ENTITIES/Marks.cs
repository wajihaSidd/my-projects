using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Management_System.Models.ENTITIES
{
    [Table("Marks")]
    public class Marks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MarkId { get; set; }

        // Link to CourseStudent
        [Required]
        public int CourseStudentId { get; set; }
        public CourseStudents CourseStudent { get; set; } = null!; // navigation property

        // Type of marks
        [Required, StringLength(50)]
        public string Type { get; set; } = null!; // Quiz / Assignment / Mid / Final

        // Title of marks
        [Required, StringLength(100)]
        public string Title { get; set; } = null!;

        // Marks values
        [Required]
        public int MaxMarks { get; set; }
        public int? ObtainedMarks { get; set; }
    }
}
