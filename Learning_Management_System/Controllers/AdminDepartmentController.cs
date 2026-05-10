 using Learning_Management_System.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learning_Management_System.Controllers
{
    public class AdminDepartmentController : Controller
    {
        private readonly DatabaseContext _context;

        public AdminDepartmentController(DatabaseContext context)
        {
            _context = context;
        }

        // ------------------ INDEX ------------------
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Department
                .Include(d => d.Faculty)
                .Include(d => d.Students)
                .Include(d => d.Courses)
                .ToListAsync();

            return View(departments);
        }

        // ------------------ DETAILS ------------------
        public async Task<IActionResult> Details(int id)
        {
            var dept = await _context.Department
                .Include(d => d.Faculty)
                .Include(d => d.Students)
                .Include(d => d.Courses)
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (dept == null) return NotFound();

            return View(dept);
        }

        // ------------------ CREATE ------------------
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill the form correctly.";
                return View(department);
            }

            _context.Department.Add(department);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Department created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ------------------ EDIT ------------------
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Fetch department with its assigned courses
            var dept = await _context.Department
                                     .Include(d => d.Courses)
                                     .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (dept == null)
            {
                TempData["ErrorMessage"] = "Department not found.";
                return RedirectToAction(nameof(Index));
            }

            // Populate all courses for the checkboxes
            ViewBag.AllCourses = await _context.Courses.ToListAsync();

            return View(dept);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Department department)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data submitted.";
                return View(department);
            }

            _context.Department.Update(department);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Department updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ------------------ DELETE ------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var dept = await _context.Department
                .Include(d => d.Faculty)
                .Include(d => d.Students)
                .Include(d => d.Courses)
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (dept == null)
            {
                TempData["ErrorMessage"] = "Department not found.";
                return RedirectToAction(nameof(Index));
            }

            if (dept.Faculty.Any() || dept.Students.Any() || dept.Courses.Any())
            {
                TempData["ErrorMessage"] = "Cannot delete department. It has related faculty, students, or courses.";
                return RedirectToAction(nameof(Index));
            }

            _context.Department.Remove(dept);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Department deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ------------------ ASSIGN COURSES ------------------
        [HttpGet]
        public async Task<IActionResult> AssignCourses(int departmentId)
        {
            var dept = await _context.Department
                .Include(d => d.Courses)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            if (dept == null) return NotFound();

            ViewBag.AllCourses = await _context.Courses.ToListAsync();
            return View(dept);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignCourses(int departmentId, int[] selectedCourseIds)
        {
            var dept = await _context.Department
                .Include(d => d.Courses)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            if (dept == null) return NotFound();

            dept.Courses.Clear();
            foreach (var id in selectedCourseIds)
            {
                var course = await _context.Courses.FindAsync(id);
                if (course != null)
                    dept.Courses.Add(course);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Courses assigned successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
