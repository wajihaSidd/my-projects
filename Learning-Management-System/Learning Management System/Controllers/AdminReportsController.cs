using Learning_Management_System.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Learning_Management_System.Controllers
{
    public class AdminReportsController : Controller
    {
        private readonly DatabaseContext _context;

        public AdminReportsController(DatabaseContext context)
        {
            _context = context;
        }

        // -------------------------
        // Reports Center Main Page
        // -------------------------
        public async Task<IActionResult> Index()
        {
            // 1️⃣ Students per department
            var studentByDept = await _context.Students
                .Include(s => s.Department)
                .GroupBy(s => s.Department.Name)
                .Select(g => new
                {
                    Department = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            // 2️⃣ Faculty per department
            var facultyByDept = await _context.Faculty
                .Include(f => f.Department)
                .GroupBy(f => f.Department.Name)
                .Select(g => new
                {
                    Department = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            // 3️⃣ Courses assigned per faculty
            var coursesPerFaculty = await _context.CourseFaculty
                .Include(cf => cf.Faculty)
                .GroupBy(cf => cf.Faculty.Name)
                .Select(g => new
                {
                    FacultyName = g.Key,
                    CoursesCount = g.Count()
                })
                .ToListAsync();

            // 4️⃣ Attendance per course
            var attendancePerCourse = await _context.Courses
                .Select(c => new
                {
                    CourseTitle = c.Title,
                    TotalClasses = _context.Attendance.Count(a => a.CourseId == c.CourseId),
                    AverageAttendance = _context.Attendance.Any(a => a.CourseId == c.CourseId)
                        ? (double)_context.Attendance.Count(a => a.CourseId == c.CourseId && a.Status == "Present") / // use your Status values
                          _context.Attendance.Count(a => a.CourseId == c.CourseId) * 100
                        : 0
                })
                .ToListAsync();

            // Pass data to ViewBag
            ViewBag.StudentByDept = studentByDept;
            ViewBag.FacultyByDept = facultyByDept;
            ViewBag.CoursesPerFaculty = coursesPerFaculty;
            ViewBag.AttendancePerCourse = attendancePerCourse;

            return View();
        }
    }
}
