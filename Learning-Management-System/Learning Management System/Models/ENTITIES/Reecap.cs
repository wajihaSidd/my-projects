using Learning_Management_System.Models.ENTITIES;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Management_System.Models
{
    public class Reecap
    {
        public int ReecapId { get; set; }

        // Link to CourseStudent table
        public int CourseStudentId { get; set; }
        public CourseStudents CourseStudent { get; set; }

        // Marks columns (nullable to handle unassigned marks)
        public int? Ass1 { get; set; }
        public int? Ass2 { get; set; }
        public int? Quiz1 { get; set; }
        public int? Quiz2 { get; set; }
        public int? Mid { get; set; }
        public int? Final { get; set; }

        // Hardcoded maximum marks (not stored in DB)
        [NotMapped] public int MaxAss1 => 10;
        [NotMapped] public int MaxAss2 => 10;
        [NotMapped] public int MaxQuiz1 => 10;
        [NotMapped] public int MaxQuiz2 => 10;
        [NotMapped] public int MaxMid => 20;
        [NotMapped] public int MaxFinal => 40;

        // Calculated total obtained marks (sum of all assigned marks)
        [NotMapped]
        public int TotalMarks
        {
            get
            {
                return (Ass1 ?? 0) + (Ass2 ?? 0) + (Quiz1 ?? 0) + (Quiz2 ?? 0) + (Mid ?? 0) + (Final ?? 0);
            }
        }

        // Optional: Total maximum marks
        [NotMapped]
        public int TotalMaxMarks
        {
            get
            {
                return MaxAss1 + MaxAss2 + MaxQuiz1 + MaxQuiz2 + MaxMid + MaxFinal;
            }
        }
    }
}
