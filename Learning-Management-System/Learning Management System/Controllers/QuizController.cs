using Learning_Management_System.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learning_Management_System.Controllers
{
    public class QuizController : Controller
    {
        private readonly DatabaseContext _context;

        public QuizController(DatabaseContext context)
        {
            _context = context;
        }

        // ---------------------------
        // LIST ALL QUIZZES (Teacher)
        // ---------------------------
        public IActionResult Index(int courseId)
        {
            var quizzes = _context.Quizzes
                                  .Include(q => q.Course)
                                  .Include(q => q.Questions)
                                  .ThenInclude(q => q.Options)
                                  .Where(q => q.CourseId == courseId)
                                  .ToList();

            ViewBag.CourseId = courseId;
            return View(quizzes);
        }

        // ---------------------------
        // CREATE QUIZ
        // ---------------------------
        public IActionResult Create(int courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int courseId, string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                TempData["ErrorMessage"] = "Quiz title is required.";
                return RedirectToAction("Create", new { courseId });
            }

            var quiz = new Quiz
            {
                Title = title,
                CourseId = courseId
            };

            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Quiz created successfully!";
            return RedirectToAction("Index", new { courseId });
        }

        // ---------------------------
        // DELETE QUIZ
        // ---------------------------
        public async Task<IActionResult> Delete(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null) return NotFound();

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Quiz deleted successfully!";
            return RedirectToAction("Index", new { courseId = quiz.CourseId });
        }

        // ---------------------------
        // ADD QUESTIONS TO QUIZ
        // ---------------------------
        public IActionResult AddQuestion(int quizId)
        {
            var quiz = _context.Quizzes
                               .Include(q => q.Course)
                               .Include(q => q.Questions)
                               .ThenInclude(q => q.Options)
                               .FirstOrDefault(q => q.QuizId == quizId);

            if (quiz == null) return NotFound();

            return View(quiz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQuestion(int quizId, string questionText, List<string> options, List<string> isCorrect)
        {
            if (string.IsNullOrEmpty(questionText) || options == null || options.Count == 0)
            {
                TempData["ErrorMessage"] = "Please provide question and options.";
                return RedirectToAction("AddQuestion", new { quizId });
            }

            var question = new QuizQuestion
            {
                QuizId = quizId,
                QuestionText = questionText
            };

            _context.QuizQuestions.Add(question);
            await _context.SaveChangesAsync();

            for (int i = 0; i < options.Count; i++)
            {
                var option = new QuizOption
                {
                    QuizQuestionId = question.QuizQuestionId,
                    OptionText = options[i],
                    IsCorrect = isCorrect != null && isCorrect.Contains(i.ToString())
                };

                _context.QuizOptions.Add(option);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Question added successfully!";
            return RedirectToAction("AddQuestion", new { quizId });
        }

        // ---------------------------
        // VIEW QUIZ RESULTS
        // ---------------------------
        public IActionResult ViewResults(int quizId)
        {
            var results = _context.QuizResults
                                  .Include(r => r.Student)
                                  .Where(r => r.QuizId == quizId)
                                  .ToList();

            var quiz = _context.Quizzes.Find(quizId);
            ViewBag.CourseId = quiz?.CourseId ?? 0; // pass actual courseId
            return View(results);
        }
    }
}
