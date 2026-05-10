using Learning_Management_System.Models.ENTITIES;
using Learning_Management_System.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Learning_Management_System.Controllers
{
    public class AdminAttendanceController : Controller
    {
        private readonly DatabaseContext _context;

        public AdminAttendanceController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: /AdminAttendance/Index
        public IActionResult Index()
        {
            // Load all departments with courses, students and attendance
            var departments = _context.Department
                .Include(d => d.Courses)
                    .ThenInclude(c => c.CourseStudents)
                        .ThenInclude(cs => cs.Student)
                .Include(d => d.Courses)
                    .ThenInclude(c => c.Attendance)
                        .ThenInclude(a => a.Student)
                .Include(d => d.Courses)
                    .ThenInclude(c => c.Attendance)
                        .ThenInclude(a => a.Faculty)
                .ToList();

            var model = new List<DepartmentCoursesViewModel>();

            foreach (var dept in departments)
            {
                var deptVM = new DepartmentCoursesViewModel
                {
                    Department = dept,
                    Courses = new List<CourseInfo>()
                };

                foreach (var course in dept.Courses)
                {
                    var studentsCount = course.CourseStudents.Count;
                    deptVM.Courses.Add(new CourseInfo
                    {
                        Course = course,
                        StudentsCount = studentsCount
                    });
                }

                model.Add(deptVM);
            }

            return View(model);
        }
    }
}
