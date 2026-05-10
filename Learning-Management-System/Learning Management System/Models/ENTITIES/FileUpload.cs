using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Management_System.Models.ENTITIES
{
    public class FileUpload
    {
        [Key]
        public int FileId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public int WeekNumber { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }

        [Required]
        [StringLength(50)]
        public string FileType { get; set; }

        public long FileSize { get; set; }

        [StringLength(50)]
        public string UploadedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
      
        // Navigation property
        [ForeignKey("CourseId")]
        public virtual Courses Courses { get; set; }
   
    }
}