using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace StudentEnrollmentSystem.Models
{
    public class StudentEnrollContext : IdentityDbContext<IdentityUser>
    {
        public StudentEnrollContext(DbContextOptions<StudentEnrollContext> options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrollment>()
                .HasKey(e => e.Id);
                //.HasKey(en => new { en.StudentId, en.CourseId });
            modelBuilder.Entity<Enrollment>()
                .HasOne(en => en.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(en => en.StudentId);
            modelBuilder.Entity<Enrollment>()
                .HasOne(en => en.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(en => en.CourseId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Enrollment> Enrollments { get; set; } = null!;
    }
}
