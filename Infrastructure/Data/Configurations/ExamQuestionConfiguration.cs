using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ExamQuestionConfiguration : IEntityTypeConfiguration<ExamQuestion>
{
    public void Configure(EntityTypeBuilder<ExamQuestion> builder)
    {
        builder.HasKey(eq => new { eq.ExamId, eq.QuestionId });
        builder.HasOne<Exam>()
            .WithMany(exam => exam.ExamQuestions)
            .HasForeignKey(exam => exam.ExamId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Question>()
            .WithMany(question => question.ExamQuestions)
            .HasForeignKey(exam => exam.QuestionId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}