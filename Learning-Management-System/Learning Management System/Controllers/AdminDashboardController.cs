using Learning_Management_System.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Learning_Management_System.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly DatabaseContext _context;

        public AdminDashboardController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin Dashboard
        public async Task<IActionResult> Index()
        {
            // Session check for admin
            var role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                // Admin nahi hai, login page redirect
                return RedirectToAction("Login", "Account");
            }

            // Optional: fetch summary data
            var departmentCount = await _context.Department.CountAsync();
            var courseCount = await _context.Courses.CountAsync();
            var facultyCount = await _context.Faculty.CountAsync();
            var studentCount = await _context.Students.CountAsync();

            ViewBag.DepartmentCount = departmentCount;
            ViewBag.CourseCount = courseCount;
            ViewBag.FacultyCount = facultyCount;
            ViewBag.StudentCount = studentCount;

            return View();
        }
    }
}
