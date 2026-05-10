using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Learning_Management_System.Models;
using Learning_Management_System.Models.ENTITIES;
using System.Linq;
using System.Collections.Generic;

namespace Learning_Management_System.Controllers
{
    public class RecapController : Controller
    {
        private readonly DatabaseContext _context;

        public RecapController(DatabaseContext context)
        {
            _context = context;
        }

        // -----------------------------
        // STUDENT VIEW: Recap Sheet (read-only)
        // -----------------------------
        [HttpGet]
        public IActionResult StudentView(int courseId)
        {
            int studentId = HttpContext.Session.GetInt32("StudentId") ?? 0;
            if (studentId == 0 || HttpContext.Session.GetString("Role")?.ToLower() != "student")
                return RedirectToAction("Login", "Account");

            var courseStudent = _context.CourseStudents
                .Include(cs => cs.Student)
                    .ThenInclude(s => s.Department)
                .Include(cs => cs.Course)
                .FirstOrDefault(cs => cs.CourseId == courseId && cs.StudentId == studentId);

            if (courseStudent == null)
            {
                TempData["ErrorMessage"] = "You are not enrolled in this course.";
                return RedirectToAction("CurrentSemester", "StudentDashboard");
            }

            var recap = _context.Reecaps
                .FirstOrDefault(r => r.CourseStudentId == courseStudent.CourseStudentId);

            if (recap == null)
            {
                recap = new Reecap
                {
                    CourseStudentId = courseStudent.CourseStudentId,
                    Ass1 = 0,
                    Ass2 = 0,
                    Quiz1 = 0,
                    Quiz2 = 0,
                    Mid = 0,
                    Final = 0
                };
                _context.Reecaps.Add(recap);
                _context.SaveChanges();
            }

            // Return read-only view for students
            return View("StudentView", recap);
        }

        // -----------------------------
        // FACULTY VIEW: All students recap + edit option
        // -----------------------------
        [HttpGet]
        public IActionResult FacultyRecap(int courseId)
        {
            int facultyId = HttpContext.Session.GetInt32("FacultyId") ?? 0;
            var role = HttpContext.Session.GetString("Role")?.ToLower() ?? "";
            if (facultyId == 0 || role != "faculty")
                return RedirectToAction("Login", "Account");

            var courseStudents = _context.CourseStudents
                .Include(cs => cs.Student)
                    .ThenInclude(s => s.Department)
                .Include(cs => cs.Course)
                .Where(cs => cs.CourseId == courseId)
                .ToList();

            var recaps = new List<Reecap>();

            foreach (var cs in courseStudents)
            {
                var recap = _context.Reecaps
                    .FirstOrDefault(r => r.CourseStudentId == cs.CourseStudentId);

                if (recap == null)
                {
                    recap = new Reecap
                    {
                        CourseStudentId = cs.CourseStudentId,
                        Ass1 = 0,
                        Ass2 = 0,
                        Quiz1 = 0,
                        Quiz2 = 0,
                        Mid = 0,
                        Final = 0
                    };
                    _context.Reecaps.Add(recap);
                    _context.SaveChanges();
                }

                recaps.Add(recap);
            }

            return View("FacultyView", recaps);
        }

        // -----------------------------
        // FACULTY EDIT MARKS - GET
        // -----------------------------
        [HttpGet]
        public IActionResult EditMarks(int recapId)
        {
            int facultyId = HttpContext.Session.GetInt32("FacultyId") ?? 0;
            var role = HttpContext.Session.GetString("Role")?.ToLower() ?? "";
            if (facultyId == 0 || role != "faculty")
                return RedirectToAction("Login", "Account");

            var recap = _context.Reecaps
                .Include(r => r.CourseStudent)
                    .ThenInclude(cs => cs.Student)
                .Include(r => r.CourseStudent.Course) // Include Course for safe redirect
                .FirstOrDefault(r => r.ReecapId == recapId);

            if (recap == null)
                return NotFound();

            return View("EditMarks", recap);
        }

        // -----------------------------
        // FACULTY EDIT MARKS - POST
        // -----------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMarks(Reecap model)
        {
            int facultyId = HttpContext.Session.GetInt32("FacultyId") ?? 0;
            var role = HttpContext.Session.GetString("Role")?.ToLower() ?? "";
            if (facultyId == 0 || role != "faculty")
                return RedirectToAction("Login", "Account");

            var recap = _context.Reecaps
                .Include(r => r.CourseStudent)
                    .ThenInclude(cs => cs.Course) // Ensure Course is loaded
                .FirstOrDefault(r => r.ReecapId == model.ReecapId);

            if (recap == null) return NotFound();

            // Update marks
            recap.Ass1 = model.Ass1;
            recap.Ass2 = model.Ass2;
            recap.Quiz1 = model.Quiz1;
            recap.Quiz2 = model.Quiz2;
            recap.Mid = model.Mid;
            recap.Final = model.Final;

            _context.SaveChanges();

            // Popup message
            TempData["SuccessMessage"] = "Marks updated successfully!";

            // Redirect to FacultyRecap safely
            int courseId = recap.CourseStudent?.Course?.CourseId ?? 0;
            if (courseId == 0)
                return RedirectToAction("FacultyRecap"); // fallback

            return RedirectToAction("FacultyRecap", new { courseId });
        }
    }
}
