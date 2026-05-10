using Learning_Management_System.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AttendanceController : Controller
{
    private readonly DatabaseContext _context;

    public AttendanceController(DatabaseContext context)
    {
        _context = context;
    }

    // -------------------------
    // 1️⃣ Select Course (for faculty) - SAME
    // -------------------------
    public async Task<IActionResult> SelectCourse()
    {
        int facultyId = HttpContext.Session.GetInt32("FacultyId") ?? 0;

        var courses = await _context.CourseFaculty
            .Where(cf => cf.FacultyId == facultyId)
            .Include(cf => cf.Course)
            .Select(cf => cf.Course)
            .ToListAsync();

        return View(courses);
    }

    // -------------------------
    // 2️⃣ Mark Attendance Page - SAME
    // -------------------------
    public async Task<IActionResult> Mark(int courseId)
    {
        int facultyId = HttpContext.Session.GetInt32("FacultyId") ?? 0;

        bool isAssigned = await _context.CourseFaculty
            .AnyAsync(cf => cf.FacultyId == facultyId && cf.CourseId == courseId);
        if (!isAssigned) return BadRequest("You are not assigned to this course.");

        var students = await _context.CourseStudents
            .Include(cs => cs.Student)
            .Where(cs => cs.CourseId == courseId)
            .Select(cs => cs.Student)
            .ToListAsync();

        var previousAttendance = await _context.Attendance
            .Include(a => a.Student)
            .Where(a => a.CourseId == courseId)
            .OrderByDescending(a => a.Date)
            .ToListAsync();

        ViewBag.CourseTitle = await _context.Courses
            .Where(c => c.CourseId == courseId)
            .Select(c => c.Title)
            .FirstOrDefaultAsync();

        ViewBag.CourseId = courseId;
        ViewBag.FacultyId = facultyId;
        ViewBag.PreviousAttendance = previousAttendance;

        return View("Mark", students);
    }

    // -------------------------
    // 3️⃣ Submit Attendance - SAME
    // -------------------------
    [HttpPost]
    public async Task<IActionResult> SubmitAttendance(
        int CourseId,
        int FacultyId,
        DateTime Date,
        Dictionary<int, string> Statuses)
    {
        bool isAssigned = await _context.CourseFaculty
            .AnyAsync(cf => cf.FacultyId == FacultyId && cf.CourseId == CourseId);
        if (!isAssigned) return BadRequest("You are not assigned to this course.");

        var utcDate = DateTime.SpecifyKind(Date, DateTimeKind.Utc);

        foreach (var studentId in Statuses.Keys)
        {
            var attendance = new Attendance
            {
                StudentId = studentId,
                CourseId = CourseId,
                FacultyId = FacultyId,
                Date = utcDate,
                Status = Statuses[studentId]
            };

            _context.Attendance.Add(attendance);
        }

        await _context.SaveChangesAsync();
        TempData["Message"] = "Attendance submitted successfully!";
        return RedirectToAction("SelectCourse");
    }

    // -------------------------
    // 4️⃣ Faculty: View Attendance by Course - SAME
    // -------------------------
    public async Task<IActionResult> FacultyCourseAttendance(int courseId)
    {
        int facultyId = HttpContext.Session.GetInt32("FacultyId") ?? 0;

        bool isAssigned = await _context.CourseFaculty
            .AnyAsync(cf => cf.FacultyId == facultyId && cf.CourseId == courseId);
        if (!isAssigned) return BadRequest("You are not assigned to this course.");

        var attendanceRecords = await _context.Attendance
            .Include(a => a.Student)
            .Include(a => a.Course)
            .Include(a => a.Faculty)
            .Where(a => a.CourseId == courseId)
            .OrderBy(a => a.Student.Name)
            .ThenBy(a => a.Date)
            .ToListAsync();

        ViewBag.CourseName = await _context.Courses
            .Where(c => c.CourseId == courseId)
            .Select(c => c.Title)
            .FirstOrDefaultAsync();

        var summary = attendanceRecords
            .GroupBy(a => a.Student)
            .Select(g => new
            {
                StudentName = g.Key.Name,
                Present = g.Count(x => x.Status == "Present"),
                Late = g.Count(x => x.Status == "Late"),
                Absent = g.Count(x => x.Status == "Absent")
            })
            .ToList();

        ViewBag.Summary = summary;

        return View(attendanceRecords);
    }

    // -------------------------
    // 5️⃣ Student: View Own Attendance - YEH ACTION UPDATE HUA HAI
    // -------------------------
    public async Task<IActionResult> StudentView(int? courseId)
    {
        int studentId = HttpContext.Session.GetInt32("StudentId") ?? 0;
        if (studentId == 0) return RedirectToAction("Login", "Account");

        var student = await _context.Students.FindAsync(studentId);
        ViewBag.StudentName = student?.Name ?? "Unknown";

        // SINGLE COURSE VIEW
        if (courseId.HasValue)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);
            if (course == null)
            {
                TempData["Error"] = "Course not found!";
                return RedirectToAction("CurrentSemester", "StudentDashboard");
            }

            var attendanceRecords = await _context.Attendance
                .Include(a => a.Student)
                .Include(a => a.Course)
                .Include(a => a.Faculty)
                .Where(a => a.StudentId == studentId && a.CourseId == courseId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            ViewBag.CourseTitle = course.Title;
            ViewBag.TotalPresent = attendanceRecords.Count(a => a.Status == "Present");
            ViewBag.TotalLate = attendanceRecords.Count(a => a.Status == "Late");
            ViewBag.TotalAbsent = attendanceRecords.Count(a => a.Status == "Absent");

            return View(attendanceRecords);
        }
        // ALL COURSES VIEW
        else
        {
            var attendanceRecords = await _context.Attendance
                .Include(a => a.Student)
                .Include(a => a.Course)
                .Include(a => a.Faculty)
                .Where(a => a.StudentId == studentId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            ViewBag.CourseTitle = "All Courses";
            ViewBag.TotalPresent = attendanceRecords.Count(a => a.Status == "Present");
            ViewBag.TotalLate = attendanceRecords.Count(a => a.Status == "Late");
            ViewBag.TotalAbsent = attendanceRecords.Count(a => a.Status == "Absent");

            return View(attendanceRecords);
        }
    }
}