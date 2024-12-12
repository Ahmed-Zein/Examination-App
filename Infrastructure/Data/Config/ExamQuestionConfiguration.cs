using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class ExamQuestionConfiguration : IEntityTypeConfiguration<ExamQuestion>
{
    public void Configure(EntityTypeBuilder<ExamQuestion> builder)
    {
        builder.HasKey(e => new { e.ExamId, e.QuestionId });
        builder
            .HasOne(e => e.Exam)
            .WithMany(q => q.ExamQuestions)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Question)
            .WithMany(e => e.ExamQuestions)
            .OnDelete(DeleteBehavior.NoAction);
    }
}