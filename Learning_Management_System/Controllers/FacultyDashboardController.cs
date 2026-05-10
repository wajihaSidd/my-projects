using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Learning_Management_System.Models.ENTITIES;
using System.Linq;

namespace Learning_Management_System.Controllers
{
    public class FacultyDashboardController : Controller
    {
        private readonly DatabaseContext _context;

        public FacultyDashboardController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int facultyId = HttpContext.Session.GetInt32("FacultyId") ?? 0;
            string role = HttpContext.Session.GetString("Role") ?? "";

            // Role & Session validation
            if (facultyId == 0 || role.ToLower() != "faculty")
            {
                return RedirectToAction("Login", "Account");
            }

            // Get faculty details
            var faculty = _context.Faculty
                .FirstOrDefault(f => f.FacultyId == facultyId);

            ViewBag.Name = faculty?.Name;
            ViewBag.Email = faculty?.Email;

            // Get faculty assigned courses dynamically
            var facultyCourses = _context.CourseFaculty
                .Include(cf => cf.Course)
                .Where(cf => cf.FacultyId == facultyId)
                .Select(cf => cf.Course)
                .ToList();

            ViewBag.FacultyCourses = facultyCourses;

            return View(); // FacultyDashboard.cshtml
        }
    }
}
