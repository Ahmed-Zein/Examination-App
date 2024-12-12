using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasMany(e => e.Answers)
            .WithOne(e => e.Question)
            .OnDelete(DeleteBehavior.Cascade);
    }
}