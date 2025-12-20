using ITI.Gymunity.FP.Domain.Models.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITI.Gymunity.FP.Infrastructure._Data.Configurations
{
    public class WorkoutLogConfiguration : IEntityTypeConfiguration<WorkoutLog>
    {
        public void Configure(EntityTypeBuilder<WorkoutLog> builder)
        {
            // Table Configuration
            builder.ToTable("WorkoutLogs");

            // Key Configuration - Using long for high volume
            builder.HasKey(wl => wl.Id);

            // Property Configurations
            builder.Property(wl => wl.ClientId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(wl => wl.ProgramDayId)
                .IsRequired();

            builder.Property(wl => wl.CompletedAt)
                .IsRequired();

            builder.Property(wl => wl.Notes)
                .HasMaxLength(2000);

            builder.Property(wl => wl.DurationMinutes);

            builder.Property(wl => wl.ExercisesLoggedJson)
                .IsRequired()
                .HasMaxLength(8000)
                .HasDefaultValue("[]");

            // Soft Delete
            builder.Property(wl => wl.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(wl => wl.UpdatedAt);

            // Indexes
            builder.HasIndex(wl => wl.ClientId);
            builder.HasIndex(wl => wl.ProgramDayId);
            builder.HasIndex(wl => new { wl.ClientId, wl.CompletedAt });
            builder.HasIndex(wl => wl.CompletedAt);

            // Relationships
            builder.HasOne(wl => wl.Client)
                .WithMany(u => u.WorkoutLogs)
                .HasForeignKey(wl => wl.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(wl => wl.ProgramDay)
                .WithMany()
                .HasForeignKey(wl => wl.ProgramDayId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
