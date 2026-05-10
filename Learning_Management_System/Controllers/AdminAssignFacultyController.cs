using Learning_Management_System.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AdminAssignFacultyController : Controller
{
    private readonly DatabaseContext _context;

    public AdminAssignFacultyController(DatabaseContext context)
    {
        _context = context;
    }

    // Show Assign Courses Page
    public async Task<IActionResult> Assign()
    {
        ViewBag.Faculties = await _context.Faculty.ToListAsync();
        ViewBag.Courses = await _context.Courses.ToListAsync();

        var assignments = await _context.CourseFaculty
            .Include(cf => cf.Faculty)
            .Include(cf => cf.Course)
            .ToListAsync();

        return View(assignments);
    }

    // Handle POST request to assign courses
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Assign(int facultyId, List<int> courseIds)
    {
        if (courseIds == null || !courseIds.Any())
        {
            TempData["ErrorMessage"] = "No courses selected!";
            return RedirectToAction("Assign");
        }

        var existingCourseIds = await _context.CourseFaculty
            .Where(cf => cf.FacultyId == facultyId)
            .Select(cf => cf.CourseId)
            .ToListAsync();

        var newAssignments = courseIds
            .Where(cId => !existingCourseIds.Contains(cId))
            .Select(cId => new CourseFaculty
            {
                FacultyId = facultyId,
                CourseId = cId
            })
            .ToList();

        if (newAssignments.Any())
        {
            _context.CourseFaculty.AddRange(newAssignments);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Courses assigned successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "Selected courses are already assigned!";
        }

        return RedirectToAction("Assign");
    }

    // Delete assignment
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAssignment(int facultyId, int courseId)
    {
        var assignment = await _context.CourseFaculty
            .FirstOrDefaultAsync(cf => cf.FacultyId == facultyId && cf.CourseId == courseId);

        if (assignment != null)
        {
            _context.CourseFaculty.Remove(assignment);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Assignment deleted successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "Assignment not found!";
        }

        return RedirectToAction("Assign");
    }
}
