using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ExamConfiguration : IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.ModelName);

        builder.HasOne<Subject>()
            .WithMany(subject => subject.Exams)
            .HasForeignKey(subject => subject.SubjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(exam => exam.ExamResults)
            .WithOne(examResult => examResult.Exam)
            .HasForeignKey(examResult => examResult.ExamId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(exam => exam.ExamQuestions)
            .WithOne(examQuestion => examQuestion.Exam)
            .HasForeignKey(examQuestion => examQuestion.ExamId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}