using Microsoft.EntityFrameworkCore;
using NotificationManagement.Domain.Entities;

namespace NotificationManagement.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Notification> Notifications => Set<Notification>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── User ─────────────────────────────────────────────────────────────
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.RegistrationDate);
            });

            // ── Notification ──────────────────────────────────────────────────────
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.Id);

                entity.Property(n => n.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(n => n.Content)
                      .IsRequired();

                entity.Property(n => n.Channel)
                      .IsRequired();
                 //   .HasConversion<string>();   // stores "Email" not 1

                entity.Property(n => n.Status)
                      .IsRequired();
                 //   .HasConversion<string>();   // stores "Sent" not 2

                entity.Property(n => n.NotificationId)
                      .IsRequired();

                entity.Property(n => n.FailureReason)
                      .IsRequired(false);

                entity.Property(n => n.Recipient)
                      .IsRequired(false);

                // one user has many notifications
                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // index for fast per-user queries
                entity.HasIndex(n => n.UserId);
            });
        }
    }
}
