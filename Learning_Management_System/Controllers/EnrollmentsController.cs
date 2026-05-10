using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Learning_Management_System.Models.ENTITIES;
using Learning_Management_System.Models.ViewModels;
using System.Linq;
using System.Collections.Generic;

namespace Learning_Management_System.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly DatabaseContext _context;

        public EnrollmentsController(DatabaseContext context)
        {
            _context = context;
        }

        // -----------------------------
        // Index
        // -----------------------------
        public IActionResult Index()
        {
            var departments = _context.Department
                .Include(d => d.Courses)
                    .ThenInclude(c => c.CourseStudents)
                        .ThenInclude(cs => cs.Student)
                .Include(d => d.Students)
                .ToList();

            var model = departments.Select(d => new DepartmentCoursesViewModel
            {
                Department = d,
                Courses = d.Courses.Select(c => new CourseInfo
                {
                    Course = c,
                    StudentsCount = c.CourseStudents
                        .Count(cs => cs.Student.DepartmentId == d.DepartmentId)
                }).ToList()
            }).ToList();

            return View(model);
        }

        // -----------------------------
        // Add Course (GET)
        // -----------------------------
        public IActionResult AddCourse(int departmentId)
        {
            var department = _context.Department
                .Include(d => d.Courses)
                .FirstOrDefault(d => d.DepartmentId == departmentId);

            if (department == null) return NotFound();

            var availableCourses = _context.Courses
                .Where(c => c.DepartmentId == null)
                .ToList();

            var model = new AddCourseViewModel
            {
                Department = department,
                AvailableCourses = availableCourses
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AddCourse(int departmentId, int courseId)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);

            if (course != null)
            {
                course.DepartmentId = departmentId;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // -----------------------------
        // Enroll all students
        // -----------------------------
        [HttpPost]
        public IActionResult EnrollAllStudents(int departmentId)
        {
            var department = _context.Department
                .Include(d => d.Students)
                .Include(d => d.Courses)
                .FirstOrDefault(d => d.DepartmentId == departmentId);

            if (department == null) return NotFound();

            foreach (var student in department.Students)
            {
                foreach (var course in department.Courses)
                {
                    bool alreadyEnrolled = _context.CourseStudents
                        .Any(cs => cs.StudentId == student.StudentId && cs.CourseId == course.CourseId);

                    if (!alreadyEnrolled)
                    {
                        _context.CourseStudents.Add(new CourseStudents
                        {
                            StudentId = student.StudentId,
                            CourseId = course.CourseId
                        });
                    }
                }
            }

            _context.SaveChanges();
            TempData["Message"] = "Students enrolled successfully!";
            return RedirectToAction("Index");
        }

        // =====================================================
        // DELETE STUDENTS (GET)
        // =====================================================
        public IActionResult DeleteStudents(int departmentId, int? courseId)
        {
            // Get all courses for dropdown
            var courses = _context.Courses
                .Where(c => c.DepartmentId == departmentId)
                .ToList();

            ViewBag.DepartmentId = departmentId;
            ViewBag.Courses = courses;
            ViewBag.SelectedCourseId = courseId;

            // Fetch students in the department and optional course filter
            var query = _context.CourseStudents
                .Include(cs => cs.Student)
                .Include(cs => cs.Course)
                .Where(cs => cs.Course.DepartmentId == departmentId);

            if (courseId.HasValue)
            {
                query = query.Where(cs => cs.CourseId == courseId.Value);
            }

            var students = query
                .Select(cs => cs.Student)
                .Distinct()
                .ToList();

            return View(students);
        }

        // =====================================================
        // DELETE STUDENTS (POST)
        // =====================================================
        [HttpPost]
        public IActionResult DeleteStudents(int departmentId, int? courseId, List<int> studentIds)
        {
            if (studentIds == null || !studentIds.Any())
            {
                TempData["Message"] = "No students selected.";
                return RedirectToAction("DeleteStudents", new { departmentId, courseId });
            }

            var enrollments = _context.CourseStudents
                .Where(cs => studentIds.Contains(cs.StudentId)
                          && cs.Course.DepartmentId == departmentId);

            if (courseId.HasValue)
            {
                enrollments = enrollments.Where(cs => cs.CourseId == courseId.Value);
            }

            _context.CourseStudents.RemoveRange(enrollments);
            _context.SaveChanges();

            TempData["Message"] = "Selected students removed successfully!";
            return RedirectToAction("DeleteStudents", new { departmentId, courseId });
        }
    }
}
