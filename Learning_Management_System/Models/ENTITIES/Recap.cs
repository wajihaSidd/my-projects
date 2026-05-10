using System.Collections.Generic;
using System.Linq;

namespace Learning_Management_System.Models.ENTITIES
{
    public class Recap
    {
        // -----------------------------
        // Student Info (linked via CourseStudents)
        // -----------------------------
        public int StudentId { get; set; }
        public Students Student { get; set; } = null!; // navigation property
        //public string Section => Student.Section;     // dynamically fetched if Section property exists
        public string Program => Student.Department?.Name ?? "N/A"; // Department as program

        // -----------------------------
        // Course Info
        // -----------------------------
        public int CourseId { get; set; }
        public Courses Course { get; set; } = null!;  // navigation property
        public string CourseTitle => Course.Title;
        // public string Semester => Course.Semester;

        // -----------------------------
        // Instructor Info (linked via CourseFaculty collection)
        // -----------------------------
        public List<Faculty> Instructors => Course.CourseFaculty.Select(cf => cf.Faculty).ToList();

        // -----------------------------
        // Link to CourseStudentId (for marks)
        // -----------------------------
        public int CourseStudentId { get; set; }

        // -----------------------------
        // Marks
        // -----------------------------
        public List<MarkItem> Quizzes { get; set; } = new();
        public List<MarkItem> Assignments { get; set; } = new();
        public int? MidMarks { get; set; }
        public int MidMaxMarks { get; set; } = 30;
        public int? FinalMarks { get; set; }
        public int FinalMaxMarks { get; set; } = 30;

        // -----------------------------
        // Total Marks Calculation
        // -----------------------------
        public int TotalMarks =>
            (Quizzes.Sum(q => q.MarksObtained ?? 0) +
             Assignments.Sum(a => a.MarksObtained ?? 0) +
             (MidMarks ?? 0) +
             (FinalMarks ?? 0));

        // -----------------------------
        // Helper
        // -----------------------------
        public bool HasMarks => Quizzes.Any() || Assignments.Any() || MidMarks.HasValue || FinalMarks.HasValue;
    }

    public class MarkItem
    {
        public int Id { get; set; } // Marks table record ID
        public string Title { get; set; } = null!;
        public int MaxMarks { get; set; }
        public int? MarksObtained { get; set; }
    }
}
