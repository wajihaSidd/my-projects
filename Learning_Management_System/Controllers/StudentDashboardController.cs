using Microsoft.AspNetCore.Mvc;
using Learning_Management_System.Models.ENTITIES;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Learning_Management_System.Controllers
{
    public class StudentDashboardController : Controller
    {
        private readonly DatabaseContext _context;

        public StudentDashboardController(DatabaseContext context)
        {
            _context = context;
        }

        // -----------------------------
        // Dashboard
        public IActionResult Index()
        {
            int studentId = HttpContext.Session.GetInt32("StudentId") ?? 0;
            if (studentId == 0) return RedirectToAction("Login", "Account");

            var studentDetails = _context.StudentDetails
                .Include(sd => sd.Student)
                .FirstOrDefault(sd => sd.StudentId == studentId);

            ViewBag.StudentName = studentDetails?.Student?.Name ?? "Student";

            if (studentDetails == null)
            {
                var student = _context.Students.FirstOrDefault(s => s.StudentId == studentId);
                studentDetails = new StudentDetails
                {
                    StudentId = studentId,
                    Student = student ?? new Students(),
                    RegistrationNumber = student?.RollNumber
                };
            }

            return View(studentDetails);
        }

        // -----------------------------
        // GET: Edit Student Details
        // -----------------------------
        public IActionResult EditStudentDetails()
        {
            int studentId = HttpContext.Session.GetInt32("StudentId") ?? 0;
            if (studentId == 0) return RedirectToAction("Login", "Account");

            var studentDetails = _context.StudentDetails
                .Include(sd => sd.Student)
                .FirstOrDefault(sd => sd.StudentId == studentId);

            var student = _context.Students.FirstOrDefault(s => s.StudentId == studentId);

            if (studentDetails == null)
            {
                studentDetails = new StudentDetails
                {
                    StudentId = studentId,
                    Student = student ?? new Students(),
                    RegistrationNumber = student?.RollNumber
                };
            }
            else
            {
                // Ensure Student navigation and RegistrationNumber are populated
                studentDetails.Student ??= student;
                studentDetails.RegistrationNumber ??= student?.RollNumber;
            }

            return View(studentDetails);
        }

        // -----------------------------
        // POST: Save Edited Student Details
        // -----------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditStudentDetails(StudentDetails model)
        {
            int studentId = HttpContext.Session.GetInt32("StudentId") ?? 0;
            if (studentId == 0) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                TempData["PersonalErrorMessage"] = "Please correct the errors and try again.";
                return View(model);
            }

            if (model.DateOfBirth.HasValue)
            {
                model.DateOfBirth = DateTime.SpecifyKind(model.DateOfBirth.Value, DateTimeKind.Utc);
            }

            var existingDetails = _context.StudentDetails
                .Include(sd => sd.Student)
                .FirstOrDefault(sd => sd.StudentId == studentId);

            var student = _context.Students.FirstOrDefault(s => s.StudentId == studentId);

            if (existingDetails == null)
            {
                // Insert new details
                model.StudentId = studentId;
                model.Student = student;
                model.RegistrationNumber = student?.RollNumber;
                model.Email = student?.Email;

                _context.StudentDetails.Add(model);
            }
            else
            {
                // Update only editable fields
                existingDetails.FirstName = model.FirstName;
                existingDetails.MiddleName = model.MiddleName;
                existingDetails.LastName = model.LastName;
                existingDetails.FathersName = model.FathersName;
                existingDetails.DateOfBirth = model.DateOfBirth;
                existingDetails.NIC = model.NIC;
                existingDetails.Gender = model.Gender;
                existingDetails.BloodGroup = model.BloodGroup;
                existingDetails.Religion = model.Religion;
                existingDetails.HasDomicile = model.HasDomicile;
                existingDetails.FathersCNIC = model.FathersCNIC;
                existingDetails.FathersOccupation = model.FathersOccupation;
                existingDetails.FathersOrganization = model.FathersOrganization;
                existingDetails.OrganizationPhone1 = model.OrganizationPhone1;
                existingDetails.FathersEmail = model.FathersEmail;

                // Ensure RegistrationNumber and Student navigation are intact
                existingDetails.RegistrationNumber ??= student?.RollNumber;
                existingDetails.Student ??= student;
                existingDetails.Email = student?.Email;
                existingDetails.Student ??= student;

            }

            _context.SaveChanges();
            TempData["PersonalSuccessMessage"] = "Your personal information has been saved successfully!";
            return RedirectToAction("Index");
        }
       // -----------------------------
       // GET: Academic Record Dashboard
       // -----------------------------
public IActionResult AcademicRecord()
        {
            int studentId = HttpContext.Session.GetInt32("StudentId") ?? 0;
            if (studentId == 0)
            {
                TempData["ErrorMessage"] = "Student not logged in!";
                return RedirectToAction("Login", "Account");
            }

            var academicRecords = _context.AcademicRecord
                .Where(ar => ar.StudentId == studentId)
                .OrderByDescending(ar => ar.EndDate)
                .ToList();

            // Get student details for the profile section
            var studentDetails = _context.StudentDetails
                .FirstOrDefault(sd => sd.StudentId == studentId);

            // Get student name for the welcome message
            var student = _context.Students
                .FirstOrDefault(s => s.StudentId == studentId);

            var studentName = student?.Name;
            HttpContext.Session.SetString("StudentName", studentName);

            // Pass data to view using ViewBag
            ViewBag.StudentDetails = studentDetails;
            ViewBag.ProfilePicture = studentDetails?.ProfilePicture;

            return View("AcademicRecord", academicRecords);
        }

        // -----------------------------
        // GET: Add/Edit Academic Record
        // -----------------------------
        // -----------------------------
        // GET: Add/Edit Academic Record
        // -----------------------------
        public IActionResult EditAcademicRecord(int? id)
        {
            int studentId = HttpContext.Session.GetInt32("StudentId") ?? 0;
            if (studentId == 0) return RedirectToAction("Login", "Account");

            AcademicRecord record;

            if (id.HasValue)
            {
                // Edit existing record
                record = _context.AcademicRecord
                    .FirstOrDefault(ar => ar.AcademicRecordId == id.Value && ar.StudentId == studentId);

                if (record == null)
                {
                    TempData["AcademicErrorMessage"] = "Record not found!";
                    return RedirectToAction("AcademicRecord");
                }
            }
            else
            {
                // New record
                record = new AcademicRecord
                {
                    StudentId = studentId
                };
            }
            // Get student details for the profile section
            var studentDetails = _context.StudentDetails
                .FirstOrDefault(sd => sd.StudentId == studentId);

            // Get student name for the welcome message
            var student = _context.Students
                .FirstOrDefault(s => s.StudentId == studentId);

            var studentName = student?.Name;
            HttpContext.Session.SetString("StudentName", studentName);

            // Pass data to view using ViewBag
            ViewBag.StudentDetails = studentDetails;
            ViewBag.ProfilePicture = studentDetails?.ProfilePicture;

            return View(record);
        }

        // -----------------------------
        // POST: Save Academic Record
        // -----------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAcademicRecord(AcademicRecord model)
        {
            int studentId = HttpContext.Session.GetInt32("StudentId") ?? 0;
            if (studentId == 0) return RedirectToAction("Login", "Account");

            // Ensure UTC for DateTimes
            if (model.StartDate.HasValue)
                model.StartDate = DateTime.SpecifyKind(model.StartDate.Value, DateTimeKind.Utc);
            if (model.EndDate.HasValue)
                model.EndDate = DateTime.SpecifyKind(model.EndDate.Value, DateTimeKind.Utc);

            // Check for duplicates: same EducationLevel, Institution, Start & End Date
            bool isDuplicate = _context.AcademicRecord
                .Any(ar => ar.StudentId == studentId
                        && ar.EducationLevel == model.EducationLevel
                        && ar.Institution == model.Institution
                        && ar.StartDate == model.StartDate
                        && ar.EndDate == model.EndDate
                        && ar.AcademicRecordId != model.AcademicRecordId); // ignore current record if editing

            if (isDuplicate)
            {
                TempData["AcademicErrorMessage"] = "This academic record already exists!";
                return RedirectToAction("AcademicRecord");
            }

            if (!ModelState.IsValid)
            {
                TempData["AcademicErrorMessage"] = "Please correct the errors and try again.";
                return View(model);
            }

            model.StudentId = studentId;

            if (model.AcademicRecordId == 0)
            {
                // Insert new record
                _context.AcademicRecord.Add(model);
            }
            else
            {
                // Update existing record
                var existing = _context.AcademicRecord
                    .FirstOrDefault(ar => ar.AcademicRecordId == model.AcademicRecordId && ar.StudentId == studentId);

                if (existing == null)
                {
                    TempData["AcademicErrorMessage"] = "Record not found!";
                    return RedirectToAction("AcademicRecord");
                }

                existing.EducationLevel = model.EducationLevel;
                existing.Institution = model.Institution;
                existing.BoardOrUniversity = model.BoardOrUniversity;
                existing.MajorSubjects = model.MajorSubjects;
                existing.Specialization = model.Specialization;
                existing.StudyMode = model.StudyMode;
                existing.DivisionOrGrade = model.DivisionOrGrade;
                existing.Percentage = model.Percentage;
                existing.CGPA = model.CGPA;
                existing.Location = model.Location;
                existing.StartDate = model.StartDate;
                existing.EndDate = model.EndDate;
            }

            _context.SaveChanges();
            TempData["AcademicSuccessMessage"] = "Academic record saved successfully!";
            return RedirectToAction("AcademicRecord");
        }

        // -----------------------------
        // POST: Upload Profile Picture (Updated for all pages)
        // -----------------------------
        [HttpPost]
        public async Task<IActionResult> UploadPicture(IFormFile picture)
        {
            // Logged-in student id from session
            int studentId = HttpContext.Session.GetInt32("StudentId") ?? 0;
            if (studentId == 0)
            {
                TempData["ErrorMessage"] = "Please login first!";
                return RedirectToAction("Login", "Account");
            }

            if (picture == null || picture.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a valid picture!";
                return RedirectToAction("Index");
            }

            // Get existing StudentDetails
            var studentDetails = await _context.StudentDetails
                .FirstOrDefaultAsync(sd => sd.StudentId == studentId);

            if (studentDetails == null)
            {
                // Record exist nahi karta → create new with only required defaults
                studentDetails = new StudentDetails
                {
                    StudentId = studentId,
                    FirstName = "N/A",
                    LastName = "N/A",
                    FathersName = "N/A"
                };
                _context.StudentDetails.Add(studentDetails);
            }

            // Convert file to byte array
            using (var ms = new MemoryStream())
            {
                await picture.CopyToAsync(ms);
                studentDetails.ProfilePicture = ms.ToArray();
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Profile picture uploaded successfully!";

            // Check where the request came from to redirect appropriately
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                // If coming from AcademicRecord page, redirect there
                if (referer.Contains("AcademicRecord", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("AcademicRecord");
                }
            }

            // Default redirect to Index
            return RedirectToAction("Index");
        }


        //// POST: Delete Academic Record
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteAcademicRecord(int id)
        //{
        //    int studentId = HttpContext.Session.GetInt32("StudentId") ?? 0;
        //    if (studentId == 0) return RedirectToAction("Login", "Account");

        //    var record = _context.AcademicRecord
        //        .FirstOrDefault(ar => ar.AcademicRecordId == id && ar.StudentId == studentId);

        //    if (record == null)
        //    {
        //        TempData["ErrorMessage"] = "Record not found!";
        //        return RedirectToAction("AcademicRecord");
        //    }

        //    _context.AcademicRecord.Remove(record);
        //    _context.SaveChanges();

        //    TempData["SuccessMessage"] = "Academic record deleted successfully!";
        //    return RedirectToAction("AcademicRecord");
        //}


        // -----------------------------
        // Current semester courses
        public IActionResult CurrentSemester()
        {
            int studentId = HttpContext.Session.GetInt32("StudentId") ?? 0;
            if (studentId == 0) return RedirectToAction("Index");

            var enrolledCourses = _context.CourseStudents
                                    .Where(cs => cs.StudentId == studentId)
                                    .Include(cs => cs.Course)
                                        .ThenInclude(c => c.CourseFaculty)
                                            .ThenInclude(cf => cf.Faculty)
                                    .Select(cs => cs.Course)
                                    .ToList();

            return View(enrolledCourses);
        }

        // -----------------------------
        // Offered courses page
        public IActionResult OfferedCourses()
        {
            int studentId = HttpContext.Session.GetInt32("StudentId") ?? 0;
            if (studentId == 0) return RedirectToAction("Index");

            var student = _context.Students
                            .Include(s => s.Department)
                            .FirstOrDefault(s => s.StudentId == studentId);

            if (student == null)
            {
                TempData["ErrorMessage"] = "Student not found!";
                return RedirectToAction("Index");
            }

            int deptId = student.DepartmentId;

            var enrolledIds = _context.CourseStudents
                                .Where(cs => cs.StudentId == studentId)
                                .Select(cs => cs.CourseId)
                                .ToList();

            var availableCourses = _context.Courses
                                    .Where(c => c.DepartmentId == deptId
                                             && !enrolledIds.Contains(c.CourseId))
                                    .Include(c => c.CourseFaculty)
                                        .ThenInclude(cf => cf.Faculty)
                                    .ToList();

            ViewBag.DepartmentName = student.Department.Name;
            ViewBag.RegisteredCourses = _context.CourseStudents
                                                .Where(sc => sc.StudentId == studentId)
                                                .Select(sc => sc.Course)
                                                .ToList();

            return View(availableCourses);
        }

        [HttpPost]
        public IActionResult RegisterCourse(int courseId)
        {
            int studentId = HttpContext.Session.GetInt32("StudentId") ?? 0;
            if (studentId == 0) return RedirectToAction("Index");

            var alreadyRegistered = _context.CourseStudents
                                    .Any(cs => cs.CourseId == courseId && cs.StudentId == studentId);

            if (alreadyRegistered)
            {
                TempData["ErrorMessage"] = "You are already registered for this course!";
                return RedirectToAction("OfferedCourses");
            }

            _context.CourseStudents.Add(new CourseStudents
            {
                CourseId = courseId,
                StudentId = studentId
            });
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Course registered successfully!";
            return RedirectToAction("OfferedCourses");
        }
    }
}
