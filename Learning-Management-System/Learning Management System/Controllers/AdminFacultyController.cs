using Learning_Management_System.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AdminFacultyController : Controller
{
    private readonly DatabaseContext _context;

    public AdminFacultyController(DatabaseContext context)
    {
        _context = context;
    }

    // --------------------------
    // LIST ALL FACULTY
    // --------------------------
    public async Task<IActionResult> Index()
    {
        var facultyList = await _context.Faculty.ToListAsync();
        return View(facultyList);
    }

    // --------------------------
    // CREATE (GET)
    // --------------------------
    public IActionResult Create()
    {
        return View();
    }

    // --------------------------
    // CREATE (POST)
    // --------------------------
    [HttpPost]
    public async Task<IActionResult> Create(Faculty model)
    {
        if (!ModelState.IsValid)
            return View(model);

        _context.Faculty.Add(model);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Faculty added successfully!";
        return RedirectToAction("Index");
    }

    // --------------------------
    // EDIT (GET)
    // --------------------------
    public async Task<IActionResult> Edit(int id)
    {
        var data = await _context.Faculty.FindAsync(id);
        if (data == null) return NotFound();

        return View(data);
    }

    // --------------------------
    // EDIT (POST)
    // --------------------------
    [HttpPost]
    public async Task<IActionResult> Edit(Faculty model)
    {
        if (!ModelState.IsValid)
            return View(model);

        _context.Faculty.Update(model);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Faculty updated successfully!";
        return RedirectToAction("Index");
    }

    // --------------------------
    // DELETE
    // --------------------------
    public async Task<IActionResult> Delete(int id)
    {
        var data = await _context.Faculty.FindAsync(id);
        if (data == null)
        {
            TempData["ErrorMessage"] = "Faculty not found!";
            return RedirectToAction("Index");
        }

        _context.Faculty.Remove(data);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Faculty deleted successfully!";
        return RedirectToAction("Index");
    }
}
