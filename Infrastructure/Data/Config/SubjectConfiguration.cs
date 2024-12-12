using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasMany(e => e.Exams)
            .WithOne(e => e.Subject)
            .OnDelete(DeleteBehavior.Cascade);
    }
}