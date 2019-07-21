using CourseApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Persistence
{
    public class CourseApiDbContext : DbContext
    {
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public CourseApiDbContext(DbContextOptions<CourseApiDbContext> options)
            : base(options)
        {
            
        }
    }
}