using Microsoft.AspNetCore.Mvc;
using Learning_Management_System.Models.ENTITIES;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Learning_Management_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseContext _context;
        private const string AdminPassword = "Admin@123"; // Hardcoded admin password

        public AccountController(DatabaseContext context)
        {
            _context = context;
        }

        // ---------------- LOGIN (GET) ----------------
        public IActionResult Login()
        {
            return View();
        }

        // ---------------- LOGIN (POST) ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string role, string email, string password)
        {
            role = role?.ToLower();

            // ---------- Admin Login ----------
            if (role == "admin")
            {
                if (password == AdminPassword)
                {
                    HttpContext.Session.SetString("Role", "admin");
                    return RedirectToAction("Index", "AdminDashboard");
                }
                else
                {
                    ViewBag.Error = "Invalid admin password";
                    return View();
                }
            }

            // ---------- Student Login ----------
            else if (role == "student")
            {
                var student = _context.Students
                    .FirstOrDefault(s => s.Email.Trim() == email.Trim()
                                      && s.Password.Trim() == password.Trim());

                if (student != null)
                {
                    HttpContext.Session.SetInt32("StudentId", student.StudentId);
                    HttpContext.Session.SetString("Role", "student");
                    HttpContext.Session.SetString("StudentName", student.Name);
                    return RedirectToAction("Index", "StudentDashboard");
                }
                else
                {
                    ViewBag.Error = "Invalid student credentials";
                    return View();
                }
            }

            // ---------- Faculty Login ----------
            else if (role == "faculty")
            {
                var faculty = _context.Faculty
                    .FirstOrDefault(f => f.Email.Trim() == email.Trim()
                                      && f.Password.Trim() == password.Trim());

                if (faculty != null)
                {
                    HttpContext.Session.SetInt32("FacultyId", faculty.FacultyId);
                    HttpContext.Session.SetString("Role", "faculty");
                    HttpContext.Session.SetString("FacultyName", faculty.Name);
                    return RedirectToAction("Index", "FacultyDashboard");
                }
                else
                {
                    ViewBag.Error = "Invalid faculty credentials";
                    return View();
                }
            }

            ViewBag.Error = "Please select a valid role";
            return View();
        }

        // ---------------- CHANGE PASSWORD (GET) ----------------
        public IActionResult ChangePassword()
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role))
                return RedirectToAction("Login");

            return View();
        }

        // ---------------- CHANGE PASSWORD (POST) ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role))
                return RedirectToAction("Login");

            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "New password and confirm password do not match";
                return View();
            }

            // -------- STUDENT --------
            if (role == "student")
            {
                int? studentId = HttpContext.Session.GetInt32("StudentId");
                if (!studentId.HasValue) return RedirectToAction("Login");

                var student = _context.Students.Find(studentId.Value);

                if (student == null || student.Password != currentPassword)
                {
                    ViewBag.Error = "Current password is incorrect";
                    return View();
                }

                student.Password = newPassword;
                _context.SaveChanges();
            }

            // -------- FACULTY -------- //

            else if (role == "faculty")
            {
                int? facultyId = HttpContext.Session.GetInt32("FacultyId");
                if (!facultyId.HasValue) return RedirectToAction("Login");

                var faculty = _context.Faculty.Find(facultyId.Value);

                if (faculty == null || faculty.Password != currentPassword)
                {
                    ViewBag.Error = "Current password is incorrect";
                    return View();
                }

                faculty.Password = newPassword;
                _context.SaveChanges();
            }

            TempData["SuccessMessage"] = "Password changed successfully!";
            return RedirectToAction("Index", role == "student" ? "StudentDashboard" : "FacultyDashboard");
        }

        // ---------------- LOGOUT ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Clear all session data
            HttpContext.Session.Clear();

            // Redirect to Login page
            return RedirectToAction("Login");
        }
    }
}
