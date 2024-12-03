using Core.Constants;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Exam> Exams { get; set; }
    public DbSet<ExamResult> ExamResults { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<ExamQuestion> ExamQuestions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ExamQuestion>().HasKey(e => new { e.QuestionId, e.ExamId });
        modelBuilder.Entity<ExamQuestion>().HasOne<Exam>(e => e.Exam).WithMany(e => e.ExamQuestions);
        modelBuilder.Entity<ExamQuestion>().HasOne<Question>(e => e.Question).WithMany(e => e.ExamQuestions);

        List<IdentityRole> roles =
        [
            new() { Id = "1", Name = AuthRolesConstants.Admin, NormalizedName = AuthRolesConstants.Admin.ToUpper() },
            new() { Id = "2", Name = AuthRolesConstants.Student, NormalizedName = AuthRolesConstants.Student.ToUpper() }
        ];

        List<Subject> subjects =
        [
            new() { Id = 1, Name = "Math" },
            new() { Id = 2, Name = "Science" }
        ];

        modelBuilder.Entity<IdentityRole>().HasData(roles);
        modelBuilder.Entity<Subject>().HasData(subjects);
    }
}