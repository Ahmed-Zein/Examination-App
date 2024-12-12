using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class ExamConfiguration : IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Ignore(e => e.Questions);
        builder.Property(e => e.Duration)
            .IsRequired();

        builder
            .HasMany(q => q.ExamQuestions)
            .WithOne(q => q.Exam);

        builder.HasMany(e => e.ExamResults)
            .WithOne(e => e.Exam)
            .OnDelete(DeleteBehavior.SetNull);
    }
}