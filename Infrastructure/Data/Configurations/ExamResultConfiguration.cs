using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ExamResultConfiguration : IEntityTypeConfiguration<ExamResult>
{
    public void Configure(EntityTypeBuilder<ExamResult> builder)
    {
        builder.HasKey(er => er.Id);

        builder.HasOne<AppUser>()
            .WithMany(usr => usr.ExamResults)
            .HasForeignKey(er => er.AppUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(er => er.Exam)
            .WithMany(e => e.ExamResults)
            .HasForeignKey(er => er.ExamId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}