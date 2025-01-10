using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;

namespace StudentManagement.Infrastructure.EntityFramework
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentInClass> StudentInClasses { get; set; }
        public DbSet<Class> Classes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Class>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<Student>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<StudentInClass>().HasKey(sc => sc.Id);

            modelBuilder.Entity<StudentInClass>()
                        .HasOne(x => x.Student)
                        .WithMany(x => x.StudentInClasses)
                        .HasForeignKey(x=>x.StudentId);

            modelBuilder.Entity<StudentInClass>()
                        .HasOne(x => x.Class)
                        .WithMany(x => x.StudentInClasses)
                        .HasForeignKey(x=>x.ClassId);

        }
    }
}
