using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasOne(e => e.Question)
            .WithMany(q => q.Answers)
            .OnDelete(DeleteBehavior.Cascade);
    }
}