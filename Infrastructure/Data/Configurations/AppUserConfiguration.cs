using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(usr => usr.Id);

        builder.Property(usr => usr.FirstName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(usr => usr.LastName)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasMany(usr => usr.ExamResults)
            .WithOne(examResult => examResult.AppUser)
            .HasForeignKey(examResult => examResult.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}