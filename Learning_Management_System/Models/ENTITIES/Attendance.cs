using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Management_System.Models.ENTITIES
{
    [Table("Attendance")]
    public class Attendance
    {
        [Key]
        [Column("AttendanceId")]
        public int AttendanceId { get; set; }

        [Column("Status")]
        public string Status { get; set; } = null!;

        [Column("Date")]
        public DateTime Date { get; set; }

        [Column("Remarks")]
        [StringLength(500)]
        public string? Remarks { get; set; }  // <-- Add this

        // Foreign Keys
        [Column("StudentId")]
        public int StudentId { get; set; }
        public Students Student { get; set; } = null!;

        [Column("FacultyId")]
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; } = null!;

        [Column("CourseId")]
        public int CourseId { get; set; }
        public Courses Course { get; set; } = null!;
    }
}
