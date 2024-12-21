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
        Database.Migrate();
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

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