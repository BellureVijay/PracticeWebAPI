using Microsoft.EntityFrameworkCore;
using PracticeProjectWebAPI.Models;

namespace PracticeProjectWebAPI.DataBaseConnection
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<AppUser> Users { get; set; }
    }
}
