using Learning_Management_System.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learning_Management_System.Controllers
{
    public class CourseFilesController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _env;

        public CourseFilesController(DatabaseContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // -------------------------------
        // VIEW FILES (Students can download)
        // -------------------------------
        public IActionResult ViewFiles(int courseId)
        {
            var files = _context.CourseFiles
                        .Where(f => f.CourseId == courseId)
                        .OrderBy(f => f.WeekNumber)
                        .ToList();

            ViewBag.CourseTitle = _context.Courses
                                  .Where(c => c.CourseId == courseId)
                                  .Select(c => c.Title)
                                  .FirstOrDefault();

            return View(files); // ViewFiles.cshtml
        }

        // -------------------------------
        // UPLOAD FILES FORM (Faculty)
        // -------------------------------
        public IActionResult UploadFiles(int courseId)
        {
            var files = _context.CourseFiles
                        .Where(f => f.CourseId == courseId)
                        .OrderBy(f => f.WeekNumber)
                        .ToList();

            ViewBag.CourseId = courseId;
            ViewBag.CourseTitle = _context.Courses
                                  .Where(c => c.CourseId == courseId)
                                  .Select(c => c.Title)
                                  .FirstOrDefault();

            return View(files); // UploadFiles.cshtml
        }

        // -------------------------------
        // HANDLE FILE UPLOAD
        // -------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFiles(int CourseId, string Title, string? Description, int WeekNumber, CourseFileCategory Category, IFormFile File)
        {
            if (File == null || File.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a file to upload.";
                return RedirectToAction(nameof(UploadFiles), new { courseId = CourseId });
            }

            var uploadDir = Path.Combine(_env.WebRootPath, "uploads", CourseId.ToString());
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            var fileName = Path.GetFileName(File.FileName);
            var filePath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await File.CopyToAsync(stream);
            }

            var courseFile = new CourseFiles
            {
                CourseId = CourseId,
                Title = Title,
                Description = Description,
                WeekNumber = WeekNumber,
                Category = Category,
                FilePath = "/uploads/" + CourseId + "/" + fileName,
                FileName = fileName,
                FileType = Path.GetExtension(fileName).TrimStart('.'),
                UploadDate = DateTime.UtcNow  
            };
            _context.CourseFiles.Add(courseFile);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "File uploaded successfully!";
            return RedirectToAction(nameof(UploadFiles), new { courseId = CourseId });
        }

        // GET: CourseFiles/StudentViewFiles?courseId=2
        public IActionResult StudentViewFiles(int courseId)
        {
            // Course files for this course
            var files = _context.CourseFiles
                        .Where(f => f.CourseId == courseId)
                        .OrderBy(f => f.WeekNumber)
                        .ToList();

            if (!files.Any())
                ViewBag.Message = "No files uploaded for this course yet.";

            return View(files); // StudentViewFiles.cshtml
        }

        // -------------------------------
        // DELETE FILE
        // -------------------------------
        public async Task<IActionResult> DeleteFile(int id)
        {
            var file = await _context.CourseFiles.FindAsync(id);
            if (file == null) return NotFound();

            var physicalPath = Path.Combine(_env.WebRootPath, file.FilePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (System.IO.File.Exists(physicalPath))
                System.IO.File.Delete(physicalPath);

            _context.CourseFiles.Remove(file);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "File deleted successfully!";
            return RedirectToAction(nameof(UploadFiles), new { courseId = file.CourseId });
        }
    }
}
