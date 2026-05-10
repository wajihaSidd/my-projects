using Learning_Management_System.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learning_Management_System.Controllers
{
    public class QuizAttemptController : Controller
    {
        private readonly DatabaseContext _context;

        public QuizAttemptController(DatabaseContext context)
        {
            _context = context;
        }

        // -------------------------------
        // List quizzes (optionally by course)
        // -------------------------------
        public IActionResult Index(int? courseId)
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                TempData["ErrorMessage"] = "Session expired. Please login again.";
                return RedirectToAction("Login", "Account");
            }

            // Load quizzes with questions
            var quizzes = _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(qt => qt.Options)
                .AsQueryable();

            if (courseId.HasValue)
                quizzes = quizzes.Where(q => q.CourseId == courseId.Value);

            // Attempted quizzes with marks (handle duplicates)
            var results = _context.QuizResults
                .Where(r => r.StudentId == studentId.Value)
                .GroupBy(r => r.QuizId)
                .Select(g => g.OrderByDescending(r => r.QuizResultId).First()) // latest attempt
                .ToDictionary(r => r.QuizId, r => r.ObtainedMarks);

            ViewBag.Results = results;

            return View(quizzes.ToList());
        }

        // -------------------------------
        // Show quiz for attempt
        // -------------------------------
        public IActionResult Attempt(int quizId)
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                TempData["ErrorMessage"] = "Session expired. Please login again.";
                return RedirectToAction("Login", "Account");
            }

            // Block re-attempt
            bool alreadyAttempted = _context.QuizResults
                .Any(r => r.QuizId == quizId && r.StudentId == studentId.Value);

            if (alreadyAttempted)
            {
                TempData["ErrorMessage"] = "You have already attempted this quiz.";
                return RedirectToAction("Index");
            }

            var quiz = _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(qt => qt.Options)
                .FirstOrDefault(q => q.QuizId == quizId);

            if (quiz == null) return NotFound();

            return View(quiz);
        }

        // -------------------------------
        // Submit quiz answers
        // -------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Attempt(int quizId, Dictionary<int, int> answers)
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                TempData["ErrorMessage"] = "Session expired. Please login again.";
                return RedirectToAction("Login", "Account");
            }

            // Prevent re-attempt
            bool alreadyAttempted = _context.QuizResults
                .Any(r => r.QuizId == quizId && r.StudentId == studentId.Value);

            if (alreadyAttempted)
            {
                TempData["ErrorMessage"] = "You have already attempted this quiz.";
                return RedirectToAction("Index");
            }

            int totalMarks = 0;

            foreach (var kvp in answers)
            {
                int questionId = kvp.Key;
                int selectedOptionId = kvp.Value;

                var option = _context.QuizOptions.Find(selectedOptionId);
                if (option != null && option.IsCorrect)
                {
                    totalMarks++;
                }
            }

            var result = new QuizResult
            {
                QuizId = quizId,
                StudentId = studentId.Value,
                ObtainedMarks = totalMarks
            };

            _context.QuizResults.Add(result);
            _context.SaveChanges();

            TempData["SuccessMessage"] = $"You scored {totalMarks} marks!";
            return RedirectToAction("Result", new { quizId = quizId });
        }

        // -------------------------------
        // View quiz result
        // -------------------------------
        public IActionResult Result(int quizId)
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                TempData["ErrorMessage"] = "Session expired. Please login again.";
                return RedirectToAction("Login", "Account");
            }

            var result = _context.QuizResults
                .Include(r => r.Quiz)
                .FirstOrDefault(r => r.QuizId == quizId && r.StudentId == studentId.Value);

            if (result == null)
            {
                TempData["ErrorMessage"] = "Result not found.";
                return RedirectToAction("Index");
            }

            return View(result);
        }
    }
}
