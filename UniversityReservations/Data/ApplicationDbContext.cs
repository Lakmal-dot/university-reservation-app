using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UniversityReservations.Models;

namespace UniversityReservations.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<LectureHall> LectureHalls { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            modelBuilder.Entity<Lecturer>()
                .HasOne(l => l.Subject)
                .WithMany(s => s.Lecturers)
                .HasForeignKey(l => l.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Lecturer)
                .WithMany(l => l.Reservations)
                .HasForeignKey(r => r.LecturerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.LectureHall)
                .WithMany(lh => lh.Reservations)
                .HasForeignKey(r => r.LectureHallId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add unique constraints for PostgreSQL
            modelBuilder.Entity<Reservation>()
                .HasIndex(r => new { r.LectureHallId, r.StartTime, r.EndTime })
                .IsUnique();

            // Seed data with static values and explicit UTC DateTime
            modelBuilder.Entity<Subject>().HasData(
                new Subject { Id = 1, Name = "Mathematics", Description = "Advanced mathematics course" },
                new Subject { Id = 2, Name = "Physics", Description = "Fundamental physics principles" },
                new Subject { Id = 3, Name = "Computer Science", Description = "Programming and algorithms" }
            );

            modelBuilder.Entity<Lecturer>().HasData(
                new Lecturer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@university.edu", SubjectId = 1 },
                new Lecturer { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@university.edu", SubjectId = 2 },
                new Lecturer { Id = 3, FirstName = "Robert", LastName = "Johnson", Email = "robert.johnson@university.edu", SubjectId = 3 }
            );

            modelBuilder.Entity<LectureHall>().HasData(
                new LectureHall { Id = 1, Name = "Hall A", Capacity = 100, HasProjector = true, HasMicrophone = true, Location = "Building 1, Floor 2" },
                new LectureHall { Id = 2, Name = "Hall B", Capacity = 50, HasProjector = false, HasMicrophone = true, Location = "Building 1, Floor 1" },
                new LectureHall { Id = 3, Name = "Auditorium", Capacity = 300, HasProjector = true, HasMicrophone = true, Location = "Building 2, Floor 1" }
            );

            // Use static DateTime values with DateTimeKind.Unspecified for seed data
            var fixedDate1 = new DateTime(2024, 1, 15, 10, 0, 0, DateTimeKind.Unspecified);
            var fixedDate2 = new DateTime(2024, 1, 16, 14, 0, 0, DateTimeKind.Unspecified);
            var fixedDate3 = new DateTime(2024, 1, 17, 9, 0, 0, DateTimeKind.Unspecified);

            modelBuilder.Entity<Reservation>().HasData(
                new Reservation
                {
                    Id = 1,
                    LecturerId = 1,
                    LectureHallId = 1,
                    StartTime = fixedDate1,
                    EndTime = fixedDate1.AddHours(2),
                    Purpose = "Calculus Lecture"
                },
                new Reservation
                {
                    Id = 2,
                    LecturerId = 2,
                    LectureHallId = 2,
                    StartTime = fixedDate2,
                    EndTime = fixedDate2.AddHours(2),
                    Purpose = "Quantum Mechanics"
                },
                new Reservation
                {
                    Id = 3,
                    LecturerId = 3,
                    LectureHallId = 3,
                    StartTime = fixedDate3,
                    EndTime = fixedDate3.AddHours(3),
                    Purpose = "Data Structures and Algorithms"
                }
            );
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>()
                .HaveColumnType("timestamp without time zone");
        }

        public class DateTimeToUtcConverter : ValueConverter<DateTime, DateTime>
        {
            public DateTimeToUtcConverter() : base(
                v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
            { }
        }
    }
}
