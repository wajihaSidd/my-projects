using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Learning_Management_System.Models.ENTITIES;
using System;

namespace Learning_Management_System.Controllers
{
    public class CertificateController : Controller
    {
        private readonly DatabaseContext _context;

        public CertificateController(DatabaseContext context)
        {
            _context = context;
        }

        // Show all students
        public IActionResult Index()
        {
            var students = _context.Students.Include(s => s.Department).ToList();
            return View(students);
        }

        // Show form to select course & certificate type
        public IActionResult GenerateForm(int studentId)
        {
            var student = _context.Students
                .Include(s => s.Department)
                .FirstOrDefault(s => s.StudentId == studentId);

            if (student == null) return NotFound();

            // Get courses only from student's department
            var courses = _context.Courses
                .Where(c => c.DepartmentId == student.DepartmentId)
                .ToList();

            ViewBag.Courses = courses;
            ViewBag.Student = student;

            return View();
        }


        // Generate certificate & save in DB
        [HttpPost]
        public IActionResult Generate(int studentId, int courseId, string type)
        {
            var student = _context.Students.Find(studentId);
            var course = _context.Courses
                .Include(c => c.CourseFaculty)
                .ThenInclude(cf => cf.Faculty)
                .FirstOrDefault(c => c.CourseId == courseId);

            if (student == null || course == null)
                return NotFound();

            // Get first instructor assigned to course
            var instructor = course.CourseFaculty.FirstOrDefault()?.Faculty?.Name ?? "Instructor";

            var certificate = new Certificate
            {
                StudentId = studentId,
                CourseId = courseId,
                CertificateType = type,
                IssueDate = DateTime.UtcNow,
                Student = student,
                Course = course
            };

            _context.Certificates.Add(certificate);
            _context.SaveChanges();

            ViewBag.Instructor = instructor;

            return View("Generate", certificate);
        }
    }
}
