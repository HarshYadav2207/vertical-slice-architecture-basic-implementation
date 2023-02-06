using MediatorTaskAPI.Features.House;
using MediatorTaskAPI.Features.Student;
using Microsoft.EntityFrameworkCore;

namespace MediatorTaskAPI.Data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<StudentModel> Students { get; set; }
        public DbSet<HouseModel> houses { get; set; }

    }
}
