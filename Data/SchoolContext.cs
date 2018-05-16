using entityframeworkcore.Models;
using Microsoft.EntityFrameworkCore;

namespace entityframeworkcore.Data
{
    public class SchoolContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
        }

        //https://www.learnentityframeworkcore.com/walkthroughs/console-application
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(local);Database=EFCore;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}