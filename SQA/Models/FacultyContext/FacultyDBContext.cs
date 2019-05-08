using Microsoft.EntityFrameworkCore;

namespace SQA.Models.FacultyContext
{
    public class FacultyDBContext : DbContext
    {
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Message> Messages { get; set; }

        public FacultyDBContext(DbContextOptions<FacultyDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
