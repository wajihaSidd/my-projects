using Learning_Management_System.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Learning_Management_System.Controllers
{
    public class AdminCoursesController : Controller
    {
        private readonly DatabaseContext _context;

        public AdminCoursesController(DatabaseContext context)
        {
            _context = context;
        }

        private bool IsAdmin() => HttpContext.Session.GetString("Role") == "admin";

        // ---------------- INDEX ----------------
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var courses = await _context.Courses
                                        .Include(c => c.Department)
                                        .ToListAsync();
            return View(courses);
        }

        // ---------------- CREATE ----------------
        public async Task<IActionResult> Create()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            ViewBag.DepartmentList = new SelectList(await _context.Department.ToListAsync(), "DepartmentId", "Name");
            return View(new Courses());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,DepartmentId")] Courses model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (model.DepartmentId == 0)
                ModelState.AddModelError("DepartmentId", "Please select a department");

            if (!ModelState.IsValid)
            {
                ViewBag.DepartmentList = new SelectList(await _context.Department.ToListAsync(), "DepartmentId", "Name", model.DepartmentId);
                return View(model);
            }

            try
            {
                _context.Courses.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Course created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var realError = ex.InnerException?.Message ?? ex.Message;

                ModelState.AddModelError("", realError);

                ViewBag.DepartmentList = new SelectList(
                    await _context.Department.ToListAsync(),
                    "DepartmentId",
                    "Name",
                    model.DepartmentId
                );

                return View(model);
            }

        }

        // ---------------- EDIT ----------------
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            ViewBag.DepartmentList = new SelectList(await _context.Department.ToListAsync(), "DepartmentId", "Name", course.DepartmentId);
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,Title,Description,DepartmentId")] Courses model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            if (id != model.CourseId) return BadRequest();

            if (model.DepartmentId == 0)
                ModelState.AddModelError("DepartmentId", "Please select a department");

            if (!ModelState.IsValid)
            {
                ViewBag.DepartmentList = new SelectList(await _context.Department.ToListAsync(), "DepartmentId", "Name", model.DepartmentId);
                return View(model);
            }

            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Course updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = "Error updating course: " + ex.Message;
                ViewBag.DepartmentList = new SelectList(await _context.Department.ToListAsync(), "DepartmentId", "Name", model.DepartmentId);
                return View(model);
            }
        }

        // ---------------- DELETE ----------------
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var course = await _context.Courses
                                       .Include(c => c.Department)
                                       .FirstOrDefaultAsync(c => c.CourseId == id);
            if (course == null) return NotFound();

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Course deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Course not found!";
            }

            return RedirectToAction(nameof(Index));
        }

     
    }
}
