using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Learning_Management_System.Models.ENTITIES
{
    // -----------------------
    // 1️⃣ Quiz Table
    // -----------------------
    [Table("Quizzes")]
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = null!;

        [Required]
        public int CourseId { get; set; }

        [ValidateNever]
        public Courses Course { get; set; } = null!;

        [ValidateNever]
        public ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();

        [ValidateNever]
        public ICollection<QuizResult> Results { get; set; } = new List<QuizResult>();
    }

    // -----------------------
    // 2️⃣ QuizQuestion Table
    // -----------------------
    [Table("QuizQuestions")]
    public class QuizQuestion
    {
        [Key]
        public int QuizQuestionId { get; set; }

        [Required]
        public int QuizId { get; set; }

        [Required]
        [StringLength(500)]
        public string QuestionText { get; set; } = null!;

        [ValidateNever]
        public Quiz Quiz { get; set; } = null!;

        [ValidateNever]
        public ICollection<QuizOption> Options { get; set; } = new List<QuizOption>();
    }

    // -----------------------
    // 3️⃣ QuizOption Table
    // -----------------------
    [Table("QuizOptions")]
    public class QuizOption
    {
        [Key]
        public int QuizOptionId { get; set; }

        [Required]
        public int QuizQuestionId { get; set; }

        [Required]
        [StringLength(250)]
        public string OptionText { get; set; } = null!;

        [Required]
        public bool IsCorrect { get; set; } = false;

        [ValidateNever]
        public QuizQuestion Question { get; set; } = null!;
    }

    // -----------------------
    // 4️⃣ QuizResult Table
    // -----------------------
    [Table("QuizResults")]
    public class QuizResult
    {
        [Key]
        public int QuizResultId { get; set; }

        [Required]
        public int QuizId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int ObtainedMarks { get; set; } = 0;

        [ValidateNever]
        public Quiz Quiz { get; set; } = null!;

        [ValidateNever]
        public Students Student { get; set; } = null!;
    }
}
