using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Learning_Management_System.Models.ENTITIES
{
    public enum CourseFileCategory
    {
        Material = 1,     // PPT, PDF, Lectures
        Assignment = 2    // Assignments
    }

    [Table("CourseFiles")]
    public class CourseFiles
    {
        [Key]
        public int CourseFileId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public int WeekNumber { get; set; }

        [Required]
        public DateTime UploadDate { get; set; } = DateTime.Now;

        // 🔥 NEW: Category (Material / Assignment)
        [Required]
        public CourseFileCategory Category { get; set; }

        [Required]
        [StringLength(300)]
        public string FilePath { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string FileName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string FileType { get; set; } = null!;

        // Assignment-only fields (optional)
        public DateTime? DueDate { get; set; }
        public int? TotalMarks { get; set; }

        [ValidateNever]
        public Courses Course { get; set; } = null!;
    }
}
