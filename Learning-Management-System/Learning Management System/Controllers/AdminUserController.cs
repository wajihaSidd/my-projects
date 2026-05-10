using Learning_Management_System.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Learning_Management_System.Controllers
{
    public class AdminUserController : Controller
    {
        private readonly DatabaseContext _context;

        public AdminUserController(DatabaseContext context)
        {
            _context = context;
        }

        // =========================
        // INDEX – List Students & Faculty
        // =========================
        public IActionResult Index()
        {
            var students = _context.Students.Include(s => s.Department).ToList();
            var faculty = _context.Faculty.Include(f => f.Department).ToList();
            ViewBag.Students = students;
            ViewBag.Faculty = faculty;
            return View();
        }

        // =========================
        // STUDENTS CRUD
        // =========================
        public IActionResult CreateStudent()
        {
            ViewBag.Departments = new SelectList(_context.Department.ToList(), "DepartmentId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStudent(Students student)
        {
            // 🔥 FORCE CLEAN INSERT
            student.StudentId = 0;
            ModelState.Remove("StudentId");
            ModelState.Remove("Department");

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(
                    _context.Department,
                    "DepartmentId",
                    "Name",
                    student.DepartmentId
                );
                return View(student);
            }

            _context.Entry(student).State = EntityState.Added;
            _context.Students.Add(student);
            _context.SaveChanges();

            TempData["Success"] = "Student added successfully!";
            return RedirectToAction("Index");
        }


        public IActionResult EditStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null) return NotFound();

            ViewBag.Departments = new SelectList(_context.Department.ToList(), "DepartmentId", "Name", student.DepartmentId);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditStudent(Students student)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(_context.Department.ToList(), "DepartmentId", "Name", student.DepartmentId);
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["Error"] = errors;
                return View(student);
            }

            _context.Students.Update(student);
            _context.SaveChanges();
            TempData["Success"] = "Student updated successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
                TempData["Success"] = "Student deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Student not found.";
            }
            return RedirectToAction("Index");
        }

        // =========================
        // FACULTY CRUD
        // =========================
        public IActionResult CreateFaculty()
        {
            ViewBag.Departments = new SelectList(_context.Department.ToList(), "DepartmentId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public IActionResult CreateFaculty(Faculty faculty)
        {
            faculty.FacultyId = 0;
            ModelState.Remove("FacultyId");
            ModelState.Remove("Department");

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(
                    _context.Department,
                    "DepartmentId",
                    "Name",
                    faculty.DepartmentId
                );
                return View(faculty);
            }

            _context.Entry(faculty).State = EntityState.Added;
            _context.Faculty.Add(faculty);
            _context.SaveChanges();

            TempData["Success"] = "Faculty added successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult EditFaculty(int id)
        {
            var faculty = _context.Faculty.Find(id);
            if (faculty == null) return NotFound();

            ViewBag.Departments = new SelectList(_context.Department.ToList(), "DepartmentId", "Name", faculty.DepartmentId);
            return View(faculty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditFaculty(Faculty faculty)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(_context.Department.ToList(), "DepartmentId", "Name", faculty.DepartmentId);
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["Error"] = errors;
                return View(faculty);
            }

            _context.Faculty.Update(faculty);
            _context.SaveChanges();
            TempData["Success"] = "Faculty updated successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult DeleteFaculty(int id)
        {
            var faculty = _context.Faculty.Find(id);
            if (faculty != null)
            {
                _context.Faculty.Remove(faculty);
                _context.SaveChanges();
                TempData["Success"] = "Faculty deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Faculty not found.";
            }
            return RedirectToAction("Index");
        }
    }
}
