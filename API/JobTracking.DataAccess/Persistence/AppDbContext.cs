using JobTracking.DataAccess.Data.Models;
using JobTracking.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace JobTracking.DataAccess.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<JobListing> JobListings { get; set; }
        public DbSet<Application> Applications { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Replace with your actual connection string
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=JobTrackingLocalDb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");
            }
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Username).IsUnique(); // Ensure unique usernames
                entity.Property(u => u.Role)
                      .HasConversion<string>(); // Store enum as string
            });

            // Configure JobListing entity
            modelBuilder.Entity<JobListing>(entity =>
            {
                entity.Property(j => j.Status)
                      .HasConversion<string>(); // Store enum as string
            });

            // Configure Application entity
            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasOne(a => a.JobListing)
                      .WithMany(jl => jl.Applications)
                      .HasForeignKey(a => a.JobListingId);

                entity.HasOne(a => a.User)
                      .WithMany(u => u.Applications)
                      .HasForeignKey(a => a.UserId);

                entity.Property(a => a.Status)
                      .HasConversion<string>(); // Store enum as string

                // Ensure a user can apply only once for a specific job listing
                entity.HasIndex(a => new { a.JobListingId, a.UserId }).IsUnique();
            });

            // Seed initial data (for demonstration purposes)
            // Hashed password for "admin" and "user" is "password123"
            // You should use a stronger hashing mechanism in a production environment
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "System",
                    MiddleName = "",
                    LastName = "Admin",
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), // Using BCrypt for hashing
                    Role = UserRole.Admin
                },
                new User
                {
                    Id = 2,
                    FirstName = "Regular",
                    MiddleName = "",
                    LastName = "User",
                    Username = "user",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    Role = UserRole.User
                }
            );

            modelBuilder.Entity<JobListing>().HasData(
                new JobListing
                {
                    Id = 1,
                    Title = "Senior Software Engineer",
                    Company = "Tech Solutions Inc.",
                    Description = "Developing scalable web applications using .NET Core and Angular.",
                    PublishDate = DateTime.UtcNow.AddDays(-7),
                    Status = JobStatus.Active
                },
                new JobListing
                {
                    Id = 2,
                    Title = "Junior UI/UX Designer",
                    Company = "Creative Agency",
                    Description = "Designing intuitive user interfaces and user experiences.",
                    PublishDate = DateTime.UtcNow.AddDays(-5),
                    Status = JobStatus.Active
                },
                new JobListing
                {
                    Id = 3,
                    Title = "Product Manager",
                    Company = "Innovate Corp.",
                    Description = "Leading product development from concept to launch.",
                    PublishDate = DateTime.UtcNow.AddDays(-10),
                    Status = JobStatus.Inactive // Example of an inactive job
                }
            );
        }
    }
}