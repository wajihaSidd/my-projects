using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Management_System.Models.ENTITIES
{
    [Table("AcademicRecords")]
    public class AcademicRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AcademicRecordId { get; set; }

        // Foreign key to Student
        [Required]
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Students? Student { get; set; }

        [Required, StringLength(150)]
        public string EducationLevel { get; set; } = null!; // e.g., Matric, Intermediate, Bachelors

        [Required, StringLength(200)]
        public string Institution { get; set; } = null!;

        [StringLength(100)]
        public string BoardOrUniversity { get; set; } = null!;

        [StringLength(200)]
        public string MajorSubjects { get; set; } = null!;

        [StringLength(100)]
        public string Specialization { get; set; } = null!;

        [StringLength(50)]
        public string StudyMode { get; set; } = null!;

        [StringLength(50)]
        public string DivisionOrGrade { get; set; } = null!;

        public decimal? Percentage { get; set; }
        public decimal? CGPA { get; set; }

        [StringLength(200)]
        public string Location { get; set; } = null!;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [NotMapped]
        public int DurationMonths => StartDate.HasValue && EndDate.HasValue
                                     ? ((EndDate.Value.Year - StartDate.Value.Year) * 12) + EndDate.Value.Month - StartDate.Value.Month
                                     : 0;
    }
}
