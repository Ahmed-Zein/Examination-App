using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasMany<Exam>()
            .WithOne(exam => exam.Subject)
            .HasForeignKey(exam => exam.SubjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany<Question>()
            .WithOne(question => question.Subject)
            .HasForeignKey(question => question.SubjectId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}