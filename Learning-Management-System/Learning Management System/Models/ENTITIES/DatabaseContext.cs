using Microsoft.EntityFrameworkCore;

namespace Learning_Management_System.Models.ENTITIES
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }

        public DbSet<Students> Students { get; set; }
        public DbSet<Faculty> Faculty { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<CourseStudents> CourseStudents { get; set; }
        public DbSet<CourseFaculty> CourseFaculty { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<SystemSettings> SystemSettings { get; set; }
        public DbSet<Marks> Marks { get; set; } = null!;

        public DbSet<Reecap> Reecaps { get; set; }
        public DbSet<StudentDetails> StudentDetails { get; set; }
        public DbSet<AcademicRecord> AcademicRecord { get; set; }
        public DbSet<CourseOutline> CourseOutline { get; set; }

        // ✅ CourseFiles
        public DbSet<CourseFiles> CourseFiles { get; set; }

        // ✅ Quiz Module
        public DbSet<Quiz> Quizzes { get; set; } = null!;
        public DbSet<QuizQuestion> QuizQuestions { get; set; } = null!;
        public DbSet<QuizOption> QuizOptions { get; set; } = null!;
        public DbSet<QuizResult> QuizResults { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Department
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");
                entity.HasKey(d => d.DepartmentId);
            });

            // Students
            modelBuilder.Entity<Students>(entity =>
            {
                entity.ToTable("Students");
                entity.HasKey(s => s.StudentId);

                entity.HasOne(s => s.Department)
                      .WithMany(d => d.Students)
                      .HasForeignKey(s => s.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);

                // QuizResults relationship
                entity.HasMany(s => s.QuizResults)
                      .WithOne(r => r.Student)
                      .HasForeignKey(r => r.StudentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Faculty
            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.ToTable("Faculty");
                entity.HasKey(f => f.FacultyId);

                entity.HasOne(f => f.Department)
                      .WithMany(d => d.Faculty)
                      .HasForeignKey(f => f.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Courses
            modelBuilder.Entity<Courses>(entity =>
            {
                entity.ToTable("Courses");
                entity.HasKey(c => c.CourseId);

                entity.HasOne(c => c.Department)
                      .WithMany(d => d.Courses)
                      .HasForeignKey(c => c.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);

                // CourseFiles
                entity.HasMany(c => c.CourseFiles)
                      .WithOne(cf => cf.Course)
                      .HasForeignKey(cf => cf.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Quizzes
                entity.HasMany(c => c.Quizzes)
                      .WithOne(q => q.Course)
                      .HasForeignKey(q => q.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // CourseStudents
            modelBuilder.Entity<CourseStudents>(entity =>
            {
                entity.ToTable("CourseStudents");
                entity.HasKey(cs => cs.CourseStudentId);

                entity.HasOne(cs => cs.Student)
                      .WithMany(s => s.CourseStudents)
                      .HasForeignKey(cs => cs.StudentId);

                entity.HasOne(cs => cs.Course)
                      .WithMany(c => c.CourseStudents)
                      .HasForeignKey(cs => cs.CourseId);
            });

            // CourseFaculty
            modelBuilder.Entity<CourseFaculty>(entity =>
            {
                entity.ToTable("CourseFaculty");

                entity.HasKey(cf => new { cf.CourseId, cf.FacultyId });

                entity.HasOne(cf => cf.Course)
                      .WithMany(c => c.CourseFaculty)
                      .HasForeignKey(cf => cf.CourseId);

                entity.HasOne(cf => cf.Faculty)
                      .WithMany(f => f.CourseFaculty)
                      .HasForeignKey(cf => cf.FacultyId);
            });

            // Attendance
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.ToTable("Attendance");
                entity.HasKey(a => a.AttendanceId);

                entity.HasOne(a => a.Course)
                      .WithMany(c => c.Attendance)
                      .HasForeignKey(a => a.CourseId);

                entity.HasOne(a => a.Student)
                      .WithMany(s => s.Attendances)
                      .HasForeignKey(a => a.StudentId);

                entity.HasOne(a => a.Faculty)
                      .WithMany(f => f.Attendances)
                      .HasForeignKey(a => a.FacultyId);
            });

            // CourseOutline
            modelBuilder.Entity<CourseOutline>(entity =>
            {
                entity.ToTable("CourseOutline");
                entity.HasKey(e => e.CourseOutlineId);
            });

            // -----------------------
            // Quiz Module Relationships
            // -----------------------
            modelBuilder.Entity<QuizQuestion>()
                .HasOne(q => q.Quiz)
                .WithMany(qz => qz.Questions)
                .HasForeignKey(q => q.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuizOption>()
                .HasOne(o => o.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(o => o.QuizQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuizResult>()
                .HasOne(r => r.Quiz)
                .WithMany(qz => qz.Results)
                .HasForeignKey(r => r.QuizId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
