using Learning_Management_System.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Learning_Management_System.Controllers
{
    public class CourseOutlineController : Controller
    {
        private readonly DatabaseContext _context;

        public CourseOutlineController(DatabaseContext context)
        {
            _context = context;
        }

        // ==========================
        // STUDENT VIEW (Read-only)
        // ==========================
        [HttpGet]
        public IActionResult StudentView(int courseId)
        {
            var outlines = _context.CourseOutline
                                   .Where(co => co.CourseId == courseId)
                                   .OrderBy(co => co.WeekNumber)
                                   .ToList();

            return View("StudentView", outlines); // Uses separate StudentView.cshtml
        }

        // ==========================
        // FACULTY CRUD
        // ==========================

        // List all outlines for faculty (with Add/Edit/Delete buttons)
        [HttpGet]
        public IActionResult Index(int courseId)
        {
            var outlines = _context.CourseOutline
                                   .Where(co => co.CourseId == courseId)
                                   .OrderBy(co => co.WeekNumber)
                                   .ToList();

            ViewBag.CourseId = courseId;
            ViewBag.IsFaculty = true; // Show Add/Edit/Delete buttons
            return View(outlines);
        }

        // Create new outline (GET)
        [HttpGet]
        public IActionResult Create(int courseId)
        {
            ViewBag.CourseId = courseId; // Required for hidden input in form
            var model = new CourseOutline { CourseId = courseId };
            return View(model);
        }

        // Create new outline (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CourseOutline model)
        {
            if (ModelState.IsValid)
            {
                // ✅ Ab navigation property nahi hai, seedhe save ho jayega
                _context.CourseOutline.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Outline added successfully!";
                return RedirectToAction("Index", new { courseId = model.CourseId });
            }

            ViewBag.CourseId = model.CourseId;
            return View(model);
        }

        // Edit existing outline (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var outline = _context.CourseOutline.Find(id);
            if (outline == null) return NotFound();

            ViewBag.CourseId = outline.CourseId;
            return View(outline);
        }

        //// Edit existing outline (POST)
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(CourseOutline model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.CourseOutline.Update(model);
        //        _context.SaveChanges();

        //        TempData["SuccessMessage"] = "Outline updated successfully!";
        //        return RedirectToAction("Index", new { courseId = model.CourseId });
        //    }

        //    ViewBag.CourseId = model.CourseId;
        //    return View(model);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CourseOutline model)
        {
            if (ModelState.IsValid)
            {
              
                _context.CourseOutline.Update(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Outline updated successfully!";
                return RedirectToAction("Index", new { courseId = model.CourseId });
            }

            ViewBag.CourseId = model.CourseId;
            return View(model);
        }

       
    }
}
