using Core.Constants;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
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
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.Entity<ExamQuestion>().HasKey(e => new { ExamId = e.ExamId, QuestionId = e.QuestionId });
        modelBuilder.Entity<Question>()
            .HasMany(q => q.ExamQuestions)
            .WithOne(q => q.Question)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Exam>()
            .HasMany(q => q.ExamQuestions)
            .WithOne(q => q.Exam)
            .OnDelete(DeleteBehavior.NoAction);

        // Added to disambiguate the existence of two relations [Many2Many, One2Many] 
        modelBuilder.Entity<Exam>()
            .Ignore(e => e.Questions);

        var roles = new List<IdentityRole>
        {
            new()
            {
                Id = "1",
                Name = AuthRolesConstants.Admin,
                NormalizedName = AuthRolesConstants.Admin.ToUpper()
            },
            new()
            {
                Id = "2",
                Name = AuthRolesConstants.Student,
                NormalizedName = AuthRolesConstants.Student.ToUpper()
            }
        };

        var subjects = new List<Subject>
        {
            new() { Id = 1, Name = "Math" },
            new() { Id = 2, Name = "Science" }
        };

        modelBuilder.Entity<IdentityRole>().HasData(roles);
        modelBuilder.Entity<Subject>().HasData(subjects);
    }
}